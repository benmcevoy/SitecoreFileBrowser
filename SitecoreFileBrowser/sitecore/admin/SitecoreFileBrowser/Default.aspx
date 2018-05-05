<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Default.aspx.cs" Inherits="SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sitecore file browser</title>
    <script
        src="https://code.jquery.com/jquery-3.3.1.min.js"
        integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
        crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.12/css/all.css" integrity="sha384-G0fIWCsCzJIMAVNQPfjH08cyYaUtMwjJwqiRKxxE/rx96Uroj1BtIQ6MLJuheaO9" crossorigin="anonymous">
    <link rel="stylesheet" href="assets/darkly.css" />
    <style>
        header, #controls {
            border-bottom: 1px solid #f5f5f5;
            margin-bottom: 1rem;
            padding-bottom: 1rem;
        }
		
        ul { list-style-type: none !important; }

        .message.is-danger .message-header {
            background-color: #ff3860;
            color: #fff;
        }
    </style>
</head>
<body class="container">
    <form id="form1" runat="server">
        <header id="header">
            <h1 class="title is-1">Sitecore file browser</h1>
            <p class="subtitle">Browse files on the local server or remotely.</p>
        </header>

        <section id="controls">
            <div class="field">
                <asp:Label runat="server" CssClass="label" AssociatedControlID="Address">Enter the remote address, e.g. https://www-delivery2.com</asp:Label>
                <div class="control">
                    <asp:TextBox runat="server" CssClass="input" ID="Address" Text="http://localhost" placeholder="Enter the remote address, e.g. https://www-delivery2.com"></asp:TextBox>
                </div>
            </div>
            <div class="field">
                <div class="control">
                    <asp:Button runat="server" CssClass="button" Text="Browse" OnCommand="OnCommand" CommandName="Browse" />
                </div>
            </div>
        </section>

        <section>

            <asp:Literal runat="server" ID="CurrentMachine" />

            <div class="tree content">
                <asp:Literal runat="server" ID="TreeView" />
            </div>
            <div class="detail"></div>
        </section>
    </form>

    <script>
        $('.directory').click((e) => $('ul', e.target).toggle())
    </script>
</body>
</html>

