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
        string filePathLog = Console.ReadLine();

        // Check if the source directory exists.
        if (!Directory.Exists(filePathLog))
        {
            Console.WriteLine("O diretório de destino não existe.");
            return;
        }

        // Prompt the user to enter the synchronization period in minutes.
        Console.Write("Enter the synchronization period in minutes: ");
        if (!int.TryParse(Console.ReadLine(), out int timeSync))
        {
            Console.WriteLine("Invalid value for the synchronization period.");
            return;
        }


        // Create or open the log file for writing.
        using (StreamWriter writer = File.AppendText("sync_log.txt"))
        {

            Console.WriteLine("Press Enter to start synchronization. Press Ctrl+C to exit.");
            Console.WriteLine(fileSource, fileDestination);
            InitialSync(fileSource, fileDestination, writer);
            Console.WriteLine(fileSource, fileDestination);
            // Cria o FileSystemWatcher para monitorar a pasta de origem.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = fileSource;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;


                writer.WriteLine("anota isso aqui rapidão");
                // Loop principal para sincronização contínua.
                while (true)
                {
                    // Assina o evento para quando um arquivo é criado, alterado ou deletado.
                    watcher.Created += (sender, e) => FilesSync(fileSource, fileDestination, e.FullPath, writer);
                    watcher.Changed += (sender, e) => FilesSync(fileSource, fileDestination, e.FullPath, writer);
                    watcher.Deleted += (sender, e) => FileDelete(fileDestination, e.FullPath, writer);
                    watcher.Renamed += (sender, e) => FileRename(fileDestination, e.OldFullPath, e.FullPath, writer);
                    writer.Flush();
                    // Aguarda o intervalo de tempo especificado antes de sincronizar novamente.
                    System.Threading.Thread.Sleep(timeSync * 10000); // Converte para milissegundos.
                }
            }
        }
    }

    static void InitialSync(string fileSource, string fileDestination, StreamWriter writer)
    {
        //Create or open the log file for writing.
        using StreamWriter test = new StreamWriter(@"C:\Users\User\Documents\outra pasta\pasta1\log.txt");
        {
            string[] arquivos = Directory.GetFiles(fileSource);
            Console.WriteLine(fileSource);
            foreach (string item in arquivos)
            {
                string nomeArquivo = Path.GetFileName(item);
                string caminhoDestino = Path.Combine(fileDestination, nomeArquivo);
                string caminhoOrigem = Path.Combine(fileSource, nomeArquivo);

                File.Copy(caminhoOrigem, caminhoDestino, true);
                string mensagemLog = $"Arquivo sincronizado: {fileSource}  {caminhoDestino}";
                Console.WriteLine(mensagemLog);
                test.WriteLine(mensagemLog);

            }

        }
    }
    static void FilesSync(string fileSource, string fileDestination, string filePath, StreamWriter writer)
    {
        //Create or open the log file for writing.
        using StreamWriter test = new StreamWriter(@"C:\Users\User\Documents\outra pasta\pasta1\log.txt");
        {
            try
            {
                // Get the file name.
                string fileName = Path.GetFileName(filePath);

                // Cria o caminho completo para o arquivo de destino.
                string pathDestination = Path.Combine(fileDestination, fileName);

                // Copia o arquivo de origem para o destino, sobrescrevendo se necessário.
                File.Copy(filePath, pathDestination, true);

                // Record the error in the log file.
                string mensagemLog = $"Arquivo sincronizado: {filePath} -> {fileDestination}";
                Console.WriteLine(mensagemLog);
                writer.WriteLine(mensagemLog);
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"Ocorreu um erro durante a sincronização: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }
        }
    }
    static void FileRename(string pastaDestino, string oldName, string caminhoArquivo, StreamWriter writer)
    {
        //Create or open the log file for writing.
        using StreamWriter test = new StreamWriter(@"C:\Users\User\Documents\outra pasta\pasta1\log.txt");
        {
            try
            {
                string archiveName = Path.GetFileName(caminhoArquivo);
                string oldarchiveName = Path.GetFileName(oldName);

                string oldDestinyPath = Path.Combine(pastaDestino, oldName);
                string archiveDestinyPath = Path.Combine(pastaDestino, archiveName);

                File.Move(oldarchiveName, archiveDestinyPath);
                
                string mensageLog = $"Archive renamed: {oldarchiveName} -> {archiveName}";
                // Record the error in the log file.
                Console.WriteLine(mensageLog);
                writer.WriteLine(mensageLog);
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"Ocorreu um erro ao renomear o arquivo: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }

        }
    }
    static void FileDelete(string pastaDestino, string caminhoArquivo, StreamWriter writer)
    {
        //Create or open the log file for writing.
        using StreamWriter test = new StreamWriter(@"C:\Users\User\Documents\outra pasta\pasta1\log.txt");
        {
            try
            {
                // Get the file name.
                string nomeArquivo = Path.GetFileName(caminhoArquivo);

                // Create the full path for the destination file.
                string caminhoDestino = Path.Combine(pastaDestino, nomeArquivo);

                // Remove the file from the destination folder if it exists.
                if (File.Exists(caminhoDestino))
                {
                    File.Delete(caminhoDestino);

                    // Record the error in the log file.
                    string mensagemLog = $"Arquivo apagado: {caminhoDestino}";
                    Console.WriteLine(mensagemLog);
                    writer.WriteLine(mensagemLog);
                }
            }
            catch (Exception ex)
            {
                // Record the error in the log file.
                string mensagemErro = $"Ocorreu um erro ao apagar o arquivo: {ex.Message}";
                Console.WriteLine(mensagemErro);
                writer.WriteLine(mensagemErro);
            }
        }
    }
}
