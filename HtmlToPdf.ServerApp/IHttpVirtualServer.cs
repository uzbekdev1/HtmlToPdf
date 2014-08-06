using System;
using HtmlToPdf.ServerApp.Handlers;

namespace HtmlToPdf.ServerApp
{
    public interface IHttpVirtualServer : IDisposable
    {

        void AddHttpRequestHandler(IHttpRequestHandler requestRequestHandler);

        void Start();

        void Stop();

        bool IsRunning { get; }

    }
}