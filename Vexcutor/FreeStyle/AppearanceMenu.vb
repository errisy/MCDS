Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes

Public Class AppearanceMenu
    Inherits System.Windows.Controls.ContextMenu
    Public Sub New(vType As Type)
        For Each pi As System.Reflection.PropertyInfo In vType.GetProperties
            For Each att As Attribute In pi.GetCustomAttributes(True)
                If TypeOf att Is MenuAttribute Then
                    If pi.PropertyType.IsAssignableFrom(GetType(Brush)) Then
                        Items.Add(New BrushPropertyMenuItem(Me, pi))
                    ElseIf pi.PropertyType Is GetType(Double) Then
                        Items.Add(New DoublePropertyMenuItem(Me, pi))
                    ElseIf pi.PropertyType Is GetType(FontWeight) Then
                        Items.Add(New FontWeightPropertyMenuItem(Me, pi))
                    ElseIf pi.PropertyType Is GetType(FontStyle) Then
                        Items.Add(New FontStylePropertyMenuItem(Me, pi))
                    ElseIf pi.PropertyType.IsAssignableFrom(GetType(Effects.Effect)) Then
                        Items.Add(New EffectPropertyMenu(Me, pi))
                    End If
                End If
            Next
        Next
    End Sub
    Public Sub Bind(value As IActor)
        RelatedObject = value
        For Each b As Action(Of Object) In _BindCalls
            b.Invoke(value)
        Next
    End Sub
    Private _BindCalls As New List(Of Action(Of Object))
    Public Sub AddBinder(vBinder As Action(Of Object))
        _BindCalls.Add(vBinder)
    End Sub
    Public Property RelatedObject As Object
End Class

Public Class PropertyMenuItem
    Inherits MenuItem
    Protected Shared Empty As Object() = New Object() {}
    Public PropertyInfo As System.Reflection.PropertyInfo
    Public Sub New(pi As System.Reflection.PropertyInfo)
        MyBase.New()
        PropertyInfo = pi
    End Sub
    Public ReadOnly Property RelatedObject As Object
        Get
            If TypeOf Parent Is PropertyMenuItem Then
                Return DirectCast(Parent, PropertyMenuItem).RelatedObject
            ElseIf TypeOf Parent Is AppearanceMenu Then
                Return DirectCast(Parent, AppearanceMenu).RelatedObject
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub AddBinder(vBinder As Action(Of Object))
        If TypeOf Parent Is PropertyMenuItem Then
            DirectCast(Parent, PropertyMenuItem).AddBinder(vBinder)
        ElseIf TypeOf Parent Is AppearanceMenu Then
            DirectCast(Parent, AppearanceMenu).AddBinder(vBinder)
        End If
    End Sub
End Class
Public Class BrushPropertyMenuItem
    Inherits PropertyMenuItem
    Private gdHost As New GridBase
    Private Title As New System.Windows.Controls.Label
    'Private stdBrushes As StandardBrushPropertyMenuItem
    Public Sub New(top As AppearanceMenu, pi As System.Reflection.PropertyInfo)
        MyBase.New(pi)
        'stdBrushes = New StandardBrushPropertyMenuItem()
        Header = gdHost
        Title.Content = pi.Name
        gdHost.AddColumnItem(Title)
        PropertyInfo = pi
        Items.Add(New StandardBrushPropertyMenuItem())
        Items.Add(New ARGBBrushMenu())
        Items.Add(New LinearGradientBrushMenu)
        Items.Add(New RadiusGradientBrushMenu)
        Items.Add(New PickerBrushMenu)
        Items.Add(New ImageBrushMenu)
        Me.Icon = IconImages.ImageFromString(IconImages.Color, 20, 20)
        top.AddBinder(AddressOf Bind)
    End Sub
    Public Sub Bind(obj As Object)
        Dim brsh As Brush = PropertyInfo.GetValue(obj, Empty)
        For Each it As BaseBrushMenuItem In Items
            it.SetBrush(brsh)
        Next
    End Sub
    Public Sub SetBrush(brsh As Brush)
        PropertyInfo.SetValue(RelatedObject, brsh, Empty)
    End Sub
End Class
Public MustInherit Class BaseBrushMenuItem
    Inherits MenuItem
    Protected gdHost As New GridBase
    Protected Title As New System.Windows.Controls.Label
    Public Sub New()
        Header = gdHost
        gdHost.AddColumnItem(Title)
    End Sub
    Public MustOverride Sub SetBrush(brsh As Brush)
    Public Shadows ReadOnly Property Parent As BrushPropertyMenuItem
        Get
            Return MyBase.Parent
        End Get
    End Property
End Class

