Friend Class WorkControl
    Public FeatureCol As New List(Of Nuctions.Feature)
    'Public EnzymeCol As New List(Of Nuctions.RestrictionEnzyme)
    Public EnzymeCol As New List(Of String)
    '保存自己所属的Tab页
    Private vTabPage As TabPage

    '返回自己所属的Tab页
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property ParentTab() As TabPage
        Get
            Return vTabPage
        End Get
        Set(ByVal value As TabPage)
            vTabPage = value
        End Set
    End Property

    '双击之后打开文件
    Private Sub lv_Chart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If lv_Chart.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = lv_Chart.SelectedItems(0)
            If Form.ModifierKeys = Keys.Alt Then
                'view the property
                'ShowProperties(Me.lv_Chart.SelectedItems(0))
            Else
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                    gf.WriteToFile("Temp.gb")
                    Nuctions.ShowGBFile("Temp.gb")
                Else
                    'view the property
                    'ShowProperties(Me.lv_Chart.SelectedItems(0))
                End If
            End If
        End If
    End Sub

    Private Sub ViewVToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewVToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = lv_Chart.SelectedItems(0)
            Dim DNA As Nuctions.GeneFile
            Dim i As Integer = 0
            For Each DNA In ci.MolecularInfo.DNAs
                SettingEntry.AddDNAViewTab(DNA, EnzymeCol)
                i += 1
            Next
        End If
    End Sub
#Region "IO操作"
    '获得文件记录
    Public Function GetWorkSpace() As WorkSpace
        Dim WS As New WorkSpace
        Dim Citems As New List(Of DNAInfo)
        For Each o As ChartItem In lv_Chart.Items
            Citems.Add(o.MolecularInfo)
        Next

        WS.Scale = lv_Chart.ScaleValue
        WS.OffsetX = lv_Chart.Offset.X
        WS.OffsetY = lv_Chart.Offset.Y

        WS.ChartItems.AddRange(Citems)

        WS.Features.AddRange(FeatureCol)
        Dim cenzs As New List(Of String)
        For Each enz As String In EnzymeCol
            cenzs.Add(enz)
        Next
        WS.Enzymes.AddRange(cenzs)

        WS.Summary = pvMain.rtbSummary.Text
        WS.PrintPages = lv_Chart.PrintPages
        WS.PrintView = lv_Chart.PrintView
        WS.PrimerList = lv_Chart.Primers
        WS.Hosts = lv_Chart.Hosts
        WS.Published = Published
        WS.PublicationID = PublicationID
        WS.Quoted = Quoted
        WS.QuotationID = QuotationID
        WS.ProjectServiceStatus = ProjectServiceStatus
        Return WS
    End Function
    '保存实验到指定文件
    Public Sub SaveTo(ByVal filename As String)
#If ReaderMode = 0 Then
        Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", GetWorkSpace()}}

        SettingEntry.SaveToZXML(DC, filename)

        Dim fi As New IO.FileInfo(filename)

        ParentTab.Text = fi.Name
        Changed = False
        FileAddress = filename
#End If
    End Sub

    '保存成为字节流
    Public Function SaveToBytes() As Byte()
        Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", GetWorkSpace()}}
        Return SettingEntry.SaveToZXMLBytes(DC)
    End Function
    Private vFileAddress As String = ""
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Friend Property FileAddress As String
        Get
            Return vFileAddress
        End Get
        Set(ByVal value As String)
            pvMain.lbFileAddress.Text = String.Format("[File Location] - {0}", value)
            vFileAddress = value
        End Set
    End Property

    '在远程保存的过程中将最先尝试这个密码组合
    Friend RemoteUserName As String = ""
    Friend RemotePassword As String = ""
    '标记当前的保存中这个账户和密码是否有效
    Friend AccountWorking As Boolean = False
    Friend ProjectServiceStatus As ProjectServiceStatusEnum = ProjectServiceStatusEnum.None
    '配置工作区文件
    Public Shared Function SetWorkControl(ByVal WS As WorkSpace, ByVal filename As String, Optional ByVal vUserName As String = "", Optional ByVal vPassword As String = "") As WorkControl
        Dim wc As New WorkControl
        wc.QuotationID = WS.QuotationID
        wc.PublicationID = WS.PublicationID
        wc.Published = WS.Published
        wc.ProjectServiceStatus = WS.ProjectServiceStatus
        wc.Quoted = WS.Quoted

        wc.FileAddress = filename
#If Remote = 1 Then
        MsgBox(filename, MsgBoxStyle.OkOnly)
#End If
        '这个两个属性是为了远程保存而使用的 平时根本用不到的
        '保存的时候 先检测文件名是不是带有tcp样式
        wc.RemoteUserName = vUserName
#If Remote = 1 Then
        MsgBox(vUserName, MsgBoxStyle.OkOnly)
#End If
        wc.RemotePassword = vPassword
#If Remote = 1 Then
        MsgBox(vPassword, MsgBoxStyle.OkOnly)
#End If
        wc.lv_Chart.ScaleValue = WS.Scale
#If Remote = 1 Then
        MsgBox(WS.Scale.ToString, MsgBoxStyle.OkOnly)
#End If
        wc.lv_Chart.Offset = New PointF(WS.OffsetX, WS.OffsetY)
#If Remote = 1 Then
        MsgBox(wc.lv_Chart.Offset.ToString, MsgBoxStyle.OkOnly)
#End If
        wc.Text = ParseLevel2Name(wc.FileAddress)
#If Remote = 1 Then
        MsgBox(wc.Text, MsgBoxStyle.OkOnly)
#End If
        wc.FeatureCol = WS.Features
#If Remote = 1 Then
        MsgBox("Features = " + WS.Features.Count.ToString, MsgBoxStyle.OkOnly)
#End If
        wc.EnzymeCol = WS.Enzymes
#If Remote = 1 Then
        MsgBox("Enzymes = " + WS.Enzymes.Count.ToString, MsgBoxStyle.OkOnly)
#End If
        '错误出在了这一步 为什么呢？
        wc.lv_Chart.LoadSummary(WS.ChartItems, wc.EnzymeCol, wc.FeatureCol)
#If Remote = 1 Then
        MsgBox("Load Summary", MsgBoxStyle.OkOnly)
#End If
        wc.pvMain.rtbSummary.Text = WS.Summary
#If Remote = 1 Then
        MsgBox("Summary" + ControlChars.NewLine + wc.pvMain.rtbSummary.Text, MsgBoxStyle.OkOnly)
#End If
        wc.pvMain.ShowEnzymes()
#If Remote = 1 Then
        MsgBox("ShowEnzymes", MsgBoxStyle.OkOnly)
#End If
        wc.lv_Chart.PrintView = WS.PrintView
        wc.lv_Chart.PrintPages = WS.PrintPages
        wc.lv_Chart.Primers = WS.PrimerList
        If WS.Hosts Is Nothing Then
            WS.Hosts = New List(Of Nuctions.Host)
            WS.Hosts.Add(New Nuctions.Host With {.Name = "in vitro", .Description = ""})
            WS.Hosts.Add(New Nuctions.Host With {.Name = "in vivo", .Description = ""})
        End If
        wc.lv_Chart.Hosts = WS.Hosts
#If Remote = 1 Then
        MsgBox("PrintPages", MsgBoxStyle.OkOnly)
#End If
#If Remote = 1 Then
        MsgBox("SetWorkControl Done!", MsgBoxStyle.OkOnly)
#End If
        Return wc
    End Function

    '从指定文件中加载实验
    Public Shared Function LoadFrom(ByVal filename As String) As WorkControl
        'do not load a file if there are already files.
        'filename tcp://127.0.0.1/1/Vectors/pBluescript.vct

        Dim vList As New List(Of String) From {"WorkChart"}
        Dim WS As WorkSpace = SettingEntry.LoadFromZXML(vList, filename)("WorkChart")
        If WS Is Nothing Then Return Nothing
        Return SetWorkControl(WS, filename)
    End Function
    Public Shared Function LoadFrom(ByVal buf As Byte(), ByVal filename As String, Optional ByVal vUserName As String = "", Optional ByVal vPassword As String = "") As WorkControl
        'do not load a file if there are already files.
        'filename tcp://127.0.0.1/1/Vectors/pBluescript.vct

#If Remote = 1 Then
        Try
#End If
        Dim vList As New List(Of String) From {"WorkChart"}
        Dim WS As WorkSpace = SettingEntry.LoadFromZXMLBytes(vList, buf)("WorkChart")
        If WS Is Nothing Then Return Nothing
#If Remote = 1 Then
            If WS Is Nothing Then
                MsgBox("Read File Error", MsgBoxStyle.OkOnly)
            Else
                MsgBox("Read File " + WS.ChartItems.Count.ToString, MsgBoxStyle.OkOnly)
            End If
#End If
        Return SetWorkControl(WS, filename, vUserName, vPassword)
#If Remote = 1 Then
        Catch ex As Exception
            MsgBox("Read File Failure", MsgBoxStyle.OkOnly)
        End Try
#End If
    End Function

    Public ReadOnly Property ContentChanged() As Boolean
        Get
            Return Changed
        End Get
    End Property
