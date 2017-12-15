using System;
using System.IO;
using System.Collections;
namespace Server.Routing
{
    public class RoutManager
    {
        /// <summary>
        /// The routes.
        /// </summary>
        private ArrayList routes;

        private bool autoRouteCSS = true;
        public bool GetAutoRouteCSS() => this.autoRouteCSS;
        public bool SetAutoRouteCSS(bool autoRouteCSS) => this.autoRouteCSS = autoRouteCSS;

        private bool autoRouteJS = true;
        public bool GetAutoRouteJS() => this.autoRouteJS;
        public bool SetAutoRouteJS(bool autoRouteJS) => this.autoRouteJS = autoRouteJS;

        private bool autoRouteHTML = false;
        public bool GetAutoRouteHTML() => this.autoRouteHTML;
        public bool SetAutoRouteHTML(bool autoRouteHTML) => this.autoRouteHTML = autoRouteHTML;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server.Routing.RoutManager"/> class.
        /// </summary>
        public RoutManager()
        {
            this.routes = new ArrayList();
        }

        /// <summary>
        /// Adds the route to the Route List.
        /// No Exception handling
        /// </summary>
        /// <param name="route">Route.</param>
        public void AddRoute(Route route)
        {
            this.routes.Add(route);    
        }

        /// <summary>
        /// Try to create a route from a given url to a path.
        /// </summary>
        /// <returns><c>true</c>, if add route to file was build, <c>false</c> otherwise.</returns>
        /// <param name="url">URL.</param>
        /// <param name="path">Path.</param>
        public bool CheckedAddRouteToFile(String url, String path)
        {
            try
            {
                this.AddRoute(new RouteToFile(url, path));
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the route by URL.
        /// </summary>
        /// <returns>The route by URL or NULL if no route can be found</returns>
        /// <param name="url">URL.</param>
        public Route GetRouteByUrl(String url)
        {
            Route route = null;

            int i = 0;
            bool found = false;
            while(i < this.routes.Count && found == false)
            {
                if( ((Route)this.routes[i]).GetUrl().Equals(url) == true)
                {
                    found = true;
                    route = (Route)this.routes[i];
                }

                i++;
            }

            if (route != null)
            {
                Console.WriteLine(route.GetUrl());
            }

            if (route ==  null)
            {
                route = this.GetAutoRoute(url);
            }

            return route ?? new Server.Routing.RouteNotFound(url);
        }

        /// <summary>
        /// Try to build a <see cref="T:Server.Routing.AutoRoute"/>
        /// </summary>
        /// <returns>The auto route.</returns>
        /// <param name="url">URL.</param>
        private AutoRoute GetAutoRoute(String url)
        {
            if(url.EndsWith(".js", StringComparison.CurrentCulture) && this.autoRouteJS)
            {
                return AutoRoute.GenrateAutoRoute(url);
            }

            if(url.EndsWith(".css", StringComparison.CurrentCulture) && this.autoRouteCSS)
            {
                return AutoRoute.GenrateAutoRoute(url);
            }

            if(url.EndsWith(".html", StringComparison.CurrentCulture) && this.autoRouteHTML)
            {
                return AutoRoute.GenrateAutoRoute(url);
            }

            return null;
        }
    }
}
