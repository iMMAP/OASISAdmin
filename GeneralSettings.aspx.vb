Imports Ext.Net
Imports System.Data.SqlClient
Imports System.Data
Imports Immap.Service
Imports Immap.Model
Partial Class GeneralSettings
    Inherits System.Web.UI.Page

    Private sqlServer As String = System.Configuration.ConfigurationManager.AppSettings("Server,Port").ToString()
    Private sqlUserId As String = System.Configuration.ConfigurationManager.AppSettings("UserId").ToString()
    Private sqlPassword As String = System.Configuration.ConfigurationManager.AppSettings("Password").ToString()
    Private sqlTrustedConnection As String = System.Configuration.ConfigurationManager.AppSettings("TrustedConnection").ToString()

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        'btnSave.Attributes.Add("onclick", "if(confirm('Are you sure you want to perform this action?')== false) return false;")
        InitStartModule()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "General Settings"
            LoadUserGroups()
            If Not (Session("cboUserGroupsIndex") Is Nothing) Then
                cboUserGroups.SelectedIndex = Convert.ToInt32(Session("cboUserGroupsIndex"))
                cboUserGroups.DataBind()
            End If
            LoadAllSetting()
        End If
    End Sub

    Protected Sub OverlayLayerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)

    End Sub
    Protected Sub FeatureLayerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)

    End Sub
    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) 'Handles btnSave.Click
        '<asp:Button ID="btnSave" runat="server" Text="Save" Height="30px" Width="58px" />
        Dim msg As New MessageBox()
        Dim overLayValues As String = ""
        'overLayValues = e.ExtraParams("overLayValues")
        Dim featureValues As String = ""
        'featureValues = e.ExtraParams("featureValues")
        If (chkOasisProfile.Checked = False) AndAlso (chkOperations.Checked = False) AndAlso (chkRssFeeds.Checked = False) AndAlso
           (chkRssFeeds.Checked = False) AndAlso (chkDynamicReports.Checked = False) Then
            chkOperations.Checked = True
            cboStartModule.SetValue("cbOperations")
            cboStartModule.SelectedIndex = 1
        End If
        If (cboStartModule.SelectedIndex = 0) AndAlso (chkOasisProfile.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "Please select enable Oasis Profile to set start module with Oasis Profile",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If
        If (cboStartModule.SelectedIndex = 1) AndAlso (chkOperations.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable Operations to set start module with Operations",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If
        If (cboStartModule.SelectedIndex = 2) AndAlso (chkRssFeeds.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable RSS Feed to set start module with Dynamic content",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If

        If (cboStartModule.SelectedIndex = 3) AndAlso (chkDynamicData.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable Dynamic Data to set start module with Data Entry",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If

        If (cboStartModule.SelectedIndex = 4) AndAlso (chkDynamicReports.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "Please select enable Dynamic Reports to set start module with Reporting",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If

        'SaveAllSettings(overLayValues, featureValues)
        'cboUserGroups_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub btnSave2_DirectClick(sender As Object, e As Ext.Net.DirectEventArgs)
        Dim msg As New MessageBox()
        Dim overLayValues As String = ""
        overLayValues = e.ExtraParams("overLayValues")
        Dim featureValues As String = ""
        featureValues = e.ExtraParams("featureValues")
        If (chkOasisProfile.Checked = False) AndAlso (chkOperations.Checked = False) AndAlso (chkRssFeeds.Checked = False) AndAlso
           (chkRssFeeds.Checked = False) AndAlso (chkDynamicReports.Checked = False) Then
            chkOperations.Checked = True
            cboStartModule.SetValue("cbOperations")
            cboStartModule.SelectedIndex = 1
        End If
        If (cboStartModule.SelectedIndex = 0) AndAlso (chkOasisProfile.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "Please select enable Oasis Profile to set start module with Oasis Profile",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If
        If (cboStartModule.SelectedIndex = 1) AndAlso (chkOperations.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable Operations to set start module with Operations",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If
        If (cboStartModule.SelectedIndex = 2) AndAlso (chkRssFeeds.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable RSS Feed to set start module with Dynamic content",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If

        If (cboStartModule.SelectedIndex = 3) AndAlso (chkDynamicData.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "You must select enable Dynamic Data to set start module with Data Entry",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If

        If (cboStartModule.SelectedIndex = 4) AndAlso (chkDynamicReports.Checked = False) Then
            msg.Show(New MessageBoxConfig() With {
                           .Title = "Warning",
                           .Message = "Please select enable Dynamic Reports to set start module with Reporting",
                           .Buttons = MessageBox.Button.OK,
                           .Icon = MessageBox.Icon.WARNING,
                          .AnimEl = Me.FrmSecurity.ClientID
                         })
            Exit Sub
        End If
        SaveAllSettings(overLayValues, featureValues)
        cboUserGroups_SelectedIndexChanged(sender, e)
    End Sub

    Protected Function ReadOverlayFormat(ByVal xOverlay As String) As List(Of SpatialCalOverlayModels)
        Dim StrData() As String = Split(xOverlay, ";")
        Dim Overlay() As String = Nothing
        Dim SpatialCalOverlayModels As New List(Of SpatialCalOverlayModels)
        For j = LBound(StrData) To UBound(StrData)
            If String.IsNullOrEmpty(StrData(j)) = False Then
                Overlay = Split(StrData(j), ",")
                Dim scom As SpatialCalOverlayModels = New SpatialCalOverlayModels()
                scom.LayerName = ""
                scom.ZoomValue = ""
                For IndexIn = LBound(Overlay) To UBound(Overlay) 'TableName
                    If IndexIn = 0 Then
                        scom.LayerName = Overlay(IndexIn).ToString()
                    ElseIf IndexIn = 1 Then
                        scom.ZoomValue = Overlay(IndexIn).ToString()
                    End If
                Next
                If String.IsNullOrWhiteSpace(scom.LayerName.Trim()) = False Then
                    SpatialCalOverlayModels.Add(scom)
                End If
            End If
        Next
        Return SpatialCalOverlayModels
    End Function

    Protected Function ReadFeatureFormat(ByVal xFeature As String) As List(Of SpatialCalFeatureLayerModels)
        Dim feature = CheckIfNull(xFeature).Split(",")
        Dim SpatialCalFeatureLayerModels As New List(Of SpatialCalFeatureLayerModels)

        For j = LBound(feature) To UBound(feature)
            Dim scfl As New SpatialCalFeatureLayerModels()
            scfl.LayerName = feature(j).ToString()
            SpatialCalFeatureLayerModels.Add(scfl)
        Next
        Return SpatialCalFeatureLayerModels
    End Function

    Protected Function GetOverlayFormat(ByVal jsonValues As String) As String
        Dim result As String = ""
        If jsonValues.Length > 0 Then
            Dim records As List(Of Dictionary(Of String, String)) = JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValues)
            Dim LayerName As String
            Dim ZoomValue As String
            Dim database As String = Convert.ToString(Session("database"))


            For Each record In records
                LayerName = CStr(record("LayerName"))
                ZoomValue = CStr(record("ZoomValue"))
                If String.IsNullOrWhiteSpace(LayerName.Trim()) = False AndAlso String.IsNullOrWhiteSpace(ZoomValue.Trim()) = False Then
                    result = result & String.Format("{0},{1};", LayerName, ZoomValue)
                End If
            Next
        End If
        Return result
    End Function

    Protected Function GetFeatureFormat(ByVal jsonValues As String) As String
        Dim result As String = ""
        If jsonValues.Length > 0 Then
            Dim records As List(Of Dictionary(Of String, String)) = JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValues)
            Dim LayerName As String
            Dim database As String = Convert.ToString(Session("database"))
            For Each record In records
                LayerName = CStr(record("LayerName"))
                If String.IsNullOrWhiteSpace(LayerName.Trim()) = False Then
                    If result.Length = 0 Then
                        result = LayerName
                    Else
                        result = result & "," & LayerName
                    End If
                End If
            Next
        End If
        Return result
    End Function

    Protected Sub InitStartModule()
        Dim store = Me.cboStartModule.GetStore()
        store.DataSource = New Object() {
          New Object() {"cbProfile", "OASIS Profile", "OASIS Profile"},
          New Object() {"cbOperations", "Operations", "Operations"},
          New Object() {"cbContent", "Dynamic Content", "Dynamic Content"},
          New Object() {"cbDynamicData", "Data Entry", "Data Entry"},
          New Object() {"cbReports", "Reporting", "Reporting"}
          }
        store.DataBind()
    End Sub

    Public Sub LoadUserGroups()
        Dim database As String = Session("database")
        Dim dt As DataTable
        Try
            dt = UserGroupService.GetInstance().LoadForComboboxUserGroupDatable(database)
            If dt.Rows.Count > 0 Then
                cboUserGroups.DataSource = dt
                cboUserGroups.DataTextField = "Name"
                cboUserGroups.DataValueField = "ID"
            Else
                cboUserGroups.DataSource = Nothing
            End If
            cboUserGroups.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub LoadUserGrpupsOld()
        Dim sqlConnect As New SqlConnection("Server=" & sqlServer & ";Database=" & Session("database") & ";User Id=" & sqlUserId & ";Password=" & sqlPassword & ";Trusted_Connection=" & sqlTrustedConnection & ";")
        Dim sqlCommand As New SqlCommand()
        Dim commandText As String = Nothing
        Dim dr As SqlDataReader = Nothing
        commandText = "SELECT ID,Name FROM [UserGroups]"
        Try
            sqlConnect.Open()
            With sqlCommand
                .CommandText = commandText
                .Connection = sqlConnect
                .CommandType = CommandType.Text
                dr = .ExecuteReader()
            End With
            If dr.HasRows Then
                Dim dt As New DataTable()
                dt.Load(dr)
                cboUserGroups.DataSource = dt
                cboUserGroups.DataTextField = "Name"
                cboUserGroups.DataValueField = "ID"
            Else
                cboUserGroups.DataSource = Nothing
            End If
            cboUserGroups.DataBind()
            sqlConnect.Close()
            dr.Close()
        Catch ex As Exception
            'Response.Write("<script>alert('" & ex.Message & "');</script>")
            If Not (sqlConnect Is Nothing) Then
                If sqlConnect.State = Data.ConnectionState.Open Then
                    sqlConnect.Close()
                End If
            End If
        End Try
        sqlConnect = Nothing
        sqlCommand = Nothing
        commandText = Nothing
        dr = Nothing
    End Sub

    Protected Sub SaveAllSettings(ByVal overLayValues As String, ByVal featureValues As String)
        Dim database As String = Session("database")
        Dim sModMenus As String = ""
        Dim isUpdate As Integer = -1

        'General
        If chkOasisProfile.Checked = True Then
            If chkOasisProfile.Checked = True Then sModMenus = "cbProfile"
        End If
        If chkOperations.Checked = True Then
            If Len(sModMenus) > 2 Then
                sModMenus &= ","
            End If
            sModMenus &= "cbOperations"
        End If

        If chkRssFeeds.Checked = True Then
            If Len(sModMenus) > 2 Then
                sModMenus &= ","
            End If
            sModMenus &= "cbContent"
        End If

        If chkDynamicData.Checked = True Then
            If Len(sModMenus) > 2 Then
                sModMenus &= ","
            End If
            sModMenus &= "cbDynamicData"
        End If

        If chkDynamicReports.Checked = True Then
            If Len(sModMenus) > 2 Then
                sModMenus &= ","
            End If
            sModMenus &= "cbReports"
        End If
        Select Case cboStartModule.SelectedIndex
            Case 0
                ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "CurrentActiveMeny", "SettingValue1", "cbProfile")
            Case 1
                ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "CurrentActiveMeny", "SettingValue1", "cbOperations")
            Case 2
                ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "CurrentActiveMeny", "SettingValue1", "cbContent")
            Case 3
                ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "CurrentActiveMeny", "SettingValue1", "cbDynamicData")
            Case 4
                ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "CurrentActiveMeny", "SettingValue1", "cbReports")
        End Select

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "VisibleMainModuleMenus", "SettingValue1", sModMenus)

        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"ProfileSettings", "SettingValue2", Now())
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"ProfileSettings", "SettingValue5", txtClientLanguage.Text)

        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"ServerConnectionParameters", "SettingValue1", txtServerConnectionTimeOut.Text)
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"ServerConnectionParameters", "SettingValue2", txtServerConnectionRetries.Text)


        'Synchronisation

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "InetConnectionSettings", "SettingValue1", If(chkEnableInternetSync.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "InetConnectionSettings", "SettingValue2", txtInterval.Text)

        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"attachmentsURL", "SettingValue1", txtAttachmentURL.Text)

        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue1", Integer.Parse(txtColorBack.Text, System.Globalization.NumberStyles.HexNumber).ToString())
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue2", Integer.Parse(txtColorFore.Text, System.Globalization.NumberStyles.HexNumber).ToString())

        If String.IsNullOrWhiteSpace(txtColorBack.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue1", ImmapUtil.GetInstance.Hex2LongVB6(txtColorBack.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue1", "")
        End If
        If String.IsNullOrWhiteSpace(txtColorFore.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue2", ImmapUtil.GetInstance.Hex2LongVB6(txtColorFore.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Notifier", "SettingValue2", "")
        End If


        'Administrative Location
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel0", "SettingValue1", txtAdminLevel1.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel0", "SettingValue2", txtFldName1.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel0", "SettingValue3", txtAdmCode1.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel0", "SettingValue4", txtAlias1.Text)

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel1", "SettingValue1", txtAdminLevel2.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel1", "SettingValue2", txtFldName2.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel1", "SettingValue3", txtAdmCode2.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel1", "SettingValue4", txtAlias2.Text)

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel2", "SettingValue1", txtAdminLevel3.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel2", "SettingValue2", txtFldName3.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel2", "SettingValue3", txtAdmCode3.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel2", "SettingValue4", txtAlias3.Text)


        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel3", "SettingValue1", txtAdminLevel4.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel3", "SettingValue2", txtFldName4.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel3", "SettingValue3", txtAdmCode4.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLevel3", "SettingValue4", txtAlias4.Text)


        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLocation", "SettingValue1", txtAdminLevel5.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLocation", "SettingValue2", txtFldName5.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLocation", "SettingValue3", txtAdmCode5.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AdminLocation", "SettingValue4", txtAlias5.Text)
        If overLayValues.Length > 0 Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "SpatialCalculator", "SettingValue2", GetOverlayFormat(overLayValues))
        End If
        If featureValues.Length > 0 Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "SpatialCalculator", "settingvalue3", GetFeatureFormat(featureValues))
        End If
        'Hot Keys
        Dim sScripts As String = ""

        If Not (txtHotKey1.Text.Trim.Equals("")) Then
            sScripts = txtHotKey1.Text
        End If

        If Not (txtHotKey2.Text.Trim.Equals("")) Then
            If Not (sScripts.Equals("")) Then
                sScripts = sScripts & ","
            End If

            sScripts = sScripts & txtHotKey2.Text
        End If

        If Not (txtHotKey3.Text.Trim.Equals("")) Then
            If Not (sScripts.Equals("")) Then
                sScripts = sScripts & ","
            End If

            sScripts = sScripts & txtHotKey3.Text
        End If

        If Not (txtHotKey4.Text.Trim.Equals("")) Then
            If Not (sScripts.Equals("")) Then
                sScripts = sScripts & ","
            End If

            sScripts = sScripts & txtHotKey4.Text
        End If

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "Scripts", "SettingValue1", sScripts)

        'Misc
        ' ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"InitialOperationsTabNumber", "SettingValue2", If(chkPromptSaveOnExits.Value = True, "1", "0"))

        'chkCommondity.Value = False
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"ShowCommodityTab", "SettingValue1", If(chkCommondity.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowMATab", "SettingValue1", If(chkMineActionModule.Value = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowAdvancedDebug", "SettingValue1", If(chkAdvanceDebug.Value = True, "1", "0"))


        ImmapUtil.GetInstance().IncreseSetting(database, cboUserGroups.SelectedItem.Text, "ProfileSettings", "[SettingValue1]")

        Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex

    End Sub

    Private Sub ClearControl()
        chkOasisProfile.Value = False
        chkOperations.Value = False
        chkRssFeeds.Value = False
        chkDynamicData.Value = False
        chkDynamicReports.Value = False
        'txtClientLanguage.Text = ""
        'txtServerConnectionTimeOut.Text = ""
        'txtServerConnectionRetries.Text = ""

        'Synchronisation
        chkEnableInternetSync.Value = False
        txtInterval.Text = ""
        'txtAttachmentURL.Text = ""
        txtColorBack.Text = "FF3300"
        txtColorFore.Text = "FFFF66"

        'Administrative Location
        txtAdminLevel1.Text = ""
        txtFldName1.Text = ""
        txtAdmCode1.Text = ""
        txtAlias1.Text = ""

        txtAdminLevel2.Text = ""
        txtFldName2.Text = ""
        txtAdmCode2.Text = ""
        txtAlias2.Text = ""

        txtAdminLevel3.Text = ""
        txtFldName3.Text = ""
        txtAdmCode3.Text = ""
        txtAlias3.Text = ""

        txtAdminLevel4.Text = ""
        txtFldName4.Text = ""
        txtAdmCode4.Text = ""
        txtAlias4.Text = ""

        txtAdminLevel5.Text = ""
        txtFldName5.Text = ""
        txtAdmCode5.Text = ""
        txtAlias5.Text = ""

        'Hot Keys
        txtHotKey1.Text = ""
        txtHotKey2.Text = ""
        txtHotKey3.Text = ""
        txtHotKey4.Text = ""

        'Misc
        'chkPromptSaveOnExits.Value = False
        'chkCommondity.Value = False
        chkMineActionModule.Value = False
        chkAdvanceDebug.Value = False
    End Sub

    Protected Sub cboUserGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboUserGroups.SelectedIndexChanged
        LoadAllSetting()
    End Sub

    Private Sub LoadAllSetting()
        ClearControl()
        Dim database As String = Session("database")
        Dim dtConfig As DataTable
        If (IsNothing(cboUserGroups.SelectedItem) = False) Then
            Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex
            Dim dr As SqlDataReader = Nothing
            Dim commandText As String = "SELECT [SettingName],[SettingValue1],[SettingValue2],[SettingValue3],[SettingValue4],[SettingValue5] FROM " & _
              cboUserGroups.SelectedItem.Text & "AppSettings"
            Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex

            dtConfig = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(database), commandText)
            If dtConfig.Rows.Count > 0 Then
                For i As Integer = 0 To dtConfig.Rows.Count - 1
                    Try
                        Dim SettingName As String = Convert.ToString(dtConfig.Rows(i)(0))
                        Dim SettingValue1 As String = Convert.ToString(dtConfig.Rows(i)(1))
                        If SettingName.Equals("VisibleMainModuleMenus") = True Then
                            Dim sModMenus = Split(CheckIfNull(SettingValue1), ",")
                            For index As Integer = 0 To UBound(sModMenus)
                                Select Case sModMenus(index)
                                    Case "cbProfile"
                                        chkOasisProfile.Value = True
                                    Case "cbOperations"
                                        chkOperations.Value = True
                                    Case "cbContent"
                                        chkRssFeeds.Value = True
                                    Case "cbDynamicData"
                                        chkDynamicData.Value = True
                                    Case "cbReports"
                                        chkDynamicReports.Value = True
                                End Select
                            Next
                        End If

                        If SettingName.Equals("CurrentActiveMeny") = True Then
                            If SettingValue1.Equals("cbOperations") = True Then
                                cboStartModule.SetValue("cbOperations")
                            ElseIf SettingValue1.Equals("cbContent") = True Then
                                cboStartModule.SetValue("cbContent")
                            ElseIf SettingValue1.Equals("cbDynamicData") = True Then
                                cboStartModule.SetValue("cbDynamicData")
                            ElseIf SettingValue1.Equals("cbReports") = True Then
                                cboStartModule.SetValue("cbReports")
                            Else
                                cboStartModule.SetValue("cbProfile")
                            End If
                        End If
                        If SettingName.Equals("ProfileSettings") = True Then
                            Dim SettingValue5 As String = Convert.ToString(dtConfig.Rows(i)(5))
                            '  txtClientLanguage.Text = CheckIfNull(SettingValue5)
                        End If

                        If SettingName.Equals("ServerConnectionParameters") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            '   txtServerConnectionTimeOut.Text = CheckIfNull(SettingValue1)
                            '  txtServerConnectionRetries.Text = CheckIfNull(SettingValue2)
                        End If

                        If SettingName.Equals("InetConnectionSettings") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            If SettingValue1.Equals("1") Then
                                chkEnableInternetSync.Value = True
                            Else
                                chkEnableInternetSync.Value = False
                            End If
                            txtInterval.Text = CheckIfNull(SettingValue2)
                        End If


                        If SettingName.Equals("Scripts") = True Then
                            Dim sScripts() As String
                            If Not (SettingValue1) = Nothing Then
                                If SettingValue1.Length > 4 Then
                                    sScripts = SettingValue1.Split(",")
                                    Dim cnt = 0
                                    For j = LBound(sScripts) To UBound(sScripts)
                                        If cnt = 0 Then
                                            txtHotKey1.Text = sScripts(j)
                                        ElseIf cnt = 1 Then
                                            txtHotKey2.Text = sScripts(j)
                                        ElseIf cnt = 2 Then
                                            txtHotKey3.Text = sScripts(j)
                                        ElseIf cnt = 3 Then
                                            txtHotKey4.Text = sScripts(j)
                                        End If
                                        cnt = cnt + 1
                                    Next

                                End If
                            End If
                        End If

                        If SettingName.Equals("ProfileSettings") = True Then
                            Dim SettingValue5 As String = Convert.ToString(dtConfig.Rows(i)(5))
                            '  txtClientLanguage.Text = CheckIfNull(SettingValue5)
                        End If


                        If SettingName.Equals("SpatialCalculator") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SpatialCalOverlayModels As New List(Of SpatialCalOverlayModels)
                            Dim SpatialCalFeatureLayerModels As New List(Of SpatialCalFeatureLayerModels)
                            SpatialCalOverlayModels = ReadOverlayFormat(SettingValue2)
                            SpatialCalFeatureLayerModels = ReadFeatureFormat(SettingValue3)
                            If SpatialCalOverlayModels IsNot Nothing Then
                                OverlayLayerStore.DataSource = SpatialCalOverlayModels
                            Else
                                OverlayLayerStore.DataSource = New DataTable()
                            End If

                            OverlayLayerStore.DataBind()

                            If SpatialCalOverlayModels IsNot Nothing Then
                                FeatureLayerStore.DataSource = SpatialCalFeatureLayerModels
                            Else
                                FeatureLayerStore.DataSource = New DataTable()
                            End If

                            FeatureLayerStore.DataBind()
                        End If

                        If SettingName.Equals("Notifier") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            'Dim s1 As String = CLng(SettingValue1).ToString("X6")
                            'Dim s2 As String = CLng(SettingValue2).ToString("X6")
                            Dim s1 As String = ImmapUtil.GetInstance.Long2HexVB6(CLng(SettingValue1))
                            Dim s2 As String = ImmapUtil.GetInstance.Long2HexVB6(CLng(SettingValue2))
                            txtColorBack.Text = s1
                            txtColorFore.Text = s2
                            buttonCPE1.SelectedColor = CheckIfNull(s1)
                            buttonCPE2.SelectedColor = CheckIfNull(s2)
                        End If



                        If SettingName.Equals("AdminLevel0") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            txtAdminLevel1.Text = CheckIfNull(SettingValue1)
                            txtFldName1.Text = CheckIfNull(SettingValue2)
                            txtAdmCode1.Text = CheckIfNull(SettingValue3)
                            txtAlias1.Text = CheckIfNull(SettingValue4)
                        End If
                        If SettingName.Equals("AdminLevel1") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            txtAdminLevel2.Text = CheckIfNull(SettingValue1)
                            txtFldName2.Text = CheckIfNull(SettingValue2)
                            txtAdmCode2.Text = CheckIfNull(SettingValue3)
                            txtAlias2.Text = CheckIfNull(SettingValue4)
                        End If
                        If SettingName.Equals("AdminLevel2") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            txtAdminLevel3.Text = CheckIfNull(SettingValue1)
                            txtFldName3.Text = CheckIfNull(SettingValue2)
                            txtAdmCode3.Text = CheckIfNull(SettingValue3)
                            txtAlias3.Text = CheckIfNull(SettingValue4)
                        End If
                        If SettingName.Equals("AdminLevel3") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            txtAdminLevel4.Text = CheckIfNull(SettingValue1)
                            txtFldName4.Text = CheckIfNull(SettingValue2)
                            txtAdmCode4.Text = CheckIfNull(SettingValue3)
                            txtAlias4.Text = CheckIfNull(SettingValue4)
                        End If
                        If SettingName.Equals("AdminLocation") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            txtAdminLevel5.Text = CheckIfNull(SettingValue1)
                            txtFldName5.Text = CheckIfNull(SettingValue2)
                            txtAdmCode5.Text = CheckIfNull(SettingValue3)
                            txtAlias5.Text = CheckIfNull(SettingValue4)
                        End If

                        'Misc
                        If SettingName.Equals("InitialOperationsTabNumber") = True Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))

                            If SettingValue2.Equals("1") Then
                                '       chkPromptSaveOnExits.Value = True
                            Else
                                '       chkPromptSaveOnExits.Value = False
                            End If
                        End If

                        If SettingName.Equals("ShowCommodityTab") = True Then
                            If SettingValue1.Equals("1") Then
                                '     chkCommondity.Value = True
                            Else
                                '    chkCommondity.Value = False
                            End If
                        End If

                        If SettingName.Equals("ShowMATab") = True Then
                            If SettingValue1.Equals("1") Then
                                chkMineActionModule.Value = True
                            Else
                                chkMineActionModule.Value = False
                            End If
                        End If

                        If SettingName.Equals("ShowAdvancedDebug") = True Then
                            If SettingValue1.Equals("1") Then
                                chkAdvanceDebug.Value = True
                            Else
                                chkAdvanceDebug.Value = False
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                Next
            Else
                Exit Sub
            End If
        End If
    End Sub
    Private Function CheckIfNull(sString As Object) As String
        If String.IsNullOrWhiteSpace(sString) Then
            CheckIfNull = ""
        Else
            CheckIfNull = sString
        End If
    End Function
End Class