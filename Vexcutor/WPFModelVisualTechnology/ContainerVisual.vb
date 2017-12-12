Imports System.Collections.ObjectModel, System.Windows

<System.Windows.Markup.ContentProperty("Visuals")>
Public Class ContainerVisual
    Inherits PanelVisual
    Protected Overrides Sub OnUpdateRenderBounds()
        Dim _Size = New Size(IIf(DescendantBounds.Right >= 0.0#, DescendantBounds.Right, 0.0#), IIf(DescendantBounds.Bottom >= 0.0#, DescendantBounds.Bottom, 0.0#))
        SetValue(DesiredSizeProperty, _Size)
    End Sub
    'ContainerVisual->DesiredSize As Size Default: New Size
    Public Property DesiredSize As Size
        Get
            Return GetValue(DesiredSizeProperty)
        End Get
        Set(ByVal value As Size)
            SetValue(DesiredSizeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly DesiredSizeProperty As DependencyProperty =
                           DependencyProperty.Register("DesiredSize",
                           GetType(Size), GetType(ContainerVisual),
                           New PropertyMetadata(New Size))
    Protected Overrides Sub OnRender()

    End Sub
End Class