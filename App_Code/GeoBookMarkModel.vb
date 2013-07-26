Imports Microsoft.VisualBasic
Namespace Immap
    Namespace Model
        Public Class GeoBookMarkModel
            Public Property ID As Integer
            Public Property Name As String
            Public Property X As Double
            Public Property Y As Double
            Public Property Z As Double
            Public Property Description As String
            Public Property UseSymbol As Boolean
            Public Property SymbolChar As String
            Public Property SymbolFont As String
            Public Property SymbolSize As String
            Public Property MapName As String
            Public Property BmkrID As Integer
            Public Property sGUID As String
            Public Property dTimeStamp As Date
            Public Property OwnerGUID As String
            Public Property Deleted As Boolean
            Public Property isURLMark As Boolean
            Public Property sURL As String
        End Class
    End Namespace
End Namespace