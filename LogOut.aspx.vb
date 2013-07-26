
Partial Class AdminZone_LogOut
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Session("database") = ""
        Session("IsPass") = "FALSE"
        Session("cboUserGroupsIndex") = Nothing
        ' Session.Abandon()
        'Dim delCookie1 As HttpCookie
        If Not (Request.Cookies("ADMINTOOL") Is Nothing) Then
            'delCookie1 = New HttpCookie("ADMINTOOL")
            'delCookie1.Expires = DateTime.Now.AddDays(-1D)
            'Response.Cookies.Add(delCookie1)
        End If
        Response.Redirect("~/Login")
        'Response.End()
    End Sub
End Class
