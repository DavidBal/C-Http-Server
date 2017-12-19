using System.IO;
using Server.Core;

namespace Server.Routing
{
    /*
     * class that make it esay to Route to a file
     */
    public class RouteToFile : Route
    {
        FileTransfer file;

        public FileTransfer GetFile() => this.file;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server.Routing.RouteToFile"/> class.
        /// <para />- throws <see cref="T:System.IO.IOExceptions"/>
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="filePath">File path.</param>
        /// <exception cref = "IOException" > When the file is not existing</exception> 
        public RouteToFile(string url, string filePath) : base(url)
        {
            this.file = new FileTransfer(filePath);
        }

        protected override void RouteTask()
        {
            this.respond.SetStatusCode(200);
            this.respond.SetContentType("text/html");
            this.respond.AddContent(this.file);
        }
    }
}