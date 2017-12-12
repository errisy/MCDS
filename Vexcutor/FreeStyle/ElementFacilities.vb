Imports System.Runtime.Serialization, System.Reflection, System.Windows.Controls, System.Windows.Media, System.Windows.Shapes, System.Windows.Input, System.Windows


Public Module Click
    <System.Runtime.CompilerServices.Extension()> Public Function ClickAdd(Of TItem As TreeViewItem)(Host As TreeViewItem, Creator As Func(Of TItem)) As RoutedEventHandler
        Return Sub(sender As Object, e As RoutedEventArgs)
                   Host.Items.Add(Creator.Invoke)
                   Host.ExpandSubtree()
               End Sub
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ClickRemove(Host As TreeViewItem) As RoutedEventHandler
        Return Sub(sender As Object, e As RoutedEventArgs)
                   If TypeOf Host.Parent Is TreeViewItem Then
                       CType(Host.Parent, TreeViewItem).Items.Remove(Host)
                   ElseIf TypeOf Host.Parent Is TreeView Then
                       CType(Host.Parent, TreeView).Items.Remove(Host)
                   End If
               End Sub
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ClickRemove(Host As TreeViewItem, AdditionalAction As System.Action) As RoutedEventHandler
        Return Sub(sender As Object, e As RoutedEventArgs)
                   If TypeOf Host.Parent Is TreeViewItem Then
                       CType(Host.Parent, TreeViewItem).Items.Remove(Host)
                   ElseIf TypeOf Host.Parent Is TreeView Then
                       CType(Host.Parent, TreeView).Items.Remove(Host)
                   End If
                   AdditionalAction.Invoke()
               End Sub
    End Function
End Module

