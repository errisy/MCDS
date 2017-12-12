Imports System.Windows

Public Class WPFCodonAnalyzer
    Dim sList As New List(Of KEGGOrganismCodonModel)
    Dim sOrganisms As New ObjectModel.ObservableCollection(Of KEGGOrganismCodonModel)
    Dim sCodons As New System.Collections.ObjectModel.ObservableCollection(Of KEGGCodonPreferenceAnalyzer)
    Private Sub WPFCodonAnalyzer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim orgList = KEGGUtil.OrganismList
        For Each org In orgList
            sList.Add(New KEGGOrganismCodonModel(org))
        Next
        For Each org In sList
            sOrganisms.Add(org)
        Next
        dgOrganisms.ItemsSource = sOrganisms
        dgUsage.ItemsSource = sCodons
    End Sub
    Private Sub AnalyzedCallBack(code As String)
        If Dispatcher.CheckAccess Then
            For Each org In sOrganisms
                If org.Code = code Then org.Status = OrganismStatusEnum.Analyzed
            Next
        Else
            Dispatcher.Invoke(Sub() AnalyzedCallBack(code))
        End If
    End Sub
    Private Sub DownloadAnalyze(sender As Object, e As Windows.Input.MouseButtonEventArgs)
        Dim scr As System.Windows.FrameworkContentElement = e.Source

        Dim cc = scr.Ancestor(Of System.Windows.Controls.ContentControl)

        If TypeOf cc.DataContext Is KEGGOrganismCodonModel Then
            Dim cd As KEGGOrganismCodonModel = cc.DataContext
            If cd.Status = OrganismStatusEnum.NotAnalyzed Then
                Dim kan As New KEGGCodonPreferenceAnalyzer With {.Organsim = cd.Code, .CodonAnalyzedCallBack = AddressOf AnalyzedCallBack}
                sCodons.Add(kan)
                kan.StartAnalyze()
            End If
        End If
    End Sub
    Private Sub SetAsDefault(sender As Object, e As Windows.Input.MouseButtonEventArgs)
        Dim scr As System.Windows.FrameworkContentElement = e.Source
        Dim cc = scr.Ancestor(Of System.Windows.Controls.ContentControl)
        If TypeOf cc.DataContext Is KEGGOrganismCodonModel Then
            Dim cd As KEGGOrganismCodonModel = cc.DataContext
            Dim kt = KEGGUtil.GetCodonUsage(cd.Code)
            IO.File.WriteAllText(SettingEntry.TranslationTableFile, System.Xaml.XamlServices.Save(kt))
        End If
    End Sub

    Public Event CloseTab As EventHandler
    Public Property ParentTab As CustomTabPage

    Private Sub SearchOrganism(sender As Object, e As RoutedEventArgs)
        Dim cTaxonomy As New List(Of String)
        Dim cCode As New List(Of String)
        If tbTaxon.Text IsNot Nothing And tbTaxon.Text.Length > 0 Then cTaxonomy.AddRange(tbTaxon.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries))
        If tbCode.Text IsNot Nothing And tbCode.Text.Length > 0 Then cCode.AddRange(tbCode.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries))
        sOrganisms = New ObjectModel.ObservableCollection(Of KEGGOrganismCodonModel)
        For Each org In sList
            Dim valid As Boolean = True
            For Each s In cTaxonomy
                If Not org.Taxonomy.Contains(s) Then valid = False : Exit For
            Next
            If valid Then
                For Each s In cCode
                    If Not org.Code.Contains(s) Then valid = False : Exit For
                Next
            End If
            If valid Then sOrganisms.Add(org)
        Next
        dgOrganisms.ItemsSource = sOrganisms
    End Sub
    Private Sub ClearCondition(sender As Object, e As RoutedEventArgs)
        tbTaxon.Clear()
        tbCode.Clear()
    End Sub
End Class
