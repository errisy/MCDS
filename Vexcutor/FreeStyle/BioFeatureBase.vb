Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
Public MustInherit Class BioFeatureBase
    Inherits GridBase
    Implements IActor
    Public Event AdornerChanged(sender As Object, e As System.EventArgs) Implements IActor.AdornerChanged
    Public MustOverride ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint) Implements IActor.Adorners
    Public MustOverride ReadOnly Property Shape As System.Windows.Shapes.Shape
    Public WithEvents InnerGrid As New GridBase
    'Public WithEvents ComboList As New System.Windows.Controls.ComboBox With {.BorderThickness = New Thickness(0D), .Background = Brushes.Transparent}
    Protected TemperatorySender As List(Of Object)
    Public Sub New()
        Shape.Fill = Brushes.Transparent
        Shape.Stroke = Brushes.Black
        Children.Add(Shape)
        Children.Add(InnerGrid)
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
    <Menu(), Act()> Public Property GridVisible As Double
        Get
            If InnerGrid.Visibility = Windows.Visibility.Hidden Then
                If _TextVisibility > 0D Then _TextVisibility = 0D
            ElseIf InnerGrid.Visibility = Windows.Visibility.Visible Then
                If _TextVisibility <= 0D Then _TextVisibility = 1D
            End If
            Return _TextVisibility
        End Get
        Set(value As Double)
            _TextVisibility = value
            If _TextVisibility > 0D Then
                InnerGrid.Visibility = Windows.Visibility.Visible
            Else
                InnerGrid.Visibility = Windows.Visibility.Hidden
            End If
        End Set
    End Property


    Public Overridable Sub OnTextChanged()

    End Sub

    <Menu(), Act()> Public Overridable Property Fill As System.Windows.Media.Brush
        Get
            Return Shape.Fill
        End Get
        Set(value As System.Windows.Media.Brush)
            Shape.Fill = value
        End Set
    End Property

    <Menu(), Act()> Public Overridable Property FontSize As Double
        Get
            Return 0D
        End Get
        Set(value As Double)

        End Set
    End Property

    <Act()> Public Overridable Property FontStyleD As Double
        Get
            Return 0D
        End Get
        Set(value As Double)

        End Set
    End Property

    <Act()> Public Overridable Property FontWeightD As Double
        Get
            Return 0D
        End Get
        Set(value As Double)

        End Set
    End Property
    <Menu()> Public Overridable Property FontStyle As System.Windows.FontStyle
        Get

        End Get
        Set(value As System.Windows.FontStyle)
        End Set
    End Property

    <Menu()> Public Overridable Property FontWeight As System.Windows.FontWeight
        Get
            Return Nothing
        End Get
        Set(value As System.Windows.FontWeight)

        End Set
    End Property
    <Menu(), Act()> Public Overridable Property Fore As System.Windows.Media.Brush
        Get
            Return Nothing
        End Get
        Set(value As System.Windows.Media.Brush)

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
    Protected _IsReadOnly As Boolean = True
    Public Overridable Property IsReadOnly As Boolean Implements IActor.IsReadOnly
        Get
            Return _IsReadOnly
        End Get
        Set(value As Boolean)
            _IsReadOnly = value
            If _IsReadOnly Then
                For Each cp As ControlPoint In Adorners.Values
                    cp.Visibility = Windows.Visibility.Hidden
                Next
            Else
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
            Return InnerGrid.Opacity
        End Get
        Set(value As Double)
            InnerGrid.Opacity = value
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
            Return InnerGrid.Effect
        End Get
        Set(value As System.Windows.Media.Effects.Effect)
            InnerGrid.Effect = value
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
            InnerGrid.RenderTransform = New RotateTransform(180, az.X / 2, az.Y / 2)
        Else
            InnerGrid.RenderTransform = Transform.Identity
        End If
    End Sub
    Public MustOverride Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector Implements IActor.AdornerLocation

    Public MustOverride Sub PassMovement() Implements IActor.PassMovement

    '<EarlyBind("Stage")>
    Public Property Host As FreeStage Implements IActor.Host

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
        Else
            Host.RelatedDirector.Crew.Items.Add(RelatedActor)
            AddHandler RelatedActor.OnActivate, AddressOf Activate
        End If
        OnAdded()
    End Sub
    ''' <summary>
    ''' 当已经加载到舞台之后执行的操作
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub OnAdded()

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
        OnActivate()
    End Sub
    Public Overridable Sub OnActivate()

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

