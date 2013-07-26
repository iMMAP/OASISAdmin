<%@ Page Title="Oasis Admin Tools --- Map view" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="MapViewSettings.aspx.vb" Inherits="MapViewSettings" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 40%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager2" runat="server" Theme="Gray" />
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" />
    User Groups&nbsp;
    <ajaxToolkit:ComboBox ID="cboUserGroups" runat="server" AutoPostBack="True" DropDownStyle="DropDownList"
        AutoCompleteMode="None" CaseSensitive="False" CssClass="WindowsStyle" ItemInsertLocation="Append">
    </ajaxToolkit:ComboBox>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSave" runat="server" Text="Save" Height="30px" Width="58px" />
    <br />
    <br />
    <ext:Panel ID="panelMap" runat="server" AutoHeight="true" Padding="5" Title="Map"
        AutoWidth="True" Height="900">
        <Content>
            <ext:FieldSet ID="FieldSet6" runat="server" Title="Avaliable Map Tools" Height="550">
                <Content>
                    <table>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom Rectangular" ToolTip="Zoom Rectangular" ID="Image1"
                                    runat="server" ImageUrl="~/Content/Admintools/ZoomRect.png" /><asp:CheckBox ID="chkZoomRect"
                                        runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnZoomRect)" />
                                <asp:Label ID="lblZoomRectangular" runat="server" Text="Zoom Rectangular" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom In Map" ToolTip="Zoom In Map" ID="Image2" runat="server"
                                    ImageUrl="~/Content/Admintools/ZoomIn.png" />
                                <asp:CheckBox ID="chkZoomIn" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnZoomin)" />
                                <asp:Label ID="Label2" runat="server" Text="Zoom In Map" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom Out in Map" ToolTip="Zoom Out in Map" ID="Image3"
                                    runat="server" ImageUrl="~/Content/Admintools/ZoomOut.png" /><asp:CheckBox ID="chkZoomOut"
                                        runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnZoomout)" />
                                <asp:Label ID="Label3" runat="server" Text="Zoom Out in Map" Font-Size="Small" Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Seamless Zoom" ToolTip="Seamless Zoom" ID="Image4" runat="server"
                                    ImageUrl="~/Content/Admintools/Zoom.png" />
                                <asp:CheckBox ID="chkZoom" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnZoom)" />
                                <asp:Label ID="Label4" runat="server" Text="Seamless Zoom" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Pan/Move The Map" ToolTip="Pan/Move The Map" ID="Image5"
                                    runat="server" ImageUrl="~/Content/Admintools/Pan.png" />
                                <asp:CheckBox ID="chkPan" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnPan)" />
                                <asp:Label ID="Label5" runat="server" Text="Pan/Move The Map" Font-Size="Small" Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Get Detailed Information" ToolTip="Get Detailed Information"
                                    ID="Image6" runat="server" ImageUrl="~/Content/Admintools/Info.png" />
                                <asp:CheckBox ID="chkInFo" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnInfo)" />
                                <asp:Label ID="Label6" runat="server" Text="Get Detailed Information" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Measurement Tool" ToolTip="Measurement Tool" ID="Image26"
                                    runat="server" ImageUrl="~/Content/Admintools/blueruler.png" />
                                <asp:CheckBox ID="chkMeasure" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnMeasure)" />
                                <asp:Label ID="Label26" runat="server" Text="Measurement Tool" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Measurement Area Tool" ToolTip="Measurement Area Tool"
                                    ID="Image14" runat="server" ImageUrl="~/Content/Admintools/blueruler.png" />
                                <asp:CheckBox ID="chkMeasureArea" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnMeasureArea)" />
                                <asp:Label ID="Label27" runat="server" Text="Measurement Area Tool" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="OASIS Selector Tool" ToolTip="OASIS Selector Tool" ID="Image22"
                                    runat="server" ImageUrl="~/Content/Admintools/Selector.png" />
                                <asp:CheckBox ID="chkShowSelector" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnSelector)" />
                                <asp:Label ID="Label14" runat="server" Text="OASIS Selector Tool" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom Back To Previous Extent" ToolTip="Zoom Back To Previous Extent"
                                    ID="Image25" runat="server" ImageUrl="~/Content/Admintools/PreviousExtent.png" />
                                <asp:CheckBox ID="chkPreviousExtent" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnPreviousExtent)" />
                                <asp:Label ID="Label25" runat="server" Text="Zoom Back To Previous Extent" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom To Full Map" ToolTip="Zoom To Full Map" ID="Image7"
                                    runat="server" ImageUrl="~/Content/Admintools/FullExtent.png" />
                                <asp:CheckBox ID="chkFullExtent" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnFullExtent)" />
                                <asp:Label ID="Label7" runat="server" Text="Zoom To Full Map" Font-Size="Small" Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Zoom To Layer" ToolTip="Zoom To Layer" ID="Image8" runat="server"
                                    ImageUrl="~/Content/Admintools/LayerExtent.png" />
                                <asp:CheckBox ID="chkLayerExtent" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnLayerExtent)" />
                                <asp:Label ID="Label8" runat="server" Text="Zoom To Layer" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Add Map to Clipboard" ToolTip="Add Map to Clipboard" ID="Image12"
                                    runat="server" ImageUrl="~/Content/Admintools/AddClippBoard.png" />
                                <asp:CheckBox ID="chkAddClippBoard" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnAddClippBoard)" />
                                <asp:Label ID="Label12" runat="server" Text="Add Map to Clipboard" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Emergency" ToolTip="Emergency" ID="Image28" runat="server"
                                    ImageUrl="~/Content/Admintools/Emergency.png" />
                                <asp:CheckBox ID="chkEmergency" runat="server" ToolTip="AvailableMapToolsV4 SettingValue1 (csv value: btnEmergency)" />
                                <asp:Label ID="Label21" runat="server" Text="Emergency" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Canned Reports" ToolTip="Canned Reports" ID="Image23" runat="server"
                                    ImageUrl="~/Content/Admintools/CannedReports.png" />
                                <asp:CheckBox ID="chkCannedReports" runat="server" ToolTip="AvailableMapToolsV4 SettingValue3 (csv value: btnCannedReports)" />
                                <asp:Label ID="Label23" runat="server" Text="Canned Reports" Font-Size="Small" ToolTip="155px"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="OASIS Admin Locator" ToolTip="OASIS  Admin Locator" ID="Image20"
                                    runat="server" ImageUrl="~/Content/Admintools/AdminLocator.png" />
                                <asp:CheckBox ID="chkAdminLocator" runat="server" ToolTip="AvailableMapToolsV4 SettingValue3 (csv value: btnAdminLocator)" />
                                <asp:Label ID="Label20" runat="server" Text="OASIS Admin Locator" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Add Text Annotation" ToolTip="Add Text Annotation" ID="Image24"
                                    runat="server" ImageUrl="~/Content/Admintools/AddAnnotation.png" />
                                <asp:CheckBox ID="chkAddAnnotation" runat="server" ToolTip="AvailableMapToolsV4 SettingValue3 (csv value: btnAddAnnotation)" />
                                <asp:Label ID="Label24" runat="server" Text="Add Text Annotation" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Spatial Density Calculator" ToolTip="OASISv1 Charts" ID="Image19"
                                    runat="server" ImageUrl="~/Content/Admintools/RU8MMKQ.png" />
                                <asp:CheckBox ID="chkSpatialCalc" runat="server" ToolTip="AvailableMapToolsV4 SettingValue3 (csv value: btnSpatialCalc)" />
                                <asp:Label ID="Label19" runat="server" Text="Spatial Density Calculator" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Print" ToolTip="Print" ID="Image11" runat="server" ImageUrl="~/Content/Admintools/Print.png" />
                                <asp:CheckBox ID="chkPrint" runat="server" ToolTip="AvailableMapToolsV4 SettingValue4 (csv value: btnMapPrintTemplate)" />
                                <asp:Label ID="Label11" runat="server" Text="Print" Font-Size="Small" Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Remove layer" ToolTip="Remove layer" ID="Image10" runat="server"
                                    ImageUrl="~/Content/Admintools/RemoveLyr.png" />
                                <asp:CheckBox ID="chkRemoveLyr" runat="server" ToolTip="AvailableMapToolsV4 SettingValue5 (csv value: btnRemoveLyr)" />
                                <asp:Label ID="Label10" runat="server" Text="Remove layer" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Add Map Data from File" ToolTip="Add Map Data from File"
                                    ID="Image9" runat="server" ImageUrl="~/Content/Admintools/AdLyr.png" />
                                <asp:CheckBox ID="chkAddLyr" runat="server" ToolTip="AvailableMapToolsV4 SettingValue6 (csv value: btnAddLyr)" />
                                <asp:Label ID="Label9" runat="server" Text="Add Map Data from File" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Add Map Data from WMS/WFS" ToolTip="Add Map Data from WMS/WFS"
                                    ID="Image27" runat="server" ImageUrl="~/Content/Admintools/LoadWMSWFS.png" />
                                <asp:CheckBox ID="chkAddWMS" runat="server" ToolTip="AvailableMapToolsV4 SettingValue6 (csv value: btnAddWMS_WFS)" />
                                <asp:Label ID="Label22" runat="server" Text="Add Map Data from WMS/WFS" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Add Map Data from Database" ToolTip="Add Map Data from Database"
                                    ID="Image18" runat="server" ImageUrl="~/Content/Admintools/LoadSQLLyr.png" />
                                <asp:CheckBox ID="chkLoadSQLLyr" runat="server" ToolTip="AvailableMapToolsV4 SettingValue6 (csv value: btnLoadSQLLyr)" />
                                <asp:Label ID="Label18" runat="server" Text="Add Map Data from Database" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="New Project" ToolTip="New Project" ID="Image21" runat="server"
                                    ImageUrl="~/Content/Admintools/NewProject.png" />
                                <asp:CheckBox ID="chkNewProject" runat="server" ToolTip="AvailableMapToolsV4 SettingValue7 (csv value: btnNewProject)" />
                                <asp:Label ID="Label28" runat="server" Text="New Project" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Open Map Defination File" ToolTip="Open Map Defination File"
                                    ID="Image13" runat="server" ImageUrl="~/Content/Admintools/OpenMap.png" />
                                <asp:CheckBox ID="chkOpenMap" runat="server" ToolTip="AvailableMapToolsV4 SettingValue7(csv value: btnOpenMap)" />
                                <asp:Label ID="Label13" runat="server" Text="Open Map Defination File" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Create Map Defination File" ToolTip="Create Map Defination File"
                                    ID="Image16" runat="server" ImageUrl="~/Content/Admintools/ExportMapDefFile.png" />
                                <asp:CheckBox ID="chkExportMapDefFile" runat="server" ToolTip="AvailableMapToolsV4 SettingValue7 (csv value: btnExportMapDefFile)" />
                                <asp:Label ID="Label16" runat="server" Text="Create Map Defination File" Font-Size="Small"
                                    Width="155px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Image AlternateText="Export to  Database" ToolTip="Export to  Database" ID="Image15"
                                    runat="server" ImageUrl="~/Content/Admintools/CreateDBLyr.png" />
                                <asp:CheckBox ID="chkCreateDBLyr" runat="server" ToolTip="AvailableMapToolsV4 SettingValue8 (csv value: btnCreateDBLyr)" />
                                <asp:Label ID="Label15" runat="server" Text="Export to  Database" Font-Size="Small"
                                    Width="155px" />
                            </td>
                            <td class="style1">
                                <asp:Image AlternateText="Export to File" ToolTip="Export to File" ID="Image17" runat="server"
                                    ImageUrl="~/Content/Admintools/ExportToShape.png" />
                                <asp:CheckBox ID="chkExportToShape" runat="server" ToolTip="AvailableMapToolsV4 SettingValue8 (csv value: btnExportToShape)" />
                                <asp:Label ID="Label17" runat="server" Text="Export to File" Font-Size="Small" Width="155px" />
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:FieldSet>
            <ext:TextField ID="txtInitialMap" runat="server" Width="500" FieldLabel="Initial Map">
                <ToolTips>
                    <ext:ToolTip ID="ToolTip1" runat="server" Html="<p><b>InitMap</b></p><p>SettingValue1</p>" />
                </ToolTips>
            </ext:TextField>
            <ext:ComboBox ID="cboInitialMenu" runat="server" DisplayField="display" ValueField="abbr"
                EmptyText="Select a intital menu..." FieldLabel="Intital Menu" Width="300">
                <Store>
                    <ext:Store ID="Store2" runat="server">
                        <Reader>
                            <ext:ArrayReader>
                                <Fields>
                                    <ext:RecordField Name="abbr" />
                                    <ext:RecordField Name="value" />
                                    <ext:RecordField Name="display" />
                                </Fields>
                            </ext:ArrayReader>
                        </Reader>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:Checkbox ID="chkPromptSave" runat="server" FieldLabel="Save map changes on exit?"
                LabelWidth="180" Visible="false">
                <ToolTips>
                    <ext:ToolTip ID="ToolTip18" runat="server" Html="<p><b>InitialOperationsTabNumber</b></p><p>SettingValue2  (boolean value: 0,1)</p>" />
                </ToolTips>
            </ext:Checkbox>
            <ext:FieldSet ID="FieldSet1" runat="server" Title="Data Grid Settings" Layout="RowLayout">
                <Items>
                    <ext:CompositeField ID="CompositeField8" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:Label ID="Label1" runat="server" Text="" />
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField1" runat="server" Text="Use Max Records" />
                            <ext:Checkbox ID="chkUseMaxRecords" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip5" runat="server" Html="<p><b>UserDefGridRecords</b></p><p>SettingValue1  (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                            <ext:DisplayField ID="DisplayField2" runat="server" Text="Max Level" />
                            <ext:TextField ID="txtMaxLevel" runat="server" Width="100">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip3" runat="server" Html="<p><b>UserDefGridRecords</b></p><p>SettingValue2</p>" />
                                </ToolTips>
                            </ext:TextField>
                            <ext:DisplayField ID="DisplayField3" runat="server" Text="Warning Level" />
                            <ext:TextField ID="txtWarningLevel" runat="server" Width="100">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip4" runat="server" Html="<p><b>UserDefGridRecords</b></p><p>SettingValue3</p>" />
                                </ToolTips>
                            </ext:TextField>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField9" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:Label ID="DisplayField20" runat="server" Text="" />
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField2" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField4" runat="server" Text="Map Filter" />
                            <ext:Checkbox ID="chkMapFilter" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip6" runat="server" Html="<p><b>UserDefGridRecords</b></p><p>SettingValue4 (MapFilter (0,1),MapSelect (0,1))</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                            <ext:DisplayField ID="DisplayField5" runat="server" Text="Map Select" />
                            <ext:Checkbox ID="chkMapSelect" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip7" runat="server" Html="<p><b>UserDefGridRecords</b></p><p>SettingValue4 (MapFilter (0,1),MapSelect (0,1))</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:FieldSet>
            <ext:FieldSet ID="fsZoomBar" runat="server" Border="true" Header="true" Layout="FormLayout"
                LabelAlign="Top" Height="280" Title="Zoom Bar">
                <Items>
                    <ext:CompositeField ID="CompositeField7" runat="server" MsgTarget="Under" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField17" runat="server" Text="Show Zoombar" />
                            <ext:Checkbox ID="chkShowZoombar" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip36" runat="server" Html="<p><b>ShowZoomBar</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                            <ext:DisplayField ID="DisplayField98" runat="server" Text="Show Scalebar" />
                            <ext:Checkbox ID="chkShowScalebar" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip32" runat="server" Html="<p><b>ShowScalebar</b></p><p>SettingValue1  (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField10" runat="server" MsgTarget="Under" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField18" runat="server" Text="X-MIN" />
                            <ext:TextField ID="txtXMin" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip19" runat="server" Html="<p><b>ShowZoomBar</b></p><p>SettingValue5  (Double value: 141.1454)</p>" />
                                </ToolTips>
                            </ext:TextField>
                            <ext:DisplayField ID="DisplayField21" runat="server" Text="X-MAX" />
                            <ext:TextField ID="txtXMax" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip21" runat="server" Html="<p><b>ShowZoomBar</b></p><p>SettingValue6  (Double value: 141.1454)</p>" />
                                </ToolTips>
                            </ext:TextField>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField11" runat="server" MsgTarget="Under" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField19" runat="server" Text="Y-MIN" />
                            <ext:TextField ID="txtYMIN" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip20" runat="server" Html="<p><b>ShowZoomBar</b></p><p>SettingValue7  (Double value: 141.1454)</p>" />
                                </ToolTips>
                            </ext:TextField>
                            <ext:DisplayField ID="DisplayField22" runat="server" Text="Y-MAX" />
                            <ext:TextField ID="txtYMAX" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip22" runat="server" Html="<p><b>ShowZoomBar</b></p><p>SettingValue8  (Double value: 141.1454)</p>" />
                                </ToolTips>
                            </ext:TextField>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <Content>
                    <ajaxToolkit:ColorPickerExtender ID="CPEtxtZoomSelectorColour" runat="server" OnClientColorSelectionChanged="colorChanged"
                        TargetControlID="txtZoomSelectorColour" />
                    <ajaxToolkit:ColorPickerExtender ID="CPEtxtZoomRageMakers" runat="server" OnClientColorSelectionChanged="colorChanged"
                        TargetControlID="txtZoomRageMakers" />
                    <ajaxToolkit:ColorPickerExtender ID="CPEtxtBackPanel" runat="server" OnClientColorSelectionChanged="colorChanged"
                        TargetControlID="txtBackPanel" />
                    <script type="text/javascript">
                        function colorChanged(sender) {
                            sender.get_element().style.color = "#" + sender.get_selectedColor();
                        }
                    </script>
                    <table border="0" cellpadding="5">
                        <tr>
                            <td style="width: 25%;">
                                <p>
                                    <span class="x-label-text">Zoom selector Colour</span></p>
                            </td>
                            <td style="width: 75%;">
                                <asp:TextBox runat="server" ID="txtZoomSelectorColour" AutoCompleteType="None" MaxLength="6"
                                    Style="float: left" ToolTip="ShowZoomBar SettingValue2 (long integer)" />
                                <asp:ImageButton runat="Server" ID="btnImage3" Style="float: left; margin: 0 3px"
                                    ImageUrl="~/Content/images/cp_button.png" AlternateText="Click to show color picker" />
                                <asp:Panel ID="Sample3" Style="width: 18px; height: 18px; border: 1px solid #000;
                                    margin: 0 3px; float: left" runat="server" />
                                <ajaxToolkit:ColorPickerExtender ID="btnCPE3" runat="server" TargetControlID="txtZoomSelectorColour"
                                    PopupButtonID="btnImage3" SampleControlID="Sample3" SelectedColor="FF3300" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <p>
                                    <span class="x-label-text">Zoom rage makers Colour</span></p>
                            </td>
                            <td style="width: 70%;">
                                <asp:TextBox runat="server" ID="txtZoomRageMakers" AutoCompleteType="None" MaxLength="6"
                                    Style="float: left" ToolTip="ShowZoomBar SettingValue3 (long integer)" />
                                <asp:ImageButton runat="Server" ID="btnImage4" Style="float: left; margin: 0 3px"
                                    ImageUrl="~/Content/images/cp_button.png" AlternateText="Click to show color picker" />
                                <asp:Panel ID="Sample4" Style="width: 18px; height: 18px; border: 1px solid #000;
                                    margin: 0 3px; float: left" runat="server" />
                                <ajaxToolkit:ColorPickerExtender ID="btnCPE4" runat="server" TargetControlID="txtZoomRageMakers"
                                    PopupButtonID="btnImage4" SampleControlID="Sample4" SelectedColor="FF2300" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <p>
                                    <span class="x-label-text">Zoom back panel Colour</span></p>
                            </td>
                            <td style="width: 75%;">
                                <asp:TextBox runat="server" ID="txtBackPanel" AutoCompleteType="None" MaxLength="6"
                                    Style="float: left" ToolTip="ShowZoomBar SettingValue4 (long integer)" />
                                <asp:ImageButton runat="Server" ID="btnImage5" Style="float: left; margin: 0 3px"
                                    ImageUrl="~/Content/images/cp_button.png" AlternateText="Click to show color picker" />
                                <asp:Panel ID="Sample5" Style="width: 18px; height: 18px; border: 1px solid #000;
                                    margin: 0 3px; float: left" runat="server" />
                                <ajaxToolkit:ColorPickerExtender ID="btnCPE5" runat="server" TargetControlID="txtBackPanel"
                                    PopupButtonID="btnImage5" SampleControlID="Sample5" SelectedColor="F03300" />
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet2" runat="server" Title="Setting" Layout="RowLayout">
                <Items>
                    <ext:CompositeField ID="CompositeField3" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField6" runat="server" Text="Legend" />
                            <ext:Checkbox ID="chkLegend" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip8" runat="server" Html="<p><b>ShowlegendTab</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                            <ext:DisplayField ID="DisplayField7" runat="server" Text="Map Library" />
                            <ext:Checkbox ID="chkMapLibrary" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip9" runat="server" Html="<p><b>ShowMapLibraryTab</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                            <ext:DisplayField ID="DisplayField8" runat="server" Text="Security" />
                            <ext:Checkbox ID="chkSecurity" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip10" runat="server" Html="<p><b>ShowSecurityTab</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet3" runat="server" Title="Utility " Layout="RowLayout" Height="100">
                <Items>
                    <ext:CompositeField ID="CompositeField4" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField14" runat="server" Text="Show Utility Toolbar" />
                            <ext:Checkbox ID="chkSHowUtilityToolbar" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip11" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue1  (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                    <ext:FieldSet ID="FieldSet4" runat="server" Title="Availale Utility Toolbar " Layout="RowLayout">
                        <Items>
                            <ext:CompositeField ID="CompositeField5" runat="server" AnchorHorizontal="100%">
                                <Items>
                                    <ext:DisplayField ID="DisplayField9" runat="server" Text="Geo Marks" />
                                    <ext:Checkbox ID="chkGeoMarks" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip12" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue3  (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:DisplayField ID="DisplayField10" runat="server" Text="Coordinate tools" />
                                    <ext:Checkbox ID="chkCoordinateTools" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip13" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue4  (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:DisplayField ID="DisplayField11" runat="server" Text="Goto location" />
                                    <ext:Checkbox ID="chkGotoLocation" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip14" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue5  (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:DisplayField ID="DisplayField12" runat="server" Text="Settings" />
                                    <ext:Checkbox ID="chkSettings" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip15" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue6  (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:DisplayField ID="DisplayField13" runat="server" Text="Magnifier" />
                                    <ext:Checkbox ID="chkMagnifier" runat="server">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip16" runat="server" Html="<p><b>ShowExpandedToolBoxInGISWin</b></p><p>SettingValue7  (boolean value: 0,1)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                </Items>
                            </ext:CompositeField>
                        </Items>
                    </ext:FieldSet>
                </Items>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet5" runat="server" Title="Legend " Layout="RowLayout" Height="100">
                <Items>
                    <ext:DisplayField ID="DisplayField15" runat="server" Text="Hidden Layers" />
                    <ext:TextField ID="txtHiddenLayer" runat="server" Width="650">
                        <ToolTips>
                            <ext:ToolTip ID="ToolTip2" runat="server" Html="<p><b>HiddenLayers</b></p><p>SettingValue1</p>" />
                        </ToolTips>
                    </ext:TextField>
                    <ext:CompositeField ID="CompositeField6" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField16" runat="server" Text="Use Predefined Themes" />
                            <ext:Checkbox ID="chkUsePredefinedThemes" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip17" runat="server" Html="<p><b>ThemeTool</b></p><p>SettingValue1  (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:FieldSet>
        </Content>
    </ext:Panel>
</asp:Content>
