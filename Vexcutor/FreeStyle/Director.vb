Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes, System.Windows.Media.Animation
<Shallow()>
Public Class DesignButton
    Inherits ImageSwitch
    Public Sub New()
        MyBase.New(New Dictionary(Of Object, String) From {{"Run", IconImages.Run}, {"Design", IconImages.Design}}, 48)
    End Sub
End Class
<Shallow()>
Public Class Director
    Inherits System.Windows.Controls.TreeView
    Private _Crew As New Crew
    Private _Storyline As New Storyline

    Public Sub New()
        Items.Add(_Crew)
        Items.Add(_Storyline)
        BorderThickness = New Thickness(0)
    End Sub
    <Save()> Public Property Crew As Crew
        Get
            Return _Crew
        End Get
        Set(value As Crew)
            If TypeOf value Is Crew Then
                Items.Clear()
                _Crew = value
                Items.Add(_Crew)
            End If
        End Set
    End Property
    <Save()> Public Property Storyline As Storyline
        Get
            Return _Storyline
        End Get
        Set(value As Storyline)
            If TypeOf value Is Storyline Then
                Items.Clear()
                _Storyline = value
                Items.Add(_Storyline)
            End If
        End Set
    End Property
    Private _Host As FreeStage
    <EarlyBind("Stage")> Public Property Host As FreeStage
        Get
            Return _Host
        End Get
        Set(value As FreeStage)
            _Host = value
        End Set
    End Property
    Public Function Play() As Boolean
        Dim NextScene As Scene = Storyline.TryGetNextScene()
        If TypeOf NextScene Is Scene Then
            NextScene.Play()
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub PlayAgain()
        Reset()
        'Dim NextScene As Scene = Storyline.TryGetNextScene()
        'If TypeOf NextScene Is Scene Then
        '    NextScene.Play()
        '    Return True
        'Else
        '    Return False
        'End If
    End Sub
    Private _SelectedScene As Scene
    Public ReadOnly Property CurrentSelectScene()
        Get
            Return _SelectedScene
        End Get
    End Property
    Public Sub ResetPlayer()
        Storyline.SelectedScene = Nothing
    End Sub
    Public Sub Reset()
        For Each it As Story In _Storyline.Items
            If it.Items.Count > 0 Then
                Dim sc As Scene = it.Items(0)
                sc.Show()
                Exit For
            End If
        Next
        For Each it As Story In _Storyline.Items
            it.Reset()
        Next
        _SelectedScene = Nothing
        _Storyline.SelectedScene = Nothing
    End Sub
    Public RelatedPropertGrid As PropertyGridX

    Public Function ViewProperty(obj As Object)
        If RelatedPropertGrid.OK Then RelatedPropertGrid.Load(obj)
    End Function
End Class
<Shallow()>
Public Class Crew
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Public txtName As New System.Windows.Controls.Label
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Content = "Crew"
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Staff, 24, 24))
        gdHost.AddColumnItem(txtName)
        Background = Brushes.Transparent
    End Sub
    Private Sub AddActor(vActor As IActor)
        Dim sc As New Actor
        sc.RelatedActor = vActor
        Items.Add(sc)
        ExpandSubtree()
    End Sub
    <Save()> Public Property Actors As ShallowList(Of Actor)
        Get
            Dim sl As New ShallowList(Of Actor)
            For Each it As Actor In Items
                sl.Add(it)
            Next
            Return sl
        End Get
        Set(value As ShallowList(Of Actor))
            If TypeOf value Is ShallowList(Of Actor) Then
                Items.Clear()
                For Each it As Actor In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property
    Public Shadows ReadOnly Property Parent As Director
        Get
            Return MyBase.Parent
        End Get
    End Property
End Class
<Shallow()>
Public Class Actor
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Public WithEvents btnVisible As New IdeaButton With {.IsCheckable = True}
    Public WithEvents txtName As New EditBox
    Public WithEvents lblClose As New DeleteButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "Actor"
        gdHost.AddColumnItem(btnVisible)
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(lblClose)
        Background = Brushes.Transparent
        AddHandler lblClose.Click, Me.ClickRemove(Sub() _Actor.Remove())
    End Sub
    Private _Actor As IActor
    <Save()> Public Property RelatedActor As IActor
        Get
            Return _Actor
        End Get
        Set(value As IActor)
            _Actor = value
            txtName.Text = value.ID
        End Set
    End Property
    Public Event OnActivate As RoutedEventHandler
    Protected Overrides Sub OnPreviewMouseDown(e As System.Windows.Input.MouseButtonEventArgs)
        Me.Background = Brushes.LightYellow
        RaiseEvent OnActivate(Me, Nothing)
        MyBase.OnPreviewMouseDown(e)
    End Sub
    Public Shadows ReadOnly Property Parent As Crew
        Get
            Return MyBase.Parent
        End Get
    End Property
    Public Sub Activate(sender As Object, e As EventArgs)
        Parent.Parent.ViewProperty(_Actor)
        If sender Is Me Then Exit Sub
        txtName.ActivateColor()
    End Sub
    Public Sub Deactivate()
        txtName.DeactivateColor()
    End Sub

    Private Sub txtName_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles txtName.GotFocus
        _Actor.Activate(Me, Nothing)
    End Sub
    Private Sub txtName_LostFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles txtName.LostFocus
        _Actor.Deactivate()
    End Sub

    Private Sub btnVisible_CheckChanged(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnVisible.CheckChanged
        _Actor.Visible = IIf(btnVisible.Checked, 1D, 0D)
    End Sub
End Class
<Shallow()>
Public Class Storyline
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Public txtName As New System.Windows.Controls.Label
    Public lblAdd As New AddButton
    'Public btnSave As New SaveButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Content = "Storyline"
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Storyline, 24, 24))
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(lblAdd)
        'gdHost.AddColumnItem(btnSave)
        Background = Brushes.Transparent
        AddHandler lblAdd.Click, Click.ClickAdd(Me, Function() New Story)
    End Sub

    Public Sub WriteGroup(sd As ShallowDictionary(Of String, IActor))
        For Each st As Story In Items
            For Each sc As Scene In st.Items
                sc.RewriteGroup(sd)
                For Each ac As Action In sc.Items
                    For Each sa As Status In ac.Items
                        sa.RewriteGroup(sd)
                    Next
                Next
            Next
        Next
    End Sub
    Public SelectedStatusCollection As New List(Of IAnimationState)
    Public Sub SelectStatus(ias As IAnimationState, adding As Boolean)
        If adding Then
            SelectedStatusCollection.Add(ias)
        Else
            SelectedStatusCollection.Clear()
            SelectedStatusCollection.Add(ias)
        End If
        UpdateStatusSelection()
    End Sub
    Public Sub SelectStatusWithSubStatus(sc As Scene)
        SelectedStatusCollection.Add(sc)
        For Each ac As Action In sc.Items
            For Each sa As Status In ac.Items
                SelectedStatusCollection.Add(sa)
            Next
        Next
        UpdateStatusSelection()
    End Sub
    Public Sub SelectStatusWithSubStatus(st As Story)
        For Each sc As Scene In st.Items
            SelectedStatusCollection.Add(sc)
            For Each ac As Action In sc.Items
                For Each sa As Status In ac.Items
                    SelectedStatusCollection.Add(sa)
                Next
            Next
        Next
        UpdateStatusSelection()
    End Sub
    Public Sub UpdateStatusSelection()
        Dim sel As Boolean
        Dim all As Boolean
        For Each st As Story In Items
            For Each sc As Scene In st.Items
                sc.Selected1 = SelectedStatusCollection.Contains(sc)
                all = True
                For Each ac As Action In sc.Items
                    For Each sa As Status In ac.Items
                        sel = SelectedStatusCollection.Contains(sa)
                        sa.Selected1 = sel
                        all = all And sel
                    Next
                Next
                sc.DragDropBar.Fill = IIf(all, Brushes.HotPink, Brushes.Yellow)
            Next
        Next
    End Sub
    Public Sub WriteSelected(sd As ShallowDictionary(Of String, IActor), ParamArray ActTypes As AnimationTypeEnum())
        For Each IAS As IAnimationState In SelectedStatusCollection
            IAS.ReWriteType(sd, ActTypes)
        Next
    End Sub

    Public ReadOnly Property Host As FreeStage
        Get
            Return Parent.Host
        End Get
    End Property
    Public Shadows ReadOnly Property Parent As Director
        Get
            Return MyBase.Parent
        End Get
    End Property
    Public Sub StopAll()
        For Each it As Story In Items
            it.StopAll()
        Next
    End Sub
    <Save()> Public Property Stories As ShallowList(Of Story)
        Get
            Dim sl As New ShallowList(Of Story)
            For Each it As Story In Items
                sl.Add(it)
            Next
            Return sl
        End Get
        Set(value As ShallowList(Of Story))
            If TypeOf value Is ShallowList(Of Story) Then
                StopAll()
                Items.Clear()
                For Each it As Story In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property
    Public SelectedScene As Scene
    Public Reseted As Boolean = True
    Public Sub SelectScene(sc As Scene)
        SelectedScene = sc
    End Sub
    Public Function TryGetScene() As Scene
        If SelectedScene Is Nothing Then
            If Items.Count > 0 Then
                For Each st As Story In Items
                    SelectedScene = st.TryGetScene
                    If SelectedScene IsNot Nothing Then Return SelectedScene
                Next
                Return Nothing
            Else
                Return Nothing
            End If
        Else
            Return SelectedScene
        End If
    End Function
    Public Function TryGetNextScene() As Scene
        If SelectedScene.OK Then SelectedScene.OnStop()

        If Reseted OrElse SelectedScene Is Nothing Then
            Reseted = False
            If Items.Count > 0 Then
                For Each st As Story In Items
                    SelectedScene = st.TryGetNextScene(SelectedScene)
                    If SelectedScene IsNot Nothing Then Return SelectedScene
                Next
                Return Nothing
            Else
                Return Nothing
            End If
        Else
            If Items.Count > 0 Then
                Dim FoundButNext As Boolean = False
                For Each st As Story In Items
                    If st.Items.Contains(SelectedScene) Then
                        Dim i As Integer = st.Items.IndexOf(SelectedScene)
                        If i < st.Items.Count - 1 Then
                            SelectedScene = st.Items(i + 1)
                            Return SelectedScene
                        Else
                            FoundButNext = True
                        End If
                    Else
                        'try search next
                        If FoundButNext Then
                            If st.Items.Count > 0 Then
                                SelectedScene = st.Items(0)
                                Return SelectedScene
                            Else
                                FoundButNext = True
                            End If
                        Else
                            'do nothing 
                        End If
                    End If
                Next
                If FoundButNext Then SelectedScene = Nothing
                Return Nothing
            Else
                Return Nothing
            End If
        End If
    End Function
End Class

