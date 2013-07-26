<%@ Page Title="Oasis Admin Tools --- Web Tiles" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false"
    CodeFile="WebTiles.aspx.vb" Inherits="WebTiles" %>

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
    <ext:GridPanel ID="gpWebTiles" runat="server" Title="Web Tiles List" Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="WebTilesStore" runat="server" OnRefreshData="WebTilesStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="HiddenCaption">
                        <Fields>
                            <ext:RecordField Name="Caption" />
                            <ext:RecordField Name="URL1" />
                            <ext:RecordField Name="URL2" />
                            <ext:RecordField Name="URL3" />
                            <ext:RecordField Name="ESPGNumber" />
                            <ext:RecordField Name="ImageFormat" />
                            <ext:RecordField Name="ForceWGS" />
                            <ext:RecordField Name="HiddenCaption" />
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
                <ext:Column DataIndex="Caption" Header="Caption" Width="100" Hidden="false" />
                <ext:Column DataIndex="URL1" Header="URL1" Width="100" />
                <ext:Column DataIndex="URL2" Header="URL2" Width="100" />
                <ext:Column DataIndex="URL3" Header="URL3" Width="100" />
                <ext:Column DataIndex="ESPGNumber" Header="ESPG Number" Width="100" />
                <ext:Column DataIndex="ImageFormat" Header="Image Format" Width="100" />
                <ext:Column DataIndex="ForceWGS" Header="Force WGS" Width="100" />
                <ext:Column ColumnID="HiddenCaption" Header="HiddenCaption" DataIndex="HiddenCaption"
                    Width="100" Hidden="true" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="Caption" Value="record.data.HiddenCaption" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmWebTiles}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmWebTiles" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="Web Tiles" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtCaption" runat="server" FieldLabel="Caption" DataIndex="Caption"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextField ID="txtURL1" runat="server" FieldLabel="URL 1" DataIndex="URL1" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtURL2" runat="server" FieldLabel="URL 2" DataIndex="URL2" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtURL3" runat="server" FieldLabel="URL 3" DataIndex="URL3" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:NumberField ID="nbfESPGNumber" runat="server" FieldLabel="ESPG Number" DataIndex="ESPGNumber"
                AllowBlank="true" MsgTarget="Under" MaskRe="[0-9]" />
            <ext:Checkbox ID="chkForceWGS" runat="server" FieldLabel="Force WGS" DataIndex="ForceWGS">
            </ext:Checkbox>
            <ext:ComboBox ID="cboImageFormat" runat="server" DisplayField="display" ValueField="value"
                ForceSelection="true" Editable="false" DataIndex="ImageFormat" EmptyText="Select a image format..."
                FieldLabel="ImageFormat" Width="300">
                <Store>
                    <ext:Store ID="Store2" runat="server">
                        <Reader>
                            <ext:ArrayReader>
                                <Fields>
                                    <ext:RecordField Name="value" />
                                    <ext:RecordField Name="display" />
                                </Fields>
                            </ext:ArrayReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:TextField ID="txtHiddenCaption" runat="server" FieldLabel="HiddenCaption" DataIndex="HiddenCaption"
                AllowBlank="true" MsgTarget="Under" Hidden="true" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmWebTiles}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmWebTiles}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmWebTiles}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
