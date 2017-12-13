using System;
using Server;

namespace Server.Routing
{
    public abstract class Router
    {
        private String url;

        protected HttpRequest request;
        protected HttpResponde respond;

        public Router(String url)
        {
            this.url = url;
        }

        public String GetUrl() 
        { 
            return this.url; 
        }

        /**
         * 
         */
        protected abstract void RouteTask();


        public void RunRouteTask(HttpResponde respond, HttpRequest request){
            this.request = request;
            this.respond = respond;

            this.RouteTask();
        }

    }
}
