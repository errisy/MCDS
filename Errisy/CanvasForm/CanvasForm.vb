Public Class CanvasForm
    Inherits CanvasFormBase
    Public Sub New()
        'Template = DefaultTemplate
    End Sub
#Region "Control Template"
    Private WithEvents _RectNW As Rectangle
    Private WithEvents _RectN As Rectangle
    Private WithEvents _RectNE As Rectangle
    Private WithEvents _RectW As Rectangle
    Private WithEvents _RectE As Rectangle
    Private WithEvents _RectSW As Rectangle
    Private WithEvents _RectS As Rectangle
    Private WithEvents _RectSE As Rectangle
    Private WithEvents _HeaderBorder As Border
    Private WithEvents _CloseButton As CloseButton

    Private MouseResizeMode As ResizeModeEnum
    Private MouseCapturer As FrameworkElement
    Private BeginPosition As Point
    Private BeginSize As Point
    Private BeginMouse As Point
    Private IsResizing As Boolean = False
    Private Property Position As Point
        Get
            Return New Point(CanvasEditor.GetLeft(Me), CanvasEditor.GetTop(Me))
        End Get
        Set(value As Point)
            CanvasEditor.SetLeft(Me, value.X)
            CanvasEditor.SetTop(Me, value.Y)
        End Set
    End Property
    Private Property Size As Point
        Get
            Return New Point(Width, Height)
        End Get
        Set(value As Point)
            Width = IIf(value.X >= MinWidth, value.X, MinWidth)
            Height = IIf(value.Y >= MinHeight, value.Y, MinHeight)
        End Set
    End Property
    Public Overrides Sub OnApplyTemplate()
        If System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then Return
        _RectNW = Template.FindName("_RectNW", Me)
        _RectN = Template.FindName("_RectN", Me)
        _RectNE = Template.FindName("_RectNE", Me)
        _RectW = Template.FindName("_RectW", Me)
        _RectE = Template.FindName("_RectE", Me)
        _RectSW = Template.FindName("_RectSW", Me)
        _RectS = Template.FindName("_RectS", Me)
        _RectSE = Template.FindName("_RectSE", Me)
        _HeaderBorder = Template.FindName("_HeaderBorder", Me)
        _CloseButton = Template.FindName("_CloseButton", Me)
        MyBase.OnApplyTemplate()
    End Sub
    Private Sub ApplyOffset(oPosition As Point, oSize As Point, Offset As Vector, mode As ResizeModeEnum)
        Dim zoom = Ancestor(Of IZoom)()
        If zoom IsNot Nothing Then Offset = zoom.CalibrateMouseOffset(Offset)
        Select Case mode
            Case ResizeModeEnum.NW
                Position = New Point(oPosition.X + Offset.X, oPosition.Y + Offset.Y)
                Size = New Point(oSize.X - Offset.X, oSize.Y - Offset.Y)
            Case ResizeModeEnum.N
                Position = New Point(oPosition.X, oPosition.Y + Offset.Y)
                Size = New Point(oSize.X, oSize.Y - Offset.Y)
            Case ResizeModeEnum.NE
                Position = New Point(oPosition.X, oPosition.Y + Offset.Y)
                Size = New Point(oSize.X + Offset.X, oSize.Y - Offset.Y)
            Case ResizeModeEnum.W
                Position = New Point(oPosition.X + Offset.X, oPosition.Y)
                Size = New Point(oSize.X - Offset.X, oSize.Y)
            Case ResizeModeEnum.E
                Size = New Point(oSize.X + Offset.X, oSize.Y)
            Case ResizeModeEnum.SW
                Position = New Point(oPosition.X + Offset.X, oPosition.Y)
                Size = New Point(oSize.X - Offset.X, oSize.Y + Offset.Y)
            Case ResizeModeEnum.S
                Size = New Point(oSize.X, oSize.Y + Offset.Y)
            Case ResizeModeEnum.SE
                Size = New Point(oSize.X + Offset.X, oSize.Y + Offset.Y)
            Case ResizeModeEnum.Move
                Position = New Point(oPosition.X + Offset.X, oPosition.Y + Offset.Y)
        End Select
    End Sub
    Private Sub _RectNW_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectNW.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.NW)
    End Sub
    Private Sub _RectN_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectN.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.N)
    End Sub
    Private Sub _RectNE_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectNE.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.NE)
    End Sub
    Private Sub _RectW_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectW.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.W)
    End Sub
    Private Sub _RectE_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectE.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.E)
    End Sub
    Private Sub _RectSW_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectSW.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.SW)
    End Sub
    Private Sub _RectS_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectS.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.S)
    End Sub
    Private Sub _RectSE_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _RectSE.MouseLeftButtonDown
        BeginResize(e, ResizeModeEnum.SE)
    End Sub
    Private Sub _HeaderBorder_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles _HeaderBorder.MouseLeftButtonDown
        TrySelect()
        BeginResize(e, ResizeModeEnum.Move)
    End Sub
    Private Sub BeginResize(e As MouseButtonEventArgs, mode As ResizeModeEnum)
        MouseResizeMode = mode
        MouseCapturer = e.Source
        BeginSize = Size
        BeginPosition = Position
        BeginMouse = PointToScreen(e.GetPosition(Me))
        IsResizing = True
        MouseCapturer.CaptureMouse()
    End Sub
    Private Sub Resize(sender As Object, e As MouseEventArgs) Handles _RectNW.MouseMove, _RectN.MouseMove, _RectNE.MouseMove,
        _RectW.MouseMove, _RectE.MouseMove, _RectSW.MouseMove, _RectS.MouseMove, _RectSE.MouseMove, _HeaderBorder.MouseMove
        If IsResizing Then
            If e.LeftButton = MouseButtonState.Pressed Then
                Dim CurrentMouse = PointToScreen(e.GetPosition(Me))
                Using x = Dispatcher.DisableProcessing
                    ApplyOffset(BeginPosition, BeginSize, CurrentMouse - BeginMouse, MouseResizeMode)
                End Using
            Else
                MouseResizeMode = ResizeModeEnum.None
                MouseCapturer.ReleaseMouseCapture()
                IsResizing = False
            End If
        End If
    End Sub
    Private Sub EndResize(sender As Object, e As MouseButtonEventArgs) Handles _RectNW.MouseLeftButtonUp, _RectN.MouseLeftButtonUp, _RectNE.MouseLeftButtonUp,
        _RectW.MouseLeftButtonUp, _RectE.MouseLeftButtonUp, _RectSW.MouseLeftButtonUp, _RectS.MouseLeftButtonUp, _RectSE.MouseLeftButtonUp, _HeaderBorder.MouseLeftButtonUp
        If IsResizing Then
            Dim CurrentMouse = PointToScreen(e.GetPosition(Me))
            Using x = Dispatcher.DisableProcessing
                ApplyOffset(BeginPosition, BeginSize, CurrentMouse - BeginMouse, MouseResizeMode)
            End Using
            MouseResizeMode = ResizeModeEnum.None
            MouseCapturer.ReleaseMouseCapture()
            IsResizing = False
        End If
    End Sub

    Private Sub _CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles _CloseButton.Click
        RaiseFormClose()
    End Sub
