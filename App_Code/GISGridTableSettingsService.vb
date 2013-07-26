Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Service
Imports Immap.Model
Namespace Immap
    Namespace Service
        Public Class GISGridTableSettingsService
            Private Shared ReadOnly _instance As New GISGridTableSettingsService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As GISGridTableSettingsService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As GISGridTableSettingsService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [id],[name],[alias],[visible],[datasetwarning],[warninglevel],[MaxRec],[excludedFlds],[isURLLayer],[autoRunUrls],[URLLayerField] FROM " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "]"
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
            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As GISGridTableSettingsModel
                Dim commandText = "SELECT [id],[name],[alias] as _alias,[visible],[datasetwarning],[warninglevel],[MaxRec],[excludedFlds],[isURLLayer],[autoRunUrls],[URLLayerField] FROM " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "]"
                commandText &= " WHERE id = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As GISGridTableSettingsModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New GISGridTableSettingsModel()
                        fm.Id = dr.GetInt32(0)
                        fm.Name = dr.GetString(1)
                        fm.Alia = dr.GetString(2)
                        fm.Visible = dr.GetBoolean(3)
                        fm.DatasetWarning = dr.GetBoolean(4)
                        fm.Warninglevel = dr.GetInt32(5)
                        fm.MaxRec = dr.GetInt32(6)
                        fm.ExcludedFlds = dr.GetString(7)
                        fm.IsURLLayer = dr.GetBoolean(8)
                        fm.AutoRunUrls = dr.GetBoolean(9)
                        fm.URLLayerField = dr.GetString(10)
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return fm
            End Function

            Public Sub Insert(ByVal DatabaseName As String, ByVal UserGroup As String,
                              ByVal name As String,
                              ByVal _alias As String,
                              ByVal visible As Integer,
                              ByVal datasetwarning As Integer,
                              ByVal warninglevel As Integer,
                              ByVal maxrec As Integer,
                              ByVal excludedFlds As String,
                              ByVal isURLLayer As Integer,
                              ByVal autoRunUrls As Integer,
                              ByVal urlLayerField As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)

                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "]" & _
                    " ([id],[name],[alias]," & _
                    " [visible],[datasetwarning],[warninglevel], " & _
                    " [MaxRec],[excludedFlds],[isURLLayer]," & _
                    " [autoRunUrls],[URLLayerField]) " & _
                    " VALUES(@id,@name,@_alias," & _
                    " @visible,@datasetwarning,@warninglevel, " & _
                    " @MaxRec,@excludedFlds,@isURLLayer," & _
                    " @autoRunUrls,@URLLayerField) "
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([id])+1 FROM " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@id", id),
                                          New SqlParameter("@name", name),
                                          New SqlParameter("@_alias", _alias),
                                          New SqlParameter("@visible", visible),
                                          New SqlParameter("@datasetwarning", datasetwarning),
                                          New SqlParameter("@warninglevel", warninglevel),
                                          New SqlParameter("@MaxRec", maxrec),
                                          New SqlParameter("@excludedFlds", excludedFlds),
                                          New SqlParameter("@isURLLayer", isURLLayer),
                                          New SqlParameter("@autoRunUrls", autoRunUrls),
                                          New SqlParameter("@URLLayerField", urlLayerField))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue4]")
            End Sub

            Public Sub DeleteById(ByVal DatabaseName As String,
                    ByVal UserGroup As String,
                    ByVal id As String)
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "] WHERE id=@id"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@id", id))
            End Sub

            Public Sub Update(ByVal DatabaseName As String, ByVal UserGroup As String,
                              ByVal name As String,
                              ByVal _alias As String,
                              ByVal visible As Integer,
                              ByVal datasetwarning As Integer,
                              ByVal warninglevel As Integer,
                              ByVal maxrec As Integer,
                              ByVal excludedFlds As String,
                              ByVal isURLLayer As Integer,
                              ByVal autoRunUrls As Integer,
                              ByVal urlLayerField As String,
                              ByVal id As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "GISGridTableSettings" & "]" & _
                          " SET id=@id," & _
                          " name=@name," & _
                            " [alias]=@_alias," & _
                            " visible=@visible," & _
                            " datasetwarning=@datasetwarning," & _
                            " warninglevel=@warninglevel," & _
                            " MaxRec=@MaxRec," & _
                            " excludedFlds=@excludedFlds," & _
                            " isURLLayer=@isURLLayer," & _
                            " autoRunUrls=@autoRunUrls," & _
                            " URLLayerField=@URLLayerField" & _
                            " WHERE id=@id"
                SQLHelper.ExecuteNonQuery(con,
                                            CommandText,
                                            New SqlParameter("@id", id),
                                            New SqlParameter("@name", name),
                                            New SqlParameter("@_alias", _alias),
                                            New SqlParameter("@visible", visible),
                                            New SqlParameter("@datasetwarning", datasetwarning),
                                            New SqlParameter("@warninglevel", warninglevel),
                                            New SqlParameter("@MaxRec", maxrec),
                                            New SqlParameter("@excludedFlds", excludedFlds),
                                            New SqlParameter("@isURLLayer", isURLLayer),
                                            New SqlParameter("@autoRunUrls", autoRunUrls),
                                            New SqlParameter("@URLLayerField", urlLayerField))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue4]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [name] FROM [dbo].[" & UserGroup & "GISGridTableSettings" & "]" &
                                                        " WHERE UPPER([name]) LIKE UPPER(@name);",
                                                        New SqlParameter("@name", Name))
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

            Public Function CheckDuplicateByNameAndAliasAndId(ByVal database As String, ByVal UserGroup As String, ByVal Name As String, ByVal aliass As String, ByVal Id As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim connStr As String = ImmapUtil.getConnectionStringByDatabase(database)
                Dim sbCommandText As New System.Text.StringBuilder()
                sbCommandText.AppendFormat("SELECT [name] FROM [dbo].[{0}GISGridTableSettings]", UserGroup)
                sbCommandText.Append(" WHERE UPPER([name]) LIKE UPPER(@name) AND UPPER([alias]) LIKE UPPER(@Alias) AND  id<>@id;")
                Dim sqlreader = SQLHelper.ExecuteReader(connStr, sbCommandText.ToString(), New SqlParameter("@name", Name),
                                                        New SqlParameter("@Alias", aliass), New SqlParameter("@id", CInt(Id)))
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