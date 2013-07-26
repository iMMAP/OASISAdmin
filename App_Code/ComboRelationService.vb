Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Namespace Immap
    Namespace Service

        Public Class ComboRelationService

            Private Const tablename As String = "ComboRelations"
            Private Shared ReadOnly _instance As New ComboRelationService()

            Shared Sub New()
            End Sub

            Public Shared ReadOnly Property GetInstance() As ComboRelationService
                Get
                    Return _instance
                End Get
            End Property

            Public Shared ReadOnly Property Instance() As ComboRelationService
                Get
                    Return _instance
                End Get
            End Property

            Public Sub RunCreateComboRelationIfNotExits(ByVal DatabaseName As String, ByVal DDDefName As String)
                Dim fullDDDefName, fullDatabaseName, PrepareComboRelation, scriptText As String
                PrepareComboRelation = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("PrepareComboRelation").ToString())
                fullDatabaseName = "USE [" & DatabaseName & "]"
                fullDDDefName = "declare @ddname nvarchar(255)" & Environment.NewLine & "set @ddname ='" & DDDefName & "'"
                scriptText = fullDatabaseName & Environment.NewLine & fullDDDefName & Environment.NewLine & IO.File.ReadAllText(PrepareComboRelation)
                SQLHelper.ExecuteScript(SQLHelper.SetConnectionString(DatabaseName), scriptText)
            End Sub

            Public Function FindAll(ByVal DatabaseName As String, ByVal DDDefName As String, Optional ByRef store As Ext.Net.Store = Nothing) As DataTable
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append("SELECT [GUID1],[sTableName],[sFieldName],[sParentFieldName]")
                sb.AppendFormat(" FROM [dbo].[dd_{0}_ComboRelations]", DDDefName)
                Dim connstr As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim dt As DataTable = SQLHelper.ExecuteDataTable(connstr, sb.ToString())
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
                Dim CommandText = String.Format("DELETE FROM [dbo].[dd_{0}_ComboRelations] WHERE GUID1=@GUID1", DDDefName)
                Dim con = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(con, CommandText, New SqlParameter("@GUID1", GUID1))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_ComboRelations", DDDefName), GUID1, ImmapUtil.TrueDelete)
                End If
            End Sub

            Public Sub Insert(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                          ByVal GUID1 As String,
                          ByVal sTableName As String,
                          ByVal sFieldName As String,
                          ByVal sParentFieldName As String)
                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat("INSERT INTO [dbo].[dd_{0}_ComboRelations]", DDDefName)
                sb.Append(" ([GUID1],[sTableName],[sFieldName],[sParentFieldName])")
                sb.Append(" VALUES(@GUID1,@sTableName,@sFieldName,@sParentFieldName)")
                Dim connstr As String = SQLHelper.SetConnectionString(DatabaseName)
                Dim result As Integer = SQLHelper.ExecuteNonQuery(connstr,
                                          sb.ToString(),
                                          New SqlParameter("@GUID1", GUID1),
                                          New SqlParameter("@sFieldName", sFieldName),
                                          New SqlParameter("@sParentFieldName", sParentFieldName),
                                          New SqlParameter("@sTableName", sTableName))
                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_ComboRelations", DDDefName), GUID1)
                End If
            End Sub

            Public Sub Update(ByVal DatabaseName As String,
                          ByVal DDDefName As String,
                            ByVal GUID1 As String,
                          ByVal sTableName As String,
                          ByVal sFieldName As String,
                          ByVal sParentFieldName As String)

                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendFormat(" UPDATE [dbo].[dd_{0}_ComboRelations]", DDDefName)
                sb.Append(" SET [sTableName]=@sTableName,[sFieldName]=@sFieldName,[sParentFieldName]=@sParentFieldName")
                sb.Append(" WHERE [GUID1]=@GUID1")

                Dim result As Integer = SQLHelper.ExecuteNonQuery(SQLHelper.SetConnectionString(DatabaseName),
                                        sb.ToString(),
                                        New SqlParameter("@GUID1", GUID1),
                                        New SqlParameter("@sTableName", sTableName),
                                        New SqlParameter("@sFieldName", sFieldName),
                                        New SqlParameter("@sParentFieldName", sParentFieldName))

                If (result > 0) Then
                    ImmapUtil.GetInstance.UpdateSynchHistory(DatabaseName, "dd_" & DDDefName & "_", String.Format("dd_{0}_ComboRelations", DDDefName), GUID1)
                End If
            End Sub
        End Class
    End Namespace
End Namespace

