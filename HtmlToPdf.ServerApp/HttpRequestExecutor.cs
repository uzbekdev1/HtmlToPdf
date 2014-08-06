using System.Net;
using HtmlToPdf.ServerApp.Handlers;

namespace HtmlToPdf.ServerApp
{ 
    public class HttpRequestExecutor
    {
        private readonly IHttpRequestHandler _requestHandler;
        private readonly HttpListenerContext _context;

        public HttpRequestExecutor(IHttpRequestHandler requestHandler, HttpListenerContext context)
        {
            _requestHandler = requestHandler;
            _context = context;
        }

        public void Execute()
        {
            _requestHandler.Handle(_context);
        }
    }
}
