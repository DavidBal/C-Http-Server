using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Server;
using Server.Routing;

namespace target
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

        private static void WorkerThread(IAsyncResult ar)
        {
            allDone.Set();

            //Get the socket that handle Client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);


            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private static void ReadCallback(IAsyncResult ar){
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesread = handler.EndReceive(ar);

            if ( 0 < bytesread ){

                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesread));

                //todo need to read more data if the file is not finished with reading

                content = state.sb.ToString();

                Console.WriteLine(content);

                HttpRequest request = Server.HttpRequest.HttpRequestBuilder(state);

                //Answer Building   
                Server.HttpResponde response = new HttpResponde("POST");

                Router route= ServerMainThread.routManager.GetRouteByUrl(request.GetUrl());

                route.RunRouteTask(response, request);

                response.SendHttpRespond(handler);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}