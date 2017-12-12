Imports System.ComponentModel
Friend Class CancelRunViewModel
    Implements INotifyPropertyChanged
    Private _TokenSource As New System.Threading.CancellationTokenSource
    Private _StopWatch As New Stopwatch
    Private _Timer As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 0, 500)}
    Public Sub New()
        _StopWatch.Start()
        AddHandler _Timer.Tick, AddressOf TimerCallback
    End Sub
    Public ReadOnly Property TokenSource As System.Threading.CancellationTokenSource
        Get
            Return _TokenSource
        End Get
    End Property
    Public ReadOnly Property Token As System.Threading.CancellationToken
        Get
            Return _TokenSource.Token
        End Get
    End Property

    Private Sub TimerCallback(sender As Object, e As EventArgs)
        Time = (_StopWatch.ElapsedMilliseconds / 1000).ToString("0.000")
    End Sub
    Private _Operation As String
    Public Property Operation As String
        Get
            Return _Operation
        End Get
        Set(value As String)
            _Operation = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Operation"))
        End Set
    End Property
    Private _Time As String
    Public Property Time As String
        Get
            Return _Time
        End Get
        Set(value As String)
            _Time = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Time"))
        End Set
    End Property
    Public Property Window As frmCancelRun
    Public ReadOnly Property Cancel As New ViewModelCommand(AddressOf cmdCancel)
    Private Sub cmdCancel(value As Object)
        _TokenSource.Cancel()
        Window.DialogResult = DialogResult.Cancel
    End Sub
    Public Sub Close()
        While _Window Is Nothing
            System.Threading.Thread.Sleep(5)
        End While
        SettingEntry.MainUIWindow.Invoke(Sub()
                                             Window.DialogResult = DialogResult.OK
                                         End Sub)
    End Sub
    Shared Sub ShowCancelRunWindow(_Model As CancelRunViewModel)
        Dim _Cancel As New frmCancelRun With {.Owner = SettingEntry.MainUIWindow}
        _Model.Window = _Cancel
        DirectCast(_Cancel.ihCancel.Child, WPFCancelRun).DataContext = _Model
        _Cancel.ShowDialog()
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
