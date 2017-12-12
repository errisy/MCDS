Imports System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows, System.Windows.Shapes, Microsoft.Expression.Media.Effects, System.Windows.Media.Effects
Public Class PropertyGridX
    Inherits StackPanel
    Protected Friend TextExpander As New PropertyExpander With {.Header = "Text"}
    Protected Friend BrushExpander As New PropertyExpander With {.Header = "Brush"}
    Protected Friend PositionExpander As New PropertyExpander With {.Header = "Position"}
    Protected Friend ValueExpander As New PropertyExpander With {.Header = "Value"}
    Protected Friend EffectExpander As New PropertyExpander With {.Header = "Effect"}
    Public RelatedProject As ChapterTree
    Public Sub New()
        Me.CanVerticallyScroll = True
        Children.Add(TextExpander)
        Children.Add(BrushExpander)
        Children.Add(PositionExpander)
        Children.Add(ValueExpander)
        Children.Add(EffectExpander)
    End Sub
    Public Sub Load(Items As IEnumerable(Of IActor))
        If Items.OK AndAlso Items.Count > 0 Then
            Dim x = Me.Dispatcher.DisableProcessing
            TextExpander.Clear()
            BrushExpander.Clear()
            PositionExpander.Clear()
            ValueExpander.Clear()
            EffectExpander.Clear()
            If RelatedProject IsNot Nothing Then
                TextExpander.ImageSource = RelatedProject.Root.Images
                BrushExpander.ImageSource = RelatedProject.Root.Images
                PositionExpander.ImageSource = RelatedProject.Root.Images
                ValueExpander.ImageSource = RelatedProject.Root.Images
                EffectExpander.ImageSource = RelatedProject.Root.Images

                TextExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
                BrushExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
                PositionExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
                ValueExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
                TextExpander.BrushStylesSource = RelatedProject.Root.BrushStyles

                TextExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
                BrushExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
                PositionExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
                ValueExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
                EffectExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
            End If
            Dim Item As Object = Items(0)
            Dim objs As New List(Of Object)
            For Each obj In Items
                If obj IsNot Item Then objs.Add(obj)
            Next
            Dim ara As Object() = objs.ToArray
            For Each pi As System.Reflection.PropertyInfo In Item.GetType.GetProperties
                If pi.IsAct Then
                    Select Case pi.PropertyType.AnimationType
                        Case AnimationTypeEnum.Brush
                            BrushExpander.AddProperty(Item, pi, ara)
                        Case AnimationTypeEnum.Effect
                            EffectExpander.AddProperty(Item, pi, ara)
                        Case AnimationTypeEnum.Movement
                            PositionExpander.AddProperty(Item, pi, ara)
                        Case AnimationTypeEnum.Switch
                            'BrushExpander.AddProperty(Item, pi)
                        Case AnimationTypeEnum.Text
                            TextExpander.AddProperty(Item, pi, ara)
                        Case AnimationTypeEnum.Value
                            ValueExpander.AddProperty(Item, pi, ara)
                    End Select
                End If
            Next
            TextExpander.IsExpanded = True
            BrushExpander.IsExpanded = True
            PositionExpander.IsExpanded = True
            ValueExpander.IsExpanded = True
            EffectExpander.IsExpanded = True
            x.Dispose()
        End If
    End Sub
    Private Shared Empty As Object() = New Object() {}
    Public Sub Load(Item As IActor)
        Dim x = Me.Dispatcher.DisableProcessing
        TextExpander.Clear()
        BrushExpander.Clear()
        PositionExpander.Clear()
        ValueExpander.Clear()
        EffectExpander.Clear()
        If RelatedProject IsNot Nothing Then
            TextExpander.ImageSource = RelatedProject.Root.Images
            BrushExpander.ImageSource = RelatedProject.Root.Images
            PositionExpander.ImageSource = RelatedProject.Root.Images
            ValueExpander.ImageSource = RelatedProject.Root.Images
            EffectExpander.ImageSource = RelatedProject.Root.Images

            TextExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
            BrushExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
            PositionExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
            ValueExpander.BrushStylesSource = RelatedProject.Root.BrushStyles
            TextExpander.BrushStylesSource = RelatedProject.Root.BrushStyles

            TextExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
            BrushExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
            PositionExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
            ValueExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
            EffectExpander.EffectStylesSource = RelatedProject.Root.EffectStyles
        End If
        For Each pi As System.Reflection.PropertyInfo In Item.GetType.GetProperties
            If pi.IsAct Then
                Select Case pi.PropertyType.AnimationType
                    Case AnimationTypeEnum.Brush
                        BrushExpander.AddProperty(Item, pi, Empty)
                    Case AnimationTypeEnum.Effect
                        EffectExpander.AddProperty(Item, pi, Empty)
                    Case AnimationTypeEnum.Movement
                        PositionExpander.AddProperty(Item, pi, Empty)
                    Case AnimationTypeEnum.Switch
                        'BrushExpander.AddProperty(Item, pi)
                    Case AnimationTypeEnum.Text
                        TextExpander.AddProperty(Item, pi, Empty)
                    Case AnimationTypeEnum.Value
                        ValueExpander.AddProperty(Item, pi, Empty)
                End Select
            End If
        Next
        TextExpander.IsExpanded = True
        BrushExpander.IsExpanded = True
        PositionExpander.IsExpanded = True
        ValueExpander.IsExpanded = True
        EffectExpander.IsExpanded = True
        x.Dispose()
        'CreateProperties(Item)
    End Sub
    'Private PropertyMutex As New System.Threading.Mutex
    'Public Function CreateProperties(Item As IActor) As System.Threading.Tasks.Task(Of Boolean)
    '    Dim t As New System.Threading.Tasks.Task(Of Boolean)(Function()
    '                                                             PropertyMutex.WaitOne()
    '                                                             TextExpander.Clear()
    '                                                             BrushExpander.Clear()
    '                                                             PositionExpander.Clear()
    '                                                             ValueExpander.Clear()
    '                                                             EffectExpander.Clear()
    '                                                             If RelatedProject IsNot Nothing Then
    '                                                                 TextExpander.ImageSource = RelatedProject.Root.Images
    '                                                                 BrushExpander.ImageSource = RelatedProject.Root.Images
    '                                                                 PositionExpander.ImageSource = RelatedProject.Root.Images
    '                                                                 ValueExpander.ImageSource = RelatedProject.Root.Images
    '                                                                 EffectExpander.ImageSource = RelatedProject.Root.Images
    '                                                             End If
    '                                                             For Each pi As System.Reflection.PropertyInfo In Item.GetType.GetProperties
    '                                                                 If pi.IsAct Then
    '                                                                     Select Case pi.PropertyType.AnimationType
    '                                                                         Case AnimationTypeEnum.Brush
    '                                                                             BrushExpander.AddProperty(Item, pi)
    '                                                                         Case AnimationTypeEnum.Effect
    '                                                                             EffectExpander.AddProperty(Item, pi)
    '                                                                         Case AnimationTypeEnum.Movement
    '                                                                             PositionExpander.AddProperty(Item, pi)
    '                                                                         Case AnimationTypeEnum.Switch
    '                                                                             'BrushExpander.AddProperty(Item, pi)
    '                                                                         Case AnimationTypeEnum.Text
    '                                                                             TextExpander.AddProperty(Item, pi)
    '                                                                         Case AnimationTypeEnum.Value
    '                                                                             ValueExpander.AddProperty(Item, pi)
    '                                                                     End Select
    '                                                                 End If
    '                                                             Next
    '                                                             TextExpander.IsExpanded = True
    '                                                             BrushExpander.IsExpanded = True
    '                                                             PositionExpander.IsExpanded = True
    '                                                             ValueExpander.IsExpanded = True
    '                                                             EffectExpander.IsExpanded = True
    '                                                             PropertyMutex.ReleaseMutex()
    '                                                             Return True
    '                                                         End Function)
    '    t.Start()
    '    Return t
    'End Function
