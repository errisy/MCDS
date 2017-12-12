Imports System.Text.RegularExpressions, System.Collections.ObjectModel
Partial Public Class KEGGUtil
    Private Shared Function GetDirectory(Name As String) As String
        If Not IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name) Then IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name)
        Return System.AppDomain.CurrentDomain.BaseDirectory + "\" + Name + "\"
    End Function

End Class
