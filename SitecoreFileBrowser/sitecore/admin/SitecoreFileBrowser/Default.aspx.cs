using System;
using System.Web.UI.WebControls;
using Sitecore.sitecore.admin;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser
{
    public partial class Default : AdminPage
    {
        protected override void OnInit(EventArgs e)
        {
            CheckSecurity(true);

            if (!Configuration.Enabled) throw new ApplicationException("SitecoreFileBrowser is disabled");
            
            if (!IsPostBack)
            {
                Configuration.Repository.Add(new MachineInfo
                {
                    Address = LocalAddress(),
                    Name = "local"
                });
            }

            MachineRepeater.DataSource = Configuration.Repository.Get();
            MachineRepeater.DataBind();
        }

        protected void AddMachineClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void OnCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Browse")
            {
                var command = $"{LocalAddress()}{Configuration.Route}?command=proxy&remoteCommand=browse&address={e.CommandArgument}";
                var client = Configuration.AuthenticationProvider.CreateAuthenticatedWebClient(command);
                json.Text = client.DownloadString(command);
            }
        }

        private string LocalAddress()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority +
                   Request.ApplicationPath.TrimEnd('/');
        }
    }
}