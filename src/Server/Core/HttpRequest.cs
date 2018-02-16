/* David Baldauf 
 * 
 * GET - should work fine
 * POST - unimplemented
 */
using System;
using System.IO;
using System.Collections.Generic;
namespace Server.Core
{
    enum RequstType{
        GET, POST
    }

    public class HttpRequest
    {
        private RequstType requestType;

        public String _url
        {
            get;
            set;
        }

        private int _length;



        //
        private String _requestContent;

        //todo enum of all request types?
        Dictionary<String, String> requestParameter = new Dictionary<String, String>();
        Dictionary<String, String> urlParameter = new Dictionary<String, String>();

        public HttpRequest(String requestType, String url)
        {
            if (requestType.ToLower().Equals("get") == true)
            {
                this.requestType = RequstType.GET;
            }
            else if (requestType.ToLower().Equals("post") == true)
            {
                this.requestType = RequstType.POST;
            }
            else
            {
                throw new ServerException("Failed to build Httprequest - requestType do not match GET or POST - " + requestType);
            }

            this._url = ExtractUrlParameter(url);

            _requestContent = "";
        }

        public static HttpRequest HttpRequestBuilder(StreamReader input)
        {
            // get first Line 
            // First Line schould look like GET/POST url HTTP/1.1
            String firstLine = input.ReadLine();

            String[] firstLineSplit = firstLine.Split(' ');

            HttpRequest request = new HttpRequest(firstLineSplit[0], firstLineSplit[1]);

      
            bool endofhead = false;

            while (endofhead == false)
            {
                String line = input.ReadLine();

                Console.WriteLine(line);

                if (line.Length != 0)
                {
                    String[] helper = line.Split(':');
                    request.requestParameter.Add(helper[0], helper[1]);
                } else {
                    endofhead = true;
                }
            }

            if (request.requestType == RequstType.POST)
            {
                request.FindContentLength();
                Console.WriteLine(request._length);

                char[] buffer = new char[request._length];

                //only read content length
                input.Read(buffer, 0, request._length);
                request._requestContent = new string(buffer);

                Console.WriteLine(request._requestContent);
            }

            Console.WriteLine($"Finished Building Request for : {request._url}");

            return request;
        }

        private String ExtractUrlParameter(String url)
        {
            Console.WriteLine(url);

            String[] tmp= url.Split('?');

            //Keine Parameter
            if (tmp.Length <= 1){
                return tmp[0];
            }

            String[] allParameter = tmp[1].Split('&');

            for (int i = 0; i < allParameter.Length; i++)
            {
                String[] parameter = allParameter[i].Split('=');
                this.urlParameter.Add(parameter[0], parameter[1]);
            }

            //Console.WriteLine(this.urlParameter.Count);

            return tmp[0];
        }

        private void FindContentLength()
        {
            if (this.requestParameter.TryGetValue("Content-Length", out string slength))
            {
                this._length = Int32.Parse(slength) ;
            } 
        }
    }
}
