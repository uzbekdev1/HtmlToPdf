using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlToPdf.ServerApp;

namespace HtmlToPdf.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var content = File.ReadAllText("../../tmp.html");
            var helper = new HtmlToPdfHelper(content);

            helper.BeginRequest();

            //launch pdf file
            helper.OpenPdf();

            //save to desktop
            var data = helper.GetPdf();
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), String.Format("{0}.pdf", Guid.NewGuid()));
            File.WriteAllBytes(file, data);

            //browse in browser
            helper.BrowsePdf();

            Console.ReadKey();

            helper.EndRequest();
        }
    }
}
