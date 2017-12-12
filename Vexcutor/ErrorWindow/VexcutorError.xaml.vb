Public Class VexcutorError
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        DataContext = _ErrorInfo
    End Sub
    Private _ErrorInfo As New VexError
    Public Property ErrorInfo As VexError
        Get
            Return _ErrorInfo
        End Get
        Set(value As VexError)
            _ErrorInfo = value
            DataContext = value
        End Set
    End Property
    Private Sub OK(sender As System.Object, e As System.Windows.RoutedEventArgs)
        WPFContainer.GetWinForm(Me).DialogResult = DialogResult.OK
    End Sub
End Class
