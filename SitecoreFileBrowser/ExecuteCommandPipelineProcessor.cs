using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security;
using System.Web;
using Newtonsoft.Json;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.SecurityModel;
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

            // this securitydisabler allows the control panel to execute unfettered when debug compilation is enabled but you are not signed into Sitecore
            using (new SecurityDisabler())
            {
                if (WasChallenge(context)) return;

                var securityState = Configuration.AuthenticationProvider.ValidateRequest(new HttpRequestWrapper(HttpContext.Current.Request));

                if (!securityState.IsAllowed) throw new SecurityException();

                DispatchCommand(context, command);
            }
        }

        private static void DispatchCommand(HttpContextBase context, string command)
        {
            var args = new CommandArguments(ToDictionary(context.Request.QueryString));
            object result;

            switch (command.ToUpperInvariant())
            {
                case "BROWSE":
                    result = new Commands.Browse().Execute(args);
                    break;

                case "DOWNLOAD":
                    result = new Download().Execute(args);
                    break;

                case "PROXY":
                    result = new Proxy().Execute(args);
                    break;

                default: throw new InvalidOperationException("unknown command");
            }

            Write(context, JsonConvert.SerializeObject(result));
        }

        private static IDictionary<string, string> ToDictionary(NameValueCollection queryString)
        {
            var source = queryString.AllKeys.Where(s => !string.IsNullOrWhiteSpace(s));

            return source.ToDictionary(key => key, key => queryString[key]);
        }

        private static bool WasChallenge(HttpContextBase context)
        {
            if (!context.Request.QueryString["command"].Equals("challenge", StringComparison.OrdinalIgnoreCase)) return false;

            Write(context, Configuration.AuthenticationProvider.GetChallengeToken());

            return true;
        }

        private static void Write(HttpContextBase context, string text)
        {
            context.Response.Write(text);
        }
    }
}