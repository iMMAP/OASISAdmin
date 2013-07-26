Imports Microsoft.VisualBasic
Imports System.Web
Namespace Immap
    Namespace Service
        Public Class ImmapService
            Private Shared ReadOnly _instance As New ImmapService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub
            Public Shared ReadOnly Property GetInstance() As ImmapService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As ImmapService
                Get
                    Return _instance
                End Get
            End Property

            Private Const SESSION_REDIRECTURL As String = "RedirectURL"
            Private Const SESSION_DATABASE As String = "database"
            Private Const COOKIE_ADMINTOOL As String = "ADMINTOOL"
            Private Const REDIRECT_LOGINURL As String = "Login"
            Public Sub CheckDatabaseIsExitIfNotRedirectTologin()
                Dim database As String = Convert.ToString(HttpContext.Current.Session(SESSION_DATABASE))
                If String.IsNullOrEmpty(database) = True Then
                    HttpContext.Current.Session(SESSION_REDIRECTURL) = HttpContext.Current.Request.Url.AbsoluteUri
                    Dim cookieDetails = HttpContext.Current.Request.Cookies(COOKIE_ADMINTOOL)
                    If cookieDetails IsNot Nothing Then
                        HttpContext.Current.Response.Cookies.Add(cookieDetails)
                    End If
                    HttpContext.Current.Response.Redirect(REDIRECT_LOGINURL)
                End If
            End Sub

        End Class
    End Namespace
End Namespace