#End Region
    Private Sub WorkChart_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        Select Case MsgBox("Do you want save before closing the current file?", MsgBoxStyle.YesNoCancel, "File Closing")
            Case MsgBoxResult.Yes
                If FileAddress.Length = 0 Then
                    If sfdProject.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Me.SaveTo(sfdProject.FileName)
                    End If
                Else
                    SaveTo(FileAddress)
                End If
            Case MsgBoxResult.No
                'the file will be lost
            Case MsgBoxResult.Cancel
                e.Cancel = True
        End Select
    End Sub


    Public Sub DeleteItem(Optional ByVal Query As Boolean = True)

        If Query Then
            If MsgBox("Do You Really Want to Delete All the Selected Items?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                lv_Chart.RemoveItems(lv_Chart.SelectedItems)
            Else
                Exit Sub
            End If
        End If

        'Dim ci As ChartItem
        'Dim delCol As New List(Of ChartItem)
        'For Each ci In Me.lv_Chart.SelectedItems
        '    delCol.Add(ci)
        'Next
        'For Each ci In delCol
        '    lv_Chart.Remove(ci)
        'Next
        'lv_Chart.SelectedItems.Clear()

    End Sub

#Region "拖拽操作"

    Dim dragging As Boolean = False

    Private Sub lv_Chart_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            dragging = True
            cX = e.X
            cY = e.Y
        End If
    End Sub

    Dim dX As Integer, dY As Integer 'start position
    Dim cX As Integer, cY As Integer

    'Private Sub lv_Chart_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    If Not dragging Then Exit Sub
    '    If Me.lv_Chart.SelectedItems.Count > 0 Then
    '        Dim ci As ChartItem
    '        dX = e.X - cX
    '        dY = e.Y - cY
    '        cX = e.X
    '        cY = e.Y
    '        For Each ci In Me.lv_Chart.SelectedItems
    '            ci.Position = New Point(ci.Position.X + dX, ci.Position.Y + dY)
    '        Next
    '        SavePositions()
    '    End If
    'End Sub

    Private MenuLocation As Point
    'Private Sub lv_Chart_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    If dragging Then
    '        dragging = False
    '        SavePositions()
    '    Else

    '    End If
    '    If e.Button = Windows.Forms.MouseButtons.Right Then
    '        MenuLocation = e.Location
    '        'cms_ChartItem.Show(Me, e.Location)
    '    End If
    'End Sub

#End Region
    'Public OffsetO As Point = New Point(0, 0)
#Region "ListView坐标和绘图"


    '每次移动之后 记录所有项的位置 以及屏幕的offset
#End Region

    Private frmRecord As New frmExpRecord

    Private Sub lv_Chart_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.PositionChanged
        Changed = True
    End Sub


    '加载文件时绘图


    '选中操作
    Private Sub lv_Chart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.SelectedIndexChanged
        If lv_Chart.SourceMode Then
            vTarget.Source.Clear()
            For Each ci As ChartItem In lv_Chart.SelectedItems
                vTarget.Source.Add(ci.MolecularInfo)
            Next
            lv_Chart.Draw()
        Else
            Dim sitems As New List(Of ChartItem)

            For Each it As ChartItem In lv_Chart.SelectedItems
                sitems.Add(it)
            Next
            pvMain.SelectItem = sitems
            lv_Chart.Focus()
        End If

    End Sub
#Region "打印页面处理"
    Private Sub pvMain_AddPrintPage(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.AddPrintPage
        AddPrintPage()
    End Sub

    Private Sub pvMain_DirectPrintAllPages(sender As Object, e As System.EventArgs) Handles pvMain.DirectPrintAllPages
        lv_Chart.PrintPage(lv_Chart.PrintPages, True)
    End Sub

    Private Sub pvMain_DirectPrintSelectedPages(sender As Object, e As System.EventArgs) Handles pvMain.DirectPrintSelectedPages
        lv_Chart.PrintPage(lv_Chart.SelectedPrintPages, True)
    End Sub
    Private Sub pvMain_PrintAllPages(sender As Object, e As System.EventArgs) Handles pvMain.PrintAllPages
        lv_Chart.PrintPage(lv_Chart.PrintPages)
    End Sub
    Private Sub pvMain_PrintSelectedPages(sender As Object, e As System.EventArgs) Handles pvMain.PrintSelectedPages
        lv_Chart.PrintPage(lv_Chart.SelectedPrintPages)
    End Sub
    Private Sub pvMain_DeleteSelectedPages(sender As Object, e As System.EventArgs) Handles pvMain.DeleteSeletedPages
        lv_Chart.DeletePage(lv_Chart.SelectedPrintPages)
    End Sub
    Public Sub AddPrintPage()
        If lv_Chart.PrintView Then '只在打印模式下才能添加打印页面
            Dim pp As New PrintPage
            pp.Text = "New Page"
            pp.PageID = lv_Chart.PrintPages.Count + 1
            'pp.Left = lv_Chart.Offset.X
            'pp.Top = lv_Chart.Offset.Y
            pp.Width = pp.PrintPixelWidth
            pp.Height = pp.PrintPixelHeight
            pp.Left = (lv_Chart.Width / 2.0F) / lv_Chart.ScaleValue - lv_Chart.Offset.X - pp.Width / 2.0F
            pp.Top = (lv_Chart.Height / 2.0F) / lv_Chart.ScaleValue - lv_Chart.Offset.Y - pp.Height / 2.0F
            lv_Chart.PrintPages.Add(pp)
            lv_Chart.SelectPrintPage(pp)
            lv_Chart.Draw()
        End If
    End Sub
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property PrintMode As Boolean
        Get
            Return lv_Chart.PrintView
        End Get
        Set(ByVal value As Boolean)
            lv_Chart.PrintView = value
            lv_Chart.Draw()
            If value Then pvMain.SetPrintPageView(value)
        End Set
    End Property
    Public Sub Print()

    End Sub
    Private Sub lv_Chart_SelectedPrintPageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.SelectedPrintPageChanged
        pvMain.SelectPage = lv_Chart.SelectedPrintPages
    End Sub

#End Region
#Region "PropertyView 指令事件"



    Private Sub pvMain_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.Close
        Close()
    End Sub

    Private Sub pvMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.Load

    End Sub

    Private Sub pvMain_LoadExperiment(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadExperiment
        LoadExperiment()
    End Sub
    Private Sub pvMain_LoadGeneFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadGeneFile
        LoadVector()
    End Sub
    Private Sub pvMain_LoadSequenceFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadSequenceFile

        If ofdSequence.ShowDialog = DialogResult.OK Then
            LoadSequenceFromFile(ofdSequence.FileName)
        End If
    End Sub

    Private Sub pvMain_LoadSequencingResultFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadSequencingResultFile
        If ofdSequencingResult.ShowDialog = DialogResult.OK Then
            For Each f In ofdSequencingResult.FileNames
                LoadSequencingResultFromFile(f)
            Next
        End If
    End Sub
    Private Sub pvMain_ManageEnzymes(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles pvMain.ManageEnzymes
        EnzymeCol = e.RESites
        pvMain.ShowEnzymes()
        lv_Chart.SetEnzymes(EnzymeCol)

    End Sub

    Private Sub pvMain_ManageFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles pvMain.ManageFeatures
        Me.FeatureCol.Clear()
        For Each ft As Nuctions.Feature In e.Features
            Me.FeatureCol.Add(ft)
        Next
    End Sub

    Private Sub pvMain_ReqireFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles pvMain.ReqireFeatures
        Dim ftList As New List(Of Nuctions.Feature)
        For Each ft As Nuctions.Feature In FeatureCol
            ftList.Add(ft)
        Next
        e.Features = ftList
    End Sub

    Private Sub pvMain_RequireEnzymeSite(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles pvMain.RequireEnzymeSite
        e.RESites = EnzymeCol
    End Sub

    Private Sub pvMain_SaveExperiment(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.SaveExperiment
        Save()
    End Sub

    Private Sub pvMain_SaveExperimentAs(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.SaveExperimentAs
        SaveAs()
    End Sub
#End Region

#Region "功能指令"
    Public Event CloseWorkControl(ByVal sender As Object, ByVal e As EventArgs)
    Public ReadOnly Property CurrentFileName() As String
        Get
            Return FileAddress
        End Get
    End Property
    Public ReadOnly Property ProjectName As String
        Get
            If vFileAddress Is Nothing OrElse vFileAddress.Length = 0 OrElse Not IO.File.Exists(vFileAddress) Then Return ""
            Dim fi As New IO.FileInfo(vFileAddress)
            Return fi.Name.Substring(0, fi.Name.LastIndexOf("."))
        End Get
    End Property

    Public Sub Close()
        '询问关闭前是否保存
        RaiseEvent CloseWorkControl(Me, New EventArgs)

    End Sub
    Public Event LoadWorkControl(ByVal sender As Object, ByVal e As EventArgs)
    '加载文件
    Public Sub LoadExperiment()
        RaiseEvent LoadWorkControl(Me, New EventArgs)
    End Sub
    '保存文件
    Public Sub Save()
#If ReaderMode = 0 Then
        If FileAddress Is Nothing OrElse FileAddress = "" Then
            SaveAs()
        ElseIf FileAddress.StartsWith("tcp:\\") Then
            'tcp://user#
            SaveToServer()
        Else
            SaveTo(FileAddress)
        End If
#End If
    End Sub
    '另存文件
    Public Sub SaveAs()
#If ReaderMode = 0 Then
        If ReaderMode Then Exit Sub
        If sfdProject.ShowDialog = DialogResult.OK Then
            SaveTo(sfdProject.FileName)
        End If
#End If
    End Sub

    Public Sub SaveToServer()
#If ReaderMode = 0 Then
        If ReaderMode Then Exit Sub
        If RemoteUserName Is Nothing OrElse RemoteUserName.Length = 0 OrElse RemotePassword Is Nothing OrElse RemotePassword.Length = 0 Then
            Stop
            'frmMain.SaveWorkControlAsToServer(Me, ParentTab.Text)
        Else
            Stop
            'frmMain.SaveWorkControlToServer(Me)
        End If
#End If
    End Sub

    '加载一个GeneBank文件
    Public Sub LoadVector()
        If Not ofdGeneFile.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub
        LoadVectorFromFile(ofdGeneFile.FileName)
    End Sub
    Public Sub LoadFeature(ByVal gf As Nuctions.GeneFile)
        Dim ft As Nuctions.Feature
        Dim exist As Boolean
        For Each gn As Nuctions.GeneAnnotation In gf.Features
            'Add the annotation to the collection so that we can store the features
            'The features are useful in the ligation and screen
            If gn.Type = "f_end" OrElse gn.Type = "r_end" Then Continue For
            gn.Vector = gf
            ft = New Nuctions.Feature(gn.Label, gn.GetSuqence, gn.Label, gn.Type, gn.Note)
            If gn.Feature IsNot Nothing Then
                For Each bf As Nuctions.FeatureFunction In gn.Feature.BioFunctions
                    ft.BioFunctions.Add(bf.Clone)
                Next
            End If
            exist = False
            For Each fta As Nuctions.Feature In FeatureCol
                If fta.Sequence = ft.Sequence Then
                    exist = True
                    Exit For
                End If
            Next
            If Not exist Then
                FeatureCol.Add(ft)
                gn.Feature = ft
            End If
        Next
    End Sub
    Public Sub LoadVectorFromFile(ByVal sFilename As String)
        Dim vec As Nuctions.GeneFile
        If sFilename.ToLower.EndsWith(".gb") Then
            vec = Nuctions.GeneFile.LoadFromGeneBankFile(sFilename)
        ElseIf sFilename.EndsWith(".vct") Then
            Dim dict As Dictionary(Of String, Object) = SettingEntry.LoadFromZXML(New List(Of String) From {"DNA", "Enzyme"}, sFilename)
            vec = dict("DNA")
        End If
        If vec Is Nothing Then Return
        Dim ci As New DNAInfo
        LoadFeature(vec)
        ci.DNAs.Add(vec)
        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
        Dim fi As New IO.FileInfo(sFilename)

        ci.Name = fi.Name.Substring(0, fi.Name.LastIndexOf("."))
        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
        ci.File_Filename = sFilename
        'ci.MolecularInfo.FeatureCol = FeatureCol
        ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
        ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
        lv_Chart.Add(ci, EnzymeCol)
        lv_Chart.Draw()
        Changed = True
    End Sub
    Public Sub LoadVectorFile(ByVal vec As Nuctions.GeneFile, Optional ByVal sFilename As String = "")
        Dim ci As New DNAInfo
        LoadFeature(vec)
        ci.DNAs.Add(vec)
        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
        ci.Name = vec.Name
        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
        ci.File_Filename = sFilename
        'ci.MolecularInfo.FeatureCol = FeatureCol
        ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
        ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
        lv_Chart.Add(ci, EnzymeCol)
        lv_Chart.Draw()
        Changed = True
    End Sub
    Public Sub LoadSequenceFromFile(ByVal sFilename As String)
        Dim str As String = Nuctions.TAGCFilter(IO.File.ReadAllText(sFilename))
        Dim fi As New System.IO.FileInfo(sFilename)
        LoadSequence(str, fi.Name, sFilename)
    End Sub
    Public Sub LoadSequencingResultFromFile(sFilename As String)
        Dim fi As New System.IO.FileInfo(sFilename)
        Dim ab1 As New AB1Data
        Dim bytes = IO.File.ReadAllBytes(sFilename)
        ab1.Read(bytes)
        Dim vec As New Nuctions.GeneFile With {.Sequence = ab1.Sequence, .End_F = "*B"， .End_R = "*B"， .Name = fi.Name}
        Dim ci As New DNAInfo With {.Name = fi.Name}
        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
        ci.File_Filename = sFilename
        Dim im = New SequenceItem With {.Data = bytes, .FileType = SequencingFileTypeEnum.AB1, .Name = fi.Name, .RawData = ab1.GetRawData}
        ci.SCFFiles.Add(im)
        ci.DNAs.Add(vec)
        Nuctions.AddFeatures(ci.DNAs, FeatureCol)
        ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
        ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
        lv_Chart.Add(ci, EnzymeCol)
        lv_Chart.Draw()
        Changed = True
    End Sub

    Public Sub LoadSequence(ByVal seq As String, ByVal vName As String, Optional ByVal sFilename As String = "")
        Dim vec As New Nuctions.GeneFile
        vec.Name = vName
        vec.Sequence = seq
        vec.End_F = "*B"
        vec.End_R = "*B"
        Dim ci As New DNAInfo
        ci.DNAs.Add(vec)
        If vName.ToLower.EndsWith(".seq") Or vName.ToLower.EndsWith(".txt") Then
            Nuctions.AddFeatures(ci.DNAs, FeatureCol)
        End If
        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
        ci.Name = vec.Name
        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
        ci.File_Filename = sFilename
        'ci.MolecularInfo.FeatureCol = FeatureCol
        ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
        ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
        lv_Chart.Add(ci, EnzymeCol)
        lv_Chart.Draw()
        Changed = True
    End Sub
    Private Changed As Boolean = False

    Public Function TryCopyDNAs() As List(Of Nuctions.GeneFile)
#If ReaderMode = 0 Then
        Dim glist As New List(Of Nuctions.GeneFile)
        For Each si As ChartItem In lv_Chart.SelectedItems
            If si.MolecularInfo.DNAs.Count = 1 Then
                glist.Add(si.MolecularInfo.DNAs(1))
            End If
        Next
        Return glist
#Else
        Return New List(Of Nuctions.GeneFile)
#End If
    End Function
    Public Sub TryPasteDNAs(ByVal gList As List(Of Nuctions.GeneFile))
#If ReaderMode = 0 Then
        For Each gf As Nuctions.GeneFile In gList
            'vec.Iscircular = False
            Dim ci As New DNAInfo
            ci.DNAs.Add(gf)
            'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
            ci.Name = gf.Name
            ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
            ci.File_Filename = "<None>"
            'ci.MolecularInfo.FeatureCol = FeatureCol
            ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
            ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
            lv_Chart.Add(ci, EnzymeCol)
        Next
        lv_Chart.Draw()
        Changed = True
#End If
    End Sub
#End Region

    '处理右键菜单
    Private Sub FunctionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeDigestionEToolStripMenuItem.Click,
        GelSeparationGToolStripMenuItem.Click, LigationLToolStripMenuItem.Click, ModificationMToolStripMenuItem.Click,
        PCRRToolStripMenuItem.Click, TransformationScreenTToolStripMenuItem.Click, RecombinationCToolStripMenuItem.Click,
        EnzymeAnalysisAToolStripMenuItem.Click, FreeDesignFToolStripMenuItem.Click, SequencingSToolStripMenuItem.Click, CompareToolStripMenuItem.Click
        AddNewOperation(sender.tag)
    End Sub

    Public Function AddNewOperation(ByVal Method As Nuctions.MolecularOperationEnum, Optional ByVal AutoPosition As Boolean = False) As ChartItem
#If ReaderMode = 0 Then
        If Me.lv_Chart.SelectedItems.Count = 0 And Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then Return Nothing
        Dim ci As New DNAInfo
        Dim ui As New ChartItem
        If Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then
            For Each ui In Me.lv_Chart.SelectedItems
                ci.Source.Add(ui.MolecularInfo)
            Next
        End If
        ci.MolecularOperation = Method
        Select Case ci.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Enzyme
                ci.Name = "Digest"
            Case Nuctions.MolecularOperationEnum.Gel
                ci.Name = "Gel Separate"
            Case Nuctions.MolecularOperationEnum.Modify
                ci.Name = "Modify"
            Case Nuctions.MolecularOperationEnum.PCR
                ci.Name = "PCR"
            Case Nuctions.MolecularOperationEnum.Screen
                ci.Name = "Screen"
            Case Nuctions.MolecularOperationEnum.Recombination
                ci.Name = "Recombination"
            Case Nuctions.MolecularOperationEnum.EnzymeAnalysis
                ci.Name = "Analysis"
            Case Nuctions.MolecularOperationEnum.Ligation
                ci.Name = "Ligation"
            Case Nuctions.MolecularOperationEnum.FreeDesign
                ci.Name = "Free Design"
            Case Nuctions.MolecularOperationEnum.HashPicker
                ci.Name = "Hash Picker"
            Case Nuctions.MolecularOperationEnum.Host
                ci.Name = "Bacteria Host"
            Case Nuctions.MolecularOperationEnum.Transformation
                ci.Name = "Transformation"
            Case Nuctions.MolecularOperationEnum.Incubation
                ci.Name = "Incubation"
            Case Nuctions.MolecularOperationEnum.Extraction
                ci.Name = "Extraction"
            Case Nuctions.MolecularOperationEnum.Expression
                ci.Name = "Expression"
            Case Nuctions.MolecularOperationEnum.GibsonDesign
                ci.Name = "Gibson Design"
            Case Nuctions.MolecularOperationEnum.CRISPRCut
                ci.Name = "CRISPR Cut"
        End Select

        ci.DX = lv_Chart.MenuLocation.X
        ci.DY = lv_Chart.MenuLocation.Y
        Dim ch As ChartItem
        If AutoPosition Then
            If ci.Source.Count > 0 Then
                ch = Me.lv_Chart.Add(ci, EnzymeCol)
                ch.Draw(Graphics.FromImage(New Bitmap(1, 1, Imaging.PixelFormat.Format32bppArgb)))
                ch.AutoFit()
            Else
                ci.DX = (lv_Chart.Width / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.X
                ci.DY = (lv_Chart.Height / 2) / lv_Chart.ScaleValue - lv_Chart.Offset.Y
                ch = Me.lv_Chart.Add(ci, EnzymeCol)
            End If
        Else
            ch = Me.lv_Chart.Add(ci, EnzymeCol)
        End If
        Me.lv_Chart.SelectedItems.Clear()
        ch.Selected = True

        '几种简单的修饰方法是直接计算的
        Select Case ci.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Gel
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
            Case Nuctions.MolecularOperationEnum.Modify
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
            Case Nuctions.MolecularOperationEnum.Host
                ci.DetermineHost(lv_Chart.Hosts)
            Case Nuctions.MolecularOperationEnum.Transformation
                ci.DetermineHost(lv_Chart.Hosts)
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
            Case Nuctions.MolecularOperationEnum.Incubation
                ci.DetermineHost(lv_Chart.Hosts)
            Case Nuctions.MolecularOperationEnum.Extraction
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
        End Select

        '表明已经修改过文件
        Changed = True

        lv_Chart.Draw()
        Return ch
#Else
        Return Nothing
#End If
    End Function

    Public Function CopySelectedSequence() As String
        Return pvMain.CopySelectSequence
    End Function

    Private Sub pvMain_LoadSequenceEvent(ByVal sender As Object, ByVal e As LoadSequenceEventArgs) Handles pvMain.LoadSequenceEvent
        LoadSequence(e.Sequence, "Sequence " + e.Sequence.Length.ToString)
    End Sub

    Private Sub GenerateSummary(ByVal stb As System.Text.StringBuilder)
        Dim l As Integer = lv_Chart.Items.Count.ToString.Length


        stb.AppendLine("-------- Project Information --------")
        stb.Append(pvMain.rtbSummary.Text.Replace(ControlChars.Lf, ControlChars.NewLine))
        stb.AppendLine()
        stb.AppendLine()


        stb.AppendLine("-------- Starting Materials --------")

        stb.AppendLine("Plasmids:")
        Dim plList As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Vector Then
                If Not plList.Contains(ci.MolecularInfo.Name) Then plList.Add(ci.MolecularInfo.Name)
                stb.Append(ci.Index.ToString.PadLeft(l, "0") + " " + ci.MolecularInfo.Name)
                stb.AppendLine(":")
                If Not (ci.MolecularInfo.OperationDescription Is Nothing) Then
                    stb.AppendLine(ci.MolecularInfo.OperationDescription.Replace(ControlChars.Lf, ControlChars.NewLine))
                Else
                    stb.AppendLine("N/A")
                End If

                stb.AppendLine()
            End If
        Next
        stb.AppendLine("Strains:")
        Dim stList As New List(Of String)
        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Host Then
                If ci.MolecularInfo.Cells.Count > 0 Then
                    If Not stList.Contains(ci.MolecularInfo.Cells(0).Host.Name) Then stList.Add(ci.MolecularInfo.Cells(0).Host.Name)
                    stb.Append(ci.Index.ToString.PadLeft(l, "0") + " " + ci.MolecularInfo.Cells(0).Host.Name)
                    stb.AppendLine(":")
                    If Not (ci.MolecularInfo.OperationDescription Is Nothing) Then
                        stb.AppendLine(ci.MolecularInfo.OperationDescription.Replace(ControlChars.Lf, ControlChars.NewLine))
                    Else
                        stb.AppendLine("N/A")
                    End If
                End If
                stb.AppendLine()
            End If
        Next

        stb.AppendLine("-------- Enzyme Information --------")


        Dim ezlist As New List(Of String)
        Dim tlList As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            Select Case ci.MolecularInfo.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Enzyme
                    For Each ez As String In ci.MolecularInfo.Enzyme_Enzymes
                        If Not ezlist.Contains(ez) Then
                            ezlist.Add(ez)
                        End If
                    Next
                    If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                        If Not tlList.Contains("CIAP") Then tlList.Add("CIAP")
                    End If
                Case Nuctions.MolecularOperationEnum.Ligation
                    'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                    If Not tlList.Contains("T4 DNA Ligase") Then tlList.Add("T4 DNA Ligase")
                'End If
                Case Nuctions.MolecularOperationEnum.Modify
                    Select Case ci.MolecularInfo.Modify_Method
                        Case Nuctions.ModificationMethodEnum.CIAP
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Calf Intestinal Alkaline Phosphatase") Then tlList.Add("Calf Intestinal Alkaline Phosphatase")
                        'End If
                        Case Nuctions.ModificationMethodEnum.Klewnow
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Klewnow Fragment") Then tlList.Add("Klewnow Fragment")
                        'End If
                        Case Nuctions.ModificationMethodEnum.PNK
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Polynucleatide Kinase") Then tlList.Add("Polynucleatide Kinase")
                        'End If
                        Case Nuctions.ModificationMethodEnum.T4DNAP
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("T4 DNA Polymerase") Then tlList.Add("T4 DNA Polymerase")
                            'End If
                    End Select
                Case Nuctions.MolecularOperationEnum.Recombination
                    If Not ci.MolecularInfo.IsCellNode Then
                        Select Case ci.MolecularInfo.RecombinationMethod
                            Case RecombinationMethod.FRT
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("FLP Recombinase") Then tlList.Add("FLP Recombinase")
                            'End If
                            Case RecombinationMethod.LoxP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("Cre Recombinase") Then tlList.Add("Cre Recombinase")
                            'End If
                            Case RecombinationMethod.LambdaAttBP
                                ' If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("λ Int, λ Xis and IHF") Then tlList.Add("λ Int, λ Xis and IHF")
                            'End If
                            Case RecombinationMethod.Phi80AttBP
                                ' If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("φ80 Int and IHF") Then tlList.Add("φ80 Int and IHF")
                            'End If
                            Case RecombinationMethod.HK022AttBP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("HK022 Int and IHF") Then tlList.Add("HK022 Int and IHF")
                            ' End If
                            Case RecombinationMethod.P21AttBP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("P21 Int and IHF") Then tlList.Add("P21 Int and IHF")
                            'End If
                            Case RecombinationMethod.P22AttBP

                                If Not tlList.Contains("P22 Int and IHF") Then tlList.Add("P22 Int and IHF")

                            Case RecombinationMethod.LambdaAttLR

                                If Not tlList.Contains("λ Int and IHF") Then tlList.Add("λ Int and IHF")

                            Case RecombinationMethod.Phi80AttLR

                                If Not tlList.Contains("φ80 Int, φ80 Xis and IHF") Then tlList.Add("φ80 Int, φ80 Xis and IHF")

                            Case RecombinationMethod.HK022AttLR

                                If Not tlList.Contains("HK022 Int, HK022 Xis and IHF") Then tlList.Add("HK022 Int, HK022 Xis and IHF")

                            Case RecombinationMethod.P21AttLR

                                If Not tlList.Contains("P21 Int, P21 Xis and IHF") Then tlList.Add("P21 Int, P21 Xis and IHF")

                            Case RecombinationMethod.P22AttLR

                                If Not tlList.Contains("P22 Int, P22 Xis and IHF") Then tlList.Add("P22 Int, P22 Xis and IHF")

                            Case RecombinationMethod.invitroAnnealing
                                If Not tlList.Contains("T4 DNA Polymerase") Then tlList.Add("T4 DNA Polymerase")

                        End Select
                    End If
                Case Nuctions.MolecularOperationEnum.PCR
                    If Not tlList.Contains("Pfu DNA Polymerase") Then tlList.Add("Pfu DNA Polymerase")
                    If Not tlList.Contains("Taq DNA Polymerase") Then tlList.Add("Taq DNA Polymerase")
            End Select
        Next
        ezlist.Sort()
        stb.AppendLine("Restriction Enzymes:")
        For Each ez As String In ezlist
            stb.Append(ez)
            stb.Append(" ")
        Next
        stb.AppendLine()

        tlList.Sort()
        stb.AppendLine("Tool Enzymes:")
        For Each ez As String In tlList
            stb.Append(ez)
            stb.Append(" ")
        Next
        stb.AppendLine()


        Dim pmrDict As New Dictionary(Of String, String)
        Dim idxDict As New Dictionary(Of String, Integer)

        Dim pmK As String
        Dim pmR As String

        'reduce conflicting names of primers.
        Dim fPrimer As New frmPrimerConflict
        fPrimer.Owner = Me.ParentForm
        Dim primerconflict As Boolean = False
        Dim conflictedKey As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                If ci.MolecularInfo.PrimerDesignerMode Then
                    For Each pr In ci.MolecularInfo.DesignedPrimers
                        pmK = pr.Key
                        pmR = pr.Value
                        fPrimer.AddPrimer(ci.Index, pmK, pmR)
                        If pmrDict.ContainsKey(pmK) Then
                            If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                            Else
                                ci.Selected = True
                                'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                                primerconflict = True
                                conflictedKey.Add(pmK)
                            End If
                        ElseIf pmrDict.ContainsValue(pmR) Then
                            Dim sKey As String = "<Not Found>"
                            For Each key As String In pmrDict.Keys
                                If pmrDict(key) = pmR Then
                                    sKey = key
                                    Exit For

                                End If
                            Next
                            pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                            idxDict.Add(pmK, ci.Index)
                        Else
                            pmrDict.Add(pmK, pmR)
                            idxDict.Add(pmK, ci.Index)
                        End If
                    Next
                Else
                    pmK = ci.MolecularInfo.PCR_FPrimerName
                    pmR = ci.MolecularInfo.PCR_ForwardPrimer
                    fPrimer.AddPrimer(ci.Index, pmK, pmR)
                    If pmrDict.ContainsKey(pmK) Then
                        If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                        Else
                            ci.Selected = True
                            'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                            primerconflict = True
                            conflictedKey.Add(pmK)
                        End If
                    ElseIf pmrDict.ContainsValue(pmR) Then
                        Dim sKey As String = "<Not Found>"
                        For Each key As String In pmrDict.Keys
                            If pmrDict(key) = pmR Then
                                sKey = key
                                Exit For

                            End If
                        Next
                        pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                        idxDict.Add(pmK, ci.Index)
                    Else
                        pmrDict.Add(pmK, pmR)
                        idxDict.Add(pmK, ci.Index)
                    End If

                    pmK = ci.MolecularInfo.PCR_RPrimerName
                    pmR = ci.MolecularInfo.PCR_ReversePrimer
                    fPrimer.AddPrimer(ci.Index, pmK, pmR)
                    If pmrDict.ContainsKey(pmK) Then
                        If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                        Else
                            ci.Selected = True
                            'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                            primerconflict = True
                            conflictedKey.Add(pmK)
                        End If
                    ElseIf pmrDict.ContainsValue(pmR) Then
                        Dim sKey As String = "<Not Found>"
                        For Each key As String In pmrDict.Keys
                            If pmrDict(key) = pmR Then
                                sKey = key
                                Exit For

                            End If
                        Next
                        pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                        idxDict.Add(pmK, ci.Index)
                    Else
                        pmrDict.Add(pmK, pmR)
                        idxDict.Add(pmK, ci.Index)
                    End If
                End If


            ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen AndAlso ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                pmK = ci.MolecularInfo.Screen_FName
                pmR = ci.MolecularInfo.Screen_FPrimer
                fPrimer.AddPrimer(ci.Index, pmK, pmR)
                If pmrDict.ContainsKey(pmK) Then
                    If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                    Else
                        ci.Selected = True
                        'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                        primerconflict = True
                        conflictedKey.Add(pmK)
                    End If
                ElseIf pmrDict.ContainsValue(pmR) Then
                    Dim sKey As String = "<Not Found>"
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) = pmR Then
                            sKey = key
                            Exit For

                        End If
                    Next
                    pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                    idxDict.Add(pmK, ci.Index)
                Else
                    pmrDict.Add(pmK, pmR)
                    idxDict.Add(pmK, ci.Index)
                End If

                pmK = ci.MolecularInfo.Screen_RName
                pmR = ci.MolecularInfo.Screen_RPrimer
                fPrimer.AddPrimer(ci.Index, pmK, pmR)
                If pmrDict.ContainsKey(pmK) Then
                    If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                    Else
                        ci.Selected = True
                        'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                        primerconflict = True
                        conflictedKey.Add(pmK)
                    End If
                ElseIf pmrDict.ContainsValue(pmR) Then
                    Dim sKey As String = "<Not Found>"
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) = pmR Then
                            sKey = key
                            Exit For

                        End If
                    Next
                    pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                    idxDict.Add(pmK, ci.Index)
                Else
                    pmrDict.Add(pmK, pmR)
                    idxDict.Add(pmK, ci.Index)
                End If
            End If
        Next
        If primerconflict Then
            fPrimer.HighlightPrimers(conflictedKey)
            fPrimer.Show()
            Exit Sub
        End If
        stb.AppendLine()
        stb.AppendLine("-------- Detailed Primer Information --------")
        Dim nts As Integer = 0
        Dim pms As Integer = 0

        For Each key As String In pmrDict.Keys
            stb.Append(idxDict(key).ToString.PadLeft(l, "0"))
            stb.Append(" ")
            stb.Append(key)
            stb.Append(":")
            If pmrDict(key).StartsWith("<See") Then
                stb.AppendLine()
                stb.AppendLine(pmrDict(key))
                stb.AppendLine()
            Else
                stb.Append(ControlChars.Tab)
                stb.Append("Tm:")
                stb.Append(Nuctions.CalculateTm(pmrDict(key), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                stb.Append("/")
                stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(pmrDict(key)), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                stb.Append(ControlChars.Tab)
                stb.Append(Nuctions.TAGCFilter(pmrDict(key)).Length.ToString)
                stb.Append("nt")
                stb.AppendLine()
                stb.Append(pmrDict(key))
                nts += Nuctions.TAGCFilter(pmrDict(key)).Length
                pms += 1
                stb.AppendLine()
                stb.AppendLine()
            End If
        Next

        stb.Append(">>Total: ")
        stb.Append(pms.ToString)
        If pms > 1 Then
            stb.Append(" Primers ")
        Else
            stb.Append(" Primer ")
        End If
        stb.Append(nts.ToString)
        If nts > 1 Then
            stb.Append(" Nucleotides")
        Else
            stb.Append(" Nucleotide")
        End If
        stb.AppendLine()
        stb.AppendLine()

        stb.AppendLine("-------- Double Strand DNA Synthesis Information --------")
        Dim DSeqLength As Integer = 0
        Dim SeqCount As Integer = 0
        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                    stb.AppendLine("(" + ci.Index.ToString + ")" + ci.MolecularInfo.FreeDesignName)
                    stb.Append(ci.MolecularInfo.FreeDesignName + ":")
                    stb.AppendLine(String.Format("Length {0} bps", gf.Length.ToString))
                    stb.AppendLine("<Start of Sequence>")
                    stb.AppendLine(gf.Sequence)
                    DSeqLength += gf.Length
                    SeqCount += 1
                    stb.AppendLine("<End of Sequence>")
                    stb.AppendLine()
                End If
            End If
        Next
        stb.AppendLine(String.Format(">>Total: {0} Sequence{1} added up to {2}bp", SeqCount.ToString, IIf(SeqCount > 1, "s", ""), DSeqLength.ToString))
        stb.AppendLine()
        Select Case RegionalLanguage
            Case Language.English
                stb.AppendLine("Exmperimentals:")
                stb.AppendLine()
                stb.AppendLine("Plasmids:")
                stb.AppendLine(DescribeStringList(plList, Language.English))
                stb.AppendLine()
                stb.AppendLine("Bacteria Strains:")
                stb.AppendLine(DescribeStringList(stList, Language.English))
                stb.AppendLine()
                stb.AppendLine("Restriction Enzymes:")
                stb.AppendLine(DescribeStringList(ezlist, Language.English))
                stb.AppendLine()
                stb.AppendLine("Modification, Recombination and PCR Enzymes:")
                stb.AppendLine(DescribeStringList(tlList, Language.English))
                stb.AppendLine()
            Case Language.Chinese
                stb.AppendLine("实验部分：")
                stb.AppendLine()
                stb.AppendLine("质粒:")
                stb.AppendLine(DescribeStringList(plList, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("菌种:")
                stb.AppendLine(DescribeStringList(stList, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("限制性内切酶:")
                stb.AppendLine(DescribeStringList(ezlist, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("工具酶:")
                stb.AppendLine(DescribeStringList(tlList, Language.Chinese))
                stb.AppendLine()
        End Select
        Select Case RegionalLanguage
            Case Language.English
                stb.AppendLine("Primers:")
                For Each key As String In pmrDict.Keys
                    If pmrDict(key).StartsWith("<See") Then Continue For
                    stb.Append(key)
                    stb.Append(":")
                    stb.Append(Nuctions.TAGCFilter(pmrDict(key)))
                    stb.AppendLine()
                Next
                stb.AppendLine()
            Case Language.Chinese
                stb.AppendLine("引物合成：")
                For Each key As String In pmrDict.Keys
                    If pmrDict(key).StartsWith("<See") Then Continue For
                    stb.Append(key)
                    stb.Append(":")
                    stb.Append(Nuctions.TAGCFilter(pmrDict(key)))
                    stb.AppendLine()
                Next
        End Select
        Select Case RegionalLanguage
            Case Language.English
                stb.AppendLine("Gene Synthesis:")
                For Each ci As ChartItem In lv_Chart.Items
                    If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
                        If ci.MolecularInfo.DNAs.Count = 1 Then
                            Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                            'stb.AppendLine("(" + ci.Index.ToString + ")" + ci.MolecularInfo.FreeDesignName)
                            stb.Append(ci.MolecularInfo.FreeDesignName + ":")
                            'stb.AppendLine(String.Format("Length {0} bps", gf.Length.ToString))
                            'stb.AppendLine("<Start of Sequence>")
                            stb.AppendLine(gf.Sequence)
                            DSeqLength += gf.Length
                            SeqCount += 1
                            'stb.AppendLine("<End of Sequence>")
                            'stb.AppendLine()
                        End If
                    End If
                Next
                'stb.AppendLine(String.Format(">>Total: {0} Sequence{1} added up to {2}bp", SeqCount.ToString, IIf(SeqCount > 1, "s", ""), DSeqLength.ToString))
                stb.AppendLine()
            Case Language.Chinese
                stb.AppendLine("基因合成：")
                For Each ci As ChartItem In lv_Chart.Items
                    If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
                        If ci.MolecularInfo.DNAs.Count = 1 Then
                            Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                            'stb.AppendLine("(" + ci.Index.ToString + ")" + ci.MolecularInfo.FreeDesignName)
                            stb.Append(ci.MolecularInfo.FreeDesignName + ":")
                            'stb.AppendLine(String.Format("Length {0} bps", gf.Length.ToString))
                            'stb.AppendLine("<Start of Sequence>")
                            stb.AppendLine(gf.Sequence)
                            DSeqLength += gf.Length
                            SeqCount += 1
                            'stb.AppendLine("<End of Sequence>")
                            'stb.AppendLine()
                        End If
                    End If
                Next
                'stb.AppendLine(String.Format(">>Total: {0} Sequence{1} added up to {2}bp", SeqCount.ToString, IIf(SeqCount > 1, "s", ""), DSeqLength.ToString))
        End Select
        '获取实验流程
        Dim DI As DNAInfo
        For Each ci As ChartItem In lv_Chart.Items
            DI = ci.MolecularInfo
        Next
        Select Case RegionalLanguage
            Case Language.English
                stb.AppendLine("Constructions:")
            Case Language.Chinese
                stb.AppendLine("实验步骤：")
        End Select

        Dim infolist As New List(Of DNAInfo)
        Dim finallist As New List(Of DNAInfo)
        For Each ci As ChartItem In lv_Chart.Items
            infolist.Add(ci.MolecularInfo)
        Next

        Dim CountList As New Dictionary(Of DNAInfo, Integer)

        For Each dit As DNAInfo In infolist
            CountList.Add(dit, 0)
        Next
        For Each dit As DNAInfo In infolist
            For Each sc As DNAInfo In dit.Source
                If Not dit.IsAnalytical Then CountList(sc) += 1
            Next
        Next
        For Each dit As DNAInfo In infolist
            If CountList(dit) = 0 AndAlso (Not dit.IsAnalytical) Then
                dit.IsConstructionNode = True
                dit.IsKeyName = True
            End If
            If CountList(dit) > 1 Then
                If dit.Source.Count > 0 Then
                    dit.IsConstructionNode = True
                    dit.IsKeyName = True
                Else
                    dit.IsConstructionNode = False
                    dit.IsKeyName = True
                End If
            End If
        Next
        Dim contained As Boolean
        For Each v As DNAInfo In infolist
            contained = False
            If v.IsConstructionNode Then
                finallist.Add(v)
            Else
                contained = False
                For Each t As DNAInfo In infolist
                    If t.Source.Contains(v) Then
                        contained = True
                        Exit For
                    End If
                Next
                If Not contained And Not v.IsVerificationStep Then finallist.Add(v)
            End If
        Next
        stb.Append(GetDescripotion(finallist, True))
    End Sub

    Private Sub GenerateSummary(ByVal stb As System.Text.StringBuilder, smr As SummaryEventArgs)
        If FileAddress Is Nothing OrElse FileAddress.Length = 0 OrElse Not IO.File.Exists(FileAddress) Then Save() : Return

        smr.ProjectName = ProjectName

        Dim l As Integer = lv_Chart.Items.Count.ToString.Length
        stb.AppendLine("-------- Project Information --------")
        smr.ProjectInformation = pvMain.rtbSummary.Text.Replace(ControlChars.Lf, ControlChars.NewLine)
        stb.Append(smr.ProjectInformation)
        stb.AppendLine()
        stb.AppendLine()

        Dim stbPrimerError As New System.Text.StringBuilder
        Dim HasPrimerError As Boolean = False
        Dim PrimerNodes As New Dictionary(Of String, Integer)
        Dim AddPrimerToSummary = Sub(_pKey As String, _pPrimer As String, id As Integer)
                                     Dim res = smr.Primers.Where(Function(kvp) kvp.Value = _pPrimer AndAlso kvp.Key <> _pKey)
                                     If res.Any Then
                                         Dim _first = res.First
                                         stbPrimerError.AppendFormat("Node #{0} Primer {1} has the same sequence as Node #{2} Primer {3}.", id, _pKey, PrimerNodes(_first.Key), _first.Key)
                                         stbPrimerError.AppendLine()
                                         HasPrimerError = True
                                     Else
                                         If Not smr.Primers.ContainsKey(_pKey) Then
                                             If _pPrimer Is Nothing Then
                                                 stbPrimerError.AppendFormat("Node #{0} Primer {1} is Empty.", id, _pKey)
                                                 stbPrimerError.AppendLine()
                                                 HasPrimerError = True
                                             ElseIf _pPrimer.Length >= 10 Then
                                                 smr.Primers.Add(_pKey, _pPrimer)
                                                 PrimerNodes.Add(_pKey, id)
                                             ElseIf _pPrimer.Length > 0 Then
                                                 smr.Primers.Add(_pKey, _pPrimer)
                                                 PrimerNodes.Add(_pKey, id)
                                                 stbPrimerError.AppendFormat("Node #{0} Primer {1} is shorter than 10nt.", id, _pKey)
                                                 stbPrimerError.AppendLine()
                                                 HasPrimerError = True
                                             Else
                                                 stbPrimerError.AppendFormat("Node #{0} Primer {1} is Empty.", id, _pKey)
                                                 stbPrimerError.AppendLine()
                                                 HasPrimerError = True
                                             End If
                                         ElseIf _pPrimer <> smr.Primers(_pKey) Then
                                             stbPrimerError.AppendFormat("Node #{0} Primer {1} conflicts with Node #{2} Primer {3}.", id, _pKey, PrimerNodes(_pKey), _pKey)
                                             stbPrimerError.AppendLine()
                                             HasPrimerError = True
                                         End If
                                     End If
                                 End Sub

        For Each ci As ChartItem In lv_Chart.Items
            Dim dx As DNAInfo = ci.MolecularInfo
            Select Case dx.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Vector
                    If dx.DNAs.Count > 0 Then
                        Dim gf As Nuctions.GeneFile = dx.DNAs(1)
                        Dim ghash As String = gf.GetHash
                        If Not smr.Vectors.ContainsKey(ghash) Then smr.Vectors.Add(ghash, gf)
                    End If
                Case Nuctions.MolecularOperationEnum.Host
                    If dx.Cells.Count > 0 Then

                    End If
                Case Nuctions.MolecularOperationEnum.Enzyme
                    For Each ez As String In dx.Enzyme_Enzymes
                        smr.Enzymes.Add(ez)
                    Next
                    If dx.DephosphorylateWhenDigestion Then
                        Dim tKey As String
                        'Dim ts As ToolSummary
                        tKey = "CIAP"
                        smr.ToolEnzymes.Add(tKey)
                    End If
                Case Nuctions.MolecularOperationEnum.PCR
                    If dx.PrimerDesignerMode Then
                        Dim pmx As String
                        For Each pKey As String In dx.DesignedPrimers.Keys
                            pmx = Nuctions.TAGCFilter(dx.DesignedPrimers(pKey))

                            AddPrimerToSummary(pKey, pmx, ci.Index)
                            'If Not smr.Primers.ContainsKey(pKey) Then
                            '    If pmx IsNot Nothing OrElse pmx.Length > 10 Then smr.Primers.Add(pKey, pmx)
                            'ElseIf pmx <> smr.Primers(pKey) Then

                            'End If
                        Next
                    Else
                        Dim pmx As String
                        'Dim ps As PrimerSummary
                        Dim dic As New Dictionary(Of String, String) From {{dx.PCR_FPrimerName, dx.PCR_ForwardPrimer}, {dx.PCR_RPrimerName, dx.PCR_ReversePrimer}}
                        For Each pKey As String In dic.Keys
                            pmx = Nuctions.TAGCFilter(dic(pKey))
                            AddPrimerToSummary(pKey, pmx, ci.Index)
                            'If Not smr.Primers.ContainsKey(pKey) Then smr.Primers.Add(pKey, pmx)
                        Next
                    End If
                Case Nuctions.MolecularOperationEnum.Screen
                    If dx.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                        If dx.PrimerDesignerMode Then
                            Dim pmx As String
                            For Each pKey As String In dx.DesignedPrimers.Keys
                                pmx = Nuctions.TAGCFilter(dx.DesignedPrimers(pKey))
                                AddPrimerToSummary(pKey, pmx, ci.Index)
                                'If Not smr.Primers.ContainsKey(pKey) Then smr.Primers.Add(pKey, pmx)
                            Next
                        Else
                            Dim pmx As String
                            Dim dic As New Dictionary(Of String, String) From {{dx.Screen_FName, dx.Screen_FPrimer}, {dx.Screen_RName, dx.Screen_RPrimer}}
                            For Each pKey As String In dic.Keys
                                pmx = Nuctions.TAGCFilter(dic(pKey))
                                AddPrimerToSummary(pKey, pmx, ci.Index)
                                'If Not smr.Primers.ContainsKey(pKey) Then smr.Primers.Add(pKey, pmx)
                            Next
                        End If
                    End If
                Case Nuctions.MolecularOperationEnum.Modify
                    Dim tKey As String
                    Select Case dx.Modify_Method
                        Case Nuctions.ModificationMethodEnum.CIAP
                            tKey = "CIAP"
                            smr.ToolEnzymes.Add(tKey)
                        Case Nuctions.ModificationMethodEnum.Klewnow
                            tKey = "Klewnow"
                            smr.ToolEnzymes.Add(tKey)
                        Case Nuctions.ModificationMethodEnum.PNK
                            tKey = "PNK"
                            smr.ToolEnzymes.Add(tKey)
                        Case Nuctions.ModificationMethodEnum.T4DNAP
                            tKey = "T4DNAP"
                            smr.ToolEnzymes.Add(tKey)
                    End Select
                Case Nuctions.MolecularOperationEnum.Recombination

                Case Nuctions.MolecularOperationEnum.FreeDesign
                    If ci.MolecularInfo.DNAs.Count > 0 Then
                        Dim gf As Nuctions.GeneFile = dx.DNAs(1)
                        Dim ghash As String = gf.GetHash
                        If Not smr.Sequences.ContainsKey(ghash) Then smr.Sequences.Add(ghash, gf)
                    End If
            End Select
        Next


        If HasPrimerError Then MsgBox(stbPrimerError.ToString, MsgBoxStyle.OkOnly, "Primer Errors:")

        stb.AppendLine("-------- Starting Materials --------")

        stb.AppendLine("Plasmids:")
        Dim plList As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Vector Then
                If Not plList.Contains(ci.MolecularInfo.Name) Then plList.Add(ci.MolecularInfo.Name)
                stb.Append(ci.Index.ToString.PadLeft(l, "0") + " " + ci.MolecularInfo.Name)
                stb.AppendLine(":")
                If Not (ci.MolecularInfo.OperationDescription Is Nothing) Then
                    stb.AppendLine(ci.MolecularInfo.OperationDescription.Replace(ControlChars.Lf, ControlChars.NewLine))
                Else
                    stb.AppendLine("N/A")
                End If
                stb.AppendLine()
            End If
        Next
        stb.AppendLine("Strains:")
        Dim stList As New List(Of String)
        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Host Then
                If ci.MolecularInfo.Cells.Count > 0 Then
                    If Not stList.Contains(ci.MolecularInfo.Cells(0).Host.Name) Then stList.Add(ci.MolecularInfo.Cells(0).Host.Name)
                    stb.Append(ci.Index.ToString.PadLeft(l, "0") + " " + ci.MolecularInfo.Cells(0).Host.Name)
                    stb.AppendLine(":")
                    If Not (ci.MolecularInfo.OperationDescription Is Nothing) Then
                        stb.AppendLine(ci.MolecularInfo.OperationDescription.Replace(ControlChars.Lf, ControlChars.NewLine))
                    Else
                        stb.AppendLine("N/A")
                    End If
                End If
                stb.AppendLine()

            End If
        Next

        stb.AppendLine("-------- Enzyme Information --------")


        Dim ezlist As New List(Of String)
        Dim tlList As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            Select Case ci.MolecularInfo.MolecularOperation
                Case Nuctions.MolecularOperationEnum.Enzyme
                    For Each ez As String In ci.MolecularInfo.Enzyme_Enzymes
                        If Not ezlist.Contains(ez) Then
                            ezlist.Add(ez)
                        End If
                    Next
                    If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                        If Not tlList.Contains("CIAP") Then tlList.Add("CIAP")
                    End If
                Case Nuctions.MolecularOperationEnum.Ligation
                    'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                    If Not tlList.Contains("T4 DNA Ligase") Then tlList.Add("T4 DNA Ligase")
                'End If
                Case Nuctions.MolecularOperationEnum.Modify
                    Select Case ci.MolecularInfo.Modify_Method
                        Case Nuctions.ModificationMethodEnum.CIAP
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Calf Intestinal Alkaline Phosphatase") Then tlList.Add("Calf Intestinal Alkaline Phosphatase")
                        'End If
                        Case Nuctions.ModificationMethodEnum.Klewnow
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Klewnow Fragment") Then tlList.Add("Klewnow Fragment")
                        'End If
                        Case Nuctions.ModificationMethodEnum.PNK
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("Polynucleatide Kinase") Then tlList.Add("Polynucleatide Kinase")
                        'End If
                        Case Nuctions.ModificationMethodEnum.T4DNAP
                            'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                            If Not tlList.Contains("T4 DNA Polymerase") Then tlList.Add("T4 DNA Polymerase")
                            'End If
                    End Select
                Case Nuctions.MolecularOperationEnum.Recombination
                    If Not ci.MolecularInfo.IsCellNode Then
                        Select Case ci.MolecularInfo.RecombinationMethod
                            Case RecombinationMethod.FRT
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("FLP Recombinase") Then tlList.Add("FLP Recombinase")
                            'End If
                            Case RecombinationMethod.LoxP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("Cre Recombinase") Then tlList.Add("Cre Recombinase")
                            'End If
                            Case RecombinationMethod.LambdaAttBP
                                ' If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("λ Int, λ Xis and IHF") Then tlList.Add("λ Int, λ Xis and IHF")
                            'End If
                            Case RecombinationMethod.Phi80AttBP
                                ' If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("φ80 Int and IHF") Then tlList.Add("φ80 Int and IHF")
                            'End If
                            Case RecombinationMethod.HK022AttBP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("HK022 Int and IHF") Then tlList.Add("HK022 Int and IHF")
                            ' End If
                            Case RecombinationMethod.P21AttBP
                                'If ci.MolecularInfo.DephosphorylateWhenDigestion Then
                                If Not tlList.Contains("P21 Int and IHF") Then tlList.Add("P21 Int and IHF")
                            'End If
                            Case RecombinationMethod.P22AttBP

                                If Not tlList.Contains("P22 Int and IHF") Then tlList.Add("P22 Int and IHF")

                            Case RecombinationMethod.LambdaAttLR

                                If Not tlList.Contains("λ Int and IHF") Then tlList.Add("λ Int and IHF")

                            Case RecombinationMethod.Phi80AttLR

                                If Not tlList.Contains("φ80 Int, φ80 Xis and IHF") Then tlList.Add("φ80 Int, φ80 Xis and IHF")

                            Case RecombinationMethod.HK022AttLR

                                If Not tlList.Contains("HK022 Int, HK022 Xis and IHF") Then tlList.Add("HK022 Int, HK022 Xis and IHF")

                            Case RecombinationMethod.P21AttLR

                                If Not tlList.Contains("P21 Int, P21 Xis and IHF") Then tlList.Add("P21 Int, P21 Xis and IHF")

                            Case RecombinationMethod.P22AttLR

                                If Not tlList.Contains("P22 Int, P22 Xis and IHF") Then tlList.Add("P22 Int, P22 Xis and IHF")

                            Case RecombinationMethod.invitroAnnealing
                                If Not tlList.Contains("T4 DNA Polymerase") Then tlList.Add("T4 DNA Polymerase")

                        End Select
                    End If
                Case Nuctions.MolecularOperationEnum.PCR
                    If Not tlList.Contains("Pfu DNA Polymerase") Then tlList.Add("Pfu DNA Polymerase")
                    If Not tlList.Contains("Taq DNA Polymerase") Then tlList.Add("Taq DNA Polymerase")
            End Select
        Next
        ezlist.Sort()
        stb.AppendLine("Restriction Enzymes:")
        For Each ez As String In ezlist
            stb.Append(ez)
            stb.Append(" ")
        Next
        stb.AppendLine()

        tlList.Sort()
        stb.AppendLine("Tool Enzymes:")
        For Each ez As String In tlList
            stb.Append(ez)
            stb.Append(" ")
        Next
        stb.AppendLine()


        Dim pmrDict As New Dictionary(Of String, String)
        Dim idxDict As New Dictionary(Of String, Integer)

        Dim pmK As String
        Dim pmR As String

        'reduce conflicting names of primers.
        Dim fPrimer As New frmPrimerConflict
        fPrimer.Owner = Me.ParentForm
        Dim primerconflict As Boolean = False
        Dim conflictedKey As New List(Of String)

        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.PCR Then
                If ci.MolecularInfo.PrimerDesignerMode Then
                    For Each pr In ci.MolecularInfo.DesignedPrimers
                        pmK = pr.Key
                        pmR = pr.Value
                        fPrimer.AddPrimer(ci.Index, pmK, pmR)
                        If pmrDict.ContainsKey(pmK) Then
                            If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                            Else
                                ci.Selected = True
                                'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                                primerconflict = True
                                conflictedKey.Add(pmK)
                            End If
                        ElseIf pmrDict.ContainsValue(pmR) Then
                            Dim sKey As String = "<Not Found>"
                            For Each key As String In pmrDict.Keys
                                If pmrDict(key) = pmR Then
                                    sKey = key
                                    Exit For

                                End If
                            Next
                            pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                            idxDict.Add(pmK, ci.Index)
                        Else
                            pmrDict.Add(pmK, pmR)
                            idxDict.Add(pmK, ci.Index)
                        End If
                    Next
                Else
                    pmK = ci.MolecularInfo.PCR_FPrimerName
                    pmR = ci.MolecularInfo.PCR_ForwardPrimer
                    fPrimer.AddPrimer(ci.Index, pmK, pmR)
                    If pmrDict.ContainsKey(pmK) Then
                        If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                        Else
                            ci.Selected = True
                            'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                            primerconflict = True
                            conflictedKey.Add(pmK)
                        End If
                    ElseIf pmrDict.ContainsValue(pmR) Then
                        Dim sKey As String = "<Not Found>"
                        For Each key As String In pmrDict.Keys
                            If pmrDict(key) = pmR Then
                                sKey = key
                                Exit For

                            End If
                        Next
                        pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                        idxDict.Add(pmK, ci.Index)
                    Else
                        pmrDict.Add(pmK, pmR)
                        idxDict.Add(pmK, ci.Index)
                    End If

                    pmK = ci.MolecularInfo.PCR_RPrimerName
                    pmR = ci.MolecularInfo.PCR_ReversePrimer
                    fPrimer.AddPrimer(ci.Index, pmK, pmR)
                    If pmrDict.ContainsKey(pmK) Then
                        If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                        Else
                            ci.Selected = True
                            'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                            primerconflict = True
                            conflictedKey.Add(pmK)
                        End If
                    ElseIf pmrDict.ContainsValue(pmR) Then
                        Dim sKey As String = "<Not Found>"
                        For Each key As String In pmrDict.Keys
                            If pmrDict(key) = pmR Then
                                sKey = key
                                Exit For

                            End If
                        Next
                        pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                        idxDict.Add(pmK, ci.Index)
                    Else
                        pmrDict.Add(pmK, pmR)
                        idxDict.Add(pmK, ci.Index)
                    End If
                End If
            ElseIf ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Screen AndAlso ci.MolecularInfo.Screen_Mode = Nuctions.ScreenModeEnum.PCR Then
                pmK = ci.MolecularInfo.Screen_FName
                pmR = ci.MolecularInfo.Screen_FPrimer
                fPrimer.AddPrimer(ci.Index, pmK, pmR)
                If pmrDict.ContainsKey(pmK) Then
                    If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                    Else
                        ci.Selected = True
                        'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                        primerconflict = True
                        conflictedKey.Add(pmK)
                    End If
                ElseIf pmrDict.ContainsValue(pmR) Then
                    Dim sKey As String = "<Not Found>"
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) = pmR Then
                            sKey = key
                            Exit For

                        End If
                    Next
                    pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                    idxDict.Add(pmK, ci.Index)
                Else
                    pmrDict.Add(pmK, pmR)
                    idxDict.Add(pmK, ci.Index)
                End If

                pmK = ci.MolecularInfo.Screen_RName
                pmR = ci.MolecularInfo.Screen_RPrimer
                fPrimer.AddPrimer(ci.Index, pmK, pmR)
                If pmrDict.ContainsKey(pmK) Then
                    If Nuctions.TAGCFilter(pmrDict(pmK)) = Nuctions.TAGCFilter(pmR) Then

                    Else
                        ci.Selected = True
                        'MsgBox("Different Primers have the same name At Item " + ci.Index.ToString.PadLeft(l, "0"), MsgBoxStyle.OkOnly, "Primer Error")
                        primerconflict = True
                        conflictedKey.Add(pmK)
                    End If
                ElseIf pmrDict.ContainsValue(pmR) Then
                    Dim sKey As String = "<Not Found>"
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) = pmR Then
                            sKey = key
                            Exit For

                        End If
                    Next
                    pmrDict.Add(pmK, "<See " + idxDict(sKey).ToString.PadLeft(l, "0") + " " + sKey + ">")
                    idxDict.Add(pmK, ci.Index)
                Else
                    pmrDict.Add(pmK, pmR)
                    idxDict.Add(pmK, ci.Index)
                End If
            End If
        Next
        If primerconflict Then
            fPrimer.HighlightPrimers(conflictedKey)
            fPrimer.Show()
            Exit Sub
        End If
        stb.AppendLine()
        stb.AppendLine("-------- Detailed Primer Information --------")
        Dim nts As Integer = 0
        Dim pms As Integer = 0

        For Each key As String In pmrDict.Keys
            If pmrDict(key) Is Nothing Then Continue For
            stb.Append(idxDict(key).ToString.PadLeft(l, "0"))
            stb.Append(" ")
            stb.Append(key)
            stb.Append(":")
            If pmrDict(key).StartsWith("<See") Then
                stb.AppendLine()
                stb.AppendLine(pmrDict(key))
                stb.AppendLine()
            Else
                stb.Append(ControlChars.Tab)
                stb.Append("Tm:")
                stb.Append(Nuctions.CalculateTm(pmrDict(key), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                stb.Append("/")
                stb.Append(Nuctions.CalculateTm(Nuctions.ParseInnerPrimer(pmrDict(key)), 80 * 0.001, 625 * 0.000000001).Tm.ToString("0.0"))
                stb.Append(ControlChars.Tab)
                stb.Append(Nuctions.TAGCFilter(pmrDict(key)).Length.ToString)
                stb.Append("nt")
                stb.AppendLine()
                stb.Append(pmrDict(key))
                nts += Nuctions.TAGCFilter(pmrDict(key)).Length
                pms += 1
                stb.AppendLine()
                stb.AppendLine()
            End If
        Next

        stb.Append(">>Total: ")
        stb.Append(pms.ToString)
        If pms > 1 Then
            stb.Append(" Primers ")
        Else
            stb.Append(" Primer ")
        End If
        stb.Append(nts.ToString)
        If nts > 1 Then
            stb.Append(" Nucleotides")
        Else
            stb.Append(" Nucleotide")
        End If
        stb.AppendLine()
        stb.AppendLine()


        stb.AppendLine("-------- Double Strand DNA Synthesis Information --------")
        Dim DSeqLength As Integer = 0
        Dim SeqCount As Integer = 0

        Dim snList As New List(Of Pair(Of String, String))
        For Each ci As ChartItem In lv_Chart.Items
            If ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign Then
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                    stb.AppendLine("(" + ci.Index.ToString + ")" + ci.MolecularInfo.FreeDesignName)
                    stb.Append(ci.MolecularInfo.FreeDesignName + ":")
                    stb.AppendLine(String.Format("Length {0} bps", gf.Length.ToString))
                    stb.AppendLine("<Start of Sequence>")
                    stb.AppendLine(gf.Sequence)
                    DSeqLength += gf.Length
                    SeqCount += 1
                    stb.AppendLine("<End of Sequence>")
                    stb.AppendLine()
                    snList.Add(New Pair(Of String, String) With {.Key = ci.MolecularInfo.FreeDesignName, .Value = gf.Sequence})
                End If
            End If
        Next
        stb.AppendLine(String.Format(">>Total: {0} Sequence{1} added up to {2}bp", SeqCount.ToString, IIf(SeqCount > 1, "s", ""), DSeqLength.ToString))
        stb.AppendLine()
        'stb.ItalicItems.Add("E. coli")
        'stb.FontSize(FontSizeEnum.F5)

        Select Case RegionalLanguage
            Case Language.English
                'stb.FirstIndent(0)
                'stb.Font("Shruti")
                'stb.StartItalic()
                stb.AppendLine("Experimentals:")
                'stb.EndItalic()
                'stb.Font("Courier New")
                'stb.FirstIndent()
                If plList.Count > 0 Then
                    'stb.StartBold()
                    stb.Append("Plasmids: ")
                    'stb.EndBold()
                    stb.AppendLine(DescribeStringList(plList, Language.English))
                End If
                If stList.Count > 0 Then
                    'stb.StartBold()
                    stb.Append("Bacteria Strains: ")
                    'stb.EndBold()
                    stb.AppendLine(DescribeStringList(stList, Language.English))
                End If
                If ezlist.Count > 0 Then
                    'stb.StartBold()
                    stb.Append("Restriction Enzymes: ")
                    'stb.EndBold()
                    stb.AppendLine(DescribeStringList(ezlist, Language.English))
                End If
                If tlList.Count > 0 Then
                    'stb.StartBold()
                    stb.Append("Modification, Recombination and PCR Enzymes: ")
                    'stb.EndBold()
                    stb.AppendLine(DescribeStringList(tlList, Language.English))
                End If
            Case Language.Chinese
                stb.AppendLine("实验部分：")
                stb.AppendLine()
                stb.AppendLine("质粒: ")
                stb.AppendLine(DescribeStringList(plList, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("菌种: ")
                stb.AppendLine(DescribeStringList(stList, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("限制性内切酶: ")
                stb.AppendLine(DescribeStringList(ezlist, Language.Chinese))
                stb.AppendLine()
                stb.AppendLine("工具酶: ")
                stb.AppendLine(DescribeStringList(tlList, Language.Chinese))
                stb.AppendLine()
        End Select
        If pmrDict.Count > 0 Then
            Select Case RegionalLanguage
                Case Language.English
                    'stb.StartBold()
                    stb.AppendLine("Primers:")
                    'stb.EndBold()
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) Is Nothing Then Continue For
                        If pmrDict(key).StartsWith("<See") Then Continue For
                        stb.Append(key)
                        stb.Append(":")
                        stb.Append(Nuctions.TAGCFilter(pmrDict(key)))
                        stb.AppendLine()
                    Next
                Case Language.Chinese
                    stb.AppendLine("引物合成：")
                    For Each key As String In pmrDict.Keys
                        If pmrDict(key) Is Nothing Then Continue For
                        If pmrDict(key).StartsWith("<See") Then Continue For
                        stb.Append(key)
                        stb.Append(":")
                        stb.Append(Nuctions.TAGCFilter(pmrDict(key)))
                        stb.AppendLine()
                    Next
            End Select
        End If

        If snList.Count > 0 Then
            Select Case RegionalLanguage
                Case Language.English
                    'stb.StartBold()
                    stb.AppendLine("Gene Synthesis:")
                    'stb.EndBold()
                    For Each _p In snList
                        stb.Append(_p.Key + ":")
                        stb.AppendLine(_p.Value)
                    Next
                Case Language.Chinese
                    stb.AppendLine("基因合成：")
                    For Each _p In snList
                        stb.Append(_p.Key + ":")
                        stb.AppendLine(_p.Value)
                    Next
            End Select
        End If

        '获取实验流程
        Dim DI As DNAInfo
        For Each ci As ChartItem In lv_Chart.Items
            DI = ci.MolecularInfo
        Next

        'stb.StartBold()
        Select Case RegionalLanguage
            Case Language.English
                stb.AppendLine("Constructions:")
            Case Language.Chinese
                stb.AppendLine("实验步骤：")
        End Select
        'stb.EndBold()
        stb.AppendLine()
        Dim infolist As New List(Of DNAInfo)
        Dim finallist As New List(Of DNAInfo)
        For Each ci As ChartItem In lv_Chart.Items
            infolist.Add(ci.MolecularInfo)
        Next

        Dim CountList As New Dictionary(Of DNAInfo, Integer)

        For Each dit As DNAInfo In infolist
            CountList.Add(dit, 0)
        Next
        For Each dit As DNAInfo In infolist
            For Each sc As DNAInfo In dit.Source
                If Not dit.IsAnalytical Then CountList(sc) += 1
            Next
        Next
        For Each dit As DNAInfo In infolist
            If CountList(dit) = 0 AndAlso (Not dit.IsAnalytical) Then
                dit.IsConstructionNode = True
                dit.IsKeyName = True
            End If
            If CountList(dit) > 1 Then
                If dit.Source.Count > 0 Then
                    dit.IsConstructionNode = True
                    dit.IsKeyName = True
                Else
                    dit.IsConstructionNode = False
                    dit.IsKeyName = True
                End If
            End If
        Next
        Dim contained As Boolean
        For Each v As DNAInfo In infolist
            contained = False
            If v.IsConstructionNode Then
                finallist.Add(v)
            Else
                contained = False
                For Each t As DNAInfo In infolist
                    If t.Source.Contains(v) Then
                        contained = True
                        Exit For
                    End If
                Next
                If Not contained And Not v.IsVerificationStep Then finallist.Add(v)
            End If
        Next
        'stb.AppendText = GetDescripotion(finallist, True)

        Dim primerlist As New System.Text.StringBuilder
        primerlist.AppendLine()
        For Each pkey In smr.Primers.Keys
            primerlist.AppendFormat("{0}{1}{2}", pkey, vbTab, Nuctions.TAGCFilter(smr.Primers(pkey)))
            primerlist.AppendLine()
        Next
        stb.AppendLine(primerlist.ToString)


    End Sub

    Private Sub pvMain_ExportPrimerList(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.ExportProject
        Export()
    End Sub
    Friend Sub Export()
        Dim stb As New System.Text.StringBuilder
        GenerateSummary(stb)
        If sfdExport.ShowDialog = DialogResult.OK Then
            Dim fi As New IO.FileInfo(sfdExport.FileName)

            IO.File.WriteAllText(fi.FullName, stb.ToString)

            Dim fs As OperationView = lv_Chart
            Dim slist As New List(Of DNAInfo)
            For Each ci As ChartItem In lv_Chart.Items
                slist.Add(ci.MolecularInfo)
            Next
            fs.LoadSummary(slist, EnzymeCol, FeatureCol)

            Dim di As New IO.DirectoryInfo(fi.FullName.Substring(0, fi.FullName.LastIndexOf(".")))
            di.Create()
            For Each dif As IO.FileInfo In di.GetFiles
                dif.Delete()
            Next
            fs.Draw()
            fs.SavePictureTo(di.FullName + "\Project Flowchart")
            IO.File.WriteAllText(di.FullName + "\Project Info.txt", stb.ToString)
            fs.SaveFilesTo(di.FullName)
        End If
    End Sub

    Private Sub pvMain_RemarkFeature(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.RemarkFeature

        Dim geneCol As New List(Of Nuctions.GeneFile)

        For Each ci As ChartItem In lv_Chart.Items
            For Each gf As Nuctions.GeneFile In ci.MolecularInfo.DNAs
                If Not geneCol.Contains(gf) Then geneCol.Add(gf)
            Next
            For Each c As Nuctions.Cell In ci.MolecularInfo.Cells
                For Each gf As Nuctions.GeneFile In c.DNAs
                    If Not geneCol.Contains(gf) Then geneCol.Add(gf)
                Next
            Next
        Next
        Nuctions.AddFeatures(geneCol, FeatureCol)
        For Each ci As ChartItem In lv_Chart.Items
            ci.Reload(ci.MolecularInfo, EnzymeCol)
        Next
        lv_Chart.Draw()
    End Sub

    Private Sub RecalculateAllChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecalculateAllChildrenToolStripMenuItem.Click
        RecalculateAllChildren()
        lv_Chart.Draw()
    End Sub

    Public Sub UpdateView()
        lv_Chart.Draw()
    End Sub


    Public Sub RecalculateItem(ByVal ci As ChartItem)
        ci.MolecularInfo.Calculate()
        ci.Reload(ci.MolecularInfo, EnzymeCol)
    End Sub

    Public Sub RecalculateAllChildren()


        Dim cnLevel As New List(Of ChartItem)

        Dim ntLevel As New List(Of ChartItem)


        For Each ci As ChartItem In lv_Chart.SelectedItems
            ntLevel.Add(ci)
        Next

        While ntLevel.Count > 0
            cnLevel.Clear()
            cnLevel.AddRange(ntLevel)
            ntLevel.Clear()

            '排列下一层
            For Each ci As ChartItem In cnLevel
                RecalculateItem(ci)
            Next

            '再寻找下一层
            For Each si As ChartItem In lv_Chart.Items
                For Each ci As ChartItem In cnLevel
                    If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
                        ntLevel.Add(si)
                        Exit For
                    End If
                Next
            Next
        End While

    End Sub

    Private Sub AutoArragneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoArragneToolStripMenuItem.Click
        lv_Chart.AutoArragne()
    End Sub

    Private Sub AutoFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoFitChildrenToolStripMenuItem.Click
        lv_Chart.AutoFitChildren()
    End Sub

    Private Sub StepFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StepFitChildrenToolStripMenuItem.Click
        lv_Chart.FitChildrenStepByStep()
    End Sub

    Private Sub pvMain_ExportSummary(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub WorkControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lv_Chart.Menu = cms_ChartItem
        lv_Chart.SetEnzymes(EnzymeCol)
        lv_Chart.SetFeatures(FeatureCol)
    End Sub


    Dim vTarget As DNAInfo
    Dim vOldSource As List(Of DNAInfo)
    Public ReadOnly Property SourceMode() As Boolean
        Get
            Return lv_Chart.SourceMode
        End Get
    End Property
    Private Sub pvMain_RequireSource(ByVal sender As Object, ByVal e As SourceEventArgs) Handles pvMain.RequireSource
        vTarget = e.Target
        vOldSource = e.Target.Source
        e.Target.Source = New List(Of DNAInfo)
        e.Target.Source.AddRange(vOldSource)
        lv_Chart.SelectedItems.Clear()
        For Each ci As ChartItem In lv_Chart.Items
            If vTarget.Source.Contains(ci.MolecularInfo) Then lv_Chart.SelectedItems.Add(ci)
        Next
        lv_Chart.SourceMode = True
        'enter key to stop source mode
    End Sub

    Public Sub ExitMode()
        lv_Chart.SelectedItems.Clear()
        lv_Chart.SelectedItems.Add(pvMain.PrpC.RelatedChartItem)
        lv_Chart.SourceMode = False
        pvMain.PrpC.SetSource()
    End Sub
    Public Sub AcceptMode()
        If (vTarget Is Nothing) Or (Not lv_Chart.SourceMode) Then Exit Sub
        vTarget.Source.Clear()
        For Each ci As ChartItem In lv_Chart.SelectedItems
            vTarget.Source.Add(ci.MolecularInfo)
        Next
        lv_Chart.SelectedItems.Clear()
        lv_Chart.SelectedItems.Add(pvMain.PrpC.RelatedChartItem)
        lv_Chart.SourceMode = False
        pvMain.PrpC.SetSource()
        Changed = True
    End Sub

    Public Event GroupCopy(ByVal sender As Object, ByVal e As GroupCopyEventArgs)

    Private Sub tsmCopyGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmCopyGroup.Click
        OnGroupCopy()
    End Sub

    Public Sub OnGroupCopy()
        Dim Group As New List(Of DNAInfo)
        For Each ci As ChartItem In Me.lv_Chart.SelectedItems
            Group.Add(ci.MolecularInfo)
        Next
        SettingEntry.GroupCopy = DuplicateGroup(Group)
        SettingEntry.GroupHost = Me
    End Sub
    ''' <summary>
    ''' 在接收方的项目当中复制这些元件
    ''' </summary>
    ''' <param name="cList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DuplicateGroup(ByVal cList As List(Of DNAInfo)) As List(Of DNAInfo)
        Dim Group As New List(Of DNAInfo)
        Dim DNADict As New Dictionary(Of DNAInfo, DNAInfo)

        Dim cl As DNAInfo
        For Each ci As DNAInfo In cList
            cl = ci.Clone
            cl.Source = New List(Of DNAInfo)
            cl.DNAs = New Collection
            For Each gf As Nuctions.GeneFile In ci.DNAs
                cl.DNAs.Add(gf.CloneWithoutFeatures)
            Next
            Nuctions.AddFeatures(cl.DNAs, FeatureCol)
            DNADict.Add(ci, cl)
        Next
        For Each di As DNAInfo In DNADict.Keys
            For Each si As DNAInfo In di.Source
                If DNADict.ContainsKey(si) Then
                    DNADict(di).Source.Add(DNADict(si))
                End If
            Next
        Next
        For Each di As DNAInfo In DNADict.Values
            Group.Add(di)
        Next
        Return Group
    End Function
    Public Sub GroupPaste()
        If SettingEntry.GroupHost Is Nothing OrElse SettingEntry.GroupCopy Is Nothing OrElse SettingEntry.GroupCopy.Count = 0 Then Exit Sub
        Dim cList As List(Of DNAInfo) = SettingEntry.GroupHost.DuplicateGroup(SettingEntry.GroupCopy)
        If cList Is Nothing Then Exit Sub
        If cList.Count > 0 Then
            Dim px As Single = lv_Chart.MenuLocation.X

            Dim py As Single = lv_Chart.MenuLocation.Y

            Dim dx As Single = px - cList(0).DX
            Dim dy As Single = py - cList(0).DY

            Dim vSource As New List(Of DNAInfo)

            For Each ci As ChartItem In lv_Chart.SelectedItems
                vSource.Add(ci.MolecularInfo)
            Next

            For Each di As DNAInfo In cList
                di.DX += dx
                di.DY += dy
                Select Case di.MolecularOperation
                    Case Nuctions.MolecularOperationEnum.Vector, Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.FreeDesign
                    Case Else
                        If di.Source.Count = 0 Then
                            For Each scrdi As DNAInfo In vSource
                                If scrdi IsNot di Then di.Source.Add(scrdi)
                            Next
                        End If
                End Select
                For Each gf As Nuctions.GeneFile In di.DNAs
                    LoadFeature(gf)
                Next
                lv_Chart.Add(di, EnzymeCol)
            Next
            lv_Chart.Draw()
        End If
    End Sub
    Private Sub tsmPasteGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPasteGroup.Click
        GroupPaste()
        Changed = True
    End Sub

    Dim SwitchDistance As Integer

    Dim Switched As Boolean = False
    Public Sub SwitchSplitter()
        If Switched Then
            scMain.SplitterDistance = SwitchDistance
            Switched = False
        Else
            SwitchDistance = scMain.SplitterDistance
            scMain.SplitterDistance = 100
            Switched = True
        End If
    End Sub

    Private Sub CopySequenceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySequenceToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count = 1 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
            Dim gf As Nuctions.GeneFile = lv_Chart.SelectedItems(0).MolecularInfo.DNAs(1)
            Dim copied As Boolean = False
            While Not copied
                Try
                    Clipboard.SetDataObject(gf.Sequence, True, 10, 50)
                    copied = True
                Catch ex As Exception

                End Try
                Application.DoEvents()
            End While
        End If
    End Sub

    Private Sub RemarkFeaturesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemarkFeaturesToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 Then
            For Each ci As ChartItem In lv_Chart.SelectedItems
                Nuctions.AddFeatures(ci.MolecularInfo.DNAs, FeatureCol)
            Next
        End If
    End Sub

    Private Sub ExportSelectionWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportSelectionWToolStripMenuItem.Click
        If sfdEMF.ShowDialog = DialogResult.OK Then
            Dim fs As OperationView = lv_Chart
            fs.SaveSelectionPictureTo(sfdEMF.FileName)
        End If
    End Sub

    Private Sub ExportAllZToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportAllZToolStripMenuItem.Click
        If sfdEMF.ShowDialog = DialogResult.OK Then
            Dim fs As OperationView = lv_Chart
            fs.SavePictureTo(sfdEMF.FileName)
        End If
    End Sub
    Private Sub ExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.Click

        If lv_Chart.SelectedItems.Count = 1 Then
            Dim info = lv_Chart.SelectedItems(0).MolecularInfo
            sfdGeneBank.FileName = info.Name
            If sfdGeneBank.ShowDialog = DialogResult.OK Then
                If sfdGeneBank.FileName.EndsWith(".gb") Then
                    If info.DNAs.Count = 1 Then
                        Dim gf As Nuctions.GeneFile = info.DNAs(1)
                        gf.WriteToFile(sfdGeneBank.FileName)
                    ElseIf info.DNAs.Count > 1 Then
                        Dim index As Integer = 1
                        Dim basefilename As String = sfdGeneBank.FileName.Substring(0, sfdGeneBank.FileName.Length - 3)
                        For Each gf As Nuctions.GeneFile In info.DNAs
                            gf.WriteToFile(basefilename + index.ToString + ".gb")
                            index += 1
                        Next
                    End If
                ElseIf sfdGeneBank.FileName.EndsWith(".fasta") Then
                    Dim stb As New System.Text.StringBuilder
                    For Each gf As Nuctions.GeneFile In info.DNAs
                        stb.AppendFormat(">{0}", gf.Name)
                        stb.AppendLine()
                        Dim seq = gf.Sequence
                        Dim l = seq.Length
                        For i As Integer = 0 To seq.Length - 1 Step 100
                            stb.AppendLine(seq.Substring(i, Math.Min(100, l - i)))
                        Next
                    Next
                    IO.File.WriteAllText(sfdGeneBank.FileName, stb.ToString)
                End If
            End If
        End If

    End Sub
    Private Sub ViewTmBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTmBToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count = 1 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
            SettingEntry.AddTmViewerTab(lv_Chart.SelectedItems(0).MolecularInfo.DNAs(1))
        End If
    End Sub
    Private Sub AnalyzeCDSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnalyzeCDSToolStripMenuItem.Click
        Dim orf As New frmORF With {.Owner = Me.ParentForm, .StartPosition = FormStartPosition.CenterParent}
        If orf.ShowDialog = DialogResult.OK Then
            Dim options As ORFSearchOptions = orf.ORFOptions

            Dim startCodons As New List(Of String)
            If options.UseATG Then startCodons.Add("ATG")
            If options.UseCTG Then startCodons.Add("CTG")
            If options.UseGTG Then startCodons.Add("GTG")
            If options.UseTTG Then startCodons.Add("TTG")

            If lv_Chart.SelectedItems.Count > 0 Then
                For Each ci In lv_Chart.SelectedItems
                    Nuctions.SearchORFs(ci.AllGeneFiles, startCodons, options.MinimalLength, FeatureCol)
                    ci.Reload(ci.MolecularInfo, ci.DisplayedEnzymes)
                Next
            End If

        End If
        lv_Chart.Draw()

    End Sub


    Private Sub CopySelectedVectorMapDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySelectedVectorMapDToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
            If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then

                Dim bmp As New Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim g As Graphics = Graphics.FromImage(bmp)
                Dim sz As SizeF = lv_Chart.SelectedItems(0).DrawMap(g)
                g.Dispose()
                bmp.Dispose()


                bmp = New Bitmap(Math.Ceiling(sz.Width), Math.Ceiling(sz.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                g = Graphics.FromImage(bmp)

                Dim emf As New System.Drawing.Imaging.Metafile(New System.IO.MemoryStream, g.GetHdc, Imaging.EmfType.EmfPlusDual)
                Dim eg As Graphics = Graphics.FromImage(emf)
                lv_Chart.SelectedItems(0).DrawMap(eg)
                eg.Dispose()
                g.ReleaseHdc()
                ClipboardMetafileHelper.PutEnhMetafileOnClipboard(Me.ParentForm.Handle, emf)
            End If
        End If
    End Sub

    Private Sub ExportSelectedVectorJToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportSelectedVectorJToolStripMenuItem.Click
        If sfdEMF.ShowDialog = DialogResult.OK Then
            If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then

                Dim bmp As New Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim g As Graphics = Graphics.FromImage(bmp)
                Dim sz As SizeF = lv_Chart.SelectedItems(0).DrawMap(g)
                g.Dispose()
                bmp.Dispose()


                bmp = New Bitmap(Math.Ceiling(sz.Width), Math.Ceiling(sz.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                g = Graphics.FromImage(bmp)

                Dim emf As New System.Drawing.Imaging.Metafile(sfdEMF.FileName, g.GetHdc, Imaging.EmfType.EmfPlusDual)
                Dim eg As Graphics = Graphics.FromImage(emf)
                lv_Chart.SelectedItems(0).DrawMap(eg)
                eg.Dispose()
                emf.Dispose()
                g.ReleaseHdc()
                bmp.Dispose()
            End If
        End If
    End Sub

    Private Sub HashPickHToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HashPickHToolStripMenuItem.Click
        AddNewOperation(sender.tag, True)
    End Sub


    Private Sub pvMain_RequireUpdateView(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.RequireUpdateView
        lv_Chart.Draw()
    End Sub

    Private Sub pvMain_RequireSummary(ByVal sender As Object, ByVal e As SummaryEventArgs) Handles pvMain.RequireSummary
        Dim stb As New System.Text.StringBuilder
        GenerateSummary(stb, e)
        e.Summary = stb.ToString
        'e.Append = stb.AppendText
    End Sub

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
#If ReaderMode = 1 Then
        tsmCopyGroup.Visible = False
        tsmPasteGroup.Visible = False
        EnzymeAnalysisAToolStripMenuItem.Visible = False
        EnzymeDigestionEToolStripMenuItem.Visible = False
        GelSeparationGToolStripMenuItem.Visible = False
        HashPickHToolStripMenuItem.Visible = False
        LigationLToolStripMenuItem.Visible = False
        MergeXToolStripMenuItem.Visible = False
        ModificationMToolStripMenuItem.Visible = False
        PCRRToolStripMenuItem.Visible = False
        RecombinationCToolStripMenuItem.Visible = False
        TransformationScreenTToolStripMenuItem.Visible = False
        FreeDesignFToolStripMenuItem.Visible = False
        SequencingSToolStripMenuItem.Visible = False
        CompareToolStripMenuItem.Visible = False
        tssOperations.Visible = False
        RecalculateAllChildrenToolStripMenuItem.Visible = False
        tssRecalculate.Visible = False
#End If
#If ServerConnect Then
#If EnableAd Then
        AddHandler lv_Chart.SelectedIndexChanged, AddressOf Advertisement.UpdateSelectedItems
        Advertisement.ParentWorkControl = Me
#End If
#End If

    End Sub

    Private Sub GetConstructionDescriptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetConstructionDescriptionToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 Then
            Dim vRoots As New List(Of DNAInfo)
            For Each ci As ChartItem In lv_Chart.SelectedItems
                vRoots.Add(ci.MolecularInfo)
            Next
            'Dim ssb As New System.Text.StringBuilder
            'GenerateSummary(ssb)
            'ssb.Clear()
            'Dim stb As New System.Text.StringBuilder
            'Dim vList As New List(Of DNAInfo)
            'Dim vRoots As New List(Of DNAInfo)
            'Dim vTreeStack As Stack(Of DNAInfo)
            'Dim vIterateStack As New Stack(Of DNAInfo)
            'Dim vDependenceQueue As New Queue(Of Stack(Of DNAInfo))
            'For Each ci As ChartItem In lv_Chart.SelectedItems
            '    vRoots.Add(ci.MolecularInfo)
            'Next
            'Dim di As DNAInfo
            'While vRoots.Count > 0
            '    di = vRoots(0)
            '    vRoots.Remove(di)
            '    vTreeStack = New Stack(Of DNAInfo)
            '    vTreeStack.Push(di)
            '    di.TraceSourceStack(vIterateStack, vList, vRoots, vTreeStack)
            '    vDependenceQueue.Enqueue(vTreeStack)
            'End While
            'vList.Clear()
            'While vDependenceQueue.Count > 0
            '    vTreeStack = vDependenceQueue.Dequeue
            '    While vTreeStack.Count > 0
            '        di = vTreeStack.Pop
            '        stb.Append(di.GetConstructionDescription(vList))
            '    End While
            'End While
            Dim cd As String = GetDescripotion(vRoots)
            Dim copied As Boolean = False
            While Not copied
                Try
                    Clipboard.SetDataObject(cd, True, 10, 50)
                    copied = True
                Catch ex As Exception

                End Try
                Application.DoEvents()
            End While
        End If
    End Sub

    Private Function GetDescripotion(ByVal Sources As List(Of DNAInfo), Optional ByVal Summary As Boolean = True) As String
        Dim vRoots As New List(Of DNAInfo)

        Dim infolist As New List(Of DNAInfo)
        For Each ci As ChartItem In lv_Chart.Items
            infolist.Add(ci.MolecularInfo)
        Next
        Dim CountList As New Dictionary(Of DNAInfo, Integer)
        For Each dit As DNAInfo In infolist
            CountList.Add(dit, 0)
        Next
        For Each dit As DNAInfo In infolist
            For Each sc As DNAInfo In dit.Source
                CountList(sc) += 1
            Next
        Next
        For Each dit As DNAInfo In infolist
            If CountList(dit) = 0 Then vRoots.Add(dit)
        Next
        Dim stb As New System.Text.StringBuilder
        Dim vList As New List(Of DNAInfo)
        Dim vStack As Stack(Of DNAInfo)
        vList.AddRange(Sources)
        Dim dii As DNAInfo
        Dim vVisited As New List(Of DNAInfo)

        Dim vQueue As New Queue(Of Stack(Of DNAInfo))

        While vRoots.Count > 0
            '这里面有个逻辑关系
            '从vRoots出发时，先访问的东西在后面不需要再访问。
            '所以先访问的东西先构建 因为把它们添加到以vRoots中元素为顺序的Queue当中
            dii = vRoots(0)
            vStack = New Stack(Of DNAInfo)
            dii.TraceDependencyStack(vStack, vList, vVisited)
            If vStack.Count > 0 Then vQueue.Enqueue(vStack)
            If vRoots.Contains(dii) Then vRoots.Remove(dii)
        End While
        While vQueue.Count > 0
            vStack = vQueue.Dequeue
            While vStack.Count > 0
                stb.Append(vStack.Pop.ConstructionProcess(Summary))
            End While
        End While
        Return stb.ToString
    End Function

    Private Sub ConvertToFreeDesignToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToFreeDesignToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 Then
            For Each ci As ChartItem In lv_Chart.SelectedItems
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim di As DNAInfo = ci.MolecularInfo
                    di.Source.Clear()
                    di.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign
                    Dim gf As Nuctions.GeneFile = di.DNAs(1)
                    di.FreeDesignCode = gf.ToFreeDesignCode
                    di.Calculate()
                    ci.Reload(di, EnzymeCol)
                End If
            Next
            If lv_Chart.SelectedItems.Count = 1 Then
                Dim li As New List(Of ChartItem)
                li.AddRange(lv_Chart.SelectedItems)
                pvMain.SelectItem = li
            End If
            lv_Chart.Draw()
        End If
    End Sub

    Private Sub ConvertToHostToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToHostToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 Then
            For Each ci As ChartItem In lv_Chart.SelectedItems
                If ci.MolecularInfo.Cells.Count = 1 Then
                    Dim di As DNAInfo = ci.MolecularInfo
                    di.Source.Clear()
                    di.MolecularOperation = Nuctions.MolecularOperationEnum.Host
                    ci.Reload(di, EnzymeCol)
                End If
            Next
            If lv_Chart.SelectedItems.Count = 1 Then
                Dim li As New List(Of ChartItem)
                li.AddRange(lv_Chart.SelectedItems)
                pvMain.SelectItem = li
            End If
            lv_Chart.Draw()
        End If
    End Sub

    Private Sub ConvertToVectorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToVectorToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count > 0 Then
            For Each ci As ChartItem In lv_Chart.SelectedItems
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim di As DNAInfo = ci.MolecularInfo
                    di.Source.Clear()
                    di.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
                    ci.Reload(di, EnzymeCol)
                End If
            Next
            If lv_Chart.SelectedItems.Count = 1 Then
                Dim li As New List(Of ChartItem)
                li.AddRange(lv_Chart.SelectedItems)
                pvMain.SelectItem = li
            End If
            lv_Chart.Draw()
        End If
    End Sub

    Private Sub pvMain_RequirePrimer(sender As Object, e As PrimerEventArgs) Handles pvMain.RequirePrimer
        e.Primers = lv_Chart.Primers
    End Sub

    Private Sub CopySelectionToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles CopySelectionToolStripMenuItem.Click
        lv_Chart.CopySelectionAsEMF()
    End Sub

    Private Sub pvMain_RequireHost(sender As Object, e As HostEventArgs) Handles pvMain.RequireHost
        e.Hosts = lv_Chart.Hosts
    End Sub

    Private Sub pvMain_ReqireWorkControl(sender As Object, e As WorkControlEventArgs) Handles pvMain.ReqireWorkControl
        e.WorkControl = Me
    End Sub


#Region "添加预定义的Host and Feature"
    ''' <summary>
    ''' 响应PropertyView的IncludeCommonDefination事件。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pvMain_IncludeCommonDefination(sender As Object, e As System.EventArgs) Handles pvMain.IncludeCommonDefination
        IncludeCommonDefination()
    End Sub
    ''' <summary>
    ''' 将NativeDefine当中定义的Host和Feature添加到系统当中。
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub IncludeCommonDefination()
        For Each ft As Nuctions.Feature In NativeOrigin.NativeOrigins
            AddNativeFeature(ft)
        Next
        For Each ft As Nuctions.Feature In NativeAntibioticResistance.NativeAntibioticResistances
            AddNativeFeature(ft)
        Next
        For Each ft As Nuctions.Feature In NativePrimase.NativePrimases
            AddNativeFeature(ft)
        Next
        For Each h As Nuctions.Host In NativeHost.NativeHosts
            AddHost(h)
        Next
    End Sub
    Private Sub AddNativeFeature(newFeature As Nuctions.Feature)
        Dim contains As Boolean = False
        Dim rmList As New List(Of Nuctions.Feature)
        For Each ft As Nuctions.Feature In FeatureCol
            If ft.Sequence = newFeature.Sequence Or ft.RCSequence = newFeature.RCSequence Then
                rmList.Add(ft)
            End If
        Next
        For Each ft As Nuctions.Feature In rmList
            FeatureCol.Remove(ft)
        Next
        FeatureCol.Add(newFeature)
    End Sub
    Private Sub AddHost(newHost As Nuctions.Host)
        Dim contains As Boolean = False
        For Each ht As Nuctions.Host In lv_Chart.Hosts
            If ht.Name = newHost.Name Then contains = True : Exit For
        Next
        If Not contains Then
            lv_Chart.Hosts.Add(newHost)
        End If
    End Sub
#End Region

    Public Published As Boolean
    Public PublicationID As Integer
    Public Quoted As Boolean
    Public QuotationID As Integer

    Public Sub PresentSummary(sec As SummarySectionEnum)
        pvMain.PresentSummary(sec)
    End Sub

#Region "自动保存功能"
    Private WithEvents AutoSaveTimer As New System.Windows.Forms.Timer() With {.Enabled = True, .Interval = 1000 * 60 * 10}
    Private Sub AutoSave(obj As Object, e As EventArgs) Handles AutoSaveTimer.Tick
        Me.ParentForm.Invoke(New Action(Of WorkControl)(AddressOf AutoSaveWorkControl), New Object() {Me})
    End Sub
    Private TempFilename As String
    Private Sub AutoSaveWorkControl(wc As WorkControl)
        Dim lastTemp As String = TempFilename
        Try
            Dim fi As New IO.FileInfo(FileAddress)
            If fi.Exists Then
                TempFilename = fi.FullName + ".tmp"
            ElseIf TempFilename Is Nothing OrElse TempFilename = "" OrElse Not IO.File.Exists(TempFilename) Then
                If Not IO.Directory.Exists(Application.StartupPath + "\Temp") Then IO.Directory.CreateDirectory(Application.StartupPath + "\Temp")
                TempFilename = Application.StartupPath + "\Temp\" + Now.ToString("yyyy-MM-dd HH-mm-ss-ffff") + ".tmp"
            End If
        Catch ex As Exception
            If Not IO.Directory.Exists(Application.StartupPath + "\Temp") Then IO.Directory.CreateDirectory(Application.StartupPath + "\Temp")
            TempFilename = Application.StartupPath + "\Temp\" + Now.ToString("yyyy-MM-dd HH-mm-ss-ffff") + ".tmp"
        End Try
        Try
            If lastTemp IsNot Nothing AndAlso lastTemp <> TempFilename AndAlso IO.File.Exists(lastTemp) Then
                IO.File.Delete(lastTemp)
            End If
        Catch ex As Exception

        End Try
        Try
            SettingEntry.SaveToZXML(New Dictionary(Of String, Object) From {{"WorkChart", Me.GetWorkSpace}}, TempFilename)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ClearnTempFile()
        Try
            AutoSaveTimer.Enabled = False
            AutoSaveTimer.Dispose()
            If IO.File.Exists(TempFilename) Then IO.File.Delete(TempFilename)
        Catch ex As Exception

        End Try
    End Sub


#End Region


End Class
Friend Class WorkControlEventArgs
    Inherits EventArgs
    Friend WorkControl As WorkControl
End Class
Public Enum SummarySectionEnum As Integer
    Project
    Strain
    Vector
    Enzyme
    Tool
    Primer
    Synthesis
    Protocol
End Enum