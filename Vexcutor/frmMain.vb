Imports System.Management
Imports System.Security.Cryptography

Public Class frmMain
    Private EmergencyExit As Boolean = False

    Public GroupCopy As List(Of DNAInfo)
    Friend GroupHost As WorkControl

    '用于读取和保存信息的 加载自独立的加密的DLL文件
    Public Loader As Object
    Public Saver As Object

#Region "管理Tab"
    Public Sub NewTab()
        Dim WC As New WorkControl
        WC.Dock = DockStyle.Fill
        AddHandler WC.CloseWorkControl, AddressOf OnTabClose
        AddHandler WC.LoadWorkControl, AddressOf OnLoadTab
        Dim TP As New CustomTabPage
        WC.ParentTab = TP
        WC.lv_Chart.Hosts.Add(New Nuctions.Host With {.Name = "in vitro", .Description = ""})
        WC.lv_Chart.Hosts.Add(New Nuctions.Host With {.Name = "in vivo", .Description = ""})
        TP.Text = "New Experiment"
        TP.Controls.Add(WC)
        TP.SetCustomStyle(CustomTabPageStyle.Project)
        tcMainHost.TabPages.Add(TP)
    End Sub
    Public Sub LoadTabFromFile(ByVal filename As String)
        'Try
        AddRecentFileMenuItem(filename)
        Dim lf As String = filename.ToLower
        If lf.EndsWith(".stone") Or lf.EndsWith(".vxt") Then
            Dim wc As WorkControl = WorkControl.LoadFrom(filename)
            If wc Is Nothing Then
                Dim frmErr As New frmError
                Dim ei As New VexError With {.ErrorTitle = "Failed to Open File.",
                                             .ErrorMessage = <A>Please double check if the file is MCDS format.</A>}
                frmErr.VexcutorErrorInfo.ErrorInfo = ei
                frmErr.ShowDialog()

                Exit Sub
            End If

            wc.Dock = DockStyle.Fill
            AddHandler wc.CloseWorkControl, AddressOf OnTabClose
            AddHandler wc.LoadWorkControl, AddressOf OnLoadTab
            Dim TP As New CustomTabPage
            wc.ParentTab = TP
            Dim fi As New IO.FileInfo(filename)
            TP.Text = fi.Name
            TP.Controls.Add(wc)
            TP.SetCustomStyle(CustomTabPageStyle.Project)
            If tcMainHost.TabPages.Count > 0 Then
                'Dim SWC As WorkControl = SelectedWorkControl
                'If Not (SWC Is Nothing) Then
                '    If SWC.ContentChanged = False AndAlso SWC.lv_Chart.Items.Count = 0 Then
                '        CloseTab(SWC)
                '        TP.Select()
                '    End If
                'End If
                tcMainHost.TabPages.Insert(tcMainHost.SelectedIndex, TP)
                tcMainHost.SelectedTab = TP

            Else
                tcMainHost.TabPages.Add(TP)
            End If
        ElseIf lf.EndsWith(".vct") Then
            Dim dict As Dictionary(Of String, Object) = LoadFromZXML(New List(Of String) From {"DNA", "Enzyme"}, filename)
            If dict Is Nothing Then Exit Sub
            Dim gf As Nuctions.GeneFile = dict("DNA")
            Dim REs As New List(Of String)
            If Not dict("Enzyme") Is Nothing Then
                REs = dict("Enzyme")
            End If
            AddDNAViewTab(gf, REs, filename)
        ElseIf lf.EndsWith(".gb") Then
            Dim gf As Nuctions.GeneFile = Nuctions.GeneFile.LoadFromGeneBankFile(filename)
            AddDNAViewTab(gf, New List(Of String), filename)
        End If
    End Sub
    Public Sub LoadTab()
        If ofdProject.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadTabFromFile(ofdProject.FileName)
        End If
    End Sub
    Public Sub OnTabClose(ByVal sender As Object, ByVal e As EventArgs)
        If TypeOf sender Is WorkControl Then
            Dim wc As WorkControl = sender
            TryCloseTab(wc.ParentTab)
        ElseIf TypeOf sender Is SequenceViewer Then
            Dim sv As SequenceViewer = sender
            TryCloseTab(sv.ParentTab)
            'RemoveHandler sv.CloseTab, AddressOf OnTabClose
            'tcMainHost.TabPages.Remove(sv.ParentTab)
        ElseIf TypeOf sender Is WPFRecombinationSetManager
            Dim sv As WPFRecombinationSetManager = sender
            TryCloseTab(sv.ParentTab)
        ElseIf TypeOf sender Is WPFRestrictionEnzymeManager
            Dim sv As WPFRestrictionEnzymeManager = sender
            TryCloseTab(sv.ParentTab)
        End If
        '#End If
    End Sub
    Friend Sub CloseTab(ByVal WC As WorkControl)
        RemoveHandler WC.CloseWorkControl, AddressOf OnTabClose
        RemoveHandler WC.LoadWorkControl, AddressOf OnLoadTab
        WC.ClearnTempFile()
        tcMainHost.TabPages.Remove(WC.ParentTab)
    End Sub
    Friend Sub CloseTab(ByVal SV As SequenceViewer)
        RemoveHandler SV.CloseTab, AddressOf OnTabClose
        tcMainHost.TabPages.Remove(SV.ParentTab)
    End Sub
    Public Function TryCloseTab(ByVal TP As TabPage) As Boolean
        If Not TP Is Nothing Then
            If TypeOf TP.Controls(0) Is WorkControl Then
                Dim WC As WorkControl = TP.Controls(0)
                Return TryCloseTab(WC)
            ElseIf TypeOf TP.Controls(0) Is TmViewer Then
                Dim TV As TmViewer = TP.Controls(0)
                Return TryCloseTab(TV)
                'ElseIf TypeOf TP.Controls(0) Is DigestionManageView Then
                '    Dim DV As DigestionManageView = TP.Controls(0)
                '    Return TryCloseTab(DV)
            ElseIf TypeOf TP.Controls(0) Is SequenceViewer Then
                Dim SV As SequenceViewer = TP.Controls(0)
                Return TryCloseTab(SV)
            ElseIf TypeOf TP.Controls(0) Is CodonLibraryManager Then
                Dim CM As CodonLibraryManager = TP.Controls(0)
                Return TryCloseTab(CM)
            ElseIf TypeOf TP.Controls(0) Is ClientBrowswer Then
                Dim CB As ClientBrowswer = TP.Controls(0)
                Return TryCloseTab(CB)
            ElseIf TypeOf TP.Controls(0) Is WebBrowser Then
                Dim WB As WebBrowser = TP.Controls(0)
                Return TryCloseTab(WB)
            ElseIf TypeOf TP.Controls(0) Is InteropHost
                Dim iHost As InteropHost = TP.Controls(0)
                If TypeOf iHost.Child Is WPFRecombinationSetManager Then
                    Return TryCloseTab(DirectCast(iHost.Child, WPFRecombinationSetManager))
                ElseIf TypeOf iHost.Child Is WPFRestrictionEnzymeManager Then
                    Return TryCloseTab(DirectCast(iHost.Child, WPFRestrictionEnzymeManager))
                ElseIf TypeOf iHost.Child Is WPFDigestionBufferManager Then
                    Return TryCloseTab(DirectCast(iHost.Child, WPFDigestionBufferManager))
                ElseIf TypeOf iHost.Child Is WPFCodonAnalyzer Then
                    Return TryCloseTab(DirectCast(iHost.Child, WPFCodonAnalyzer))
                ElseIf TypeOf iHost.Child Is WPFgRNAManager Then
                    Return TryCloseTab(DirectCast(iHost.Child, WPFgRNAManager))
                End If
            End If
        Else
            Return True
        End If
    End Function
    Public Function TryCloseTab(wpfCdnAnl As WPFCodonAnalyzer) As Boolean
        If wpfCdnAnl Is Nothing Then Return True
        tcMainHost.TabPages.Remove(wpfCdnAnl.ParentTab)
        Return True
    End Function
    Public Function TryCloseTab(wpfRecMan As WPFRecombinationSetManager) As Boolean
        If wpfRecMan Is Nothing Then Return True
        tcMainHost.TabPages.Remove(wpfRecMan.ParentTab)
        Return True
    End Function
    Public Function TryCloseTab(wpfRecMan As WPFgRNAManager) As Boolean
        If wpfRecMan Is Nothing Then Return True
        tcMainHost.TabPages.Remove(wpfRecMan.ParentTab)
        Return True
    End Function
    Public Function TryCloseTab(wpfDigMan As WPFDigestionBufferManager) As Boolean
        If wpfDigMan Is Nothing Then Return True
        tcMainHost.TabPages.Remove(wpfDigMan.ParentTab)
        Return True
    End Function
    Public Function TryCloseTab(wpfResMan As WPFRestrictionEnzymeManager) As Boolean
        If wpfResMan Is Nothing Then Return True
        tcMainHost.TabPages.Remove(wpfResMan.ParentTab)
        Return True
    End Function
    Friend Function TryCloseTab(ByVal SV As SequenceViewer) As Boolean
        If SV Is Nothing Then Return True

        If SV.ContentChanged Then
