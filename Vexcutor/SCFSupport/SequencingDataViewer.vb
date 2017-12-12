Imports System.Windows, System.Windows.Media
Public Class SequencingDataViewer
    Inherits FrameworkElement
    Protected _Visuals As New VisualCollection(Me)
    Protected _SequenceVisual As New SequenceVisual
    Public Sub New()
        _Visuals.Add(_SequenceVisual)
        'SetValue(SequenceVisualPropertyKey, _SequenceVisual)
    End Sub
    'Public ReadOnly Property SequenceVisual As SequenceVisual
    '    Get
    '        Return GetValue(SequencingDataViewer.SequenceVisualProperty)
    '    End Get
    'End Property
    'Private Shared ReadOnly SequenceVisualPropertyKey As DependencyPropertyKey =
    '                              DependencyProperty.RegisterReadOnly("SequenceVisual",
    '                              GetType(SequenceVisual), GetType(SequencingDataViewer),
    '                              New PropertyMetadata(Nothing))
    'Public Shared ReadOnly SequenceVisualProperty As DependencyProperty =
    '                             SequenceVisualPropertyKey.DependencyProperty
    Public Sub Draw(Offset As Integer)
        _SequenceVisual.DrawRegion(Offset, ActualWidth, ActualHeight)
    End Sub
    Public Property Data As RawSequencingData.RawData
        Get
            Return GetValue(DataProperty)
        End Get
        Set(ByVal value As RawSequencingData.RawData)
            SetValue(DataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly DataProperty As DependencyProperty =
                           DependencyProperty.Register("Data",
                           GetType(RawSequencingData.RawData), GetType(SequencingDataViewer),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedDataChanged)))
    Private Shared Sub SharedDataChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingDataViewer).DataChanged(d, e)
    End Sub
    Private Sub DataChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf e.NewValue Is RawSequencingData.RawData Then
            Dim rd As RawSequencingData.RawData = e.NewValue
            _SequenceVisual.AData = rd.A
            _SequenceVisual.CData = rd.C
            _SequenceVisual.GData = rd.G
            _SequenceVisual.TData = rd.T
            _SequenceVisual.Sequence = rd.Sequence
            _SequenceVisual.Locations = rd.Locations
        Else
            _SequenceVisual.AData = Nothing
            _SequenceVisual.CData = Nothing
            _SequenceVisual.GData = Nothing
            _SequenceVisual.TData = Nothing
            _SequenceVisual.Sequence = Nothing
            _SequenceVisual.Locations = Nothing
        End If
    End Sub
    Protected Overrides Function GetVisualChild(index As Integer) As Visual
        Return _Visuals(index)
    End Function
    Protected Overrides ReadOnly Property VisualChildrenCount As Integer
        Get
            Return _Visuals.Count
        End Get
    End Property
End Class

