Imports Immap.Service
Imports Ext.Net
Imports System.Data
Partial Class Validation
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Validation"
            ReloadDynamicDataManager()
        End If
    End Sub

    Protected Sub DynamicDataManagerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadDynamicDataManager()
    End Sub

    Protected Sub ColumnNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        If (cboDynamicModule.SelectedItem IsNot Nothing AndAlso cboDataEntryTableName.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim sTableName As String = cboDataEntryTableName.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName))
            cboDataEntryFieldName.SetValue("")
        End If
    End Sub

    Protected Sub TableNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            ReloadTableName(cboDynamicModule.SelectedItem.Text)
        End If
    End Sub

    Protected Sub cboDynamicModule_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ClearFormValidation()
        If (cboDynamicModule.SelectedItem IsNot Nothing) Then
            ReloadValidation(cboDynamicModule.SelectedItem.Text)
            ReloadTableName(cboDynamicModule.SelectedItem.Text)
        End If
    End Sub

    Protected Sub cboTableName_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        If (cboDynamicModule.SelectedItem IsNot Nothing AndAlso cboDataEntryTableName.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Dim sTableName As String = cboDataEntryTableName.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sTableName))
            cboDataEntryFieldName.SetValue("")
        End If
    End Sub

    Protected Sub GPValidation_RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim GUID1 As String = e.ExtraParams("GUID1")
        If GUID1 Is Nothing Then
            e.Success = False
            Exit Sub
        End If
        Try
            If Not String.IsNullOrEmpty(GUID1) Then
                ValidationService.GetInstance().DeleteById(database, DDDefName, GUID1)
                ReloadValidation(cboDynamicModule.SelectedItem.Text)
                ClearFormValidation()
            End If
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
    End Sub

    Protected Sub GBValidation_RowSelect(sender As Object, e As DirectEventArgs)
        Dim sDataEntryTableName As String = e.ExtraParams("sDataEntryTableName")
        Dim sDataEntryFieldName As String = e.ExtraParams("sDataEntryFieldName")

        If (cboDynamicModule.SelectedItem IsNot Nothing) Then
            ReloadTableName(cboDynamicModule.SelectedItem.Text)
        End If

        If (String.IsNullOrEmpty(sDataEntryTableName) = False AndAlso cboDynamicModule.SelectedItem IsNot Nothing) Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
            Me.ReloadColumnName(String.Format("dd_{0}_{1}", DDDefName, sDataEntryTableName))
            Me.cboDataEntryFieldName.SetValue(sDataEntryFieldName)
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

    Protected Sub Save(ByVal saveType As ImmapUtil.SaveType, ByVal DDDefName As String)
        Dim GUID1 As String
        Dim msg As New MessageBox()
        Dim database As String = Convert.ToString(Session("database"))
        Select Case saveType
            Case ImmapUtil.SaveType.INSERT
                GUID1 = ImmapUtil.NewGUid()
                ValidationService.GetInstance().Insert(database,
                                                     DDDefName,
                                                     GUID1,
                                                     cboDataEntryTableName.SelectedItem.Text,
                                                     cboDataEntryFieldName.SelectedItem.Text,
                                                     If(chkRequired.Checked = True, "1", "0"),
                                                     txtEditMask.Text,
                                                     txtValidation.Text)
            Case ImmapUtil.SaveType.UPDATE
                GUID1 = txtGUID1.Text
                If String.IsNullOrWhiteSpace(GUID1) Then
                    msg.Show(New MessageBoxConfig() With {
                        .Title = "Warning",
                        .Message = "Please select Validatione",
                        .Buttons = MessageBox.Button.OK,
                        .Icon = MessageBox.Icon.WARNING,
                    .AnimEl = Me.frmValidation.ClientID
                  })
                    Exit Sub
                End If
                ValidationService.GetInstance().Update(database,
                                                     DDDefName,
                                                     GUID1,
                                                     cboDataEntryTableName.SelectedItem.Text,
                                                     cboDataEntryFieldName.SelectedItem.Text,
                                                     If(chkRequired.Checked = True, "1", "0"),
                                                     txtEditMask.Text,
                                                     txtValidation.Text)
        End Select
        ClearFormValidation()
        ClearTableNameStore()
        ClearColumnNameStore()
        ReloadValidation(DDDefName)
        Dim sm As RowSelectionModel = TryCast(Me.gpValidation.SelectionModel.Primary, RowSelectionModel)
        sm.ClearSelections()
        gpValidation.Call("clearMemory")
    End Sub
    Protected Sub ReloadDynamicDataManager()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataManagerService.GetInstance().GetDDDefNameAll(database, DynamicDataManagerStore)
    End Sub

    Protected Sub ReloadValidation(ByVal DDDefName As String)
        If Not (String.IsNullOrEmpty(DDDefName)) Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataDefService.GetInstance().RunPrepareDynamicDatabaseScript(database, DDDefName)
            ValidationService.GetInstance().FindAll(database, DDDefName, ValidationStore)
        End If
    End Sub

    Protected Sub ReloadTableName(ByVal DDDefName As String)
        If String.IsNullOrEmpty(DDDefName) = False Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataManagerService.GetInstance().GetTableNameByDDDefName(database, DDDefName, TableNameStore)
        End If
    End Sub

    Protected Sub ReloadColumnName(ByVal tableName As String)
        If String.IsNullOrEmpty(tableName) = False Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataManagerService.GetInstance().GetAllColumnByTableName(database, tableName, ColumnNameStore)
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

    Protected Sub ClearFormValidation()
        frmValidation.Reset()
    End Sub
End Class
