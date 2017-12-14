using System.IO;
using System.Net.Sockets;
using System.Text;
using System;

namespace Server
{
    /* 
     * Class that helps with the file transferation
     */
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

        /*
         * Check if a file exists
         */
        public bool IsFileExisting()
        {
            return File.Exists(this.filePath);
        }

        /* transfer a file over a given socket
         * 
         */
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