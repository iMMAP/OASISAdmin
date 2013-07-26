Imports Microsoft.VisualBasic
Namespace Immap
    Namespace Model
        Public Class TableColumnModel
            Property TName As String = Nothing
            Property CName As String = Nothing
            Property LockFiled As Boolean = False
            Property ExcludeFiled As Boolean = False

            Public Sub New()

            End Sub
            Public Sub New(ByVal xTName As String, ByVal xCName As String)
                TName = xTName
                CName = xCName
            End Sub
        End Class
    End Namespace
End Namespace
