﻿using System;

namespace Server.Routing
{
    public class RouteTextTest : Server.Routing.Router
    {
        public RouteTextTest(string url) : base(url)
        {
        }

        protected override void RouteTask()
        {
            this.respond.SetContentType("text/html");
            this.respond.setStatusCode(200);

            this.respond.AddContent("<!DOCTYPE html>");
            this.respond.AddContent("<html>");
            this.respond.AddContent("<head>");
            this.respond.AddContent("<title>Standart Text implementation</title>");
            this.respond.AddContent("<link rel=\"stylesheet\" type=\"text/css\" href=\"test.css\">");
            this.respond.AddContent("<script src=\"test.js\"></script>");
            this.respond.AddContent("</head>");
            this.respond.AddContent("<body>");
            this.respond.AddContent("<h1>Standart Text implementation</h1>");
            this.respond.AddContent("<p>" + request.GetUrl() + " - This is just for Test!</p>");
            this.respond.AddContent("</body>");
            this.respond.AddContent("</html>");

        }
    }
}