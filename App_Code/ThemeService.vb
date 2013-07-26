Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Namespace Immap
    Namespace Service
        Public Class ThemeService
            Private Shared ReadOnly _instance As New ThemeService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As ThemeService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As ThemeService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = " SELECT t.[ID], t.[Name],t.[ThemeGroup] AS [ThemeGroupID] ,tg.[Name] As [ThemeGroupName]," & _
                                  " t.[Description],t.[AnalysisField],t.[Maps]," & _
                                  " t.[AnalysisLayer],t.[ThemeConfigName] " & _
                                  " FROM " & "[dbo].[" & UserGroup & "Themes" & "] AS t" & _
                                  " INNER JOIN " & "[dbo].[" & UserGroup & "ThemeGroups" & "] AS tg ON tg.ID = t.ThemeGroup"

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

            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As ThemeModel
                Dim commandText = "SELECT [ID], [Name],[ThemeGroup],[Description],[AnalysisField],[Maps],[AnalysisLayer],[ThemeConfigName] FROM " & "[dbo].[" & UserGroup & "Themes" & "]"
                commandText &= " WHERE ID = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As ThemeModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New ThemeModel()
                        fm.ID = dr.GetInt32(0)
                        fm.Name = dr.GetString(1)
                        fm.ThemeGroup = dr.GetInt32(2)
                        fm.Description = dr.GetString(3)
                        fm.AnalysisField = dr.GetString(4)
                        fm.Maps = dr.GetString(5)
                        fm.AnalysisLayer = dr.GetString(6)
                        fm.ThemeConfigName = dr.GetString(7)
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return fm
            End Function

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Name As String,
                              ByVal ThemeGroup As Integer,
                              ByVal Description As String,
                              ByVal AnalysisField As String,
                              ByVal Maps As String,
                              ByVal AnalysisLayer As String,
                              ByVal ThemeConfigName As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "Themes" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "Themes" & "]" & _
                    " (ID,Name,ThemeGroup,[Description],AnalysisField,Maps,AnalysisLayer,ThemeConfigName)" &
                    " VALUES(@ID,@Name,@ThemeGroup,@Description,@AnalysisField,@Maps,@AnalysisLayer,@ThemeConfigName)"
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@ID", id),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@ThemeGroup", ThemeGroup),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@AnalysisField", AnalysisField),
                                          New SqlParameter("@Maps", Maps),
                                          New SqlParameter("@AnalysisLayer", AnalysisLayer),
                                          New SqlParameter("@ThemeConfigName", ThemeConfigName))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub

            Public Sub DeleteById(ByVal DatabaseName As String,
                          ByVal UserGroup As String,
                          ByVal ID As Integer)
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "Themes" & "] WHERE ID=@ID"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@ID", ID))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                            ByVal UserGroup As String,
                            ByVal Name As String,
                            ByVal ThemeGroup As Integer,
                            ByVal Description As String,
                            ByVal AnalysisField As String,
                            ByVal Maps As String,
                            ByVal AnalysisLayer As String,
                            ByVal ThemeConfigName As String,
                            ByVal id As String)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "Themes" & "]" & _
                        " SET Name=@Name,ThemeGroup=@ThemeGroup,[Description]=@Description," & _
                        " AnalysisField=@AnalysisField,Maps=@Maps,AnalysisLayer=@AnalysisLayer," & _
                        " ThemeConfigName=@ThemeConfigName" & _
                        " WHERE ID=@ID"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@ID", CInt(id)),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@ThemeGroup", ThemeGroup),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@AnalysisField", AnalysisField),
                                          New SqlParameter("@Maps", Maps),
                                          New SqlParameter("@AnalysisLayer", AnalysisLayer),
                                          New SqlParameter("@ThemeConfigName", ThemeConfigName))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue10]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [Name] FROM [dbo].[" & UserGroup & "Themes" & "]" &
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
                                                       "SELECT [Name] FROM [dbo].[" & UserGroup & "Themes" & "]" &
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