Public Class StandardBrushPropertyMenuItem
    Inherits BaseBrushMenuItem
    Public Sub New()
        Title.Content = "Standard Brushes"
        Dim t As Type = GetType(System.Windows.Media.Brushes)
        Dim tSCB As Type = GetType(SolidColorBrush)
        For Each cpi As System.Reflection.PropertyInfo In t.GetProperties
            If cpi.PropertyType Is tSCB Then
                Dim mi As New System.Windows.Controls.MenuItem
                mi.Background = cpi.GetValue(Nothing, New Object() {})
                mi.Header = cpi.Name
                mi.IsCheckable = True
                mi.Foreground = ReverseColor(mi.Background)
                AddHandler mi.Click, AddressOf OnSubItemsClick
                Me.Items.Add(mi)
            End If
        Next
        Me.Icon = IconImages.ImageFromString(IconImages.Color, 20, 20)
    End Sub
    Private Sub OnSubItemsClick(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = sender
        Parent.SetBrush(mi.Background)
    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is SolidColorBrush Then
            Dim scb As Color = DirectCast(brsh, SolidColorBrush).Color
            For Each mi As MenuItem In Items
                If DirectCast(mi.Background, SolidColorBrush).Color = scb Then
                    mi.IsChecked = True
                Else
                    mi.IsChecked = False
                End If
            Next
        End If
    End Sub
End Class
Public Class ARGBBrushMenu
    Inherits BaseBrushMenuItem
    Private tOpacity As New System.Windows.Controls.Label With {.Content = "A"}
    Private WithEvents efOpacity As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tR As New System.Windows.Controls.Label With {.Content = "R"}
    Private WithEvents efR As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tG As New System.Windows.Controls.Label With {.Content = "G"}
    Private WithEvents efG As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tB As New System.Windows.Controls.Label With {.Content = "B"}
    Private WithEvents efB As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Public Sub New()
        Title.Content = "Solid Color Brush "
        tOpacity.FontStyle = Windows.FontStyles.Italic
        tR.FontStyle = Windows.FontStyles.Italic
        tG.FontStyle = Windows.FontStyles.Italic
        tB.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(tOpacity)
        gdHost.AddColumnItem(efOpacity)
        gdHost.AddColumnItem(tR)
        gdHost.AddColumnItem(efR)
        gdHost.AddColumnItem(tG)
        gdHost.AddColumnItem(efG)
        gdHost.AddColumnItem(tB)
        gdHost.AddColumnItem(efB)
        efOpacity.Value = 255D
        efR.Value = 255D
        efG.Value = 255D
        efB.Value = 255D
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efOpacity.PreviewKeyDown, efR.PreviewKeyDown, efG.PreviewKeyDown, efB.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Protected Overrides Sub OnClick()
        If efOpacity.Value < 0 Then efOpacity.Value = 0
        If efOpacity.Value > 255 Then efOpacity.Value = 255
        If efR.Value < 0 Then efR.Value = 0
        If efR.Value > 255 Then efR.Value = 255
        If efG.Value < 0 Then efG.Value = 0
        If efG.Value > 255 Then efG.Value = 255
        If efB.Value < 0 Then efB.Value = 0
        If efB.Value > 255 Then efB.Value = 255
        Dim ef As New SolidColorBrush(Color.FromArgb(efOpacity.Value, efR.Value, efG.Value, efB.Value))
        Parent.SetBrush(ef)
        MyBase.OnClick()
    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is SolidColorBrush Then
            IsChecked = True
            Dim dse As SolidColorBrush = brsh
            efOpacity.Value = dse.Color.A
            efR.Value = dse.Color.R
            efG.Value = dse.Color.G
            efB.Value = dse.Color.B
        Else
            IsChecked = False
        End If
    End Sub
End Class
Public Class PickerBrushMenu
    Inherits BaseBrushMenuItem
    Private Picker As New ColorPicker
    Private WithEvents OK As New MenuItem With {.Header = "Select Brush"}
    Public Sub New()
        gdHost.Children.Remove(Title)
        gdHost.Children.Add(Picker)
        Height = 366D
        Items.Add(OK)
    End Sub

    'Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efOpacity.PreviewKeyDown, efR.PreviewKeyDown, efG.PreviewKeyDown, efB.PreviewKeyDown
    '    If e.Key = Key.Enter Then
    '        Me.OnClick()
    '    End If
    'End Sub
    Protected Overrides Sub OnClick()
        Parent.SetBrush(Picker.Brush)
        MyBase.OnClick()
    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is SolidColorBrush Then
            Picker.Brush = brsh
        Else
            IsChecked = False
        End If
    End Sub

    Private Sub OK_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles OK.Click
        OnClick()
    End Sub
End Class
Public Class ImageBrushMenu
    Inherits BaseBrushMenuItem
    Public Sub New()
        Title.Content = "Image ..."
    End Sub
    Protected Overrides Sub OnClick()
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
            Parent.SetBrush(imgb)
        Else

        End If

    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is System.Windows.Media.ImageBrush Then
            Me.IsChecked = True
        End If

    End Sub
End Class
Public Class PositionColorMenuItem
    Inherits MenuItem
    Protected gdHost As New GridBase
    Protected Title As New System.Windows.Controls.Label
    Private tPosition As New System.Windows.Controls.Label With {.Content = "P"}
    Private WithEvents efPosition As New MenuNumberBox With {.AllowDecimal = True, .AllowNegative = False}
    Private tOpacity As New System.Windows.Controls.Label With {.Content = "A"}
    Private WithEvents efOpacity As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tR As New System.Windows.Controls.Label With {.Content = "R"}
    Private WithEvents efR As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tG As New System.Windows.Controls.Label With {.Content = "G"}
    Private WithEvents efG As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private tB As New System.Windows.Controls.Label With {.Content = "B"}
    Private WithEvents efB As New MenuNumberBox With {.AllowDecimal = False, .AllowNegative = False}
    Private Shared Magnitude As Double = 420
    Public Sub New(vNode As String, vPos As Double)
        Header = gdHost
        Title.Content = vNode
        gdHost.AddColumnItem(Title)
        tPosition.FontStyle = Windows.FontStyles.Italic
        tOpacity.FontStyle = Windows.FontStyles.Italic
        tR.FontStyle = Windows.FontStyles.Italic
        tG.FontStyle = Windows.FontStyles.Italic
        tB.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(tPosition)
        gdHost.AddColumnItem(efPosition)
        gdHost.AddColumnItem(tOpacity)
        gdHost.AddColumnItem(efOpacity)
        gdHost.AddColumnItem(tR)
        gdHost.AddColumnItem(efR)
        gdHost.AddColumnItem(tG)
        gdHost.AddColumnItem(efG)
        gdHost.AddColumnItem(tB)
        gdHost.AddColumnItem(efB)
        efPosition.Value = vPos * Magnitude
        efOpacity.Value = 255D
        efR.Value = 255D
        efG.Value = 255D
        efB.Value = 255D
    End Sub
    Public ReadOnly Property Text As String
        Get
            Return Title.Content
        End Get
    End Property
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efPosition.PreviewKeyDown, efOpacity.PreviewKeyDown, efR.PreviewKeyDown, efG.PreviewKeyDown, efB.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Sub SetNode(Position As Double, c As Color)
        efPosition.Value = Position * Magnitude
        efOpacity.Value = c.A
        efR.Value = c.R
        efG.Value = c.G
        efB.Value = c.B
    End Sub
    Public Function GetPosition() As Double
        If efPosition.Value < 0 Then efPosition.Value = 0
        If efPosition.Value > Magnitude Then efPosition.Value = Magnitude
        Return efPosition.Value / Magnitude
    End Function
    Public Function GetColor() As Color
        If efR.Value < 0 Then efR.Value = 0
        If efR.Value > 255 Then efR.Value = 255
        If efG.Value < 0 Then efG.Value = 0
        If efG.Value > 255 Then efG.Value = 255
        If efB.Value < 0 Then efB.Value = 0
        If efB.Value > 255 Then efB.Value = 255
        Return Color.FromArgb(efOpacity.Value, efR.Value, efG.Value, efB.Value)
    End Function
End Class
Public Class PointMenuItem
    Inherits MenuItem
    Protected gdHost As New GridBase
    Protected Title As New System.Windows.Controls.Label
    Private tX As New System.Windows.Controls.Label With {.Content = "X"}
    Private WithEvents efX As New MenuNumberBox With {.AllowDecimal = True, .AllowNegative = True}
    Private tY As New System.Windows.Controls.Label With {.Content = "Y"}
    Private WithEvents efY As New MenuNumberBox With {.AllowDecimal = True, .AllowNegative = True}
    Public Sub New(vNode As String)
        Header = gdHost
        Title.Content = vNode
        gdHost.AddColumnItem(Title)
        tX.FontStyle = Windows.FontStyles.Italic
        tY.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(tX)
        gdHost.AddColumnItem(efX)
        gdHost.AddColumnItem(tY)
        gdHost.AddColumnItem(efY)
        efX.Value = 0D
        efY.Value = 0D
    End Sub
    Public ReadOnly Property Text As String
        Get
            Return Title.Content
        End Get
    End Property
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efX.PreviewKeyDown, efY.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Sub SetLocation(Position As System.Windows.Point)
        efX.Value = Position.X
        efY.Value = Position.Y
    End Sub
    Public Function GetLocation() As System.Windows.Point
        Return New Point(efX.Value, efY.Value)
    End Function
End Class
Public Class LinearGradientBrushMenu
    Inherits BaseBrushMenuItem
    Private mStart As New PointMenuItem("Start")
    Private mEnd As New PointMenuItem("End")
    Private _0 As New PositionColorMenuItem("0", 0D)
    Private _1 As New PositionColorMenuItem("1", 1D)
    Private WithEvents xAdd As New AddButton
    Private WithEvents xRemove As New DeleteButton
    Public Sub New()
        Title.Content = "Linear Gradient"
        gdHost.AddColumnItem(xAdd)
        gdHost.AddColumnItem(xRemove)
        mStart.SetLocation(P(0, 0.5))
        mEnd.SetLocation(P(1, 0.5))
        Items.Add(mStart)
        Items.Add(mEnd)
        Items.Add(_0)
        Items.Add(_1)
        For Each it As MenuItem In Items
            AddHandler it.Click, AddressOf ReadBrush
        Next
    End Sub
    Private Sub OnAdd(sender As Object, e As RoutedEventArgs) Handles xAdd.Click
        Dim it As PositionColorMenuItem = New PositionColorMenuItem("i", 0.5D)
        Items.Add(it)
        AddHandler it.Click, AddressOf ReadBrush
    End Sub
    Private Sub OnRemove(sender As Object, e As RoutedEventArgs) Handles xRemove.Click
        If Items.Count > 4 Then
            Dim it As PositionColorMenuItem = Items(Items.Count - 1)
            Items.Remove(it)
            RemoveHandler it.Click, AddressOf ReadBrush
        End If
    End Sub
    Private Sub Clear()
        Dim del As New List(Of PositionColorMenuItem)
        For Each it As MenuItem In Items
            If TypeOf it Is PositionColorMenuItem AndAlso (it IsNot _0 And it IsNot _1) Then del.Add(it)
        Next
        For Each it As PositionColorMenuItem In del
            Items.Remove(it)
        Next
    End Sub
    Private Sub ReadBrush(sender As Object, e As RoutedEventArgs)
        Dim lb As New LinearGradientBrush
        lb.MappingMode = BrushMappingMode.RelativeToBoundingBox
        lb.SpreadMethod = GradientSpreadMethod.Pad
        Dim colors As New Dictionary(Of Double, Color)
        For Each it As MenuItem In Items
            If TypeOf it Is PointMenuItem Then
                Dim pmi As PointMenuItem = it
                Select Case pmi.Text
                    Case "Start"
                        lb.StartPoint = pmi.GetLocation
                    Case "End"
                        lb.EndPoint = pmi.GetLocation
                End Select
            ElseIf TypeOf it Is PositionColorMenuItem Then
                Dim pci As PositionColorMenuItem = it
                Dim l As Double = pci.GetPosition
                If Not colors.ContainsKey(l) Then colors.Add(l, pci.GetColor)
            End If
        Next
        For Each u In colors.OrderBy(Function(kvp) kvp.Key)
            lb.GradientStops.Add(New GradientStop(u.Value, u.Key))
        Next
        Parent.SetBrush(lb)
    End Sub
    Protected Overrides Sub OnClick()
        MyBase.OnClick()
    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is LinearGradientBrush Then
            IsChecked = True
            Dim dse As LinearGradientBrush = brsh
            Clear()
            mStart.SetLocation(dse.StartPoint)
            mEnd.SetLocation(dse.EndPoint)
            For Each gs As GradientStop In dse.GradientStops
                If gs.Offset = 0D Then
                    _0.SetNode(0D, gs.Color)
                ElseIf gs.Offset = 1D Then
                    _1.SetNode(1D, gs.Color)
                Else
                    Dim it As PositionColorMenuItem = New PositionColorMenuItem("i", gs.Offset)
                    it.SetNode(gs.Offset, gs.Color)
                    Items.Add(it)
                    AddHandler it.Click, AddressOf ReadBrush
                End If
            Next
        Else
            IsChecked = False
        End If
    End Sub
End Class
Public Class RadiusGradientBrushMenu
    Inherits BaseBrushMenuItem
    Private mOrigin As New PointMenuItem("Origin")
    Private mCenter As New PointMenuItem("Center")
    Private mRadius As New PointMenuItem("Radius")
    Private _0 As New PositionColorMenuItem("0", 0D)
    Private _1 As New PositionColorMenuItem("1", 1D)
    Private WithEvents xAdd As New AddButton
    Private WithEvents xRemove As New DeleteButton
    Public Sub New()
        Title.Content = "Radius Gradient"
        gdHost.AddColumnItem(xAdd)
        gdHost.AddColumnItem(xRemove)
        mOrigin.SetLocation(P(0.5, 0.5))
        mCenter.SetLocation(P(0.5, 0.5))
        mRadius.SetLocation(P(0.5, 0.5))
        Items.Add(mOrigin)
        Items.Add(mCenter)
        Items.Add(mRadius)
        Items.Add(_0)
        Items.Add(_1)
        For Each it As MenuItem In Items
            AddHandler it.Click, AddressOf ReadBrush
        Next
    End Sub
    Private Sub OnAdd(sender As Object, e As RoutedEventArgs) Handles xAdd.Click
        Dim it As PositionColorMenuItem = New PositionColorMenuItem("i", 0.5D)
        Items.Add(it)
        AddHandler it.Click, AddressOf ReadBrush
    End Sub
    Private Sub OnRemove(sender As Object, e As RoutedEventArgs) Handles xRemove.Click
        If Items.Count > 4 Then
            Dim it As PositionColorMenuItem = Items(Items.Count - 1)
            Items.Remove(it)
            RemoveHandler it.Click, AddressOf ReadBrush
        End If
    End Sub
    Private Sub Clear()
        Dim del As New List(Of PositionColorMenuItem)
        For Each it As MenuItem In Items
            If TypeOf it Is PositionColorMenuItem AndAlso (it IsNot _0 And it IsNot _1) Then del.Add(it)
        Next
        For Each it As PositionColorMenuItem In del
            Items.Remove(it)
        Next
    End Sub
    Private Sub ReadBrush(sender As Object, e As RoutedEventArgs)
        Dim lb As New RadialGradientBrush
        lb.MappingMode = BrushMappingMode.RelativeToBoundingBox
        lb.SpreadMethod = GradientSpreadMethod.Pad
        Dim colors As New Dictionary(Of Double, Color)
        For Each it As MenuItem In Items
            If TypeOf it Is PointMenuItem Then
                Dim pmi As PointMenuItem = it
                Select Case pmi.Text
                    Case "Origin"
                        lb.GradientOrigin = pmi.GetLocation
                    Case "Center"
                        lb.Center = pmi.GetLocation
                    Case "Radius"
                        Dim p = pmi.GetLocation
                        lb.RadiusX = p.X
                        lb.RadiusY = p.Y
                End Select
            ElseIf TypeOf it Is PositionColorMenuItem Then
                Dim pci As PositionColorMenuItem = it
                Dim l As Double = pci.GetPosition
                If Not colors.ContainsKey(l) Then colors.Add(l, pci.GetColor)
            End If
        Next
        For Each u In colors.OrderBy(Function(kvp) kvp.Key)
            lb.GradientStops.Add(New GradientStop(u.Value, u.Key))
        Next
        Parent.SetBrush(lb)
    End Sub
    Protected Overrides Sub OnClick()
        MyBase.OnClick()
    End Sub
    Public Overrides Sub SetBrush(brsh As System.Windows.Media.Brush)
        If TypeOf brsh Is RadialGradientBrush Then
            IsChecked = True
            Dim dse As RadialGradientBrush = brsh
            Clear()
            mOrigin.SetLocation(dse.GradientOrigin)
            mCenter.SetLocation(dse.Center)
            mRadius.SetLocation(P(dse.RadiusX, dse.RadiusY))
            For Each gs As GradientStop In dse.GradientStops
                If gs.Offset = 0D Then
                    _0.SetNode(0D, gs.Color)
                ElseIf gs.Offset = 1D Then
                    _1.SetNode(1D, gs.Color)
                Else
                    Dim it As PositionColorMenuItem = New PositionColorMenuItem("i", gs.Offset)
                    it.SetNode(gs.Offset, gs.Color)
                    Items.Add(it)
                    AddHandler it.Click, AddressOf ReadBrush
                End If
            Next
        Else
            IsChecked = False
        End If
    End Sub
End Class
Public Class MenuNumberBox
    Inherits NumberBox
    Protected Overrides Sub OnMouseEnter(e As System.Windows.Input.MouseEventArgs)
        If Not IsFocused Then
            Focus()
            SelectAll()
        End If
        MyBase.OnMouseEnter(e)
    End Sub
End Class
Public Class DoublePropertyMenuItem
    Inherits PropertyMenuItem
    Private gdHost As New GridBase
    Private WithEvents stdValue As New MenuNumberBox
    Public Sub New(top As AppearanceMenu, pi As System.Reflection.PropertyInfo)
        MyBase.New(pi)
        Header = gdHost
        PropertyInfo = pi
        gdHost.AddColumnItem(New System.Windows.Controls.Label With {.Content = pi.Name})
        gdHost.AddColumnItem(stdValue)
        stdValue.AllowDecimal = True
        Me.Icon = IconImages.ImageFromString(IconImages.Visible, 20, 20)
        top.AddBinder(AddressOf Bind)
    End Sub
    Private Sub Bind(obj As Object)
        stdValue.Value = PropertyInfo.GetValue(obj, Empty)
    End Sub
    Protected Overrides Sub OnClick()
        PropertyInfo.SetValue(RelatedObject, stdValue.Value, Empty)
        MyBase.OnClick()
    End Sub
    Private Sub stdValue_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles stdValue.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
End Class
Public Class FontStylePropertyMenuItem
    Inherits PropertyMenuItem
    Private gdHost As New GridBase
    Public Sub New(top As AppearanceMenu, pi As System.Reflection.PropertyInfo)
        MyBase.New(pi)
        Header = gdHost
        PropertyInfo = pi
        For Each fpi As System.Reflection.PropertyInfo In GetType(FontStyles).GetProperties
            Dim mi As New MenuItem
            mi.Header = fpi.Name
            mi.FontStyle = fpi.GetValue(Nothing, Empty)
            Items.Add(mi)
            mi.IsCheckable = True
            AddHandler mi.Click, AddressOf OnSubItemsClick
        Next
        gdHost.AddColumnItem(New System.Windows.Controls.Label With {.Content = pi.Name})

        Me.Icon = IconImages.ImageFromString(IconImages.Font, 20, 20)
        top.AddBinder(AddressOf Bind)
    End Sub
    Private Sub Bind(obj As Object)
        For Each mi As MenuItem In Items
            mi.IsChecked = mi.FontStyle = PropertyInfo.GetValue(obj, Empty)
        Next
    End Sub
    Private Sub OnSubItemsClick(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = sender
        PropertyInfo.SetValue(RelatedObject, mi.FontStyle, Empty)
    End Sub
End Class
Public Class FontWeightPropertyMenuItem
    Inherits PropertyMenuItem
    Private gdHost As New GridBase
    Public Sub New(top As AppearanceMenu, pi As System.Reflection.PropertyInfo)
        MyBase.New(pi)
        Header = gdHost
        PropertyInfo = pi
        For Each fpi As System.Reflection.PropertyInfo In GetType(FontWeights).GetProperties
            Dim mi As New MenuItem
            mi.Header = fpi.Name
            mi.FontWeight = fpi.GetValue(Nothing, Empty)
            Items.Add(mi)
            mi.IsCheckable = True
            AddHandler mi.Click, AddressOf OnSubItemsClick
        Next
        gdHost.AddColumnItem(New System.Windows.Controls.Label With {.Content = pi.Name})

        Me.Icon = IconImages.ImageFromString(IconImages.Font, 20, 20)
        top.AddBinder(AddressOf Bind)
    End Sub
    Private Sub Bind(obj As Object)
        For Each mi As MenuItem In Items
            mi.IsChecked = mi.FontWeight = PropertyInfo.GetValue(obj, Empty)
        Next
    End Sub
    Private Sub OnSubItemsClick(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = sender
        PropertyInfo.SetValue(RelatedObject, mi.FontWeight, Empty)
    End Sub
End Class
Public Class EffectPropertyMenu
    Inherits PropertyMenuItem
    Public Sub New(top As AppearanceMenu, pi As System.Reflection.PropertyInfo)
        MyBase.New(pi)
        top.AddBinder(AddressOf Bind)
        Header = pi.Name
        Me.Icon = IconImages.ImageFromString(IconImages.Effect, 20, 20)
        Items.Add(New NoEffectMenu)
        Items.Add(New BlurEffectMenu)
        Items.Add(New PixelateEffectMenu)
        Items.Add(New DropShadowEffectMenu)
        Items.Add(New BloomEffectMenu)
        Items.Add(New RippleEffectMenu)
        Dim bd As New Border
        bd.CornerRadius = New CornerRadius()
    End Sub
    Private Sub Bind(obj As Object)
        For Each mi As BaseEffectMenu In Items
            mi.SetEffect(PropertyInfo.GetValue(obj, Empty))
        Next
    End Sub
    Public Sub SetEffect(ef As Effects.Effect)
        PropertyInfo.SetValue(RelatedObject, ef, Empty)
    End Sub
End Class
Public MustInherit Class BaseEffectMenu
    Inherits MenuItem
    Protected gdHost As New GridBase
    Public Sub New()
        Header = gdHost
        IsCheckable = True
    End Sub
    Public MustOverride Sub SetEffect(ef As Effects.Effect)
    Public Shadows ReadOnly Property Parent As EffectPropertyMenu
        Get
            Return MyBase.Parent
        End Get
    End Property
End Class
Public Class NoEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "No Effect "}
    Public Sub New()
        gdHost.AddColumnItem(Title)
    End Sub

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If ef Is Nothing Then
            IsChecked = True
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Parent.SetEffect(Nothing)
        MyBase.OnClick()
    End Sub
End Class
Public Class BlurEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "Blur Effect "}
    Private tRadius As New System.Windows.Controls.Label With {.Content = "Radius:"}
    Private WithEvents Radius As New MenuNumberBox With {.AllowDecimal = True}
    Public Sub New()
        tRadius.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(Title)
        gdHost.AddColumnItem(tRadius)
        gdHost.AddColumnItem(Radius)
        Radius.Value = 2D
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Radius.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Effects.BlurEffect Then
            IsChecked = True
            Radius.Value = DirectCast(ef, Effects.BlurEffect).Radius
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Dim ef As New Effects.BlurEffect
        ef.Radius = Radius.Value
        Parent.SetEffect(ef)
        MyBase.OnClick()
    End Sub
End Class
Public Class PixelateEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "Pixelate Effect "}
    Private tRadius As New System.Windows.Controls.Label With {.Content = "Pixelation:"}
    Private WithEvents Radius As New MenuNumberBox With {.AllowDecimal = True, .AllowNegative = False}
    Public Sub New()
        tRadius.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(Title)
        gdHost.AddColumnItem(tRadius)
        gdHost.AddColumnItem(Radius)
        Radius.Value = 50D
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Radius.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Microsoft.Expression.Media.Effects.PixelateEffect Then
            IsChecked = True
            Radius.Value = DirectCast(ef, Microsoft.Expression.Media.Effects.PixelateEffect).Pixelation * 100
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Dim ef As New Microsoft.Expression.Media.Effects.PixelateEffect
        If Radius.Value > 100 Then Radius.Value = 100
        If Radius.Value < 0 Then Radius.Value = 0
        ef.Pixelation = Radius.Value / 100
        Parent.SetEffect(ef)
        MyBase.OnClick()
    End Sub
