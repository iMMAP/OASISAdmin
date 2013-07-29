Imports Ext.Net
Imports Immap.Model
Imports Immap.Service
Partial Class ChartSettings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Chart Setting"
            ReloadDynamicDataManager()
        End If
    End Sub


    Protected Sub DynamicDataManagerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadDynamicDataManager()
    End Sub

    Protected Sub cboDynamicModule_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ClearForm()
        ReloadChartSettings()
    End Sub

    Protected Sub RowChartSettingsDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim GUID1 As String = e.ExtraParams("GUID1")

            If GUID1 Is Nothing Then
                e.Success = False
                e.ErrorMessage = "The list of user is empty"
                Exit Sub
            End If
            Try
                ChartSettingsService.GetInstance().DeleteByGUID(database, DDDefName, GUID1)
                e.Success = True
                ReloadChartSettings()
                ClearForm()
            Catch ex As Exception
                e.Success = False
            End Try
        End If
    End Sub

    Protected Sub ReloadChartSettings()
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Value
            If String.IsNullOrEmpty(DDDefName) = False Then
                Dim database As String = Convert.ToString(Session("database"))
                ChartSettingsService.GetInstance().FindAll(database, DDDefName, ChartSettingStore)
            End If
        End If
    End Sub

    Protected Sub ClearForm()
        frmChartSettings.Reset()
    End Sub

    Protected Sub ReloadDynamicDataManager()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataManagerService.GetInstance().GetDDDefNameAll(database, DynamicDataManagerStore)
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        ChartSettingSave(ImmapUtil.SaveType.INSERT)
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        ChartSettingSave(ImmapUtil.SaveType.UPDATE)
    End Sub

    Protected Sub ChartSettingSave(ByVal saveType As ImmapUtil.SaveType)
        Dim this = ChartSettingsService.GetInstance()
        Dim msg As New MessageBox()
        Dim database As String = Convert.ToString(Session("database"))
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Value
            If saveType = ImmapUtil.SaveType.INSERT Then
                this.Insert(database, DDDefName, ImmapUtil.NewGUid(), txtQueryName.Text, txtaSQLCommand.Text, txtaMSSQLCommand.Text,
                            If(chkUseChart.Checked = True, "1", "0"), If(chkbAutoLoadReport.Checked = True, "1", "0"),
                            txtaFilterSQL.Text, txtGroup.Text, txtaFilterMSSQL.Text)
            ElseIf saveType = ImmapUtil.SaveType.UPDATE Then
                If String.IsNullOrEmpty(txtGUID1.Text) Then
                    msg.Show(New MessageBoxConfig() With {
                        .Title = "Warning",
                        .Message = "Please select chart setting",
                        .Buttons = MessageBox.Button.OK,
                        .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmChartSettings.ClientID
                  })
                End If
                this.Update(database, DDDefName, txtGUID1.Text, txtQueryName.Text, txtaSQLCommand.Text, txtaMSSQLCommand.Text,
                                If(chkUseChart.Checked = True, "1", "0"), If(chkbAutoLoadReport.Checked = True, "1", "0"),
                                txtaFilterSQL.Text, txtGroup.Text, txtaFilterMSSQL.Text)
            End If
            ReloadChartSettings()
            ClearForm()
            Dim sm As RowSelectionModel = TryCast(Me.gpChartSettings.SelectionModel.Primary, RowSelectionModel)
            sm.ClearSelections()
            gpChartSettings.Call("clearMemory")
        End If
    End Sub
End Class