#If ReaderMode = 0 Then
            Select Case MsgBox("Do you want to save the changes to " + SV.ParentTab.Text, MsgBoxStyle.YesNoCancel)
                Case MsgBoxResult.Yes
                    SV.Save()
                    CloseTab(SV)
                    Return True
                Case MsgBoxResult.No
                    CloseTab(SV)
                    Return True
                Case MsgBoxResult.Cancel
                    Return False
            End Select
#Else
                CloseTab(SV)
                Return True
#End If
        Else
            CloseTab(SV)
            Return True
        End If

    End Function
    Friend Function TryCloseTab(ByVal WC As WorkControl) As Boolean
        If Not WC Is Nothing Then
            If WC.ContentChanged Then
#If ReaderMode = 0 Then
                Select Case MsgBox("Do you want to save the changes to " + WC.ParentTab.Text, MsgBoxStyle.YesNoCancel)
                    Case MsgBoxResult.Yes
                        WC.Save()
                        CloseTab(WC)
                        Return True
                    Case MsgBoxResult.No
                        CloseTab(WC)
                        Return True
                    Case MsgBoxResult.Cancel
                        Return False
                End Select
#Else
                CloseTab(WC)
                Return True
#End If
            Else
                CloseTab(WC)
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Public Function TryCloseTab(ByVal TV As TmViewer) As Boolean
        If Not TV Is Nothing Then
            tcMainHost.TabPages.Remove(TV.Parent)
            Return True
        End If
    End Function
    Public Function TryCloseTab(ByVal CM As CodonLibraryManager) As Boolean
        If Not CM Is Nothing Then
            tcMainHost.TabPages.Remove(CM.Parent)
            Return True
        End If
    End Function
    Public Function TryCloseTab(ByVal CB As ClientBrowswer) As Boolean
        If Not CB Is Nothing Then
            tcMainHost.TabPages.Remove(CB.Parent)
            Return True
        End If
    End Function
    Public Function TryCloseTab(ByVal WB As WebBrowser) As Boolean
        If Not WB Is Nothing Then
            tcMainHost.TabPages.Remove(WB.Parent)
            Return True
        End If
    End Function

    Public Sub OnLoadTab(ByVal sender As Object, ByVal e As EventArgs)
        LoadTab()
    End Sub

