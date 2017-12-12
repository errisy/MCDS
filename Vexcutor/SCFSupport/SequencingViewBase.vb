Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Shapes

Public Class SequencingViewBase
    Inherits UserControl
    Protected WithEvents Part_ScrollBar As System.Windows.Controls.Primitives.ScrollBar
    Protected Part_Viewer As SequencingDataViewer
    Public Overrides Sub OnApplyTemplate()
        Part_Viewer = Template.FindName("Part_Viewer", Me)
        Part_ScrollBar = Template.FindName("Part_ScrollBar", Me)
        Part_ScrollBar.Maximum = Length
        If _Data IsNot Nothing Then Part_Viewer.Data = _Data
        MyBase.OnApplyTemplate()
    End Sub
    Private _Data As RawSequencingData.RawData
    'SequencingView->Data As SCFData with Event Default: Nothing
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
                           GetType(RawSequencingData.RawData), GetType(SequencingViewBase),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedDataChanged)))
    Private Shared Sub SharedDataChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).DataChanged(d, e)
    End Sub
    Private Sub DataChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        _Data = e.NewValue
        UpdateLength()
        Refresh()
    End Sub
    Protected Overrides Sub OnRenderSizeChanged(sizeInfo As SizeChangedInfo)
        UpdateLength()
        Refresh()
        MyBase.OnRenderSizeChanged(sizeInfo)
    End Sub
    Protected Sub UpdateLength()
        If TypeOf _Data Is RawSequencingData.RawData Then
            Dim l = CInt(_Data.A.Length - Me.ActualWidth \ SequenceVisual.ZoomFactor - 1)
            SetValue(LengthPropertyKey, l)
            If Part_ScrollBar IsNot Nothing Then Part_ScrollBar.Maximum = l
            If Part_Viewer IsNot Nothing Then Part_Viewer.Data = _Data
        End If
    End Sub
    Public ReadOnly Property Length As Integer
        Get
            Return GetValue(SequencingViewBase.LengthProperty)
        End Get
    End Property
    Private Shared ReadOnly LengthPropertyKey As DependencyPropertyKey =
                            DependencyProperty.RegisterReadOnly("Length",
                            GetType(Integer), GetType(SequencingViewBase),
                            New PropertyMetadata(Nothing))
    Public Shared ReadOnly LengthProperty As DependencyProperty =
                           LengthPropertyKey.DependencyProperty
    'SCFViewBase->Offset As Integer with Event Default: 0
    Public Property Offset As Integer
        Get
            Return GetValue(OffsetProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(OffsetProperty, value)
        End Set
    End Property
    Public Shared ReadOnly OffsetProperty As DependencyProperty =
                           DependencyProperty.Register("Offset",
                           GetType(Integer), GetType(SequencingViewBase),
                           New PropertyMetadata(0, New PropertyChangedCallback(AddressOf SharedOffsetChanged)))
    Private Shared Sub SharedOffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).OffsetChanged(d, e)
    End Sub
    Private Sub OffsetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Refresh()
    End Sub

    Private info = System.Globalization.CultureInfo.GetCultureInfo("en-us")

    Private Sub Refresh()
        If Part_Viewer Is Nothing Then Return
        Part_Viewer.Draw(Offset)

        'If _Data Is Nothing Then Return
        'If Part_ATrace Is Nothing Then Return
        'If Part_CTrace Is Nothing Then Return
        'If Part_GTrace Is Nothing Then Return
        'If Part_TTrace Is Nothing Then Return

        'Dim count As Integer = Me.ActualWidth
        'Dim h = Me.ActualHeight - 72.0#

        'Dim fgA = New PathFigure
        'Dim fgC = New PathFigure
        'Dim fgG = New PathFigure
        'Dim fgT = New PathFigure
        'Dim vlength As Integer = _Data.ATrace.Length
        'If Offset < 0 Then Offset = 0 : Return
        'If Offset >= vlength - count Then Offset = vlength - count - 1 : Return
        'fgA.StartPoint = New Point(0, ScaleH(Offset, h, _Data.ATrace))
        'fgC.StartPoint = New Point(0, ScaleH(Offset, h, _Data.CTrace))
        'fgG.StartPoint = New Point(0, ScaleH(Offset, h, _Data.GTrace))
        'fgT.StartPoint = New Point(0, ScaleH(Offset, h, _Data.TTrace))
        'For i As Integer = 1 To count
        '    fgA.Segments.Add(New LineSegment(New Point(i * 5, ScaleH(i + Offset, h, _Data.ATrace)), True))
        '    fgC.Segments.Add(New LineSegment(New Point(i * 5, ScaleH(i + Offset, h, _Data.CTrace)), True))
        '    fgG.Segments.Add(New LineSegment(New Point(i * 5, ScaleH(i + Offset, h, _Data.GTrace)), True))
        '    fgT.Segments.Add(New LineSegment(New Point(i * 5, ScaleH(i + Offset, h, _Data.TTrace)), True))
        'Next
        'Part_ATrace.Geometry.Clear()
        'Part_CTrace.Geometry.Clear()
        'Part_GTrace.Geometry.Clear()
        'Part_TTrace.Geometry.Clear()
        'Part_ATrace.Geometry.Figures.Add(fgA)
        'Part_CTrace.Geometry.Figures.Add(fgC)
        'Part_GTrace.Geometry.Figures.Add(fgG)
        'Part_TTrace.Geometry.Figures.Add(fgT)

        'Dim ft As System.Windows.Media.FormattedText

        'Dim TF As New System.Windows.Media.Typeface(FontFamily, FontStyle, FontWeight, FontStretch)
        'Dim fg As New PathFigure
        'Dim rgn = QuickSearchIndexRegion(Offset, Offset + count, _Data.Indices)
        'Dim gA = Part_ALabel.Geometry
        'Dim gC = Part_CLabel.Geometry
        'Dim gG = Part_GLabel.Geometry
        'Dim gT = Part_TLabel.Geometry
        'Dim gL = Part_Scale.Geometry
        'gA.Clear()
        'gC.Clear()
        'gG.Clear()
        'gT.Clear()
        'gL.Clear()
        'For i As Integer = rgn.Item1 To rgn.Item2
        '    Dim vl = MaxValue(i)
        '    If vl.Contains("A") Then
        '        ft = New System.Windows.Media.FormattedText("A", info, Windows.FlowDirection.LeftToRight, TF, FontSize, Brushes.Black)
        '        gA.AddGeometry(ft.BuildGeometry(New Point((_Data.Indices(i) - Offset) * 5 - ft.Width / 2, 0)))
        '    End If
        '    If vl.Contains("C") Then
        '        ft = New System.Windows.Media.FormattedText("C", info, Windows.FlowDirection.LeftToRight, TF, FontSize, Brushes.Black)
        '        gC.AddGeometry(ft.BuildGeometry(New Point((_Data.Indices(i) - Offset) * 5 - ft.Width / 2, 0)))
        '    End If
        '    If vl.Contains("G") Then
        '        ft = New System.Windows.Media.FormattedText("G", info, Windows.FlowDirection.LeftToRight, TF, FontSize, Brushes.Black)
        '        gG.AddGeometry(ft.BuildGeometry(New Point((_Data.Indices(i) - Offset) * 5 - ft.Width / 2, 0)))
        '    End If
        '    If vl.Contains("T") Then
        '        ft = New System.Windows.Media.FormattedText("T", info, Windows.FlowDirection.LeftToRight, TF, FontSize, Brushes.Black)

        '        gT.AddGeometry(ft.BuildGeometry(New Point((_Data.Indices(i) - Offset) * 5 - ft.Width / 2, 0)))
        '    End If
        '    ft = New System.Windows.Media.FormattedText((i + 1).ToString, info, Windows.FlowDirection.LeftToRight, TF, FontSize, Brushes.Black)
        '    gL.AddGeometry(ft.BuildGeometry(New Point((_Data.Indices(i) - Offset) * 5 - ft.Width / 2, 0)))
        'Next

        'Part_ATrace.Render()
        'Part_CTrace.Render()
        'Part_GTrace.Render()
        'Part_TTrace.Render()
        'Part_ALabel.Render()
        'Part_CLabel.Render()
        'Part_GLabel.Render()
        'Part_TLabel.Render()
        'Part_Scale.Render()
    End Sub
    Private Function ScaleH(index As Integer, h As Double, Trace As Int16()) As Double
        Dim u As Integer = Trace(index)
        If u > 0 Then u = 0
        Return 5 + (h - 10) * (1 + u / 256)
    End Function
    Private Function QuickSearchIndexRegion(rStart As Integer, rEnd As Integer, Indices As UInt32()) As Tuple(Of Integer, Integer)
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
    'Private Function MaxValue(index As Integer) As List(Of String)
    '    Dim a = _Data.AProb(index)
    '    Dim c = _Data.CProb(index)
    '    Dim g = _Data.GProb(index)
    '    Dim t = _Data.TProb(index)
    '    Dim k = Math.Max(Math.Max(a, c), Math.Max(g, t))
    '    Dim stb As New List(Of String)
    '    If k = a Then stb.Add("A")
    '    If k = c Then stb.Add("C")
    '    If k = g Then stb.Add("G")
    '    If k = t Then stb.Add("T")
    '    Return stb
    'End Function

    'SCFViewBase->ABrush As Brush with Event Default: Brushes.Green
    Public Property ABrush As Brush
        Get
            Return GetValue(ABrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(ABrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ABrushProperty As DependencyProperty = _
                           DependencyProperty.Register("ABrush", _
                           GetType(Brush), GetType(SequencingViewBase), _
                           New PropertyMetadata(Brushes.Green, New PropertyChangedCallback(AddressOf SharedABrushChanged)))
    Private Shared Sub SharedABrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).ABrushChanged(d, e)
    End Sub
    Private Sub ABrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    'SCFViewBase->CBrush As Brush with Event Default: Brushes.Black
    Public Property CBrush As Brush
        Get
            Return GetValue(CBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(CBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("CBrush", _
                           GetType(Brush), GetType(SequencingViewBase), _
                           New PropertyMetadata(Brushes.Black, New PropertyChangedCallback(AddressOf SharedCBrushChanged)))
    Private Shared Sub SharedCBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).CBrushChanged(d, e)
    End Sub
    Private Sub CBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    'SCFViewBase->GBrush As Brush with Event Default: Brushes.Blue
    Public Property GBrush As Brush
        Get
            Return GetValue(GBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(GBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly GBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("GBrush", _
                           GetType(Brush), GetType(SequencingViewBase), _
                           New PropertyMetadata(Brushes.Blue, New PropertyChangedCallback(AddressOf SharedGBrushChanged)))
    Private Shared Sub SharedGBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).GBrushChanged(d, e)
    End Sub
    Private Sub GBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    'SCFViewBase->TBrush As Brush with Event Default: Brushes.Red
    Public Property TBrush As Brush
        Get
            Return GetValue(TBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(TBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("TBrush", _
                           GetType(Brush), GetType(SequencingViewBase), _
                           New PropertyMetadata(Brushes.Red, New PropertyChangedCallback(AddressOf SharedTBrushChanged)))
    Private Shared Sub SharedTBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, SequencingViewBase).TBrushChanged(d, e)
    End Sub
    Private Sub TBrushChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub
    'SCFViewBase->LabelBrush As Brush Default: Brushes.Gray
    Public Property LabelBrush As Brush
        Get
            Return GetValue(LabelBrushProperty)
        End Get
        Set(ByVal value As Brush)
            SetValue(LabelBrushProperty, value)
        End Set
    End Property
    Public Shared ReadOnly LabelBrushProperty As DependencyProperty = _
                           DependencyProperty.Register("LabelBrush", _
                           GetType(Brush), GetType(SequencingViewBase), _
                           New PropertyMetadata(Brushes.Gray))

    Private Sub Part_ScrollBar_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles Part_ScrollBar.ValueChanged
        Offset = Part_ScrollBar.Value
    End Sub
End Class
