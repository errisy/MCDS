Public Class PseudoGeneFile
    '这个是虚拟的连接文件 为了加快计算速度 节省内存
    Public Sub New()

    End Sub
    Public Sub New(ByVal vGeneFile As Nuctions.GeneFile)
        GeneFiles.Add(vGeneFile)
        Directions.Add(True)
    End Sub
    Public Sub New(ByVal vPGF1 As PseudoGeneFile, ByVal vDrt1 As Boolean, ByVal vPGF2 As PseudoGeneFile, ByVal vDrt2 As Boolean)
        Append(vPGF1, vDrt1)
        Append(vPGF2, vDrt2)
    End Sub
    Public Overrides Function ToString() As String
        Dim stb As New System.Text.StringBuilder
        stb.Append(End_F)
        For i As Integer = 0 To GeneFiles.Count - 1
            If Directions(i) Then
                stb.AppendFormat("[>{0}>]", GeneFiles(i).Length)
            Else
                stb.AppendFormat("[<{0}<]", GeneFiles(i).Length)
            End If
        Next
        stb.Append(End_R)
        Return stb.ToString()
    End Function
    Public Sub Append(ByVal vPGF As PseudoGeneFile, ByVal vDirectioin As Boolean)
        If vDirectioin Then
            For i As Integer = 0 To vPGF.GeneFiles.Count - 1
                GeneFiles.Add(vPGF.GeneFiles(i))
                Directions.Add(vPGF.Directions(i))
            Next
        Else
            For i As Integer = 0 To vPGF.GeneFiles.Count - 1
                GeneFiles.Add(vPGF.GeneFiles(i))
                Directions.Add(Not vPGF.Directions(i))
            Next
        End If
    End Sub
    Public Sub Append(ByVal vGF As Nuctions.GeneFile, ByVal vDirectioin As Boolean)
        GeneFiles.Add(vGF)
        Directions.Add(vDirectioin)
    End Sub
    Public Function Clone()
        Dim PGF As New PseudoGeneFile
        For i As Integer = 0 To GeneFiles.Count - 1
            PGF.GeneFiles.Add(GeneFiles(i))
            PGF.Directions.Add(Directions(i))
        Next
        Return PGF
    End Function
    Public Function RC()
        Dim PGF As New PseudoGeneFile
        For i As Integer = GeneFiles.Count - 1 To 0 Step -1
            PGF.GeneFiles.Add(GeneFiles(i))
            PGF.Directions.Add(Not Directions(i))
        Next
        Return PGF
    End Function
    Public Directions As New List(Of Boolean)
    Public GeneFiles As New List(Of Nuctions.GeneFile)
    Public IsCircular As Boolean = False
    Public ReadOnly Property End_F As String
        Get
            If GeneFiles.Count = 0 Then
                Return ""
            ElseIf IsCircular Then
                Return "::"
            Else
                Dim l As Integer = 0
                If Directions(l) Then
                    Return GeneFiles(l).End_F
                Else
                    Return GeneFiles(l).End_R
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property End_R As String
        Get
            If GeneFiles.Count = 0 Then
                Return ""
            ElseIf IsCircular Then
                Return "::"
            Else
                Dim l As Integer = GeneFiles.Count - 1
                If Directions(l) Then
                    Return GeneFiles(l).End_R
                Else
                    Return GeneFiles(l).End_F
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property Head(ByVal N As Integer) As String
        Get
            If GeneFiles.Count = 0 Then
                Return ""
            Else
                Dim l As Integer = 0
                If Directions(l) Then
                    Return GeneFiles(l).Sequence.Substring(0, N)
                Else
                    Return GeneFiles(l).RCSequence.Substring(0, N)
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property Tail(ByVal N As Integer) As String
        Get
            If GeneFiles.Count = 0 Then
                Return ""
            Else
                Dim l As Integer = GeneFiles.Count - 1
                If Directions(l) Then
                    Return GeneFiles(l).RCSequence.Substring(0, N)
                Else
                    Return GeneFiles(l).Sequence.Substring(0, N)
                End If
            End If
        End Get
    End Property
    '通过shared operator来实现连接操作

    Public Shared Operator +(ByVal gf1 As PseudoGeneFile, ByVal gf2 As PseudoGeneFile) As PseudoGeneFile
        If (gf1 Is Nothing) Or (gf2 Is Nothing) Then Return Nothing
        If Nuctions.CanLigate(gf1.End_R, gf2.End_F) Then
            Dim gf As New PseudoGeneFile
            gf.Append(gf1, True)
            gf.Append(gf2, True)
            Return gf
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator +(ByVal gf1 As PseudoGeneFile) As PseudoGeneFile
        If Nuctions.CanLigate(gf1.End_R, gf1.End_F) Then
            Dim gf As New PseudoGeneFile
            gf.Append(gf1, True)
            gf.IsCircular = True
            Return gf
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator -(ByVal gf1 As PseudoGeneFile, ByVal gf2 As PseudoGeneFile) As PseudoGeneFile
        If (gf1 Is Nothing) Or (gf2 Is Nothing) Then Return Nothing
        If Nuctions.CanRecombine(gf1.End_R, gf2.End_F) Then
            Dim gf As New PseudoGeneFile
            gf.Append(gf1, True)
            gf.Append(gf2, True)
            Return gf
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator -(ByVal gf1 As PseudoGeneFile) As PseudoGeneFile
        If Nuctions.CanRecombine(gf1.End_R, gf1.End_F) Then
            Dim gf As New PseudoGeneFile
            gf.Append(gf1, True)
            gf.IsCircular = True
            Return gf
        Else
            Return Nothing
        End If
    End Operator

    Public Function GetResult() As Nuctions.GeneFile
        Dim stb As New System.Text.StringBuilder
        If GeneFiles.Count > 0 Then


            Dim i As Integer = 0
            Dim gf As New Nuctions.GeneFile
            gf.End_F = End_F
            gf.End_R = End_R
            gf.ModificationDate = Date.Now


            If IsCircular Then
                If Directions(i) Then
                    stb.Append(GeneFiles(i).Sequence, GeneFiles(i).End_F.Length - 2, GeneFiles(i).Length - GeneFiles(i).End_F.Length + 2)
                Else
                    stb.Append(GeneFiles(i).RCSequence, GeneFiles(i).End_R.Length - 2, GeneFiles(i).Length - GeneFiles(i).End_R.Length + 2)
                End If
            Else
                If Directions(i) Then
                    stb.Append(GeneFiles(i).Sequence)
                Else
                    stb.Append(GeneFiles(i).RCSequence)
                End If
            End If




            For i = 1 To GeneFiles.Count - 1
                If Directions(i) Then
                    stb.Append(GeneFiles(i).Sequence, GeneFiles(i).End_F.Length - 2, GeneFiles(i).Length - GeneFiles(i).End_F.Length + 2)
                Else
                    stb.Append(GeneFiles(i).RCSequence, GeneFiles(i).End_R.Length - 2, GeneFiles(i).Length - GeneFiles(i).End_R.Length + 2)
                End If
            Next

            gf.Sequence = stb.ToString
            gf.Name = "MultiLig" + gf.Length.ToString
            Return gf
        Else
            Return Nothing
        End If
    End Function

    Public Shared Operator =(ByVal G1 As PseudoGeneFile, ByVal G2 As PseudoGeneFile) As Boolean
        If (G1.GeneFiles.Count <> G2.GeneFiles.Count) OrElse (G1.IsCircular Xor G2.IsCircular) Then
            Return False
        ElseIf G1.IsCircular And G2.IsCircular Then
            'circular DNA
            Dim ForwardG As List(Of Nuctions.GeneFile) = G1.GetCircularDNA()
            Dim ForwardB As List(Of Boolean) = G1.GetCircularDir()

            Dim ReverseG As List(Of Nuctions.GeneFile) = G1.GetRCCircularDNA()
            Dim ReverseB As List(Of Boolean) = G1.GetRCCircularDir()


            For i As Integer = 0 To G1.GeneFiles.Count - 1
                If ContinueMatch(ForwardG, G2.GeneFiles, ForwardB, G2.Directions, i) AndAlso ContinueMatch(ReverseG, G2.GeneFiles, ReverseB, G2.Directions, i) Then Return True
            Next
            Return False

        Else
            Dim ForwardG As List(Of Nuctions.GeneFile) = G1.GeneFiles
            Dim ForwardB As List(Of Boolean) = G1.Directions

            Dim vRC As PseudoGeneFile = G1.RC
            Dim ReverseG As List(Of Nuctions.GeneFile) = vRC.GeneFiles
            Dim ReverseB As List(Of Boolean) = vRC.Directions

            For i As Integer = 0 To G1.GeneFiles.Count - 1
                If ContinueMatch(ForwardG, G2.GeneFiles, ForwardB, G2.Directions, i) AndAlso ContinueMatch(ReverseG, G2.GeneFiles, ReverseB, G2.Directions, i) Then Return True
            Next
            Return False
        End If
    End Operator
    Public Shared Operator <>(ByVal G1 As PseudoGeneFile, ByVal G2 As PseudoGeneFile) As Boolean
        Return Not (G1 = G2)
    End Operator
    'Public Function IndexOf(ByVal PGF As PseudoGeneFile) As Integer
    '    Dim l As Integer = 0
    '    If IsCircular Then
    '        For i As Integer = 0 To GeneFiles.Count - 1

    '        Next
    '    Else

    '    End If
    'End Function
    Private Shared Function ContinueMatch(ByVal GFLSource As List(Of Nuctions.GeneFile), ByVal GFLTarget As List(Of Nuctions.GeneFile), ByVal DIRSource As List(Of Boolean), ByVal DIRTarget As List(Of Boolean), ByVal firstindex As Integer) As Boolean
        Dim i As Integer = firstindex
        Dim j As Integer = 0
        Dim match As Boolean = True
        While j < GFLTarget.Count
            If (i + j) >= GFLSource.Count Then Return False
            match = (GFLSource(i + j) Is GFLTarget(j)) And (DIRSource(i + j) = DIRTarget(j))
            If Not match Then Return False
            j += 1
        End While
        Return match
    End Function
    Private Function GetCircularDNA() As List(Of Nuctions.GeneFile)
        Dim vList As New List(Of Nuctions.GeneFile)
        For i As Integer = 0 To GeneFiles.Count - 1
            vList.Add(GeneFiles(i))
        Next
        For i As Integer = 0 To GeneFiles.Count - 1
            vList.Add(GeneFiles(i))
        Next
        Return vList
    End Function
    Private Function GetRCCircularDNA() As List(Of Nuctions.GeneFile)
        Dim vList As New List(Of Nuctions.GeneFile)
        For i As Integer = GeneFiles.Count - 1 To 0 Step -1
            vList.Add(GeneFiles(i))
        Next
        For i As Integer = GeneFiles.Count - 1 To 0 Step -1
            vList.Add(GeneFiles(i))
        Next
        Return vList
    End Function
    Private Function GetCircularDir() As List(Of Boolean)
        Dim vList As New List(Of Boolean)
        For i As Integer = 0 To Directions.Count - 1
            vList.Add(Directions(i))
        Next
        For i As Integer = 0 To Directions.Count - 1
            vList.Add(Directions(i))
        Next
        Return vList
    End Function
    Private Function GetRCCircularDir() As List(Of Boolean)
        Dim vList As New List(Of Boolean)
        For i As Integer = Directions.Count - 1 To 0 Step -1
            vList.Add(Directions(i))
        Next
        For i As Integer = Directions.Count - 1 To 0 Step -1
            vList.Add(Directions(i))
        Next
        Return vList
    End Function

    Friend Shared Function EndLigate(ByVal G1 As PseudoGeneFile, ByVal G2 As PseudoGeneFile) As PseudoGeneFile
        Return G1 + G2
    End Function
    Friend Shared Function SelfLigate(ByVal G1 As PseudoGeneFile) As PseudoGeneFile
        Return +G1
    End Function
    Friend Shared Function EndRecombine(ByVal G1 As PseudoGeneFile, ByVal G2 As PseudoGeneFile) As PseudoGeneFile
        Return G1 - G2
    End Function
    Friend Shared Function SelfRecombine(ByVal G1 As PseudoGeneFile) As PseudoGeneFile
        Return -G1
    End Function
    Friend Shared Sub ReducePseudoDNA(ByVal DNAs As List(Of PseudoGeneFile), Token As System.Threading.CancellationToken)
        Dim gf1 As PseudoGeneFile, gf2 As PseudoGeneFile
        Dim DuplicateList As New List(Of PseudoGeneFile)
        Dim UniqueList As New List(Of PseudoGeneFile)

        'Dim gfList As New List(Of GeneFile)
        For Each gf1 In DNAs
            If Token.IsCancellationRequested Then Exit For
            If DuplicateList.Contains(gf1) Then
                Continue For
            ElseIf UniqueList.Contains(gf1) Then
                Continue For
            Else
                UniqueList.Add(gf1)
                For Each gf2 In DNAs
                    If Token.IsCancellationRequested Then Exit For
                    If DuplicateList.Contains(gf2) Then Continue For
                    If gf2 Is gf1 Then Continue For
                    If gf1 = gf2 Then DuplicateList.Add(gf2)
                    If Token.IsCancellationRequested Then Exit For
                Next
            End If
            If Token.IsCancellationRequested Then Exit For
        Next
        DNAs.Clear()
        DNAs.AddRange(UniqueList)
    End Sub
