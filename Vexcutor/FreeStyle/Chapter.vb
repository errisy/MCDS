Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
<Shallow()>
Public Class ChapterTree
    Inherits TreeView
    Public _Root As New ChapterRoot
    Public Sub New()
        Root = _Root
        BrushVector.ImageSources = _Root.Images
    End Sub
    <EarlyBind("ToolBox")> Public RelatedToolBox As ToolBox
    <Save()> Public Property Root As ChapterRoot
        Get
            Return _Root
        End Get
        Set(value As ChapterRoot)
            _Root = value
            Items.Clear()
            Items.Add(_Root)
            If _Root.SelectionIndex > -1 AndAlso _Root.SelectionIndex < _Root.Items.Count Then
                _Root.SelectChapter(_Root.Items(_Root.SelectionIndex))
            End If
        End Set
    End Property
    Public Sub InitializeNewProject()
        _Root.AddNewVolumn().AddNewChapter()
        _Root.ExpandSubtree()
    End Sub
    <EarlyBind("Stage")> Public Property RelatedStage As FreeStage
    <EarlyBind("Director")> Public Property RelatedDirector As Director
    <EarlyBind("BackStage")> Public Property RelatedBackStage As FreeStage
    <EarlyBind("BackStage")> Public Property RelatedBackDirector As Director
