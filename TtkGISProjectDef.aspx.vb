Imports Ext.Net
Imports Immap.Service
Partial Class TtkGISProjectDef
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "Map Library"
            UserGroupService.GetInstance().LoadUserGroupStore(database, UserGroupStore)
            If Not (Session("cboUserGroupValue") Is Nothing) Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.DataBind()
                ReloadTtkGISProjectDef()
            End If
        End If
    End Sub

    Protected Sub cboUserGroups_Select(sender As Object, e As Ext.Net.DirectEventArgs)
        Session("cboUserGroupValue") = cboUserGroups.SelectedItem.Value
        Session("cboUserGroupText") = cboUserGroups.SelectedItem.Text
        ReloadTtkGISProjectDef()
    End Sub

    Protected Sub TtkGISProjectDefStore_Refresh(sender As Object, e As Ext.Net.StoreRefreshDataEventArgs)
        ReloadTtkGISProjectDef()
    End Sub

    Public Sub ReloadTtkGISProjectDef()
        Try
            Dim database As String = Convert.ToString(Session("database"))
            If String.IsNullOrWhiteSpace(cboUserGroups.SelectedItem.Text) = True Then
                cboUserGroups.SetValue(Session("cboUserGroupsSelectedItem"))
                cboUserGroups.DataBind()
            End If
            Dim userGroup As String = cboUserGroups.SelectedItem.Text

            TtkGISProjectDefService.GetInstance().FindAll(database, userGroup, Me.TtkGISProjectDefStore)
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        If Session("database") Is Nothing Then
            Exit Sub
        End If
    End Sub

    Protected Sub RowDelete(sender As Object, e As DirectEventArgs)
        Dim database As String = Convert.ToString(Session("database"))
        Dim msg As New MessageBox()
        Dim userGroup As String = cboUserGroups.SelectedItem.Text

        Dim sGUID As String = e.ExtraParams("sGUID")
        If sGUID Is Nothing Then
            e.Success = False
            e.ErrorMessage = "The list of ID is empty"
            Exit Sub
        End If
        Try
            TtkGISProjectDefService.GetInstance().DeleteById(database, userGroup, sGUID)
            e.Success = True
        Catch ex As Exception
            e.Success = False
        End Try
        frmTtkGISProjectDef.Reset()
        ReloadTtkGISProjectDef()
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
        If String.IsNullOrWhiteSpace(txtsGUID.Text) Then

        End If
        If command.Equals("UPDATE") Then
            If String.IsNullOrWhiteSpace(txtsGUID.Text) Then
                msg.Show(New MessageBoxConfig() With {
                  .Title = "Warning",
                  .Message = "Please select Map library before update",
                  .Buttons = MessageBox.Button.OK,
                  .Icon = MessageBox.Icon.WARNING,
                 .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
        End If



        Dim centerX As String = ""
        Dim centerY As String = ""
        Dim EPSG As String = ""
        Dim isNum As Boolean = False
        Dim Num As Double

        If String.IsNullOrEmpty(txtXMAX.Text.Trim()) = False Then
            isNum = Double.TryParse(txtXMAX.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on XMAX",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
        End If

        If String.IsNullOrEmpty(txtXMIN.Text.Trim()) = False Then
            isNum = Double.TryParse(txtXMIN.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on XMIN",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
        End If

        If String.IsNullOrEmpty(txtYMAX.Text.Trim()) = False Then
            isNum = Double.TryParse(txtYMAX.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on YMAX",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
        End If

        If String.IsNullOrEmpty(txtYMIN.Text.Trim()) = False Then
            isNum = Double.TryParse(txtYMIN.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on YMIN",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
        End If

        If String.IsNullOrEmpty(txtCenterX.Text.Trim()) = False Then
            isNum = Double.TryParse(txtCenterX.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on CenterX",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            Else
                centerX = Num.ToString()
            End If
        End If

        If String.IsNullOrEmpty(txtCenterY.Text.Trim()) = False Then
            isNum = Double.TryParse(txtCenterY.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on center Y",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            Else
                centerY = Num.ToString()
            End If
        End If

        If String.IsNullOrEmpty(txtEPSG.Text.Trim()) = False Then
            isNum = Double.TryParse(txtEPSG.Text, Num)
            If isNum = False Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "Please enter number only on EPSG",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            Else
                EPSG = Num.ToString()
            End If
        End If
        Dim CreatedDate As String = txtCreateDate.Text
        'If String.IsNullOrEmpty(dfCreateDate.RawText.Trim()) = False Then
        '    CreatedDate = dfCreateDate.RawText
        'End If

        If command.Equals("INSERT") Then
            Dim isDuplicate As Boolean = TtkGISProjectDefService.GetInstance().CheckDuplicateName(database, userGroup, txtsName.Text.Trim())
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
            TtkGISProjectDefService.GetInstance().Insert(database, userGroup,
                                                        If(chkInUse.Checked = True, 1, 0),
                                                        txtsName.Text,
                                                        txtaMapData.Text,
                                                        ImmapUtil.NewGUid(),
                                                        If(String.IsNullOrWhiteSpace(txtXMIN.Text) = True, 0.0, CDbl(txtXMIN.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtXMAX.Text) = True, 0.0, CDbl(txtXMAX.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtYMIN.Text) = True, 0.0, CDbl(txtYMIN.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtYMAX.Text) = True, 0.0, CDbl(txtYMAX.Text)),
                                                        txtSInfo.Text,
                                                        If(chkbSavedToDB.Checked = True, 1, 0),
                                                        txtsFilePath.Text,
                                                        centerX,
                                                        centerY,
                                                        EPSG,
                                                        txtScale.Text,
                                                        txtCreateBy.Text,
                                                        CreatedDate,
                                                        txtDescription.Text,
                                                        txtContact.Text,
                                                        txtRestrictions.Text,
                                                        txtCopyright.Text,
                                                        txturl.Text,
                                                        txtStandardLyrs.Text,
                                                        txtSource.Text,
                                                        txtAdminLyr1Name.Text,
                                                        txtAdminLyr2Name.Text,
                                                        txtAdminLyr3Name.Text,
                                                        txtAdminLyr4Name.Text,
                                                        txtAdminLyr5Name.Text)
            frmTtkGISProjectDef.Reset()
            ReloadTtkGISProjectDef()
        ElseIf command.Equals("UPDATE") Then
            Dim isDuplicate As Boolean = TtkGISProjectDefService.GetInstance().CheckDuplicateByNameAndSguid(database, userGroup, txtsName.Text.Trim(), txtsGUID.Text)
            If isDuplicate Then
                msg.Show(New MessageBoxConfig() With {
                       .Title = "Warning",
                       .Message = "This map library name is duplicated",
                       .Buttons = MessageBox.Button.OK,
                       .Icon = MessageBox.Icon.WARNING,
                      .AnimEl = Me.frmTtkGISProjectDef.ClientID
                })
                Exit Sub
            End If
            TtkGISProjectDefService.GetInstance().Update(database, userGroup,
                                                        If(chkInUse.Checked = True, 1, 0),
                                                        txtsName.Text,
                                                        txtaMapData.Text,
                                                        txtsGUID.Text,
                                                        If(String.IsNullOrWhiteSpace(txtXMIN.Text) = True, 0.0, CDbl(txtXMIN.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtXMAX.Text) = True, 0.0, CDbl(txtXMAX.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtYMIN.Text) = True, 0.0, CDbl(txtYMIN.Text)),
                                                        If(String.IsNullOrWhiteSpace(txtYMAX.Text) = True, 0.0, CDbl(txtYMAX.Text)),
                                                        txtSInfo.Text,
                                                        If(chkbSavedToDB.Checked = True, 1, 0),
                                                        txtsFilePath.Text,
                                                        centerX,
                                                        centerY,
                                                        EPSG,
                                                        txtScale.Text,
                                                        txtCreateBy.Text,
                                                        CreatedDate,
                                                        txtDescription.Text,
                                                        txtContact.Text,
                                                        txtRestrictions.Text,
                                                        txtCopyright.Text,
                                                        txturl.Text,
                                                        txtStandardLyrs.Text,
                                                        txtSource.Text,
                                                        txtAdminLyr1Name.Text,
                                                        txtAdminLyr2Name.Text,
                                                        txtAdminLyr3Name.Text,
                                                        txtAdminLyr4Name.Text,
                                                        txtAdminLyr5Name.Text)
            frmTtkGISProjectDef.Reset()
            ReloadTtkGISProjectDef()
        End If
    End Sub
End Class
