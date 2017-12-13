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
            routeManager.AddRoute(new RouteToFile("/testfile","testfile.html"));

            target.ServerMainThread test = new ServerMainThread(8080, routeManager);
            test.MainLoop();

            return 0;
        }
    }
}
