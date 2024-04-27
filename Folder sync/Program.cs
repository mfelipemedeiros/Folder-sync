using System;
using System.IO;

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
            return;
        }
        else
        {

        }

        // Prompt the user to enter the path of the destination folder.
        Console.Write("Define the destination path: ");
        string fileDestination = Console.ReadLine();

        // Check if the source directory exists.
        if (!Directory.Exists(fileDestination))
        {
            Console.WriteLine("The destination directory does not exist.");
            return;
        }

        // Prompt the user to enter the destination path for the logs.
        Console.Write("Set the log destination path: ");
        // Set the path for the log file.
        string logFilePath = Console.ReadLine();
        
        // Check if the source directory exists.
        if (!Directory.Exists(logFilePath))
        {
            Console.WriteLine("The destination directory does not exist.");
            
            return;
        }else { logFilePath = logFilePath + @"\logsync.txt"; }
       
        Console.WriteLine(logFilePath);
        // Prompt the user to enter the synchronization period in minutes.
        Console.Write("Enter the synchronization period in minutes: ");
        if (!int.TryParse(Console.ReadLine(), out int timeSync))
        {
            Console.WriteLine("Invalid value for the synchronization period.");
            return;
        }


        // Create or open the log file for writing.
 

            Console.WriteLine("Press Enter to start synchronization. Press Ctrl+C to exit.");
            InitialSync(logFilePath, fileSource, fileDestination);
            Console.WriteLine(fileSource, fileDestination);
            // Create the FileSystemWatcher to monitor the source folder.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = fileSource;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;



                // Loop for continuous synchronization.
                while (true)
                {
                    //Execute the event for when a file is created, changed, or deleted.
                    watcher.Created += (sender, e) => FilesSync(logFilePath, fileSource, fileDestination, e.FullPath);
                    watcher.Changed += (sender, e) => FilesSync(logFilePath, fileSource, fileDestination, e.FullPath);
                    watcher.Deleted += (sender, e) => FileDelete(logFilePath, fileDestination, e.FullPath);
                    watcher.Renamed += (sender, e) => FileRename(logFilePath, fileDestination, e.OldFullPath, e.FullPath);
                    
                    // Wait for the specified time interval before synchronizing again.
                    System.Threading.Thread.Sleep(timeSync * 10000); // Convert to minutes.
                
            }
        }
    }

    static void InitialSync(string logFilePath, string fileSource, string fileDestination)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath);
        {
            string[] arquivos = Directory.GetFiles(fileSource);
            Console.WriteLine(fileSource);
            foreach (string item in arquivos)
            {
                string fileName = Path.GetFileName(item);
                string destinationPath = Path.Combine(fileDestination, fileName);
                string sourcePath = Path.Combine(fileSource, fileName);

                File.Copy(sourcePath, destinationPath, true);
                string mensagemLog = $"File synchronized: {fileSource}  {destinationPath}";
                Console.WriteLine(mensagemLog);
                writer.WriteLine(mensagemLog);
                writer.Close();

            }

        }
    }
    static void FilesSync(string logFilePath, string fileSource, string fileDestination, string filePath)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath);
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
                string mensagemLog = $"File synchronized: {filePath} -> {fileDestination}";
                Console.WriteLine(mensagemLog);
                writer.WriteLine(mensagemLog);
                writer.Close();
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"An error occurred during synchronization: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }
        }
    }
    static void FileRename(string logFilePath , string destinationFolder, string oldName, string filePath)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath);
        {
            try
            {
                string caminhoArquivoSincronizado = Path.Combine(filePath, e.Name);
                string novoCaminhoArquivoSincronizado = Path.Combine(destinationFolder, e.FullPath.Substring(e.OldFullPath.Length + 1));

                File.Move(caminhoArquivoSincronizado, novoCaminhoArquivoSincronizado);
                
                string mensageLog = $"Archive renamed: {oldarchiveName} -> {archiveName}";
                // Record the error in the log file.
                Console.WriteLine(mensageLog);
                writer.WriteLine(mensageLog);
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"An error occurred during synchronization: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }

        }
    }
    static void FileDelete(string logFilePath, string archiveDestinyPath, string filePath)
    {
        //Create or open the log file for writing.
        using StreamWriter writer = new StreamWriter(logFilePath);
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
                    string mensagemLog = $"File deleted: {destinationFolder}";
                    Console.WriteLine(mensagemLog);
                    writer.WriteLine(mensagemLog);
                }
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"An error occurred during synchronization: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }
        }
    }
}
