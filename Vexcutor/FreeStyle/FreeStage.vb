Imports System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows, System.Windows.Shapes

Public Interface IShallowHost
    Sub RemoveChild(sender As Object)
    Sub Add(nType As Type)
    Sub Add(nElement As System.Windows.FrameworkElement)
End Interface

Public Class FreeStage
    Inherits System.Windows.Controls.Canvas
    Implements IShallow, IShallowHost
    Public Sub New()
        Background = New SolidColorBrush(Color.FromArgb(1, 255, 255, 255))
        cvMain.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
        cvMain.VerticalAlignment = Windows.VerticalAlignment.Stretch
        Children.Add(cvMain)
        cvMain.Children.Add(AddingRect)
        AddingRect.Visibility = Windows.Visibility.Hidden
    End Sub
    Public HostDNAInfo As DNAInfo
    Public HostChartItem As ChartItem
    Public Sub UpdateSequences()
        'read primer and calculate Results
        HostDNAInfo.DNAs.Clear()
        Dim ori As SequenceOrigin
        Dim enzymes As IEnumerable(Of String) = New List(Of String)
        enzymes = enzymes.Union(HostChartItem.Parent.EnzymeCol)
        For Each act As IActor In Actors
            If TypeOf act Is SequenceOrigin Then
                ori = act
                Dim gfb As New Nuctions.GeneFileBuilder
                ori.BuildGeneFile(gfb, ori)
                Dim gf As Nuctions.GeneFile = gfb.ToGeneFile
                If gf.Length > 0 Then
                    HostDNAInfo.DNAs.Add(gfb.ToGeneFile)
                End If
                enzymes.Union(gfb.Enzymes)
            End If
        Next
        Nuctions.AddFeatures(HostDNAInfo.DNAs, HostChartItem.Parent.Features)
        HostChartItem.Reload(HostDNAInfo, enzymes.ToList)
        HostChartItem.Parent.Draw()
        If RelatedPropertyTab.OK Then
            RelatedPropertyTab.cbDesign_UseDesigner.Checked = True
            RelatedPropertyTab.RelatedChartItem = HostChartItem
        End If
    End Sub
    Public Sub UpdatePrimers()
        Dim ori As PrimerOrigin
        Dim enzymes As IEnumerable(Of String) = New List(Of String)
        enzymes = enzymes.Union(HostChartItem.Parent.EnzymeCol)
        Dim PrimerDicts As New Dictionary(Of String, String)
        For Each act As IActor In Actors
            If TypeOf act Is PrimerOrigin Then
                ori = act
                Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
                ori.BuildGeneFile(gfb, ori)

                Dim i As Integer = -1
                While PrimerDicts.ContainsKey(gfb.Name + IIf(i > -1, i.ToString, ""))
                    i += 1
                End While
                gfb.Name = gfb.Name + IIf(i > -1, i.ToString, "")
                ori.PrimerName = gfb.Name
                PrimerDicts.Add(gfb.Name, gfb.Sequences.ToString)
            End If
        Next
        RelatedAnalysisView.AnalyzePrimers(PrimerDicts, HostDNAInfo.AllDNAinSource, 80 * 0.001, 625 * 0.000000001)
        'Nuctions.AddFeatures(HostDNAInfo.DNAs, HostChartItem.Parent.Features)
        'HostChartItem.Reload(HostDNAInfo, enzymes.ToList)
        'HostChartItem.Parent.Draw()
        'Nuctions.AnalyzePrimer()
    End Sub
    Public RelatedPropertyTab As PropertyControl
    Public RequirePCR As System.Action(Of List(Of Nuctions.GeneFile), Dictionary(Of String, String))
    Public Function ExtendPrimer(vPrimer As String, FromNode As IActor, AdditionalLength As Integer) As String
        Dim pe As New Nuctions.PrimerExtender
        pe.MainPrimer = vPrimer
        Dim ori As PrimerOrigin
        For Each act As IActor In Actors
            If act IsNot FromNode AndAlso TypeOf act Is PrimerOrigin Then
                ori = act
                Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
                ori.BuildGeneFile(gfb, ori)
                pe.OtherPrimers.Add(gfb.Sequences.ToString)
            End If
        Next
        pe.Templates.AddRange(HostDNAInfo.AllDNAinSource)
        Return pe.Extend(AdditionalLength)
    End Function
    Public Function ExtendPrimerTask(vPrimer As String, FromNode As IActor, AdditionalLength As Integer) As System.Threading.Tasks.Task(Of String)
        Dim pe As New Nuctions.PrimerExtender
        pe.MainPrimer = vPrimer
        Dim ori As PrimerOrigin
        For Each act As IActor In Actors
            If act IsNot FromNode AndAlso TypeOf act Is PrimerOrigin Then
                ori = act
                Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
                ori.BuildGeneFile(gfb, ori)
                pe.OtherPrimers.Add(gfb.Sequences.ToString)
            End If
        Next
        pe.Templates.AddRange(HostDNAInfo.AllDNAinSource)
        Dim t As New System.Threading.Tasks.Task(Of String)(Function() As String
                                                                Return pe.Extend(AdditionalLength)
                                                            End Function)
        t.Start()
        Return t
    End Function
    Public Sub ChildRequirePCRView()
        If RequirePCR.OK Then
            Dim ori As PrimerOrigin
            Dim enzymes As IEnumerable(Of String) = New List(Of String)
            enzymes = enzymes.Union(HostChartItem.Parent.EnzymeCol)
            Dim PrimerDicts As New Dictionary(Of String, String)
            For Each act As IActor In Actors
                If TypeOf act Is PrimerOrigin Then
                    ori = act
                    Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
                    ori.BuildGeneFile(gfb, ori)

                    Dim i As Integer = -1
                    While PrimerDicts.ContainsKey(gfb.Name + IIf(i > -1, i.ToString, ""))
                        i += 1
                    End While
                    gfb.Name = gfb.Name + IIf(i > -1, i.ToString, "")
                    ori.PrimerName = gfb.Name
                    PrimerDicts.Add(gfb.Name, gfb.Sequences.ToString)
                End If
            Next
            RequirePCR.Invoke(HostDNAInfo.AllDNAinSource, PrimerDicts)
        End If
    End Sub
    Public Sub DNAApply()
        Dim ori As PrimerOrigin
        Dim enzymes As IEnumerable(Of String) = New List(Of String)
        enzymes = enzymes.Union(HostChartItem.Parent.EnzymeCol)
        HostDNAInfo.DesignedPrimers = New Dictionary(Of String, String)
        For Each act As IActor In Actors
            If TypeOf act Is PrimerOrigin Then
                ori = act
                Dim gfb As New Nuctions.GeneFileBuilder With {.PrimerMode = True}
                ori.BuildGeneFile(gfb, ori)
                Dim i As Integer = -1
                While HostDNAInfo.DesignedPrimers.ContainsKey(gfb.Name + IIf(i > -1, i.ToString, ""))
                    i += 1
                End While
                gfb.Name = gfb.Name + IIf(i > -1, i.ToString, "")
                ori.PrimerName = gfb.Name
                HostDNAInfo.DesignedPrimers.Add(gfb.Name, gfb.Sequences.ToString)
            End If
        Next
        HostDNAInfo.PrimerDesignerMode = True
        HostDNAInfo.Calculate()
        HostChartItem.Reload(HostDNAInfo, enzymes.ToList)
        HostChartItem.Parent.Draw()
        If RelatedPropertyTab.OK Then RelatedPropertyTab.RelatedChartItem = HostChartItem
    End Sub
    <Save()> Private _Actors As New ShallowList(Of IActor)
    Friend Property RequireWorkControl As Func(Of WorkControl)
    Public Property RelatedAnalysisView As PrimerAnalysisFrame
    Public Property Actors As ShallowList(Of IActor)
        Get
            Return _Actors
        End Get
        Set(value As ShallowList(Of IActor))
            If TypeOf value Is ShallowList(Of IActor) Then
                Clear()
                For Each actor As IActor In value
                    actor.AddTo(Me)
                Next
            End If
        End Set
    End Property
    Private Sub Clear()
        Dim uActors As New ShallowList(Of IActor)
        uActors.AddRange(_Actors)
        For Each el As IActor In uActors
            el.Remove()
        Next
    End Sub
    Public ReadOnly Property ElementCollection As UIElementCollection
        Get
            Return cvMain.Children
        End Get
    End Property
    Public Sub AddActor(act As IActor)
        act.AddTo(Me)
    End Sub
    Private cvMain As New System.Windows.Controls.Canvas
    Private WithEvents slZoom As New System.Windows.Controls.Slider With {.Minimum = -1, .Maximum = 1, .Value = 0, .SmallChange = 0.05}
    Public Property HostInitialized As Boolean Implements IShallow.HostInitialized
    Private _DesignMode As Boolean = False
    Public Property Menus As New Dictionary(Of Type, AppearanceMenu)

    <LateLoad()> Public Property DesignMode As Boolean
        Get
            Return _DesignMode
        End Get
        Set(value As Boolean)
            _DesignMode = value
            If _DesignMode Then
                'Dim elements As New List(Of FrameworkElement)
                'For Each el As FrameworkElement In cvMain.Children
                '    elements.Add(el)
                'Next
                'For Each el As FrameworkElement In elements
                '    If TypeOf el Is ControlPoint Then
                '        Dim cp As ControlPoint = el
                '        cp.Move()
                '    ElseIf TypeOf el Is IActor Then
                '        DirectCast(el, IActor).IsReadOnly = False
                '    End If
                'Next
                For Each act As IActor In _Actors
                    act.IsReadOnly = False
                Next
            Else
                'Dim elements As New List(Of FrameworkElement)
                'For Each el As FrameworkElement In cvMain.Children
                '    elements.Add(el)
                'Next
                'For Each el As FrameworkElement In elements
                '    If TypeOf el Is ControlPoint Then
                '        Dim cp As ControlPoint = el
                '        cp.Move()
                '    ElseIf TypeOf el Is IActor Then
                '        DirectCast(el, IActor).IsReadOnly = True
                '    End If
                'Next
                For Each act As IActor In _Actors
                    act.IsReadOnly = True
                Next
            End If
        End Set
    End Property
    <Save()> Public Property MinimumZoom As Double
        Get
            Return slZoom.Minimum
        End Get
        Set(value As Double)
            slZoom.Minimum = value
        End Set
    End Property
    <Save()> Public Property MaximumZoom As Double
        Get
            Return slZoom.Maximum
        End Get
        Set(value As Double)
            slZoom.Maximum = value
        End Set
    End Property
    Public ReadOnly Property ZoomSlider As System.Windows.Controls.Slider
        Get
            Return slZoom
        End Get
    End Property
    Private Adding As Boolean = False
    Public Property AddingMode As Boolean
        Get
            Return Adding
        End Get
        Set(value As Boolean)
            Adding = value
            If Adding Then
                Me.Cursor = System.Windows.Input.Cursors.Cross
            Else
                Me.Cursor = System.Windows.Input.Cursors.Arrow
            End If
        End Set
    End Property
    Public Sub RemoveChild(sender As Object) Implements IShallowHost.RemoveChild
        If Children.Contains(sender) Then
            Dim er As New ElementEventArgs With {.Instance = sender}
            OnElementRemoving(er)
            If Not er.Handled Then cvMain.Children.Remove(sender)
        End If
    End Sub
    Protected Sub OnElementRemoving(e As ElementEventArgs)
        RaiseEvent ElementRemoving(Me, e)
    End Sub
 
    Private AddingArgs As ElementEventArgs
    Public Sub Add(nType As System.Type) Implements IShallowHost.Add
        AddingMode = True
        AddingArgs = New ElementEventArgs
        AddingArgs.ElementType = nType
    End Sub
    Public Sub Add(nElement As System.Windows.FrameworkElement) Implements IShallowHost.Add
        AddingMode = True
        AddingArgs = New ElementEventArgs
        AddingArgs.Instance = nElement
    End Sub
    <System.ComponentModel.Description("Customize the ElementCreating")> Public Event ElementCreating(sender As Object, e As ElementEventArgs)
    <System.ComponentModel.Description("Customize the ElementRemoving")> Public Event ElementRemoving(sender As Object, e As ElementEventArgs)
    Protected Overridable Sub OnElementCreating(e As ElementEventArgs)
        RaiseEvent ElementCreating(Me, e)
    End Sub
    Public RelatedDirector As Director
    Public RelatedPropertyGrid As PropertyGridX
    Private Sub Add()
        OnElementCreating(AddingArgs)
        If Not AddingArgs.Handled Then
            Dim index As Integer = -1
            If AddingArgs.Instance Is Nothing Then
                Try

                    Dim obj As IActor = AddingArgs.ElementType.Assembly.CreateInstance(AddingArgs.ElementType.FullName)
                    obj.AddTo(Me)
                    Dim vX As Double = Math.Min(AddingArgs.EndSizeLocation.X, AddingArgs.Location.X)
                    Dim vY As Double = Math.Min(AddingArgs.EndSizeLocation.Y, AddingArgs.Location.Y)

                    obj.Location = V(vX, vY)
                    Dim w, h As Double
                    w = Math.Abs(AddingArgs.EndSizeLocation.X - AddingArgs.Location.X)
                    h = Math.Abs(AddingArgs.EndSizeLocation.Y - AddingArgs.Location.Y)
                    If w = 0 Then w = 24
                    If h = 0 Then h = 24
                    obj.Size = V(w, h)

                    'index = cvMain.Children.Add(obj)
                Catch ex As Exception

                End Try
            Else
                Try
                    'index = cvMain.Children.Add(AddingArgs.Instance)
                    Dim obj As IActor = AddingArgs.Instance
                    obj.AddTo(Me)
                    Dim vX As Double = Math.Min(AddingArgs.EndSizeLocation.X, AddingArgs.Location.X)
                    Dim vY As Double = Math.Min(AddingArgs.EndSizeLocation.Y, AddingArgs.Location.Y)
                    obj.Location = V(vX, vY)
                    Dim w, h As Double
                    w = Math.Abs(AddingArgs.EndSizeLocation.X - AddingArgs.Location.X)
                    h = Math.Abs(AddingArgs.EndSizeLocation.Y - AddingArgs.Location.Y)
                    If w = 0 Then w = 24
                    If h = 0 Then h = 24
                    obj.Size = V(w, h)

                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Private AddingRect As New System.Windows.Shapes.Rectangle With {.Fill = New SolidColorBrush(Color.FromArgb(32, 64, 255, 255)), .Stroke = Brushes.DarkRed}
    Private _arX As Double
    Private _arY As Double

    Private Selecting As Boolean = False
    Private SelectingRect As New System.Windows.Shapes.Rectangle With {.Fill = New SolidColorBrush(Color.FromArgb(16, 255, 255, 0)), .Stroke = Brushes.Purple}
    Private SelectionStartPoint As System.Windows.Point
    Protected Overrides Sub OnMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        If Adding Then
            If AddingArgs IsNot Nothing Then
                If Not cvMain.Children.Contains(AddingRect) Then cvMain.Children.Add(AddingRect)
                Me.CaptureMouse()
                AddingArgs.Location = e.GetPosition(cvMain)
                _arX = AddingArgs.Location.X
                _arY = AddingArgs.Location.Y
                Canvas.SetLeft(AddingRect, _arX)
                Canvas.SetTop(AddingRect, _arY)
                AddingRect.Width = 0
                AddingRect.Height = 0
                AddingRect.Visibility = Windows.Visibility.Visible
            End If
        ElseIf Not (GroupMoving Or Copying) AndAlso Not SeeThroughControlPoint(e.GetPosition(cvMain)) Then
            'Rect Selection
            If Not cvMain.Children.Contains(SelectingRect) Then cvMain.Children.Add(SelectingRect)

            Me.CaptureMouse()
            Selecting = True
            SelectionStartPoint = e.GetPosition(cvMain)
            SelectingRect.SetCanvasLocation(SelectionStartPoint)
            SelectingRect.SetSize(V(0D, 0D))
        End If
        MyBase.OnMouseLeftButtonDown(e)
    End Sub
    Public Function SeeThroughControlPoint(pv As System.Windows.Point) As Boolean
        Dim cp As ControlPoint
        For Each el As FrameworkElement In cvMain.Children
            If TypeOf el Is ControlPoint Then
                cp = el
                Dim u As System.Windows.Point = cp.Position
                If Math.Abs(u.X - pv.X) <= 6D AndAlso Math.Abs(u.Y - pv.Y) <= 6D Then Return True
            End If
        Next
        Return False
    End Function

    Protected Overrides Sub OnMouseLeftButtonUp(e As System.Windows.Input.MouseButtonEventArgs)
        If Adding Then
            If AddingArgs IsNot Nothing Then
                Me.ReleaseMouseCapture()
                AddingArgs.EndSizeLocation = e.GetPosition(cvMain)
                Dim lX As Double = AddingArgs.EndSizeLocation.X
                Dim lY As Double = AddingArgs.EndSizeLocation.Y
                Canvas.SetLeft(AddingRect, Math.Min(_arX, lX))
                Canvas.SetTop(AddingRect, Math.Min(_arY, lY))
                AddingRect.Width = Math.Abs(lX - _arX)
                AddingRect.Height = Math.Abs(lY - _arY)
                AddingRect.Visibility = Windows.Visibility.Hidden
                Add()
                If cvMain.Children.Contains(AddingRect) Then cvMain.Children.Remove(AddingRect)
            End If
            AddingMode = False
        ElseIf Selecting Then
            Selecting = False
            Me.ReleaseMouseCapture()
            Dim p As System.Windows.Point = e.GetPosition(cvMain)
            Dim x As Double = Math.Min(p.X, SelectionStartPoint.X)
            Dim y As Double = Math.Min(p.Y, SelectionStartPoint.Y)
            Dim w As Double = Math.Max(p.X, SelectionStartPoint.X) - x
            Dim h As Double = Math.Max(p.Y, SelectionStartPoint.Y) - y
            SelectingRect.SetCanvasLocation(V(x, y))
            SelectingRect.SetSize(V(w, h))
            If w > 4D And h > 4D Then
                SelectedActor = SelectMultipleActors(x, y, x + w, y + h)
                If RelatedPropertyGrid.OK Then RelatedPropertyGrid.Load(SelectedActor)
                If Not cvMain.Children.Contains(CollectionMovingBar) Then cvMain.Children.Add(CollectionMovingBar)
                CollectionMovingBar.SetCanvasLocation(V(x + w / 2 - 16D, y + h / 2 - 16D))
            Else
                If cvMain.Children.Contains(CollectionMovingBar) Then cvMain.Children.Remove(CollectionMovingBar)
            End If
            GroupMoving = False
            If cvMain.Children.Contains(SelectingRect) Then cvMain.Children.Remove(SelectingRect)
        End If
        MyBase.OnMouseLeftButtonDown(e)
    End Sub

    Public Function SelectMultipleActors(l As Double, t As Double, r As Double, b As Double) As ShallowList(Of IActor)
        Dim cp As ControlPoint
        Dim p As System.Windows.Point
        Dim sla As New ShallowList(Of IActor)
        For Each el As FrameworkElement In cvMain.Children
            If TypeOf el Is ControlPoint Then
                cp = el
                p = cp.Position
                If l <= p.X AndAlso r >= p.X AndAlso t <= p.Y AndAlso b >= p.Y AndAlso Not sla.Contains(cp.BindingTarget) Then
                    sla.Add(cp.BindingTarget)
                End If
            End If
        Next
        Return sla
    End Function
    Public Sub AssignID(vList As IEnumerable(Of IActor))
        Dim idHolder As New Dictionary(Of IActor, String)
        Dim idList As List(Of String) = _Actors.Select(Function(iax) iax.ID).ToList
        For Each dc As IActor In vList
            While idList.Contains(dc.ID)
                dc.ID = Math.Round(Rnd() * 16777216).ToString
            End While
            idList.Add(dc.ID)
            idHolder.Add(dc, dc.ID)
        Next
    End Sub
    Public Function CopyAsBytes() As Byte()
        If SelectedActor.NO Then Return Nothing
        Dim cpd As New CopyPasteData
        Dim sd As New ShallowDictionary(Of String, IActor)
        For Each ia As IActor In SelectedActor
            cpd.IDTypes.Add(ia.ID, ia.GetType.FullName)
            sd.Add(ia.ID, ia)
        Next
        cpd.Status.Record(sd)
        Return ShallowSerializer.Serialize(cpd)
    End Function
    Public Function PasteBytes(bytes As Byte()) As Boolean
        Try
            Dim cpd As CopyPasteData = ShallowSerializer.Deserialize(bytes)
            Dim sd As New ShallowDictionary(Of String, IActor)
            Dim vT As Type
            For Each id As String In cpd.IDTypes.Keys
                vT = Type.GetType(cpd.IDTypes(id))
                Dim dc As IActor = vT.Assembly.CreateInstance(vT.FullName)
                dc.ID = id
                sd.Add(id, dc)
            Next
            For Each dc As IActor In sd.Values
                dc.AddTo(Me)
            Next
            cpd.Status.Present(sd, AnimationTypeEnum.Movement)
            cpd.Status.Present(sd, AnimationTypeEnum.Brush)
            cpd.Status.Present(sd, AnimationTypeEnum.Value)
            cpd.Status.Present(sd, AnimationTypeEnum.Effect)
            cpd.Status.Present(sd, AnimationTypeEnum.Text)
            AssignID(sd.Values)

            'Make them selected
            SelectedActor = New ShallowList(Of IActor)
            For Each dc As IActor In sd.Values
                SelectedActor.Add(dc)
            Next
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function
    Public Function CopyToPosition(delta As System.Windows.Vector) As ShallowList(Of IActor)
        Dim x = Me.Dispatcher.DisableProcessing
        Dim status As New Status
        Dim sd As New ShallowDictionary(Of String, IActor)
        Dim duplicated As New ShallowDictionary(Of String, IActor)
        Dim vT As Type
        Dim idList As List(Of String) = _Actors.Select(Function(iax) iax.ID).ToList
        Dim idHolder As New Dictionary(Of IActor, String)
        For Each ia As IActor In SelectedActor
            vT = ia.GetType
            Dim dc As IActor = vT.Assembly.CreateInstance(vT.FullName)
            While idList.Contains(dc.ID)
                dc.ID = Math.Round(Rnd() * 16777216).ToString
            End While
            idList.Add(dc.ID)
            duplicated.Add(dc.ID, dc)
            sd.Add(dc.ID, ia)
            idHolder.Add(dc, dc.ID)
        Next
        status.Record(sd)
        For Each dc As IActor In duplicated.Values
            dc.AddTo(Me)
        Next
        status.Present(duplicated, AnimationTypeEnum.Movement)
        status.Present(duplicated, AnimationTypeEnum.Brush)
        status.Present(duplicated, AnimationTypeEnum.Value)
        status.Present(duplicated, AnimationTypeEnum.Effect)
        status.Present(duplicated, AnimationTypeEnum.Text)
        Dim nSL As New ShallowList(Of IActor)
        For Each dc As IActor In duplicated.Values
            dc.Location = dc.Location + delta
            nSL.Add(dc)
            dc.ID = idHolder(dc)
        Next
        x.Dispose()
        Return nSL
    End Function
    Private SelectedActor As ShallowList(Of IActor)
    Private WithEvents CollectionMovingBar As New Ellipse With {.Width = 32, .Height = 32, .Fill = New SolidColorBrush(Color.FromArgb(32, 127, 255, 0)), .Stroke = Brushes.Gold, .ContextMenu = New RewriteMenu With {.Rewrite = AddressOf WriteSelection, .Delete = AddressOf DeleteSelection}}
    Private GroupMoving As Boolean = False
    Private Copying As Boolean = False
    Private GroupStart As System.Windows.Vector
    Private GroupOrigin As System.Windows.Vector
    Private GroupOriginDict As New Dictionary(Of IActor, System.Windows.Vector)
    Public Sub DeleteSelection()
        If SelectedActor.OK AndAlso SelectedActor.Count > 0 AndAlso MsgBox("Do you want to delete the selected items?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            For Each ia As IActor In SelectedActor
                ia.Remove()
            Next
            SelectedActor.Clear()
        End If
    End Sub
    Public Sub WriteSelection(aType As AnimationTypeEnum)
        Dim sd As New ShallowDictionary(Of String, IActor)
        For Each ia As IActor In SelectedActor
            sd.Add(ia.ID, ia)
        Next
        Dim dr As Director = Me.RelatedDirector
        If aType = AnimationTypeEnum.All Then
            dr.Storyline.WriteGroup(sd)
        Else
            dr.Storyline.WriteSelected(sd, aType)
        End If
    End Sub

    Private Sub MovingBarMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles CollectionMovingBar.PreviewMouseLeftButtonDown
        If SelectedActor IsNot Nothing AndAlso SelectedActor.Count > 0 Then

            If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Control Then
                Copying = True
                CollectionMovingBar.Stroke = Brushes.Red
                CollectionMovingBar.CaptureMouse()
                GroupStart = e.GetPosition(cvMain)
                GroupOrigin = CollectionMovingBar.GetCanvasLocation
            Else
                GroupMoving = True
                CollectionMovingBar.Stroke = Brushes.Gold
                CollectionMovingBar.CaptureMouse()
                GroupStart = e.GetPosition(cvMain)
                GroupOrigin = CollectionMovingBar.GetCanvasLocation
                GroupOriginDict.Clear()
                For Each ia As IActor In SelectedActor
                    GroupOriginDict.Add(ia, ia.Location)
                Next
            End If
        End If
    End Sub
    Private Sub MovingBarMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles CollectionMovingBar.MouseLeftButtonUp
        If GroupMoving Then
            GroupMoving = False
            CollectionMovingBar.ReleaseMouseCapture()
            Dim p As System.Windows.Vector = e.GetPosition(cvMain)
            Dim delta As System.Windows.Vector = p - GroupStart
            CollectionMovingBar.SetCanvasLocation(GroupOrigin + delta)
            For Each ia As IActor In SelectedActor
                If GroupOriginDict.ContainsKey(ia) Then ia.Location = GroupOriginDict(ia) + delta
            Next
        ElseIf Copying Then
            Copying = False
            CollectionMovingBar.ReleaseMouseCapture()
            Dim p As System.Windows.Vector = e.GetPosition(cvMain)
            Dim delta As System.Windows.Vector = p - GroupStart
            CollectionMovingBar.SetCanvasLocation(GroupOrigin + delta)
            SelectedActor = CopyToPosition(delta)
            CollectionMovingBar.Stroke = Brushes.Gold
        End If
    End Sub
    Private Sub MovingBarMouseMove(sender As Object, e As MouseEventArgs) Handles CollectionMovingBar.MouseMove
        If GroupMoving Then
            Dim p As System.Windows.Vector = e.GetPosition(cvMain)
            Dim delta As System.Windows.Vector = p - GroupStart
            Dim loc = GroupOrigin + delta

            Canvas.SetLeft(CollectionMovingBar, loc.X)
            Canvas.SetTop(CollectionMovingBar, loc.Y)
            For Each ia As IActor In SelectedActor
                If GroupOriginDict.ContainsKey(ia) Then ia.Location = GroupOriginDict(ia) + delta
            Next
        ElseIf Copying Then
            Dim p As System.Windows.Vector = e.GetPosition(cvMain)
            Dim delta As System.Windows.Vector = p - GroupStart
            CollectionMovingBar.SetCanvasLocation(GroupOrigin + delta)
        End If
    End Sub
    Private Moving As Boolean = False
    Private StartP As System.Windows.Point
    Private StartMouse As System.Windows.Point
    Public ReadOnly Property Scale As Double
        Get
            Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
            If t IsNot Nothing Then
                Return t.Value.M11
            Else
                Return 1D
            End If
        End Get
    End Property
    Protected Overrides Sub OnMouseMove(e As System.Windows.Input.MouseEventArgs)
        If Adding Then
            Dim loc As System.Windows.Point = e.GetPosition(cvMain)
            Dim lX As Double = loc.X
            Dim lY As Double = loc.Y
            Canvas.SetLeft(AddingRect, Math.Min(_arX, lX))
            Canvas.SetTop(AddingRect, Math.Min(_arY, lY))
            AddingRect.Width = Math.Abs(lX - _arX)
            AddingRect.Height = Math.Abs(lY - _arY)
        ElseIf Moving Then
            Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
            If t IsNot Nothing Then
                Dim currentMouse As System.Windows.Point = cvMain.PointToScreen(e.GetPosition(cvMain))
                Dim cp As System.Windows.Point = StartP + (currentMouse - StartMouse)
                Dim t1 As New System.Windows.Media.MatrixTransform(t.Value.M11, 0, 0, t.Value.M22, cp.X, cp.Y)
                cvMain.RenderTransform = t1
            End If
        ElseIf Selecting Then
            Dim p As System.Windows.Point = e.GetPosition(cvMain)
            Dim x As Double = Math.Min(p.X, SelectionStartPoint.X)
            Dim y As Double = Math.Min(p.Y, SelectionStartPoint.Y)
            Dim w As Double = Math.Max(p.X, SelectionStartPoint.X) - x
            Dim h As Double = Math.Max(p.Y, SelectionStartPoint.Y) - y
            SelectingRect.SetCanvasLocation(V(x, y))
            SelectingRect.SetSize(V(w, h))

        End If
        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseRightButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
        If t IsNot Nothing Then
            StartP = New System.Windows.Point(t.Value.OffsetX, t.Value.OffsetY)
            StartMouse = cvMain.PointToScreen(e.GetPosition(cvMain))
        End If
        Moving = True
        MyBase.OnMouseRightButtonDown(e)
    End Sub
    Protected Overrides Sub OnMouseRightButtonUp(e As System.Windows.Input.MouseButtonEventArgs)
        If Moving Then
            Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
            If t IsNot Nothing Then
                Dim currentMouse As System.Windows.Point = cvMain.PointToScreen(e.GetPosition(cvMain))
                Dim cp As System.Windows.Point = StartP + (currentMouse - StartMouse)
                Dim t1 As New System.Windows.Media.MatrixTransform(t.Value.M11, 0, 0, t.Value.M22, cp.X, cp.Y)
                cvMain.RenderTransform = t1
            End If
            Moving = False
        End If
        MyBase.OnMouseRightButtonUp(e)
    End Sub
    <Save()> Public Property ZoomScale As Double
        Get
            Return slZoom.Value
        End Get
        Set(value As Double)
            slZoom.Value = value
        End Set
    End Property
    <Save()> Public Property CanvasOffset As System.Drawing.PointF
        Get
            Return New PointF(cvMain.RenderTransform.Value.OffsetX, cvMain.RenderTransform.Value.OffsetY)
        End Get
        Set(value As System.Drawing.PointF)
            Dim zoom As Double = 10 ^ slZoom.Value
            Dim t1 As New System.Windows.Media.MatrixTransform(zoom, 0, 0, zoom, value.X, value.Y)
            cvMain.RenderTransform = t1
        End Set
    End Property
    Private _ScrollZooming As Boolean = False
    Private Sub slZoom_ValueChanged(sender As Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles slZoom.ValueChanged
        If _ScrollZooming Then Exit Sub
        Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
        If t IsNot Nothing Then
            Dim p2h As New System.Windows.Vector(ActualWidth / 2, ActualHeight / 2)
            Dim p2c As System.Windows.Vector = TranslatePoint(p2h, cvMain)
            Dim zoom As Double = 10 ^ slZoom.Value
            Dim O1 As System.Windows.Vector = p2h - p2c * zoom
            Dim t1 As New System.Windows.Media.MatrixTransform(zoom, 0, 0, zoom, O1.X, O1.Y)
            cvMain.RenderTransform = t1
        End If
    End Sub
    Protected Overrides Sub OnMouseWheel(e As System.Windows.Input.MouseWheelEventArgs)
        If System.Windows.Input.Keyboard.Modifiers = Windows.Input.ModifierKeys.Control Then
            Dim t As System.Windows.Media.Transform = cvMain.RenderTransform
            If t IsNot Nothing Then
                Dim p2c As System.Windows.Vector = e.GetPosition(cvMain)
                Dim p2h As System.Windows.Vector = e.GetPosition(Me)
                Dim zoom As Double = t.Value.M11
                zoom = zoom * 1.25 ^ (e.Delta / 60)
                If zoom < 10 ^ slZoom.Minimum Then zoom = 10 ^ slZoom.Minimum
                If zoom > 10 ^ slZoom.Maximum Then zoom = 10 ^ slZoom.Maximum
                _ScrollZooming = True
                slZoom.Value = Math.Log10(zoom)
                _ScrollZooming = False
                Dim O1 As System.Windows.Vector = p2h - p2c * zoom
                Dim t1 As New System.Windows.Media.MatrixTransform(zoom, 0, 0, zoom, O1.X, O1.Y)
                cvMain.RenderTransform = t1
            End If
        End If
        MyBase.OnMouseWheel(e)
    End Sub
    Public Function TryBindPoint(scrPoint As ControlPoint) As IActor
        If scrPoint.CanBind Then
            For Each act As IActor In Actors
                If act.BindPoint(scrPoint) Then Return act
            Next
        End If
        Return Nothing
    End Function
    Public Sub PassMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        For Each el As FrameworkElement In cvMain.Children
            If TypeOf el Is ControlPoint Then
                If DirectCast(el, ControlPoint).PassMouseLeftButtonDown(e, True) Then Exit For
            End If
        Next
    End Sub
    Public ReadOnly Property Actor(vID As String) As IActor
        Get
            For Each el As FrameworkElement In cvMain.Children
                If TypeOf el Is IActor AndAlso DirectCast(el, IActor).ID = vID Then Return el
            Next
            Return Nothing
        End Get
    End Property
#Region "Activation and Deactivation"
    Public Sub IActorActivated(sender As Object, e As RoutedEventArgs)
        For Each el As IActor In _Actors
            If el IsNot sender Then
                CType(el, IActor).Deactivate()
            End If
        Next
    End Sub
    'Public Event IActorDeactivate As RoutedEventHandler

#End Region
End Class

Public Class RewriteMenu
    Inherits ContextMenu
    Private RewriteText As New MenuItem With {.Header = "Rewrite Text"}
    Private RewritePosition As New MenuItem With {.Header = "Rewrite Position"}
    Private RewriteBrush As New MenuItem With {.Header = "Rewrite Brush"}
    Private RewriteValue As New MenuItem With {.Header = "Rewrite Value"}
    Private RewriteEffect As New MenuItem With {.Header = "Rewrite Effect"}
    Private RewriteAll As New MenuItem With {.Header = "Rewrite Effect"}
    Private DeleteMenu As New MenuItem With {.Header = "Remove Selected"}
    Public Sub New()
        Items.Add(RewriteText)
        Items.Add(RewritePosition)
        Items.Add(RewriteBrush)
        Items.Add(RewriteValue)
        Items.Add(RewriteEffect)
        Items.Add(RewriteAll)
        Items.Add(DeleteMenu)
        AddHandler RewriteText.Click, AddressOf WriteValue
        AddHandler RewritePosition.Click, AddressOf WriteValue
        AddHandler RewriteBrush.Click, AddressOf WriteValue
        AddHandler RewriteValue.Click, AddressOf WriteValue
        AddHandler RewriteEffect.Click, AddressOf WriteValue
        AddHandler RewriteAll.Click, AddressOf WriteValue
        AddHandler DeleteMenu.Click, AddressOf DeleteSelection
    End Sub
    Private Sub WriteValue(sender As Object, e As RoutedEventArgs)
        If sender Is RewriteText Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.Text)
        ElseIf sender Is RewritePosition Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.Movement)
        ElseIf sender Is RewriteBrush Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.Brush)
        ElseIf sender Is RewriteValue Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.Value)
        ElseIf sender Is RewriteEffect Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.Effect)
        ElseIf sender Is RewriteAll Then
            If Rewrite.OK Then Rewrite.Invoke(AnimationTypeEnum.All)
        End If
    End Sub
    Private Sub DeleteSelection(sender As Object, e As RoutedEventArgs)
        If Delete.OK Then Delete.Invoke()
    End Sub
    Public Rewrite As Action(Of AnimationTypeEnum)
    Public Delete As System.Action
