Public Class WpfDialog
    Inherits System.Windows.Forms.Form
    Public Property WPFHost As InteropHost
    Public Sub New()
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterScreen
        'Me.Icon = frmMain.Icon
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        WPFHost = New InteropHost With {.Dock = DockStyle.Fill}
        Controls.Add(WPFHost)
    End Sub
    Private _StartScene As System.Windows.Controls.UserControl
    Public Property StartScene As System.Windows.Controls.UserControl
        Get
            Return _StartScene
        End Get
        Set(value As System.Windows.Controls.UserControl)
            _StartScene = value
            WPFHost.Child = _StartScene
        End Set
    End Property
    Public Property Scenes As System.Windows.Controls.UserControl()
    Public Sub NextScene()
        If TypeOf Scenes Is System.Windows.Controls.UserControl() Then
            Dim i = Scenes.IndexOf(WPFHost.Child)
            i += 1
            If i < Scenes.Length Then
                WPFHost.Child = Scenes(i)
            End If
        End If
    End Sub
    Private Sub WpfDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.DialogResult = Windows.Forms.DialogResult.None Then DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class
Public Module WpfDialogExtension
    <System.Runtime.CompilerServices.Extension> Public Function ShowDialog(wpfControl As System.Windows.Controls.UserControl, Title As String, Width As Integer, Height As Integer,
ParamArray AdditionalScenes As System.Windows.Controls.UserControl()) As DialogResult
        Dim wForm As New WpfDialog With {.Text = Title, .Size = New Size(Width, Height), .StartScene = wpfControl}
        Return wForm.ShowDialog
    End Function
End Module