Public MustInherit Class EnumEditorBase
    Inherits Control
    Public Sub New()
        SetValue(ItemsPropertyKey, _Items)
    End Sub
    'EnumEditorBase -> EnumType As Type Default: Nothing
    Public ReadOnly Property EnumType As Type
        Get
            Return GetValue(EnumEditor.EnumTypeProperty)
        End Get
    End Property
    Private Shared ReadOnly EnumTypePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("EnumType", _
                              GetType(Type), GetType(EnumEditor), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly EnumTypeProperty As DependencyProperty = _
                             EnumTypePropertyKey.DependencyProperty
    'EnumEditorBase -> Label As String Default: ""
    Public ReadOnly Property Label As String
        Get
            Return GetValue(EnumEditorBase.LabelProperty)
        End Get
    End Property
    Private Shared ReadOnly LabelPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Label", _
                              GetType(String), GetType(EnumEditorBase), _
                              New PropertyMetadata(""))
    Public Shared ReadOnly LabelProperty As DependencyProperty = _
                             LabelPropertyKey.DependencyProperty

    Private _Items As New System.Collections.ObjectModel.ObservableCollection(Of EnumItem)
    'EnumEditorBase -> Items As System.Collections.ObjectModel.ObservableCollection(of EnumItem) Default: Nothing
    Public ReadOnly Property Items As System.Collections.ObjectModel.ObservableCollection(Of EnumItem)
        Get
            Return GetValue(EnumEditorBase.ItemsProperty)
        End Get
    End Property
    Private Shared ReadOnly ItemsPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Items", _
                              GetType(System.Collections.ObjectModel.ObservableCollection(Of EnumItem)), GetType(EnumEditorBase), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly ItemsProperty As DependencyProperty = _
                             ItemsPropertyKey.DependencyProperty
    'EnumEditorBase->Value As Object with Event Default: Nothing
    Public Property Value As Object
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueProperty As DependencyProperty = _
                           DependencyProperty.Register("Value", _
                           GetType(Object), GetType(EnumEditorBase), _
                           New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf SharedValueChanged)))
    Private Shared Sub SharedValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EnumEditorBase).ValueChanged(d, e)
    End Sub
    Private Sub ValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        UpdateValue()
    End Sub
    'EnumEditorBase->PopupBackground As Brush Default: Brushes.White
    Public Property PopupBackground As Brush
        Get
            Return GetValue(PopupBackgroundProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(PopupBackgroundProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PopupBackgroundProperty As DependencyProperty = _
                           DependencyProperty.Register("PopupBackground", _
                           GetType(Brush), GetType(EnumEditorBase), _
                           New PropertyMetadata(Brushes.White))
    'EnumEditor->PopupBorderBrush As Brush Default: Brushes.Black
    Public Property PopupBorderBrush As Brush
        Get
            Return GetValue(PopupBorderBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(PopupBorderBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PopupBorderBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("PopupBorderBrush", _
                           GetType(Brush), GetType(EnumEditorBase), _
                           New PropertyMetadata(Brushes.Black))
    'EnumEditor->PopupBorderThickness As Thickness Default: New Thickness(1#)
    Public Property PopupBorderThickness As Thickness
        Get
            Return GetValue(PopupBorderThicknessProperty)
        End Get
        Set(ByVal value As Thickness)
            SetValue(PopupBorderThicknessProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PopupBorderThicknessProperty As DependencyProperty = _
                           DependencyProperty.Register("PopupBorderThickness", _
                           GetType(Thickness), GetType(EnumEditorBase), _
                           New PropertyMetadata(New Thickness(1.0#)))
    'EnumEditor->PopupCornerRadius As CornerRadius Default: New CornerRadius(3#)
    Public Property PopupCornerRadius As CornerRadius
        Get
            Return GetValue(PopupCornerRadiusProperty)
        End Get
        Set(ByVal value As CornerRadius)
            SetValue(PopupCornerRadiusProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PopupCornerRadiusProperty As DependencyProperty = _
                           DependencyProperty.Register("PopupCornerRadius", _
                           GetType(CornerRadius), GetType(EnumEditorBase), _
                           New PropertyMetadata(New CornerRadius(3.0#)))


#Region "Logic"
    Protected WithEvents PART_Label As Border
    Protected WithEvents PART_Popup As System.Windows.Controls.Primitives.Popup
    Protected PART_ItemsControl As ItemsControl
    Protected PART_Scroll As ScrollViewer
    Protected WithEvents PART_ResizeGrip As Resizer
    Public Overrides Sub OnApplyTemplate()

        If System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then Return
        PART_Label = Template.FindName("PART_Label", Me)
        PART_Popup = Template.FindName("PART_Popup", Me)
        PART_ItemsControl = Template.FindName("PART_ItemsControl", Me)
        PART_Scroll = Template.FindName("PART_Scroll", Me)
        PART_ResizeGrip = Template.FindName("PART_ResizeGrip", Me)
        If PART_ItemsControl IsNot Nothing Then PART_ItemsControl.ItemsSource = _Items
        MyBase.OnApplyTemplate()
    End Sub
#Region "Resize"
    Private OriginalSize As Size
    Private StartPoint As Point
    Private Dragging As Boolean = False
    Private Sub PART_ResizeGrip_DragStarted(sender As Object, e As MouseEventArgs) Handles PART_ResizeGrip.MouseDown
        Dragging = True
        OriginalSize = New Size(PART_Scroll.ActualWidth, PART_Scroll.ActualHeight)
        StartPoint = Me.PointToScreen(e.GetPosition(Me))
        PART_ResizeGrip.CaptureMouse()
    End Sub
    Private Sub PART_ResizeGrip_DragDelta(sender As Object, e As MouseEventArgs) Handles PART_ResizeGrip.MouseMove
        If Dragging AndAlso PART_Popup.IsOpen Then
            Dim delta As Vector = Me.PointToScreen(e.GetPosition(Me)) - StartPoint
            Dim w As Double = OriginalSize.Width + delta.X
            If w < 40.0# Then w = 40.0#
            Dim h As Double = OriginalSize.Height + delta.Y
            If h < 60.0# Then h = 60.0#
            PART_Popup.Width = w
            PART_Popup.Height = h
        End If
    End Sub
    Private Sub PART_ResizeGrip_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles PART_ResizeGrip.MouseUp
        If Dragging Then
            PART_ResizeGrip.ReleaseMouseCapture()
            Dragging = False
        End If
    End Sub
#End Region
    Private Sub PART_Label_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles PART_Label.MouseUp
        If PART_Popup.IsOpen Then
            'PART_Popup.StaysOpen = False
            PART_Popup.IsOpen = False
        Else
            PART_Popup.Focus()
            'PART_Popup.StaysOpen = True
            PART_Popup.IsOpen = True
        End If
        e.Handled = True
    End Sub
    Private Sub UpdateValue()

        If Value Is Nothing Then
            _Items.Clear()
            SetValue(EnumTypePropertyKey, Nothing)
            SetValue(LabelPropertyKey, "N/A")
        Else
            Dim eType As Type = Value.GetType
            _Items.Clear()
            If eType.IsEnum Then
                SetValue(EnumTypePropertyKey, eType)

                For Each eValue In [Enum].GetValues(EnumType)
                    _Items.Add(New EnumItem With {.Name = [Enum].GetName(EnumType, eValue), .Value = eValue})
                Next
                For Each item In _Items
                    item.IsChecked = item.Value.Equals(Value)
                Next
                SetValue(LabelPropertyKey, [Enum].GetName(EnumType, Value))
            Else
                SetValue(EnumTypePropertyKey, Nothing)
                SetValue(LabelPropertyKey, "N/A")
            End If
        End If
    End Sub
    Protected Sub TickItem(ItemContext As EnumItem)
        Value = ItemContext.Value
    End Sub
#End Region
End Class

Public Class EnumItem
    Implements System.ComponentModel.INotifyPropertyChanged
    'Public Property Name As String
    Private _Name As String
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Name"))
        End Set
    End Property
    'Public Property Value As Object
    Private _Value As Object
    Public Property Value As Object
        Get
            Return _Value
        End Get
        Set(value As Object)
            _Value = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Value"))
        End Set
    End Property
    'Public Property IsChecked As Boolean
    Private _IsChecked As Boolean
    Public Property IsChecked As Boolean
        Get
            Return _IsChecked
        End Get
        Set(value As Boolean)
            _IsChecked = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsChecked"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class