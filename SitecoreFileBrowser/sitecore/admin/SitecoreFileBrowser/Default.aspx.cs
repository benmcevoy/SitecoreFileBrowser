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
                Address.Text = LocalAddress();
            }
        }

        protected void OnCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Browse") Browse(e);
        }

        private void Browse(CommandEventArgs e)
        {
            try
            {
                var remoteAddress = Address.Text;

                if (string.IsNullOrWhiteSpace(remoteAddress))
                {
                    CurrentMachine.Text = ErrorMessage("<p>The address is missing.</p>");
                    return;
                }

                var command = $"{LocalAddress()}{Configuration.Route}?command=proxy&remoteCommand=browse&address={remoteAddress}";
                var client = Configuration.AuthenticationProvider.CreateAuthenticatedWebClient(command);

                client.SetCookies(Request.Cookies, Request.Url.Authority);

                var response = client.DownloadString(command);
                var machineInfo = JsonConvert.DeserializeObject<MachineInfo>(response);

                machineInfo.Address = remoteAddress;

                CurrentMachine.Text = new CurrentMachine().Render(machineInfo);
                TreeView.Text = new TreeView().Render(machineInfo);
            }
            catch (Exception exception)
            {
                var address = string.IsNullOrWhiteSpace(Address.Text) ? "missing" : Address.Text;

                CurrentMachine.Text = ErrorMessage($@"<p>{exception.Message}</p>
                        <p>Check the address '<strong>{address}</strong>' is correct.</p>");
            }
        }

        protected string LocalAddress()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority +
                   Request.ApplicationPath?.TrimEnd('/');
        }

        private static string ErrorMessage(string message)
        {
            return $@"<article class='message is-danger'>
                        <div class='message-header'>
                        <p>Oh no &nbsp; :( &nbsp;</p>
                        <button class='delete' aria-label='delete'></button>
                        </div>
                        <div class='message-body'>
                        {message}
                        </div>
                        </article>";
        }
    }
}