End Class

Public Interface IValueEditor
    Property Value As Object
    Event ValueChanged As RoutedEventHandler
End Interface

Public Class StringBox
    Inherits GridBase
    Implements IValueEditor
    Private Label As New System.Windows.Controls.Label
    Private WithEvents T As New System.Windows.Controls.TextBox
    Public Sub New()
        AddColumnItem(Label)
        AddColumnItem(T)
    End Sub
    Public Property Title As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return T.Text
        End Get
        Set(value As Object)
            Setting = True
            Try
                T.Text = value
            Catch ex As Exception
            End Try
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False

    Private Sub T_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles T.TextChanged
        If Setting Then Exit Sub
        OnValueChanged()
    End Sub
    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Public Event ValueChanged As RoutedEventHandler Implements IValueEditor.ValueChanged
End Class

Public Class ColorBox
    Inherits GridBase
    Implements IValueEditor
    Private Label As New System.Windows.Controls.Label
    Private WithEvents T As New Rectangle With {.Width = 24, .Height = 24}
    Public Sub New()
        T.Fill = EffectBox.EffectBrush
        AddColumnItem(Label)
        AddColumnItem(T)
    End Sub
    Public Property Title As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property
    Private _Color As Color = Colors.Transparent
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return _Color
        End Get
        Set(value As Object)
            Setting = True
            Try
                If TypeOf value Is Color Then
                    _Color = value
                    T.Fill = New SolidColorBrush(_Color)
                End If
            Catch ex As Exception
            End Try
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False
    Public RequiredColorEditor As BrushEditor

    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Private Sub CallBack(br As Brush)
        If TypeOf br Is SolidColorBrush Then
            _Color = DirectCast(br, SolidColorBrush).Color
            T.Fill = New SolidColorBrush(_Color)
        End If
        OnValueChanged()
    End Sub
    Public Event ValueChanged As RoutedEventHandler Implements IValueEditor.ValueChanged
    Private Sub T_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles T.MouseLeftButtonDown
        If RequiredColorEditor.OK Then
            RequiredColorEditor.ValueCallBack = AddressOf CallBack
            RequiredColorEditor.Visibility = Windows.Visibility.Visible
        End If
    End Sub
