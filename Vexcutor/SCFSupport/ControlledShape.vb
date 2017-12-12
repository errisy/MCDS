Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Shapes
Public Class ControlledShape
    Inherits Shape
    'ControlledShape->RenderSwitch As Boolean Default: False, FrameworkPropertyMetadataOptions.AffectsRender 
    Public Property RenderSwitch As Boolean
        Get
            Return GetValue(RenderSwitchProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(RenderSwitchProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RenderSwitchProperty As DependencyProperty = _
                           DependencyProperty.Register("RenderSwitch", _
                           GetType(Boolean), GetType(ControlledShape), _
                           New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.AffectsRender))
    Public Sub Render()
        RenderSwitch = Not RenderSwitch
    End Sub
    Private _Path As New PathGeometry
    Public ReadOnly Property Geometry As PathGeometry
        Get
            Return _Path
        End Get
    End Property
    Protected Overrides ReadOnly Property DefiningGeometry As Geometry
        Get
            Return _Path
        End Get
    End Property
End Class