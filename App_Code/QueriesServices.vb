Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Namespace Immap
    Namespace Service
        Public Class QueriesServices

            Private Shared ReadOnly _instance As New QueriesServices()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub
            Public Shared ReadOnly Property GetInstance() As QueriesServices
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As QueriesServices
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append("SELECT [GUID1],[QueryName],[QuerySQL],[QueryMSSQL]")
                sb.AppendFormat(" FROM [dbo].[dd_{0}_Queries]", DDDefName)
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                        sb.ToString())
                If Not (store Is Nothing) AndAlso Not (dt Is Nothing) Then
                    store.DataSource = dt
                    store.DataBind()
                End If
                If Not (store Is Nothing) Then
                    store.DataBind()
                End If
                If Not (dt Is Nothing) AndAlso dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Function

            Public Sub DeleteById(ByVal DatabaseName As String,
                         ByVal DDDefName As String,
                         ByVal GUID1 As String)
                Dim CommandText As String = String.Format("DELETE FROM [dbo].[dd_{0}_Queries] WHERE GUID1=@GUID1", DDDefName)
                Dim con As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con, CommandText, New SqlParameter("@GUID1", GUID1))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Queries", DDDefName), GUID1, ImmapUtil.TrueDelete)
                End If
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                          ByVal GUID1 As String,
                          ByVal QueryName As String,
                          ByVal QuerySQL As String,
                          ByVal QueryMSSQL As String)

                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat("INSERT INTO [dbo].[dd_{0}_Queries]", DDDefName)
                sb.Append(" ([GUID1],[QueryName],[QuerySQL],[QueryMSSQL])")
                sb.Append(" VALUES(@GUID1,@QueryName,@QuerySQL,@QueryMSSQL)")
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          sb.ToString(),
                                          New SqlParameter("@GUID1", GUID1),
                                          New SqlParameter("@QueryName", QueryName),
                                          New SqlParameter("@QuerySQL", QuerySQL),
                                          New SqlParameter("@QueryMSSQL", QueryMSSQL))

                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Queries", DDDefName), GUID1)
                End If
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                          ByVal GUID1 As String,
                          ByVal QueryName As String,
                          ByVal QuerySQL As String,
                          ByVal QueryMSSQL As String)

                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat(" UPDATE [dbo].[dd_{0}_Queries]", DDDefName)
                sb.Append(" SET [QueryName]=@QueryName,[QuerySQL]=@QuerySQL,[QueryMSSQL]=@QueryMSSQL")
                sb.Append(" WHERE [GUID1]=@GUID1")

                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                    sb.ToString(),
                                    New SqlParameter("@GUID1", GUID1),
                                    New SqlParameter("@QueryName", QueryName),
                                    New SqlParameter("@QuerySQL", QuerySQL),
                                    New SqlParameter("@QueryMSSQL", QueryMSSQL))

                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Queries", DDDefName), GUID1)
                End If
            End Sub
        End Class
    End Namespace
End Namespace
