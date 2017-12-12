Public MustInherit Class DirectDataTemplateSelector
    Public Overridable Function SelectTemplate(item As Object) As DataTemplate
        Return New DataTemplate() With {.VisualTree = New FrameworkElementFactory(GetType(ContentPresenter))}
    End Function

End Class
<System.Windows.Markup.ContentProperty("Templates")>
Public Class DirectTypeTemplateSelector
    Inherits DirectDataTemplateSelector
    Public Sub New()
    End Sub
    Public Overrides Function SelectTemplate(item As Object) As DataTemplate
        For Each t In _Templates
            If TypeOf t.DataType Is Type AndAlso item.GetType.IsAssignableFrom(DirectCast(t.DataType, Type)) Then Return t
        Next
        Return MyBase.SelectTemplate(item)
    End Function
    Private _Templates As New System.Collections.ObjectModel.ObservableCollection(Of DataTemplate)
    Public ReadOnly Property Templates As System.Collections.ObjectModel.ObservableCollection(Of DataTemplate)
        Get
            Return _Templates
        End Get
    End Property
End Class