End Class
Public Class DoubleBox
    Inherits GridBase
    Implements IValueEditor
    Private Label As New System.Windows.Controls.Label
    Private WithEvents Bar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.Red, .Stroke = Brushes.Black}
    Private WithEvents T As New NumberBox
    Public Sub New()
        T.Value = 0D
        AddColumnItem(Label)
        AddColumnItem(Bar)
        AddColumnItem(T)
    End Sub
    Public Property Title As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property


    Public Property Value As Object Implements IValueEditor.Value
        Get
            Dim d As Double = 0D
            Try
                d = T.Value
            Catch ex As Exception

            End Try
            Return d
        End Get
        Set(value As Object)
            Setting = True
            Try
                T.Value = value
            Catch ex As Exception
            End Try
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False

    Private Sub T_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles T.TextChanged
        If Setting Then Exit Sub
        OnValueChanged()
    End Sub
    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Public Event ValueChanged As RoutedEventHandler Implements IValueEditor.ValueChanged

    Private Dragging As Boolean = False
    Private StartPoint As System.Windows.Point
    Private StartValue As Double
    Private Sub Bar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Bar.MouseLeftButtonDown
        Dragging = True
        StartPoint = e.GetPosition(Me)
        StartValue = Value
        Bar.CaptureMouse()
    End Sub
    Private Sub Bar_MouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Bar.MouseLeftButtonUp
        If Dragging Then
            Dragging = False
            Dim p As System.Windows.Point = e.GetPosition(Me)
            Value = StartValue + (p - StartPoint).X
            Bar.ReleaseMouseCapture()
            OnValueChanged()
        End If
    End Sub
    Private Sub Bar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles Bar.MouseMove
        If Dragging Then
            Dim p As System.Windows.Point = e.GetPosition(Me)
            Value = StartValue + (p - StartPoint).X
            OnValueChanged()
        End If
    End Sub

End Class
Public Class TouchBox
    Inherits Label
    Implements IValueEditor
    Public Property Value As Object Implements IValueEditor.Value
        Get

        End Get
        Set(value As Object)

        End Set
    End Property
    Public Property Title As String
        Get
            Return Content
        End Get
        Set(value As String)
            Content = value
        End Set
    End Property
    Protected Overrides Sub OnMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        RaiseEvent ValueChanged(Me, e)
        MyBase.OnMouseLeftButtonDown(e)
    End Sub
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
End Class
Public Class PointBox
    Inherits GridBase
    Implements IValueEditor
    Private Label As New System.Windows.Controls.Label
    Private WithEvents Bar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.Red, .Stroke = Brushes.Black}
    Private WithEvents X As New NumberBox
    Private WithEvents Y As New NumberBox
    Private Setting As Boolean = False
    Public Sub New()
        X.Value = 0D
        Y.Value = 0D
        AddColumnItem(Label)
        AddColumnItem(Bar)
        AddColumnItem(X)
        AddColumnItem(Y)
    End Sub
    Public Property Title As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Dim p As System.Windows.Point
            Try
                Dim _x As Double = X.Value
                Dim _y As Double = Y.Value
                p.X = _x
                p.Y = _y
            Catch ex As Exception

            End Try
            Return p
        End Get
        Set(value As Object)
            Setting = True
            Try
                X.Value = value.X
                Y.Value = value.Y
            Catch ex As Exception

            End Try
            Setting = False
        End Set
    End Property
    Private Sub X_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles X.TextChanged, Y.TextChanged
        If Setting Then Exit Sub
        OnValueChanged()
    End Sub
    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Public Event ValueChanged As RoutedEventHandler Implements IValueEditor.ValueChanged

    Private Dragging As Boolean = False
    Private StartPoint As System.Windows.Point
    Private StartValue As System.Windows.Point
    Private Sub Bar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Bar.MouseLeftButtonDown
        Dragging = True
        StartPoint = e.GetPosition(Me)
        StartValue = Value
        Bar.CaptureMouse()
    End Sub
    Private Sub Bar_MouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Bar.MouseLeftButtonUp
        If Dragging Then
            Dragging = False
            Dim p As System.Windows.Point = e.GetPosition(Me)
            Value = StartValue + p - StartPoint
            OnValueChanged()
            Bar.ReleaseMouseCapture()
        End If
    End Sub
    Private Sub Bar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles Bar.MouseMove
        If Dragging Then
            Dim p As System.Windows.Point = e.GetPosition(Me)
            Value = StartValue + p - StartPoint
            OnValueChanged()
        End If
    End Sub
