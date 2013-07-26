<%@ Page Title="Oasis Admin Tools ---  Dynamic Modules" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="DynamicDataDefsNew.aspx.vb" Inherits="DynamicDataDefs" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <script type="text/javascript">
        Ext.data.Connection.override({
            timeout: 120000
        });
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
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
    <ext:GridPanel ID="gpDynamicDataDefs" runat="server" Title="Dynamic Data " Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="DynamicDataDefsStore" runat="server" OnRefreshData="DynamicDataDefsStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="HiddenDDDefName">
                        <Fields>
                            <ext:RecordField Name="DDDefName" />
                            <ext:RecordField Name="Description" />
                            <ext:RecordField Name="AccessRights" />
                            <ext:RecordField Name="ConnectionString" />
                            <ext:RecordField Name="Synch" />
                            <ext:RecordField Name="EnableDataEntry" />
                            <ext:RecordField Name="EnableReporting" />
                            <ext:RecordField Name="LockedFields" />
                            <ext:RecordField Name="ExcludedFields" />
                            <ext:RecordField Name="HiddenDDDefName" />
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
                <ext:Column DataIndex="DDDefName" Header="Dynamic Data Name" Width="100" Hidden="false" />
                <ext:Column DataIndex="Description" Header="Description" Width="100" />
                <ext:Column DataIndex="AccessRights" Width="100" Header="Access Rights" />
                <ext:Column DataIndex="ConnectionString" Width="100" Header="Connection String" />
                <ext:Column DataIndex="Synch" Width="100" Header="Synch" />
                <ext:Column DataIndex="EnableDataEntry" Width="100" Header="Enable Data Entry" />
                <ext:Column DataIndex="EnableReporting" Width="100" Header="Enable Reporting" />
                <ext:Column DataIndex="LockedFields" Width="100" Header="Locked Fields" />
                <ext:Column DataIndex="ExcludedFields" Width="100" Header="Excluded Fields" />
                <ext:Column ColumnID="HiddenDDDefName" Header="HiddenDDDefName" DataIndex="HiddenDDDefName"
                    Width="100" Hidden="true" />
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmDynamicDataDefs}.getForm().loadRecord(record);" />
                </Listeners>
                <DirectEvents>
                    <RowSelect OnEvent="RowSelect" Buffer="100">
                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{frmDynamicDataDefs}" />
                        <ExtraParams>
                            <%-- or can use params[2].id as value %> --%>
                            <ext:Parameter Name="DDefName" Value="record.data.DDDefName" Mode="Raw" />
                            <ext:Parameter Name="AccessRights" Value="record.data.AccessRights" Mode="Raw" />
                            <ext:Parameter Name="LockFields" Value="record.data.LockedFields" Mode="Raw" />
                            <ext:Parameter Name="ExcludeFileds" Value="record.data.ExcludedFields" Mode="Raw" />
                        </ExtraParams>
                    </RowSelect>
                </DirectEvents>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="DDDefName" Value="record.data.DDDefName" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmDynamicDataDefs" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="Dynamic Data Detail" DefaultAnchor="100%">
        <Items>
            <ext:TextField ID="txtDDDefName" runat="server" FieldLabel="Dynamic Data Name" DataIndex="DDDefName"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextField ID="txtHiddenDDDefName" runat="server" FieldLabel="HiddenDDDefName"
                DataIndex="HiddenDDDefName" AllowBlank="true" MsgTarget="Under" Hidden="true" />
            <ext:TextField ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="Description"
                AllowBlank="false" MsgTarget="Under" />
            <ext:TextArea IDMode="Client" Name="txtaAccessRights" ID="txtaAccessRights" runat="server"
                FieldLabel="Access Rights" DataIndex="AccessRights" AllowBlank="true" MsgTarget="Under"
                Disabled="true" />
            <ext:GridPanel ID="gpTableName" runat="server" StripeRows="true" Title="Access Rights"
                DisableSelection="false" Width="600" Height="300">
                <Store>
                    <ext:Store ID="StoreTableName" runat="server" OnRefreshData="StoreTableName_Refresh">
                        <Reader>
                            <ext:JsonReader IDProperty="TBName">
                                <Fields>
                                    <ext:RecordField Name="TBName" Type="String" />
                                    <ext:RecordField Name="Read" Type="Boolean" />
                                    <ext:RecordField Name="Add" Type="Boolean" />
                                    <ext:RecordField Name="Edit" Type="Boolean" />
                                    <ext:RecordField Name="Delete" Type="Boolean" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel2" runat="server">
                    <Columns>
                        <ext:RowNumbererColumn />
                        <ext:Column DataIndex="TBName" Header="Table Name" ColumnID="TBName" Width="240" />
                        <ext:CheckColumn DataIndex="Read" Header="Read" Editable="true" />
                        <ext:CheckColumn DataIndex="Add" Header="Add" Editable="true" />
                        <ext:CheckColumn DataIndex="Edit" Header="Edit" Editable="true" />
                        <ext:CheckColumn DataIndex="Delete" Header="Delete" Editable="true" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="10" />
                </BottomBar>
            </ext:GridPanel>
            <ext:TextArea ID="txtaConnectionString" runat="server" FieldLabel="Connection String"
                DataIndex="ConnectionString" AllowBlank="true" MsgTarget="Under" />
            <ext:Checkbox ID="chkSynch" runat="server" FieldLabel="Synch" DataIndex="Synch">
            </ext:Checkbox>
            <ext:Checkbox ID="chkEnableDataEntry" runat="server" FieldLabel="Enable Data Entry"
                DataIndex="EnableDataEntry">
            </ext:Checkbox>
            <ext:Checkbox ID="chkEnableReporting" runat="server" FieldLabel="Enable Reporting"
                DataIndex="EnableReporting">
            </ext:Checkbox>
            <ext:TextArea ID="txtaLockedFields" runat="server" FieldLabel="Locked Fields" DataIndex="LockedFields"
                AllowBlank="true" MsgTarget="Under" Disabled="true" />
            <ext:TextArea ID="txtaExcludedFields" runat="server" FieldLabel="Excluded Fields"
                DataIndex="ExcludedFields" AllowBlank="true" MsgTarget="Under" Disabled="true" />
            <ext:GridPanel ID="gpTableColumnName" runat="server" StripeRows="true" Title="Lock or Exclude fileds"
                DisableSelection="false" Width="600" Height="300">
                <Store>
                    <ext:Store ID="StoreTableColumnName" runat="server" GroupField="TName">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="TName" Type="String" />
                                    <ext:RecordField Name="CName" Type="String" />
                                    <ext:RecordField Name="LockFiled" Type="Boolean" />
                                    <ext:RecordField Name="ExcludeFiled" Type="Boolean" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel3" runat="server">
                    <Columns>
                        <ext:Column Header="Tables" DataIndex="TName" Hidden="true" />
                        <ext:Column Header="Fileds" DataIndex="CName" Width="320" />
                        <ext:CheckColumn Header="Locked Filed" DataIndex="LockFiled" Editable="true" />
                        <ext:CheckColumn Header="Excluded Filed" DataIndex="ExcludeFiled" Editable="true" />
                    </Columns>
                </ColumnModel>
                <View>
                    <ext:GroupingView ID="GroupingView1" runat="server" StartCollapsed="true">
                    </ext:GroupingView>
                </View>
                <DirectEvents>
                    <Command OnEvent="SelectGroup_Click">
                        <EventMask ShowMask="true" />
                        <ExtraParams>
                            <ext:Parameter Name="values" Value="#{gpTableName}.getRowsValues()" Mode="Raw" Encode="true" />
                            <ext:Parameter Name="gpTableColumnNameRowsValues" Value="#{gpTableColumnName}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                            <ext:Parameter Name="groupId" Value="groupId" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Processing..." />
                    </Command>
                </DirectEvents>
            </ext:GridPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmDynamicDataDefs}.isValid();">
                        <ExtraParams>
                            <ext:Parameter Name="values" Value="#{gpTableName}.getRowsValues()" Mode="Raw" Encode="true" />
                            <ext:Parameter Name="gpTableColumnName" Value="#{gpTableColumnName}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <ToolTips>
                    <ext:ToolTip ID="ToolTip1" runat="server" Html="<p><b>Notice</b></p><p>Dynamic Data def is updated by <b>Name.</b> If you wanna copy data from to another , you should change the name to copy it.</p>" />
                </ToolTips>
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmDynamicDataDefs}.isValid();">
                        <ExtraParams>
                            <ext:Parameter Name="values" Value="#{gpTableName}.getRowsValues()" Mode="Raw" Encode="true" />
                            <ext:Parameter Name="gpTableColumnName" Value="#{gpTableColumnName}.getRowsValues()"
                                Mode="Raw" Encode="true" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmDynamicDataDefs}.getForm().reset();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="ClearAccessRightTable_Click">
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
