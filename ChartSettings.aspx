<%@ Page Title="Oasis Admin Tools ---  Chart Settings" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="ChartSettings.aspx.vb" Inherits="ChartSettings" %>

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
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpChartSettings}" />
                    </Select>
                </DirectEvents>
            </ext:ComboBox>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpChartSettings" runat="server" Title="" Margins="0 0 5 5" Frame="true"
        Height="303">
        <Store>
            <ext:Store ID="ChartSettingStore" runat="server">
                <Reader>
                    <ext:JsonReader IDProperty="GUID1">
                        <Fields>
                            <ext:RecordField Name="GUID1" />
                            <ext:RecordField Name="QueryName" />
                            <ext:RecordField Name="Group" />
                            <ext:RecordField Name="UseChart" />
                            <ext:RecordField Name="bAutoLoadReport" />
                            <ext:RecordField Name="SQLCommand" />
                            <ext:RecordField Name="MSSQLCommand" />
                            <ext:RecordField Name="FilterSQL" />
                            <ext:RecordField Name="FilterMSSQL" />
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
                <ext:Column DataIndex="QueryName" Header="Query Name" Width="100" Hidden="false" />
                <ext:Column DataIndex="Group" Header="Group" Width="100" Hidden="false" />
                <ext:Column DataIndex="UseChart" Header="Use Chart" Width="100" Hidden="false" />
                <ext:Column DataIndex="bAutoLoadReport" Header="Auto Load Report" Width="100" Hidden="false" />
                <ext:Column DataIndex="SQLCommand" Header="SQL Command" Hidden="true" />
                <ext:Column DataIndex="MSSQLCommand" Width="100" Header="MSSQL Commands" Hidden="true" />
                <ext:Column DataIndex="FilterSQL" Header="Filter SQL" Width="100" Hidden="true" />
                <ext:Column DataIndex="FilterMSSQL" Header="Filter MSSQL" Width="100" Hidden="true" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmChartSettings}.getForm().loadRecord(record);" />
                </Listeners>
            </ext:RowSelectionModel>
        </SelectionModel>
        <DirectEvents>
            <Command OnEvent="RowChartSettingsDelete">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{gpChartSettings}" />
                <ExtraParams>
                    <ext:Parameter Name="GUID1" Value="record.data.GUID1" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
    </ext:GridPanel>
    <ext:FormPanel ID="frmChartSettings" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Chart Setting Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtGUID1" runat="server" FieldLabel="GUID1" DataIndex="GUID1"
                AllowBlank="true" MsgTarget="Under" Hidden="false" Disabled="true" />
            <ext:TextField ID="txtQueryName" runat="server" FieldLabel="Query Name" DataIndex="QueryName"
                AllowBlank="false" MsgTarget="Under" Hidden="false" MaxLength="255" />
            <ext:TextField ID="txtGroup" runat="server" FieldLabel="Group" DataIndex="Group"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:Checkbox ID="chkUseChart" runat="server" FieldLabel="Use Chart" DataIndex="UseChart"
                Hidden="false" />
            <ext:Checkbox ID="chkbAutoLoadReport" runat="server" FieldLabel=" Auto Load Report"
                DataIndex="bAutoLoadReport" Hidden="false" />
            <ext:TextArea ID="txtaSQLCommand" runat="server" FieldLabel="SQL Command" DataIndex="SQLCommand"
                AllowBlank="true" MsgTarget="Under" Height="300" />
            <ext:TextArea ID="txtaMSSQLCommand" runat="server" FieldLabel="MSSQL Command" DataIndex="MSSQLCommand"
                AllowBlank="true" MsgTarget="Under" Height="300" />
            <ext:TextArea ID="txtaFilterSQL" runat="server" FieldLabel="Filter SQL" DataIndex="FilterSQL"
                AllowBlank="true" MsgTarget="Under" Height="100" />
            <ext:TextArea ID="txtaFilterMSSQL" runat="server" FieldLabel="Filter MSSQL" DataIndex="FilterMSSQL"
                AllowBlank="true" MsgTarget="Under" Height="100" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmChartSettings}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmChartSettings}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmChartSettings}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
