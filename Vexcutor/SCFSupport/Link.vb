Imports System.Windows, System.Windows.Controls, System.Windows.Input, System.Windows.Documents, System.Windows.Media
Public Class Link
    Inherits Underline
    Public Sub New()
        Dim dpd = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(Underline.IsMouseOverProperty, GetType(Underline))
        dpd.AddValueChanged(Me, AddressOf IsMouseDirectlyOverPropertyChanged)
    End Sub
    'Link->HighlightBrush As Brush Default: Brushes.Red
    Public Property HighlightBrush As Brush
        Get
            Return GetValue(HighlightBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(HighlightBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly HighlightBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("HighlightBrush", _
                           GetType(Brush), GetType(Link), _
                           New PropertyMetadata(Brushes.Red))
    'Link->ClickBrush As Brush Default: Brushes.Purple
    Public Property ClickBrush As Brush
        Get
            Return GetValue(ClickBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(ClickBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ClickBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("ClickBrush", _
                           GetType(Brush), GetType(Link), _
                           New PropertyMetadata(Brushes.Purple))
    Private _Foreground As Brush
    Private Sub IsMouseDirectlyOverPropertyChanged(sender As Object, e As EventArgs)
        If IsMouseOver Then
            _Foreground = Foreground
            Foreground = HighlightBrush
        Else
            Foreground = _Foreground
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        Foreground = ClickBrush
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseButtonEventArgs)
        If IsMouseOver Then
            Foreground = HighlightBrush
        Else
            Foreground = _Foreground
        End If
        MyBase.OnMouseUp(e)
    End Sub
End Class
