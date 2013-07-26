<%@ Page Title="Oasis Admin Tools --- Map Library" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="TtkGISProjectDef.aspx.vb" validateRequest="false" Inherits="TtkGISProjectDef" %>

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
    <ext:GridPanel ID="gpTtkGISProjectDef" runat="server" Title="OASIS Map Library" Margins="0 0 5 5"
        Frame="true" Height="320">
        <Store>
            <ext:Store ID="TtkGISProjectDefStore" runat="server" OnRefreshData="TtkGISProjectDefStore_Refresh">
                <Reader>
                    <ext:JsonReader IDProperty="sGUID">
                        <Fields>
                            <ext:RecordField Name="InUse" />
                            <ext:RecordField Name="sName" />
                            <ext:RecordField Name="MapData" />
                            <ext:RecordField Name="sGUID" />
                            <ext:RecordField Name="XMIN" />
                            <ext:RecordField Name="XMAX" />
                            <ext:RecordField Name="YMIN" />
                            <ext:RecordField Name="YMAX" />
                            <ext:RecordField Name="sInfo" />
                            <ext:RecordField Name="bSavedToDB" Type="Boolean" />
                            <ext:RecordField Name="sFilePath" />
                            <ext:RecordField Name="centerX" Type="String" />
                            <ext:RecordField Name="centerY" Type="String" />
                            <ext:RecordField Name="EPSG" Type="String" />
                            <ext:RecordField Name="scale" />
                            <ext:RecordField Name="CreatedBy" />
                            <ext:RecordField Name="CreatedDate" Type="String" />
                            <ext:RecordField Name="Description" />
                            <ext:RecordField Name="Contact" />
                            <ext:RecordField Name="Restrictions" />
                            <ext:RecordField Name="Copyright" />
                            <ext:RecordField Name="url" />
                            <ext:RecordField Name="StandardLyrs" />
                            <ext:RecordField Name="Source" />
                            <ext:RecordField Name="AdminLyr1Name" />
                            <ext:RecordField Name="AdminLyr2Name" />
                            <ext:RecordField Name="AdminLyr3Name" />
                            <ext:RecordField Name="AdminLyr4Name" />
                            <ext:RecordField Name="AdminLyr5Name" />
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
                <ext:Column DataIndex="InUse" Header="In Use" Width="100" />
                <ext:Column DataIndex="sName" Header="Name" Width="100" />
                <ext:Column ColumnID="sGUID" Header="sGUID" Width="100" DataIndex="sGUID" Hidden="false" />
                <ext:Column DataIndex="MapData" Header="Map Data" Width="100" />
                <ext:Column DataIndex="XMIN" Header="X MIN" Width="100" />
                <ext:Column DataIndex="XMAX" Width="100" Header="X MAX" />
                <ext:Column DataIndex="YMIN" Width="100" Header="Y MIN" />
                <ext:Column DataIndex="YMAX" Width="100" Header="Y MAX" />
                <ext:Column DataIndex="sInfo" Header="Info" />
                <ext:Column DataIndex="bSavedToDB" Header="SavedToDB" />
                <ext:Column DataIndex="sFilePath" Header="FilePath" />
                <ext:Column DataIndex="centerX" Header="Center X" />
                <ext:Column DataIndex="centerY" Header="Center Y" />
                <ext:Column DataIndex="EPSG" Header="EPSG" />
                <ext:Column DataIndex="scale" Header="scale" />
                <ext:Column DataIndex="CreatedBy" Header="Create By" />
                <ext:Column DataIndex="CreatedDate" Header="Created Date" />
                <ext:Column DataIndex="Description" Header="Description" />
                <ext:Column DataIndex="Contact" Header="Contact" />
                <ext:Column DataIndex="Restrictions" Header="Restrictions" />
                <ext:Column DataIndex="Copyright" Header="Copyright" />
                <ext:Column DataIndex="url" Header="URL" />
                <ext:Column DataIndex="StandardLyrs" Header="Standard Lyrs" />
                <ext:Column DataIndex="Source" Header="Source" />
                <ext:Column DataIndex="AdminLyr1Name" Header="AdminLyr1Name" />
                <ext:Column DataIndex="AdminLyr2Name" Header="AdminLyr2Name" />
                <ext:Column DataIndex="AdminLyr3Name" Header="AdminLyr3Name" />
                <ext:Column DataIndex="AdminLyr4Name" Header="AdminLyr4Name" />
                <ext:Column DataIndex="AdminLyr5Name" Header="AdminLyr5Name" />
            </Columns>
        </ColumnModel>
        <DirectEvents>
            <Command OnEvent="RowDelete">
                <EventMask ShowMask="true" />
                <ExtraParams>
                    <ext:Parameter Name="sGUID" Value="record.data.sGUID" Mode="Raw" />
                </ExtraParams>
                <Confirmation BeforeConfirm="if (command!='Delete') return false;" ConfirmRequest="true"
                    Message="Are you sure to delete this record" Title="Attention" />
            </Command>
        </DirectEvents>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                <Listeners>
                    <RowSelect Handler="#{frmTtkGISProjectDef}.getForm().loadRecord(record);" />
                </Listeners>
                <%--                <DirectEvents>
                    <RowSelect onevent="btnSave_Click" before="return #{frmTtkGISProjectDef}.isValid();">
                        <EventMask ShowMask="true" Msg="Saving..." />
                    </RowSelect>
                </DirectEvents>--%>
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:FormPanel ID="frmTtkGISProjectDef" runat="server" Split="true" Margins="0 5 5 5"
        Frame="true" Title="OASIS Map Library Detail" DefaultAnchor="100%">
        <Items>
            <ext:Checkbox ID="chkInUse" runat="server" FieldLabel="In Use" DataIndex="InUse">
            </ext:Checkbox>
            <ext:TextField ID="txtsName" runat="server" FieldLabel="Name" DataIndex="sName" AllowBlank="false"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtsGUID" runat="server" FieldLabel="sGUID" DataIndex="sGUID"
                AllowBlank="false" MsgTarget="Under" Disabled="true" />
            <ext:TextArea ID="txtaMapData" runat="server" FieldLabel="Map Data" DataIndex="MapData"
                AllowBlank="true" MsgTarget="Under" Height="400" />
            <ext:TextField ID="txtXMIN" runat="server" FieldLabel="X MIN" DataIndex="XMIN"
                AllowBlank="true" MsgTarget="Under" MaskRe="[0-9]" />
            <ext:TextField ID="txtXMAX" runat="server" FieldLabel="X MAX" DataIndex="XMAX"
                AllowBlank="true" MsgTarget="Under" MaskRe="[0-9]"/>
            <ext:TextField ID="txtYMIN" runat="server" FieldLabel="Y MIN" DataIndex="YMIN"
                AllowBlank="true" MsgTarget="Under" MaskRe="[0-9]" />
            <ext:TextField ID="txtYMAX" runat="server" FieldLabel="Y MAX" DataIndex="YMAX"
                AllowBlank="true" MsgTarget="Under" MaskRe="[0-9]" />
            <ext:TextField ID="txtSInfo" runat="server" FieldLabel="Info" DataIndex="sInfo" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:Checkbox ID="chkbSavedToDB" runat="server" FieldLabel="Saved To DataBase" DataIndex="bSavedToDB" />
