Public Class WPFErrorWindow
    Public Sub New()
        System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(Me)
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Public Sub ShowQueue(Queue As Queue(Of String))
        lbErrors.ItemsSource = Queue
    End Sub
End Class
