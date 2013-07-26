Imports Microsoft.VisualBasic
Namespace Immap
    Namespace Model
        Public Class GISGridTableSettingsModel
            Public Property Id As Integer
            Public Property Name As String
            Public Property Alia As String
            Public Property Visible As Boolean
            Public Property DatasetWarning As Boolean
            Public Property Warninglevel As Integer
            Public Property MaxRec As Integer
            Public Property ExcludedFlds As String
            Public Property IsURLLayer As Boolean
            Public Property AutoRunUrls As Boolean
            Public Property URLLayerField As String
        End Class
    End Namespace
End Namespace