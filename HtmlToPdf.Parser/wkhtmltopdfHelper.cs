using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdf.Parser
{
    public static class wkhtmltopdfHelper
    {
        public static string GetCompilePdfFile(string url)
        {
            var outputFile = Path.Combine(ParserSettings.OutputDirectory, String.Format("{0}.pdf", Guid.NewGuid()));
            var arguments = String.Format("{0} {1}", url, outputFile);
            var appPath = ParserSettings.ApplicationPath;

            CommandLineHelper.Run(appPath, arguments);

            return outputFile;
        }
    }
}
