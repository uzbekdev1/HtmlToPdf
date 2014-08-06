using System;
using System.Net;
using System.Text;

namespace HtmlToPdf.ServerApp.Handlers
{
    public class HtmlRequestHandler : IHttpRequestHandler
    {
        private string _name;
        private readonly string _content;

        public HtmlRequestHandler(string content=null)
        {
            _name = "/html";
            _content = content??"Sorry,no content";
        }

        public void Handle(HttpListenerContext context)
        {
            using (var serverResponse = context.Response)
            {  
                var data = Encoding.UTF8.GetBytes(_content); 

                serverResponse.StatusCode = (int)HttpStatusCode.OK;
                serverResponse.ContentType = "text/html";
                serverResponse.KeepAlive = false;
                serverResponse.ContentEncoding = Encoding.UTF8;
                serverResponse.ContentLength64 = data.Length;
                serverResponse.OutputStream.Write(data, 0, data.Length);
                serverResponse.Close();
            }

            Console.WriteLine("Request from client. Request string: {0}", context.Request.RawUrl);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
