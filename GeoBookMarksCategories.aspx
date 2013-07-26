<%@ Page Title="Oasis Admin Tools ---  Geobookmark Groups" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="GeoBookMarksCategories.aspx.vb" Inherits="GeoBookMarksCategories" %>

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
    <ext:GridPanel ID="gpGeoBookMarksCategories" runat="server" Title="Geo BookMarks Categories"
        Margins="0 0 5 5" Frame="true" Height="320">
        <Store>
            <ext:Store ID="GeoBookMarksCategoriesStore" runat="server" OnRefreshData="GeoBookMarksCategoriesStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="ID">
                        <Fields>
                            <ext:RecordField Name="ID" />
                            <ext:RecordField Name="Name" />
                            <ext:RecordField Name="Description" />
                            <ext:RecordField Name="GUID1" />
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
                <ext:Column ColumnID="ID" Header="ID" Width="100" DataIndex="ID" Hidden="false" />
                <ext:Column DataIndex="Name" Header="Name" Width="100" />
                <ext:Column DataIndex="Description" Header="Description" Width="100" />
                <ext:Column DataIndex="GUID1" Width="250" Header="GUID1" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw" />
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmGeoBookMarksCategories}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmGeoBookMarksCategories" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Geo Book Marks Categories Detail" DefaultAnchor="100%">
        <Items>
            <ext:NumberField ID="nbfId" runat="server" FieldLabel="ID" DataIndex="ID" AllowBlank="false"
                MsgTarget="Under" Disabled="true" />
            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" DataIndex="Name" AllowBlank="true"
                MsgTarget="Under" />
            <ext:TextField ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="Description"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtsGUID" runat="server" FieldLabel="GUID" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Disabled="true" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmGeoBookMarksCategories}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmGeoBookMarksCategories}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmGeoBookMarksCategories}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