Public MustInherit Class LineBaseBioFeature
    Inherits BioFeatureBase
    Protected WithEvents ShapeBase As Shape
    Protected Shared ResultBrush As New SolidColorBrush(Color.FromArgb(198, 32, 160, 64))
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
    Public Property PositionPoint() As System.Windows.Point
        Get
            Dim ss = (LinearY - LinearX).ClockwiseSystem
            ss.O = (LinearX + LinearY).AsVector / 2
            Dim az As System.Windows.Vector = ActualSize
            Return ss(0D, -az.Y / 2)
        End Get
        Set(value As System.Windows.Point)
            Dim ss = (LinearY - LinearX).ClockwiseSystem
            ss.O = (LinearX + LinearY).AsVector / 2
            Dim az As System.Windows.Vector = ActualSize
            Dim delta = value - ss(0D, -az.Y / 2)
            Location += delta
            PassMovement()
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
            cp.BindingTarget.AdornerLocation(New List(Of Object) From {Me}, cp.ID) = LinearX
            Return True
        End If
        If (p - LinearY.AsVector).Length < 8 Then
            BindingPoints.Add(cp.Mapping(Me), -1)
            cp.BindingTarget.AdornerLocation(New List(Of Object) From {Me}, cp.ID) = LinearY
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
            If Not TemperatorySender.Contains(cp.Target) And Not TemperatorySender.Contains(cp) Then
                If BindingPoints(cp) = 1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = lx
                ElseIf BindingPoints(cp) = -1 Then
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = ly
                End If
            End If
        Next
    End Sub
    Private _Adorners As New Dictionary(Of String, ControlPoint) From {
{"X", New ControlPoint("X", Me) With {.MutualBind = True}},
 {"Y", New ControlPoint("Y", Me) With {.Fill = Brushes.Orange, .MutualBind = True}},
 {"H", New ControlPoint("H", Me, Nothing, AddressOf AutoHeight) With {.Fill = Brushes.Green}},
 {"C", New ControlPoint("C", Me) With {.Fill = Brushes.Pink}}
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
                    Return PositionPoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            If sender.OK AndAlso sender.Contains(Me) Then
                Exit Property
            ElseIf sender.OK Then
                TemperatorySender = sender
            Else
                TemperatorySender = New List(Of Object)
            End If
            TemperatorySender.Add(Me)
            Select Case aID
                Case "X"
                    Dim pnt As System.Windows.Vector = LinearX
                    If pnt <> value Then
                        LinearX = value
                    End If
                Case "Y"
                    Dim pnt As System.Windows.Vector = LinearY
                    If pnt <> value Then
                        LinearY = value
                    End If
                Case "H"
                    LinearHeight = value
                Case "C"
                    PositionPoint = value
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Public Overrides Sub PassMovement()
        If TemperatorySender.NO Then TemperatorySender = New List(Of Object)
        If Not TemperatorySender.Contains(Me) Then TemperatorySender.Add(Me)
        For Each cp As ControlPoint In _Adorners.Values
            If TemperatorySender IsNot cp Then cp.Move()
        Next
        ProcessBindingPoints()
        TemperatorySender = Nothing
    End Sub
    Public Overridable Function GetSequence() As String
        Return ""
    End Function
    Public Function GetTheOtherSide(SideKey As String) As LineBaseBioFeature
        If SideKey = "X" Then
            If TypeOf _Adorners("Y").DependentTarget Is LineBaseBioFeature Then Return _Adorners("Y").DependentTarget
        ElseIf SideKey = "Y" Then
            If TypeOf _Adorners("X").DependentTarget Is LineBaseBioFeature Then Return _Adorners("X").DependentTarget
        End If
        Return Nothing
    End Function
    Public Function GetTheOtherSide(FromSide As LineBaseBioFeature) As LineBaseBioFeature
        If FromSide Is Nothing OrElse FromSide Is Me Then
            If _Adorners("Y").DependentTarget.OK Then Return _Adorners("Y").DependentTarget
            If _Adorners("X").DependentTarget.OK Then Return _Adorners("X").DependentTarget
        End If
        If _Adorners("X").DependentTarget Is FromSide Then Return _Adorners("Y").DependentTarget
        If _Adorners("Y").DependentTarget Is FromSide Then Return _Adorners("X").DependentTarget
        Return Nothing
    End Function
    Public Sub PassChain(SideKey As LineBaseBioFeature, Chain As List(Of LineBaseBioFeature))
        Dim tos As LineBaseBioFeature = GetTheOtherSide(SideKey)
        If tos.OK Then
            Chain.Add(tos)
            tos.PassChain(Me, Chain)
        End If
    End Sub
    Public Sub BuildGeneFile(gf As Nuctions.GeneFileBuilder, FromSide As LineBaseBioFeature)
        'gf.Sequences.Append(GetSequence)
        Dim nextNode As LineBaseBioFeature = GetTheOtherSide(FromSide)
        ExportToGeneFile(gf)
        If nextNode.OK Then nextNode.BuildGeneFile(gf, Me)
        OnGeneFileBuilt(gf)
    End Sub
    Public Overridable Sub OnGeneFileBuilt(vBuiltGeneFile As Nuctions.GeneFileBuilder)

    End Sub
    Public Overridable Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)

    End Sub
    Public Sub ChainSignal(FromSide As LineBaseBioFeature, SourceNode As LineBaseBioFeature)
        If _Adorners("X").DependentTarget IsNot FromSide AndAlso TypeOf _Adorners("X").DependentTarget Is LineBaseBioFeature Then DirectCast(_Adorners("X").DependentTarget, LineBaseBioFeature).ChainSignal(Me, SourceNode)
        If _Adorners("Y").DependentTarget IsNot FromSide AndAlso TypeOf _Adorners("Y").DependentTarget Is LineBaseBioFeature Then DirectCast(_Adorners("Y").DependentTarget, LineBaseBioFeature).ChainSignal(Me, SourceNode)
        OnChainSignal(SourceNode)
    End Sub
    ''' <summary>
    ''' 如果需要即时更新的话 需要重写这个函数
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub OnChainSignal(SourceNode As LineBaseBioFeature)

    End Sub
End Class

<Shallow()>
Public Class RestrictionSite
    Inherits LineBaseBioFeature
    Protected NameGrid As New GridBase
    Protected EnzymeNameLabel As New Label With {.Content = "Enzyme"}
    Protected WithEvents EnzymeNameText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected WithEvents IsReverseComplemented As New System.Windows.Controls.Label With {.Content = "->"}
    Protected WithEvents RestrictionNameCombo As New System.Windows.Controls.ComboBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D)}
    Protected EnzymeSequenceLabel As New Label With {.Content = ""}
    Protected DesignGrid As New GridBase
    Protected WithEvents DesignEnzymeSequence As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(EnzymeNameLabel)
        NameGrid.AddColumnItem(EnzymeNameText)
        NameGrid.AddColumnItem(RestrictionNameCombo)
        InnerGrid.AddRowItem(DesignGrid)
        DesignGrid.AddColumnItem(IsReverseComplemented)
        DesignGrid.AddColumnItem(EnzymeSequenceLabel)
        DesignGrid.AddColumnItem(DesignEnzymeSequence)
        InnerGrid.AddRowItem(Result, "*")
        Shape.Fill = New SolidColorBrush(Color.FromArgb(108, 255, 192, 255))
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Public Overrides Sub OnAdded()
        LoadEnzyme()
    End Sub
    Public Sub LoadEnzyme()
        Dim j As Integer = 0
        Dim ezlist As New List(Of Nuctions.RestrictionEnzyme)
        ezlist.AddRange(SettingEntry.EnzymeCol.Values)
        ezlist.Sort()
        For Each ez In ezlist
            If ez.Name.ToLower = EnzymeNameText.Text.ToLower Then RestrictionNameCombo.SelectedIndex = j
            RestrictionNameCombo.Items.Add(ez)
            j += 1
        Next
    End Sub
    Private cbSetting As Boolean = False
    Private Sub RestrictionNameCombo_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles RestrictionNameCombo.SelectionChanged
        If cbSetting Then Exit Sub
        If TypeOf RestrictionNameCombo.SelectedItem Is Nuctions.RestrictionEnzyme Then
            Dim ez As Nuctions.RestrictionEnzyme = RestrictionNameCombo.SelectedItem
            EnzymeNameText.Text = ez.Name
        End If
    End Sub
    Private Sub EnzymeNameText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles EnzymeNameText.TextChanged
        UpdateSequence()
    End Sub
    Public Sub UpdateSequence()
        Dim vName As String = EnzymeNameText.Text
        cbSetting = True
        dcSetting = True
        For Each ez As Nuctions.RestrictionEnzyme In RestrictionNameCombo.Items
            If ez.Name = vName Then
                EnzymeSequenceLabel.Content = ez.Sequence
                RestrictionNameCombo.SelectedIndex = RestrictionNameCombo.Items.IndexOf(ez)
                Dim seq As String = ez.WrapDesignSequence(DesignEnzymeSequence.Text)
                If NotReverseComplemented Then
                    seq = Nuctions.TAGCFilter(seq)
                Else
                    seq = Nuctions.ReverseComplement(seq)
                End If
                Result.Text = seq
            End If
        Next
        cbSetting = False
        dcSetting = False
        ChainSignal(Me, Me)
    End Sub
    <Save()> Public Property EnzymeSequence As String
        Get
            Return EnzymeSequenceLabel.Content
        End Get
        Set(value As String)
            EnzymeSequenceLabel.Content = value
        End Set
    End Property
    <Save()> Public Property NotReverseComplemented As Boolean
        Get
            Return IsReverseComplemented.Content = "->"
        End Get
        Set(value As Boolean)
            If value Then
                IsReverseComplemented.Content = "<-"
            Else
                IsReverseComplemented.Content = "->"
            End If
            'UpdateSequence()
        End Set
    End Property
    Private Sub IsReverseComplemented_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles IsReverseComplemented.MouseLeftButtonDown
        If IsReverseComplemented.Content = "->" Then
            IsReverseComplemented.Content = "<-"
        Else
            IsReverseComplemented.Content = "->"
        End If
        UpdateSequence()
    End Sub
    Private dcSetting As Boolean = False
    Private Sub DesignEnzymeSequence_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles DesignEnzymeSequence.TextChanged
        If dcSetting Then Exit Sub
        UpdateSequence()
    End Sub
    Public Overrides Function GetSequence() As String
        Return Result.Text
    End Function
    <Save()> Public Property Direction As Boolean
        Get
            Return IsReverseComplemented.Content = "->"
        End Get
        Set(value As Boolean)
            If value Then
                IsReverseComplemented.Content = "->"
            Else
                IsReverseComplemented.Content = "<-"
            End If
        End Set
    End Property
    <Save()> Public Property RestrictionEnzyme As String
        Get
            Return EnzymeNameText.Text
        End Get
        Set(value As String)
            EnzymeNameText.Text = value
        End Set
    End Property
    <Save()> Public Property InnerSequence As String
        Get
            Return DesignEnzymeSequence.Text
        End Get
        Set(value As String)
            DesignEnzymeSequence.Text = value
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        gf.Sequences.Append(Result.Text)
    End Sub
