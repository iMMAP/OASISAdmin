Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Xml
Imports Ext.Net
Imports System.Globalization

Partial Class Login
    Inherits System.Web.UI.Page

    Private sqlServer As String = System.Configuration.ConfigurationManager.AppSettings("Server,Port").ToString()
    Private sqlUserId As String = System.Configuration.ConfigurationManager.AppSettings("UserId").ToString()
    Private sqlPassword As String = System.Configuration.ConfigurationManager.AppSettings("Password").ToString()
    Private sqlTrustedConnection As String = System.Configuration.ConfigurationManager.AppSettings("TrustedConnection").ToString()

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Session.Timeout = 20

            Session("TITLE") = "Login"
            LoadDatabase()
            If Session("LASTINDEX") IsNot Nothing Then
                ' cboDatabase.SelectedIndex = Session("LASTINDEX")
                Response.Cookies("ADMINTOOL")("LASTINDEX") = Session("LASTINDEX")
            End If
            Dim cookieDetails = Request.Cookies("ADMINTOOL")
            If cookieDetails IsNot Nothing Then
                Response.Cookies.Add(cookieDetails)
            End If
            If Response.Cookies("ADMINTOOL")("LASTINDEX") IsNot Nothing Then
                cboDatabase.SelectedIndex = CInt(Response.Cookies("ADMINTOOL")("LASTINDEX"))
            End If
            cboDatabase.DataBind()
        End If

    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        txtUsername.Text = ""
        txtPassword.Attributes.Add("value", "")

        If Session("LASTINDEX") IsNot Nothing Then
            ' cboDatabase.SelectedIndex = Session("LASTINDEX")
            Response.Cookies("ADMINTOOL")("LASTINDEX") = CInt(Request.Cookies("ADMINTOOL")("LASTINDEX"))
        End If
        If Response.Cookies("ADMINTOOL")("LASTINDEX") Is Nothing Then
            Response.Cookies("ADMINTOOL")("LASTINDEX") = CInt(Request.Cookies("ADMINTOOL")("LASTINDEX"))
        End If
        If Response.Cookies("ADMINTOOL")("LASTINDEX") IsNot Nothing Then
            cboDatabase.SelectedIndex = CInt(Response.Cookies("ADMINTOOL")("LASTINDEX"))
        Else
        End If
        cboDatabase.DataBind()
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As System.EventArgs) Handles btnLogin.Click
        Dim commandText = "SELECT  u.[id], u.[user], u.[Fname], u.[Lname], g.[Name] FROM [users] u INNER JOIN [userGroups] as g ON g.id = u.UserGroupID" & _
        " WHERE (u.[user] LIKE @user) AND (u.[pwd] LIKE @pwd) AND (g.Name LIKE 'iMMAP');"
        Session("IsPass") = "FALSE"
        'Try
        If cboDatabase.SelectedItem IsNot Nothing Then
            Dim dr = SQLHelper.ExecuteReader(SQLHelper.SetConnectionString(cboDatabase.SelectedItem.Text),
                           commandText,
                           New SqlParameter("@user", txtUsername.Text),
                           New SqlParameter("@pwd", ImmapUtil.Instance.CalculateMD5(txtPassword.Text)))
            Dim cultures(Request.UserLanguages.Length) As CultureInfo
            Dim isPass As Boolean = False
            If dr.HasRows Then
                isPass = True
                dr.Read()
                Session("database") = cboDatabase.SelectedItem.ToString()
                Session("LASTINDEX") = cboDatabase.SelectedIndex.ToString()
                Session("UserGroup") = dr.GetString(4)
                Session("tk") = Request.Cookies("tk").Value
                Dim kk = New TimeSpan(0, CInt(Session("tk")), 0)
                Dim datAdjusment As DateTime = DateAdd("n", -CInt(Session("tk")), DateTime.UtcNow)
                Session("DateLogged") = datAdjusment.ToString("dd-MMM-yyyy")
                Session("TimeLogged") = datAdjusment.ToString("HH:mm")
                Response.Cookies("ADMINTOOL")("FullName") = dr.GetString(2) & " " & dr.GetString(3)
                Response.Cookies("ADMINTOOL")("LASTINDEX") = cboDatabase.SelectedIndex.ToString()
                Response.Cookies("ADMINTOOL").Expires = Date.Today.AddDays(365)
            End If
            dr = Nothing
            If isPass = True Then

                Session("IsPass") = "TRUE"
                Dim url As String = CStr(Session("RedirectURL"))
                If String.IsNullOrWhiteSpace(url) Then
                    Response.Redirect("~/Home")
                Else
                    Session("RedirectURL") = ""
                    Response.Redirect(url)
                End If
            ElseIf isPass = False Then
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning",
                                                        "<script language='javascript'>alert('The username or password you entered is incorrect.');</script>")
            End If
            'Catch ex As Exception
            '    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning",
            '     "<script language='javascript'>alert('The server was problem while logging. Please try again');</script>")
            'End Try
        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning",
                                                      "<script language='javascript'>alert('Please select a database form list');</script>")
        End If
      


    End Sub
    Protected Sub LoadDatabase()
        Try
            'Dim doc = New XmlDocument()
            'doc.Load("http://atlantis.oasiswebservice.org/oasis.asp?id=SELECT%20[name]%20FROM%20sys.sysdatabases")

            'Dim ds As New DataSet
            '' Dim url = "http://atlantis.oasiswebservice.org/oasis.asp?id=SELECT%20[name]%20FROM%20sys.sysdatabases"
            'Dim url = "http://atlantis.oasiswebservice.org/oasis.asp?id=EXEC%20sp_databases"
            'ds.ReadXml(url)
            'Dim dt = ds.Tables("row")
            'cboDatabase.Items.Clear()
            'For i As Integer = 0 To dt.Rows.Count - 1
            '    Dim value As String = Convert.ToString(dt.Rows(i)(0))
            '    If value.ToLower().StartsWith("oasisdb-") = True Then
            '        cboDatabase.Items.Add(value)
            '    End If
            'Next i
            Dim dbUrl As String = System.Configuration.ConfigurationManager.AppSettings("DbUrl").ToString()
            Dim XmlReader As New XmlTextReader(dbUrl)
            Dim dbname As String = Nothing
            Dim cnt As Integer = 0
            While XmlReader.Read()
                If XmlReader.Name.ToString() = "z:row" Then
                    If XmlReader.HasAttributes Then
                        While XmlReader.MoveToNextAttribute()
                            If XmlReader.Name.ToLower() = "name" Then
                                If XmlReader.Value.ToLower().StartsWith("oasisdb-") = True Then
                                    cboDatabase.Items.Add(XmlReader.Value)
                                    cnt = 1
                                    Exit While
                                End If
                            End If
                        End While
                    End If
                End If
                dbname = ""
            End While
            XmlReader.Close()
            cboDatabase.DataBind()
            If cnt = 1 Then
                cboDatabase.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class
