Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System

Namespace Immap
    Namespace Service
        Public Class ThemeGroupService
            Private Shared ReadOnly _instance As New ThemeGroupService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub
            Public Shared ReadOnly Property Instance() As ThemeGroupService
                Get
                    Return _instance
                End Get
            End Property
            Public Shared ReadOnly Property GetInstance() As ThemeGroupService
                Get
                    Return _instance
                End Get
            End Property
            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [ID] ,[Name] ,[Description] FROM " & "[dbo].[" & UserGroup & "ThemeGroups" & "] ORDER BY [ID]"
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                        commandText)

                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                    store.DataBind()
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
                Dim commandText = "SELECT [ID] As ThemeGroupId ,[Name] AS ThemeGroupName FROM " & "[dbo].[" & UserGroup & "ThemeGroups" & "] ORDER BY [ThemeGroupId]"
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                    commandText)
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                    store.DataBind()
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

            Public Function CountThemesByGroupId(ByVal DatabaseName As String,
                      ByVal UserGroup As String,
                      ByVal ID As Integer) As System.Int32
                Dim CommandText = "SELECT COUNT(*) AS GroupCnt FROM " & "[dbo].[" & UserGroup & "Themes" & "] WHERE ThemeGroup = @ThemeGroup"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim dt = SQLHelper.ExecuteDataTable(con,
                                     CommandText,
                                     New SqlParameter("@ThemeGroup", ID))
                Dim cnt = Convert.ToInt32(dt.Rows(0)(0))
                Return cnt
            End Function


            Public Sub DeleteById(ByVal DatabaseName As String,
                          ByVal UserGroup As String,
                          ByVal ID As Integer)
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "ThemeGroups" & "] WHERE ID=@ID"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@ID", ID))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub

            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As ThemeGroupModel
                Dim commandText = "SELECT [ID] ,[Name] ,[Description] FROM " & "[dbo].[" & UserGroup & "ThemeGroups" & "]"
                commandText &= " WHERE ID = @ID ORDER BY [ID]"
                Dim dr As SqlDataReader = Nothing
                Dim tm As ThemeGroupModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                commandText,
                                                New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        tm = New ThemeGroupModel()
                        tm.ID = dr.GetInt32(0)
                        tm.Name = dr.GetString(1)
                        tm.Description = dr.GetString(2)
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return tm
            End Function

            Public Sub Insert(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Name As String, ByVal Description As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "ThemeGroups" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "ThemeGroups" & "]" & " (ID,Name,Description) VALUES(@ID,@Name,@Description)"
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@ID", id),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@Description", Description))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub


            Public Sub Update(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Name As String, ByVal Description As String, ByVal id As String)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "ThemeGroups" & "]" & " SET Name=@Name,Description=@Description WHERE ID=@ID"
                SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@Id", CInt(id)),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@Description", Description))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [Name] FROM [dbo].[" & UserGroup & "ThemeGroups" & "]" &
                                                        " WHERE UPPER([Name]) LIKE UPPER(@Name);",
                                                        New SqlParameter("@Name", Name))
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
                                                       "SELECT [Name] FROM [dbo].[" & UserGroup & "ThemeGroups" & "]" &
                                                       " WHERE UPPER([Name]) LIKE UPPER(@Name) AND ID<>@ID;",
                                                       New SqlParameter("@Name", Name), New SqlParameter("@ID", CInt(Id)))
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