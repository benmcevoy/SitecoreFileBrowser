using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreFileBrowser.Commands
{
    public class CommandArguments
    {
        public HttpContextBase HttpContext { get; }

        public CommandArguments(HttpContextBase httpContext, IDictionary<string, string> context)
        {
            HttpContext = httpContext;
            Context = context ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Get the value from the context or null if not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => Context.TryGetValue(key, out var result) ? result : null;
            set => Context[key] = value;
        }

        public override string ToString()
        {
            return Context.Aggregate("", (s, pair) =>
            {
                s += $"{pair.Key}={pair.Value}&";
                return s;
            });
        }

        public string Command => Context["command"];

        public IDictionary<string, string> Context { get; }
    }
}