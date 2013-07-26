<%@ Page Title="Oasis Admin Tools --- User" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="User.aspx.vb" Inherits="User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <ext:ResourceManager ID="ResourceManager2" runat="server" Theme="Gray" />
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    <% 
        If (lblError.Text.Length > 0) Then
            Response.Write("<br /><br /><br />")
        End If
    %>
    <ext:GridPanel ID="GridPanel1" runat="server" Title="Users" Margins="0 0 5 5" Icon="UserSuit"
        Region="Center" Frame="true" Height="300">
        <Store>
            <ext:Store ID="Store1" runat="server" OnRefreshData="Store1_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="UserId">
                        <Fields>
                            <ext:RecordField Name="UserId" />
                            <ext:RecordField Name="Username" />
                            <ext:RecordField Name="First Name" />
                            <ext:RecordField Name="Last Name" />
                            <ext:RecordField Name="SettingUrl" />
                            <ext:RecordField Name="Groups" />
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
                <ext:Column ColumnID="UserId" Header="UserId" Width="120" DataIndex="UserId" Hidden="true" />
                <ext:Column DataIndex="Username" Header="Username" Width="120" />
                <ext:Column DataIndex="First Name" Header="First Name" Width="120" />
                <ext:Column DataIndex="Last Name" Header="Last Name" Width="120" />
                <ext:Column DataIndex="SettingUrl" Header="Setting Url" Width="120" />
                <ext:Column DataIndex="Groups" Header="Groups" Width="100" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="UserId" Value="record.data.UserId" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <DirectEvents>
                    <RowSelect OnEvent="RowSelect" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{FormPanel1}" />
                        <ExtraParams>
                            <ext:Parameter Name="UserId" Value="this.getSelected().id" Mode="Raw" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5"
        Frame="true" Title="User Details" Icon="User" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtId" runat="server" FieldLabel="User ID" DataIndex="UserID"  Disabled="true"/>
            <ext:TextField ID="txtUserName" runat="server" FieldLabel="User Name" DataIndex="UserName"
                AllowBlank="false" MsgTarget="Under" EnableKeyEvents="true" MaskRe="[a-zA-Z0-9]">
            </ext:TextField>
            <ext:TextField ID="txtPassword" runat="server" FieldLabel="Password" DataIndex="Password1"
                AllowBlank="false" InputType="Password" MsgTarget="Under" />
            <ext:TextField ID="txtConfirmPassword" runat="server" FieldLabel="Confirm Password"
                DataIndex="Password2" AllowBlank="false" IsRemoteValidation="true" InputType="Password"
                MsgTarget="Under">
                <RemoteValidation OnValidation="CheckPassword" />
            </ext:TextField>
            <ext:TextField ID="txtFirstName" runat="server" FieldLabel="First Name" DataIndex="FirstName"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextField ID="txtLastName" runat="server" FieldLabel="Last Name" DataIndex="LastName"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextField ID="txtSettingURL" runat="server" FieldLabel="Setting URL" DataIndex="SettingURL"
                AllowBlank="false" MsgTarget="Under" />
            <ext:ComboBox ID="cboUserGroups" runat="server" EmptyText="Select a user group..."
                Editable="false" FieldLabel="User groups" Width="300" DisplayField="Name" ValueField="Id"
                ForceSelection="true" DataIndex="GroupID" AllowBlank="false" MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="UserGroupStore" OnRefreshData="UserGroupStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="Id" Type="String" Mapping="Id" />
                                    <ext:RecordField Name="Name" Type="String" Mapping="Name" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
        </Items>
        <Buttons>
<%--            <ext:Button ID="btnSave" runat="server" Text="Save">
                <DirectEvents>
                    <Click OnEvent="btnSave_Click" Before="return #{FormPanel1}.isValid();">
                        <EventMask ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>--%> 
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{FormPanel1}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{FormPanel1}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
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
