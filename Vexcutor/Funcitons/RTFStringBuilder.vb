Public Class RTFStringBuilder
    Private stb As New System.Text.StringBuilder
    Private Shared _Header As String = "{\rtf1\fbidis\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl"
    Private Shared _View As String = "\viewkind4\uc1"
    Private Shared _Footer As String = "}"
    Private Shared _NewLine As String = "\par"

    'Private Shared _Footer As String = "\pard\ltrpar\lang2052\b0\f3\fs18\par"

    Public Sub New()

    End Sub
    '{\f0\fswiss\fprq2\fcharset0 Shruti;}{\f1\fmodern\fprq1\fcharset0 Courier New;}{\f2\froman\fprq2\fcharset0 Times New Roman;}{\f3\fnil\fcharset134 \'cb\'ce\'cc\'e5;}
    Private Shared _FontDefine As String = "{{\f{0}\fnil\fcharset0 {1};}}"
    Private vFonts As New List(Of String)
    Private Function GetFonts() As String
        Dim stb As New System.Text.StringBuilder
        Dim f As String
        For i As Integer = 0 To vFonts.Count - 1
            f = vFonts(i)
            stb.AppendFormat(_FontDefine, i.ToString, f)
        Next
        Return stb.ToString
    End Function
    Public Property ItalicItems As New List(Of String)
    Public Property BoldItems As New List(Of String)
    Private _Operational As Boolean = False
    Private _Space As String = " "
    Private Sub Operation()
        _Operational = True
    End Sub
    Private Sub Append()
        If _Operational Then stb.Append(_Space) : _Operational = False
    End Sub
    Private Function Replacer(value As String) As String
        Dim format As String = value.Replace("\", "\\")
        For Each ii As String In ItalicItems
            format = format.Replace(ii, String.Format("{0} {1}{2} ", "\i", ii, "\i0"))
        Next
        For Each ii As String In BoldItems
            format = format.Replace(ii, String.Format("{0} {1}{2} ", "\b", ii, "\b0"))
        Next
        Return format
    End Function
    Private Sub Header()
        stb.AppendLine(_Header)
        stb.AppendLine(_View)
        Operation()
    End Sub
    Public Sub AppendLine()
        stb.Append(_NewLine)
        Operation()
    End Sub
    Private Shared _FirstIndent As String = "\fi"
    Private Shared _LineIndent As String = "\li"
    Private Shared _Bold As String = "\b"
    Private Shared _DeBold As String = "\b0"
    Private Shared _Italic As String = "\i"
    Private Shared _DeItalic As String = "\i0"
    Public Sub FirstIndent(Optional Value As Integer = 420)
        stb.Append(_FirstIndent + Value.ToString)
        Operation()
    End Sub
    Public Sub LineIndent(Optional Value As Integer = 420)
        stb.Append(_LineIndent + Value.ToString)
        Operation()
    End Sub
    Public Sub StartBold()
        stb.Append(_Bold)
        Operation()
    End Sub
    Public Sub EndBold()
        stb.Append(_DeBold)
        Operation()
    End Sub
    Public Sub StartItalic()
        stb.Append(_Italic)
        Operation()
    End Sub
    Public Sub EndItalic()
        stb.Append(_DeItalic)
        Operation()
    End Sub
    Private Shared _Font As String = "\f"
    Private Shared _FontSize As String = "\fs"
    Public Sub Font(Index As Integer)
        stb.Append(_Font + Index.ToString)
        Operation()
    End Sub
    Public Sub Font(Name As String)
        Dim Index As Integer = 0
        If Not vFonts.Contains(Name.ToLower) Then vFonts.Add(Name)
        Index = vFonts.IndexOf(Name)
        stb.Append(_Font + Index.ToString)
        Operation()
    End Sub

    Public Sub FontSize(Size As FontSizeEnum)
        stb.Append(_FontSize + CInt(Size).ToString)
        Operation()
    End Sub
    Public Sub FontSize(Size As Integer)
        stb.Append(_FontSize + Size.ToString)
        Operation()
    End Sub
    Public Sub Append(Value As String)
        Append()
        stb.Append(Replacer(Value))
    End Sub
    Public Sub AppendFormat(Value As String, ParamArray parameters As Object())
        Append()
        stb.Append(Replacer(String.Format(Value, parameters)))
    End Sub
    Public Sub AppendLine(Value As String)
        Append()
        stb.Append(Replacer(Value))
        AppendLine()
    End Sub
    Public Sub AppendLine(Value As String, ParamArray parameters As Object())
        Append()
        stb.Append(Replacer(String.Format(Value, parameters)))
        AppendLine()
    End Sub
    Public Overrides Function ToString() As String
        Return String.Format("{0}{1}{2}{3}{4}{5}", _Header, GetFonts, _Footer, _View, stb.ToString, _Footer)
    End Function
    Public AppendText As String = ""
End Class

Public Enum FontSizeEnum As Integer
    F0 = 84
    F0s = 72
    F1 = 52
    F1s = 48
    F2 = 44
    F2s = 36
    F3 = 32
    F3s = 30
    F4 = 28
    F4s = 42
    F5 = 21
    F5s = 18
    F6 = 15
    F6s = 13
    F7 = 11
    F8 = 10
End Enum