<Shallow()>
Public Class Story
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Public WithEvents DragDropBar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.YellowGreen, .AllowDrop = True}
    Public txtName As New EditBox
    Public WithEvents lblAdd As New AddButton
    Public WithEvents btnDelete As New DeleteButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "Animation"
        gdHost.AddColumnItem(DragDropBar)
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(lblAdd)
        'gdHost.AddColumnItem(btnSave)
        gdHost.AddColumnItem(btnDelete)
        Background = Brushes.Transparent
        'AddHandler btnDelete.Click, ClickRemove()
    End Sub

    Private Sub lblClose_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If MsgBox("Do you really want to remove this Story?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Parent.Items.Remove(Me)
        End If
    End Sub
    <Save()> Public Property Scenes As ShallowList(Of Scene)
        Get
            Dim sl As New ShallowList(Of Scene)
            For Each it As Scene In Items
                sl.Add(it)
            Next
            Return sl
        End Get
        Set(value As ShallowList(Of Scene))
            If TypeOf value Is ShallowList(Of Scene) Then
                Items.Clear()
                For Each it As Scene In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property
    <Save()> Public Property StoryName As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    Public ReadOnly Property Host As FreeStage
        Get
            Return Parent.Host
        End Get
    End Property
    Public Shadows ReadOnly Property Parent As Storyline
        Get
            Return MyBase.Parent
        End Get
    End Property

    Private Sub lblAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblAdd.Click
        Dim s As New Scene
        Items.Add(s)
        s.Record()
    End Sub

    Public SelectedScene As Scene
    Public Sub SelectScene(sc As Scene)
        SelectedScene = sc
        Parent.SelectScene(sc)
    End Sub
    Public Sub Reset()
        SelectedScene = Nothing
    End Sub
    Public Function TryGetScene() As Scene
        If SelectedScene Is Nothing Then
            If Items.Count > 0 Then
                SelectedScene = Items(0)
                Return SelectedScene
            Else
                Return Nothing
            End If
        Else
            Return SelectedScene
        End If
    End Function
    Public Function TryGetNextScene() As Scene
        If SelectedScene Is Nothing Then
            If Items.Count > 0 Then
                SelectedScene = Items(0)
                Return SelectedScene
            Else
                Return Nothing
            End If
        Else
            Dim i As Integer = Items.IndexOf(SelectedScene)
            If i < Items.Count - 1 Then
                SelectedScene = Items(i + 1)
                Return SelectedScene
            Else
                SelectedScene = Nothing
                Return Nothing
            End If
        End If
    End Function
    Public Function TryGetNextScene(sc As Scene) As Scene
        If sc Is Nothing Then
            If Items.Count > 0 Then
                SelectedScene = Items(0)
                Return SelectedScene
            Else
                Return Nothing
            End If
        ElseIf Items.Contains(sc) Then
            Dim i As Integer = Items.IndexOf(sc)
            If i < Items.Count - 1 Then
                SelectedScene = Items(i + 1)
                Return SelectedScene
            Else
                SelectedScene = Nothing
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
    Public Sub StopAll()
        For Each sc As Scene In Items
            sc.OnStop()
        Next
    End Sub

    Private PrepareDrag As Boolean = False

    Private Sub DragDropBar_DragEnter(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.DragEnter
        If TypeOf e.Data.GetData("StorySource") Is Story AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub
    Private Sub DragDropBar_Drop(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.Drop
        If TypeOf e.Data.GetData("StorySource") Is Story AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
            Dim st As Story = e.Data.GetData("StorySource")
            st.PrepareDrag = False
            'frmAnimation.Text = st.StoryName
            Dim j As Integer = Parent.Items.IndexOf(Me)
            Dim p As Storyline = Parent
            st.Parent.Items.Remove(st)
            p.Items.Insert(j, st)
        End If
    End Sub
    Private Sub DragDropBar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseLeftButtonDown
        PrepareDrag = True
    End Sub
    Private Sub DragDropBar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles DragDropBar.MouseMove
        If PrepareDrag Then
            Dim dataObj As New DataObject("MoveStory")
            dataObj.SetData("StorySource", Me)
            DragDrop.DoDragDrop(Me, dataObj, DragDropEffects.Move)
        End If
    End Sub

    Private Sub DragDropBar_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseRightButtonDown
        Parent.SelectStatusWithSubStatus(Me)
    End Sub
End Class

<Shallow()>
Public Class Scene

    Inherits System.Windows.Controls.TreeViewItem
    Implements IAnimationState


    Private gdHost As New GridBase
    Public WithEvents DragDropBar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.Yellow, .AllowDrop = True}
    Public WithEvents lblShow As New IdeaButton
    Public txtName As New EditBox
    Public WithEvents lblRecord As New ImageButton(IconImages.Load, 24)
    Public WithEvents lblPlay As New PlayButton
    Public WithEvents lblStop As New StopButton
    Public WithEvents lblAdd As New AddButton
    Public WithEvents lblClose As New DeleteButton
    Protected WithEvents Animator As New System.Windows.Forms.Timer With {.Interval = 10}
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "Scene"
        gdHost.AddColumnItem(DragDropBar)
        gdHost.AddColumnItem(lblShow)
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(lblRecord)
        gdHost.AddColumnItem(lblPlay)
        gdHost.AddColumnItem(lblStop)
        gdHost.AddColumnItem(lblAdd)
        gdHost.AddColumnItem(lblClose)
        'AddHandler lblClose.Click, ClickRemove()
    End Sub

    '<Save()> Public Property Data As Byte()
    <Save()> Public BeginStatus As New Status
    '<Save()> Public Property ActorList As New List(Of String)
    Public Sub Record()
        'Only Load the Visible Actors on the stage
        _Actors.Clear()
        For Each act In Host.Actors
            If act.Visible > 0D Then
                _Actors.Add(act.ID, act)
            End If
        Next
        BeginStatus.Record(_Actors)
        '_Data = ShallowSerializer.Serialize(Host.Actors)
    End Sub
    <Save()> Public Property SceneName As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    Public Sub Show()
        Parent.SelectScene(Me)
        Dim x = Dispatcher.DisableProcessing
        'For Each act In Host.Actors
        '    act.Visible = IIf(_Actors.ContainsValue(act), 1D, 0D)
        'Next
        BeginStatus.Present(_Actors, AnimationTypeEnum.Brush)
        BeginStatus.Present(_Actors, AnimationTypeEnum.Effect)
        BeginStatus.Present(_Actors, AnimationTypeEnum.Movement)
        BeginStatus.Present(_Actors, AnimationTypeEnum.Text)
        For Each act As Action In Items
            act.Show()
        Next
        x.Dispose()
    End Sub
    Private Sub lblAdd_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles lblAdd.MouseLeftButtonDown
        AddAction()
    End Sub
    'Private Sub lblClose_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles lblClose.MouseLeftButtonDown
    '    DirectCast(Parent, Story).Items.Remove(Me)
    'End Sub
    Private Sub AddAction()
        Dim act As New Action
        Items.Add(act)
        ExpandSubtree()
    End Sub
    <Save()> Public Property Scenes As ShallowList(Of Action)
        Get
            Dim sl As New ShallowList(Of Action)
            For Each it As Action In Items
                sl.Add(it)
            Next
            Return sl
        End Get
        Set(value As ShallowList(Of Action))
            If TypeOf value Is ShallowList(Of Action) Then
                Items.Clear()
                For Each it As Action In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property

    Public ReadOnly Property Host As FreeStage
        Get
            Return Parent.Host
        End Get
    End Property
    Private _Actors As New ShallowDictionary(Of String, IActor)
    <LateLoad()> Public Property Actors As ShallowDictionary(Of String, IActor)
        Get
            Return _Actors
        End Get
        Set(value As ShallowDictionary(Of String, IActor))
            _Actors = value
        End Set
    End Property
    Public Shadows ReadOnly Property Parent As Story
        Get
            Return MyBase.Parent
        End Get
    End Property

    Private Sub Tick(sender As Object, e As EventArgs) Handles Animator.Tick
        Dim t As DateTime = Now
        Dim offset As Double = (t - StartTime).TotalSeconds
        Dim x = Dispatcher.DisableProcessing
        Play(offset)
        x.Dispose()
    End Sub
    Private StoppableActions As New List(Of Action)
    Private RunningActions As New List(Of Action)
    Private StoppedActions As New List(Of Action)
    Private StartTime As DateTime
    Public Sub PrepareQueue()
        StoppableActions.Clear()
        RunningActions.Clear()
        StoppedActions.Clear()
        For Each it As Action In Items
            Select Case it.RunType
                Case RunTypeEnum.SingleRun, RunTypeEnum.BackRun
                    StoppableActions.Add(it)
                    RunningActions.Add(it)
                    it.PrepareQueue()
                Case RunTypeEnum.CycleRun
                    RunningActions.Add(it)
                    it.PrepareQueue()
            End Select
        Next
    End Sub
    Public Sub Play()
        Show()
        PrepareQueue()
        StartTime = DateTime.Now
        Animator.Enabled = True
    End Sub
    Public Sub Play(t As Double)
        If StoppedActions.Count > 0 Then
            For Each it As Action In StoppedActions
                If RunningActions.Contains(it) Then RunningActions.Remove(it)
                If StoppableActions.Contains(it) Then StoppableActions.Remove(it)
            Next
        End If
        For Each it As Action In RunningActions
            it.Play(t)
        Next
        If RunningActions.Count = 0 Then
            Animator.Enabled = False
        End If
        If StoppableActions.Count = 0 Then
            WhenAllStop()
        End If
    End Sub
    Public Sub WhenAllStop()

    End Sub
    Public Sub StopSubItem(it As Action)
        StoppedActions.Clear()
        StoppedActions.Add(it)
    End Sub
    Public Sub OnStop()
        Animator.Enabled = False
        RunningActions.Clear()
        StoppableActions.Clear()
    End Sub
    Private Sub lblPlay_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblPlay.Click
        Play()
    End Sub
    Private Sub lblStop_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblStop.Click
        OnStop()
    End Sub

    Private Sub lblRecord_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblRecord.Click
        Record()
    End Sub

    Private Sub lblShow_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblShow.Click
        Show()
    End Sub

    Private PrepareDrag As Boolean = False

    Private Sub DragDropBar_DragEnter(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.DragEnter
        If TypeOf e.Data.GetData("StorySource") Is Story AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub
    Private Sub DragDropBar_Drop(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.Drop
        If TypeOf e.Data.GetData("StorySource") Is Story AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
            Dim sc As Scene = e.Data.GetData("StorySource")
            sc.PrepareDrag = False
            'frmAnimation.Text = sc.SceneName
            Dim j As Integer = Parent.Items.IndexOf(Me)
            Dim p As Story = Parent
            sc.Parent.Items.Remove(sc)
            p.Items.Insert(j, sc)
        End If
    End Sub
    Private Sub DragDropBar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseLeftButtonDown
        PrepareDrag = True
    End Sub
    Private Sub DragDropBar_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseRightButtonDown
        If Input.Keyboard.Modifiers = ModifierKeys.Control Then
            Parent.Parent.SelectStatusWithSubStatus(Me)
            'DragDropBar.Fill = Brushes.HotPink
        Else
            Parent.Parent.SelectStatus(Me, Input.Keyboard.Modifiers = ModifierKeys.Shift)
        End If
    End Sub
    Private Sub DragDropBar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles DragDropBar.MouseMove
        If PrepareDrag Then
            Dim dataObj As New DataObject("MoveStory")
            dataObj.SetData("StorySource", Me)
            DragDrop.DoDragDrop(Me, dataObj, DragDropEffects.Move)
        End If
    End Sub

    Private Sub lblClose_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblClose.Click
        If MsgBox("Do you really want to remove this Scene?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Parent.Items.Remove(Me)
        End If
    End Sub
    Public Sub RewriteGroup(sd As ShallowDictionary(Of String, IActor))
        BeginStatus.ReWrite(sd)
    End Sub

    Public Property ActorBrushs As ShallowDictionary(Of String, ActorBrush) Implements IAnimationState.ActorBrushs
        Get
            Return BeginStatus.ActorBrushs
        End Get
        Set(value As ShallowDictionary(Of String, ActorBrush))
            BeginStatus.ActorBrushs = value
        End Set
    End Property

    Public Property ActorEffects As ShallowDictionary(Of String, ActorEffect) Implements IAnimationState.ActorEffects
        Get
            Return BeginStatus.ActorEffects
        End Get
        Set(value As ShallowDictionary(Of String, ActorEffect))
            BeginStatus.ActorEffects = value
        End Set
    End Property

    Public Property ActorTexts As ShallowDictionary(Of String, ActorText) Implements IAnimationState.ActorTexts
        Get
            Return BeginStatus.ActorTexts
        End Get
        Set(value As ShallowDictionary(Of String, ActorText))
            BeginStatus.ActorTexts = value
        End Set
    End Property

    Public Property ActorValues As ShallowDictionary(Of String, ActorValue) Implements IAnimationState.ActorValues
        Get
            Return BeginStatus.ActorValues
        End Get
        Set(value As ShallowDictionary(Of String, ActorValue))
            BeginStatus.ActorValues = value
        End Set
    End Property

    Public Property ActorVectors As ShallowDictionary(Of String, ActorVector) Implements IAnimationState.ActorVectors
        Get
            Return BeginStatus.ActorVectors
        End Get
        Set(value As ShallowDictionary(Of String, ActorVector))
            BeginStatus.ActorVectors = value
        End Set
    End Property

    Public Property Peroid As Double Implements IAnimationState.Peroid
        Get
            Return BeginStatus.Peroid
        End Get
        Set(value As Double)
            BeginStatus.Peroid = value
        End Set
    End Property

    Public Property Selected1 As Boolean Implements IAnimationState.Selected
        Get
            Return TypeOf DragDropBar.Stroke Is SolidColorBrush AndAlso DirectCast(DragDropBar.Stroke, SolidColorBrush).Color = Colors.Red
        End Get
        Set(value As Boolean)
            DragDropBar.Stroke = IIf(value, Brushes.Red, Brushes.Transparent)
        End Set
    End Property
End Class

<Shallow()>
Public Class Action '定义一个场景 所有的对象进行序列化
    Inherits System.Windows.Controls.TreeViewItem
    'Implements IAnimationState
    Private gdHost As New GridBase
    Public txtName As New EditBox
    'Public nbLength As New NumberBox
    'Public WithEvents lblRecord As New ImageButton(IconImages.Load, 24)
    Public WithEvents lblShow As New IdeaButton
    Public WithEvents lblAdd As New AddButton
    Public WithEvents lblClose As New DeleteButton
    Public WithEvents lblAnimationType As New ImageSwitch(New Dictionary(Of Object, String) From {{AnimationTypeEnum.Movement, IconImages.Movement},
                      {AnimationTypeEnum.Brush, IconImages.Color}, {AnimationTypeEnum.Value, IconImages.Visible},
                      {AnimationTypeEnum.Effect, IconImages.Effect}, {AnimationTypeEnum.Text, IconImages.Text},
                       {AnimationTypeEnum.Property, IconImages.Property}}, 24)
    Public WithEvents lblRunType As New ImageSwitch(New Dictionary(Of Object, String) From {{RunTypeEnum.SingleRun, IconImages.SingleRun}, {RunTypeEnum.BackRun, IconImages.BackRun}, {RunTypeEnum.CycleRun, IconImages.CycleRun}, {RunTypeEnum.NoRun, IconImages.Clock}}, 24)
    Public WithEvents lblPlay As New PlayButton
    Public WithEvents lblStop As New StopButton
    Public WithEvents Animator As New Timer With {.Interval = 10}
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "Action"
        gdHost.AddColumnItem(lblShow)
        gdHost.AddColumnItem(txtName)
        'gdHost.AddColumnItem(nbLength)
        'gdHost.AddColumnItem(lblRecord)
        gdHost.AddColumnItem(lblAnimationType)
        gdHost.AddColumnItem(lblRunType)
        gdHost.AddColumnItem(lblPlay)
        gdHost.AddColumnItem(lblStop)
        gdHost.AddColumnItem(lblAdd)
        gdHost.AddColumnItem(lblClose)
        'nbLength.Value = 1D
    End Sub
    <Save()> Public Property ActionName As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    <Save()> Public Property Statuses As ShallowList(Of Status)
        Get
            Dim sl As New ShallowList(Of Status)
            For Each it As Status In Items
                sl.Add(it)
            Next
            Return sl
        End Get
        Set(value As ShallowList(Of Status))
            If TypeOf value Is ShallowList(Of Status) Then
                Items.Clear()
                For Each it As Status In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property
    '<Save()> Public Property States As New ShallowDictionary(Of String, ShallowDictionary(Of String, Object))
    Public Sub Show()
        If Me.Items.Count > 0 Then
            CType(Items(0), IAnimationState).Present(Actors, AnimationType)
        End If
        'Present(Actors, AnimationType)
    End Sub

    ''' <summary>
    ''' 如果是自己的计时器在运动，则停止自己。否则从父节点的动画队列中删除自己。
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnStop()
        If Animator.Enabled Then
            Animator.Enabled = False
        Else
            Parent.StopSubItem(Me)
        End If
    End Sub
    Public Sub PrepareQueue()
        AnimationItems.Clear()
        For Each it As IAnimationState In Items
            AnimationItems.Add(it)
        Next
    End Sub
    Public Sub Play()
        PrepareQueue()
        StartTime = Now
        Play(0)
        Animator.Enabled = True
    End Sub
    Private StartTime As DateTime
    Private Sub Tick(obj As Object, arg As EventArgs) Handles Animator.Tick
        Dim t As DateTime = Now
        Dim x = Dispatcher.DisableProcessing
        Play((t - StartTime).TotalSeconds)
        x.Dispose()
    End Sub
    Private AnimationItems As New List(Of IAnimationState)
    Public ReadOnly Property Item(vIndex As Integer) As IAnimationState
        Get
            Return AnimationItems(vIndex)
        End Get
    End Property
    Public Sub Play(t As Double)
        If AnimationItems.Count = 0 Then
            OnStop()
            Exit Sub
        ElseIf AnimationItems.Count = 1 Then
            AnimationItems(0).Present(Actors, AnimationType)
            OnStop()
            Exit Sub
        End If
        Dim running As Boolean = False
        Dim pr As Double
        Dim iB As Integer
        Dim iE As Integer
        'frmAnimation.Text = t.ToString
        Select Case RunType
            Case RunTypeEnum.SingleRun
                iB = -1
                For i As Integer = 0 To AnimationItems.Count - 1
                    pr = Item(i).Peroid
                    If t > pr Then
                        t -= pr
                    Else
                        iB = i
                        If i < AnimationItems.Count Then
                            running = True
                            iE = iB + 1
                        End If
                        Exit For
                    End If
                Next
                If iE >= AnimationItems.Count Or iB < 0 Then
                    iE = AnimationItems.Count - 1
                    iB = iE - 1
                    t = 0
                    running = False
                End If
            Case RunTypeEnum.BackRun
                'Dim fetched As Boolean = False
                '0,1,2,3,4
                '0,1,2,1,0
                Dim h As Integer = AnimationItems.Count - 1 '2 
                Dim l As Integer = 2 * h '5
                Dim j As Integer = 0
                iB = -1
                For i As Integer = 0 To l
                    If i > h Then
                        j = l - i
                    Else
                        j = i
                    End If
                    pr = Item(j).Peroid
                    If t > pr Then
                        t -= pr
                    Else
                        running = True
                        iB = j
                        If i < h Then
                            iE = iB + 1
                        Else
                            iE = iB - 1
                        End If
                        'fetched = True
                        Exit For
                    End If
                Next
                If iE < 0 Or iB < 0 Then
                    iE = 0
                    iB = 1
                    t = 0
                    running = False
                End If
                'If iE >= AnimationItems.Count Or iE < 0 Then
                '    iE = iB
                '    t = 0
                '    running = False
                'End If
            Case RunTypeEnum.CycleRun
                Dim i As Integer = 0
                pr = Item(i).Peroid
                While t > pr
                    t -= pr
                    i += 1
                    If i = AnimationItems.Count Then i = 0
                    pr = Item(i).Peroid
                End While
                iB = i
                iE = (iB + 1) Mod AnimationItems.Count
                If i = AnimationItems.Count Then iE = 0
                running = True
            Case RunTypeEnum.NoRun
                'do nothing
        End Select
        If running Then
            Select Case AnimationType
                Case AnimationTypeEnum.Value
                    Dim vB As Double, vE As Double
                    For Each actKey As String In Item(iB).ActorValues.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorValues(actKey).Keys
                                vB = Item(iB).ActorValues(actKey)(vKey)
                                vE = Item(iE).ActorValues(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, (vB * (pr - t) + vE * t) / pr)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Movement
                    Dim vB As System.Windows.Vector, vE As System.Windows.Vector
                    For Each actKey As String In Item(iB).ActorVectors.Keys
                        If Item(iE).ActorVectors.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorVectors(actKey).Keys
                                vB = Item(iB).ActorVectors(actKey)(vKey)
                                vE = Item(iE).ActorVectors(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, CType((vB * (pr - t) + vE * t) / pr, System.Windows.Point))
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Brush
                    Dim vB As BrushVector, vE As BrushVector
                    For Each actKey As String In Item(iB).ActorBrushs.Keys
                        If Item(iE).ActorBrushs.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorBrushs(actKey).Keys
                                vB = Item(iB).ActorBrushs(actKey)(vKey)
                                vE = Item(iE).ActorBrushs(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, ((vB * (pr - t) + vE * t) / pr).GetValue)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Effect
                    Dim vB As EffectVector, vE As EffectVector
                    For Each actKey As String In Item(iB).ActorEffects.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorEffects(actKey).Keys
                                vB = Item(iB).ActorEffects(actKey)(vKey)
                                vE = Item(iE).ActorEffects(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, ((vB * (pr - t) + vE * t) / pr).GetValue)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Text
                    Dim vB As String, vE As String
                    For Each actKey As String In Item(iB).ActorEffects.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorTexts(actKey).Keys
                                vB = Item(iB).ActorTexts(actKey)(vKey)
                                vE = Item(iE).ActorTexts(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, StringVector.AnimateString(vB, vE, t / pr))
                                End If
                            Next
                        End If
                    Next
            End Select
        Else
            'render the last position
            Select Case AnimationType
                Case AnimationTypeEnum.Value
                    Dim vB As Double, vE As Double
                    For Each actKey As String In Item(iB).ActorValues.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorValues(actKey).Keys
                                vB = Item(iB).ActorValues(actKey)(vKey)
                                vE = Item(iE).ActorValues(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, vE)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Movement
                    Dim vB As System.Windows.Vector, vE As System.Windows.Vector
                    For Each actKey As String In Item(iB).ActorVectors.Keys
                        If Item(iE).ActorVectors.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorVectors(actKey).Keys
                                vB = Item(iB).ActorVectors(actKey)(vKey)
                                vE = Item(iE).ActorVectors(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, CType(vE, System.Windows.Point))
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Brush
                    Dim vB As BrushVector, vE As BrushVector
                    For Each actKey As String In Item(iB).ActorBrushs.Keys
                        If Item(iE).ActorBrushs.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorBrushs(actKey).Keys
                                vB = Item(iB).ActorBrushs(actKey)(vKey)
                                vE = Item(iE).ActorBrushs(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, vE.GetValue)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Effect
                    Dim vB As EffectVector, vE As EffectVector
                    For Each actKey As String In Item(iB).ActorEffects.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorEffects(actKey).Keys
                                vB = Item(iB).ActorEffects(actKey)(vKey)
                                vE = Item(iE).ActorEffects(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, vE.GetValue)
                                End If
                            Next
                        End If
                    Next
                Case AnimationTypeEnum.Text
                    Dim vB As String, vE As String
                    For Each actKey As String In Item(iB).ActorEffects.Keys
                        If Item(iE).ActorValues.ContainsKey(actKey) Then
                            For Each vKey As String In Item(iB).ActorTexts(actKey).Keys
                                vB = Item(iB).ActorTexts(actKey)(vKey)
                                vE = Item(iE).ActorTexts(actKey)(vKey)
                                If vB <> vE Then
                                    Actors(actKey).SetProperty(vKey, vE)
                                End If
                            Next
                        End If
                    Next
            End Select
            OnStop()
            Exit Sub
        End If
    End Sub
    <Save()> Public Property RunType As RunTypeEnum
        Get
            Return lblRunType.ImageKey
        End Get
        Set(value As RunTypeEnum)
            lblRunType.ImageKey = value
        End Set
    End Property
    Private Sub lblAdd_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles lblAdd.MouseLeftButtonDown
        AddStatus()
    End Sub
    Private Sub AddStatus()
        Dim st As New Status
        Items.Add(st)
        st.Record()
        ExpandSubtree()
    End Sub
    Public ReadOnly Property Host As FreeStage
        Get
            Return Parent.Host
        End Get
    End Property
    Public ReadOnly Property Actors As ShallowDictionary(Of String, IActor)
        Get
            If TypeOf Parent Is Scene Then
                Return DirectCast(Parent, Scene).Actors
            Else
                Return Nothing
            End If
        End Get
    End Property
    <Save()> Public Property AnimationType As AnimationTypeEnum
        Get
            Return lblAnimationType.ImageKey
        End Get
        Set(value As AnimationTypeEnum)
            lblAnimationType.ImageKey = value
        End Set
    End Property

    Public Shadows ReadOnly Property Parent As Scene
        Get
            Return MyBase.Parent
        End Get
    End Property
    Private Sub lblClose_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblClose.Click
        Parent.Items.Remove(Me)
    End Sub
    Private Sub lblShow_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblShow.Click
        Show()
    End Sub
    Private Sub lblPlay_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblPlay.Click
        Play()
    End Sub

    Private Sub lblStop_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblStop.Click
        OnStop()
    End Sub
End Class

<Shallow()>
Public Class Status '需要记录当前所有场景
    Inherits System.Windows.Controls.TreeViewItem
    Implements IAnimationState


    Private gdHost As New GridBase
    Public WithEvents DragDropBar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.Yellow, .AllowDrop = True}
    Public txtName As New EditBox
    Public WithEvents lblRecord As New ImageButton(IconImages.Load, 24)
    Public WithEvents lblShow As New IdeaButton
    Public nbLength As New NumberBox
    Public WithEvents lblMoveColor As New System.Windows.Controls.Label
    Public WithEvents lblClose As New DeleteButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "Status"
        gdHost.AddColumnItem(DragDropBar)
        gdHost.AddColumnItem(lblShow)
        gdHost.AddColumnItem(txtName)
        nbLength.AllowDecimal = True
        nbLength.AllowNegative = False
        nbLength.Value = 1D
        gdHost.AddColumnItem(nbLength)
        gdHost.AddColumnItem(lblRecord)
        gdHost.AddColumnItem(lblClose)
    End Sub
    <Save()> Public Property StatusName As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property

    <Save()> Public Property States As New ShallowDictionary(Of String, ShallowDictionary(Of String, Object))

    Private Shared Empty As Object() = New Object() {}

    Public Sub RewriteGroup(sd As ShallowDictionary(Of String, IActor))
        ReWrite(sd)
    End Sub
    Public ReadOnly Property AnimationType As AnimationTypeEnum
        Get
            If TypeOf Parent Is Action Then
                Return DirectCast(Parent, Action).AnimationType
            Else
                Return AnimationTypeEnum.Movement
            End If
        End Get
    End Property
    Public ReadOnly Property Actors As ShallowDictionary(Of String, IActor)
        Get
            If TypeOf Parent Is Action Then
                Return DirectCast(Parent, Action).Actors
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub Show()
        Present(Actors, Parent.AnimationType)
    End Sub

    Public ReadOnly Property Host As FreeStage
        Get
            Return Parent.Host
        End Get
    End Property
    Private Sub lblRecord_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles lblRecord.MouseLeftButtonDown
        Record()
    End Sub
    Public Sub Record()
        Dim vActors As ShallowDictionary(Of String, IActor) = Actors
        For Each act As IActor In Host.Actors
            vActors.Bind(act)
        Next
        Record(Actors)
    End Sub
    <Save()> Public Property ActorBrushs As New ShallowDictionary(Of String, ActorBrush) Implements IAnimationState.ActorBrushs
    <Save()> Public Property ActorEffects As New ShallowDictionary(Of String, ActorEffect) Implements IAnimationState.ActorEffects
    <Save()> Public Property ActorValues As New ShallowDictionary(Of String, ActorValue) Implements IAnimationState.ActorValues
    <Save()> Public Property ActorVectors As New ShallowDictionary(Of String, ActorVector) Implements IAnimationState.ActorVectors
    <Save()> Public Property ActorTexts As New ShallowDictionary(Of String, ActorText) Implements IAnimationState.ActorTexts
    Public Shadows ReadOnly Property Parent As Action
        Get
            Return MyBase.Parent
        End Get
    End Property
    <Save()> Public Property Peroid As Double Implements IAnimationState.Peroid
        Get
            Return nbLength.Value
        End Get
        Set(value As Double)
            nbLength.Value = value
        End Set
    End Property

    Private Sub lblClose_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblClose.Click
        Parent.Items.Remove(Me)
    End Sub
    Private Sub lblShow_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblShow.Click
        Show()
    End Sub

    Public Property Selected1 As Boolean Implements IAnimationState.Selected
        Get
            Return TypeOf DragDropBar.Stroke Is SolidColorBrush AndAlso DirectCast(DragDropBar.Stroke, SolidColorBrush).Color = Colors.Red
        End Get
        Set(value As Boolean)
            DragDropBar.Stroke = IIf(value, Brushes.Red, Brushes.Transparent)
        End Set
    End Property
    Private PrepareDrag As Boolean = False

    Private Sub DragDropBar_DragEnter(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.DragEnter
        If TypeOf e.Data.GetData("StatusSource") Is Status AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub
    Private Sub DragDropBar_Drop(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.Drop
        If TypeOf e.Data.GetData("StatusSource") Is Status AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
            Dim sc As Status = e.Data.GetData("StatusSource")
            sc.PrepareDrag = False
            Dim j As Integer = Parent.Items.IndexOf(Me)
            Dim p As Action = Parent
            sc.Parent.Items.Remove(sc)
            p.Items.Insert(j, sc)
        End If
    End Sub
    Private Sub DragDropBar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseLeftButtonDown
        PrepareDrag = True
    End Sub
    Private Sub DragDropBar_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseRightButtonDown
        Parent.Parent.Parent.Parent.SelectStatus(Me, Input.Keyboard.Modifiers = ModifierKeys.Shift)
    End Sub
    Private Sub DragDropBar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles DragDropBar.MouseMove
        If PrepareDrag Then
            Dim dataObj As New DataObject("MoveStatus")
            dataObj.SetData("StatusSource", Me)
            DragDrop.DoDragDrop(Me, dataObj, DragDropEffects.Move)
        End If
    End Sub
End Class
Public Enum RunTypeEnum
    NoRun
    SingleRun
    CycleRun
    BackRun
End Enum
Public Enum AnimationTypeEnum
    Movement
    Brush
    Value
    Effect
    Text
    Switch
    [Property]
    All
End Enum

Public Module IActorMethods
    Private Empty0 As Object = New Object() {}
    Private Empty1 As Object = New Object() {Nothing}
    Private ReadOnly Property Empty(vIndex As Integer) As Object()
        Get
            Select Case vIndex
                Case 0
                    Return Empty0
                Case 1
                    Return Empty1
                Case Else
                    Return Empty0
            End Select
        End Get
    End Property
    Private BrushType As Type = GetType(System.Windows.Media.Brush)
    Private EffectType As Type = GetType(System.Windows.Media.Effects.Effect)
    Private PointType As Type = GetType(System.Windows.Point)
    Private VectorType As Type = GetType(System.Windows.Vector)
    Private DoubleType As Type = GetType(Double)
    Private StringType As Type = GetType(String)
    Private BooleanType As Type = GetType(Boolean)
    <System.Runtime.CompilerServices.Extension()> Public Function AnimationType(vType As Type) As AnimationTypeEnum
        If vType.IsBrush Then
            Return AnimationTypeEnum.Brush
        ElseIf vType.IsEffect Then
            Return AnimationTypeEnum.Effect
        ElseIf vType.IsString Then
            Return AnimationTypeEnum.Text
        ElseIf vType.IsDouble Then
            Return AnimationTypeEnum.Value
        ElseIf vType.IsPoint Then
            Return AnimationTypeEnum.Movement
        ElseIf vType.IsBoolean Then
            Return AnimationTypeEnum.Switch
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsBrush(vType As Type) As Boolean
        Return vType.IsAssignableFrom(BrushType)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsEffect(vType As Type) As Boolean
        Return vType.IsAssignableFrom(EffectType)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsPoint(vType As Type) As Boolean
        Return vType Is PointType
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsVector(vType As Type) As Boolean
        Return vType Is VectorType
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsDouble(vType As Type) As Boolean
        Return vType Is DoubleType
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsString(vType As Type) As Boolean
        Return vType Is StringType
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function IsBoolean(vType As Type) As Boolean
        Return vType Is BooleanType
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub Record(state As IAnimationState, Actors As ShallowDictionary(Of String, IActor))
        Dim act As Object
        state.ActorVectors.Clear()
        state.ActorBrushs.Clear()
        state.ActorEffects.Clear()
        state.ActorValues.Clear()
        state.ActorTexts.Clear()
        For Each actkey As String In Actors.Keys
            act = Actors(actkey)
            Dim vt As Type = act.GetType
            For Each pi As System.Reflection.PropertyInfo In vt.GetProperties(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
                If pi.PropertyType Is PointType AndAlso IsAct(pi, act) Then
                    If Not state.ActorVectors.ContainsKey(actkey) Then state.ActorVectors.Add(actkey, New ActorVector)
                    state.ActorVectors(actkey).Add(pi.Name, V(DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), System.Windows.Point)))
                ElseIf pi.PropertyType Is DoubleType AndAlso IsAct(pi, act) Then
                    If Not state.ActorValues.ContainsKey(actkey) Then state.ActorValues.Add(actkey, New ActorValue)
                    state.ActorValues(actkey).Add(pi.Name, DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), Double))
                ElseIf pi.PropertyType.IsAssignableFrom(BrushType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorBrushs.ContainsKey(actkey) Then state.ActorBrushs.Add(actkey, New ActorBrush)
                    state.ActorBrushs(actkey).Add(pi.Name, New BrushVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count))))
                ElseIf pi.PropertyType.IsAssignableFrom(EffectType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorEffects.ContainsKey(actkey) Then state.ActorEffects.Add(actkey, New ActorEffect)
                    state.ActorEffects(actkey).Add(pi.Name, New EffectVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count))))
                ElseIf pi.PropertyType Is StringType Then
                    If Not state.ActorTexts.ContainsKey(actkey) Then state.ActorTexts.Add(actkey, New ActorText)
                    state.ActorTexts(actkey).Add(pi.Name, pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                End If
            Next
        Next
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub ReWrite(state As IAnimationState, Actors As ShallowDictionary(Of String, IActor))
        Dim act As Object
        'state.ActorVectors.Clear()
        'state.ActorBrushs.Clear()
        'state.ActorEffects.Clear()
        'state.ActorValues.Clear()
        'state.ActorTexts.Clear()
        For Each actkey As String In Actors.Keys
            act = Actors(actkey)
            Dim vt As Type = act.GetType
            For Each pi As System.Reflection.PropertyInfo In vt.GetProperties(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
                If pi.PropertyType Is PointType AndAlso IsAct(pi, act) Then
                    If Not state.ActorVectors.ContainsKey(actkey) Then state.ActorVectors.Add(actkey, New ActorVector)
                    If state.ActorVectors(actkey).ContainsKey(pi.Name) Then
                        state.ActorVectors(actkey)(pi.Name) = V(DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), System.Windows.Point))
                    Else
                        state.ActorVectors(actkey).Add(pi.Name, V(DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), System.Windows.Point)))
                    End If
                ElseIf pi.PropertyType Is DoubleType AndAlso IsAct(pi, act) Then
                    If Not state.ActorValues.ContainsKey(actkey) Then state.ActorValues.Add(actkey, New ActorValue)
                    If state.ActorValues(actkey).ContainsKey(pi.Name) Then
                        state.ActorValues(actkey)(pi.Name) = DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), Double)
                    Else
                        state.ActorValues(actkey).Add(pi.Name, DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), Double))
                    End If
                ElseIf pi.PropertyType.IsAssignableFrom(BrushType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorBrushs.ContainsKey(actkey) Then state.ActorBrushs.Add(actkey, New ActorBrush)
                    If state.ActorBrushs(actkey).ContainsKey(pi.Name) Then
                        state.ActorBrushs(actkey)(pi.Name) = New BrushVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    Else
                        state.ActorBrushs(actkey)(pi.Name) = New BrushVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    End If

                ElseIf pi.PropertyType.IsAssignableFrom(EffectType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorEffects.ContainsKey(actkey) Then state.ActorEffects.Add(actkey, New ActorEffect)
                    If state.ActorEffects(actkey).ContainsKey(pi.Name) Then
                        state.ActorEffects(actkey)(pi.Name) = New EffectVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    Else
                        state.ActorEffects(actkey).Add(pi.Name, New EffectVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count))))
                    End If

                ElseIf pi.PropertyType Is StringType Then
                    If Not state.ActorTexts.ContainsKey(actkey) Then state.ActorTexts.Add(actkey, New ActorText)
                    If state.ActorTexts(actkey).ContainsKey(pi.Name) Then
                        state.ActorTexts(actkey)(pi.Name) = pi.GetValue(act, Empty(pi.GetIndexParameters.Count))
                    Else
                        state.ActorTexts(actkey).Add(pi.Name, pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    End If

                End If
            Next
        Next
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub ReWriteType(state As IAnimationState, Actors As ShallowDictionary(Of String, IActor), ParamArray AniTypes As AnimationTypeEnum())
        Dim act As Object
        For Each actkey As String In Actors.Keys
            act = Actors(actkey)
            Dim vt As Type = act.GetType
            For Each pi As System.Reflection.PropertyInfo In vt.GetProperties(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
                If AniTypes.Contains(AnimationTypeEnum.Movement) AndAlso pi.PropertyType Is PointType AndAlso IsAct(pi, act) Then
                    If Not state.ActorVectors.ContainsKey(actkey) Then state.ActorVectors.Add(actkey, New ActorVector)
                    If state.ActorVectors(actkey).ContainsKey(pi.Name) Then
                        state.ActorVectors(actkey)(pi.Name) = V(DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), System.Windows.Point))
                    Else
                        state.ActorVectors(actkey).Add(pi.Name, V(DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), System.Windows.Point)))
                    End If
                ElseIf AniTypes.Contains(AnimationTypeEnum.Value) AndAlso pi.PropertyType Is DoubleType AndAlso IsAct(pi, act) Then
                    If Not state.ActorValues.ContainsKey(actkey) Then state.ActorValues.Add(actkey, New ActorValue)
                    If state.ActorValues(actkey).ContainsKey(pi.Name) Then
                        state.ActorValues(actkey)(pi.Name) = DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), Double)
                    Else
                        state.ActorValues(actkey).Add(pi.Name, DirectCast(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)), Double))
                    End If
                ElseIf AniTypes.Contains(AnimationTypeEnum.Brush) AndAlso pi.PropertyType.IsAssignableFrom(BrushType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorBrushs.ContainsKey(actkey) Then state.ActorBrushs.Add(actkey, New ActorBrush)
                    If state.ActorBrushs(actkey).ContainsKey(pi.Name) Then
                        state.ActorBrushs(actkey)(pi.Name) = New BrushVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    Else
                        state.ActorBrushs(actkey)(pi.Name) = New BrushVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    End If

                ElseIf AniTypes.Contains(AnimationTypeEnum.Effect) AndAlso pi.PropertyType.IsAssignableFrom(EffectType) AndAlso IsAct(pi, act) Then
                    If Not state.ActorEffects.ContainsKey(actkey) Then state.ActorEffects.Add(actkey, New ActorEffect)
                    If state.ActorEffects(actkey).ContainsKey(pi.Name) Then
                        state.ActorEffects(actkey)(pi.Name) = New EffectVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    Else
                        state.ActorEffects(actkey).Add(pi.Name, New EffectVector(pi.GetValue(act, Empty(pi.GetIndexParameters.Count))))
                    End If

                ElseIf AniTypes.Contains(AnimationTypeEnum.Text) AndAlso pi.PropertyType Is StringType Then
                    If Not state.ActorTexts.ContainsKey(actkey) Then state.ActorTexts.Add(actkey, New ActorText)
                    If state.ActorTexts(actkey).ContainsKey(pi.Name) Then
                        state.ActorTexts(actkey)(pi.Name) = pi.GetValue(act, Empty(pi.GetIndexParameters.Count))
                    Else
                        state.ActorTexts(actkey).Add(pi.Name, pi.GetValue(act, Empty(pi.GetIndexParameters.Count)))
                    End If

                End If
            Next
        Next
    End Sub

    <System.Runtime.CompilerServices.Extension()> Public Sub Present(state As IAnimationState, Actors As ShallowDictionary(Of String, IActor), AnimationType As AnimationTypeEnum)
        Dim act As Object

        Select Case AnimationType
            Case AnimationTypeEnum.Value
                For Each actkey As String In state.ActorValues.Keys
                    If Actors.ContainsKey(actkey) Then
                        act = Actors(actkey)
                        For Each pKey As String In state.ActorValues(actkey).Keys
                            SetProperty(act, pKey, state.ActorValues(actkey)(pKey))
                        Next
                    End If
                Next
            Case AnimationTypeEnum.Brush
                For Each actkey As String In state.ActorBrushs.Keys
                    If Actors.ContainsKey(actkey) Then
                        act = Actors(actkey)
                        For Each pKey As String In state.ActorBrushs(actkey).Keys
                            SetProperty(act, pKey, state.ActorBrushs(actkey)(pKey).GetValue)
                        Next
                    End If
                Next
            Case AnimationTypeEnum.Movement
                For Each actkey As String In state.ActorVectors.Keys
                    If Actors.ContainsKey(actkey) Then
                        act = Actors(actkey)
                        For Each pKey As String In state.ActorVectors(actkey).Keys
                            SetProperty(act, pKey, CType(state.ActorVectors(actkey)(pKey), System.Windows.Point))
                        Next
                    End If
                Next
            Case AnimationTypeEnum.Effect
                For Each actkey As String In state.ActorEffects.Keys
                    If Actors.ContainsKey(actkey) Then
                        act = Actors(actkey)
                        For Each pKey As String In state.ActorEffects(actkey).Keys
                            SetProperty(act, pKey, state.ActorEffects(actkey)(pKey).GetValue)
                        Next
                    End If
                Next
            Case AnimationTypeEnum.Text
                For Each actkey As String In state.ActorTexts.Keys
                    If Actors.ContainsKey(actkey) Then
                        act = Actors(actkey)
                        For Each pKey As String In state.ActorTexts(actkey).Keys
                            SetProperty(act, pKey, state.ActorTexts(actkey)(pKey))
                        Next
                    End If
                Next
        End Select
    End Sub
    Private TypeInfos As New Dictionary(Of Type, Dictionary(Of String, System.Reflection.PropertyInfo))
    <System.Runtime.CompilerServices.Extension()> Function GetProperty(Of T)(obj As T, propertyName As String) As Object
        Dim vT As Type = GetType(T)
        If Not TypeInfos.ContainsKey(vT) Then TypeInfos.Add(vT, New Dictionary(Of String, System.Reflection.PropertyInfo))
        Dim pi As System.Reflection.PropertyInfo
        If Not TypeInfos(vT).ContainsKey(propertyName) Then
            pi = vT.GetProperty(propertyName)
            TypeInfos(vT).Add(propertyName, pi)
        Else
            pi = TypeInfos(vT)(propertyName)
        End If
        Return pi.GetValue(obj, Empty(pi.GetIndexParameters.Count))
    End Function
    <System.Runtime.CompilerServices.Extension()> Sub SetProperty(obj As Object, propertyName As String, value As Object)
        Dim vT As Type = obj.GetType
        If Not TypeInfos.ContainsKey(vT) Then TypeInfos.Add(vT, New Dictionary(Of String, System.Reflection.PropertyInfo))
        Dim pi As System.Reflection.PropertyInfo
        If Not TypeInfos(vT).ContainsKey(propertyName) Then
            pi = vT.GetProperty(propertyName)
            TypeInfos(vT).Add(propertyName, pi)
        Else
            pi = TypeInfos(vT)(propertyName)
        End If
        TypeInfos(vT)(propertyName).SetValue(obj, value, Empty(pi.GetIndexParameters.Count))
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function IsAct(pi As System.Reflection.PropertyInfo) As Boolean
        For Each att As Attribute In pi.GetCustomAttributes(True)
            If TypeOf att Is ActAttribute Then
                Dim ActAtt As ActAttribute = att
                Return True
            End If
        Next
        Return False
    End Function
    Private Function IsAct(pi As System.Reflection.PropertyInfo, ByRef vActor As IActor) As Boolean
        For Each att As Attribute In pi.GetCustomAttributes(True)
            If TypeOf att Is ActAttribute Then
                Dim ActAtt As ActAttribute = att
                If ActAtt.BindName IsNot Nothing Then
                    Return vActor.Adorners(ActAtt.BindName).DependentTarget Is Nothing
                Else
                    Return True
                End If
            End If
        Next
        Return False
    End Function
End Module

'Inherits ShallowDictionary(Of String, IActor)
Public Module ActorCollection
    <System.Runtime.CompilerServices.Extension()> Public Function Bind(host As ShallowDictionary(Of String, IActor), obj As IActor) As String
        If host.ContainsValue(obj) Then
            Return host.GetKey(obj)
        Else
            Dim key As String = obj.ID
            host.Add(key, obj)
            Return key
        End If
    End Function
End Module
Public Interface IAnimation
End Interface
Public Interface IAnimationState
    Property Peroid As Double
    Property ActorValues As ShallowDictionary(Of String, ActorValue)
    Property ActorVectors As ShallowDictionary(Of String, ActorVector)
    Property ActorBrushs As ShallowDictionary(Of String, ActorBrush)
    Property ActorEffects As ShallowDictionary(Of String, ActorEffect)
    Property ActorTexts As ShallowDictionary(Of String, ActorText)
    Property Selected As Boolean
End Interface
<Serializable()>
Public Class ActorValue
    Inherits Dictionary(Of String, Double)
    Implements System.Runtime.Serialization.ISerializable
    Sub New()
    End Sub
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.New()
        Dim cnt As Integer = info.GetInt32("Count")
        For i As Integer = 0 To cnt - 1
            Add(info.GetString("K" + i.ToString), info.GetDouble("V" + i.ToString))
        Next
    End Sub
    Public Shadows Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Count", MyBase.Count)
        For i As Integer = 0 To MyBase.Count - 1
            info.AddValue("K" + i.ToString, Keys(i))
            info.AddValue("V" + i.ToString, Values(i))
        Next
    End Sub
End Class
<Serializable()>
Public Class ActorVector
    Inherits Dictionary(Of String, System.Windows.Vector)
    Implements System.Runtime.Serialization.ISerializable
    Sub New()
    End Sub
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.New()
        Dim cnt As Integer = info.GetInt32("Count")
        For i As Integer = 0 To cnt - 1
            Add(info.GetValue("K" + i.ToString, GetType(String)), info.GetValue("V" + i.ToString, GetType(Vector)))
        Next
    End Sub
    Public Shadows Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Count", MyBase.Count)
        For i As Integer = 0 To MyBase.Count - 1
            info.AddValue("K" + i.ToString, Keys(i))
            info.AddValue("V" + i.ToString, Values(i))
        Next
    End Sub
End Class
<Serializable()>
Public Class ActorBrush
    Inherits Dictionary(Of String, BrushVector)
    Implements System.Runtime.Serialization.ISerializable
    Sub New()
    End Sub
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.New()
        Dim cnt As Integer = info.GetInt32("Count")
        For i As Integer = 0 To cnt - 1
            Add(info.GetValue("K" + i.ToString, GetType(String)), info.GetValue("V" + i.ToString, GetType(BrushVector)))
        Next
    End Sub
    Public Shadows Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Count", MyBase.Count)
        For i As Integer = 0 To MyBase.Count - 1
            info.AddValue("K" + i.ToString, Keys(i))
            info.AddValue("V" + i.ToString, Values(i))
        Next
    End Sub
End Class
<Serializable()>
Public Class ActorEffect
    Inherits Dictionary(Of String, EffectVector)
    Implements System.Runtime.Serialization.ISerializable
    Sub New()
    End Sub
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.New()
        Dim cnt As Integer = info.GetInt32("Count")
        For i As Integer = 0 To cnt - 1
            Add(info.GetValue("K" + i.ToString, GetType(String)), info.GetValue("V" + i.ToString, GetType(EffectVector)))
        Next
    End Sub
    Public Shadows Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Count", MyBase.Count)
        For i As Integer = 0 To MyBase.Count - 1
            info.AddValue("K" + i.ToString, Keys(i))
            info.AddValue("V" + i.ToString, Values(i))
        Next
    End Sub
End Class
<Serializable()>
Public Class ActorText
    Inherits Dictionary(Of String, String)
    Implements System.Runtime.Serialization.ISerializable
    Sub New()
    End Sub
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.New()
        Dim cnt As Integer = info.GetInt32("Count")
        For i As Integer = 0 To cnt - 1
            Add(info.GetString("K" + i.ToString), info.GetString("V" + i.ToString))
        Next
    End Sub
    Public Shadows Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Count", MyBase.Count)
        For i As Integer = 0 To MyBase.Count - 1
            info.AddValue("K" + i.ToString, Keys(i))
            info.AddValue("V" + i.ToString, Values(i))
        Next
    End Sub
End Class
<Serializable()>
Public Class StringVector
    Public Shared Function AnimateString(V1 As String, V2 As String, weight As Double) As String
        'Dim stb As New System.Text.StringBuilder
        Dim s1 As String = IIf(V1 Is Nothing, "", V1)
        Dim s2 As String = IIf(V2 Is Nothing, "", V2)
        If weight < 0D Then weight = 0D
        If weight > 1D Then weight = 1D
        Dim l As Integer = s2.Length * weight
        Dim r As Integer = s1.Length * weight
        Return s2.Substring(0, l) + s1.Substring(r)
    End Function
End Class

Public Module ImageBrushRegister
    Public RegistedBrushes As New Dictionary(Of ImageBrush, Double)
    <System.Runtime.CompilerServices.Extension()> Public Sub Register(imgb As ImageBrush, Key As Double)

    End Sub
End Module

<Serializable()>
Public Class BrushVector
    Implements System.Runtime.Serialization.ISerializable
    Public Shared ImageSources As New Dictionary(Of Double, StreamImage)

    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        Name = info.GetString("Name")
        Count = info.GetInt32("Count")
        Values = info.GetValue("Values", GetType(Dictionary(Of String, Object)))
    End Sub
    Public Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Name", Name)
        info.AddValue("Count", Count)
        info.AddValue("Values", Values)
    End Sub
    Public Name As String = ""
    Public Count As Integer = 0
    Public Values As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

    <NonSerialized()> Private Shared Empty As Object() = New Object() {}
    Public Sub New()
    End Sub
    Public Sub New(obj As Object)
        If obj Is Nothing Then
            Name = "Nothing"
            Exit Sub
        End If
        Dim vT As Type = obj.GetType
        Name = vT.Name
        Select Case vT.Name
            Case "SolidColorBrush"
                Values.Add("Color", New ColorVector(DirectCast(obj, SolidColorBrush)))
            Case "LinearGradientBrush"
                Dim lgb As LinearGradientBrush = obj
                For i As Integer = 0 To lgb.GradientStops.Count - 1
                    Values.Add("GS" + i.ToString, New GradientVector(lgb.GradientStops(i)))
                Next
                Count = lgb.GradientStops.Count
            Case "RadialGradientBrush"
                Dim lgb As RadialGradientBrush = obj
                For i As Integer = 0 To lgb.GradientStops.Count - 1
                    Values.Add("GS" + i.ToString, New GradientVector(lgb.GradientStops(i)))
                Next
                Count = lgb.GradientStops.Count
            Case "ImageBrush"
                Dim lgb As ImageBrush = obj
                If ImageSources IsNot Nothing Then
                    Dim kv As KeyValuePair(Of Double, StreamImage) = ImageSources.FirstOf(Function(kvp As KeyValuePair(Of Double, StreamImage)) kvp.Value.ImageSource Is lgb.ImageSource)
                    If kv.OK Then
                        Values.Add("Key", kv.Key)
                    Else
                        Values.Add("Key", 0D)
                    End If
                Else
                    Values.Add("Key", 0D)
                End If
                Values.Add("Stretch", CDbl(lgb.Stretch))
                Values.Add("TileMode", CDbl(lgb.TileMode))
        End Select
        For Each sp As String In ComplexMapping(Name)
            Dim pi As System.Reflection.PropertyInfo = vT.GetProperty(sp)
            Values.Add(sp, pi.GetValue(obj, Empty))
        Next
    End Sub
    Public Function GetValue() As Object
        Dim obj As Object
        Select Case Name
            Case "Nothing"
                Return Nothing
            Case "SolidColorBrush"
                Dim cv As ColorVector = Values("Color")
                obj = New SolidColorBrush(Color.FromArgb(ColorVector.Normal(cv.A), ColorVector.Normal(cv.R), ColorVector.Normal(cv.G), ColorVector.Normal(cv.B)))
            Case "LinearGradientBrush"
                obj = New LinearGradientBrush With {.MappingMode = BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = GradientSpreadMethod.Pad}
                Dim colors As New List(Of GradientVector)
                For Each gsv As Object In Values.Values
                    If TypeOf gsv Is GradientVector Then
                        colors.Add(gsv)
                    End If
                Next
                Dim lgb As LinearGradientBrush = obj
                colors.Sort()
                For Each gsv As GradientVector In colors
                    lgb.GradientStops.Add(CType(gsv, GradientStop))
                Next
            Case "RadialGradientBrush"
                obj = New RadialGradientBrush With {.MappingMode = BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = GradientSpreadMethod.Pad}
                Dim colors As New List(Of GradientVector)
                For Each gsv As Object In Values.Values
                    If TypeOf gsv Is GradientVector Then
                        colors.Add(gsv)
                    End If
                Next
                Dim lgb As RadialGradientBrush = obj
                colors.Sort()
                For Each gsv As GradientVector In colors
                    lgb.GradientStops.Add(CType(gsv, GradientStop))
                Next
            Case "ImageBrush"
                Dim ib As New ImageBrush
                obj = ib
                If ImageSources IsNot Nothing AndAlso ImageSources.ContainsKey(Values("Key")) Then
                    ib.ImageSource = ImageSources(Values("Key")).GetImageSource
                End If
                ib.Stretch = CInt(Values("Stretch"))
                ib.TileMode = CInt(Values("TileMode"))
            Case Else
                obj = TypeCreate(Name)
        End Select
        If obj IsNot Nothing Then
            Dim vT As Type = obj.GetType
            For Each sp As String In ComplexMapping(Name)
                Dim pi As System.Reflection.PropertyInfo = vT.GetProperty(sp)
                pi.SetValue(obj, PackVector(Values(sp)), Empty)
            Next
        End If
        Return obj
    End Function
    Public Shared Function Inequals(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As GradientVector, V2 As GradientVector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As ColorVector, V2 As ColorVector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As Double, V2 As Double) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Sum(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As System.Windows.Vector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As GradientVector, V2 As GradientVector) As GradientVector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As ColorVector, V2 As ColorVector) As ColorVector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As Double, V2 As Double) As Double
        Return V1 + V2
    End Function
    Public Shared Function Subtract(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As System.Windows.Vector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As GradientVector, V2 As GradientVector) As GradientVector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As ColorVector, V2 As ColorVector) As ColorVector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As Double, V2 As Double) As Double
        Return V1 - V2
    End Function
    Public Shared Function Multiply(V1 As System.Windows.Vector, d As Double) As System.Windows.Vector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As GradientVector, d As Double) As GradientVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As ColorVector, d As Double) As ColorVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As Double, d As Double) As Double
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As System.Windows.Vector) As System.Windows.Vector
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As GradientVector) As GradientVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As ColorVector) As ColorVector
        Return V1 * d
    End Function
    Public Shared Function Divide(V1 As System.Windows.Vector, d As Double) As System.Windows.Vector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As GradientVector, d As Double) As GradientVector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As ColorVector, d As Double) As ColorVector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As Double, d As Double) As Double
        Return V1 / d
    End Function
    Public Shared Function TypeCreate(TypeName As String) As Object
        Select Case TypeName
            Case "LinearGradientBrush"
                Return New LinearGradientBrush
            Case "RadialGradientBrush"
                Return New RadialGradientBrush
            Case "ImageBrush"
                Return New ImageBrush
            Case "BlurEffect"
                Return New Effects.BlurEffect
            Case "DropShadowEffect"
                Return New Effects.DropShadowEffect
            Case "BloomEffect"
                Return New Microsoft.Expression.Media.Effects.BloomEffect
            Case "RippleEffect"
                Return New Microsoft.Expression.Media.Effects.RippleEffect
            Case "PixelateEffect"
                Return New Microsoft.Expression.Media.Effects.PixelateEffect
            Case Else
                Return Nothing
        End Select
    End Function
    Public Shared Function ComplexMapping(TypeName As String) As String()
        Select Case TypeName
            Case "LinearGradientBrush"
                Return New String() {"StartPoint", "EndPoint"}
            Case "RadialGradientBrush"
                Return New String() {"GradientOrigin", "Center", "RadiusX", "RadiusY"}
            Case "ImageBrush"
                Return New String() {}
            Case "BlurEffect"
                Return New String() {"Radius"}
            Case "DropShadowEffect"
                Return New String() {"BlurRadius", "Direction", "Opacity", "Color"}
            Case "BloomEffect"
                Return New String() {"BaseIntensity", "BloomIntensity", "BaseSaturation", "BloomSaturation", "Threshold"}
            Case "RippleEffect"
                Return New String() {"Center", "Frequency", "Magnitude", "Phase"}
            Case "PixelateEffect"
                Return New String() {"Pixelation"}
            Case Else
                Return New String() {}
        End Select
    End Function
    Public Shared Function MappingType(value As Double) As Double
        Return value
    End Function
    Public Shared Function MappingType(value As System.Windows.Vector) As System.Windows.Vector
        Return value
    End Function
    Public Shared Function MappingType(value As Color) As ColorVector
        Return New ColorVector(value)
    End Function
    Public Shared Function MappingType(value As GradientStop) As GradientVector
        Return New GradientVector(value)
    End Function
    Public Shared Operator +(cv1 As BrushVector, cv2 As BrushVector) As BrushVector
        If cv1.Name = cv2.Name Then
            Dim cv As New BrushVector With {.Name = cv1.Name}
            Dim cn As Integer = Math.Min(cv1.Values.Count, cv2.Values.Count)
            cv.Count = cn
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                cv.Values.Add(Key, Sum(cv1.Values(Key), cv2.Values(Key)))
                i += 1
                If i = cn Then Exit For
            Next
            Return cv
        Else
            Return Nothing
        End If
    End Operator
    Public Shared Operator -(cv1 As BrushVector, cv2 As BrushVector) As BrushVector
        If cv1.Name = cv2.Name Then
            Dim cv As New BrushVector With {.Name = cv1.Name}
            Dim cn As Integer = Math.Min(cv1.Values.Count, cv2.Values.Count)
            cv.Count = cn
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                cv.Values.Add(Key, Subtract(cv1.Values(Key), cv2.Values(Key)))
                i += 1
                If i = cn Then Exit For
            Next
            Return cv
        Else
            Return Nothing
        End If
    End Operator
    Public Shared Operator *(cv1 As BrushVector, cv2 As Double) As BrushVector
        Dim cv As New BrushVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Multiply(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator *(cv2 As Double, cv1 As BrushVector) As BrushVector
        Dim cv As New BrushVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Multiply(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator /(cv1 As BrushVector, cv2 As Double) As BrushVector
        Dim cv As New BrushVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Divide(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator =(cv1 As BrushVector, cv2 As BrushVector) As Boolean
        If cv1.Name = cv2.Name AndAlso cv1.Count = cv2.Count Then
            'Dim cv As New BrushVector
            If cv1.Values.Count <> cv2.Values.Count Then Return False
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                If Inequals(cv1.Values(Key), cv2.Values(Key)) Then Return False
            Next
            Return True
        Else
            Return False
        End If
    End Operator
    Public Shared Operator <>(cv1 As BrushVector, cv2 As BrushVector) As Boolean
        Return Not (cv1 = cv2)
    End Operator

End Class
<Serializable()>
Public Class EffectVector
    Implements System.Runtime.Serialization.ISerializable
    Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        Name = info.GetString("Name")
        Count = info.GetInt32("Count")
        Values = info.GetValue("Values", GetType(Dictionary(Of String, Object)))
    End Sub
    Public Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        info.AddValue("Name", Name)
        info.AddValue("Count", Count)
        info.AddValue("Values", Values)
    End Sub
    Public Name As String = ""
    Public Count As Integer = 0
    Public Values As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
    <NonSerialized()> Private Shared Empty As Object() = New Object() {}
    Public Sub New()
    End Sub
    Public Sub New(obj As Object)
        If obj Is Nothing Then
            Name = "Nothing"
            Exit Sub
        End If
        Dim vT As Type = obj.GetType
        Name = vT.Name
        Select Case vT.Name
            Case "SolidColorBrush"
                Values.Add("Color", New ColorVector(DirectCast(obj, SolidColorBrush)))
            Case "LinearGradientBrush"
                Dim lgb As LinearGradientBrush = obj
                For i As Integer = 0 To lgb.GradientStops.Count - 1
                    Values.Add("GS" + i.ToString, New GradientVector(lgb.GradientStops(i)))
                Next
                Count = lgb.GradientStops.Count
            Case "RadialGradientBrush"
                Dim lgb As RadialGradientBrush = obj
                For i As Integer = 0 To lgb.GradientStops.Count - 1
                    Values.Add("GS" + i.ToString, New GradientVector(lgb.GradientStops(i)))
                Next
                Count = lgb.GradientStops.Count
        End Select
        For Each sp As String In ComplexMapping(Name)
            Dim pi As System.Reflection.PropertyInfo = vT.GetProperty(sp)
            Values.Add(sp, PackVector(pi.GetValue(obj, Empty)))
        Next
    End Sub
    Public Function GetValue() As Object
        Dim obj As Object
        Select Case Name
            Case "Nothing"
                Return Nothing
            Case "SolidColorBrush"
                obj = New SolidColorBrush(Values("Color"))
            Case "LinearGradientBrush"
                obj = New LinearGradientBrush With {.MappingMode = BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = GradientSpreadMethod.Pad}
                Dim colors As New List(Of GradientVector)
                For Each gsv As Object In Values.Values
                    If TypeOf gsv Is GradientVector Then
                        colors.Add(gsv)
                    End If
                Next
                Dim lgb As LinearGradientBrush = obj
                colors.Sort()
                For Each gsv As GradientVector In colors
                    lgb.GradientStops.Add(CType(gsv, GradientStop))
                Next
            Case "RadialGradientBrush"
                obj = New RadialGradientBrush With {.MappingMode = BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = GradientSpreadMethod.Pad}
                Dim colors As New List(Of GradientVector)
                For Each gsv As Object In Values.Values
                    If TypeOf gsv Is GradientVector Then
                        colors.Add(gsv)
                    End If
                Next
                Dim lgb As RadialGradientBrush = obj
                colors.Sort()
                For Each gsv As GradientVector In colors
                    lgb.GradientStops.Add(CType(gsv, GradientStop))
                Next
            Case Else
                obj = TypeCreate(Name)
        End Select
        If obj IsNot Nothing Then
            Dim vT As Type = obj.GetType
            For Each sp As String In ComplexMapping(Name)
                Dim pi As System.Reflection.PropertyInfo = vT.GetProperty(sp)
                pi.SetValue(obj, UnPackVector(Values(sp)), Empty)
            Next
        End If
        Return obj
    End Function
    Public Shared Function Inequals(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As GradientVector, V2 As GradientVector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As ColorVector, V2 As ColorVector) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Inequals(V1 As Double, V2 As Double) As Boolean
        Return V1 <> V2
    End Function
    Public Shared Function Sum(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As System.Windows.Vector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As GradientVector, V2 As GradientVector) As GradientVector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As ColorVector, V2 As ColorVector) As ColorVector
        Return V1 + V2
    End Function
    Public Shared Function Sum(V1 As Double, V2 As Double) As Double
        Return V1 + V2
    End Function
    Public Shared Function Subtract(V1 As System.Windows.Vector, V2 As System.Windows.Vector) As System.Windows.Vector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As GradientVector, V2 As GradientVector) As GradientVector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As ColorVector, V2 As ColorVector) As ColorVector
        Return V1 - V2
    End Function
    Public Shared Function Subtract(V1 As Double, V2 As Double) As Double
        Return V1 - V2
    End Function
    Public Shared Function Multiply(V1 As System.Windows.Vector, d As Double) As System.Windows.Vector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As GradientVector, d As Double) As GradientVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As ColorVector, d As Double) As ColorVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(V1 As Double, d As Double) As Double
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As System.Windows.Vector) As System.Windows.Vector
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As GradientVector) As GradientVector
        Return V1 * d
    End Function
    Public Shared Function Multiply(d As Double, V1 As ColorVector) As ColorVector
        Return V1 * d
    End Function
    Public Shared Function Divide(V1 As System.Windows.Vector, d As Double) As System.Windows.Vector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As GradientVector, d As Double) As GradientVector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As ColorVector, d As Double) As ColorVector
        Return V1 / d
    End Function
    Public Shared Function Divide(V1 As Double, d As Double) As Double
        Return V1 / d
    End Function
    Public Shared Function TypeCreate(TypeName As String) As Object
        Select Case TypeName
            Case "LinearGradientBrush"
                Return New LinearGradientBrush
            Case "RadialGradientBrush"
                Return New RadialGradientBrush
            Case "BlurEffect"
                Return New Effects.BlurEffect
            Case "DropShadowEffect"
                Return New Effects.DropShadowEffect
            Case "BloomEffect"
                Return New Microsoft.Expression.Media.Effects.BloomEffect
            Case "RippleEffect"
                Return New Microsoft.Expression.Media.Effects.RippleEffect
            Case "PixelateEffect"
                Return New Microsoft.Expression.Media.Effects.PixelateEffect
            Case "SwirlEffect"
                Return New Microsoft.Expression.Media.Effects.SwirlEffect
            Case "ColorToneEffect"
                Return New Microsoft.Expression.Media.Effects.ColorToneEffect
            Case "MonochromeEffect"
                Return New Microsoft.Expression.Media.Effects.MonochromeEffect
            Case "SharpenEffect"
                Return New Microsoft.Expression.Media.Effects.SharpenEffect
            Case Else
                Return Nothing
        End Select
    End Function
    Public Shared Function ComplexMapping(TypeName As String) As String()
        'Add( )
        'Add( )
        'Add( )
        'Add( )
        'Add( )
        'Add( )
        'Add(ColorTone)
        'Add(MonoChrome)
        'Add(Sharpen)
        Select Case TypeName
            Case "LinearGradientBrush"
                Return New String() {"StartPoint", "EndPoint"}
            Case "RadialGradientBrush"
                Return New String() {"GradientOrigin", "Center", "RadiusX", "RadiusY"}
            Case "BlurEffect"
                Return New String() {"Radius"}
            Case "DropShadowEffect"
                Return New String() {"BlurRadius", "Direction", "Opacity", "Color"}
            Case "BloomEffect"
                Return New String() {"BaseIntensity", "BloomIntensity", "BaseSaturation", "BloomSaturation", "Threshold"}
            Case "RippleEffect"
                Return New String() {"Center", "Frequency", "Magnitude", "Phase"}
            Case "PixelateEffect"
                Return New String() {"Pixelation"}
            Case "SwirlEffect"
                Return New String() {"Center", "TwistAmount"}
            Case "ColorToneEffect"
                Return New String() {"LightColor", "DarkColor", "Desaturation", "ToneAmount"}
            Case "MonochromeEffect"
                Return New String() {"Color"}
            Case "SharpenEffect"
                Return New String() {"Height", "Amount"}
            Case Else
                Return New String() {}
        End Select
    End Function
    Public Shared Function MappingType(value As Double) As Double
        Return value
    End Function
    Public Shared Function MappingType(value As System.Windows.Vector) As System.Windows.Vector
        Return value
    End Function
    Public Shared Function MappingType(value As Color) As ColorVector
        Return New ColorVector(value)
    End Function
    Public Shared Function MappingType(value As GradientStop) As GradientVector
        Return New GradientVector(value)
    End Function
    Public Shared Operator +(cv1 As EffectVector, cv2 As EffectVector) As EffectVector
        If cv1.Name = cv2.Name Then
            Dim cv As New EffectVector With {.Name = cv1.Name, .Count = cv1.Count}
            Dim cn As Integer = Math.Min(cv1.Values.Count, cv2.Values.Count)
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                cv.Values.Add(Key, Sum(cv1.Values(Key), cv2.Values(Key)))
                i += 1
                If i = cn Then Exit For
            Next
            Return cv
        Else
            Return Nothing
        End If
    End Operator
    Public Shared Operator -(cv1 As EffectVector, cv2 As EffectVector) As EffectVector
        If cv1.Name = cv2.Name Then
            Dim cv As New EffectVector With {.Name = cv1.Name, .Count = cv1.Count}
            Dim cn As Integer = Math.Min(cv1.Values.Count, cv2.Values.Count)
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                cv.Values.Add(Key, Subtract(cv1.Values(Key), cv2.Values(Key)))
                i += 1
                If i = cn Then Exit For
            Next
            Return cv
        Else
            Return Nothing
        End If
    End Operator
    Public Shared Operator *(cv1 As EffectVector, cv2 As Double) As EffectVector
        Dim cv As New EffectVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Multiply(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator *(cv2 As Double, cv1 As EffectVector) As EffectVector
        Dim cv As New EffectVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Multiply(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator /(cv1 As EffectVector, cv2 As Double) As EffectVector
        Dim cv As New EffectVector With {.Name = cv1.Name, .Count = cv1.Count}
        Dim i As Integer = 0
        For Each Key As String In cv1.Values.Keys
            cv.Values.Add(Key, Divide(cv1.Values(Key), cv2))
        Next
        Return cv
    End Operator
    Public Shared Operator =(cv1 As EffectVector, cv2 As EffectVector) As Boolean
        If cv1.Name = cv2.Name AndAlso cv1.Count = cv2.Count Then
            'Dim cv As New EffectVector With {.Name = cv1.Name}
            If cv1.Values.Count <> cv2.Values.Count Then Return False
            Dim i As Integer = 0
            For Each Key As String In cv1.Values.Keys
                If Inequals(cv1.Values(Key), cv2.Values(Key)) Then Return False
            Next
            Return True
        Else
            Return False
        End If
    End Operator
    Public Shared Operator <>(cv1 As EffectVector, cv2 As EffectVector) As Boolean
        Return Not (cv1 = cv2)
    End Operator

End Class
<Serializable()>
Public Structure GradientVector
    Implements IComparable(Of GradientVector)

    Public Sub New(gs As GradientStop)
        O = gs.Offset
        C = gs.Color
    End Sub
    Public Sub New(vO As Double, vC As ColorVector)
        O = vO
        C = vC
    End Sub
    Public O As Double
    Public C As ColorVector
    Public Shared Narrowing Operator CType(gv As GradientVector) As GradientStop
        Return New GradientStop With {.Offset = gv.O, .Color = gv.C}
    End Operator
    Public Shared Widening Operator CType(gs As GradientStop) As GradientVector
        Return New GradientVector(gs)
    End Operator
    Public Shared Operator +(cv1 As GradientVector, cv2 As GradientVector) As GradientVector
        Return New GradientVector(cv1.O + cv2.O, cv1.C + cv2.C)
    End Operator
    Public Shared Operator -(cv1 As GradientVector, cv2 As GradientVector) As GradientVector
        Return New GradientVector(cv1.O - cv2.O, cv1.C - cv2.C)
    End Operator
    Public Shared Operator *(cv1 As GradientVector, cv2 As Double) As GradientVector
        Return New GradientVector(cv1.O * cv2, cv1.C * cv2)
    End Operator
    Public Shared Operator *(cv2 As Double, cv1 As GradientVector) As GradientVector
        Return New GradientVector(cv1.O * cv2, cv1.C * cv2)
    End Operator
    Public Shared Operator /(cv1 As GradientVector, cv2 As Double) As GradientVector
        Return New GradientVector(cv1.O / cv2, cv1.C / cv2)
    End Operator
    Public Function CompareTo(other As GradientVector) As Integer Implements System.IComparable(Of GradientVector).CompareTo
        Return Math.Sign(O - other.O)
    End Function
    Public Shared Operator =(cv1 As GradientVector, cv2 As GradientVector) As Boolean
        Return cv1.O = cv2.O AndAlso cv1.C = cv2.C
    End Operator
    Public Shared Operator <>(cv1 As GradientVector, cv2 As GradientVector) As Boolean
        Return cv1.O <> cv2.O OrElse cv1.C <> cv2.C
    End Operator
End Structure
<Serializable()>
Public Structure ColorVector
    Public Sub New(scb As SolidColorBrush)
        A = scb.Color.A
        R = scb.Color.R
        G = scb.Color.G
        B = scb.Color.B
    End Sub
    Public Sub New(nA As String, nR As String, nG As String, nB As String)
        If Not Double.TryParse(nA, A) Then A = 255D
        Double.TryParse(nR, R)
        Double.TryParse(nG, G)
        Double.TryParse(nB, B)
    End Sub
    Public Sub New(c As Color)
        A = c.A
        R = c.R
        G = c.G
        B = c.B
    End Sub
    Public Sub New(vA As Double, vR As Double, vG As Double, vB As Double)
        A = vA
        R = vR
        G = vG
        B = vB
    End Sub
    Public A As Double
    Public R As Double
    Public G As Double
    Public B As Double
    Public Shared Function Normal(value As Double) As Integer
        If value > 255 Then value = 255
        If value < 0 Then value = 0
        Return value
    End Function
    Public Shared Narrowing Operator CType(cv As ColorVector) As Color
        Return Color.FromArgb(Normal(cv.A), Normal(cv.R), Normal(cv.G), Normal(cv.B))
    End Operator
    Public Shared Narrowing Operator CType(cv As ColorVector) As SolidColorBrush
        Return New SolidColorBrush(Color.FromArgb(Normal(cv.A), Normal(cv.R), Normal(cv.G), Normal(cv.B)))
    End Operator
    Public Shared Widening Operator CType(cv As Color) As ColorVector
        Return New ColorVector(cv.A, cv.R, cv.G, cv.B)
    End Operator
    Public Shared Widening Operator CType(scb As SolidColorBrush) As ColorVector
        Dim cv As Color = scb.Color
        Return New ColorVector(cv.A, cv.R, cv.G, cv.B)
    End Operator
    Public Shared Operator +(cv1 As ColorVector, cv2 As ColorVector) As ColorVector
        Return New ColorVector(cv1.A + cv2.A, cv1.R + cv2.R, cv1.G + cv2.G, cv1.B + cv2.B)
    End Operator
    Public Shared Operator -(cv1 As ColorVector, cv2 As ColorVector) As ColorVector
        Return New ColorVector(cv1.A - cv2.A, cv1.R - cv2.R, cv1.G - cv2.G, cv1.B - cv2.B)
    End Operator
    Public Shared Operator *(cv1 As ColorVector, cv2 As Double) As ColorVector
        Return New ColorVector(cv1.A * cv2, cv1.R * cv2, cv1.G * cv2, cv1.B * cv2)
    End Operator
    Public Shared Operator *(cv2 As Double, cv1 As ColorVector) As ColorVector
        Return New ColorVector(cv1.A * cv2, cv1.R * cv2, cv1.G * cv2, cv1.B * cv2)
    End Operator
    Public Shared Operator /(cv1 As ColorVector, cv2 As Double) As ColorVector
        Return New ColorVector(cv1.A / cv2, cv1.R / cv2, cv1.G / cv2, cv1.B / cv2)
    End Operator
    Public Shared Operator =(cv1 As ColorVector, cv2 As ColorVector) As Boolean
        Return cv1.A = cv2.A AndAlso cv1.R = cv2.R AndAlso cv1.G = cv2.G AndAlso cv1.B = cv2.B
    End Operator
    Public Shared Operator <>(cv1 As ColorVector, cv2 As ColorVector) As Boolean
        Return cv1.A <> cv2.A OrElse cv1.R <> cv2.R OrElse cv1.G <> cv2.G OrElse cv1.B <> cv2.B
    End Operator
End Structure
<AttributeUsage(AttributeTargets.Property)>
Public Class ActAttribute
    Inherits Attribute
    Private _BindName As String = Nothing
    Public Sub New()
    End Sub
    Public Sub New(vBindName As String)
        _BindName = vBindName
    End Sub
    Public ReadOnly Property BindName As String
        Get
            Return _BindName
        End Get
    End Property
End Class
Public Class ButtonAdd
    Inherits Canvas
    Private pl As New Shapes.Polygon
    Public Sub New()
        pl.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
        pl.VerticalAlignment = Windows.VerticalAlignment.Stretch
        Dim V1 As Integer = 2
        Dim V2 As Integer = 10
        pl.Points = New Media.PointCollection From {New Windows.Point(V1, V2),
                                                     New Windows.Point(V2, V2),
                                                     New Windows.Point(V2, V1),
                                                     New Windows.Point(24 - V2, V1),
                                                    New Windows.Point(24 - V2, V2),
                                                    New Windows.Point(24 - V1, V2),
                                                     New Windows.Point(24 - V1, 24 - V2),
                                                     New Windows.Point(24 - V2, 24 - V2),
                                                     New Windows.Point(24 - V2, 24 - V1),
                                                     New Windows.Point(V2, 24 - V1),
                                                     New Windows.Point(V2, 24 - V2),
                                                     New Windows.Point(V1, 24 - V2)}
        pl.Fill = Brushes.Yellow
        pl.Stroke = Brushes.BlueViolet
        Me.Children.Add(pl)
        Canvas.SetLeft(pl, V1)
        Canvas.SetTop(pl, V1)
        Background = New SolidColorBrush(Color.FromArgb(1, 255, 255, 255))
        Me.Width = 24
        Me.Height = 24
    End Sub
End Class
Public Class ButtonClose
    Inherits Canvas
    Private pl As New Shapes.Polygon
    Public Sub New()
        pl.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
        pl.VerticalAlignment = Windows.VerticalAlignment.Stretch
        Dim V1 As Integer = 2
        Dim V2 As Integer = 10
        pl.Points = New Media.PointCollection From {New Windows.Point(V1, V2),
                                                     New Windows.Point(V2, V2),
                                                     New Windows.Point(V2, V1),
                                                     New Windows.Point(24 - V2, V1),
                                                    New Windows.Point(24 - V2, V2),
                                                    New Windows.Point(24 - V1, V2),
                                                     New Windows.Point(24 - V1, 24 - V2),
                                                     New Windows.Point(24 - V2, 24 - V2),
                                                     New Windows.Point(24 - V2, 24 - V1),
                                                     New Windows.Point(V2, 24 - V1),
                                                     New Windows.Point(V2, 24 - V2),
                                                     New Windows.Point(V1, 24 - V2)}
        Dim sr As Double = Math.Sqrt(2) / 2
        pl.RenderTransform = New Media.MatrixTransform(New Matrix(sr, -sr, sr, sr, -6, 12))
        pl.Fill = Brushes.Red
        pl.Stroke = Brushes.BlueViolet
        Background = New SolidColorBrush(Color.FromArgb(1, 255, 255, 255))
        Me.Children.Add(pl)
        Canvas.SetLeft(pl, V1)
        Canvas.SetTop(pl, V1)
        Me.Width = 24
        Me.Height = 24
    End Sub
End Class
Public Class NumberBox
    Inherits System.Windows.Controls.TextBox
    Public Sub New()
        _AllowDecimal = True
        _AllowNegative = True
    End Sub
    Public Property AllowDecimal As Boolean
    Public Property AllowNegative As Boolean
    Protected Overrides Sub OnKeyDown(e As System.Windows.Input.KeyEventArgs)
        Dim sel As Integer
        Select Case e.Key
            Case Windows.Input.Key.Subtract, Windows.Input.Key.OemMinus
                If _AllowNegative AndAlso Not Text.Contains("-") Then
                    sel = SelectionStart
                    Text = "-" + Text
                    SelectionStart = IIf(sel + 1 < Text.Length, sel + 1, Text.Length)
                End If
                e.Handled = True
            Case Windows.Input.Key.Add, Windows.Input.Key.OemPlus
                If Text.Contains("-") Then
                    sel = SelectionStart
                    Text = Text.Replace("-", "")
                    SelectionStart = IIf(sel - 1 > -1, sel - 1, 0)
                End If
                e.Handled = True
            Case Windows.Input.Key.Decimal, Windows.Input.Key.OemPeriod
                If _AllowDecimal Then
                    sel = SelectionStart
                    Dim di As Integer = Text.IndexOf(".")
                    If di > -1 Then
                        Text = Text.Replace(".", "")
                        SelectionStart = IIf(di < sel, sel - 1, sel)
                    End If
                End If
            Case Windows.Input.Key.Delete

            Case Windows.Input.Key.Back

            Case Windows.Input.Key.D0, Windows.Input.Key.D1, Windows.Input.Key.D2, Windows.Input.Key.D3, Windows.Input.Key.D4, Windows.Input.Key.D5, Windows.Input.Key.D6, Windows.Input.Key.D7, Windows.Input.Key.D8, Windows.Input.Key.D9
            Case Key.NumPad0 To Key.NumPad9
            Case Else
                e.Handled = True
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Public Property NumberType As String

    Public Property Value As Object
        Get
            If Text Is Nothing OrElse Text.Length = 0 Then Text = "0"
            Select Case _NumberType
                Case "Integer"
                    Dim i As Integer
                    If Integer.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Long"
                    Dim i As Long
                    If Long.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Short"
                    Dim i As Short
                    If Short.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Byte"
                    Dim i As Byte
                    If Byte.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "UInteger"
                    Dim i As UInteger
                    If UInteger.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "ULong"
                    Dim i As ULong
                    If ULong.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "UShort"
                    Dim i As UShort
                    If UShort.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Single"
                    Dim i As Single
                    If Single.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Double"
                    Dim i As Double
                    If Double.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case "Decimal"
                    Dim i As Decimal
                    If Decimal.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case Else
                    Return 0
            End Select
        End Get
        Set(value As Object)
            If TypeOf value Is Integer Then
                NumberType = "Integer"
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Long Then
                NumberType = "Long"
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Short Then
                NumberType = "Short"
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Byte Then
                NumberType = "Byte"
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is UInteger Then
                NumberType = "UInteger"
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is ULong Then
                NumberType = "ULong"
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is UShort Then
                NumberType = "UShort"
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is Single Then
                NumberType = "Single"
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Double Then
                NumberType = "Double"
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Decimal Then
                NumberType = "Decimal"
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            End If
        End Set
    End Property
    Protected Overrides Sub OnMouseWheel(e As System.Windows.Input.MouseWheelEventArgs)
        Dim i As Integer
        Try
            If Integer.TryParse(Text, i) Then
                Dim j As Integer = i + e.Delta / 120
                If Not AllowNegative And j < 0 Then j = 0
                Text = j.ToString
            End If
        Catch ex As Exception

        End Try
        MyBase.OnMouseWheel(e)
    End Sub
End Class

Public Class ButtonLabel
    Inherits Label
    Public Sub New()
        BorderThickness = New System.Windows.Thickness(1)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As System.Windows.Input.MouseEventArgs)
        BorderBrush = Brushes.RoyalBlue
        Background = Brushes.LightYellow
        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(e As System.Windows.Input.MouseEventArgs)
        BorderBrush = Brushes.Transparent
        Background = Brushes.Transparent
        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        Background = Brushes.LightGreen
        MyBase.OnMouseLeftButtonDown(e)
    End Sub
    Protected Overrides Sub OnMouseLeftButtonUp(e As System.Windows.Input.MouseButtonEventArgs)
        Background = Brushes.LightYellow
        MyBase.OnMouseLeftButtonUp(e)
    End Sub
End Class

Public Class ObsoleteEditBox
    Inherits Grid
    Private WithEvents _TextBox As New System.Windows.Controls.TextBox
    Private WithEvents _Label As New System.Windows.Controls.Label
    Public Sub New()
        Children.Add(_TextBox)
        Children.Add(_Label)
        _TextBox.Text = ""
        _TextBox.Visibility = Windows.Visibility.Hidden
        _Label.Content = ""
        AddHandler OnEnterEdit, AddressOf PeerEnterEdit
    End Sub
    Private Sub PeerEnterEdit(sender As Object, e As RoutedEventArgs)
        If sender IsNot Me Then Deactivate()
    End Sub
    Public Property Text As String
        Get
            Return _Label.Content
        End Get
        Set(value As String)
            _Label.Content = value
            _TextBox.Text = value
        End Set
    End Property
    Public Sub Activate()
        _Label.Visibility = Windows.Visibility.Collapsed
        _TextBox.Text = _Label.Content
        _TextBox.Visibility = Windows.Visibility.Visible
        RaiseEvent OnEnterEdit(Me, New RoutedEventArgs)
    End Sub
    Public Sub Deactivate()
        _TextBox.Visibility = Windows.Visibility.Collapsed
        _Label.Content = _TextBox.Text
        _Label.Visibility = Windows.Visibility.Visible
    End Sub
    Private Sub _TextBox_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles _TextBox.KeyDown
        If e.Key = Key.Escape Or e.Key = Key.Enter Or e.Key = Key.ImeAccept Then Deactivate()
    End Sub
    Private Sub _TextBox_LostFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles _TextBox.LostFocus
        Deactivate()
    End Sub
    Private Sub _Label_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles _Label.MouseLeftButtonDown
        If Not _IsReadOnly Then Activate()
    End Sub
    Private _IsReadOnly As Boolean = False
    Public Property IsReadOnly As Boolean
        Get
            Return _IsReadOnly
        End Get
        Set(value As Boolean)
            _IsReadOnly = value
        End Set
    End Property
    Public Shared Event OnEnterEdit(sender As Object, e As RoutedEventArgs)
End Class