End Class
<Shallow()>
Public Class ChapterRoot
    Inherits TreeViewItem
    Private gdHost As New GridBase
    Public txtName As New EditBox
    Public WithEvents lblAdd As New AddButton

    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "New Project"
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Storyline, 24, 24))
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(lblAdd)
        'gdHost.AddColumnItem(btnSave)
        Background = Brushes.Transparent
        'AddHandler lblAdd.Click, Click.ClickAdd(Me, Function() New Chapter)
    End Sub
    Friend SelectionIndex As Integer = -1
    Public Sub Save()
        If _CurrentVolumn.OK Then _CurrentVolumn.Save()
        If _CurrentSelection.OK Then _CurrentSelection.Save()
    End Sub
    <Save()> Public Property CurrentSelectionIndex As Integer
        Get
            If _CurrentSelection IsNot Nothing AndAlso Items.Count > 0 AndAlso Items.Contains(_CurrentSelection) Then
                Return Items.IndexOf(_CurrentSelection)
            Else
                Return -1
            End If
        End Get
        Set(value As Integer)
            SelectionIndex = value
        End Set
    End Property
    <Save()> Public Property Volumns As ShallowList(Of Volumn)
        Get
            Dim cList As New ShallowList(Of Volumn)
            For Each it As Volumn In Items
                cList.Add(it)
            Next
            Return cList
        End Get
        Set(value As ShallowList(Of Volumn))
            Items.Clear()
            If value IsNot Nothing Then
                For Each it As Volumn In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property

    Private _CurrentSelection As Chapter
    Public Sub SelectChapter(chp As Chapter)
        Parent.RelatedBackDirector.Visibility = Windows.Visibility.Hidden
        Parent.RelatedStage.Visibility = Windows.Visibility.Visible
        Parent.RelatedDirector.Visibility = Windows.Visibility.Visible
        Parent.RelatedToolBox.Stage = Parent.RelatedStage
        SetSelection(chp)
        If _CurrentSelection IsNot Nothing Then
            _CurrentSelection.Save()
        End If
        _CurrentSelection = chp
        chp.Load()
    End Sub
    Public ReadOnly Property SelectedChapter As Chapter
        Get
            Return _CurrentSelection
        End Get
    End Property
    Public ReadOnly Property SelecteVolumn As Volumn
        Get
            Return _CurrentVolumn
        End Get
    End Property
    Private _CurrentVolumn As Volumn
    Public Sub SelectVolumn(vlm As Volumn)
        If _CurrentVolumn IsNot Nothing Then
            _CurrentVolumn.Save()
        End If
        _CurrentVolumn = vlm
        SetSelection(vlm)
        vlm.Load()
        Parent.RelatedStage.Visibility = Windows.Visibility.Hidden
        Parent.RelatedDirector.Visibility = Windows.Visibility.Hidden
        Parent.RelatedBackDirector.Visibility = Windows.Visibility.Visible
        Parent.RelatedToolBox.Stage = Parent.RelatedBackStage
    End Sub
    Private Sub SetSelection(obj As Volumn)
        Parent.RelatedBackStage.DesignMode = True
        For Each it As Volumn In Items
            If it Is obj Then
                it.btnVisible.Opacity = 1D
            Else
                it.btnVisible.Opacity = 0.4D
            End If
            For Each ch As Chapter In it.Items
                ch.btnVisible.Opacity = 0.4D
            Next
        Next
    End Sub
    Private Sub SetSelection(obj As Chapter)
        Parent.RelatedBackStage.DesignMode = False
        For Each it As Volumn In Items
            it.btnVisible.Opacity = 0.4D
            For Each ch As Chapter In it.Items
                If ch Is obj Then
                    ch.btnVisible.Opacity = 1D
                    If ch.Parent IsNot _CurrentVolumn Then
                        If _CurrentVolumn IsNot Nothing Then _CurrentVolumn.Save()
                        _CurrentVolumn = ch.Parent
                        _CurrentVolumn.Load()
                    End If
                Else
                    ch.btnVisible.Opacity = 0.4D
                End If
            Next
        Next
    End Sub
    Public Function [Next]() As Boolean
        If _CurrentVolumn.NO Then
            _CurrentVolumn = NextOf(_CurrentVolumn, Items, Function(item As Volumn) item.Items.Count > 0)
            If _CurrentVolumn.OK Then _CurrentVolumn.Load()
        End If
        If _CurrentVolumn.OK Then
            _CurrentSelection = NextFrom(_CurrentSelection, _CurrentVolumn.Items)
            If _CurrentSelection.OK Then
                SetSelection(_CurrentSelection)
                _CurrentSelection.LoadAndPlay()
                Return True
            Else
                _CurrentVolumn = NextOf(_CurrentVolumn, Items, Function(item As Volumn) item.Items.Count > 0)
                If _CurrentVolumn.OK Then _CurrentVolumn.Load()
                Return Me.Next
            End If
        Else
            Return False
        End If
    End Function
    Public Function [Previous]() As Boolean
        If _CurrentVolumn.NO Then
            _CurrentVolumn = PreviousOf(_CurrentVolumn, Items, Function(item As Volumn) item.Items.Count > 0)
            If _CurrentVolumn.OK Then _CurrentVolumn.Load()
        End If
        If _CurrentVolumn.OK Then
            _CurrentSelection = PreviousFrom(_CurrentSelection, _CurrentVolumn.Items)
            If _CurrentSelection.OK Then
                SetSelection(_CurrentSelection)
                _CurrentSelection.LoadAndPlay()
                Return True
            Else
                _CurrentVolumn = PreviousOf(_CurrentVolumn, Items, Function(item As Volumn) item.Items.Count > 0)
                If _CurrentVolumn.OK Then _CurrentVolumn.Load()
                Return Me.Previous
            End If
        Else
            Return False
        End If
    End Function
    Public Function [First]() As Boolean
        _CurrentVolumn = FirstOf(Items, Function(item As Volumn) item.Items.Count > 0)
        If _CurrentVolumn.OK Then _CurrentVolumn.Load()
        If _CurrentVolumn.OK Then
            _CurrentSelection = _CurrentVolumn(0)
            SetSelection(_CurrentSelection)
            _CurrentSelection.LoadAndPlay()
            Return True
        Else
            Clean()
            Return False
        End If
    End Function
    Public Function [Last]() As Boolean
        _CurrentVolumn = LastOf(Items, Function(item As Volumn) item.Items.Count > 0)
        If _CurrentVolumn.OK Then _CurrentVolumn.Load()
        If _CurrentVolumn.OK Then
            _CurrentSelection = _CurrentVolumn(-1)
            SetSelection(_CurrentSelection)
            _CurrentSelection.LoadAndPlay()
            Return True
        Else
            Clean()
            Return False
        End If
    End Function
    Public ReadOnly Property RelatedStage As FreeStage
        Get
            Return Parent.RelatedStage
        End Get
    End Property
    Public ReadOnly Property RelatedDirector As Director
        Get
            Return Parent.RelatedDirector
        End Get
    End Property
    Public Shadows ReadOnly Property Parent As ChapterTree
        Get
            Return MyBase.Parent
        End Get
    End Property
    <Save()> Public Property Text As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    Public Sub RemoveAndShowNext(item As Chapter)
        If _CurrentVolumn.Items.Contains(item) Then
            If item Is _CurrentSelection Then
                'unload all elements
                Clean()
                Dim i As Integer = _CurrentVolumn.Items.IndexOf(item)
                _CurrentVolumn.Items.Remove(item)
                _CurrentSelection = Nothing
                If _CurrentVolumn.Items.Count > i Then
                    _CurrentSelection = _CurrentVolumn.Items(i)
                    _CurrentSelection.Load()
                ElseIf _CurrentVolumn.Items.Count > 0 Then
                    i = _CurrentVolumn.Items.Count - 1
                    _CurrentSelection = _CurrentVolumn.Items(i)
                    _CurrentSelection.Load()
                ElseIf _CurrentVolumn.Items.Count = 0 Then
                    SelectVolumn(_CurrentVolumn)
                End If
            End If
        Else
            Dim vlm As Volumn = item.Parent
            vlm.Items.Remove(item)
        End If
    End Sub
    Public Sub RemoveAndShowNext(vlm As Volumn)
        If vlm Is _CurrentVolumn Then
            'unload all elements
            Clean()
            CleanBack()
            Dim i As Integer = Items.IndexOf(vlm)
            Items.Remove(vlm)
            _CurrentSelection = Nothing
            If Items.Count > i Then
                _CurrentVolumn = Items(i)
                _CurrentVolumn.Load()
            ElseIf Items.Count > 0 Then
                i = Items.Count - 1
                _CurrentVolumn = Items(i)
                _CurrentVolumn.Load()
            End If
        Else
            Items.Remove(vlm)
        End If
    End Sub
    Public Sub Clean()
        If Parent.RelatedStage IsNot Nothing And Parent.RelatedDirector IsNot Nothing Then
            Dim x = Dispatcher.DisableProcessing
            Parent.RelatedStage.Actors = New ShallowList(Of IActor)
            Parent.RelatedDirector.Crew.Actors = New ShallowList(Of Actor)
            Parent.RelatedDirector.Storyline.Stories = New ShallowList(Of Story)
            x.Dispose()
        End If
    End Sub
    Public Sub CleanBack()
        If Parent.RelatedBackStage IsNot Nothing And Parent.RelatedBackDirector IsNot Nothing Then
            Dim x = Parent.RelatedBackDirector.Dispatcher.DisableProcessing
            Parent.RelatedBackStage.Actors = New ShallowList(Of IActor)
            Parent.RelatedBackDirector.Crew.Actors = New ShallowList(Of Actor)
            Parent.RelatedBackDirector.Storyline.Stories = New ShallowList(Of Story)
            x.Dispose()
        End If
    End Sub
    Private Sub lblAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblAdd.Click
        AddNewVolumn()
    End Sub
    Public Function AddNewVolumn() As Volumn
        Dim vlm As New Volumn
        Items.Add(vlm)
        SelectVolumn(vlm)
        Return vlm
    End Function
    Default Public ReadOnly Property Volumn(index As Integer) As Volumn
        Get
            If index < 0 Then
                Return Items(Items.Count + index)
            Else
                Return Items(index)
            End If
        End Get
    End Property