End Class

Public Class EffectBox
    Inherits GridBase
    Implements IValueEditor
    Private WithEvents Rect As New Rectangle
    Private Text As New System.Windows.Controls.Label
    Public Sub New()
        Text.Content = "Effect"
        Rect.Fill = EffectBrush
        Children.Add(Rect)
        Children.Add(Text)
        'AddHandler Rect.MouseLeftButtonDown, AddressOf CallEditor
    End Sub
    Public Sub CallEditor(s As Object, e As MouseButtonEventArgs) Handles Rect.MouseLeftButtonDown, Me.MouseLeftButtonDown
        If RequireEdit.OK Then RequireEdit.Invoke(Me.Effect)
    End Sub
    Public Sub ChangeValue(ef As Effect)
        Me.Effect = ef
        OnValueChanged()
    End Sub
    Public Shared ReadOnly Property EffectBrush As LinearGradientBrush
        Get
            Dim lgb As New LinearGradientBrush
            lgb.StartPoint = P(0, 0.5)
            lgb.EndPoint = P(1, 0.5)
            lgb.MappingMode = BrushMappingMode.RelativeToBoundingBox
            lgb.SpreadMethod = GradientSpreadMethod.Pad
            lgb.GradientStops.Add(New GradientStop(Colors.Red, 0D))
            lgb.GradientStops.Add(New GradientStop(Colors.Yellow, 0.2D))
            lgb.GradientStops.Add(New GradientStop(Colors.Green, 0.4D))
            lgb.GradientStops.Add(New GradientStop(Colors.Cyan, 0.6D))
            lgb.GradientStops.Add(New GradientStop(Colors.Blue, 0.8D))
            lgb.GradientStops.Add(New GradientStop(Colors.Violet, 1D))
            Return lgb
        End Get
    End Property

    Private Setting As Boolean = False
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return Me.Effect
        End Get
        Set(value As Object)
            Setting = True
            Me.Effect = value
            Setting = False
        End Set
    End Property
    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
    Public RequireEdit As System.Action(Of Effect)
End Class
Public Class BrushBox
    Inherits GridBase
    Implements IValueEditor
    Private WithEvents Rect As New Rectangle
    Private Setting As Boolean = False
    Public Sub New()
        Rect.Fill = EffectBox.EffectBrush
        Children.Add(Rect)
        Background = EffectBox.EffectBrush
    End Sub
    Public Sub CallEditor(s As Object, e As MouseButtonEventArgs) Handles Rect.MouseLeftButtonDown, Me.MouseLeftButtonDown
        If RequireEdit.OK Then RequireEdit.Invoke(Rect.Fill)
    End Sub
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return Rect.Fill
        End Get
        Set(value As Object)
            Setting = True
            Try
                Rect.Fill = value
            Catch ex As Exception

            End Try
            Setting = False
        End Set
    End Property
    Public Sub OnValueChanged()
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged

    Public Sub ChangeValue(vBrush As Brush)
        Rect.Fill = vBrush
        OnValueChanged()
    End Sub
    Public RequireEdit As System.Action(Of Brush)

End Class

Public Class PropertyItem
    Public Visible As New IdeaButton
    Public WithEvents Label As New System.Windows.Controls.Label
    Public WithEvents ValueEditor As IValueEditor
    Public RelatedObject As Object
    Public PropertyInfo As System.Reflection.PropertyInfo
    Public Event RequireExtraEditor As RoutedEventHandler
    Public ExtraGrid As GridBase


    Private WithEvents TextEditor As New StringBox
    Private WithEvents BrushEditor As New BrushBox
    Private WithEvents EffectEditor As New EffectBox
    Private WithEvents NumberEditor As New DoubleBox
    Private WithEvents PointEditor As New PointBox

    Private Shared Empty As Object() = New Object() {}

    Private CombinedObjects As Object()
    Public Sub New(obj As Object, pi As System.Reflection.PropertyInfo, vCombinedObjects As Object())
        PropertyInfo = pi
        RelatedObject = obj
        CombinedObjects = vCombinedObjects
        Label.Content = PropertyInfo.Name
        Select Case pi.PropertyType.AnimationType
            Case AnimationTypeEnum.Brush
                ValueEditor = BrushEditor
                BrushEditor.RequireEdit = AddressOf ShowBrushEditor
            Case AnimationTypeEnum.Effect
                ValueEditor = EffectEditor
                EffectEditor.RequireEdit = AddressOf ShowEffectEditor
            Case AnimationTypeEnum.Movement
                ValueEditor = PointEditor
            Case AnimationTypeEnum.Switch

            Case AnimationTypeEnum.Text
                ValueEditor = TextEditor
            Case AnimationTypeEnum.Value
                ValueEditor = NumberEditor
        End Select
        ValueEditor.Value = PropertyInfo.GetValue(RelatedObject, Empty)
    End Sub

    Public BrushEdit As BrushEditor
    Public EffectEdit As EffectEditor
    Public Sub ShowEffectEditor(Value As Effect)
        ExtraGrid.Children.Clear()
        EffectEdit.ValueCallBack = AddressOf EffectEditor.ChangeValue
        EffectEdit.SetEffect(Value)
        ExtraGrid.Children.Add(EffectEdit)
    End Sub
    Public Sub ShowBrushEditor(Value As Brush)
        ExtraGrid.Children.Clear()
        BrushEdit.Activate()
        BrushEdit.SetBrush(Value)
        BrushEdit.ValueCallBack = AddressOf BrushEditor.ChangeValue
        ExtraGrid.Children.Add(BrushEdit)
    End Sub


    Private Sub ValueEditor_ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Handles ValueEditor.ValueChanged
        PropertyInfo.SetValue(RelatedObject, ValueEditor.Value, Empty)
        Try
            If CombinedObjects.OK Then
                For Each co As Object In CombinedObjects
                    Try
                        PropertyInfo.SetValue(co, ValueEditor.Value, Empty)
                    Catch ex As Exception

                    End Try
                Next
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class

