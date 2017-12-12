Imports Errisy
Public Class MainUI
    Public Sub New()
        System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(Me)
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Private Sub MainUI_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        'Load Advertisement Information


    End Sub
 

#Region "File Menu"

    Private Sub NewProject(sender As Object, e As Windows.RoutedEventArgs)
        Dim _NewProjectTab As New ClosableTabItem With {.Header = "Test"}
        AddHandler _NewProjectTab.TabClose, AddressOf WhenTabClosing
        _NewProjectTab.Content = New WPFWorkControl
        tcHost.Items.Add(_NewProjectTab)
        tcHost.SelectedItem = _NewProjectTab
    End Sub
    Private Sub OpenFile(sender As Object, e As Windows.RoutedEventArgs)
        If SettingEntry.OpenFileDialog.ShowDialog Then
            Dim bytes As Byte() = System.IO.File.ReadAllBytes(SettingEntry.OpenFileDialog.FileName)
            Dim obj = SettingEntry.LoadFromZXMLBytes(SettingEntry.VectorSerializationList, bytes)

        End If
    End Sub
    Private Sub CloseFile(sender As Object, e As Windows.RoutedEventArgs)
        If TypeOf tcHost.SelectedItem Is Errisy.ClosableTabItem Then
            Dim cTabItem As ClosableTabItem = tcHost.SelectedItem


        End If
    End Sub
    Private Sub SaveFileAs(sender As Object, e As Windows.RoutedEventArgs)

    End Sub
    Private Sub SaveFile(sender As Object, e As Windows.RoutedEventArgs)
      
        
    End Sub
    Private Sub OpenVector(sender As Object, e As Windows.RoutedEventArgs)
        If SettingEntry.OpenGeneDialog.ShowDialog Then
            Dim gf = Nuctions.GeneFile.LoadFromGeneBankFile(SettingEntry.OpenGeneDialog.FileName)
            Dim gfMap As New WPFVectorMap With {.GeneFile = gf, .RestrictionEnzymes = New List(Of String) From {"BamHI", "HindIII", "EcoRI"}}
            Dim VectorMapViewer As New WPFVectorMapControl With {.DataContext = gfMap}
            Dim ti As New ClosableTabItem With {.Content = VectorMapViewer, .Header = gf.Name}
            tcHost.Items.Add(ti)
            tcHost.SelectedItem = ti
        End If
    End Sub
    Private Sub OpenSequence(sender As Object, e As Windows.RoutedEventArgs)
        If SettingEntry.OpenGeneDialog.ShowDialog Then
            Dim gf = Nuctions.GeneFile.LoadFromGeneBankFile(SettingEntry.OpenGeneDialog.FileName)
            Dim gfSequence As New WPFSequenceView With {.GeneFile = gf, .RestrictionEnzymes = New List(Of String) From {"BamHI", "HindIII", "EcoRI"}}
            gfSequence.Load()
            Dim VectorSequenceControl As New WPFSequenceControl With {.DataContext = gfSequence}
            Dim ti As New ClosableTabItem With {.Content = VectorSequenceControl, .Header = gf.Name}
            tcHost.Items.Add(ti)
            tcHost.SelectedItem = ti
        End If
    End Sub
    Private Sub WhenTabClosing(sender As Object, e As Windows.RoutedEventArgs)

    End Sub

    Private Sub AddRecentFileMenuItem(fileaddress As String)
        Dim rfi As New System.IO.FileInfo(fileaddress)
        Dim mit As System.Windows.Forms.ToolStripMenuItem
        mit = New System.Windows.Forms.ToolStripMenuItem
        mit.Text = rfi.Name
        mit.ToolTipText = rfi.FullName
        AddHandler mit.Click, AddressOf OnClickRecentFileMenuItem
        Dim dList As New List(Of System.Windows.Controls.Ribbon.RibbonApplicationMenuItem)
        For Each tsm As System.Windows.Controls.Ribbon.RibbonApplicationMenuItem In RecentFilesMenuItem.Items
            If tsm.ToolTip = rfi.FullName Then
                dList.Add(tsm)
            End If
        Next
        RecentFilesMenuItem.Items.Insert(0, mit)
        For Each tsm As System.Windows.Controls.Ribbon.RibbonApplicationMenuItem In dList
            RecentFilesMenuItem.Items.Remove(tsm)
            RemoveHandler tsm.Click, AddressOf OnClickRecentFileMenuItem
        Next
        While RecentFilesMenuItem.Items.Count > 12
            RecentFilesMenuItem.Items.Remove(RecentFilesMenuItem.Items(12))
        End While
    End Sub
    Public Sub LoadTabFromFile(ByVal filename As String)
        '        AddRecentFileMenuItem(filename)
        '        Dim lf As String = filename.ToLower
        '        If lf.EndsWith(".stone") Or lf.EndsWith(".vxt") Then


        '            Dim wc As WorkControl = WorkControl.LoadFrom(filename)
        '            If wc Is Nothing Then
        '                Dim frmErr As New frmError
        '                Dim ei As New VexError With {.ErrorTitle = "Failed to open a file locked by other users.",
        '                                             .ErrorMessage = <A>Vexcutor has encrypted all your files to protect intellectual properties as well as other users'. 
        'To obtain access to files from other users, ask the user go to Menu "Setting"->"Authorize Access" to grant access to you. 
        'Once granted, you can use Menu "Setting"->"Accept Access" to accept the Access License. Each Access License costs US$20.
        'To make your file public available, please visit www.synthenome.org for publication assistance.</A>}
        '                frmErr.VexcutorErrorInfo.ErrorInfo = ei
        '                frmErr.ShowDialog()
        '                Exit Sub
        '            End If

        '            wc.Dock = DockStyle.Fill
        '            AddHandler wc.CloseWorkControl, AddressOf WhenTabClosing
        '            'AddHandler wc.LoadWorkControl, AddressOf OnLoadTab

        '            Dim _NewProjectTab As New ClosableTabItem With {.Header = "Test"}
        '            AddHandler _NewProjectTab.TabClose, AddressOf WhenTabClosing
        '            _NewProjectTab.Content = New WPFWorkControl
        '            tcHost.Items.Add(_NewProjectTab)
        '            tcHost.SelectedItem = _NewProjectTab
        '            DirectCast(_NewProjectTab.Content, WPFWorkControl)

        '            Dim TP As New CustomTabPage
        '            wc.ParentTab = TP
        '            Dim fi As New IO.FileInfo(filename)
        '            TP.Text = fi.Name
        '            TP.Controls.Add(wc)
        '            TP.SetCustomStyle(CustomTabPageStyle.Project)
        '            If tcHost.Items.Count > 0 Then
        '                tcHost.Items.Insert(tcHost.SelectedIndex, TP)
        '                tcHost.SelectedItem = TP
        '                Dim SWC As WorkControl = SelectedWorkControl
        '                If Not (SWC Is Nothing) Then
        '                    If SWC.ContentChanged = False And SWC.lv_Chart.Items.Count = 0 Then
        '                        CloseTab(SWC)
        '                        TP.Select()
        '                    End If
        '                End If
        '            Else
        '                tcHost.Items.Add(TP)
        '            End If
        '        ElseIf lf.EndsWith(".vct") Then
        '            Dim dict As Dictionary(Of String, Object) = WPFEntry.LoadFromZXML(WPFEntry.VectorSerializationList, filename)
        '            If dict Is Nothing Then Exit Sub
        '            Dim gf As Nuctions.GeneFile = dict("DNA")
        '            Dim REs As New List(Of String)
        '            If Not dict("Enzyme") Is Nothing Then
        '                REs = dict("Enzyme")
        '            End If
        '            AddDNAViewTab(gf, REs, filename)
        '        ElseIf lf.EndsWith(".gb") Then
        '            Dim gf As Nuctions.GeneFile = Nuctions.GeneFile.LoadFromGeneBankFile(filename)
        '            AddDNAViewTab(gf, New List(Of String), lf)
        '        End If

    End Sub
    Private Sub OnClickRecentFileMenuItem(sender As Object, e As Windows.RoutedEventArgs)

    End Sub