#Region "Images"
    Private _Images As New ShallowDictionary(Of Double, StreamImage)
    <Save()> Public Property Images As ShallowDictionary(Of Double, StreamImage)
        Get
            Return _Images
        End Get
        Set(value As ShallowDictionary(Of Double, StreamImage))
            _Images = value
            BrushVector.ImageSources = _Images
        End Set
    End Property
#End Region
#Region "Styles"
    Private _BrushStyles As New ShallowList(Of BrushStyle)
    Private _EffectStyles As New ShallowList(Of EffectStyle)
    <Save()> Public Property BrushStyles As ShallowList(Of BrushStyle)
        Get
            Return _BrushStyles
        End Get
        Set(value As ShallowList(Of BrushStyle))
            _BrushStyles = value
        End Set
    End Property
    <Save()> Public Property EffectStyles As ShallowList(Of EffectStyle)
        Get
            Return _EffectStyles
        End Get
        Set(value As ShallowList(Of EffectStyle))
            _EffectStyles = value
        End Set
    End Property
#End Region
End Class
<Shallow()>
Public Class Volumn
    Inherits TreeViewItem
    Private gdHost As New GridBase
    Public WithEvents DragDropBar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.LightGreen, .AllowDrop = True}
    Public WithEvents btnVisible As New IdeaButton
    Public WithEvents txtName As New EditBox
    Public WithEvents lblAdd As New AddButton
    Public WithEvents btnSave As New SaveButton
    Public WithEvents btnDelete As New DeleteButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "New Chapter"
        gdHost.AddColumnItem(DragDropBar)
        gdHost.AddColumnItem(btnVisible)
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(btnSave)
        gdHost.AddColumnItem(lblAdd)
        gdHost.AddColumnItem(btnDelete)
        Background = Brushes.Transparent
    End Sub

    Private PrepareDrag As Boolean = False

    Private Sub DragDropBar_DragEnter(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.DragEnter
        If TypeOf e.Data.GetData("VolumnSource") Is Volumn AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub
    Private Sub DragDropBar_Drop(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.Drop
        If TypeOf e.Data.GetData("VolumnSource") Is Volumn AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
            Dim sc As Volumn = e.Data.GetData("VolumnSource")
            sc.PrepareDrag = False
            Dim j As Integer = Parent.Items.IndexOf(Me)
            Dim p As ChapterRoot = Parent
            sc.Parent.Items.Remove(sc)
            p.Items.Insert(j, sc)
        End If
    End Sub
    Private Sub DragDropBar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseLeftButtonDown
        PrepareDrag = True
    End Sub
    Private Sub DragDropBar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles DragDropBar.MouseMove
        If PrepareDrag Then
            Dim dataObj As New DataObject("MoveVolumn")
            dataObj.SetData("VolumnSource", Me)
            DragDrop.DoDragDrop(Me, dataObj, DragDropEffects.Move)
        End If
    End Sub

    <Save()> Public Property Chapters As ShallowList(Of Chapter)
        Get
            Dim cList As New ShallowList(Of Chapter)
            For Each it As Chapter In Items
                cList.Add(it)
            Next
            Return cList
        End Get
        Set(value As ShallowList(Of Chapter))
            Items.Clear()
            If value IsNot Nothing Then
                For Each it As Chapter In value
                    Items.Add(it)
                Next
            End If
        End Set
    End Property
    Default Public ReadOnly Property Chapter(index As Integer) As Chapter
        Get
            If index < 0 Then
                Return Items(Items.Count + index)
            Else
                Return Items(index)
            End If
        End Get
    End Property
    Private _Data As Byte()
    <Save()> Public Property Data As Byte()
        Get
            Return _Data
        End Get
        Set(value As Byte())
            _Data = value
        End Set
    End Property
    Private Sub lblAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles lblAdd.Click
        AddNewChapter()
    End Sub
    Public Sub AddNewChapter()
        Dim chp As New Chapter
        Items.Add(chp)
        Parent.SelectChapter(chp)
        Me.ExpandSubtree()
    End Sub
    <Save()> Public Property Text As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    Public ReadOnly Property RelatedDirector As Director
        Get
            Return Parent.RelatedDirector
        End Get
    End Property
    Public ReadOnly Property RelatedStage As FreeStage
        Get
            Return Parent.RelatedStage
        End Get
    End Property
    Public Shadows ReadOnly Property Parent As ChapterRoot
        Get
            Return MyBase.Parent
        End Get
    End Property
    Private Sub btnDelete_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Control OrElse MsgBox("Are you sure to delete this show?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            If Parent IsNot Nothing Then
                'Clean the stage
                Parent.RemoveAndShowNext(Me)
            End If
        End If
    End Sub
    Private Sub txtName_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnVisible.Click
        [Select]()
    End Sub
    Public Sub [Select]()
        Parent.SelectVolumn(Me)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        Save()
    End Sub
    Public Sub Save()
        If Parent.SelecteVolumn Is Me AndAlso Parent.Parent.RelatedStage IsNot Nothing And Parent.Parent.RelatedBackDirector IsNot Nothing Then
            _Data = SaveStage(Parent.Parent.RelatedBackStage, Parent.Parent.RelatedBackDirector)
        End If
    End Sub
    Public Sub Load()
        Parent.CleanBack()
        If _Data Is Nothing Then
        ElseIf Parent.Parent.RelatedStage IsNot Nothing And Parent.Parent.RelatedBackDirector IsNot Nothing Then
            LoadStage(_Data, Parent.Parent.RelatedBackStage, Parent.Parent.RelatedBackDirector)
        End If
    End Sub
End Class

<Shallow()>
Public Class Chapter
    Inherits TreeViewItem
    Private gdHost As New GridBase
    Public WithEvents DragDropBar As New Ellipse With {.Width = 24, .Height = 24, .Fill = Brushes.MediumPurple, .AllowDrop = True}
    Public WithEvents btnVisible As New IdeaButton
    Public WithEvents txtName As New EditBox
    Public WithEvents btnSave As New SaveButton
    Public WithEvents btnDuplicate As New DuplicateButton
    Public WithEvents btnDelete As New DeleteButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        txtName.Text = "New Chapter"
        gdHost.AddColumnItem(DragDropBar)
        gdHost.AddColumnItem(btnVisible)
        gdHost.AddColumnItem(txtName)
        gdHost.AddColumnItem(btnSave)
        gdHost.AddColumnItem(btnDuplicate)
        gdHost.AddColumnItem(btnDelete)
        Background = Brushes.Transparent
    End Sub
    Private PrepareDrag As Boolean = False

    Private Sub DragDropBar_DragEnter(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.DragEnter
        If TypeOf e.Data.GetData("ChapterSource") Is Chapter AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub

    Private Sub DragDropBar_Drop(sender As Object, e As System.Windows.DragEventArgs) Handles DragDropBar.Drop
        If TypeOf e.Data.GetData("ChapterSource") Is Chapter AndAlso e.Source IsNot Me Then
            e.Effects = DragDropEffects.Move
            Dim sc As Chapter = e.Data.GetData("ChapterSource")
            sc.PrepareDrag = False
            Dim j As Integer = Parent.Items.IndexOf(Me)
            Dim p As Volumn = Parent
            sc.Parent.Items.Remove(sc)
            p.Items.Insert(j, sc)
        End If
    End Sub

    Private Sub DragDropBar_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DragDropBar.MouseLeftButtonDown
        PrepareDrag = True
    End Sub

    Private Sub DragDropBar_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles DragDropBar.MouseMove
        If PrepareDrag Then
            Dim dataObj As New DataObject("MoveChapter")
            dataObj.SetData("ChapterSource", Me)
            DragDrop.DoDragDrop(Me, dataObj, DragDropEffects.Move)
        End If
    End Sub

    Private _Data As Byte()
    <Save()> Public Property Data As Byte()
        Get
            Return _Data
        End Get
        Set(value As Byte())
            _Data = value
        End Set
    End Property
    <Save()> Public Property Text As String
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property
    Public Sub Save()
        If Parent.Parent.SelectedChapter Is Me AndAlso Parent.RelatedStage IsNot Nothing And Parent.RelatedDirector IsNot Nothing Then
            _Data = SaveStage(Parent.RelatedStage, Parent.RelatedDirector)
        End If
    End Sub
    Public Sub Load()
        Parent.Parent.Clean()
        If _Data Is Nothing Then

        ElseIf Parent.RelatedStage IsNot Nothing And Parent.RelatedDirector IsNot Nothing Then
            LoadStage(_Data, Parent.RelatedStage, Parent.RelatedDirector)
        End If
    End Sub
    Public Sub LoadAndPlay()
        If Parent.RelatedStage IsNot Nothing And Parent.RelatedDirector IsNot Nothing Then
            LoadStageAndPlay(_Data, Parent.RelatedStage, Parent.RelatedDirector)
        End If
    End Sub
    Public Shadows ReadOnly Property Parent As Volumn
        Get
            Return MyBase.Parent
        End Get
    End Property
    Private Sub btnDelete_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If System.Windows.Input.Keyboard.Modifiers = ModifierKeys.Control OrElse MsgBox("Are you sure to delete this show?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            If Parent IsNot Nothing Then
                'Clean the stage
                Parent.Parent.RemoveAndShowNext(Me)
            End If
        End If
    End Sub
    Private Sub txtName_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnVisible.Click
        Me.Select()
    End Sub
    Public Sub [Select]()
        Parent.Parent.SelectChapter(Me)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        Save()
    End Sub

    Private Sub btnDuplicate_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDuplicate.Click
        Dim chp As New Chapter
        chp._Data = Data.Clone
        Me.Parent.Items.Insert(Parent.Items.IndexOf(Me) + 1, chp)
    End Sub
End Class

Public Module StageFuctions
    Public Function SaveStage(vStage As FreeStage, vDirector As Director) As Byte()
        Dim si As New ShallowSerializer
        Dim Shapes As ShallowList(Of IActor) = vStage.Actors
        Dim Actors As ShallowList(Of Actor) = vDirector.Crew.Actors
        Dim Stories As ShallowList(Of Story) = vDirector.Storyline.Stories
        Dim Status As New Status
        Dim AllActors As New ShallowDictionary(Of String, IActor)
        For Each act As IActor In Shapes
            AllActors.Add(act.ID, act)
        Next
        Status.Record(AllActors)
        si.AddObj("Shape", Shapes)
        si.AddObj("Actor", Actors)
        si.AddObj("Story", Stories)
        si.AddObj("Status", Status)
        si.AddObj("Scale", vStage.ZoomScale)
        si.AddObj("Offset", vStage.CanvasOffset)
        Return si.GetBytes
    End Function
    Public Sub LoadStageAndPlay(_Data As Byte(), vStage As FreeStage, vDirector As Director)
        Dim bytes As Byte() = _Data
        Dim si As New ShallowSerializer(bytes)
        si.AddDevice("Stage", vStage)
        Dim Shapes As ShallowList(Of IActor) = si.GetObject("Shape")
        Dim Actors As ShallowList(Of Actor) = si.GetObject("Actor")
        Dim Stories As ShallowList(Of Story) = si.GetObject("Story")
        vStage.ZoomScale = si.GetObject("Scale")
        vStage.CanvasOffset = si.GetObject("Offset")
        Dim status As Status = si.GetObject("Status")
        Dim AllActors As New ShallowDictionary(Of String, IActor)
        Dim x = vStage.Dispatcher.DisableProcessing
        vStage.Actors = Shapes
        vDirector.Crew.Actors = Actors
        vDirector.Storyline.Stories = Stories
        For Each act As IActor In Shapes
            AllActors.Add(act.ID, act)
        Next
        status.Present(AllActors, AnimationTypeEnum.Brush)
        status.Present(AllActors, AnimationTypeEnum.Effect)
        status.Present(AllActors, AnimationTypeEnum.Movement)
        status.Present(AllActors, AnimationTypeEnum.Value)
        status.Present(AllActors, AnimationTypeEnum.Text)
        vDirector.Reset()
        x.Dispose()
    End Sub
    Public Sub LoadStage(_Data As Byte(), vStage As FreeStage, vDirector As Director)
        Dim bytes As Byte() = _Data
        Dim si As New ShallowSerializer(bytes)
        si.AddDevice("Stage", vStage)
        Dim Shapes As ShallowList(Of IActor) = si.GetObject("Shape")
        Dim Actors As ShallowList(Of Actor) = si.GetObject("Actor")
        Dim Stories As ShallowList(Of Story) = si.GetObject("Story")
        vStage.ZoomScale = si.GetObject("Scale")
        vStage.CanvasOffset = si.GetObject("Offset")
        Dim status As Status = si.GetObject("Status")
        Dim AllActors As New ShallowDictionary(Of String, IActor)
        Dim x = vStage.Dispatcher.DisableProcessing
        vStage.Actors = Shapes
        vDirector.Crew.Actors = Actors
        vDirector.Storyline.Stories = Stories
        For Each act As IActor In Shapes
            AllActors.Add(act.ID, act)
        Next
        status.Present(AllActors, AnimationTypeEnum.Brush)
        status.Present(AllActors, AnimationTypeEnum.Effect)
        status.Present(AllActors, AnimationTypeEnum.Movement)
        status.Present(AllActors, AnimationTypeEnum.Value)
        status.Present(AllActors, AnimationTypeEnum.Text)
        x.Dispose()
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function FirstOf(items As IEnumerable, selector As Func(Of Object, Boolean)) As Object
        For Each obj In items
            If selector.Invoke(obj) Then Return obj
        Next
        Return Nothing
    End Function
    Public Function NextFrom(Of T As Class)(item As T, items As IEnumerable) As Object
        Dim found As Boolean = item Is Nothing
        For Each obj In items
            If found Then Return obj
            If obj Is item Then found = True
        Next
        Return Nothing
    End Function

    Public Function PreviousFrom(Of T As Class)(item As T, items As IEnumerable) As Object
        Dim found As T = Nothing
        For Each obj In items
            If obj Is item Then Return found
            found = obj
        Next
        Return found
    End Function
    Public Function PreviousOf(Of T As Class)(item As T, items As IEnumerable, selector As Func(Of T, Boolean)) As Object
        Dim found As T = Nothing
        For Each obj In items
            If obj Is item Then Return found
            If selector.Invoke(obj) Then found = obj
        Next
        Return Nothing
    End Function
    Public Function NextOf(Of T As Class)(item As T, items As IEnumerable, selector As Func(Of T, Boolean)) As Object
        Dim found As Boolean = False
        For Each obj In items
            If found AndAlso selector.Invoke(obj) Then Return obj
            If obj Is item Then found = True
        Next
        Return Nothing
    End Function
    Public Function LastOf(items As IEnumerable, selector As Func(Of Object, Boolean)) As Object
        Dim found As Object = Nothing
        For Each obj In items
            If selector.Invoke(obj) Then found = obj
        Next
        Return found
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function OK(Of T)(obj As T) As Boolean
        Return obj IsNot Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function NO(Of T)(obj As T) As Boolean
        Return obj Is Nothing
    End Function
End Module