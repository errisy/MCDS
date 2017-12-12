Public Class AdvertisementBar
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        'gdAdvertisement.SetBinding(System.Windows.Controls.ItemsControl.ItemsSourceProperty, New System.Windows.Data.Binding With {.Source = _ItemsSource, .Converter = New SelectableEnumerableConverter})
        gdAdvertisement.ItemsSource = _ItemsSource
        SelectNothing()
        'If TypeOf LoginManagement.Customer Is SynContract.Customer Then tbEmail.Text = LoginManagement.Customer.EmailAddress
    End Sub
    Private _SelectedItems As IEnumerable(Of ChartItem)
    Private _ItemsSource As New System.Collections.ObjectModel.ObservableCollection(Of Object)

    Public Property ParentWorkControl As WorkControl

    <System.ComponentModel.Description("获取或者设置被选中的项目。")>
    Public Property SelectedItems As IEnumerable(Of ChartItem)
        Get
            Return _SelectedItems
        End Get
        Set(value As IEnumerable(Of ChartItem))
            If TypeOf value Is IEnumerable(Of ChartItem) AndAlso value.Any Then
                _SelectedItems = value
                _SelectedItem = _SelectedItems(0)
                GetAdverstiment(_SelectedItem.MolecularInfo.MolecularOperation)
            Else
                _SelectedItems = New List(Of ChartItem)
                _SelectedItem = Nothing
                GetAdverstiment(Nothing)
            End If
        End Set
    End Property
    Private _SelectedItem As ChartItem
    Public ReadOnly Property SelectedItem As ChartItem
        Get
            Return _SelectedItem
        End Get
    End Property
    Public Sub GetAdverstiment(nMode As Nuctions.MolecularOperationEnum?)
        If nMode Is Nothing Then SelectNothing() : Return
        Dim mode As SynContract.AdvertisementType = CInt(nMode)
        lbName.Content = [Enum].GetName(GetType(SynContract.AdvertisementType), mode)
        Select Case mode
            Case SynContract.AdvertisementType.FreeDesign
                SelectSynthesis()
            Case Else
                Dim sp = LoginManagement.Advertisements
                If TypeOf sp Is SynContract.Suppliers Then
                    _ItemsSource.Clear()
                    If sp.Suppliers.ContainsKey(mode) Then
                        For Each cp In sp.Suppliers(mode).Companies
                            _ItemsSource.Add(New SelectableDynamicObject(Of SynContract.Company)(cp) With {._IsDynamicSelected = False})
                        Next
                    End If
                    AppendProject()
                End If
        End Select
    End Sub
    Public Sub SelectNothing()
        lbName.Content = "Synthenome Services"
        _ItemsSource.Clear()
        _ItemsSource.Add(LoginManagement.ProjectPrice)
        _ItemsSource.Add(LoginManagement.SynthesisPrice)
        _ItemsSource.Add(LoginManagement.PrimerPrice)
    End Sub
    Private Sub AppendProject()
        _ItemsSource.Add(LoginManagement.ProjectPrice)
        _ItemsSource.Add(LoginManagement.SynthesisPrice)
        _ItemsSource.Add(LoginManagement.PrimerPrice)
    End Sub
    Public Sub SelectSynthesis()
        lbName.Content = "Synthenome Synthesis"
        _ItemsSource.Clear()
        _ItemsSource.Add(LoginManagement.SynthesisPrice)
    End Sub
 
    Private Sub EmailtoAll(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim companies = gdAdvertisement.Items.OfType(Of SelectableDynamicObject(Of SynContract.Company)).Where(Function(so As SelectableDynamicObject(Of SynContract.Company)) so._IsDynamicSelected)
        If Not TypeOf companies Is IEnumerable(Of SynContract.Company) Then Exit Sub
        Dim xamlflow As String = System.Windows.Markup.XamlWriter.Save(fdEmailContent)
        Dim res As String = HTMLConverter.HtmlFromXamlConverter.ConvertXamlToHtml(xamlflow)
        Dim cList As New List(Of SynContract.Company)
        For Each cc In companies
            cList.Add(CTypeDynamic(Of SynContract.Company)(cc))
        Next
        LoginManagement.SendEmailtoCompanies(cList, res)
    End Sub
    Friend Sub UpdateSelectedItems(sender As Object, e As EventArgs)
        Me.SelectedItems = CType(sender, OperationView).SelectedItems
    End Sub
    Private Sub Navigate(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim company = CTypeDynamic(Of SynContract.Company)(DirectCast(e.Source, System.Windows.FrameworkContentElement).DataContext)
        Process.Start(company.WebAddress)
    End Sub
    Private Sub MailTo(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim company = CTypeDynamic(Of SynContract.Company)(DirectCast(e.Source, System.Windows.FrameworkContentElement).DataContext)
        Process.Start("mailto:" + company.Email)
    End Sub
    Private Sub QuotaProject(sender As Object, e As EventArgs)
        ParentWorkControl.PresentSummary(SummarySectionEnum.Project)
    End Sub
    Private Sub QuotaSynthesis(sender As Object, e As EventArgs)
        ParentWorkControl.PresentSummary(SummarySectionEnum.Synthesis)
    End Sub
    Private Sub QuotaPrimer(sender As Object, e As EventArgs)
        ParentWorkControl.PresentSummary(SummarySectionEnum.Primer)
    End Sub
End Class