End Class

Public Class ElementEventArgs
    Inherits EventArgs
    Public ElementType As Type
    Public Handled As Boolean = False
    Public Instance As Object
    Public Location As System.Windows.Point
    Public EndSizeLocation As System.Windows.Point
End Class

<AttributeUsage(AttributeTargets.Property)>
Public Class DependencyAttribute
    Inherits Attribute
End Class

<AttributeUsage(AttributeTargets.Property)>
Public Class BindGridAttribute
    Inherits Attribute
    Private _X As Integer
    Private _Y As Integer
    Public Sub New(vX As Integer, vY As Integer)
        _X = vX
        _Y = vY
    End Sub
    Public ReadOnly Property X As Integer
        Get
            Return _X
        End Get
    End Property
    Public ReadOnly Property Y As Integer
        Get
            Return _Y
        End Get
    End Property
End Class
<AttributeUsage(AttributeTargets.Property)>
Public Class RightHitMethodAttribute
    Inherits Attribute
    Private _Method As String
    Public Sub New(vMethod As String)
        _Method = vMethod
    End Sub
    Public ReadOnly Property Method As String
        Get
            Return _Method
        End Get
    End Property
End Class
<AttributeUsage(AttributeTargets.Property)>
Public Class LeftHitMethodAttribute
    Inherits Attribute
    Private _Method As String
    Public Sub New(vMethod As String)
        _Method = vMethod
    End Sub
    Public ReadOnly Property Method As String
        Get
            Return _Method
        End Get
    End Property
