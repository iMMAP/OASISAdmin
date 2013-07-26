Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System.Web
Imports System

Namespace Immap
    Namespace Service
        Public Class GeoBookMarkService
            Private Shared ReadOnly _instance As New GeoBookMarkService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As GeoBookMarkService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As GeoBookMarkService
                Get
                    Return _instance
                End Get
            End Property

            Public Sub RunPrepareGeoBookMarkAndCategories(ByVal DatabaseName As String)
                Dim fullDatabaseName, PrepareGeoBookMarkAndCategories, scriptText As String
                PrepareGeoBookMarkAndCategories = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("PrepareGeoBookMarkAndCategories").ToString())
                fullDatabaseName = "USE [" & DatabaseName & "]"
                scriptText = fullDatabaseName & Environment.NewLine & Environment.NewLine & IO.File.ReadAllText(PrepareGeoBookMarkAndCategories)
                SQLHelper.ExecuteScript(SQLHelper.SetConnectionString(DatabaseName), scriptText)
            End Sub


            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [ID],[Name] ,[X] ,[Y],[Z]," & _
                                " [Description],[UseSymbol],[SymbolChar]," & _
                                " [SymbolFont],[SymbolSize],[MapName]," & _
                                " [BmkrID],[GUID1],[dTimeStamp],[OwnerGUID]," & _
                                " [Deleted],[isURLMark],[sURL] FROM " & _
                                " [dbo].[" & UserGroup & "GeoBookMarks" & "]"
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
            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As GeoBookMarkModel
                Dim commandText = "SELECT [ID],[Name] ,[X] ,[Y],[Z]," & _
                                    " [Description],[UseSymbol],[SymbolChar]," & _
                                    " [SymbolFont],[SymbolSize],[MapName]," & _
                                    " [BmkrID],[GUID1],[dTimeStamp],[OwnerGUID]," & _
                                    " [Deleted],[isURLMark],[sURL] FROM " & _
                                    " [dbo].[" & UserGroup & "GeoBookMarks" & "]"
                commandText &= " WHERE ID = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As GeoBookMarkModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New GeoBookMarkModel()
                        fm.ID = dr.GetInt32(0)
                        fm.Name = dr.GetString(1)
                        fm.X = dr.GetDouble(2)
                        fm.Y = dr.GetDouble(3)
                        fm.Z = dr.GetDouble(4)
                        fm.Description = dr.GetString(5)
                        fm.UseSymbol = dr.GetBoolean(6)
                        fm.SymbolChar = dr.GetString(7)
                        fm.SymbolFont = dr.GetString(8)
                        fm.SymbolSize = dr.GetString(9)
                        fm.MapName = dr.GetString(10)
                        fm.BmkrID = dr.GetInt32(11)
                        fm.sGUID = dr.GetString(12)
                        fm.dTimeStamp = dr.GetDateTime(13)
                        fm.OwnerGUID = dr.GetString(14)
                        fm.Deleted = dr.GetBoolean(15)
                        fm.isURLMark = dr.GetBoolean(16)
                        fm.sURL = dr.GetString(17)
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
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "GeoBookMarks" & "] WHERE ID=@ID"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con, CommandText, New SqlParameter("@ID", ID))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarks", GUID1)
                End If
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Name As String,
                              ByVal X As String,
                              ByVal Y As String,
                              ByVal Z As String,
                              ByVal Description As String,
                              ByVal UseSymbol As String,
                              ByVal SymbolChar As String,
                              ByVal SymbolFont As String,
                              ByVal SymbolSize As String,
                              ByVal MapName As String,
                              ByVal BmkrID As String,
                              ByVal GUID1 As String,
                              ByVal OwnerGUID As String,
                              ByVal Deleted As String,
                              ByVal isURLMark As String,
                              ByVal sURL As String)

                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "GeoBookMarks" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "GeoBookMarks" & "]" & _
                                    " ([ID],[Name],[X],[Y],[Z]," & _
                                    " [Description],[UseSymbol],[SymbolChar]," & _
                                    " [SymbolFont],[SymbolSize],[MapName],[BmkrID]," & _
                                    " [GUID1],[dTimeStamp],[OwnerGUID]," & _
                                    " [Deleted],[isURLMark],[sURL])" & _
                                    " VALUES(@ID,@Name,@XX,@YY,@ZZ," & _
                                    " @Description,@UseSymbol,@SymbolChar," & _
                                    " @SymbolFont,@SymbolSize,@MapName,@BmkrID," & _
                                    " @GUID1,@dTimeStamp,@OwnerGUID," & _
                                    " @Deleted,@isURLMark,@sURL)"

                Dim paraX = New SqlParameter("@XX", SqlDbType.NVarChar)
                paraX.Value = X
                Dim paraYY = New SqlParameter("@YY", SqlDbType.NVarChar)
                paraYY.Value = Y
                Dim paraZZ = New SqlParameter("@ZZ", SqlDbType.NVarChar)
                paraZZ.Value = Z
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@ID", id),
                                          New SqlParameter("@Name", Name),
                                          paraX,
                                          paraYY,
                                          paraZZ,
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@UseSymbol", UseSymbol),
                                          New SqlParameter("@SymbolChar", SymbolChar),
                                          New SqlParameter("@SymbolFont", SymbolFont),
                                          New SqlParameter("@SymbolSize", SymbolSize),
                                          New SqlParameter("@MapName", MapName),
                                          New SqlParameter("@BmkrID", BmkrID),
                                          New SqlParameter("@GUID1", GUID1),
                                          New SqlParameter("@dTimeStamp", Date.Now()),
                                          New SqlParameter("@OwnerGUID", OwnerGUID),
                                          New SqlParameter("@Deleted", Deleted),
                                          New SqlParameter("@isURLMark", isURLMark),
                                          New SqlParameter("@sURL", sURL))

                ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarks", GUID1)
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Name As String,
                              ByVal X As String,
                              ByVal Y As String,
                              ByVal Z As String,
                              ByVal Description As String,
                              ByVal UseSymbol As String,
                              ByVal SymbolChar As String,
                              ByVal SymbolFont As String,
                              ByVal SymbolSize As String,
                              ByVal MapName As String,
                              ByVal BmkrID As String,
                              ByVal GUID1 As String,
                              ByVal OwnerGUID As String,
                              ByVal Deleted As String,
                              ByVal isURLMark As String,
                              ByVal sURL As String,
                              ByVal id As Integer)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "GeoBookMarks" & "]" & _
                                    " SET Name=@Name,X=@XX,Y=@YY,Z=@ZZ," & _
                                    " Description=@Description,UseSymbol=@UseSymbol," & _
                                    " SymbolChar=@SymbolChar,SymbolFont=@SymbolFont," & _
                                    " SymbolSize=@SymbolSize,MapName=@MapName,BmkrID=@BmkrID," & _
                                    " GUID1=@GUID1,dTimeStamp=@dTimeStamp," & _
                                    " OwnerGUID=@OwnerGUID,Deleted=@Deleted," & _
                                    " isURLMark=@isURLMark,sURL=@sURL " & _
                                    " WHERE ID=@ID"
                Dim paraX = New SqlParameter("@XX", SqlDbType.Float)
                paraX.Value = X
                Dim paraYY = New SqlParameter("@YY", SqlDbType.Float)
                paraYY.Value = X
                Dim paraZZ = New SqlParameter("@ZZ", SqlDbType.Float)
                paraZZ.Value = X
                SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@Name", Name),
                                          paraX,
                                          paraYY,
                                          paraZZ,
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@UseSymbol", UseSymbol),
                                          New SqlParameter("@SymbolChar", SymbolChar),
                                          New SqlParameter("@SymbolFont", SymbolFont),
                                          New SqlParameter("@SymbolSize", SymbolSize),
                                          New SqlParameter("@MapName", MapName),
                                          New SqlParameter("@BmkrID", BmkrID),
                                          New SqlParameter("@GUID1", GUID1),
                                          New SqlParameter("@dTimeStamp", Date.Now()),
                                          New SqlParameter("@OwnerGUID", OwnerGUID),
                                          New SqlParameter("@Deleted", Deleted),
                                          New SqlParameter("@isURLMark", isURLMark),
                                          New SqlParameter("@sURL", sURL),
                                          New SqlParameter("@ID", id))
                ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, UserGroup, UserGroup & "GeoBookMarks", GUID1)
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [Name] FROM [dbo].[" & UserGroup & "GeoBookMarks" & "]" &
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
                                                       "SELECT [Name] FROM [dbo].[" & UserGroup & "GeoBookMarks" & "]" &
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