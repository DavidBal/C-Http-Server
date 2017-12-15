using System;
using System.IO;

namespace Server.Routing
{
    /*
     * A automatic generated Route to EVERY File
     */
    public class AutoRoute : RouteToFile
    {
        public AutoRoute(String url, String filePath) : base(url, filePath)
        {
            
        }

        /// <summary>
        /// Genrates the auto route.
        /// </summary>
        /// <returns>The auto route.</returns>
        /// <param name="url">URL.</param>
        public static AutoRoute GenrateAutoRoute(String url)
        {
            AutoRoute route = null;
            String path = url;

            // cut leading '/'
            if (url.StartsWith("/", StringComparison.CurrentCulture))
            {
                path = path.Remove(0, 1);
            }


            try
            {
                route = new AutoRoute(url, path);
            } catch (IOException e) {
                Console.WriteLine(e.Message);
                route = null;
            }
            return route;
        }
    }
}