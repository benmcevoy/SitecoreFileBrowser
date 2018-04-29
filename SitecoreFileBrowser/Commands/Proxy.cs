using System;
using System.Linq;

namespace SitecoreFileBrowser.Commands
{
    public class Proxy : Command
    {
        public Proxy() : base("Proxy") { }

        public override object Execute(CommandArguments args)
        {
            var remoteCommand = $"{args["address"]}{Configuration.Route}?command={args["remoteCommand"]}&{Arguments(args)}";
            
            var client = Configuration.AuthenticationProvider.CreateAuthenticatedWebClient(remoteCommand);

            // TODO: handle streams
            var result = client.DownloadString(remoteCommand);

            return result;
        }

        private static string Arguments(CommandArguments args)
        {
            return args.Context.Aggregate("", (s, pair) =>
            {
                if (pair.Key.Equals("address", StringComparison.OrdinalIgnoreCase)) return s;
                if (pair.Key.Equals("remoteCommand", StringComparison.OrdinalIgnoreCase)) return s;

                s += $"{pair.Key}={pair.Value}&";

                return s;
            });
        }
    }
}