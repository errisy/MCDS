
Public Module FacilityFunctions
#If ReaderMode = 0 Then
    Friend ReaderMode As Boolean = False
#Else
    Friend ReaderMode As Boolean = True
#End If
    Friend Function ListToString(ByVal vList As List(Of String), Optional ByVal Separator As String = " ") As String
        Dim stb As New System.Text.StringBuilder
        For Each value As String In vList
            stb.Append(value)
            stb.Append(Separator)
        Next
        Return stb.ToString
    End Function
    Public Function DescribeStringList(vList As List(Of String), lang As Language) As String
        Dim stb As New System.Text.StringBuilder
        Dim i As Integer
        Select Case lang
            Case Language.English
                For Each s As String In vList
                    i += 1
                    stb.Append(s)
                    If i < vList.Count - 1 Then
                        stb.Append(", ")
                    ElseIf i = vList.Count - 1 Then
                        stb.Append(" and ")
                    ElseIf i = vList.Count Then
                        'stb.Append("，")
                    End If
                Next
            Case Language.Chinese
                For Each s As String In vList
                    i += 1
                    stb.Append(s)
                    If i < vList.Count - 1 Then
                        stb.Append("、")
                    ElseIf i = vList.Count - 1 Then
                        stb.Append("和")
                    ElseIf i = vList.Count Then
                        'stb.Append("，")
                    End If
                Next
            Case Language.Japanese

        End Select

        Return stb.ToString
    End Function
    Private rgxValue As New System.Text.RegularExpressions.Regex("\w+")
    Friend Function StringToList(ByVal value As String) As List(Of String)
        Dim sList As New List(Of String)
        If TypeOf value Is String Then
            For Each m As System.Text.RegularExpressions.Match In rgxValue.Matches(value)
                sList.Add(m.Groups(0).Value)
            Next
        End If
        Return sList
    End Function

    Public Function Vector(ByVal vP As Point) As Vector2
        Return New Vector2(vP)
    End Function
    Public Function Vector(ByVal vP As PointF) As Vector2
        Return New Vector2(vP)
    End Function

    Public Function CVec(ByVal vP As Point) As Vector2
        Return New Vector2(vP)
    End Function
    Public Function CVec(ByVal vP As PointF) As Vector2
        Return New Vector2(vP)
    End Function
    Public Function Bytes2Hex(ByVal bytes As Byte()) As String
        Dim stb As New System.Text.StringBuilder
        For Each b As Byte In bytes
            stb.Append(Hex(b))
        Next
        Return stb.ToString
    End Function
    Public Const Inch As Single = 25.4 'mm/inch

    Public RegionalLanguage As Language = Language.English
    Public Enum Language As Integer
        English
        Chinese
        Japanese
    End Enum
    Friend Function CastStr(v As Object) As String
        If TypeOf v Is String Then
            Return v
        ElseIf v Is Nothing Then
            Return ""
        Else
            Return v.ToString
        End If
    End Function
    Friend Function GetDGVRows(dgv As DataGridView) As List(Of DataGridViewRow)
        Dim rows As New List(Of DataGridViewRow)
        If dgv.SelectedRows.Count > 0 Then
            For Each r As DataGridViewRow In dgv.SelectedRows
                If r.Index > -1 Then rows.Add(r)
            Next
        ElseIf dgv.SelectedCells.Count > 0 Then
            For Each r As DataGridViewCell In dgv.SelectedCells
                If r.RowIndex > -1 AndAlso Not rows.Contains(r.OwningRow) Then rows.Add(r.OwningRow)
            Next
        End If
        Return rows
    End Function
    Friend Sub LoadEnvironments(items As System.Windows.Forms.ComboBox.ObjectCollection, Hosts As List(Of Nuctions.Host))
        items.Clear()
        For Each h As Nuctions.Host In Hosts
            items.Add(h.Name)
        Next
    End Sub

    Friend Function CanFind(Of T)(Value As T, vList As IList(Of T)) As Boolean
        For Each o As T In vList
            If TryCast(o, Object) = TryCast(Value, Object) Then
                Return True
            End If
        Next
        Return False
    End Function
    Friend Function TryFind(Value As Object, vList As IList) As Object
        For Each o In vList
            If o = Value Then
                Return o
            End If
        Next
        Return Nothing
    End Function
    Friend Function TryFind(Of T)(Value As T, vList As IList(Of T)) As T
        For Each o As T In vList
            If TryCast(o, Object) = TryCast(Value, Object) Then
                Return o
            End If
        Next
        Return Nothing
    End Function
    Friend Function ListDiff(Of T)(List1 As List(Of T), List2 As List(Of T)) As List(Of T)
        Dim LD As New List(Of T)
        LD.AddRange(List1)
        Dim v As T
        For Each o As T In List1
            v = TryFind(o, List2)
            If v IsNot Nothing Then LD.Remove(v)
        Next
        Return LD
    End Function
    Friend Function IsListDifferent(Of T)(List1 As List(Of T), List2 As List(Of T)) As Boolean
        Dim v As T
        For Each o As T In List1
            v = TryFind(o, List2)
            If v Is Nothing Then Return True
        Next
        For Each o As T In List2
            v = TryFind(o, List1)
            If v Is Nothing Then Return True
        Next
        Return False
    End Function
    Public onehour As New TimeSpan(1, 0, 0)
    Friend Function IsRefListSame(Of T)(List1 As List(Of T), List2 As List(Of T)) As Boolean
        For Each o As T In List1
            If Not List2.Contains(o) Then Return False
        Next
        For Each o As T In List2
            If Not List1.Contains(o) Then Return False
        Next
        Return False
    End Function
    Friend Sub RemoveFromList(Of T)(ListHost As List(Of T), ListToRemove As List(Of T))
        For Each item As T In ListToRemove
            If ListHost.Contains(item) Then ListHost.Remove(item)
        Next
    End Sub
    Friend Function FullCombinationGroup(Of T)(vList As List(Of T)) As List(Of List(Of T))
        Dim c As Integer = vList.Count
        Dim fc As List(Of List(Of Integer)) = FullBinaryCombination(c)
        Dim ltt As New List(Of List(Of T))
        Dim lt As List(Of T)
        For Each l As List(Of Integer) In fc
            lt = New List(Of T)
            For Each i As Integer In l
                lt.Add(vList(i))
            Next
            ltt.Add(lt)
        Next
        Return ltt
    End Function

    Public Function FullBinaryCombination(Count As Integer) As List(Of List(Of Integer))
        If Count > 12 Then Return Nothing
        Dim cList As New List(Of List(Of Integer))
        For i As Integer = 0 To 2 ^ Count - 1
            cList.Add(TranslateToBinary(i))
        Next
        Return cList
    End Function

    Private Function TranslateToBinary(i As Integer) As List(Of Integer)
        Dim j As Integer = 0
        Dim vList As New List(Of Integer)
        While j < 12
            If i Mod 2 = 1 Then vList.Add(j)
            i = i \ 2
            j += 1
        End While
        Return vList
    End Function


    Public Function FullCombination(Count As Integer) As List(Of List(Of Integer))
        Dim res As New List(Of List(Of Integer))
        Dim idx As List(Of Integer)
        For i As Integer = -1 To Count - 1
            idx = New List(Of Integer)
            For j As Integer = 0 To i
                idx.Add(j)
            Next
            res.Add(idx)
            If idx.Count > 0 Then
                idx = NextCombination(idx, i)
                While idx IsNot Nothing
                    res.Add(idx)
                    idx = NextCombination(idx, i)
                End While
            End If
        Next
        Return res
    End Function

    Public Function NextCombination(vidx As List(Of Integer), count As Integer) As List(Of Integer)
        Dim idx As New List(Of Integer)
        idx.AddRange(vidx)
        Dim i As Integer = idx.Count - 1
        idx(i) += 1
        Dim r As Integer = 0
        While idx(i - r) = count
            r += 1
            If r = count Then Return Nothing
            idx(i - r) += 1
        End While
        For r = r - 1 To 0 Step -1
            idx(i - r) = idx(i - r - 1) + 1
        Next
        Return idx
    End Function

    Public Function Plural(word As String, count As Integer) As String
        If count > 1 Then
            Return word + "s"
        Else
            Return word
        End If
    End Function
    Public Function Plural(word As String, vList As IList) As String
        If vList.Count > 1 Then
            Return word + "s"
        Else
            Return word
        End If
    End Function
    Public Function Combination(Of T)(values As IEnumerable(Of T)) As List(Of List(Of T))
        Dim rlist As New List(Of List(Of T))
        Dim count As Integer = values.Count
        For i As Integer = 0 To Pow(2, values.Count)
            rlist.Add(FetchList(values, Num2Bin(i, count)))
        Next
        Return rlist
    End Function
    Public Function FetchList(Of T)(values As IEnumerable(Of T), bools As IEnumerable(Of Boolean)) As List(Of T)
        Dim emt As IEnumerator(Of T) = values.GetEnumerator
        Dim emb As IEnumerator(Of Boolean) = bools.GetEnumerator
        emt.Reset()
        emb.Reset()
        Dim vList As New List(Of T)
        While emb.MoveNext
            emt.MoveNext()
            If emb.Current Then vList.Add(emt.Current)
        End While
        Return vList
    End Function
    Public Function Num2Bin(i As Integer, length As Integer) As List(Of Boolean)
        Dim bools As New List(Of Boolean)
        While bools.Count < length
            bools.Add(i Mod 2 = 1)
            i = i \ 2
        End While
        Return bools
    End Function
    Public Function Pow(base As Integer, index As Integer) As Integer
        Dim v As Integer = 1
        While Index > 0
            v *= base
            Index -= 1
        End While
        Return v
    End Function
    Public Function DescribeMultipleDNASize(iDNAs As IEnumerable) As List(Of String)
        Dim stb As New System.Text.StringBuilder
        Dim vList As New List(Of Integer)
        Dim it As IEnumerator = iDNAs.GetEnumerator
        Dim gf As Nuctions.GeneFile
        While it.MoveNext
            If TypeOf it.Current Is Nuctions.GeneFile Then
                gf = it.Current
                If Not vList.Contains(gf.Length) Then vList.Add(gf.Length)
            End If
        End While
        vList.Sort()
        Dim sList As New List(Of String)
        For Each i As Integer In vList
            sList.Add(i.ToString + " bps")
        Next
        Return sList
    End Function
