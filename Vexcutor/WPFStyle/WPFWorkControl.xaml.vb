Public Class WPFWorkControl
    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        SetValue(OperationViewPropertyKey, WinFormOperationView)
    End Sub

    Private WithEvents WorkSpaceViewModel As WPFWorkSpaceViewModel
    Private Sub WPFWorkControl_DataContextChanged(sender As Object, e As Windows.DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
        WorkSpaceViewModel = e.NewValue
    End Sub

    'WPFWorkControl -> OperationView As OperationView Default: Nothing
    Public ReadOnly Property OperationView As OperationView
        Get
            Return GetValue(WPFWorkControl.OperationViewProperty)
        End Get
    End Property
    Private Shared ReadOnly OperationViewPropertyKey As System.Windows.DependencyPropertyKey = _
                              System.Windows.DependencyProperty.RegisterReadOnly("OperationView", _
                              GetType(OperationView), GetType(WPFWorkControl), _
                              New System.Windows.PropertyMetadata(Nothing))
    Public Shared ReadOnly OperationViewProperty As System.Windows.DependencyProperty = _
                             OperationViewPropertyKey.DependencyProperty


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
        If OperationView.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = WinFormOperationView.SelectedItems(0)
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

    Private Sub ViewVToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If WinFormOperationView.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = WinFormOperationView.SelectedItems(0)
            Dim DNA As Nuctions.GeneFile
            Dim i As Integer = 0
            For Each DNA In ci.MolecularInfo.DNAs
                SettingEntry.AddDNAViewTab(DNA, EnzymeCol)
                i += 1
            Next
        End If
    End Sub
#Region "IO操作"
    '获得文件记录 /// WPFWorkSpace将是这个空间的DataContext
    'Public Function GetWorkSpace() As WorkSpace
    '    Dim WS As New WorkSpace
    '    Dim Citems As New List(Of DNAInfo)
    '    For Each o As ChartItem In WinFormOperationView.Items
    '        Citems.Add(o.MolecularInfo)
    '    Next

    '    WS.Scale = WinFormOperationView.ScaleValue
    '    WS.OffsetX = WinFormOperationView.Offset.X
    '    WS.OffsetY = WinFormOperationView.Offset.Y

    '    WS.ChartItems.AddRange(Citems)

    '    WS.Features.AddRange(FeatureCol)
    '    Dim cenzs As New List(Of String)
    '    For Each enz As String In EnzymeCol
    '        cenzs.Add(enz)
    '    Next
    '    WS.Enzymes.AddRange(cenzs)

    '    WS.Summary = pvMain.rtbSummary.Text
    '    WS.PrintPages = WinFormOperationView.PrintPages
    '    WS.PrintView = WinFormOperationView.PrintView
    '    WS.PrimerList = WinFormOperationView.Primers
    '    WS.Hosts = WinFormOperationView.Hosts
    '    WS.Published = Published
    '    WS.PublicationID = PublicationID
    '    WS.Quoted = Quoted
    '    WS.QuotationID = QuotationID
    '    WS.ProjectServiceStatus = ProjectServiceStatus
    '    Return WS
    'End Function
    '保存实验到指定文件
    '    Public Sub SaveTo(ByVal filename As String)
    '#If ReaderMode = 0 Then
    '        Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", GetWorkSpace()}}

    '        WPFEntry.SaveToZXML(DC, filename)

    '        Dim fi As New IO.FileInfo(filename)

    '        ParentTab.Text = fi.Name
    '        Changed = False
    '        FileAddress = filename
    '#End If
    '    End Sub

    '    '保存成为字节流
    '    Public Function SaveToBytes() As Byte()
    '        Dim DC As New Dictionary(Of String, Object) From {{"WorkChart", GetWorkSpace()}}
    '        Return WPFEntry.SaveToZXMLBytes(DC)
    '    End Function
    '    Private vFileAddress As String = ""
    '    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    '    Friend Property FileAddress As String
    '        Get
    '            Return vFileAddress
    '        End Get
    '        Set(ByVal value As String)
    '            pvMain.lbFileAddress.Text = String.Format("[File Location] - {0}", value)
    '            vFileAddress = value
    '        End Set
    '    End Property

    '在远程保存的过程中将最先尝试这个密码组合
    Friend RemoteUserName As String = ""
    Friend RemotePassword As String = ""
    '标记当前的保存中这个账户和密码是否有效
    Friend AccountWorking As Boolean = False
    Friend ProjectServiceStatus As ProjectServiceStatusEnum = ProjectServiceStatusEnum.None
    '配置工作区文件
    Friend Shared Function SetWorkControl(ByVal WS As WorkSpace, ByVal filename As String, Optional ByVal vUserName As String = "", Optional ByVal vPassword As String = "") As WorkControl
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
    Friend Shared Function LoadFrom(ByVal filename As String) As WorkControl
        'do not load a file if there are already files.
        'filename tcp://127.0.0.1/1/Vectors/pBluescript.vct

        Dim vList As New List(Of String) From {"WorkChart"}
        Dim WS As WorkSpace = SettingEntry.LoadFromZXML(vList, filename)("WorkChart")
        If WS Is Nothing Then Return Nothing
        Return SetWorkControl(WS, filename)
    End Function
    Friend Shared Function LoadFrom(ByVal buf As Byte(), ByVal filename As String, Optional ByVal vUserName As String = "", Optional ByVal vPassword As String = "") As WorkControl
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

    'Public ReadOnly Property ContentChanged() As Boolean
    '    Get
    '        Return Changed
    '    End Get
    'End Property
#End Region

    Private Sub WorkChart_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        Select Case MsgBox("Do you want save before closing the current file?", MsgBoxStyle.YesNoCancel, "File Closing")
            Case MsgBoxResult.Yes
                WorkSpaceViewModel.Save()
            Case MsgBoxResult.No
                'the file will be lost
            Case MsgBoxResult.Cancel
                e.Cancel = True
        End Select
    End Sub


    Public Sub DeleteItem(Optional ByVal Query As Boolean = True)
        If ReaderMode Then Exit Sub
        If Query Then
            If MsgBox("Do You Really Want to Delete All the Selected Items?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Else
                Exit Sub
            End If
        End If
        Dim ci As ChartItem
        Dim delCol As New List(Of ChartItem)
        For Each ci In WinFormOperationView.SelectedItems
            delCol.Add(ci)
        Next
        For Each ci In delCol
            WinFormOperationView.Remove(ci)
        Next
        WinFormOperationView.SelectedItems.Clear()
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

    'Private Sub lv_Chart_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.PositionChanged
    '    Changed = True
    'End Sub


    '加载文件时绘图


    '选中操作
    'Private Sub lv_Chart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.SelectedIndexChanged
    '    If WinFormOperationView.SourceMode Then
    '        vTarget.Source.Clear()
    '        For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '            vTarget.Source.Add(ci.MolecularInfo)
    '        Next
    '        WinFormOperationView.Draw()
    '    Else
    '        Dim sitems As New List(Of ChartItem)

    '        For Each it As ChartItem In WinFormOperationView.SelectedItems
    '            sitems.Add(it)
    '        Next
    '        pvMain.SelectItem = sitems
    '        WinFormOperationView.Focus()
    '    End If
    'End Sub
#Region "打印页面处理"
    '以下这些业务逻辑将转移到ViewModel当中

    'Private Sub pvMain_AddPrintPage(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.AddPrintPage
    '    AddPrintPage()
    'End Sub

    'Private Sub pvMain_DirectPrintAllPages(sender As Object, e As System.EventArgs) Handles pvMain.DirectPrintAllPages
    '    WinFormOperationView.PrintPage(WinFormOperationView.PrintPages, True)
    'End Sub

    'Private Sub pvMain_DirectPrintSelectedPages(sender As Object, e As System.EventArgs) Handles pvMain.DirectPrintSelectedPages
    '    WinFormOperationView.PrintPage(WinFormOperationView.SelectedPrintPages, True)
    'End Sub
    'Private Sub pvMain_PrintAllPages(sender As Object, e As System.EventArgs) Handles pvMain.PrintAllPages
    '    WinFormOperationView.PrintPage(WinFormOperationView.PrintPages)
    'End Sub
    'Private Sub pvMain_PrintSelectedPages(sender As Object, e As System.EventArgs) Handles pvMain.PrintSelectedPages
    '    WinFormOperationView.PrintPage(WinFormOperationView.SelectedPrintPages)
    'End Sub
    'Public Sub AddPrintPage()
    '    If WinFormOperationView.PrintView Then '只在打印模式下才能添加打印页面
    '        Dim pp As New PrintPage
    '        pp.Text = "New Page"
    '        pp.PageID = WinFormOperationView.PrintPages.Count + 1
    '        'pp.Left = lv_Chart.Offset.X
    '        'pp.Top = lv_Chart.Offset.Y
    '        pp.Width = pp.PrintPixelWidth
    '        pp.Height = pp.PrintPixelHeight
    '        pp.Left = (WinFormOperationView.Width / 2.0F) / WinFormOperationView.ScaleValue - lv_Chart.Offset.X - pp.Width / 2.0F
    '        pp.Top = (WinFormOperationView.Height / 2.0F) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y - pp.Height / 2.0F
    '        WinFormOperationView.PrintPages.Add(pp)
    '        WinFormOperationView.SelectPrintPage(pp)
    '        WinFormOperationView.Draw()
    '    End If
    'End Sub
    '<System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    'Public Property PrintMode As Boolean
    '    Get
    '        Return WinFormOperationView.PrintView
    '    End Get
    '    Set(ByVal value As Boolean)
    '        WinFormOperationView.PrintView = value
    '        WinFormOperationView.Draw()
    '        If value Then pvMain.SetPrintPageView(value)
    '    End Set
    'End Property
    'Public Sub Print()

    'End Sub
    'Private Sub lv_Chart_SelectedPrintPageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.SelectedPrintPageChanged
    '    pvMain.SelectPage = WinFormOperationView.SelectedPrintPages
    'End Sub

#End Region
#Region "PropertyView 指令事件"



    'Private Sub pvMain_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.Close
    '    Close()
    'End Sub

    'Private Sub pvMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.Load

    'End Sub

    'Private Sub pvMain_LoadExperiment(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadExperiment
    '    LoadExperiment()
    'End Sub
    'Private Sub pvMain_LoadGeneFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadGeneFile
    '    LoadVector()
    'End Sub
    'Private Sub pvMain_LoadSequenceFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadSequenceFile

    '    If ofdSequence.ShowDialog = DialogResult.OK Then
    '        LoadSequenceFromFile(ofdSequence.FileName)
    '    End If
    'End Sub

    'Private Sub pvMain_LoadSequencingResultFile(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.LoadSequencingResultFile

    'End Sub

    'Private Sub pvMain_ManageEnzymes(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles pvMain.ManageEnzymes
    '    EnzymeCol = e.RESites
    '    pvMain.ShowEnzymes()
    '    WinFormOperationView.SetEnzymes(EnzymeCol)

    'End Sub

    'Private Sub pvMain_ManageFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles pvMain.ManageFeatures
    '    Me.FeatureCol.Clear()
    '    For Each ft As Nuctions.Feature In e.Features
    '        Me.FeatureCol.Add(ft)
    '    Next
    'End Sub

    'Private Sub pvMain_ReqireFeatures(ByVal sender As Object, ByVal e As FeatureEventArgs) Handles pvMain.ReqireFeatures
    '    Dim ftList As New List(Of Nuctions.Feature)
    '    For Each ft As Nuctions.Feature In FeatureCol
    '        ftList.Add(ft)
    '    Next
    '    e.Features = ftList
    'End Sub

    'Private Sub pvMain_RequireEnzymeSite(ByVal sender As Object, ByVal e As RestrictionEnzymeView.RESiteEventArgs) Handles pvMain.RequireEnzymeSite
    '    e.RESites = EnzymeCol
    'End Sub

    'Private Sub pvMain_SaveExperiment(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.SaveExperiment
    '    Save()
    'End Sub

    'Private Sub pvMain_SaveExperimentAs(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.SaveExperimentAs
    '    SaveAs()
    'End Sub
#End Region

#Region "功能指令"
    '    Public Event CloseWorkControl(ByVal sender As Object, ByVal e As EventArgs)
    '    Public ReadOnly Property CurrentFileName() As String
    '        Get
    '            Return FileAddress
    '        End Get
    '    End Property
    '    Public Sub Close()
    '        '询问关闭前是否保存
    '        RaiseEvent CloseWorkControl(Me, New EventArgs)

    '    End Sub
    '    Public Event LoadWorkControl(ByVal sender As Object, ByVal e As EventArgs)
    '    '加载文件
    '    Public Sub LoadExperiment()
    '        RaiseEvent LoadWorkControl(Me, New EventArgs)
    '    End Sub
    '    '保存文件
    '    Public Sub Save()
    '#If ReaderMode = 0 Then
    '        If FileAddress Is Nothing OrElse FileAddress = "" Then
    '            SaveAs()
    '        ElseIf FileAddress.StartsWith("tcp:\\") Then
    '            'tcp://user#
    '            SaveToServer()
    '        Else
    '            SaveTo(FileAddress)
    '        End If
    '#End If
    '    End Sub
    '    '另存文件
    '    Public Sub SaveAs()
    '#If ReaderMode = 0 Then
    '        If ReaderMode Then Exit Sub
    '        If sfdProject.ShowDialog = DialogResult.OK Then
    '            SaveTo(sfdProject.FileName)
    '        End If
    '#End If
    '    End Sub

    '    Public Sub SaveToServer()
    '#If ReaderMode = 0 Then
    '        If ReaderMode Then Exit Sub
    '        If RemoteUserName Is Nothing OrElse RemoteUserName.Length = 0 OrElse RemotePassword Is Nothing OrElse RemotePassword.Length = 0 Then
    '            Stop
    '            'frmMain.SaveWorkControlAsToServer(Me, ParentTab.Text)
    '        Else
    '            Stop
    '            'frmMain.SaveWorkControlToServer(Me)
    '        End If
    '#End If
    '    End Sub

    '    '加载一个GeneBank文件
    '    Public Sub LoadVector()
    '        If Not ofdGeneFile.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub
    '        LoadVectorFromFile(ofdGeneFile.FileName)
    '    End Sub
    '    Public Sub LoadFeature(ByVal gf As Nuctions.GeneFile)
    '        Dim ft As Nuctions.Feature
    '        Dim exist As Boolean
    '        For Each gn As Nuctions.GeneAnnotation In gf.Features
    '            'Add the annotation to the collection so that we can store the features
    '            'The features are useful in the ligation and screen
    '            If gn.Type = "f_end" OrElse gn.Type = "r_end" Then Continue For

    '            gn.Vector = gf
    '            ft = New Nuctions.Feature(gn.Label + " <" + gf.Name + "> (" + FeatureCol.Count.ToString + ")", gn.GetSuqence, gn.Label, gn.Type, gn.Note + " <" + gf.Name + ">")
    '            If gn.Feature IsNot Nothing Then
    '                For Each bf As Nuctions.FeatureFunction In gn.Feature.BioFunctions
    '                    ft.BioFunctions.Add(bf.Clone)
    '                Next
    '            End If
    '            exist = False
    '            For Each fta As Nuctions.Feature In FeatureCol
    '                If fta.Sequence = ft.Sequence Then
    '                    exist = True
    '                    Exit For
    '                End If
    '            Next
    '            If Not exist Then FeatureCol.Add(ft)



    '        Next
    '    End Sub
    '    Public Sub LoadVectorFromFile(ByVal sFilename As String)
    '        Dim vec As Nuctions.GeneFile
    '        If sFilename.ToLower.EndsWith(".gb") Then
    '            vec = Nuctions.GeneFile.LoadFromGeneBankFile(sFilename)
    '        ElseIf sFilename.EndsWith(".vct") Then
    '            Dim dict As Dictionary(Of String, Object) = WPFEntry.LoadFromZXML(New List(Of String) From {"DNA", "Enzyme"}, sFilename)
    '            vec = dict("DNA")
    '        End If
    '        If vec Is Nothing Then Exit Sub

    '        Dim ci As New DNAInfo
    '        LoadFeature(vec)

    '        ci.DNAs.Add(vec)
    '        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
    '        Dim fi As New IO.FileInfo(sFilename)

    '        ci.Name = fi.Name.Substring(0, fi.Name.LastIndexOf("."))
    '        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
    '        ci.File_Filename = sFilename
    '        'ci.MolecularInfo.FeatureCol = FeatureCol
    '        ci.DX = (WinFormOperationView.Width / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.X
    '        ci.DY = (WinFormOperationView.Height / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y
    '        WinFormOperationView.Add(ci, EnzymeCol)
    '        WinFormOperationView.Draw()
    '        Changed = True
    '    End Sub
    '    Public Sub LoadVectorFile(ByVal vec As Nuctions.GeneFile, Optional ByVal sFilename As String = "")
    '        Dim ci As New DNAInfo
    '        LoadFeature(vec)
    '        'Dim gn As Nuctions.GeneAnnotation
    '        'Dim ft As Nuctions.Feature
    '        'Dim fts As List(Of Nuctions.Feature) = FeatureCol
    '        'Dim exist As Boolean
    '        'For Each gn In vec.Features
    '        '    'Add the annotation to the collection so that we can store the features
    '        '    'The features are useful in the ligation and screen
    '        '    ft = New Nuctions.Feature(gn.Label + " <" + vec.Name + "> (" + fts.Count.ToString + ")", gn.GetSuqence, gn.Label, gn.Type, gn.Note + " <" + vec.Name + ">")
    '        '    exist = False
    '        '    For Each fta As Nuctions.Feature In fts
    '        '        If Not (fta.Useful = "Native") AndAlso fta = ft Then
    '        '            exist = True
    '        '            Exit For
    '        '        End If
    '        '    Next
    '        '    If Not exist Then fts.Add(ft)
    '        'Next
    '        ci.DNAs.Add(vec)
    '        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
    '        ci.Name = vec.Name
    '        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
    '        ci.File_Filename = sFilename
    '        'ci.MolecularInfo.FeatureCol = FeatureCol
    '        ci.DX = (WinFormOperationView.Width / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.X
    '        ci.DY = (WinFormOperationView.Height / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y
    '        WinFormOperationView.Add(ci, EnzymeCol)
    '        WinFormOperationView.Draw()
    '        Changed = True
    '    End Sub
    '    Public Sub LoadSequenceFromFile(ByVal sFilename As String)
    '        Dim str As String = Nuctions.TAGCFilter(IO.File.ReadAllText(sFilename))
    '        Dim fi As New System.IO.FileInfo(sFilename)
    '        LoadSequence(str, fi.Name, sFilename)
    '    End Sub
    '    Public Sub LoadSequence(ByVal seq As String, ByVal vName As String, Optional ByVal sFilename As String = "")
    '        Dim vec As New Nuctions.GeneFile
    '        vec.Name = vName
    '        vec.Sequence = seq
    '        vec.End_F = "*B"
    '        vec.End_R = "*B"
    '        'vec.Iscircular = False
    '        Dim ci As New DNAInfo
    '        ci.DNAs.Add(vec)
    '        If vName.ToLower.EndsWith(".seq") Or vName.ToLower.EndsWith(".txt") Then
    '            'analyze the sequence
    '            'to be done
    '            Nuctions.AddFeatures(ci.DNAs, FeatureCol)
    '        End If
    '        'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
    '        ci.Name = vec.Name
    '        ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
    '        ci.File_Filename = sFilename
    '        'ci.MolecularInfo.FeatureCol = FeatureCol
    '        ci.DX = (WinFormOperationView.Width / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.X
    '        ci.DY = (WinFormOperationView.Height / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y
    '        WinFormOperationView.Add(ci, EnzymeCol)
    '        WinFormOperationView.Draw()
    '        Changed = True
    '    End Sub
    '    Private Changed As Boolean = False

    '    Public Function TryCopyDNAs() As List(Of Nuctions.GeneFile)
    '#If ReaderMode = 0 Then
    '        Dim glist As New List(Of Nuctions.GeneFile)
    '        For Each si As ChartItem In WinFormOperationView.SelectedItems
    '            If si.MolecularInfo.DNAs.Count = 1 Then
    '                glist.Add(si.MolecularInfo.DNAs(1))
    '            End If
    '        Next
    '        Return glist
    '#Else
    '        Return New List(Of Nuctions.GeneFile)
    '#End If
    '    End Function
    '    Public Sub TryPasteDNAs(ByVal gList As List(Of Nuctions.GeneFile))
    '#If ReaderMode = 0 Then
    '        For Each gf As Nuctions.GeneFile In gList
    '            'vec.Iscircular = False
    '            Dim ci As New DNAInfo
    '            ci.DNAs.Add(gf)
    '            'If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
    '            ci.Name = gf.Name
    '            ci.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
    '            ci.File_Filename = "<None>"
    '            'ci.MolecularInfo.FeatureCol = FeatureCol
    '            ci.DX = (WinFormOperationView.Width / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.X
    '            ci.DY = (WinFormOperationView.Height / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y
    '            WinFormOperationView.Add(ci, EnzymeCol)
    '        Next
    '        WinFormOperationView.Draw()
    '        Changed = True
    '#End If
    '    End Sub
#End Region

    '处理右键菜单
    'Private Sub FunctionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeDigestionEToolStripMenuItem.Click,
    '    GelSeparationGToolStripMenuItem.Click, LigationLToolStripMenuItem.Click, ModificationMToolStripMenuItem.Click,
    '    PCRRToolStripMenuItem.Click, TransformationScreenTToolStripMenuItem.Click, RecombinationCToolStripMenuItem.Click,
    '    EnzymeAnalysisAToolStripMenuItem.Click, FreeDesignFToolStripMenuItem.Click, SequencingSToolStripMenuItem.Click, CompareToolStripMenuItem.Click
    '    AddNewOperation(sender.tag)
    'End Sub

    Public Function AddNewOperation(ByVal Method As Nuctions.MolecularOperationEnum, Optional ByVal AutoPosition As Boolean = False) As ChartItem
#If ReaderMode = 0 Then
        If WinFormOperationView.SelectedItems.Count = 0 And Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then Return Nothing
        Dim ci As New DNAInfo
        Dim ui As New ChartItem
        If Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then
            For Each ui In WinFormOperationView.SelectedItems
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
        End Select

        ci.DX = WinFormOperationView.MenuLocation.X
        ci.DY = WinFormOperationView.MenuLocation.Y
        Dim ch As ChartItem
        If AutoPosition Then
            If ci.Source.Count > 0 Then
                ch = WinFormOperationView.Add(ci, EnzymeCol)
                ch.Draw(Graphics.FromImage(New Bitmap(1, 1, Imaging.PixelFormat.Format32bppArgb)))
                ch.AutoFit()
            Else
                ci.DX = (WinFormOperationView.Width / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.X
                ci.DY = (WinFormOperationView.Height / 2) / WinFormOperationView.ScaleValue - WinFormOperationView.Offset.Y
                ch = WinFormOperationView.Add(ci, EnzymeCol)
            End If
        Else
            ch = WinFormOperationView.Add(ci, EnzymeCol)
        End If
        WinFormOperationView.SelectedItems.Clear()
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
                ci.DetermineHost(WinFormOperationView.Hosts)
            Case Nuctions.MolecularOperationEnum.Transformation
                ci.DetermineHost(WinFormOperationView.Hosts)
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
            Case Nuctions.MolecularOperationEnum.Incubation
                ci.DetermineHost(WinFormOperationView.Hosts)
            Case Nuctions.MolecularOperationEnum.Extraction
                ci.Calculate()
                ch.Reload(ci, EnzymeCol)
        End Select

        '表明已经修改过文件
        'Changed = True

        WinFormOperationView.Draw()
        Return ch
#Else
        Return Nothing
#End If
    End Function

    'Public Function CopySelectedSequence() As String
    '    Return pvMain.CopySelectSequence
    'End Function

    'Private Sub pvMain_LoadSequenceEvent(ByVal sender As Object, ByVal e As LoadSequenceEventArgs) Handles pvMain.LoadSequenceEvent
    '    LoadSequence(e.Sequence, "Sequence " + e.Sequence.Length.ToString)
    'End Sub

    Private Sub GenerateSummary(ByVal stb As System.Text.StringBuilder)
 
    End Sub

    Private Sub GenerateSummary(ByVal stb As RTFStringBuilder, smr As SummaryEventArgs)
      
    End Sub

    'Private Sub pvMain_ExportPrimerList(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.ExportProject
    '    Export()
    'End Sub
    'Friend Sub Export()

    'End Sub

    'Private Sub pvMain_RemarkFeature(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.RemarkFeature

    '    Dim geneCol As New List(Of Nuctions.GeneFile)

    '    For Each ci As ChartItem In WinFormOperationView.Items
    '        For Each gf As Nuctions.GeneFile In ci.MolecularInfo.DNAs
    '            If Not geneCol.Contains(gf) Then geneCol.Add(gf)
    '        Next
    '        For Each c As Nuctions.Cell In ci.MolecularInfo.Cells
    '            For Each gf As Nuctions.GeneFile In c.DNAs
    '                If Not geneCol.Contains(gf) Then geneCol.Add(gf)
    '            Next
    '        Next
    '    Next
    '    Nuctions.AddFeatures(geneCol, FeatureCol)
    '    For Each ci As ChartItem In WinFormOperationView.Items
    '        ci.Reload(ci.MolecularInfo, EnzymeCol)
    '    Next
    '    WinFormOperationView.Draw()
    'End Sub

    'Private Sub RecalculateAllChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecalculateAllChildrenToolStripMenuItem.Click
    '    RecalculateAllChildren()
    '    WinFormOperationView.Draw()
    'End Sub

    'Public Sub UpdateView()
    '    WinFormOperationView.Draw()
    'End Sub


    'Private Sub RecalculateItem(ByVal ci As ChartItem)
    '    ci.MolecularInfo.Calculate()
    '    ci.Reload(ci.MolecularInfo, EnzymeCol)
    'End Sub

    'Public Sub RecalculateAllChildren()


    '    Dim cnLevel As New List(Of ChartItem)

    '    Dim ntLevel As New List(Of ChartItem)


    '    For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '        ntLevel.Add(ci)
    '    Next

    '    While ntLevel.Count > 0
    '        cnLevel.Clear()
    '        cnLevel.AddRange(ntLevel)
    '        ntLevel.Clear()

    '        '排列下一层
    '        For Each ci As ChartItem In cnLevel
    '            RecalculateItem(ci)
    '        Next

    '        '再寻找下一层
    '        For Each si As ChartItem In WinFormOperationView.Items
    '            For Each ci As ChartItem In cnLevel
    '                If si.MolecularInfo.Source.Contains(ci.MolecularInfo) Then
    '                    ntLevel.Add(si)
    '                    Exit For
    '                End If
    '            Next
    '        Next
    '    End While

    'End Sub

    'Private Sub AutoArragneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoArragneToolStripMenuItem.Click
    '    WinFormOperationView.AutoArragne()
    'End Sub

    'Private Sub AutoFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoFitChildrenToolStripMenuItem.Click
    '    WinFormOperationView.AutoFitChildren()
    'End Sub

    'Private Sub StepFitChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StepFitChildrenToolStripMenuItem.Click
    '    WinFormOperationView.FitChildrenStepByStep()
    'End Sub

    Private Sub pvMain_ExportSummary(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    'Private Sub WorkControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    WinFormOperationView.Menu = cms_ChartItem
    '    WinFormOperationView.SetEnzymes(EnzymeCol)
    '    WinFormOperationView.SetFeatures(FeatureCol)
    'End Sub


    Dim vTarget As DNAInfo
    Dim vOldSource As List(Of DNAInfo)
    Public ReadOnly Property SourceMode() As Boolean
        Get
            Return WinFormOperationView.SourceMode
        End Get
    End Property
    'Private Sub pvMain_RequireSource(ByVal sender As Object, ByVal e As SourceEventArgs) Handles pvMain.RequireSource
    '    vTarget = e.Target
    '    vOldSource = e.Target.Source
    '    e.Target.Source = New List(Of DNAInfo)
    '    e.Target.Source.AddRange(vOldSource)
    '    WinFormOperationView.SelectedItems.Clear()
    '    For Each ci As ChartItem In WinFormOperationView.Items
    '        If vTarget.Source.Contains(ci.MolecularInfo) Then lv_Chart.SelectedItems.Add(ci)
    '    Next
    '    WinFormOperationView.SourceMode = True
    '    'enter key to stop source mode
    'End Sub

    'Public Sub ExitMode()
    '    WinFormOperationView.SelectedItems.Clear()
    '    WinFormOperationView.SelectedItems.Add(pvMain.PrpC.RelatedChartItem)
    '    WinFormOperationView.SourceMode = False
    '    pvMain.PrpC.SetSource()
    'End Sub
    'Public Sub AcceptMode()
    '    If (vTarget Is Nothing) Or (Not WinFormOperationView.SourceMode) Then Exit Sub
    '    vTarget.Source.Clear()
    '    For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '        vTarget.Source.Add(ci.MolecularInfo)
    '    Next
    '    WinFormOperationView.SelectedItems.Clear()
    '    WinFormOperationView.SelectedItems.Add(pvMain.PrpC.RelatedChartItem)
    '    WinFormOperationView.SourceMode = False
    '    pvMain.PrpC.SetSource()
    '    Changed = True
    'End Sub

    Public Event GroupCopy(ByVal sender As Object, ByVal e As GroupCopyEventArgs)

    'Private Sub tsmCopyGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmCopyGroup.Click
    '    OnGroupCopy()
    'End Sub

    'Public Sub OnGroupCopy()
    '    Dim Group As New List(Of DNAInfo)
    '    For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '        Group.Add(ci.MolecularInfo)
    '    Next
    '    WPFEntry.GroupCopy = DuplicateGroup(Group)
    '    WPFEntry.GroupHost = Me
    'End Sub
    ''' <summary>
    ''' 在接收方的项目当中复制这些元件
    ''' </summary>
    ''' <param name="cList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Function DuplicateGroup(ByVal cList As List(Of DNAInfo)) As List(Of DNAInfo)
    '    Dim Group As New List(Of DNAInfo)
    '    Dim DNADict As New Dictionary(Of DNAInfo, DNAInfo)

    '    Dim cl As DNAInfo
    '    For Each ci As DNAInfo In cList
    '        cl = ci.Clone
    '        cl.Source = New List(Of DNAInfo)
    '        cl.DNAs = New Collection
    '        For Each gf As Nuctions.GeneFile In ci.DNAs
    '            cl.DNAs.Add(gf.CloneWithoutFeatures)
    '        Next
    '        Nuctions.AddFeatures(cl.DNAs, FeatureCol)
    '        DNADict.Add(ci, cl)
    '    Next
    '    For Each di As DNAInfo In DNADict.Keys
    '        For Each si As DNAInfo In di.Source
    '            If DNADict.ContainsKey(si) Then
    '                DNADict(di).Source.Add(DNADict(si))
    '            End If
    '        Next
    '    Next
    '    For Each di As DNAInfo In DNADict.Values
    '        Group.Add(di)
    '    Next
    '    Return Group
    'End Function
    'Public Sub GroupPaste()
    '    If WPFEntry.GroupHost Is Nothing OrElse WPFEntry.GroupCopy Is Nothing OrElse WPFEntry.GroupCopy.Count = 0 Then Exit Sub
    '    Dim cList As List(Of DNAInfo) = WPFEntry.GroupHost.DuplicateGroup(WPFEntry.GroupCopy)
    '    If cList Is Nothing Then Exit Sub
    '    If cList.Count > 0 Then
    '        Dim px As Single = WinFormOperationView.MenuLocation.X

    '        Dim py As Single = WinFormOperationView.MenuLocation.Y

    '        Dim dx As Single = px - cList(0).DX
    '        Dim dy As Single = py - cList(0).DY

    '        Dim vSource As New List(Of DNAInfo)

    '        For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '            vSource.Add(ci.MolecularInfo)
    '        Next

    '        For Each di As DNAInfo In cList
    '            di.DX += dx
    '            di.DY += dy
    '            Select Case di.MolecularOperation
    '                Case Nuctions.MolecularOperationEnum.Vector, Nuctions.MolecularOperationEnum.Host, Nuctions.MolecularOperationEnum.FreeDesign
    '                Case Else
    '                    If di.Source.Count = 0 Then
    '                        For Each scrdi As DNAInfo In vSource
    '                            If scrdi IsNot di Then di.Source.Add(scrdi)
    '                        Next
    '                    End If
    '            End Select
    '            For Each gf As Nuctions.GeneFile In di.DNAs
    '                LoadFeature(gf)
    '            Next
    '            WinFormOperationView.Add(di, EnzymeCol)
    '        Next
    '        WinFormOperationView.Draw()
    '    End If
    'End Sub
    'Private Sub tsmPasteGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPasteGroup.Click
    '    GroupPaste()
    '    Changed = True
    'End Sub

    'Dim SwitchDistance As Integer

    'Dim Switched As Boolean = False
    'Public Sub SwitchSplitter()
    '    If Switched Then
    '        scMain.SplitterDistance = SwitchDistance
    '        Switched = False
    '    Else
    '        SwitchDistance = scMain.SplitterDistance
    '        scMain.SplitterDistance = 100
    '        Switched = True
    '    End If
    'End Sub

    'Private Sub CopySequenceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySequenceToolStripMenuItem.Click
    '    If WinFormOperationView.SelectedItems.Count = 1 AndAlso WinFormOperationView.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
    '        Dim gf As Nuctions.GeneFile = WinFormOperationView.SelectedItems(0).MolecularInfo.DNAs(1)
    '        Dim copied As Boolean = False
    '        While Not copied
    '            Try
    '                Clipboard.SetDataObject(gf.Sequence, True, 10, 50)
    '                copied = True
    '            Catch ex As Exception

    '            End Try
    '            Application.DoEvents()
    '        End While
    '    End If
    'End Sub

    'Private Sub RemarkFeaturesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemarkFeaturesToolStripMenuItem.Click
    '    If WinFormOperationView.SelectedItems.Count > 0 Then
    '        For Each ci As ChartItem In WinFormOperationView.SelectedItems
    '            Nuctions.AddFeatures(ci.MolecularInfo.DNAs, FeatureCol)
    '        Next
    '    End If
    'End Sub

    'Private Sub ExportSelectionWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportSelectionWToolStripMenuItem.Click
    '    If sfdEMF.ShowDialog = DialogResult.OK Then
    '        Dim fs As OperationView = WinFormOperationView
    '        fs.SaveSelectionPictureTo(sfdEMF.FileName)
    '    End If
    'End Sub

    'Private Sub ExportAllZToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportAllZToolStripMenuItem.Click
    '    If sfdEMF.ShowDialog = DialogResult.OK Then
    '        Dim fs As OperationView = WinFormOperationView
    '        fs.SavePictureTo(sfdEMF.FileName)
    '    End If
    'End Sub
    'Private Sub ExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportAllZToolStripMenuItem.Click
    '    If sfdGeneBank.ShowDialog = DialogResult.OK Then
    '        If pvMain.SelectItem.Count = 1 AndAlso pvMain.SelectItem(0).MolecularInfo.DNAs.Count = 1 Then
    '            Dim gf As Nuctions.GeneFile = pvMain.SelectItem(0).MolecularInfo.DNAs(1)
    '            gf.WriteToFile(sfdGeneBank.FileName)
    '        End If
    '    End If
    'End Sub
    'Private Sub ViewTmBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTmBToolStripMenuItem.Click
    '    If lv_Chart.SelectedItems.Count = 1 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
    '        Stop 'AddTmViewerTab
    '        'frmMain.AddTmViewerTab(lv_Chart.SelectedItems(0).MolecularInfo.DNAs(1))
    '    End If
    'End Sub

    'Private Sub CopySelectedVectorMapDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySelectedVectorMapDToolStripMenuItem.Click
    '    If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
    '        If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then

    '            Dim bmp As New Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
    '            Dim g As Graphics = Graphics.FromImage(bmp)
    '            Dim sz As SizeF = lv_Chart.SelectedItems(0).DrawMap(g)
    '            g.Dispose()
    '            bmp.Dispose()


    '            bmp = New Bitmap(Math.Ceiling(sz.Width), Math.Ceiling(sz.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb)
    '            g = Graphics.FromImage(bmp)

    '            Dim emf As New System.Drawing.Imaging.Metafile(New System.IO.MemoryStream, g.GetHdc, Imaging.EmfType.EmfPlusDual)
    '            Dim eg As Graphics = Graphics.FromImage(emf)
    '            lv_Chart.SelectedItems(0).DrawMap(eg)
    '            eg.Dispose()
    '            g.ReleaseHdc()
    '            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(Me.ParentForm.Handle, emf)
    '        End If
    '    End If
    'End Sub

    'Private Sub ExportSelectedVectorJToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportSelectedVectorJToolStripMenuItem.Click
    '    If sfdEMF.ShowDialog = DialogResult.OK Then
    '        If lv_Chart.SelectedItems.Count > 0 AndAlso lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then

    '            Dim bmp As New Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
    '            Dim g As Graphics = Graphics.FromImage(bmp)
    '            Dim sz As SizeF = lv_Chart.SelectedItems(0).DrawMap(g)
    '            g.Dispose()
    '            bmp.Dispose()


    '            bmp = New Bitmap(Math.Ceiling(sz.Width), Math.Ceiling(sz.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb)
    '            g = Graphics.FromImage(bmp)

    '            Dim emf As New System.Drawing.Imaging.Metafile(sfdEMF.FileName, g.GetHdc, Imaging.EmfType.EmfPlusDual)
    '            Dim eg As Graphics = Graphics.FromImage(emf)
    '            lv_Chart.SelectedItems(0).DrawMap(eg)
    '            eg.Dispose()
    '            emf.Dispose()
    '            g.ReleaseHdc()
    '            bmp.Dispose()
    '        End If
    '    End If
    'End Sub

    'Private Sub HashPickHToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HashPickHToolStripMenuItem.Click
    '    AddNewOperation(sender.tag, True)
    'End Sub


    'Private Sub pvMain_RequireUpdateView(ByVal sender As Object, ByVal e As System.EventArgs) Handles pvMain.RequireUpdateView
    '    lv_Chart.Draw()
    'End Sub

    'Private Sub pvMain_RequireSummary(ByVal sender As Object, ByVal e As SummaryEventArgs) Handles pvMain.RequireSummary
    '    Dim stb As New RTFStringBuilder
    '    GenerateSummary(stb, e)
    '    e.Summary = stb.ToString
    '    e.Append = stb.AppendText
    'End Sub

    'Private Sub GetConstructionDescriptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetConstructionDescriptionToolStripMenuItem.Click
    '    If lv_Chart.SelectedItems.Count > 0 Then
    '        Dim vRoots As New List(Of DNAInfo)
    '        For Each ci As ChartItem In lv_Chart.SelectedItems
    '            vRoots.Add(ci.MolecularInfo)
    '        Next

    '        Dim cd As String = GetDescripotion(vRoots)
    '        Dim copied As Boolean = False
    '        While Not copied
    '            Try
    '                Clipboard.SetDataObject(cd, True, 10, 50)
    '                copied = True
    '            Catch ex As Exception

    '            End Try
    '            Application.DoEvents()
    '        End While
    '    End If
    'End Sub

    'Private Function GetDescripotion(ByVal Sources As List(Of DNAInfo), Optional ByVal Summary As Boolean = True) As String
    '    Dim vRoots As New List(Of DNAInfo)

    '    Dim infolist As New List(Of DNAInfo)
    '    For Each ci As ChartItem In lv_Chart.Items
    '        infolist.Add(ci.MolecularInfo)
    '    Next
    '    Dim CountList As New Dictionary(Of DNAInfo, Integer)
    '    For Each dit As DNAInfo In infolist
    '        CountList.Add(dit, 0)
    '    Next
    '    For Each dit As DNAInfo In infolist
    '        For Each sc As DNAInfo In dit.Source
    '            CountList(sc) += 1
    '        Next
    '    Next
    '    For Each dit As DNAInfo In infolist
    '        If CountList(dit) = 0 Then vRoots.Add(dit)
    '    Next
    '    Dim stb As New System.Text.StringBuilder
    '    Dim vList As New List(Of DNAInfo)
    '    Dim vStack As Stack(Of DNAInfo)
    '    vList.AddRange(Sources)
    '    Dim dii As DNAInfo
    '    Dim vVisited As New List(Of DNAInfo)

    '    Dim vQueue As New Queue(Of Stack(Of DNAInfo))

    '    While vRoots.Count > 0
    '        '这里面有个逻辑关系
    '        '从vRoots出发时，先访问的东西在后面不需要再访问。
    '        '所以先访问的东西先构建 因为把它们添加到以vRoots中元素为顺序的Queue当中
    '        dii = vRoots(0)
    '        vStack = New Stack(Of DNAInfo)
    '        dii.TraceDependencyStack(vStack, vList, vVisited)
    '        If vStack.Count > 0 Then vQueue.Enqueue(vStack)
    '        If vRoots.Contains(dii) Then vRoots.Remove(dii)
    '    End While
    '    While vQueue.Count > 0
    '        vStack = vQueue.Dequeue
    '        While vStack.Count > 0
    '            stb.Append(vStack.Pop.ConstructionProcess(Summary))
    '        End While
    '    End While
    '    Return stb.ToString
    'End Function

    'Private Sub ConvertToFreeDesignToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToFreeDesignToolStripMenuItem.Click
    '    If lv_Chart.SelectedItems.Count > 0 Then
    '        For Each ci As ChartItem In lv_Chart.SelectedItems
    '            If ci.MolecularInfo.DNAs.Count = 1 Then
    '                Dim di As DNAInfo = ci.MolecularInfo
    '                di.Source.Clear()
    '                di.MolecularOperation = Nuctions.MolecularOperationEnum.FreeDesign
    '                Dim gf As Nuctions.GeneFile = di.DNAs(1)
    '                di.FreeDesignCode = gf.ToFreeDesignCode
    '                di.Calculate()
    '                ci.Reload(di, EnzymeCol)
    '            End If
    '        Next
    '        If lv_Chart.SelectedItems.Count = 1 Then
    '            Dim li As New List(Of ChartItem)
    '            li.AddRange(lv_Chart.SelectedItems)
    '            pvMain.SelectItem = li
    '        End If
    '        lv_Chart.Draw()
    '    End If
    'End Sub

    'Private Sub ConvertToHostToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToHostToolStripMenuItem.Click
    '    If WinFormOperationView.SelectedItems.Count > 0 Then
    '        For Each ci As ChartItem In lv_Chart.SelectedItems
    '            If ci.MolecularInfo.Cells.Count = 1 Then
    '                Dim di As DNAInfo = ci.MolecularInfo
    '                di.Source.Clear()
    '                di.MolecularOperation = Nuctions.MolecularOperationEnum.Host
    '                ci.Reload(di, EnzymeCol)
    '            End If
    '        Next
    '        If WinFormOperationView.SelectedItems.Count = 1 Then
    '            Dim li As New List(Of ChartItem)
    '            li.AddRange(lv_Chart.SelectedItems)
    '            pvMain.SelectItem = li
    '        End If
    '        WinFormOperationView.Draw()
    '    End If
    'End Sub

    'Private Sub ConvertToVectorToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConvertToVectorToolStripMenuItem.Click
    '    If lv_Chart.SelectedItems.Count > 0 Then
    '        For Each ci As ChartItem In lv_Chart.SelectedItems
    '            If ci.MolecularInfo.DNAs.Count = 1 Then
    '                Dim di As DNAInfo = ci.MolecularInfo
    '                di.Source.Clear()
    '                di.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
    '                ci.Reload(di, EnzymeCol)
    '            End If
    '        Next
    '        If lv_Chart.SelectedItems.Count = 1 Then
    '            Dim li As New List(Of ChartItem)
    '            li.AddRange(lv_Chart.SelectedItems)
    '            pvMain.SelectItem = li
    '        End If
    '        lv_Chart.Draw()
    '    End If
    'End Sub

    'Private Sub pvMain_RequirePrimer(sender As Object, e As PrimerEventArgs) Handles pvMain.RequirePrimer
    '    e.Primers = lv_Chart.Primers
    'End Sub

    'Private Sub CopySelectionToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles CopySelectionToolStripMenuItem.Click
    '    lv_Chart.CopySelectionAsEMF()
    'End Sub

    'Private Sub pvMain_RequireHost(sender As Object, e As HostEventArgs) Handles pvMain.RequireHost
    '    e.Hosts = lv_Chart.Hosts
    'End Sub

    'Private Sub pvMain_ReqireWorkControl(sender As Object, e As WorkControlEventArgs) Handles pvMain.ReqireWorkControl
    '    e.WorkControl = Me
    'End Sub


#Region "添加预定义的Host and Feature"
    ''' <summary>
    ''' 响应PropertyView的IncludeCommonDefination事件。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    'Private Sub pvMain_IncludeCommonDefination(sender As Object, e As System.EventArgs) Handles pvMain.IncludeCommonDefination
    '    IncludeCommonDefination()
    'End Sub
    ''' <summary>
    ''' 将NativeDefine当中定义的Host和Feature添加到系统当中。
    ''' </summary>
    ''' <remarks></remarks>
    'Friend Sub IncludeCommonDefination()
    '    For Each ft As Nuctions.Feature In NativeOrigin.NativeOrigins
    '        AddNativeFeature(ft)
    '    Next
    '    For Each ft As Nuctions.Feature In NativeAntibioticResistance.NativeAntibioticResistances
    '        AddNativeFeature(ft)
    '    Next
    '    For Each ft As Nuctions.Feature In NativePrimase.NativePrimases
    '        AddNativeFeature(ft)
    '    Next
    '    For Each h As Nuctions.Host In NativeHost.NativeHosts
    '        AddHost(h)
    '    Next
    'End Sub
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
    'Private Sub AddHost(newHost As Nuctions.Host)
    '    Dim contains As Boolean = False
    '    For Each ht As Nuctions.Host In lv_Chart.Hosts
    '        If ht.Name = newHost.Name Then contains = True : Exit For
    '    Next
    '    If Not contains Then
    '        lv_Chart.Hosts.Add(newHost)
    '    End If
    'End Sub
#End Region

    Public Published As Boolean
    Public PublicationID As Integer
    Public Quoted As Boolean
    Public QuotationID As Integer

    'Public Sub PresentSummary(sec As SummarySectionEnum)
    '    pvMain.PresentSummary(sec)
    'End Sub

#Region "自动保存功能"
    Private WithEvents AutoSaveTimer As New System.Windows.Forms.Timer() With {.Enabled = True, .Interval = 1000 * 60 * 10}
    'Private Sub AutoSave(obj As Object, e As EventArgs) Handles AutoSaveTimer.Tick
    '    Me.ParentForm.Invoke(New Action(Of WorkControl)(AddressOf AutoSaveWorkControl), New Object() {Me})
    'End Sub
    Private TempFilename As String
    'Private Sub AutoSaveWorkControl(wc As WorkControl)
    '    Dim lastTemp As String = TempFilename
    '    Try
    '        Dim fi As New IO.FileInfo(FileAddress)
    '        If fi.Exists Then
    '            TempFilename = fi.FullName + ".tmp"
    '        ElseIf TempFilename Is Nothing OrElse TempFilename = "" OrElse Not IO.File.Exists(TempFilename) Then
    '            If Not IO.Directory.Exists(Application.StartupPath + "\Temp") Then IO.Directory.CreateDirectory(Application.StartupPath + "\Temp")
    '            TempFilename = Application.StartupPath + "\Temp\" + Now.ToString("yyyy-MM-dd HH-mm-ss-ffff") + ".tmp"
    '        End If
    '    Catch ex As Exception
    '        If Not IO.Directory.Exists(Application.StartupPath + "\Temp") Then IO.Directory.CreateDirectory(Application.StartupPath + "\Temp")
    '        TempFilename = Application.StartupPath + "\Temp\" + Now.ToString("yyyy-MM-dd HH-mm-ss-ffff") + ".tmp"
    '    End Try
    '    Try
    '        If lastTemp IsNot Nothing AndAlso lastTemp <> TempFilename AndAlso IO.File.Exists(lastTemp) Then
    '            IO.File.Delete(lastTemp)
    '        End If
    '    Catch ex As Exception

    '    End Try
    '    Try
    '        WPFEntry.SaveToZXML(New Dictionary(Of String, Object) From {{"WorkChart", Me.GetWorkSpace}}, TempFilename)
    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Public Sub ClearnTempFile()
    '    Try
    '        AutoSaveTimer.Enabled = False
    '        AutoSaveTimer.Dispose()
    '        If IO.File.Exists(TempFilename) Then IO.File.Delete(TempFilename)
    '    Catch ex As Exception

    '    End Try
    'End Sub
#End Region




End Class