End Class
Public Class DropShadowEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "DropShadow Effect "}
    Private tBlurRadius As New System.Windows.Controls.Label With {.Content = "Radius:"}
    Private WithEvents efBlurRadius As New MenuNumberBox With {.AllowDecimal = True}
    Private tDirection As New System.Windows.Controls.Label With {.Content = "Direction:"}
    Private WithEvents efDirection As New MenuNumberBox With {.AllowDecimal = True}
    Private tOpacity As New System.Windows.Controls.Label With {.Content = "Opacity:"}
    Private WithEvents efOpacity As New MenuNumberBox With {.AllowDecimal = True}
    Private tR As New System.Windows.Controls.Label With {.Content = "R"}
    Private WithEvents efR As New MenuNumberBox With {.AllowDecimal = False}
    Private tG As New System.Windows.Controls.Label With {.Content = "G"}
    Private WithEvents efG As New MenuNumberBox With {.AllowDecimal = False}
    Private tB As New System.Windows.Controls.Label With {.Content = "B"}
    Private WithEvents efB As New MenuNumberBox With {.AllowDecimal = False}
    Public Sub New()
        tBlurRadius.FontStyle = Windows.FontStyles.Italic
        tDirection.FontStyle = Windows.FontStyles.Italic
        tOpacity.FontStyle = Windows.FontStyles.Italic
        tR.FontStyle = Windows.FontStyles.Italic
        tG.FontStyle = Windows.FontStyles.Italic
        tB.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(Title)
        gdHost.AddColumnItem(tBlurRadius)
        gdHost.AddColumnItem(efBlurRadius)
        gdHost.AddColumnItem(tDirection)
        gdHost.AddColumnItem(efDirection)
        gdHost.AddColumnItem(tOpacity)
        gdHost.AddColumnItem(efOpacity)
        gdHost.AddColumnItem(tR)
        gdHost.AddColumnItem(efR)
        gdHost.AddColumnItem(tG)
        gdHost.AddColumnItem(efG)
        gdHost.AddColumnItem(tB)
        gdHost.AddColumnItem(efB)

        efBlurRadius.Value = 2D
        efDirection.Value = -45D
        efOpacity.Value = 0.5D
        efR.Value = 0
        efG.Value = 0
        efB.Value = 0
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efBlurRadius.PreviewKeyDown, efDirection.PreviewKeyDown,
        efOpacity.PreviewKeyDown, efR.PreviewKeyDown, efG.PreviewKeyDown, efB.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Effects.DropShadowEffect Then
            IsChecked = True
            Dim dse As Effects.DropShadowEffect = ef
            efBlurRadius.Value = dse.BlurRadius
            efDirection.Value = dse.Direction
            efOpacity.Value = dse.Opacity
            efR.Value = dse.Color.R
            efG.Value = dse.Color.G
            efB.Value = dse.Color.B
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Dim ef As New Effects.DropShadowEffect
        ef.BlurRadius = efBlurRadius.Value
        ef.Direction = efDirection.Value
        ef.Opacity = efOpacity.Value
        If efR.Value < 0 Then efR.Value = 0
        If efR.Value > 255 Then efR.Value = 255
        If efG.Value < 0 Then efG.Value = 0
        If efG.Value > 255 Then efG.Value = 255
        If efB.Value < 0 Then efB.Value = 0
        If efB.Value > 255 Then efB.Value = 255
        ef.Color = Color.FromArgb(255, efR.Value, efG.Value, efB.Value)
        Parent.SetEffect(ef)
        MyBase.OnClick()
    End Sub
