<%@ Page Title="Oasis Admin Tools ---  RSS" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="Feeds.aspx.vb" Inherits="Feeds" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <ext:FormPanel ID="frmUserGroup" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="User Groups" Icon="Group" DefaultAnchor="100%">
        <Items>
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
                <DirectEvents>
                    <Select OnEvent="cboUserGroups_Select" Before="return #{frmUserGroup}.isValid();">
                        <EventMask ShowMask="true" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpFeeds" runat="server" Title="RSS" Margins="0 0 5 5" Frame="true"
        Height="320">
        <Store>
            <ext:Store ID="FeedsStore" runat="server" OnRefreshData="FeedsStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="FeedID">
                        <Fields>
                            <ext:RecordField Name="FeedID" />
                            <ext:RecordField Name="FeedGroupName" />
                            <ext:RecordField Name="FeedGroupID" />
                            <ext:RecordField Name="CustomId" />
                            <ext:RecordField Name="FeedName" />
                            <ext:RecordField Name="FeedDescription" />
                            <ext:RecordField Name="FeedURL" />
                            <ext:RecordField Name="FeedImageURL" />
                            <ext:RecordField Name="CheckInterval" />
                            <ext:RecordField Name="Subscribed" />
                            <ext:RecordField Name="LastCheck" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:CommandColumn Width="50" Header="Actions">
                    <Commands>
                        <ext:GridCommand Icon="Delete" CommandName="Delete">
                            <ToolTip Text="Delete" />
                        </ext:GridCommand>
                    </Commands>
                </ext:CommandColumn>
                <ext:Column DataIndex="FeedID" ColumnID="FeedID" Header="Id" Width="100" Hidden="false" />
                <ext:Column DataIndex="FeedGroupName" Header="Groups" Width="100" />
                <ext:Column DataIndex="FeedGroupID" Header="FeedGroupID" Width="100" Hidden="true" />
                <ext:Column DataIndex="CustomId" Header="Custom Id" Width="100" />
                <ext:Column DataIndex="FeedName" Width="100" Header="Feed Name" />
                <ext:Column DataIndex="FeedDescription" Width="100" Header="Feed Description" />
                <ext:Column DataIndex="FeedURL" Width="100" Header="Feed URL" />
                <ext:Column DataIndex="FeedImageURL" Width="100" Header="Feed Image URL" />
                <ext:Column DataIndex="CheckInterval" Width="100" Header="Check Interval" />
                <ext:Column DataIndex="Subscribed" Width="100" Header="Subscribed" />
                <ext:Column DataIndex="LastCheck" Width="100" Header="Last Check" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmFeeds}.getForm().loadRecord(record); #{cboFeedGroups}.setValue(record.data.FeedGroupID);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="ID" Value="record.data.FeedID" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmFeeds" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="RSS Detail" DefaultAnchor="100%">
        <Items>
            <ext:NumberField ID="nbfFeedID" runat="server" FieldLabel="Feed Id" DataIndex="FeedID"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:ComboBox ID="cboFeedGroups" runat="server" EmptyText="Select a feed group..."
                Editable="false" FieldLabel="Feed groups" Width="300" DisplayField="FeedGroupName"
                ValueField="FeedGroupID" ForceSelection="true" DataIndex="FeedGroupId" AllowBlank="false"
                MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="FeedGroupStore" AutoLoad="false" OnRefreshData="FeedGroupStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="FeedGroupID">
                                <Fields>
                                    <ext:RecordField Name="FeedGroupID" Type="String" Mapping="FeedGroupID" />
                                    <ext:RecordField Name="FeedGroupName" Type="String" Mapping="FeedGroupName" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:NumberField ID="nbfCustomId" runat="server" FieldLabel="Custom Id" DataIndex="CustomId"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtFeedName" runat="server" FieldLabel="Name" DataIndex="FeedName"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextArea ID="txtaFeedDescription" runat="server" FieldLabel="Feed Description"
                DataIndex="FeedDescription" AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtFeedURL" runat="server" FieldLabel="URL" DataIndex="FeedURL"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtFeedImageURL" runat="server" FieldLabel="Image URL" DataIndex="FeedImageURL"
                AllowBlank="true" MsgTarget="Under" />
            <ext:NumberField ID="nbfCheckInterval" runat="server" FieldLabel="Check Interval"
                DataIndex="CheckInterval" AllowBlank="true" MsgTarget="Under" Hidden="true" />
            <ext:TextField ID="txtSubscribed" runat="server" FieldLabel="Subscribed" DataIndex="Subscribed"
                AllowBlank="true" MsgTarget="Under" Hidden="true" />
            <ext:DateField ID="dfLastCheck" runat="server" Format="dd/MM/yyyy" Vtype="daterange"
                FieldLabel="Last Check (dd/mm/yyy)" AnchorHorizontal="100%" EnableKeyEvents="true"
                AllowBlank="true" Hidden="true">
            </ext:DateField>
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmFeeds}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmFeeds}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmFeeds}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
