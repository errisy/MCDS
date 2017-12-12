Imports System.Windows, System.Windows.Media, System.Windows.Data, System.Windows.Input
Public Class WPFRestrictionEnzymeManager
    Private _CutSiteViewModel As CutSiteViewModel
    Private _RestrictionEnzymeDefinitions As RestrictionEnzymeDefinitions
    Private Sub WFPRestrictionEnzymeManager_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If IO.File.Exists(SettingEntry.RestrictionEnzymeFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.RestrictionEnzymeFilePath))
                If TypeOf obj Is RestrictionEnzymeDefinitions Then _RestrictionEnzymeDefinitions = obj
            Catch ex As Exception
                _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions
            End Try
        Else
            _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions
        End If
        If _RestrictionEnzymeDefinitions Is Nothing Then _RestrictionEnzymeDefinitions = New RestrictionEnzymeDefinitions
        lvMain.ItemsSource = _RestrictionEnzymeDefinitions
    End Sub
    Private Sub WFPRestrictionEnzymeManager_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        IO.File.WriteAllText(SettingEntry.RestrictionEnzymeFilePath, System.Xaml.XamlServices.Save(_RestrictionEnzymeDefinitions))
    End Sub
    Private Sub SetRestrictionSite(_RestrictionDefinition As RestrictionEnzymeDefinition)
        gdSiteDetail.DataContext = _RestrictionDefinition
        vpCutSite.ContainerVisual.Visuals.Clear()
        Dim model As New CutSitePanelVisual With {.SiteDef = _RestrictionDefinition}
        vpCutSite.ContainerVisual.Visuals.Add(model)
    End Sub
    Private Sub EnzymeChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs)
        Dim _RestrictionEnzymeDefinition As RestrictionEnzymeDefinition = lvMain.SelectedItem
        SetRestrictionSite(_RestrictionEnzymeDefinition)
    End Sub
    Public Property ParentTab As TabPage
    Public Event CloseTab As EventHandler

    Private Sub AddNewEnzyme(sender As Object, e As RoutedEventArgs)
        Dim _RestrictionEnzymeDefinition As New RestrictionEnzymeDefinition
        _RestrictionEnzymeDefinitions.Add(_RestrictionEnzymeDefinition)
        SetRestrictionSite(_RestrictionEnzymeDefinition)
    End Sub

    Private Sub SaveEnzymes(sender As Object, e As RoutedEventArgs)
        IO.File.WriteAllText(SettingEntry.RestrictionEnzymeFilePath, System.Xaml.XamlServices.Save(_RestrictionEnzymeDefinitions))
        SettingEntry.LoadRestrictionEnzymes()
    End Sub
End Class