End Class
Public Class BloomEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "Bloom Effect "}
    Private tBaseIntensity As New System.Windows.Controls.Label With {.Content = "BaseIntensity:"}
    Private WithEvents efBaseIntensity As New MenuNumberBox With {.AllowDecimal = True}
    Private tBloomIntensity As New System.Windows.Controls.Label With {.Content = "BloomIntensity:"}
    Private WithEvents efBloomIntensity As New MenuNumberBox With {.AllowDecimal = True}
    Private tBaseSaturation As New System.Windows.Controls.Label With {.Content = "BaseSaturation:"}
    Private WithEvents efBaseSaturation As New MenuNumberBox With {.AllowDecimal = True}
    Private tBloomSaturation As New System.Windows.Controls.Label With {.Content = "BloomSaturation"}
    Private WithEvents efBloomSaturation As New MenuNumberBox With {.AllowDecimal = True}
    Private tThreshold As New System.Windows.Controls.Label With {.Content = "Threshold"}
    Private WithEvents efThreshold As New MenuNumberBox With {.AllowDecimal = True}
    Public Sub New()
        tBaseIntensity.FontStyle = Windows.FontStyles.Italic
        tBloomIntensity.FontStyle = Windows.FontStyles.Italic
        tBaseSaturation.FontStyle = Windows.FontStyles.Italic
        tBloomSaturation.FontStyle = Windows.FontStyles.Italic
        tThreshold.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(Title)
        gdHost.AddColumnItem(tBaseIntensity)
        gdHost.AddColumnItem(efBaseIntensity)
        gdHost.AddColumnItem(tBloomIntensity)
        gdHost.AddColumnItem(efBloomIntensity)
        gdHost.AddColumnItem(tBaseSaturation)
        gdHost.AddColumnItem(efBaseSaturation)
        gdHost.AddColumnItem(tBloomSaturation)
        gdHost.AddColumnItem(efBloomSaturation)
        gdHost.AddColumnItem(tThreshold)
        gdHost.AddColumnItem(efThreshold)
        efBaseIntensity.Value = 1D
        efBloomIntensity.Value = 1D
        efBaseSaturation.Value = 1D
        efBloomSaturation.Value = 1D
        efThreshold.Value = 0.25D
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efBaseIntensity.PreviewKeyDown, efBloomIntensity.PreviewKeyDown,
        efBaseSaturation.PreviewKeyDown, efBloomSaturation.PreviewKeyDown, efThreshold.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Microsoft.Expression.Media.Effects.BloomEffect Then
            IsChecked = True
            Dim dse As Microsoft.Expression.Media.Effects.BloomEffect = ef
            efBaseIntensity.Value = dse.BaseIntensity
            efBloomIntensity.Value = dse.BloomIntensity
            efBaseSaturation.Value = dse.BaseSaturation
            efBloomSaturation.Value = dse.BloomSaturation
            efThreshold.Value = dse.Threshold
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Dim ef As New Microsoft.Expression.Media.Effects.BloomEffect
        ef.BaseIntensity = efBaseIntensity.Value
        ef.BloomIntensity = efBloomIntensity.Value
        ef.BaseSaturation = efBaseSaturation.Value
        ef.BloomSaturation = efBloomSaturation.Value
        ef.Threshold = efThreshold.Value
        Parent.SetEffect(ef)
        MyBase.OnClick()
    End Sub
