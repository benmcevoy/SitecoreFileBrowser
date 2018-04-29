using System.Collections.Generic;

namespace SitecoreFileBrowser.Browse.Model
{
    public class DirectoryInfo : FileInfo
    {
        public DirectoryInfo()
        {
            Files = new List<FileInfo>();
            Directories = new List<DirectoryInfo>();
        }

        public override string Type { get; } = "Directory";
        public IList<FileInfo> Files { get; set; }
        public IList<DirectoryInfo> Directories { get; set; }
    }
}