End Class

<AttributeUsage(AttributeTargets.Property)> Public Class MenuAttribute
    Inherits Attribute
End Class

Public Interface IActor
    <Save()> Property ID As String
    Property IsReadOnly As Boolean
    Sub OnMenu()
    Function BindPoint(cp As ControlPoint) As Boolean
    Sub ReleasePoint(cp As ControlPoint)
    Sub PassMovement()
    ''' <summary>
    ''' 这个是所有辅助点的定义 这样就把辅助点的定义交给控件自己来实现了 而不是在环境当中实现
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Adorner 是不需要设设置成自动记录的 它的生产需要有宿主类型自己管理</remarks>
    ReadOnly Property Adorners As Dictionary(Of String, ControlPoint)
    ''' <summary>
    ''' 通知设计器Adorner发生改变 可能是增加或者减少了
    ''' </summary>
    ''' <remarks></remarks>
    Event AdornerChanged As EventHandler
    'Event RequireMenu As EventHandler
    Property AdornerLocation(sender As List(Of Object), aID As String) As System.Windows.Vector
    <EarlyBind("Stage")> Property Host As FreeStage
    Sub AddTo(vHost As FreeStage)
    Sub Remove()
    Property Visible As Double
    Sub Activate(sender As Object, e As RoutedEventArgs)
    Sub Deactivate()
    Sub AddAdorner(position As System.Windows.Point, done As Boolean)
    Function AddConnector(position As System.Windows.Point, vActor As System.Tuple(Of IActor, ControlPoint, ControlPoint), done As Boolean) As Tuple(Of IActor, ControlPoint, ControlPoint)
    Property Size As System.Windows.Vector
    Property Location As System.Windows.Vector