End Class
Public Class RippleEffectMenu
    Inherits BaseEffectMenu
    Private Title As New System.Windows.Controls.Label With {.Content = "Ripple Effect "}
    Private tBaseIntensity As New System.Windows.Controls.Label With {.Content = "CenterX:"}
    Private WithEvents efBaseIntensity As New MenuNumberBox With {.AllowDecimal = True}
    Private tBloomIntensity As New System.Windows.Controls.Label With {.Content = "CenterY:"}
    Private WithEvents efBloomIntensity As New MenuNumberBox With {.AllowDecimal = True}
    Private tBaseSaturation As New System.Windows.Controls.Label With {.Content = "Frequency:"}
    Private WithEvents efBaseSaturation As New MenuNumberBox With {.AllowDecimal = True}
    Private tBloomSaturation As New System.Windows.Controls.Label With {.Content = "Magnitude"}
    Private WithEvents efBloomSaturation As New MenuNumberBox With {.AllowDecimal = True}
    Private tThreshold As New System.Windows.Controls.Label With {.Content = "Phase"}
    Private WithEvents efThreshold As New MenuNumberBox With {.AllowDecimal = True}
    Public Sub New()
        tBaseIntensity.FontStyle = Windows.FontStyles.Italic
        tBloomIntensity.FontStyle = Windows.FontStyles.Italic
        tBaseSaturation.FontStyle = Windows.FontStyles.Italic
        tBloomSaturation.FontStyle = Windows.FontStyles.Italic
        tThreshold.FontStyle = Windows.FontStyles.Italic
        gdHost.AddColumnItem(Title)
        gdHost.AddColumnItem(tBaseIntensity)
        gdHost.AddColumnItem(efBaseIntensity)
        gdHost.AddColumnItem(tBloomIntensity)
        gdHost.AddColumnItem(efBloomIntensity)
        gdHost.AddColumnItem(tBaseSaturation)
        gdHost.AddColumnItem(efBaseSaturation)
        gdHost.AddColumnItem(tBloomSaturation)
        gdHost.AddColumnItem(efBloomSaturation)
        gdHost.AddColumnItem(tThreshold)
        gdHost.AddColumnItem(efThreshold)
        efBaseIntensity.Value = 0.5D
        efBloomIntensity.Value = 0.5D
        efBaseSaturation.Value = 80D
        efBloomSaturation.Value = 0.02D
        efThreshold.Value = 0D
    End Sub
    Private Sub Value_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles efBaseIntensity.PreviewKeyDown, efBloomIntensity.PreviewKeyDown,
        efBaseSaturation.PreviewKeyDown, efBloomSaturation.PreviewKeyDown, efThreshold.PreviewKeyDown
        If e.Key = Key.Enter Then
            Me.OnClick()
        End If
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Microsoft.Expression.Media.Effects.RippleEffect Then
            IsChecked = True
            Dim dse As Microsoft.Expression.Media.Effects.RippleEffect = ef
            efBaseIntensity.Value = dse.Center.X
            efBloomIntensity.Value = dse.Center.Y
            efBaseSaturation.Value = dse.Frequency
            efBloomSaturation.Value = dse.Magnitude
            efThreshold.Value = dse.Phase
        Else
            IsChecked = False
        End If
    End Sub
    Protected Overrides Sub OnClick()
        Dim ef As New Microsoft.Expression.Media.Effects.RippleEffect
        ef.Center = New Point(efBaseIntensity.Value, efBloomIntensity.Value)
        ef.Frequency = efBaseSaturation.Value
        ef.Magnitude = efBloomSaturation.Value
        ef.Phase = efThreshold.Value
        Parent.SetEffect(ef)
        MyBase.OnClick()
    End Sub
