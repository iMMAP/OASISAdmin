<%@ Page Title="Oasis Admin Tools --- Security Setting" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="SecurityModuleSettings.aspx.vb" Inherits="SecurityModuleSettings" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .tableGroup
        {
            height: 93px;
            width: 560px;
        }
        .tdGroup
        {
            width: 242px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    User Groups&nbsp;
    <ajaxToolkit:ComboBox ID="cboUserGroups" runat="server" AutoPostBack="True" DropDownStyle="DropDownList"
        AutoCompleteMode="None" CaseSensitive="False" CssClass="WindowsStyle" ItemInsertLocation="Append">
    </ajaxToolkit:ComboBox>
    &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" Height="30px" Width="58px" />
    <br />
    <ext:FormPanel ID="Security" runat="server" Title="Security Module Settings" MonitorPoll="500"
        MonitorValid="true" Padding="5" AutoWidth="True" Height="200" ButtonAlign="Right"
        Layout="RowLayout">
        <Items>
            <ext:FieldSet ID="FieldSet5" runat="server" Title="Incident Layer Name" Width="340"
                Layout="form" LabelAlign="Top">
                <Items>
                    <ext:TextField ID="txtIncidentLayerName" Text="Oincidents" runat="server" Width="300">
                        <ToolTips>
                            <ext:ToolTip ID="ToolTip4" runat="server" Html="<p><b>OASIS_Incident_Layer_Name</b></p><p>SettingValue1</p>" />
                        </ToolTips>
                    </ext:TextField>
                </Items>
            </ext:FieldSet>
            <ext:Panel ID="Panel99" runat="server" Border="false" Header="false" Layout="ColumnLayout"
                LabelAlign="Top" Height="80">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Avaliable Tools" ColumnWidth=".75">
                        <Items>
                            <ext:CompositeField ID="CompositeField7" runat="server" AnchorHorizontal="100%">
                                <Items>
                                    <ext:DisplayField ID="DisplayField18" runat="server" Text="Security trends" />
                                    <ext:Checkbox ID="chkSecurityTrends" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip2" runat="server" Html="<p><b>SecurityTools</b></p><p>SettingValue3 (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:DisplayField ID="DisplayField19" runat="server" Text="Add incident" />
                                    <ext:Checkbox ID="chkAddIncident" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip3" runat="server" Html="<p><b>SecurityTools</b></p><p>SettingValue4 (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                </Items>
                            </ext:CompositeField>
                        </Items>
                    </ext:FieldSet>
                    <ext:FieldSet ID="FieldSet2" runat="server" Title="Misc" ColumnWidth=".25" Layout="Form">
                        <Items>
                            <ext:Panel ID="Panel5" runat="server" Border="false" Header="false" ColumnWidth=".20"
                                Layout="Form" LabelAlign="Top">
                                <Items>
                                    <ext:TextField ID="txtLegendSymbolSize" runat="server" FieldLabel="Legend Symbol Size"
                                        Width="120">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip1" runat="server" Html="<p><b>ShowSecurityTab</b></p><p>SettingValue2</p>" />
                                        </ToolTips>
                                    </ext:TextField>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:FieldSet>
                </Items>
            </ext:Panel>
        </Items>
    </ext:FormPanel>
</asp:Content>
