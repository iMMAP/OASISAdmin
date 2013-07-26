<%@ Page Title="Oasis Admin Tools ---  Geobookmarks" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="GeoBookMarks.aspx.vb" Inherits="GeoBookMarks" %>

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
    <ext:GridPanel ID="gpGeoBookMarks" runat="server" Title="Geo BookMarks" Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="GeoBookMarksStore" runat="server" OnRefreshData="GeoBookMarksStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="ID">
                        <Fields>
                            <ext:RecordField Name="ID" />
                            <ext:RecordField Name="Name" />
                            <ext:RecordField Name="X" />
                            <ext:RecordField Name="Y" />
                            <ext:RecordField Name="Z" />
                            <ext:RecordField Name="Description" />
                            <ext:RecordField Name="UseSymbol" />
                            <ext:RecordField Name="SymbolChar" />
                            <ext:RecordField Name="SymbolFont" />
                            <ext:RecordField Name="SymbolSize" />
                            <ext:RecordField Name="MapName" />
                            <ext:RecordField Name="BmkrID" />
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="dTimeStamp" />
                            <ext:RecordField Name="OwnerGUID" />
                            <ext:RecordField Name="Deleted" />
                            <ext:RecordField Name="isURLMark" />
                            <ext:RecordField Name="sURL" />
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
                <ext:Column DataIndex="X" Header="X" Width="100" />
                <ext:Column DataIndex="Y" Header="Y" Width="100" />
                <ext:Column DataIndex="Z" Header="Z" Width="100" />
                <ext:Column DataIndex="Description" Header="Description" Width="100" />
                <ext:Column DataIndex="UseSymbol" Header="Use Symbol" Width="100" />
                <ext:Column DataIndex="SymbolChar" Header="Symbol Char" Width="100" />
                <ext:Column DataIndex="SymbolFont" Header="Symbol Font" Width="100" />
                <ext:Column DataIndex="SymbolSize" Header="Symbol Size" Width="100" />
                <ext:Column DataIndex="MapName" Header="Map Name" Width="100" />
                <ext:Column DataIndex="BmkrID" Header="BmkrID" Width="100" Hidden="true" />
                <ext:Column DataIndex="GUID1" Header="GUID1" Width="100" />
                <ext:Column DataIndex="dTimeStamp" Header="dTimeStamp" Width="100" />
                <ext:Column DataIndex="OwnerGUID" Header="Owner GUID" Width="100" />
                <ext:Column DataIndex="Deleted" Header="Deleted" Width="100" />
                <ext:Column DataIndex="isURLMark" Header="Is URLMark" Width="100" />
                <ext:Column DataIndex="sURL" Header="sURL" Width="100" />
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
                    <RowSelect Handler="#{frmGeoBookMarks}.getForm().loadRecord(record); #{cboGeoBookMarksGroups}.setValue(record.data.BmkrID);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmGeoBookMarks" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Geo BookMarks Detail" DefaultAnchor="100%">
        <Items>
            <ext:NumberField ID="nbfId" runat="server" FieldLabel="ID" DataIndex="ID" AllowBlank="false"
                MsgTarget="Under" Disabled="true" />
            <ext:ComboBox ID="cboGeoBookMarksGroups" runat="server" EmptyText="Select a geobookmark group..."
                Editable="false" FieldLabel="Geobookmark groups" Width="320" DisplayField="GeoBookMarksCategoriesName"
                ValueField="GeoBookMarksCategoriesId" ForceSelection="true" AllowBlank="false"
                MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="GeoBookMarksGroupStore" AutoLoad="false" OnRefreshData="GeoBookMarksGroupStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="GeoBookMarksCategoriesId">
                                <Fields>
                                    <ext:RecordField Name="GeoBookMarksCategoriesId" Type="String" Mapping="GeoBookMarksCategoriesId" />
                                    <ext:RecordField Name="GeoBookMarksCategoriesName" Type="String" Mapping="GeoBookMarksCategoriesName" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" DataIndex="Name" AllowBlank="true"
                MsgTarget="Under" />
            <ext:NumberField ID="nbfX" runat="server" FieldLabel="X" DataIndex="X" AllowBlank="true"
                MsgTarget="Under" />
            <ext:NumberField ID="nbfY" runat="server" FieldLabel="Y" DataIndex="Y" AllowBlank="true"
                MsgTarget="Under" />
            <ext:NumberField ID="nbfZ" runat="server" FieldLabel="Z" DataIndex="X" AllowBlank="true"
                MsgTarget="Under" />
            <ext:TextField ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="Description"
                AllowBlank="true" MsgTarget="Under" />
            <ext:Checkbox ID="chkUseSymbol" runat="server" FieldLabel="Use Symbol" DataIndex="UseSymbol">
            </ext:Checkbox>
            <ext:TextField ID="txtSymbolChar" runat="server" FieldLabel="Symbol Char" DataIndex="SymbolChar"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtSymbolFont" runat="server" FieldLabel="Symbol Font" DataIndex="Symbol Font"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtSymbolSize" runat="server" FieldLabel="Symbol Size" DataIndex="SymbolSize"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtMapName" runat="server" FieldLabel="Map Name" DataIndex="MapName"
                AllowBlank="true" MsgTarget="Under" />
            <%--     <ext:NumberField ID="nbfBmkrID" runat="server" FieldLabel="BmkrID" DataIndex="BmkrID"
                AllowBlank="true" MsgTarget="Under" Disabled="true" />--%>
            <ext:TextField ID="txtsGUID" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Disabled="true" />
            <ext:DateField ID="dfdTimeStamp" runat="server" Format="dd/MM/yyyy" Vtype="daterange"
                FieldLabel="dTimeStamp (dd/mm/yyy)" AnchorHorizontal="100%" EnableKeyEvents="true"
                Hidden="true">
            </ext:DateField>
            <ext:TextField ID="txtOwnerGUID" runat="server" FieldLabel="Owner GUID" DataIndex="OwnerGUID"
                AllowBlank="true" MsgTarget="Under" Disabled="true" />
            <ext:Checkbox ID="chkDeleted" runat="server" FieldLabel="Deleted" DataIndex="Deleted">
            </ext:Checkbox>
            <ext:Checkbox ID="chkisURLMark" runat="server" FieldLabel="is URLMark" DataIndex="isURLMark">
            </ext:Checkbox>
            <ext:TextArea ID="txtasURL" runat="server" FieldLabel="sURL" DataIndex="sURL" AllowBlank="true"
                MsgTarget="Under" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmGeoBookMarks}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmGeoBookMarks}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmGeoBookMarks}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
