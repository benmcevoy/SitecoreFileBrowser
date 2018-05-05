using System.Collections.Generic;

namespace SitecoreFileBrowser.Browse.Model
{
    public class FileInfo
    {
        public FileInfo()
        {
            Attributes = new Dictionary<string, string>();
        }

        public virtual string FullName { get; set; }
        public virtual string Path { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; } = "File";
        public IDictionary<string, string> Attributes { get; set; }
    }
}