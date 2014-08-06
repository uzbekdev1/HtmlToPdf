using System;
using System.Net;
using System.Threading;
using HtmlToPdf.ServerApp.Handlers;

namespace HtmlToPdf.ServerApp
{
    public sealed class HttpVirtualServer : IHttpVirtualServer
    {
        private readonly HttpListener _httpListener;
        private Thread _connectionThread;
        private bool _running;
        private readonly HttpRequestResourceHandle _requestResource;

        private void ThreadStart()
        {
            try
            {
                while (_running)
                {
                    var context = _httpListener.GetContext();

                    _requestResource.HandleContext(context);
                }
            }
            catch (HttpListenerException exp)
            {
                Console.WriteLine("HTTP server was shut down - \r\n{0}", exp.Message);
            }

        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_running)
                Stop();

            if (_connectionThread == null)
                return;

            _connectionThread.Abort();
        }

        private HttpVirtualServer()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("The Http Server cannot run on this operating system.");

            _httpListener = new HttpListener();
            _requestResource = new HttpRequestResourceHandle();
        }

        public HttpVirtualServer(params string[] prefixes)
            : this()
        {

            foreach (var prefix in prefixes)
                _httpListener.Prefixes.Add(prefix);

        }

        public void AddHttpRequestHandler(IHttpRequestHandler requestRequestHandler)
        {
            _requestResource.AddHttpRequestHandler(requestRequestHandler);
        }

        public void Start()
        {
            if (_httpListener.IsListening)
                return;

            _httpListener.Start();
            _running = true;

            _connectionThread = new Thread(new ThreadStart(() =>
            {
                ThreadStart();
            }));

            _connectionThread.Start();

            Console.WriteLine("Server started");
        }

        public void Stop()
        {
            if (!_httpListener.IsListening)
                return;

            _running = false;
            _httpListener.Stop();

            Console.WriteLine("Server stopped");
        }
        public bool IsRunning
        {
            get { return _running; }
        }
         
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
