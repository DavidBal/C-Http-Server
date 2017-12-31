using Server.Core;
using System.Collections;
using System;

namespace Server.Routing
{
    
    /// <summary>
    /// This Class helps with rendering Data to a given file by inserting them
    /// to a  "<loadtarget: ID>"
    /// 
    /// </summary>
    public class OnServerRender : Route
    {
        FileTransfer file;
        /// <summary>
        /// the list with the objects that are to render
        /// </summary>
        SortedList renderList;

        private string marker = "loadtarget:";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server.Routing.OnServerRender"/> class.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="path">Path.</param>
        public OnServerRender(string url, String path ) : base(url)
        {
            this.file = new FileTransfer(path);
            this.renderList = new SortedList();
        }


        /// <summary>
        /// Routes the task.
        /// Empty use this function to run a function on this route you can add 
        /// render to file objects in this functions
        /// </summary>
        protected override void RouteTask(){
            
        }


        /// <summary>
        /// Builds the answer
        /// </summary>
        private void DoRenderToFile()
        {
            String line;

            Console.WriteLine("Start OnCallRender: ");

            this.respond.SetStatusCode(200);
            this.respond.SetContentType("text/html");

            line = this.file.ReadLine();
            while (line != null)
            {
                //Console.WriteLine(line);

                //That the line can be checked for multi markers
                if (line.Contains("<" + this.marker))
                {
                    //get the marker
                    String[] substrings;
                    substrings = line.Split('<');

                    this.respond.AddContent(substrings[0]);

                    int i = 1;
                    while (i < substrings.Length)
                    {
                        if (substrings[i].StartsWith(this.marker, StringComparison.CurrentCulture))
                        {
                            String[] tmp;
                            Console.WriteLine(substrings[i]);
                            tmp = substrings[i].Split('>');

                            //get the identifiyer
                            String id = tmp[0].Remove(0, this.marker.Length).Trim();

                            //get the pos in the List
                            int pos = this.renderList.IndexOfKey(id);

                            Console.WriteLine(pos + " , " + id);
                            //
                            if (pos >= 0)
                            {
                                String render = ((RenderObject)this.renderList.GetByIndex(pos)).GetContent();
                                Console.WriteLine(render);
                                this.respond.AddContent(render);
                            }
                            i++;
                        }
                    }

                }
                else
                {
                    //Console.WriteLine(line);
                    this.respond.AddContent(line);
                }
                line = this.file.ReadLine();
            }
        }


        /// <summary>
        /// Fast hand for string render 
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="content">Content.</param>
        public void AddToRenderObject(String target, String content)
        {
            this.AddToRenderObject(new RenderTextObject(target, content));
        }

        public void AddToRenderObject(RenderObject render)
        {
            this.renderList.Add(render.GetRenderTarget(), render);
        }


        /// <summary>
        /// Runs the route task.
        /// Overrite the function.
        /// </summary>
        /// <param name="respond">Respond.</param>
        /// <param name="request">Request.</param>
        public override void RunRouteTask(HttpResponde respond, HttpRequest request)
        {
            this.request = request;
            this.respond = respond;

            this.RouteTask();

            this.DoRenderToFile();
        }

    }


    /// <summary>
    /// Represent a abstract object that can be renderd to a given target.
    /// 
    /// </summary>
    public abstract class RenderObject
    {
        /// <summary>
        /// The Target where that conent should be renderd
        /// -> id 
        /// </summary>
        private String renderTarget;

        public String GetRenderTarget() => this.renderTarget;
       
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server.Routing.RenderObject"/> class.
        /// </summary>
        /// <param name="renderTarget">Target.</param>
        public RenderObject(String renderTarget){
            this.renderTarget = renderTarget;
        }

        /// <summary>
        /// Get the Content
        /// Is called by the Router
        /// </summary>
        /// <returns>the Content</returns>
        public abstract String GetContent();
    }

    /// <summary>
    /// Render text object.
    /// Fast hand for a Text based object
    /// </summary>
    public class RenderTextObject : RenderObject
    {
        String content;
        public RenderTextObject(string renderTarget, string content) : base(renderTarget)
        {
            this.content = content;   
        }

        public override string GetContent()
        {
            return this.content;
        }
    }
}