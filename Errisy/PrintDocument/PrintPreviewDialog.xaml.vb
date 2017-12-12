<System.ComponentModel.Description("Please add pages before you show this dialog.")>
Public Class PrintPreviewDialog

 

    'PrintPreviewDialog->PageMargin As Thickness Default: Nothing
    Public Property PageMargin As Thickness
        Get
            Return GetValue(PageMarginProperty)
        End Get
        Set(ByVal value As Thickness)
            SetValue(PageMarginProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PageMarginProperty As DependencyProperty = _
                           DependencyProperty.Register("PageMargin", _
                           GetType(Thickness), GetType(PrintPreviewDialog), _
                           New PropertyMetadata(New Thickness(12.0#, 12.0#, 12.0#, 12.0#)))

 
    'PrintPreviewDialog->PageWidth As Double Default: 8.3# * 300.0#
    Public Property PageWidth As Double
        Get
            Return GetValue(PageWidthProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(PageWidthProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PageWidthProperty As DependencyProperty = _
                           DependencyProperty.Register("PageWidth", _
                           GetType(Double), GetType(PrintPreviewDialog), _
                           New PropertyMetadata(793.70078740157476#))

    'PrintPreviewDialog->PageHeight As Double Default: 11.7# * 300.0#
    Public Property PageHeight As Double
        Get
            Return GetValue(PageHeightProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(PageHeightProperty, value)
        End Set
    End Property
    Public Shared ReadOnly PageHeightProperty As DependencyProperty = _
                           DependencyProperty.Register("PageHeight", _
                           GetType(Double), GetType(PrintPreviewDialog), _
                           New PropertyMetadata(1122.5196850393702#))

    Private _Pages As New List(Of VisualPage)
    Public Sub AddSimpleVisualPage(PageVisual As Visual)
        Dim vra As New VisualRowAlignment
        vra.Elements.Add(PageVisual)
        vra.RowType = RowTypeEnum.FullPage
        VisualRows.Add(vra)
        'Dim uControl As New UserControl With {.Background = Brushes.White}
        'uControl.SetBinding(UserControl.MarginProperty, New Binding(PageMarginProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
        'uControl.SetBinding(UserControl.WidthProperty, New Binding(PageWidthProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
        'uControl.SetBinding(UserControl.HeightProperty, New Binding(PageHeightProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
        'uControl.Content = PageVisual
        '_Pages.Add(PageVisual)
        'spPages.Children.Add(uControl)
    End Sub

    Private VisualRows As New List(Of VisualRowAlignment)
    <System.ComponentModel.Description("If you want the Print Preview to automatically align your visuals into multiple pages. Please use this one instead.")>
    Public Sub AddRow(Row As VisualRowAlignment)
        VisualRows.Add(Row)
    End Sub

    Private Sub BuildPages()
        If spPages Is Nothing Then Return
        spPages.Children.Clear()
        For Each v1 In _Pages
            If TypeOf v1.Content Is PageStackPanel Then
                Dim v2 As PageStackPanel = v1.Content
                For Each v3 In v2.Children
                    If TypeOf v3 Is PageStackPanel Then
                        Dim v4 As PageStackPanel = v3
                        Dim dList As New List(Of FrameworkElement)
                        For Each v5 In v4.Children
                            dList.Add(v5)
                        Next
                        For Each v5 In dList
                            v4.Children.Remove(v5)
                        Next
                    ElseIf TypeOf v3 Is PageGrid Then
                        Dim v4 As PageGrid = v3
                        Dim dList As New List(Of FrameworkElement)
                        For Each v5 In v4.Children
                            dList.Add(v5)
                        Next
                        For Each v5 In dList
                            v4.Children.Remove(v5)
                        Next
                    End If
                Next
            ElseIf TypeOf v1.Content Is PageGrid Then
                Dim v4 As PageGrid = v1.Content
                Dim dList As New List(Of FrameworkElement)
                For Each v5 In v4.Children
                    dList.Add(v5)
                Next
                For Each v5 In dList
                    v4.Children.Remove(v5)
                Next
            End If
        Next
        _Pages.Clear()

        Dim cPage As VisualPage = Nothing
        Dim cStack As StackPanel = Nothing
        'Dim cScroll As ScrollViewer = Nothing
        Dim availableHeight As Double = PageHeight - PageMargin.Top - PageMargin.Bottom
        Dim availableWidth As Double = PageWidth - PageMargin.Left - PageMargin.Right

        Dim DesiredHeight As Double = 0.0#
        Dim LevelDesiredHeight As Double = 0.0#

        Dim CreatePage = Sub()
                             cPage = New VisualPage
                             cPage.SetBinding(VisualPage.PageMarginProperty, New Binding(PageMarginProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
                             cPage.SetBinding(VisualPage.WidthProperty, New Binding(PageWidthProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
                             cPage.SetBinding(VisualPage.HeightProperty, New Binding(PageHeightProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
                             Dim wrapPage As New UserControl
                             wrapPage.SetBinding(UserControl.WidthProperty, New Binding(PageWidthProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
                             wrapPage.SetBinding(UserControl.HeightProperty, New Binding(PageHeightProperty.Name) With {.Source = Me, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.OneWay})
                             wrapPage.Content = cPage
                             _Pages.Add(cPage)
                             spPages.Children.Add(wrapPage)
                             cStack = New PageStackPanel
                             'cScroll = New ScrollViewer With {.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled, .HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled}
                             cPage.Content = cStack
                             DesiredHeight = 0.0#
                             LevelDesiredHeight = 0.0#
                             UpdateLayout()
                         End Sub
        CreatePage()



        For Each vRow In VisualRows
            Select Case vRow.RowType
                Case RowTypeEnum.Auto
                    If DesiredHeight >= availableHeight Then CreatePage()
                    Dim cGrid As New PageGrid
                    cStack.Children.Add(cGrid)
                    UpdateLayout()
                    LevelDesiredHeight = 0.0#
                    For Each e In vRow.Elements
                        cGrid.Children.Add(e)
                    Next
                    LevelDesiredHeight = cGrid.Children.OfType(Of FrameworkElement).Max(Function(f) IIf(Double.IsInfinity(f.DesiredSize.Height), 0.0#, f.DesiredSize.Height))
                    UpdateLayout()
                    If cStack.Children.Count > 1 AndAlso DesiredHeight + LevelDesiredHeight > availableHeight Then
                        cStack.Children.Remove(cGrid)
                        UpdateLayout()
                        CreatePage()
                        cStack.Children.Add(cGrid)
                        UpdateLayout()
                    Else
                        DesiredHeight += LevelDesiredHeight
                        LevelDesiredHeight = 0.0#
                    End If
                Case RowTypeEnum.Fixed
                    If DesiredHeight >= availableHeight Then CreatePage()
                    If cStack.Children.Count > 0 AndAlso DesiredHeight + vRow.Height > availableHeight Then CreatePage()
                    Dim cGrid As New PageGrid With {.Height = vRow.Height}
                    cStack.Children.Add(cGrid)
                    UpdateLayout()
                    For Each e In vRow.Elements
                        cGrid.Children.Add(e)
                    Next
                    UpdateLayout()
                Case RowTypeEnum.Flow
                    Dim flowIndex As Integer = 0
                    While flowIndex < vRow.Elements.Count
                        Dim sRow As New PageStackPanel With {.Orientation = Orientation.Horizontal}
                        cStack.Children.Add(sRow)
                        UpdateLayout()
                        While sRow.DesiredSize.Width < availableWidth AndAlso flowIndex < vRow.Elements.Count
                            sRow.Children.Add(vRow.Elements(flowIndex))
                            flowIndex += 1
                            UpdateLayout()
                        End While
                        If sRow.DesiredSize.Width > availableWidth Then
                            flowIndex -= 1
                            sRow.Children.Remove(vRow.Elements(flowIndex))
                            UpdateLayout()
                        End If
                        LevelDesiredHeight = sRow.Children.OfType(Of FrameworkElement).Max(Function(f) IIf(Double.IsInfinity(f.DesiredSize.Height), 0.0#, f.DesiredSize.Height))
                        If cStack.Children.Count > 1 AndAlso DesiredHeight + LevelDesiredHeight > availableHeight Then
                            cStack.Children.Remove(sRow)
                            UpdateLayout()
                            CreatePage()
                            cStack.Children.Add(sRow)
                            DesiredHeight = sRow.Children.OfType(Of FrameworkElement).Max(Function(f) IIf(Double.IsInfinity(f.DesiredSize.Height), 0.0#, f.DesiredSize.Height))
                        Else
                            DesiredHeight += LevelDesiredHeight
                        End If
                    End While
                Case RowTypeEnum.FullPage
                    Dim cGrid As New PageGrid
                    If cStack.Children.Count > 0 Then
                        CreatePage()
                        cPage.Content = cGrid
                        For Each e In vRow.Elements
                            cGrid.Children.Add(e)
                        Next
                        UpdateLayout()
                        CreatePage()
                    Else
                        cPage.Content = cGrid
                        For Each e In vRow.Elements
                            cGrid.Children.Add(e)
                        Next
                        CreatePage()
                    End If
            End Select
        Next
        UpdateLayout()
    End Sub

    Private Sub Print(sender As Object, e As RoutedEventArgs)
        Dim pd As New PrintDialog()
        If pd.ShowDialog() Then
            Dim pnt As New VisualPaginator
            For Each p In _Pages
                pnt.AddPageVisual(p)
            Next
            pd.PrintDocument(pnt, Title)
            Me.DialogResult = True
        End If
    End Sub

    Private Sub Cancel(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub PrintPreviewDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        spbSizes.ItemsSource = PageSettings.PageSizes
        spbMargins.ItemsSource = PageSettings.PageMargins
        BuildPages()
    End Sub


    Private Sub SizeSelected(sender As Object, e As RoutedEventArgs)
        If TypeOf e.OriginalSource Is System.Windows.Controls.Ribbon.RibbonMenuItem Then
            Dim item As System.Windows.Controls.Ribbon.RibbonMenuItem = e.OriginalSource
            Dim ps As PageSize = item.DataContext
            PageWidth = ps.RealWidth
            PageHeight = ps.RealHeight
            spbSizes.Label = ps.Description
            BuildPages()
        End If
    End Sub

    Private Sub MarginSelected(sender As Object, e As RoutedEventArgs)
        If TypeOf e.OriginalSource Is System.Windows.Controls.Ribbon.RibbonMenuItem Then
            Dim item As System.Windows.Controls.Ribbon.RibbonMenuItem = e.OriginalSource
            Dim pm As PageMargin = item.DataContext
            nbLeft.Value = pm.Left
            nbTop.Value = pm.Top
            nbRight.Value = pm.Right
            nbBottom.Value = pm.Bottom
            spbMargins.Label = pm.Description
            BuildPages()
        End If
    End Sub

    Private Sub UpdateMargin(sender As Object, e As TextChangedEventArgs)
        If nbLeft Is Nothing Then Return
        If nbTop Is Nothing Then Return
        If nbRight Is Nothing Then Return
        If nbBottom Is Nothing Then Return
        PageMargin = New Thickness(nbLeft.Value * PageSettings.PointsPerMillimeter, nbTop.Value * PageSettings.PointsPerMillimeter, nbRight.Value * PageSettings.PointsPerMillimeter, nbBottom.Value * PageSettings.PointsPerMillimeter)
        BuildPages()
    End Sub
End Class
