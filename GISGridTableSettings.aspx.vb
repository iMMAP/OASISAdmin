Imports Ext.Net
Imports Immap.Service
Partial Class GISGridTableSettings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Data View"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadGridTableSettings()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        ReloadGridTableSettings()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim id As String = e.ExtraParams("id")
        If id Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of user is empty"
            Exit Sub
        End If
        Try
            GISGridTableSettingsService.GetInstance().DeleteById(database, userGroup, id)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmGISGridTableSettings.Reset()
        ReloadGridTableSettings()
    End Sub

    Protected Sub GISGridTableSettingsStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadGridTableSettings()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Public Sub ReloadGridTableSettings()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        GISGridTableSettingsService.GetInstance().FindAll(database, userGroup, GISGridTableSettingsStore)
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
            If String.IsNullOrWhiteSpace(txtId.Text) Then
                msg.Show(New MessageBoxConfig() With {
                  .Title = "Warning",
                  .Message = "Please select GIS Grid Table setting before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmGISGridTableSettings.ClientID
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
              .AnimEl = Me.frmGISGridTableSettings.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmGISGridTableSettings.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtAlias.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter alia",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmGISGridTableSettings.ClientID
          })
            Exit Sub
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = GISGridTableSettingsService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This gis grid table name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGISGridTableSettings.ClientID
                })
                Exit Sub
            End If
            GISGridTableSettingsService.GetInstance().Insert(database, userGroup,
                                                       txtName.Text,
                                                       txtAlias.Text,
                                                       If(chkVisible.Checked = True, 1, 0),
                                                       If(chkDatasetwarning.Checked = True, 1, 0),
                                                       If(String.IsNullOrWhiteSpace(nbfWarninglevel.Text) = True, 0, CInt(nbfWarninglevel.Text)),
                                                       If(String.IsNullOrWhiteSpace(nbfMaxRec.Text) = True, 0, CInt(nbfMaxRec.Text)),
                                                       txtaExcludedFlds.Text,
                                                       If(chkIsURLLayer.Checked = True, 1, 0),
                                                       If(chkautoRunUrls.Checked = True, 1, 0),
                                                       txtaURLLayerField.Text)
            frmGISGridTableSettings.Reset()
            ReloadGridTableSettings()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = GISGridTableSettingsService.GetInstance().CheckDuplicateByNameAndAliasAndId(database, userGroup, txtName.Text.Trim(), txtAlias.Text, txtId.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This gis grid table name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmGISGridTableSettings.ClientID
                })
                Exit Sub
            End If
            GISGridTableSettingsService.GetInstance().Update(database, userGroup,
                                                    txtName.Text,
                                                    txtAlias.Text,
                                                    If(chkVisible.Checked = True, 1, 0),
                                                    If(chkDatasetwarning.Checked = True, 1, 0),
                                                    If(String.IsNullOrWhiteSpace(nbfWarninglevel.Text) = True, 0, CInt(nbfWarninglevel.Text)),
                                                    If(String.IsNullOrWhiteSpace(nbfMaxRec.Text) = True, 0, CInt(nbfMaxRec.Text)),
                                                    txtaExcludedFlds.Text,
                                                    If(chkIsURLLayer.Checked = True, 1, 0),
                                                    If(chkautoRunUrls.Checked = True, 1, 0),
                                                    txtaURLLayerField.Text,
                                                    txtId.Text)
            frmGISGridTableSettings.Reset()
            ReloadGridTableSettings()
        End If
    End Sub
End Class
