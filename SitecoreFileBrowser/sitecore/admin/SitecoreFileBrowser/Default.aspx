<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script
        src="https://code.jquery.com/jquery-3.3.1.min.js"
        integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
        crossorigin="anonymous"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="controls">
            <asp:Button runat="server" Text="Add machine" OnClick="AddMachineClick" />
        </div>

        <div id="machines">
            <asp:Repeater runat="server" ID="MachineRepeater" ItemType="SitecoreFileBrowser.Browse.Model.MachineInfo">
                <ItemTemplate>
                    <asp:Button runat="server" Text="<%# Item.Name %>" OnCommand="OnCommand" CommandName="Browse"
                        CommandArgument="<%# Item.Address %>" />
                   </ItemTemplate>
            </asp:Repeater>
        </div>

        <div>
            
            <asp:Literal runat="server" id="CurrentMachine"/>

            <div class="tree">
                <asp:Literal runat="server" ID="TreeView"/>
            </div>
            <div class="detail"></div>
        </div>
    </form>
    

</body>
</html>
