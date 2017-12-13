using System;
using System.Collections;

namespace Server{
    class HttpResponde{

        private String responseType;
        private int contentlength = 0;

        private int statusCode;
        private String statusCodeText;


        public HttpResponde(String responseType){
            this.responseType = responseType;
        }

        public void setStatusCode(int? statusCode){
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

        //todo file transfer;

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

    }
}