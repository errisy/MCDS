Imports System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows, System.Windows.Shapes, Microsoft.Expression.Media.Effects, System.Windows.Media.Effects
Public Class EffectEditor
    Inherits GridBase
    Public RelatedProperty As PropertyItem
    Private Picker As New BrushEditor(True)
    Public ValueCallBack As Action(Of Effects.Effect)
    Private None As New NoEffectEditor
    Private Blur As New BlurEffectEditor
    Private Pixelate As New PixelateEffectEditor
    Private DropShadow As New DropShadowEffectEditor
    Private Bloom As New BloomEffectEditor
    Private Ripple As New RippleEffectEditor
    Private Swirl As New SwirlEffectEditor
    Private ColorTone As New ColorToneEffectEditor
    Private MonoChrome As New MonoChromeEffectEditor
    Private Sharpen As New SharpenEffectEditor
    Public Styles As New EffectStyleEditor
    Public Sub New()
        Add(None)
        Add(Blur)
        Add(Pixelate)
        Add(DropShadow)
        Add(Bloom)
        Add(Ripple)
        Add(Swirl)
        Add(ColorTone)
        Add(MonoChrome)
        Add(Sharpen)
        AddRowItem(Picker)
        Add(Styles)
        DropShadow.zColor.RequiredColorEditor = Picker
        ColorTone.zCB.RequiredColorEditor = Picker
        ColorTone.zCD.RequiredColorEditor = Picker
        MonoChrome.zColor.RequiredColorEditor = Picker
    End Sub
    Private Editors As New List(Of BaseEffectEditor)
    Public Sub Add(ee As BaseEffectEditor)
        Editors.Add(ee)
        AddRowItem(ee)
        ee.ValueCallBack = AddressOf SelectEffect
    End Sub
    Public Sub SelectEffect(ef As Effects.Effect)
        ValueCallBack.Invoke(ef)
    End Sub
    Public Sub SetEffect(ef As Effect)
        For Each ed As BaseEffectEditor In Editors
            ed.SetEffect(ef)
        Next
    End Sub
End Class
Public MustInherit Class BaseEffectEditor
    Inherits Expander
    Protected gdHost As New GridBase
    Property TG As New Trigger
    Public Sub New()
        Header = Title
        Content = gdHost
        TG.HostingGrid = gdHost
        TG.ChangeCall = AddressOf WhenChange
    End Sub
    Public MustOverride ReadOnly Property Title As String
    Private Sub MLD(o As Object, e As RoutedEventArgs)
        ValueCallBack.Invoke(Nothing)
    End Sub
    Public ValueCallBack As Action(Of Effects.Effect)
    Public MustOverride Sub WhenChange()
    Public MustOverride Sub SetEffect(ef As Effects.Effect)
End Class
Public Class Trigger
    Private Sub GetValueChanged(sender As Object, e As RoutedEventArgs)
        If ChangeCall.OK Then ChangeCall.Invoke()
    End Sub
    Public Sub Add(Obj As IValueEditor)
        HostingGrid.AddRowItem(Obj)
        AddHandler Obj.ValueChanged, AddressOf GetValueChanged
    End Sub
    Public HostingGrid As GridBase
    Public ChangeCall As System.Action
End Class
Public Class NoEffectEditor
    Inherits BaseEffectEditor
    Private NoLabel As New TouchBox With {.Content = "Set No Effect"}
    Public Sub New()
        Header = "Click to Clear Effects"
        Content = gdHost
        TG.Add(NoLabel)
        AddHandler NoLabel.MouseLeftButtonDown, AddressOf MLD
    End Sub
    Private Sub MLD(o As Object, e As RoutedEventArgs)
        ValueCallBack.Invoke(Nothing)
    End Sub
    Public Overrides ReadOnly Property Title As String
        Get
            Return "No Effect"
        End Get
    End Property
    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(Nothing)
    End Sub

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)

    End Sub
End Class
Public Class BlurEffectEditor
    Inherits BaseEffectEditor

    Private Radius As New DoubleBox With {.Title = "R"}
    Public Sub New()
        TG.Add(Radius)
    End Sub
    Public Overrides ReadOnly Property Title As String
        Get
            Return "Blur"
        End Get
    End Property
    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New Effects.BlurEffect With {.Radius = Radius.Value})
    End Sub

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is Effects.BlurEffect Then
            With ef.AsBlur()
                Radius.Value = .Radius
            End With
        End If
    End Sub
