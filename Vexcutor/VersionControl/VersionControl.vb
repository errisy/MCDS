Imports System.ComponentModel

Friend Class VersionControl

    Friend Const Major As Integer = 1
    Friend Const Minor As Integer = 22
    Friend Shared Async Sub Check(_Form As frmMain)

        Dim CheckTask As New System.Threading.Tasks.Task(Of VersionInformation)(Function()
                                                                                    Dim vi As New VersionInformation
                                                                                    Try
                                                                                        Dim wc As New System.Net.WebClient
                                                                                        Dim html = wc.DownloadString("https://github.com/errisy/MCDS/releases/tag/binary")
                                                                                        Dim reg As New System.Text.RegularExpressions.Regex("MCDS Version (\d+)\.(\d\d\d)")
                                                                                        If reg.IsMatch(html) Then
                                                                                            Dim m = reg.Match(html)

                                                                                            Dim Link As String = "https://github.com/errisy/MCDS/releases/download/binary/MCDS.zip"

                                                                                            Dim vMajor As Integer
                                                                                            Dim vMinor As Integer
                                                                                            If Integer.TryParse(m.Groups(1).Value, vMajor) AndAlso Integer.TryParse(m.Groups(2).Value, vMinor) Then
                                                                                                vi.Version = m.Groups(0).Value
                                                                                                vi.Link = Link
                                                                                                vi.IsNew = vMajor > Major OrElse vMinor > Minor
                                                                                            End If
                                                                                        End If
                                                                                    Catch ex As Exception

                                                                                    End Try
                                                                                    Return vi
                                                                                End Function)
        CheckTask.Start()
        Dim version = Await CheckTask
        If version.Link = "N/A" Then
            version.Message = "Internet Error: Please check if you can access github.com"
        End If
        If version.IsNew Then
            version.Message = String.Format("{1} is avaialble.{0}The Current version is {2}.{3}.{0}Click ""OK"" to shutdown MCDS and update now.", vbCrLf, version.Version, Major, Minor.ToString.PadLeft(3, "0"c))
            Dim _Window As New UpdateWindow
            _Window.wpfUpdateControl.DataContext = version
            version.Window = _Window
            _Window.ShowDialog(_Form)
        End If
    End Sub

End Class

<Serializable>
Friend Class VersionInformation
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _Window As UpdateWindow
    Friend Property Window As UpdateWindow
        Get
            Return _Window
        End Get
        Set(value As UpdateWindow)
            _Window = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Window"))
        End Set
    End Property
    Private _IsNew As Boolean
    Friend Property IsNew As Boolean
        Get
            Return _IsNew
        End Get
        Set(value As Boolean)
            _IsNew = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("IsNew"))
        End Set
    End Property
    Private _Version As String = "N/A"
    Friend Property Version As String
        Get
            Return _Version
        End Get
        Set(value As String)
            _Version = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Version"))
        End Set
    End Property
    Private _Link As String = "N/A"
    Friend Property Link As String
        Get
            Return _Link
        End Get
        Set(value As String)
            _Link = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Link"))
        End Set
    End Property
    Private _Message As String
    Friend Property Message As String
        Get
            Return _Message
        End Get
        Set(value As String)
            _Message = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Message"))
        End Set
    End Property
    Public ReadOnly Property Cancel As New ViewModelCommand(AddressOf cmdCancel)
    Private Sub cmdCancel(value As Object)
        _Window.DialogResult = DialogResult.Cancel
    End Sub
    Public ReadOnly Property Update As New ViewModelCommand(AddressOf cmdUpdate)
    Private Sub cmdUpdate(value As Object)
        Dim updater As String = AppDomain.CurrentDomain.BaseDirectory + "MCDSUpdate.exe"
        Process.Start(updater)
        _Window.DialogResult = DialogResult.OK
        Application.Exit()
    End Sub
    Friend Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class