Imports System.Data.SqlClient
Imports System.Data
Imports Ext.Net
Imports ImmapUtil
Imports Immap.Service
Partial Class User
    Inherits System.Web.UI.Page
    'Private sql As New SQLUtil()
    Public Const ASCENDING As String = " ASC"
    Public Const DESCENDING As String = " DESC"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Users"
            ReloadUsers()
            UserGroupService.GetInstance().LoadUserGroupStore(Convert.ToString(Session("database")), UserGroupStore)
        End If
    End Sub

    Protected Sub Store1_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        ReloadUsers()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        UserGroupService.GetInstance().LoadUserGroupStore(Convert.ToString(Session("database")), UserGroupStore)
    End Sub

    Protected Sub RowSelect(sender As Object, e As DirectEventArgs)
        Dim UserID As String = e.ExtraParams("UserId")
        Dim User = UserService.GetInstance().FindUserById(Session("database"), CInt(UserID))
        Me.FormPanel1.SetValues(New With {User.UserID, User.UserName, User.FirstName, User.LastName, User.SettingURL, User.GroupID})
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim UserID As String = e.ExtraParams("UserId")
        If UserID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of user is empty"
            Exit Sub
        End If
        Try
            SQLHelper.ExecuteNonQuery(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                                          "DELETE FROM Users WHERE [ID] = @ID",
                                          New SqlParameter("@ID", CInt(UserID)))
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        ReloadUsers()
        FormPanel1.Reset()
    End Sub

    Protected Sub CheckPassword(sender As Object, e As RemoteValidationEventArgs)
        If String.Equals(txtPassword.Text, txtConfirmPassword.Text) = True Then
            e.Success = True
        Else
            e.Success = False
            e.ErrorMessage = "The password and confirm password must be the same."
        End If

    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        e.Success = Me.InsertOrUpdate(ImmapUtil.SaveType.INSERT)
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        If (String.IsNullOrEmpty(txtId.Text)) Then
            Dim msg As New MessageBox()
            msg.Show(New MessageBoxConfig() With {
                 .Title = "Warning",
                 .Message = "Please select user",
                 .Buttons = MessageBox.Button.OK,
                 .Icon = MessageBox.Icon.WARNING,
                .AnimEl = Me.FormPanel1.ClientID
          })
            Exit Sub
        End If
        e.Success = Me.InsertOrUpdate(ImmapUtil.SaveType.UPDATE, txtId.Text)
    End Sub

    Protected Function InsertOrUpdate(ByVal saveType As ImmapUtil.SaveType, Optional ByVal userId As Integer = -1) As Boolean
        Dim commandText As String = Nothing
        Dim isDuplicate As Boolean = UserService.GetInstance().CheckDuplicateUserName(Session("database"), txtUserName.Text, userId)
        If isDuplicate Then
            Dim msg As New MessageBox()
            msg.Show(New MessageBoxConfig() With {
                   .Title = "Warning",
                   .Message = "This username is duplicated",
                   .Buttons = MessageBox.Button.OK,
                   .Icon = MessageBox.Icon.WARNING,
                  .AnimEl = Me.FormPanel1.ClientID
            })
            Return True
        End If
        Dim sGUID As String = ""
        sGUID = ImmapUtil.NewGUid()
        Try
            If (saveType = ImmapUtil.SaveType.INSERT) Then
                commandText = "INSERT INTO [Users] ([user],[pwd],[Fname],[Lname],[SettingUrl],[UserGroupID],[sGUID]) VALUES (@user,@pwd,@Fname,@Lname,@SettingUrl,@UserGroupID,@sGUID);"
            ElseIf (saveType = ImmapUtil.SaveType.UPDATE) Then
                commandText = "UPDATE [Users] SET [user]=@user,[pwd]=@pwd,[Fname]=@Fname,[Lname]=@Lname,[SettingUrl]=@SettingUrl,[UserGroupID]=@UserGroupID WHERE ID=@userId"
            End If
            SQLHelper.ExecuteNonQuery(ImmapUtil.getConnectionStringByDatabase(Session("database").ToString()),
                                      commandText,
                                    New SqlParameter("@user", txtUserName.Text),
                                    New SqlParameter("@pwd", ImmapUtil.Instance.CalculateMD5(txtConfirmPassword.Text)),
                                    New SqlParameter("@Fname", txtFirstName.Text),
                                    New SqlParameter("@Lname", txtLastName.Text),
                                    New SqlParameter("@SettingUrl", txtSettingURL.Text),
                                    New SqlParameter("@UserGroupID", CInt(cboUserGroups.SelectedItem.Value)),
                                    New SqlParameter("@sGUID", sGUID),
                                    New SqlParameter("@userId", userId))

        Catch ex As Exception
            Return False
        End Try
        ReloadUsers()
        Me.FormPanel1.Reset()
        Return True
    End Function
    Protected Sub btnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Dim isDuplicate As Boolean = UserService.GetInstance().CheckDuplicateUserName(Session("database"), txtUserName.Text)
        Dim msg As New MessageBox()
        If isDuplicate Then
            msg.Show(New MessageBoxConfig() With {
                   .Title = "Warning",
                   .Message = "This username is duplicated",
                   .Buttons = MessageBox.Button.OK,
                   .Icon = MessageBox.Icon.WARNING,
                  .AnimEl = Me.FormPanel1.ClientID
            })
            Exit Sub
        End If

        Dim commandText As String = "INSERT INTO [Users] " & _
          "([user],[pwd],[Fname],[Lname],[SettingUrl],[UserGroupID],[sGUID])" & _
          " VALUES (@user,@pwd,@Fname,@Lname,@SettingUrl,@UserGroupID,@sGUID);"
        Try
            SQLHelper.ExecuteNonQuery(ImmapUtil.getConnectionStringByDatabase(Session("database").ToString()),
                                      commandText,
                                    New SqlParameter("@user", txtUserName.Text),
                                    New SqlParameter("@pwd", ImmapUtil.Instance.CalculateMD5(txtConfirmPassword.Text)),
                                    New SqlParameter("@Fname", txtFirstName.Text),
                                    New SqlParameter("@Lname", txtLastName.Text),
                                    New SqlParameter("@SettingUrl", txtSettingURL.Text),
                                    New SqlParameter("@UserGroupID", CInt(cboUserGroups.SelectedItem.Value)),
                                    New SqlParameter("@sGUID", ImmapUtil.NewGUid()))
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        ReloadUsers()
        Me.FormPanel1.Reset()
    End Sub

    Protected Sub ReloadUsers()
        Me.Store1.DataSource = UserService.GetInstance.FindAll(Session("database").ToString())
        Me.Store1.DataBind()
    End Sub
End Class