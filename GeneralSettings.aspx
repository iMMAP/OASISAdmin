<%@ Page Title="Oasis Admin Tools --- General" Language="VB" MasterPageFile="~/site.master"
    AutoEventWireup="false" CodeFile="GeneralSettings.aspx.vb" Inherits="GeneralSettings" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .tableGroup
        {
            height: 93px;
            width: 560px;
        }
        .tdGroup
        {
            width: 242px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Gray" />
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" />
    <table>
        <tr>
        <td>User Groups&nbsp;</td>
            
    
        <td>
            <ajaxToolkit:ComboBox ID="cboUserGroups" runat="server" AutoPostBack="True" DropDownStyle="DropDownList"
                AutoCompleteMode="None" CaseSensitive="False" CssClass="WindowsStyle" ItemInsertLocation="Append">
            </ajaxToolkit:ComboBox>
        </td>
        <td>
            <ext:Button ID="btnSave2" runat="server" Text="Save" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="btnSave2_DirectClick">
                        <EventMask ShowMask="true" />
                        <ExtraParams>
                            <ext:Parameter Name="overLayValues" Value="#{gpOverlayLayer}.getRowsValues()" Mode="Raw"
                                Encode="true" />
                            <ext:Parameter Name="featureValues" Value="#{gpFeatureLayer}.getRowsValues()" Mode="Raw"
                                Encode="true" />
                        </ExtraParams>
                        <EventMask ShowMask="true" Msg="Processing..." />
                    </Click>
                </DirectEvents>
            </ext:Button>
        </td>
    </table>
   
    <br />
    <ext:FormPanel ID="FrmSecurity" runat="server" Title="General Settings" MonitorPoll="500"
        MonitorValid="true" Padding="5" AutoWidth="True" Height="900" ButtonAlign="Right"
        Layout="FormLayout">
        <Items>
            <ext:FieldSet ID="fsGeneral" runat="server" Border="true" Header="true" Layout="ColumnLayout"
                LabelAlign="Top" Height="200" Title="General">
                <Items>
                    <ext:CompositeField ID="CompositeField0" runat="server">
                        <Items>
                            <ext:Panel ID="panelActiveCoreFunction" runat="server" AutoHeight="true" Padding="5"
                                Title="Active Core Functuns" Width="330">
                                <Items>
                                    <ext:Checkbox IDMode="Legacy" ID="chkOasisProfile" runat="server" FieldLabel="Oasis Profile">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip38" runat="server" Html="<p><b>VisibleMainModuleMenus</b></p><p>SettingValue1 (csv value: cbProfile)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="chkOperations" runat="server" FieldLabel="Operations">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip5" runat="server" Html="<p><b>VisibleMainModuleMenus</b></p><p>SettingValue1 (csv value: cbOperations)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="chkRssFeeds" runat="server" FieldLabel="RSS Feeds">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip1" runat="server" Html="<p><b>VisibleMainModuleMenus</b></p><p>SettingValue1 (csv value: cbContent)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="chkDynamicData" runat="server" FieldLabel="Dynamic Data">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip2" runat="server" Html="<p><b>VisibleMainModuleMenus</b></p><p>SettingValue1 (csv value: cbDynamicData)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="chkDynamicReports" runat="server" FieldLabel="DynamicReports">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip3" runat="server" Html="<p><b>VisibleMainModuleMenus</b></p><p>SettingValue1 (csv value: cbReports)</p>" />
                                        </ToolTips>
                                    </ext:Checkbox>
                                    <ext:ComboBox ID="cboStartModule" runat="server" DisplayField="display" ValueField="abbr"
                                        EmptyText="Select a start module..." FieldLabel="Start Module" Width="300">
                                        <Store>
                                            <ext:Store ID="vvd" runat="server">
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
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:FieldSet>
            <ext:FieldSet ID="fsSynchronisation" runat="server" Border="true" Header="true" Layout="FormLayout"
                LabelAlign="Top" Height="220" Title="Synchronisation">
                <Items>
                    <ext:CompositeField ID="CompositeField1" runat="server" MsgTarget="Under" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField1" runat="server" Text="Enable Internet Sync" />
                            <ext:Checkbox ID="chkEnableInternetSync" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip34" runat="server" Html="<p><b>chkEnableInternetSync</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField2" runat="server" MsgTarget="Under" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField2" runat="server" Text="Interval (Seconds)" />
                            <ext:TextField ID="txtInterval" runat="server" Width="100">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip33" runat="server" Html="<p><b>InetConnectionSettings</b></p><p>SettingValue2</p>" />
                                </ToolTips>
                            </ext:TextField>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <Content>
                    <ajaxToolkit:ColorPickerExtender ID="CPEtxtColorBack" runat="server" OnClientColorSelectionChanged="colorBackChanged"
                        TargetControlID="txtColorBack" />
                    <ajaxToolkit:ColorPickerExtender ID="CPEtxtColorFore" runat="server" OnClientColorSelectionChanged="colorForeChanged"
                        TargetControlID="txtColorFore" />
                    <br />
                    <script type="text/javascript">
                        function colorBackChanged(sender) {
                            sender.get_element().style.color = "#" + sender.get_selectedColor();
                        }
                        function colorForeChanged(sender) {
                            sender.get_element().style.color = "#" + sender.get_selectedColor();
                        }
                        function hexToDeci(num) {
                            res4 = 999;
                            args = num;
                            //hexAr = new Array('1','2','3','4','5','6','7','8','9','a','b','c','d','e','f');

                            k = args.length - 1;
                            for (var i = 0; i < args.length; i++) {
                                thisnum = args.substring(i, i + 1);
                                var resd = Math.pow(16, k);
                                if (thisnum == 'A')
                                    thisnum = 10;
                                else if (thisnum == 'B')
                                    thisnum = 11;
                                else if (thisnum == 'C')
                                    thisnum = 12;
                                else if (thisnum == 'D')
                                    thisnum = 13;
                                else if (thisnum == 'E')
                                    thisnum = 14;
                                else if (thisnum == 'F')
                                    thisnum = 15;
                                resd = resd * thisnum;
                                k = k - 1;
                                if (res4 == 999) {
                                    res4 = resd.toString();
                                }
                                else {
                                    res4 = parseInt(res4) + parseInt(resd);
                                }
                            }
                            return res4;
                        }
                    </script>
                    <table border="0" cellpadding="5">
                        <tr>
                            <td style="width: 25%;">
                                <p>
                                    <span class="x-label-text">Notifier Back Colour</span></p>
                            </td>
                            <td style="width: 75%;">
                                <asp:TextBox runat="server" ID="txtColorBack" AutoCompleteType="None" MaxLength="6"
                                    Style="float: left" ToolTip="Notifier SettingValue1 (Hex number)" />
                                <asp:ImageButton runat="Server" ID="btnImage1" Style="float: left; margin: 0 3px"
                                    ImageUrl="~/Content/images/cp_button.png" AlternateText="Click to show color picker" />
                                <asp:Panel ID="Sample1" Style="width: 18px; height: 18px; border: 1px solid #000;
                                    margin: 0 3px; float: left" runat="server" />
                                <ajaxToolkit:ColorPickerExtender ID="buttonCPE1" runat="server" TargetControlID="txtColorBack"
                                    PopupButtonID="btnImage1" SampleControlID="Sample1" SelectedColor="FF3300" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <p>
                                    <span class="x-label-text">Notifier Fore Colour</span></p>
                            </td>
                            <td style="width: 75%;">
                                <asp:TextBox runat="server" ID="txtColorFore" AutoCompleteType="None" MaxLength="6"
                                    Style="float: left" ToolTip="Notifier SettingValue2 (Hex number)" />
                                <asp:ImageButton runat="Server" ID="btnImage2" Style="float: left; margin: 0 3px"
                                    ImageUrl="~/Content/images/cp_button.png" AlternateText="Click to show color picker" />
                                <asp:Panel ID="Sample2" Style="width: 18px; height: 18px; border: 1px solid #000;
                                    margin: 0 3px; float: left" runat="server" />
                                <ajaxToolkit:ColorPickerExtender ID="buttonCPE2" runat="server" TargetControlID="txtColorFore"
                                    PopupButtonID="btnImage2" SampleControlID="Sample2" SelectedColor="FFFF66" />
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:FieldSet>
            <ext:FieldSet ID="fsAdminLocations" runat="server" Title="Administrative Locations">
                <Items>
                    <ext:CompositeField ID="CompositeField6" runat="server">
                        <Items>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <Content>
                    <table border="0">
                        <tr>
                            <td style="width: 12.5%;">
                                <p>
                                    <span class="x-label-text">Level 1</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtAdminLevel1" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip31" runat="server" Html="<p><b>AdminLevel0</b></p><p>SettingValue1</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 16.5%;">
                                <p>
                                    <span class="x-label-text">Fld Name 1</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtFldName1" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip30" runat="server" Html="<p><b>AdminLevel0</b></p><p>SettingValue2</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 17.5%;">
                                <p>
                                    <span class="x-label-text">Adm Code 1</span></p>
                            </td>
                            <td style="width: 10.5%;">
                                <ext:TextField ID="txtAdmCode1" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip29" runat="server" Html="<p><b>AdminLevel0</b></p><p>SettingValue3</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 14.5%;">
                                <p>
                                    <span class="x-label-text">Alias 1</span></p>
                            </td>
                            <td style="width: 8.5%;">
                                <ext:TextField ID="txtAlias1" runat="server" Width="90">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip28" runat="server" Html="<p><b>AdminLevel0</b></p><p>SettingValue4</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <p>
                                    <span class="x-label-text">Level 2</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtAdminLevel2" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip27" runat="server" Html="<p><b>AdminLevel1</b></p><p>SettingValue1</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 16.5%;">
                                <p>
                                    <span class="x-label-text">Fld Name 2</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtFldName2" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip26" runat="server" Html="<p><b>AdminLevel1</b></p><p>SettingValue2</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 17.5%;">
                                <p>
                                    <span class="x-label-text">Adm Code 2</span></p>
                            </td>
                            <td style="width: 10.5%;">
                                <ext:TextField ID="txtAdmCode2" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip23" runat="server" Html="<p><b>AdminLevel1</b></p><p>SettingValue3</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 14.5%;">
                                <p>
                                    <span class="x-label-text">Alias 2</span></p>
                            </td>
                            <td style="width: 8.5%;">
                                <ext:TextField ID="txtAlias2" runat="server" Width="90">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip4" runat="server" Html="<p><b>AdminLevel1</b></p><p>SettingValue4</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <p>
                                    <span class="x-label-text">Level 3</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtAdminLevel3" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip22" runat="server" Html="<p><b>AdminLevel1</b></p><p>SettingValue1</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 16.5%;">
                                <p>
                                    <span class="x-label-text">Fld Name 3</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtFldName3" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip21" runat="server" Html="<p><b>AdminLevel2</b></p><p>SettingValue2</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 17.5%;">
                                <p>
                                    <span class="x-label-text">Adm Code 3</span></p>
                            </td>
                            <td style="width: 10.5%;">
                                <ext:TextField ID="txtAdmCode3" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip20" runat="server" Html="<p><b>AdminLevel2</b></p><p>SettingValue3</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 14.5%;">
                                <p>
                                    <span class="x-label-text">Alias 3</span></p>
                            </td>
                            <td style="width: 8.5%;">
                                <ext:TextField ID="txtAlias3" runat="server" Width="90">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip19" runat="server" Html="<p><b>AdminLevel2</b></p><p>SettingValue4</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <p>
                                    <span class="x-label-text">Level 4</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtAdminLevel4" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip18" runat="server" Html="<p><b>AdminLevel3</b></p><p>SettingValue1</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 16.5%;">
                                <p>
                                    <span class="x-label-text">Fld Name 4</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtFldName4" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip17" runat="server" Html="<p><b>AdminLevel3</b></p><p>SettingValue2</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 17.5%;">
                                <p>
                                    <span class="x-label-text">Adm Code 4</span></p>
                            </td>
                            <td style="width: 10.5%;">
                                <ext:TextField ID="txtAdmCode4" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip16" runat="server" Html="<p><b>AdminLevel3</b></p><p>SettingValue3</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 14.5%;">
                                <p>
                                    <span class="x-label-text">Alias 4</span></p>
                            </td>
                            <td style="width: 8.5%;">
                                <ext:TextField ID="txtAlias4" runat="server" Width="90">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip15" runat="server" Html="<p><b>AdminLevel3</b></p><p>SettingValue4</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <p>
                                    <span class="x-label-text">Location</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtAdminLevel5" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip14" runat="server" Html="<p><b>AdminLocation</b></p><p>SettingValue1</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 16.5%;">
                                <p>
                                    <span class="x-label-text">Fld Name 5</span></p>
                            </td>
                            <td style="width: 14.5%;">
                                <ext:TextField ID="txtFldName5" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip13" runat="server" Html="<p><b>AdminLocation</b></p><p>SettingValue2</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 17.5%;">
                                <p>
                                    <span class="x-label-text">Adm Code 5</span></p>
                            </td>
                            <td style="width: 10.5%;">
                                <ext:TextField ID="txtAdmCode5" runat="server" Width="80">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip12" runat="server" Html="<p><b>AdminLocation</b></p><p>SettingValue3</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                            <td style="width: 14.5%;">
                                <p>
                                    <span class="x-label-text">Alias 5</span></p>
                            </td>
                            <td style="width: 8.5%;">
                                <ext:TextField ID="txtAlias5" runat="server" Width="90">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip11" runat="server" Html="<p><b>AdminLocation</b></p><p>SettingValue4</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:FieldSet>
            <ext:FieldSet ID="fsHotKeys" runat="server" Title="Security Grid Settings">
                <Items>
                    <ext:CompositeField ID="CompositeField10" runat="server">
                        <Items>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <Content>
                    <table border="0" cellpadding="5" width="100%">
                        <tr>
                            <td style="width: 50%;">
                                <p>
                                    <span class="x-label-text">Scipt Name Hot Key 2 (CTRL + ALT + 1):</span></p>
                            </td>
                            <td style="width: 50%;">
                                <ext:TextField ID="txtHotKey1" runat="server" Width="350">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip10" runat="server" Html="<p><b>Scripts</b></p><p>SettingValue1 (csv value)</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%;">
                                <p>
                                    <span class="x-label-text">Scipt Name Hot Key 2 (CTRL + ALT + 2):</span></p>
                            </td>
                            <td style="width: 50%;">
                                <ext:TextField ID="txtHotKey2" runat="server" Width="350">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip9" runat="server" Html="<p><b>Scripts</b></p><p>SettingValue1 (csv value)</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%;">
                                <p>
                                    <span class="x-label-text">Scipt Name Hot Key 2 (CTRL + ALT + 3):</span></p>
                            </td>
                            <td style="width: 50%;">
                                <ext:TextField ID="txtHotKey3" runat="server" Width="350">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip8" runat="server" Html="<p><b>Scripts</b></p><p>SettingValue1 (csv value)</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%;">
                                <p>
                                    <span class="x-label-text">Scipt Name Hot Key 2 (CTRL + ALT + 4):</span></p>
                            </td>
                            <td style="width: 50%;">
                                <ext:TextField ID="txtHotKey4" runat="server" Width="350">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip24" runat="server" Html="<p><b>Scripts</b></p><p>SettingValue1 (csv value)</p>" />
                                    </ToolTips>
                                </ext:TextField>
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:FieldSet>
            <ext:FieldSet ID="fsMisc" runat="server" Border="true" Header="true" Layout="RowLayout"
                LabelAlign="Top" Height="80" Title="Misc">
                <Items>
                    <ext:CompositeField ID="CompositeField14" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField29" runat="server" Text="Mine Action Module" />
                            <ext:Checkbox ID="chkMineActionModule" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip7" runat="server" Html="<p><b>ShowMATab</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField3" runat="server" AnchorHorizontal="100%">
                        <Items>
                            <ext:DisplayField ID="DisplayField3" runat="server" Text="Advance Debug" />
                            <ext:Checkbox ID="chkAdvanceDebug" runat="server">
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip6" runat="server" Html="<p><b>ShowAdvancedDebug</b></p><p>SettingValue1 (boolean value: 0,1)</p>" />
                                </ToolTips>
                            </ext:Checkbox>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:FieldSet>
        </Items>
    </ext:FormPanel>
    <ext:GridPanel ID="gpOverlayLayer" runat="server" Title="Spatial Density Calculator Overlay Layer"
        Margins="0 0 5 5" Frame="true" Height="320">
        <Store>
            <ext:Store ID="OverlayLayerStore" runat="server" AutoSave="true" RefreshAfterSaving="None"
                OnRefreshData="OverlayLayerStore_Refresh">
                <Reader>
                    <ext:JsonReader>
                        <Fields>
                            <ext:RecordField Name="LayerName" Type="String" />
                            <ext:RecordField Name="ZoomValue" Type="String" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                    <ext:Button ID="btnNewOverlayLayer" runat="server" Text="New" Icon="Add">
                        <Listeners>
                            <Click Handler="#{gpOverlayLayer}.insertRecord(0,{LayerName:' ',ZoomValue:' '});" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnDeleteOverlayLayer" runat="server" Text="Delete" Icon="Delete">
                        <Listeners>
                            <Click Handler="#{gpOverlayLayer}.deleteSelected();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column DataIndex="LayerName" Header="Layer Name" Width="300">
                    <Editor>
                        <ext:TextField ID="txtOverlayLayerName" runat="server" />
                    </Editor>
                </ext:Column>
                <ext:Column DataIndex="ZoomValue" Header="Zoom value" Width="300">
                    <Editor>
                        <ext:TextField ID="txtOverlayZoomValue" runat="server" />
                    </Editor>
                </ext:Column>
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
    <ext:GridPanel ID="gpFeatureLayer" runat="server" Title="Spatial Density Calculator Feature Layer"
        Margins="0 0 5 5" Frame="true" Height="320">
        <Store>
            <ext:Store ID="FeatureLayerStore" runat="server" AutoSave="true" RefreshAfterSaving="None"
                OnRefreshData="FeatureLayerStore_Refresh">
                <Reader>
                    <ext:JsonReader>
                        <Fields>
                            <ext:RecordField Name="LayerName" Type="String" DefaultValue="" />
                        </Fields>
                    </ext:JsonReader>
                </Reader>
            </ext:Store>
        </Store>
        <TopBar>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:Button ID="btnNewFeatureLayer" runat="server" Text="New" Icon="Add">
                        <Listeners>
                            <Click Handler="#{gpFeatureLayer}.insertRecord(0,{LayerName:' '});" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnDeleteFeatureLayer" runat="server" Text="Delete" Icon="Delete">
                        <Listeners>
                            <Click Handler="#{gpFeatureLayer}.deleteSelected();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel ID="ColumnModel2" runat="server">
            <Columns>
                <ext:Column DataIndex="LayerName" Header="Layer Name" Width="300">
                    <Editor>
                        <ext:TextField ID="txtFeatureLayerName" runat="server" />
                    </Editor>
                </ext:Column>
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
            </ext:RowSelectionModel>
        </SelectionModel>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="10" />
        </BottomBar>
        <LoadMask ShowMask="true" />
    </ext:GridPanel>
</asp:Content>
