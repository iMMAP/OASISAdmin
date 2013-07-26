<%@ Page Title="Oasis Admin Tools --- Theme" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="Themes.aspx.vb" Inherits="Themes" %>

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
    <ext:GridPanel ID="gpThemes" runat="server" Title="Themes" Margins="0 0 5 5"
        Frame="true" Height="300">
        <Store>
            <ext:Store ID="ThemesStore" runat="server" OnRefreshData="ThemesStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="ID">
                        <Fields>
                            <ext:RecordField Name="ID" />
                            <ext:RecordField Name="ThemeGroupName" />
                            <ext:RecordField Name="ThemeGroupID" />
                            <ext:RecordField Name="Name" />
                            <ext:RecordField Name="Description" />
                            <ext:RecordField Name="AnalysisField" />
                            <ext:RecordField Name="AnalysisLayer" />
                            <ext:RecordField Name="Map" />
                            <ext:RecordField Name="ThemeConfigName" />
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
                <ext:Column DataIndex="ThemeGroupName" Header="Groups" Width="100" />
                <ext:Column DataIndex="ThemeGroupID" Header="ThemeGroupID" Width="100" Hidden="true" />
                <ext:Column DataIndex="Name" Header="Name" Width="100" />
                <ext:Column DataIndex="Description" Header="Description" Width="100" />
                <ext:Column DataIndex="AnalysisField" Header="Analysis Field" Width="100" />
                <ext:Column DataIndex="AnalysisLayer" Header="Analysis Layer" Width="100" />
                <ext:Column DataIndex="Map" Header="Map" Width="100" />
                <ext:Column DataIndex="ThemeConfigName" Header="Theme Config Name" Width="100" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmThemes}.getForm().loadRecord(record); #{cboThemeGroups}.setValue(record.data.ThemeGroupID);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmThemes" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="Themes Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtID" runat="server" FieldLabel="ID" DataIndex="ID" AllowBlank="true"
                MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:ComboBox ID="cboThemeGroups" runat="server" EmptyText="Select a theme group..."
                Editable="false" FieldLabel="Theme groups" Width="320" DisplayField="ThemeGroupName"
                ValueField="ThemeGroupId" ForceSelection="true" AllowBlank="false" MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="ThemeGroupStore" AutoLoad="false" OnRefreshData="ThemeGroupStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="ThemeGroupId">
                                <Fields>
                                    <ext:RecordField Name="ThemeGroupId" Type="String" Mapping="ThemeGroupId" />
                                    <ext:RecordField Name="ThemeGroupName" Type="String" Mapping="ThemeGroupName" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" DataIndex="Name" AllowBlank="true"
                MaxLengthText="255" MsgTarget="Under" />
            <ext:TextArea ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="Description"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtAnalysisField" runat="server" FieldLabel="Analysis Field" DataIndex="AnalysisField"
                AllowBlank="true" MsgTarget="Under" MaxLengthText="50" />
            <ext:TextField ID="txtAnalysisLayer" runat="server" FieldLabel="Analysis Layer" DataIndex="AnalysisLayer"
                AllowBlank="true" MsgTarget="Under" MaxLengthText="250" />
            <ext:TextArea ID="txtMap" runat="server" FieldLabel="Map" DataIndex="Map" AllowBlank="true"
                MsgTarget="Under" />
            <ext:TextField ID="txtThemeConfigName" runat="server" FieldLabel="Theme Config Name"
                DataIndex="ThemeConfigName" AllowBlank="true" MaxLengthText="250" MsgTarget="Under"
                LabelWidth="120" />
        </Items>
         <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmThemes}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmThemes}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmThemes}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
