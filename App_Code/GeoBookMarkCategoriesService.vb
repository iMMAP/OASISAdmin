Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System

Namespace Immap
    Namespace Service
        Public Class GeoBookMarkCategoriesService
            Private Shared ReadOnly _instance As New GeoBookMarkCategoriesService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As GeoBookMarkCategoriesService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As GeoBookMarkCategoriesService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText As String = "SELECT [ID],[Name],[Description],[GUID1] FROM " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "]"
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName), commandText)

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
            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As GeoBookMarksCategoriesModel
                Dim commandText As String = "SELECT [ID],[Name],[Description],[GUID1] FROM " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "]"
                commandText &= " WHERE ID = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As GeoBookMarksCategoriesModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New GeoBookMarksCategoriesModel()
                        fm.Id = dr.GetInt32(0)
                        fm.Name = dr.GetString(1)
                        fm.Description = dr.GetString(2)
                        fm.sGUID = dr.GetString(3)
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return fm
            End Function

            Public Sub DeleteById(ByVal DatabaseName As String,
                          ByVal UserGroup As String,
                          ByVal ID As Integer,
                          ByVal GUID1 As String)
                Dim CommandText As String = "DELETE FROM " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "] WHERE ID=@ID"
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con, CommandText, New SqlParameter("@ID", ID))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarksCategories", GUID1, True)
                End If
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Name As String,
                              ByVal Description As String,
                              ByVal GUID1 As String)
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim CommandText As String = "INSERT INTO " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "]" & _
                                    " ([ID],[Name],[Description],[GUID1]) VALUES(@ID,@Name,@Description,@GUID1)"
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@ID", id),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@GUID1", GUID1))
                If (result > 0) Then ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarksCategories", GUID1)
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Name As String,
                              ByVal Description As String,
                              ByVal GUID1 As String,
                              ByVal id As String)
                Dim CommandText As String = "UPDATE " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "]" & _
                                    " SET Name=@Name,Description=@Description,[GUID1]=@GUID1 WHERE ID=@ID"
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@ID", id),
                                          New SqlParameter("@Name", Name),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@GUID1", GUID1))
                If (result > 0) Then ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarksCategories", GUID1)
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [Name] FROM [dbo].[" & UserGroup & "GeoBookMarksCategories" & "]" &
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

            Public Function CheckDuplicateByNameAndSguid(ByVal database As String, ByVal UserGroup As String, ByVal Name As String, ByVal Id As String) As Boolean
                Dim isNotValid As Boolean = False

                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                       "SELECT [Name] FROM [dbo].[" & UserGroup & "GeoBookMarksCategories" & "]" &
                                                       " WHERE UPPER([Name]) LIKE UPPER(@Name) AND [GUID1]<>@GUID1;",
                                                       New SqlParameter("@Name", Name), New SqlParameter("@GUID1", Id))
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

            Public Function GetIdAndName(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText As String = "SELECT [ID] As GeoBookMarksCategoriesId ,[Name] AS GeoBookMarksCategoriesName FROM " & "[dbo].[" & UserGroup & "GeoBookMarksCategories" & "] ORDER BY [GeoBookMarksCategoriesId]"
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
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
        End Class
    End Namespace
End Namespace