Public Class BrushEditor
    Inherits StackPanel
    Implements IValueEditor
    Public RelatedProperty As PropertyItem
    Private WithEvents Standards As New StandardBrushEditor
    Private WithEvents Gradients As New GradientBrushEditor
    Public WithEvents Images As New ImageBrushEditor
    Public WithEvents Styles As New BrushStyleEditor
    Public ValueCallBack As System.Action(Of Brush)
    Public Sub New()
        Height = 400D
        Children.Add(Standards)
        Children.Add(Gradients)
        Children.Add(Images)
        Children.Add(Styles)
    End Sub
    Public Sub New(ColorOnly As Boolean)
        Height = 400D
        Children.Add(Standards)
        If ColorOnly Then
            Children.Add(Gradients)
            Gradients.Picker.LockMode(PickerModeEnum.Solid)
        Else
            Children.Add(Gradients)
            Children.Add(Images)
        End If
    End Sub
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return LastValue
        End Get
        Set(value As Object)
            LastValue = value
            If value Is Nothing Then

            ElseIf TypeOf value Is SolidColorBrush Then
                If Standards.IsStandardSolidColorBrush(value) Then
                    Standards.Value = value
                    Standards.IsExpanded = True
                Else
                    Gradients.Value = value
                    Gradients.IsExpanded = True
                End If
            ElseIf TypeOf value Is ImageBrush Then

            Else
                Gradients.Value = value
                Gradients.IsExpanded = True
            End If
            Styles.Value = value
        End Set
    End Property
    Private LastValue As Brush
    Public Sub OnValueUpdate(sender As Object, e As System.Windows.RoutedEventArgs) Handles Standards.ValueChanged, Gradients.ValueChanged, Images.ValueChanged, Styles.ValueChanged
        LastValue = DirectCast(sender, IValueEditor).Value
        RaiseEvent ValueChanged(Me, e)
        ValueCallBack.Invoke(Value)
    End Sub
    Public Sub Activate()
        Images.Activate()
        Styles.Activate()
    End Sub
    Public Sub SetBrush(value As Brush)
        LastValue = Value
        If Value Is Nothing Then

        ElseIf TypeOf Value Is SolidColorBrush Then
            If Standards.IsStandardSolidColorBrush(Value) Then
                Standards.Value = Value
                Standards.IsExpanded = True

            End If
        ElseIf TypeOf Value Is ImageBrush Then

        Else
            Gradients.Value = Value
            Gradients.IsExpanded = True
        End If
        If TypeOf value Is SolidColorBrush OrElse TypeOf value Is LinearGradientBrush OrElse TypeOf value Is RadialGradientBrush Then
            Gradients.Value = value
            Gradients.IsExpanded = True
        End If
        Styles.Value = Value
    End Sub
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
End Class
Public Class StandardBrushEditor
    Inherits Expander
    Implements IValueEditor
    Private WithEvents ColorList As New ListView
    Public Sub New()
        Header = "Standard Colors"
        ColorList.Height = 360
        Content = ColorList
        For Each ci As ColorItem In ColorItem.GetItems
            ci.Host = Me
            ColorList.Items.Add(ci)
        Next
    End Sub
    Private Setting As Boolean = False
    Public Property Value As Object Implements IValueEditor.Value
        Get
            If TypeOf ColorList.SelectedItem Is ColorItem Then
                Dim it As ColorItem = ColorList.SelectedItem
                Return it.Brush
            Else
                Return Nothing
            End If
        End Get
        Set(value As Object)
            If TypeOf value Is SolidColorBrush Then
                Dim c As Color = DirectCast(value, SolidColorBrush).Color
                Setting = True
                For Each it As ColorItem In ColorList.Items

                    If it.BrushColor = c Then
                        it.Selected = True
                    Else
                        it.Selected = False
                    End If
                Next
                Setting = False
            Else
                Setting = True
                For Each it As ColorItem In ColorList.Items

                    it.Selected = False
                Next
                Setting = False
            End If
        End Set
    End Property
    Public Function IsStandardSolidColorBrush(vBrush As Brush) As Boolean
        If TypeOf vBrush Is SolidColorBrush Then
            Dim sb As SolidColorBrush = vBrush
            For Each b As SolidColorBrush In ColorItem.Brushes.Values
                If b.Color = sb.Color Then
                    Return True
                End If
            Next
            Return False
        Else
            Return False
        End If
    End Function
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged

    Public Sub SelectColor(ci As ColorItem)
        If Setting Then Exit Sub
        ColorList.SelectedItem = ci
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
End Class
Public Class GradientBrushEditor
    Inherits Expander
    Implements IValueEditor
    Public WithEvents Picker As New ColorPicker
    Public Sub New()
        Header = "Picking Colors"
        Content = Picker
    End Sub
    Private Setting As Boolean = False
    Public Property Value As Object Implements IValueEditor.Value
        Get
            Return Picker.Brush
        End Get
        Set(value As Object)
            Setting = True
            Picker.Brush = value
            Setting = False
        End Set
    End Property
    Private Sub ColorPicked(sender As Object, e As System.Windows.RoutedEventArgs) Handles Picker.BrushChanged
        RaiseEvent ValueChanged(Me, e)
    End Sub
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
End Class
Public Class ColorItem
    Inherits GridBase
    Public ImageSelected As New IdeaButton
    Public Label As New System.Windows.Controls.Label
    Public WithEvents Color As New Rectangle With {.Width = 120D}
    Public Host As StandardBrushEditor

    Private cKey As String
    Private Sub New(vName As String, vBrush As SolidColorBrush)
        Label.Content = vName
        cKey = vName
        Color.Fill = vBrush
        AddColumnItem(ImageSelected)
        ImageSelected.Visibility = Windows.Visibility.Hidden
        AddColumnItem(Color)
        AddColumnItem(Label)
    End Sub
    Public Shared Brushes As New Dictionary(Of String, SolidColorBrush)
    Public Property Selected As Boolean
        Get
            Return ImageSelected.Visibility = Windows.Visibility.Visible
        End Get
        Set(value As Boolean)
            If value Then
                ImageSelected.Visibility = Windows.Visibility.Visible
            Else
                ImageSelected.Visibility = Windows.Visibility.Hidden
            End If
        End Set
    End Property
    Public ReadOnly Property Brush As SolidColorBrush
        Get
            Return Brushes(cKey)
        End Get
    End Property
    Public ReadOnly Property BrushColor As Color
        Get
            Return Brushes(cKey).Color
        End Get
    End Property

    Shared Sub New()
        Dim t As Type = GetType(System.Windows.Media.Brushes)
        For Each pi As System.Reflection.PropertyInfo In t.GetProperties
            Brushes.Add(pi.Name, pi.GetValue(Nothing, New Object() {}))
        Next
    End Sub
    Public Shared Function GetItems() As List(Of ColorItem)
        Dim l As New List(Of ColorItem)
        For Each key As String In Brushes.Keys
            l.Add(New ColorItem(key, Brushes(key)))
        Next
        Return l
    End Function

    Private Sub Color_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Color.MouseLeftButtonDown
        Host.SelectColor(Me)
    End Sub
