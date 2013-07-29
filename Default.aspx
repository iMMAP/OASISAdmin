<%@ Page Title="OASIS Admin Tool" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Scripts/accordionImageMenu-0.4.min.js"></script>
    <link rel="stylesheet" href="Content/accordionImageMenu.css" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 149px;
        }
        .imagetext
        {
            text-align: center;
        }
        .imagetext2
        {
            text-align: left;
        }
        .style7
        {
            width: 263px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#acc-menu3').AccordionImageMenu({
                'border': 0,
                'color': '#FFF',
                'duration': 350,
                'position': 'vertical',
                'openDim': 115,
                'closeDim': 60,
                'effect': 'easeOutBack',
                'width': 125
            });
            $('#acc-menu4').AccordionImageMenu({
                'border': 0,
                'color': '#FFF',
                'duration': 350,
                'position': 'vertical',
                'openDim': 115,
                'closeDim': 60,
                'effect': 'easeOutBack',
                'width': 115
            });
            $('#acc-menu5').AccordionImageMenu({
                'border': 0,
                'color': '#FFF',
                'duration': 350,
                'position': 'vertical',
                'openDim': 125,
                'closeDim': 60,
                'effect': 'easeOutBack',
                'width': 115
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <% If (Session IsNot Nothing) AndAlso Session("UserGroup").ToString().ToLower().Trim() = "immap" Then%>
    <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Width="700" Height="600"
        Plain="true">
        <Items>
            <ext:Panel ID="Tab1" runat="server" Title="General" Padding="6" AutoScroll="true">
                <Content>
                    <table>
                        <tr>
                            <td>
                                <ext:FieldSet ID="FieldSet1" runat="server" Title="General" Width="670">
                                    <Content>
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="HyperLink15" ToolTip="Users" NavigateUrl="~/User" ImageUrl="~/Content/images/User.png"
                                                        runat="server" />
                                                    <asp:HyperLink ID="HyperLink16" ToolTip="User Groups" NavigateUrl="~/UserGroup" ImageUrl="~/Content/images/UserGroup.png"
                                                        runat="server" />
                                                    <asp:HyperLink ID="HyperLink3" ToolTip="General Setting" NavigateUrl="~/General"
                                                        ImageUrl="~/Content/images/General.png" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </Content>
                                </ext:FieldSet>
                            </td>
                    </table>
                    <ext:FieldSet ID="FieldSet3" runat="server" Title="Map" Width="670">
                        <Content>
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="HyperLink19" ToolTip="Map Setting" NavigateUrl="~/Map" ImageUrl="~/Content/images/Map.png"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink17" ToolTip="Map Library" NavigateUrl="~/MapLibrary"
                                            ImageUrl="~/Content/images/map-library.png" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink1" ToolTip="Data View" NavigateUrl="~/DataViews" ImageUrl="~/Content/images/gisgrid.png"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <div id="acc-menu3">
                                            <asp:HyperLink ID="HyperLink2" ToolTip="GeoBookmarks" NavigateUrl="~/GeoBookMarks"
                                                ImageUrl="~/Content/images/bookmark1.png" runat="server" /><br />
                                            <asp:HyperLink ID="HyperLink6" ToolTip="GeoBookmark Groups" NavigateUrl="~/GeoBookMarksCategories"
                                                ImageUrl="~/Content/images/bookmark2.png" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div id="acc-menu4">
                                            <asp:HyperLink ID="HyperLink8" ToolTip="Themes" NavigateUrl="~/Themes" ImageUrl="~/Content/images/theme1.png"
                                                runat="server" />
                                            <asp:HyperLink ID="HyperLink9" ToolTip="Theme Groups" NavigateUrl="~/ThemeGroups"
                                                ImageUrl="~/Content/images/theme2.png" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:FieldSet>
                    <ext:FieldSet ID="FieldSet4" runat="server" Title="Module" Width="670">
                        <Content>
                            <table class="style1">
                                <tr>
                                    <td class="style7">
                                        <asp:HyperLink ID="HyperLink4" ToolTip="Dynamic Data" NavigateUrl="~/DynamicModules"
                                            ImageUrl="~/Content/images/dddef.png" runat="server" />
                                        <asp:HyperLink ID="HyperLink5" ToolTip="Security Setting" NavigateUrl="~/Security"
                                            ImageUrl="~/Content/images/Security_1.png" runat="server" />
                                    </td>
                                    <td>
                                        <div id="acc-menu5">
                                            <asp:HyperLink ID="HyperLink23" ToolTip="RSS" NavigateUrl="~/RSS" ImageUrl="~/Content/images/rss1.png"
                                                runat="server" />
                                            <asp:HyperLink ID="HyperLink14" ToolTip="Groups" NavigateUrl="~/RSSGroups" ImageUrl="~/Content/images/rss2.png"
                                                runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:FieldSet>
                </Content>
            </ext:Panel>
            <ext:Panel ID="Panel2" runat="server" Title="Dynamic Settings" Padding="6" AutoScroll="true">
                <Content>
                    <ext:FieldSet ID="FieldSet2" runat="server" Title="Map" Width="670">
                        <Content>
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="HyperLink7" ToolTip="Formatting" NavigateUrl="~/Formatting" ImageUrl="~/Content/images/formatting.png"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink10" ToolTip="Validation" NavigateUrl="~/Validation.aspx"
                                            ImageUrl="~/Content/images/Validation.png" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink11" ToolTip="Nearby Features" NavigateUrl="~/NearbyFeatures.aspx"
                                            ImageUrl="~/Content/images/nearbyfeature.png" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink12" ToolTip="Chart" NavigateUrl="~/ChartSettings.aspx"
                                            ImageUrl="~/Content/images/chartsetting.png" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink13" ToolTip="Queries" NavigateUrl="~/Queries.aspx" ImageUrl="~/Content/images/Queries.png"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:FieldSet>
                </Content>
            </ext:Panel>
        </Items>
    </ext:TabPanel>
    <% End If%>
</asp:Content>
