Public Class ItemsContainerControl
    Inherits ItemsControl
    'ItemsContainerControl->ItemContainerTemplate As DataTemplate Default: Nothing
    Public Property ItemContainerTemplate As DataTemplate
        Get
            Return GetValue(ItemContainerTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemContainerTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemContainerTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemContainerTemplate", _
                           GetType(DataTemplate), GetType(ItemsContainerControl), _
                           New PropertyMetadata(Nothing))
    Protected Overrides Function GetContainerForItemOverride() As DependencyObject
        If ItemContainerTemplate IsNot Nothing Then
            Return ItemContainerTemplate.LoadContent
        Else
            Return MyBase.GetContainerForItemOverride()
        End If
    End Function
End Class