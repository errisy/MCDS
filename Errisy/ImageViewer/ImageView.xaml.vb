Public Class ImageView
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Timer = New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 0, 16)}
        Dim scr = System.Windows.Forms.Screen.FromHandle(New System.Windows.Interop.WindowInteropHelper(Me.Parent).Handle)
        ScreenWidth = scr.Bounds.Width
        ScreenHeight = scr.Bounds.Height
    End Sub
    Protected ScreenWidth As Double
    Protected ScreenHeight As Double
    Protected Friend WithEvents Timer As System.Windows.Threading.DispatcherTimer
    Protected Maneuvers As New List(Of Maneuver)
    Protected PageCount As Integer = 200
    Protected PageCountDown As Integer = PageCount
    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        If IsDragging Then Exit Sub
        If PageShift <> 0 Then PageCountDown -= 1
        If PageCountDown = 0 Then
            If PageShift > 0 Then NextFile()
            If PageShift < 0 Then LastFile()
            PageCountDown = PageCount
        End If
        Dim changed As Boolean = False
        Dim rmList As New List(Of Maneuver)
        Dim Accumulate As Vector = New Vector(0.0#, 0.0#)
        For Each m In Maneuvers
            m.Tick()
            If m.Alive Then
                Offset += m.MoveSpeed
                Accumulate += m.MoveSpeed
                Angle += m.RotateSpeed

                Dim del = m.ZoomCenter - New Point(Image.ActualWidth / 2, Image.ActualHeight / 2)
                Dim deltaZoom = m.ZoomSpeed + 1
                Offset -= del * (deltaZoom - 1) * Zoom
                Zoom *= deltaZoom

                changed = True
            Else
                rmList.Add(m)
            End If
        Next
        For Each m In rmList
            Maneuvers.Remove(m)
        Next
        'Bounce
        Dim xFactor As Double = Image.ActualWidth * Zoom / 2 + ScreenWidth / 2
        Dim yFactor As Double = Image.ActualHeight * Zoom / 2 + ScreenHeight / 2
        If (Offset.X - xFactor > 0.0# AndAlso Accumulate.X > 0.0#) OrElse (Offset.X + xFactor < 0.0# AndAlso Accumulate.X < 0.0#) Then
            For Each m In Maneuvers
                m.BounceX()
            Next
            NextFile()
        End If
        If (Offset.Y - yFactor > 0.0# AndAlso Accumulate.Y > 0.0#) OrElse (Offset.Y + yFactor < 0.0# AndAlso Accumulate.Y < 0.0#) Then
            For Each m In Maneuvers
                m.BounceY()
            Next
        End If
        If changed Then Image.RenderTransform = ImageTransform
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Image.RenderTransformOrigin = New Point(0.5#, 0.5#)
        LoadCurrentFile()
    End Sub

    Private NextImageTask As Task(Of BitmapImage)
    Private LastImageTask As Task(Of BitmapImage)
    Private Async Sub LoadCurrentFile()
        If CurrentFile IsNot Nothing AndAlso CurrentFile.Exists Then
            Dim bmpTask As New Task(Of BitmapImage)(Function()
                                                        Dim bmp As New System.Windows.Media.Imaging.BitmapImage
                                                        bmp.BeginInit()
                                                        bmp.UriSource = New Uri("File://" + CurrentFile.FullName)
                                                        bmp.CacheOption = BitmapCacheOption.OnLoad
                                                        bmp.EndInit()
                                                        bmp.Freeze()
                                                        Return bmp
                                                    End Function)
            bmpTask.Start()
            Image.Source = Await bmpTask
            Dim i As Integer = FileList.IndexOf(CurrentFile)
            Dim oldbmp As BitmapImage = Image.Source
            Dim nf = GetFileByIndex(i + 1)
            If nf Is Nothing Then
                NextImageTask = New Task(Of BitmapImage)(Function()
                                                             Return Nothing
                                                         End Function)
                NextImageTask.Start()
            ElseIf nf Is CurrentFile Then
                NextImageTask = New Task(Of BitmapImage)(Function()
                                                             Return oldbmp
                                                         End Function)
                NextImageTask.Start()
            Else
                Dim newfile As String = nf.FullName
                NextImageTask = New Task(Of BitmapImage)(Function()
                                                             Dim bi As New System.Windows.Media.Imaging.BitmapImage
                                                             bi.BeginInit()
                                                             bi.UriSource = New Uri("File://" + newfile)
                                                             bi.CacheOption = BitmapCacheOption.OnLoad
                                                             bi.EndInit()

                                                             bi.Freeze()
                                                             Return bi
                                                         End Function)
                NextImageTask.Start()
            End If
            Dim lf = GetFileByIndex(i - 1)
            If lf Is Nothing Then
                LastImageTask = New Task(Of BitmapImage)(Function()
                                                             Return Nothing
                                                         End Function)
                LastImageTask.Start()
            ElseIf lf Is CurrentFile Then
                LastImageTask = New Task(Of BitmapImage)(Function()
                                                             Return oldbmp
                                                         End Function)
                LastImageTask.Start()
            Else
                Dim newfile As String = lf.FullName
                LastImageTask = New Task(Of BitmapImage)(Function()
                                                             Dim bi As New System.Windows.Media.Imaging.BitmapImage
                                                             bi.BeginInit()
                                                             bi.UriSource = New Uri("File://" + newfile)
                                                             bi.CacheOption = BitmapCacheOption.OnLoad
                                                             bi.EndInit()

                                                             bi.Freeze()
                                                             Return bi
                                                         End Function)
                LastImageTask.Start()
            End If
        End If
    End Sub
    Private Sub LoadCurrentFile(bmp As BitmapImage)
        Image.Source = bmp
    End Sub
    Private Sub NextFile()
        If Not NextImageTask.IsCompleted Then NextImageTask.Wait()
        Dim i As Integer = FileList.IndexOf(CurrentFile)
        i += 1
        If i = FileList.Count Then i = 0
        CurrentFile = FileList(i)
        Dim oldbmp As BitmapImage = Image.Source
        LoadCurrentFile(NextImageTask.Result)
        Dim nf = GetFileByIndex(i + 1)
        If nf Is Nothing Then
            NextImageTask = New Task(Of BitmapImage)(Function()
                                                         Return Nothing
                                                     End Function)
            NextImageTask.Start()
        ElseIf nf Is CurrentFile Then
            NextImageTask = New Task(Of BitmapImage)(Function()
                                                         Return oldbmp
                                                     End Function)
            NextImageTask.Start()
        Else
            Dim newfile As String = nf.FullName
            NextImageTask = New Task(Of BitmapImage)(Function()
                                                         Dim bi As New System.Windows.Media.Imaging.BitmapImage
                                                         bi.BeginInit()
                                                         bi.UriSource = New Uri("File://" + newfile)
                                                         bi.CacheOption = BitmapCacheOption.OnLoad
                                                         bi.EndInit()
                                                         bi.Freeze()
                                                         Return bi
                                                     End Function)
            NextImageTask.Start()
        End If
        LastImageTask = New Task(Of BitmapImage)(Function()
                                                     Return oldbmp
                                                 End Function)
        LastImageTask.Start()
    End Sub
    Private Sub LastFile()
        If Not LastImageTask.IsCompleted Then LastImageTask.Wait()
        Dim i As Integer = FileList.IndexOf(CurrentFile)
        i -= 1
        If i < 0 Then i = FileList.Count - 1
        CurrentFile = FileList(i)
        Dim oldbmp As BitmapImage = Image.Source
        LoadCurrentFile(LastImageTask.Result)
        Dim lf = GetFileByIndex(i - 1)
        If lf Is Nothing Then
            LastImageTask = New Task(Of BitmapImage)(Function()
                                                         Return Nothing
                                                     End Function)
            LastImageTask.Start()
        ElseIf lf Is CurrentFile Then
            LastImageTask = New Task(Of BitmapImage)(Function()
                                                         Return oldbmp
                                                     End Function)
            LastImageTask.Start()
        Else
            Dim newfile As String = lf.FullName
            LastImageTask = New Task(Of BitmapImage)(Function()
                                                         Dim bi As New System.Windows.Media.Imaging.BitmapImage
                                                         bi.BeginInit()
                                                         bi.UriSource = New Uri("File://" + newfile)
                                                         bi.CacheOption = BitmapCacheOption.OnLoad
                                                         bi.EndInit()
                                                         bi.Freeze()
                                                         Return bi
                                                     End Function)
            LastImageTask.Start()
        End If
        NextImageTask = New Task(Of BitmapImage)(Function()
                                                     Return oldbmp
                                                 End Function)
        NextImageTask.Start()
    End Sub

    Private IsDragging As Boolean = False
    Private DragStart As Point
    Private DragOrigin As Vector
    Private DragStoptimer As New System.Diagnostics.Stopwatch
    Private LastMousePosition As Point
    Private LastDragOffset As Vector
    Private Sub ImageMouseDown(sender As Object, e As MouseButtonEventArgs)
        Select Case e.ChangedButton
            Case MouseButton.Left
                If CurrentFile Is Nothing OrElse Not CurrentFile.Exists Then
                    TryOpenFile()
                Else
                    Select Case e.ClickCount
                        Case 1
                            IsDragging = True
                            DragOrigin = Offset
                            DragStart = e.GetPosition(Me)
                            LastMousePosition = DragStart
                            LastDragOffset = New Vector(0.0#, 0.0#)
                            DragStoptimer.Reset()
                            DragStoptimer.Start()
                        Case 2
                            IsDragging = False

                            Dim ip = e.GetPosition(Image)
                            Dim del = ip - New Point(Image.ActualWidth / 2, Image.ActualHeight / 2)
                            Dim IdentityZoom = DirectCast(Image.Source, BitmapImage).Width / Image.ActualWidth
                            Dim deltaZoom As Double = 1.0#
                            If Math.Abs(IdentityZoom - Zoom) < 0.001# Then
                                deltaZoom = 1 / Zoom
                            Else
                                deltaZoom = IdentityZoom / Zoom
                            End If
                            Offset -= del * (deltaZoom - 1) * Zoom
                            Zoom *= deltaZoom
                            Image.RenderTransform = ImageTransform
                    End Select

                End If
            Case MouseButton.Right
                Maneuvers.Clear()
        End Select
    End Sub
    Private Sub ImageMouseMove(sender As Object, e As MouseEventArgs)
        If IsDragging Then
            Dim d = e.GetPosition(Me)
            LastDragOffset = d - LastMousePosition
            LastMousePosition = d
            Offset = DragOrigin + d - DragStart
            Image.RenderTransform = ImageTransform
            DragStoptimer.Reset()
        End If
    End Sub
    Private Sub ImageMouseUp(sender As Object, e As MouseButtonEventArgs)
        If IsDragging Then
            IsDragging = False
            Dim d = e.GetPosition(Me)
            Offset = DragOrigin + d - DragStart
            Image.RenderTransform = ImageTransform
            DragStoptimer.Stop()
            If LastDragOffset.Length / DragStoptimer.ElapsedMilliseconds > 10 Then
                Dim m As New StoppingManeuver With {.MoveSpeed = LastDragOffset, .MoveAccelerator = LastDragOffset / -240}
                Maneuvers.Add(m)
            End If
        End If
    End Sub
    Private WheelStoptime As New System.Diagnostics.Stopwatch
    Private Sub ImageMouseWheel(sender As Object, e As MouseWheelEventArgs)
        WheelStoptime.Stop()
        Dim ip = e.GetPosition(Image)
        'Dim del = ip - New Point(Image.ActualWidth / 2, Image.ActualHeight / 2)
        Dim deltaZoom = QRT2 ^ (e.Delta / 120)
        'Offset -= del * (deltaZoom - 1) * Zoom
        'Zoom *= deltaZoom
        'Image.RenderTransform = ImageTransform

        Dim m As New StoppingManeuver With {.ZoomSpeed = (deltaZoom - 1) / 4, .ZoomAccelerator = (deltaZoom - 1) / -120, .ZoomCenter = ip}
        Maneuvers.Add(m)

        WheelStoptime.Reset()
        WheelStoptime.Start()
    End Sub



    Private Sub ImageKeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.Left
                LastFile()
            Case Key.Right
                NextFile()
            Case Key.LeftCtrl
                'rotation 
                Angle -= 2
            Case Key.RightCtrl
                'rotation
                Angle += 2
            Case Key.Up
                Zoom *= QRT2
            Case Key.Down
                Zoom /= QRT2
            Case Key.Enter
                TryOpenFile()
            Case Key.Escape
            Case Key.F
                Image.Stretch = Stretch.UniformToFill
            Case Key.U
                Image.Stretch = Stretch.Uniform
            Case Key.R
                DynamicBrowser()
            Case Key.S
                Dim m As New StoppingManeuver With {.MoveSpeed = New Vector(0.0#, -8.0#), .MoveAccelerator = New Vector(0.0#, -5.0# / -120.0#)}
                Maneuvers.Add(m)
            Case Key.W
                Dim m As New StoppingManeuver With {.MoveSpeed = New Vector(0.0#, 8.0#), .MoveAccelerator = New Vector(0.0#, 5.0# / -120.0#)}
                Maneuvers.Add(m)
            Case Key.D
                Dim m As New StoppingManeuver With {.MoveSpeed = New Vector(-8.0#, 0.0#), .MoveAccelerator = New Vector(-5.0# / -120.0#, 0.0#)}
                Maneuvers.Add(m)
            Case Key.A
                Dim m As New StoppingManeuver With {.MoveSpeed = New Vector(8.0#, 0.0#), .MoveAccelerator = New Vector(5.0# / -120.0#, 0.0#)}
                Maneuvers.Add(m)
            Case Key.Space
                PageShift = 0
                Maneuvers.Clear()
            Case Key.LeftShift
                PageShift = -1
            Case Key.RightShift
                PageShift = 1
        End Select
        Image.RenderTransform = ImageTransform
    End Sub

    Private PageShift As Integer = 0

    Private Sub DynamicBrowser()
        If Image.Source IsNot Nothing Then
            If (Image.ActualHeight / Image.ActualWidth) / (ScreenHeight / ScreenHeight) > 1.0# Then
                Dim m As New ForeverManeuver With {.MoveSpeed = New Vector(0.0#, 5.0#)}
                Maneuvers.Add(m)
            ElseIf (Image.ActualHeight / Image.ActualWidth) / (ScreenHeight / ScreenHeight) < 1.0# Then
                Dim m As New ForeverManeuver With {.MoveSpeed = New Vector(5.0#, 0.0#)}
                Maneuvers.Add(m)
            End If
        End If
    End Sub

    Property FileDialog As New Microsoft.Win32.OpenFileDialog With {.Filter = "Images|*.jpg;*.png;*.bmp;*.gif"}
    Private Offset As Vector = New Vector(0.0#, 0.0#)
    Private Zoom As Double = 1.0#
    Private Angle As Double = 0.0#
    Private Sub TryOpenFile()
        If FileDialog.ShowDialog = True Then
            If ApplyFile(FileDialog.FileName) Then LoadCurrentFile()
        End If
    End Sub
    Private ReadOnly Property ImageTransform As TransformGroup
        Get
            Dim tp As New TransformGroup
            tp.Children.Add(New RotateTransform(Angle))
            tp.Children.Add(New ScaleTransform(Zoom, Zoom))
            tp.Children.Add(New TranslateTransform(Offset.X, Offset.Y))
            Return tp
        End Get
    End Property
    Private Transform As TransformGroup = New TransformGroup


End Class
