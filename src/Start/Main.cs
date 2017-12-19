using System;
using Server.Routing;
using Server.Core;

namespace Start
{
    public class Startup
    {
        public static int Main()
        {
            Console.WriteLine("__ Starting __");

            RoutManager routeManager = new RoutManager();

            OnServerRender rendertest = new OnServerRender("/testrender", "fileextra.html");
            rendertest.AddToRenderObject("ein", "Das ist ein erstertest!");
            rendertest.AddToRenderObject("zwei", "Das ist ein zweiter!");

            routeManager.AddRoute(rendertest);
            routeManager.AddRoute(new RouteTextTest("/testtext"));
            routeManager.CheckedAddRouteToFile("/testfile","testfile.html");
            routeManager.CheckedAddRouteToFile("/hans", "hans.html");

            ServerMainThread test = new ServerMainThread(8080, routeManager);
            test.MainLoop();

            return 0;
        }
    }
}