End Interface

'Public Interface ILinearLayout
'    Inherits IActor
'    <BindGrid(12, 12)> Property X(Optional sender As IActor = Nothing) As System.Windows.Point
'    <BindGrid(12, 12)> Property Y(Optional sender As IActor = Nothing) As System.Windows.Point
'    <Dependency(), RightHitMethod("AutoHeight")> Property Height(Optional sender As IActor = Nothing) As System.Windows.Point
'    Sub ClearDependency()
'    Sub AutoHeight()
'End Interface

Public Interface IRectangleLayout
    Inherits IActor
     
    Property Rotation As System.Windows.Point
End Interface

<Shallow()>
Public Class ControlPointMapping
    ''' <summary>
    ''' 所有者
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Save()> Public Property Host As IActor
    ''' <summary>
    ''' 包含这个点的IActor
    ''' </summary>
    ''' <remarks></remarks>
    <Save()> Public Target As IActor
    ''' <summary>
    ''' 指向的点
    ''' </summary>
    ''' <remarks></remarks>
    Public WithEvents Point As ControlPoint
    Public Property HostID As String
        Get
            Return Host.ID
        End Get
        Set(value As String)
            Host = Host.Host.Actor(value)
        End Set
    End Property
    <LateLoad(4)> Public Property PointID As String
        Get
            Return Point.ID
        End Get
        Set(value As String)
            Point = Target.Adorners(value)
            Point.DependentTarget = Host
        End Set
    End Property
    ''' <summary>
    ''' 通知自己的所有者 该点已经被删除或者请求解除绑定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove(sender As Object, e As EventArgs) Handles Point.Removed
        Host.ReleasePoint(Point)
    End Sub
