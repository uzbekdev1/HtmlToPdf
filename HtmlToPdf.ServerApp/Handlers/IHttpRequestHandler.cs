using System.Net;

namespace HtmlToPdf.ServerApp.Handlers
{
    public interface IHttpRequestHandler
    {

        void Handle(HttpListenerContext context);

        string Name { get; set; }

    }
}
