Imports Immap.Service
Imports Ext.Net
Imports System.Data
Partial Class ComboRelations
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Combobox Relations"
            ReloadDynamicDataManager()
        End If
    End Sub

    Protected Sub DynamicDataManagerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadDynamicDataManager()
    End Sub

    Protected Sub ColumnNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        If (cboDynamicModule.SelectedItem IsNot Nothing AndAlso cboTableName.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim sTableName As String = cboTableName.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ColumnNameStore)
            cboFieldName.SetValue("")
        End If
    End Sub

    Protected Sub ParentColumnNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        If (cboDynamicModule.SelectedItem IsNot Nothing AndAlso cboTableName.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim sTableName As String = cboTableName.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ParentColumnNameStore)
            cboParentFieldName.SetValue("")
        End If
    End Sub

    Protected Sub TableNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            ReloadTableName(cboDynamicModule.SelectedItem.Text)
        End If
    End Sub

    Protected Sub cboDynamicModule_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ClearFormComboRelation()
        If (cboDynamicModule.SelectedItem IsNot Nothing) Then
            Me.ReloadComboRelation(cboDynamicModule.SelectedItem.Text)
            Me.ReloadTableName(cboDynamicModule.SelectedItem.Text)
            Dim database As String = Convert.ToString(Session("database"))
            ComboRelationService.Instance().RunCreateComboRelationIfNotExits(database, cboDynamicModule.SelectedItem.Text)
        End If
    End Sub

    Protected Sub cboTableName_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        If (cboDynamicModule.SelectedItem IsNot Nothing AndAlso cboTableName.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim sTableName As String = cboTableName.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ColumnNameStore)
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ParentColumnNameStore)
        End If
    End Sub

    Protected Sub GPComboRelation_RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim GUID1 As String = e.ExtraParams("GUID1")
        If GUID1 Is Nothing Then
            e.Success = False
            Exit Sub
        End If
        Try
            If Not String.IsNullOrEmpty(GUID1) Then
                ComboRelationService.GetInstance().DeleteById(database, DDDefName, GUID1)
                Me.ReloadComboRelation(cboDynamicModule.SelectedItem.Text)
                ClearFormComboRelation()
            End If
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
    End Sub

    Protected Sub GBComboRelaion_RowSelect(sender As Object, e As DirectEventArgs)
        Dim sTableName As String = e.ExtraParams("sTableName")
        Dim sFieldName As String = e.ExtraParams("sFieldName")
        Dim sParentFieldName As String = e.ExtraParams("sParentFieldName")
        If (cboDynamicModule.SelectedItem IsNot Nothing) Then
            ReloadTableName(cboDynamicModule.SelectedItem.Text)
        End If

        If (String.IsNullOrEmpty(sTableName) = False AndAlso cboDynamicModule.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ColumnNameStore)
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName), ParentColumnNameStore)
            Me.cboFieldName.SetValue(sFieldName)
            Me.cboParentFieldName.SetValue(sParentFieldName)
        End If
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Me.Save(ImmapUtil.SaveType.INSERT, DDDefName)
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Me.Save(ImmapUtil.SaveType.UPDATE, DDDefName)
        End If
    End Sub

    Protected Sub CheckSameColumn(sender As System.Object, e As Ext.Net.RemoteValidationEventArgs)
        If cboFieldName.SelectedItem IsNot Nothing AndAlso cboParentFieldName IsNot Nothing Then
            If (cboFieldName.SelectedItem.Text.Equals(cboParentFieldName.SelectedItem.Text)) Then
                e.Success = False
            Else
                e.Success = True
            End If
        Else
            e.Success = True
        End If
    End Sub

    Protected Sub Save(ByVal saveType As ImmapUtil.SaveType, ByVal DDDefName As String)
        Dim GUID1 As String
        Dim msg As New MessageBox()
        Dim database As String = Convert.ToString(Session("database"))
        If cboFieldName.SelectedItem IsNot Nothing AndAlso cboParentFieldName IsNot Nothing Then
            If (cboFieldName.SelectedItem.Text.Equals(cboParentFieldName.SelectedItem.Text)) Then
                msg.Show(New MessageBoxConfig() With {
                     .Title = "Warning",
                     .Message = "Please do not select the same column",
                     .Buttons = MessageBox.Button.OK,
                     .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmComboRelation.ClientID
               })
                Exit Sub
            End If
        End If
        Select Case saveType
            Case ImmapUtil.SaveType.INSERT
                GUID1 = ImmapUtil.NewGUid()
                ComboRelationService.GetInstance().Insert(database,
                                                     DDDefName,
                                                     GUID1,
                                                     cboTableName.SelectedItem.Text,
                                                     cboFieldName.SelectedItem.Text,
                                                     cboParentFieldName.SelectedItem.Text)
            Case ImmapUtil.SaveType.UPDATE
                GUID1 = txtGUID1.Text
                If String.IsNullOrWhiteSpace(GUID1) Then
                    msg.Show(New MessageBoxConfig() With {
                        .Title = "Warning",
                        .Message = "Please select Combo Relation",
                        .Buttons = MessageBox.Button.OK,
                        .Icon = MessageBox.Icon.WARNING,
                    .AnimEl = Me.frmComboRelation.ClientID
                  })
                    Exit Sub
                End If
                ComboRelationService.GetInstance().Update(database,
                                                     DDDefName,
                                                     GUID1,
                                                     cboTableName.SelectedItem.Text,
                                                     cboFieldName.SelectedItem.Text,
                                                     cboParentFieldName.SelectedItem.Text)
        End Select
        ClearFormComboRelation()
        ClearTableNameStore()
        ClearColumnNameStore()
        ReloadComboRelation(DDDefName)
        Dim sm As RowSelectionModel = TryCast(Me.gpComboRelation.SelectionModel.Primary, RowSelectionModel)
        sm.ClearSelections()
        gpComboRelation.Call("clearMemory")
    End Sub
    Protected Sub ReloadDynamicDataManager()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataManagerService.GetInstance().GetDDDefNameAll(database, DynamicDataManagerStore)
    End Sub

    Protected Sub ReloadComboRelation(ByVal DDDefName As String)
        If Not (String.IsNullOrEmpty(DDDefName)) Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataDefService.GetInstance().RunPrepareDynamicDatabaseScript(database, DDDefName)
            ComboRelationService.GetInstance().FindAll(database, DDDefName, ComboRelationStore)
        End If
    End Sub

    Protected Sub ReloadTableName(ByVal DDDefName As String)
        If String.IsNullOrEmpty(DDDefName) = False Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataManagerService.GetInstance().GetTableNameByDDDefName(database, DDDefName, TableNameStore)
        End If
    End Sub

    Protected Sub ReloadColumnName(ByVal tableName As String, ByRef store As Ext.Net.Store)
        If String.IsNullOrEmpty(tableName) = False Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataManagerService.GetInstance().GetAllColumnByTableName(database, tableName, store)
        End If
    End Sub

    Protected Sub ClearTableNameStore()
        TableNameStore.DataSource = New DataTable()
        TableNameStore.DataBind()
    End Sub

    Protected Sub ClearColumnNameStore()
        ColumnNameStore.DataSource = New DataTable()
        ColumnNameStore.DataBind()
    End Sub

    Protected Sub ClearParentNameStore()
        ParentColumnNameStore.DataSource = New DataTable()
        ParentColumnNameStore.DataBind()
    End Sub

    Protected Sub ClearFormComboRelation()
        frmComboRelation.Reset()
    End Sub
End Class
