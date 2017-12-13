using System;
using System.Collections;
namespace Server.Routing
{
    public class RoutManager
    {
        private ArrayList routes;

        public RoutManager()
        {
            this.routes = new ArrayList();
        }

        public void AddRoute(Router route)
        {
            this.routes.Add(route);    
        }

        public Router GetRouteByUrl(String url)
        {
            Router route = null;

            int i = 0;
            bool found = false;
            while(i < this.routes.Count && found == false)
            {
                if( ((Router)this.routes[i]).GetUrl().Equals(url) == true)
                {
                    found = true;
                    route = (Router)this.routes[i];
                }

                i++;
            }

            if(route != null)
                Console.WriteLine(route.GetUrl());

            if(route is RouteToFile)
            {
                if(((RouteToFile)route).GetFile().IsFileReadable() == false)
                {
                    route = null;
                }
            }

            return route ?? new Server.Routing.RouteNotFound(url);
        }

    }
}
