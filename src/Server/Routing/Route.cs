using System;
using Server.Core;

namespace Server.Routing
{
    /// <summary>
    /// The abstract implementation of a Route
    /// </summary>
    public abstract class Route
    {
        private String url;

        protected HttpRequest request;
        protected HttpResponde respond;

        public Route(String url)
        {
            this.url = url;
        }

        public String GetUrl() 
        { 
            return this.url; 
        }

       
        /// <summary>
        /// Routes the task.
        /// This function need to get implemted.
        /// </summary>
        protected abstract void RouteTask();


        /// <summary>
        /// Runs the route task. 
        /// This function get called to excute to route.
        /// !!! DO NOT CHANGE !!!
        /// </summary>
        /// <param name="respond">Respond.</param>
        /// <param name="request">Request.</param>
        public void RunRouteTask(HttpResponde respond, HttpRequest request){
            this.request = request;
            this.respond = respond;

            this.RouteTask();
        }

    }
}
