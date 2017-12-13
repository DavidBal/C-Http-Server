namespace Server.Routing
{
    public class RouteToFile : Router
    {
        FileTransfer file;

        public FileTransfer GetFile() => this.file;

        public RouteToFile(string url, string filePath) : base(url)
        {
            this.file = new FileTransfer(filePath);
        }

        protected override void RouteTask()
        {
            this.respond.setStatusCode(200);
            this.respond.SetContentType("text/html");
            this.respond.AddContent(this.file);
        }
    }
}