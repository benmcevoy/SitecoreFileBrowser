using System;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
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
                    Name = Environment.MachineName
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
            if (e.CommandName == "Browse") Browse(e);
        }

        private void Browse(CommandEventArgs e)
        {
            var remoteAddress = (string) e.CommandArgument;
            var command = $"{LocalAddress()}{Configuration.Route}?command=proxy&remoteCommand=browse&address={remoteAddress}";
            var client = Configuration.AuthenticationProvider.CreateAuthenticatedWebClient(command);

            client.SetCookies(Request.Cookies, Request.Url.Authority);

            var response = client.DownloadString(command);
            var machineInfo = JsonConvert.DeserializeObject<MachineInfo>(response);

            machineInfo.Address = remoteAddress;

            CurrentMachine.Text = new CurrentMachine().Render(machineInfo);
            TreeView.Text = new TreeView().Render(machineInfo);
        }

        protected string LocalAddress()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority +
                   Request.ApplicationPath?.TrimEnd('/');
        }
    }
}