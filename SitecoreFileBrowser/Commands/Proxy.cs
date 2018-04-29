using System;
using System.Linq;
using System.Security;
using Sitecore.Diagnostics;
using Sitecore.Security.Authentication;

namespace SitecoreFileBrowser.Commands
{
    public class Proxy : Command
    {
        public Proxy() : base("Proxy") { }

        public override CommandArguments Execute(CommandArguments args)
        {
            Log.Info($"SitecoreFileBrowser: Begin executing Proxy '{args}'", this);

            // user must be loggged in to issue a proxy request
            var user = AuthenticationManager.GetActiveUser();

            if (!user.IsAdministrator) throw new SecurityException();

            var remoteCommand = $"{args["address"]}{Configuration.Route}?command={args["remoteCommand"]}&{Arguments(args)}";
            
            var client = Configuration.AuthenticationProvider.CreateAuthenticatedWebClient(remoteCommand);

            args.HttpContext.Response.Clear();
            
            client.OpenRead(remoteCommand).CopyTo(args.HttpContext.Response.OutputStream);

            args.HttpContext.Response.Headers.Add(client.ResponseHeaders);

            Log.Info($"SitecoreFileBrowser: Finished executing Proxy '{args}'", this);

            return args;
        }

        private static string Arguments(CommandArguments args)
        {
            return args.Context.Aggregate("", (s, pair) =>
            {
                if (pair.Key.Equals("address", StringComparison.OrdinalIgnoreCase)) return s;
                if (pair.Key.Equals("command", StringComparison.OrdinalIgnoreCase)) return s;
                if (pair.Key.Equals("remoteCommand", StringComparison.OrdinalIgnoreCase)) return s;

                s += $"{pair.Key}={pair.Value}&";

                return s;
            });
        }
    }
}