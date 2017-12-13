using System.IO;
using System.Net.Sockets;
using System.Text;
using System;

namespace Server
{
    public class FileTransfer
    {

        string filePath;
        long fileSize;

        public long GetFileSize() => this.fileSize;

        public FileTransfer(string filePath)
        {
            this.filePath = filePath;
            this.fileSize = new FileInfo(filePath).Length;
        }

        public bool IsFileReadable()
        {
            return File.Exists(this.filePath);
        }

        public void TransferFile(Socket handler){
            StreamReader reader = new StreamReader(this.filePath);
            while(reader.Peek() >= 0)
            {
                string read = reader.ReadLine();
                Console.WriteLine(read);
                handler.Send(Encoding.ASCII.GetBytes(read + "\n"));
            }
            reader.Close();
        }
    }
}