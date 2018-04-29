using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sitecore.Pipelines.HttpRequest;
using SitecoreFileBrowser.Commands;

namespace SitecoreFileBrowser
{
    public class ExecuteCommandPipelineProcessor : HttpRequestProcessor
    {
        private readonly string _route;

        public ExecuteCommandPipelineProcessor(string route)
        {
            _route = route;
        }

        public override void Process(HttpRequestArgs args)
        {
            if (string.IsNullOrWhiteSpace(_route)) return;

            if (!args.HttpContext.Request.RawUrl.StartsWith(_route, StringComparison.OrdinalIgnoreCase)) return;
            if (!Configuration.Enabled) return;

            ProcessRequest(args.HttpContext);

            args.HttpContext.Response.End();
        }

        private static void ProcessRequest(HttpContextBase context)
        {
            var command = context.Request.QueryString["command"];

            if (string.IsNullOrWhiteSpace(command))
                throw new InvalidOperationException("command was missing");

            context.Server.ScriptTimeout = 86400;

            context.Response.Clear();
            context.Response.ContentType = "plain/text";

            // workaround to allow streaming output without an exception in Sitecore 8.1 Update-3 and later
            context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";

            DispatchCommand(context, command);
        }

        private static CommandArguments DispatchCommand(HttpContextBase context, string command)
        {
            var args = new CommandArguments(context, ToDictionary(context.Request.QueryString));
           
            switch (command.ToUpperInvariant())
            {
                case "CHALLENGE": return new Challenge().Execute(args);
                case "PROXY": return new Proxy().Execute(args);
                case "BROWSE": return new Commands.Browse().Execute(args);
                case "DOWNLOAD": return new Download().Execute(args);
                default: throw new InvalidOperationException("unknown command");
            }
        }

        private static IDictionary<string, string> ToDictionary(NameValueCollection queryString)
        {
            var source = queryString.AllKeys.Where(s => !string.IsNullOrWhiteSpace(s));

            return source.ToDictionary(key => key, key => queryString[key]);
        }
    }
}