Public Module ClickAdd
    <System.Runtime.CompilerServices.Extension()> Public Sub ClickToClose(value As FrameworkElement, target As FrameworkElement, nType As Type)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Add(value, New KeyValuePair(Of FrameworkElement, Type)(target, nType))
            AddHandler value.Unloaded, AddressOf OnUnLoaded
            AddHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub UnBind(value As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Remove(value)
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    Private Sub OnUnLoaded(sender As Object, e As RoutedEventArgs)
        If DragMoveBindings.ContainsKey(sender) Then
            DragMoveBindings.Remove(sender)
            Dim value As FrameworkElement = sender
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    Private StartPoint As System.Windows.Vector
    Private StartLocation As System.Windows.Vector
    Private MouseCatcher As IFrameworkInputElement
    Private DragMoveBindings As New Dictionary(Of FrameworkElement, KeyValuePair(Of FrameworkElement, Type))
    Private Sub OnStartDrag(sender As Object, e As MouseButtonEventArgs)
        Dim target As FrameworkElement = DragMoveBindings(sender).Key
        Dim vType As Type = DragMoveBindings(sender).Value
        AddHandler target.MouseDown, Sub(senderx As Object, ex As MouseButtonEventArgs)

                                     End Sub
        Dim tp As Object = target.Parent
        If TypeOf tp Is Canvas Then
            DirectCast(tp, Canvas).Children.Remove(target)
        ElseIf TypeOf tp Is Grid Then
            DirectCast(tp, Grid).Children.Remove(target)
        ElseIf TypeOf tp Is StackPanel Then
            DirectCast(tp, StackPanel).Children.Remove(target)
        ElseIf TypeOf tp Is TreeViewItem Then
            DirectCast(tp, TreeViewItem).Items.Remove(target)
        ElseIf TypeOf tp Is TreeView Then
            DirectCast(tp, TreeView).Items.Remove(target)
        End If
    End Sub


End Module

Public Module ClickClose
    <System.Runtime.CompilerServices.Extension()> Public Sub ClickToClose(value As FrameworkElement, target As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Add(value, target)
            AddHandler value.Unloaded, AddressOf OnUnLoaded
            AddHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub UnBind(value As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Remove(value)
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    Private Sub OnUnLoaded(sender As Object, e As RoutedEventArgs)
        If DragMoveBindings.ContainsKey(sender) Then
            DragMoveBindings.Remove(sender)
            Dim value As FrameworkElement = sender
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
        End If
    End Sub
    Private StartPoint As System.Windows.Vector
    Private StartLocation As System.Windows.Vector
    Private MouseCatcher As IFrameworkInputElement
    Private DragMoveBindings As New Dictionary(Of FrameworkElement, FrameworkElement)
    Private Sub OnStartDrag(sender As Object, e As MouseButtonEventArgs)
        Dim target As FrameworkElement = DragMoveBindings(sender)
        Dim tp As Object = target.Parent
        If TypeOf tp Is Canvas Then
            DirectCast(tp, Canvas).Children.Remove(target)
        ElseIf TypeOf tp Is Grid Then
            DirectCast(tp, Grid).Children.Remove(target)
        ElseIf TypeOf tp Is StackPanel Then
            DirectCast(tp, StackPanel).Children.Remove(target)
        ElseIf TypeOf tp Is TreeViewItem Then
            DirectCast(tp, TreeViewItem).Items.Remove(target)
        ElseIf TypeOf tp Is TreeView Then
            DirectCast(tp, TreeView).Items.Remove(target)
        End If
    End Sub
End Module
Public Module DragOrder
    <System.Runtime.CompilerServices.Extension()> Public Sub DragToOrder(value As FrameworkElement, target As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Add(value, target)
            AddHandler value.Unloaded, AddressOf OnUnLoaded
            AddHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            AddHandler value.MouseMove, AddressOf OnDragging
            AddHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub

    <System.Runtime.CompilerServices.Extension()> Public Sub UnBind(value As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Remove(value)
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private Sub OnUnLoaded(sender As Object, e As RoutedEventArgs)
        If DragMoveBindings.ContainsKey(sender) Then
            DragMoveBindings.Remove(sender)
            Dim value As FrameworkElement = sender
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private StartPoint As System.Windows.Vector
    Private StartLocation As System.Windows.Vector
    Private MouseCatcher As IFrameworkInputElement
    Private DragMoveBindings As New Dictionary(Of FrameworkElement, FrameworkElement)
    Private Sub OnStartDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            StartLocation = DragMoveBindings(sender).GetCanvasLocation
            StartPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
            MouseCatcher = sender
            MouseCatcher.CaptureMouse()
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnDragging(sender As Object, e As MouseEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetCanvasLocation(StartLocation + CurrentPoint - StartPoint)
                'frmLambdaHost.Text = CurrentPoint.ToString
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnEndDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetCanvasLocation(StartLocation + CurrentPoint - StartPoint)
                MouseCatcher.ReleaseMouseCapture()
                MouseCatcher = Nothing
                'frmLambdaHost.Text = CurrentPoint.ToString
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    '<System.Runtime.CompilerServices.Extension()> Public Function GetCanvasLocation(value As FrameworkElement) As System.Windows.Vector
    '    Return New System.Windows.Vector(Canvas.GetLeft(value), Canvas.GetTop(value))
    'End Function
    '<System.Runtime.CompilerServices.Extension()> Public Sub SetCanvasLocation(value As FrameworkElement, p As System.Windows.Vector)
    '    Canvas.SetLeft(value, p.X)
    '    Canvas.SetTop(value, p.Y)
    'End Sub
End Module
Public Module DragSize
    <System.Runtime.CompilerServices.Extension()> Public Sub DragToResize(value As FrameworkElement, target As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Add(value, target)
            AddHandler value.Unloaded, AddressOf OnUnLoaded
            AddHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            AddHandler value.MouseMove, AddressOf OnDragging
            AddHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub UnBind(value As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Remove(value)
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private Sub OnUnLoaded(sender As Object, e As RoutedEventArgs)
        If DragMoveBindings.ContainsKey(sender) Then
            DragMoveBindings.Remove(sender)
            Dim value As FrameworkElement = sender
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private StartPoint As System.Windows.Vector
    Private StartLocation As System.Windows.Vector
    Private MouseCatcher As IFrameworkInputElement
    Private DragMoveBindings As New Dictionary(Of FrameworkElement, FrameworkElement)
    Private Sub OnStartDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            StartLocation = DragMoveBindings(sender).GetSize
            StartPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
            MouseCatcher = sender
            MouseCatcher.CaptureMouse()
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnDragging(sender As Object, e As MouseEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetSize(StartLocation + CurrentPoint - StartPoint)
                'frmLambdaHost.Text = CurrentPoint.ToString + StartLocation.ToString + StartPoint.ToString
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnEndDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetSize(StartLocation + CurrentPoint - StartPoint)
                MouseCatcher.ReleaseMouseCapture()
                MouseCatcher = Nothing
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function GetSize(value As FrameworkElement) As System.Windows.Vector
        Return New System.Windows.Vector(value.Width, value.Height)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub SetSize(value As FrameworkElement, p As System.Windows.Vector)
        If p.X < 32 Then p.X = 32D
        If p.Y < 32 Then p.Y = 32D
        value.Width = p.X
        value.Height = p.Y
    End Sub
End Module

Public Module DragMove
    <System.Runtime.CompilerServices.Extension()> Public Sub DragToMove(value As FrameworkElement, target As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Add(value, target)
            AddHandler value.Unloaded, AddressOf OnUnLoaded
            AddHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            AddHandler value.MouseMove, AddressOf OnDragging
            AddHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub

    <System.Runtime.CompilerServices.Extension()> Public Sub UnBind(value As FrameworkElement)
        If Not DragMoveBindings.ContainsKey(value) Then
            DragMoveBindings.Remove(value)
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private Sub OnUnLoaded(sender As Object, e As RoutedEventArgs)
        If DragMoveBindings.ContainsKey(sender) Then
            DragMoveBindings.Remove(sender)
            Dim value As FrameworkElement = sender
            RemoveHandler value.Unloaded, AddressOf OnUnLoaded
            RemoveHandler value.MouseLeftButtonDown, AddressOf OnStartDrag
            RemoveHandler value.MouseMove, AddressOf OnDragging
            RemoveHandler value.MouseLeftButtonUp, AddressOf OnEndDrag
        End If
    End Sub
    Private StartPoint As System.Windows.Vector
    Private StartLocation As System.Windows.Vector
    Private MouseCatcher As IFrameworkInputElement
    Private DragMoveBindings As New Dictionary(Of FrameworkElement, FrameworkElement)
    Private Sub OnStartDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            StartLocation = DragMoveBindings(sender).GetCanvasLocation
            StartPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
            MouseCatcher = sender
            MouseCatcher.CaptureMouse()
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnDragging(sender As Object, e As MouseEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetCanvasLocation(StartLocation + CurrentPoint - StartPoint)
                'frmLambdaHost.Text = CurrentPoint.ToString
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    Private Sub OnEndDrag(sender As Object, e As MouseButtonEventArgs)
        Try
            If MouseCatcher Is sender Then
                Dim CurrentPoint = DirectCast(sender, FrameworkElement).PointToScreen(e.GetPosition(sender))
                DragMoveBindings(sender).SetCanvasLocation(StartLocation + CurrentPoint - StartPoint)
                MouseCatcher.ReleaseMouseCapture()
                MouseCatcher = Nothing
                'frmLambdaHost.Text = CurrentPoint.ToString
            End If
        Catch ex As Exception
            UnBind(sender)
        End Try
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function GetCanvasLocation(value As System.Windows.FrameworkElement) As System.Windows.Vector
        Return New System.Windows.Vector(Canvas.GetLeft(value), Canvas.GetTop(value))
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub SetCanvasLocation(value As System.Windows.FrameworkElement, p As System.Windows.Vector)
        Canvas.SetLeft(value, p.X)
        Canvas.SetTop(value, p.Y)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub SetCanvasLocation(value As System.Windows.FrameworkElement, p As System.Windows.Point)
        Canvas.SetLeft(value, p.X)
        Canvas.SetTop(value, p.Y)
    End Sub
End Module