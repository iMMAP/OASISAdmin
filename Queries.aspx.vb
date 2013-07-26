Imports Immap.Service
Imports Ext.Net
Imports System.Data
Partial Class Queries
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Queries"
            ReloadDynamicDataManager()
        End If
    End Sub

    Protected Sub DynamicDataManagerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadDynamicDataManager()
    End Sub

    Protected Sub cboDynamicModule_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ClearFormNearbyFeatures()
        If (cboDynamicModule.SelectedItem IsNot Nothing) Then
            ReloadQueries(cboDynamicModule.SelectedItem.Text)
        End If
    End Sub

    Protected Sub gpQuries_RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim GUID1 As String = e.ExtraParams("GUID1")
        If GUID1 Is Nothing Then
            e.Success = False
            Exit Sub
        End If
        Try
            If Not String.IsNullOrEmpty(GUID1) Then
                QueriesServices.GetInstance().DeleteById(database, DDDefName, GUID1)
                ReloadQueries(cboDynamicModule.SelectedItem.Text)
                ClearFormNearbyFeatures()
            End If
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            SaveQueries(ImmapUtil.SaveType.INSERT, DDDefName)
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            SaveQueries(ImmapUtil.SaveType.UPDATE, DDDefName)
        End If
    End Sub

    Protected Sub SaveQueries(ByVal saveType As ImmapUtil.SaveType, ByVal DDDefName As String)
        Dim GUID1 As String = ""
        Dim msg As New MessageBox()
        Dim database As String = Convert.ToString(Session("database"))
        Select Case saveType
            Case ImmapUtil.SaveType.INSERT
                GUID1 = ImmapUtil.NewGUid()
                QueriesServices.GetInstance().Insert(database,
                                                     DDDefName,
                                                     GUID1,
                                                     txtQueryName.Text,
                                                     txtaQuerySQL.Text,
                                                     txtaQueryMSSQL.Text)
            Case ImmapUtil.SaveType.UPDATE
                GUID1 = txtGUID1.Text
                If String.IsNullOrWhiteSpace(GUID1) Then
                    msg.Show(New MessageBoxConfig() With {
                        .Title = "Warning",
                        .Message = "Please select Queries",
                        .Buttons = MessageBox.Button.OK,
                        .Icon = MessageBox.Icon.WARNING,
                    .AnimEl = Me.frmQueries.ClientID
                  })
                    Exit Sub
                End If
                QueriesServices.GetInstance().Update(database,
                                                     DDDefName,
                                                     GUID1,
                                                     txtQueryName.Text,
                                                     txtaQuerySQL.Text,
                                                     txtaQueryMSSQL.Text)
        End Select
        ClearFormNearbyFeatures()
        ReloadQueries(DDDefName)
    End Sub


    Protected Sub ReloadDynamicDataManager()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataManagerService.GetInstance().GetDDDefNameAll(database, DynamicDataManagerStore)
    End Sub

    Protected Sub ReloadQueries(ByVal DDDefName As String)
        If Not (String.IsNullOrEmpty(DDDefName)) Then
            Dim database As String = Convert.ToString(Session("database"))
            QueriesServices.GetInstance().FindAll(database, DDDefName, QuriesStore)
        End If
    End Sub

    Protected Sub ClearFormNearbyFeatures()
        frmQueries.Reset()
    End Sub
End Class
