Imports Ext.Net
Imports Immap.Service
Partial Class GeoBookMarksCategories
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Geo Bookmark Groups"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadGeoBookMarksCategories()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadGeoBookMarksCategories()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim ID As String = e.ExtraParams("ID")
        Dim GUID1 As String = e.ExtraParams("GUID1")
        If ID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of user is empty"
            Exit Sub
        End If
        Try
            GeoBookMarkCategoriesService.GetInstance().DeleteById(database, userGroup, ID, GUID1)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmGeoBookMarksCategories.Reset()
        ReloadGeoBookMarksCategories()

    End Sub

    Protected Sub GeoBookMarksCategoriesStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadGeoBookMarksCategories()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub ReloadGeoBookMarksCategories()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        GeoBookMarkService.GetInstance().RunPrepareGeoBookMarkAndCategories(database)
        GeoBookMarkCategoriesService.GetInstance().FindAll(database, userGroup, GeoBookMarksCategoriesStore)
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
            If String.IsNullOrWhiteSpace(nbfId.Text) Then
                msg.Show(New MessageBoxConfig() With {
                  .Title = "Warning",
                  .Message = "Please select Geo bookmark category before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmGeoBookMarksCategories.ClientID
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
              .AnimEl = Me.frmGeoBookMarksCategories.ClientID
          })
        End If
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmGeoBookMarksCategories.ClientID
          })
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtDescription.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter description",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmGeoBookMarksCategories.ClientID
          })
            Exit Sub
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = GeoBookMarkCategoriesService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This geo bookmark category name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGeoBookMarksCategories.ClientID
                })
                Exit Sub
            End If
            GeoBookMarkCategoriesService.GetInstance().Insert(database,
                                                              userGroup,
                                                              txtName.Text,
                                                              txtDescription.Text,
                                                              ImmapUtil.NewGUid())
            frmGeoBookMarksCategories.Reset()
            ReloadGeoBookMarksCategories()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = GeoBookMarkCategoriesService.GetInstance().CheckDuplicateByNameAndSguid(database, userGroup, txtName.Text.Trim(), txtsGUID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This geo bookmark category name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGeoBookMarksCategories.ClientID
                })
                Exit Sub
            End If
            GeoBookMarkCategoriesService.GetInstance().Update(database,
                                                         userGroup,
                                                         txtName.Text,
                                                         txtDescription.Text,
                                                         txtsGUID.Text,
                                                         CInt(nbfId.Text))
            frmGeoBookMarksCategories.Reset()
            ReloadGeoBookMarksCategories()
        End If
    End Sub
End Class