End Class

Public Delegate Function PseudoConnector(ByVal PGF1 As PseudoGeneFile, ByVal PGF2 As PseudoGeneFile) As PseudoGeneFile
Public Delegate Function PseudoSelfConnector(ByVal PGF As PseudoGeneFile) As PseudoGeneFile

Friend Class MultipleVirtualReactor
    Private vGeneFiles As New List(Of PseudoGeneFile)
    Public Connector As PseudoConnector
    Public SelfConnector As PseudoSelfConnector
    Private _Rounds As Integer
    Public Sub New(ByVal gList As List(Of Nuctions.GeneFile), ByVal vConnector As PseudoConnector, ByVal vSelfConnector As PseudoSelfConnector, Optional ByVal vBothSide As Boolean = True, Optional Rounds As Integer = 1)
        If Not (gList Is Nothing) Then
            For Each gf As Nuctions.GeneFile In gList
                vGeneFiles.Add(New PseudoGeneFile(gf))
            Next
        End If
        'if any two fragments can only ligate two each other, they can be considered as one fragment.
        ReduceComplexity()
        Connector = vConnector
        SelfConnector = vSelfConnector

        _Rounds = Rounds

    End Sub
    Public Sub Connect()

        Dim _Cancel As New CancelRunViewModel() With {.Operation = "Joining DNA Fragments"}
        Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                  Try
                                                                      For i As Integer = 1 To _Rounds
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          TryConnectOne()
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          ReduceComplexity()
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          TrySelfConnect()
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          ReduceComplexity()
                                                                          If token.IsCancellationRequested Then Exit For
                                                                      Next
                                                                  Catch ex As Exception

                                                                  End Try
                                                                  If token.IsCancellationRequested Then vGeneFiles.Clear()
                                                                  _Cancel.Close()
                                                              End Sub, _Cancel.Token)
        ConnectingTask.Start()
        CancelRunViewModel.ShowCancelRunWindow(_Cancel)
    End Sub
    Public Sub ReduceComplexity()
        Dim rmList As New List(Of PseudoGeneFile)
        For Each pgP In vGeneFiles
            If rmList.Contains(pgP) Then Continue For
            For Each pgC In vGeneFiles
                If pgC Is pgP Then Continue For
                If rmList.Contains(pgC) Then Continue For
                If pgP = pgC Then
                    rmList.Add(pgC)
                End If
            Next
        Next
        For Each g In rmList
            vGeneFiles.Remove(g)
        Next
    End Sub
    Public Sub TryConnectOne()
        Dim pList As New List(Of PseudoGeneFile)
        Dim Result As PseudoGeneFile
        For Each pgP In vGeneFiles
            For Each pgC In vGeneFiles
                Result = Connector.Invoke(pgP, pgC)
                If Result IsNot Nothing Then pList.Add(Result)
                Result = Connector.Invoke(pgP, pgC.RC)
                If Result IsNot Nothing Then pList.Add(Result)
                Result = Connector.Invoke(pgP.RC, pgC)
                If Result IsNot Nothing Then pList.Add(Result)
                Result = Connector.Invoke(pgP.RC, pgC.RC)
                If Result IsNot Nothing Then pList.Add(Result)
            Next
        Next
        vGeneFiles.AddRange(pList)
    End Sub
    Public Sub TrySelfConnect()
        Dim pList As New List(Of PseudoGeneFile)
        For Each pgP In vGeneFiles
            Dim Result = SelfConnector.Invoke(pgP)
            If Result IsNot Nothing Then pList.Add(Result)
        Next
        vGeneFiles.AddRange(pList)
    End Sub
    Public Function GetProducts() As List(Of Nuctions.GeneFile)
        Dim vResults As New List(Of Nuctions.GeneFile)
        Dim _Cancel As New CancelRunViewModel() With {.Operation = "Generating DNAs from Pseudo Products"}
        Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                  Try
                                                                      For Each pgf In vGeneFiles
                                                                          vResults.Add(pgf.GetResult)
                                                                      Next
                                                                  Catch ex As Exception

                                                                  End Try
                                                                  _Cancel.Close()
                                                              End Sub, _Cancel.Token)
        ConnectingTask.Start()
        CancelRunViewModel.ShowCancelRunWindow(_Cancel)
        Return vResults
    End Function
