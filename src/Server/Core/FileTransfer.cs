using System.IO;
using System.Net.Sockets;
using System.Text;
using System;

namespace Server.Core
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
            this.reader = null;
        }

        /// <summary>
        /// The reader.
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// Transfers the file over a given Socket
        /// </summary>
        /// <param name="handler">Handler.</param>
        public void TransferFile(Socket handler)
        {
            if (reader == null)
            {
                this.reader = new StreamReader(this.filePath);
            }

            //Send file
            while (this.reader.Peek() >= 0)
            {
                string read = this.reader.ReadLine();
                Console.WriteLine(read);
                handler.Send(Encoding.ASCII.GetBytes(read + "\n"));
            }

            this.CloseReader();
        }

        /// <summary>
        /// Read a line
        /// </summary>
        /// <returns>the line from the file or null if eof is reached</returns>
        public String ReadLine()
        {
            if (reader == null)
            {
                this.reader = new StreamReader(this.filePath);
            }

            String line;

            if (this.reader.Peek() >= 0)
            {
                line = this.reader.ReadLine();
            }
            else
            {
                line = null;
                this.CloseReader();
            }

            return line;
        }

        /// <summary>
        /// Closes the reader and set the reader = null
        /// </summary>
        private void CloseReader()
        {
            this.reader.Close();
            this.reader = null;
        }
    }
}