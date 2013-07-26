Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System

Namespace Immap
    Namespace Service
        Public Class FeedGroupService

            Private Shared ReadOnly _instance As New FeedGroupService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As FeedGroupService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As FeedGroupService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [GroupId] ,[GroupText] ,[CustomGroup] FROM " & "[dbo].[" & UserGroup & "FeedGroups" & "]"
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

            Public Function GetIdAndName(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText As String = "SELECT [GroupId] AS [FeedGroupID],[GroupText] AS [FeedGroupName] FROM " _
                                  & "[dbo].[" & UserGroup & "FeedGroups" & "]"
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
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


            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As FeedGroupModel
                Dim commandText As String = "SELECT [GroupId] ,[GroupText] ,[CustomGroup] FROM " & "[dbo].[" & UserGroup & "FeedGroups" & "]"
                commandText &= " WHERE GroupId = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As FeedGroupModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New FeedGroupModel()
                        fm.GroupId = dr.GetInt32(0)
                        fm.GroupText = dr.GetString(1)
                        fm.CustomGroup = dr.GetBoolean(2)
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return fm
            End Function

            Public Function CountFeedsByGroupId(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal GroupId As Integer) As System.Int32
                Dim CommandText As String = "SELECT COUNT(*) AS GroupCnt FROM " & "[dbo].[" & UserGroup & "Feeds" & "] WHERE GroupID = @GroupId"
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(con,
                                     CommandText,
                                     New SqlParameter("@GroupId", GroupId))
                Dim cnt As Integer = Convert.ToInt32(dt.Rows(0)(0))
                Return cnt
            End Function

            Public Sub DeleteById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal FeedID As Integer)
                Dim CommandText As String = "DELETE FROM " & "[dbo].[" & UserGroup & "FeedGroups" & "] WHERE GroupId=@GroupId"
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@GroupId", FeedID))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub Insert(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal GroupText As String, ByVal CustomGroup As Integer)
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([GroupId])+1 FROM " & "[dbo].[" & UserGroup & "FeedGroups" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim CommandText As String = "INSERT INTO " & "[dbo].[" & UserGroup & "FeedGroups" & "]" & _
                    " (GroupId,GroupText,CustomGroup) VALUES(@GroupId,@GroupText,@CustomGroup)"
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@GroupId", id),
                                          New SqlParameter("@GroupText", GroupText),
                                          New SqlParameter("@CustomGroup", CustomGroup))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal GroupText As String, ByVal CustomGroup As Integer, ByVal id As String)
                Dim CommandText As String = "UPDATE " & "[dbo].[" & UserGroup & "FeedGroups" & "]" & _
                    " SET GroupText=@GroupText,CustomGroup=@CustomGroup WHERE GroupId=@GroupId"
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@GroupId", CInt(id)),
                                          New SqlParameter("@GroupText", GroupText),
                                          New SqlParameter("@CustomGroup", CustomGroup))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [GroupText] FROM [dbo].[" & UserGroup & "FeedGroups" & "]" &
                                                        " WHERE UPPER([GroupText]) LIKE UPPER(@GroupText);",
                                                        New SqlParameter("@GroupText", Name))
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

            Public Function CheckDuplicateByNameAndId(ByVal database As String, ByVal UserGroup As String, ByVal Name As String, ByVal Id As String) As Boolean
                Dim isNotValid As Boolean = False

                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                "SELECT [GroupText] FROM [dbo].[" & UserGroup & "FeedGroups" & "]" &
                                                " WHERE UPPER([GroupText]) LIKE UPPER(@GroupText) AND GroupId<>@GroupId;",
                                                New SqlParameter("@GroupText", Name), New SqlParameter("@GroupId", CInt(Id)))
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