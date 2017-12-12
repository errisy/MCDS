Public Class ConfirmSynthesis
    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        synVectorColumn.ItemsSource = New String() {"pUC", "p15a", "Custom"}
        DataContext = New SynContract.GeneSynthesisOrder
    End Sub
    Private _Order As New SynContract.GeneSynthesisOrder
    Public Property Order As SynContract.GeneSynthesisOrder
        Get
            Return DataContext
        End Get
        Set(value As SynContract.GeneSynthesisOrder)
            DataContext = value
        End Set
    End Property

    Private Sub Confirm(sender As System.Object, e As System.Windows.RoutedEventArgs)

        WPFContainer.GetWinForm(Me).DialogResult = DialogResult.OK
    End Sub
End Class
