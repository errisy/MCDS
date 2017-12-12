Imports System.Windows

Public Class WPFgRNAManager
    Private _CutSiteViewModel As CutSiteViewModel
    Private _gRNADefinitions As gRNADefinitions
    Private Sub WPFgRNAManager_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If IO.File.Exists(SettingEntry.gRNAFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.gRNAFilePath))
                If TypeOf obj Is gRNADefinitions Then _gRNADefinitions = obj
            Catch ex As Exception
                _gRNADefinitions = New gRNADefinitions
            End Try
        Else
            _gRNADefinitions = New gRNADefinitions
        End If
        If _gRNADefinitions Is Nothing Then _gRNADefinitions = New gRNADefinitions
        lvMain.ItemsSource = _gRNADefinitions
    End Sub

    Public Event CloseTab As EventHandler
    Public Property ParentTab As CustomTabPage

    Private Sub SetgRNASite(_RestrictionDefinition As gRNADefinition)
        gdSiteDetail.DataContext = _RestrictionDefinition
        vpCutSite.ContainerVisual.Visuals.Clear()
        Dim model As New CutSitePanelVisual With {.SiteDef = _RestrictionDefinition}
        vpCutSite.ContainerVisual.Visuals.Add(model)
    End Sub
    Private Sub AddNewgRNA(sender As Object, e As Windows.RoutedEventArgs)
        Dim _gRNADefinition As New gRNADefinition
        _gRNADefinitions.Add(_gRNADefinition)
        SetgRNASite(_gRNADefinition)
    End Sub

    Private Sub SavegRNAs(sender As Object, e As Windows.RoutedEventArgs)
        IO.File.WriteAllText(SettingEntry.gRNAFilePath, System.Xaml.XamlServices.Save(_gRNADefinitions))
        SettingEntry.LoadgRNAs()
    End Sub

    Private Sub gRNAChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs)
        Dim _gRNADefinition As gRNADefinition = lvMain.SelectedItem
        SetgRNASite(_gRNADefinition)
    End Sub
End Class