<%--            <ext:Checkbox ID="chkbUGMap" runat="server" FieldLabel="UGMap" DataIndex="bUGMap" />--%>
            <ext:TextField ID="txtsFilePath" runat="server" FieldLabel="File Path" DataIndex="sFilePath"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtCenterX" runat="server" FieldLabel="Center X" DataIndex="centerX"
                AllowBlank="true" MsgTarget="Under" MaxLength="255"  />
            <ext:TextField ID="txtCenterY" runat="server" FieldLabel="Center Y" DataIndex="centerY"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtEPSG" runat="server" FieldLabel="EPSG" DataIndex="EPSG" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtScale" runat="server" FieldLabel="Scale" DataIndex="scale"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtCreateBy" runat="server" FieldLabel="Created By" DataIndex="CreatedBy" />
            <ext:TextField ID="txtCreateDate" runat="server" FieldLabel="Created Date" DataIndex="CreatedDate" />
            <%--  <ext:DateField ID="dfCreateDate" runat="server" Format="dd/MM/yyyy" Vtype="daterange"
                FieldLabel="Create Date (dd/mm/yyy)" EnableKeyEvents="true" Hidden="false" DataIndex="CreatedDate">
            </ext:DateField>--%>
            <ext:TimeField ID="tfCreateTime" runat="server" Format="HH:mm" Increment="1" FieldLabel="Create Time (HH:mm)"
                EnableKeyEvents="true" Hidden="false" DataIndex="CreateDate" Visible="false">
            </ext:TimeField>
            <ext:TextArea ID="txtDescription" runat="server" FieldLabel="Description" DataIndex="Description"
                AllowBlank="true" MsgTarget="Under" />
            <ext:TextField ID="txtContact" runat="server" FieldLabel="Contact" DataIndex="Contact"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtRestrictions" runat="server" FieldLabel="Restrictions" DataIndex="Restrictions"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
            <ext:TextField ID="txtCopyright" runat="server" FieldLabel="Copyright" DataIndex="Copyright"
                AllowBlank="true" MsgTarget="Under" MaxLength="150" />
            <ext:TextField ID="txturl" runat="server" FieldLabel="URL" DataIndex="url" AllowBlank="true"
                MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtStandardLyrs" runat="server" FieldLabel="StandardLyrs" DataIndex="StandardLyrs"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtSource" runat="server" FieldLabel="Source" DataIndex="Source"
                AllowBlank="true" MsgTarget="Under" MaxLength="255" />
            <ext:TextField ID="txtAdminLyr1Name" runat="server" FieldLabel="Admin Layer 1" DataIndex="AdminLyr1Name"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
            <ext:TextField ID="txtAdminLyr2Name" runat="server" FieldLabel="Admin Layer 2" DataIndex="AdminLyr2Name"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
            <ext:TextField ID="txtAdminLyr3Name" runat="server" FieldLabel="Admin Layer 3" DataIndex="AdminLyr3Name"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
            <ext:TextField ID="txtAdminLyr4Name" runat="server" FieldLabel="Admin Layer 4" DataIndex="AdminLyr4Name"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
            <ext:TextField ID="txtAdminLyr5Name" runat="server" FieldLabel="Admin Layer 5" DataIndex="AdminLyr5Name"
                AllowBlank="true" MsgTarget="Under" MaxLength="250" />
        </Items>
        <Buttons>
            <ext:Button ID="btnInsert" runat="server" Text="Insert">
                <DirectEvents>
                    <Click OnEvent="btnInsert_Click" Before="return #{frmTtkGISProjectDef}.isValid();">
                        <EventMask ShowMask="true" Msg="Inserting..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnUpdate" runat="server" Text="Update">
                <DirectEvents>
                    <Click OnEvent="btnUpdate_Click" Before="return #{frmTtkGISProjectDef}.isValid();">
                        <EventMask ShowMask="true" Msg="Updating..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnResetFields" runat="server" Text="Reset Fields">
                <Listeners>
                    <Click Handler="#{frmTtkGISProjectDef}.getForm().reset();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>