End Class

<Shallow()>
Public Class PrimerOrigin
    Inherits LineBaseBioFeature
    Protected HeaderGrid As New GridBase
    Protected HeaderLabel As New Label With {.Content = "Primer"}
    Protected SequenceNameText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected NameGrid As New GridBase
    Protected InfoGrid As New GridBase
    Protected ProtectionLabel As New Label With {.Content = "Protection Nucleotides"}
    Protected WithEvents GotoEditor As New Label With {.Content = "View Sequence", .FontWeight = FontWeights.Bold, .Foreground = Brushes.Violet}
    Protected WithEvents ApplyLabel As New Label With {.Content = "Apply", .FontWeight = FontWeights.Bold, .Foreground = Brushes.Violet}
    Protected SeqLengthLabel As New Label With {.Content = "0 bp"}
    Protected WithEvents ProtectionLength As New NumberBox With {.Value = 1, .AllowDecimal = False, .AllowNegative = False, .Background = Brushes.Transparent, .BorderBrush = Brushes.Black}
    Protected WithEvents ProtectionSequenceText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(HeaderGrid)
        HeaderGrid.AddColumnItem(HeaderLabel)
        HeaderGrid.AddColumnItem(SequenceNameText)
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(ProtectionLabel)
        NameGrid.AddColumnItem(ProtectionLength)
        NameGrid.AddColumnItem(ProtectionSequenceText)
        InnerGrid.AddRowItem(InfoGrid)
        InfoGrid.AddColumnItem(GotoEditor)
        InfoGrid.AddColumnItem(ApplyLabel)
        InfoGrid.AddColumnItem(SeqLengthLabel)
        InnerGrid.AddRowItem(Result, "*")
        Shape.Fill = New SolidColorBrush(Color.FromArgb(48, 255, 255, 169))
    End Sub
    Private Sub ProtectionSequenceText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles ProtectionSequenceText.TextChanged
        UpdateSequence()
    End Sub
    Public Sub UpdateSequence()
        Result.Text = Nuctions.TAGCFilter(ProtectionSequenceText.Text)
        ChainSignal(Me, Me)
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Public Overrides Sub OnChainSignal(SourceNode As LineBaseBioFeature)
        'read primer and calculate Results
        If Host.OK Then Host.UpdatePrimers()
    End Sub
    Public Property PrimerName As String
        Get
            Return SequenceNameText.Text
        End Get
        Set(value As String)
            SequenceNameText.Text = value
        End Set
    End Property
    <Save()> Private Property SaveSequenceName As String
        Get
            Return SequenceNameText.Text
        End Get
        Set(value As String)
            SequenceNameText.Text = value
        End Set
    End Property
    Private _SequenceLength As Integer = 0
    <Save()> Private Property SequenceLength As Integer
        Get
            Return _SequenceLength
        End Get
        Set(value As Integer)
            _SequenceLength = value
            SeqLengthLabel.Content = _SequenceLength.ToString + " bp"
        End Set
    End Property
    <Save()> Private Property DesignedSequence As String
        Get
            Return ProtectionSequenceText.Text
        End Get
        Set(value As String)
            ProtectionSequenceText.Text = value
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Private plSetting As Boolean = False
    Private extending As Boolean = False
    Private Shared WorkingBrush As New SolidColorBrush(Color.FromArgb(128, 64, 0, 0))
    Private Async Sub ProtectionLength_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles ProtectionLength.TextChanged
        If plSetting Then Exit Sub
        Dim i As Integer = 0
        If Integer.TryParse(ProtectionLength.Text, i) Then
            If i > 24 Then
                i = 24
                plSetting = True
                ProtectionLength.Value = 24
                plSetting = False
            End If
            Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
            extending = True
            BuildGeneFile(gfb, Me)
            extending = False
            ProtectionLength.Background = WorkingBrush
            ProtectionLength.IsEnabled = False
            ProtectionSequenceText.Text = Await Extender(gfb.Sequences.ToString, i)
            ProtectionLength.IsEnabled = True
            ProtectionLength.Background = Brushes.Transparent
        End If
    End Sub
    Private Function Extender(seq As String, vlength As Integer) As System.Threading.Tasks.Task(Of String)
        'Dim t As New System.Threading.Tasks.Task(Of String)(Function() As String
        '                                                        Return Host.ExtendPrimer(seq, Me, vlength)
        '                                                    End Function)
        't.Start()
        Return Host.ExtendPrimerTask(seq, Me, vlength)
    End Function
    <Save()> Private Property ProtectingLength As Integer
        Get
            Dim i As Integer = 0
            Integer.TryParse(ProtectionLength.Text, i)
            Return i
        End Get
        Set(value As Integer)
            plSetting = True
            ProtectionLength.Value = value
            plSetting = False
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        If extending Then Exit Sub
        gf.Name = SequenceNameText.Text
        gf.Sequences.Append(Result.Text)
    End Sub
    Public Overrides Sub OnGeneFileBuilt(vBuiltGeneFile As Nuctions.GeneFileBuilder)
        If extending Then Exit Sub
        SequenceLength = vBuiltGeneFile.Sequences.Length
    End Sub
    Private Sub GotoEditor_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles GotoEditor.MouseLeftButtonDown
        If Host.OK Then Host.ChildRequirePCRView()
    End Sub
    Private Sub ApplyLabel_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles ApplyLabel.MouseLeftButtonDown
        If Host.OK Then Host.DNAApply()
    End Sub
End Class

