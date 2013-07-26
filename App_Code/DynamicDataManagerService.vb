Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Concurrent
Imports Immap.Model
Imports Immap.Service
Imports System.Collections.Generic

Namespace Immap
    Namespace Service
        Public Class DynamicDataManagerService
            Private Shared ReadOnly _instance As New DynamicDataManagerService()

            Shared Sub New()
            End Sub

            Private Property queryGetDDDefName As String
            Private Property queryGetDescription As String
            Private Property queryGetDataFileds As String
            Private Property queryGetColumnName As String

            Private Sub New()
                queryGetDDDefName = "SELECT DISTINCT LEFT(REPLACE(Table_Name, 'dd_',''), -1 + CHARINDEX( '_', REPLACE(Table_Name, 'dd_',''))) AS [DDDefName]"
                queryGetDDDefName &= " FROM INFORMATION_SCHEMA.TABLES"
                queryGetDDDefName &= " WHERE Table_Name LIKE @DDDefName"

                queryGetDescription = "SELECT OBJECT_NAME(c.object_id) as [TABLE_NAME], "
                queryGetDescription &= " [Column Name] = c.name, [Description] = CAST(ex.value AS varchar(255)) "
                queryGetDescription &= " FROM sys.columns c LEFT OUTER JOIN sys.extended_properties ex ON ex.major_id = c.object_id "
                queryGetDescription &= " AND ex.minor_id = c.column_id AND ex.name = 'MS_Description' "
                queryGetDescription &= " WHERE OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0 "
                queryGetDescription &= " AND OBJECT_NAME(c.object_id) LIKE @DDDefName "
                queryGetDescription &= " ORDER BY OBJECT_NAME(c.object_id), c.column_id"


                queryGetDataFileds = "SELECT OBJECT_NAME(c.object_id) as [TABLE_NAME],"
                queryGetDataFileds &= " [Column Name] = c.name, [Description] = CAST(ex.value AS varchar(255))"
                queryGetDataFileds &= " FROM sys.columns c"
                queryGetDataFileds &= " LEFT OUTER JOIN sys.extended_properties ex ON ex.major_id = c.object_id"
                queryGetDataFileds &= " AND ex.minor_id = c.column_id AND ex.name = 'MS_Description'"
                queryGetDataFileds &= " WHERE OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0"
                queryGetDataFileds &= " AND OBJECT_NAME(c.object_id) like @DDDefName"
                queryGetDataFileds &= " ORDER BY OBJECT_NAME(c.object_id), c.column_id"


                queryGetColumnName = "SELECT COLUMN_NAME FROM Information_SCHEMA.columns WHERE Table_name = @TableName"
            End Sub

            Public Shared ReadOnly Property GetInstance() As DynamicDataManagerService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As DynamicDataManagerService
                Get
                    Return _instance
                End Get
            End Property

            Public Function GetDDDefNameAll(ByVal DatabaseName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = "dd_%"
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), queryGetDDDefName, ddfNameParam)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                ddfNameParam = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetMasterAndLinkTableNameByDDDefName(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sbCommandText As New StringBuilder()
                sbCommandText.AppendFormat(" SELECT REPLACE(Table_Name,'dd_{0}_','') as [TableName]", DDDefName)
                sbCommandText.Append(" From INFORMATION_SCHEMA.TABLES")
                sbCommandText.AppendFormat(" WHERE (TABLE_NAME LIKE 'dd_{0}_link%' OR TABLE_NAME LIKE 'dd_{1}_mastertable'", DDDefName, DDDefName)
                sbCommandText.Append(" AND TABLE_TYPE='BASE TABLE'")
                sbCommandText.Append(" ORDER BY Table_Name;")
                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = "dd_" & DDDefName & "_%"
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), sbCommandText.ToString(), ddfNameParam)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                ddfNameParam = Nothing
                sbCommandText = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function


            Public Function GetDynamicDataTableNameByDDDefName(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sbCommandText As New StringBuilder()
                sbCommandText.AppendFormat(" SELECT REPLACE(Table_Name,'dd_{0}_','') as [TableName]", DDDefName)
                sbCommandText.Append(" FROM INFORMATION_SCHEMA.TABLES")
                sbCommandText.Append(" WHERE Table_Name LIKE @DDDefName")
                sbCommandText.Append(" AND Table_TYPE = 'BASE TABLE'")
                sbCommandText.AppendFormat(" AND Table_Name LIKE 'dd_{0}_dd%", DDDefName)
                sbCommandText.Append(" AND Table_Name not LIKE '%_WKB'")
                sbCommandText.Append(" AND Table_Name not LIKE '%_GEO'")
                sbCommandText.Append(" ORDER BY Table_Name;")


                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = "dd_" & DDDefName & "%"
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), sbCommandText.ToString(), ddfNameParam)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                ddfNameParam = Nothing
                sbCommandText = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetColumnNameByTableName(ByVal DatabaseName As String, ByVal TableName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText As String = "select COLUMN_NAME from Information_SCHEMA.columns WHERE TABLE_NAME = @TableName"
                Dim tableNameParam As New SqlParameter()
                With tableNameParam
                    .ParameterName = "@TableName"
                    .Value = TableName
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim database As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(database, commandText, tableNameParam)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                tableNameParam = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetTableNameByDDDefName(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sbCommandText As New StringBuilder()
                sbCommandText.AppendFormat(" SELECT REPLACE(Table_Name,'dd_{0}_','') as [TableName]", DDDefName)
                sbCommandText.Append(" From INFORMATION_SCHEMA.TABLES")
                sbCommandText.Append(" Where Table_Name like @DDDefName")
                sbCommandText.Append(" AND Table_TYPE = 'BASE TABLE'")
                sbCommandText.AppendFormat(" AND (TABLE_NAME LIKE 'dd_{0}_link%' OR TABLE_NAME LIKE 'dd_{1}_mastertable' OR TABLE_NAME LIKE 'dd_{2}_dd%')", DDDefName, DDDefName, DDDefName)
                sbCommandText.Append(" AND Table_Name not like '%_WKB'")
                sbCommandText.Append(" AND Table_Name not like '%_GEO'")
                'sbCommandText.AppendFormat(" AND Table_Name <>'dd_{0}_Queries'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE 'dd_{0}_qry%'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE 'dd_{0}_Specification'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE 'dd_{0}_SynchHistory'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE  'dd_{0}_SynchHistoryOverview'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE  'dd_{0}_Validation'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE  'dd_{0}_ChartSettings'", DDDefName)
                'sbCommandText.AppendFormat(" AND Table_Name NOT LIKE  'dd_{0}_NearbyFeatures'", DDDefName)
                sbCommandText.Append(" ORDER BY Table_Name;")


                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = "dd_" & DDDefName & "%"
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), sbCommandText.ToString(), ddfNameParam)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                ddfNameParam = Nothing
                sbCommandText = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetDataFiledsByTableName(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = DDDefName
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), queryGetDataFileds, ddfNameParam)
                ddfNameParam = Nothing
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetAllColumnByTableName(ByVal DatabaseName As String, ByVal TableName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim TableNameParam As New SqlParameter()
                With TableNameParam
                    .ParameterName = "@TableName"
                    .Value = TableName
                    .SqlDbType = SqlDbType.NVarChar
                End With

                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), queryGetColumnName, TableNameParam)
                TableNameParam = Nothing
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                    store.DataBind()
                Else
                    store.DataSource = New DataTable()
                    store.DataBind()
                End If
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Sub MapDataTableOfDataFiledToStore(ByVal dtDataFileds As DataTable,
                                                      ByVal sDataEntryFields As String,
                                                      ByRef SpecificationDataFiledsStore As Ext.Net.Store)
                Dim ListDynamicSpecificationModel As New List(Of DynamicDataFiledModel)
                Dim ddfm As DynamicDataFiledModel

                Dim dictDataEntryFields As Dictionary(Of String, String) = Nothing
                If String.IsNullOrEmpty(sDataEntryFields) = False Then
                    dictDataEntryFields = sDataEntryFields.Split(",").ToDictionary(Function(v) v, Function(v) v)
                End If
                If IsNothing(dtDataFileds) = False Then
                    Dim cnt As Integer = dtDataFileds.Rows.Count() - 1
                    For i As Integer = 0 To cnt
                        ddfm = New DynamicDataFiledModel()
                        ddfm.FieldName = dtDataFileds.Rows(i)(1).ToString()
                        ddfm.Caption = dtDataFileds.Rows(i)(2).ToString()
                        'If (String.IsNullOrWhiteSpace(ddfm.Caption)) Then ddfm.Caption = ddfm.FieldName
                        If Not IsNothing(dictDataEntryFields) AndAlso dictDataEntryFields.ContainsKey(ddfm.FieldName) Then
                            ddfm.IsCheck = True
                        Else
                            ddfm.IsCheck = False
                        End If
                        ListDynamicSpecificationModel.Add(ddfm)
                    Next i
                    SpecificationDataFiledsStore.DataSource = ListDynamicSpecificationModel
                Else
                    SpecificationDataFiledsStore.DataSource = New DataTable()
                End If
                SpecificationDataFiledsStore.DataBind()
            End Sub

            Public Function GetDataFiledSettingFromJSON(ByVal GUID As String, ByVal jsonValue As String) As ConcurrentDictionary(Of String, String)
                Dim DataFiledsRecords As List(Of Dictionary(Of String, String)) = Ext.Net.JSON.Deserialize(Of List(Of Dictionary(Of String, String)))(jsonValue)
                Dim dataFiledSetting As String = ""
                Dim isCheck As Boolean = False
                Dim dict As New ConcurrentDictionary(Of String, String)
                Dim filed = "", caption = ""
                For Each record As Dictionary(Of String, String) In DataFiledsRecords
                    isCheck = CBool(record("IsCheck"))
                    caption = TryCast(record("Caption"), String)
                    filed = TryCast(record("FieldName"), String)
                    If (isCheck = True) Then
                        dataFiledSetting &= If(dataFiledSetting.Length > 0, ",", "") & filed
                    End If
                    dict(filed) = caption
                Next
                dict.TryAdd(GUID, dataFiledSetting)
                Return dict
            End Function

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal GUID1 As String, ByVal lRank As String,
                              ByVal sTableName As String, ByVal sCaption As String,
                              ByVal sDescription As String, ByVal lDescFontSize As String,
                              ByVal bIsMasterTable As Integer, ByVal bIsLinkedTable As Integer,
                              ByVal sGridQuery As String, ByVal sGridQueryMSSQL As String,
                              ByVal sDDDefName As String, ByVal dataFiledDict As ConcurrentDictionary(Of String, String))

                Dim sbCommandText As New StringBuilder()
                Dim fullTabelDataFiledName As String = String.Format("dd_{0}_{1}", sDDDefName, sTableName)
                Dim ttName As String = String.Format("dd_{0}_Specification", sDDDefName)
                sbCommandText.AppendFormat("INSERT INTO [dbo].[{0}] ", ttName)
                sbCommandText.Append(" (")
                sbCommandText.Append(" [GUID1],[lRank],[sTableName],[sCaption],[sDescription]")
                sbCommandText.Append(" ,[lDescFontSize],[sDataEntryFields],[bIsMasterTable]")
                sbCommandText.Append(" ,[bIsLinkedTable],[sGridQuery],[sGridQueryMSSQL]")
                sbCommandText.Append(" )")
                sbCommandText.Append(" VALUES(")
                sbCommandText.Append(" @GUID1,@lRank,@sTableName,@sCaption,@sDescription")
                sbCommandText.Append(" ,@lDescFontSize,@sDataEntryFields,@bIsMasterTable")
                sbCommandText.Append(" ,@bIsLinkedTable,@sGridQuery,@sGridQueryMSSQL")
                sbCommandText.Append(" )")
                Dim connectionString As String = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(connectionString,
                                             sbCommandText.ToString(),
                                             New SqlParameter("@GUID1", GUID1),
                                             New SqlParameter("@lRank", lRank),
                                             New SqlParameter("@sTableName", sTableName),
                                             New SqlParameter("@sCaption", sCaption),
                                             New SqlParameter("@sDescription", sDescription),
                                             New SqlParameter("@sDataEntryFields", dataFiledDict(GUID1)),
                                             New SqlParameter("@lDescFontSize", lDescFontSize),
                                             New SqlParameter("@bIsMasterTable", bIsMasterTable),
                                             New SqlParameter("@bIsLinkedTable", bIsLinkedTable),
                                             New SqlParameter("@sGridQuery", sGridQuery),
                                             New SqlParameter("@sGridQueryMSSQL", sGridQueryMSSQL))
                sbCommandText.Clear()
                DynamicDataManagerService.GetInstance.SaveCaption(DatabaseName, GUID1, fullTabelDataFiledName, dataFiledDict)
                ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & sDDDefName & "_", ttName, GUID1)
            End Sub
            Public Sub Update(ByVal DatabaseName As String,
                              ByVal GUID1 As String, ByVal lRank As String,
                              ByVal sTableName As String, ByVal sCaption As String,
                              ByVal sDescription As String, ByVal lDescFontSize As String,
                              ByVal bIsMasterTable As Integer, ByVal bIsLinkedTable As Integer,
                              ByVal sGridQuery As String, ByVal sGridQueryMSSQL As String,
                              ByVal sDDDefName As String, ByVal dataFiledDict As ConcurrentDictionary(Of String, String))

                Dim sbCommandText As New StringBuilder()
                Dim fullTabelDataFiledName As String = String.Format("dd_{0}_{1}", sDDDefName, sTableName)
                Dim ttName As String = String.Format("dd_{0}_Specification", sDDDefName)
                sbCommandText.AppendFormat("UPDATE [dbo].[{0}]", ttName)
                sbCommandText.Append(" SET")
                sbCommandText.Append(" [lRank]=@lRank,[sTableName]=@sTableName,[sCaption]=@sCaption,[sDescription]=@sDescription")
                sbCommandText.Append(" ,[lDescFontSize]=@lDescFontSize,[sDataEntryFields]=@sDataEntryFields,[bIsMasterTable]=@bIsMasterTable")
                sbCommandText.Append(" ,[bIsLinkedTable]=@bIsLinkedTable,[sGridQuery]=@sGridQuery,[sGridQueryMSSQL]=@sGridQueryMSSQL")
                sbCommandText.Append(" WHERE GUID1 = @GUID1")

                SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                             sbCommandText.ToString(),
                                             New SqlParameter("@GUID1", GUID1),
                                             New SqlParameter("@lRank", lRank),
                                             New SqlParameter("@sTableName", sTableName),
                                             New SqlParameter("@sCaption", sCaption),
                                             New SqlParameter("@sDescription", sDescription),
                                             New SqlParameter("@sDataEntryFields", dataFiledDict(GUID1)),
                                             New SqlParameter("@lDescFontSize", lDescFontSize),
                                             New SqlParameter("@bIsMasterTable", bIsMasterTable),
                                             New SqlParameter("@bIsLinkedTable", bIsLinkedTable),
                                             New SqlParameter("@sGridQuery", sGridQuery),
                                             New SqlParameter("@sGridQueryMSSQL", sGridQueryMSSQL))
                DynamicDataManagerService.GetInstance.SaveCaption(DatabaseName, GUID1, fullTabelDataFiledName, dataFiledDict)
                ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & sDDDefName & "_", ttName, GUID1)
            End Sub
            Private Sub FixDDRank(ByVal DatabaseName As String, ByVal DDDefName As String)
                Dim sbCommandText As New StringBuilder()
                Dim connectString As String = SQLHelper.SetConnectionString(DatabaseName)

                Dim ttName As String = String.Format("dd_{0}_Specification", DDDefName)
                sbCommandText.AppendFormat("SELECT GUID1 FROM [dbo].[{0}]", ttName)
                sbCommandText.Append(" WHERE [bIsMasterTable] = 0")
                sbCommandText.Append(" AND [bIsLinkedTable] = 0")
                sbCommandText.Append(" AND [lRank]<>9999999")
                sbCommandText.Append(" AND NOT ([sTableName] LIKE 'mastertable' OR [sTableName] LIKE 'link%')")
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(connectString, sbCommandText.ToString())
                sbCommandText.Clear()
                sbCommandText.AppendFormat("UPDATE [dbo].[{0}] SET [lRank]=9999999", ttName)
                sbCommandText.Append(" WHERE [bIsMasterTable] = 0")
                sbCommandText.Append(" AND [lRank]<>9999999")
                sbCommandText.Append(" AND [bIsLinkedTable] = 0")
                sbCommandText.Append(" AND NOT ([sTableName] LIKE 'mastertable' OR [sTableName] LIKE 'link%')")
                SQLHelper.ExecuteNonQuery(connectString, sbCommandText.ToString())
                If dt IsNot Nothing Then
                    Dim cnt As Integer = dt.Rows.Count - 1
                    Dim GUID1 As String
                    For i As Integer = 0 To cnt
                        GUID1 = TryCast(dt.Rows(i)("GUID1"), String)
                        ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", ttName, GUID1, False)
                    Next
                End If
            End Sub
            Private Sub FixLink(ByVal DatabaseName As String, ByVal DDDefName As String)
                Dim sbCommandText As New StringBuilder()
                Dim connectString As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim ttName As String = String.Format("dd_{0}_Specification", DDDefName)
                sbCommandText.AppendFormat("SELECT GUID1 FROM [dbo].[{0}]", ttName)
                sbCommandText.Append(" WHERE [bIsLinkedTable] = 0")
                sbCommandText.Append(" AND [sTableName] LIKE 'link%'")
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(connectString, sbCommandText.ToString())
                sbCommandText.Clear()
                sbCommandText.AppendFormat("UPDATE [dbo].dd_{0}_Specification SET [bIsLinkedTable]=1 , [bIsMasterTable]=0", DDDefName)
                sbCommandText.Append(" WHERE [bIsLinkedTable] = 0")
                sbCommandText.Append(" AND [sTableName] LIKE 'link%'")
                SQLHelper.ExecuteNonQuery(connectString, sbCommandText.ToString())
                If dt IsNot Nothing Then
                    Dim cnt As Integer = dt.Rows.Count - 1
                    Dim GUID1 As String
                    For i As Integer = 0 To cnt
                        GUID1 = TryCast(dt.Rows(i)("GUID1"), String)
                        ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", ttName, GUID1, False)
                    Next
                End If
            End Sub

            Private Sub FixMaster(ByVal DatabaseName As String, ByVal DDDefName As String)
                Dim sbCommandText As New StringBuilder()
                Dim connectString As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim ttName As String = String.Format("dd_{0}_Specification", DDDefName)
                sbCommandText.AppendFormat("SELECT GUID1 FROM [dbo].[{0}]", ttName)
                sbCommandText.Append(" WHERE [bIsMasterTable] = 0")
                sbCommandText.Append(" AND [sTableName] LIKE 'mastertable'")
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(connectString, sbCommandText.ToString())
                sbCommandText.Clear()
                sbCommandText.AppendFormat("UPDATE [dbo].dd_{0}_Specification SET [bIsLinkedTable]=0 , [bIsMasterTable]=1", DDDefName)
                sbCommandText.Append(" WHERE [bIsMasterTable] = 0")
                sbCommandText.Append(" AND [sTableName] LIKE 'mastertable'")
                SQLHelper.ExecuteNonQuery(connectString, sbCommandText.ToString())
                If dt IsNot Nothing Then
                    Dim cnt As Integer = dt.Rows.Count - 1
                    Dim GUID1 As String
                    For i As Integer = 0 To cnt
                        GUID1 = TryCast(dt.Rows(i)("GUID1"), String)
                        ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", ttName, GUID1, False)
                    Next
                End If
            End Sub

            Private Sub SaveCaption(ByVal DatabaseName As String,
                                   ByVal GUID1 As String,
                                   ByVal fullTabelDataFiledName As String,
                                   ByVal dataFiledDict As ConcurrentDictionary(Of String, String))
                Dim connectionString As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim scriptText As New StringBuilder()
                Dim caption As String
                'SELECT *
                'FROM(sys.extended_properties)
                'WHERE  major_id = Object_id('dd_SDC_ddDomain')
                For Each kvp As KeyValuePair(Of String, String) In dataFiledDict
                    If (kvp.Key.Equals(GUID1) = False AndAlso String.IsNullOrEmpty(kvp.Key) = False) Then
                        If (String.IsNullOrWhiteSpace(kvp.Value)) Then
                            caption = kvp.Key
                        Else
                            caption = kvp.Value
                        End If
                        scriptText.Append("IF EXISTS (")
                        scriptText.Append(" SELECT *")
                        scriptText.AppendFormat(" FROM ::fn_listextendedproperty (NULL,N'SCHEMA','dbo',N'TABLE','{0}',N'COLUMN','{1}')", fullTabelDataFiledName, kvp.Key)
                        scriptText.Append(" WHERE name='MS_Description'")
                        scriptText.AppendLine(")")
                        scriptText.AppendLine(" BEGIN")
                        scriptText.Append(" EXEC sys.sp_updateextendedproperty @level0type = N'SCHEMA',")
                        scriptText.Append(" @level0name = [dbo] ,")
                        scriptText.Append(" @level1type = N'TABLE',")
                        scriptText.Append(String.Format(" @level1name = [{0}],", fullTabelDataFiledName))
                        scriptText.Append(" @level2type = N'COLUMN',")
                        scriptText.Append(String.Format(" @level2name = [{0}],", kvp.Key))
                        scriptText.Append(" @name = N'MS_Description',")
                        scriptText.AppendLine(String.Format(" @value = N'{0}'", caption))
                        scriptText.AppendLine(" End")
                        scriptText.AppendLine(" Else")
                        scriptText.AppendLine(" BEGIN")
                        scriptText.Append(" EXEC sys.sp_addextendedproperty @level0type = N'SCHEMA',")
                        scriptText.Append(" @level0name = [dbo] ,")
                        scriptText.Append(" @level1type = N'TABLE',")
                        scriptText.Append(String.Format(" @level1name = [{0}],", fullTabelDataFiledName))
                        scriptText.Append(" @level2type = N'COLUMN',")
                        scriptText.Append(String.Format(" @level2name = [{0}],", kvp.Key))
                        scriptText.Append(" @name = N'MS_Description',")
                        scriptText.AppendLine(String.Format(" @value = N'{0}'", caption))
                        scriptText.AppendLine(" End")
                    End If
                Next
                If (Not (String.IsNullOrEmpty(scriptText.ToString()))) Then
                    SQLHelper.ExecuteScript(connectionString, scriptText.ToString() & "GO")
                End If
            End Sub
            Public Function GetTableSpecificationMasterAndLinkByTableName(ByVal DatabaseName As String, ByVal TableName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sbCommandText As New StringBuilder()
                sbCommandText.Append(" SELECT [GUID1]")
                sbCommandText.Append(",[lRank]")
                sbCommandText.Append(",[sTableName]")
                sbCommandText.Append(",[sCaption]")
                sbCommandText.Append(",[sDescription]")
                sbCommandText.Append(",[lDescFontSize]")
                sbCommandText.Append(",[sDataEntryFields]")
                sbCommandText.Append(",[bIsMasterTable]")
                sbCommandText.Append(",[bIsLinkedTable]")
                sbCommandText.Append(",[sGridQuery]")
                sbCommandText.Append(",[sGridQueryMSSQL]")
                sbCommandText.AppendFormat(" FROM [dbo].[dd_{0}_Specification]", TableName)
                sbCommandText.Append(" WHERE ([sTableName] LIKE 'mastertable' OR [sTableName] LIKE 'link%')")
                sbCommandText.Append(" OR ([bIsMasterTable] <> 0 OR [bIsLinkedTable] <> 0)")
                sbCommandText.Append(" ORDER BY [lRank]")

                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), sbCommandText.ToString())
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                sbCommandText = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetTableSpecificationDynamicDataByTableName(ByVal DatabaseName As String, ByVal TableName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sbCommandText As New StringBuilder()
                sbCommandText.Append(" SELECT [GUID1]")
                sbCommandText.Append(",[lRank]")
                sbCommandText.Append(",[sTableName]")
                sbCommandText.Append(",[sCaption]")
                sbCommandText.Append(",[sDescription]")
                sbCommandText.Append(",[lDescFontSize]")
                sbCommandText.Append(",[sDataEntryFields]")
                sbCommandText.Append(",[bIsMasterTable]")
                sbCommandText.Append(",[bIsLinkedTable]")
                sbCommandText.Append(",[sGridQuery]")
                sbCommandText.Append(",[sGridQueryMSSQL]")
                sbCommandText.AppendFormat(" FROM [dbo].[dd_{0}_Specification]", TableName)
                sbCommandText.Append(" WHERE [bIsMasterTable] = 0 AND [bIsLinkedTable] = 0")
                sbCommandText.Append(" AND NOT ([sTableName] LIKE 'mastertable' OR [sTableName] LIKE 'link%')")
                sbCommandText.Append(" ORDER BY [lRank]")

                Me.FixMaster(DatabaseName, TableName)
                Me.FixLink(DatabaseName, TableName)

                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), sbCommandText.ToString())
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                'Me.FixDDRank(DatabaseName, TableName)
                sbCommandText = Nothing
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function GetTableDescriptionAll(ByVal DatabaseName As String, ByVal TableName As String, ByVal dddefName As String) As DataTable
                Return GetTableDescriptionByDDDefName(DatabaseName, TableName, "dd_%")
            End Function

            Public Function GetTableDescriptionByDDDefName(ByVal DatabaseName As String, ByVal TableName As String, ByVal dddefName As String) As DataTable
                Dim ddfNameParam As New SqlParameter()
                With ddfNameParam
                    .ParameterName = "@DDDefName"
                    .Value = dddefName
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim dt As DataTable
                dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), queryGetDDDefName, ddfNameParam)
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Function DeleteByGUID1(ByVal DatabaseName As String, ByVal DDDefName As String, ByVal GUID1 As String) As Integer
                Dim tableName As String = String.Format("dd_{0}_Specification", DDDefName)
                Dim commandDeleteByGUID As String = String.Format("DELETE FROM {0} WHERE GUID1 = @GUID", tableName)
                Dim guidParam As New SqlParameter()
                With guidParam
                    .ParameterName = "@GUID"
                    .Value = GUID1
                    .SqlDbType = SqlDbType.NVarChar
                End With
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName), commandDeleteByGUID, guidParam)
                guidParam = Nothing
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", tableName, GUID1, ImmapUtil.TrueDelete)
                End If
                Return result
            End Function

            Private Function record() As Object
                Throw New NotImplementedException
            End Function

        End Class
    End Namespace
End Namespace