End Class


Public Class MultipleVirtualRecombinator
    Private vGeneFiles As New List(Of PseudoGeneFile)
    Private vBoxes As New List(Of MultipleVirtualRecmbineBox)
    Private _Connector As PseudoConnector
    Private _SelfConnector As PseudoSelfConnector

    Public Sub New(ByVal gList As List(Of Nuctions.GeneFile), ByVal vConnector As PseudoConnector, ByVal vSelfConnector As PseudoSelfConnector, Optional ByVal vBothSide As Boolean = True, Optional Duplicator As Integer = 1)
        _Connector = vConnector
        _SelfConnector = vSelfConnector
        If Not (gList Is Nothing) Then
            For Each gf As Nuctions.GeneFile In gList
                For i As Integer = 1 To Duplicator
                    vGeneFiles.Add(New PseudoGeneFile(gf))
                Next
            Next
        End If
        'if any two fragments can only ligate two each other, they can be considered as one fragment.
        ReduceComplexity()
        For Each vG As PseudoGeneFile In vGeneFiles
            vBoxes.Add(New MultipleVirtualRecmbineBox(vG, vGeneFiles, vConnector, vSelfConnector, vBothSide))
        Next

    End Sub
    Friend Sub Connect()
        Dim _Cancel As New CancelRunViewModel() With {.Operation = "Assembling DNA Fragments"}
        Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                  Try
                                                                      For Each box As MultipleVirtualRecmbineBox In vBoxes
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          While (Not token.IsCancellationRequested) AndAlso box.TryConnectOne(token)
                                                                          End While
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          box.TrySelfConnect(token)
                                                                          If token.IsCancellationRequested Then Exit For
                                                                      Next
                                                                  Catch ex As Exception

                                                                  End Try
                                                                  If token.IsCancellationRequested Then vBoxes.Clear()
                                                                  _Cancel.Close()
                                                              End Sub, _Cancel.Token)
        ConnectingTask.Start()
        CancelRunViewModel.ShowCancelRunWindow(_Cancel)
    End Sub

    Private Sub ReduceComplexity()
        Dim i As Integer = 0
        Dim j As Integer = 1
        Dim key1 As PseudoGeneFile
        Dim key2 As PseudoGeneFile
        Dim match As PseudoGeneFile
        While i < vGeneFiles.Count
            key1 = vGeneFiles(i)
            '找到唯一可以连接的器件
            j = -1
            match = FindOnlyMatch(i, j)

            '如果有效的话 连接之后替换现有的
            If j > -1 And j < vGeneFiles.Count Then
                If i = j Then
                    i += 1
                Else
                    key2 = vGeneFiles(j)
                    vGeneFiles.Insert(i, match)
                    vGeneFiles.Remove(key1)
                    vGeneFiles.Remove(key2)
                End If
            Else
                i += 1
            End If
        End While
    End Sub

    Private Function FindOnlyMatch(ByVal i As Integer, ByRef k As Integer) As PseudoGeneFile
        Dim count As Integer = 0
        Dim product As PseudoGeneFile = Nothing
        Dim temp As PseudoGeneFile
        For j As Integer = 0 To vGeneFiles.Count - 1
            If i <> j Then
                temp = _Connector(vGeneFiles(i), vGeneFiles(j))
                If Not (temp Is Nothing) Then
                    product = temp
                    k = j
                    count += 1
                End If
                temp = _Connector(vGeneFiles(i), vGeneFiles(j).RC)
                If Not (temp Is Nothing) Then
                    product = temp
                    k = j
                    count += 1
                End If
            Else
                temp = _SelfConnector(vGeneFiles(i))
                If Not (temp Is Nothing) Then
                    product = temp
                    k = j
                    count += 1
                End If
            End If
        Next
        If count = 1 Then
            Return product
        Else
            k = -1
            Return Nothing
        End If
    End Function

    Public Function GetProducts() As List(Of Nuctions.GeneFile)
        Dim vProducts As New List(Of Nuctions.GeneFile)

        Dim _Cancel As New CancelRunViewModel() With {.Operation = "Generating DNAs from Pseudo Products"}
        Dim ConnectingTask As New System.Threading.Tasks.Task(Sub(token As System.Threading.CancellationToken)
                                                                  Try

                                                                      Dim vPSFs As New List(Of PseudoGeneFile)
                                                                      For Each box As MultipleVirtualRecmbineBox In vBoxes
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          vPSFs.AddRange(box.GetProducts(token))
                                                                      Next
                                                                      PseudoGeneFile.ReducePseudoDNA(vPSFs, token)
                                                                      For Each psf As PseudoGeneFile In vPSFs
                                                                          If token.IsCancellationRequested Then Exit For
                                                                          vProducts.Add(psf.GetResult)
                                                                      Next
                                                                      If token.IsCancellationRequested Then vProducts.Clear() : _Cancel.Close() : Return
                                                                      Nuctions.ReduceDNA(vProducts, token)
                                                                  Catch ex As Exception

                                                                  End Try
                                                                  If token.IsCancellationRequested Then vProducts.Clear()
                                                                  _Cancel.Close()
                                                              End Sub, _Cancel.Token)
        ConnectingTask.Start()
        CancelRunViewModel.ShowCancelRunWindow(_Cancel)

        Return vProducts
    End Function
