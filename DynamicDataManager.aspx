<%@ Page Title="Oasis Admin Tools ---  Dynamic Module Manager" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="DynamicDataManager.aspx.vb" Inherits="DynamicDataManager" %>

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
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpSpecification}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpSpecification" runat="server" Title="" Margins="0 0 5 5" Frame="true"
        Height="141" AutoExpandColumn="sTableName">
        <Store>
            <ext:Store ID="SpecificationStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="lRank" />
                            <ext:RecordField Name="sTableName" />
                            <ext:RecordField Name="sCaption" />
                            <ext:RecordField Name="sDescription" />
                            <ext:RecordField Name="lDescFontSize" />
                            <ext:RecordField Name="sDataEntryFields" />
                            <ext:RecordField Name="bIsMasterTable" />
                            <ext:RecordField Name="bIsLinkedTable" />
                            <ext:RecordField Name="sGridQuery" />
                            <ext:RecordField Name="sGridQueryMSSQL" />
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
                <ext:Column DataIndex="lRank" Header="Rank" Width="70" Hidden="false" />
                <ext:Column DataIndex="sTableName" Header="Table Name" Width="100" Hidden="false" />
                <ext:Column DataIndex="sCaption" Header="Caption" Width="100" Hidden="false" />
                <ext:Column DataIndex="sDescription" Header="Description" Width="100" Hidden="false" />
                <ext:Column DataIndex="lDescFontSize" Header="Font Size" Hidden="true" />
                <ext:Column DataIndex="sDataEntryFields" Width="100" Header="Data Entry Fields" Hidden="true" />
                <ext:Column DataIndex="bIsMasterTable" Header="Master Table" Width="100" Hidden="true" />
                <ext:Column DataIndex="bIsLinkedTable" Header="Linked Table" Width="100" Hidden="true" />
                <ext:Column DataIndex="sGridQuery" Header="sGridQuery" Width="100" Hidden="true" />
                <ext:Column DataIndex="sGridQueryMSSQL" Header="sGridQueryMSSQL" Hidden="true" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmSpecification}.getForm().loadRecord(record);" />
                </Listeners>
                <DirectEvents>
                    <RowSelect OnEvent="RowSelectSpecification" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmSpecification}" />
                        <ExtraParams>
                            <ext:Parameter Name="sTableName" Value="record.data.sTableName" Mode="Raw" />
                            <ext:Parameter Name="sDataEntryFields" Value="record.data.sDataEntryFields" Mode="Raw" />
                            <ext:Parameter Name="islRankDisabled" Value="False" Mode="Value" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="RowSpecificationDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpSpecification}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:GridPanel ID="gpSpecificationDynamicData" runat="server" Title="" Margins="0 0 5 5"
        Frame="true" Height="303" AutoExpandColumn="sTableName">
        <Store>
            <ext:Store ID="SpecificationDynamicDataStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="lRank" />
                            <ext:RecordField Name="sTableName" />
                            <ext:RecordField Name="sCaption" />
                            <ext:RecordField Name="sDescription" />
                            <ext:RecordField Name="lDescFontSize" />
                            <ext:RecordField Name="sDataEntryFields" />
                            <ext:RecordField Name="bIsMasterTable" />
                            <ext:RecordField Name="bIsLinkedTable" />
                            <ext:RecordField Name="sGridQuery" />
                            <ext:RecordField Name="sGridQueryMSSQL" />
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
                <ext:Column ColumnID="GUID1" DataIndex="GUID1" Header="GUID1" Hidden="true" />
                <ext:Column DataIndex="lRank" Header="Rank" Width="70" Hidden="true" />
                <ext:Column DataIndex="sTableName" Header="Table Name" Width="100" Hidden="false" />
                <ext:Column DataIndex="sCaption" Header="Caption" Width="100" Hidden="false" />
                <ext:Column DataIndex="sDescription" Header="Description" Width="100" Hidden="false" />
                <ext:Column DataIndex="lDescFontSize" Header="Font Size" Hidden="true" />
                <ext:Column DataIndex="sDataEntryFields" Width="100" Header="Data Entry Fields" Hidden="true" />
                <ext:Column DataIndex="bIsMasterTable" Header="Master Table" Width="100" Hidden="true" />
                <ext:Column DataIndex="bIsLinkedTable" Header="Linked Table" Width="100" Hidden="true" />
                <ext:Column DataIndex="sGridQuery" Header="sGridQuery" Width="100" Hidden="true" />
                <ext:Column DataIndex="sGridQueryMSSQL" Header="sGridQueryMSSQL" Hidden="true" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmSpecification}.getForm().loadRecord(record);" />
                </Listeners>
                <DirectEvents>
                    <RowSelect OnEvent="RowSelectSpecification" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmSpecification}" />
                        <ExtraParams>
                            <ext:Parameter Name="sTableName" Value="record.data.sTableName" Mode="Raw" />
                            <ext:Parameter Name="sDataEntryFields" Value="record.data.sDataEntryFields" Mode="Raw" />
                            <ext:Parameter Name="islRankDisabled" Value="True" Mode="Value" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="RowSpecificationDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpSpecification}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:FormPanel ID="frmSpecification" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Specification Detail" DefaultAnchor="100%">
        <Items>
            <ext:ComboBox ID="cboTableName" runat="server" EmptyText="Select a table name..."
                Editable="false" FieldLabel="Table Names" Width="320" DisplayField="TableName"
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
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmSpecification}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
            <ext:TextField ID="txtGUID1" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:NumberField ID="nbflRank" runat="server" FieldLabel="Rank" DataIndex="lRank"
                AllowBlank="false" MsgTarget="Under" Hidden="false" />
            <ext:TextField ID="txtCaption" runat="server" FieldLabel="Caption" DataIndex="sCaption"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextField ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="sDescription"
                AllowBlank="true" MsgTarget="Under" />
            <ext:NumberField ID="nbfFontSize" runat="server" FieldLabel="Font Size" DataIndex="lDescFontSize"
                AllowBlank="false" MsgTarget="Under" Hidden="false" />
            <ext:Checkbox ID="chkIsMaster" runat="server" FieldLabel="Master Table" DataIndex="bIsMasterTable"
                Hidden="false" />
            <ext:Checkbox ID="chkIsLinkedTable" runat="server" FieldLabel="Linked Table" DataIndex="bIsLinkedTable"
                Hidden="false" />
            <ext:TextArea ID="txtaGridQuery" runat="server" FieldLabel="Grid Query" DataIndex="sGridQuery"
                AllowBlank="true" MsgTarget="Under" Height="200" />
            <ext:TextArea ID="txtaGridQueryMSSQL" runat="server" FieldLabel="Grid Query MSSQL"
                DataIndex="sGridQueryMSSQL" AllowBlank="true" MsgTarget="Under" Height="200" />
            <ext:TextField ID="txtHiddenDataFileds" runat="server" FieldLabel="" DataIndex="sTableName"
                Hidden="true" Disabled="true" />
            <ext:GridPanel ID="gpSpecificationDataFileds" runat="server" StripeRows="false" Title="Data fileds"
                DisableSelection="false" Width="600" Height="300" AutoExpandColumn="Caption">
                <Store>
                    <ext:Store ID="StoreSpecificationDataFileds" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="FieldName" Type="String" />
                                    <ext:RecordField Name="Caption" Type="String" />
                                    <ext:RecordField Name="IsCheck" Type="Boolean" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel3" runat="server">
                    <Columns>
                        <ext:CheckColumn Header="" DataIndex="IsCheck" Width="50" Editable="true" />
                        <ext:Column Header="Field Name" DataIndex="FieldName" Width="200" Editable="false" />
                        <ext:Column Header="Caption" DataIndex="Caption" Width="300" Editable="true">
                            <Editor>
                                <ext:TextField ID="txtDataFiledsCaption" runat="server" />
                            </Editor>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmSpecification}.isValid();">
                        <ExtraParams>
                            <ext:Parameter Name="JSONDataFileds" Value="#{gpSpecificationDataFileds}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmSpecification}.isValid();">
                        <ExtraParams>
                            <ext:Parameter Name="JSONDataFileds" Value="#{gpSpecificationDataFileds}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                            <ext:Parameter Name="JSONLRank" Value="#{gpSpecification}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmSpecification}.getForm().reset();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="btnfrmSpecificationReset_Click">
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