<Shallow()>
Public Class SequenceOrigin
    Inherits LineBaseBioFeature
    Protected HeaderGrid As New GridBase
    Protected HeaderLabel As New Label With {.Content = "DNA"}
    Protected SequenceNameText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected NameGrid As New GridBase
    Protected NameLabel As New Label With {.Content = "Start"}
    Protected SeqLengthLabel As New Label With {.Content = "0 bp"}
    Protected WithEvents OverHangType As New ComboBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D)}
    Protected WithEvents SequenceText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
     Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(HeaderGrid)
        HeaderGrid.AddColumnItem(HeaderLabel)
        HeaderGrid.AddColumnItem(SequenceNameText)
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(OverHangType)
        NameGrid.AddColumnItem(NameLabel)
        NameGrid.AddColumnItem(SequenceText)
        NameGrid.AddColumnItem(SeqLengthLabel)
        InnerGrid.AddRowItem(Result, "*")
        OverHangType.Items.Add("*B")
        OverHangType.Items.Add("^B")
        OverHangType.Items.Add("::")
        OverHangType.Items.Add("0B")
        OverHangType.Items.Add("=B")
        OverHangType.Items.Add("*3")
        OverHangType.Items.Add("^3")
        OverHangType.Items.Add("*5")
        OverHangType.Items.Add("^5")
        ohSetting = True
        OverHangType.SelectedIndex = 0
        SequenceText.Visibility = Windows.Visibility.Collapsed
        ohSetting = False
        Shape.Fill = New SolidColorBrush(Color.FromArgb(48, 255, 215, 128))
    End Sub
    Private ohSetting As Boolean = False
    Private Sub OverHangType_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles OverHangType.SelectionChanged
        If ohSetting Then Exit Sub
        UpdateSequence()
    End Sub
    Private _SequenceLength As Integer
    <Save()> Private Property SequenceLength As Integer
        Get
            Return _SequenceLength
        End Get
        Set(value As Integer)
            _SequenceLength = value
            SeqLengthLabel.Content = _SequenceLength.ToString + " bp"
        End Set
    End Property
    Public Overrides Sub OnGeneFileBuilt(vBuiltGeneFile As Nuctions.GeneFileBuilder)
        SequenceLength = vBuiltGeneFile.ToGeneFile.Length
    End Sub
    Public Sub UpdateSequence()
        Select Case OverHangType.SelectedIndex
            Case 0 To 4
                SequenceText.Visibility = Windows.Visibility.Collapsed
                Result.Text = OverHangType.SelectedItem
            Case 5 To 8
                SequenceText.Visibility = Windows.Visibility.Visible
                Result.Text = Nuctions.TAGCFilter(SequenceText.Text) & OverHangType.SelectedItem
        End Select
        ChainSignal(Me, Me)
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Private Sub SequenceText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles SequenceText.TextChanged
        UpdateSequence()
    End Sub
    Public Overrides Sub OnChainSignal(SourceNode As LineBaseBioFeature)
        'read primer and calculate Results
 
        If Host.OK Then Host.UpdateSequences()
    End Sub
    <Save()> Private Property SaveSequenceName As String
        Get
            Return SequenceNameText.Text
        End Get
        Set(value As String)
            SequenceNameText.Text = value
        End Set
    End Property
    <Save()> Private Property DesignedSequence As String
        Get
            Return SequenceText.Text
        End Get
        Set(value As String)
            ohSetting = True
            SequenceText.Text = value
            ohSetting = False
        End Set
    End Property
    <Save()> Private Property OverHang As Integer
        Get
            Return OverHangType.SelectedIndex
        End Get
        Set(value As Integer)
            OverHangType.SelectedIndex = value
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        gf.Name = SequenceNameText.Text
        gf.End_F = FinalSequence
    End Sub
End Class

<Shallow()>
Public Class SequenceEnd
    Inherits LineBaseBioFeature
    Protected NameGrid As New GridBase
    Protected NameLabel As New Label With {.Content = "End"}
    Protected WithEvents OverHangType As New ComboBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D)}
    Protected WithEvents SequenceText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(OverHangType)
        NameGrid.AddColumnItem(NameLabel)
        NameGrid.AddColumnItem(SequenceText)
        InnerGrid.AddRowItem(Result, "*")
        OverHangType.Items.Add("*B")
        OverHangType.Items.Add("^B")
        OverHangType.Items.Add("::")
        OverHangType.Items.Add("0B")
        OverHangType.Items.Add("=B")
        OverHangType.Items.Add("*3")
        OverHangType.Items.Add("^3")
        OverHangType.Items.Add("*5")
        OverHangType.Items.Add("^5")
        ohSetting = True
        OverHangType.SelectedIndex = 0
        SequenceText.Visibility = Windows.Visibility.Collapsed
        ohSetting = False
        Shape.Fill = New SolidColorBrush(Color.FromArgb(48, 255, 215, 128))
    End Sub
    Private ohSetting As Boolean = False
    Private Sub OverHangType_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles OverHangType.SelectionChanged
        If ohSetting Then Exit Sub
        UpdateSequence()
    End Sub
    Public Sub UpdateSequence()
        Select Case OverHangType.SelectedIndex
            Case 0 To 4
                SequenceText.Visibility = Windows.Visibility.Collapsed
                Result.Text = OverHangType.SelectedItem
            Case 5 To 8
                SequenceText.Visibility = Windows.Visibility.Visible
                Result.Text = Nuctions.TAGCFilter(SequenceText.Text) & OverHangType.SelectedItem
        End Select
        ChainSignal(Me, Me)
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Private Sub SequenceText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles SequenceText.TextChanged
        UpdateSequence()
    End Sub
    <Save()> Private Property OverHang As Integer
        Get
            Return OverHangType.SelectedIndex
        End Get
        Set(value As Integer)
            OverHangType.SelectedIndex = value
        End Set
    End Property
    <Save()> Private Property DesignedSequence As String
        Get
            Return SequenceText.Text
        End Get
        Set(value As String)
            SequenceText.Text = value
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        gf.End_R = FinalSequence
    End Sub
End Class

