Imports Microsoft.VisualBasic
Namespace Immap
    Namespace Model
        Public Class DynamicDataDefModel
            Public Property DDDefName As String
            Public Property Description As String
            Public Property AccessRights As String
            Public Property ConnectionString As String
            Public Property Synch As Boolean
            Public Property EnableDataEntry As Boolean
            Public Property EnableReporting As Boolean
            Public Property LockedFields As String
            Public Property ExcludedFields As String
        End Class
    End Namespace
End Namespace