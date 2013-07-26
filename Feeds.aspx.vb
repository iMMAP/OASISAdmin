Imports Ext.Net
Imports Immap.Service
Partial Class Feeds
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "RSS"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadFeed()
            End If

        End If

    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadFeed()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim ID As String = e.ExtraParams("ID")
        Try
            FeedServices.GetInstance().DeleteById(database, userGroup, ID)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmFeeds.Reset()
        ReloadFeed()
        ReloadFeedGroup()
    End Sub

    Protected Sub FeedsStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadFeed()
    End Sub
    Protected Sub FeedGroupStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadFeedGroup()
    End Sub
    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
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
            If String.IsNullOrWhiteSpace(nbfFeedID.Text) Then
                msg.Show(New MessageBoxConfig() With {
                  .Title = "Warning",
                  .Message = "Please select RSS before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmFeeds.ClientID
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
              .AnimEl = Me.frmFeeds.ClientID
          })
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = FeedServices.GetInstance().CheckDuplicateName(database, userGroup, txtFeedName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This feed name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmFeeds.ClientID
                })
                Exit Sub
            End If
            If (String.IsNullOrWhiteSpace(dfLastCheck.RawValue)) Then
                FeedServices.GetInstance().Insert(database,
                                            userGroup,
                                            CInt(cboFeedGroups.SelectedItem.Value),
                                            If(String.IsNullOrWhiteSpace(nbfCustomId.Text) = True, "", nbfCustomId.Text),
                                            txtFeedName.Text,
                                            txtaFeedDescription.Text,
                                            txtFeedURL.Text,
                                            txtFeedImageURL.Text,
                                            If(String.IsNullOrWhiteSpace(nbfCheckInterval.Text) = True, 0.0, CDbl(nbfCheckInterval.Text)),
                                            txtSubscribed.Text)
            Else
                FeedServices.GetInstance().Insert(database,
                                            userGroup,
                                            CInt(cboFeedGroups.SelectedItem.Value),
                                            If(String.IsNullOrWhiteSpace(nbfCustomId.Text) = True, "", nbfCustomId.Text),
                                            txtFeedName.Text,
                                            txtaFeedDescription.Text,
                                            txtFeedURL.Text,
                                            txtFeedImageURL.Text,
                                            If(String.IsNullOrWhiteSpace(nbfCheckInterval.Text) = True, 0.0, CDbl(nbfCheckInterval.Text)),
                                            txtSubscribed.Text,
                                            If(String.IsNullOrWhiteSpace(dfLastCheck.RawValue) = True, Nothing, dfLastCheck.Value))

            End If
            frmFeeds.Reset()
            ReloadFeed()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = FeedServices.GetInstance().CheckDuplicateByNameAndId(database, userGroup, txtFeedName.Text.Trim(), nbfFeedID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This feed name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmFeeds.ClientID
                })
                Exit Sub
            End If
            If (String.IsNullOrWhiteSpace(dfLastCheck.RawValue)) Then
                FeedServices.GetInstance().Update(database,
                                           userGroup,
                                           CInt(cboFeedGroups.SelectedItem.Value),
                                           If(String.IsNullOrWhiteSpace(nbfCustomId.Text) = True, "", nbfCustomId.Text),
                                           txtFeedName.Text,
                                           txtaFeedDescription.Text,
                                           txtFeedURL.Text,
                                           txtFeedImageURL.Text,
                                           If(String.IsNullOrWhiteSpace(nbfCheckInterval.Text) = True, 0.0, CDbl(nbfCheckInterval.Text)),
                                           txtSubscribed.Text,
                                           CInt(nbfFeedID.Text))
            Else
                FeedServices.GetInstance().Update(database,
                                           userGroup,
                                           CInt(cboFeedGroups.SelectedItem.Value),
                                           If(String.IsNullOrWhiteSpace(nbfCustomId.Text) = True, "", nbfCustomId.Text),
                                           txtFeedName.Text,
                                           txtaFeedDescription.Text,
                                           txtFeedURL.Text,
                                           txtFeedImageURL.Text,
                                           If(String.IsNullOrWhiteSpace(nbfCheckInterval.Text) = True, 0.0, CDbl(nbfCheckInterval.Text)),
                                           txtSubscribed.Text,
                                           If(String.IsNullOrWhiteSpace(dfLastCheck.RawValue) = True, Nothing, dfLastCheck.Value),
                                           CInt(nbfFeedID.Text))
            End If
            frmFeeds.Reset()
            ReloadFeed()
        End If
    End Sub
    Public Sub ReloadFeed()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        FeedServices.GetInstance().FindAll(database, userGroup, FeedsStore)
        FeedGroupService.GetInstance().GetIdAndName(database, userGroup, FeedGroupStore)
        frmFeeds.Reset()
    End Sub

    Protected Sub ReloadFeedGroup()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        FeedGroupService.GetInstance().GetIdAndName(database, userGroup, FeedGroupStore)
    End Sub
End Class

