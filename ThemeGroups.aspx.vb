﻿Imports Ext.Net
Imports System.Data.Sql
Imports System.Data
Imports System.Data.SqlClient
Imports Immap.Service
Partial Class ThemeGroups
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            Session("TITLE") = "Theme groups"
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.SelectedItem.Text = Session("cboUserGroupText")
                cboUserGroups.DataBind()
                ReloadThemeGroup()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadThemeGroup()
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim ID As String = e.ExtraParams("ID")
        Dim cnt As Integer = ThemeGroupService.GetInstance.CountThemesByGroupId(database, userGroup, ID)
        If cnt > 0 Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please delete all themes that belong to this theme group",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmThemeGroups.ClientID
          })
            Exit Sub
        End If
        Try
            ThemeGroupService.GetInstance().DeleteById(database, userGroup, ID)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmThemeGroups.Reset()
        ReloadThemeGroup()
    End Sub

    Protected Sub ThemeGroupsStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadThemeGroup()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub ReloadThemeGroup()
        Dim database As String = Convert.ToString(Session("database"))
        Dim userGroup As String = cboUserGroups.SelectedItem.Text
        ThemeGroupService.GetInstance().FindAll(database, userGroup, ThemeGroupsStore)
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
                  .Message = "Please select Theme group before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmThemeGroups.ClientID
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
              .AnimEl = Me.frmThemeGroups.ClientID
          })
        End If

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter name",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
            .AnimEl = Me.frmThemeGroups.ClientID
          })
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtDescription.Text) Then
            msg.Show(New MessageBoxConfig() With {
                .Title = "Warning",
                .Message = "Please enter description",
                .Buttons = MessageBox.Button.OK,
                .Icon = MessageBox.Icon.WARNING,
              .AnimEl = Me.frmThemeGroups.ClientID
          })
            Exit Sub
        End If
        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = ThemeGroupService.GetInstance().CheckDuplicateName(database, userGroup, txtName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This theme group name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmThemeGroups.ClientID
                })
                Exit Sub
            End If
            ThemeGroupService.GetInstance().Insert(database, userGroup, txtName.Text, txtDescription.Text)
            frmThemeGroups.Reset()
            ReloadThemeGroup()
            Dim sm As RowSelectionModel = TryCast(Me.gpThemeGroups.SelectionModel.Primary, RowSelectionModel)
            sm.ClearSelections()
            gpThemeGroups.Call("clearMemory")
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = ThemeGroupService.GetInstance().CheckDuplicateByNameAndId(database, userGroup, txtName.Text.Trim(), txtID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This theme group name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmThemeGroups.ClientID
                })
                Exit Sub
            End If
            ThemeGroupService.GetInstance().Update(database, userGroup, txtName.Text, txtDescription.Text, Me.txtID.Text)
            frmThemeGroups.Reset()
            ReloadThemeGroup()
            Dim sm As RowSelectionModel = TryCast(Me.gpThemeGroups.SelectionModel.Primary, RowSelectionModel)
            sm.ClearSelections()
            gpThemeGroups.Call("clearMemory")
        End If
    End Sub
End Class
