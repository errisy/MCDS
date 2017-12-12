Public Class frmSummary
    Private smList As New List(Of SummaryItem)

    Public Sub LoadSummary(ByVal Source As List(Of DNAInfo), ByVal Enzymes As List(Of String))
        smList.Clear()
        Dim i As Integer = 0
        Dim l As Integer = Source.Count.ToString.Length

        For Each di As DNAInfo In Source
            smList.Add(New SummaryItem(di, Enzymes, i, l))
        Next
        For Each si As SummaryItem In smList
            For Each zi As SummaryItem In smList
                si.TryFindSource(zi)
            Next
        Next
    End Sub

    Public Sub Draw()
        Dim g As Graphics = bpbMain.BufferedGraphics
        g.Clear(Color.White)
        g.ResetTransform()
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.Low
        g.ScaleTransform(vScale, vScale)
        g.TranslateTransform(offsetX, offsetY)
        For Each si As SummaryItem In smList
            si.Draw(g, True)
        Next
        For Each si As SummaryItem In smList
            si.DrawArrow(g)
        Next
        bpbMain.Draw()
    End Sub


    Public Class SummaryItem
        'Private vVectorMap As Bitmap

        Private dnai As DNAInfo
        Private Shared vFont As Font = New Font("Arial", 10)
        Private Shared tFont As Font = New Font("Calibri", 16, FontStyle.Bold)
        Private scrList As New List(Of SummaryItem)
        Private title As String
        Private descr As String = ""
        Private vVector As VectorMap
        Private sList As New List(Of String)
        Private MapID As String = ""

        Public Sub New(ByVal di As DNAInfo, ByVal Enzymes As List(Of String), ByRef vID As Integer, ByVal IDlength As Integer)
            If di.DNAs.Count = 1 Then
                vVector = New VectorMap
                vVector.Width = 300
                vVector.RestrictionSite = Enzymes
                vVector.GeneFile = di.DNAs(1)
                MapID = vID.ToString.PadLeft(IDlength, "0")
                vID += 1
                'vVectorMap = vVector.GetVectorMap
            End If

            X = di.DX
            Y = di.DY

            dnai = di
            Dim stb As System.Text.StringBuilder

            Select Case dnai.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Vector
                    title = dnai.Name
                Case Nuctions.MolecularOperationEnum.Enzyme

                    title = "Digestion"
                    stb = New System.Text.StringBuilder
                    For Each s As String In dnai.Enzyme_Enzymes
                        stb.Append(s)
                        stb.Append(" ")
                    Next
                    sList.Add(stb.ToString)
                Case Nuctions.MolecularOperationEnum.Gel
                    title = "Gel Extraction"

                    stb = New System.Text.StringBuilder
                    stb.Append("From ")
                    stb.Append(dnai.Gel_Minimum.ToString)
                    stb.Append(" to ")
                    stb.Append(dnai.Gel_Maximun.ToString)
                    sList.Add(stb.ToString)
                Case Nuctions.MolecularOperationEnum.PCR
                    title = "PCR"
                    stb = New System.Text.StringBuilder
                    stb.Append(dnai.PCR_FPrimerName)
                    stb.Append(": ")
                    stb.Append(dnai.PCR_ForwardPrimer)
                    stb.Append(" Tm:")
                    stb.Append(Nuctions.CalculateTm(dnai.PCR_ForwardPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    stb.Append("/")
                    stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(dnai.PCR_ForwardPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    sList.Add(stb.ToString)
                    stb = New System.Text.StringBuilder
                    stb.Append(dnai.PCR_RPrimerName)
                    stb.Append(": ")
                    stb.Append(dnai.PCR_ReversePrimer)
                    stb.Append(" Tm:")
                    stb.Append(Nuctions.CalculateTm(dnai.PCR_ReversePrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    stb.Append("/")
                    stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(dnai.PCR_ReversePrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                    sList.Add(stb.ToString)
                Case Nuctions.MolecularOperationEnum.Recombination
                    title = "Recombination"
                    stb = New System.Text.StringBuilder
                    stb.Append(dnai.RecombinationMethod.ToString)
                    sList.Add(stb.ToString)
                Case Nuctions.MolecularOperationEnum.Ligation
                    title = "Ligation"
                    stb = New System.Text.StringBuilder
                    stb.Append(dnai.LigationMethod.ToString)
                    sList.Add(stb.ToString)
                Case Nuctions.MolecularOperationEnum.Screen
                    Select Case dnai.Screen_Mode
                        Case Nuctions.ScreenModeEnum.PCR
                            title = "PCR Screening"
                            stb = New System.Text.StringBuilder
                            stb.Append(dnai.Screen_FPrimer)
                            stb.Append(": ")
                            stb.Append(dnai.Screen_FPrimer)
                            stb.Append(" Tm:")
                            stb.Append(Nuctions.CalculateTm(dnai.Screen_FPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                            stb.Append("°C/")
                            stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(dnai.Screen_FPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                            stb.Append("°C")
                            sList.Add(stb.ToString)

                            stb = New System.Text.StringBuilder
                            stb.Append(dnai.PCR_RPrimerName)
                            stb.Append(": ")
                            stb.Append(dnai.Screen_RPrimer)
                            stb.Append(" Tm:")
                            stb.Append(Nuctions.CalculateTm(dnai.Screen_RPrimer, 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                            stb.Append("°C/")
                            stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(dnai.Screen_RPrimer), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                            stb.Append("°C")
                            sList.Add(stb.ToString)

                        Case Nuctions.ScreenModeEnum.Features
                            title = "Feature Screening"
                            stb = New System.Text.StringBuilder
                            If dnai.Screen_OnlyCircular Then stb.Append("Only Circular; ")
                            For Each s As FeatureScreenInfo In dnai.Screen_Features
                                stb.Append(s.Feature.Name)
                                stb.Append(" <")
                                stb.Append(s.ScreenMethod.ToString)
                                stb.Append(">; ")
                            Next
                            sList.Add(stb.ToString)
                    End Select
                Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                    title = "Enzyme Analysis"

                    For Each eai As EnzymeAnalysisItem In di.EnzymeAnalysisParamters
                        stb = New System.Text.StringBuilder
                        If eai.Use Then
                            stb.Append(eai.GeneFile.Name)
                            stb.Append(" <")
                            stb.Append(eai.Region)
                            stb.Append(">  Appearace ")

                            Select Case eai.Method
                                Case EnzymeAnalysisEnum.Equal
                                    stb.Append("= ")

                                Case EnzymeAnalysisEnum.Greater
                                    stb.Append("> ")

                                Case EnzymeAnalysisEnum.Less
                                    stb.Append("< ")

                            End Select
                            stb.Append(eai.Value.ToString)
                            sList.Add(stb.ToString)
                        End If
                    Next
                    sList.Add("Enzyme Found: " + dnai.OperationDescription)
            End Select
            If MapID.Length > 0 Then title += " (Map File ID: " + MapID + ")"
            stb = New System.Text.StringBuilder
            For Each s As Nuctions.GeneFile In dnai.DNAs
                stb.Append(s.Length.ToString)
                stb.Append("bp ")
                stb.Append(IIf(s.Iscircular, "C; ", "L; "))
            Next
            sList.Add(stb.ToString)
            If di.MolecularOperation <> Nuctions.MolecularOperationEnum.EnzymeAnalysis Then
                descr = di.OperationDescription
            End If
        End Sub
        Dim R As Single
        Dim B As Single
        Dim X As Single
        Dim Y As Single

        Public Sub SaveGeneFile(ByVal Path As String)
            If vVector Is Nothing Then Exit Sub

            Dim regex As New System.Text.RegularExpressions.Regex("[\\/\:\*\?""<>\|]")
            Dim vName As String = dnai.Name
            vName = regex.Replace(vName, " ")
            If Not Path.EndsWith("\") Then Path += "\"
            vVector.GeneFile.WriteToFile(Path + MapID + " " + vName + ".gb")
        End Sub

        Public Sub Draw(ByVal g As Graphics, Optional ByVal redrawvector As Boolean = False)
            Dim vX As Single
            Dim vY As Single
            vX = X
            vY = Y
            dnai.DX = X
            dnai.DY = Y
            R = X
            Dim vS As SizeF
            Dim vstr As String

            If Not (vVector Is Nothing) Then
                'If redrawvector Then
                vVector.DrawImage(g, X, Y)
                'Else
                '    g.DrawImage(vVectorMap, New PointF(X, Y))
                'End If
                vY += vVector.Height
                R += vVector.Width
            End If
            vstr = title
            g.DrawString(vstr, tFont, Brushes.Black, vX, vY)
            vS = g.MeasureString(vstr, tFont)
            R = Math.Max(R, vX + vS.Width)
            vY += vS.Height
            For Each vstr In sList
                g.DrawString(vstr, vFont, Brushes.Black, vX, vY)
                vS = g.MeasureString(vstr, vFont)
                R = Math.Max(R, vX + vS.Width)
                vY += vS.Height
            Next

            If Not (descr Is Nothing) AndAlso descr.Length > 0 Then
                vstr = descr
                g.DrawString(vstr, vFont, Brushes.Red, vX, vY)
                vS = g.MeasureString(vstr, vFont)
                R = Math.Max(R, vX + vS.Width)
                vY += vS.Height
            End If

            g.DrawRectangle(Pens.Blue, X, Y, R - X, B - Y)
            B = vY
        End Sub

        Public Sub TryFindSource(ByVal si As SummaryItem)
            If dnai.Source.Contains(si.dnai) Then
                If Not scrList.Contains(si) Then scrList.Add(si)
            End If
        End Sub

        Public Sub DrawArrow(ByVal g As Graphics)

            Dim pnts As PointF()
            Dim vx As Vector2

            For Each si As SummaryItem In scrList
                vx = Center - si.Center
                pnts = GetArrow(si.GetPosition(vx), GetPosition(-vx), 0, 0)
                'grdBrush = New System.Drawing.Drawing2D.LinearGradientBrush(pnts(0), pnts(4), Color.Yellow, Color.Red)
                g.FillPolygon(Brushes.LightYellow, pnts)
                g.DrawPolygon(Pens.Gray, pnts)
            Next
        End Sub

 

        Public Function Hittest(ByVal vP As PointF) As Boolean
            Return vP.X > X And vP.X < R And vP.Y > Y And vP.Y < B
        End Function
        Private Shared Function GetArrow(ByVal V0 As Vector2, ByVal VE As Vector2, ByVal prefix As Single, ByVal suffix As Single) As PointF()
            Dim VX As Vector2 = VE - V0
            Dim VY As Vector2 = VX.GetBase.Turn(Math.PI / 2)
            Dim L As Single = VX.GetLength
            If L < (prefix + suffix + 24) Then
                prefix = (L - 24) / 2 + 8
                suffix = (L - 24) / 2 - 8
            End If
            VX = VX.GetBase
            Return New PointF() {V0 + VX * prefix, V0 + VX * prefix + VY * 6, VE - VX * (suffix + 12) + VY * 6, _
                                 VE - VX * (suffix + 12) + VY * 12, VE - VX * suffix, _
                                 VE - VX * (suffix + 12) - VY * 12, VE - VX * (suffix + 12) - VY * 6, V0 + VX * prefix - VY * 6}
        End Function
        Public Property Left() As Single
            Get
                Return X
            End Get
            Set(ByVal value As Single)
                Dim delta As Single = value - X

                X += delta
                R += delta
            End Set
        End Property
        Public ReadOnly Property Right() As Single
            Get
                Return R
            End Get
        End Property
        Public Property Top() As Single
            Get
                Return Y
            End Get
            Set(ByVal value As Single)
                Dim delta As Single = value - Y
                Y += delta
                B += delta
            End Set
        End Property
        Public ReadOnly Property Bottom() As Single
            Get
                Return B
            End Get
        End Property
        Public ReadOnly Property Center() As Vector2
            Get
                Return New Vector2((X + R) / 2, (Y + B) / 2)
            End Get
        End Property
        Public Function GetPosition(ByVal vt As Vector2) As Vector2
            Dim w As Single = (R - X) / 2
            Dim h As Single = (B - Y) / 2

            If vt.X = 0 And vt.Y = 0 Then
                Return New Vector2((X + R) / 2, B)
            ElseIf vt.Y = 0 Then
                Return New Vector2((X + R) / 2 + w * Math.Sign(vt.X), (Y + B) / 2)
            ElseIf vt.X = 0 Then
                Return New Vector2((X + R) / 2, (Y + B) / 2 + h * Math.Sign(vt.Y))
            Else
                If h * Math.Abs(vt.X) > w * Math.Abs(vt.Y) Then
                    Return New Vector2((X + R) / 2 + w * Math.Sign(vt.X), (Y + B) / 2)
                Else
                    Return New Vector2((X + R) / 2, (Y + B) / 2 + h * Math.Sign(vt.Y))
                End If
            End If
        End Function
        Public ReadOnly Property Source() As List(Of SummaryItem)
            Get
                Return scrList
            End Get
        End Property
        Public Sub AutoFit()
            If scrList.Count > 0 Then
                Dim X1 As Single = 0
                Dim Y1 As Single = Single.MinValue
                Dim W As Single = (R - X) / 2

                For Each si As SummaryItem In scrList
                    X1 += si.Center.X
                    Y1 = IIf(Y1 > si.Bottom, Y1, si.Bottom)
                Next

                Left = X1 / scrList.Count - W
                Top = Y1 + 40
            End If
        End Sub
    End Class

    Dim offsetX As Single
    Dim offsetY As Single
    Dim startX As Single
    Dim startY As Single
    Dim oldX As Single
    Dim oldY As Single
    Dim vScale As Single = 1
    Dim dragging As Boolean = False
    Dim moving As Boolean = False
    Dim draggingItem As SummaryItem
    'x2 = x1*scale + offset
    Dim relatedItems As New List(Of SummaryItem)

    Public Sub FindRelatedItems()
        Dim cnLevel As SummaryItem

        Dim ntLevel As New List(Of SummaryItem)

        relatedItems.Clear()

        cnLevel = draggingItem

        While Not (cnLevel Is Nothing)

            ntLevel.Clear()
            '再寻找下一层
            For Each si As SummaryItem In smList

                If si.Source.Count = 1 And si.Source.Contains(cnLevel) Then
                    ntLevel.Add(si)
                End If
            Next
            If ntLevel.Count <> 1 Then Exit While
            cnLevel = ntLevel(0)
            cnLevel.AutoFit()
            '排列下一层
            relatedItems.Add(cnLevel)
        End While
        Draw()
    End Sub

    Private Sub bpbMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                Dim si As SummaryItem
                Dim vp As PointF
                vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                For i As Integer = smList.Count - 1 To 0 Step -1
                    si = smList(i)
                    If si.Hittest(vp) Then
                        oldX = si.Left
                        oldY = si.Top
                        startX = vp.X
                        startY = vp.Y
                        dragging = True
                        draggingItem = si
                        If ModifierKeys = Keys.Shift Then
                            FindRelatedItems()
                        End If
                        Exit For
                    End If
                Next
                If Not dragging Then
                    vp = New PointF(e.X, e.Y)
                    oldX = offsetX
                    oldY = offsetY
                    startX = vp.X
                    startY = vp.Y
                    moving = True
                End If
            Case Windows.Forms.MouseButtons.Right
                Dim si As SummaryItem
                Dim vp As PointF
                vp = New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
                For i As Integer = smList.Count - 1 To 0 Step -1
                    si = smList(i)
                    If si.Hittest(vp) Then
                        draggingItem = si
                        cmsFigure.Show(bpbMain, e.Location)
                        Exit For
                    End If
                Next
        End Select
    End Sub

    Private Sub bpbMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseMove
        If dragging Then
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            draggingItem.Left = oldX + vp.X - startX
            draggingItem.Top = oldY + vp.Y - startY
            If ModifierKeys = Keys.Shift Then
                For Each si As SummaryItem In relatedItems
                    si.AutoFit()
                Next
            End If
            Draw()
        End If
        If moving Then
            Dim vp As New PointF(e.X, e.Y)
            offsetX = oldX + (vp.X - startX) / vScale
            offsetY = oldY + (vp.Y - startY) / vScale
            Draw()
        End If
    End Sub

    Private Sub bpbMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseUp
        If dragging Then
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            draggingItem.Left = oldX + vp.X - startX
            draggingItem.Top = oldY + vp.Y - startY
            If ModifierKeys = Keys.Shift Then
                For Each si As SummaryItem In relatedItems
                    si.AutoFit()
                Next
            End If
            Draw()
            dragging = False
        End If
        If moving Then
            Dim vp As New PointF(e.X, e.Y)
            offsetX = oldX + (vp.X - startX) / vScale
            offsetY = oldY + (vp.Y - startY) / vScale
            moving = False
            Draw()
        End If
    End Sub

    Private Sub frmSummary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.S
                If ModifierKeys = Keys.Control Then
                    SavePicture()
                End If
        End Select
    End Sub
    Public Sub SavePicture()
        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As SummaryItem In smList
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text
            If sfdPic.ShowDialog = Windows.Forms.DialogResult.OK Then


                Dim ofsX As Single = L - 10

                Dim ofsY As Single = T - 10

                Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)
                Dim vg As Graphics = Graphics.FromImage(bitmap)
                Dim hdc As IntPtr = vg.GetHdc
                Dim emf As New System.Drawing.Imaging.Metafile(sfdPic.FileName, hdc, Imaging.EmfType.EmfPlusDual)
                Dim g As Graphics = Graphics.FromImage(emf)
                g.Clear(Color.White)
                g.ResetTransform()
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.TranslateTransform(-ofsX, -ofsY)
                For Each si As SummaryItem In smList
                    si.Draw(g, True)
                Next
                For Each si As SummaryItem In smList
                    si.DrawArrow(g)
                Next
                g.Dispose()
                emf.Dispose()
                vg.ReleaseHdc()
                bitmap.Dispose()
            End If
        End If
    End Sub

    Public Sub SavePictureTo(ByVal vFilename As String)
        Dim L As Single
        Dim R As Single
        Dim T As Single
        Dim B As Single
        Dim NotSet As Boolean = True
        For Each si As SummaryItem In smList
            If NotSet Then
                L = si.Left
                R = si.Right
                T = si.Top
                B = si.Bottom
                NotSet = False
            Else
                L = IIf(L < si.Left, L, si.Left)
                T = IIf(T < si.Top, T, si.Top)
                R = IIf(R > si.Right, R, si.Right)
                B = IIf(B > si.Bottom, B, si.Bottom)
            End If
        Next

        If R - L > 0 And B - T > 0 Then
            sfdPic.FileName = Me.Text



            Dim ofsX As Single = L - 10

            Dim ofsY As Single = T - 10

            Dim bitmap As New Bitmap(R - L + 20, B - T + 20, Imaging.PixelFormat.Format32bppArgb)
             
            Dim vg As Graphics = Graphics.FromImage(bitmap)

            Dim hdc As IntPtr = vg.GetHdc

            'bitmap.GetHbitmap()
            Dim emf As New System.Drawing.Imaging.Metafile(vFilename, hdc, Imaging.EmfType.EmfPlusDual)
            Dim g As Graphics = Graphics.FromImage(emf)
            g.Clear(Color.White)
            g.ResetTransform()
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.TranslateTransform(-ofsX, -ofsY)
            For Each si As SummaryItem In smList
                si.DrawArrow(g)
            Next
            For Each si As SummaryItem In smList
                si.Draw(g, True)
            Next
            g.Dispose()

            emf.Dispose()
            vg.ReleaseHdc()
            bitmap.Dispose()

        End If
    End Sub

    Public Sub SaveFilesTo(ByVal DirectoryPath As String)
        For Each si As SummaryItem In smList
            si.SaveGeneFile(DirectoryPath)
        Next
    End Sub

    Private Sub bpbMain_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpbMain.MouseDoubleClick
        Dim si As SummaryItem
        For i As Integer = smList.Count - 1 To 0 Step -1
            si = smList(i)
            Dim vp As New PointF(e.X / vScale - offsetX, e.Y / vScale - offsetY)
            If si.Hittest(vp) Then
                si.AutoFit()
                dragging = False
                Draw()
                Exit For
            End If
        Next
    End Sub

    Private Sub bpbMain_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

        Dim os As Single = vScale
        vScale = vScale * (1.25) ^ (e.Delta / 120)
        If vScale > 10 Then vScale = 10
        If vScale < 0.01 Then vScale = 0.01
        offsetX += e.X / vScale - e.X / os
        offsetY += e.Y / vScale - e.Y / os
        Draw()
    End Sub

    Private Sub bpbMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles bpbMain.SizeChanged
        Draw()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SavePicture()
    End Sub

    Public Sub AutoArragne()

        Dim cnLevel As New List(Of SummaryItem)

        For Each si As SummaryItem In smList
            If si.Source.Count = 0 Then
                cnLevel.Add(si)
            End If
        Next

        '在顶层排列

        Dim vX As Single
        vX = 0
        Dim w As Single
        For Each si As SummaryItem In cnLevel
            w = si.Right - si.Left
            si.Left = vX
            si.Top = 0
            vX += w + 100
        Next

        '寻找下一层
        Dim ntLevel As New List(Of SummaryItem)

        For Each si As SummaryItem In smList
            For Each ci As SummaryItem In cnLevel
                If si.Source.Contains(ci) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0


            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As SummaryItem In cnLevel
                si.AutoFit()
            Next

            '再寻找下一层
            For Each si As SummaryItem In smList
                For Each ci As SummaryItem In cnLevel
                    If si.Source.Contains(ci) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
        End While
        Draw()
    End Sub

    Private Sub AutoArragneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoArragneToolStripMenuItem.Click
        AutoArragne()
    End Sub
    Public Sub AutoFitChildren()
        Dim cnLevel As New List(Of SummaryItem)
        cnLevel.Add(draggingItem)

        Dim ntLevel As New List(Of SummaryItem)

        For Each si As SummaryItem In smList
            For Each ci As SummaryItem In cnLevel
                If si.Source.Contains(ci) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0


            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As SummaryItem In cnLevel
                si.AutoFit()
            Next

            '再寻找下一层
            For Each si As SummaryItem In smList
                For Each ci As SummaryItem In cnLevel
                    If si.Source.Contains(ci) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
        End While
        Draw()
    End Sub

    Public Sub FitChildrenStepByStep()
        Dim cnLevel As New List(Of SummaryItem)
        cnLevel.Add(draggingItem)

        Dim ntLevel As New List(Of SummaryItem)

        For Each si As SummaryItem In smList
            For Each ci As SummaryItem In cnLevel
                If si.Source.Contains(ci) Then
                    ntLevel.Add(si)
                    Exit For
                End If
            Next
        Next

        While ntLevel.Count > 0

            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each si As SummaryItem In cnLevel
                si.AutoFit()
            Next

            If MsgBox("Continue Next Level?", MsgBoxStyle.YesNo, "Fit Children Step by Step") = MsgBoxResult.No Then Exit While

            '再寻找下一层
            For Each si As SummaryItem In smList
                For Each ci As SummaryItem In cnLevel
                    If si.Source.Contains(ci) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
            Draw()
        End While
    End Sub

    Private Sub AutoFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoFitChildrenToolStripMenuItem.Click
        AutoFitChildren()
    End Sub

    Private Sub StepFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StepFitChildrenToolStripMenuItem.Click
        FitChildrenStepByStep()
    End Sub
End Class