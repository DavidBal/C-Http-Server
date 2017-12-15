using System.IO;
using System.Net.Sockets;
using System.Text;
using System;

namespace Server
{
    /* 
     * Class that helps with the file transferation
     * 
     */
    public class FileTransfer
    {

        string filePath;
        long fileSize;

        public long GetFileSize() => this.fileSize;



        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server.FileTransfer"/> class.
        /// Create a file transfer frome a given path.
        /// <para />- throws <see cref="T:System.IO.IOExceptions"/>
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <exception cref = "IOException" > When the file is not existing</exception>
        public FileTransfer(string filePath)  
        {
            this.filePath = filePath;
            this.fileSize = new FileInfo(filePath).Length;
        }

     

        /// <summary>
        /// Transfers the file over a given Socket
        /// </summary>
        /// <param name="handler">Handler.</param>
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