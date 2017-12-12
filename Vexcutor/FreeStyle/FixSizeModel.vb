Imports System.Runtime.Serialization, System.Reflection, System.Windows.Controls, System.Windows.Media, System.Windows.Shapes, System.Windows.Input, System.Windows

<Shallow()>
Public MustInherit Class FixedSizeBase
    Inherits TextShape
    Protected WithEvents ShapeBase As Shape
    Protected MustOverride ReadOnly Property GetDesiredSize() As System.Windows.Vector

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
        ShapeBase.Width = w
        ShapeBase.Height = h
    End Sub
    Public Property RotationAngle As Double
        Get
            Return MyBase.Rotation
        End Get
        Set(value As Double)
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
    Public Property RealSize() As System.Windows.Point
        Get
            Return ActualSize
        End Get
        Set(value As System.Windows.Point)
            Dim ct As System.Windows.Vector = Location
            ActualSize = value
            CanvasLocation = ct - (value.AsVector / 2).RotateByDegree(Rotation)
            PassMovement()
        End Set
    End Property

    <Act("O")> Public Property O() As System.Windows.Point
        Get
            Return CanvasLocation '+ (ActualSize / 2).RotateByDegree(Rotation) '- (ActualSize / 2).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)
            'Dim ct As System.Windows.Vector = value + (ActualSize / 2).RotateByDegree(Rotation)
            CanvasLocation = value '- (ActualSize / 2).RotateByDegree(Rotation)
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
            Dim ct As System.Windows.Vector = Location
            az = (value.AsVector - ct).RotateByDegree(-Rotation)
            If az.X < 6D Then az.X = 6D
            If az.Y < 6D Then az.Y = 6D
            ActualSize = az * 2
            Location = ct
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
            Return Location + V(-az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property RightPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(az.X, 0).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property TopPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, -az.Y).RotateByDegree(Rotation)
        End Get
    End Property
    Private ReadOnly Property BottomPoint As System.Windows.Vector
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return Location + V(0, az.Y).RotateByDegree(Rotation)
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
    Protected _Adorners As New Dictionary(Of String, ControlPoint) From {
{"O", New ControlPoint("O", Me) With {.Fill = New SolidColorBrush(Color.FromArgb(64, 255, 255, 0))}},
 {"C", New ControlPoint("C", Me, AddressOf OnMenu) With {.Fill = Brushes.Pink}}
}
    Public Sub Square()

        Dim az As System.Windows.Vector = ActualSize
        Dim min As Double = Math.Min(az.X, az.Y)
        Dim ct As System.Windows.Vector = Location
        ActualSize = az * 2
        Location = ct
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
                    Return O
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
                    O = value
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
    Protected Overrides Sub OnSizeChanged()
        If TypeOf Host Is FreeStage Then
            Dim az As System.Windows.Vector = GetDesiredSize
            If ActualSize <> az Then
                ActualSize = az
                If Shape IsNot Nothing Then
                    Shape.Width = az.X
                    Shape.Height = az.Y
                End If
                PassMovement()
            End If
        End If
    End Sub
End Class

