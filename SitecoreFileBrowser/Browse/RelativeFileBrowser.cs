using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using SitecoreFileBrowser.Browse.Model;
using FileInfo = SitecoreFileBrowser.Browse.Model.FileInfo;

namespace SitecoreFileBrowser.Browse
{
    public class RelativeFileBrowser : IFileBrowser
    {
        public int BrowseCacheInMinutes { get; set; }

        public MachineInfo Browse(string address)
        {
            return Cache(() => new MachineInfo
            {
                Address = address,
                Name = Environment.MachineName,
                Root = Map(HttpRuntime.AppDomainAppPath, HttpRuntime.AppDomainAppPath)
            });
        }

        public Stream Download(FileInfo file)
        {
            return File.OpenRead(Path.Combine(HttpRuntime.AppDomainAppPath, file.Path.FromBase64()));
        }

        private static Model.DirectoryInfo Map(string absoluteFilePath, string rootPath)
        {
            var result = new Model.DirectoryInfo();

            if (Directory.Exists(absoluteFilePath))
            {
                var d = new System.IO.DirectoryInfo(absoluteFilePath);
                var r = d.FullName.RelativePath(rootPath);

                result.Name = d.Name;
                result.Path = r.ToBase64();
                result.FullName = r;

                foreach (var f in d.EnumerateFiles())
                {
                    result.Files.Add(Map(f, rootPath));
                }

                foreach (var enumerateDirectory in d.EnumerateDirectories())
                {
                    result.Directories.Add(Map(enumerateDirectory.FullName, rootPath));
                }
            }
            else
            {
                var f = new System.IO.FileInfo(absoluteFilePath);

                result.Files.Add(Map(f, rootPath));
            }

            return result;
        }

        private static FileInfo Map(System.IO.FileInfo f, string rootPath)
        {
            var r = f.FullName.RelativePath(rootPath);

            return new FileInfo
            {
                FullName = r,
                Name = f.Name,
                Path = r.ToBase64(),
                Attributes = new Dictionary<string, string>
                {
                    { "Extension", f.Extension},
                    { "Name" , f.Name},
                    { "Length" , f.Length.ToString()},
                    { "CreationTimeUtc" , f.CreationTimeUtc.ToString("dd MMM yyyy hh:mm:ss tt")},
                    { "LastWriteTimeUtc" , f.LastWriteTimeUtc.ToString("dd MMM yyyy hh:mm:ss tt")}
                }
            };
        }

        private MachineInfo Cache(Func<MachineInfo> f)
        {
            var cached = HttpContext.Current.Cache["__browse_ck"];

            if (cached != null) return (MachineInfo)cached;

            var result = f();

            if (result == null) return null;

            HttpRuntime.Cache.Insert("__browse_ck", result, null,
                DateTime.Now.Add(TimeSpan.FromMinutes(BrowseCacheInMinutes)), TimeSpan.Zero);

            return result;
        }
    }
}
