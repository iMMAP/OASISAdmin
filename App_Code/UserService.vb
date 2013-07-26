Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Model
Namespace Immap
    Namespace Service
        Public Class UserService
            Private Shared ReadOnly _instance As New UserService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property Instance() As UserService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property GetInstance() As UserService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String) As DataTable
                Dim dt As DataTable = Nothing
                Dim commandText As String = Nothing
                commandText = "SELECT  u.[id] as UserId," &
                              " [user] AS 'Username'," &
                              " [pwd] ," &
                              " [Fname] AS 'First Name'," &
                              " [Lname] AS 'Last Name'," &
                              " [SettingUrl]," &
                              " g.[Name] AS 'Groups'" & _
                              "FROM [users] u INNER JOIN [userGroups] as g ON g.id = u.UserGroupID; "
                Try
                    dt = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(DatabaseName),
                                                        commandText)
                Catch ex As Exception
                End Try

                Return dt
            End Function


            Public Function FindUserById(ByVal DatabaseName As String, ByVal UserId As String) As UserModel
                Dim dr As SqlDataReader = Nothing
                Dim user As UserModel = Nothing
                Dim commandText As String = "SELECT  u.[id] as UserId,"
                commandText &= " [user] AS 'Username',"
                commandText &= " [pwd],"
                commandText &= " [Fname] AS 'First Name',"
                commandText &= " [Lname] AS 'Last Name',"
                commandText &= " [SettingUrl],"
                commandText &= " g.[ID] AS 'Group ID',"
                commandText &= " g.[Name] As 'Groups'"
                commandText &= " FROM [users] u"
                commandText &= " INNER JOIN [userGroups] as g ON g.id = u.UserGroupID"
                commandText &= " WHERE u.[id]=@UserId"
                Try
                    dr = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(DatabaseName),
                                    commandText,
                                    New SqlParameter("@UserId", CInt(UserId)))
                    If dr.HasRows Then
                        dr.Read()
                        user = New UserModel()
                        user.UserID = dr.GetInt32(0)
                        user.UserName = dr.GetString(1)
                        user.FirstName = dr.GetString(3)
                        user.LastName = dr.GetString(4)
                        user.SettingURL = dr.GetString(5)
                        user.GroupID = dr.GetInt32(6)
                        user.GroupName = dr.GetString(7)
                    End If
                Catch ex As Exception
                End Try
                If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
                    dr.Close()
                    dr = Nothing
                End If
                Return user
            End Function

            Public Function CheckDuplicateUserName(ByVal DatabaseName As String,
                                                   ByVal Username As String,
                                                   Optional ByVal UserId As Integer = -1) As Boolean
                Dim isNotValid As Boolean = False
                Dim sqlreader As SqlDataReader = Nothing
                Dim commandText As String = "SELECT [user] FROM [Users] WHERE UPPER([user]) LIKE UPPER(@user)"
                Try
                    If (UserId <> -1) Then
                        commandText &= " AND id<>@Id"
                    End If
                    sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(DatabaseName),
                                                        commandText,
                                                        New SqlParameter("@user", "%" & Username & "%"),
                                                        New SqlParameter("@id", UserId))
                    If sqlreader.HasRows Then
                        isNotValid = True
                    End If
                Catch ex As Exception

                End Try
                If IsNothing(sqlreader) = False AndAlso sqlreader.IsClosed = False Then
                    sqlreader.Close()
                    sqlreader = Nothing
                End If
                Return isNotValid
            End Function

        End Class

    End Namespace
End Namespace