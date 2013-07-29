Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System
Imports System.Web

Namespace Immap
    Namespace Service
        Public Class WebTilesService
            Private Shared ReadOnly _instance As New WebTilesService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property GetInstance() As WebTilesService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As WebTilesService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText = "SELECT [Caption],[URL1],[URL2],[URL3],[ESPGNumber],[ImageFormat],[ForceWGS],[Caption] AS [HiddenCaption] FROM " & "[dbo].[" & UserGroup & "WebTiles" & "]"
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

            Public Function FindByName(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal sCaption As String) As DataTable
                Dim commandText = "SELECT [Caption],[URL1],[URL2],[URL3],[ESPGNumber],[ImageFormat],[ForceWGS],[Caption] AS [HiddenCaption] FROM " & "[dbo].[" & UserGroup & "WebTiles" & "]"
                commandText &= " WHERE LTRIM(RTRIM(UPPER(Caption))) LIKE LTRIM(RTRIM(UPPER(@Caption)))"
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                        commandText,
                                                        New SqlParameter("@Caption", sCaption))
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Sub RunPrepareUserGroupDatabaseScript(ByVal DatabaseName As String, ByVal sUserGroup As String)
                Dim fullDDDefName, fullDatabaseName, PrepareWebTilesScript, scriptText As String
                PrepareWebTilesScript = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("PrepareWebTilesScript").ToString())
                fullDatabaseName = "USE [" & DatabaseName & "]"
                fullDDDefName = "declare @tableprefix nvarchar(255)" & Environment.NewLine & "set @tableprefix ='" & sUserGroup & "'"
                scriptText = fullDatabaseName & Environment.NewLine & fullDDDefName & Environment.NewLine & IO.File.ReadAllText(PrepareWebTilesScript)
                SQLHelper.ExecuteScript(SQLHelper.SetConnectionString(DatabaseName), scriptText)
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Caption As String,
                              ByVal URL1 As String,
                              ByVal URL2 As String,
                              ByVal URL3 As String,
                              ByVal ESPGNumber As Integer,
                              ByVal ImageFormat As String,
                              ByVal ForceWGS As Integer)
                Dim CommandText = "INSERT INTO " & "[dbo].[" & UserGroup & "WebTiles" & "]" & _
                    " ([Caption],[URL1],[URL2],[URL3],[ESPGNumber],[ImageFormat],[ForceWGS])" & _
                    " VALUES(@Caption,@URL1,@URL2,@URL3,@ESPGNumber," & _
                    " @ImageFormat,@ForceWGS)"

                URL1 = ImmapUtil.GetInstance().CheckIfNull(URL1)
                URL2 = ImmapUtil.GetInstance().CheckIfNull(URL2)
                URL3 = ImmapUtil.GetInstance().CheckIfNull(URL3)
                ImageFormat = ImmapUtil.GetInstance().CheckIfNull(ImageFormat)
                SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                              CommandText,
                                              New SqlParameter("@Caption", Caption),
                                              New SqlParameter("@URL1", URL1),
                                              New SqlParameter("@URL2", URL2),
                                              New SqlParameter("@URL3", URL3),
                                              New SqlParameter("@ESPGNumber", ESPGNumber),
                                              New SqlParameter("@ImageFormat", ImageFormat),
                                              New SqlParameter("@ForceWGS", ForceWGS))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue3]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal Caption As String,
                               ByVal HiddenCaption As String,
                              ByVal URL1 As String,
                              ByVal URL2 As String,
                              ByVal URL3 As String,
                              ByVal ESPGNumber As Integer,
                              ByVal ImageFormat As String,
                              ByVal ForceWGS As Integer)
                Dim CommandText = "UPDATE " & "[dbo].[" & UserGroup & "WebTiles" & "]" & _
                    " SET [Caption]=@Caption,[URL1]=@URL1,[URL2]=@URL2," & _
                    " [URL3]=@URL3,[ESPGNumber]=@ESPGNumber,[ImageFormat]=@ImageFormat," & _
                    " [ForceWGS]=@ForceWGS WHERE LTRIM(RTRIM(UPPER(Caption))) = LTRIM(RTRIM(UPPER(@HiddenCaption)))"
                URL1 = ImmapUtil.GetInstance().CheckIfNull(URL1)
                URL2 = ImmapUtil.GetInstance().CheckIfNull(URL2)
                URL3 = ImmapUtil.GetInstance().CheckIfNull(URL3)
                ImageFormat = ImmapUtil.GetInstance().CheckIfNull(ImageFormat)
                SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                              CommandText,
                                              New SqlParameter("@Caption", Caption),
                                              New SqlParameter("@HiddenCaption", HiddenCaption),
                                              New SqlParameter("@URL1", URL1),
                                              New SqlParameter("@URL2", URL2),
                                              New SqlParameter("@URL3", URL3),
                                              New SqlParameter("@ESPGNumber", ESPGNumber),
                                              New SqlParameter("@ImageFormat", ImageFormat),
                                              New SqlParameter("@ForceWGS", ForceWGS))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue3]")
            End Sub


            Public Sub DeleteByCaption(ByVal DatabaseName As String,
                ByVal UserGroup As String,
                ByVal sCaption As String)
                Dim CommandText = "DELETE FROM " & "[dbo].[" & UserGroup & "WebTiles" & "] WHERE Caption=@Caption"
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@Caption", sCaption))
                ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue3]")
            End Sub


            Public Function CheckDuplicateCaption(ByVal database As String, ByVal UserGroup As String, ByVal sCaption As String, Optional ByVal hiddenCaption As String = Nothing) As Boolean
                Dim isNotValid As Boolean = False
                Dim commandText As String = "SELECT [Caption] FROM [dbo].[" & UserGroup & "WebTiles" & "]" &
                                                  " WHERE UPPER([Caption]) LIKE UPPER(@Caption)"
                If String.IsNullOrEmpty(hiddenCaption) = False Then commandText &= " AND LTRIM(RTRIM(UPPER(Caption))) <> LTRIM(RTRIM(UPPER(@HiddenCaption)))"
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database), commandText,
                                                        New SqlParameter("@Caption", sCaption),
                                                        New SqlParameter("@HiddenCaption", hiddenCaption))
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