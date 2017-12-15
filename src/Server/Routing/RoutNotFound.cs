
namespace Server.Routing
{
    /// <summary>
    /// Standart implementation for route not found!
    /// </summary>
    public class RouteNotFound : Route
    {
        public RouteNotFound(string url) : base(url)
        {
        }

        protected override void RouteTask()
        {
            this.respond.SetContentType("text/html");
            this.respond.setStatusCode(404);

            this.respond.AddContent("<!DOCTYPE html>");
            this.respond.AddContent("<html>");
            this.respond.AddContent("<head>");
            this.respond.AddContent("<title>404 Not Found</title>");
            this.respond.AddContent("<link rel=\"stylesheet\" type=\"text/css\" href=\"test.css\">");
            this.respond.AddContent("<script src=\"test.js\"></script>");
            this.respond.AddContent("</head>");
            this.respond.AddContent("<body>");
            this.respond.AddContent("<h1>Not Found</h1>");
            this.respond.AddContent("<p>" + request.GetUrl() + " - has no Route assigend!</p>");
            this.respond.AddContent("</body>");
            this.respond.AddContent("</html>");
        }
    }
}