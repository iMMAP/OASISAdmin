<%@ Page Title="Oasis Admin Tools ---  Validation" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="Validation.aspx.vb" Inherits="Validation" %>

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
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpValidation}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpValidation" runat="server" Title="Validation List" Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="ValidationStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="sDataEntryTableName" />
                            <ext:RecordField Name="sDataEntryFieldName" />
                            <ext:RecordField Name="bRequired" />
                            <ext:RecordField Name="sEditMask" />
                            <ext:RecordField Name="sValidation" />
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
                <ext:Column DataIndex="sDataEntryTableName" Header="Data Entry Table Name" Width="200"
                    Hidden="false" />
                <ext:Column DataIndex="sDataEntryFieldName" Header="Data Entry Field Name" Width="200"
                    Hidden="false" />
                <ext:Column DataIndex="bRequired" Header="Required" Width="100" Hidden="false" />
                <ext:Column DataIndex="sEditMask" Header="Edit Mask" Width="200" Hidden="false" />
                <ext:Column DataIndex="sValidation" Header="Validation" Width="200" Hidden="false" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmValidation}.getForm().loadRecord(record);" />
                </Listeners>
                <DirectEvents>
                    <RowSelect OnEvent="GBValidation_RowSelect" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmValidation}" />
                        <ExtraParams>
                            <ext:Parameter Name="sDataEntryTableName" Value="record.data.sDataEntryTableName"
                                Mode="Raw" />
                            <ext:Parameter Name="sDataEntryFieldName" Value="record.data.sDataEntryFieldName"
                                Mode="Raw" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="GPValidation_RowDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpValidation}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:FormPanel ID="frmValidation" runat="server" Split="true" Margins="0 5 5 5" Frame="true"
        Title="Nearby Features Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtGUID1" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:ComboBox ID="cboDataEntryTableName" runat="server" EmptyText="Select a data entry table name..."
                Editable="false" FieldLabel="Data Entry Table Name" Width="320" DisplayField="TableName"
                DataIndex="sDataEntryTableName" ValueField="TableName" ForceSelection="true"
                AllowBlank="false" MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="TableNameStore" AutoLoad="false" OnRefreshData="TableNameStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="TableName">
                                <Fields>
                                    <ext:RecordField Name="TableName" Type="String" Mapping="TableName" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <DirectEvents>
                    <Select OnEvent="cboTableName_Select">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmValidation}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
            <ext:ComboBox ID="cboDataEntryFieldName" runat="server" EmptyText="Select a data entry field name..."
                Editable="false" FieldLabel="Data Entry Filed Name" Width="320" DisplayField="COLUMN_NAME"
                DataIndex="sDataEntryFieldName" ValueField="COLUMN_NAME" ForceSelection="true"
                AllowBlank="false" MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="ColumnNameStore" AutoLoad="false" OnRefreshData="ColumnNameStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="COLUMN_NAME">
                                <Fields>
                                    <ext:RecordField Name="COLUMN_NAME" Type="String" Mapping="COLUMN_NAME" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:Checkbox ID="chkRequired" runat="server" FieldLabel="Required" DataIndex="bRequired"
                Hidden="false" />
            <ext:TextField ID="txtEditMask" runat="server" FieldLabel="Edit Mask" DataIndex="sEditMask"
                AllowBlank="false" MsgTarget="Under" Hidden="false" MaxLength="255" />
            <ext:TextField ID="txtValidation" runat="server" FieldLabel="Validation" DataIndex="sValidation"
                AllowBlank="false" MsgTarget="Under" MaxLength="255" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmValidation}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmValidation}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmValidation}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
