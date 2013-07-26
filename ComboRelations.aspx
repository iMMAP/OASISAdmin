<%@ Page Title="" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false"
    CodeFile="ComboRelations.aspx.vb" Inherits="ComboRelations" %>

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
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpComboRelation}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpComboRelation" runat="server" Title="Combobox Relation List"
        Margins="0 0 5 5" Frame="true" Height="320">
        <Store>
            <ext:Store ID="ComboRelationStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="sTableName" />
                            <ext:RecordField Name="sFieldName" />
                            <ext:RecordField Name="sParentFieldName" />
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
                <ext:Column DataIndex="sTableName" Header="Table Name" Width="200" Hidden="false" />
                <ext:Column DataIndex="sFieldName" Header="Field Name" Width="200" Hidden="false" />
                <ext:Column DataIndex="sParentFieldName" Header="Parent Field Name" Width="200" Hidden="false" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmComboRelation}.getForm().loadRecord(record);" />
                </Listeners>
                <DirectEvents>
                    <RowSelect OnEvent="GBComboRelaion_RowSelect" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmComboRelation}" />
                        <ExtraParams>
                            <ext:Parameter Name="sTableName" Value="record.data.sTableName" Mode="Raw" />
                            <ext:Parameter Name="sFieldName" Value="record.data.sFieldName" Mode="Raw" />
                            <ext:Parameter Name="sParentFieldName" Value="record.data.sParentFieldName" Mode="Raw" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="GPComboRelation_RowDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpComboRelation}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:FormPanel ID="frmComboRelation" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Nearby Features Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtGUID1" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:ComboBox ID="cboTableName" runat="server" EmptyText="Select a table..." Editable="false"
                FieldLabel="Table Name" Width="320" DisplayField="TableName" DataIndex="sTableName"
                ValueField="TableName" ForceSelection="true" AllowBlank="false" MsgTarget="Under">
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
            <ext:ComboBox ID="cboFieldName" runat="server" EmptyText="Select a field name..."
                Editable="false" FieldLabel="Field Name" Width="320" DisplayField="COLUMN_NAME"
                DataIndex="sFieldName" ValueField="COLUMN_NAME" ForceSelection="true"
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
                <RemoteValidation OnValidation="CheckSameColumn" ValidationEvent="select" EventOwner="Field">
                </RemoteValidation>
            </ext:ComboBox>
            <ext:ComboBox ID="cboParentFieldName" runat="server" EmptyText="Select a parent field name..."
                Editable="false" FieldLabel="Parent Field Name" Width="320" DisplayField="COLUMN_NAME"
                DataIndex="sParentFieldName" ValueField="COLUMN_NAME" ForceSelection="true" AllowBlank="false"
                MsgTarget="Under">
                <Store>
                    <ext:Store runat="server" ID="ParentColumnNameStore" AutoLoad="false" OnRefreshData="ParentColumnNameStore_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="COLUMN_NAME">
                                <Fields>
                                    <ext:RecordField Name="COLUMN_NAME" Type="String" Mapping="COLUMN_NAME" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
              <%--  <RemoteValidation OnValidation="CheckSameColumn" ValidationEvent="select" EventOwner="Field">
                </RemoteValidation>--%>
            </ext:ComboBox>
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmComboRelation}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmComboRelation}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmComboRelation}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
