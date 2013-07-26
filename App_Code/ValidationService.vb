Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Namespace Immap
    Namespace Service
        Public Class ValidationService
            Private Shared ReadOnly _instance As New ValidationService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub
            Public Shared ReadOnly Property GetInstance() As ValidationService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As ValidationService
                Get
                    Return _instance
                End Get
            End Property

            Public Function FindAll(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append("SELECT [GUID1],[sDataEntryTableName],[sDataEntryFieldName],[bRequired],[sEditMask],[sValidation]")
                sb.AppendFormat(" FROM [dbo].[dd_{0}_Validation]", DDDefName)
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
                Dim CommandText = String.Format("DELETE FROM [dbo].[dd_{0}_Validation] WHERE GUID1=@GUID1", DDDefName)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con, CommandText, New SqlParameter("@GUID1", GUID1))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Validation", DDDefName), GUID1, ImmapUtil.TrueDelete)
                End If
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                          ByVal GUID1 As String,
                          ByVal sDataEntryTableName As String,
                          ByVal sDataEntryFieldName As String,
                          ByVal bRequired As String,
                          ByVal sEditMask As String,
                          ByVal sValidation As String)
                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat("INSERT INTO [dbo].[dd_{0}_Validation]", DDDefName)
                sb.Append(" ([GUID1],[sDataEntryTableName],[sDataEntryFieldName],[bRequired],[sEditMask],[sValidation])")
                sb.Append(" VALUES(@GUID1,@sDataEntryTableName,@sDataEntryFieldName,@bRequired,@sEditMask,@sValidation)")
                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                          sb.ToString(),
                                          New SqlParameter("@GUID1", GUID1),
                                          New SqlParameter("@sDataEntryTableName", sDataEntryTableName),
                                          New SqlParameter("@sDataEntryFieldName", sDataEntryFieldName),
                                          New SqlParameter("@bRequired", bRequired),
                                          New SqlParameter("@sEditMask", sEditMask),
                                          New SqlParameter("@sValidation", sValidation))

                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Validation", DDDefName), GUID1)
                End If
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                          ByVal GUID1 As String,
                          ByVal sDataEntryTableName As String,
                          ByVal sDataEntryFieldName As String,
                          ByVal bRequired As String,
                          ByVal sEditMask As String,
                          ByVal sValidation As String)

                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat(" UPDATE [dbo].[dd_{0}_Validation]", DDDefName)
                sb.Append(" SET [sDataEntryTableName]=@sDataEntryTableName,[sDataEntryFieldName]=@sDataEntryFieldName,[bRequired]=@bRequired,")
                sb.Append(" [sEditMask]=@sEditMask,[sValidation]=@sValidation")
                sb.Append(" WHERE [GUID1]=@GUID1")

                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                        sb.ToString(),
                                        New SqlParameter("@GUID1", GUID1),
                                        New SqlParameter("@sDataEntryTableName", sDataEntryTableName),
                                        New SqlParameter("@sDataEntryFieldName", sDataEntryFieldName),
                                        New SqlParameter("@bRequired", bRequired),
                                        New SqlParameter("@sEditMask", sEditMask),
                                        New SqlParameter("@sValidation", sValidation))

                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_Validation", DDDefName), GUID1)
                End If
            End Sub
        End Class
    End Namespace
End Namespace

