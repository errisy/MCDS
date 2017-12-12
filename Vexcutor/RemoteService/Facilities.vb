Public Module Facilities
    Private tcprgx As New System.Text.RegularExpressions.Regex("tcp:\\\\([\w\s\.\-]+)")
    Friend Function FormatFileName(ByVal vFileName As String) As String
        Dim s As String() = vFileName.Split(New String() {"\", "/", ":", "*", "?", "\", "<", ">", "|", """"}, System.StringSplitOptions.None)
        Dim stb As New System.Text.StringBuilder
        For Each v As String In s
            stb.Append(v)
        Next
        Return stb.ToString
    End Function
    Friend Function ParseIP(ByVal vAddress As String) As String
        Dim m As System.Text.RegularExpressions.Match = tcprgx.Match(vAddress)
        If m Is Nothing Then
            Return ""
        Else
            Return m.Groups(1).Value
        End If
    End Function
    Friend Function ParseTCPName(ByVal vIP As String, ByVal vFilename As String) As String
        Return String.Format("tcp:\\{0}{1}", vIP, vFilename)
    End Function
    Friend Function ParseTCPName(ByVal vIP As String, ByVal vName As String, ByVal vFilename As String) As String
        Dim vShortName As String
        If tcprgx.IsMatch(vFilename) Then
            vShortName = ParseLevel2Name(vFilename)
        Else
            vShortName = vFilename
        End If
        If Not (vShortName Is Nothing) AndAlso vShortName.Length > 0 Then
            If vFilename.ToLower.EndsWith(".vxt") Then
                Return String.Format("tcp:\\{0}\{1}\Projects\{2}", vIP, vName, vShortName)
            ElseIf vFilename.ToLower.EndsWith(".vct") Then
                Return String.Format("tcp:\\{0}\{1}\Vectors\{2}", vIP, vName, vShortName)
            End If
        End If
        Return ""
    End Function
    Friend Function ParseLevel2Name(ByVal vAddress As String) As String
        '"tcp:\\ip\user\type\name.vnt\sub.vnt"
        If vAddress.StartsWith("tcp:\\") Then
            Dim lvs As String() = vAddress.Split(New Char() {"\"}, System.StringSplitOptions.RemoveEmptyEntries)
            If lvs.Length = 6 Then
                Return lvs(4)
            ElseIf lvs.Length = 5 Then
                Return lvs(4)
            End If
        ElseIf vAddress.StartsWith("\") Then
            '"\user\type\name.vnt\sub.vnt"
            Dim lvs As String() = vAddress.Split(New Char() {"\"}, System.StringSplitOptions.RemoveEmptyEntries)
            If lvs.Length = 4 Then
                Return lvs(2)
            ElseIf lvs.Length = 3 Then
                Return lvs(2)
            End If
        ElseIf Not vAddress.Contains("\") Then
            Return vAddress
        Else
            Try
                Dim fi As New IO.FileInfo(vAddress)
                If fi.Exists Then
                    Return fi.Name
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function
    Public Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String
        Dim fi As System.Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
        Dim aattr() As System.ComponentModel.DescriptionAttribute = _
            DirectCast( _
                fi.GetCustomAttributes(GetType(System.ComponentModel.DescriptionAttribute), False),  _
                System.ComponentModel.DescriptionAttribute() _
            )
        If aattr.Length > 0 Then
            Return aattr(0).Description
        Else
            Return EnumConstant.ToString()
        End If
    End Function
    'Public Function GetGlobalIP() As String
    '    Dim UIP As String
    '    Dim NC As New System.Net.WebClient
    '    Dim regexIP As New System.Text.RegularExpressions.Regex("\d+.\d+.\d+.\d+")
    '    UIP = NC.DownloadString("http://archive.apnic.net/templates/ipv6man/")
    '    UIP = regexIP.Match(UIP).Captures(0).Value
    '    Return UIP
    'End Function
    Public rexPhoneNumber As New System.Text.RegularExpressions.Regex("[0-9\s]+")
    Public Function ValidString(ByVal value As String) As Boolean
        Return Not (value Is Nothing) AndAlso value.Length > 0
    End Function

    Public Function DescribeObject(ByVal component As Object) As String
        Dim value As Object
        Dim stb As New System.Text.StringBuilder
        Dim IsFirst As Boolean = True
        stb.Append("{")
        If TypeOf component Is IList Then
            For Each value In component
                If IsFirst Then
                    IsFirst = False
                Else
                    stb.Append(", ")
                End If
                If value.ToString = value.GetType.ToString Then
                    stb.Append(DescribeObject(value))
                Else
                    stb.Append(value.ToString)
                End If
            Next
        ElseIf TypeOf component Is IDictionary Then
            Dim key As Object
            Dim vDict As IDictionary = component
            For Each key In vDict.Keys
                value = vDict(key)
                If IsFirst Then
                    IsFirst = False
                Else
                    stb.Append(", ")
                End If
                If key.ToString = key.ToString Then
                    stb.Append(DescribeObject(key))
                Else
                    stb.Append(key.ToString)
                End If
                stb.Append("→")
                If value.ToString = value.GetType.ToString Then
                    stb.Append(DescribeObject(value))
                Else
                    stb.Append(value.ToString)
                End If
            Next
        ElseIf TypeOf component Is String Then
            Return component.ToString
        ElseIf TypeOf component Is Integer Then
            Return component.ToString
        ElseIf TypeOf component Is Short Then
            Return component.ToString
        ElseIf TypeOf component Is Byte Then
            Return component.ToString
        ElseIf TypeOf component Is Long Then
            Return component.ToString
        ElseIf TypeOf component Is Decimal Then
            Return component.ToString
        ElseIf TypeOf component Is Single Then
            Return component.ToString
        ElseIf TypeOf component Is Double Then
            Return component.ToString
        ElseIf TypeOf component Is Date Then
            Return component.ToString
        Else
            For Each p As System.ComponentModel.PropertyDescriptor In System.ComponentModel.TypeDescriptor.GetProperties(component)
                value = p.GetValue(component)
                If IsFirst Then
                    IsFirst = False
                Else
                    stb.Append(", ")
                End If
                If value Is Nothing Then
                    stb.Append("N/A")
                ElseIf value.ToString = value.GetType.ToString Then
                    stb.Append(DescribeObject(value))
                Else
                    stb.Append(value.ToString)
                End If
            Next

        End If
        stb.Append("}")
        Return stb.ToString
    End Function

    Public Function CompareObject(Of T)(ByVal Value As T, ByVal Criterion As T) As Integer
        Dim score As Integer
        If TypeOf Value Is IList AndAlso TypeOf Criterion Is IList Then
            Dim vl As IList = Value
            Dim cl As IList = Criterion
            For Each c As Object In cl
                For Each v As Object In vl
                    score += CompareObject(v, c)
                Next
            Next
        ElseIf TypeOf Value Is IDictionary AndAlso TypeOf Criterion Is IDictionary Then
            Dim vd As IDictionary = Value
            Dim cd As IDictionary = Criterion
            For Each c As Object In cd.Values
                For Each v As Object In vd.Values
                    score += CompareObject(v, c)
                Next
            Next
        ElseIf TypeOf Value Is String AndAlso TypeOf Criterion Is String Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vs As String = vo
            Dim cs As String = co
            Dim ci As String() = cs.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
            Dim rg As System.Text.RegularExpressions.Regex
            For Each ct As String In ci
                rg = New System.Text.RegularExpressions.Regex(ct, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                score += rg.Matches(vs).Count
            Next
        ElseIf TypeOf Value Is Integer AndAlso TypeOf Criterion Is Integer Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Integer = vo
            Dim ci As Integer = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Short AndAlso TypeOf Criterion Is Short Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Integer = vo
            Dim ci As Integer = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Byte AndAlso TypeOf Criterion Is Byte Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Byte = vo
            Dim ci As Byte = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Long AndAlso TypeOf Criterion Is Long Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Long = vo
            Dim ci As Long = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Decimal AndAlso TypeOf Criterion Is Decimal Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Decimal = vo
            Dim ci As Decimal = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Single AndAlso TypeOf Criterion Is Single Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Single = vo
            Dim ci As Single = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Double AndAlso TypeOf Criterion Is Double Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Double = vo
            Dim ci As Double = co
            If vi = ci Then
                score += 3
            ElseIf vi.ToString.Contains(ci.ToString) Then
                score += 1
            End If
        ElseIf TypeOf Value Is Date AndAlso TypeOf Criterion Is Date Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            Dim vi As Date = vo
            Dim ci As Date = co
            If vi = ci Then
                score += 3
            End If
        ElseIf Not (Value Is Nothing) AndAlso Not (Criterion Is Nothing) Then
            Dim vo As Object = Value
            Dim co As Object = Criterion
            If vo.GetType.AssemblyQualifiedName = co.GetType.AssemblyQualifiedName Then
                Dim vp As Object
                Dim cp As Object
                For Each p As System.ComponentModel.PropertyDescriptor In System.ComponentModel.TypeDescriptor.GetProperties(Value)
                    vp = p.GetValue(vo)
                    cp = p.GetValue(co)
                    score += CompareObject(vp, cp)
                Next
            End If
        End If

        Return score
    End Function


    Public Function OfType(ByVal T As Type, ByVal Base As Type) As Boolean
        If T Is Base Then Return True
        While Not (T.BaseType Is GetType(Object))
            T = T.BaseType
            If T Is Base Then Return True
        End While
        Return False
    End Function

    Private para As Object() = New Object() {}
    Public Function SearchInList(ByVal Values As IList, ByVal Criterion As EditBase) As IList
        If Values.Count > 0 Then
            Dim vT As Type = Values(0).GetType
            Dim vPS As New Dictionary(Of ValueQuery, System.Reflection.PropertyInfo)

            For Each v As ValueQuery In Criterion.Query.Values
                Dim vp As System.Reflection.PropertyInfo = vT.GetProperty(v.PropertyName)
                vPS.Add(v, vp)
            Next



            Dim Candidates As New List(Of Object)
            Dim Results As New List(Of Object)
            Candidates.AddRange(Values)
            For Each v As ValueQuery In vPS.Keys

                If vPS(v).PropertyType Is GetType(String) Then
                    Select Case v.QueryType
                        Case ValueQueryType.Contains
                            Dim oc As New OrCondition(CStr(v.Value))
                            For Each o As Object In Candidates
                                Dim vl As String = vPS(v).GetValue(o, para)
                                If oc.Match(vl) Then Results.Add(o)
                            Next
                        Case ValueQueryType.EqualTo
                            For Each o As Object In Candidates
                                Dim vl As String = vPS(v).GetValue(o, para)
                                If vl = CStr(v.Value) Then Results.Add(o)
                            Next
                        Case ValueQueryType.NotEqualTo
                            For Each o As Object In Candidates
                                Dim vl As String = vPS(v).GetValue(o, para)
                                If vl <> CStr(v.Value) Then Results.Add(o)
                            Next
                        Case Else
                            Results.AddRange(Candidates)
                    End Select
                ElseIf vPS(v).PropertyType Is GetType(Integer) Then
                    CompareDigial(Of Integer)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(Long) Then
                    CompareDigial(Of Long)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(Single) Then
                    CompareDigial(Of Single)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(Double) Then
                    CompareDigial(Of Double)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(Decimal) Then
                    CompareDigial(Of Decimal)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(Date) Then
                    CompareDigial(Of Date)(v, vPS(v), Candidates, Results)
                ElseIf vPS(v).PropertyType Is GetType(List(Of String)) Then
                    Select Case v.QueryType
                        Case ValueQueryType.Contains
                            Dim oc As New OrCondition(CStr(v.Value))
                            For Each o As Object In Candidates
                                Dim vl As List(Of String) = vPS(v).GetValue(o, para)
                                For Each s As String In vl
                                    If oc.Match(s) Then
                                        Results.Add(o)
                                        Exit For
                                    End If
                                Next
                            Next
                        Case ValueQueryType.EqualTo
                            For Each o As Object In Candidates
                                Dim vl As List(Of String) = vPS(v).GetValue(o, para)
                                For Each s As String In vl
                                    If s = v.Value Then
                                        Results.Add(o)
                                        Exit For
                                    End If
                                Next
                            Next
                        Case ValueQueryType.NotEqualTo
                            For Each o As Object In Candidates
                                Dim vl As List(Of String) = vPS(v).GetValue(o, para)
                                For Each s As String In vl
                                    If s <> v.Value Then
                                        Results.Add(o)
                                        Exit For
                                    End If
                                Next
                            Next
                        Case Else
                            Results.AddRange(Candidates)
                    End Select
                ElseIf vPS(v).PropertyType.GetInterfaces.Contains(GetType(IList)) Then
                    Dim vCl As IList = vPS(v).GetValue(Criterion, para)
                    If vCl.Count > 0 Then
                        Dim vC As EditBase = vCl(0)
                        For Each o As Object In Candidates
                            Dim vVl As IList = vPS(v).GetValue(o, para)
                            If SearchInList(vVl, vC).Count > 0 Then Results.Add(o)
                        Next
                    End If
                ElseIf vPS(v).PropertyType.IsEnum Then
                    CompareDigial(Of Integer)(v, vPS(v), Candidates, Results)
                ElseIf OfType(vPS(v).PropertyType, GetType(EditBase)) Then
                    Dim vC As EditBase = vPS(v).GetValue(Criterion, para)
                    If Not (vC Is Nothing) Then
                        Dim vVl As New List(Of Object)
                        For Each o As Object In Candidates
                            vVl.Add(vPS(v).GetValue(o, para))
                            If SearchInList(vVl, vC).Count > 0 Then Results.Add(o)
                            vVl.Clear()
                        Next
                    End If
                End If
                Candidates.Clear()
                Candidates.AddRange(Results)
                Results.Clear()
            Next
            Return Candidates
        Else
            Return Values
        End If
    End Function
    Private Sub CompareDigial(Of T)(ByVal Value As ValueQuery, ByVal pi As System.Reflection.PropertyInfo, ByVal Candidates As IList, ByVal Results As List(Of Object))
        Select Case Value.QueryType
            Case ValueQueryType.GreaterThan
                For Each o As Object In Candidates
                    Dim vl As T = pi.GetValue(o, para)
                    If vl > Value.Value Then Results.Add(o)
                Next
            Case ValueQueryType.LessThan
                For Each o As Object In Candidates
                    Dim vl As T = pi.GetValue(o, para)
                    If vl < Value.Value Then Results.Add(o)
                Next
            Case ValueQueryType.EqualTo
                For Each o As Object In Candidates
                    Dim vl As T = pi.GetValue(o, para)
                    If vl = Value.Value Then Results.Add(o)
                Next
            Case ValueQueryType.NotEqualTo
                For Each o As Object In Candidates
                    Dim vl As T = pi.GetValue(o, para)
                    If vl <> Value.Value Then Results.Add(o)
                Next
            Case ValueQueryType.Contains
                For Each o As Object In Candidates
                    Dim vl As T = pi.GetValue(o, para)
                    If vl.ToString.Contains(Value.Value.ToString) Then Results.Add(o)
                Next
            Case Else
                Results.AddRange(Candidates)
        End Select
    End Sub
    Class SearchOrCondition
        Private List As New List(Of String)
        Public Sub New(ByVal ParamArray strs As String())
            For Each s As String In strs
                List.Add(s.ToLower)
            Next
        End Sub
        Public Function Match(ByVal value As String) As Boolean
            value = value.ToLower
            For Each s As String In List
                If value.Contains(s) Then Return True
            Next
            Return False
        End Function
    End Class
    Class AndCondition
        Private List As New List(Of SearchOrCondition)
        Public Sub New(ByVal str As String)
            str.Replace("|", " | ")
            Dim strs As String() = str.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
            Dim vList As New List(Of String)
            For i As Integer = 0 To strs.Length - 1
                If strs(i) = "|" Then

                Else
                    If i < strs.Length - 1 Then
                        If strs(i + 1) = "|" Then
                            vList.Add(strs(i))
                        Else
                            vList.Add(strs(i))
                            List.Add(New SearchOrCondition(vList.ToArray))
                            vList.Clear()
                        End If
                    Else
                        vList.Add(strs(i))
                        List.Add(New SearchOrCondition(vList.ToArray))
                        vList.Clear()
                    End If
                End If
            Next
        End Sub
        Public Function Match(ByVal Value As String) As Boolean
            Dim allmatach As Boolean = True
            For Each soc As SearchOrCondition In List
                allmatach = allmatach And soc.Match(Value)
            Next
            Return allmatach
        End Function
    End Class
    Class OrCondition
        Private List As New List(Of AndCondition)
        Public Sub New(ByVal str As String)
            Dim strs As String() = str.Split(New String() {"||"}, StringSplitOptions.RemoveEmptyEntries)
            For Each s As String In strs
                List.Add(New AndCondition(s))
            Next
        End Sub
        Public Function Match(ByVal Value As String) As Boolean
            For Each ac As AndCondition In List
                If ac.Match(Value) Then Return True
            Next
            Return False
        End Function
    End Class
    Public Function CreateInstanceByType(ByVal Type As Type) As Object
        If Type Is GetType(String) Then
            Dim i As String = ""
            Return ""
        ElseIf Type Is GetType(Integer) Then
            Dim i As Integer = 0
            Return i
        ElseIf Type Is GetType(Long) Then
            Dim i As Long = 0
            Return i
        ElseIf Type Is GetType(Single) Then
            Dim i As Single = 0
            Return i
        ElseIf Type Is GetType(Double) Then
            Dim i As Double = 0
            Return i
        ElseIf Type Is GetType(Decimal) Then
            Dim i As Decimal = 0
            Return i
        ElseIf Type Is GetType(Byte) Then
            Dim i As Byte = 0
            Return i
        ElseIf Type Is GetType(Short) Then
            Dim i As Short = 0
            Return i
        ElseIf Type Is GetType(Date) Then
            Return Date.Now
        Else
            Return Type.GetConstructor(New Type() {}).Invoke(para)
        End If
    End Function

    Public Function Clone(ByVal Obj As Object) As Object
        If Obj Is Nothing Then Return Nothing
        Dim vT As Type = Obj.GetType

        If vT.IsValueType Then
            '值传递的类型 一般可以直接比较
            Return Obj
        ElseIf vT Is GetType(String) Then
            Dim Copy As String = ""
            Copy &= Obj
        ElseIf Not (vT.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            Dim iDic1 As IDictionary = Obj
            Dim iDic2 As IDictionary = Copy
            Dim iD1 As IDictionaryEnumerator = iDic1.GetEnumerator

            iD1.Reset()

            While iD1.MoveNext
                iDic2.Add(Clone(iD1.Entry.Key), Clone(iD1.Entry.Value))
            End While
            Return Copy
        ElseIf Not (vT.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            Dim iE1 As IEnumerable = Obj
            Dim iEr1 As IEnumerator = iE1.GetEnumerator
            iEr1.Reset()
            While iEr1.MoveNext
                Copy.Add(iEr1.Current)
            End While
            Return Copy
        Else  '普通的类型
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            For Each pi As System.Reflection.PropertyInfo In vT.GetProperties
                pi.SetValue(Copy, Clone(pi.GetValue(Obj, para)), para)
            Next
            Return Copy
        End If
        Return Obj
    End Function
End Module