Public Class SequenceVisual
    Inherits DrawingVisual
    Public Property Sequence As String
        Get
            Return GetValue(SequenceProperty)
        End Get
        Set(ByVal value As String)
            SetValue(SequenceProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SequenceProperty As DependencyProperty =
                             DependencyProperty.Register("Sequence",
                             GetType(String), GetType(SequenceVisual),
                             New PropertyMetadata(""))
    Public Property ABrush As Brush
        Get
            Return GetValue(ABrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(ABrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ABrushProperty As DependencyProperty =
                             DependencyProperty.Register("ABrush",
                             GetType(Brush), GetType(SequenceVisual),
                             New PropertyMetadata(Brushes.Green))
    Public Property CBrush As Brush
        Get
            Return GetValue(CBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(CBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CBrushProperty As DependencyProperty =
                             DependencyProperty.Register("CBrush",
                             GetType(Brush), GetType(SequenceVisual),
                             New PropertyMetadata(Brushes.Black))
    Public Property GBrush As Brush
        Get
            Return GetValue(GBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(GBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GBrushProperty As DependencyProperty =
                             DependencyProperty.Register("GBrush",
                             GetType(Brush), GetType(SequenceVisual),
                             New PropertyMetadata(Brushes.Blue))
    Public Property TBrush As Brush
        Get
            Return GetValue(TBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(TBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TBrushProperty As DependencyProperty =
                             DependencyProperty.Register("TBrush",
                             GetType(Brush), GetType(SequenceVisual),
                             New PropertyMetadata(Brushes.Red))
    Public Property Locations As Integer()
        Get
            Return GetValue(LocationsProperty)
        End Get
        Set(ByVal value As Integer())
            SetValue(LocationsProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LocationsProperty As DependencyProperty =
                             DependencyProperty.Register("Locations",
                             GetType(Integer()), GetType(SequenceVisual),
                             New PropertyMetadata(Nothing))
    Public Property AData As Double()
        Get
            Return GetValue(ADataProperty)
        End Get
        Set(ByVal value As Double())
            SetValue(ADataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ADataProperty As DependencyProperty =
                             DependencyProperty.Register("AData",
                             GetType(Double()), GetType(SequenceVisual),
                             New PropertyMetadata(Nothing))
    Public Property CData As Double()
        Get
            Return GetValue(CDataProperty)
        End Get
        Set(ByVal value As Double())
            SetValue(CDataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CDataProperty As DependencyProperty =
                             DependencyProperty.Register("CData",
                             GetType(Double()), GetType(SequenceVisual),
                             New PropertyMetadata(Nothing))
    Public Property GData As Double()
        Get
            Return GetValue(GDataProperty)
        End Get
        Set(ByVal value As Double())
            SetValue(GDataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GDataProperty As DependencyProperty =
                             DependencyProperty.Register("GData",
                             GetType(Double()), GetType(SequenceVisual),
                             New PropertyMetadata(Nothing))
    Public Property TData As Double()
        Get
            Return GetValue(TDataProperty)
        End Get
        Set(ByVal value As Double())
            SetValue(TDataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TDataProperty As DependencyProperty =
                             DependencyProperty.Register("TData",
                             GetType(Double()), GetType(SequenceVisual),
                             New PropertyMetadata(Nothing))
    Private Shared info = System.Globalization.CultureInfo.CurrentCulture
    Public Shared ZoomFactor As Double = 4.0#
    Private Shared _TypeFace As New System.Windows.Media.Typeface(New FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal)
    Private Shared FontSize As Double = 14.0#


    Public Sub DrawRegion(offset As Integer, length As Integer, height As Double)
        Dim dA = AData
        Dim dC = CData
        Dim dG = GData
        Dim dT = TData
        Dim l = Locations
        Dim seq = Sequence
        If offset >= dA.Length - 1 Then Return
        Dim endset = offset + length \ 4
        If endset >= dA.Length Then
            endset = dA.Length - 1
        End If
        Dim bA = ABrush
        Dim bC = CBrush
        Dim bG = GBrush
        Dim bT = TBrush
        Dim pA = New Pen(ABrush, 1.0#)
        Dim pC = New Pen(CBrush, 1.0#)
        Dim pG = New Pen(GBrush, 1.0#)
        Dim pT = New Pen(TBrush, 1.0#)

        Dim gA As New StreamGeometry
        Dim gC As New StreamGeometry
        Dim gG As New StreamGeometry
        Dim gT As New StreamGeometry


        Dim x As Double = 0#
        Dim cA = gA.Open
        Dim cC = gC.Open
        Dim cG = gG.Open
        Dim cT = gT.Open

        Dim ht = height - 46.0#

        cA.BeginFigure(New Point(x * ZoomFactor, 42.0# + ht * (1 - dA(offset))), False, False)
        cC.BeginFigure(New Point(x * ZoomFactor, 42.0# + ht * (1 - dC(offset))), False, False)
        cG.BeginFigure(New Point(x * ZoomFactor, 42.0# + ht * (1 - dG(offset))), False, False)
        cT.BeginFigure(New Point(x * ZoomFactor, 42.0# + ht * (1 - dT(offset))), False, False)
        x += 1.0#

        For i As Integer = offset + 1 To endset
            cA.LineTo(New Point(x * ZoomFactor, 42.0# + ht * (1 - dA(i))), True, False)
            cC.LineTo(New Point(x * ZoomFactor, 42.0# + ht * (1 - dC(i))), True, False)
            cG.LineTo(New Point(x * ZoomFactor, 42.0# + ht * (1 - dG(i))), True, False)
            cT.LineTo(New Point(x * ZoomFactor, 42.0# + ht * (1 - dT(i))), True, False)
            x += 1.0#
        Next

        cA.Close()
        cC.Close()
        cG.Close()
        cT.Close()



        Dim ind = QuickSearchIndexRegion(offset, endset, l)
        Dim lF = ind.Item1
        Dim lR = ind.Item2



        Dim fA = New System.Windows.Media.FormattedText("A", info, Windows.FlowDirection.LeftToRight, _TypeFace, FontSize, bA)
        Dim fC = New System.Windows.Media.FormattedText("C", info, Windows.FlowDirection.LeftToRight, _TypeFace, FontSize, bC)
        Dim fG = New System.Windows.Media.FormattedText("G", info, Windows.FlowDirection.LeftToRight, _TypeFace, FontSize, bG)
        Dim fT = New System.Windows.Media.FormattedText("T", info, Windows.FlowDirection.LeftToRight, _TypeFace, FontSize, bT)


        Dim hf = FontSize / 2.0#
        Dim xValue As Double
        Using c = RenderOpen()
            c.DrawGeometry(Nothing, pA, gA)
            c.DrawGeometry(Nothing, pC, gC)
            c.DrawGeometry(Nothing, pG, gG)
            c.DrawGeometry(Nothing, pT, gT)
            For i As Integer = lF To lR
                xValue = (l(i) - offset) * ZoomFactor - hf
                c.DrawText(New System.Windows.Media.FormattedText((i + 1).ToString, info, Windows.FlowDirection.LeftToRight, _TypeFace, FontSize, Brushes.Gray), New Point(xValue, 2.0#))
                Select Case seq(i)
                    Case "A"c
                        c.DrawText(fA, New Point(xValue, 20.0#))
                    Case "C"c
                        c.DrawText(fC, New Point(xValue, 20.0#))
                    Case "G"c
                        c.DrawText(fG, New Point(xValue, 20.0#))
                    Case "T"c
                        c.DrawText(fT, New Point(xValue, 20.0#))
                End Select
            Next
        End Using
    End Sub


    Private Function QuickSearchIndexRegion(rStart As Integer, rEnd As Integer, Indices As Integer()) As Tuple(Of Integer, Integer)
        Dim c As Double

        Dim half As Double

        Dim i As Integer
        Dim l As Integer
        Dim r As Integer

        If rStart <= Indices(0) Then
            l = 0
        ElseIf rStart >= Indices.Last Then
            l = Indices.Count - 1
        Else
            c = Indices.Count / 2
            half = c / 2
            i = Math.Round(c)
            While Not (Indices(i - 1) < rStart And rStart <= Indices(i))

                If rStart > Indices(i) Then
                    c += half
                Else
                    c -= half
                End If
                i = Math.Round(c)
                half = half / 2
            End While
            l = i
        End If

        If rEnd <= Indices(0) Then
            r = 0
        ElseIf rEnd >= Indices.Last Then
            r = Indices.Count - 1
        Else
            c = Indices.Count / 2
            half = c / 2
            i = Math.Round(c)
            While Not (Indices(i - 1) < rEnd And rEnd <= Indices(i))

                If rEnd > Indices(i) Then
                    c += half
                Else
                    c -= half
                End If
                i = Math.Round(c)
                half = half / 2
            End While
            r = i
        End If
        Return New Tuple(Of Integer, Integer)(l, r)
    End Function
End Class
Public Class PolylineVisual
    Inherits DrawingVisual
    Public Property Pen As Pen
        Get
            Return GetValue(PenProperty)
        End Get
        Set(ByVal value As Pen)
            SetValue(PenProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PenProperty As DependencyProperty =
                             DependencyProperty.Register("Pen",
                             GetType(Pen), GetType(PolylineVisual),
                             New PropertyMetadata(Pens.Black))
    Public Property Data As Single()
        Get
            Return GetValue(DataProperty)
        End Get
        Set(ByVal value As Single())
            SetValue(DataProperty, value)
        End Set
    End Property
    Public Shared ReadOnly DataProperty As DependencyProperty =
                             DependencyProperty.Register("Data",
                             GetType(Single()), GetType(PolylineVisual),
                             New PropertyMetadata(Nothing))
    Public Sub DrawRegion(offset As Integer, length As Integer, height As Double)
        Dim geo As New StreamGeometry
        If offset >= Data.Length - 1 Then Return
        Dim x As Double = 0#
        Using g = geo.Open
            g.BeginFigure(New Point(x, Data(offset)), False, False)
            x += 1.0#
            For i As Integer = offset + 1 To Data.Length - 1
                g.LineTo(New Point(x, Data(i)), True, False)
                x += 1.0#
            Next
        End Using
        Using c = RenderOpen()
            c.DrawGeometry(Nothing, Pen, geo)
        End Using
    End Sub
End Class
