/* David Baldauf 
 * 
 * GET - should work fine
 * POST - unimplemented
 */
using System;
using System.Collections.Generic;
namespace Server.Core
{
    enum RequstType{
        GET, POST
    }

    public class HttpRequest
    {
        private RequstType requestType;
        private String url;

        public String GetUrl() => this.url;

        public StateObject state;
        public void AddStateObject(StateObject state) => this.state = state;


        //
        private String requestContent;

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

            this.url = ExtractUrlParameter(url);

            requestContent = "";
        }

        public static HttpRequest HttpRequestBuilder(StateObject state){
            // get first Line 
            // First Line schould look like GET/POST url HTTP/1.1
            String workString = state.sb.ToString();

            String[] split = workString.Split('\n');

            String[] firstLineSplit = split[0].Split(' ');

            HttpRequest request = new HttpRequest(firstLineSplit[0], firstLineSplit[1]);

            Console.WriteLine(split.Length);
            bool endofhead = false;

            while (endofhead == false)
            {
                endofhead = request.AddRequestParameter(split, endofhead);
                if (endofhead == false)
                {
                    //Read more from socket
                    state.workSocket.Receive(state.buffer);
                    workString = state.sb.ToString();
                    split = workString.Split('\n');
                }
            }

            if (request.requestType == RequstType.POST){
                //todo need to find content lenght field
            }

            Console.WriteLine("Finished Building Request for : {0}", request.url);

            return request;
        }

        private bool AddRequestParameter(String[] array, bool endofhead){
            int i = 1;

            while (i < array.Length && endofhead == false)
            {
                Console.WriteLine(i + " : " + array[i]);
                if (array[i].Length <= 1)
                {
                    //End of http-Head reached
                    endofhead = true;

                    //Content only need to be saved when request type is post
                    while (i < array.Length && requestType == RequstType.POST)
                    {
                        this.requestContent += array[i];
                    }
                }
                else
                {
                    //add http-head parameter
                    String[] helper = array[i].Split(':');
                    this.requestParameter.Add(helper[0], helper[1]);
                }
                i++;
            }
            return endofhead;
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
    }
}
