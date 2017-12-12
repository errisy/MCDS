Imports System.Windows, System.Windows.Media, System.Windows.Data, System.Windows.Input
Public Class WPFRecombinationSetManager

    Private _CutSiteViewModel As CutSiteViewModel
    Private _RecombinationSetDefinitions As RecombinationSetDefinitions

    Private Sub WFPRecombinationSetManager_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim RecombinationEndTypes As New ObjectModel.ObservableCollection(Of String)
        RecombinationEndTypes.Add("F")
        RecombinationEndTypes.Add("B")
        RecombinationEndTypes.Add("P")
        RecombinationEndTypes.Add("L")
        RecombinationEndTypes.Add("R")
        cbRecombinationType.ItemsSource = RecombinationEndTypes
        If IO.File.Exists(SettingEntry.RecombinationSiteFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.RecombinationSiteFilePath))
                If TypeOf obj Is RecombinationSetDefinitions Then _RecombinationSetDefinitions = obj
            Catch ex As Exception
                _RecombinationSetDefinitions = New RecombinationSetDefinitions
            End Try
        Else
            _RecombinationSetDefinitions = New RecombinationSetDefinitions
        End If
        If _RecombinationSetDefinitions Is Nothing Then _RecombinationSetDefinitions = New RecombinationSetDefinitions
        lvMain.ItemsSource = _RecombinationSetDefinitions
    End Sub
    Private Sub WFPRecombinationSetManager_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        IO.File.WriteAllText(SettingEntry.RecombinationSiteFilePath, System.Xaml.XamlServices.Save(_RecombinationSetDefinitions))
    End Sub
    Private Sub SetRecombinationSite(_RecombinationDefinition As RecombinationDefinition)
        gdSiteDetail.DataContext = _RecombinationDefinition
        vpCutSite.ContainerVisual.Visuals.Clear()
        Dim model As New CutSitePanelVisual With {.SiteDef = _RecombinationDefinition}
        vpCutSite.ContainerVisual.Visuals.Add(model)
    End Sub
    Private Sub SetChanged(sender As Object, e As Controls.SelectionChangedEventArgs)
        Dim _RecombinationSetDefinition As RecombinationSetDefinition = lvMain.SelectedItem
        gdRecombinationSetDetail.DataContext = _RecombinationSetDefinition
        SetRecombinationSite(Nothing)
    End Sub
    Private Sub SiteChanged(sender As Object, e As Controls.SelectionChangedEventArgs)
        Dim _RecombinationDefinition As RecombinationDefinition = lvSet.SelectedItem
        SetRecombinationSite(_RecombinationDefinition)
    End Sub
    Public Property ParentTab As TabPage
    Public Event CloseTab As EventHandler
    Private Sub AddNewSet(sender As Object, e As RoutedEventArgs)
        Dim _RecombinationSetDefinition As New RecombinationSetDefinition
        _RecombinationSetDefinitions.Add(_RecombinationSetDefinition)
        gdRecombinationSetDetail.DataContext = _RecombinationSetDefinition
        SetRecombinationSite(Nothing)
    End Sub
    Private Sub SaveSets(sender As Object, e As RoutedEventArgs)
        IO.File.WriteAllText(SettingEntry.RecombinationSiteFilePath, System.Xaml.XamlServices.Save(_RecombinationSetDefinitions))
        SettingEntry.LoadRecombinationSites()
    End Sub
    Private Sub AddNewSite(sender As Object, e As RoutedEventArgs)
        If Not (TypeOf gdRecombinationSetDetail.DataContext Is RecombinationSetDefinition) Then Return
        Dim _RecombinationSetDefinition As RecombinationSetDefinition = gdRecombinationSetDetail.DataContext
        Dim _RecombinationDefinition As New RecombinationDefinition
        _RecombinationSetDefinition.Add(_RecombinationDefinition)
        SetRecombinationSite(_RecombinationDefinition)
    End Sub
End Class