End Class
Public Class PixelateEffectEditor
    Inherits BaseEffectEditor

    Private Pixelation As New DoubleBox With {.Title = "Pixelation"}
    Public Sub New()
        TG.Add(Pixelation)
    End Sub
    Public Overrides ReadOnly Property Title As String
        Get
            Return "Pixelate"
        End Get
    End Property
    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New Microsoft.Expression.Media.Effects.PixelateEffect With {.Pixelation = Pixelation.Value / 100D})
    End Sub

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is PixelateEffect Then
            With ef.AsPixelate()
                Pixelation.Value = .Pixelation * 100D
            End With

        End If
    End Sub
End Class
Public Class DropShadowEffectEditor
    Inherits BaseEffectEditor
    Private zRadius As New DoubleBox With {.Title = "R"}
    Private zDirection As New DoubleBox With {.Title = "Angle"}
    Private zOpacity As New DoubleBox With {.Title = "Opacity"}
    Private zDepth As New DoubleBox With {.Title = "Depth"}
    Public zColor As New ColorBox With {.Title = ""}
    Public Sub New()
        TG.Add(zRadius)
        TG.Add(zDirection)
        TG.Add(zOpacity)
        TG.Add(zDepth)
        TG.Add(zColor)
    End Sub
    Public Overrides ReadOnly Property Title As String
        Get
            Return "DropShadow"
        End Get
    End Property
    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New Effects.DropShadowEffect With {.Color = zColor.Value, .BlurRadius = zRadius.Value, .Direction = zDirection.Value, .Opacity = zOpacity.Value / 100D, .ShadowDepth = zDepth.Value})
    End Sub

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is DropShadowEffect Then
            With ef.AsDropShadow
                zRadius.Value = .BlurRadius
                zDirection.Value = .Direction
                zOpacity.Value = .Opacity * 100D
                zDepth.Value = .ShadowDepth
                zColor.Value = .Color
            End With
        End If
    End Sub
End Class
Public Class BloomEffectEditor
    Inherits BaseEffectEditor
    Private zBsInt As New DoubleBox With {.Title = "BaseIntensity"}
    Private zBlInt As New DoubleBox With {.Title = "BloomIntensity"}
    Private zBsSat As New DoubleBox With {.Title = "BaseSaturation"}
    Private zBlSat As New DoubleBox With {.Title = "BloomSaturation"}
    Private zThr As New DoubleBox With {.Title = "Threshold"}
    Public Sub New()
        TG.Add(zBsInt)
        TG.Add(zBlInt)
        TG.Add(zBsSat)
        TG.Add(zBlSat)
        TG.Add(zThr)
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "Bloom"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New BloomEffect With {.BaseIntensity = zBsInt.Value / 100D, .BaseSaturation = zBsSat.Value / 100D,
                                                    .BloomIntensity = zBlInt.Value / 100D, .BloomSaturation = zBlSat.Value / 100D, .Threshold = zThr.Value / 100D})
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is BloomEffect Then
            With ef.AsBloom
                zBsInt.Value = .BaseIntensity * 100D
                zBlInt.Value = .BloomIntensity * 100D
                zBsSat.Value = .BaseSaturation * 100D
                zBlSat.Value = .BloomSaturation * 100D
                zThr.Value = .Threshold * 100D
            End With
        End If
    End Sub
End Class
Public Class RippleEffectEditor
    Inherits BaseEffectEditor
    Private zCenter As New PointBox With {.Title = "Center"}
    Private zFre As New DoubleBox With {.Title = "Frequency"}
    Private zMag As New DoubleBox With {.Title = "Magnitude"}
    Private zPhs As New DoubleBox With {.Title = "Phase"}
    Public Sub New()
        TG.Add(zCenter)
        TG.Add(zFre)
        TG.Add(zMag)
        TG.Add(zPhs)
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is RippleEffect Then
            With ef.AsRipple
                zCenter.Value = (.Center.AsVector - V(0.5D, 0.5D)) * 100D
                zFre.Value = .Frequency
                zMag.Value = .Magnitude * 100D
                zPhs.Value = .Phase
            End With
        End If
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "Ripple"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New RippleEffect With {.Center = (CType(zCenter.Value, System.Windows.Point).AsVector + V(50, 50)) / 100D, .Frequency = zFre.Value, .Magnitude = zMag.Value / 100D, .Phase = zPhs.Value})
    End Sub