End Class
Public Delegate Function Connector(ByVal vGeneFile1 As PseudoGeneFile, ByVal vGeneFile2 As PseudoGeneFile) As PseudoGeneFile
Public Delegate Function SelfConnector(ByVal vGeneFile1 As PseudoGeneFile) As PseudoGeneFile
Public Class MultipleVirtualRecmbineBox
    Public Seed As PseudoGeneFile
    Public Connector As PseudoConnector
    Public SelfConnector As PseudoSelfConnector
    Public Products As New List(Of MultipleProductCombination)
    Public Intermediates As New List(Of MultipleProductCombination)
    Public BothSide As Boolean = True
    Public Sub New(ByVal vSeed As PseudoGeneFile, ByVal vPool As List(Of PseudoGeneFile), ByVal vConnector As PseudoConnector, ByVal vSelfConnector As PseudoSelfConnector, Optional ByVal vBothSide As Boolean = True)
        Seed = vSeed
        Connector = vConnector
        SelfConnector = vSelfConnector
        Products.Add(New MultipleProductCombination(Seed, Seed, vPool))
    End Sub
    Public Function TryConnectOne(token As System.Threading.CancellationToken) As Boolean
        Intermediates.AddRange(Products)
        Products.Clear()
        Dim vResult As PseudoGeneFile = Nothing
        Dim vTotalNewRecombineOccured As Boolean = False
        Dim vIntermediateNewRecombineOccured As Boolean = False
        For Each im As MultipleProductCombination In Intermediates
            If token.IsCancellationRequested Then Exit For
            If im.Working Then
                im.Working = False
                vIntermediateNewRecombineOccured = False
                For Each gf As PseudoGeneFile In im.Pool
                    If token.IsCancellationRequested Then Exit For
                    vResult = Connector.Invoke(im.Product, gf)
                    If Not (vResult Is Nothing) Then
                        Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                        vIntermediateNewRecombineOccured = True
                    End If
                    vResult = Connector.Invoke(im.Product, gf.RC)
                    If Not (vResult Is Nothing) Then
                        Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                        vIntermediateNewRecombineOccured = True
                    End If
                    If BothSide Then
                        vResult = Connector.Invoke(im.Product.RC, gf)
                        If Not (vResult Is Nothing) Then
                            Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                            vIntermediateNewRecombineOccured = True
                        End If
                        vResult = Connector.Invoke(im.Product.RC, gf.RC)
                        If Not (vResult Is Nothing) Then
                            Products.Add(New MultipleProductCombination(vResult, gf, im.Pool))
                            vIntermediateNewRecombineOccured = True
                        End If
                    End If
                    vTotalNewRecombineOccured = vTotalNewRecombineOccured Or vIntermediateNewRecombineOccured
                Next
                im.Working = im.Working Or vIntermediateNewRecombineOccured
                If Not im.Working Then Products.Add(im)
            Else
                Products.Add(im)
            End If
        Next
        Intermediates.Clear()
        Return vTotalNewRecombineOccured
    End Function
    Public Function TrySelfConnect(token As System.Threading.CancellationToken) As Boolean
        Intermediates.AddRange(Products)
        Products.Clear()
        Dim vResult As PseudoGeneFile = Nothing
        Dim vTotalNewRecombineOccured As Boolean = False
        Dim vIntermediateNewRecombineOccured As Boolean = False
        For Each im As MultipleProductCombination In Intermediates
            If token.IsCancellationRequested Then Exit For
            vIntermediateNewRecombineOccured = False
            vResult = SelfConnector.Invoke(im.Product)
            If Not (vResult Is Nothing) Then
                Products.Add(New MultipleProductCombination(vResult, im.Pool))
                vIntermediateNewRecombineOccured = True
            Else
                Products.Add(im)
            End If
            vTotalNewRecombineOccured = vTotalNewRecombineOccured And vIntermediateNewRecombineOccured
            'If Not vIntermediateNewRecombineOccured Then Products.Add(im)
        Next
        Intermediates.Clear()
        Return vTotalNewRecombineOccured
    End Function
    Public Function GetProducts(token As System.Threading.CancellationToken) As List(Of PseudoGeneFile)
        Dim vGeneFiles As New List(Of PseudoGeneFile)
        For Each MPC As MultipleProductCombination In Products
            If token.IsCancellationRequested Then Exit For
            vGeneFiles.Add(MPC.Product)
        Next
        PseudoGeneFile.ReducePseudoDNA(vGeneFiles, token)
        Return vGeneFiles
    End Function
End Class
Public Class MultipleProductCombination
    Public Product As PseudoGeneFile
    Public Pool As New List(Of PseudoGeneFile)
    Public Working As Boolean = True
    Public Sub New(ByVal vProduct As PseudoGeneFile, ByVal vTarget As PseudoGeneFile, ByVal vPreviousPool As List(Of PseudoGeneFile))
        Product = vProduct
        Pool.AddRange(vPreviousPool)
        If Pool.Contains(vTarget) Then Pool.Remove(vTarget)
    End Sub
    Public Sub New(ByVal vProduct As PseudoGeneFile, ByVal vPreviousPool As List(Of PseudoGeneFile))
        Product = vProduct
        Pool.AddRange(vPreviousPool)
    End Sub
End Class
