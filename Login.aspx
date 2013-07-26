<%@ Page Title="Oasis Admin Tools --- Log in" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Content/jquery.cookie.js") %>"> </script>
    <script type="text/javascript">
        var now = new Date()
        var offset = now.getTimezoneOffset();
        $.cookie('tk',offset);
    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
            margin-top: 0px;
        }
        
        .style3
        {
            width: 104px;
            height: 28px;
        }
        .style4
        {
            height: 28px;
        }
        .style5
        {
            width: 104px;
            height: 27px;
        }
        .style6
        {
            height: 27px;
        }
        .style7
        {
            width: 104px;
            height: 25px;
        }
        .style8
        {
            height: 25px;
        }
        .style9
        {
            width: 104px;
            height: 24px;
        }
        .style10
        {
            height: 24px;
        }
        .style11
        {
            width: 104px;
            height: 8px;
        }
        .style12
        {
            height: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <table class="style1">
            <tr>
                <td class="style3">
                    <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
                </td>
                <td class="style4">
                    <asp:TextBox ID="txtUsername" runat="server" Height="24px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style5">
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style6">
                    <asp:TextBox ID="txtPassword" runat="server" Height="25px" MaxLength="250" Width="282px"
                        TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style7">
                    <asp:Label ID="lblDatabase" runat="server" Text="Database"></asp:Label>
                </td>
                <td class="style8">
                    <ajaxToolkit:ComboBox ID="cboDatabase" runat="server" DropDownStyle="DropDownList"
                        AutoCompleteMode="None" CaseSensitive="False" CssClass="WindowsStyle" ItemInsertLocation="Append"
                        ToolTip="databases">
                    </ajaxToolkit:ComboBox>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    &nbsp;
                </td>
                <td class="style10">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style11">
                    &nbsp;
                </td>
                <td class="style12">
                    <asp:Button ID="btnLogin" runat="server" Text="Log In" Height="34px" Width="66px" />
                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" Height="34px"
                        Width="66px" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