End Class


Public Class VisibilityMenu
    Inherits System.Windows.Controls.ContextMenu
    Private miVisible As New MenuItem
    Private viIcon As Image = IconImages.ImageFromString(IconImages.Visible, 20, 20)
    Private viEffect As New Microsoft.Expression.Media.Effects.BloomEffect
    Public Sub New()
        'miVisible.IsCheckable = True
        miVisible.Header = "Visible"
        miVisible.Icon = viIcon
        viIcon.Effect = viEffect
        viEffect.BaseSaturation = 1
        Items.Add(miVisible)
        AddHandler miVisible.Click, AddressOf OnVisibleChanged
    End Sub
    Private _Visible As Boolean
    Public Sub Show(value As Boolean)
        viEffect.BloomSaturation = IIf(value, 1, -0.5)
        _Visible = value
        IsOpen = True
    End Sub
    Private Sub OnVisibleChanged(sender As Object, e As RoutedEventArgs)
        RaiseEvent VisibilityChanged(Me, New VisibleEventArgs With {.Visible = Not _Visible})
    End Sub
    Public Event VisibilityChanged(sender As Object, e As VisibleEventArgs)
    Public Shared DefaultMenu As New VisibilityMenu
End Class
Public Class VisibleEventArgs
    Inherits EventArgs
    Public Property Visible As Boolean
