Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
<Shallow()>
Public MustInherit Class LayoutToolBase
    Implements IActor
    Public MustOverride ReadOnly Property LayoutShape As LayoutShapeBase
    Protected TemperatorySender As Object
    'Protected LockedLocation As System.Windows.Vector
    Protected Locked As Boolean = False
    Public Property Location() As System.Windows.Vector Implements IActor.Location
        Get
            Return LayoutShape.O
        End Get
        Set(value As System.Windows.Vector)
            LayoutShape.O = value
            PassMovement()
        End Set
    End Property

    <Act("O")> Public Property O() As System.Windows.Point
        Get
            Return LayoutShape.O
        End Get
        Set(value As System.Windows.Point)
            LayoutShape.O = value
            PassMovement()
        End Set
    End Property
    Private _RunTimeVisible As Boolean

    <Menu(), Act()> Public Property RunTimeVisible As Double
        Get
            Return IIf(_RunTimeVisible, 1D, -1D)
        End Get
        Set(value As Double)
            _RunTimeVisible = value > 0D
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

    Public Overridable Function BindPoint(cp As ControlPoint) As Boolean Implements IActor.BindPoint
        If _Adorners.ContainsValue(cp) Then Return False
        Return False
    End Function
    Public Overridable Sub ReleasePoint(cp As ControlPoint) Implements IActor.ReleasePoint
        Dim keys = BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If BindingPoints.ContainsKey(Key) Then BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overridable Sub ProcessBindingPoints()

    End Sub
    Protected _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 255, 255, 0))}}
}
    Public MustOverride Sub AddAdorner(position As System.Windows.Point, done As Boolean) Implements IActor.AddAdorner

    Public Overridable ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IActor.Adorners
        Get
            Return _Adorners
        End Get
    End Property
    Public MustOverride Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector Implements IActor.AdornerLocation

    Public Overridable Sub PassMovement() Implements IActor.PassMovement
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        If TemperatorySender Is Nothing Then TemperatorySender = Me
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
    Public Sub Activate(sender As Object, e As System.Windows.RoutedEventArgs) Implements IActor.Activate
        If sender Is Me Then Exit Sub
        If _Host IsNot Nothing Then _Host.IActorActivated(Me, Nothing)
        Activate()
        'RaiseEvent OnActivate(sender, Nothing)
    End Sub
    <Save()> Public RelatedActor As Actor
    Public Sub AddTo(vHost As FreeStage) Implements IActor.AddTo
        Host = vHost
        Dim vT As Type = Me.GetType
        If Not Host.Menus.ContainsKey(vT) Then
            Host.Menus.Add(vT, New AppearanceMenu(vT))
        End If
        IsReadOnly = Not Host.DesignMode
        Host.ElementCollection.Add(Me.LayoutShape.Path)
        Host.Actors.Add(Me)
        For Each cp As ControlPoint In Adorners.Values
            cp.AddToStage()
        Next
        'AddHandler Me.OnActivate, AddressOf Host.IActorActivated
        If RelatedActor Is Nothing Then
            RelatedActor = New Actor With {.RelatedActor = Me}
            Host.RelatedDirector.Crew.Items.Add(RelatedActor)
            AddHandler RelatedActor.OnActivate, AddressOf Activate
        End If
    End Sub
    Public Event AdornerChanged(sender As Object, e As System.EventArgs) Implements IActor.AdornerChanged
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
    <EarlyBind("Stage")> Public Property Host As FreeStage Implements IActor.Host

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

    Public Property IsReadOnly As Boolean Implements IActor.IsReadOnly
        Get

        End Get
        Set(value As Boolean)
            If LayoutShape IsNot Nothing AndAlso LayoutShape.Path IsNot Nothing Then
                If _RunTimeVisible Then
                    LayoutShape.Path.Visibility = Visibility.Visible
                Else
                    If value Then
                        LayoutShape.Path.Visibility = Visibility.Hidden
                    Else
                        LayoutShape.Path.Visibility = Visibility.Visible
                    End If
                End If
                If value Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Hidden
                    Next
                Else
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            End If
        End Set
    End Property

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

    Public Sub Remove() Implements IActor.Remove
        For Each cp As ControlPoint In Adorners.Values
            cp.RemoveFromStage()
        Next
        Host.ElementCollection.Remove(Me.LayoutShape.Path)
        Host.Actors.Remove(Me)
        Host = Nothing
    End Sub

    <Menu(), Act()> Public Property Visible As Double Implements IActor.Visible
        Get
            Return IIf(Me.LayoutShape.Path.Visibility = Windows.Visibility.Visible, 1D, 0D)
        End Get
        Set(value As Double)
            If value > 0 Then
                Me.LayoutShape.Path.Visibility = Windows.Visibility.Visible
                If TypeOf _Host Is FreeStage AndAlso _Host.DesignMode Then
                    For Each cp As ControlPoint In Adorners.Values
                        cp.Visibility = Windows.Visibility.Visible
                    Next
                End If
            Else
                Me.LayoutShape.Path.Visibility = Windows.Visibility.Hidden
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            End If
        End Set
    End Property
    Public MustOverride Property Size As System.Windows.Vector Implements IActor.Size

    Public Overridable Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As Tuple(Of IActor, ControlPoint, ControlPoint) Implements IActor.AddConnector

    End Function