End Class

Public Class ImageBrushEditor
    Inherits Expander
    Implements IValueEditor
    Private HostGrid As New GridBase
    Private HeaderGrid As New GridBase
    Private WithEvents ImageList As New ListView
    Private WithEvents btnAdd As New AddButton
    Private WithEvents StretchMode As New System.Windows.Controls.ComboBox
    Private WithEvents TileMode As New System.Windows.Controls.ComboBox
    Public Sub New()
        Header = "Images"
        Content = HostGrid
        HostGrid.AddRowItem(HeaderGrid)
        HeaderGrid.AddColumnItem(btnAdd)
        HeaderGrid.AddColumnItem(StretchMode)
        HeaderGrid.AddColumnItem(TileMode)
        HostGrid.AddRowItem(ImageList)
        ImageList.Height = 360
        Setting = True
        StretchMode.Items.Add(Stretch.None)
        StretchMode.Items.Add(Stretch.Fill)
        StretchMode.Items.Add(Stretch.Uniform)
        StretchMode.Items.Add(Stretch.UniformToFill)
        StretchMode.SelectedItem = Stretch.UniformToFill
        TileMode.Items.Add(System.Windows.Media.TileMode.None)
        TileMode.Items.Add(System.Windows.Media.TileMode.Tile)
        TileMode.Items.Add(System.Windows.Media.TileMode.FlipX)
        TileMode.Items.Add(System.Windows.Media.TileMode.FlipY)
        TileMode.Items.Add(System.Windows.Media.TileMode.FlipXY)
        TileMode.SelectedItem = System.Windows.Media.TileMode.None
        Setting = False
    End Sub
    Public Sub Activate()
        ImageList.Items.Clear()
        'If Source IsNot Nothing Then
        For Each si As StreamImage In Source.Values
            Dim it = si.GetPreviewItem
            it.Host = Me
            ImageList.Items.Add(it)
        Next
        'End If
    End Sub
    Public Property Value As Object Implements IValueEditor.Value
        Get
            If TypeOf ImageList.SelectedItem Is ImageItem Then
                Dim ib As New ImageBrush
                ib.Stretch = StretchMode.SelectedItem
                ib.TileMode = TileMode.SelectedItem
                Dim it As ImageItem = ImageList.SelectedItem
                ib.ImageSource = it.RelatedSource.GetImageSource

                Return ib
            Else
                Return Nothing
            End If
        End Get
        Set(value As Object)
            Setting = True

            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False
    Public Source As ShallowDictionary(Of Double, StreamImage)
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        Dim obj = StreamImage.FromFile
        If obj IsNot Nothing Then
            If Source IsNot Nothing Then
                While Source.ContainsKey(obj.DoubleKey)
                    obj.DoubleKey = Rnd()
                End While
                Source.Add(obj.DoubleKey, obj)
            End If
            Dim it = obj.GetPreviewItem
            it.Host = Me
            ImageList.Items.Add(it)
            ImageList.SelectedItem = it
        End If
    End Sub
    Public Sub SelectImage(it As ImageItem)
        If Setting Then Exit Sub
        ImageList.SelectedItem = it
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub

    Private Sub Mode_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles StretchMode.SelectionChanged, TileMode.SelectionChanged
        If TypeOf ImageList.SelectedItem Is ImageItem Then
            RaiseEvent ValueChanged(Me, New RoutedEventArgs)
        End If
    End Sub
