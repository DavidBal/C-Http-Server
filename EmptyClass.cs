using System;
using target;
namespace Einleben
{
    public class World
    {
        public static int Main()
        {
            Console.WriteLine("__ Starting __");
            target.ServerMainThread test = new ServerMainThread(8080);
            test.MainLoop();

            return 0;
        }
    }
}
