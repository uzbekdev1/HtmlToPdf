using System;
using System.Net;
using System.Text;

namespace HtmlToPdf.ServerApp.Handlers
{
    public class NotFoundRequestHandler : IHttpRequestHandler
    {
        private string _name;
        private readonly string _message;

        public NotFoundRequestHandler(string message = null)
        {
            _message = message ?? "Sorry, that page doesn't exist";
            _name = "/not_found";
        }

        public void Handle(HttpListenerContext context)
        {
            using (var serverResponse = context.Response)
            {
                var messageBytes = Encoding.UTF8.GetBytes(_message);

                serverResponse.StatusCode = (int)HttpStatusCode.NotFound;
                serverResponse.ContentType = "text/plain";
                serverResponse.KeepAlive = false;
                serverResponse.ContentEncoding = Encoding.UTF8;
                serverResponse.ContentLength64 = messageBytes.Length;
                serverResponse.OutputStream.Write(messageBytes, 0, messageBytes.Length);
                serverResponse.Close();
            }

            Console.WriteLine("Invalid request from client. Request string: {0}", context.Request.RawUrl);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
