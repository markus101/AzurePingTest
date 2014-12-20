using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PingTest
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                var ping = new
                           {
                               Version = GetVersion(),
                               CompileTime = GetCompileTime().ToString("s") + "Z"
                           };

                return Response.AsJson(ping);
            };
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