End Class

<Shallow()>
Public Class VerticalLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind
    Private _VerticalLine As New VerticalLine
    Public Sub New()
        _Adorners.Add("H", New ControlPoint("H", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
    End Sub
    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)

    End Sub

    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _VerticalLine.O
                Case "H"
                    Return _VerticalLine.H
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    _VerticalLine.O = value
                Case "H"
                    _VerticalLine.H = value
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    <Act("H")> Public Property HeightPoint As System.Windows.Point
        Get
            Return _VerticalLine.H
        End Get
        Set(value As System.Windows.Point)
            _VerticalLine.H = value
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _VerticalLine Is Nothing Then _VerticalLine = New VerticalLine
            Return _VerticalLine
        End Get
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _VerticalLine.H - _VerticalLine.O
        End Get
        Set(value As System.Windows.Vector)
            _VerticalLine.H = value + _VerticalLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return _VerticalLine.H - _VerticalLine.O
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _VerticalLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _VerticalLine.H
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property Rotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Return 90D
        End Get
        Set(value As Double)

        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender

    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get

        End Get
    End Property

    Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get

        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))

        End Set
    End Property
End Class
<Shallow()>
Public Class HorizontalLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind
    Private _HorizontolLine As New HorizontalLine
    Public Sub New()
        _Adorners.Add("W", New ControlPoint("W", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
    End Sub
    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)

    End Sub
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _HorizontolLine.O
                Case "W"
                    Return _HorizontolLine.W
            End Select
        End Get
        Set(value As System.Windows.Vector)
            Select Case aID
                Case "O"
                    _HorizontolLine.O = value
                Case "W"
                    _HorizontolLine.W = value
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _HorizontolLine Is Nothing Then _HorizontolLine = New HorizontalLine
            Return _HorizontolLine
        End Get
    End Property
    <Act("W")> Public Property WidthPoint As System.Windows.Point
        Get
            Return _HorizontolLine.W
        End Get
        Set(value As System.Windows.Point)
            _HorizontolLine.W = value
        End Set
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _HorizontolLine.W - _HorizontolLine.O
        End Get
        Set(value As System.Windows.Vector)
            _HorizontolLine.W = value + _HorizontolLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return _HorizontolLine.W - _HorizontolLine.O
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _HorizontolLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _HorizontolLine.W
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property Rotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Return 0D
        End Get
        Set(value As Double)

        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender

    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get

        End Get
    End Property

    Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get

        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))

        End Set
    End Property
End Class