End Class

Public Class ColorMenu
    Inherits System.Windows.Controls.ContextMenu
    Private stdColors As New StandardColors
    Public Sub New()
        AddHandler stdColors.ColorSelected, AddressOf OnColorSelected
        Me.Items.Add(stdColors)
    End Sub
    Public Event BrushSelected(sender As Object, e As BrushEventArgs)
    Private Sub OnColorSelected(sender As Object, e As BrushEventArgs)
        RaiseEvent BrushSelected(Me, e)
    End Sub
    Public Shared DefaultMenu As New ColorMenu
End Class
Public Class StandardColors
    Inherits System.Windows.Controls.MenuItem
    Public Sub New()
        Header = "Standard Colors"
        Dim t As Type = GetType(System.Windows.Media.Brushes)
        Dim tSCB As Type = GetType(SolidColorBrush)
        For Each pi As System.Reflection.PropertyInfo In t.GetProperties
            If pi.PropertyType Is tSCB Then
                Dim mi As New System.Windows.Controls.MenuItem
                mi.Background = pi.GetValue(Nothing, New Object() {})
                mi.Header = pi.Name
                mi.Foreground = ReverseColor(mi.Background)
                AddHandler mi.Click, AddressOf OnSubItemsClick
                Me.Items.Add(mi)
            End If
        Next
        Me.Icon = IconImages.ImageFromString(IconImages.Color, 20, 20)
    End Sub
    Public Event ColorSelected(sender As Object, e As BrushEventArgs)
    Private Sub OnSubItemsClick(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = sender
        RaiseEvent ColorSelected(Me, New BrushEventArgs With {.Brush = mi.Background})
    End Sub
End Class

Public Class BrushEventArgs
    Inherits EventArgs
    Public Brush As System.Windows.Media.Brush
End Class