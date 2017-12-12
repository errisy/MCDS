Public Class IncludableElementSelector
    Inherits DataTemplateSelector
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If TypeOf item Is FormatedTextViewModel Then
            Return IncludableDataTemplate.IncludableFormatedTextDataTemplate
        ElseIf TypeOf item Is GeometryTextViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        ElseIf TypeOf item Is GeometryViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        ElseIf TypeOf item Is LineViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        End If
    End Function
End Class

Public Class DirectIncludableElementSelector
    Inherits DirectDataTemplateSelector
    Public Overrides Function SelectTemplate(item As Object) As DataTemplate
        If TypeOf item Is FormatedTextViewModel Then
            Return IncludableDataTemplate.IncludableFormatedTextDataTemplate
        ElseIf TypeOf item Is GeometryTextViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        ElseIf TypeOf item Is GeometryViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        ElseIf TypeOf item Is LineViewModel Then
            Return IncludableDataTemplate.IncludableShapeDataTemplate
        End If
    End Function
End Class

Public Class IncludableDataTemplate
    Public Shared ReadOnly IncludableFormatedTextDataTemplate As DataTemplate = New DataTemplate() With {.VisualTree = New FrameworkElementFactory(GetType(IncludableFormatedText))}
    Public Shared ReadOnly IncludableShapeDataTemplate As DataTemplate = New DataTemplate() With {.VisualTree = New FrameworkElementFactory(GetType(IncludableShape))}
    Shared Sub New()
        IncludableFormatedTextDataTemplate.Seal()
        IncludableShapeDataTemplate.Seal()
    End Sub
End Class