<Shallow()>
Public Class FeatureDesigner
    Inherits LineBaseBioFeature
    Protected NameGrid As New GridBase
    Protected NameLabel As New Label With {.Content = "Feature"}
    Protected WithEvents IsReverseComplemented As New System.Windows.Controls.Label With {.Content = "->"}
    Protected WithEvents FeatureNameText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected WithEvents FeatureNameCombo As New System.Windows.Controls.ComboBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D)}
    Protected WithEvents FeatureStart As New NumberBox With {.Value = 0, .Background = Nothing, .BorderBrush = Nothing}
    Protected WithEvents FeatureEnd As New NumberBox With {.Value = 0, .Background = Nothing, .BorderBrush = Nothing}
    Protected DesignEnzymeSequence As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(NameLabel)
        NameGrid.AddColumnItem(FeatureNameText)
        NameGrid.AddColumnItem(FeatureNameCombo)
        NameGrid.AddColumnItem(FeatureStart)
        NameGrid.AddColumnItem(FeatureEnd)
        InnerGrid.AddRowItem(DesignEnzymeSequence)
        InnerGrid.AddRowItem(Result, "*")
        Shape.Fill = New SolidColorBrush(Color.FromArgb(48, 144, 255, 169))
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    <Save()> Public Property SubSequenceStart As Integer
        Get
            Return FeatureStart.Value
        End Get
        Set(value As Integer)
            FeatureStart.Value = value
        End Set
    End Property
    <Save()> Public Property SubSequenceEnd As Integer
        Get
            Return FeatureEnd.Value
        End Get
        Set(value As Integer)
            FeatureEnd.Value = value
        End Set
    End Property
    Public Overrides Sub OnAdded()
        LoadFeatures()
    End Sub
    Public Overrides Sub OnActivate()
        LoadFeatures()
    End Sub
    Private Sub LoadFeatures()
        FeatureNameCombo.Items.Clear()
        If Host.RequireWorkControl.OK Then
            Dim si As Integer = 0
            Dim wc As WorkControl = Host.RequireWorkControl.Invoke
            For Each ft As Nuctions.Feature In wc.FeatureCol
                FeatureNameCombo.Items.Add(ft)
                If _SelectedFeature.OK AndAlso ft = _SelectedFeature Then FeatureNameCombo.SelectedIndex = si
                si += 1
            Next
        End If
    End Sub
    Public Overrides Function GetSequence() As String
        Return Result.Text
    End Function
    Private ftSetting As Boolean = False
    Private fnSetting As Boolean = False
    Private _SelectedFeature As Nuctions.Feature
    <Save()> Public Property SelectedFeature As Nuctions.Feature
        Get
            Return _SelectedFeature
        End Get
        Set(value As Nuctions.Feature)
            _SelectedFeature = value
            If _SelectedFeature.OK Then
                ftSetting = True
                FeatureNameText.Text = _SelectedFeature.Name
                ftSetting = False
            End If
        End Set
    End Property
    Private Sub FeatureNameText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles FeatureNameText.TextChanged
        If ftSetting Then Exit Sub
        fnSetting = True
        For Each ft As Nuctions.Feature In FeatureNameCombo.Items
            If ft.Name.ToLower = FeatureNameText.Text Then
                _SelectedFeature = ft
                FeatureNameCombo.SelectedIndex = FeatureNameCombo.Items.IndexOf(ft)
                Exit For
            End If
        Next
        fnSetting = False
        UpdateSequence()
    End Sub
    Public Sub UpdateSequence()
        If _SelectedFeature.OK Then
            Dim si As Integer = FeatureStart.Value
            Dim ei As Integer = FeatureEnd.Value
            Dim seq As String = ""
            If ei > 0 And si > 0 AndAlso ei > si Then
                If ei > _SelectedFeature.Sequence.Length Then ei = _SelectedFeature.Sequence.Length
                If si > _SelectedFeature.Sequence.Length Then si = _SelectedFeature.Sequence.Length
                seq = _SelectedFeature.Sequence.Substring(si - 1, ei - si + 1)
            Else
                seq = _SelectedFeature.Sequence
            End If
            If IsReverseComplemented.Content = "->" Then
                Result.Text = Nuctions.TAGCFilter(seq)
            Else
                Result.Text = Nuctions.ReverseComplement(seq)
            End If
        Else
            Result.Text = ""
        End If
        ChainSignal(Me, Me)
    End Sub
    Private Sub UpdateFromNumber(sender As Object, e As RoutedEventArgs) Handles FeatureStart.TextChanged, FeatureEnd.TextChanged
        UpdateSequence()
    End Sub
    Private Sub FeatureNameCombo_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles FeatureNameCombo.SelectionChanged
        If fnSetting Then Exit Sub
        ftSetting = True
        If TypeOf FeatureNameCombo.SelectedItem Is Nuctions.Feature Then
            _SelectedFeature = FeatureNameCombo.SelectedItem
            FeatureNameText.Text = DirectCast(FeatureNameCombo.SelectedItem, Nuctions.Feature).Name
        End If
        ftSetting = False
        UpdateSequence()
    End Sub
    Private Sub IsReverseComplemented_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles IsReverseComplemented.MouseLeftButtonDown
        If IsReverseComplemented.Content = "->" Then
            IsReverseComplemented.Content = "<-"
        Else
            IsReverseComplemented.Content = "->"
        End If
        UpdateSequence()
    End Sub
    <Save()> Public Property Direction As Boolean
        Get
            Return IsReverseComplemented.Content = "->"
        End Get
        Set(value As Boolean)
            If value Then
                IsReverseComplemented.Content = "->"
            Else
                IsReverseComplemented.Content = "<-"
            End If
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        gf.Sequences.Append(Result.Text)
    End Sub
End Class

<Shallow()>
Public Class DNASequence
    Inherits LineBaseBioFeature
    Protected NameGrid As New GridBase
    Protected NameLabel As New Label With {.Content = "Name"}
    Protected SequenceNameText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected InfoGrid As New GridBase
    Protected WithEvents SequenceDirection As New System.Windows.Controls.Label With {.Content = "->"}
    Protected SeqLengthLabel As New Label With {.Content = "0 bp"}
    Protected SequenceGrid As New GridBase
    Protected InfoLabel As New Label With {.Content = "Comments"}
    Protected InfoText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected WithEvents SequenceText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .VerticalAlignment = Windows.VerticalAlignment.Stretch, .AcceptsReturn = True, .AcceptsTab = True}
    Protected Result As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(NameLabel)
        NameGrid.AddColumnItem(SequenceNameText)
        InnerGrid.AddRowItem(InfoGrid)
        InfoGrid.AddColumnItem(SequenceDirection)
        InfoGrid.AddColumnItem(InfoLabel)
        InfoGrid.AddColumnItem(InfoText)
        InfoGrid.AddColumnItem(SeqLengthLabel)
        InnerGrid.AddRowItem(SequenceText, "*")
        Shape.Fill = New SolidColorBrush(Color.FromArgb(108, 224, 255, 120))
        InnerGrid.AddRowItem(Result)
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Private _SequenceLength As Integer = 0
    <Save()> Private Property SequenceLength As Integer
        Get
            Return _SequenceLength
        End Get
        Set(value As Integer)
            _SequenceLength = value
            SeqLengthLabel.Content = _SequenceLength.ToString + " bp"
        End Set
    End Property
    Public Sub UpdateSequence()
        If SequenceDirection.Content = "->" Then
            Result.Text = Nuctions.TAGCFilter(SequenceText.Text)
        Else
            Result.Text = Nuctions.ReverseComplement(SequenceText.Text)
        End If
        SequenceLength = Result.Text.Length
        ChainSignal(Me, Me)
    End Sub
    Public Overrides Function GetSequence() As String
        Return Result.Text
    End Function
    Private Sub SequenceDirection_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles SequenceDirection.MouseLeftButtonDown
        If SequenceDirection.Content = "->" Then
            SequenceDirection.Content = "<-"
        Else
            SequenceDirection.Content = "->"
        End If
    End Sub
    Private stSetting As Boolean = False
    Private Sub SequenceText_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles SequenceText.TextChanged
        If stSetting Then Exit Sub
        UpdateSequence()
    End Sub
    <Save()> Public Property Direction As Boolean
        Get
            Return SequenceDirection.Content = "->"
        End Get
        Set(value As Boolean)
            If value Then
                SequenceDirection.Content = "->"
            Else
                SequenceDirection.Content = "<-"
            End If
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    <Save()> Public Property SequenceName As String
        Get
            Return SequenceNameText.Text
        End Get
        Set(value As String)
            SequenceNameText.Text = value
        End Set
    End Property
    Public Property Sequence As String
        Get
            Return SequenceText.Text
        End Get
        Set(value As String)
            'stSetting = True
            SequenceText.Text = value
            'stSetting = False
        End Set
    End Property
    <Save()> Private Property SequenceSave As String
        Get
            Return SequenceText.Text
        End Get
        Set(value As String)
            stSetting = True
            SequenceText.Text = value
            stSetting = False
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        If gf.PrimerMode Then
            gf.Sequences.Append(">")
            gf.Sequences.Append(Result.Text)
        Else
            gf.Sequences.Append(Result.Text)
        End If

    End Sub
