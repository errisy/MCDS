Public Class SiteRecombination
    Public Name As String
    Public Site As String
    Public SCut As Integer
    Public RCut As Integer
    Public Type As Integer
End Class

Public Enum SiteRecombinationTypeEnum As Integer
    Reversible
    Irreversible
    HairpinSplit
End Enum

Public Class SiteRecombinationGroup
    Inherits Dictionary(Of String, SiteRecombination)
    Public Name As String = ""
End Class

Public Class SiteRecombinationGroups
    Inherits Dictionary(Of String, SiteRecombinationGroup)
    Private vFileName As String = Application.StartupPath + "\SiteRecombinations.vxs"
    Public Sub New()
        Read()
    End Sub
    Public Sub Save()
        If IO.File.Exists(vFileName) Then
            IO.File.Delete(vFileName)
        End If
        Dim fs As New System.IO.FileStream(vFileName, IO.FileMode.Create)
        Dim sw As New System.IO.StreamWriter(fs)
        For Each SRG As SiteRecombinationGroup In Values
            sw.Write("<Group=")
            sw.Write(SRG.Name)
            sw.WriteLine(">")
            For Each SR As SiteRecombination In SRG.Values
                sw.Write("<Recombination=")
                sw.Write(SR.Name)
                sw.WriteLine(">")
                sw.Write("Site=")
                sw.WriteLine(SR.Site)
                sw.Write("CoreStart=")
                sw.WriteLine(SR.SCut.ToString)
                sw.Write("CoreEnd=")
                sw.WriteLine(SR.RCut.ToString)
                sw.Write("Type=")
                sw.WriteLine(SR.Type.ToString)
                sw.WriteLine("</Recombination>")
            Next
            sw.WriteLine("</Group>")
        Next
        sw.Close()
        fs.Close()
    End Sub
    Public Sub Read()
        If IO.File.Exists(vFileName) Then
            Dim s As String = IO.File.ReadAllText(vFileName)
            Dim rgxGP As New System.Text.RegularExpressions.Regex("<(Group)\s*=\s*([^>^<^=]+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim rgxSR As New System.Text.RegularExpressions.Regex("<Recombination=\s*([^>^<^=]+)>\s*Site=([ATGC]+)\s*CoreStart\s*=\s*(\d+)\s*CoreEnd\s*=\s*(\d+)\s*Type\s*=\s*(\d+)\s*</Recombination>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim groups As String() = rgxGP.Split(s)
            Dim SRG As SiteRecombinationGroup
            For i As Integer = 0 To groups.Length - 1
                If groups(i).ToLower = "group" Then
                    SRG = New SiteRecombinationGroup
                    SRG.Name = groups(i + 1)
                    For Each m As System.Text.RegularExpressions.Match In rgxSR.Matches(groups(i + 2))
                        Dim SR As New SiteRecombination
                        SR.Name = m.Groups(1).Value
                        SR.Site = m.Groups(2).Value
                        SR.SCut = CInt(m.Groups(3).Value)
                        SR.RCut = CInt(m.Groups(4).Value)
                        SR.Type = CInt(m.Groups(5).Value)
                        SRG.Add(SR.Name, SR)
                    Next
                    Add(SRG.Name, SRG)
                End If
            Next
        End If
    End Sub
End Class