#End Region



    Private Sub NewNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewNToolStripMenuItem.Click
        NewTab()
    End Sub

    Private Sub PCRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCRToolToolStripMenuItem.Click
        'PCR Kit
        Dim a As New frmPCR
        a.MdiParent = Me
        a.Show()

    End Sub

    Private Sub EnzymeSiteFinderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeSiteFinderToolStripMenuItem.Click
        Dim a As New frmNucleotideEnzyme
        a.MdiParent = Me
        a.Show()
    End Sub

    Private Sub EnzymeSpiderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeSpiderToolStripMenuItem.Click
        Dim a As New frm_EnzSpider

        a.Show()
    End Sub


    Private Sub tsb_TAGC_NF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_TAGC_NF.Click
        Dim seq As String = Clipboard.GetText
        seq = Nuctions.TAGCFilter(seq)
        Try
            Clipboard.SetDataObject(seq, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub tsb_TAGC_RC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_TAGC_RC.Click
        Dim seq As String = Clipboard.GetText
        seq = Nuctions.ReverseComplement(Nuctions.TAGCFilter(seq))
        Try
            Clipboard.SetDataObject(seq, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub tsb_TAGC_NR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_TAGC_NR.Click
        Dim seq As String = Clipboard.GetText
        seq = Nuctions.Reverse(Nuctions.TAGCFilter(seq))
        Try
            Clipboard.SetDataObject(seq, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub tsb_TAGC_FC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_TAGC_FC.Click
        Dim seq As String = Clipboard.GetText
        seq = Nuctions.Complement(Nuctions.TAGCFilter(seq))
        Try
            Clipboard.SetDataObject(seq, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub RestrictionEnzymesRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestrictionEnzymesRToolStripMenuItem.Click

        Dim TP As New CustomTabPage
        Dim foundPage As Boolean = False

        'try to find existing editor
        For Each openTab As TabPage In tcMainHost.TabPages
            If TypeOf openTab.Controls(0) Is InteropHost Then
                Dim iHost As InteropHost = openTab.Controls(0)
                If TypeOf iHost.Child Is WPFRestrictionEnzymeManager Then
                    TP = openTab
                    foundPage = True
                End If
            End If
        Next

        'New setting file editor
        If Not foundPage Then
            Dim wpfRecMan As New WPFRestrictionEnzymeManager
            Dim iHost As New InteropHost With {.Child = wpfRecMan}

            iHost.Dock = DockStyle.Fill
            AddHandler wpfRecMan.CloseTab, AddressOf OnTabClose
            ' wpfRecMan.CloseTab, AddressOf OnLoadTab

            wpfRecMan.ParentTab = TP
            TP.Text = "Restriction Enzymes Management"
            TP.Controls.Add(iHost)
            TP.SetCustomStyle(CustomTabPageStyle.BufferManager)
            tcMainHost.TabPages.Add(TP)
        End If

        tcMainHost.SelectedTab = TP

    End Sub
    'RecombinationSitesRToolStripMenuItem
    Private Sub RecombinationSitesRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecombinationSitesRToolStripMenuItem.Click

        Dim TP As New CustomTabPage
        Dim foundPage As Boolean = False

        'try to find existing editor
        For Each openTab As TabPage In tcMainHost.TabPages
            If TypeOf openTab.Controls(0) Is InteropHost Then
                Dim iHost As InteropHost = openTab.Controls(0)
                If TypeOf iHost.Child Is WPFRecombinationSetManager Then
                    TP = openTab
                    foundPage = True
                End If
            End If
        Next

        'New setting file editor
        If Not foundPage Then
            Dim wpfRecMan As New WPFRecombinationSetManager
            Dim iHost As New InteropHost With {.Child = wpfRecMan}

            iHost.Dock = DockStyle.Fill
            AddHandler wpfRecMan.CloseTab, AddressOf OnTabClose
            ' wpfRecMan.CloseTab, AddressOf OnLoadTab

            wpfRecMan.ParentTab = TP
            TP.Text = "Recombination Site Management"
            TP.Controls.Add(iHost)
            TP.SetCustomStyle(CustomTabPageStyle.BufferManager)
            tcMainHost.TabPages.Add(TP)
        End If

        tcMainHost.SelectedTab = TP

    End Sub
    Private Sub gRNASitesRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gRNASitesRToolStripMenuItem.Click

        Dim TP As New CustomTabPage
        Dim foundPage As Boolean = False

        'try to find existing editor
        For Each openTab As TabPage In tcMainHost.TabPages
            If TypeOf openTab.Controls(0) Is InteropHost Then
                Dim iHost As InteropHost = openTab.Controls(0)
                If TypeOf iHost.Child Is WPFgRNAManager Then
                    TP = openTab
                    foundPage = True
                End If
            End If
        Next

        'New setting file editor
        If Not foundPage Then
            Dim wpfRNAMan As New WPFgRNAManager
            Dim iHost As New InteropHost With {.Child = wpfRNAMan}

            iHost.Dock = DockStyle.Fill
            AddHandler wpfRNAMan.CloseTab, AddressOf OnTabClose
            ' wpfRecMan.CloseTab, AddressOf OnLoadTab

            wpfRNAMan.ParentTab = TP
            TP.Text = "gRNA Management"
            TP.Controls.Add(iHost)
            TP.SetCustomStyle(CustomTabPageStyle.CodonManager)
            tcMainHost.TabPages.Add(TP)
        End If

        tcMainHost.SelectedTab = TP

    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        For Each tp As TabPage In tcMainHost.TabPages
            If Not TryCloseTab(tp) Then e.Cancel = True : Exit Sub
        Next
    End Sub

    Friend DatabasePath As String

    Public Function LoadDatabasePath() As Boolean
        fbdDatabase.Description = "Please Set the database folder for MCDS."
        If fbdDatabase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            DatabasePath = fbdDatabase.SelectedPath
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub RemoveDatabaseMenus()
        Dim tsi As ToolStripMenuItem
        For Each tsi In tssbDatabase.DropDownItems
            If tsi.DropDownItems.Count > 0 Then
                RemoveItems(tsi)
            Else
                RemoveHandler tsi.Click, AddressOf MenuLoadVector
            End If
        Next
        tssbDatabase.DropDownItems.Clear()
    End Sub
    Public Sub RemoveItems(ByVal tsmi As ToolStripMenuItem)
        Dim tsi As ToolStripMenuItem
        For Each tsi In tsmi.DropDownItems
            If tsi.DropDownItems.Count > 0 Then
                RemoveItems(tsi)
            Else
                RemoveHandler tsi.Click, AddressOf MenuLoadVector
            End If
        Next
    End Sub

    Public Sub LoadDatabase()
#If ReaderMode = 0 Then
        Dim di As IO.DirectoryInfo
        di = New IO.DirectoryInfo(DatabasePath)

        Dim tsi As ToolStripMenuItem
        For Each fi As IO.FileInfo In di.GetFiles("*.vct")
            tsi = New ToolStripMenuItem(fi.Name)
            tsi.ToolTipText = fi.FullName
            AddHandler tsi.Click, AddressOf MenuLoadVector
            tssbDatabase.DropDownItems.Add(tsi)
        Next
        For Each fi As IO.FileInfo In di.GetFiles("*.gb")
            tsi = New ToolStripMenuItem(fi.Name)
            tsi.ToolTipText = fi.FullName
            AddHandler tsi.Click, AddressOf MenuLoadVector
            tssbDatabase.DropDownItems.Add(tsi)
        Next
        For Each sdi As IO.DirectoryInfo In di.GetDirectories()
            tsi = New ToolStripMenuItem(sdi.Name)
            tsi.ToolTipText = sdi.FullName
            tssbDatabase.DropDownItems.Add(tsi)
            AddDataItems(tsi, sdi)
        Next
#End If
    End Sub
    Public Sub AddDataItems(ByVal tsmi As ToolStripMenuItem, ByVal di As IO.DirectoryInfo)
        Dim tsi As ToolStripMenuItem
        For Each fi As IO.FileInfo In di.GetFiles("*.vct")
            tsi = New ToolStripMenuItem(fi.Name)
            tsi.ToolTipText = fi.FullName
            AddHandler tsi.Click, AddressOf MenuLoadVector
            tsmi.DropDownItems.Add(tsi)
        Next
        For Each fi As IO.FileInfo In di.GetFiles("*.gb")
            tsi = New ToolStripMenuItem(fi.Name)
            tsi.ToolTipText = fi.FullName
            AddHandler tsi.Click, AddressOf MenuLoadVector
            tsmi.DropDownItems.Add(tsi)
        Next
        For Each sdi As IO.DirectoryInfo In di.GetDirectories()
            tsi = New ToolStripMenuItem(sdi.Name)
            tsi.ToolTipText = sdi.FullName
            tsmi.DropDownItems.Add(tsi)
            AddDataItems(tsi, di)
        Next
    End Sub

    Public Sub MenuLoadVector(ByVal sender As Object, ByVal e As EventArgs)
        If Not (SelectedWorkControl Is Nothing) Then
            If TypeOf sender Is ToolStripMenuItem Then
                Dim tsi As ToolStripMenuItem = sender
                If IO.File.Exists(tsi.ToolTipText) Then SelectedWorkControl.LoadVectorFromFile(tsi.ToolTipText)

            End If
        End If
    End Sub

    Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub
    Private RecentFiles As New List(Of String)

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SettingEntry.MainUIWindow = Me
        SettingEntry.SmallIconList = SmallIconList
        LoadNews()
        VersionControl.Check(Me)
    End Sub

    Private Sub LoadNews()
        Dim wb As New WebBrowser
        Dim tb As New CustomTabPage
        tb.Controls.Add(wb)
        tb.Text = "Start Page"
        wb.Dock = DockStyle.Fill
        tcMainHost.TabPages.Add(tb)
        wb.Navigate("file://" + AppDomain.CurrentDomain.BaseDirectory + "Tutorials.html")
        tcMainHost.SelectedTab = tb
        tb.Select()
    End Sub

    Private Sub OnClickRecentFileMenuItem(sender As Object, e As EventArgs)
        If TypeOf sender Is System.Windows.Forms.ToolStripMenuItem Then
            Dim mit As System.Windows.Forms.ToolStripMenuItem = sender
            LoadTabFromFile(mit.ToolTipText)
        End If
    End Sub

    Private Sub AddRecentFileMenuItem(fileaddress As String)
        Dim rfi As New System.IO.FileInfo(fileaddress)
        Dim mit As System.Windows.Forms.ToolStripMenuItem
        mit = New System.Windows.Forms.ToolStripMenuItem
        mit.Text = rfi.Name
        mit.ToolTipText = rfi.FullName
        AddHandler mit.Click, AddressOf OnClickRecentFileMenuItem
        Dim dList As New List(Of System.Windows.Forms.ToolStripMenuItem)
        For Each tsm As System.Windows.Forms.ToolStripMenuItem In RecentFilesMenuItem.DropDownItems
            If tsm.ToolTipText = rfi.FullName Then
                dList.Add(tsm)
            End If
        Next
        RecentFilesMenuItem.DropDownItems.Insert(0, mit)
        For Each tsm As System.Windows.Forms.ToolStripMenuItem In dList
            RecentFilesMenuItem.DropDownItems.Remove(tsm)
            RemoveHandler tsm.Click, AddressOf OnClickRecentFileMenuItem
        Next
        While RecentFilesMenuItem.DropDownItems.Count > 12
            RecentFilesMenuItem.DropDownItems.Remove(RecentFilesMenuItem.DropDownItems(12))
        End While
    End Sub
    Private Sub SaveSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveSToolStripMenuItem.Click
        SaveTab()
    End Sub
    Private Sub CloseCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseCToolStripMenuItem.Click
        LoadTab()
    End Sub
    Public Sub OpenFile(ByVal vFileName As String)

    End Sub
    Private Sub SaveAsRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsRToolStripMenuItem.Click
        If SelectedWorkControl IsNot Nothing Then
            SelectedWorkControl.SaveAs()
        ElseIf SelectedSequenceViewer IsNot Nothing Then
            SelectedSequenceViewer.SaveAs()
        End If
    End Sub


    Private Sub SequenceMergerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SequenceMergerToolStripMenuItem.Click
        Dim f As New pasteurMerger
        f.Owner = Me
        f.Show()
    End Sub

    Private Sub tsbTranslate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTranslate.Click
        Dim seq As String = Clipboard.GetText
        Dim tsl As String = Nuctions.Translate(seq)
        Try
            Clipboard.SetDataObject(tsl, True, 5, 20)
        Catch ex As Exception
            MsgBox("Clipboard Error.")
        End Try
    End Sub

    Private Sub SequenceViewerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SequenceViewerToolStripMenuItem.Click
        Dim svw As New SequenceWindow
        Dim ofd As New OpenFileDialog
        ofd.Filter = "Genebank|*.gb"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim gf As Nuctions.GeneFile = Nuctions.GeneFile.LoadFromGeneBankFile(ofd.FileName)
            Dim rec As New List(Of String)
            rec.Add("BamHI")
            rec.Add("SacI")
            rec.Add("EcoRI")
            rec.Add("HindIII")
            rec.Add("XhoI")
            rec.Add("SpeI")
            rec.Add("XbaI")
            rec.Add("SalI")
            svw.RestrictionSites = rec
            svw.GeneFile = gf
            svw.Show()
        End If
    End Sub

    Public Sub LoadVector()
        If SelectedWorkControl Is Nothing Then Exit Sub
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.LoadVector()
        End If
    End Sub
    Private Sub LoadVectorLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadVectorLToolStripMenuItem.Click
        LoadVector()
    End Sub

    Friend Property SelectedWorkControl() As WorkControl
        Get
            If tcMainHost.TabCount > 0 AndAlso tcMainHost.SelectedTab.Controls.Count > 0 AndAlso TypeOf tcMainHost.SelectedTab.Controls(0) Is WorkControl Then
                Return tcMainHost.SelectedTab.Controls(0)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As WorkControl)
            If tcMainHost.TabCount > 0 Then
                value.ParentTab.Select()
            End If
        End Set
    End Property

#Region "刷新视图"
    '用来在更新窗体的时候刷新视图
    Private Sub ActivateDraw()
        If Not (SelectedWorkControl Is Nothing) Then
            tsbAddPrintPage.Checked = SelectedWorkControl.PrintMode
        End If
    End Sub

    Private Sub tcMainHost_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)

        If tcMainHost.TabPages.Count > e.Index Then
            Dim gp As New System.Drawing.Drawing2D.GraphicsPath
            Dim VO As New Vector2(e.Bounds.X, e.Bounds.Y)
            Dim VH As New Vector2(0, e.Bounds.Height)
            Dim VW As New Vector2(e.Bounds.Width, 0)

            e.DrawFocusRectangle()

            gp.AddCurve(New PointF() {VO + VH, VO + (VW.GetBase * VH.GetLength + VH) / 2, VO + VW.GetBase * VH.GetLength, VO + VW / 2, VO + VW, VO + VW + VH}, 0.5)
            gp.CloseFigure()
            Dim gb As New System.Drawing.Drawing2D.LinearGradientBrush(CType(VO, PointF), CType(VO + VH, PointF), Color.White, Color.OrangeRed)
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
            e.Graphics.FillPath(gb, gp)
            e.Graphics.DrawPath(Pens.Red, gp)
            e.Graphics.DrawString(tcMainHost.TabPages(e.Index).Text, e.Font, Brushes.Black, e.Bounds.X + 6, e.Bounds.Y + 3)
        End If
    End Sub
    Private Sub tcMainHost_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ActivateDraw()
    End Sub
    Private Sub frmMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        ActivateDraw()
    End Sub
#End Region

    Private CopiedGeneFiles As List(Of Nuctions.GeneFile)
    Friend ReadOnly Property SelectedSequenceTab() As SequenceViewer
        Get
            If tcMainHost.TabCount > 0 AndAlso tcMainHost.SelectedTab.Controls.Count > 0 Then
                Dim cView = tcMainHost.SelectedTab.Controls(0)
                If TypeOf cView Is SequenceViewer Then
                    Return cView
                End If
            End If
            Return Nothing
        End Get
    End Property
    Friend ReadOnly Property SelectedSequenceViewer() As SequenceViewer
        Get
            If tcMainHost.TabCount > 0 AndAlso tcMainHost.SelectedTab.Controls.Count > 0 Then
                Dim cView = tcMainHost.SelectedTab.Controls(0)
                If TypeOf cView Is WorkControl Then
                    Dim wcView As WorkControl = cView
                    If wcView.pvMain.tcMain.SelectedTab Is wcView.pvMain.TPDNA Then
                        Dim gView = wcView.pvMain.gvDNA
                        If TypeOf gView.tcViewers.SelectedTab.Controls(0) Is SubGroupViewer Then
                            Dim sView As SubGroupViewer = gView.tcViewers.SelectedTab.Controls(0)
                            If TypeOf sView.tcViewers.SelectedTab.Controls(0) Is SequenceViewer Then
                                Dim sViewer = sView.tcViewers.SelectedTab.Controls(0)
                                Return sViewer
                            End If
                        ElseIf TypeOf gView.tcViewers.SelectedTab.Controls(0) Is SequenceViewer Then
                            Dim sViewer = gView.tcViewers.SelectedTab.Controls(0)
                            Return sViewer
                        End If
                    ElseIf wcView.pvMain.tcMain.SelectedTab Is wcView.pvMain.TPPCR Then
                        Dim gView = wcView.pvMain.gvDNA
                        If TypeOf gView.tcViewers.SelectedTab.Controls(0) Is SubGroupViewer Then
                            Dim sView As SubGroupViewer = gView.tcViewers.SelectedTab.Controls(0)
                            If TypeOf sView.tcViewers.SelectedTab.Controls(0) Is SequenceViewer Then
                                Dim sViewer = sView.tcViewers.SelectedTab.Controls(0)
                                Return sViewer
                            End If
                        ElseIf TypeOf gView.tcViewers.SelectedTab.Controls(0) Is SequenceViewer Then
                            Dim sViewer = gView.tcViewers.SelectedTab.Controls(0)
                            Return sViewer
                        End If
                    End If
                ElseIf TypeOf cView Is SequenceViewer Then
                    Return cView
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        Select Case e.KeyCode
            Case Keys.C
                If ModifierKeys = Keys.Control Then
                    If Not (SelectedWorkControl Is Nothing) Then
                        Dim wc As WorkControl = SelectedWorkControl
                        Dim str As String = Nothing
                        If TypeOf wc.pvMain.tcMain.SelectedTab.Controls(0) Is PropertyControl Then
                            Dim pc As PropertyControl = wc.pvMain.tcMain.SelectedTab.Controls(0)
                            If pc.TabControl_Operation.SelectedTab.Text = "PCR" Then
                                '粘贴引物的
                                Dim stb As New System.Text.StringBuilder
                                stb.Append(pc.tbFP.Text)
                                stb.Append(ControlChars.Tab)
                                stb.Append(pc.PCR_ForwardPrimer_TextBox.Text)
                                stb.AppendLine()
                                stb.Append(pc.tbRP.Text)
                                stb.Append(ControlChars.Tab)
                                stb.Append(pc.PCR_ReversePrimer_TextBox.Text)
                                stb.AppendLine()
                                str = stb.ToString
                            ElseIf pc.TabControl_Operation.SelectedTab.Text = "Screen" Then
                                Dim stb As New System.Text.StringBuilder
                                stb.Append(pc.tbSCRFP.Text)
                                stb.Append(ControlChars.Tab)
                                stb.Append(pc.Screen_PCR_F.Text)
                                stb.AppendLine()
                                stb.Append(pc.tbSCRRP.Text)
                                stb.Append(ControlChars.Tab)
                                stb.Append(pc.Screen_PCR_R.Text)
                                stb.AppendLine()
                                str = stb.ToString
                            ElseIf pc.TabControl_Operation.SelectedTab.Text = "Enzyme" Then
                                str = pc.tbEnzymes.Text
                            End If
                        End If
                        If TypeOf wc.pvMain.tcMain.SelectedTab.Controls(0) Is GroupViewer Then
                            str = Me.SelectedWorkControl.CopySelectedSequence()
                        End If


                        If Not (str Is Nothing) Then
                            If str.Length > 0 Then
                                Try
                                    Clipboard.Clear()
                                    Clipboard.SetText(str)
                                Catch ex As Exception

                                End Try
                            ElseIf Me.SelectedWorkControl.lv_Chart.Focused Then
                                '尝试复制选中的DNA
                                Dim gList As List(Of Nuctions.GeneFile) = SelectedWorkControl.TryCopyDNAs()
                                If gList.Count > 0 Then
                                    Dim t As Type = GetType(List(Of Nuctions.GeneFile))
                                    Clipboard.SetText(t.FullName)
                                    CopiedGeneFiles = gList
                                End If
                            End If
                        End If
                    ElseIf Not (SelectedSequenceViewer Is Nothing) Then
                        Dim str As String = Me.SelectedSequenceViewer.CopySelectedSequence()
                        If str.Length > 0 Then
                            Try
                                Clipboard.Clear()
                                Clipboard.SetText(str)
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                End If
            Case Keys.Delete
                If SelectedWorkControl Is Nothing Then Exit Sub
                If ModifierKeys = Keys.Shift Then
                    Me.SelectedWorkControl.DeleteItem()
                End If
            Case Keys.V
                If SelectedWorkControl Is Nothing Then Exit Sub
                If ModifierKeys = Keys.Control Then
                    If Clipboard.ContainsData(DataFormats.FileDrop) Then
                        Dim files As String() = Clipboard.GetData(DataFormats.FileDrop)
                        If files Is Nothing Then
                            Dim obj As String = Clipboard.GetText
                            Dim t As Type = GetType(List(Of Nuctions.GeneFile))
                            If obj = t.FullName Then
                                SelectedWorkControl.TryPasteDNAs(CopiedGeneFiles)
                            End If
                        Else
                            For Each s As String In files
                                Dim fi As New IO.FileInfo(s)
                                If fi.Exists Then
                                    Select Case fi.Extension.ToLower
                                        Case ".vxt"
                                            LoadTabFromFile(fi.FullName)
                                        Case ".gb"
                                            SelectedWorkControl.LoadVectorFromFile(fi.FullName)
                                        Case ".vct"
                                            SelectedWorkControl.LoadVectorFromFile(fi.FullName)
                                        Case ".txt"
                                            SelectedWorkControl.LoadSequenceFromFile(fi.FullName)
                                        Case ".seq"
                                            SelectedWorkControl.LoadSequenceFromFile(fi.FullName)
                                        Case ".ab1"
                                            SelectedWorkControl.LoadSequencingResultFromFile(fi.FullName)

                                    End Select
                                End If
                            Next
                        End If
                    ElseIf Clipboard.ContainsData(DataFormats.Text) Then
                        Dim str As String = Clipboard.GetData(DataFormats.Text)
                        Dim wc As WorkControl = SelectedWorkControl
                        If Not (wc Is Nothing) Then

                            If TypeOf wc.pvMain.tcMain.SelectedTab.Controls(0) Is PropertyControl Then
                                Dim pc As PropertyControl = wc.pvMain.tcMain.SelectedTab.Controls(0)
                                If pc.TabControl_Operation.SelectedTab.Text = "PCR" Then
                                    '粘贴引物的
                                    Dim rgxPCR As New System.Text.RegularExpressions.Regex("(\w+)[\s\:]+([\w><]+)\s+(\w+)[\s\:]+([\w><]+)")
                                    If rgxPCR.IsMatch(str) Then
                                        Dim m As System.Text.RegularExpressions.Match = rgxPCR.Match(str)
                                        pc.tbFP.Text = m.Groups(1).Value
                                        pc.tbRP.Text = m.Groups(3).Value
                                        pc.PCR_ForwardPrimer_TextBox.Text = m.Groups(2).Value
                                        pc.PCR_ReversePrimer_TextBox.Text = m.Groups(4).Value
                                    End If
                                ElseIf pc.TabControl_Operation.SelectedTab.Text = "Screen" Then
                                    If pc.Screen_PCR.Checked Then
                                        Dim rgxPCR As New System.Text.RegularExpressions.Regex("(\w+)[\s\:]+([\w><]+)\s+(\w+)[\s\:]+([\w><]+)")
                                        If rgxPCR.IsMatch(str) Then
                                            Dim m As System.Text.RegularExpressions.Match = rgxPCR.Match(str)
                                            pc.tbSCRFP.Text = m.Groups(1).Value
                                            pc.tbSCRRP.Text = m.Groups(3).Value
                                            pc.Screen_PCR_F.Text = m.Groups(2).Value
                                            pc.Screen_PCR_R.Text = m.Groups(4).Value
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    End If

                End If
            Case Keys.Enter
                If SelectedWorkControl Is Nothing Then Exit Sub
                If SelectedWorkControl.SourceMode Then
                    SelectedWorkControl.AcceptMode()
                End If
            Case Keys.Escape
                If SelectedWorkControl Is Nothing Then Exit Sub
                If SelectedWorkControl.SourceMode Then
                    SelectedWorkControl.ExitMode()
                End If
            Case Keys.F3
                If SelectedWorkControl Is Nothing Then Exit Sub
                SelectedWorkControl.SwitchSplitter()
            Case Keys.F
                If Control.ModifierKeys = Keys.Control Then
                    If SelectedWorkControl Is Nothing Then Exit Sub
                    tstbSearch.Focus()
                End If
        End Select
    End Sub

    Public Sub AddDNAViewTab(ByVal gf As Nuctions.GeneFile, ByVal ez As List(Of String), Optional ByVal FileAddress As String = "", Optional ByVal Username As String = "", Optional ByVal Password As String = "")
        Dim tp As New CustomTabPage
        Dim sv As New SequenceViewer
        If tcMainHost.TabPages.Count > 0 Then
            tcMainHost.TabPages.Insert(tcMainHost.SelectedIndex + 1, tp)
        Else
            tcMainHost.TabPages.Add(tp)
        End If
        AddHandler sv.CloseTab, AddressOf OnTabClose
        tp.Controls.Add(sv)
        tp.Text = gf.Name
        tp.SetCustomStyle(CustomTabPageStyle.Vector)
        sv.Dock = DockStyle.Fill
        sv.RestrictionSites = ez
        sv.GeneFile = gf
        sv.FileAddress = FileAddress
        sv.RemoteUserName = Username
        sv.RemotePassword = Password
        sv.ParentTab = tp
        sv.ClearMode()
        tp.Select()
    End Sub
    Public Sub AddTmViewerTab(ByVal gf As Nuctions.GeneFile)
        Dim tp As New CustomTabPage
        Dim tv As New TmViewer
        tp.SetCustomStyle(CustomTabPageStyle.TmViewer)
        tcMainHost.TabPages.Add(tp)
        tp.Controls.Add(tv)
        tv.GeneFile = gf
        tp.Text = gf.Name + " - Tm"
        tv.Dock = DockStyle.Fill
        tp.Select()
    End Sub
    Private Sub tsbRecalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRecalculate.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.RecalculateAllChildren()
    End Sub
    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        NewTab()
    End Sub
    Private Sub tsbOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOpen.Click
        LoadTab()
    End Sub
    Public Sub SaveTab()
        If Not (SelectedWorkControl Is Nothing) Then
            Me.SelectedWorkControl.Save()
        ElseIf Not (SelectedSequenceTab Is Nothing) Then
            Me.SelectedSequenceTab.Save()
        End If
    End Sub
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        SaveTab()
    End Sub
    Private Sub tsbVector_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbVector.Click
        If (Not SelectedWorkControl Is Nothing) Then
            SelectedWorkControl.LoadVector()
        End If
    End Sub

    Private Sub tsbOperation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEnzyme.Click, tsbPCR.Click, tsbModify.Click,
        tsbGel.Click, tsbLigate.Click, tsbRecombine.Click,
    tsbRestrictionAnalysis.Click, tsbScreen.Click, tsbSequencingResult.Click, tsbFreeDesign.Click, tsbHashPicker.Click,
    tsbMerge.Click, tsbCompare.Click, tsbHost.Click, tsbTransformation.Click, tsbIncubation.Click, tsbExtraction.Click, tsbExpression.Click, tsbGibson.Click, tsbCRISPR.Click
        Dim obj As ToolStripButton = sender
        AddOperationByMenu(obj.Tag)
    End Sub
    Public Sub AddOperationByMenu(ByVal Method As Nuctions.MolecularOperationEnum)
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.AddNewOperation(Method, True)
    End Sub
    Public Sub AddOperation(ByVal Method As Nuctions.MolecularOperationEnum)
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.AddNewOperation(Method, False)
    End Sub

    Private Sub tsbCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCopy.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.OnGroupCopy()
    End Sub

    Private Sub tsbPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPaste.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.GroupPaste()
    End Sub

    Private Sub CopyGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyGroupToolStripMenuItem.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.OnGroupCopy()
    End Sub

    Private Sub PasteGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteGroupToolStripMenuItem.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.GroupPaste()
    End Sub

    Private Sub FunctionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeDigestionFToolStripMenuItem.Click, PCRToolStripMenuItem.Click, ModificationMToolStripMenuItem.Click,
    GelElectrophoresisGToolStripMenuItem.Click, LigationToolStripMenuItem.Click, ScreenToolStripMenuItem.Click, RecombinationToolStripMenuItem.Click, RestrictionAnalysisToolStripMenuItem.Click,
    MergeToolStripMenuItem.Click, FreeDesignToolStripMenuItem.Click, HashPickerUToolStripMenuItem.Click, SequencingResultNToolStripMenuItem.Click, SequenceCompareLToolStripMenuItem.Click,
    HostToolStripMenuItem.Click, TransformationToolStripMenuItem.Click, IncubationToolStripMenuItem.Click, ExtractionToolStripMenuItem.Click, ExpressionToolStripMenuItem.Click, GibsonDesignToolStripMenuItem.Click,
    CRISPRCutToolStripMenuItem.Click
        Dim obj As ToolStripMenuItem = sender
        AddOperationByMenu(obj.Tag)
    End Sub

    Private Sub GeneFromOnlineDatabase(sender As Object, e As EventArgs) Handles OnlineDatabaseToolStripMenuItem.Click, tsbOnline.Click
        If SelectedWorkControl Is Nothing Then Return
        Dim frmOL As New frmOnline
        frmOL.Owner = Me
        frmOL.ShowDialog()
        If frmOL.Model IsNot Nothing Then
            Dim m = frmOL.Model
            Dim wc = SelectedWorkControl
            For Each gf In m.ObtainedGenes
                wc.LoadVectorFile(gf)
            Next
        End If
    End Sub

    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.DeleteItem()
    End Sub

    Private Sub tsbCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCut.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.OnGroupCopy()
        SelectedWorkControl.DeleteItem(False)
        SelectedWorkControl.UpdateView()
    End Sub

    Private Sub tsbSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSearch.Click
        If Not (SelectedSequenceViewer Is Nothing) Then
            SelectedSequenceViewer.SearchSequence(tstbSearch.Text)
        End If
    End Sub

    Private Sub tsbDatabaseSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDatabaseSave.Click
        If Not (SelectedWorkControl Is Nothing) Then
            If SelectedWorkControl.lv_Chart.SelectedItems.Count > 0 AndAlso SelectedWorkControl.lv_Chart.SelectedItems(0).MolecularInfo.DNAs.Count = 1 Then
                Dim gf As Nuctions.GeneFile = SelectedWorkControl.lv_Chart.SelectedItems(0).MolecularInfo.DNAs(1)
                sfdGene.InitialDirectory = DatabasePath
                If Not (gf.Name Is Nothing) Then sfdGene.FileName = gf.Name

                If sfdGene.ShowDialog = Windows.Forms.DialogResult.OK Then
                    If sfdGene.FileName.ToLower.EndsWith(".vct") Then
                        Dim fi As New IO.FileInfo(sfdGene.FileName)
                        SaveToZXML(New Dictionary(Of String, Object) From {{"DNA", gf}, {"Enzyme", SelectedWorkControl.EnzymeCol}}, sfdGene.FileName)
                        If fi.FullName.IndexOf(DatabasePath) > -1 Then
                            RemoveDatabaseMenus()
                            LoadDatabase()
                        End If
                    Else
                        gf.WriteToFile(sfdGene.FileName)
                        Dim fi As New IO.FileInfo(sfdGene.FileName)
                        gf.WriteIndexFile(fi.FullName.Substring(0, fi.FullName.LastIndexOf(".")) + ".idx")
                        If fi.FullName.IndexOf(DatabasePath) > -1 Then
                            RemoveDatabaseMenus()
                            LoadDatabase()
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub tsbRemoveFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRemoveFeature.Click
        If Not (SelectedSequenceViewer Is Nothing) Then
            Dim f = SelectedSequenceViewer.RemoveFeature()
            If f IsNot Nothing AndAlso SelectedWorkControl IsNot Nothing Then SelectedWorkControl.FeatureCol.Remove(f)
        End If
    End Sub

    Private Sub tsbAddFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAddFeature.Click
        If Not (SelectedSequenceViewer Is Nothing) Then
            Dim f = SelectedSequenceViewer.AddFeature()
            If f IsNot Nothing AndAlso SelectedWorkControl IsNot Nothing Then SelectedWorkControl.FeatureCol.Add(f)
        End If
    End Sub

    Private Sub tsbManageFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbManageFeature.Click
        If SelectedSequenceViewer Is Nothing Then Return 'this function must be linked to a selected sequence viewer
        Dim _Annotation = SelectedSequenceViewer.ManageFeature()
        Dim foundFeature As Nuctions.Feature
        If SelectedWorkControl Is Nothing Then Return
        For Each _Feature In SelectedWorkControl.FeatureCol
            If _Feature Is _Annotation.Feature Then
                foundFeature = _Feature
                Exit For
            End If
        Next

        If foundFeature IsNot Nothing Then SelectedWorkControl.FeatureCol.Remove(foundFeature)

        Dim newFeature As New Nuctions.Feature(_Annotation.Label, _Annotation.GetSuqence, _Annotation.Label, _Annotation.Type, _Annotation.Note)
        _Annotation.Feature = newFeature
        SelectedWorkControl.FeatureCol.Add(newFeature)
    End Sub


    Private Sub ManageCodonTablesMToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageCodonTablesMToolStripMenuItem.Click
        Dim TP As New CustomTabPage
        Dim foundPage As Boolean = False

        'try to find existing editor
        For Each openTab As TabPage In tcMainHost.TabPages
            If TypeOf openTab.Controls(0) Is InteropHost Then
                Dim iHost As InteropHost = openTab.Controls(0)
                If TypeOf iHost.Child Is WPFCodonAnalyzer Then
                    TP = openTab
                    foundPage = True
                End If
            End If
        Next

        'New setting file editor
        If Not foundPage Then
            Dim wpfCdnAnl As New WPFCodonAnalyzer
            Dim iHost As New InteropHost With {.Child = wpfCdnAnl}

            iHost.Dock = DockStyle.Fill
            AddHandler wpfCdnAnl.CloseTab, AddressOf OnTabClose

            wpfCdnAnl.ParentTab = TP
            TP.Text = "Codon Table Management"
            TP.Controls.Add(iHost)
            TP.SetCustomStyle(CustomTabPageStyle.CodonManager)
            tcMainHost.TabPages.Add(TP)
        End If

        tcMainHost.SelectedTab = TP
    End Sub
    Public Delegate Sub WebPageDelegate(ByVal vURL As String)

    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Dim tp As TabPage = tcMainHost.SelectedTab
        TryCloseTab(tp)
    End Sub

    Private Sub JunctionSequenceDesignerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JunctionSequenceDesignerToolStripMenuItem.Click
        If Not (SelectedWorkControl Is Nothing) Then
            Dim JSC As New JunctionSequenceDesigner
            JSC.RelatedWorkControl = SelectedWorkControl
            JSC.Owner = Me
            JSC.Show()
        End If
    End Sub


    Private Sub tsbAddPrintPage_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsbAddPrintPage.MouseDown
        If Not (SelectedWorkControl Is Nothing) Then
            SelectedWorkControl.PrintMode = True
            tsbAddPrintPage.Checked = SelectedWorkControl.PrintMode
            SelectedWorkControl.AddPrintPage()
        End If
    End Sub
    Private Sub tsbPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPrint.Click
        If Not (SelectedWorkControl Is Nothing) Then
            If SelectedWorkControl.PrintMode Then
                SelectedWorkControl.PrintMode = False
                tsbAddPrintPage.Visible = False
            Else
                SelectedWorkControl.PrintMode = True
                tsbAddPrintPage.Visible = True
            End If
        End If
    End Sub

    Private Sub AboutAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutAToolStripMenuItem.Click
        Dim fa As New frmAbout
        fa.ShowDialog()
    End Sub

    Private Sub ConnectServerXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectServerXToolStripMenuItem.Click
        Dim tp As New CustomTabPage
        tcMainHost.TabPages.Add(tp)
        Dim cbClient As New ClientBrowswer
        tp.Controls.Add(cbClient)
        tp.Text = "Vexcutor Client"
        tp.SetCustomStyle(CustomTabPageStyle.Server)
        cbClient.Dock = DockStyle.Fill
    End Sub

#Region "处理远程调用的代码"
    Friend Function SaveToRemote(ByVal vIP As String, ByVal vName As String, ByVal vPassword As String, ByVal vFilename As String, ByVal vFile As Byte()) As Boolean
        Dim rService As BaseService = Nothing
        Dim uIP As String = vIP + ":8928"
        Try
            If smMain.ConnectTo(uIP, "Synthenome Vexcutor", rService) Then
                Dim login As New Login
                login.UserName = vName
                login.Password = vPassword
                Return rService.Save(login, vFilename, Convert.ToBase64String(vFile))
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Public Shared ClientSettingAddress As String = Application.StartupPath + "\VXCSNT.vsn"
    Public Shared ClientSetting As New ClientSetting
    '下面需要制造一个通用的Load函数
    Friend Sub LoadFile(ByVal vFilename As String, Optional ByVal vUserName As String = "", Optional ByVal vPassword As String = "")
        '首先解析vFilename的地址协议 确认是本机地址还是远程地址 然后查询是否有有效的链接
        Dim remotergx As New System.Text.RegularExpressions.Regex("tcp:\\\\([\w\.\s\-]+)")
        If remotergx.IsMatch(vFilename) Then
            '问题是如何确认用哪个账户来加载远程文件？
            Dim m As System.Text.RegularExpressions.Match = remotergx.Match(vFilename)
            Dim vIP As String = m.Groups(1).Value
            Dim subFilename As String = vFilename.Substring(m.Groups(1).Index + m.Groups(1).Length)

            '优先使用提供的 
            Dim success As Boolean = False
            If Not (vUserName Is Nothing) AndAlso Not (vPassword Is Nothing) AndAlso vUserName.Length > 0 AndAlso vPassword.Length > 0 Then
                Try
                    success = LoadFromRemote(vIP, vUserName, vPassword, subFilename, vFilename)
                Catch ex As Exception

                End Try
            End If
            '然后使用保存的记录
            If Not success Then
                For Each acc As Account In ClientSetting.Accounts
                    If vIP = acc.IP Then
                        vUserName = acc.UserName
                        vPassword = acc.Password
                        If Not (vUserName Is Nothing) AndAlso Not (vPassword Is Nothing) AndAlso vUserName.Length > 0 AndAlso vPassword.Length > 0 Then
                            Try
                                success = LoadFromRemote(vIP, vUserName, vPassword, subFilename, vFilename)
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                Next
            End If

            '最后向用户询问用户名密码
            If Not success Then
                Dim frmQuery As New LoginServer
                If frmQuery.ShowDialog = Windows.Forms.DialogResult.OK Then
                    vUserName = frmQuery.UsernameTextBox.Text
                    vPassword = frmQuery.PasswordTextBox.Text
                    If Not (vUserName Is Nothing) AndAlso Not (vPassword Is Nothing) AndAlso vUserName.Length > 0 AndAlso vPassword.Length > 0 Then
                        Try
                            success = LoadFromRemote(vIP, vUserName, vPassword, subFilename, vFilename)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            End If
        End If
    End Sub

    Friend Function LoadFromRemote(ByVal vIP As String, ByVal vName As String, ByVal vPassword As String, ByVal vSubFilename As String, ByVal vFilename As String) As Boolean
        Dim rService As BaseService = Nothing
        Dim uIP As String = vIP + ":8928"
        Try
            If smMain.ConnectTo(uIP, "Synthenome Vexcutor", rService) Then
                Dim login As New Login
                login.UserName = vName
                login.Password = vPassword
                Dim buf As Byte() = Convert.FromBase64String(rService.Load(login, vSubFilename))
                If vSubFilename.ToLower.EndsWith(".vxt") Then
                    'project file
                    Dim wc As WorkControl = WorkControl.LoadFrom(buf, vFilename)
                    wc.Dock = DockStyle.Fill
                    AddHandler wc.CloseWorkControl, AddressOf OnTabClose
                    AddHandler wc.LoadWorkControl, AddressOf OnLoadTab
                    Dim TP As New CustomTabPage
                    wc.ParentTab = TP
                    Dim fi As New IO.FileInfo(vSubFilename)
                    TP.Text = fi.Name
                    TP.Controls.Add(wc)
                    If tcMainHost.TabPages.Count > 0 Then
                        tcMainHost.TabPages.Insert(tcMainHost.SelectedIndex, TP)
                        Dim SWC As WorkControl = SelectedWorkControl
                        If SWC.ContentChanged = False And SWC.lv_Chart.Items.Count = 0 Then
                            CloseTab(SWC)
                            TP.Select()
                        End If
                    Else
                        tcMainHost.TabPages.Add(TP)
                    End If
                ElseIf vSubFilename.ToLower.EndsWith(".vct") Then
                    'vector file

                End If
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Friend Sub SaveWorkControlAsToServer(ByVal wc As WorkControl, ByVal oldFileName As String)
        Dim sts As New frmSaveToServer
        sts.lbExtension.Text = ".vxt"
        If oldFileName.ToLower.EndsWith(".vxt") Then
            sts.tbFileName.Text = oldFileName.Substring(0, oldFileName.LastIndexOf("."))
        Else
            sts.tbFileName.Text = oldFileName
        End If
        Dim oldtext As String = wc.ParentTab.Text

        If sts.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim vIP As String
            Dim vName As String
            Dim vPass
            Dim vFileName As String
            vIP = sts.cbIP.Text
            vName = sts.tbName.Text
            vPass = sts.mtbPassword.Text
            vFileName = sts.tbFileName.Text + ".vxt"
            Dim vShortName As String = ParseLevel2Name(wc.FileAddress)
            wc.ParentTab.Text = oldtext + " - Saving..."
            Dim buf As String = Convert.ToBase64String(wc.SaveToBytes)
            Dim thr As New System.Threading.Thread(Sub()
                                                       Try
                                                           Dim rmName As String = SaveToRemote(vIP, vName, vPass, vFileName, buf)
                                                           If rmName.Length > 0 Then
                                                               ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedSaved, wc, ParseTCPName(vIP, rmName), vShortName)
                                                           Else
                                                               ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, wc, vFileName, oldtext)
                                                           End If
                                                       Catch ex As Exception
                                                           ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, wc, vFileName, oldtext)
                                                       End Try
                                                   End Sub)
            thr.Start()
        End If
        sts.Close()
    End Sub
    Friend Sub SaveWorkControlToServer(ByVal wc As WorkControl)
        Dim oldtext As String = wc.ParentTab.Text
        Dim vIP As String
        Dim vName As String
        Dim vPass
        Dim vFileName As String
        vIP = ParseIP(wc.FileAddress)
        vName = wc.RemoteUserName
        vPass = wc.RemotePassword
        vFileName = wc.FileAddress
        Dim vShortName As String = ParseLevel2Name(wc.FileAddress)
        wc.ParentTab.Text = oldtext + " - Saving..."
        Dim buf As String = Convert.ToBase64String(wc.SaveToBytes)
        Dim thr As New System.Threading.Thread(Sub()
                                                   Try
                                                       Dim rmName As String = SaveToRemote(vIP, vName, vPass, vFileName, buf)
                                                       If rmName.Length > 0 Then
                                                           ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedSaved, wc, ParseTCPName(vIP, rmName), vShortName)
                                                       Else
                                                           ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, wc, vFileName, oldtext)
                                                       End If
                                                   Catch ex As Exception
                                                       ThreadHeader(Of WorkControl, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, wc, vFileName, oldtext)
                                                   End Try
                                               End Sub)
        thr.Start()
    End Sub
    Friend Sub SaveSequenceViewerAsToServer(ByVal sv As SequenceViewer, ByVal oldFileName As String)
        Dim sts As New frmSaveToServer
        sts.lbExtension.Text = ".vct"
        If oldFileName.ToLower.EndsWith(".vct") Then
            sts.tbFileName.Text = oldFileName.Substring(0, oldFileName.LastIndexOf("."))
        Else
            sts.tbFileName.Text = oldFileName
        End If


        Dim oldtext As String = sv.ParentTab.Text.Clone

        If sts.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim vIP As String
            Dim vName As String
            Dim vPass
            Dim vFileName As String
            vIP = sts.cbIP.Text
            vName = sts.tbName.Text
            vPass = sts.mtbPassword.Text
            vFileName = sts.tbFileName.Text + ".vct"
            Dim vShortName As String = ParseLevel2Name(sv.FileAddress)
            sv.ParentTab.Text = oldtext + " - Saving..."
            Dim buf As String = Convert.ToBase64String(sv.SaveToBytes)
            Dim thr As New System.Threading.Thread(Sub()
                                                       Try
                                                           Dim rmName As String = SaveToRemote(vIP, vName, vPass, vFileName, buf)
                                                           If rmName.Length > 0 Then
                                                               ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedSaved, sv, ParseTCPName(vIP, rmName), vShortName)
                                                           Else
                                                               ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, sv, vFileName, oldtext)
                                                           End If
                                                       Catch ex As Exception
                                                           ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, sv, vFileName, oldtext)
                                                       End Try
                                                   End Sub)
            thr.Start()
        End If
        sts.Close()
    End Sub
    Friend Sub SaveSequenceViewerToServer(ByVal sv As SequenceViewer)
        Dim oldtext As String = sv.ParentTab.Text.Clone
        Dim vIP As String
        Dim vName As String
        Dim vPass
        Dim vFileName As String
        vIP = ParseIP(sv.FileAddress)
        vName = sv.RemoteUserName
        vPass = sv.RemotePassword
        vFileName = sv.FileAddress
        Dim vShortName As String = ParseLevel2Name(sv.FileAddress)
        sv.ParentTab.Text = oldtext + " - Saving..."
        Dim buf As String = Convert.ToBase64String(sv.SaveToBytes)
        Dim thr As New System.Threading.Thread(Sub()
                                                   Try
                                                       Dim rmName As String = SaveToRemote(vIP, vName, vPass, vFileName, buf)
                                                       If rmName.Length > 0 Then
                                                           ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedSaved, sv, ParseTCPName(vIP, rmName), vShortName)
                                                       Else
                                                           ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, sv, vFileName, oldtext)
                                                       End If
                                                   Catch ex As Exception
                                                       ThreadHeader(Of SequenceViewer, String, String).ControlInvoke(Me, AddressOf RemotedUnsaved, sv, vFileName, oldtext)
                                                   End Try
                                               End Sub)
        thr.Start()
    End Sub
    Private Sub RemotedSaved(ByVal wc As WorkControl, ByVal vFileName As String, ByVal vShortName As String)
        wc.ParentTab.Text = ParseLevel2Name(vFileName)
        wc.FileAddress = vFileName
    End Sub
    Private Sub RemotedUnsaved(ByVal wc As WorkControl, ByVal vFileName As String, ByVal vShortName As String)
        wc.ParentTab.Text = vShortName
        '需要重新调用保存方法
        SaveWorkControlAsToServer(wc, vShortName)
    End Sub
    Private Sub RemotedSaved(ByVal sv As SequenceViewer, ByVal vFileName As String, ByVal vShortName As String)
        If Not (sv.ParentTab Is Nothing) Then sv.ParentTab.Text = sv.GeneFile.Name
    End Sub
    Private Sub RemotedUnsaved(ByVal sv As SequenceViewer, ByVal vFileName As String, ByVal vShortName As String)
        If Not (sv.ParentTab Is Nothing) Then sv.ParentTab.Text = sv.GeneFile.Name
        SaveSequenceViewerAsToServer(sv, vShortName)
    End Sub
    Friend Sub SaveAsToServer() 'ByVal WC As WorkControl, ByVal nUser As String, ByVal nPassword As String, ByVal nFileName As String)
        If Not (SelectedWorkControl Is Nothing) Then
            Dim wc As WorkControl = SelectedWorkControl
            SaveWorkControlAsToServer(wc, wc.ParentTab.Text)
        ElseIf Not (SelectedSequenceViewer Is Nothing) Then
            Dim sv As SequenceViewer = SelectedSequenceViewer
            SaveSequenceViewerAsToServer(sv, sv.ParentTab.Text)
        End If
    End Sub
    Friend Function SaveToRemote(ByVal vIP As String, ByVal vName As String, ByVal vPassword As String, ByVal vSubFilename As String, ByVal buf As String) As String
        Try
            Dim rService As BaseService
            Dim uIP As String = vIP + ":8928"
            If smMain.ConnectTo(uIP, "Synthenome Vexcutor", rService) Then
                Dim login As New Login
                login.UserName = vName
                login.Password = vPassword
                Return rService.Save(login, vSubFilename, buf)
            Else
                Return ""
            End If
            Return True
        Catch ex As Exception
            Return ""
        End Try
    End Function
