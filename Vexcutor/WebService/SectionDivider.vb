Imports System.Text
Imports System.Text.RegularExpressions
Public Class SectionDivider
    Public Shared Function Divide(Value As String, Pattern As String, Options As RegexOptions) As List(Of String)
        'If Value.Contains("b0002") Then Stop
        Dim regSection As New Regex(Pattern, Options)
        Dim Sections As New List(Of String)
        Dim lastPos As Integer = -1
        For Each m As System.Text.RegularExpressions.Match In regSection.Matches(Value)
            If lastPos > -1 Then
                If lastPos = 0 Then
                    Sections.Add(Value.Substring(lastPos, m.Index - lastPos))
                Else
                    Sections.Add(Value.Substring(lastPos + 1, m.Index - lastPos))
                End If
            End If
            lastPos = m.Index
        Next
        If lastPos = 0 Then
            Sections.Add(Value.Substring(lastPos))
        Else
            Sections.Add(Value.Substring(lastPos + 1))
        End If
        Return Sections
    End Function
    Public Shared Function SelectSection(Sections As List(Of String), Pattern As String, Options As RegexOptions) As List(Of String)
        Dim rex As New Regex(Pattern, Options)
        Dim results As New List(Of String)
        For Each sec In Sections
            If rex.IsMatch(sec) Then results.Add(sec)
        Next
        Return results
    End Function
    Public Shared Function RemoveQuotation(Value As String) As String
        Return Regex.Replace(Regex.Replace(Value, "^\s*""", ""), """\s*$", "")
    End Function
End Class