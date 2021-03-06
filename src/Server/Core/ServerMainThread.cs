﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Server.Routing;
using System.IO;

namespace Server.Core
{    
    public class ServerMainThread
    {

        private int port;

        private static RoutManager routManager;

        /* 
         * Konstruiert den Main Server Thread von einem gegebnen Port
         */
        public ServerMainThread(int port, RoutManager routManager)
        {
            this.port = port;
            ServerMainThread.routManager = routManager;
        }

        // Thread signal
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// Mains the loop.
        /// </summary>
        public void MainLoop()
        {

            byte[] bytes = new Byte[1024];

            //
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostAddresses("localhost")[0]);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.port);

            Console.WriteLine(localEndPoint.ToString());
            Console.WriteLine("localhost:{0}", this.port);
            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //bind port to adress
                listener.Bind(localEndPoint);
                listener.Listen(100);

                int i = 0;

                while (i < 10)
                {
                    allDone.Reset();

                    Console.WriteLine("Waiting for Connection...");

                    listener.BeginAccept(new AsyncCallback(WorkerThread), listener);

                    allDone.WaitOne();
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Starts to handle a single request
        /// </summary>
        /// <param name="ar">Ar.</param>
        private static void WorkerThread(IAsyncResult ar)
        {
            allDone.Set();
            //Get the socket that handle Client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StreamReader input = new StreamReader(new NetworkStream(handler));;
            HttpRequest request = HttpRequest.HttpRequestBuilder(input);

            //Answer Building   
            Server.Core.HttpResponde response = new HttpResponde("POST");

            Route route = ServerMainThread.routManager.GetRouteByUrl(request._url);

            route.RunRouteTask(response, request);

            response.SendHttpRespond(handler);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}