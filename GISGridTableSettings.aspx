<%@ Page Title="Oasis Admin Tools ---  Data view" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="GISGridTableSettings.aspx.vb" Inherits="GISGridTableSettings" %>

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
    <ext:GridPanel ID="gpGISGridTableSettings" runat="server" Title="Data View"
        Margins="0 0 5 5" Frame="true" Height="320">
        <Store>
            <ext:Store ID="GISGridTableSettingsStore" runat="server" OnRefreshData="GISGridTableSettingsStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="id">
                        <Fields>
                            <ext:RecordField Name="id" />
                            <ext:RecordField Name="name" />
                            <ext:RecordField Name="alias" />
                            <ext:RecordField Name="visible" />
                            <ext:RecordField Name="datasetwarning" />
                            <ext:RecordField Name="warninglevel" />
                            <ext:RecordField Name="MaxRec" />
                            <ext:RecordField Name="excludedFlds" />
                            <ext:RecordField Name="isURLLayer" />
                            <ext:RecordField Name="autoRunUrls" />
                            <ext:RecordField Name="URLLayerField" />
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
                <ext:Column ColumnID="id" Header="id" Width="100" DataIndex="id" Hidden="false" />
                <ext:Column DataIndex="name" Header="Name" Width="100" />
                <ext:Column DataIndex="alias" Header="Alias" Width="100" />
                <ext:Column DataIndex="warninglevel" Width="100" Header="Warning Level" />
                <ext:Column DataIndex="MaxRec" Width="100" Header="Max Rec" />
                <ext:Column DataIndex="excludedFlds" Width="100" Header="Excluded Fields" />
                <ext:Column DataIndex="URLLayerField" Width="100" Header="URL Layer Field" />
                <ext:Column DataIndex="visible" Width="100" Header="Visible" />
                <ext:Column DataIndex="isURLLayer" Width="100" Header="URL Layer" />
                <ext:Column DataIndex="autoRunUrls" Width="100" Header="Auto Run Urls" />
                <ext:Column DataIndex="datasetwarning" Width="100" Header="Dataset Warning" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmGISGridTableSettings}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
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
    <ext:FormPanel ID="frmGISGridTableSettings" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Data View Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtId" runat="server" FieldLabel="Id" DataIndex="id" AllowBlank="false"
                MsgTarget="Under" Disabled="true" />
            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" DataIndex="name" AllowBlank="false"
                MsgTarget="Under" />
            <ext:TextField ID="txtAlias" runat="server" FieldLabel="Alias" DataIndex="alias"
                AllowBlank="false" MsgTarget="Under" />
            <ext:NumberField ID="nbfWarninglevel" runat="server" FieldLabel="Warning Level" DataIndex="warninglevel"
                AllowBlank="true" MsgTarget="Under" />
            <ext:NumberField ID="nbfMaxRec" runat="server" FieldLabel="Max Rec" DataIndex="MaxRec"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextArea ID="txtaExcludedFlds" runat="server" FieldLabel="Excluded Fields" DataIndex="excludedFlds"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextArea ID="txtaURLLayerField" runat="server" FieldLabel="URL Layer Field"
                DataIndex="URLLayerField" AllowBlank="true" MsgTarget="Under" />
            <ext:Checkbox ID="chkVisible" runat="server" FieldLabel="Visible" DataIndex="visible">
            </ext:Checkbox>
            <ext:Checkbox ID="chkIsURLLayer" runat="server" FieldLabel="Is URL Layer" DataIndex="isURLLayer">
            </ext:Checkbox>
            <ext:Checkbox ID="chkautoRunUrls" runat="server" FieldLabel="Auto Run Urls" DataIndex="autoRunUrls">
            </ext:Checkbox>
            <ext:Checkbox ID="chkDatasetwarning" runat="server" FieldLabel="Dataset Warning"
                DataIndex="datasetwarning">
            </ext:Checkbox>
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmGISGridTableSettings}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmGISGridTableSettings}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmGISGridTableSettings}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