<Shallow()>
Public Class DegreeLineLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind

    Private _DegreeLine As New DegreeLine
    Public Sub New()
        _Adorners.Add("E", New ControlPoint("E", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
        _Adorners.Add("M", New ControlPoint("M", Me, AddressOf OnMenu) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 0, 128, 0)), .CanBind = False})
    End Sub
    Public Property AppearancePoint() As System.Windows.Point
        Get
            Return O + V(0, 20)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        Return MAbsoluteLineBind.BindPoint(Me, cp)
    End Function
    Public Overrides Sub ProcessBindingPoints()
        MAbsoluteLineBind.ProcessBindingPoints(Me)
    End Sub
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        MAbsoluteLineBind.ReleasePoint(Me, cp)
    End Sub
    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)
    End Sub
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _DegreeLine.O
                Case "E"
                    Return _DegreeLine.E
                Case "M"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            Select Case aID
                Case "O"
                    _DegreeLine.O = value
                Case "E"
                    _DegreeLine.E = value
                Case "M"
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _DegreeLine Is Nothing Then _DegreeLine = New DegreeLine
            Return _DegreeLine
        End Get
    End Property
    <Act("E")> Public Property WidthPoint As System.Windows.Point
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)
            _DegreeLine.E = value
        End Set
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _DegreeLine.E - _DegreeLine.O
        End Get
        Set(value As System.Windows.Vector)
            _DegreeLine.E = value + _DegreeLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return V(_DegreeLine.Length, 0)
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _DegreeLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    <Menu(), Act()> Public Property Rotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Return _DegreeLine.Degree
        End Get
        Set(value As Double)
            _DegreeLine.Degree = value
            PassMovement()
        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender
        Return TemperatorySender
    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get
            Return _Adorners
        End Get
    End Property

    <LateLoad()> Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
End Class

<Shallow()>
Public Class RelativeDegreeLineLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind

    Private _DegreeLine As New DegreeLine
    Public Sub New()
        _Adorners.Add("E", New ControlPoint("E", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
        _Adorners.Add("M", New ControlPoint("M", Me, AddressOf OnMenu) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 0, 128, 0)), .CanBind = False})
    End Sub
    Public Property AppearancePoint() As System.Windows.Point
        Get
            Return O + V(0, 20)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <Menu(), Act()> Public Property LineLength As Double
        Get
            Return _DegreeLine.Length
        End Get
        Set(value As Double)
            _DegreeLine.Length = value
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        Return MAbsoluteLineBind.BindPoint(Me, cp)
    End Function

    Public Overrides Sub ProcessBindingPoints()
        MAbsoluteLineBind.ProcessBindingPoints(Me)
    End Sub

    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        MAbsoluteLineBind.ReleasePoint(Me, cp)
    End Sub

    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)
    End Sub

    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _DegreeLine.O
                Case "E"
                    Return _DegreeLine.E
                Case "M"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            Select Case aID
                Case "O"
                    _DegreeLine.O = value
                    If sender IsNot Me And TypeOf sender Is IAbsoluteLineBind Then
                        UpdateRotation()
                    End If
                Case "E"
                    _DegreeLine.E = value
                Case "M"
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _DegreeLine Is Nothing Then _DegreeLine = New DegreeLine
            Return _DegreeLine
        End Get
    End Property
    <Act("E")> Public Property WidthPoint As System.Windows.Point
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)
            _DegreeLine.E = value
        End Set
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _DegreeLine.E - _DegreeLine.O
        End Get
        Set(value As System.Windows.Vector)
            _DegreeLine.E = value + _DegreeLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return V(_DegreeLine.Length, 0)
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _DegreeLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    Private _RelativeRotation As Double
    <Menu(), Act()> Public Property Rotation As Double
        Get
            Return _RelativeRotation
        End Get
        Set(value As Double)
            _RelativeRotation = value
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Then
                _DegreeLine.Degree = value
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                _DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
            Else
                _DegreeLine.Degree = value
            End If
            PassMovement()
        End Set
    End Property
    Public Sub UpdateRotation()
        Dim bt As IActor = _Adorners("O").DependentTarget
        If bt Is Nothing Or bt Is Me Then
            _DegreeLine.Degree = _RelativeRotation
        ElseIf TypeOf bt Is IAbsoluteLineBind Then
            _DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
        Else
            _DegreeLine.Degree = _RelativeRotation
        End If
    End Sub
    <Menu()> Public Property TotalRotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Or bt Is Me Then
                Return _RelativeRotation
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                Return CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
            Else
                Return _RelativeRotation
            End If
        End Get
        Set(value As Double)

        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender
        Return TemperatorySender
    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get
            Return _Adorners
        End Get
    End Property

    Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    <LateLoad()> Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
