Imports Ext.Net
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports Immap.Service
Imports System.Collections.Concurrent

Partial Class DynamicDataManager
    Inherits System.Web.UI.Page
    Private Const masterTable As String = "mastertable"
    Private Const linkTable As String = "link"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            'Session("TITLE") = "Dynamic Module Manager"
            Session("TITLE") = "Formatting"
            ReloadDynamicDataManager()
        End If
    End Sub

    Protected Sub DynamicDataManagerStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ClearFormSpecfication()
        ClearSpecificationStore()
        ReloadDynamicDataManager()
    End Sub

    Protected Sub TableNameStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadTableName()
    End Sub

    Protected Sub SpecificationStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ClearFormSpecfication()
        ReloadTableSpecfication()
    End Sub

    Protected Sub cboTableName_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim sTableName As String = cboTableName.SelectedItem.Text
        If Not (sTableName.Trim().Equals(txtHiddenDataFileds.Text)) Then
            Dim sFullTableName As String = String.Format("dd_{0}_{1}", DDDefName, sTableName)
            Dim dtTmp As DataTable = DynamicDataManagerService.GetInstance().GetDataFiledsByTableName(database, sFullTableName)
            DynamicDataManagerService.GetInstance().MapDataTableOfDataFiledToStore(dtTmp, "", StoreSpecificationDataFileds)
            txtHiddenDataFileds.Text = sTableName
            Me.CheckMasterOrLink(sTableName)
        End If
    End Sub

    Protected Sub cboDynamicModule_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ClearFormSpecfication()
        ReloadTableSpecfication()
        ReloadTableName()
    End Sub

    Public Sub ReloadTableSpecfication()
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Value
        If Not String.IsNullOrEmpty(DDDefName) Then
            Dim database As String = Convert.ToString(Session("database"))
            DynamicDataManagerService.GetInstance().GetTableSpecificationMasterAndLinkByTableName(database, DDDefName, SpecificationStore)
            DynamicDataManagerService.GetInstance().GetTableSpecificationDynamicDataByTableName(database, DDDefName, SpecificationDynamicDataStore)
        End If
    End Sub


    Protected Sub RowSpecificationDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim GUID1 As String = e.ExtraParams("GUID1")

        If GUID1 Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of user is empty"
            Exit Sub
        End If
        Try
            DynamicDataManagerService.GetInstance().DeleteByGUID1(database, DDDefName, GUID1)
            e.Success = True
            ReloadTableSpecfication()
            ClearFormSpecfication()
        Catch ex As Exception
            e.Success = False
        End Try
    End Sub

    Protected Sub RowSelectSpecification(sender As Object, e As DirectEventArgs)
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        Dim sTableName As String = e.ExtraParams("sTableName")
        Dim sFullTableName As String = String.Format("dd_{0}_{1}", DDDefName, sTableName)
        Dim sDataEntryFields As String = e.ExtraParams("sDataEntryFields")
        Dim islRankDisabled As Boolean = CBool(e.ExtraParams("islRankDisabled").ToString())
        Me.ReloadTableName()
        Me.cboTableName.SetValue(sTableName)
        Me.CheckMasterOrLink(sTableName)
        Dim database As String = Convert.ToString(Session("database"))
        Dim dtTmp As DataTable = DynamicDataManagerService.GetInstance().GetDataFiledsByTableName(database, sFullTableName)
        DynamicDataManagerService.GetInstance().MapDataTableOfDataFiledToStore(dtTmp, sDataEntryFields, StoreSpecificationDataFileds)
        nbflRank.Disabled = islRankDisabled
        nbflRank.Hidden = islRankDisabled
        If islRankDisabled = True Then nbflRank.SetValue(9999999)
    End Sub
    Protected Sub CheckMasterOrLink(ByVal sTableName As String)
        chkIsMaster.Value = False
        chkIsLinkedTable.Value = False
        If (sTableName.ToLower.Trim().Equals(masterTable)) Then
            chkIsMaster.Checked = True
            chkIsMaster.Value = True
        ElseIf (sTableName.ToLower.Trim().StartsWith(linkTable)) Then
            chkIsLinkedTable.Checked = True
            chkIsLinkedTable.Value = True
        End If
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        Dim jsonDataFileds As String = e.ExtraParams("JSONDataFileds")
        Dim JSONLRank As String = e.ExtraParams("JSONLRank")
        DynamicDataManagerSave(ImmapUtil.SaveType.INSERT, jsonDataFileds)
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        Dim jsonDataFileds As String = e.ExtraParams("JSONDataFileds")
        Dim jsonLRank As String = e.ExtraParams("JSONLRank")
        DynamicDataManagerSave(ImmapUtil.SaveType.UPDATE, jsonDataFileds)
        'CheckRank(jsonLRank)
    End Sub
    Protected Sub CheckRank(ByVal jsonValue As String)
        'Dim SpecificationRecords As List(Of Dictionary(Of String, String)) = Ext.Net.JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValue)
        'Dim rank, sTableName As String
        'For Each record In SpecificationRecords
        '    rank = TryCast(record("lRank"), String)
        '    sTableName = TryCast(record("sTableName"), String)
        '    If (String.Equals(sTableName, cboTableName.SelectedItem.Text)) Then
        '        record("lRank") = nbflRank.Text
        '    End If
        'Next
    End Sub


    Protected Sub DynamicDataManagerSave(ByVal saveType As ImmapUtil.SaveType, ByVal jsonDataFileds As String)
        Dim msg As New MessageBox()
        Dim sGUID As String = ""
        Dim database As String = Convert.ToString(Session("database"))
        Dim DDDefName As String = cboDynamicModule.SelectedItem.Text
        If String.IsNullOrWhiteSpace(cboDynamicModule.SelectedItem.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please select a dynamic module",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmSpecification.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(cboTableName.SelectedItem.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please select a table name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmSpecification.ClientID
          })
            Exit Sub
        End If
        Dim fullTabelDataFiledName As String = String.Format("dd_{0}_{1}", DDDefName, cboTableName.SelectedItem.Text)
        Me.CheckMasterOrLink(txtHiddenDataFileds.Text)
        Dim dataFiledDict As ConcurrentDictionary(Of String, String)
        If (chkIsMaster.Checked = False) AndAlso (chkIsLinkedTable.Checked = False) Then
            nbflRank.SetValue(9999999)
            nbflRank.Text = 9999999
        End If
        If (saveType = ImmapUtil.SaveType.INSERT) Then
            sGUID = ImmapUtil.NewGUid()
            dataFiledDict = DynamicDataManagerService.GetInstance().GetDataFiledSettingFromJSON(sGUID, jsonDataFileds)
            DynamicDataManagerService.GetInstance().Insert(database,
                                                           sGUID,
                                                           nbflRank.Text,
                                                           cboTableName.SelectedItem.Text,
                                                           txtCaption.Text,
                                                           txtDescription.Text,
                                                           nbfFontSize.Text,
                                                           If(chkIsMaster.Checked = True, 1, 0),
                                                           If(chkIsLinkedTable.Checked = True, 1, 0),
                                                           txtaGridQuery.Text,
                                                           txtaGridQueryMSSQL.Text,
                                                           DDDefName,
                                                           dataFiledDict)
            ClearTableNameStore()
            ClearFormSpecfication()
            ClearSpecificationDataFiledsStore()
            ReloadTableSpecfication()
        ElseIf (saveType = ImmapUtil.SaveType.UPDATE) Then
            sGUID = txtGUID1.Text
            dataFiledDict = DynamicDataManagerService.GetInstance().GetDataFiledSettingFromJSON(sGUID, jsonDataFileds)
            DynamicDataManagerService.GetInstance().Update(database,
                                                           sGUID,
                                                           nbflRank.Text,
                                                           cboTableName.SelectedItem.Text,
                                                           txtCaption.Text,
                                                           txtDescription.Text,
                                                           nbfFontSize.Text,
                                                           If(chkIsMaster.Checked = True, 1, 0),
                                                           If(chkIsLinkedTable.Checked = True, 1, 0),
                                                           txtaGridQuery.Text,
                                                           txtaGridQueryMSSQL.Text,
                                                           DDDefName,
                                                           dataFiledDict)
            ClearTableNameStore()
            ClearFormSpecfication()
            ClearSpecificationDataFiledsStore()
            ReloadTableSpecfication()
        End If

    End Sub

    Protected Sub btnfrmSpecificationReset_Click(sender As Object, e As DirectEventArgs)
        ClearSpecificationDataFiledsStore()
    End Sub

    Protected Sub ReloadDynamicDataManager()
        Dim database As String = Convert.ToString(Session("database"))
        DynamicDataManagerService.GetInstance().GetDDDefNameAll(database, DynamicDataManagerStore)
    End Sub

    Protected Sub ReloadTableName()
        If cboDynamicModule.SelectedItem IsNot Nothing Then
            Dim DDDefName As String = cboDynamicModule.SelectedItem.Value
            If String.IsNullOrEmpty(DDDefName) = False Then
                Dim database As String = Convert.ToString(Session("database"))
                DynamicDataManagerService.GetInstance().GetTableNameByDDDefName(database, DDDefName, TableNameStore)
            End If
        End If
    End Sub

    Protected Sub ClearSpecificationDataFiledsStore()
        StoreSpecificationDataFileds.DataSource = New DataTable()
        StoreSpecificationDataFileds.DataBind()
    End Sub
    Protected Sub ClearSpecificationStore()
        SpecificationStore.DataSource = New DataTable()
        SpecificationStore.DataBind()
        SpecificationDynamicDataStore.DataSource = New DataTable
        SpecificationDynamicDataStore.DataBind()
    End Sub

    Protected Sub ClearTableNameStore()
        TableNameStore.DataSource = New DataTable()
        TableNameStore.DataBind()
    End Sub

    Protected Sub ClearFormSpecfication()
        frmSpecification.Reset()
        nbfFontSize.Text = "15"
    End Sub
End Class
