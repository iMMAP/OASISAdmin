Imports Ext.Net
Imports System.Data.SqlClient
Imports System.Data
Imports Immap.Service
Partial Class SecurityModuleSettings
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        btnSave.Attributes.Add("onclick", "if(confirm('Are you sure you want to perform this action?')== false) return false;")
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Security"
            LoadUserGroups()
            If Not (Session("cboUserGroupsIndex") Is Nothing) Then
                cboUserGroups.SelectedIndex = Convert.ToInt32(Session("cboUserGroupsIndex"))
                cboUserGroups.DataBind()
            End If
            LoadAllSetting()
        End If
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

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        SaveAllSetting()
        cboUserGroups_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub SaveAllSetting()
        Dim database As String = Session("database")
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "OASIS_Incident_Layer_Name", "SettingValue1", txtIncidentLayerName.Text)
        '  ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text,"SecurityTools", "SettingValue2", If(chkSecurityGraphs.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "SecurityTools", "SettingValue3", If(chkSecurityTrends.Value = True, "1", "0"))
        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "SecurityTools", "SettingValue4", If(chkAddIncident.Value = True, "1", "0"))

        ImmapUtil.GetInstance().SaveData(database, cboUserGroups.SelectedItem.Text, "ShowSecurityTab", "SettingValue2", txtLegendSymbolSize.Text)

        ImmapUtil.GetInstance().IncreseSetting(database, cboUserGroups.SelectedItem.Text, "ProfileSettings", "SettingValue1")

        Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex
    End Sub


    Protected Sub cboUserGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboUserGroups.SelectedIndexChanged
        LoadAllSetting()
    End Sub

    Protected Sub LoadAllSetting()
        ClearControl()
        Dim database As String = Session("database")
        Dim dtConfig As DataTable
        If (IsNothing(cboUserGroups.SelectedItem) = False) Then
            Dim commandText = "SELECT [SettingName],[SettingValue1],[SettingValue2],[SettingValue3],[SettingValue4],[SettingValue5] FROM " & _
              cboUserGroups.SelectedItem.Text & "AppSettings"
            Session("cboUserGroupsIndex") = cboUserGroups.SelectedIndex

            Try
                dtConfig = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(database), commandText)
                If dtConfig.Rows.Count > 0 Then
                    For i As Integer = 0 To dtConfig.Rows.Count - 1
                        Dim SettingName As String = Convert.ToString(dtConfig.Rows(i)(0))
                        Dim SettingValue1 = Convert.ToString(dtConfig.Rows(i)(1))
                        If SettingName.Equals("OASIS_Incident_Layer_Name") = True Then
                            txtIncidentLayerName.Text = CheckIfNull(SettingValue1)
                        End If

                        If SettingName.Equals("SecurityGridZoomLevels") = True Then
                            Dim SettingValue2 = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 = Convert.ToString(dtConfig.Rows(i)(3))
                        End If

                        If SettingName.Equals("ShowSecurityTab") = True Then
                            Dim SettingValue2 = Convert.ToString(dtConfig.Rows(i)(2))
                            txtLegendSymbolSize.Text = SettingValue2
                        End If

                        If SettingName.Equals("SecurityTools") = True Then
                            Dim SettingValue2 = Convert.ToString(dtConfig.Rows(i)(2))
                            Dim SettingValue3 = Convert.ToString(dtConfig.Rows(i)(3))
                            Dim SettingValue4 = Convert.ToString(dtConfig.Rows(i)(4))


                            'If SettingValue2.Equals("1") Then
                            '    chkSecurityGraphs.Value = True
                            'Else
                            '    chkSecurityGraphs.Value = False
                            'End If

                            If SettingValue3.Equals("1") Then
                                chkSecurityTrends.Value = True
                            Else
                                chkSecurityTrends.Value = False
                            End If

                            If SettingValue4.Equals("1") Then
                                chkAddIncident.Value = True
                            Else
                                chkAddIncident.Value = False
                            End If
                        End If

                        If SettingName.Equals("AdmProvSec") = True Then
                            Dim SettingValue3 = Convert.ToString(dtConfig.Rows(i)(3))
                        End If

                        If SettingName.Equals("AdmDistSec") = True Then
                            Dim SettingValue3 = Convert.ToString(dtConfig.Rows(i)(3))
                        End If

                        If SettingName.Equals("AdmLocalSec") = True Then
                            Dim SettingValue3 = Convert.ToString(dtConfig.Rows(i)(3))
                        End If
                    Next
                Else
                    Exit Sub
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub ClearControl()
        txtIncidentLayerName.Text = "Oincidents1"
        txtLegendSymbolSize.Text = ""
        ' chkSecurityGraphs.Value = False
        chkSecurityTrends.Value = False
        chkAddIncident.Value = False
    End Sub

    Private Function CheckIfNull(sString As Object) As String
        If String.IsNullOrWhiteSpace(sString) Then
            CheckIfNull = ""
        Else
            CheckIfNull = sString
        End If
    End Function
End Class
