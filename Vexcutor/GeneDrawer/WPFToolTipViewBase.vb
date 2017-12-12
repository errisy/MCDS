Imports System.Windows, System.Windows.Data, System.Windows.Input, System.Windows.Media, System.Windows.Controls
Public MustInherit Class WPFToolTipViewBase
    Inherits Control
    'WPFToolTipViewBase->Label As String Default: ""
    Public Property Label As String
        Get
            Return GetValue(LabelProperty)
        End Get
        Set(ByVal value As String)
            SetValue(LabelProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LabelProperty As DependencyProperty = _
                           DependencyProperty.Register("Label", _
                           GetType(String), GetType(WPFToolTipViewBase), _
                           New PropertyMetadata(""))
    'WPFToolTipViewBase->Type As String Default: ""
    Public Property Type As String
        Get
            Return GetValue(TypeProperty)
        End Get
        Set(ByVal value As String)
            SetValue(TypeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TypeProperty As DependencyProperty = _
                           DependencyProperty.Register("Type", _
                           GetType(String), GetType(WPFToolTipViewBase), _
                           New PropertyMetadata(""))
    'WPFToolTipViewBase->Note As String Default: ""
    Public Property Note As String
        Get
            Return GetValue(NoteProperty)
        End Get
        Set(ByVal value As String)
            SetValue(NoteProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NoteProperty As DependencyProperty = _
                           DependencyProperty.Register("Note", _
                           GetType(String), GetType(WPFToolTipViewBase), _
                           New PropertyMetadata(""))

End Class
