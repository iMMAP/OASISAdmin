Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Imports System

Namespace Immap
    Namespace Service
        Public Class FeedServices
            Private Shared ReadOnly _instance As New FeedServices()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As FeedServices
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As FeedServices
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal UserGroup As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim commandText As String = " SELECT f.[FeedID],f.[GroupID] AS [FeedGroupID]," & _
                                 " fg.[GroupText] As [FeedGroupName],f.[CustomId], " & _
                                 " f.[FeedName], f.[FeedDescription],f.[FeedURL], " & _
                                 " f.[FeedImageURL],f.[CheckInterval],CONVERT(VARCHAR(10), f.[LastCheck],103) AS LastCheck " & _
                                 " FROM " & "[dbo].[" & UserGroup & "Feeds" & "] AS f " & _
                                 " INNER JOIN " & "[dbo].[" & UserGroup & "FeedGroups" & "] AS fg ON fg.[GroupId] = f.[GroupID]"

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

            Public Function FindById(ByVal DatabaseName As String, ByVal UserGroup As String, ByVal Id As Integer) As FeedModel
                Dim commandText = "SELECT [FeedID],[GroupID],[CustomId],[FeedName],[FeedDescription], " & _
                                  "[FeedURL],[FeedImageURL],[CheckInterval],[Subscribed],[LastCheck] " & _
                                  "FROM " & "[dbo].[" & UserGroup & "FeedGroups" & "]"
                commandText &= " WHERE FeedID = @ID"
                Dim dr As SqlDataReader = Nothing
                Dim fm As FeedModel = Nothing
                Try
                    dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(DatabaseName),
                                                     commandText,
                                                     New SqlParameter("@ID", Id))
                    If dr.HasRows Then
                        dr.Read()
                        fm = New FeedModel()
                        fm.FeedID = dr.GetInt32(0)
                        fm.GroupId = dr.GetInt32(1)
                        fm.CustomId = dr.GetInt32(2)
                        fm.FeedName = dr.GetString(3)
                        fm.FeedDescription = dr.GetString(4)
                        fm.FeedURL = dr.GetString(5)
                        fm.FeedImageURL = dr.GetString(6)
                        fm.CheckInterval = dr.GetInt32(7)
                        fm.Subscribed = dr.GetString(8)
                        fm.LastCheck = dr.GetDateTime(9)
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
                              ByVal GroupID As Integer,
                              ByVal CustomId As String,
                              ByVal FeedName As String,
                              ByVal FeedDescription As String,
                              ByVal FeedURL As String,
                              ByVal FeedImageURL As String,
                              ByVal CheckInterval As Integer,
                              ByVal Subscribed As String,
                              ByVal LastCheck As Date)
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                '  Dim lastchk As String = LastCheck.Month.ToString() & "/" & LastCheck.Date.ToString() & "/" & LastCheck.Year.ToString()
                Dim CommandText As String = "INSERT INTO " & "[dbo].[" & UserGroup & "Feeds" & "]" & _
                                  " (FeedID,GroupID,CustomId,FeedName,FeedDescription, " & _
                                  " FeedURL,FeedImageURL,CheckInterval,Subscribed,LastCheck) " & _
                                  " VALUES(@FeedID,@GroupID,@CustomId,@FeedName,@FeedDescription," & _
                                  " @FeedURL,@FeedImageURL,@CheckInterval,@Subscribed,@LastCheck)"
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "Feeds" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                Dim custID As String = Nothing
                If CustomId > 0 Then
                    custID = CustomId.ToString()
                End If
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@FeedID", id),
                                          New SqlParameter("@GroupID", GroupID),
                                          New SqlParameter("@CustomId", CustomId),
                                          New SqlParameter("@FeedName", FeedName),
                                          New SqlParameter("@FeedDescription", FeedDescription),
                                          New SqlParameter("@FeedURL", FeedURL),
                                          New SqlParameter("@FeedImageURL", FeedImageURL),
                                          New SqlParameter("@CheckInterval", CheckInterval),
                                          New SqlParameter("@Subscribed", Subscribed),
                                          New SqlParameter("@LastCheck", LastCheck))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                            ByVal UserGroup As String,
                            ByVal GroupID As Integer,
                            ByVal CustomId As String,
                            ByVal FeedName As String,
                            ByVal FeedDescription As String,
                            ByVal FeedURL As String,
                            ByVal FeedImageURL As String,
                            ByVal CheckInterval As Integer,
                            ByVal Subscribed As String)
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim CommandText As String = "INSERT INTO " & "[dbo].[" & UserGroup & "Feeds" & "]" & _
                                  " (FeedID,GroupID,CustomId,FeedName,FeedDescription, " & _
                                  " FeedURL,FeedImageURL,CheckInterval,Subscribed) " & _
                                  " VALUES(@FeedID,@GroupID,@CustomId,@FeedName,@FeedDescription," & _
                                  " @FeedURL,@FeedImageURL,@CheckInterval,@Subscribed)"
                Dim id = SQLHelper.ExecuteScalar(con, "SELECT MAX([FeedID])+1 FROM " & "[dbo].[" & UserGroup & "Feeds" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                ''Dim custID As String = ""
                ''If CustomId > 0 Then
                ''    custID = CustomId.ToString()
                ''End If
                Dim custID As String = ImmapUtil.GetInstance().CheckIfNull(CustomId)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                          CommandText,
                                          New SqlParameter("@FeedID", id),
                                          New SqlParameter("@GroupID", GroupID),
                                          New SqlParameter("@CustomId", custID),
                                          New SqlParameter("@FeedName", FeedName),
                                          New SqlParameter("@FeedDescription", FeedDescription),
                                          New SqlParameter("@FeedURL", FeedURL),
                                          New SqlParameter("@FeedImageURL", FeedImageURL),
                                          New SqlParameter("@CheckInterval", CheckInterval),
                                          New SqlParameter("@Subscribed", Subscribed))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub DeleteById(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal FeedID As Integer)
                Dim CommandText As String = "DELETE FROM " & "[dbo].[" & UserGroup & "Feeds" & "] WHERE FeedID=@FeedID"
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con,
                                     CommandText,
                                     New SqlParameter("@FeedID", FeedID))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal GroupID As Integer,
                              ByVal CustomId As String,
                              ByVal FeedName As String,
                              ByVal FeedDescription As String,
                              ByVal FeedURL As String,
                              ByVal FeedImageURL As String,
                              ByVal CheckInterval As Integer,
                              ByVal Subscribed As String,
                              ByVal LastCheck As Date,
                              ByVal FeedID As Integer)
                Dim CommandText As String = "UPDATE " & "[dbo].[" & UserGroup & "Feeds" & "]" & _
                    " SET GroupID=@GroupID,CustomId=@CustomId,FeedName=@FeedName, " & _
                    " FeedDescription=@FeedDescription,FeedURL=@FeedURL," & _
                    " FeedImageURL=@FeedImageURL,CheckInterval=@CheckInterval, " & _
                    "Subscribed=@Subscribed,LastCheck=@LastCheck" & _
                    " WHERE FeedID=@FeedID"
                Dim custID As String = Nothing
                If CustomId.Trim() = "" Then
                    CustomId = 0
                End If
                If CInt(CustomId) > 0 Then
                    custID = CustomId.ToString()
                End If
                '   Dim lastchk As String = LastCheck.Month.ToString() & "/" & LastCheck.Date.ToString() & "/" & LastCheck.Year.ToString()
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@FeedID", CInt(FeedID)),
                                          New SqlParameter("@GroupID", GroupID),
                                          New SqlParameter("@CustomId", CustomId),
                                          New SqlParameter("@FeedName", FeedName),
                                          New SqlParameter("@FeedDescription", FeedDescription),
                                          New SqlParameter("@FeedURL", FeedURL),
                                          New SqlParameter("@FeedImageURL", FeedImageURL),
                                          New SqlParameter("@CheckInterval", CheckInterval),
                                          New SqlParameter("@Subscribed", Subscribed),
                                          New SqlParameter("@LastCheck", LastCheck))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                              ByVal UserGroup As String,
                              ByVal GroupID As Integer,
                              ByVal CustomId As String,
                              ByVal FeedName As String,
                              ByVal FeedDescription As String,
                              ByVal FeedURL As String,
                              ByVal FeedImageURL As String,
                              ByVal CheckInterval As Integer,
                              ByVal Subscribed As String,
                              ByVal FeedID As Integer)
                Dim CommandText As String = "UPDATE " & "[dbo].[" & UserGroup & "Feeds" & "]" & _
                    " SET GroupID=@GroupID,CustomId=@CustomId,FeedName=@FeedName, " & _
                    " FeedDescription=@FeedDescription,FeedURL=@FeedURL," & _
                    " FeedImageURL=@FeedImageURL,CheckInterval=@CheckInterval, " & _
                    "Subscribed=@Subscribed" & _
                    " WHERE FeedID=@FeedID"
                Dim custID As String = Nothing
                If CustomId.Trim() = "" Then
                    CustomId = 0
                End If
                If CInt(CustomId) > 0 Then
                    custID = CustomId.ToString()
                End If
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          CommandText,
                                          New SqlParameter("@FeedID", CInt(FeedID)),
                                          New SqlParameter("@GroupID", GroupID),
                                          New SqlParameter("@CustomId", CustomId),
                                          New SqlParameter("@FeedName", FeedName),
                                          New SqlParameter("@FeedDescription", FeedDescription),
                                          New SqlParameter("@FeedURL", FeedURL),
                                          New SqlParameter("@FeedImageURL", FeedImageURL),
                                          New SqlParameter("@CheckInterval", CheckInterval),
                                          New SqlParameter("@Subscribed", Subscribed))
                If (result > 0) Then ImmapUtil.GetInstance().IncreseSetting(DatabaseName, UserGroup, "ProfileSettings", "[SettingValue5]")
            End Sub

            Public Function CheckDuplicateName(ByVal database As String, ByVal UserGroup As String, ByVal Name As String) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(database),
                                                        "SELECT [FeedName] FROM [dbo].[" & UserGroup & "Feeds" & "]" &
                                                        " WHERE UPPER([FeedName]) LIKE UPPER(@FeedName);",
                                                        New SqlParameter("@FeedName", Name))
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
                                                        "SELECT [FeedName] FROM [dbo].[" & UserGroup & "Feeds" & "]" &
                                                        " WHERE UPPER([FeedName]) LIKE UPPER(@FeedName) AND FeedID<>@FeedID;",
                                                        New SqlParameter("@FeedName", Name), New SqlParameter("@FeedID", CInt(Id)))
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