Public Class Atom
    Inherits FixedSizeBase
    Public Sub New()
        Dim cp As New ControlPoint("L", Me)
        cp.AllowConnecting = True
        _Adorners.Add("L", cp)
    End Sub
    Public Overrides Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As System.Tuple(Of IActor, ControlPoint, ControlPoint)
        If TypeOf Host Is FreeStage Then
            If vActor Is Nothing Then
                Dim bond As New Bond
                bond.Location = position
                bond.Size = V(0D, 0D)
                bond.AddTo(Host)
                Dim az As System.Windows.Vector = ActualSize / 2
                Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
                Dim xr, yr As Double
                Dim t As Double = EllipseNear(az, position - ct, xr, yr)
                Dim cp As ControlPoint = bond.Adorners("X")
                _EdgeBindings.Add(cp.Mapping(Me), V(xr, yr))
                bond.AdornerLocation(New List(Of Object) From {Me}, "X") = V(xr * az.X + ct.X, yr * az.Y + ct.Y)
                Return New Tuple(Of IActor, ControlPoint, ControlPoint)(bond, bond.Adorners("X"), bond.Adorners("Y"))
            Else
                vActor.Item1.AdornerLocation(New List(Of Object) From {Me}, "Y") = position
                If done Then
                    Host.TryBindPoint(vActor.Item3)
                End If
                Return vActor
            End If
        Else
            Return Nothing
        End If
    End Function
    Public Overrides Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
        Get
            Select Case aID
                Case "O"
                    Return O
                Case "S"
                    Return RSize
                Case "L"
                    Return ConnectorPoint
                Case "C"
                    Return AppearancePoint
            End Select
        End Get
        Set(value As System.Windows.Vector)
            TemperatorySender = sender
            Select Case aID
                Case "O"
                    O = value
                Case "S"
                    RSize = value
                Case "L"

                Case "C"
                    _Adorners(aID).Move()
            End Select
            If sender IsNot _Adorners(aID) Then _Adorners(aID).Move()
            TemperatorySender = Nothing
        End Set
    End Property
    Protected Overrides ReadOnly Property GetDesiredSize As System.Windows.Vector
        Get
            Me.Stroke = New SolidColorBrush(Color.FromArgb(48, 0, 0, 0))
            Return V(24, 24)
        End Get
    End Property
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Ellipse
            Return ShapeBase
        End Get
    End Property
    Public _EdgeBindings As New ShallowDictionary(Of ControlPointMapping, System.Windows.Vector)
    <LateLoad()> Public Property EdgeBindings As ShallowDictionary(Of ControlPointMapping, System.Windows.Vector)
        Get
            Return _EdgeBindings
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, System.Windows.Vector))
            _EdgeBindings = value
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If Adorners.ContainsValue(cp) Then Return False
        Dim loc As System.Windows.Vector = cp.Position
        Dim az As System.Windows.Vector = ActualSize / 2
        Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
        Dim xr, yr As Double
        Dim t As Double = EllipseNear(az, loc - ct, xr, yr)
        If t < 6 Then
            _EdgeBindings.Add(cp.Mapping(Me), V(xr, yr))
            cp.PositionMove(New List(Of Object) From {Me}, V(xr * az.X + ct.X, yr * az.Y + ct.Y), True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = _EdgeBindings.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If _EdgeBindings.ContainsKey(Key) Then _EdgeBindings.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim az As System.Windows.Vector = ActualSize / 2
        Dim r As System.Windows.Vector
        Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
        For Each cp In _EdgeBindings.Keys
            r = _EdgeBindings(cp)
            If TemperatorySender IsNot cp.Target Then
                If TypeOf cp.Target Is Bond Then
                    Dim bd As Bond = cp.Target
                    Dim am As System.Windows.Vector = bd.GetOtherAtom(Me)
                    RenderBond(am, cp.Target, cp.Point)
                Else
                    cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = V(r.X * az.X + ct.X, r.Y * az.Y + ct.Y)
                End If
            End If
        Next
    End Sub
    Public ReadOnly Property Center As System.Windows.Vector
        Get
            Return CanvasLocation + ActualSize / 2
        End Get
    End Property
    Public Sub RenderBond(sender As System.Windows.Vector, vBond As Bond, vSide As ControlPoint)
        For Each cm As ControlPointMapping In _EdgeBindings.Keys
            If cm.Target Is vBond AndAlso cm.Point Is vSide Then
                Dim loc As System.Windows.Vector = sender
                Dim az As System.Windows.Vector = ActualSize / 2
                Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
                Dim xr, yr As Double
                Dim t As Double = EllipseNear(az, loc - ct, xr, yr)
                vBond.AdornerLocation(New List(Of Object) From {Me}, vSide.ID) = V(xr * az.X + ct.X, yr * az.Y + ct.Y)
            End If
        Next
    End Sub
    Private _AtomRadius As Double = -1D
    <Menu()> Public Property AtomRadius() As Double
        Get
            Return _AtomRadius
        End Get
        Set(value As Double)
            _AtomRadius = value
            If _AtomRadius > 0D Then
                ActualSize = V(_AtomRadius, _AtomRadius)
            Else
                OnSizeChanged()
            End If
        End Set
    End Property
    Public Overrides Sub OnTextChanged()
        If _AtomRadius <= 0D Then
            OnSizeChanged()
        End If
        MyBase.OnTextChanged()
    End Sub
    Public Property ConnectorPoint As System.Windows.Point
        Get
            Dim az As System.Windows.Vector = ActualSize / 2
            Return CanvasLocation + az.RotateByDegree(Rotation) + V(az.X, az.Y).RotateByDegree(Rotation)
        End Get
        Set(value As System.Windows.Point)

        End Set
    End Property
    Protected Overrides Sub OnSizeChanged()
        If TypeOf Host Is FreeStage Then
            Dim az As System.Windows.Vector
            If _AtomRadius > 0D Then
                az = V(_AtomRadius, _AtomRadius)
            ElseIf _AtomRadius < 0D Then
                Dim radius As Double
                Dim s As String = AtomTable.GetValue(MyBase.Text, 4)
                Dim i As Integer
                If Integer.TryParse(s, i) Then
                    radius = i
                    radius = 20D + Math.Sqrt(radius)
                    az = V(radius, radius)
                Else
                    az = GetDesiredSize
                End If
            ElseIf _AtomRadius = 0D Then
                az = GetDesiredSize
            End If
            If ActualSize <> az Then
                ActualSize = az
                If Shape IsNot Nothing Then
                    Shape.Width = az.X
                    Shape.Height = az.Y
                End If
                PassMovement()
            End If
        End If
    End Sub
    'Symbol ID Name - Calculated
    Public Shared AtomTable As New StringDictionaryTable(<A>CDATA[H	1	hydrogen	25	53	120	38	no data	
He	2	helium	no data	31	140	32	no data	
Li	3	lithium	145	167	182	134	no data	152
Be	4	beryllium	105	112	153 a	90	85	112
B	5	boron	85	87	192 a	82	73	
C	6	carbon	70	67	170	77	60	
N	7	nitrogen	65	56	155	75	54	
O	8	oxygen	60	48	152	73	53	
F	9	fluorine	50	42	147	71	53	
Ne	10	neon	no data	38	154	69	no data	
Na	11	sodium	180	190	227	154	no data	186
Mg	12	magnesium	150	145	173	130	127	160
Al	13	aluminium	125	118	184 a	118	111	143
Si	14	silicon	110	111	210	111	102	
P	15	phosphorus	100	98	180	106	94	
S	16	sulfur	100	88	180	102	95	
Cl	17	chlorine	100	79	175	99	93	
Ar	18	argon	71	71	188	97	96	
K	19	potassium	220	243	275	196	no data	227
Ca	20	calcium	180	194	231 a	174	133	197
Sc	21	scandium	160	184	211 a	144	114	162 b
Ti	22	titanium	140	176	no data	136	108	147
V	23	vanadium	135	171	no data	125	106	134 b
Cr	24	chromium	140	166	no data	127	103	128 b
Mn	25	manganese	140	161	no data	139	103	127 b
Fe	26	iron	140	156	no data	125	102	126 b
Co	27	cobalt	135	152	no data	126	96	125 b
Ni	28	nickel	135	149	163	121	101	124 b
Cu	29	copper	135	145	140	138	120	128 b
Zn	30	zinc	135	142	139	131	no data	134 b
Ga	31	gallium	130	136	187	126	121	135 c
Ge	32	germanium	125	125	211 a	122	114	
As	33	arsenic	115	114	185	119	106	
Se	34	selenium	115	103	190	116	107	
Br	35	bromine	115	94	185	114	110	
Kr	36	krypton	no data	88	202	110	108	
Rb	37	rubidium	235	265	303 a	211	no data	248
Sr	38	strontium	200	219	249 a	192	139	215
Y	39	yttrium	180	212	no data	162	124	180 b
Zr	40	zirconium	155	206	no data	148	121	160
Nb	41	niobium	145	198	no data	137	116	146 b
Mo	42	molybdenum	145	190	no data	145	113	139 b
Tc	43	technetium	135	183	no data	156	110	136 b
Ru	44	ruthenium	130	178	no data	126	103	134 b
Rh	45	rhodium	135	173	no data	135	106	134 b
Pd	46	palladium	140	169	163	131	112	137 b
Ag	47	silver	160	165	172	153	137	144 b
Cd	48	cadmium	155	161	158	148	no data	151 b
In	49	indium	155	156	193	144	146	167
Sn	50	tin	145	145	217	141	132	
Sb	51	antimony	145	133	206 a	138	127	
Te	52	tellurium	140	123	206	135	121	
I	53	iodine	140	115	198	133	125	
Xe	54	xenon	no data	108	216	130	122	
Cs	55	caesium	260	298	343 a	225	no data	265
Ba	56	barium	215	253	268 a	198	149	222
La	57	lanthanum	195	no data	no data	169	139	187 b
Ce	58	cerium	185	no data	no data	no data	131	181.8 c
Pr	59	praseodymium	185	247	no data	no data	128	182.4 c
Nd	60	neodymium	185	206	no data	no data	no data	181.4 c
Pm	61	promethium	185	205	no data	no data	no data	183.4 c
Sm	62	samarium	185	238	no data	no data	no data	180.4 c
Eu	63	europium	185	231	no data	no data	no data	180.4 c
Gd	64	gadolinium	180	233	no data	no data	132	180.4 c
Tb	65	terbium	175	225	no data	no data	no data	177.3 c
Dy	66	dysprosium	175	228	no data	no data	no data	178.1 c
Ho	67	holmium	175	no data	no data	no data	no data	176.2 c
Er	68	erbium	175	226	no data	no data	no data	176.1 c
Tm	69	thulium	175	222	no data	no data	no data	175.9 c
Yb	70	ytterbium	175	222	no data	no data	no data	176 c
Lu	71	lutetium	175	217	no data	160	131	173.8 c
Hf	72	hafnium	155	208	no data	150	122	159
Ta	73	tantalum	145	200	no data	138	119	146 b
W	74	tungsten	135	193	no data	146	115	139 b
Re	75	rhenium	135	188	no data	159	110	137 b
Os	76	osmium	130	185	no data	128	109	135 b
Ir	77	iridium	135	180	no data	137	107	135.5 b
Pt	78	platinum	135	177	175	128	110	138.5 b
Au	79	gold	135	174	166	144	123	144 b
Hg	80	mercury	150	171	155	149	no data	151 b
Tl	81	thallium	190	156	196	148	150	170
Pb	82	lead	180	154	202	147	137	
Bi	83	bismuth	160	143	207 a	146	135	
Po	84	polonium	190	135	197 a	no data	129	
At	85	astatine	no data	no data	202 a	no data	138	
Rn	86	radon	no data	120	220 a	145	133	
Fr	87	francium	no data	no data	348 a	no data	no data	no data
Ra	88	radium	215	no data	283 a	no data	159	no data
Ac	89	actinium	195	no data	no data	no data	140	
Th	90	thorium	180	no data	no data	no data	136	179 b
Pa	91	protactinium	180	no data	no data	no data	129	163 d
U	92	uranium	175	no data	186	no data	118	156 e
Np	93	neptunium	175	no data	no data	no data	116	155 e
Pu	94	plutonium	175	no data	no data	no data	no data	159 e
Am	95	americium	175	no data	no data	no data	no data	173 b
Cm	96	curium	no data	no data	no data	no data	no data	174 b
Bk	97	berkelium	no data	no data	no data	no data	no data	170 b
Cf	98	californium	no data	no data	no data	no data	no data	186+/- 2 b
Es	99	einsteinium	no data	no data	no data	no data	no data	186+/- 2 b
Fm	100	fermium	no data	no data	no data	no data	no data	no data
Md	101	mendelevium	no data	no data	no data	no data	no data	no data
No	102	nobelium	no data	no data	no data	no data	no data	no data
Lr	103	lawrencium	no data	no data	no data	no data	no data	no data
Rf	104	rutherfordium	no data	no data	no data	no data	131	no data
Db	105	dubnium	no data	no data	no data	no data	126	no data
Sg	106	seaborgium	no data	no data	no data	no data	121	no data
Bh	107	bohrium	no data	no data	no data	no data	119	no data
Hs	108	hassium	no data	no data	no data	no data	118	no data
Mt	109	meitnerium	no data	no data	no data	no data	113	no data
Ds	110	darmstadtium	no data	no data	no data	no data	112	no data
Rg	111	roentgenium	no data	no data	no data	no data	118	no data
Cn	112	copernicium	no data	no data	no data	no data	130	no data
Uut	113	ununtrium	no data	no data	no data	no data	no data	no data
Uuq	114	ununquadium	no data	no data	no data	no data	no data	no data
Uup	115	ununpentium	no data	no data	no data	no data	no data	no data 
Uuh	116	ununhexium	no data	no data	no data	no data	no data	no data]</A>)
End Class

Public Module Bindings
    Public Function HorizentalCentrolLineBinderTester(tool As TextShape) As Func(Of Boolean)
        Dim f As Func(Of ControlPoint, Boolean) = Function(cp) As Boolean
                                                      Dim loc As System.Windows.Vector = tool.GetCanvasLocation
                                                      Dim sz As System.Windows.Vector = tool.ActualSize
                                                      Dim x1 As Double = loc.X
                                                      Dim x2 As Double = x1 + sz.X
                                                      Dim y As Double = loc.Y + sz.Y / 2
                                                      Dim p As System.Windows.Point = cp.Position
                                                      If Math.Abs(p.Y - y) < 6 AndAlso p.X >= x1 - 6 AndAlso p.X <= x2 + 6 Then

                                                      End If
                                                  End Function
    End Function
    Public Function EllipseNear(eSize As System.Windows.Vector, P As System.Windows.Vector, ByRef xr As Double, ByRef yr As Double) As Double
        If P.X = 0D Then
            Return Math.Min(Math.Abs(P.Y - eSize.Y), Math.Abs(P.Y + eSize.Y))
        Else
            Dim k As Double = P.Y / P.X
            Dim ac As Double = eSize.Y / Math.Sqrt(eSize.Y * eSize.Y + eSize.X * eSize.X * k * k)
            Dim sc As Double = Math.Sqrt(1 - ac * ac)
            Dim min As Double = Double.MaxValue
            Dim q As System.Windows.Vector
            q = V(eSize.X * ac, eSize.Y * sc) - P
            If min > q.Length Then
                min = q.Length
                xr = ac
                yr = sc
            End If
            q = V(eSize.X * ac, -eSize.Y * sc) - P
            If min > q.Length Then
                min = q.Length
                xr = ac
                yr = -sc
            End If
            q = V(-eSize.X * ac, eSize.Y * sc) - P
            If min > q.Length Then
                min = q.Length
                xr = -ac
                yr = sc
            End If
            q = V(-eSize.X * ac, -eSize.Y * sc) - P
            If min > q.Length Then
                min = q.Length
                xr = -ac
                yr = -sc
            End If
            Return min
        End If
    End Function
End Module

Public Class ChemicalBond
    Inherits ShapeBase
    Private _Bonds As Integer = 1
    Public Property Bonds As Double
        Get
            Return _Bonds ' CDbl(GetValue(BondsProperty))
        End Get
        Set(value As Double)
            'SetValue(BondsProperty, value)
            _Bonds = value
            Dim u = Stroke
            Stroke = Brushes.Blue
            Stroke = u

            If _Bonds < 1 Then _Bonds = 1
        End Set
    End Property
    Public Sub Update()
        Dim st = Me.Stroke
        Me.Stroke = Brushes.Transparent
        Me.Stroke = st
    End Sub
    Private _Conformation As Double = 0D
    Private _HasConformation As Double = -1D
    Private _ConformationFrontBrush As Brush = Brushes.Black
    Public Property ConformationFrontBrush As Brush
        Get
            Return _ConformationFrontBrush
        End Get
        Set(value As Brush)
            If TypeOf value Is SolidColorBrush Then
                _ConformationFrontBrush = value
            Else
                _ConformationFrontBrush = Brushes.Black
            End If
            Update()
        End Set
    End Property
    Private _ConformationBackBrush As Brush = Brushes.Black
    Public Property ConformationBackBrush As Brush
        Get
            Return _ConformationBackBrush
        End Get
        Set(value As Brush)
            If TypeOf value Is SolidColorBrush Then
                _ConformationBackBrush = value
            Else
                _ConformationBackBrush = Brushes.Black
            End If
            Update()
        End Set
    End Property
    Public Property HasConformation As Double
        Get
            Return _HasConformation
        End Get
        Set(value As Double)
            _HasConformation = value
            If _HasConformation > 0D Then
                Fill = GetConformationBrush()
            Else
                Fill = Brushes.Transparent
            End If
            Update()
        End Set
    End Property
    Public Property Conformation As Double
        Get
            Return _Conformation
        End Get
        Set(value As Double)
            _Conformation = value
            If _HasConformation > 0D Then
                Fill = GetConformationBrush()
            Else
                Fill = Brushes.Transparent
            End If
            Update()
        End Set
    End Property
    Public Function GetConformationBrush() As Brush
        Dim strokecolor As Color = DirectCast(_ConformationBackBrush, SolidColorBrush).Color
        If _Conformation < 0D Then
            Dim az As System.Windows.Vector = ActualSize
            Dim lbg As New LinearGradientBrush
            lbg.StartPoint = P(0, 0.5)
            lbg.EndPoint = P(1, 0.5)
            lbg.MappingMode = BrushMappingMode.RelativeToBoundingBox
            lbg.SpreadMethod = GradientSpreadMethod.Pad
            Dim alter As Boolean = True
            For i = 0 To az.X Step 3

                If alter Then
                    lbg.GradientStops.Add(New GradientStop(strokecolor, i / az.X))
                    lbg.GradientStops.Add(New GradientStop(strokecolor, (i + 2) / az.X))
                Else
                    lbg.GradientStops.Add(New GradientStop(Colors.Transparent, i / az.X))
                    lbg.GradientStops.Add(New GradientStop(Colors.Transparent, (i + 2) / az.X))
                End If
                alter = Not alter
            Next
            Return lbg
        Else
            Return _ConformationFrontBrush
        End If
    End Function
    Protected Overrides Sub InternalDrawArrowGeometry(context As System.Windows.Media.StreamGeometryContext)
        Dim az As System.Windows.Vector = ActualSize
        Dim delta As Integer = (_Bonds - 1) * 2
        Dim h As Double = az.Y / 2
        Dim w As Double = az.X

        If _HasConformation < 0D Then
            StrokeThickness = 2
            With context
                For j As Integer = -delta To delta Step 4
                    .BeginFigure(0D, h - j)
                    .LineTo(w, h - j)
                Next
                .CloseFigure()
            End With
        Else
            StrokeThickness = 0D
            If Math.Round(_Conformation) Mod 2 = 1 Then
                With context
                    .BeginFigure(0.5D, h)
                    .LineTo(w - 1D, h - 4)
                    .LineTo(w - 1D, h + 4)
                    .LineTo(0.5D, h)
                    .CloseFigure()
                End With
            Else
                With context
                    .BeginFigure(w - 1D, h)
                    .LineTo(0.5D, h - 4)
                    .LineTo(0.5D, h + 4)
                    .LineTo(w - 1D, h)
                    .CloseFigure()
                End With
            End If
        End If
    End Sub

    Private Sub ChemicalBond_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        If _Conformation <> 0D Then
            Fill = GetConformationBrush()
        End If
    End Sub
End Class

<Shallow()>
Public MustInherit Class FixedLineBase
    Inherits TextShape
    Protected WithEvents ShapeBase As Shape
    Protected MustOverride ReadOnly Property GetDesiredHeight() As Double
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
        'ShapeBase.Points = pc
    End Sub
    Public Property HeightValue As Double
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
    Protected Overrides Sub OnSizeChanged()
        If TypeOf Host Is FreeStage Then
            HeightValue = GetDesiredHeight
            Dim az As System.Windows.Vector = ActualSize
            If Shape IsNot Nothing Then
                Shape.Width = az.X
                Shape.Height = HeightValue
            End If
        End If
    End Sub
End Class

Public Class Bond
    Inherits FixedLineBase
    Public Sub New()
        MyBase.New()
        TextVisible = 0D
    End Sub
    Protected Overrides ReadOnly Property GetDesiredHeight As Double
        Get
            Return Math.Max(18D, _Bond.Bonds * 4 + 4)
        End Get
    End Property
    <Menu(), Act()> Public Property BondNumber As Double
        Get
            Return _Bond.Bonds
        End Get
        Set(value As Double)
            _Bond.Bonds = Math.Round(value)
            OnSizeChanged()
        End Set
    End Property
    <Menu(), Act()> Public Property ConformationFrontBrush As Brush
        Get
            Return _Bond.ConformationFrontBrush
        End Get
        Set(value As Brush)
            _Bond.ConformationFrontBrush = value
        End Set
    End Property
    <Menu(), Act()> Public Property ConformationBackBrush As Brush
        Get
            Return _Bond.ConformationBackBrush
        End Get
        Set(value As Brush)
            _Bond.ConformationBackBrush = value
        End Set
    End Property
    <Menu(), Act()> Public Property HasConformation As Double
        Get
            Return _Bond.HasConformation
        End Get
        Set(value As Double)
            _Bond.HasConformation = value
        End Set
    End Property
    <Menu(), Act()> Public Property Conformation As Double
        Get
            Return _Bond.Conformation
        End Get
        Set(value As Double)
            _Bond.Conformation = value
        End Set
    End Property
    <Menu(), Act()> Public Property Dashed As Double
        Get
            If _Bond.StrokeDashArray.Count = 0 Then
                Return 0D
            Else
                Return _Bond.StrokeDashArray(0)
            End If
        End Get
        Set(value As Double)
            If value < 0D Then value = -value
            If value = 0D Then
                _Bond.StrokeDashArray.Clear()
            Else
                _Bond.StrokeDashArray.Clear()
                _Bond.StrokeDashArray.Add(value)
                _Bond.StrokeDashArray.Add(value)
            End If
        End Set
    End Property
    Private _Bond As ChemicalBond
    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then
                If _Bond Is Nothing Then _Bond = New ChemicalBond
                ShapeBase = _Bond
            End If
            Return ShapeBase
        End Get
    End Property
    Public Function GetOtherAtom(sender As Atom) As System.Windows.Vector
        Dim xAtom As IActor = Adorners("X").DependentTarget
        Dim yAtom As IActor = Adorners("Y").DependentTarget
        If xAtom Is sender Then
            If TypeOf yAtom Is Atom Then
                Dim am As Atom = yAtom
                am.RenderBond(CType(xAtom, Atom).Center, Me, Adorners("Y"))
                Return am.Center
            Else
                Return AdornerLocation(Nothing, "Y")
            End If
        End If
        If yAtom Is sender Then
            If TypeOf xAtom Is Atom Then
                Dim am As Atom = xAtom
                am.RenderBond(CType(yAtom, Atom).Center, Me, Adorners("X"))
                Return am.Center
            Else
                Return AdornerLocation(Nothing, "X")
            End If
        End If
    End Function
End Class

Public Class LineBindRectangle
    Inherits TextRectangleBase
    Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    <LateLoad()> Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double)
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If Adorners.ContainsValue(cp) Then Return False
        Dim ct As System.Windows.Vector = Location
        Dim az As System.Windows.Vector = ActualSize / 2
        Dim p As System.Windows.Vector = cp.Position

        Dim lp As System.Windows.Vector = ct + V(-az.X, 0).RotateByDegree(Rotation)
        Dim rp As System.Windows.Vector = ct + V(az.X, 0).RotateByDegree(Rotation)

        Dim bx As System.Windows.Vector = V(1, 0).RotateByDegree(Rotation)
        Dim by As System.Windows.Vector = V(0, 1).RotateByDegree(Rotation)
        Dim x1 As Double = bx.X * lp.X + bx.Y * lp.Y
        Dim x2 As Double = bx.X * rp.X + bx.Y * rp.Y
        Dim y As Double = by.X * lp.X + by.Y * lp.Y
        Dim xl As Double = Math.Min(x1, x2)
        Dim xr As Double = Math.Max(x1, x2)
        p = V(bx.X * p.X + bx.Y * p.Y, by.X * p.X + by.Y * p.Y)
        If Math.Abs(p.Y - y) < 6D AndAlso p.X > xl - 6D AndAlso p.X < xr + 6D Then
            If p.X < xl Then p.X = xl
            If p.X > xr Then p.X = xr
            Dim m As Double = (xl + xr) / 2
            Dim t As Double = (p.X - m) * 2 / (xr - xl)
            _BindingPoints.Add(cp.Mapping(Me), t)
            cp.PositionMove(New List(Of Object) From {Me}, ct + V(t * az.X, 0).RotateByDegree(Rotation), True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = _BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If _BindingPoints.ContainsKey(Key) Then _BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim az As System.Windows.Vector = ActualSize / 2
        Dim t As Double
        Dim ct As System.Windows.Vector = CanvasLocation + az.RotateByDegree(Rotation)
        For Each cp In _BindingPoints.Keys
            t = _BindingPoints(cp)
            If TemperatorySender IsNot cp.Target Then
                cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = ct + V(t * az.X, 0).RotateByDegree(Rotation)
            End If
        Next
    End Sub
End Class

Public Class LineBindLine
    Inherits TextLineBase
    Private _BindingPoints As New ShallowDictionary(Of ControlPointMapping, Double)
    <LateLoad()> Public Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double)
        Get
            Return _BindingPoints
        End Get
        Set(value As ShallowDictionary(Of ControlPointMapping, Double))
            _BindingPoints = value
        End Set
    End Property
    Public Overrides Function BindPoint(cp As ControlPoint) As Boolean
        If Adorners.ContainsValue(cp) Then Return False
        Dim ct As System.Windows.Vector = (LinearX.AsVector + LinearY.AsVector) / 2
        Dim az As System.Windows.Vector = ActualSize / 2
        Dim p As System.Windows.Vector = cp.Position

        Dim lp As System.Windows.Vector = ct + V(-az.X, 0).RotateByDegree(Rotation)
        Dim rp As System.Windows.Vector = ct + V(az.X, 0).RotateByDegree(Rotation)

        Dim bx As System.Windows.Vector = V(1, 0).RotateByDegree(Rotation)
        Dim by As System.Windows.Vector = V(0, 1).RotateByDegree(Rotation)
        Dim x1 As Double = bx.X * lp.X + bx.Y * lp.Y
        Dim x2 As Double = bx.X * rp.X + bx.Y * rp.Y
        Dim y As Double = by.X * lp.X + by.Y * lp.Y
        Dim xl As Double = Math.Min(x1, x2)
        Dim xr As Double = Math.Max(x1, x2)
        p = V(bx.X * p.X + bx.Y * p.Y, by.X * p.X + by.Y * p.Y)
        If Math.Abs(p.Y - y) < 6D AndAlso p.X > xl - 6D AndAlso p.X < xr + 6D Then
            If p.X < xl Then p.X = xl
            If p.X > xr Then p.X = xr
            Dim t As Double = Math.Abs(p.X - x1)
            _BindingPoints.Add(cp.Mapping(Me), t)
            cp.PositionMove(New List(Of Object) From {Me}, LinearX + V(t, 0).RotateByDegree(Rotation), True)
            Return True
        End If
        Return False
    End Function
    Public Overrides Sub ReleasePoint(cp As ControlPoint)
        Dim keys = _BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If _BindingPoints.ContainsKey(Key) Then _BindingPoints.Remove(Key)
        Next
    End Sub
    Public Overrides Sub ProcessBindingPoints()
        Dim t As Double
        For Each cp In _BindingPoints.Keys
            t = _BindingPoints(cp)
            If TemperatorySender IsNot cp.Target Then
                cp.Target.AdornerLocation(TemperatorySender, cp.Point.ID) = LinearX + V(t, 0).RotateByDegree(Rotation)
            End If
        Next
    End Sub

    Public Overrides ReadOnly Property Shape As System.Windows.Shapes.Shape
        Get
            If ShapeBase Is Nothing Then ShapeBase = New Rectangle
            Return ShapeBase
        End Get
    End Property
End Class
