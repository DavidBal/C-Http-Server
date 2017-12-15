using System;
using target;
using Server.Routing;
namespace Einleben
{
    public class Startup
    {
        public static int Main()
        {
            Console.WriteLine("__ Starting __");

            RoutManager routeManager = new RoutManager();

            routeManager.AddRoute(new RouteTextTest("/testtext"));
            routeManager.CheckedAddRouteToFile("/testfile","testfile.html");
            routeManager.CheckedAddRouteToFile("/hans", "hans.html");

            target.ServerMainThread test = new ServerMainThread(8080, routeManager);
            test.MainLoop();

            return 0;
        }
    }
}
