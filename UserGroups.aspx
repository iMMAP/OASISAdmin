<%@ Page Title="Oasis Admin Tools --- User Groups" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="UserGroups.aspx.vb" Inherits="UserGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            margin-top: 0px;
        }
        
        .style2
        {
            width: 100px;
        }
    </style>
    <script type="text/javascript">
        var onKeyPressPreventSpecialCharecters = function (item, event) {
            var regex = new RegExp('^[a-zA-Z0-9]+$');
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        } 
    </script>
</asp:Content>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    <ext:ResourceManager ID="ResourceManager2" runat="server" Theme="Gray" />
    <% If (lblError.Text.Length > 0) Then
            Response.Write("<br /><br /><br />")
        End If
    %>
    <ext:GridPanel ID="GridPanel1" runat="server" Title="User Groups" Margins="0 0 5 5"
        Icon="Group" Region="Center" Frame="true" Height="200">
        <Store>
            <ext:Store ID="Store1" runat="server" OnRefreshData="UserGroupStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="GPID">
                        <Fields>
                            <ext:RecordField Name="GPID" />
                            <ext:RecordField Name="GPName" />
                            <ext:RecordField Name="GPDesc" />
                            <ext:RecordField Name="SettingTablePrefix" />
                            <ext:RecordField Name="sGUID" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:CommandColumn Width="100" Header="Actions">
                    <Commands>
                        <ext:GridCommand Icon="Delete" CommandName="Delete">
                            <ToolTip Text="Delete" />
                        </ext:GridCommand>
                    </Commands>
                </ext:CommandColumn>
                <ext:Column ColumnID="GPID" Header="User Group ID" Width="120" DataIndex="GPID" Hidden="true" />
                <ext:Column DataIndex="GPName" Header="Name" Width="150" />
                <ext:Column DataIndex="GPDesc" Header="Description" Width="150" />
                <ext:Column DataIndex="SettingTablePrefix" Header="Setting Table Prefix" Width="150" />
                <ext:Column DataIndex="sGUID" Header="sGUID" Width="120" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="UserGroup_RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="GPID" Value="record.data.GPID" Mode="Raw" />
                    <ext:Parameter Name="GPName" Value="record.data.GPName" Mode="Raw" />
                    <ext:Parameter Name="SettingTablePrefix" Value="record.data.SettingTablePrefix" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this user group" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <DirectEvents>
                    <RowSelect OnEvent="UserGroup_RowSelect" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GridPanel2}" />
                        <ExtraParams>
                            <ext:Parameter Name="GPID" Value="this.getSelected().id" Mode="Raw" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
                <Listeners>
                    <RowSelect Handler="#{FormPanel1}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="4" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:GridPanel ID="GridPanel2" runat="server" Title="Users" Margins="0 0 5 5" Icon="UserSuit"
        Region="Center" Frame="true" Height="150">
        <Store>
            <ext:Store ID="Store2" runat="server" OnRefreshData="UserStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="UserId">
                        <Fields>
                            <ext:RecordField Name="UserId" />
                            <ext:RecordField Name="Username" />
                            <ext:RecordField Name="First Name" />
                            <ext:RecordField Name="Last Name" />
                            <ext:RecordField Name="SettingUrl" />
                            <ext:RecordField Name="Group" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel2" runat="server">
            <Columns>
                <ext:Column ColumnID="UserId" Header="UserId" Width="120" DataIndex="UserId" Hidden="true" />
                <ext:Column DataIndex="Username" Header="Username" Width="120" />
                <ext:Column DataIndex="First Name" Header="First Name" Width="120" />
                <ext:Column DataIndex="Last Name" Header="Last Name" Width="120" />
                <ext:Column DataIndex="SettingUrl" Header="Setting Url" Width="120" />
                <ext:Column DataIndex="Group" Header="Groups" Width="100" />
            </Columns>
        </ColumnModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="3" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5"
        Frame="true" Title="User Group Details" Icon="Group" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtGPID" runat="server" FieldLabel="ID" DataIndex="GPID"
                Hidden="false" Disabled="true" />
            <ext:TextField ID="txtGPName" runat="server" FieldLabel="Name" DataIndex="GPName"
                AllowBlank="false" MsgTarget="Under" MaskRe="[a-zA-Z0-9]" EnableKeyEvents="true">
            </ext:TextField>
            <ext:TextField ID="txtGPDesc" runat="server" FieldLabel="Description" DataIndex="GPDesc"
                AllowBlank="false" MsgTarget="Under" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{FormPanel1}.isValid();">
                        <EventMask ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{FormPanel1}.isValid();">
                        <EventMask ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{FormPanel1}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
