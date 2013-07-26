Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Namespace Immap
    Namespace Service
        Public Class TtkGISProjectDefService
            Private Shared ReadOnly _instance As New TtkGISProjectDefService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As TtkGISProjectDefService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As TtkGISProjectDefService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [InUse],[sName],[MapData],[sGUID],[XMIN],[XMAX],[YMIN],[YMAX], " &
                                " [sInfo], [bSavedToDB], [sFilePath], " & _
                                " [centerX], [centerY], " & _
                                " [EPSG], [scale], [CreatedBy],[CreatedDate], " & _
                                " [Description], [Contact],[Restrictions], " & _
                                " [Copyright],[url], [StandardLyrs], " & _
                                " [Source],[AdminLyr1Name], [AdminLyr2Name], " & _
                                " [AdminLyr3Name],[AdminLyr4Name],[AdminLyr5Name] " & _
                                " FROM " & "[dbo].[" & UserGroup & "ttkGISProjectDef" & "]"
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
            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As TtkGISProjectDefModel
                Dim commandText = "SELECT [InUse],[sName],[MapData],[sGUID],[XMIN],[XMAX],[YMIN],[YMAX] FROM " & "[dbo].[" & UserGroup & "ttkGISProjectDef" & "]"
                commandText &= " WHERE sGUID = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As TtkGISProjectDefModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New TtkGISProjectDefModel()
                        fm.InUse = dr.GetBoolean(0)
                        fm.MapData = dr.GetString(1)
                        fm.sGUID = dr.GetString(2)
                        fm.XMIN = dr.GetDouble(3)
                        fm.XMAX = dr.GetDouble(4)
                        fm.YMIN = dr.GetDouble(5)
                        fm.YMAX = dr.GetDouble(6)
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
                             ByVal InUse As Integer,
                             ByVal sName As String,
                             ByVal MapData As String, ByVal sGUID As String,
                             ByVal XMIN As Double,
                             ByVal XMAX As Double,
                             ByVal YMIN As Double,
                             ByVal YMAX As Double,
                             ByVal Info As String,
                             ByVal SavedToDB As Integer,
                             ByVal FilePath As String,
                             ByVal centerX As String,
                             ByVal centerY As String,
                             ByVal EPSG As String,
                             ByVal scale As String,
                             ByVal CreatedBy As String,
                             ByVal CreatedDate As String,
                             ByVal Description As String,
                             ByVal Contact As String,
                             ByVal Restrictions As String,
                             ByVal Copyright As String,
                             ByVal url As String,
                             ByVal StandardLyrs As String,
                             ByVal Source As String,
                             ByVal AdminLyr1Name As String,
                             ByVal AdminLyr2Name As String,
                             ByVal AdminLyr3Name As String,
                             ByVal AdminLyr4Name As String,
                             ByVal AdminLyr5Name As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "ttkGISProjectDef" & "]" & _
                    " ([InUse],[sName],[MapData],[sGUID]," & _
                    " [XMIN],[XMAX], " & _
                    " [YMIN],[YMAX], " & _
                    " [sInfo], [bSavedToDB], [sFilePath], " & _
                    " [bUGMap], [centerX], [centerY], " & _
                    " [EPSG], [scale], [CreatedBy],[CreatedDate]," & _
                    " [Description], [Contact],[Restrictions], " & _
                    " [Copyright],[url], [StandardLyrs], " & _
                    " [Source],[AdminLyr1Name], [AdminLyr2Name], " & _
                    " [AdminLyr3Name],[AdminLyr4Name],[AdminLyr5Name]) " & _
                    " VALUES(@InUse,@sName,@MapData,@sGUID," & _
                    " @XMIN,@XMAX, " & _
                    " @YMIN,@YMAX, " & _
                    " @Info, @SavedToDB, @FilePath, " & _
                    " @UGMap, @centerX, @centerY, " & _
                    " @EPSG, @scale, @CreatedBy,@CreatedDate, " & _
                    " @Description, @Contact,@Restrictions, " & _
                    " @Copyright,@url, @StandardLyrs, " & _
                    " @Source,@AdminLyr1Name, @AdminLyr2Name, " & _
                    " @AdminLyr3Name, @AdminLyr4Name, @AdminLyr5Name); "
                SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@InUse", InUse),
                                          New SqlParameter("@sName", sName),
                                          New SqlParameter("@MapData", MapData),
                                          New SqlParameter("@sGUID", sGUID),
                                          New SqlParameter("@XMIN", XMIN.ToString()),
                                          New SqlParameter("@XMAX", XMAX.ToString()),
                                          New SqlParameter("@YMIN", YMIN.ToString()),
                                          New SqlParameter("@YMAX", YMAX.ToString()),
                                          New SqlParameter("@Info", Info),
                                          New SqlParameter("@SavedToDB", SavedToDB),
                                          New SqlParameter("@FilePath", FilePath),
                                          New SqlParameter("@UGMap", "1"),
                                          New SqlParameter("@centerX", centerX.ToString()),
                                          New SqlParameter("@centerY", centerY.ToString()),
                                          New SqlParameter("@EPSG", EPSG),
                                          New SqlParameter("@scale", scale),
                                          New SqlParameter("@CreatedBy", CreatedBy),
                                          New SqlParameter("@CreatedDate", CreatedDate),
                                          New SqlParameter("@Description", Description),
                                          New SqlParameter("@Contact", Contact),
                                          New SqlParameter("@Restrictions", Restrictions),
                                          New SqlParameter("@Copyright", Copyright),
                                          New SqlParameter("@url", url),
                                          New SqlParameter("@StandardLyrs", StandardLyrs),
                                          New SqlParameter("@Source", Source),
                                          New SqlParameter("@AdminLyr1Name", AdminLyr1Name),
                                          New SqlParameter("@AdminLyr2Name", AdminLyr2Name),
                                          New SqlParameter("@AdminLyr3Name", AdminLyr3Name),
                                          New SqlParameter("@AdminLyr4Name", AdminLyr4Name),
                                          New SqlParameter("@AdminLyr5Name", AdminLyr5Name))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "MapProjectDef", "[SettingValue1]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String, ByVal UserGroup As String,
                             ByVal InUse As Integer,
                             ByVal sName As String,
                             ByVal MapData As String, ByVal sGUID As String,
                             ByVal XMIN As Double,
                             ByVal XMAX As Double,
                             ByVal YMIN As Double,
                             ByVal YMAX As Double,
                             ByVal Info As String,
                             ByVal SavedToDB As Integer,
                             ByVal FilePath As String,
                             ByVal centerX As String,
                             ByVal centerY As String,
                             ByVal EPSG As String,
                             ByVal scale As String,
                             ByVal CreatedBy As String,
                             ByVal CreatedDate As String,
                             ByVal Description As String,
                             ByVal Contact As String,
                             ByVal Restrictions As String,
                             ByVal Copyright As String,
                             ByVal url As String,
                             ByVal StandardLyrs As String,
                             ByVal Source As String,
                             ByVal AdminLyr1Name As String,
                             ByVal AdminLyr2Name As String,
                             ByVal AdminLyr3Name As String,
                             ByVal AdminLyr4Name As String,
                             ByVal AdminLyr5Name As String)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "ttkGISProjectDef" & "]" & _
                          " SET InUse=@InUse," & _
                          " [sName]=@sName," & _
                          " MapData=@MapData," & _
                            " XMIN=@XMIN," & _
                            " XMAX=@XMAX," & _
                            " YMIN=@YMIN," & _
                            " YMAX=@YMAX," & _
                            " [sInfo]=@Info, [bSavedToDB]=@SavedToDB, [sFilePath]=@FilePath, " & _
                            " [bUGMap]=@UGMap, [centerX]=@centerX, [centerY]=@centerY, " & _
                            " [EPSG]=@EPSG, [scale]=@scale, [CreatedBy]=@CreatedBy, [CreatedDate]=@CreatedDate, " & _
                            " [Description]=@Description, [Contact]=@Contact,[Restrictions]=@Restrictions, " & _
                            " [Copyright]=@Copyright,[url]=@url, [StandardLyrs]=@StandardLyrs, " & _
                            " [Source]=@Source,[AdminLyr1Name]=@AdminLyr1Name, [AdminLyr2Name]=@AdminLyr2Name, " & _
                            " [AdminLyr3Name]=@AdminLyr3Name,[AdminLyr4Name]=@AdminLyr4Name,[AdminLyr5Name]=@AdminLyr5Name " & _
                            " WHERE sGUID=@sGUID;"
                SQLHelper.ExecuteNonQuery(con,
                                               CommandText,
                                               New SqlParameter("@InUse", InUse),
                                               New SqlParameter("@sName", sName),
                                               New SqlParameter("@MapData", MapData),
                                               New SqlParameter("@sGUID", sGUID),
                                               New SqlParameter("@XMIN", XMIN.ToString()),
                                               New SqlParameter("@XMAX", XMAX.ToString()),
                                               New SqlParameter("@YMIN", YMIN.ToString()),
                                               New SqlParameter("@YMAX", YMAX.ToString()),
                                               New SqlParameter("@Info", Info),
                                               New SqlParameter("@SavedToDB", SavedToDB),
                                               New SqlParameter("@FilePath", FilePath),
                                               New SqlParameter("@UGMap", "1"),
                                               New SqlParameter("@centerX", centerX.ToString()),
                                               New SqlParameter("@centerY", centerY.ToString()),
                                               New SqlParameter("@EPSG", EPSG),
                                               New SqlParameter("@scale", scale),
                                               New SqlParameter("@CreatedBy", CreatedBy),
                                               New SqlParameter("@CreatedDate", CreatedDate),
                                               New SqlParameter("@Description", Description),
                                               New SqlParameter("@Contact", Contact),
                                               New SqlParameter("@Restrictions", Restrictions),
                                               New SqlParameter("@Copyright", Copyright),
                                               New SqlParameter("@url", url),
                                               New SqlParameter("@StandardLyrs", StandardLyrs),
                                               New SqlParameter("@Source", Source),
                                               New SqlParameter("@AdminLyr1Name", AdminLyr1Name),
                                               New SqlParameter("@AdminLyr2Name", AdminLyr2Name),
                                               New SqlParameter("@AdminLyr3Name", AdminLyr3Name),
                                               New SqlParameter("@AdminLyr4Name", AdminLyr4Name),
                                               New SqlParameter("@AdminLyr5Name", AdminLyr5Name))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "MapProjectDef", "[SettingValue1]")
            End Sub

            Public Sub DeleteById(ByVal DatabaseName As String,
                      ByVal UserGroup As String,
                      ByVal sGUID As String)
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "ttkGISProjectDef" & "] WHERE sGUID=@sGUID"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@sGUID", sGUID))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "MapProjectDef", "[SettingValue1]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [sName] FROM [dbo].[" & UserGroup & "ttkGISProjectDef" & "]" &
                                                        " WHERE UPPER([sName]) LIKE UPPER(@sName);",
                                                        New SqlParameter("@sName", Name))
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
                                                       "SELECT [sName] FROM [dbo].[" & UserGroup & "ttkGISProjectDef" & "]" &
                                                       " WHERE UPPER([sName]) LIKE UPPER(@sName) AND sGUID<>@sGUID;",
                                                       New SqlParameter("@sName", Name), New SqlParameter("@sGUID", Id))
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