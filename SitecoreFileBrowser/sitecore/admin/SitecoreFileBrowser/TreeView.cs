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
                writer.Append("<ul>");
                Directory(machine.Address, directory, writer);
                writer.Append("</ul>");
            }

            foreach (var file in machine.Root.Files)
            {
                writer.Append("<ul>");
                File(machine.Address, file, writer);
                writer.Append("</ul>");
            }

            return writer.ToString();
        }

        private static StringBuilder Directory(string address, DirectoryInfo directory, StringBuilder writer)
        {
            writer.AppendLine($"<li class='directory'><span class='icon'><i class='fas fa-folder'></i></span>{directory.Name}");

            if (directory.Directories.Any())
            {
                writer.Append("<ul>");

                foreach (var subDirectory in directory.Directories)
                {
                    Directory(address, subDirectory, writer);
                }

                writer.Append("</ul>");
            }

            if (directory.Files.Any())
            {
                writer.Append("<ul>");

                foreach (var file in directory.Files)
                {
                    File(address, file, writer);
                }

                writer.Append("</ul>");
            }

            writer.AppendLine("</li>");

            return writer;
        }
        
        private static StringBuilder File(string address, FileInfo file, StringBuilder writer)
        {
            writer.AppendLine($"<li class='file {file.Attributes["Extension"].Replace(".","")} '><span class='icon'><i class='fas fa-file-alt'></i></span><a href='{Configuration.Route}?command=proxy&address={address}&remoteCommand=download&path={file.Path}'>{file.Name}</a></li>");

            return writer;
        }
    }
}