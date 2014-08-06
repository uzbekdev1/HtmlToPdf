using System.Collections.Generic;
using System.Net;
using System.Threading;
using HtmlToPdf.ServerApp.Handlers;

namespace HtmlToPdf.ServerApp
{
    public class HttpRequestResourceHandle
    {

        private static readonly IHttpRequestHandler DefaultRequestRequestHandler;
        private readonly Dictionary<string, IHttpRequestHandler> _httpRequestHandlers;

        static HttpRequestResourceHandle()
        {
            DefaultRequestRequestHandler = new NotFoundRequestHandler();
        }

        private void InvokeHandler(IHttpRequestHandler requestHandler, HttpListenerContext context)
        {
            var handleHttpRequestCommand = new HttpRequestExecutor(requestHandler, context);
            var handleHttpRequestThread = new Thread(new ThreadStart(() =>
            {
                handleHttpRequestCommand.Execute();
            }));

            handleHttpRequestThread.Start();
        }

        public HttpRequestResourceHandle()
        {
            _httpRequestHandlers = new Dictionary<string, IHttpRequestHandler>();

            AddHttpRequestHandler(DefaultRequestRequestHandler);
        }

        public void AddHttpRequestHandler(IHttpRequestHandler httpRequestRequestHandler)
        {
            if (_httpRequestHandlers.ContainsKey(httpRequestRequestHandler.Name))
            {
                _httpRequestHandlers[httpRequestRequestHandler.Name] = httpRequestRequestHandler;
            }
            else
            {
                _httpRequestHandlers.Add(httpRequestRequestHandler.Name, httpRequestRequestHandler);
            }
        }

        public void HandleContext(HttpListenerContext listenerContext)
        {

            var request = listenerContext.Request;
            var requestHandlerName = request.Url.AbsolutePath;
            var requestHandler = _httpRequestHandlers.ContainsKey(requestHandlerName)
                ? _httpRequestHandlers[requestHandlerName]
                : _httpRequestHandlers[DefaultRequestRequestHandler.Name];

            InvokeHandler(requestHandler, listenerContext);

        }

    }
}
