Imports System.Data
Imports System.Data.SqlClient
Imports Ext.Net
Imports Immap.Service
Imports Immap.Model
Partial Class WebTiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        If String.IsNullOrEmpty(database) = True Then
            Session("RedirectURL") = Request.Url.AbsoluteUri
            Dim cookieDetails = Request.Cookies("ADMINTOOL")
            If cookieDetails IsNot Nothing Then
                Response.Cookies.Add(cookieDetails)
            End If
            Response.Redirect("~/Login")
        End If
        InitImageFomat()
        If Not (Page.IsPostBack) AndAlso Not (RequestManager.IsAjaxRequest) Then
            Session("TITLE") = "Web Titles"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadWebTiles()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadWebTiles()
        Me.frmWebTiles.Reset()
    End Sub


    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim Caption As String = e.ExtraParams("Caption")
        If ID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of Web tilels is empty"
            Exit Sub
        End If
        Try
            WebTilesService.GetInstance().DeleteByCaption(database, userGroup, Caption)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmWebTiles.Reset()
        ReloadWebTiles()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub WebTilesStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadWebTiles()
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

        If String.IsNullOrEmpty(userGroup) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please select user group",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmWebTiles.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtCaption.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter caption",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmWebTiles.ClientID
          })
            Exit Sub
        End If
        Dim espgNumber As String = Nothing
        If String.IsNullOrWhiteSpace(nbfESPGNumber.Text) = False Then espgNumber = CInt(nbfESPGNumber.Text).ToString()
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = WebTilesService.GetInstance().CheckDuplicateCaption(database, userGroup, txtCaption.Text.Trim(), txtHiddenCaption.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This caption is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmWebTiles.ClientID
                })
                Exit Sub
            End If
            WebTilesService.GetInstance().Insert(database,
                                               userGroup,
                                               txtCaption.Text,
                                               txtURL1.Text,
                                               txtURL2.Text,
                                               txtURL3.Text,
                                                espgNumber,
                                               cboImageFormat.Text.ToLower(),
                                               If(chkForceWGS.Checked = True, "1", "0"))
            frmWebTiles.Reset()
            ReloadWebTiles()
            Dim sm As RowSelectionModel = TryCast(Me.gpWebTiles.SelectionModel.Primary, RowSelectionModel)
            sm.ClearSelections()
            gpWebTiles.Call("clearMemory")
        ElseIf command.Equals("UPDATE") Then
            If String.IsNullOrEmpty(txtHiddenCaption.Text) = True Then
                msg.Show(New MessageBoxConfig() With {
                     .Title = "Warning",
                     .Message = "Please insert data before update it.",
                     .Buttons = MessageBox.Button.OK,
                     .Icon = MessageBox.Icon.WARNING,
                    .AnimEl = Me.frmWebTiles.ClientID
              })
                Exit Sub
            End If
            Dim isDuplicate As Boolean = WebTilesService.GetInstance().CheckDuplicateCaption(database, userGroup, txtCaption.Text.Trim(), txtHiddenCaption.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This caption is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmWebTiles.ClientID
                })
                Exit Sub
            End If
            WebTilesService.GetInstance().Update(database,
                                             userGroup,
                                             txtCaption.Text,
                                             txtHiddenCaption.Text,
                                             txtURL1.Text,
                                             txtURL2.Text,
                                             txtURL3.Text,
                                              espgNumber,
                                             cboImageFormat.Text.ToLower(),
                                             If(chkForceWGS.Checked = True, "1", "0"))
            frmWebTiles.Reset()
            ReloadWebTiles()
            Dim sm As RowSelectionModel = TryCast(Me.gpWebTiles.SelectionModel.Primary, RowSelectionModel)
            sm.ClearSelections()
            gpWebTiles.Call("clearMemory")
        End If
    End Sub

    Protected Sub InitImageFomat()
        Dim store = Me.cboImageFormat.GetStore()
        store.DataSource = New Object() {
            New Object() {"png", "PNG"},
            New Object() {"jpg", "JPG"}
        }
        store.DataBind()
    End Sub

    Protected Sub ReloadWebTiles()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        WebTilesService.GetInstance().RunPrepareUserGroupDatabaseScript(database, userGroup)
        WebTilesService.GetInstance().FindAll(database, cboUserGroups.SelectedItem.Text, WebTilesStore)
    End Sub
End Class