End Class


<Shallow()>
Public Class RelativeFreeDegreeLineLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind

    Private _DegreeLine As New DegreeLine
    Public Sub New()
        _Adorners.Add("E", New ControlPoint("E", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
        _Adorners.Add("M", New ControlPoint("M", Me, AddressOf OnMenu) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 0, 128, 0)), .CanBind = False})
    End Sub
    Public Property AppearancePoint() As System.Windows.Point
        Get
            Return O + V(0, 20)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <Menu(), Act()> Public Property LineLength As Double
        Get
            Return _DegreeLine.Length
        End Get
        Set(value As Double)
            _DegreeLine.Length = value
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        Return MAbsoluteLineBind.BindPoint(Me, cp)
    End Function

    Public Overrides Sub ProcessBindingPoints()
        MAbsoluteLineBind.ProcessBindingPoints(Me)
        If TemperatorySender IsNot Me And TypeOf TemperatorySender Is IAbsoluteLineBind Then
            UpdateRotation()
        End If
    End Sub

    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        MAbsoluteLineBind.ReleasePoint(Me, cp)
    End Sub

    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)
    End Sub

    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _DegreeLine.O
                Case "E"
                    Return _DegreeLine.E
                Case "M"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            Select Case aID
                Case "O"
                    _DegreeLine.O = value
                    If sender IsNot Me And TypeOf sender Is IAbsoluteLineBind Then
                        UpdateRotation()
                    End If
                Case "E"
                    Rotation = (value - _DegreeLine.O.AsVector).AngleByDegree
                    _DegreeLine.E = value

                Case "M"
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _DegreeLine Is Nothing Then _DegreeLine = New DegreeLine
            Return _DegreeLine
        End Get
    End Property
    <Act("E")> Public Property WidthPoint As System.Windows.Point
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)
            Rotation = (value - _DegreeLine.O).AngleByDegree
            _DegreeLine.E = value
            PassMovement()
        End Set
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _DegreeLine.E - _DegreeLine.O
        End Get
        Set(value As System.Windows.Vector)
            _DegreeLine.E = value + _DegreeLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return V(_DegreeLine.Length, 0)
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _DegreeLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    Private _RelativeRotation As Double
    <Menu(), Act()> Public Property Rotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Or bt Is Me Then
                Return _RelativeRotation
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                Return CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
            Else
                Return _RelativeRotation
            End If
            Return _RelativeRotation
        End Get
        Set(value As Double)
            _RelativeRotation = value
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Then
                _DegreeLine.Degree = value
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                _DegreeLine.Degree = value
                _RelativeRotation = value - CType(bt, IAbsoluteLineBind).Rotation
                '_DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation '=value 
            Else
                _DegreeLine.Degree = value
            End If
            PassMovement()
        End Set
    End Property
    Public Sub UpdateRotation()
        Dim bt As IActor = _Adorners("O").DependentTarget
        If bt Is Nothing Or bt Is Me Then
            _DegreeLine.Degree = _RelativeRotation
        ElseIf TypeOf bt Is IAbsoluteLineBind Then
            _DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
        Else
            _DegreeLine.Degree = _RelativeRotation
        End If
    End Sub
    <Menu(), Act()> Public Property RelativeRotation As Double
        Get
            Return _RelativeRotation
        End Get
        Set(value As Double)
            _RelativeRotation = value
        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender
        Return TemperatorySender
    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get
            Return _Adorners
        End Get
    End Property

    Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    <LateLoad()> Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
