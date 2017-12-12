Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Media.Effects, System.Windows.Shapes, System.Windows.Input
<Shallow()>
Public Class ControlPoint
    Inherits System.Windows.Controls.Grid
    Public WithEvents Dragger As New System.Windows.Shapes.Rectangle
    Public Sub New(vID As String,
                   bTarget As IActor,
                  Optional GridOffsetX As Integer = 12,
                   Optional GridOffsetY As Integer = 12)
        vTarget = bTarget
        _ID = vID
        GridX = GridOffsetX
        GridY = GridOffsetY
        If GridX < 1 Then GridX = 12
        If GridY < 1 Then GridY = 12
        Width = 10
        Height = 10
        Dragger.Fill = Brushes.Yellow
        Dragger.Stroke = Brushes.Black
        Children.Add(Dragger)
    End Sub
    Public Sub New(vID As String,
                   bTarget As IActor,
                    LeftClick As System.Action,
                   Optional GridOffset As System.Windows.Vector = Nothing)
        vTarget = bTarget
        _ID = vID
        GridX = GridOffset.X
        GridY = GridOffset.Y
        If GridX < 1 Then GridX = 12
        If GridY < 1 Then GridY = 12
        _LeftClick = LeftClick
        Width = 10
        Height = 10
        Dragger.Fill = Brushes.Yellow
        Dragger.Stroke = Brushes.Black
        Children.Add(Dragger)
    End Sub
    Public Sub New(vID As String,
               bTarget As IActor,
                LeftClick As System.Action,
               RightClick As System.Action)
        vTarget = bTarget
        _ID = vID
        GridX = 12
        GridY = 12
        _LeftClick = LeftClick
        _RightClick = RightClick
        Width = 10
        Height = 10
        Dragger.Fill = Brushes.Yellow
        Dragger.Stroke = Brushes.Black
        Children.Add(Dragger)
    End Sub
    <Save()> Public Property CanBind As Boolean = True
    <LateLoad()> Public Property MutualBind As Boolean = False
    Public Property Fill As Brush
        Get
            Return Dragger.Fill
        End Get
        Set(value As Brush)
            Dragger.Fill = value
        End Set
    End Property
    Public Property Stroke As Brush
        Get
            Return Dragger.Stroke
        End Get
        Set(value As Brush)
            Dragger.Stroke = value
        End Set
    End Property
    Public ReadOnly Property Mapping(vHost As IActor) As ControlPointMapping
        Get
            DependentTarget = vHost
            Return New ControlPointMapping With {.Host = vHost, .Point = Me, .Target = vTarget}
        End Get
    End Property
    Private _LeftClick As System.Action
    Private _RightClick As System.Action
    Private _ID As String
    Public ReadOnly Property ID As String
        Get
            Return _ID
        End Get
    End Property
    Public ReadOnly Property TargetID As String
        Get
            Return vTarget.ID
        End Get
    End Property
    Public Sub AddToStage()
        Host.ElementCollection.Add(Me)
        Move()
    End Sub
    Public Sub RemoveFromStage()
        Host.ElementCollection.Remove(Me)
        RaiseEvent Removed(Me, Nothing)
    End Sub
    Public Event Removed As EventHandler
    ''' <summary>
    ''' 被动移动到宿主的布局点
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Move()
        '因为这里是被动刷新位置 所以不需要设置sender.
        PositionMove(Nothing, vTarget.AdornerLocation(New List(Of Object) From {Me}, _ID), False)
        If TypeOf Host Is FreeStage AndAlso Host.DesignMode Then
            Visibility = Windows.Visibility.Visible
        Else
            Visibility = Windows.Visibility.Hidden
        End If
    End Sub
    Private Dragging As Boolean = False
    Private StartP As System.Windows.Point
    Private StartV As System.Windows.Point
    Public Property IsAppearanceButton As Boolean = False
    Public Property Color As System.Windows.Media.Color
        Get
            Return DirectCast(Dragger.Fill, SolidColorBrush).Color
        End Get
        Set(value As System.Windows.Media.Color)
            Dragger.Fill = New SolidColorBrush(value)
        End Set
    End Property
    Public Property Position As System.Windows.Point
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            Dim cl = CanvasLayout.Layout(Me)
            Return P(cl.X + w / 2, cl.Y + h / 2)
        End Get
        Set(value As System.Windows.Point)
            PositionMove(New List(Of Object) From {Me}, value, True)
        End Set
    End Property
    ''' <summary>
    ''' 使控制点移动到value，如果是Active模式，则会通知其宿主，否则不通知。
    ''' </summary>
    ''' <param name="value">目标点。</param>
    ''' <param name="Active">True时，通知宿主刷新布局；False时，是被动移动到指定点，移动请求可能会被拒绝。</param>
    ''' <remarks></remarks>
    Public Sub PositionMove(sender As List(Of Object), value As System.Windows.Point, Active As Boolean)
        If Active AndAlso vTarget IsNot Nothing Then
            vTarget.AdornerLocation(sender, ID) = value
            value = vTarget.AdornerLocation(sender, ID)
        End If
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim cl = CanvasLayout.Layout(Me)
        Dim x As Double = value.X - w / 2
        Dim y As Double = value.Y - h / 2
        cl.X = x
        cl.Y = y
        CanvasLayout.Layout(Me) = cl
    End Sub
    ''' <summary>
    ''' 父对象所在的Stage容器
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Host As FreeStage
        Get
            Return vTarget.Host
        End Get
    End Property
    Protected _DependentTarget As IActor
    Public Property DependentTarget As IActor
        Get
            Return _DependentTarget
        End Get
        Set(value As IActor)
            _DependentTarget = value
        End Set
    End Property
    Private Adding As Boolean = False
    Private Connecting As Boolean = False
    Private ConnectorSet As Tuple(Of IActor, ControlPoint, ControlPoint)
    <Save()> Public AllowAdding As Boolean = False
    <Save()> Public AllowConnecting As Boolean = False
    Public Function PassMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs, Passing As Boolean) As Boolean
        If Passing Then
            Dim uV As System.Windows.Point = e.GetPosition(Parent)
            Dim cV As System.Windows.Point = Me.Position
            If Math.Abs(uV.X - cV.X) > 5 OrElse Math.Abs(uV.Y - cV.Y) > 5 Then Return False
        End If
        vTarget.Activate(Me, New RoutedEventArgs)

        If _LeftClick IsNot Nothing And vTarget IsNot Nothing Then
            _LeftClick.Invoke()
        ElseIf System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Control AndAlso TypeOf DependentTarget Is IActor Then
            If MutualBind Then
                For Each cp As ControlPoint In DependentTarget.Adorners.Values
                    If cp.Position = Position Then vTarget.ReleasePoint(cp) : cp.DependentTarget = Nothing
                Next
            End If
            DependentTarget.ReleasePoint(Me)
            DependentTarget = Nothing
            Dragging = True
            StartP = Position
            StartV = Me.PointToScreen(e.GetPosition(Me))
            Dragger.CaptureMouse()
            Return True
        ElseIf AllowAdding AndAlso System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Shift Then
            vTarget.AddAdorner(e.GetPosition(Parent), False)
            Adding = True
            Dragger.CaptureMouse()
        ElseIf AllowConnecting AndAlso System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Shift Then
            ConnectorSet = vTarget.AddConnector(e.GetPosition(Parent), Nothing, False)
            Connecting = True
            Dragger.CaptureMouse()
        ElseIf DependentTarget Is Nothing OrElse MutualBind Then
            Dragging = True
            StartP = Position
            StartV = Me.PointToScreen(e.GetPosition(Me))
            Dragger.CaptureMouse()
            Return True
        ElseIf Not Passing Then
            Host.PassMouseLeftButtonDown(e)
        End If
        Return False
    End Function
    Private Sub DraggingRectangle_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Dragger.MouseLeftButtonDown
        PassMouseLeftButtonDown(e, False)
    End Sub
    Private Sub DraggingRectangle_MouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Dragger.MouseLeftButtonUp
        If Dragging Then
            Dim u = Me.PointToScreen(e.GetPosition(Me))
            Position = StartP + (u - StartV) / Host.Scale
            If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Control Then
                If TypeOf Host Is FreeStage Then
                    DependentTarget = Host.TryBindPoint(Me)
                    If MutualBind AndAlso DependentTarget.OK Then
                        For Each cp As ControlPoint In DependentTarget.Adorners.Values
                            If cp.Position = Position Then vTarget.BindPoint(cp)
                        Next
                    End If
                End If
                If DependentTarget Is Nothing Then
                    Adjust()
                End If
            End If

            Dragging = False
            Dragger.ReleaseMouseCapture()
        End If
        If Adding Then
            Adding = False
            vTarget.AddAdorner(e.GetPosition(Parent), True)
            Dragger.ReleaseMouseCapture()
        ElseIf Connecting Then
            Connecting = False
            ConnectorSet = vTarget.AddConnector(e.GetPosition(Parent), ConnectorSet, True)
            Dragger.ReleaseMouseCapture()
        End If
    End Sub
    Private Sub DraggingRectangle_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles Dragger.MouseMove
        If Dragging Then
            Dim u = Me.PointToScreen(e.GetPosition(Me))
            Position = StartP + (u - StartV) / Host.Scale
        End If
        If Adding Then
            vTarget.AddAdorner(e.GetPosition(Parent), False)
        ElseIf Connecting Then
            ConnectorSet = vTarget.AddConnector(e.GetPosition(Parent), ConnectorSet, False)
        End If
    End Sub

    Private vTarget As IActor

    Private Shared Empty As Object() = New Object() {}

    Public Property BindingTarget As IActor
        Get
            Return vTarget
        End Get
        Set(value As IActor)
            vTarget = value
        End Set
    End Property

    Public Property GridX As Integer = 0
    Public Property GridY As Integer = 0
    Public Sub Adjust()
        Dim pos As System.Windows.Point = Position
        If GridX > 0 Then
            pos.X = Math.Round(CInt(pos.X) / GridX) * GridX
        End If
        If GridY > 0 Then
            pos.Y = Math.Round(CInt(pos.Y) / GridY) * GridY
        End If
        Position = pos
    End Sub
    Private Sub Dragger_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Dragger.MouseRightButtonDown
        'bind to the grid
        If TypeOf _RightClick Is System.Action Then
            _RightClick.Invoke()
        ElseIf Input.Keyboard.Modifiers = ModifierKeys.Control Then
            Adjust()
        End If
    End Sub
End Class

<AttributeUsage(AttributeTargets.Property)>
Public Class PointBindAttribute
    Inherits Attribute
    Private _BindName As String
    Public Sub New(vBindName As String)
        _BindName = vBindName
    End Sub
    Public ReadOnly Property BindName As String
        Get
            Return _BindName
        End Get
    End Property
End Class