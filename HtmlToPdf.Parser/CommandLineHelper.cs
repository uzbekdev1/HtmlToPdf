using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;

namespace HtmlToPdf.Parser
{
    [SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public static class CommandLineHelper
    {
        private delegate string StringDelegate();

        private static string Run(string fileName, string arguments, out string errorMessage)
        {
            errorMessage = "";
            using (var cmdLineProcess = new Process())
            {
                cmdLineProcess.StartInfo.FileName = fileName;
                cmdLineProcess.StartInfo.Arguments = arguments;
                cmdLineProcess.StartInfo.UseShellExecute = false;
                cmdLineProcess.StartInfo.CreateNoWindow = true;
                cmdLineProcess.StartInfo.RedirectStandardOutput = true;
                cmdLineProcess.StartInfo.RedirectStandardError = true;

                if (!cmdLineProcess.Start())
                    throw new Exception(String.Format("Could not start command line process: {0}", fileName));

                return ReadProcessOutput(cmdLineProcess, ref errorMessage, fileName);
            }
        }

        private static string ReadProcessOutput(Process cmdLineProcess, ref string errorMessage, string fileName)
        {
            var outputStreamAsyncReader = new StringDelegate(cmdLineProcess.StandardOutput.ReadToEnd);
            var errorStreamAsyncReader = new StringDelegate(cmdLineProcess.StandardError.ReadToEnd);
            var outAR = outputStreamAsyncReader.BeginInvoke(null, null);
            var errAR = errorStreamAsyncReader.BeginInvoke(null, null);

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                while (!(outAR.IsCompleted && errAR.IsCompleted))
                {
                    /* Check again every 10 milliseconds: */
                    Thread.Sleep(10);
                }
            }
            else
            {
                var arWaitHandles = new WaitHandle[2];
                arWaitHandles[0] = outAR.AsyncWaitHandle;
                arWaitHandles[1] = errAR.AsyncWaitHandle;

                if (!WaitHandle.WaitAll(arWaitHandles))
                    throw new Exception(String.Format("Command line aborted: {0}", fileName));
            }

            var results = outputStreamAsyncReader.EndInvoke(outAR);
            errorMessage = errorStreamAsyncReader.EndInvoke(errAR);

            if (!cmdLineProcess.HasExited)
                cmdLineProcess.WaitForExit();

            return results;
        }

        public static string Run(string fileName, string arguments = "")
        {
            var result = String.Empty;
            var errorMsg = String.Empty;

            Run(fileName, arguments, out errorMsg);

            return result;
        }
    }
}
