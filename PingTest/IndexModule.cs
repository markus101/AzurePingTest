using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace PingTest
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                Sleep();

                var ping = new
                           {
                               Version = GetVersion(),
                               CompileTime = GetCompileTime().ToString("s") + "Z"
                           };

                return Response.AsJson(ping);
            };
        }

        private void Sleep()
        {
            Thread.Sleep(2000);
        }

        private string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fileVersionInfo.FileVersion;
        }

        private DateTime GetCompileTime()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var creationTime = new FileInfo(assembly.Location).CreationTime;

            return creationTime;
        }
    }
}