End Class

<Shallow()>
Public Class RelativeLineLayout
    Inherits LayoutToolBase
    Implements IAbsoluteLineBind

    Private _DegreeLine As New DegreeLine
    Public Sub New()
        _Adorners.Add("E", New ControlPoint("E", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 128, 0, 0)), .CanBind = False})
        _Adorners.Add("M", New ControlPoint("M", Me, AddressOf OnMenu) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 0, 128, 0)), .CanBind = False})
    End Sub
    Public Property AppearancePoint() As System.Windows.Point
        Get
            Return O + V(0, 20)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    <Menu(), Act()> Public Property LineLength As Double
        Get
            Return _DegreeLine.Length
        End Get
        Set(value As Double)
            _DegreeLine.Length = value
            PassMovement()
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        Return MAbsoluteLineBind.RelativeBindPoint(Me, cp)
    End Function

    Public Overrides Sub ProcessBindingPoints()
        MAbsoluteLineBind.ProcessRelativeBindingPoints(Me)
        If TemperatorySender IsNot Me And TypeOf TemperatorySender Is IAbsoluteLineBind Then
            UpdateRotation()
        End If
    End Sub

    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        MAbsoluteLineBind.ReleasePoint(Me, cp)
    End Sub

    Public Overrides Sub AddAdorner(position As System.Windows.Point, done As Boolean)
    End Sub

    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return _DegreeLine.O
                Case "E"
                    Return _DegreeLine.E
                Case "M"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            Select Case aID
                Case "O"
                    _DegreeLine.O = value
                    If sender IsNot Me And TypeOf sender Is IAbsoluteLineBind Then
                        UpdateRotation()
                    End If
                Case "E"
                    Rotation = (value - _DegreeLine.O.AsVector).AngleByDegree
                    _DegreeLine.E = value

                Case "M"
            End Select
            PassMovement()
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides ReadOnly Property LayoutShape As LayoutShapeBase
        Get
            If _DegreeLine Is Nothing Then _DegreeLine = New DegreeLine
            Return _DegreeLine
        End Get
    End Property
    <Act("E")> Public Property WidthPoint As System.Windows.Point
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)
            Rotation = (value - _DegreeLine.O).AngleByDegree
            _DegreeLine.E = value
            PassMovement()
        End Set
    End Property

    Public Overrides Property Size As System.Windows.Vector
        Get
            Return _DegreeLine.E - _DegreeLine.O
        End Get
        Set(value As System.Windows.Vector)
            _DegreeLine.E = value + _DegreeLine.O
            PassMovement()
        End Set
    End Property

    Public Property ActualSize As System.Windows.Vector Implements IAbsoluteLineBind.ActualSize
        Get
            Return V(_DegreeLine.Length, 0)
        End Get
        Set(value As System.Windows.Vector)

        End Set
    End Property

    Public Property LinearX As System.Windows.Point Implements IAbsoluteLineBind.LinearX
        Get
            Return _DegreeLine.O
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property

    Public Property LinearY As System.Windows.Point Implements IAbsoluteLineBind.LinearY
        Get
            Return _DegreeLine.E
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    Private _RelativeRotation As Double
    <Menu(), Act()> Public Property Rotation As Double Implements IAbsoluteLineBind.Rotation
        Get
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Or bt Is Me Then
                Return _RelativeRotation
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                Return CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
            Else
                Return _RelativeRotation
            End If
            Return _RelativeRotation
        End Get
        Set(value As Double)
            _RelativeRotation = value
            Dim bt As IActor = _Adorners("O").DependentTarget
            If bt Is Nothing Then
                _DegreeLine.Degree = value
            ElseIf TypeOf bt Is IAbsoluteLineBind Then
                _DegreeLine.Degree = value
                _RelativeRotation = value - CType(bt, IAbsoluteLineBind).Rotation
                '_DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation '=value 
            Else
                _DegreeLine.Degree = value
            End If
            PassMovement()
        End Set
    End Property
    Public Sub UpdateRotation()
        Dim bt As IActor = _Adorners("O").DependentTarget
        If bt Is Nothing Or bt Is Me Then
            _DegreeLine.Degree = _RelativeRotation
        ElseIf TypeOf bt Is IAbsoluteLineBind Then
            _DegreeLine.Degree = CType(bt, IAbsoluteLineBind).Rotation + _RelativeRotation
        Else
            _DegreeLine.Degree = _RelativeRotation
        End If
    End Sub
    <Menu(), Act()> Public Property RelativeRotation As Double
        Get
            Return _RelativeRotation
        End Get
        Set(value As Double)
            _RelativeRotation = value
        End Set
    End Property

    Public Function TempSender() As Object Implements IAbsoluteLineBind.TempSender
        Return TemperatorySender
    End Function

    Public ReadOnly Property Adorners1 As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IPointBind.Adorners
        Get
            Return _Adorners
        End Get
    End Property

    Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    <LateLoad()> Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double) Implements IPointBind.BindingPoints
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
End Class

