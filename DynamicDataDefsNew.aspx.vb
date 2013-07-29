Imports Ext.Net
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Service
Imports Immap.Model
Partial Class DynamicDataDefs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Dynamic Modules"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ClearAndReloadDynamicDataDefs()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ClearAndReloadDynamicDataDefs()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim DDDefName As String = e.ExtraParams("DDDefName")
        If ID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of user is empty"
            Exit Sub
        End If
        Try
            DynamicDataDefService.GetInstance().DeleteByName(database, userGroup, DDDefName)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmDynamicDataDefs.Reset()
        ReloadDynamicDataDefs()
    End Sub

    Protected Sub DynamicDataDefsStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadDynamicDataDefs()
    End Sub

    Protected Sub StoreTableName_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        'LoadTableAccess()
    End Sub

    Protected Sub StoreTableColumnName_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ' LoadTableLEFiled()
    End Sub

    Protected Sub RowSelect(sender As Object, e As DirectEventArgs)
        Dim DDefName As String = e.ExtraParams("DDefName")
        Dim AccessRights As String = e.ExtraParams("AccessRights")
        Dim LockFields As String = e.ExtraParams("LockFields")
        Dim ExcludeFileds As String = e.ExtraParams("ExcludeFileds")
        LoadTableAccess(DDefName, AccessRights)
        LoadTableLEFiled(DDefName, LockFields, ExcludeFileds)
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub ReloadDynamicDataDefs()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataDefService.GetInstance().FindAll(database, cboUserGroups.SelectedItem.Text, DynamicDataDefsStore)
    End Sub

    Public Sub LoadTableAccess(ByVal defName As String, ByVal AccessRights As String)
        'Dim CommandText As String = Nothing
        Dim CommandText As New System.Text.StringBuilder()
        CommandText.Append("SELECT REPLACE([TABLE_NAME],'dd_" & defName & "_','')  AS TBName")
        CommandText.Append(" FROM information_schema.tables ")
        CommandText.Append(" WHERE [TABLE_NAME] NOT LIKE '%_GEO%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_WKB%' ")
        CommandText.Append(" AND [TABLE_NAME] LIKE '%dd_" & defName & "%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_Specification%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_ChartSettings%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_SynchHistory%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_SynchHistoryOverview%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_NearbyFeatures%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_ComboRelations%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_Validation%'")
        CommandText.Append(" AND [TABLE_NAME] NOT LIKE '%_Queries%'")
        CommandText.Append(" AND information_schema.tables.[TABLE_TYPE]='BASE TABLE'")
        CommandText.Append(" ORDER by TBName")
        Dim database As String = Convert.ToString(Session("database"))
        Dim TableModels As New List(Of TableModel)
        Dim TbName As String
        Dim ds As DataSet = SQLHelper.ExecuteDataset(SQLHelper.SetConnectionString(database), CommandText.ToString())
        Dim keyValue As New Dictionary(Of String, String)
        If ds.Tables.Count > 0 Then
            ds.Tables(0).TableName = "TBName"
            Dim cnt As Integer = ds.Tables(0).Rows.Count() - 1
            For i As Integer = 0 To cnt
                TbName = ds.Tables(0).Rows(i)(0).ToString()
                TableModels.Add(New TableModel(TbName))
                keyValue.Add(TbName, i.ToString())
            Next i
            If String.IsNullOrEmpty(AccessRights.Trim()) = False Then
                Dim tableConfig() As String
                Dim config() As String
                Dim indexOut As String = Nothing
                Dim IsCanGet As Boolean = False
                tableConfig = Split(AccessRights, ";")
                For j = LBound(tableConfig) To UBound(tableConfig)
                    If String.IsNullOrEmpty(tableConfig(j)) = False Then
                        'Dim tm As New TableModel()
                        config = Split(tableConfig(j), ",")
                        IsCanGet = False
                        For IndexIn = LBound(config) To UBound(config) 'TableName
                            If IsCanGet = False Then
                                If CanGetKeyValue(keyValue, config, IndexIn, indexOut) Then
                                    IsCanGet = True
                                End If
                            Else
                                For l As Integer = 0 To config(IndexIn).Length - 1 'Config
                                    Select Case config(IndexIn)(l).ToString().ToUpper()
                                        Case "R"
                                            TableModels(indexOut).Read = True
                                        Case "A"
                                            TableModels(indexOut).Add = True
                                            'tm.Add = True
                                        Case "E"
                                            TableModels(indexOut).Edit = True
                                            'tm.Edit = True
                                        Case "D"
                                            TableModels(indexOut).Delete = True
                                            'tm.Delete = True
                                    End Select
                                Next l
                                'End If
                            End If
                        Next IndexIn
                    End If
                Next j
            End If
            StoreTableName.DataSource = TableModels
        Else
            StoreTableName.DataSource = New DataTable()
        End If
        StoreTableName.DataBind()
        CommandText.Clear()
    End Sub

    Public Sub LoadTableLEFiled(ByVal defName As String,
                                 ByVal LockField As String,
                                 ByVal ExcludeFiled As String)

        Dim CommandText As String = Nothing
        CommandText = "SELECT REPLACE([TABLE_NAME],'dd_" & defName & "_','')  AS TName, Column_Name AS CName "
        CommandText &= " FROM information_schema.COLUMNS "
        'CommandText &= " WHERE TABLE_NAME LIKE '%_FEA%' AND"
        CommandText &= " WHERE TABLE_NAME LIKE '%dd_" & defName & "%' "
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_GEO%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_WKB%' "
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_Specification%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_ChartSettings%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_SynchHistory%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_Validation%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_SynchHistoryOverview%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_NearbyFeatures%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_Queries%'"
        CommandText &= " AND [TABLE_NAME] NOT LIKE '%_ComboRelations%'"
        CommandText &= " AND (SELECT TABLE_TYPE FROM information_schema.tables WHERE information_schema.tables.TABLE_NAME = information_schema.COLUMNS.TABLE_NAME) = 'BASE TABLE'"
        CommandText &= " ORDER by TName"

        Dim database As String = Convert.ToString(Session("database"))
        Dim TableColumnModels As New List(Of TableColumnModel)
        Dim TName, CName As String
        Dim ds As DataSet = SQLHelper.ExecuteDataset(SQLHelper.SetConnectionString(database), CommandText)
        Dim keyValue As New Dictionary(Of String, String)
        Dim tmpTName As String = Nothing
        Dim isCanGet As Boolean = False
        Dim indexOut As String = Nothing
        If ds.Tables.Count > 0 Then
            ds.Tables(0).TableName = "TableLEFiled"
            Dim cnt As Integer = ds.Tables(0).Rows.Count() - 1
            For i As Integer = 0 To cnt
                TName = ds.Tables(0).Rows(i)(0).ToString()
                CName = ds.Tables(0).Rows(i)(1).ToString()
                TableColumnModels.Add(New TableColumnModel(TName, CName))
                keyValue.Add(TName & CName, i.ToString())
            Next i
            If String.IsNullOrEmpty(LockField.Trim()) = False Then
                Dim tableConfig() As String
                Dim config() As String
                tableConfig = Split(LockField, ";")
                For j = LBound(tableConfig) To UBound(tableConfig)
                    If String.IsNullOrEmpty(tableConfig(j)) = False Then
                        config = Split(tableConfig(j), ",")
                        For IndexIn = LBound(config) To UBound(config) 'TableName
                            If IndexIn = 0 Then
                                tmpTName = config(IndexIn).ToString()
                            Else
                                For l As Integer = 0 To config(IndexIn).Length - 1 'Config
                                    If CanGetKeyValue(keyValue, config, IndexIn, indexOut, tmpTName) Then
                                        isCanGet = True
                                    End If
                                    If (isCanGet = True) AndAlso (indexOut >= 0) Then
                                        TableColumnModels(indexOut).LockFiled = True
                                    End If
                                Next l
                            End If
                        Next IndexIn
                    End If
                Next j
            End If
            If String.IsNullOrEmpty(ExcludeFiled.Trim()) = False Then
                Dim tableConfig() As String
                Dim config() As String
                'Dim index As Integer
                tableConfig = Split(ExcludeFiled, ";")
                For j = LBound(tableConfig) To UBound(tableConfig)
                    If String.IsNullOrEmpty(tableConfig(j)) = False Then
                        config = Split(tableConfig(j), ",")
                        isCanGet = False
                        For IndexIn = LBound(config) To UBound(config) 'TableName
                            If IndexIn = 0 Then
                                tmpTName = config(IndexIn).ToString()
                            Else
                                For l As Integer = 0 To config(IndexIn).Length - 1 'Config
                                    If CanGetKeyValue(keyValue, config, IndexIn, indexOut, tmpTName) Then
                                        isCanGet = True
                                    End If
                                    If (isCanGet = True) AndAlso (indexOut >= 0) Then
                                        TableColumnModels(indexOut).ExcludeFiled = True
                                    End If
                                Next l
                            End If
                        Next IndexIn
                    End If
                Next j
            End If
            StoreTableColumnName.DataSource = TableColumnModels
        Else
            StoreTableColumnName.DataSource = New DataTable()
        End If
        StoreTableColumnName.DataBind()
    End Sub

    Public Function CanGetKeyValue(ByVal keyValue As Dictionary(Of String, String),
                                 config() As String,
                                 IndexIn As Integer,
                                 ByRef IndexOut As String,
                                 Optional tmpTName As String = Nothing) As Boolean
        IndexOut = Nothing
        Try
            If String.IsNullOrWhiteSpace(tmpTName) = True Then
                IndexOut = CInt(keyValue.Item(config(IndexIn).ToString()))
            Else
                IndexOut = CInt(keyValue.Item(tmpTName & config(IndexIn).ToString()))
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    <DirectMethod()> Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        Dim jsonValues As String = e.ExtraParams("values")
        Dim gpTableColumnName As String = e.ExtraParams("gpTableColumnName")
        InsertOrUpdate("INSERT", ReadRecords(jsonValues),
                       ReadTableColumnNames(gpTableColumnName, "LockFiled"),
                       ReadTableColumnNames(gpTableColumnName, "ExcludeFiled"))
    End Sub
    <DirectMethod()> Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        Dim jsonValues As String = e.ExtraParams("values")
        Dim gpTableColumnName As String = e.ExtraParams("gpTableColumnName")
        InsertOrUpdate("UPDATE", ReadRecords(jsonValues),
                       ReadTableColumnNames(gpTableColumnName, "LockFiled"),
                       ReadTableColumnNames(gpTableColumnName, "ExcludeFiled"))
    End Sub

    Protected Sub ClearAccessRightTable_Click(sender As Object, e As DirectEventArgs)
        StoreTableName.DataSource = New DataTable()
        StoreTableName.DataBind()
        StoreTableColumnName.DataSource = New DataTable()
        StoreTableColumnName.DataBind()
    End Sub
    Protected Sub SelectGroup_Click(sender As Object, e As DirectEventArgs)
        Dim records As List(Of Dictionary(Of String, String)) = JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(CStr(e.ExtraParams("values")))
        Dim gpTableColumnNameRowsValues As String = e.ExtraParams("gpTableColumnNameRowsValues")
        Dim groupId As String = e.ExtraParams("groupId")
        LoadTableLEFiled(txtDDDefName.Text, txtaLockedFields.Text, txtaExcludedFields.Text) ', groupId)
    End Sub

    Protected Function ReadTableColumnNames(ByVal jsonValues As String, ByVal file As String) As String
        Dim records As List(Of Dictionary(Of String, String)) = JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValues)
        Dim tbName As String = Nothing
        Dim oldTableName As String = tbName
        Dim CName As String
        Dim isCheck As Boolean = False
        Dim fileNames As String = ""
        Dim result As String = Nothing
        For Each record In records
            tbName = record("TName")
            tbName = tbName.Replace("dd_" & txtDDDefName.Text.Trim() & "_", "")
            If oldTableName = "" Then
                oldTableName = tbName
            ElseIf Not (tbName.Equals(oldTableName)) = True Then
                If fileNames.Length > 2 Then
                    result = result & String.Format("{0},{1};", oldTableName, fileNames)
                End If
                fileNames = ""
                oldTableName = tbName
            End If
            CName = record("CName")
            isCheck = CBool(record(file))
            If isCheck Then
                If fileNames.Length > 2 Then
                    fileNames = String.Format("{0},{1}", fileNames, CName)
                Else
                    fileNames = CName
                End If
            End If
        Next
        Return result
    End Function

    Protected Function ReadRecords(ByVal jsonValues As String) As String
        Dim records As List(Of Dictionary(Of String, String)) = JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValues)
        Dim tbName As String
        Dim bRead As Boolean
        Dim bAdd As Boolean
        Dim bEdit As Boolean
        Dim bDelete As Boolean
        Dim sAccessRight As String = Nothing
        Dim rights As String = Nothing
        Dim result As String = Nothing
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        For Each record In records
            tbName = record("TBName")
            tbName = tbName.Replace("dd_" & txtDDDefName.Text.Trim() & "_", "")
            bRead = CBool(record("Read"))
            bAdd = CBool(record("Add"))
            bEdit = CBool(record("Edit"))
            bDelete = CBool(record("Delete"))
            If bRead = True Then
                rights &= "r"
            End If
            If bAdd = True Then
                rights &= "a"
            End If
            If bEdit = True Then
                rights &= "e"
            End If
            If bDelete = True Then
                rights &= "d"
            End If
            If String.IsNullOrEmpty(rights) = False Then
                result = result & String.Format("{0},{1};", tbName, rights)
            End If
            rights = Nothing
        Next
        Return result
    End Function

    Protected Sub InsertOrUpdate(ByVal command As String, ByVal accessRights As String, ByVal LockFiled As String, ByVal ExcludeFiled As String)

        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        If String.IsNullOrEmpty(userGroup) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please select user group",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmDynamicDataDefs.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtDDDefName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter DDDef Name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmDynamicDataDefs.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtDescription.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter descriptioname",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmDynamicDataDefs.ClientID
          })
            Exit Sub
        End If

        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = DynamicDataDefService.GetInstance().CheckDuplicateName(database, userGroup, txtDDDefName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This dynamic data def name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmDynamicDataDefs.ClientID
                })
                Exit Sub
            End If
            DynamicDataDefService.GetInstance().Insert(database,
                                        userGroup,
                                        txtDDDefName.Text.Replace(" ", ""),
                                        txtDescription.Text,
                                        accessRights,
                                        txtaConnectionString.Text,
                                        If(chkSynch.Checked = True, "1", "0"),
                                        If(chkEnableDataEntry.Checked = True, "1", "0"),
                                        If(chkEnableReporting.Checked = True, "1", "0"),
                                        LockFiled,
                                        ExcludeFiled)
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = DynamicDataDefService.GetInstance().CheckDuplicateName(database, userGroup, txtDDDefName.Text.Trim(), txtHiddenDDDefName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This dynamic data def name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmDynamicDataDefs.ClientID
                })
                Exit Sub
            End If
            DynamicDataDefService.GetInstance().Update(database,
                                              userGroup,
                                              txtDDDefName.Text.Replace(" ", ""),
                                              txtDescription.Text,
                                              accessRights,
                                              txtaConnectionString.Text,
                                              If(chkSynch.Checked = True, "1", "0"),
                                              If(chkEnableDataEntry.Checked = True, "1", "0"),
                                              If(chkEnableReporting.Checked = True, "1", "0"),
                                              LockFiled,
                                              ExcludeFiled,
                                              txtHiddenDDDefName.Text)

        End If
        DynamicDataDefService.GetInstance().RunPrepareDynamicDatabaseScript(database, txtDDDefName.Text)
        ClearAndReloadDynamicDataDefs()
        Dim sm As RowSelectionModel = TryCast(Me.gpDynamicDataDefs.SelectionModel.Primary, RowSelectionModel)
        sm.ClearSelections()
        gpDynamicDataDefs.Call("clearMemory")
        ' LoadTableAccess(txtDDDefName.Text, accessRights)
    End Sub

    Public Sub ClearAndReloadDynamicDataDefs()
        frmDynamicDataDefs.Reset()
        StoreTableName.DataSource = New DataTable()
        StoreTableName.DataBind()
        StoreTableColumnName.DataSource = New DataTable()
        StoreTableColumnName.DataBind()
        ReloadDynamicDataDefs()
    End Sub
End Class
