Public Class BriefTextBox
    Inherits BriefTextBoxBase
    Shared Sub New()
        Dim DefaultStyle As Style = Markup.XamlReader.Parse(
      <Style
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:e="clr-namespace:Errisy;assembly=Errisy"
          TargetType="{x:Type e:BriefTextBoxBase}">
          <Style.Setters>
              <Setter Property="Template">
                  <Setter.Value>
                      <ControlTemplate TargetType="{x:Type e:BriefTextBoxBase}">
                          <Border Background="{TemplateBinding Background}" BorderThickness="{TemplateBinding BorderThickness}" BorderBrush="{TemplateBinding BorderBrush}" CornerRadius="{TemplateBinding CornerRadius}">
                              <ScrollViewer Margin="-3,-1,-3,-1" x:Name="PART_ContentHost"/>
                          </Border>
                      </ControlTemplate>
                  </Setter.Value>
              </Setter>
          </Style.Setters>
      </Style>.ToString)
        CanvasFormBase.StyleProperty.OverrideMetadata(GetType(BriefTextBox), New FrameworkPropertyMetadata(DefaultStyle))
    End Sub
End Class

Public MustInherit Class BriefTextBoxBase
    Inherits TextBox
    'BriefTextBoxBase->CornerRadius As CornerRadius Default: New CornerRadius(0#)
    Public Property CornerRadius As CornerRadius
        Get
            Return GetValue(CornerRadiusProperty)
        End Get
        Set(ByVal value As CornerRadius)
            SetValue(CornerRadiusProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CornerRadiusProperty As DependencyProperty = _
                           DependencyProperty.Register("CornerRadius", _
                           GetType(CornerRadius), GetType(BriefTextBoxBase), _
                           New PropertyMetadata(New CornerRadius(0.0#)))

End Class