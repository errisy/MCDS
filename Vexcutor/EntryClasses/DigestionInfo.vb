Public Class Digestion
    Inherits List(Of DigestionInfo)
    Public Title As String
    Public FileAddress As String
    Private Shared rgxTitle As New System.Text.RegularExpressions.Regex("\s*<([\w\s\-\.]+)>\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Shared rgxName As New System.Text.RegularExpressions.Regex("\s*Name\s*\:", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Shared rgxTemp As New System.Text.RegularExpressions.Regex("\s*Temp\s*\:", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Shared rgxAdditive As New System.Text.RegularExpressions.Regex("\s*Additive\s*\:", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Shared rgxActivity As New System.Text.RegularExpressions.Regex("\s*Activity\s*\:", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    Private Shared rgxWordDigital As New System.Text.RegularExpressions.Regex("\w+")
    Private Shared rgxBufferItem As New System.Text.RegularExpressions.Regex("([\w]+)\s*=\s*([\d]\*?)")
    Friend Function FindDigestionCondition(ByVal ezs As List(Of String)) As String
        Dim em As New List(Of DigestionInfo)
        Dim notfind As New List(Of String)
        Dim find As Boolean = False
        For Each ez As String In ezs
            find = False
            For Each di As DigestionInfo In Me
                If di.Names.Contains(ez) Then
                    em.Add(di)
                    find = True
                    Exit For
                End If
            Next
            If Not find Then notfind.Add(ez)
        Next

        If notfind.Count > 0 Then
            Return String.Format("{0} does not have {1}.", Title, ListToString(notfind))
        Else
            Dim DRs As New List(Of DigestionResult)
            Dim dr As DigestionResult
            Dim rList As New List(Of DigestionInfo)
            rList.AddRange(em)
            em.Clear()
            While rList.Count > 0
                em.AddRange(rList)
                rList.Clear()
                dr = New DigestionResult
                For Each di As DigestionInfo In em
                    If dr.TryAdd(di) Then
                        dr.Add(di)
                    Else
                        rList.Add(di)
                    End If
                Next
                em.Clear()
                DRs.Add(dr)
            End While
            Dim stb As New System.Text.StringBuilder
            stb.Append(Title)
            stb.Append(": ")
            If DRs.Count > 1 Then
                For Each dr In DRs
                    dr.GetFullResult(stb)
                Next
            ElseIf DRs.Count = 1 Then
                For Each dr In DRs
                    dr.GetResult(stb)
                Next
            Else
                stb.Append("N/A")
            End If
            Return stb.ToString
        End If
    End Function
    Public Sub Save()
        Try
            Dim fi As New System.IO.FileInfo(FileAddress)
            Write(fi.FullName)
        Catch ex As Exception
            Write(Application.StartupPath + "\" + FormatFileName(Title) + ".dgi")
        End Try
    End Sub
    Public Sub Write(ByVal filename As String)
        If System.IO.File.Exists(filename) Then
            System.IO.File.Delete(filename)
        End If
        Dim fs As New System.IO.FileStream(filename, IO.FileMode.Create)
        Dim sw As New System.IO.StreamWriter(fs)
        sw.Write("<")
        sw.Write(Title)
        sw.WriteLine(">")
        sw.WriteLine()

        For Each di As DigestionInfo In Me
            sw.Write("Name: ")
            For Each Name As String In di.Names
                sw.Write(Name)
                sw.Write(", ")
            Next
            sw.WriteLine()

            sw.Write("Temp: ")
            For Each Temp As String In di.Temp
                sw.Write(Temp)
                sw.Write(", ")
            Next
            sw.WriteLine()

            sw.Write("Additive: ")
            For Each Additive As String In di.Additives
                sw.Write(Additive)
                sw.Write(", ")
            Next
            sw.WriteLine()

            sw.Write("Activity: ")
            For Each Buffer As String In di.BufferActivities.Keys

                sw.Write(Buffer)
                sw.Write("=")
                sw.Write(di.BufferActivities(Buffer))
                sw.Write(", ")
            Next
            sw.WriteLine()

            sw.WriteLine()
        Next
        sw.Close()
        fs.Close()

    End Sub
    Public Sub ReadDigestionInfo(ByVal filename As String)
        'Names:BamHI,HindIII,
        'Temp:37,
        'Additive:oligo,SAM
        'Activity:O=2,G=3*, "[\w]+=[\d]\*?
        If System.IO.File.Exists(filename) Then
            FileAddress = filename
            Dim SR As New System.IO.StreamReader(filename)
            Dim line As String
            Dim info As String
            Dim m As System.Text.RegularExpressions.Match
            Dim DI As DigestionInfo
            Dim findnewenzyme As Boolean = False

            line = SR.ReadLine

            While Not (line Is Nothing)

                If rgxTitle.IsMatch(line) Then
                    m = rgxTitle.Match(line)
                    Title = m.Groups(1).Value
                End If
                If rgxName.IsMatch(line) Then
                    findnewenzyme = False
                    DI = New DigestionInfo

                    'Read Names
                    m = rgxName.Match(line)
                    info = line.Substring(m.Index + m.Length)
                    For Each m In rgxWordDigital.Matches(info)
                        DI.Names.Add(m.Groups(0).Value)
                    Next

                    line = SR.ReadLine

                    While Not (line Is Nothing)
                        'Read Temp
                        If rgxTemp.IsMatch(line) Then
                            m = rgxTemp.Match(line)
                            info = line.Substring(m.Index + m.Length)
                            For Each m In rgxWordDigital.Matches(info)
                                DI.Temp.Add(m.Groups(0).Value)
                            Next
                        End If

                        'Read Additives
                        If rgxAdditive.IsMatch(line) Then
                            m = rgxAdditive.Match(line)
                            info = line.Substring(m.Index + m.Length)
                            For Each m In rgxWordDigital.Matches(info)
                                DI.Additives.Add(m.Groups(0).Value)
                            Next
                        End If

                        'Read Buffer Activities
                        If rgxActivity.IsMatch(line) Then
                            m = rgxActivity.Match(line)
                            info = line.Substring(m.Index + m.Length)
                            For Each m In rgxBufferItem.Matches(info)
                                DI.BufferActivities.Add(m.Groups(1).Value, m.Groups(2).Value)
                            Next
                        End If

                        'rgxName.IsMatch(line)
                        If rgxName.IsMatch(line) Then
                            findnewenzyme = True
                            Exit While
                        End If



                        'Read Title by chance
                        If rgxTitle.IsMatch(line) Then
                            m = rgxTitle.Match(line)
                            Title = m.Groups(1).Value
                        End If
                        line = SR.ReadLine
                    End While

                    Add(DI)
                End If
                If Not findnewenzyme Then line = SR.ReadLine
            End While
            SR.Close()
        End If
    End Sub
End Class

Public Class DigestionInfo
    Public Names As New List(Of String)
    Public Temp As New List(Of String)
    Public Additives As New List(Of String)
    Public BufferActivities As New Dictionary(Of String, String)
    Public Shared Operator And(ByVal DI1 As DigestionInfo, ByVal DI2 As DigestionInfo) As DigestionInfo
        Dim DI As New DigestionInfo
        For Each add As String In DI1.Additives
            If Not DI.Additives.Contains(add) Then DI.Additives.Add(add)
        Next
        For Each add As String In DI2.Additives
            If Not DI.Additives.Contains(add) Then DI.Additives.Add(add)
        Next
        For Each tmp As String In DI1.Temp
            If Not DI2.Temp.Contains(tmp) Then DI2.Temp.Add(tmp)
        Next
        For Each tmp As String In DI2.Temp
            If Not DI.Temp.Contains(tmp) Then DI.Temp.Add(tmp)
        Next
        Dim Keys As New List(Of String)
        For Each k As String In DI1.BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        For Each k As String In DI2.BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        Dim Activity1 As Integer = 0
        Dim Activity2 As Integer = 0
        Dim Star As Boolean = False

        For Each k As String In Keys
            If DI1.BufferActivities.ContainsKey(k) Then
                Activity1 = CInt(DI1.BufferActivities(k).Substring(0, 1))
                Star = Star Or DI1.BufferActivities(k).EndsWith("*")
            End If
            If DI2.BufferActivities.ContainsKey(k) Then
                Activity2 = CInt(DI2.BufferActivities(k).Substring(0, 1))
                Star = Star Or DI2.BufferActivities(k).EndsWith("*")
            End If
            DI.BufferActivities.Add(k, Math.Min(Activity1, Activity2) + IIf(Star, "*", ""))
        Next
        Return DI
    End Operator
End Class

Friend Class DigestionResult
    Public Temp As New Dictionary(Of String, List(Of DigestionInfo))
    Public Additives As New List(Of String)
    Public BufferActivities As New Dictionary(Of String, DigestionCondition)
    Public Count As Integer = 0
    Public Function TryAdd(ByVal di As DigestionInfo) As Boolean
        Dim Keys As New List(Of String)
        For Each k As String In BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        For Each k As String In di.BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        Dim Activity As Integer = 0
        Dim dc As DigestionCondition
        Dim TryRank2 As Integer
        Dim Star As Boolean
        Dim CountKey As Integer = Keys.Count
        Dim Pass As Integer = 0

        For Each k As String In Keys
            If Not BufferActivities.ContainsKey(k) Then
                dc = New DigestionCondition With {.BufferName = k}
                BufferActivities.Add(k, dc)
            End If
            Star = BufferActivities(k).Star

            If di.BufferActivities.ContainsKey(k) Then
                Activity = CInt(di.BufferActivities(k).Substring(0, 1))
                TryRank2 = BufferActivities(k).Rank2 + IIf(Activity > 1, 1, 0)
                Star = Star Or di.BufferActivities(k).EndsWith("*")
                If TryRank2 = Count + 1 And Not Star Then
                    Pass += 1
                End If
            End If
        Next
        Return Pass > 0
    End Function
    Public Sub Add(ByVal di As DigestionInfo)
        Count += 1
        For Each add As String In di.Additives
            If Not Additives.Contains(add) Then Additives.Add(add)
        Next
        For Each tmp As String In di.Temp
            If Temp.ContainsKey(tmp) Then
                Temp(tmp).Add(di)
            Else
                Temp.Add(tmp, New List(Of DigestionInfo))
                Temp(tmp).Add(di)
            End If
        Next
        Dim Keys As New List(Of String)
        For Each k As String In BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        For Each k As String In di.BufferActivities.Keys
            If Not Keys.Contains(k) Then Keys.Add(k)
        Next
        Dim Activity As Integer = 0
        Dim dc As DigestionCondition

        For Each k As String In Keys
            If Not BufferActivities.ContainsKey(k) Then
                dc = New DigestionCondition With {.BufferName = k}
                BufferActivities.Add(k, dc)
            End If

            If di.BufferActivities.ContainsKey(k) Then
                Activity = CInt(di.BufferActivities(k).Substring(0, 1))
                BufferActivities(k).Rank3 += IIf(Activity = 3, 1, 0)
                BufferActivities(k).Rank2 += IIf(Activity > 1, 1, 0)
                BufferActivities(k).Star = BufferActivities(k).Star Or di.BufferActivities(k).EndsWith("*")
            End If
        Next
    End Sub

    Public Sub GetResult(ByVal stb As System.Text.StringBuilder)
        '最理想的是不分部
        Dim findlist As New List(Of DigestionCondition)
        For Each dc As DigestionCondition In BufferActivities.Values
            If dc.Rank3 = Count Or dc.Rank2 = Count Then
                findlist.Add(dc)
            End If
        Next
        findlist.Sort()
        Dim MX As Integer = Math.Min(findlist.Count - 1, 3)
        'Dim stb As New System.Text.StringBuilder
        Dim di As DigestionInfo
        If Temp.Count > 1 Then
            For Each tmp As String In Temp.Keys
                For dx As Integer = 0 To Temp(tmp).Count - 1
                    di = Temp(tmp)(dx)
                    For ni As Integer = 0 To di.Names.Count - 1
                        stb.Append(di.Names(ni))
                        If ni < di.Names.Count - 1 Then
                            stb.Append("/")
                        End If
                    Next
                    If dx < Temp(tmp).Count - 1 Then
                        stb.Append(" And ")
                    End If
                Next
                stb.Append(" at ")
                stb.Append(tmp)
                stb.Append("°C ")
            Next
        End If
        For i As Integer = 0 To MX
            If findlist(i).Rank3 = Count Then
                stb.Append(findlist(i).BufferName)
                stb.Append("=100%; ")

            Else
                stb.Append(findlist(i).BufferName)
                stb.Append(">50%; ")
            End If
        Next
        '其次情况下可以分步进行
    End Sub
    Public Sub GetFullResult(ByVal stb As System.Text.StringBuilder)
        '最理想的是不分部
        Dim findlist As New List(Of DigestionCondition)
        For Each dc As DigestionCondition In BufferActivities.Values
            If dc.Rank3 = Count Then
                findlist.Add(dc)
            End If
        Next
        findlist.Sort()
        Dim MX As Integer = Math.Min(findlist.Count - 1, 3)
        'Dim stb As New System.Text.StringBuilder
        Dim di As DigestionInfo

        For Each tmp As String In Temp.Keys
            For dx As Integer = 0 To Temp(tmp).Count - 1
                di = Temp(tmp)(dx)
                For ni As Integer = 0 To di.Names.Count - 1
                    stb.Append(di.Names(ni))
                    If ni < di.Names.Count - 1 Then
                        stb.Append("/")
                    End If
                Next
                If dx < Temp(tmp).Count - 1 Then
                    stb.Append(" And ")
                End If
            Next
            stb.Append(" at ")
            stb.Append(tmp)
            stb.Append("°C ")
        Next

        For i As Integer = 0 To MX
            If findlist(i).Rank3 = Count Then
                stb.Append(findlist(i).BufferName)
                stb.Append("=100%; ")

            Else
                stb.Append(findlist(i).BufferName)
                stb.Append(">50%; ")
            End If
        Next

    End Sub
End Class
Friend Class DigestionCondition
    Implements IComparable(Of DigestionCondition)
    Public BufferName As String
    Public Rank3 As Integer
    Public Rank2 As Integer
    Public Star As Boolean
    Public Function CompareTo(ByVal other As DigestionCondition) As Integer Implements System.IComparable(Of DigestionCondition).CompareTo
        Return Math.Sign(other.Rank3 + Rank2 - Rank3 - Rank2)
    End Function
End Class
