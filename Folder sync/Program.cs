using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        // Prompt the user to enter the path of the source folder.
        Console.Write("Define the folder to be synchronized: ");
        string fileSource = Console.ReadLine();

        // Check if the source directory exists.
        if (!Directory.Exists(fileSource))
        {
            Console.WriteLine("The source directory does not exist.");
            Console.Write("Define the folder to be synchronized: ");
            fileSource = Console.ReadLine();

            }


        // Prompt the user to enter the path of the destination folder.
        Console.Write("Define the destination path: ");
        string fileDestination = Console.ReadLine();

        // Check if the source directory exists.
        if (!Directory.Exists(fileDestination))
        {
            Console.WriteLine("The destination directory does not exist.");
            Console.Write("Define the destination path: ");
            fileDestination = Console.ReadLine();
            
        }

        // Prompt the user to enter the destination path for the logs.
        Console.Write("Set the log destination path: ");
        // Set the path for the log file.
        string logFilePath = Console.ReadLine();
        
        // Check if the source directory exists.
        if (!Directory.Exists(logFilePath))
        {
            Console.WriteLine("The destination directory does not exist.");
            Console.Write("Set the log destination path: ");
            // Set the path for the log file.
            logFilePath = Console.ReadLine();
            
        }else { logFilePath = logFilePath + @"\logsync.txt"; }
       

        // Prompt the user to enter the synchronization period in minutes.
        Console.Write("Enter the synchronization period in minutes: ");
        if (!int.TryParse(Console.ReadLine(), out int timeSync))
        {
            Console.WriteLine("Invalid value for the synchronization period.");
            return;
        }


        // Create or open the log file for writing.
 

        Console.WriteLine(" Press Ctrl+C to exit.");
        Console.WriteLine("log path: " + logFilePath);
        InitialSync(logFilePath, fileSource, fileDestination);
            
        // Create the FileSystemWatcher to monitor the source folder.
        using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = fileSource;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

            //Execute the event for when a file is created, changed, or deleted.
            watcher.Created += (sender, e) => FilesSync(logFilePath, fileDestination, e.FullPath);
            watcher.Changed += (sender, e) => FilesSync(logFilePath, fileDestination, e.FullPath);
            watcher.Deleted += (sender, e) => FileDelete(logFilePath, fileDestination, e.FullPath);
            Console.WriteLine($"The next synchronization will occur in {timeSync} minutes.");
            // Loop for continuous synchronization.
            while (true)
                {

                
                // Wait for the specified time interval before synchronizing again.
                System.Threading.Thread.Sleep(timeSync * 10000); // Convert to minutes.
                
            }
        }
    }

    static void InitialSync(string logFilePath, string fileSource, string fileDestination)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath, true);
        Console.WriteLine($"[{DateTime.Now}] -Initial sync.");
        {
            string[] arquivos = Directory.GetFiles(fileSource);
            
            foreach (string item in arquivos)
            {
                
                // Get the file name.
                string fileName = Path.GetFileName(item);
                // Create the full path for the destination file.
                string destinationPath = Path.Combine(fileDestination, fileName);
                // Get the full path for the source file.
                string sourcePath = Path.Combine(fileSource, fileName);
                // Copy the source file to the destination, overwriting if necessary.
                File.Copy(sourcePath, destinationPath, true);
                // Record the error in the log file.
                string mensagemLog = $"[{DateTime.Now}]-File synchronized: {fileSource}->{destinationPath}";
                Console.WriteLine(mensagemLog);
                writer.WriteLine(mensagemLog);
                

            }
            Console.WriteLine($"[{DateTime.Now}] -Initial sync concluded.");
            Console.WriteLine("Wait for the new changes.");
            writer.WriteLine($"[{DateTime.Now}] -Initial sync concluded.");
            writer.Close();
        }
    }
    static void FilesSync(string logFilePath, string fileDestination, string filePath)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath, true);
        {
            try
            {

                // Get the file name.
                string fileName = Path.GetFileName(filePath);

                // Create the full path for the destination file.
                string pathDestination = Path.Combine(fileDestination, fileName);

                // Copy the source file to the destination, overwriting if necessary.
                File.Copy(filePath, pathDestination, true);

                // Record the error in the log file.

                
                string mensagemLog = $"[{DateTime.Now}]-File synchronized: {filePath} -> {fileDestination}";
               Console.WriteLine(mensagemLog);
                writer.WriteLine(mensagemLog);
                
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"[{DateTime.Now}]-An error occurred during synchronization: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
                
            }
        }
    }

    static void FileDelete(string logFilePath, string archiveDestinyPath, string filePath)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath, true);
        {
            try
            {
                // Get the file name.
                string archiveName = Path.GetFileName(filePath);

                // Create the full path for the destination file.
                string destinationFolder = Path.Combine(archiveDestinyPath, archiveName);

                // Remove the file from the destination folder if it exists.
                if (File.Exists(destinationFolder))
                {
                    File.Delete(destinationFolder);

                    // Record the error in the log file.
                    string mensagemLog = $"[{DateTime.Now}]-File deleted: {destinationFolder}";
                    Console.WriteLine(mensagemLog);
                    writer.WriteLine(mensagemLog);
                    
                }
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"[{DateTime.Now}]-An error occurred during synchronization: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
                
            }
        }
    }
}