End Module


Public Class StringList
    Inherits List(Of String)
    Public Sub New()
    End Sub
    Public Sub New(sList As List(Of String))
        MyBase.AddRange(sList)
    End Sub
    Public Shared Operator -(sl1 As StringList, sl2 As StringList) As StringList
        Dim sl As New StringList
        sl.AddRange(sl1)
        For Each s As String In sl2
            If sl.Contains(s) Then sl.Remove(s)
        Next
        Return sl
    End Operator
    Public Shared Widening Operator CType(sList As String()) As StringList
        Dim sl As New StringList
        sl.AddRange(sList)
        Return sl
    End Operator
    Public Shared Narrowing Operator CType(sl As StringList) As String()
        Return sl.ToArray
    End Operator
End Class

Public Class EnglishSentenceConstructor
    Public Words As New List(Of String)
    Public Sub Multiple(vList As IEnumerable(Of String))
        Words.Add(DescribeStringList(vList, Language.English))
    End Sub
    Public Sub Append(vList As IEnumerable(Of String))
        For Each s As String In vList
            Words.Add(s)
        Next
    End Sub
    Public Sub Append(ParamArray varWords() As String)
        If varWords IsNot Nothing Then
            For Each s As String In varWords
                Words.Add(s)
            Next
        End If
    End Sub
    Public Sub Be(vList As IList)
        If vList.Count > 1 Then
            Words.Add("are")
        Else
            Words.Add("is")
        End If
    End Sub
    Public Sub PastBe(vList As IList)
        If vList.Count > 1 Then
            Words.Add("were")
        Else
            Words.Add("was")
        End If
    End Sub
    Public Sub Comma()
        Words.Add(",")
    End Sub
    Public Sub Period()
        Words.Add(".")
    End Sub
    Public Sub Plural(word As String, count As Integer)
        Words.Add(FacilityFunctions.Plural(word, count))
    End Sub
    Public Sub Plural(word As String, vList As IList)
        Words.Add(FacilityFunctions.Plural(word, vList))
    End Sub
    Public Overrides Function ToString() As String
        Dim stb As New System.Text.StringBuilder
        Dim Start As Boolean = True
        Dim i As Integer = 0
        For Each s As String In Words
            Select Case s
                Case ","
                    stb.Append(",")
                Case "."
                    stb.Append(".")
                    Start = True
                Case Else
                    If Start Then
                        If s.Length > 0 Then
                            stb.Append(" ")
                            stb.Append(s.Substring(0, 1).ToUpper)
                            If s.Length > 1 Then stb.Append(s.Substring(1))
                        End If
                        Start = False
                    ElseIf s.Length > 0 Then
                        stb.Append(" ")
                        stb.Append(s)
                    End If
            End Select
            i += 1
        Next
        Return stb.ToString
    End Function
End Class


Public Class Pair(Of T1, T2)
    Public Key As T1
    Public Value As T2
End Class