#End Region
#Region "Operation Menu"
    Private ReadOnly Property IsProject As Boolean
        Get
            Return TypeOf tcHost.SelectedContent Is WPFWorkControl
        End Get
    End Property
    Private ReadOnly Property SelectedWPFWorkControl As WPFWorkControl
        Get
            If TypeOf tcHost.SelectedContent Is WPFWorkControl Then
                Return tcHost.SelectedContent
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private Sub btn_LoadDNA(sender As Object, e As Windows.RoutedEventArgs)
        If Not TypeOf tcHost.SelectedItem Is WPFWorkSpaceViewModel Then Return
        Dim wsvmSelected As WPFWorkSpaceViewModel = tcHost.SelectedItem

        Dim ofdVector As New Microsoft.Win32.OpenFileDialog With {.Filter = "GeneBank File|*.gb;*.gbk"}
        Dim newChartItem As New ChartItem() With {.MolecularInfo = New DNAInfo With {.MolecularOperation = Nuctions.MolecularOperationEnum.Vector}}
        Dim di As New DNAInfo

        wsvmSelected.ChartItems.Add(di)

    End Sub
#End Region
#Region "Print Menu"

#End Region

 


#Region "Settings Ribbon"
    Private Sub ManageRecombinationSets(sender As Object, e As Windows.RoutedEventArgs)
        Dim _WFPRecombinationSetManager As New WPFRecombinationSetManager
        Dim ti As New ClosableTabItem With {.Content = _WFPRecombinationSetManager, .Header = "Recombination Sets Manager"}
        tcHost.Items.Add(ti)
        tcHost.SelectedItem = ti
    End Sub
    Private Sub ManageRestrictionEnzymes(sender As Object, e As Windows.RoutedEventArgs)
        Dim _WFPRestrictionEnzymeManager As New WPFRestrictionEnzymeManager
        Dim ti As New ClosableTabItem With {.Content = _WFPRestrictionEnzymeManager, .Header = "Restriction Enzyme Manager"}
        tcHost.Items.Add(ti)
        tcHost.SelectedItem = ti
    End Sub
#End Region




End Class
