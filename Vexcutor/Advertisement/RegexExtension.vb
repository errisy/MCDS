Public Module RegexExtension
    <System.Runtime.CompilerServices.Extension()> Public Function IsMatch(value As String, pattern As String) As Boolean
        Try
            Dim rgx As New System.Text.RegularExpressions.Regex(pattern)
            Return rgx.IsMatch(value)
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