End Class


Public Class SelectionBar
    Inherits Canvas
    Private MovingBar As New Ellipse With {.Width = 32, .Height = 32, .Fill = New SolidColorBrush(Color.FromArgb(32, 127, 255, 0)), .Stroke = Brushes.Gold}
    Private Counter As New System.Windows.Controls.TextBox With {.Background = Brushes.Transparent, .BorderThickness = New Thickness(0D), .IsReadOnly = True, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .VerticalContentAlignment = Windows.VerticalAlignment.Center}
    Public Sub New()
        Children.Add(MovingBar)
        Children.Add(Counter)
        Me.Width = 32D
        Me.Height = 32D
        Canvas.SetLeft(MovingBar, 0D)
        Canvas.SetTop(MovingBar, 0D)
        Counter.Text = "0"
        Counter.Focusable = False
        Counter.Cursor = Cursors.Arrow
    End Sub
    Public Property Number As Integer
        Get
            Dim i As Integer = 0
            If Integer.TryParse(Counter.Text, i) Then

            End If
            Return i
        End Get
        Set(value As Integer)
            Counter.Text = value.ToString
        End Set
    End Property
    Public Property Stroke As Brush
        Get
            Return MovingBar.Stroke
        End Get
        Set(value As Brush)
            MovingBar.Stroke = value
        End Set
    End Property
End Class

<Shallow()>
Public Class CopyPasteData
    <Save()> Public IDTypes As New ShallowDictionary(Of String, String)
    <Save()> Public Status As New Status
End Class