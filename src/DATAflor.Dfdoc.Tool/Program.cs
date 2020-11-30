using System;
using DATAflor.Dfdoc.Metadata;

namespace DATAflor.Dfdoc.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("DATAflor.DfDoc.Tool.exe (filename) (directory)");
                Console.WriteLine("(filename) is the path to a DFDoc-File");
                Console.WriteLine("(directory) is the path to a directory where the data is extracted to");
                return;
            }

            string filePath = args[0];
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} not found.");
                return;
            }

            string directory = args[1];
            if (!System.IO.Directory.Exists(directory))
            {
                Console.WriteLine($"Directory {directory} not found.");
                return;
            }

            try
            {
                using var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var dfDoc = DFDoc.Load(stream);
                foreach ((DocumentType Document, byte[] Content) entry in dfDoc.Entries())
                {
                    string targetName = System.IO.Path.Combine(directory, entry.Document.Filename);
                    Console.WriteLine($"Extracting {entry.Document.Filename} to {targetName}");
                    if (System.IO.File.Exists(targetName))
                    {
                        System.IO.File.SetAttributes(targetName, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(targetName);
                    }
                    System.IO.File.WriteAllBytes(targetName, entry.Content);
                }
            }
            catch (InvalidFileformatException e)
            {
                Console.WriteLine("Error reading file\n" + e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
