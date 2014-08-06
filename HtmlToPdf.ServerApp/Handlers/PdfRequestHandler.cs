using System;
using System.IO;
using System.Net;
using System.Text;

namespace HtmlToPdf.ServerApp.Handlers
{
    public class PdfRequestHandler : IHttpRequestHandler
    {
        private string _name;

        public PdfRequestHandler()
        {
            _name = "/pdf";
        }

        public void Handle(HttpListenerContext context)
        {
            using (var serverResponse = context.Response)
            {
                var file = context.Request.QueryString["file"];
                var name = Path.GetFileName(file);

                if (!File.Exists(file))
                    throw new FileNotFoundException();

                var data = File.ReadAllBytes(file);

                serverResponse.StatusCode = (int)HttpStatusCode.OK;
                serverResponse.ContentType = "application/pdf";
                serverResponse.Headers.Add("Content-Disposition", String.Format("attachment;Filename={0}", name));
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
