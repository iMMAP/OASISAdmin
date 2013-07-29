Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System

Namespace Immap
    Namespace Service
        Public Class DynamicDataDefService

            Private Shared ReadOnly _instance As New DynamicDataDefService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property GetInstance() As DynamicDataDefService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As DynamicDataDefService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [DDDefName],[Description],[AccessRights],[ConnectionString],[Synch],[EnableDataEntry],[EnableReporting],[LockedFields],[ExcludedFields],[DDDefName] AS [HiddenDDDefName] FROM " & "[dbo].[" & UserGroup & "DynamicDataDefs" & "]"
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                        commandText)

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

            Public Function FindByName(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal DDDefName As String) As DataTable
                Dim commandText = "SELECT [DDDefName],[Description],[AccessRights]," & _
                    "[ConnectionString],[Synch],[EnableDataEntry],[EnableReporting]," & _
                    "[LockedFields],[ExcludedFields],[DDDefName] AS [HiddenDDDefName] FROM " & "[dbo].[" & UserGroup & "DynamicDataDefs" & "]"
                commandText &= " WHERE LTRIM(RTRIM(UPPER(DDDefName))) LIKE LTRIM(RTRIM(UPPER(@DDDefName)))"
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                        commandText,
                                                        New SqlParameter("@DDDefName", DDDefName))
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Sub RunPrepareDynamicDatabaseScript(ByVal DatabaseName As String, ByVal DDDefName As String)
                Dim fullDDDefName, fullDatabaseName, prepareDynamicDatabaseScriptPath, scriptText As String
                prepareDynamicDatabaseScriptPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("PrepareDynamicDatabaseScript").ToString())
                fullDatabaseName = "USE [" & DatabaseName & "]"
                fullDDDefName = "declare @ddname nvarchar(255)" & Environment.NewLine & "set @ddname ='dd_" & DDDefName & "_'"
                scriptText = fullDatabaseName & Environment.NewLine & fullDDDefName & Environment.NewLine & IO.File.ReadAllText(prepareDynamicDatabaseScriptPath)
                SQLHelper.ExecuteScript(SQLHelper.SetConnectionString(DatabaseName), scriptText)
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal DDDefName As String,
                              ByVal Description As String,
                              ByVal AccessRights As String,
                              ByVal ConnectionString As String,
                              ByVal Synch As Integer,
                              ByVal EnableDataEntry As Integer,
                              ByVal EnableReporting As Integer,
                              ByVal LockedFields As String,
                              ByVal ExcludedFields As String)
                Dim CommandText As String = "INSERT INTO " & "[dbo].[" & UserGroup & "DynamicDataDefs" & "]" & _
                    " ([DDDefName],[Description],[AccessRights],[ConnectionString],Synch,EnableDataEntry," & _
                    " EnableReporting,LockedFields,ExcludedFields)" & _
                    " VALUES(@DDDefName,@Description,@AccessRights,@ConnectionString,@Synch," & _
                    " @EnableDataEntry,@EnableReporting,@LockedFields,@ExcludedFields)"

                AccessRights = ImmapUtil.GetInstance().CheckIfNull(AccessRights)
                LockedFields = ImmapUtil.GetInstance().CheckIfNull(LockedFields)
                ExcludedFields = ImmapUtil.GetInstance().CheckIfNull(ExcludedFields)
                ConnectionString = ImmapUtil.GetInstance().CheckIfNull(ConnectionString)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                              CommandText,
                                              New SqlParameter("@DDDefName", DDDefName),
                                              New SqlParameter("@Description", Description),
                                              New SqlParameter("@AccessRights", AccessRights),
                                              New SqlParameter("@ConnectionString", ConnectionString),
                                              New SqlParameter("@Synch", Synch),
                                              New SqlParameter("@EnableDataEntry", EnableDataEntry),
                                              New SqlParameter("@EnableReporting", EnableReporting),
                                              New SqlParameter("@LockedFields", LockedFields),
                                              New SqlParameter("@ExcludedFields", ExcludedFields))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue7]")
            End Sub

            Public Sub DeleteByName(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal DDDefName As String)
                Dim CommandText As String = "DELETE FROM " & "[dbo].[" & UserGroup & "DynamicDataDefs" & "] WHERE DDDefName=@DDDefName"
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@DDDefName", DDDefName))
                If (result > 0) Then
                    ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue7]")
                End If
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal DDDefName As String,
                              ByVal Description As String,
                              ByVal AccessRights As String,
                              ByVal ConnectionString As String,
                              ByVal Synch As Integer,
                              ByVal EnableDataEntry As Integer,
                              ByVal EnableReporting As Integer,
                              ByVal LockedFields As String,
                              ByVal ExcludedFields As String,
                              ByVal hiddenDDDefName As String)

                AccessRights = ImmapUtil.GetInstance().CheckIfNull(AccessRights)
                LockedFields = ImmapUtil.GetInstance().CheckIfNull(LockedFields)
                ExcludedFields = ImmapUtil.GetInstance().CheckIfNull(ExcludedFields)
                ConnectionString = ImmapUtil.GetInstance().CheckIfNull(ConnectionString)

                Dim CommandText As String = "UPDATE " & "[dbo].[" & UserGroup & "DynamicDataDefs" & "]" & _
                    " SET [DDDefName]=@DDDefName,[Description]=@Description,[AccessRights]=@AccessRights,ConnectionString=@ConnectionString," & _
                    " Synch=@Synch,EnableDataEntry=@EnableDataEntry,EnableReporting=@EnableReporting," & _
                    " LockedFields=@LockedFields,ExcludedFields=@ExcludedFields WHERE LTRIM(RTRIM(UPPER(DDDefName))) LIKE LTRIM(RTRIM(UPPER(@hiddenDDDefName)))"
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@DDDefName", DDDefName),
                                          New SqlParameter("@hiddenDDDefName", hiddenDDDefName),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@AccessRights", AccessRights),
                                          New SqlParameter("@ConnectionString", ConnectionString),
                                          New SqlParameter("@Synch", Synch),
                                          New SqlParameter("@EnableDataEntry", EnableDataEntry),
                                          New SqlParameter("@EnableReporting", EnableReporting),
                                          New SqlParameter("@LockedFields", LockedFields),
                                          New SqlParameter("@ExcludedFields", ExcludedFields))
                If (result > 0) Then
                    ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue7]")
                End If
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String, Optional ByVal hiddenDDDefName As String = Nothing) As Boolean
                Dim isNotValid As Boolean = False
                Dim commandText As String = "SELECT [DDDefName] FROM [dbo].[" & UserGroup & "DynamicDataDefs" & "]" &
                                               " WHERE UPPER([DDDefName]) LIKE UPPER(@DDDefName)"
                If String.IsNullOrEmpty(hiddenDDDefName) = False Then commandText &= " AND LTRIM(RTRIM(UPPER(DDDefName))) <> LTRIM(RTRIM(UPPER(@hiddenDDDefName)))"
                Dim sqlreader As SqlDataReader
                If String.IsNullOrEmpty(hiddenDDDefName) = False Then
                    sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        commandText,
                                                        New SqlParameter("@DDDefName", Name),
                                                        New SqlParameter("@hiddenDDDefName", hiddenDDDefName))
                Else
                    sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                      commandText,
                                      New SqlParameter("@DDDefName", Name))
                End If
                If sqlreader.HasRows Then
                    isNotValid = True
                    sqlreader.Close()
                End If
                If IsNothing(sqlreader) = False AndAlso sqlreader.IsClosed = False Then
                    sqlreader.Close()
                    sqlreader = Nothing
                End If
                Return isNotValid
            End Function
        End Class
    End Namespace
End Namespace
