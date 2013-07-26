<%@ Page Title="Oasis Admin Tools ---  Queries" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="Queries.aspx.vb" Inherits="Queries" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <ext:FormPanel ID="frmDynamicModule" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Dynamic Modules" DefaultAnchor="100%">
        <Items>
            <ext:ComboBox ID="cboDynamicModule" runat="server" EmptyText="Select a dynamic module..."
                Editable="false" FieldLabel="Dynamic Modules" DisplayField="DDDefName" ValueField="DDDefName"
                ForceSelection="true" DataIndex="DDDefName" AllowBlank="false" MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="DynamicDataManagerStore" OnRefreshData="DynamicDataManagerStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="DDDefName">
                                <Fields>
                                    <ext:RecordField Name="DDDefName" Type="String" Mapping="DDDefName" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <DirectEvents>
                    <Select OnEvent="cboDynamicModule_Select">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpQuries}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpQuries" runat="server" Title="Query List" Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="QuriesStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="QueryName" />
                            <ext:RecordField Name="QuerySQL" />
                            <ext:RecordField Name="QueryMSSQL" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel2" runat="server">
            <Columns>
                <ext:CommandColumn Width="50" Header="Actions">
                    <Commands>
                        <ext:GridCommand Icon="Delete" CommandName="Delete">
                            <ToolTip Text="Delete" />
                        </ext:GridCommand>
                    </Commands>
                </ext:CommandColumn>
                <ext:Column ColumnID="GUID1" DataIndex="GUID1" Header="GUID1" Hidden="true" />
                <ext:Column DataIndex="QueryName" Header="Query Name" Width="200" Hidden="false" />
                <ext:Column DataIndex="QuerySQL" Header="Query SQL" Width="200" Hidden="false" />
                <ext:Column DataIndex="QueryMSSQL" Header="Query MSSQL" Width="200" Hidden="false" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmQueries}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="gpQuries_RowDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpQuries}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:FormPanel ID="frmQueries" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="Query Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtGUID1" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:TextField ID="txtQueryName" runat="server" FieldLabel="Query Name" DataIndex="QueryName"
                AllowBlank="false" MsgTarget="Under" Hidden="false" />
            <ext:TextArea ID="txtaQuerySQL" runat="server" FieldLabel="Query SQL" DataIndex="QuerySQL"
                AllowBlank="false" Height="400" MsgTarget="Under" />
            <ext:TextArea ID="txtaQueryMSSQL" runat="server" FieldLabel="Query MSSQL" DataIndex="QueryMSSQL"
                Hidden="false" Height="400" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmQueries}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmQueries}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmQuries}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