End Class
Public Class SwirlEffectEditor
    Inherits BaseEffectEditor
    Private zCenter As New PointBox With {.Title = "Center"}
    Private zTA As New DoubleBox With {.Title = "TwistAmount"}
    Public Sub New()
        TG.Add(zCenter)
        TG.Add(zTA)
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is SwirlEffect Then
            With ef.AsSwirl
                zCenter.Value = (.Center.AsVector - V(0.5D, 0.5D)) * 100D
                zTA.Value = .TwistAmount * 100D
            End With
        End If
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "Swirl"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New SwirlEffect With {.Center = CType(zCenter.Value, System.Windows.Point).AsVector / 100D + V(0.5D, 0.5D), .TwistAmount = zTA.Value / 100D})
    End Sub
End Class
Public Class ColorToneEffectEditor
    Inherits BaseEffectEditor
    Public zCB As New ColorBox With {.Title = "LightColor"}
    Public zCD As New ColorBox With {.Title = "DarkColor"}
    Private zDS As New DoubleBox With {.Title = "Desaturation"}
    Private zTA As New DoubleBox With {.Title = "ToneAmount"}
    Public Sub New()
        TG.Add(zCB)
        TG.Add(zCD)
        TG.Add(zDS)
        TG.Add(zTA)
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is ColorToneEffect Then
            With ef.AsColorTone
                zCB.Value = .LightColor
                zCD.Value = .DarkColor
                zDS.Value = .Desaturation * 100D
                zTA.Value = .ToneAmount * 100D
            End With
        End If
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "ColorTone"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New ColorToneEffect With {.LightColor = zCB.Value, .DarkColor = zCD.Value, .Desaturation = zDS.Value / 100D, .ToneAmount = zTA.Value / 100D})
    End Sub
End Class
Public Class MonoChromeEffectEditor
    Inherits BaseEffectEditor
    Public zColor As New ColorBox With {.Title = "Color"}
    Public Sub New()
        TG.Add(zColor)

    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is MonochromeEffect Then
            With ef.AsMonochrome
                zColor.Value = .Color
            End With
        End If
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "MonoChrome"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New MonochromeEffect With {.Color = zColor.Value})
    End Sub
End Class
Public Class SharpenEffectEditor
    Inherits BaseEffectEditor
    Private zH As New DoubleBox With {.Title = "Height"}
    Private zA As New DoubleBox With {.Title = "Amount"}
    Public Sub New()
        TG.Add(zH)
        TG.Add(zA)
    End Sub
    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        If TypeOf ef Is MonochromeEffect Then
            With ef.AsSharpen
                zH.Value = .Height * 100D
                zA.Value = .Amount * 100D
            End With
        End If
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "Sharpen"
        End Get
    End Property

    Public Overrides Sub WhenChange()
        ValueCallBack.Invoke(New SharpenEffect With {.Height = zH.Value / 100D, .Amount = zA.Value / 100D})
    End Sub
End Class

