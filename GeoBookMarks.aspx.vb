Imports Ext.Net
Imports Immap.Service
Partial Class GeoBookMarks
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Geo Bookmarks"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadGeoBookMarks()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadGeoBookMarks()
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
            GeoBookMarkService.GetInstance().DeleteById(database, userGroup, ID, GUID1)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmGeoBookMarks.Reset()
        ReloadGeoBookMarks()
        ReloadGeoBookMarksGroup()
    End Sub

    Protected Sub GeoBookMarksStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadGeoBookMarks()
    End Sub

    Protected Sub GeoBookMarksGroupStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadGeoBookMarksGroup()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub ReloadGeoBookMarks()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        GeoBookMarkService.GetInstance().RunPrepareGeoBookMarkAndCategories(database)
        GeoBookMarkService.GetInstance().FindAll(database, userGroup, GeoBookMarksStore)
        GeoBookMarkCategoriesService.GetInstance().GetIdAndName(database, userGroup, GeoBookMarksGroupStore)
    End Sub

    Public Sub ReloadGeoBookMarksGroup()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        GeoBookMarkCategoriesService.GetInstance().GetIdAndName(database, userGroup, GeoBookMarksGroupStore)
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
                  .Message = "Please select Geo bookmark before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmGeoBookMarks.ClientID
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
              .AnimEl = Me.frmGeoBookMarks.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmGeoBookMarks.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtDescription.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter description",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmGeoBookMarks.ClientID
          })
            Exit Sub
        End If

        Dim x As String
        If String.IsNullOrWhiteSpace(nbfX.Text) Then
            x = ""
        Else
            x = CDbl(nbfX.Text).ToString()
        End If
        Dim y As String
        If String.IsNullOrWhiteSpace(nbfY.Text) Then
            y = ""
        Else
            y = CDbl(nbfY.Text).ToString()
        End If
        Dim z As String
        If String.IsNullOrWhiteSpace(nbfZ.Text) Then
            z = ""
        Else
            z = CDbl(nbfZ.Text).ToString()
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = GeoBookMarkService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This geo bookmark name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGeoBookMarks.ClientID
                })
                Exit Sub
            End If
            GeoBookMarkService.GetInstance().Insert(database,
                                                    userGroup,
                                                    txtName.Text,
                                                    x,
                                                    y,
                                                    z,
                                                    txtDescription.Text,
                                                    If(chkUseSymbol.Checked = True, "1", "0"),
                                                    txtSymbolChar.Text,
                                                    txtSymbolFont.Text,
                                                    txtSymbolSize.Text,
                                                    txtMapName.Text,
                                                    CInt(cboGeoBookMarksGroups.SelectedItem.Value),
                                                    ImmapUtil.NewGUid(),
                                                    txtOwnerGUID.Text,
                                                    If(chkDeleted.Checked = True, "1", "0"),
                                                    If(chkisURLMark.Checked = True, "1", "0"),
                                                    txtasURL.Text)
            frmGeoBookMarks.Reset()
            ReloadGeoBookMarks()
            ReloadGeoBookMarksGroup()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = GeoBookMarkService.GetInstance().CheckDuplicateByNameAndId(database, userGroup, txtName.Text.Trim(), nbfId.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This geo bookmark name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGeoBookMarks.ClientID
                })
                Exit Sub
            End If
            GeoBookMarkService.GetInstance().Update(database,
                                                    userGroup,
                                                    txtName.Text,
                                                    x,
                                                    y,
                                                    z,
                                                    txtDescription.Text,
                                                    If(chkUseSymbol.Checked = True, "1", "0"),
                                                    txtSymbolChar.Text,
                                                    txtSymbolFont.Text,
                                                    txtSymbolSize.Text,
                                                    txtMapName.Text,
                                                    CInt(cboGeoBookMarksGroups.SelectedItem.Value),
                                                    txtsGUID.Text,
                                                    txtOwnerGUID.Text,
                                                    If(chkDeleted.Checked = True, "1", "0"),
                                                    If(chkisURLMark.Checked = True, "1", "0"),
                                                    txtasURL.Text,
                                                    CInt(nbfId.Text))
            frmGeoBookMarks.Reset()
            ReloadGeoBookMarks()
            ReloadGeoBookMarksGroup()
        End If
    End Sub
End Class