#End Region

#Region "处理外部调用的代码"

    Friend ReadOnly Property GetXOS As Object
        Get
#If TestMode = 1 Then
            Return New _Misaka
#ElseIf AccessTest = 1 Then
            '        Dim XOS As Object = New _Misaka
            '#ElseIf ReaderMode = 0 Then
            'Dim XOS As Object = railgun.CreateInstance("_Misaka")
            Return New XmlObjectSerializer
#Else
        Dim xos As New XmlObjectSerializer
#End If
        End Get
    End Property
    Friend ReadOnly Property GetXOD As Object
        Get
#If TestMode = 1 Then
            Return New _Mikoto
#ElseIf AccessTest = 1 Then
            '        Dim XOD As Object = New _Mikoto
            '#ElseIf ReaderMode = 0 Then
            'Dim XOD As Object = railgun.CreateInstance("_Mikoto")
            Return New XmlObjectDeserializer
            'Dim keys As String = XOD.GetKey
            'MsgBox(keys)

#Else
        Dim XOD As New XmlObjectDeserializer
#End If
        End Get
    End Property
    '这个是为了防止盗版设计的麻烦系统
    Friend Sub SaveToZXML(ByVal vDict As Dictionary(Of String, Object), ByVal vFilename As String)
        Dim XOS = GetXOS
        XOS.Michael(vDict)
        XOS.SaveGZip(vFilename)
    End Sub
    Friend Function SaveToZXMLBytes(ByVal vDict As Dictionary(Of String, Object)) As Byte()
        Dim XOS = GetXOS
        XOS.Michael(vDict)
        Return XOS.SaveToZipBytes
    End Function
    Friend Function LoadFromZXML(ByVal vList As List(Of String), ByVal vFilename As String) As Dictionary(Of String, Object)

        Dim XOD = GetXOD
        Return XOD.Michael(vFilename, vList)
    End Function
    Friend Function LoadFromZXMLBytes(ByVal vList As List(Of String), ByVal buf As Byte()) As Dictionary(Of String, Object)
        Dim XOD = GetXOD
        Return XOD.Michael(buf, vList)
    End Function
