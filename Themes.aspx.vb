Imports Ext.Net
Imports System.Data.Sql
Imports Immap.Service
Partial Class Themes
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Themes"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadTheme()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadTheme()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim ID As String = e.ExtraParams("ID")
        If ID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of this theme is empty"
            Exit Sub
        End If
        Try
            ThemeService.GetInstance().DeleteById(database, userGroup, ID)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmThemes.Reset()
        ReloadTheme()
        ReloadThemeGroup()
    End Sub

    Protected Sub ThemesStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadTheme()
    End Sub

    Protected Sub ThemeGroupStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadThemeGroup()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
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
            If String.IsNullOrWhiteSpace(txtID.Text) Then
                msg.Show(New MessageBoxConfig() With {
                  .Title = "Warning",
                  .Message = "Please select Theme before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmThemes.ClientID
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
              .AnimEl = Me.frmThemes.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmThemes.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtDescription.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter description",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmThemes.ClientID
          })
            Exit Sub
        End If

        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = ThemeService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This theme name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmThemes.ClientID
                })
                Exit Sub
            End If
            ThemeService.GetInstance().Insert(database,
                                               userGroup,
                                               txtName.Text,
                                               CInt(cboThemeGroups.SelectedItem.Value),
                                               txtDescription.Text,
                                               txtAnalysisLayer.Text,
                                               txtMap.Text,
                                               txtAnalysisLayer.Text,
                                               txtThemeConfigName.Text)
            frmThemes.Reset()
            ReloadTheme()
            ReloadThemeGroup()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = ThemeService.GetInstance().CheckDuplicateByNameAndId(database, userGroup, txtName.Text.Trim(), txtID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This theme name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmThemes.ClientID
                })
                Exit Sub
            End If
            ThemeService.GetInstance().Update(database,
                                         userGroup,
                                         txtName.Text,
                                         CInt(cboThemeGroups.SelectedItem.Value),
                                         txtDescription.Text,
                                         txtAnalysisLayer.Text,
                                         txtMap.Text,
                                         txtAnalysisLayer.Text,
                                         txtThemeConfigName.Text,
                                         txtID.Text)
            frmThemes.Reset()
            ReloadTheme()
            ReloadThemeGroup()
        End If
    End Sub
    Public Sub ReloadTheme()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        ThemeService.GetInstance().FindAll(database, userGroup, ThemesStore)
        ThemeGroupService.GetInstance().GetIdAndName(database, userGroup, ThemeGroupStore)
        frmThemes.Reset()
    End Sub

    Public Sub ReloadThemeGroup()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        ThemeGroupService.GetInstance().GetIdAndName(database, userGroup, ThemeGroupStore)
    End Sub
End Class