End Class
Public Class ImageItem
    Inherits GridBase
    Public WithEvents ImageID As New System.Windows.Controls.TextBox
    Public WithEvents ImageView As New System.Windows.Controls.Image With {.Width = 120, .Height = 120}
    Public RelatedSource As StreamImage
    Public Host As ImageBrushEditor
    Public Sub New()
        AddColumnItem(ImageView)
        AddColumnItem(ImageID)
    End Sub
    Public Property Text As String
        Get
            Return ImageID.Text
        End Get
        Set(value As String)
            Setting = True
            ImageID.Text = value
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False
    Private Sub ImageID_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles ImageID.TextChanged
        If Setting Then Exit Sub
        RelatedSource.Key = ImageID.Text
    End Sub
    Private Sub ImageView_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles ImageView.MouseLeftButtonDown
        Host.SelectImage(Me)
    End Sub
End Class
Public Class BrushStyleEditor
    Inherits Expander
    Implements IValueEditor
    Private HostGrid As New GridBase
    Private HeaderGrid As New GridBase
    Private WithEvents ImageList As New ListView
    Private WithEvents btnAdd As New AddButton
    Private WithEvents btnDelete As New DeleteButton
    Public Sub New()
        Header = "BrushStyle"
        Content = HostGrid
        HostGrid.VerticalAlignment = Windows.VerticalAlignment.Stretch
        HostGrid.AddRowItem(HeaderGrid)
        HeaderGrid.AddColumnItem(btnAdd)
        HeaderGrid.AddColumnItem(btnDelete)
        HostGrid.AddRowItem(ImageList)
        ImageList.Height = 300
        'Me.VerticalAlignment = Windows.VerticalAlignment.Stretch
        Setting = True
        Setting = False
    End Sub
    Public Sub Activate()
        ImageList.Items.Clear()
        'If Source IsNot Nothing Then
        For Each bs As BrushStyle In Source
            Dim it = BrushStyleItem.FromBrushStyle(bs)
            it.Host = Me
            ImageList.Items.Add(it)
        Next
        'End If
    End Sub
    Private CurrentValue As Brush
    Public Property Value As Object Implements IValueEditor.Value
        Get
            If TypeOf ImageList.SelectedItem Is BrushStyleItem Then
                Dim ib As BrushStyleItem = ImageList.SelectedItem
                Return ib.Value
            Else
                Return Nothing
            End If
        End Get
        Set(value As Object)
            Setting = True
            CurrentValue = value
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False
    Public Source As ShallowList(Of BrushStyle)
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        If CurrentValue.OK Then
            Dim bs As New BrushStyle With {.Name = "New", .Vector = New BrushVector(CurrentValue)}
            If Source.OK Then Source.Add(bs)
            Dim it = BrushStyleItem.FromBrushStyle(bs)
            it.Host = Me
            ImageList.Items.Add(it)
        End If
    End Sub
    Public Sub SelectItem(it As BrushStyleItem)
        If Setting Then Exit Sub
        ImageList.SelectedItem = it
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If TypeOf ImageList.SelectedItem Is BrushStyleItem Then
            Dim bsi As BrushStyleItem = ImageList.SelectedItem
            ImageList.Items.Remove(ImageList)
            Source.Remove(bsi.RelatedBrushStyle)
        End If
    End Sub
