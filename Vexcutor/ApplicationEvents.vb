Namespace My

    ' The following events are availble for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        'Protected Friend SerailNubmer As String
        'Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown

        'End Sub
        Private Sub MyApplication_Startup(
            ByVal sender As Object,
            ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs
        ) Handles Me.Startup
            'AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf HandleException
            'FileTypeAssociation.Register("vxt", "MCDS Project")
            'FileTypeAssociation.Register("vct", "MCDS Vector")
            Dim exe As String = System.Reflection.Assembly.GetExecutingAssembly.Location
            'Dim folder As String = exe.Substring(0, exe.LastIndexOf("\"))
            'If (Not System.IO.File.Exists(folder + "\license.vfx")) Or (Not IO.File.Exists(folder + "\license.vfa")) Then
            '    Dim frmPL As New frmPreload
            '    frmPL.ShowDialog()
            'End If
            Try
                ProcessCommand(e.CommandLine)
            Catch ex As Exception

            End Try

        End Sub
        Private Sub MyApplication_Startup(
            ByVal sender As Object,
            ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs
        ) Handles Me.StartupNextInstance
            e.BringToForeground = True
            Try
                ProcessCommand(e.CommandLine)
            Catch ex As Exception

            End Try

        End Sub
        Private Sub ProcessCommand(ByVal lines As System.Collections.ObjectModel.ReadOnlyCollection(Of String))
            Try
                Dim fileOK As Boolean = True
                For Each s As String In lines
                    fileOK = True
                    Try
                        Dim fi As New IO.FileInfo(s)
                    Catch ex As Exception
                        fileOK = False
                    End Try
                    If fileOK Then frmMain.LoadTabFromFile(s)
                    'If frmMain.IsHandleCreated Then

                    'Else
                    '    LateFileLoader.FilesToLoad.Add(s)
                    'End If
                Next
            Catch ex As Exception

            End Try
        End Sub

    End Class

End Namespace

