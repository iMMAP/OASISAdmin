Imports Ext.Net
Imports System.Data.SqlClient
Imports System.Data
Imports Immap.Service
Partial Class MapViewSettings
    Inherits System.Web.UI.Page
    Private sqlServer As String = System.Configuration.ConfigurationManager.AppSettings("Server,Port").ToString()
    Private sqlUserId As String = System.Configuration.ConfigurationManager.AppSettings("UserId").ToString()
    Private sqlPassword As String = System.Configuration.ConfigurationManager.AppSettings("Password").ToString()
    Private sqlTrustedConnection As String = System.Configuration.ConfigurationManager.AppSettings("TrustedConnection").ToString()

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        btnSave.Attributes.Add("onclick", "if(confirm('Are you sure you want to perform this action?')== false) return false;")
        InitInitialModule()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Map Settings"
            LoadUserGroups()
            If Not (Session("cboUserGroupsIndex") Is Nothing) AndAlso (cboUserGroups.Items.Count > 0) Then
                cboUserGroups.SelectedIndex = Convert.ToInt32(Session("cboUserGroupsIndex"))
                cboUserGroups.DataBind()
            End If
            LoadAllSetting()
        End If
    End Sub

    Protected Sub InitInitialModule()
        Dim store = Me.cboInitialMenu.GetStore()
        store.DataSource = New Object() {
          New Object() {"L", "Legend", "Legend"},
          New Object() {"ML", "Map Library", "Map Library"},
          New Object() {"S", "Security", "Security"},
          New Object() {"C", "Commodity/NFI", "Commodity/NFI"},
          New Object() {"MA", "Mine Action", "Mine Action"},
          New Object() {"LIS", "LIS", "LIS"}
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
                cboUserGroups.SelectedIndex = 0
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


    Protected Sub SaveAllSettings()
        Dim database As String = Session("database")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "InitMap", "SettingValue1", txtInitialMap.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "InitialOperationsTabNumber", "SettingValue1", cboInitialMenu.SelectedIndex)
        '  ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "InitialOperationsTabNumber", "SettingValue2", If(chkPromptSave.Value = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowlegendTab", "SettingValue1", If(chkLegend.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowMapLibraryTab", "SettingValue1", If(chkMapLibrary.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowSecurityTab", "SettingValue1", If(chkSecurity.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "OpsTools", "SettingValue2", If(chkLegendgrouping.Value = True, "1", "0"))


        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "UserDefGridRecords", "SettingValue1", If(chkUseMaxRecords.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "UserDefGridRecords", "SettingValue2", txtMaxLevel.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "UserDefGridRecords", "SettingValue3", txtWarningLevel.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "UserDefGridRecords", "SettingValue4", If(chkMapFilter.Checked = True, "1", "0") & "," & If(chkMapSelect.Checked = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue1", If(chkSHowUtilityToolbar.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue3", If(chkGeoMarks.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue4", If(chkCoordinateTools.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue5", If(chkGotoLocation.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue6", If(chkSettings.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowExpandedToolBoxInGISWin", "SettingValue7", If(chkMagnifier.Value = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "HiddenLayers", "SettingValue1", txtHiddenLayer.Text)
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ThemeTool", "SettingValue1", If(chkUsePredefinedThemes.Value = True, "1", "0"))

        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowSelector", "SettingValue1", "0")
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowScalebar", "SettingValue1", "0")
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowScalebar", "SettingValue1", If(chkShowScalebar.Value = True, "1", "0"))


        Dim sTools As String = Nothing
        sTools = ""
        sTools = sTools & If(chkZoomIn.Checked = True, "btnZoomin", "")
        sTools = sTools & If(chkZoomOut.Checked = True, If(sTools.Length > 0, ",", "") & "btnZoomout", "")
        sTools = sTools & If(chkZoom.Checked = True, If(sTools.Length > 0, ",", "") & "btnZoom", "")
        sTools = sTools & If(chkPan.Checked = True, If(sTools.Length > 0, ",", "") & "btnPan", "")
        sTools = sTools & If(chkInFo.Checked = True, If(sTools.Length > 0, ",", "") & "btnInfo", "")
        sTools = sTools & If(chkFullExtent.Checked = True, If(sTools.Length > 0, ",", "") & "btnFullExtent", "")
        sTools = sTools & If(chkLayerExtent.Checked = True, If(sTools.Length > 0, ",", "") & "btnLayerExtent", "")
        sTools = sTools & If(chkZoomRect.Checked = True, If(sTools.Length > 0, ",", "") & "btnZoomRect", "")
        sTools = sTools & If(chkMeasure.Checked = True, If(sTools.Length > 0, ",", "") & "btnMeasure", "")
        sTools = sTools & If(chkMeasureArea.Checked = True, If(sTools.Length > 0, ",", "") & "btnMeasureArea", "")
        sTools = sTools & If(chkAddClippBoard.Checked = True, If(sTools.Length > 0, ",", "") & "btnAddClippBoard", "")
        sTools = sTools & If(chkShowSelector.Checked = True, If(sTools.Length > 0, ",", "") & "btnSelector", "")
        sTools = sTools & If(chkEmergency.Checked = True, If(sTools.Length > 0, ",", "") & "btnEmergency", "")
        sTools = sTools & If(chkPreviousExtent.Checked = True, If(sTools.Length > 0, ",", "") & "btnPreviousExtent", "")
        sTools = sTools & ",mnuSeparator"
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsV4", "SettingValue1", sTools)

        sTools = ""
        sTools = If(chkAddAnnotation.Checked = True, "btnAddAnnotation", "")
        sTools = sTools & If(chkAdminLocator.Checked = True, If(sTools.Length > 0, ",", "") & "btnAdminLocator", "")
        sTools = sTools & If(chkCannedReports.Checked = True, If(sTools.Length > 0, ",", "") & "btnCannedReports", "")
        sTools = sTools & If(chkSpatialCalc.Checked = True, If(sTools.Length > 0, ",", "") & "btnSpatialCalc", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsv4", "SettingValue3", sTools)

        sTools = ""
        sTools = If(chkPrint.Checked = True, "btnMapPrintTemplate", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsv4", "SettingValue4", sTools)

        sTools = ""
        sTools = If(chkRemoveLyr.Checked = True, "btnRemoveLyr", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsV4", "SettingValue5", sTools)

        sTools = ""
        sTools = If(chkAddLyr.Checked = True, "btnAddLyr", "")
        sTools = sTools & If(chkLoadSQLLyr.Checked = True, If(sTools.Length > 0, ",", "") & "btnLoadSQLLyr", "")
        sTools = sTools & If(chkAddWMS.Checked = True, If(sTools.Length > 0, ",", "") & "btnAddWMS_WFS", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsV4", "SettingValue6", sTools)

        sTools = ""
        sTools = If(chkOpenMap.Checked = True, "btnOpenMap", "")
        sTools = sTools & If(chkExportMapDefFile.Checked = True, If(sTools.Length > 0, ",", "") & "btnExportMapDefFile", "")
        sTools = sTools & If(chkNewProject.Checked = True, If(sTools.Length > 0, ",", "") & "btnNewProject", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsV4", "SettingValue7", sTools)

        sTools = ""
        sTools = sTools & If(chkCreateDBLyr.Checked = True, "btnCreateDBLyr", "")
        sTools = sTools & If(chkExportToShape.Checked = True, If(sTools.Length > 0, ",", "") & "btnExportToShape", "")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "AvailableMapToolsV4", "SettingValue8", sTools)


        sTools = ""

        ImmapUtil.GetInstance().IncreseSetting(database, cboUserGroups.SelectedItem.Text, "ProfileSettings", "SettingValue1")


        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowScalebar", "SettingValue1", If(chkShowScalebar.Value = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue1", If(chkShowZoombar.Value = True, "1", "0"))
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue2", Integer.Parse(txtZoomSelectorColour.Text, System.Globalization.NumberStyles.HexNumber).ToString())
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue3", Integer.Parse(txtZoomRageMakers.Text, System.Globalization.NumberStyles.HexNumber).ToString())
        'ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue4", Integer.Parse(txtBackPanel.Text, System.Globalization.NumberStyles.HexNumber).ToString())
        If String.IsNullOrWhiteSpace(txtZoomSelectorColour.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue2", ImmapUtil.GetInstance.Hex2LongVB6(txtZoomSelectorColour.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue2", "")
        End If
        If String.IsNullOrWhiteSpace(txtZoomRageMakers.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue3", ImmapUtil.GetInstance.Hex2LongVB6(txtZoomRageMakers.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue3", "")
        End If
        If String.IsNullOrWhiteSpace(txtBackPanel.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue4", ImmapUtil.GetInstance.Hex2LongVB6(txtBackPanel.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue4", "")
        End If



        If String.IsNullOrWhiteSpace(txtXMin.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue5", Double.Parse(txtXMin.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue5", txtXMin.Text.Trim())
        End If
        If String.IsNullOrWhiteSpace(txtXMax.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue6", Double.Parse(txtXMax.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue6", txtXMax.Text.Trim())
        End If
        If String.IsNullOrWhiteSpace(txtYMIN.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue7", Double.Parse(txtYMIN.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue7", txtYMIN.Text.Trim())
        End If
        If String.IsNullOrWhiteSpace(txtYMAX.Text) = False Then
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue8", Double.Parse(txtYMAX.Text))
        Else
            ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowZoomBar", "SettingValue8", txtYMAX.Text.Trim())
        End If


        Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex
    End Sub

    Protected Sub cboUserGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboUserGroups.SelectedIndexChanged
        LoadAllSetting()
    End Sub

    Private Sub LoadAllSetting()
        ClearControl()
        If (IsNothing(cboUserGroups.SelectedItem) = False) Then
            Dim database As String = Session("database")
            Dim dtConfig As DataTable
            Dim commandText = "SELECT [SettingName],[SettingValue1],[SettingValue2],[SettingValue3],[SettingValue4]," &
                              " [SettingValue5],[SettingValue6],[SettingValue7],[SettingValue8]  FROM " &
                              cboUserGroups.SelectedItem.Text & "AppSettings"
            Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex
            Try
                dtConfig = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(database), commandText)
                If dtConfig.Rows.Count > 0 Then
                    For i As Integer = 0 To dtConfig.Rows.Count - 1
                        Dim SettingName As String = Convert.ToString(dtConfig.Rows(i)(0))
                        Dim SettingValue1 As String = Convert.ToString(dtConfig.Rows(i)(1))

                        If SettingName.Equals("InitMap") Then
                            txtInitialMap.Text = SettingValue1
                        End If

                        If SettingName.Equals("InitialOperationsTabNumber") Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            If SettingValue1.Equals("0") Then
                                cboInitialMenu.SetValue("L")
                            ElseIf SettingValue1.Equals("1") Then
                                cboInitialMenu.SetValue("ML")
                            ElseIf SettingValue1.Equals("2") Then
                                cboInitialMenu.SetValue("S")
                            ElseIf SettingValue1.Equals("3") Then
                                cboInitialMenu.SetValue("C")
                            ElseIf SettingValue1.Equals("4") Then
                                cboInitialMenu.SetValue("MA")
                            ElseIf SettingValue1.Equals("5") Then
                                cboInitialMenu.SetValue("LIS")
                            End If
                            chkPromptSave.Value = If(CheckIfNull(SettingValue2) = "1", True, False)
                        End If

                        If SettingName.Equals("ShowlegendTab") Then
                            If SettingValue1.Equals("1") Then
                                chkLegend.Value = True
                            Else
                                chkLegend.Value = False
                            End If
                        End If


                        If SettingName.Equals("OpsTools") Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            If SettingValue2.Equals("1") Then
                                chkLegendgrouping.Value = True
                            Else
                                chkLegendgrouping.Value = False
                            End If
                        End If
                        If SettingName.Equals("ShowMapLibraryTab") Then
                            If SettingValue1.Equals("1") Then
                                chkMapLibrary.Value = True
                            Else
                                chkMapLibrary.Value = False
                            End If
                        End If

                        If SettingName.Equals("ShowSecurityTab") Then
                            If SettingValue1.Equals("1") Then
                                chkSecurity.Value = True
                            Else
                                chkSecurity.Value = False
                            End If
                        End If

                        If SettingName.Equals("UserDefGridRecords") Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))

                            If SettingValue1.Equals("1") Then
                                chkUseMaxRecords.Value = True
                            Else
                                chkUseMaxRecords.Value = False
                            End If

                            txtMaxLevel.Text = CheckIfNull(SettingValue2)
                            txtWarningLevel.Text = CheckIfNull(SettingValue3)

                            Dim sGridOpt() As String
                            If Not String.IsNullOrWhiteSpace(SettingValue4) Then
                                If SettingValue4.Length > 2 Then
                                    sGridOpt = Split(SettingValue4, ",")
                                    chkMapFilter.Value = If(sGridOpt(0) = "1", True, False)
                                    chkMapSelect.Value = If(sGridOpt(1) = "1", True, False)
                                End If
                            End If
                        End If

                        If SettingName.Equals("ShowExpandedToolBoxInGISWin") Then
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            Dim SettingValue5 As String = Convert.ToString(dtConfig.Rows(i)(5))
                            Dim SettingValue6 As String = Convert.ToString(dtConfig.Rows(i)(6))
                            Dim SettingValue7 As String = Convert.ToString(dtConfig.Rows(i)(7))

                            chkSHowUtilityToolbar.Value = If(CheckIfNull(SettingValue1) = "1", True, False)
                            chkGeoMarks.Value = If(CheckIfNull(SettingValue3) = "1", True, False)
                            chkCoordinateTools.Value = If(CheckIfNull(SettingValue4) = "1", True, False)
                            chkGotoLocation.Value = If(CheckIfNull(SettingValue5) = "1", True, False)
                            chkSettings.Value = If(CheckIfNull(SettingValue6) = "1", True, False)
                            chkMagnifier.Value = If(CheckIfNull(SettingValue7) = "1", True, False)
                        End If
                        If SettingName.Equals("HiddenLayers") Then
                            txtHiddenLayer.Text = CheckIfNull(SettingValue1)
                        End If

                        If SettingName.Equals("ThemeTool") Then
                            chkUsePredefinedThemes.Value = If(CheckIfNull(SettingValue1) = "1", True, False)
                        End If

                        If SettingName.Equals("ShowZoomBar") = True Then
                            If SettingValue1.Equals("1") Then
                                chkShowZoombar.Value = True
                            Else
                                chkShowZoombar.Value = False
                            End If
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            Dim SettingValue5 As String = Convert.ToString(dtConfig.Rows(i)(5))
                            Dim SettingValue6 As String = Convert.ToString(dtConfig.Rows(i)(6))
                            Dim SettingValue7 As String = Convert.ToString(dtConfig.Rows(i)(7))
                            Dim SettingValue8 As String = Convert.ToString(dtConfig.Rows(i)(8))
                            'Dim s2 As String = CLng(SettingValue2).ToString("X6")
                            'Dim s3 As String = CLng(SettingValue3).ToString("X6")
                            'Dim s4 As String = CLng(SettingValue4).ToString("X6")
                            Dim s2 As String = ImmapUtil.GetInstance.Long2HexVB6(CLng(SettingValue2))
                            Dim s3 As String = ImmapUtil.GetInstance.Long2HexVB6(CLng(SettingValue3))
                            Dim s4 As String = ImmapUtil.GetInstance.Long2HexVB6(CLng(SettingValue4))

                            txtZoomSelectorColour.Text = s2
                            txtZoomRageMakers.Text = s3
                            txtBackPanel.Text = s4
                            txtXMin.Text = SettingValue5
                            txtXMax.Text = SettingValue6
                            txtYMIN.Text = SettingValue7
                            txtYMAX.Text = SettingValue8
                            btnCPE3.SelectedColor = CheckIfNull(s2)
                            btnCPE4.SelectedColor = CheckIfNull(s3)
                            btnCPE5.SelectedColor = CheckIfNull(s4)
                        End If

                        If SettingName.Equals("ShowScalebar") = True Then
                            If SettingValue1.Equals("1") Then
                                chkShowScalebar.Value = True
                            Else
                                chkShowScalebar.Value = False
                            End If
                        End If

                        If SettingName.Equals("ShowSelector") = True Then
                            If SettingValue1.Equals("1") Then
                                chkShowSelector.Checked = True
                            Else
                                chkShowSelector.Checked = False
                            End If
                        End If

                        If SettingName.Equals("AvailableMapToolsV4") Then
                            Dim SettingValue2 As String = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 As String = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 As String = Convert.ToString(dtConfig.Rows(i)(4))
                            Dim SettingValue5 As String = Convert.ToString(dtConfig.Rows(i)(5))
                            Dim SettingValue6 As String = Convert.ToString(dtConfig.Rows(i)(6))
                            Dim SettingValue7 As String = Convert.ToString(dtConfig.Rows(i)(7))
                            Dim SettingValue8 As String = Convert.ToString(dtConfig.Rows(i)(8))
                            Dim sTools = CheckIfNull(SettingValue1).Split(",")

                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnPreviousExtent"
                                        chkPreviousExtent.Checked = True
                                    Case "btnAddClippBoard"
                                        chkAddClippBoard.Checked = True
                                    Case "btnZoomin"
                                        chkZoomIn.Checked = True
                                    Case "btnZoomout"
                                        chkZoomOut.Checked = True
                                    Case "btnZoom"
                                        chkZoom.Checked = True
                                    Case "btnPan"
                                        chkPan.Checked = True
                                    Case "btnZoomRect"
                                        chkZoomRect.Checked = True
                                    Case "btnInfo"
                                        chkInFo.Checked = True
                                    Case "btnFullExtent"
                                        chkFullExtent.Checked = True
                                    Case "btnLayerExtent"
                                        chkLayerExtent.Checked = True
                                    Case "btnMeasure"
                                        chkMeasure.Checked = True
                                    Case "btnSelector"
                                        chkShowSelector.Checked = True
                                    Case "btnEmergency"
                                        chkEmergency.Checked = True
                                    Case "btnMeasureArea"
                                        chkMeasureArea.Checked = True
                                End Select
                            Next

                            sTools = CheckIfNull(SettingValue3).Split(",")
                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnAddAnnotation"
                                        chkAddAnnotation.Checked = True
                                    Case "btnAdminLocator"
                                        chkAdminLocator.Checked = True
                                    Case "btnCannedReports"
                                        chkCannedReports.Checked = True
                                    Case "btnSpatialCalc"
                                        chkSpatialCalc.Checked = True
                                End Select
                            Next

                            sTools = CheckIfNull(SettingValue4).Split(",")
                            For j = LBound(sTools) To UBound(sTools)

                                Select Case sTools(j)
                                    Case "btnMapPrintTemplate"
                                        chkPrint.Checked = True
                                End Select
                            Next

                            sTools = CheckIfNull(SettingValue5).Split(",")
                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnRemoveLyr"
                                        chkRemoveLyr.Checked = True
                                End Select
                            Next

                            sTools = CheckIfNull(SettingValue6).Split(",")
                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnAddLyr"
                                        chkAddLyr.Checked = True
                                    Case "btnAddWMS_WFS"
                                        chkAddWMS.Checked = True
                                    Case "btnLoadSQLLyr"
                                        chkLoadSQLLyr.Checked = True
                                End Select
                            Next
                            sTools = CheckIfNull(SettingValue7).Split(",")
                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnExportMapDefFile"
                                        chkExportMapDefFile.Checked = True
                                    Case "btnOpenMap"
                                        chkOpenMap.Checked = True
                                    Case "btnNewProject"
                                        chkNewProject.Checked = True
                                End Select
                            Next
                            sTools = CheckIfNull(SettingValue8).Split(",")
                            For j = LBound(sTools) To UBound(sTools)
                                Select Case sTools(j)
                                    Case "btnCreateDBLyr"
                                        chkCreateDBLyr.Checked = True
                                    Case "btnExportToShape"
                                        chkExportToShape.Checked = True
                                End Select
                            Next
                        End If
                    Next
                Else
                    Exit Sub
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub ClearControl()
        txtZoomSelectorColour.Text = ""
        txtZoomRageMakers.Text = ""
        txtBackPanel.Text = ""
        txtXMax.Text = ""
        txtXMin.Text = ""
        txtYMAX.Text = ""
        txtYMIN.Text = ""
        chkNewProject.Checked = False
        chkShowZoombar.Value = False
        chkShowZoombar.Checked = False
        chkShowSelector.Checked = False
        chkShowSelector.Checked = False
        chkShowScalebar.Checked = False
        chkShowScalebar.Value = False
        chkMeasure.Checked = False
        chkSpatialCalc.Checked = False
        txtInitialMap.Text = ""
        chkPromptSave.Value = False
        chkPromptSave.Checked = False
        chkLegend.Value = False
        chkMapLibrary.Value = False
        chkSecurity.Value = False
        chkUseMaxRecords.Value = False
        chkMapFilter.Value = False
        chkMapSelect.Value = False
        chkSHowUtilityToolbar.Value = False
        chkGeoMarks.Value = False
        chkCoordinateTools.Value = False
        chkGotoLocation.Value = False
        chkSettings.Value = False
        chkMagnifier.Value = False
        chkUsePredefinedThemes.Value = False
        txtMaxLevel.Text = ""
        txtHiddenLayer.Text = ""
        txtWarningLevel.Text = ""

        chkMeasureArea.Checked = False
        chkPreviousExtent.Checked = False
        chkAddWMS.Checked = False
        chkEmergency.Checked = False
        chkAddLyr.Checked = False
        chkRemoveLyr.Checked = False
        chkPrint.Checked = False
        chkAddClippBoard.Checked = False
        chkZoomIn.Checked = False
        chkZoom.Checked = False
        chkZoomOut.Checked = False
        chkPan.Checked = False
        chkInFo.Checked = False
        chkFullExtent.Checked = False
        chkLayerExtent.Checked = False
        chkZoomRect.Checked = False
        chkOpenMap.Checked = False
        'chkRepairDB.Checked = False
        chkCreateDBLyr.Checked = False
        chkExportMapDefFile.Checked = False
        chkExportToShape.Checked = False
        chkLoadSQLLyr.Checked = False
        'chkDynamicDataEntry.Checked = False
        'chkAdvancedDBManagement.Checked = False
        chkAdminLocator.Checked = False
        'chkCharting.Checked = False
        'chkSpatialAnalysis.Checked = False
        chkCannedReports.Checked = False
        chkAddAnnotation.Checked = False
        'chkOASISv1Charts.Checked = False
    End Sub

    Private Function CheckIfNull(sString As Object) As String
        If String.IsNullOrWhiteSpace(sString) Then
            CheckIfNull = ""
        Else
            CheckIfNull = sString
        End If
    End Function

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        Dim xTmp As Double
        If String.IsNullOrWhiteSpace(txtXMin.Text) = False AndAlso Double.TryParse(txtXMin.Text, xTmp) = False Then
            msg.Show(New MessageBoxConfig() With {
              .Title = "Warning",
              .Message = "Please enter number only for X-Min",
              .Buttons = MessageBox.Button.OK,
              .Icon = MessageBox.Icon.WARNING,
             .AnimEl = Me.panelMap.ClientID
            })
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtXMax.Text) = False AndAlso Double.TryParse(txtXMax.Text, xTmp) = False Then
            msg.Show(New MessageBoxConfig() With {
              .Title = "Warning",
              .Message = "Please enter number only for X-Max",
              .Buttons = MessageBox.Button.OK,
              .Icon = MessageBox.Icon.WARNING,
             .AnimEl = Me.panelMap.ClientID
            })
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtYMIN.Text) = False AndAlso Double.TryParse(txtYMIN.Text, xTmp) = False Then
            msg.Show(New MessageBoxConfig() With {
              .Title = "Warning",
              .Message = "Please enter number only for Y-Min",
              .Buttons = MessageBox.Button.OK,
              .Icon = MessageBox.Icon.WARNING,
             .AnimEl = Me.panelMap.ClientID
            })
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtYMAX.Text) = False AndAlso Double.TryParse(txtYMAX.Text, xTmp) = False Then
            msg.Show(New MessageBoxConfig() With {
              .Title = "Warning",
              .Message = "Please enter number only for Y-Max",
              .Buttons = MessageBox.Button.OK,
              .Icon = MessageBox.Icon.WARNING,
             .AnimEl = Me.panelMap.ClientID
            })
            Exit Sub
        End If

        SaveAllSettings()
        cboUserGroups_SelectedIndexChanged(sender, e)
    End Sub
End Class

