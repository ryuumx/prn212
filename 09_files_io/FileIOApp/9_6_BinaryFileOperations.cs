using System;
using System.IO;
using System.Text;

namespace FileIODemos
{
    public class BinaryFileOperations
    {
        public static void RunDemo()
        {
            string binaryFile = "sample_binary.dat";
            
            Console.WriteLine("=== Binary File Operations ===");
            DemonstrateBinaryWriter(binaryFile);
            DemonstrateBinaryReader(binaryFile);
        }
        
        private static void DemonstrateBinaryWriter(string fileName)
        {
            Console.WriteLine($"Writing binary data to {fileName}");
            
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Write different data types
                writer.Write(42);                           // int
                writer.Write(3.14159);                     // double
                writer.Write(true);                        // bool
                writer.Write("Hello Binary World!");       // string
                writer.Write(DateTime.Now.ToBinary());     // DateTime as binary
                
                // Write array of bytes
                byte[] data = Encoding.UTF8.GetBytes("Binary data chunk");
                writer.Write(data.Length);                 // Write length first
                writer.Write(data);                        // Write actual data
                
                // Write custom structure data
                writer.Write((short)1000);                 // short
                writer.Write(123.456f);                    // float
                writer.Write('A');                         // char
            }
            
            Console.WriteLine("Binary data written successfully");
        }
        
        private static void DemonstrateBinaryReader(string fileName)
        {
            Console.WriteLine($"\nReading binary data from {fileName}");
            
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                try
                {
                    // Read data in the same order it was written
                    int intValue = reader.ReadInt32();
                    double doubleValue = reader.ReadDouble();
                    bool boolValue = reader.ReadBoolean();
                    string stringValue = reader.ReadString();
                    DateTime dateValue = DateTime.FromBinary(reader.ReadInt64());
                    
                    Console.WriteLine($"Integer: {intValue}");
                    Console.WriteLine($"Double: {doubleValue}");
                    Console.WriteLine($"Boolean: {boolValue}");
                    Console.WriteLine($"String: {stringValue}");
                    Console.WriteLine($"DateTime: {dateValue}");
                    
                    // Read byte array
                    int arrayLength = reader.ReadInt32();
                    byte[] data = reader.ReadBytes(arrayLength);
                    string dataString = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"Byte array as string: {dataString}");
                    
                    // Read remaining data
                    short shortValue = reader.ReadInt16();
                    float floatValue = reader.ReadSingle();
                    char charValue = reader.ReadChar();
                    
                    Console.WriteLine($"Short: {shortValue}");
                    Console.WriteLine($"Float: {floatValue}");
                    Console.WriteLine($"Char: {charValue}");
                    
                    Console.WriteLine($"\nFile position: {fs.Position} of {fs.Length} bytes");
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine("Reached end of file");
                }
            }
        }
    }
}