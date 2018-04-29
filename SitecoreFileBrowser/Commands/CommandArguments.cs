using System.Collections.Generic;
using System.Linq;

namespace SitecoreFileBrowser.Commands
{
    public class CommandArguments
    {
        public CommandArguments(IDictionary<string, string> context)
        {
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