#End Region
    Private Shared DefaultTemplate As ControlTemplate = Markup.XamlReader.Parse(
         <ControlTemplate
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:e="clr-namespace:Errisy;assembly=Errisy"
             TargetType="{x:Type e:CanvasForm}">
             <Border>
                 <Grid>
                     <Grid.RowDefinitions>
                         <RowDefinition Height="{TemplateBinding TopBorderSize}"/>
                         <RowDefinition Height="{TemplateBinding HeaderSize}"/>
                         <RowDefinition Height="*"/>
                         <RowDefinition Height="{TemplateBinding BottomBorderSize}"/>
                     </Grid.RowDefinitions>
                     <Grid.ColumnDefinitions>
                         <ColumnDefinition Width="{TemplateBinding LeftBorderSize}"/>
                         <ColumnDefinition Width="*"/>
                         <ColumnDefinition Width="{TemplateBinding RightBorderSize}"/>
                     </Grid.ColumnDefinitions>
                     <Rectangle Name="_RectNW" Grid.Row="0" Grid.Column="0" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNWSE"/>
                     <Rectangle Name="_RectN" Grid.Row="0" Grid.Column="1" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNS"/>
                     <Rectangle Name="_RectNE" Grid.Row="0" Grid.Column="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNESW"/>
                     <Rectangle Name="_RectW" Grid.Row="1" Grid.Column="0" Grid.RowSpan="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeWE"/>
                     <Rectangle Name="_RectE" Grid.Row="1" Grid.Column="2" Grid.RowSpan="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeWE"/>
                     <Rectangle Name="_RectSW" Grid.Row="3" Grid.Column="0" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNESW"/>
                     <Rectangle Name="_RectS" Grid.Row="3" Grid.Column="1" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNS"/>
                     <Rectangle Name="_RectSE" Grid.Row="3" Grid.Column="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNWSE"/>
                     <Border Name="_HeaderBorder" Grid.Row="1" Grid.Column="1" Background="{TemplateBinding HeaderBackground}">
                         <Grid Name="_Grid">
                             <Grid.ColumnDefinitions>
                                 <ColumnDefinition Width="*"/>
                                 <ColumnDefinition Width="Auto"/>
                             </Grid.ColumnDefinitions>
                             <ContentPresenter Grid.Column="0" ContentSource="Header" DataContext="{TemplateBinding DataContext}"/>
                             <e:CloseButton Name="_CloseButton" Grid.Column="1" Width="{Binding ElementName=_Grid, Path=ActualHeight}"/>
                         </Grid>
                     </Border>
                     <ScrollViewer Grid.Row="2" Grid.Column="1" Background="{TemplateBinding Background}" HorizontalContentAlignment="Stretch" VerticalContentAlignment="Top">
                         <ContentPresenter ContentSource="Content" HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}" VerticalAlignment="{TemplateBinding VerticalContentAlignment}"/>
                     </ScrollViewer>
                 </Grid>
             </Border>
             <ControlTemplate.Triggers>
                 <Trigger Property="HasCloseButton" Value="False">
                     <Setter TargetName="_CloseButton" Property="Visibility" Value="Collapsed"/>
                 </Trigger>
             </ControlTemplate.Triggers>
         </ControlTemplate>.ToString)
    Shared Sub New()
        Dim DefaultStyle As Style = Markup.XamlReader.Parse(
            <Style
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:e="clr-namespace:Errisy;assembly=Errisy"
                TargetType="{x:Type e:CanvasForm}">
                <Style.Setters>
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate TargetType="{x:Type e:CanvasForm}">
                                <Border>
                                    <Grid>
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height="{TemplateBinding TopBorderSize}"/>
                                            <RowDefinition Height="{TemplateBinding HeaderSize}"/>
                                            <RowDefinition Height="*"/>
                                            <RowDefinition Height="{TemplateBinding BottomBorderSize}"/>
                                        </Grid.RowDefinitions>
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width="{TemplateBinding LeftBorderSize}"/>
                                            <ColumnDefinition Width="*"/>
                                            <ColumnDefinition Width="{TemplateBinding RightBorderSize}"/>
                                        </Grid.ColumnDefinitions>
                                        <Rectangle Name="_RectNW" Grid.Row="0" Grid.Column="0" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNWSE"/>
                                        <Rectangle Name="_RectN" Grid.Row="0" Grid.Column="1" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNS"/>
                                        <Rectangle Name="_RectNE" Grid.Row="0" Grid.Column="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNESW"/>
                                        <Rectangle Name="_RectW" Grid.Row="1" Grid.Column="0" Grid.RowSpan="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeWE"/>
                                        <Rectangle Name="_RectE" Grid.Row="1" Grid.Column="2" Grid.RowSpan="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeWE"/>
                                        <Rectangle Name="_RectSW" Grid.Row="3" Grid.Column="0" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNESW"/>
                                        <Rectangle Name="_RectS" Grid.Row="3" Grid.Column="1" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNS"/>
                                        <Rectangle Name="_RectSE" Grid.Row="3" Grid.Column="2" Fill="{TemplateBinding BorderBrush}" Cursor="SizeNWSE"/>
                                        <Border Name="_HeaderBorder" Grid.Row="1" Grid.Column="1" Background="{TemplateBinding HeaderBackground}">
                                            <Grid Name="_Grid">
                                                <Grid.ColumnDefinitions>
                                                    <ColumnDefinition Width="*"/>
                                                    <ColumnDefinition Width="Auto"/>
                                                </Grid.ColumnDefinitions>
                                                <ContentPresenter Grid.Column="0" ContentSource="Header" DataContext="{TemplateBinding DataContext}"/>
                                                <e:CloseButton Name="_CloseButton" Grid.Column="1" Width="{Binding ElementName=_Grid, Path=ActualHeight}"/>
                                            </Grid>
                                        </Border>
                                        <ScrollViewer Grid.Row="2" Grid.Column="1" Background="{TemplateBinding Background}" HorizontalContentAlignment="Stretch" VerticalContentAlignment="Top">
                                            <ContentPresenter ContentSource="Content" HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}" VerticalAlignment="{TemplateBinding VerticalContentAlignment}"/>
                                        </ScrollViewer>
                                    </Grid>
                                </Border>
                                <ControlTemplate.Triggers>
                                    <Trigger Property="HasCloseButton" Value="False">
                                        <Setter TargetName="_CloseButton" Property="Visibility" Value="Collapsed"/>
                                    </Trigger>
                                </ControlTemplate.Triggers>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style.Setters>
            </Style>.ToString)
        CanvasFormBase.StyleProperty.OverrideMetadata(GetType(CanvasForm), New FrameworkPropertyMetadata(DefaultStyle))
    End Sub

End Class