<Shallow()>
Public MustInherit Class LayoutShapeBase
    Inherits PathShapeBase
    Public Sub New()
        Path.StrokeThickness = 1D
        Path.Stroke = Brushes.Blue
    End Sub
    <Save()> Property _O As System.Windows.Point
    Public Property O As System.Windows.Point
        Get
            Return _O
        End Get
        Set(value As System.Windows.Point)
            _O = value
            Update()
        End Set
    End Property

    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)

    End Sub
End Class

<Shallow()>
Public Class HorizontalLine
    Inherits LayoutShapeBase
    <Save()> Private _Width As Double
    Public Property W As System.Windows.Point
        Get
            Return _O.AsVector + V(_Width, 0D)
        End Get
        Set(value As System.Windows.Point)
            _Width = value.X - _O.X
            Update()
        End Set
    End Property
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = False
        fig.StartPoint = _O
        fig.Segments.Add(New LineSegment(W, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class

<Shallow()>
Public Class VerticalLine
    Inherits LayoutShapeBase

    <Save()> Private _Height As Double
    Public Property H As System.Windows.Point
        Get
            Return _O.AsVector + V(0D, _Height)
        End Get
        Set(value As System.Windows.Point)
            _Height = value.Y - _O.Y
            Update()
        End Set
    End Property
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = False
        fig.StartPoint = _O
        fig.Segments.Add(New LineSegment(H, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class

<Shallow()>
Public Class DegreeLine
    Inherits LayoutShapeBase
    <Save()> Private _Length As Double
    <Save()> Private _Degree As Double
    Public Property Degree As Double
        Get
            Return _Degree
        End Get
        Set(value As Double)
            _Degree = value
            Update()
        End Set
    End Property
    Public Property Length As Double
        Get
            Return _Length
        End Get
        Set(value As Double)
            _Length = value
            Update()
        End Set
    End Property
    Public Property E As System.Windows.Point
        Get
            Return _O.AsVector + V(_Length, 0).RotateByDegree(_Degree)
        End Get
        Set(value As System.Windows.Point)
            Dim delta = value - O
            Dim _b = V(1D, 0).RotateByDegree(_Degree)
            _Length = delta.Length * Math.Sign(_b.X * delta.X + _b.Y * delta.Y)
            Update()
        End Set
    End Property
    Protected Overrides Sub DefineSegment(context As System.Windows.Media.PathGeometry)
        Dim x = MyBase.Path.Dispatcher.DisableProcessing
        context.Figures.Clear()
        Dim fig As New PathFigure
        fig.IsClosed = False
        fig.StartPoint = _O
        fig.Segments.Add(New LineSegment(E, True))
        context.Figures.Add(fig)
        x.Dispose()
    End Sub
End Class
