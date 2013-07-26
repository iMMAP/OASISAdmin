Imports Ext.Net
Imports Immap.Service
Partial Class FeedGroups
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "RSS Groups"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadFeedGroup()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadFeedGroup()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim ID As String = e.ExtraParams("ID")
        Dim cnt As Integer = FeedGroupService.GetInstance.CountFeedsByGroupId(database, userGroup, ID)
        If cnt > 0 Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please delete all feeds that belong to this feed group",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmFeedGroups.ClientID
          })
            Exit Sub
        End If
        Try
            FeedGroupService.GetInstance().DeleteById(database, userGroup, ID)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmFeedGroups.Reset()
        ReloadFeedGroup()
    End Sub

    Protected Sub FeedGroupsStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadFeedGroup()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub ReloadFeedGroup()
        Dim database As String = Convert.ToString(Session("database"))
        If String.IsNullOrWhiteSpace(cboUserGroups.SelectedItem.Text) Then
            Exit Sub
        End If
        FeedGroupService.GetInstance().FindAll(database, cboUserGroups.SelectedItem.Text, FeedGroupsStore)
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As DirectEventArgs)
        InsertOrUpdate("INSERT")
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As DirectEventArgs)
        InsertOrUpdate("UPDATE")
    End Sub

    Protected Sub InsertOrUpdate(ByVal command As String)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        If command.Equals("UPDATE") Then
            If String.IsNullOrWhiteSpace(txtID.Text) = True Then
                msg.Show(New MessageBoxConfig() With {
                    .Title = "Warning",
                    .Message = "Please select RSS group",
                    .Buttons = MessageBox.Button.OK,
                    .Icon = MessageBox.Icon.WARNING,
                .AnimEl = Me.frmFeedGroups.ClientID
              })
                Exit Sub
            End If
        End If

        If String.IsNullOrEmpty(userGroup) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please select user group",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmFeedGroups.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmFeedGroups.ClientID
          })
            Exit Sub
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = FeedGroupService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This feed group name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmFeedGroups.ClientID
                })
                Exit Sub
            End If
            FeedGroupService.GetInstance().Insert(database, userGroup, txtName.Text, If(chkCustomGroup.Checked = True, "1", "0"))
            frmFeedGroups.Reset()
            ReloadFeedGroup()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = FeedGroupService.GetInstance().CheckDuplicateByNameAndId(database, userGroup, txtName.Text.Trim(), txtID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This feed group name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmFeedGroups.ClientID
                })
                Exit Sub
            End If
            FeedGroupService.GetInstance().Update(database, userGroup, txtName.Text, If(chkCustomGroup.Checked = True, "1", "0"), Me.txtID.Text)
            frmFeedGroups.Reset()
            ReloadFeedGroup()
        End If
    End Sub
End Class
