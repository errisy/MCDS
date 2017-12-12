Public Class FileTypeAssociation
    Public Shared Sub Register(filetype As String, title As String)

        Dim ext As String = "." & filetype
        Dim opencmd As String = filetype & "\shell\open\command"
        Dim extkey As Microsoft.Win32.RegistryKey = My.Computer.Registry.ClassesRoot.GetValue(ext)
        Dim cmdkey As Microsoft.Win32.RegistryKey = My.Computer.Registry.ClassesRoot.GetValue(opencmd)
        If extkey Is Nothing Then
            My.Computer.Registry.ClassesRoot.CreateSubKey(ext).SetValue("", title, Microsoft.Win32.RegistryValueKind.String)
        Else
        End If
        If cmdkey Is Nothing Then
            My.Computer.Registry.ClassesRoot.CreateSubKey(opencmd).SetValue("", Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
        Else
        End If
    End Sub
End Class

Public Class LateFileLoader
    Public Shared FilesToLoad As New List(Of String)
End Class