End Class

Public Class BrushStyleItem
    Inherits GridBase
    Public WithEvents BrushView As New BrushBox
    Public WithEvents Key As New EditBox With {.IsReadOnly = False}
    Public Host As BrushStyleEditor
    Public Sub New()
        AddColumnItem(BrushView, "80")
        AddColumnItem(Key)
    End Sub
    Private Setting As Boolean = False
    Public Property BrushName As String
        Get
            Return Key.Text
        End Get
        Set(value As String)
            Setting = True
            Key.Text = value
            Setting = False
        End Set
    End Property

    Public Property Value As Brush
        Get
            Return BrushView.Value
        End Get
        Set(value As Brush)
            BrushView.Value = value
        End Set
    End Property
    Public RelatedBrushStyle As BrushStyle
    Public Shared Function FromBrushStyle(bs As BrushStyle)
        Dim bsi As New BrushStyleItem
        bsi.Setting = True
        bsi.BrushName = bs.Name
        bsi.RelatedBrushStyle = bs
        bsi.BrushView.Value = bs.Vector.GetValue
        bsi.Setting = False
        Return bsi
    End Function
    Public Sub SelectMe()
        If Host.OK Then Host.SelectItem(Me)
    End Sub
    Private Sub BrushView_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles BrushView.MouseLeftButtonDown
        SelectMe()
    End Sub

    Private Sub Key_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles Key.TextChanged
        If Setting Then Exit Sub
        If RelatedBrushStyle.OK Then RelatedBrushStyle.Name = Key.Text
    End Sub
End Class
<Serializable()>
Public Class BrushStyle
    Public Name As String
    Public Vector As BrushVector
End Class

Public Class PropertyExpander
    Inherits Expander
    Public Sub Clear()
        ExtraGrid.Children.Clear()
        ValueGrid.Children.Clear()
        ValueGrid.RowDefinitions.Clear()
    End Sub
    Public ImageSource As ShallowDictionary(Of Double, StreamImage)
    Public EffectStylesSource As ShallowList(Of EffectStyle)
    Public BrushStylesSource As ShallowList(Of BrushStyle)
    Public Shadows ReadOnly Property Parent As StackPanel
        Get
            Return MyBase.Parent
        End Get
    End Property
    Public Sub AddProperty(obj As Object, pi As System.Reflection.PropertyInfo, vCombinedObjects As Object())
        BrushEdit.Images.Source = ImageSource
        EffectEdit.Styles.Source = EffectStylesSource
        BrushEdit.Styles.Source = BrushStylesSource
        Dim it As New PropertyItem(obj, pi, vCombinedObjects)
        ValueGrid.RowDefinitions.Add(New RowDefinition() With {.Height = New System.Windows.GridLength(1, GridUnitType.Auto)})
        it.ExtraGrid = ExtraGrid
        it.BrushEdit = BrushEdit
        it.EffectEdit = EffectEdit
        'it.LayoutHandler = AddressOf Layout
        Dim row As Integer = ValueGrid.RowDefinitions.Count - 1
        ValueGrid.Children.Add(it.Visible)
        ValueGrid.Children.Add(it.Label)
        ValueGrid.Children.Add(it.ValueEditor)
        Grid.SetRow(it.Visible, row)
        Grid.SetRow(it.Label, row)
        Grid.SetRow(it.ValueEditor, row)
        Grid.SetColumn(it.Visible, 0)
        Grid.SetColumn(it.Label, 1)
        Grid.SetColumn(it.ValueEditor, 2)
    End Sub
    Private HostGrid As New GridBase
    Private ValueGrid As New GridBase
    Private ExtraGrid As New GridBase
    Private BrushEdit As New BrushEditor
    Private EffectEdit As New EffectEditor
    Public WithEvents Label As New System.Windows.Controls.Label
    Public Sub New()
        MyBase.Header = Label
        Content = HostGrid
        ValueGrid.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New System.Windows.GridLength(1, GridUnitType.Auto)})
        ValueGrid.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New System.Windows.GridLength(1, GridUnitType.Auto)})
        ValueGrid.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New System.Windows.GridLength(1, GridUnitType.Star)})
        ExtraGrid.RowDefinitions.Add(New RowDefinition() With {.Height = New System.Windows.GridLength(1, GridUnitType.Auto)})
        HostGrid.AddRowItem(ValueGrid)
        HostGrid.AddRowItem(ExtraGrid)
    End Sub
    Public Shadows Property Header As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property
End Class


Public Class EnumCombo
    Inherits ComboBox
    Public Sub New(vT As Type)
        If vT.IsEnum Then
            For Each Value In [Enum].GetValues(vT)
                Me.Items.Add(Value)
            Next
        End If
        Me.SelectedIndex = 0
    End Sub
End Class

