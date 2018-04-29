using System.Linq;
using System.Text;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser
{
    public class TreeView
    {
        public string Render(MachineInfo machine)
        {
            var writer = new StringBuilder();

            foreach (var directory in machine.Root.Directories)
            {
                Directory(machine.Address, directory, writer);
            }

            foreach (var file in machine.Root.Files)
            {
                File(machine.Address, file, writer);
            }

            return writer.ToString();
        }

        private static StringBuilder Directory(string address, DirectoryInfo directory, StringBuilder writer)
        {
            writer.Append($"<span class='title'>{directory.Name}</span>");

            if (directory.Directories.Any())
            {
                writer.Append("<ul>");

                foreach (var subDirectory in directory.Directories)
                {
                    writer.Append("<li>");

                    Directory(address, subDirectory, writer);

                    writer.Append("</li>");
                }

                writer.Append("</ul>");
            }

            if (directory.Files.Any())
            {
                writer.Append("<ul>");

                foreach (var file in directory.Files)
                {
                    writer.Append("<li>");

                    File(address, file, writer);

                    writer.Append("</li>");
                }

                writer.Append("</ul>");
            }

            return writer;
        }
        
        private static StringBuilder File(string address, FileInfo file, StringBuilder writer)
        {
            writer.Append($"<a href='{Configuration.Route}?command=proxy&address={address}&remoteCommand=download&path={file.Path}'>{file.Name}</a>");

            return writer;
        }
    }
}