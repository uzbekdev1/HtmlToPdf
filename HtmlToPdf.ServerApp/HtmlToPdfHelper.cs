using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HtmlToPdf.Parser;
using HtmlToPdf.ServerApp.Handlers;

namespace HtmlToPdf.ServerApp
{
    public class HtmlToPdfHelper
    {
        private readonly HttpVirtualServer _server;
        private readonly string _prefix = String.Format("http://localhost:{0}/", ServerSettings.PORT);

        public HtmlToPdfHelper(string content)
        {
            _server = new HttpVirtualServer(_prefix);
            _server.AddHttpRequestHandler(new PdfRequestHandler());
            _server.AddHttpRequestHandler(new HtmlRequestHandler(content)); 
        }

        public void BeginRequest()
        {
            if (!_server.IsRunning)
                _server.Start();
        }

        public byte[] GetPdf()
        {
            var htmlUrl = String.Concat(_prefix, "html");
            var pdfFile = wkhtmltopdfHelper.GetCompilePdfFile(htmlUrl);
             
            try
            {
                return File.ReadAllBytes(pdfFile);
            }
            catch (Exception exp)
            {  
                throw exp;
            }

            return new byte[] { };
        }

        public void OpenPdf()
        {
            var htmlUrl = String.Concat(_prefix, "html");
            var pdfFile = wkhtmltopdfHelper.GetCompilePdfFile(htmlUrl);
             
            try
            {
                Process.Start(pdfFile);
            }
            catch (Exception exp)
            {  
                throw exp;
            }
        }

        public void BrowsePdf()
        {

            var htmlUrl = String.Concat(_prefix, "html");
            var pdfFile = wkhtmltopdfHelper.GetCompilePdfFile(htmlUrl);
            var pdfUrl = String.Concat(_prefix, "pdf?file=", pdfFile);
             
            try
            {
                Process.Start("iexplore.exe", pdfUrl);
            }
            catch (Exception exp)
            { 
                throw exp;
            }
        }

        public void EndRequest()
        {
            if (_server.IsRunning)
                _server.Stop();
        }

    }
}
