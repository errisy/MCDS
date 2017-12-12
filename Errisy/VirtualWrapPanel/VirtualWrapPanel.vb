Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows
Imports System.Windows.Media
Imports System.Diagnostics
Imports System.ComponentModel
Imports System.Windows.Input
Imports System.Collections.ObjectModel

Public Class VirtualizingWrapPanel
    Inherits VirtualizingPanel
    Implements IScrollInfo

#Region "Fields"

    Private _children As UIElementCollection
    Private _itemsControl As ItemsControl
    Private _generator As IItemContainerGenerator
    Private _offset As New Point(0, 0)
    Private _extent As New Size(0, 0)
    Private _viewport As New Size(0, 0)
    Private firstIndex As Integer = 0
    Private childSize As Size
    Private _pixelMeasuredViewport As New Size(0, 0)
    Private _realizedChildLayout As New Dictionary(Of UIElement, Rect)()
    Private _abstractPanel As WrapPanelAbstraction


#End Region

#Region "Properties"

    Private ReadOnly Property ChildSlotSize() As Size
        Get
            Return New Size(ItemWidth, ItemHeight)
        End Get
    End Property

#End Region

#Region "Dependency Properties"

    <TypeConverter(GetType(LengthConverter))> _
    Public Property ItemHeight() As Double
        Get
            Return CDbl(MyBase.GetValue(ItemHeightProperty))
        End Get
        Set(value As Double)
            MyBase.SetValue(ItemHeightProperty, value)
        End Set
    End Property

    <TypeConverter(GetType(LengthConverter))> _
    Public Property ItemWidth() As Double
        Get
            Return CDbl(MyBase.GetValue(ItemWidthProperty))
        End Get
        Set(value As Double)
            MyBase.SetValue(ItemWidthProperty, value)
        End Set
    End Property

    Public Property Orientation() As Orientation
        Get
            Return DirectCast(GetValue(OrientationProperty), Orientation)
        End Get
        Set(value As Orientation)
            SetValue(OrientationProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ItemHeightProperty As DependencyProperty = DependencyProperty.Register("ItemHeight", GetType(Double), GetType(VirtualizingWrapPanel), New FrameworkPropertyMetadata(Double.PositiveInfinity))
    Public Shared ReadOnly ItemWidthProperty As DependencyProperty = DependencyProperty.Register("ItemWidth", GetType(Double), GetType(VirtualizingWrapPanel), New FrameworkPropertyMetadata(Double.PositiveInfinity))
    Public Shared ReadOnly OrientationProperty As DependencyProperty = StackPanel.OrientationProperty.AddOwner(GetType(VirtualizingWrapPanel), New FrameworkPropertyMetadata(Orientation.Horizontal))

#End Region

#Region "Methods"

    Private Sub SetFirstRowViewItemIndex(index As Integer)
        SetVerticalOffset((index) / Math.Floor((_viewport.Width) / childSize.Width))
        SetHorizontalOffset((index) / Math.Floor((_viewport.Height) / childSize.Height))
    End Sub

    Public Sub Resizing(sender As Object, e As EventArgs)
        If _viewport.Width <> 0 Then
            Dim firstIndexCache As Integer = firstIndex
            _abstractPanel = Nothing
            MeasureOverride(_viewport)
            SetFirstRowViewItemIndex(firstIndex)
            firstIndex = firstIndexCache
        End If
    End Sub

    Public Function GetFirstVisibleSection() As Integer
        Dim section As Integer
        Dim maxSection = 0
        If _abstractPanel IsNot Nothing Then
            maxSection = _abstractPanel.Max(Function(x) x.Section)
        End If
        If Orientation = Orientation.Horizontal Then
            section = CInt(_offset.Y)
        Else
            section = CInt(_offset.X)
        End If
        If section > maxSection Then
            section = maxSection
        End If
        Return section
    End Function

    Public Function GetFirstVisibleIndex() As Integer
        Dim section As Integer = GetFirstVisibleSection()

        If _abstractPanel IsNot Nothing Then
            Dim item = _abstractPanel.Where(Function(x) x.Section = section).FirstOrDefault()
            If item IsNot Nothing Then
                Return item._index
            End If
        End If
        Return 0
    End Function

    Public Sub CleanUpItems(minDesiredGenerated As Integer, maxDesiredGenerated As Integer)
        For i As Integer = _children.Count - 1 To 0 Step -1
            Dim childGeneratorPos As New GeneratorPosition(i, 0)
            Dim itemIndex As Integer = _generator.IndexFromGeneratorPosition(childGeneratorPos)
            If itemIndex < minDesiredGenerated OrElse itemIndex > maxDesiredGenerated Then
                _generator.Remove(childGeneratorPos, 1)
                RemoveInternalChildRange(i, 1)
            End If
        Next
    End Sub

    Private Sub ComputeExtentAndViewport(pixelMeasuredViewportSize As Size, visibleSections As Integer)
        If Orientation = Orientation.Horizontal Then
            _viewport.Height = visibleSections
            _viewport.Width = pixelMeasuredViewportSize.Width
        Else
            _viewport.Width = visibleSections
            _viewport.Height = pixelMeasuredViewportSize.Height
        End If

        If Orientation = Orientation.Horizontal Then

            _extent.Height = _abstractPanel.SectionCount + ViewportHeight - 1
        Else
            _extent.Width = _abstractPanel.SectionCount + ViewportWidth - 1
        End If
        _owner.InvalidateScrollInfo()
    End Sub

    Private Sub ResetScrollInfo()
        _offset.X = 0
        _offset.Y = 0
    End Sub

    Private Function GetNextSectionClosestIndex(itemIndex As Integer) As Integer
        Dim abstractItem = _abstractPanel(itemIndex)
        If abstractItem.Section < _abstractPanel.SectionCount - 1 Then
            Dim ret = _abstractPanel.Where(Function(x) x.Section = abstractItem.Section + 1).OrderBy(Function(x) Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).First()
            Return ret._index
        Else
            Return itemIndex
        End If
    End Function

    Private Function GetLastSectionClosestIndex(itemIndex As Integer) As Integer
        Dim abstractItem = _abstractPanel(itemIndex)
        If abstractItem.Section > 0 Then
            Dim ret = _abstractPanel.Where(Function(x) x.Section = abstractItem.Section - 1).OrderBy(Function(x) Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).First()
            Return ret._index
        Else
            Return itemIndex
        End If
    End Function

    Private Sub NavigateDown()
        Dim gen = _generator.GetItemContainerGeneratorForPanel(Me)
        Dim selected As UIElement = DirectCast(Keyboard.FocusedElement, UIElement)
        Dim itemIndex As Integer = gen.IndexFromContainer(selected)
        Dim depth As Integer = 0
        While itemIndex = -1
            selected = DirectCast(VisualTreeHelper.GetParent(selected), UIElement)
            itemIndex = gen.IndexFromContainer(selected)
            depth += 1
        End While
        Dim [next] As DependencyObject = Nothing
        If Orientation = Orientation.Horizontal Then
            Dim nextIndex As Integer = GetNextSectionClosestIndex(itemIndex)
            [next] = gen.ContainerFromIndex(nextIndex)
            While [next] Is Nothing
                SetVerticalOffset(VerticalOffset + 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(nextIndex)
            End While
        Else
            If itemIndex = _abstractPanel._itemCount - 1 Then
                Return
            End If
            [next] = gen.ContainerFromIndex(itemIndex + 1)
            While [next] Is Nothing
                SetHorizontalOffset(HorizontalOffset + 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(itemIndex + 1)
            End While
        End If
        While depth <> 0
            [next] = VisualTreeHelper.GetChild([next], 0)
            depth -= 1
        End While
        TryCast([next], UIElement).Focus()
    End Sub

    Private Sub NavigateLeft()
        Dim gen = _generator.GetItemContainerGeneratorForPanel(Me)

        Dim selected As UIElement = DirectCast(Keyboard.FocusedElement, UIElement)
        Dim itemIndex As Integer = gen.IndexFromContainer(selected)
        Dim depth As Integer = 0
        While itemIndex = -1
            selected = DirectCast(VisualTreeHelper.GetParent(selected), UIElement)
            itemIndex = gen.IndexFromContainer(selected)
            depth += 1
        End While
        Dim [next] As DependencyObject = Nothing
        If Orientation = Orientation.Vertical Then
            Dim nextIndex As Integer = GetLastSectionClosestIndex(itemIndex)
            [next] = gen.ContainerFromIndex(nextIndex)
            While [next] Is Nothing
                SetHorizontalOffset(HorizontalOffset - 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(nextIndex)
            End While
        Else
            If itemIndex = 0 Then
                Return
            End If
            [next] = gen.ContainerFromIndex(itemIndex - 1)
            While [next] Is Nothing
                SetVerticalOffset(VerticalOffset - 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(itemIndex - 1)
            End While
        End If
        While depth <> 0
            [next] = VisualTreeHelper.GetChild([next], 0)
            depth -= 1
        End While
        TryCast([next], UIElement).Focus()
    End Sub

    Private Sub NavigateRight()
        Dim gen = _generator.GetItemContainerGeneratorForPanel(Me)
        Dim selected As UIElement = DirectCast(Keyboard.FocusedElement, UIElement)
        Dim itemIndex As Integer = gen.IndexFromContainer(selected)
        Dim depth As Integer = 0
        While itemIndex = -1
            selected = DirectCast(VisualTreeHelper.GetParent(selected), UIElement)
            itemIndex = gen.IndexFromContainer(selected)
            depth += 1
        End While
        Dim [next] As DependencyObject = Nothing
        If Orientation = Orientation.Vertical Then
            Dim nextIndex As Integer = GetNextSectionClosestIndex(itemIndex)
            [next] = gen.ContainerFromIndex(nextIndex)
            While [next] Is Nothing
                SetHorizontalOffset(HorizontalOffset + 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(nextIndex)
            End While
        Else
            If itemIndex = _abstractPanel._itemCount - 1 Then
                Return
            End If
            [next] = gen.ContainerFromIndex(itemIndex + 1)
            While [next] Is Nothing
                SetVerticalOffset(VerticalOffset + 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(itemIndex + 1)
            End While
        End If
        While depth <> 0
            [next] = VisualTreeHelper.GetChild([next], 0)
            depth -= 1
        End While
        TryCast([next], UIElement).Focus()
    End Sub

    Private Sub NavigateUp()
        Dim gen = _generator.GetItemContainerGeneratorForPanel(Me)
        Dim selected As UIElement = DirectCast(Keyboard.FocusedElement, UIElement)
        Dim itemIndex As Integer = gen.IndexFromContainer(selected)
        Dim depth As Integer = 0
        While itemIndex = -1
            selected = DirectCast(VisualTreeHelper.GetParent(selected), UIElement)
            itemIndex = gen.IndexFromContainer(selected)
            depth += 1
        End While
        Dim [next] As DependencyObject = Nothing
        If Orientation = Orientation.Horizontal Then
            Dim nextIndex As Integer = GetLastSectionClosestIndex(itemIndex)
            [next] = gen.ContainerFromIndex(nextIndex)
            While [next] Is Nothing
                SetVerticalOffset(VerticalOffset - 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(nextIndex)
            End While
        Else
            If itemIndex = 0 Then
                Return
            End If
            [next] = gen.ContainerFromIndex(itemIndex - 1)
            While [next] Is Nothing
                SetHorizontalOffset(HorizontalOffset - 1)
                UpdateLayout()
                [next] = gen.ContainerFromIndex(itemIndex - 1)
            End While
        End If
        While depth <> 0
            [next] = VisualTreeHelper.GetChild([next], 0)
            depth -= 1
        End While
        TryCast([next], UIElement).Focus()
    End Sub


#End Region

#Region "Override"

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.Key
            Case Key.Down
                NavigateDown()
                e.Handled = True
                Exit Select
            Case Key.Left
                NavigateLeft()
                e.Handled = True
                Exit Select
            Case Key.Right
                NavigateRight()
                e.Handled = True
                Exit Select
            Case Key.Up
                NavigateUp()
                e.Handled = True
                Exit Select
            Case Else
                MyBase.OnKeyDown(e)
                Exit Select
        End Select
    End Sub


    Protected Overrides Sub OnItemsChanged(sender As Object, args As ItemsChangedEventArgs)
        MyBase.OnItemsChanged(sender, args)
        _abstractPanel = Nothing
        ResetScrollInfo()
    End Sub

    Protected Overrides Sub OnInitialized(e As EventArgs)
        MyBase.OnInitialized(e)
        _itemsControl = ItemsControl.GetItemsOwner(Me)
        _children = InternalChildren
        _generator = ItemContainerGenerator
        AddHandler Me.SizeChanged, New SizeChangedEventHandler(AddressOf Me.Resizing)
    End Sub

    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        If _itemsControl Is Nothing OrElse _itemsControl.Items.Count = 0 Then
            Return availableSize
        End If
        If _abstractPanel Is Nothing Then
            _abstractPanel = New WrapPanelAbstraction(_itemsControl.Items.Count)
        End If

        _pixelMeasuredViewport = availableSize

        _realizedChildLayout.Clear()

        Dim realizedFrameSize As Size = availableSize

        Dim itemCount As Integer = _itemsControl.Items.Count
        Dim firstVisibleIndex As Integer = GetFirstVisibleIndex()

        Dim startPos As GeneratorPosition = _generator.GeneratorPositionFromIndex(firstVisibleIndex)

        Dim childIndex As Integer = If((startPos.Offset = 0), startPos.Index, startPos.Index + 1)
        Dim current As Integer = firstVisibleIndex
        Dim visibleSections As Integer = 1
        Using _generator.StartAt(startPos, GeneratorDirection.Forward, True)
            Dim [stop] As Boolean = False
            Dim isHorizontal As Boolean = Orientation = Orientation.Horizontal
            Dim currentX As Double = 0
            Dim currentY As Double = 0
            Dim maxItemSize As Double = 0
            Dim currentSection As Integer = GetFirstVisibleSection()
            While current < itemCount
                Dim newlyRealized As Boolean

                ' Get or create the child                    
                Dim child As UIElement = TryCast(_generator.GenerateNext(newlyRealized), UIElement)
                If newlyRealized Then
                    ' Figure out if we need to insert the child at the end or somewhere in the middle
                    If childIndex >= _children.Count Then
                        MyBase.AddInternalChild(child)
                    Else
                        MyBase.InsertInternalChild(childIndex, child)
                    End If
                    _generator.PrepareItemContainer(child)
                    child.Measure(ChildSlotSize)
                Else
                    ' The child has already been created, let's be sure it's in the right spot
                    'Debug.Assert(child Is _children(childIndex), "Wrong child was generated")
                End If
                childSize = child.DesiredSize
                Dim childRect As New Rect(New Point(currentX, currentY), childSize)
                If isHorizontal Then
                    maxItemSize = Math.Max(maxItemSize, childRect.Height)
                    If childRect.Right > realizedFrameSize.Width Then
                        'wrap to a new line
                        currentY = currentY + maxItemSize
                        currentX = 0
                        maxItemSize = childRect.Height
                        childRect.X = currentX
                        childRect.Y = currentY
                        currentSection += 1
                        visibleSections += 1
                    End If
                    If currentY > realizedFrameSize.Height Then
                        [stop] = True
                    End If
                    currentX = childRect.Right
                Else
                    maxItemSize = Math.Max(maxItemSize, childRect.Width)
                    If childRect.Bottom > realizedFrameSize.Height Then
                        'wrap to a new column
                        currentX = currentX + maxItemSize
                        currentY = 0
                        maxItemSize = childRect.Width
                        childRect.X = currentX
                        childRect.Y = currentY
                        currentSection += 1
                        visibleSections += 1
                    End If
                    If currentX > realizedFrameSize.Width Then
                        [stop] = True
                    End If
                    currentY = childRect.Bottom
                End If
                _realizedChildLayout.Add(child, childRect)
                _abstractPanel.SetItemSection(current, currentSection)

                If [stop] Then
                    Exit While
                End If
                current += 1
                childIndex += 1
            End While
        End Using
        CleanUpItems(firstVisibleIndex, current - 1)

        ComputeExtentAndViewport(availableSize, visibleSections)

        Return availableSize
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        If _children IsNot Nothing Then
            For Each child As UIElement In _children
                Dim layoutInfo = _realizedChildLayout(child)
                child.Arrange(layoutInfo)
            Next
        End If
        Return finalSize
    End Function

#End Region

#Region "IScrollInfo Members"

    Private _canHScroll As Boolean = False
    Public Property CanHorizontallyScroll() As Boolean Implements IScrollInfo.CanHorizontallyScroll
        Get
            Return _canHScroll
        End Get
        Set(value As Boolean)
            _canHScroll = value
        End Set
    End Property

    Private _canVScroll As Boolean = False
    Public Property CanVerticallyScroll() As Boolean Implements IScrollInfo.CanVerticallyScroll
        Get
            Return _canVScroll
        End Get
        Set(value As Boolean)
            _canVScroll = value
        End Set
    End Property

    Public ReadOnly Property ExtentHeight() As Double Implements IScrollInfo.ExtentHeight
        Get
            Return _extent.Height
        End Get
    End Property

    Public ReadOnly Property ExtentWidth() As Double Implements IScrollInfo.ExtentWidth
        Get
            Return _extent.Width
        End Get
    End Property

    Public ReadOnly Property HorizontalOffset() As Double Implements IScrollInfo.HorizontalOffset
        Get
            Return _offset.X
        End Get
    End Property

    Public ReadOnly Property VerticalOffset() As Double Implements IScrollInfo.VerticalOffset
        Get
            Return _offset.Y
        End Get
    End Property

    Public Sub LineDown() Implements IScrollInfo.LineDown
        If Orientation = Orientation.Vertical Then
            SetVerticalOffset(VerticalOffset + 20)
        Else
            SetVerticalOffset(VerticalOffset + 1)
        End If
    End Sub

    Public Sub LineLeft() Implements IScrollInfo.LineLeft
        If Orientation = Orientation.Horizontal Then
            SetHorizontalOffset(HorizontalOffset - 20)
        Else
            SetHorizontalOffset(HorizontalOffset - 1)
        End If
    End Sub

    Public Sub LineRight() Implements IScrollInfo.LineRight
        If Orientation = Orientation.Horizontal Then
            SetHorizontalOffset(HorizontalOffset + 20)
        Else
            SetHorizontalOffset(HorizontalOffset + 1)
        End If
    End Sub

    Public Sub LineUp() Implements IScrollInfo.LineUp
        If Orientation = Orientation.Vertical Then
            SetVerticalOffset(VerticalOffset - 20)
        Else
            SetVerticalOffset(VerticalOffset - 1)
        End If
    End Sub

    Public Function MakeVisible(visual As Visual, rectangle As Rect) As Rect Implements IScrollInfo.MakeVisible
        Dim gen = DirectCast(_generator.GetItemContainerGeneratorForPanel(Me), ItemContainerGenerator)
        Dim element = DirectCast(visual, UIElement)
        Dim itemIndex As Integer = gen.IndexFromContainer(element)
        While itemIndex = -1
            element = DirectCast(VisualTreeHelper.GetParent(element), UIElement)
            itemIndex = gen.IndexFromContainer(element)
        End While
        Dim section As Integer = _abstractPanel(itemIndex).Section
        Dim elementRect As Rect = _realizedChildLayout(element)
        If Orientation = Orientation.Horizontal Then
            Dim viewportHeight As Double = _pixelMeasuredViewport.Height
            If elementRect.Bottom > viewportHeight Then
                _offset.Y += 1
            ElseIf elementRect.Top < 0 Then
                _offset.Y -= 1
            End If
        Else
            Dim viewportWidth As Double = _pixelMeasuredViewport.Width
            If elementRect.Right > viewportWidth Then
                _offset.X += 1
            ElseIf elementRect.Left < 0 Then
                _offset.X -= 1
            End If
        End If
        InvalidateMeasure()
        Return elementRect
    End Function

    Public Sub MouseWheelDown() Implements IScrollInfo.MouseWheelDown
        PageDown()
    End Sub

    Public Sub MouseWheelLeft() Implements IScrollInfo.MouseWheelLeft
        PageLeft()
    End Sub

    Public Sub MouseWheelRight() Implements IScrollInfo.MouseWheelRight
        PageRight()
    End Sub

    Public Sub MouseWheelUp() Implements IScrollInfo.MouseWheelUp
        PageUp()
    End Sub

    Public Sub PageDown() Implements IScrollInfo.PageDown
        SetVerticalOffset(VerticalOffset + _viewport.Height * 0.8)
    End Sub

    Public Sub PageLeft() Implements IScrollInfo.PageLeft
        SetHorizontalOffset(HorizontalOffset - _viewport.Width * 0.8)
    End Sub

    Public Sub PageRight() Implements IScrollInfo.PageRight
        SetHorizontalOffset(HorizontalOffset + _viewport.Width * 0.8)
    End Sub

    Public Sub PageUp() Implements IScrollInfo.PageUp
        SetVerticalOffset(VerticalOffset - _viewport.Height * 0.8)
    End Sub

    Private _owner As ScrollViewer
    Public Property ScrollOwner() As ScrollViewer Implements IScrollInfo.ScrollOwner
        Get
            Return _owner
        End Get
        Set(value As ScrollViewer)
            _owner = value
        End Set
    End Property

    Public Sub SetHorizontalOffset(offset As Double) Implements IScrollInfo.SetHorizontalOffset
        If offset < 0 OrElse _viewport.Width >= _extent.Width Then
            offset = 0
        Else
            If offset + _viewport.Width >= _extent.Width Then
                offset = _extent.Width - _viewport.Width
            End If
        End If

        _offset.X = offset

        If _owner IsNot Nothing Then
            _owner.InvalidateScrollInfo()
        End If

        InvalidateMeasure()
        firstIndex = GetFirstVisibleIndex()
    End Sub

    Public Sub SetVerticalOffset(offset As Double) Implements IScrollInfo.SetVerticalOffset
        If offset < 0 OrElse _viewport.Height >= _extent.Height Then
            offset = 0
        Else
            If offset + _viewport.Height >= _extent.Height Then
                offset = _extent.Height - _viewport.Height
            End If
        End If

        _offset.Y = offset

        If _owner IsNot Nothing Then
            _owner.InvalidateScrollInfo()
        End If

        '_trans.Y = -offset;

        InvalidateMeasure()
        firstIndex = GetFirstVisibleIndex()
    End Sub

    Public ReadOnly Property ViewportHeight() As Double Implements IScrollInfo.ViewportHeight
        Get
            Return _viewport.Height
        End Get
    End Property

    Public ReadOnly Property ViewportWidth() As Double Implements IScrollInfo.ViewportWidth
        Get
            Return _viewport.Width
        End Get
    End Property

#End Region

#Region "helper data structures"

    Private Class ItemAbstraction
        Public Sub New(panel As WrapPanelAbstraction, index As Integer)
            _panel = panel
            _index = index
        End Sub

        Private _panel As WrapPanelAbstraction

        Public ReadOnly _index As Integer

        Private _sectionIndex As Integer = -1
        Public Property SectionIndex() As Integer
            Get
                If _sectionIndex = -1 Then
                    Return _index Mod _panel._averageItemsPerSection - 1
                End If
                Return _sectionIndex
            End Get
            Set(value As Integer)
                If _sectionIndex = -1 Then
                    _sectionIndex = value
                End If
            End Set
        End Property

        Private _section As Integer = -1
        Public Property Section() As Integer
            Get
                If _section = -1 Then
                    Return _index \ _panel._averageItemsPerSection
                End If
                Return _section
            End Get
            Set(value As Integer)
                If _section = -1 Then
                    _section = value
                End If
            End Set
        End Property
    End Class

    Private Class WrapPanelAbstraction
        Implements IEnumerable(Of ItemAbstraction)
        Public Sub New(itemCount As Integer)
            Dim items__1 As New List(Of ItemAbstraction)(itemCount)
            For i As Integer = 0 To itemCount - 1
                Dim item As New ItemAbstraction(Me, i)
                items__1.Add(item)
            Next

            Items = New ReadOnlyCollection(Of ItemAbstraction)(items__1)
            _averageItemsPerSection = itemCount
            _itemCount = itemCount
        End Sub

        Public ReadOnly _itemCount As Integer
        Public _averageItemsPerSection As Integer
        Private _currentSetSection As Integer = -1
        Private _currentSetItemIndex As Integer = -1
        Private _itemsInCurrentSecction As Integer = 0
        Private _syncRoot As New Object()

        Public ReadOnly Property SectionCount() As Integer
            Get
                Dim ret As Integer = _currentSetSection + 1
                If _currentSetItemIndex + 1 < Items.Count Then
                    Dim itemsLeft As Integer = Items.Count - _currentSetItemIndex
                    ret += itemsLeft \ _averageItemsPerSection + 1
                End If
                Return ret
            End Get
        End Property

        Private Property Items() As ReadOnlyCollection(Of ItemAbstraction)
            Get
                Return m_Items
            End Get
            Set(value As ReadOnlyCollection(Of ItemAbstraction))
                m_Items = value
            End Set
        End Property
        Private m_Items As ReadOnlyCollection(Of ItemAbstraction)

        Public Sub SetItemSection(index As Integer, section As Integer)
            SyncLock _syncRoot
                If section <= _currentSetSection + 1 AndAlso index = _currentSetItemIndex + 1 Then
                    _currentSetItemIndex += 1
                    Items(index).Section = section
                    If section = _currentSetSection + 1 Then
                        _currentSetSection = section
                        If section > 0 Then
                            _averageItemsPerSection = (index) \ (section)
                        End If
                        _itemsInCurrentSecction = 1
                    Else
                        _itemsInCurrentSecction += 1
                    End If
                    Items(index).SectionIndex = _itemsInCurrentSecction - 1
                End If
            End SyncLock
        End Sub

        Default Public ReadOnly Property Item(index As Integer) As ItemAbstraction
            Get
                Return Items(index)
            End Get
        End Property

#Region "IEnumerable<ItemAbstraction> Members"

        Public Function GetEnumerator() As IEnumerator(Of ItemAbstraction) Implements IEnumerable(Of VirtualizingWrapPanel.ItemAbstraction).GetEnumerator
            Return Items.GetEnumerator()
        End Function

#End Region

#Region "IEnumerable Members"

        Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function

#End Region
    End Class

#End Region
End Class