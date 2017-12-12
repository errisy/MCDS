<System.Windows.Markup.ContentProperty("Templates")>
Public Class TypeTemplateSelector
    Inherits DataTemplateSelector
    Public Sub New()
    End Sub
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If item Is Nothing Then Return Nothing
        For Each t In _Templates
            If TypeOf t.DataType Is Type AndAlso item.GetType.IsAssignableFrom(DirectCast(t.DataType, Type)) Then Return t
        Next
        Return MyBase.SelectTemplate(item, container)
    End Function
    Private _Templates As New System.Collections.ObjectModel.ObservableCollection(Of DataTemplate)
    Public ReadOnly Property Templates As System.Collections.ObjectModel.ObservableCollection(Of DataTemplate)
        Get
            Return _Templates
        End Get
    End Property
End Class
