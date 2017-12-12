Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes

Public Class AdornerEventArgs
    Inherits RoutedEventArgs
    Public AddList As New List(Of String)
    Public RemoveList As New List(Of String)
End Class

<Shallow()>
Public MustInherit Class TextShape
    Inherits GridBase
    Implements IActor
    Public Event AdornerChanged(sender As Object, e As System.EventArgs) Implements IActor.AdornerChanged
    Public MustOverride ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IActor.Adorners
    Public MustOverride ReadOnly Property Shape As System.Windows.Shapes.Shape
    Public WithEvents TextBox As New System.Windows.Controls.TextBox
    Protected TemperatorySender As Object
    Public Sub New()
        TextBox.Background = Brushes.Transparent
        TextBox.BorderBrush = Brushes.Transparent
        TextBox.TextAlignment = TextAlignment.Center
        TextBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
        TextBox.BorderThickness = New Thickness(0)
        TextBox.Cursor = Cursors.Arrow
        Shape.Fill = Brushes.Transparent
        Shape.Stroke = Brushes.Black
        Children.Add(Shape)
        Children.Add(TextBox)
    End Sub
    ''' <summary>
    ''' 向形状添加一个控制点
    ''' </summary>
    ''' <param name="position"></param>
    ''' <remarks></remarks>
    Public Overridable Sub AddAdorner(position As System.Windows.Point, done As Boolean) Implements IActor.AddAdorner

    End Sub
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        OnSizeChanged()
    End Sub
    Protected Overridable Sub OnSizeChanged()
        If Shape IsNot Nothing Then
            Dim az As System.Windows.Vector = ActualSize
            Shape.Width = az.X
            Shape.Height = az.Y
        End If
    End Sub

    Public Sub OnMenu() Implements IActor.OnMenu
        If TypeOf Host Is FreeStage Then
            Dim vT As Type = Me.GetType
            If Not Host.Menus.ContainsKey(vT) Then
                Host.Menus.Add(vT, New AppearanceMenu(vT))
            End If
            Host.Menus(vT).Bind(Me)
            Host.Menus(vT).IsOpen = True
        End If
        'RaiseEvent RequireMenu(Me, Nothing)
    End Sub
    Public MustOverride Function BindPoint(cp As ControlPoint) As Boolean Implements IActor.BindPoint
    Public MustOverride Sub ProcessBindingPoints()
    ''' <summary>
    ''' 内涵形状的边界粗细
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Menu(), Act()> Public Property Border As Double
        Get
            Return Shape.StrokeThickness
        End Get
        Set(value As Double)
            Shape.StrokeThickness = value
        End Set
    End Property
    Private _TextVisibility As Double = 1D
    <Menu(), Act()> Public Property TextVisible As Double
        Get
            If TextBox.Visibility = Windows.Visibility.Hidden Then
                If _TextVisibility > 0D Then _TextVisibility = 0D
            ElseIf TextBox.Visibility = Windows.Visibility.Visible Then
                If _TextVisibility <= 0D Then _TextVisibility = 1D
            End If
            Return _TextVisibility
        End Get
        Set(value As Double)
            _TextVisibility = value
            If _TextVisibility > 0D Then
                TextBox.Visibility = Windows.Visibility.Visible
            Else
                TextBox.Visibility = Windows.Visibility.Hidden
            End If
        End Set
    End Property

    <Menu(), Act()> Public Overridable Property Text As String
        Get
            Return TextBox.Text
        End Get
        Set(value As String)
            TextBox.Text = value
        End Set
    End Property
    Private Sub TextChanged(sender As Object, e As RoutedEventArgs) Handles TextBox.TextChanged
        OnTextChanged()
    End Sub
    Public Overridable Sub OnTextChanged()

    End Sub
    <Menu(), Act()> Public Property Lines As Double
        Get
            Return TextBox.MaxLines
        End Get
        Set(value As Double)
            Try
                Dim z As Integer = Math.Round(Math.Abs(value))
                If z < 1 Then z = 1
                TextBox.MaxLines = z
                If value > 1 Then
                    TextBox.AcceptsReturn = True
                    TextBox.AcceptsTab = True
                Else
                    TextBox.AcceptsReturn = False
                    TextBox.AcceptsTab = False
                End If
            Catch ex As Exception

            End Try
        End Set
    End Property

    <Menu(), Act()> Public Property Fill As System.Windows.Media.Brush
        Get
            Return Shape.Fill
        End Get
        Set(value As System.Windows.Media.Brush)
            Shape.Fill = value
        End Set
    End Property

    <Menu(), Act()> Public Property FontSize As Double
        Get
            Return TextBox.FontSize
        End Get
        Set(value As Double)
            TextBox.FontSize = value
        End Set
    End Property

    <Act()> Public Property FontStyleD As Double
        Get
            Return Style2Double(TextBox.FontStyle)
        End Get
        Set(value As Double)
            TextBox.FontStyle = Double2Style(value)
        End Set
    End Property

    <Act()> Public Property FontWeightD As Double
        Get
            Return Weight2Double(TextBox.FontWeight)
        End Get
        Set(value As Double)
            TextBox.FontWeight = Double2Weight(value)
        End Set
    End Property
    <Menu()> Public Property FontStyle As System.Windows.FontStyle
        Get
            Return TextBox.FontStyle
        End Get
        Set(value As System.Windows.FontStyle)
            TextBox.FontStyle = value
        End Set
    End Property

    <Menu()> Public Property FontWeight As System.Windows.FontWeight
        Get
            Return TextBox.FontWeight
        End Get
        Set(value As System.Windows.FontWeight)
            TextBox.FontWeight = value
        End Set
    End Property

    <Menu(), Act()> Public Property Fore As System.Windows.Media.Brush
        Get
            Return TextBox.Foreground
        End Get
        Set(value As System.Windows.Media.Brush)
            TextBox.Foreground = value
        End Set
    End Property

    Protected _ID As String
    <LateLoad()> Public Property ID As String Implements IActor.ID
        Get
            If _ID Is Nothing Then
                _ID = GetHashCode.ToString
            End If
            Return _ID
        End Get
        Set(value As String)
            If value Is Nothing Then
                _ID = GetHashCode.ToString
            Else
                _ID = value
            End If
            If RelatedActor IsNot Nothing Then RelatedActor.txtName.Text = _ID
        End Set
    End Property

    Public Overridable Property IsReadOnly As Boolean Implements IActor.IsReadOnly
        Get
            Return TextBox.IsReadOnly
        End Get
        Set(value As Boolean)
            TextBox.IsReadOnly = value
            If value Then
                TextBox.Focusable = False
                TextBox.MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
                TextBox.Cursor = Cursors.Arrow
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            Else
                TextBox.Focusable = True
                TextBox.Cursor = Cursors.IBeam
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Visible
                Next
            End If
        End Set
    End Property

    <Menu(), Act()> Public Shadows Property Opacity As Double
        Get
            Return MyBase.Opacity
        End Get
        Set(value As Double)
            MyBase.Opacity = value
        End Set
    End Property
    <Menu(), Act()> Public Shadows Property TextOpacity As Double
        Get
            Return TextBox.Opacity
        End Get
        Set(value As Double)
            TextBox.Opacity = value
        End Set
    End Property
    <Menu(), Act()> Public Shadows Property ShapeOpacity As Double
        Get
            Return Shape.Opacity
        End Get
        Set(value As Double)
            Shape.Opacity = value
        End Set
    End Property

    Public MustOverride Sub ReleasePoint(cp As ControlPoint) Implements IActor.ReleasePoint

    <Menu(), Act()> Public Property Stroke As System.Windows.Media.Brush
        Get
            Return Shape.Stroke
        End Get
        Set(value As System.Windows.Media.Brush)
            Shape.Stroke = value
        End Set
    End Property
    <Menu(), Act()> Public Shadows Property Effect As System.Windows.Media.Effects.Effect
        Get
            Return MyBase.Effect
        End Get
        Set(value As System.Windows.Media.Effects.Effect)
            MyBase.Effect = value
        End Set
    End Property
    <Menu(), Act()> Public Property TextEffect As System.Windows.Media.Effects.Effect
        Get
            Return TextBox.Effect
        End Get
        Set(value As System.Windows.Media.Effects.Effect)
            TextBox.Effect = value
        End Set
    End Property
    <Menu(), Act()> Public Property ShapeEffect As System.Windows.Media.Effects.Effect
        Get
            Return Shape.Effect
        End Get
        Set(value As System.Windows.Media.Effects.Effect)
            Shape.Effect = value
        End Set
    End Property

    <Menu(), Act()> Public Property ZIndex As Double
        Get
            Return Canvas.GetZIndex(Me)
        End Get
        Set(value As Double)
            Canvas.SetZIndex(Me, Math.Round(value))
        End Set
    End Property

    Protected Sub RotateText(angle As Double)
        If (angle > 90D And angle < 270D) Or (angle > -270D And angle < -90D) Then
            Dim az As System.Windows.Vector = ActualSize
            TextBox.RenderTransform = New RotateTransform(180, az.X / 2, az.Y / 2)
        Else
            TextBox.RenderTransform = Transform.Identity
        End If
    End Sub
    Protected Sub TextCancelFocus()
        TextBox.MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
    End Sub
    Protected Sub TextBox_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles TextBox.KeyDown
        If e.Key = Key.Escape Then
            TextCancelFocus()
        End If
    End Sub
    Public MustOverride Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector Implements IActor.AdornerLocation

    Public MustOverride Sub PassMovement() Implements IActor.PassMovement

    <EarlyBind("Stage")> Public Property Host As FreeStage Implements IActor.Host

    Public Sub AddTo(vHost As FreeStage) Implements IActor.AddTo
        Host = vHost
        Dim vT As Type = Me.GetType
        'If Not Host.Menus.ContainsKey(vT) Then
        '    Host.Menus.Add(vT, New AppearanceMenu(vT))
        'End If
        IsReadOnly = Not Host.DesignMode
        'AddHandler Me.RequireMenu, AddressOf Host.OnMenuRequiring
        Host.ElementCollection.Add(Me)
        Host.Actors.Add(Me)
        For Each cp As ControlPoint In Adorners.Values
            cp.AddToStage()
        Next
        'AddHandler Me.OnActivate, AddressOf Host.IActorActivated
        If RelatedActor Is Nothing Then
            RelatedActor = New Actor With {.RelatedActor = Me}
            Host.RelatedDirector.Crew.Items.Add(RelatedActor)
            'AddHandler Me.OnActivate, AddressOf RelatedActor.Activate
            AddHandler RelatedActor.OnActivate, AddressOf Activate
        End If
    End Sub
    <Save()> Public RelatedActor As Actor
    Public Sub Remove() Implements IActor.Remove
        For Each cp As ControlPoint In Adorners.Values
            cp.RemoveFromStage()
        Next
        'RemoveHandler Me.RequireMenu, AddressOf Host.OnMenuRequiring
        Host.ElementCollection.Remove(Me)
        Host.Actors.Remove(Me)
        Host = Nothing
    End Sub
    Protected Overrides Sub OnPreviewMouseDown(e As System.Windows.Input.MouseButtonEventArgs)
        Activate()
        If RelatedActor IsNot Nothing Then RelatedActor.Activate(Me, Nothing)
        If _Host IsNot Nothing Then _Host.IActorActivated(Me, Nothing)
        MyBase.OnPreviewMouseDown(e)
    End Sub
    Public Sub Activate(sender As Object, e As System.Windows.RoutedEventArgs) Implements IActor.Activate
        If sender Is Me Then Exit Sub
        If _Host IsNot Nothing Then _Host.IActorActivated(Me, Nothing)
        Activate()
        If RelatedActor IsNot Nothing AndAlso sender IsNot RelatedActor Then
            RelatedActor.Activate(sender, e)
        End If
        'RaiseEvent OnActivate(sender, Nothing)
    End Sub

    'Public Event OnActivate(sender As Object, e As System.Windows.RoutedEventArgs) Implements IActor.OnActivate

    <Menu(), Act()> Public Overridable Property Visible As Double Implements IActor.Visible
        Get
            Return IIf(MyBase.Visibility = Windows.Visibility.Visible, 1D, 0D)
        End Get
        Set(value As Double)
            If value > 0 Then
                Visibility = Windows.Visibility.Visible
                If TypeOf _Host Is FreeStage AndAlso _Host.DesignMode Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            Else
                Visibility = Windows.Visibility.Hidden
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            End If
        End Set
    End Property
    Public Sub Activate()
        For Each cp As ControlPoint In Adorners.Values
            cp.Dragger.Stroke = Brushes.Red
        Next
    End Sub
    Public Sub Deactivate() Implements IActor.Deactivate
        If RelatedActor IsNot Nothing Then RelatedActor.Deactivate()
        For Each cp As ControlPoint In Adorners.Values
            cp.Dragger.Stroke = Brushes.Black
        Next
    End Sub

    Public Overridable Property Location As System.Windows.Vector Implements IActor.Location
        Get
            Return CanvasLocation
        End Get
        Set(value As System.Windows.Vector)
            Dim delta As System.Windows.Vector = value - CanvasLocation
            OffsetMovement(delta)
            PassMovement()
        End Set
    End Property
    Overridable Sub OffsetMovement(delta As System.Windows.Vector)
        CanvasLocation = CanvasLocation + delta
    End Sub
    Public Overridable Property Size As System.Windows.Vector Implements IActor.Size
        Get
            Return ActualSize
        End Get
        Set(value As System.Windows.Vector)
            ActualSize = value
            PassMovement()
        End Set
    End Property

    Public Overridable Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As Tuple(Of IActor, ControlPoint, ControlPoint) Implements IActor.AddConnector

    End Function
End Class

<Shallow(), Serializable()>
Public Class TextRectangle
    Inherits TextShape
    Protected WithEvents ArrowPolygon As Rectangle
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ArrowPolygon Is Nothing Then ArrowPolygon = New Rectangle
            Return ArrowPolygon
        End Get
    End Property
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim vw = V(w, 0)
        Dim vh = V(0, h)
        Dim vd = V(h, 0)
        'Dim pc As New PointCollection From {vh / 3, vh / 3 + vw - vd / 2, vw - vd / 2, vw + vh / 2, vw - vd / 2 + vh, vh * (2 / 3) + vw - vd / 2, vh * (2 / 3)}
        ArrowPolygon.Width = w
        ArrowPolygon.Height = h
    End Sub
    <Save()> Protected RotationAngleValue As Double = 0
    <Menu(), Act("R")> Public Property RotationAngle As Double
        Get
            Dim v1 As System.Windows.Vector = V(1, 0)
            Dim v2 As System.Windows.Vector = V(1, 0)
            v1.RotateByDegree(RotationAngleValue)
            Dim rt As Double = MyBase.Rotation
            v2.RotateByDegree(rt)
            If (v1 - v2).Length > 0.001D Then
                RotationAngleValue = rt
            End If
            Return RotationAngleValue
        End Get
        Set(value As Double)
            RotationAngleValue = value
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            CanvasLocation = ct - az.RotateByDegree(value)
            Rotation = value
            RotateText(value)
            PassMovement()
        End Set
    End Property
    Protected Overrides ReadOnly Property RotationCenter As System.Windows.Vector
        Get
            Return V(0D, 0D)
        End Get
    End Property
    Public Property RotationPoint() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(ag)
            Dim vx = V(0, -az.Y)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
            'Dim vx As System.Windows.Vector = V(0, az.Y / 2)
            Dim ag As Double = (value.AsVector - ct).AngleByDegree + 90D
            CanvasLocation = ct - az.RotateByDegree(ag)
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    <Act("S")> Public Property RealSize() As System.Windows.Point
        Get
            Return ActualSize
        End Get
        Set(value As System.Windows.Point)
            Dim ct As System.Windows.Vector = OLocation
            ActualSize = value
            CanvasLocation = ct - (value.AsVector / 2).RotateByDegree(Rotation)
            PassMovement()
        End Set
    End Property

    <Act("O")> Public Property OLocation() As System.Windows.Point
        Get
            Return CanvasLocation + (ActualSize / 2).RotateByDegree(Rotation) '- (ActualSize / 2).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            'Dim ct As System.Windows.Vector = value + (ActualSize / 2).RotateByDegree(Rotation)
            CanvasLocation = value - (ActualSize / 2).RotateByDegree(Rotation)
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Property RSize() As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize
            Return CanvasLocation + az.RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            Dim az As System.Windows.Vector = ActualSize / 2
            Dim ct As System.Windows.Vector = OLocation
            az = (value.AsVector - ct).RotateByDegree(-Rotation)
            If az.X < 6D Then az.X = 6D
            If az.Y < 6D Then az.Y = 6D
            ActualSize = az * 2
            OLocation = ct
            Rotation = Rotation
            RotateText(Rotation)
            PassMovement()
        End Set
    End Property
    Public Event DependencyMoved(sender As Object, e As System.EventArgs)
    Public Sub Straight()
        Dim u As Integer = Math.Round(Rotation / 15)
        Rotation = u * 15D
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return CanvasLocation + az.RotateByDegree(Rotation) + V(az.X, -az.Y).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <LateLoad()> Public Property Bindings As ShallowDictionary(Of ControlPointMapping, Integer)
        Get
            Return BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Integer))
            BindingPoints = value
        End Set
    End Property
    Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    Private ReadOnly Property LeftPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return OLocation + V(-az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property RightPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return OLocation + V(az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property TopPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return OLocation + V(0, -az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property BottomPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return OLocation + V(0, az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If _Adorners.ContainsValue(cp) Then Return False
        Dim p As System.Windows.Vector = cp.Position
        If (p - LeftPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 1)
            cp.PositionMove(New List(Of Object) From {Me}, LeftPoint, True)
            Return True
        End If
        If (p - RightPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 2)
            cp.PositionMove(New List(Of Object) From {Me}, RightPoint, True)
            Return True
        End If
        If (p - TopPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 3)
            cp.PositionMove(New List(Of Object) From {Me}, TopPoint, True)
            Return True
        End If
        If (p - BottomPoint).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 4)
            cp.PositionMove(New List(Of Object) From {Me}, BottomPoint, True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim lp As System.Windows.Vector = LeftPoint
        Dim rp As System.Windows.Vector = RightPoint
        Dim tp As System.Windows.Vector = TopPoint
        Dim bp As System.Windows.Vector = BottomPoint
        For Each cp In BindingPoints.Keys
            If TemperatorySender IsNot cp.Target Then
                Select Case BindingPoints(cp)
                    Case 1
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lp
                    Case 2
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = rp
                    Case 3
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = tp
                    Case 4
                        cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = bp
                End Select
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me)},
 {"S", New ControlPoint("S", Me)},
 {"R", New ControlPoint("R", Me, Nothing, AddressOf Straight) With {.Fill = Brushes.Green}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Sub Square()
        Dim az As System.Windows.Vector = ActualSize
        Dim min As Double = Math.Min(az.X, az.Y)
        Dim ct As System.Windows.Vector = OLocation
        ActualSize = az * 2
        OLocation = ct
        Rotation = Rotation
        RotateText(Rotation)
        PassMovement()
    End Sub
    Public Overrides ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
        Get
            Return _Adorners
        End Get
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return OLocation
                Case "S"
                    Return RSize
                Case "R"
                    Return RotationPoint
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    OLocation = value
                Case "S"
                    RSize = value
                Case "R"
                    RotationPoint = value
                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
End Class

<Shallow(), Serializable()>
Public Class StaticText
    Inherits TextRectangle
    Public Sub New()
        MyBase.New()
        Shape.Stroke = Nothing
    End Sub
    Public Overrides Property Visible As Double
        Get
            Return 1D
        End Get
        Set(value As Double)
        End Set
    End Property
End Class

<Shallow(), Serializable()>
Public Class TextArrow
    Inherits TextShape
    Protected WithEvents ArrowPolygon As Polygon
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ArrowPolygon Is Nothing Then ArrowPolygon = New Polygon
            Return ArrowPolygon
        End Get
    End Property
    Private Sub TextArrow_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim w As Double
        Dim h As Double
        w = Width
        If w = 0D Then w = ActualWidth
        h = Height
        If h = 0D Then h = ActualHeight
        Dim vw = V(w, 0)
        Dim vh = V(0, h)
        Dim vd = V(h, 0)
        Dim pc As New PointCollection From {vh / 3, vh / 3 + vw - vd / 2, vw - vd / 2, vw + vh / 2, vw - vd / 2 + vh, vh * (2 / 3) + vw - vd / 2, vh * (2 / 3)}
        ArrowPolygon.Points = pc
    End Sub
    <Menu(), Act("H")> Public Property ArrowHeight As Double
        Get
            Return Me.ActualSize.Y
        End Get
        Set(value As Double)
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.Y = value
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    Public Property LinearHeight() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(0, az.Y / 2)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.Y = CType((value - ct), System.Windows.Vector).Length * 2
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property

    <Act("X")> Public Property LinearX() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(-az.X / 2, 0)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            Dim X As System.Windows.Vector = value
            Dim Y As System.Windows.Vector = LinearY
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.X = (Y - X).Length
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    <Act("Y")> Public Property LinearY() As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim ct As System.Windows.Vector = CanvasLocation + az / 2
            Dim vx = V(az.X / 2, 0)
            Return ct + vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)
            'If sender Is Me Then Exit Property
            Dim X As System.Windows.Vector = LinearX
            Dim Y As System.Windows.Vector = value
            Dim ct = (X + Y) / 2
            Dim ag As Double = (Y - X).AngleByDegree
            Dim az As System.Windows.Vector = ActualSize
            az.X = (Y - X).Length
            ActualSize = az
            CanvasLocation = ct - az / 2
            Rotation = ag
            RotateText(ag)
            PassMovement()
        End Set
    End Property
    Public Event DependencyMoved(sender As Object, e As System.EventArgs)
    Public Sub AutoHeight()
        Dim X As System.Windows.Vector = LinearX
        Dim Y As System.Windows.Vector = LinearY
        Dim ct = (X + Y) / 2
        Dim ag As Double = (Y - X).AngleByDegree
        Dim az As System.Windows.Vector = ActualSize
        az.Y = 72
        ActualSize = az
        CanvasLocation = ct - az / 2
        Rotation = ag
        RotateText(ag)
        PassMovement()
        'RaiseEvent DependencyMoved(Me, New EventArgs)
    End Sub

    Public Property AppearancePoint(Optional sender As IActor = Nothing) As System.Windows.Point
        Get
            Dim ag As Double = Rotation
            Dim az As System.Windows.Vector = ActualSize
            Dim vx = V(0, az.Y / 2)
            Dim ct As System.Windows.Vector = LinearX + LinearY
            Return ct / 2 - vx.RotateByDegree(ag)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <LateLoad()> Public Property Bindings As ShallowDictionary(Of ControlPointMapping, Integer)
        Get
            Return BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Integer))
            BindingPoints = value
        End Set
    End Property
    Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    '<LateLoad()> Private BindingPoints As New ShallowDictionary(Of ControlPointMapping, Integer)
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If _Adorners.ContainsValue(cp) Then Return False
        Dim p As System.Windows.Vector = cp.Position
        If (p - LinearX.AsVector).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), 1)
            cp.PositionMove(New List(Of Object) From {Me}, LinearX, True)
            Return True
        End If
        If (p - LinearY.AsVector).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), -1)
            cp.PositionMove(New List(Of Object) From {Me}, LinearY, True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim lx As System.Windows.Vector = LinearX
        Dim ly As System.Windows.Vector = LinearY
        For Each cp In BindingPoints.Keys
            If TemperatorySender IsNot cp.Target Then
                If BindingPoints(cp) = 1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lx
                ElseIf BindingPoints(cp) = -1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = ly
                End If
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"X", New ControlPoint("X", Me)},
 {"Y", New ControlPoint("Y", Me)},
 {"H", New ControlPoint("H", Me, Nothing, AddressOf AutoHeight) With {.Fill = Brushes.Green}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Overrides ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
        Get
            Return _Adorners
        End Get
    End Property
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "X"
                    Return LinearX
                Case "Y"
                    Return LinearY
                Case "H"
                    Return LinearHeight
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "X"
                    LinearX = value
                Case "Y"
                    LinearY = value
                Case "H"
                    LinearHeight = value
                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
End Class

Public Module PointFacilities
    Public Function ReverseColor(c As SolidColorBrush) As SolidColorBrush
        Return New SolidColorBrush(Color.FromArgb(255, 255 - c.Color.R, 255 - c.Color.G, 255 - c.Color.B))
    End Function
    Public Function P(x As Double, y As Double) As System.Windows.Point
        Return New System.Windows.Point(x, y)
    End Function
    Public Function V(x As Double, y As Double) As System.Windows.Vector
        Return New System.Windows.Vector(x, y)
    End Function
    Public Function V(vp As System.Windows.Point) As System.Windows.Vector
        Return New System.Windows.Vector(vp.X, vp.Y)
    End Function
    Public Function PackVector(V As Object) As Object
        If TypeOf V Is Vector Then
            Dim c As System.Windows.Vector = V
            Return New Point(c.X, c.Y)
        ElseIf TypeOf V Is Color Then
            Dim c As Color = V
            Return New ColorVector(c)
        ElseIf TypeOf V Is GradientStop Then
            Return New GradientVector(V)
        Else
            Return V
        End If
    End Function
    Public Function UnPackVector(V As Object) As Object
        If TypeOf V Is Vector Then
            Dim c As System.Windows.Vector = V
            Return New Point(c.X, c.Y)
        ElseIf TypeOf V Is ColorVector Then
            Dim c As ColorVector = V
            Return CType(c, Color)
        ElseIf TypeOf V Is GradientVector Then
            Return CType(V, GradientStop)
        Else
            Return V
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsVector(p As System.Windows.Point) As System.Windows.Vector
        Return New System.Windows.Vector(p.X, p.Y)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsSize(p As System.Windows.Point) As System.Windows.Size
        Dim w As Double = Math.Abs(p.X)
        Dim h As Double = Math.Abs(p.Y)
        Return New System.Windows.Size(w, h)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsSize(p As System.Windows.Vector) As System.Windows.Size
        Dim w As Double = Math.Abs(p.X)
        Dim h As Double = Math.Abs(p.Y)
        Return New System.Windows.Size(w, h)
    End Function
    Public Function V1(angle As Double) As System.Windows.Vector
        Dim rad As Double = angle / 180 * Math.PI
        Dim c As Double = Math.Cos(rad)
        Dim s As Double = Math.Sin(rad)
        Return New System.Windows.Vector(c, s)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function RotateByDegree(vec As System.Windows.Vector, angle As Double) As System.Windows.Vector
        Dim rad As Double = angle / 180 * Math.PI
        Dim c As Double = Math.Cos(rad)
        Dim s As Double = Math.Sin(rad)
        Return New System.Windows.Vector(vec.X * c - vec.Y * s, vec.X * s + vec.Y * c)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AngleByDegree(vec As System.Windows.Vector) As Double
        Return Math.Atan2(vec.Y, vec.X) / Math.PI * 180
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function LengthOnDirection(vec As System.Windows.Vector, angle As Double) As Double
        Dim base = V(1D, 0D).RotateByDegree(angle)
        Return vec.X * base.X + vec.Y * base.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function LengthOnDirection(vec As System.Windows.Point, angle As Double) As Double
        Dim base = V(1D, 0D).RotateByDegree(angle)
        Return vec.X * base.X + vec.Y * base.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function LengthOnDirection(vec As System.Windows.Vector, direction As System.Windows.Vector) As Double
        Dim base = V(1D, 0D).RotateByDegree(direction.AngleByDegree)
        Return vec.X * base.X + vec.Y * base.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function LengthOnDirection(vec As System.Windows.Point, direction As System.Windows.Vector) As Double
        Dim base = V(1D, 0D).RotateByDegree(direction.AngleByDegree)
        Return vec.X * base.X + vec.Y * base.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function DistanceToLine(vec As System.Windows.Vector, L1 As System.Windows.Vector, L2 As System.Windows.Vector) As Double
        If L1 = L2 Then
            Return 0D
        Else
            Dim base = V(1D, 0D).RotateByDegree((L2 - L1).AngleByDegree)
            Return vec.X * base.X + vec.Y * base.Y - L1.X * base.X + L1.Y * base.Y
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function DistanceToLine(vec As System.Windows.Point, L1 As System.Windows.Vector, L2 As System.Windows.Vector) As Double
        If L1 = L2 Then
            Return 0D
        Else
            Dim base = V(1D, 0D).RotateByDegree((L2 - L1).AngleByDegree)
            Return vec.X * base.X + vec.Y * base.Y - (L1.X * base.X + L1.Y * base.Y)
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function Equals(vec As System.Windows.Vector, pnt As System.Windows.Point) As Boolean
        Return vec.X = pnt.X And vec.Y = pnt.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function Equals(pnt As System.Windows.Point, vec As System.Windows.Vector) As Boolean
        Return vec.X = pnt.X And vec.Y = pnt.Y
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ClockwiseSystem(vec As System.Windows.Vector) As ScaleSystem
        If vec.X <> 0D Or vec.Y <> 0D Then
            Dim ss As New ScaleSystem
            ss.XBase = vec / vec.Length
            ss.YBase = ss.XBase.RotateByDegree(90D)
            Return ss
        Else
            Return New ScaleSystem
        End If
    End Function
End Module

Public Structure ScaleSystem
    Public XBase As System.Windows.Vector
    Public YBase As System.Windows.Vector
    Public O As System.Windows.Vector
    Default Public ReadOnly Property Vector(dX As Double, dY As Double) As System.Windows.Vector
        Get
            Return XBase * dX + YBase * dY + O
        End Get
    End Property
    Public Function Measure(vc As System.Windows.Vector) As System.Windows.Vector
        Dim u As System.Windows.Vector = vc - O
        Return V(u.X * XBase.X + u.Y * XBase.Y, u.X * YBase.X + u.Y * YBase.Y)
    End Function
End Structure