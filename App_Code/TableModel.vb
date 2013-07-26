Imports Microsoft.VisualBasic

Public Class TableModel

    Property TBName As String = Nothing
    Property Read As Boolean = False
    Property Add As Boolean = False
    Property Edit As Boolean = False
    Property Delete As Boolean = False

    Public Sub New()

    End Sub
    Public Sub New(ByVal tableName As String)
        Me.TBName = tableName
    End Sub
End Class
