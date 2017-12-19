using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;

namespace Server.Core{
    /*
     * Collect all the things for a right http respond
     */
    public class HttpResponde{

        private String responseType;
        private long contentlength = 0;

        private int statusCode;
        private String statusCodeText;

        public String GetStatusCodeText() => this.statusCodeText;

        public HttpResponde(String responseType){
            this.responseType = responseType;
        }

        public void SetStatusCode(int? statusCode){
            this.statusCode = statusCode ?? 500;

            switch(statusCode/100){
                case 1:
                    this.statusCodeText = "Informational";
                    break;
                case 2:
                    this.statusCodeText = "OK";
                    break;
                case 3:
                    this.statusCodeText = "redirect";
                    break;
                case 4: 
                    this.statusCodeText = "Client Error";
                    break;
                case 5:
                    this.statusCodeText = "Server Error";
                    break;
                default:
                    //todo throw exception;
                    break;
            }
        }

        private String contentType;
        public void SetContentType(String contentType) => this.contentType = contentType;

        private ArrayList contentArray = new ArrayList();
        public void AddContent(String content)
        {
            this.contentlength += content.Length + 1;
            this.contentArray.Add(content);
        }

        FileTransfer file = null;

        public void AddContent(FileTransfer file)
        {
            this.file = file;
            this.contentlength += file.GetFileSize();
            Console.WriteLine("FileSize: {0}; Content: {1}", this.contentlength, file.GetFileSize());
        }

        public String BuildHttpRespond(){
            String respond = "HTTP/1.1 " + this.statusCode + "\n";
            DateTime now = DateTime.Now;
            respond += now.ToLocalTime() + "\n";
            respond += "Server: c#-Server" + "\n";
            respond += "Content-Length: " + this.contentlength + "\n";
            respond += "Connection: Closed" + "\n";
            //Ende headers
            respond += "\n";
            //Beginn  contents
            for (int i = 0; i < this.contentArray.Count; i++){
                String t = (String)this.contentArray[i];
                respond += t + "\n";
            }
            return respond;
        }

        Socket handler;
        public void SendHttpRespond(Socket handler)
        {
            this.handler = handler;

            Send("HTTP/1.1 " + this.statusCode + "\n");
            DateTime now = DateTime.Now;
            Send("Date: " + now.ToLocalTime() + "\n");
            Send("Server: c#-Server" + "\n");
            Send("Content-Length: " + this.contentlength + "\n");
            Send("Connection: Closed" + "\n");
            //Ende headers
            Send("\n");
            //Beginn  contents

            //This need to get changed to better implement Routes
            //idea let the route handle the write to socket by himself 

            for (int i = 0; i < this.contentArray.Count; i++)
            {
                String t = (String)this.contentArray[i];
                Send(t + "\n");
            }

            if(this.file != null)
            {
                Console.WriteLine("File Transfer");
                this.file.TransferFile(this.handler);
            }
        }

        private void Send(String data)
        {
            this.handler.Send(Encoding.ASCII.GetBytes(data));
        }
    }
}