End Class

<Shallow()>
Public Class AnimoSequence
    Inherits LineBaseBioFeature
    Protected NameGrid As New GridBase
    Protected NameLabel As New System.Windows.Controls.Label With {.Content = "Amino Acid Sequence"}
    Protected NameTextBox As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}
    Protected OptimizeGrid As New GridBase
    Protected WithEvents SequenceDirection As New System.Windows.Controls.Label With {.Content = "->"}
    Protected WithEvents RandomOptimize As New System.Windows.Controls.Label With {.Content = "Random"}
    Protected WithEvents MappingOptimize As New System.Windows.Controls.Label With {.Content = "Mapping"}
    Protected AvoidEnzyme As New System.Windows.Controls.Label With {.Content = "Avoid Enzymes"}
    Protected AvoidEnzymeText As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black}


    Protected ScrollContainer As New System.Windows.Controls.ScrollViewer With {.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, .VerticalScrollBarVisibility = ScrollBarVisibility.Hidden}
    Protected Optimizer As New SequenceOptimizer
    Protected Result As New System.Windows.Controls.TextBox With {.Foreground = ResultBrush, .Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        MyBase.New()
        InnerGrid.AddRowItem(NameGrid)
        NameGrid.AddColumnItem(NameLabel)
        NameGrid.AddColumnItem(NameTextBox)
        InnerGrid.AddRowItem(OptimizeGrid)
        OptimizeGrid.AddColumnItem(SequenceDirection)
        OptimizeGrid.AddColumnItem(RandomOptimize)
        OptimizeGrid.AddColumnItem(MappingOptimize)
        OptimizeGrid.AddColumnItem(AvoidEnzyme)
        OptimizeGrid.AddColumnItem(AvoidEnzymeText)
        InnerGrid.AddRowItem(ScrollContainer)
        ScrollContainer.Content = Optimizer
        Optimizer.SequenceChanged = AddressOf ChangeSequence
        Optimizer.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
        Optimizer.VerticalAlignment = Windows.VerticalAlignment.Stretch
        InnerGrid.AddRowItem(Result, "*")
        Shape.Fill = New SolidColorBrush(Color.FromArgb(108, 192, 192, 255))
    End Sub
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
    Private Sub UpdateSequence()
        If SequenceDirection.Content = "->" Then
            Result.Text = Nuctions.TAGCFilter(Optimizer.GetSequence)
        Else
            Result.Text = Nuctions.ReverseComplement(Optimizer.GetSequence)
        End If
        ChainSignal(Me, Me)
    End Sub
    Private Sub ChangeSequence(value As String)
        If SequenceDirection.Content = "->" Then
            Result.Text = value
        Else
            Result.Text = Nuctions.ReverseComplement(value)
        End If
        ChainSignal(Me, Me)
    End Sub
    Private Sub RandomOptimize_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles RandomOptimize.MouseLeftButtonDown
        Optimizer.Optimize(SettingEntry.CodonCol, SettingEntry.EnzymeCol, True, Nuctions.ParseEnzymes(AvoidEnzymeText.Text, SettingEntry.EnzymeCol))
    End Sub
    Private Sub MappingOptimize_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles MappingOptimize.MouseLeftButtonDown
        Optimizer.Optimize(SettingEntry.CodonCol, SettingEntry.EnzymeCol, False, Nuctions.ParseEnzymes(AvoidEnzymeText.Text, SettingEntry.EnzymeCol))
    End Sub
    Private Sub SequenceDirection_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles SequenceDirection.MouseLeftButtonDown
        If SequenceDirection.Content = "->" Then
            SequenceDirection.Content = "<-"
        Else
            SequenceDirection.Content = "->"
        End If
        UpdateSequence()
    End Sub
    Public Overrides Function GetSequence() As String
        Return Result.Text
    End Function
    <Save()> Public Property Direction As Boolean
        Get
            Return SequenceDirection.Content = "->"
        End Get
        Set(value As Boolean)
            If value Then
                SequenceDirection.Content = "->"
            Else
                SequenceDirection.Content = "<-"
            End If
        End Set
    End Property
    <LateLoad(0)> Public Property ProteinSequence As List(Of Tuple(Of String, Integer))
        Get
            Dim vList As New List(Of Tuple(Of String, Integer))
            For Each cd As CodonDesigner In Optimizer.Children
                vList.Add(New Tuple(Of String, Integer)(cd.AminoAcid, cd.CodonOption))
            Next
            Return vList
        End Get
        Set(value As List(Of Tuple(Of String, Integer)))
            If value.OK Then
                Dim _Dispatcher = Dispatcher.DisableProcessing
                Optimizer.Children.Clear()
                Optimizer.BlockProcessing = True
                Dim cd As CodonDesigner
                For Each ct As Tuple(Of String, Integer) In value
                    cd = Optimizer.AddCodon()
                    cd.AminoAcid = ct.Item1
                    cd.CodonOption = ct.Item2
                Next
                Optimizer.BlockProcessing = False
                _Dispatcher.Dispose()
            End If
        End Set
    End Property
    <LateLoad(1)> Public Property FinalSequence As String
        Get
            Return Result.Text
        End Get
        Set(value As String)
            Result.Text = value
        End Set
    End Property
    Public Overrides Sub ExportToGeneFile(gf As Nuctions.GeneFileBuilder)
        gf.Sequences.Append(Result.Text)
    End Sub
End Class

<Shallow()>
Public Class SequenceOptimizer
    Inherits StackPanel

    Public Sub New()
        Me.Orientation = Controls.Orientation.Horizontal
        Me.CanHorizontallyScroll = True
        AddCodon()
    End Sub
    Public Sub Optimize(ByVal trans As Nuctions.Translation, ByVal RE As Nuctions.RestrictionEnzymes, ByVal IsRandom As Boolean, ByVal Avoiding As List(Of String))
        Dim os As New Nuctions.OptimizingSequence(ReadAminoSequence, trans, RE, IsRandom, Avoiding)
        Dim res = os.Optimize
        BlockProcessing = True
        If res.OK Then
            Dim stb As New System.Text.StringBuilder
            Dim j As Integer = 0
            For Each cd As CodonDesigner In Children
                cd.SequenceCombo.SelectedIndex = res(j)
                stb.Append(DirectCast(cd.SequenceCombo.SelectedItem, Nuctions.GeneticCode).Name)
                j += 1
            Next
            If SequenceChanged.OK Then SequenceChanged.Invoke(stb.ToString)
        End If
        BlockProcessing = False
    End Sub
    Public Function ReadAminoSequence() As String
        Dim cdList As New List(Of CodonDesigner)
        Dim stb As New System.Text.StringBuilder
        For Each cd As CodonDesigner In Children
            'cdList.Add(cd)
            If cd.AminoAcid = "" Or cd.AminoAcid = " " Then
                cdList.Add(cd)
            Else
                stb.Append(cd.AminoAcid)
            End If
        Next
        For Each cd As CodonDesigner In cdList
            Children.Remove(cd)
        Next
        Return stb.ToString
    End Function
    Public Sub PasteDNAFrom(cd As CodonDesigner)
        Dim _Dispatcher = Dispatcher.DisableProcessing
        BlockProcessing = True
        Dim i As Integer = cd.Index
        Dim seq As String = Nuctions.TAGCFilter(System.Windows.Clipboard.GetText)
        For j As Integer = 0 To seq.Length - 3 Step 3
            AddCodon(i + j \ 3, DCode:=seq.Substring(j, 3), BlockUpdate:=True)
        Next
        UpdateIndex()
        BlockProcessing = False
        _Dispatcher.Dispose()
    End Sub
    Public Sub PasteProteinFrom(cd As CodonDesigner)
        Dim _Dispatcher = Dispatcher.DisableProcessing
        BlockProcessing = True
        Dim i As Integer = cd.Index
        Dim seq As String = Nuctions.AminoAcidFilter(System.Windows.Clipboard.GetText)
        For j As Integer = 0 To seq.Length - 1 Step 1
            AddCodon(i + j, ACode:=seq.Substring(j, 1), BlockUpdate:=True)
        Next
        UpdateIndex()
        BlockProcessing = False
        _Dispatcher.Dispose()
    End Sub
    Public Function AddCodon(Optional idx As Integer = Integer.MaxValue, Optional ACode As String = Nothing, Optional DCode As String = Nothing, Optional BlockUpdate As Boolean = False) As CodonDesigner
        Dim cd As New CodonDesigner With {.DesignerParent = Me}
        cd.NotifyChange = AddressOf OnNotifyChange
        cd.Remove = AddressOf OnRemove
        cd.AddOrMoveNext = AddressOf OnAddOrMoveNext
        cd.MoveLeft = AddressOf OnMoveLeft
        cd.MoveRight = AddressOf OnMoveRight
        cd.IndexOf = AddressOf GetIndex
        cd.PasteDNAFrom = AddressOf PasteDNAFrom
        cd.PasteProteinFrom = AddressOf PasteProteinFrom
        If ACode.OK Then
            cd.SetAminoCode(ACode)
        End If
        If DCode.OK Then
            cd.SetDNACode(DCode)
        End If
        If idx = Integer.MaxValue Then idx = Children.Count
        Children.Insert(idx, cd)
        If Not BlockUpdate Then UpdateIndex()
        Return cd
    End Function
    Private _AminoAcidSequence As String = ""
    Public Property AminoAcidSequence As String
        Get
            If _AminoAcidSequence Is Nothing Then
                _AminoAcidSequence = ""
            End If
            Return _AminoAcidSequence
        End Get
        Set(value As String)
            If value Is Nothing Then
                _AminoAcidSequence = ""
            Else
                _AminoAcidSequence = value
            End If
        End Set
    End Property
    Private Sub OnNotifyChange(value As String)
        UpdateIndex()

    End Sub
    Private Sub OnRemove(cd As CodonDesigner)
        If Children.Count > 1 Then
            Dim idx As Integer = Children.IndexOf(cd)
            Children.Remove(cd)
            If idx > 0 Then idx -= 1
            Dim ncd As CodonDesigner = Children(idx)
            ncd.AminoText.Focus()
        Else
            cd.OnNotifyChange("")
        End If
        UpdateIndex()
    End Sub
    Public Sub UpdateIndex(Optional BlockUpdate As Boolean = False)
        Dim stb As New System.Text.StringBuilder
        For Each cd As CodonDesigner In Children
            cd.UpdateIndex()
            If TypeOf cd.SequenceCombo.SelectedItem Is Nuctions.GeneticCode Then
                stb.Append(DirectCast(cd.SequenceCombo.SelectedItem, Nuctions.GeneticCode).Name)
            End If
        Next

        If Not BlockUpdate AndAlso SequenceChanged.OK Then SequenceChanged.Invoke(stb.ToString)
    End Sub
    Public Function GetSequence() As String
        Dim stb As New System.Text.StringBuilder
        For Each cd As CodonDesigner In Children
            If TypeOf cd.SequenceCombo.SelectedItem Is Nuctions.GeneticCode Then
                stb.Append(DirectCast(cd.SequenceCombo.SelectedItem, Nuctions.GeneticCode).Name)
            End If
        Next
        Return stb.ToString
    End Function
    Private Sub OnAddOrMoveNext(cd As CodonDesigner)
        Dim idx As Integer = Children.IndexOf(cd)
        Dim ncd As CodonDesigner
        If idx < Children.Count - 1 Then
            idx += 1
            ncd = Children(idx)
        Else
            ncd = AddCodon(idx + 1)
        End If
        ncd.AminoText.Focus()
    End Sub
    Private Sub OnMoveLeft(cd As CodonDesigner)
        Dim idx As Integer = Children.IndexOf(cd)
        If idx > 0 Then idx -= 1
        Dim ncd As CodonDesigner = Children(idx)
        ncd.AminoText.Focus()
    End Sub
    Private Sub OnMoveRight(cd As CodonDesigner)
        Dim idx As Integer = Children.IndexOf(cd)
        If idx < Children.Count - 1 Then idx += 1
        Dim ncd As CodonDesigner = Children(idx)
        ncd.AminoText.Focus()
    End Sub
    Private Function GetIndex(cd As CodonDesigner) As Integer
        Return Children.IndexOf(cd)
    End Function
    Public SequenceChanged As System.Action(Of String)
    Public BlockProcessing As Boolean = False
End Class

<Shallow()>
Public Class CodonDesigner
    Inherits GridBase
    Protected cMenu As New ContextMenu
    Protected WithEvents PasteProtein As New MenuItem With {.Header = "Paste Protein"}
    Protected WithEvents PasteDNA As New MenuItem With {.Header = "Paste DNA"}
    Protected HeaderGrid As New GridBase
    Protected IndexLabel As New System.Windows.Controls.Label With {.Foreground = Brushes.Blue}
    Protected Friend WithEvents AminoText As New AminoAcidBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(1D), .BorderBrush = Brushes.Black, .FontWeight = FontWeights.Bold}
    Protected Friend WithEvents SequenceCombo As New System.Windows.Controls.ComboBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D)}
    Public Sub New()
        AminoText.NotifyChange = AddressOf OnNotifyChange
        AminoText.Remove = AddressOf OnRemove
        AminoText.AddOrMoveNext = AddressOf OnAddOrMoveNext
        AminoText.MoveLeft = AddressOf OnMoveLeft
        AminoText.MoveRight = AddressOf OnMoveRight
        HeaderGrid.AddColumnItem(IndexLabel)
        HeaderGrid.AddColumnItem(AminoText)
        AddRowItem(HeaderGrid)
        AddRowItem(SequenceCombo)
        AminoText.ContextMenu = cMenu
        cMenu.Items.Add(PasteProtein)
        cMenu.Items.Add(PasteDNA)
    End Sub
    Public Property DesignerParent As SequenceOptimizer
    Private _AminoAcid As String
    Public Property AminoAcid As String
        Get
            Return AminoText.Text
        End Get
        Set(value As String)
            AminoText.Text = value
            OnNotifyChange(value)
        End Set
    End Property
    Public Sub SetAminoCode(value As String)
        AminoText.Text = value
    End Sub
    Public Sub SetDNACode(value As String)
        Dim CD As String = value.ToUpper
        If SettingEntry.CodonCol.CodeTable.ContainsKey(CD) Then
            Dim c = SettingEntry.CodonCol.CodeTable(CD)
            AminoText.Text = c.ShortName
            Dim j As Integer = 0
            SequenceCombo.Items.Clear()
            For Each cs As Nuctions.GeneticCode In c.CodeList
                SequenceCombo.Items.Add(cs)
                If cs.Name = value Then SequenceCombo.SelectedIndex = j
                j += 1
            Next
            'SequenceCombo.SelectedIndex = 0



        End If
    End Sub
    Private _Index As Integer
    Public Property Index As Integer
        Get
            Return _Index
        End Get
        Set(value As Integer)
            _Index = value
        End Set
    End Property
    Public Property CodonOption As Integer
        Get
            Return SequenceCombo.SelectedIndex
        End Get
        Set(value As Integer)
            SequenceCombo.SelectedIndex = value
        End Set
    End Property
    'Public Property Codons As List(Of String)
    Public Sub OnNotifyChange(value As String)
        Select Case value
            Case " ", ""
                SequenceCombo.Items.Clear()
                SequenceCombo.Items.Add("")
                SequenceCombo.SelectedIndex = 0
            Case Else
                SequenceCombo.Items.Clear()
                Dim cdn = SettingEntry.CodonCol.AnimoTable(value)
                For Each cs As Nuctions.GeneticCode In cdn.CodeList
                    SequenceCombo.Items.Add(cs)
                Next
                SequenceCombo.SelectedIndex = 0
        End Select
        If BlockProcessing Then Exit Sub
        If _NotifyChange.OK Then _NotifyChange.Invoke(value)
    End Sub
    Private Sub OnRemove()
        If _Remove.OK Then _Remove.Invoke(Me)
    End Sub
    Private Sub OnAddOrMoveNext()
        If _AddOrMoveNext.OK Then _AddOrMoveNext.Invoke(Me)
    End Sub
    Private Sub OnMoveLeft()
        If _MoveLeft.OK Then _MoveLeft.Invoke(Me)
    End Sub
    Private Sub OnMoveRight()
        If _MoveRight.OK Then _MoveRight.Invoke(Me)
    End Sub
    Public Sub UpdateIndex()
        If _IndexOf.OK Then IndexLabel.Content = (_IndexOf.Invoke(Me) + 1).ToString
    End Sub
    Public Property NotifyChange As System.Action(Of String)
    Public Property Remove As System.Action(Of CodonDesigner)
    Public Property AddOrMoveNext As System.Action(Of CodonDesigner)
    Public Property MoveLeft As System.Action(Of CodonDesigner)
    Public Property MoveRight As System.Action(Of CodonDesigner)
    Public Property IndexOf As System.Func(Of CodonDesigner, Integer)
    Public Property PasteDNAFrom As System.Action(Of CodonDesigner)
    Public Property PasteProteinFrom As System.Action(Of CodonDesigner)
    Private Sub SequenceCombo_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles SequenceCombo.SelectionChanged
        If BlockProcessing Then Exit Sub
        If _NotifyChange.OK Then _NotifyChange.Invoke("")
    End Sub
    Private ReadOnly Property BlockProcessing As Boolean
        Get
            If TypeOf Parent Is SequenceOptimizer Then
                Return DirectCast(Parent, SequenceOptimizer).BlockProcessing
            Else
                Return True
            End If
        End Get
    End Property

    Private Sub PasteDNA_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles PasteDNA.Click
        If _PasteDNAFrom.OK Then _PasteDNAFrom.Invoke(Me)
    End Sub

    Private Sub PasteProtein_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles PasteProtein.Click
        If _PasteProteinFrom.OK Then _PasteProteinFrom.Invoke(Me)
    End Sub
