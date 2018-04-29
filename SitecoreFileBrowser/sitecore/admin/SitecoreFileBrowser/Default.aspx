<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="controls">
            <asp:Button runat="server" Text="Add machine" OnClick="AddMachineClick" />
        </div>

        <div id="machines">
            <asp:Repeater runat="server" id="MachineRepeater" ItemType="SitecoreFileBrowser.Browse.Model.MachineInfo">
                <ItemTemplate>
                    <asp:Button runat="server" Text="<%# Item.Name %>" OnCommand="OnCommand" CommandName="Browse" 
                        CommandArgument="<%# Item.Address %>"/>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div id="currentMachine">

            <div class="tree">
                <asp:Literal runat="server" ID="json"></asp:Literal></div>
            <div class="detail"></div>
        </div>
    </form>
</body>
</html>
