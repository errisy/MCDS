Public MustInherit Class BriefComboBoxBase
    Inherits Control
    Public Sub New()

    End Sub
 
 

 
    'BriefComboBoxBase->ItemsSource As IEnumerable with Event Default: Nothing
    Public Property ItemsSource As IEnumerable
        Get
            Return GetValue(ItemsSourceProperty)
        End Get
        Set(ByVal value As IEnumerable)
            SetValue(ItemsSourceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemsSourceProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemsSource", _
                           GetType(IEnumerable), GetType(BriefComboBoxBase), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedItemsSourceChanged)))
    Private Shared Sub SharedItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, BriefComboBoxBase).ItemsSourceChanged(d, e)
    End Sub
    Private Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        BuildWrappedItems()
        UpdateValue()
    End Sub


    'BriefComboBoxBase->Value As Object with Event Default: Nothing
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
                           GetType(Object), GetType(BriefComboBoxBase), _
                           New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf SharedValueChanged)))
    Private Shared Sub SharedValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, BriefComboBoxBase).ValueChanged(d, e)
    End Sub
    Private Sub ValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        UpdateValue()
    End Sub
    'BriefComboBoxBase->ItemTemplate As DataTemplate Default: Nothing
    Public Property ItemTemplate As DataTemplate
        Get
            Return GetValue(ItemTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ItemTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("ItemTemplate", _
                           GetType(DataTemplate), GetType(BriefComboBoxBase), _
                           New PropertyMetadata(Nothing))
    'BriefComboBoxBase->ValueTemplate As DataTemplate Default: Nothing
    Public Property ValueTemplate As DataTemplate
        Get
            Return GetValue(ValueTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(ValueTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("ValueTemplate", _
                           GetType(DataTemplate), GetType(BriefComboBoxBase), _
                           New PropertyMetadata(Nothing))

    'BriefComboBoxBase->PopupBackground As Brush Default: Brushes.White
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
                           GetType(Brush), GetType(BriefComboBoxBase), _
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
                           GetType(Brush), GetType(BriefComboBoxBase), _
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
                           GetType(Thickness), GetType(BriefComboBoxBase), _
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
                           GetType(CornerRadius), GetType(BriefComboBoxBase), _
                           New PropertyMetadata(New CornerRadius(3.0#)))


#Region "Logic"
    Protected WithEvents PART_Label As Border
    Protected WithEvents PART_Popup As System.Windows.Controls.Primitives.Popup
    Protected PART_ItemsControl As ItemsControl
    Protected PART_Scroll As ScrollViewer
    Protected WithEvents PART_ResizeGrip As Resizer
    Protected WithEvents WrappedItems As New System.Collections.ObjectModel.ObservableCollection(Of SelectionDynamicWrapper)
    Public Overrides Sub OnApplyTemplate()

        If System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then Return
        PART_Label = Template.FindName("PART_Label", Me)
        PART_Popup = Template.FindName("PART_Popup", Me)
        PART_ItemsControl = Template.FindName("PART_ItemsControl", Me)
        PART_Scroll = Template.FindName("PART_Scroll", Me)
        PART_ResizeGrip = Template.FindName("PART_ResizeGrip", Me)
        PART_ItemsControl.ItemsSource = WrappedItems
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
    Private Sub BuildWrappedItems()
        WrappedItems.Clear()
        If TypeOf ItemsSource Is IEnumerable Then
            For Each i In ItemsSource
                WrappedItems.Add(New SelectionDynamicWrapper(i))
            Next
        End If
    End Sub
    Private Sub UpdateValue()
        If Value Is Nothing Then
            For Each wi In WrappedItems
                wi._IsSelected = False
            Next
        Else
            For Each wi In WrappedItems
                wi._IsSelected = Value.Equals(wi.WrappedObject)
            Next
        End If
    End Sub
    Protected Sub TickItem(ItemContext As SelectionDynamicWrapper)
        Value = ItemContext.WrappedObject
    End Sub
#End Region
End Class
 