Public Class EffectStyleEditor
    Inherits BaseEffectEditor
    Implements IValueEditor
    'Private HostGrid As New GridBase
    Private HeaderGrid As New GridBase
    Private WithEvents ImageList As New ListView
    Private WithEvents btnAdd As New AddButton
    Private WithEvents btnDelete As New DeleteButton
    Public Sub New()
        MyBase.New()
        gdHost.AddRowItem(HeaderGrid)
        'gdHost.VerticalAlignment = Windows.VerticalAlignment.Stretch
        HeaderGrid.AddColumnItem(btnAdd)
        gdHost.AddRowItem(ImageList)
        ImageList.Height = 300
        Setting = True
 
        Setting = False
    End Sub
    Public Sub Activate()
        ImageList.Items.Clear()
        For Each bs As EffectStyle In Source
            Dim it = EffectStyleItem.FromEffectStyle(bs)
            it.Host = Me
            ImageList.Items.Add(it)
        Next
    End Sub
    Private CurrentValue As Effect
    Public Property Value As Object Implements IValueEditor.Value
        Get
            If TypeOf ImageList.SelectedItem Is EffectStyleItem Then
                Dim esi As EffectStyleItem = ImageList.SelectedItem
                Return esi.Value
            Else
                Return Nothing
            End If
        End Get
        Set(value As Object)
            Setting = True
            CurrentValue = value
            Setting = False
        End Set
    End Property
    Private Setting As Boolean = False
    Public Source As ShallowList(Of EffectStyle)
    Public Event ValueChanged(sender As Object, e As System.Windows.RoutedEventArgs) Implements IValueEditor.ValueChanged
    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        If CurrentValue.OK Then
            Dim bs As New EffectStyle With {.Name = "New", .Vector = New EffectVector(CurrentValue)}
            If Source.OK Then Source.Add(bs)
            Dim it = EffectStyleItem.FromEffectStyle(bs)
            it.Host = Me
            ImageList.Items.Add(it)
        End If
    End Sub
    Public Sub SelectItem(it As EffectStyleItem)
        If Setting Then Exit Sub
        ImageList.SelectedItem = it
        RaiseEvent ValueChanged(Me, New RoutedEventArgs)
    End Sub
 

    Public Overrides Sub SetEffect(ef As System.Windows.Media.Effects.Effect)
        CurrentValue = ef
        Activate()
    End Sub

    Public Overrides ReadOnly Property Title As String
        Get
            Return "Effect Styles"
        End Get
    End Property
    Public Overrides Sub WhenChange()
        If TypeOf ImageList.SelectedItem Is EffectStyleItem Then
            Dim esi As EffectStyleItem = ImageList.SelectedItem
            ValueCallBack.Invoke(esi.Value)
        Else
            ValueCallBack.Invoke(Nothing)
        End If
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If TypeOf ImageList.SelectedItem Is EffectStyleItem Then
            Dim bsi As EffectStyleItem = ImageList.SelectedItem
            ImageList.Items.Remove(ImageList)
            Source.Remove(bsi.RelatedEffectStyle)
        End If
    End Sub
End Class


Public Class EffectStyleItem
    Inherits GridBase
    Public WithEvents EffectView As New EffectBox
    Public WithEvents Key As New EditBox
    Public Host As EffectStyleEditor
    Public Sub New()
        AddColumnItem(EffectView, "80")
        AddColumnItem(Key)
    End Sub
    Private Setting As Boolean = False
    Public Property EffectName As String
        Get
            Return Key.Text
        End Get
        Set(value As String)
            Setting = True
            Key.Text = value
            Setting = False
        End Set
    End Property

    Public RelatedEffectStyle As EffectStyle
    Public Property Value As Effect
        Get
            Return EffectView.Value
        End Get
        Set(value As Effect)
            EffectView.Value = value
        End Set
    End Property
    Public Shared Function FromEffectStyle(bs As EffectStyle) As EffectStyleItem
        Dim bsi As New EffectStyleItem
        bsi.Setting = True
        bsi.EffectName = bs.Name
        bsi.RelatedEffectStyle = bs
        bsi.EffectView.Value = bs.Vector.GetValue
        bsi.Setting = False
        Return bsi
    End Function
    Public Sub SelectMe()
        If Host.OK Then Host.SelectItem(Me)
    End Sub
    Private Sub BrushView_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles EffectView.MouseLeftButtonDown
        SelectMe()
    End Sub
    Private Sub Key_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles Key.TextChanged
        If Setting Then Exit Sub
        If RelatedEffectStyle.OK Then RelatedEffectStyle.Name = Key.Text
    End Sub

End Class
 
<Serializable()>
Public Class EffectStyle
    Public Name As String
    Public Vector As EffectVector
End Class


Public Module EffectFunctions
    <System.Runtime.CompilerServices.Extension()> Public Function AsBlur(ef As Effects.Effect) As Effects.BlurEffect
        Return DirectCast(ef, Effects.BlurEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsDropShadow(ef As Effects.Effect) As Effects.DropShadowEffect
        Return DirectCast(ef, Effects.DropShadowEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsBloom(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.BloomEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.BloomEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsColorTone(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.ColorToneEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.ColorToneEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsRipple(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.RippleEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.RippleEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsSharpen(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.SharpenEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.SharpenEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsSwirl(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.SwirlEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.SwirlEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsPixelate(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.PixelateEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.PixelateEffect)
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function AsMonochrome(ef As Effects.Effect) As Microsoft.Expression.Media.Effects.MonochromeEffect
        Return DirectCast(ef, Microsoft.Expression.Media.Effects.MonochromeEffect)
    End Function
End Module