Imports Microsoft.VisualBasic
Namespace Immap
    Namespace Model
        Public Class FeedModel
            Public Property FeedID As Integer
            Public Property GroupId As Integer
            Public Property CustomId As Integer
            Public Property FeedName As String
            Public Property FeedDescription As String
            Public Property FeedURL As String
            Public Property FeedImageURL As String
            Public Property CheckInterval As Integer
            Public Property Subscribed As String
            Public Property LastCheck As Date
        End Class
    End Namespace
End Namespace
