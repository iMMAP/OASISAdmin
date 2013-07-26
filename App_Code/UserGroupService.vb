Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports Immap.Model
Namespace Immap
    Namespace Service
        Public Class UserGroupService
            Private Shared ReadOnly _instance As New UserGroupService()

            Shared Sub New()
            End Sub

            Private Sub New()
            End Sub

            Public Shared ReadOnly Property GetInstance() As UserGroupService
                Get
                    Return _instance
                End Get
            End Property

            Public Function LoadUserGroupStore(ByVal DatabaseName As String, ByRef store As Ext.Net.Store) As DataTable
                Dim dt = SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(DatabaseName),
                                                    "SELECT Id,Name FROM [UserGroups]")
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

            Public Function LoadForComboboxUserGroupDatable(database As String) As DataTable
                Return SQLHelper.ExecuteDataTable(SQLHelper.SetConnectionString(database),
                                                  "SELECT Id,Name FROM [UserGroups]")
            End Function
        End Class
    End Namespace
End Namespace