End Class

Public Class AminoAcidBox
    Inherits System.Windows.Controls.TextBox
    Protected Overrides Sub OnPreviewKeyDown(e As System.Windows.Input.KeyEventArgs)
        Select Case e.Key
            'Case Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z
            Case Key.A, Key.C To Key.I, Key.K To Key.N, Key.P To Key.T, Key.V, Key.W, Key.Y
                e.Handled = True
                Dim nxt As String = Chr(CInt(e.Key) + 21)
                If MyBase.Text <> nxt Then
                    MyBase.Text = nxt
                    If _NotifyChange.OK Then _NotifyChange.Invoke(nxt)
                    If _AddOrMoveNext.OK Then _AddOrMoveNext.Invoke()
                End If
            Case Key.Space
                Dim nxt As String = " "
                If MyBase.Text <> nxt Then
                    MyBase.Text = nxt
                    If _NotifyChange.OK Then _NotifyChange.Invoke(nxt)
                    If _AddOrMoveNext.OK Then _AddOrMoveNext.Invoke()
                End If
            Case Key.Back, Key.Delete
                MyBase.Text = ""
                If _Remove.OK Then _Remove.Invoke()
                e.Handled = True
            Case Key.Enter
                If _AddOrMoveNext.OK Then _AddOrMoveNext.Invoke()
            Case Key.Left
                If _MoveLeft.OK Then _MoveLeft.Invoke()
                e.Handled = True
            Case Key.Right
                If _MoveRight.OK Then _MoveRight.Invoke()
                e.Handled = True
            Case Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift

            Case Else
                e.Handled = True
                Dim nxt As String = "-"
                If MyBase.Text <> nxt Then
                    MyBase.Text = nxt
                    If _NotifyChange.OK Then _NotifyChange.Invoke(nxt)
                End If
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Public Property NotifyChange As System.Action(Of String)
    Public Property Remove As System.Action
    Public Property AddOrMoveNext As System.Action
    Public Property MoveLeft As System.Action
    Public Property MoveRight As System.Action
End Class