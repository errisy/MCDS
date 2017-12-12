
Public Class ColorPicker
    Inherits System.Windows.Controls.Canvas
    Private Hue As New Bitmap(18, 360, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
    Private SV As New Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
    Private SVmutex As New System.Threading.Mutex
    Public Function SetHue(h As Double) As System.Threading.Tasks.Task(Of Boolean)
        Dim t As New System.Threading.Tasks.Task(Of Boolean)(Function() As Boolean
                                                                 SVmutex.WaitOne()
                                                                 Dim bmpData As System.Drawing.Imaging.BitmapData = SV.LockBits(New Rectangle(0, 0, 256, 256), Imaging.ImageLockMode.WriteOnly, Imaging.PixelFormat.Format24bppRgb)
                                                                 Dim data As Byte() = New Byte(196607) {}
                                                                 System.Threading.Tasks.Parallel.For(0, 255, Sub(i)
                                                                                                                 Dim c As New HSV(h, 0D, 0D)
                                                                                                                 Dim s As Integer = 256 * 3 * i
                                                                                                                 For j As Integer = 0 To 255
                                                                                                                     c.V = (255D - i) / 255D
                                                                                                                     c.S = j / 255D
                                                                                                                     c.ToColor.WriteRGB(data, s + j * 3)
                                                                                                                 Next
                                                                                                             End Sub)
                                                                 System.Runtime.InteropServices.Marshal.Copy(data, 0, bmpData.Scan0, 196608)
                                                                 SV.UnlockBits(bmpData)
                                                                 SVmutex.ReleaseMutex()
                                                                 Return True
                                                             End Function)
        Return t
    End Function
    Public Sub GenerateHueGradient()
        Dim bmpData As System.Drawing.Imaging.BitmapData = Hue.LockBits(New Rectangle(0, 0, 18, 360), Imaging.ImageLockMode.WriteOnly, Imaging.PixelFormat.Format24bppRgb)
        Dim std As Integer = bmpData.Stride
        Dim data As Byte() = New Byte(360 * std - 1) {}
        System.Threading.Tasks.Parallel.For(0, 360, Sub(i)
                                                        Dim c As New HSV(i, 1D, 1D)
                                                        Dim st As Integer = std * i
                                                        For j As Integer = 0 To 17
                                                            c.ToColor.WriteRGB(data, st + j * 3)
                                                        Next
                                                    End Sub)
        System.Runtime.InteropServices.Marshal.Copy(data, 0, bmpData.Scan0, 360 * std)
        Hue.UnlockBits(bmpData)
    End Sub
    Private WithEvents SVImage As New System.Windows.Controls.Image
    Private WithEvents HueImage As New System.Windows.Controls.Image
    Private WithEvents H As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents S As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents V As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents R As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents G As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents B As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents A As New NumberBox With {.AllowDecimal = False, .AllowNegative = False, .FontSize = 14, .TextAlignment = Windows.TextAlignment.Right, .Width = 36, .Height = 24}
    Private WithEvents _Old As New System.Windows.Shapes.Rectangle With {.Width = 48, .Height = 24, .Stroke = System.Windows.Media.Brushes.Black}
    Private _New As New System.Windows.Shapes.Rectangle With {.Width = 48, .Height = 24, .Stroke = System.Windows.Media.Brushes.Black}
    Private HSVLabel As New System.Windows.Controls.Label With {.Content = "HSV", .FontSize = 14}
    Private RGBLabel As New System.Windows.Controls.Label With {.Content = "ARGB", .FontSize = 14}
    Private WithEvents ASlidder As New System.Windows.Controls.Slider
    Private WithEvents GrandientFrame As New System.Windows.Shapes.Rectangle With {.Width = 256, .Height = 24, .Stroke = System.Windows.Media.Brushes.Black, .Fill = System.Windows.Media.Brushes.White}
    Private WithEvents Grandient As New GradientBar
    Private WithEvents PLR As New System.Windows.Shapes.Rectangle With {.Width = 36, .Height = 24, .Stroke = System.Windows.Media.Brushes.Black, .Fill = System.Windows.Media.Brushes.Gray}
    Private Shared LBrush As New System.Windows.Media.LinearGradientBrush With {.StartPoint = P(0, 0.5), .EndPoint = P(1, 0.5), .MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = Windows.Media.GradientSpreadMethod.Pad}
    Private Shared RBrush As New System.Windows.Media.RadialGradientBrush With {.Center = P(0.5, 0.5), .GradientOrigin = P(0.5, 0.5), .RadiusX = 0.5, .RadiusY = 0.5, .MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox, .SpreadMethod = Windows.Media.GradientSpreadMethod.Pad}

    Private LinearParaGrid As New GridBase
    Private RadiusParaGrid As New GridBase
    Private lStart As New PointBox
    Private lEnd As New PointBox
    Private lMappingMode As New EnumCombo(GetType(System.Windows.Media.BrushMappingMode))
    Private lSpreadMethod As New EnumCombo(GetType(System.Windows.Media.GradientSpreadMethod))
    Private lRotation As New DoubleBox With {.Value = 0D}
    Private rCenter As New PointBox
    Private rOrigin As New PointBox
    Private rRadius As New PointBox
    Private rMappingMode As New EnumCombo(GetType(System.Windows.Media.BrushMappingMode))
    Private rSpreadMethod As New EnumCombo(GetType(System.Windows.Media.GradientSpreadMethod))
    Private rRotation As New DoubleBox With {.Value = 0D}

    Shared Sub New()
        LBrush.GradientStops.Add(New System.Windows.Media.GradientStop(System.Windows.Media.Colors.Red, 0))
        LBrush.GradientStops.Add(New System.Windows.Media.GradientStop(System.Windows.Media.Colors.Yellow, 1))
        RBrush.GradientStops.Add(New System.Windows.Media.GradientStop(System.Windows.Media.Colors.Red, 0))
        RBrush.GradientStops.Add(New System.Windows.Media.GradientStop(System.Windows.Media.Colors.Yellow, 1))
    End Sub
    Public Sub New()
        Dim x = Dispatcher.DisableProcessing
        Height = 360D
        Width = 400D
        LoadHue(0)
        LoadGradient()
        SVImage.Width = 256
        SVImage.Height = 256

        HueImage.Width = 18
        HueImage.Height = 360
        SVImage.SetCanvasXY(0, 0)
        HueImage.SetCanvasXY(260, 0)

        RGBLabel.SetCanvasXY(0, 260)
        HSVLabel.SetCanvasXY(0, 284)
        ASlidder.Minimum = 0
        ASlidder.Maximum = 255
        ASlidder.Width = 256
        ASlidder.Height = 24
        ASlidder.SetCanvasXY(0, 310)
        Grandient.Width = 256
        Grandient.Height = 24
        GrandientFrame.SetCanvasXY(0, 332)
        Grandient.SetCanvasXY(0, 332)
        Grandient.BrushChanged = AddressOf BrushChangedFromGradientBar
        A.SetCanvasXY(48, 260)
        R.SetCanvasXY(86, 260)
        G.SetCanvasXY(124, 260)
        B.SetCanvasXY(160, 260)
        PLR.SetCanvasXY(48, 284)
        H.SetCanvasXY(86, 284)
        S.SetCanvasXY(124, 284)
        V.SetCanvasXY(160, 284)
        _Old.SetCanvasXY(208, 260)
        _New.SetCanvasXY(208, 284)
        Children.Add(SVImage)
        Children.Add(HueImage)
        Children.Add(RGBLabel)
        Children.Add(HSVLabel)
        Children.Add(A)
        Children.Add(PLR)
        Children.Add(H)
        Children.Add(S)
        Children.Add(V)
        Children.Add(R)
        Children.Add(G)
        Children.Add(B)
        Children.Add(_Old)
        Children.Add(_New)
        Children.Add(ASlidder)
        Children.Add(GrandientFrame)
        Children.Add(Grandient)

        'LinearParaGrid()
        'RadiusParaGrid()

 
        x.Dispose()
    End Sub
    Public Event BrushChanged As System.Windows.RoutedEventHandler
    Public Sub BrushChangedFromGradientBar()
        If Not Setting Then RaiseEvent BrushChanged(Me, New System.Windows.RoutedEventArgs)
    End Sub
    Private AllowedModes As New List(Of PickerModeEnum) From {PickerModeEnum.Solid, PickerModeEnum.Linear, PickerModeEnum.Radius}
    Public Sub LockMode(ParamArray cMode As PickerModeEnum())
        AllowedModes = New List(Of PickerModeEnum)
        AllowedModes.AddRange(cMode)
        If Not AllowedModes.Contains(PickerModeEnum.Linear) And AllowedModes.Contains(PickerModeEnum.Radius) Then
            GrandientFrame.Visibility = Windows.Visibility.Hidden
            Grandient.Visibility = Windows.Visibility.Hidden
        End If
    End Sub
    Private Sub LoadGradient()
        GenerateHueGradient()
        Dim bi As New System.Windows.Media.Imaging.BitmapImage()
        bi.BeginInit()
        Dim ms As New System.IO.MemoryStream
        Hue.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        bi.StreamSource = ms
        bi.EndInit()
        HueImage.Source = bi
    End Sub
    Private RunningTask As System.Threading.Tasks.Task(Of Boolean)
    Private WaitingTask As System.Threading.Tasks.Task(Of Boolean)
    Private Async Sub LoadHue(h As Double)
        WaitingTask = SetHue(h)
        Do
            If RunningTask Is Nothing OrElse RunningTask.Status = Threading.Tasks.TaskStatus.RanToCompletion Then
                RunningTask = WaitingTask
                WaitingTask = Nothing
                RunningTask.Start()
                Dim b As Boolean = Await RunningTask
            Else
                Exit Sub
            End If
        Loop While WaitingTask IsNot Nothing
        SVmutex.WaitOne()
        Dim bi As New System.Windows.Media.Imaging.BitmapImage()
        bi.BeginInit()
        Dim ms As New System.IO.MemoryStream
        SV.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        bi.StreamSource = ms
        bi.EndInit()
        SVImage.Source = bi
        SVmutex.ReleaseMutex()
    End Sub

    Private ColorMoving As Boolean = False
    Private Sub HueImage_MouseButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles HueImage.MouseDown
        Dim pv = e.GetPosition(HueImage)
        If pv.Y < 0 Then
            H.Value = 0
        ElseIf pv.Y > 360 Then
            H.Value = 360
        Else
            H.Value = pv.Y
        End If
        'LoadHue(H.Value)
        HueImage.CaptureMouse()
        ColorMoving = True
    End Sub
    Private Sub HueImage_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles HueImage.MouseMove
        If ColorMoving Then
            Dim pv = e.GetPosition(HueImage)
            If pv.Y < 0 Then
                H.Value = 0
            ElseIf pv.Y > 360 Then
                H.Value = 360
            Else
                H.Value = pv.Y
            End If
            'LoadHue(H.Value)
        End If
    End Sub
    Private Sub HueImage_MouseUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles HueImage.MouseUp
        If ColorMoving Then
            Dim pv = e.GetPosition(HueImage)
            If pv.Y < 0 Then
                H.Value = 0
            ElseIf pv.Y > 360 Then
                H.Value = 360
            Else
                H.Value = pv.Y
            End If
            'LoadHue(H.Value)
            ColorMoving = False
            HueImage.ReleaseMouseCapture()
        End If
    End Sub
    Private Sub SVImage_MouseButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles SVImage.MouseDown
        Dim pv = e.GetPosition(SVImage)
        If pv.X < 0 Then
            S.Value = 0
        ElseIf pv.X > 255 Then
            S.Value = 255
        Else
            S.Value = pv.X
        End If
        If pv.Y < 0 Then
            V.Value = 255
        ElseIf pv.Y > 255 Then
            V.Value = 0
        Else
            V.Value = CInt(255 - pv.Y)
        End If
        SVImage.CaptureMouse()
        ColorMoving = True
    End Sub
    Private Sub SVImage_MouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs) Handles SVImage.MouseMove
        If ColorMoving Then
            Dim pv = e.GetPosition(SVImage)
            If pv.X < 0 Then
                S.Value = 0
            ElseIf pv.X > 255 Then
                S.Value = 255
            Else
                S.Value = pv.X
            End If
            If pv.Y < 0 Then
                V.Value = 255
            ElseIf pv.Y > 255 Then
                V.Value = 0
            Else
                V.Value = CInt(255 - pv.Y)
            End If
        End If
    End Sub
    Private Sub SVImage_MouseUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles SVImage.MouseUp
        If ColorMoving Then
            Dim pv = e.GetPosition(SVImage)
            If pv.X < 0 Then
                S.Value = 0
            ElseIf pv.X > 255 Then
                S.Value = 255
            Else
                S.Value = pv.X
            End If
            If pv.Y < 0 Then
                V.Value = 255
            ElseIf pv.Y > 255 Then
                V.Value = 0
            Else
                V.Value = CInt(255 - pv.Y)
            End If
            ColorMoving = False
            SVImage.ReleaseMouseCapture()
        End If
    End Sub
    Private Setting As Boolean = False
    Private Sub ColorFromTextOrPicker(c As ColorVector)
        _New.Fill = New System.Windows.Media.SolidColorBrush(c)
        If TypeOf PLR.Fill Is System.Windows.Media.LinearGradientBrush OrElse TypeOf PLR.Fill Is System.Windows.Media.RadialGradientBrush Then
            Grandient.CurrentColor = c
        End If
        If Not Setting Then RaiseEvent BrushChanged(Me, New System.Windows.RoutedEventArgs)
    End Sub
    Private CurrentMode As PickerModeEnum = PickerModeEnum.Solid
    Public Property Brush As System.Windows.Media.Brush
        Get
            Select Case CurrentMode
                Case PickerModeEnum.None
                    Return Nothing
                Case PickerModeEnum.Solid
                    Return _New.Fill
                Case PickerModeEnum.Linear
                    Dim lgb As New System.Windows.Media.LinearGradientBrush
                    lgb.StartPoint = P(0, 0.5)
                    lgb.EndPoint = P(1, 0.5)
                    lgb.MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox
                    lgb.SpreadMethod = Windows.Media.GradientSpreadMethod.Pad
                    For Each gs As System.Windows.Media.GradientStop In Grandient.ReadGradients
                        lgb.GradientStops.Add(gs)
                    Next
                    Return lgb
                Case PickerModeEnum.Radius
                    Dim lgb As New System.Windows.Media.RadialGradientBrush
                    lgb.Center = P(0.5, 0.5)
                    lgb.GradientOrigin = P(0.5, 0.5)
                    lgb.RadiusX = 0.5
                    lgb.RadiusY = 0.5
                    lgb.MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox
                    lgb.SpreadMethod = Windows.Media.GradientSpreadMethod.Pad
                    For Each gs As System.Windows.Media.GradientStop In Grandient.ReadGradients
                        lgb.GradientStops.Add(gs)
                    Next
                    Return lgb
                Case PickerModeEnum.Image
                    Return PLR.Fill
                Case Else
                    Return Nothing
            End Select
            'If TypeOf PLR.Fill Is System.Windows.Media.LinearGradientBrush Then
            '    Dim lgb As New System.Windows.Media.LinearGradientBrush
            '    lgb.StartPoint = P(0, 0.5)
            '    lgb.EndPoint = P(1, 0.5)
            '    lgb.MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox
            '    lgb.SpreadMethod = Windows.Media.GradientSpreadMethod.Pad
            '    For Each gs As System.Windows.Media.GradientStop In Grandient.ReadGradients
            '        lgb.GradientStops.Add(gs)
            '    Next
            '    Return lgb
            'ElseIf TypeOf PLR.Fill Is System.Windows.Media.RadialGradientBrush Then
            '    Dim lgb As New System.Windows.Media.RadialGradientBrush
            '    lgb.Center = P(0.5, 0.5)
            '    lgb.GradientOrigin = P(0.5, 0.5)
            '    lgb.RadiusX = 0.5
            '    lgb.RadiusY = 0.5
            '    lgb.MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox
            '    lgb.SpreadMethod = Windows.Media.GradientSpreadMethod.Pad
            '    For Each gs As System.Windows.Media.GradientStop In Grandient.ReadGradients
            '        lgb.GradientStops.Add(gs)
            '    Next
            '    Return lgb
            'ElseIf TypeOf PLR.Fill Is System.Windows.Media.ImageBrush Then
            '    Return PLR.Fill
            'Else
            '    Return _New.Fill
            'End If
        End Get
        Set(value As System.Windows.Media.Brush)
            Setting = True
            If value Is Nothing Then
                _Old.Fill = System.Windows.Media.Brushes.Transparent
                CurrentMode = PickerModeEnum.None
            ElseIf TypeOf value Is System.Windows.Media.SolidColorBrush Then
                PLR.Fill = System.Windows.Media.Brushes.Gray
                _Old.Fill = value
                SetColor(DirectCast(value, System.Windows.Media.SolidColorBrush).Color)
                CurrentMode = PickerModeEnum.Solid
            ElseIf TypeOf value Is System.Windows.Media.LinearGradientBrush Then
                _Old.Fill = value
                PLR.Fill = LBrush
                Grandient.LoadBrush(DirectCast(value, System.Windows.Media.LinearGradientBrush))
                CurrentMode = PickerModeEnum.Linear
            ElseIf TypeOf value Is System.Windows.Media.RadialGradientBrush Then
                _Old.Fill = value
                PLR.Fill = RBrush
                Grandient.LoadBrush(DirectCast(value, System.Windows.Media.RadialGradientBrush))
                CurrentMode = PickerModeEnum.Radius
            ElseIf TypeOf value Is System.Windows.Media.ImageBrush Then
                CurrentMode = PickerModeEnum.Image
            End If
            Setting = False
        End Set
    End Property
    Public Sub SetColor(c As ColorVector)
        RGBSetting = True
        R.Text = c.R.ToString
        G.Text = c.G.ToString
        B.Text = c.B.ToString
        A.Text = c.A.ToString
        RGBSetting = False
        HSVSetting = True
        Dim _hsv As HSV = CType(c, System.Windows.Media.Color).ToHSV
        H.Text = CInt(_hsv.H).ToString
        S.Text = CInt(_hsv.S * 255).ToString
        V.Text = CInt(_hsv.V * 255).ToString
        A.Text = CInt(_hsv.A).ToString
        LoadHue(_hsv.H)
        HSVSetting = False
        _New.Fill = New System.Windows.Media.SolidColorBrush(c)
    End Sub
    Private RGBSetting As Boolean = False
    Private HSVSetting As Boolean = False
    Private Sub HSVValueChanged(sender As Object, e As EventArgs) Handles H.TextChanged, S.TextChanged, V.TextChanged
        If HSVSetting Then Exit Sub
        If sender Is H Then LoadHue(H.Value)
        Dim _hsv As New HSV(A.Text, H.Text, S.Text, V.Text)
        _hsv.S /= 255
        _hsv.V /= 255
        RGBSetting = True
        Dim clr As System.Windows.Media.Color = _hsv.ToColor
        R.Text = clr.R.ToString
        G.Text = clr.G.ToString
        B.Text = clr.B.ToString
        A.Text = clr.A.ToString
        RGBSetting = False
        ColorFromTextOrPicker(_hsv.ToColor)
    End Sub
    Private Sub RGBValueChanged(sender As Object, e As EventArgs) Handles R.TextChanged, G.TextChanged, B.TextChanged
        If RGBSetting Then Exit Sub
        Dim _rgb As New ColorVector(A.Text, R.Text, G.Text, B.Text)
        HSVSetting = True
        Dim _hsv As HSV = CType(_rgb, System.Windows.Media.Color).ToHSV
        LoadHue(_hsv.H)
        H.Text = CInt(_hsv.H).ToString
        S.Text = CInt(_hsv.S * 255).ToString
        V.Text = CInt(_hsv.V * 255).ToString
        A.Text = CInt(_hsv.A).ToString
        HSVSetting = False
        ColorFromTextOrPicker(_rgb)
    End Sub
    Private Sub AValueChanged(sender As Object, e As EventArgs) Handles A.TextChanged
        Dim _rgb As New ColorVector(A.Text, R.Text, G.Text, B.Text)
        ColorFromTextOrPicker(_rgb)
        ASetting = True
        Dim d As Double
        If Double.TryParse(A.Text, d) Then
            ASlidder.Value = d
        End If
        ASetting = False
    End Sub
    Private ASetting As Boolean = False
    Private Sub ASlidder_ValueChanged(sender As Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles ASlidder.ValueChanged
        If ASetting Then Exit Sub
        A.Value = Math.Round(ASlidder.Value)
    End Sub
    Private Sub _Old_MouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles _Old.MouseDown
        _New.Fill = _Old.Fill
        If Not Setting Then RaiseEvent BrushChanged(Me, New System.Windows.RoutedEventArgs)
    End Sub
    Private Sub GrandientFrame_MouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles GrandientFrame.MouseDown
        Dim pv = e.GetPosition(GrandientFrame)
        Dim vx As Double = pv.X / 255
        Grandient.CreateBar(vx)
        If Not Setting Then RaiseEvent BrushChanged(Me, New System.Windows.RoutedEventArgs)
    End Sub
    Private Sub Grandient_SelectionChanged(sender As Object, e As System.EventArgs) Handles Grandient.SelectionChanged
        If Grandient.SelectedDragger IsNot Nothing Then
            SetColor(Grandient.CurrentColor)
            If TypeOf PLR.Fill Is System.Windows.Media.SolidColorBrush Then
                PLR.Fill = LBrush
                CurrentMode = PickerModeEnum.Linear
            End If
            If Not Setting Then RaiseEvent BrushChanged(Me, New System.Windows.RoutedEventArgs)
        End If
    End Sub
    Private AllowImage As Boolean = False
    Private Sub PLR_MouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles PLR.MouseDown

        Dim i As Integer = AllowedModes.IndexOf(CurrentMode)
        If i > -1 Then
            i += 1
            If i >= AllowedModes.Count Then i = 0
            CurrentMode = AllowedModes(i)
            Select Case CurrentMode
                Case PickerModeEnum.None
                    PLR.Fill = System.Windows.Media.Brushes.Transparent
                Case PickerModeEnum.Solid
                    PLR.Fill = System.Windows.Media.Brushes.Gray
                Case PickerModeEnum.Linear
                    PLR.Fill = LBrush
                Case PickerModeEnum.Radius
                    PLR.Fill = RBrush
                Case PickerModeEnum.Image
                    Static ofd As New OpenFileDialog With {.FileName = "", .Filter = "Images|*.jpg;*.png;*.gif;*.bmp;*.jpeg"}
                    If ofd.ShowDialog = DialogResult.OK Then
                        Dim bi As New System.Windows.Media.Imaging.BitmapImage()
                        bi.BeginInit()
                        Dim ms As New System.IO.MemoryStream(System.IO.File.ReadAllBytes(ofd.FileName))
                        bi.StreamSource = ms
                        bi.EndInit()
                        Dim imgb As New System.Windows.Media.ImageBrush
                        imgb.ImageSource = bi
                        imgb.Stretch = Windows.Media.Stretch.UniformToFill
                        PLR.Fill = imgb
                    Else
                        CurrentMode = PickerModeEnum.Solid
                        PLR.Fill = System.Windows.Media.Brushes.Gray
                    End If
            End Select

        Else
            CurrentMode = PickerModeEnum.None
            PLR.Fill = System.Windows.Media.Brushes.Transparent
        End If
        'If TypeOf PLR.Fill Is System.Windows.Media.LinearGradientBrush Then
        '    PLR.Fill = RBrush
        'ElseIf TypeOf PLR.Fill Is System.Windows.Media.RadialGradientBrush Then
        '    If AllowImage Then
        '        Static ofd As New OpenFileDialog With {.FileName = "", .Filter = "Images|*.jpg;*.png;*.gif;*.bmp;*.jpeg"}
        '        If ofd.ShowDialog = DialogResult.OK Then
        '            Dim bi As New System.Windows.Media.Imaging.BitmapImage()
        '            bi.BeginInit()
        '            Dim ms As New System.IO.MemoryStream(System.IO.File.ReadAllBytes(ofd.FileName))
        '            bi.StreamSource = ms
        '            bi.EndInit()
        '            Dim imgb As New System.Windows.Media.ImageBrush
        '            imgb.ImageSource = bi
        '            imgb.Stretch = Windows.Media.Stretch.UniformToFill
        '            PLR.Fill = imgb
        '        Else
        '            PLR.Fill = System.Windows.Media.Brushes.Gray
        '        End If
        '    Else
        '        PLR.Fill = System.Windows.Media.Brushes.Gray
        '    End If
        'ElseIf TypeOf PLR.Fill Is System.Windows.Media.ImageBrush Then
        '    PLR.Fill = System.Windows.Media.Brushes.Gray
        'ElseIf TypeOf PLR.Fill Is System.Windows.Media.SolidColorBrush Then
        '    PLR.Fill = LBrush
        'End If
    End Sub
End Class

Public Enum PickerModeEnum
    None
    Solid
    Linear
    Radius
    Image
End Enum

Public Class GradientBar
    Inherits System.Windows.Controls.Canvas
    Public Sub New()
    End Sub
    'Private Bars As New List(Of System.Windows.Shapes.Rectangle)
    Private DragStart As System.Windows.Vector
    Private StartPosition As System.Windows.Vector
    Private Dragging As Boolean = False
    Public BrushChanged As System.Action
    Public Function LoadBrush(brs As System.Windows.Media.LinearGradientBrush) As Boolean
        Children.Clear()
        For Each gs As System.Windows.Media.GradientStop In brs.GradientStops
            CreateBar(gs.Offset, gs.Color)
        Next
        OnBrushChanged()
        Return True
    End Function
    Public Function LoadBrush(brs As System.Windows.Media.RadialGradientBrush) As Boolean
        Children.Clear()
        For Each gs As System.Windows.Media.GradientStop In brs.GradientStops
            CreateBar(gs.Offset, gs.Color)
        Next
        OnBrushChanged()
        Return True
    End Function
    Public Function ReadGradients() As IEnumerable(Of System.Windows.Media.GradientStop)
        Dim gs As System.Windows.Media.GradientStop
        Dim gsl As New List(Of System.Windows.Media.GradientStop)
        Dim lgb As New System.Windows.Media.LinearGradientBrush
        lgb.MappingMode = Windows.Media.BrushMappingMode.RelativeToBoundingBox
        lgb.SpreadMethod = Windows.Media.GradientSpreadMethod.Pad
        lgb.StartPoint = New System.Windows.Point(0, 0.5)
        lgb.EndPoint = New System.Windows.Point(1, 0.5)
        For Each r As System.Windows.Shapes.Rectangle In Children
            gs = New System.Windows.Media.GradientStop(BarColor(r), (r.GetCanvasXY.X + 3) / Width)
            gsl.Add(gs)
        Next
        Dim gslob = gsl.OrderBy(Function(kgd) kgd.Offset).ToArray
        For Each gs In gslob
            lgb.GradientStops.Add(gs)
        Next
        If lgb.GradientStops.Count > 0 Then
            Background = lgb
        Else
            Background = System.Windows.Media.Brushes.Transparent
        End If
        Return gslob
    End Function

    Public SelectedDragger As System.Windows.Shapes.Rectangle
    Public Event SelectionChanged(sender As Object, e As EventArgs)
    Private Sub ResetGradient()
        ReadGradients()
    End Sub
    Private Property BarColor(r As System.Windows.Shapes.Rectangle) As ColorVector
        Get
            If TypeOf r Is System.Windows.Shapes.Rectangle Then
                Return CType(r.Stroke, System.Windows.Media.SolidColorBrush).Color
            Else
                Return New ColorVector(0, 0, 0, 0)
            End If
        End Get
        Set(value As ColorVector)
            If TypeOf r Is System.Windows.Shapes.Rectangle Then
                r.Stroke = New System.Windows.Media.SolidColorBrush(CType(value, System.Windows.Media.Color))
            End If
            ReadGradients()
        End Set
    End Property
    Public Property CurrentColor As ColorVector
        Get
            Return BarColor(SelectedDragger)
        End Get
        Set(value As ColorVector)
            BarColor(SelectedDragger) = value
        End Set
    End Property
    Sub StartDrag(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim r As System.Windows.Shapes.Rectangle = sender
        SelectedDragger = r
        RaiseEvent SelectionChanged(Me, New EventArgs)
        Dragging = True
        StartPosition = r.GetCanvasXY
        DragStart = e.GetPosition(Me)
        e.Handled = True
        r.CaptureMouse()
    End Sub
    Sub MoveDrag(sender As Object, e As System.Windows.Input.MouseEventArgs)
        Dim r As System.Windows.Shapes.Rectangle = sender
        If Dragging Then
            Dim p As System.Windows.Vector = e.GetPosition(Me)
            Dim vx As Double = (StartPosition + p - DragStart).X
            If vx < -3 Then vx = -3
            If vx > Width - 3 Then vx = Width - 3
            r.SetCanvasXY(vx, 0)
            ReadGradients()
            OnBrushChanged()
        End If
    End Sub
    Sub EndDrag(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim r As System.Windows.Shapes.Rectangle = sender
        If Dragging Then
            Dim p As System.Windows.Vector = e.GetPosition(Me)
            Dim vx As Double = (StartPosition + p - DragStart).X
            If vx < -3 Then vx = -3
            If vx > Width - 3 Then vx = Width - 3
            r.SetCanvasXY(vx, 0)
            Dragging = False
            r.ReleaseMouseCapture()
            ReadGradients()
            OnBrushChanged()
        End If
    End Sub
    Sub RemoveDrag(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim r As System.Windows.Shapes.Rectangle = sender
        RemoveHandler r.MouseLeftButtonDown, AddressOf StartDrag
        RemoveHandler r.MouseMove, AddressOf MoveDrag
        RemoveHandler r.MouseLeftButtonUp, AddressOf EndDrag
        RemoveHandler r.MouseRightButtonDown, AddressOf RemoveDrag
        Children.Remove(r)
        TryGetNext()
        OnBrushChanged()
    End Sub
    Protected Overridable Sub OnBrushChanged()
        If BrushChanged.OK Then BrushChanged.Invoke()
    End Sub
    Private Sub TryGetNext()
        For Each r As System.Windows.Shapes.Rectangle In Children
            SelectedDragger = r
            RaiseEvent SelectionChanged(Me, New EventArgs)
            Exit Sub
        Next
        SelectedDragger = Nothing
        RaiseEvent SelectionChanged(Me, New EventArgs)
    End Sub
    Public Sub CreateBar(offset As Double, c As System.Windows.Media.Color)
        Dim r As New System.Windows.Shapes.Rectangle
        r.Width = 6
        r.Height = Me.Height
        r.SetCanvasXY(Width * offset - 3, 0)
        r.Stroke = New System.Windows.Media.SolidColorBrush(c)
        r.Fill = New System.Windows.Media.SolidColorBrush(New ColorVector(64, 0, 0, 0))
        AddHandler r.MouseLeftButtonDown, AddressOf StartDrag
        AddHandler r.MouseMove, AddressOf MoveDrag
        AddHandler r.MouseLeftButtonUp, AddressOf EndDrag
        AddHandler r.MouseRightButtonDown, AddressOf RemoveDrag
        Children.Add(r)
        SelectedDragger = r
        RaiseEvent SelectionChanged(Me, New EventArgs)
    End Sub
    Public Sub CreateBar(offset As Double)
        Dim r As New System.Windows.Shapes.Rectangle
        r.Width = 6
        r.Height = Me.Height
        r.SetCanvasXY(Width * offset - 3, 0)
        r.Stroke = New System.Windows.Media.SolidColorBrush(GetColorAt(offset))
        r.Fill = New System.Windows.Media.SolidColorBrush(New ColorVector(64, 0, 0, 0))
        AddHandler r.MouseLeftButtonDown, AddressOf StartDrag
        AddHandler r.MouseMove, AddressOf MoveDrag
        AddHandler r.MouseLeftButtonUp, AddressOf EndDrag
        AddHandler r.MouseRightButtonDown, AddressOf RemoveDrag
        Children.Add(r)
        SelectedDragger = r
        RaiseEvent SelectionChanged(Me, New EventArgs)
    End Sub
    Private Function GetColorAt(offset As Double) As ColorVector
        If TypeOf Background Is System.Windows.Media.SolidColorBrush Then
            Return CType(Background, System.Windows.Media.SolidColorBrush).Color
        ElseIf TypeOf Background Is System.Windows.Media.LinearGradientBrush Then
            Dim gs As New Dictionary(Of Double, System.Windows.Media.Color)
            Dim lgb As System.Windows.Media.LinearGradientBrush = Background
            For Each g In lgb.GradientStops
                gs.Add(g.Offset, g.Color)
            Next
            Dim lf As Double = Double.MinValue
            Dim rf As Double = Double.MaxValue
            For Each m In gs.Keys
                If m <= offset Then
                    lf = IIf(lf < m, m, lf)
                End If
                If m >= offset Then
                    rf = IIf(rf > m, m, rf)
                End If
            Next
            If lf = offset Then
                Return gs(lf)
            ElseIf rf = offset Then
                Return gs(rf)
            ElseIf lf <> Double.MinValue And rf <> Double.MaxValue Then
                Dim lc As New ColorVector(gs(lf))
                Dim rc As New ColorVector(gs(rf))
                Return (lc * (rf - offset) + rc * (offset - lf)) / (rf - lf)
            Else
                Return New ColorVector(255, 255, 255, 255)
            End If
        Else
            Return New ColorVector(255, 255, 255, 255)
        End If
    End Function
    Protected Overrides Sub OnMouseLeftButtonDown(e As System.Windows.Input.MouseButtonEventArgs)
        Dim v As Double = e.GetPosition(Me).X / Width
        CreateBar(v)
        e.Handled = True
        MyBase.OnMouseLeftButtonDown(e)
    End Sub
End Class
 

Public Module ColorChange
    <System.Runtime.CompilerServices.Extension()> Public Function ToHSV(color As Color) As HSV
        Dim max As Integer = Math.Max(color.R, Math.Max(color.G, color.B))
        Dim min As Integer = Math.Min(color.R, Math.Min(color.G, color.B))
        Return New HSV(color.A, color.GetHue(), If(max = 0, 0, 1.0 - (1.0 * min / max)), max / 255.0)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ToHSV(color As System.Windows.Media.Color) As HSV
        Dim max As Integer = Math.Max(color.R, Math.Max(color.G, color.B))
        Dim min As Integer = Math.Min(color.R, Math.Min(color.G, color.B))
        Return New HSV(color.A, System.Drawing.Color.FromArgb(color.R, color.G, color.B).GetHue(), If(max = 0, 0, 1.0 - (1.0 * min / max)), max / 255.0)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function ToColor(HSVColor As HSV) As ColorVector
        Dim hi As Integer = Convert.ToInt32(Math.Floor(HSVColor.H / 60)) Mod 6
        Dim f As Double = HSVColor.H / 60 - Math.Floor(HSVColor.H / 60)
        HSVColor.V = HSVColor.V * 255
        Dim v As Integer = Convert.ToInt32(HSVColor.V)
        Dim p As Integer = Convert.ToInt32(HSVColor.V * (1 - HSVColor.S))
        Dim q As Integer = Convert.ToInt32(HSVColor.V * (1 - f * HSVColor.S))
        Dim t As Integer = Convert.ToInt32(HSVColor.V * (1 - (1 - f) * HSVColor.S))
        If hi = 0 Then
            Return New ColorVector(HSVColor.A, v, t, p)
        ElseIf hi = 1 Then
            Return New ColorVector(HSVColor.A, q, v, p)
        ElseIf hi = 2 Then
            Return New ColorVector(HSVColor.A, p, v, t)
        ElseIf hi = 3 Then
            Return New ColorVector(HSVColor.A, p, q, v)
        ElseIf hi = 4 Then
            Return New ColorVector(HSVColor.A, t, p, v)
        Else
            Return New ColorVector(HSVColor.A, v, p, q)
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub WriteRGB(color As ColorVector, data As Byte(), start As Integer)
        Dim R As Byte
        If color.R < 0 Then
            R = 0
        ElseIf color.R > 255 Then
            R = 255
        Else
            R = CByte(color.R)
        End If
        Dim G As Byte
        If color.G < 0 Then
            G = 0
        ElseIf color.G > 255 Then
            G = 255
        Else
            G = CByte(color.G)
        End If
        Dim B As Byte
        If color.B < 0 Then
            B = 0
        ElseIf color.B > 255 Then
            B = 255
        Else
            B = CByte(color.B)
        End If
        data(start) = B
        data(start + 1) = G
        data(start + 2) = R
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub SetCanvasXY(c As System.Windows.FrameworkElement, X As Double, Y As Double)
        System.Windows.Controls.Canvas.SetLeft(c, X)
        System.Windows.Controls.Canvas.SetTop(c, Y)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub SetCanvasXY(c As System.Windows.FrameworkElement, v As System.Windows.Vector)
        System.Windows.Controls.Canvas.SetLeft(c, v.X)
        System.Windows.Controls.Canvas.SetTop(c, v.Y)
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Function GetCanvasXY(c As System.Windows.FrameworkElement) As System.Windows.Vector
        Return New System.Windows.Vector(System.Windows.Controls.Canvas.GetLeft(c), System.Windows.Controls.Canvas.GetTop(c))
    End Function
End Module
Public Structure HSV
    Public Sub New(nH As Double, nS As Double, nV As Double)
        A = 255D
        H = nH
        S = nS
        V = nV
    End Sub
    Public Sub New(nA As Double, nH As Double, nS As Double, nV As Double)
        A = nA
        H = nH
        S = nS
        V = nV
    End Sub
    Public Sub New(nH As String, nS As String, nV As String)
        A = 255D
        Double.TryParse(nH, H)
        Double.TryParse(nS, S)
        Double.TryParse(nV, V)
    End Sub
    Public Sub New(nA As String, nH As String, nS As String, nV As String)
        If Not Double.TryParse(nA, A) Then A = 255D
        Double.TryParse(nH, H)
        Double.TryParse(nS, S)
        Double.TryParse(nV, V)
    End Sub
    Public A As Double
    Public H As Double
    Public S As Double
    Public V As Double
End Structure