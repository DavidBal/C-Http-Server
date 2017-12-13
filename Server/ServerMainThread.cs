using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Server;

namespace target
{    
    public class ServerMainThread
    {

        private int port;

        /* 
         * Konstruiert den Main Server Thread von einem gegebnen Port
         */
        public ServerMainThread(int port)
        {
            this.port = port;
        }

        // Thread signal
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void MainLoop()
        {

            byte[] bytes = new Byte[1024];

            //
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.port);

            Console.WriteLine(localEndPoint.ToString());

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
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

                //need to read more data if the file is not finished with reading

                content = state.sb.ToString();

                Console.WriteLine(content);

                HttpRequest request = Server.HttpRequest.HttpRequestBuilder(state);

                //Answer Building   
                Server.HttpResponde response = new HttpResponde("POST");

                response.SetContentType("text/html");
                response.setStatusCode(200);

                response.AddContent("<!DOCTYPE html>");
                response.AddContent("<html>");
                response.AddContent("<head>");
                response.AddContent("<title>404 Not Found</title>");
                response.AddContent("<link rel=\"stylesheet\" type=\"text/css\" href=\"test.css\">");
                response.AddContent("<script src=\"test.js\"></script>");
                response.AddContent("</head>");
                response.AddContent("<body>");
                response.AddContent("<h1>Not Found</h1>");
                response.AddContent("<p>" + request.GetUrl() + " - has no Route assigend!</p>");
                response.AddContent("</body>");
                response.AddContent("</html>");

                Send(state.workSocket, response.BuildHttpRespond());
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}