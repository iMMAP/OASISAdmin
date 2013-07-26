Imports Immap.Service
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Page.IsPostBack = False Then
            Session("TITLE") = "Home"
        End If
    End Sub
End Class