#End Region

    Private Sub SaveToServerZToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToServerZToolStripMenuItem.Click
        SaveAsToServer()
    End Sub

    Private Sub HelpHToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpHToolStripMenuItem1.Click
        System.Diagnostics.Process.Start("https://mcds.codeplex.com/")
    End Sub

#Region "View 菜单"
    Private Sub ExperimentLogLToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExperimentLogLToolStripMenuItem.Click
        '导出
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.Export()
        End If
    End Sub
    Private Sub ZoomInToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomInToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.ZoomIn()
        End If
    End Sub

    Private Sub ZoomOutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomOutToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.ZoomOut()
        End If
    End Sub

    Private Sub ResetZoomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetZoomToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.ZoomReset()
        End If
    End Sub

    Private Sub MoveUPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUPToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.MoveUp()
        End If
    End Sub

    Private Sub MoveDownToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.MoveDown()
        End If
    End Sub

    Private Sub MoveLeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveLeftToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.MoveLeft()
        End If
    End Sub

    Private Sub MoveRightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveRightToolStripMenuItem.Click
        Dim wc As WorkControl = SelectedWorkControl
        If Not (wc Is Nothing) Then
            wc.lv_Chart.MoveRight()
        End If
    End Sub
#End Region

    Private Sub ManageDigestionBufferBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageDigestionBufferBToolStripMenuItem.Click
        Dim TP As New CustomTabPage
        Dim foundPage As Boolean = False

        'try to find existing editor
        For Each openTab As TabPage In tcMainHost.TabPages
            If TypeOf openTab.Controls(0) Is InteropHost Then
                Dim iHost As InteropHost = openTab.Controls(0)
                If TypeOf iHost.Child Is WPFDigestionBufferManager Then
                    TP = openTab
                    foundPage = True
                End If
            End If
        Next

        'New setting file editor
        If Not foundPage Then
            Dim wpfRecMan As New WPFDigestionBufferManager
            Dim iHost As New InteropHost With {.Child = wpfRecMan}

            iHost.Dock = DockStyle.Fill
            AddHandler wpfRecMan.CloseTab, AddressOf OnTabClose
            ' wpfRecMan.CloseTab, AddressOf OnLoadTab

            wpfRecMan.ParentTab = TP
            TP.Text = "Digestion Buffer Management"
            TP.Controls.Add(iHost)
            TP.SetCustomStyle(CustomTabPageStyle.BufferManager)
            tcMainHost.TabPages.Add(TP)
        End If

        tcMainHost.SelectedTab = TP
    End Sub

    Private Sub SwithSplitViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwithSplitViewToolStripMenuItem.Click
        If SelectedWorkControl Is Nothing Then Exit Sub
        SelectedWorkControl.SwitchSplitter()
    End Sub

    'Private Sub CommunityToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommunityToolStripMenuItem.Click
    '    Process.Start("http://www.synthenome.com/community")
    'End Sub

    'Private Sub UpdateUserInformationStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateUserInformationStripMenuItem.Click
    '    Dim frmUpdate As New frmUpdateUserInformation
    '    frmUpdate.ShowDialog()
    'End Sub

    'Private Sub AuthorizeStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuthorizeStripMenuItem.Click
    '    Dim auth As New Authorization
    '    auth.ShowDialog("Authorize Access to Other Users", 640, 480)
    'End Sub
    'Private Sub PrivilegeStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrivilegeStripMenuItem.Click
    '    Dim priv As New Privilege
    '    priv.ShowDialog("Accept Access from Other Users", 640, 480)
    'End Sub

End Class
