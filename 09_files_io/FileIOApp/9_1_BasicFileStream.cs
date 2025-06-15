using System;
using System.IO;
using System.Text;

namespace FileIODemos
{
    public class BasicFileStreamDemo
    {
        public static void RunDemo()
        {
            string fileName = "sample.txt";
            string content = "Hello, FileStream World!\nThis is line 2.\nThis is line 3.";
            
            // Writing to file using FileStream
            Console.WriteLine("=== Writing with FileStream ===");
            WriteWithFileStream(fileName, content);
            
            // Reading from file using FileStream
            Console.WriteLine("\n=== Reading with FileStream ===");
            ReadWithFileStream(fileName);
            
            // Seeking in FileStream
            Console.WriteLine("\n=== Seeking in FileStream ===");
            SeekInFileStream(fileName);
        }
        
        private static void WriteWithFileStream(string fileName, string content)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = Encoding.UTF8.GetBytes(content);
                fs.Write(data, 0, data.Length);
                Console.WriteLine($"Written {data.Length} bytes to {fileName}");
            }
        }
        
        private static void ReadWithFileStream(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                string content = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Read {bytesRead} bytes:");
                Console.WriteLine(content);
            }
        }
        
        private static void SeekInFileStream(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // Seek to position 7 (after "Hello, ")
                fs.Seek(7, SeekOrigin.Begin);
                
                byte[] buffer = new byte[10];
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                string partialContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                Console.WriteLine($"Content from position 7: '{partialContent}'");
                Console.WriteLine($"Current position: {fs.Position}");
                Console.WriteLine($"File length: {fs.Length}");
            }
        }
    }
}