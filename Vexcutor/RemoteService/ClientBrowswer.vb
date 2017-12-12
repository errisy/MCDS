Public Class ClientBrowswer


    Private rService As BaseService

#Region "列出服务内容"
    Private Sub llConnect_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llConnect.LinkClicked
        Connect()
    End Sub
    Private Sub EnterKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbName.KeyDown, mtbPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            Connect()
        End If
    End Sub
    Private Sub Connect()
        lbStatus.Text = "Connecting..."
        Dim login As New Login With {.UserName = tbName.Text, .Password = mtbPassword.Text}
        Dim sip As String = cbIP.Text
        Dim ip As String = sip + ":8928"
        Dim thr As New Threading.Thread(Sub()
                                            'If frmMain.smMain.ConnectTo(ip, "Synthenome Vexcutor", rService) Then
                                            '    Dim result As String = rService.List(login, "")
                                            '    ThreadHeader(Of String, String).ControlInvoke(Me, AddressOf OnList, sip, result)
                                            'Else
                                            '    ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Connection Failed")
                                            'End If
                                        End Sub)

        Me.Enabled = False
        thr.Start()
    End Sub

    Public Sub OnList(ByVal IP As String, ByVal info As String)
        If info Is Nothing Then
            lbStatus.Text = "Error in Search"
        Else
            Dim uNode As BaseService.Node
            Try
                uNode = BinarySerializer.FromString(info)
            Catch ex As Exception
                lbStatus.Text = "Error in Search"
                Exit Sub
            End Try
            Dim tnRoot As New TreeNode(uNode.Name, 0, 0)
            tnRoot.Name = uNode.Path
            tvFolder.Nodes.Clear()
            tvFolder.Nodes.Add(tnRoot)

            AppendSubNodes(tnRoot, uNode, IP)
            Me.Enabled = True
            lbStatus.Text = "Listed."
        End If 
    End Sub
    Public Sub AppendSubNodes(ByVal lvNode As TreeNode, ByVal uNode As BaseService.Node, ByVal sIP As String)
        For Each vNode As BaseService.Node In uNode.Nodes
            Dim tnRoot As New TreeNode(vNode.Name, vNode.Type, vNode.Type)
            tnRoot.Name = String.Format("tcp:\\{0}{1}", sIP, vNode.Path)
            lvNode.Nodes.Add(tnRoot)
            AppendSubNodes(tnRoot, vNode, sIP)
        Next
        If lvNode.Level < 3 Then lvNode.Expand()
    End Sub
#End Region

#Region "通用失败回叫"
    Public Sub OnFail(ByVal info As String)
        lbStatus.Text = info
        Me.Enabled = True
    End Sub
#End Region

#Region "服务器历史记录"
    Private Sub llSave_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSave.LinkClicked
        Dim acc As Account
        If cbSaveLoginInfo.Checked Then
            acc = New Account With {.IP = cbIP.Text, .UserName = tbName.Text, .Password = mtbPassword.Text}
        Else
            acc = New Account With {.IP = cbIP.Text}
        End If
        If Not (acc Is Nothing) Then
            frmMain.ClientSetting.AddAccount(acc)
            BinarySerializer.ToFile(frmMain.ClientSetting, frmMain.ClientSettingAddress)
        End If
    End Sub

    Private Sub llDelete_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDelete.LinkClicked
        Dim acc As Account
        If cbIP.SelectedIndex > -1 Then
            acc = cbIP.Items(cbIP.SelectedIndex)
            If frmMain.ClientSetting.Accounts.Contains(acc) Then
                frmMain.ClientSetting.Accounts.Remove(acc)
                BinarySerializer.ToFile(frmMain.ClientSetting, frmMain.ClientSettingAddress)
            End If
        End If
    End Sub

    Private Sub ClientBrowswer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each acc As Account In frmMain.ClientSetting.Accounts
            cbIP.Items.Add(acc)
        Next
    End Sub

    Private Sub cbIP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIP.SelectedIndexChanged
        If cbIP.SelectedIndex > -1 Then
            Try
                Dim acc As Account = cbIP.Items(cbIP.SelectedIndex)
                If Not (acc.UserName Is Nothing) Then tbName.Text = acc.UserName
                If Not (acc.Password Is Nothing) Then mtbPassword.Text = acc.Password
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

#Region "视图右键菜单"
    Private Sub lvResults_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvResults.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right AndAlso (lvResults.SelectedItems.Count > 0) Then
            Dim tsmi As ToolStripMenuItem
            Dim dpmi As ToolStripMenuItem

            Select Case lvResults.SelectedItems(0).ImageIndex
                Case 0
                Case 1, 2, 3
                    'vector
                    tsmi = cmsVector.Items(1)
                    For Each dpmi In tsmi.DropDownItems
                        RemoveHandler dpmi.Click, AddressOf LoadToProject
                    Next
                    tsmi.DropDownItems.Clear()

                    dpmi = tsmi.DropDownItems.Add("New")
                    AddHandler dpmi.Click, AddressOf LoadToProject

                    Stop 'Add Handler
                    'For Each tp As TabPage In frmMain.tcMainHost.TabPages
                    '    If TypeOf tp.Controls(0) Is WorkControl Then
                    '        dpmi = tsmi.DropDownItems.Add(tp.Text)
                    '        dpmi.Tag = tp.Controls(0)
                    '        AddHandler dpmi.Click, AddressOf LoadToProject
                    '    End If
                    'Next
                    cmsVector.Show(lvResults.PointToScreen(e.Location))
                Case 4, 5, 6
                    'project
                    cmsProject.Show(lvResults.PointToScreen(e.Location))
            End Select
        End If
    End Sub
    Private Sub tvFolder_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvFolder.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right AndAlso Not (tvFolder.SelectedNode Is Nothing) Then
            Dim tsmi As ToolStripMenuItem
            Dim dpmi As ToolStripMenuItem

            Select Case tvFolder.SelectedNode.ImageIndex
                Case 0
                Case 1, 2, 3
                    'vector
                    tsmi = cmsVector.Items(1)
                    For Each dpmi In tsmi.DropDownItems
                        RemoveHandler dpmi.Click, AddressOf LoadToProject
                    Next
                    tsmi.DropDownItems.Clear()

                    dpmi = tsmi.DropDownItems.Add("New")
                    AddHandler dpmi.Click, AddressOf LoadToProject

                    Stop 'AddHandler
                    'For Each tp As TabPage In frmMain.tcMainHost.TabPages
                    '    If TypeOf tp.Controls(0) Is WorkControl Then
                    '        dpmi = tsmi.DropDownItems.Add(tp.Text)
                    '        dpmi.Tag = tp.Controls(0)
                    '        AddHandler dpmi.Click, AddressOf LoadToProject
                    '    End If
                    'Next
                    cmsVector.Show(tvFolder.PointToScreen(e.Location))
                Case 4, 5, 6
                    'project
                    cmsProject.Show(tvFolder.PointToScreen(e.Location))
            End Select
        End If
    End Sub
#End Region

#Region "远程读取文件"
    Private Sub tsmOpenVector_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmOpenVector.Click
        LoadFile(SelectItem)
    End Sub

    Private Sub tsmOpenProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmOpenProject.Click
        LoadFile(SelectItem)
    End Sub

    Private Sub LoadToProject(ByVal sender As Object, ByVal e As EventArgs)
        If TypeOf sender Is ToolStripMenuItem Then
            Dim tsmi As ToolStripMenuItem = sender
            If TypeOf tsmi.Tag Is WorkControl Then
                TargetWorkControl = tsmi.Tag
                UseTarget = True
                LoadFile(SelectItem)
            End If
        End If
    End Sub
    Private UseTarget As Boolean = False
    Private TargetWorkControl As WorkControl = Nothing

    Private Sub LoadFile(ByVal vAddress As String)
        lbStatus.Text = "Loading..."
        Dim login As New Login With {.UserName = tbName.Text, .Password = mtbPassword.Text}
        Dim sip As String = cbIP.Text
        Dim ip As String = sip + ":8928"
        Dim address As String = vAddress
        Dim thr As New Threading.Thread(Sub()
                                            'If frmMain.smMain.ConnectTo(ip, "Synthenome Vexcutor", rService) Then
                                            '    Dim result As String = rService.Load(login, address)
                                            '    ThreadHeader(Of String, String, String).ControlInvoke(Me, AddressOf OnLoadFile, sip, address, result)
                                            'Else
                                            '    ThreadHeader(Of String).ControlInvoke(frmMain, AddressOf OnFail, "Load Failed")
                                            'End If
                                        End Sub)
        Me.Enabled = False
        thr.Start()
    End Sub
    Private Sub OnLoadFile(ByVal sIP As String, ByVal Address As String, ByVal file As String)
#If Remote = 1 Then
        MsgBox("File returned", MsgBoxStyle.OkOnly)
#End If

        If file Is Nothing Then
            lbStatus.Text = "Error in Permission."
            Enabled = True
        Else
            Dim bytes As Byte() = Convert.FromBase64String(file)
            If Address.ToLower.EndsWith(".vct") Then
                '质粒
                If TargetWorkControl Is Nothing Then
                    Dim dict As Dictionary(Of String, Object) = SettingEntry.LoadFromZXMLBytes(New List(Of String) From {"DNA", "Enzyme"}, bytes)
                    Dim gf As Nuctions.GeneFile = dict("DNA")
                    Dim REs As New List(Of String)
                    If Not dict("Enzyme") Is Nothing Then
                        REs = dict("Enzyme")
                    End If
                    SettingEntry.AddDNAViewTab(gf, REs, Address, tbName.Text, mtbPassword.Text)
                    'Dim tp As New CustomTabPage
                    'Dim sv As New SequenceViewer
                    'If frmMain.tcMainHost.TabPages.Count > 0 Then
                    '    frmMain.tcMainHost.TabPages.Insert(frmMain.tcMainHost.SelectedIndex + 1, tp)
                    'Else
                    '    frmMain.tcMainHost.TabPages.Add(tp)
                    'End If
                    'tp.Controls.Add(sv)
                    'tp.Text = gf.Name
                    'tp.SetCustomStyle(CustomTabPageStyle.Vector)
                    'sv.ParentTab = tp
                    'sv.Dock = DockStyle.Fill

                    'sv.FileAddress = Address
                    'sv.GeneFile = gf
                    'sv.RemoteUserName = tbName.Text
                    'sv.RemotePassword = mtbPassword.Text
                    'sv.ClearMode()
                Else
                    Dim dict As Dictionary(Of String, Object) = SettingEntry.LoadFromZXMLBytes(New List(Of String) From {"DNA", "Enzyme"}, bytes)
                    Dim gf As Nuctions.GeneFile = dict("DNA")
                    TargetWorkControl.LoadVectorFile(gf, Address)
                End If
            ElseIf Address.ToLower.EndsWith(".vxt") Then
                '工程文件
#If Remote = 1 Then
                MsgBox("Start read project", MsgBoxStyle.OkOnly)
#End If

                Dim wc As WorkControl = WorkControl.LoadFrom(bytes, Address, tbName.Text, mtbPassword.Text)
#If Remote = 1 Then
                MsgBox("Deserialized", MsgBoxStyle.OkOnly)
#End If
                Stop 'AddHandler
                'AddHandler wc.CloseWorkControl, AddressOf frmMain.OnTabClose
                'AddHandler wc.LoadWorkControl, AddressOf frmMain.OnLoadTab
                Dim TP As New CustomTabPage
                wc.ParentTab = TP
                TP.Text = ParseLevel2Name(Address)
                TP.Controls.Add(wc)
                TP.SetCustomStyle(CustomTabPageStyle.Project)
                wc.Dock = DockStyle.Fill
                wc.RemoteUserName = tbName.Text
                wc.RemotePassword = mtbPassword.Text

                Stop 'Add TabPage
                'If frmMain.tcMainHost.TabPages.Count > 0 Then
                '    frmMain.tcMainHost.TabPages.Insert(frmMain.tcMainHost.SelectedIndex + 1, TP)
                'Else
                '    frmMain.tcMainHost.TabPages.Add(TP)
                'End If
            End If
        End If
        UseTarget = False
        TargetWorkControl = Nothing
        Enabled = True
    End Sub
#End Region

#Region "搜索"
    Private Sub llSearch_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSearch.LinkClicked
        lbStatus.Text = "Searching..."
        Dim login As New Login With {.UserName = tbName.Text, .Password = mtbPassword.Text}
        Dim sIP As String = cbIP.Text
        Dim ip As String = cbIP.Text + ":8928"
        Dim si As New BaseService.SearchInfo With {.IncludeProject = cbProject.Checked, .IncludeVector = cbVector.Checked}
        If rbName.Checked Then
            si.SearchType = 0
        ElseIf rbSequence.Checked Then
            si.SearchType = 1
        ElseIf rbFeature.Checked Then
            si.SearchType = 2
        End If
        si.Query = rtbQuery.Text
        Dim query As String = BinarySerializer.ToString(si)
        Dim thr As New Threading.Thread(Sub()
                                            'Try
                                            '    If frmMain.smMain.ConnectTo(ip, "Synthenome Vexcutor", rService) Then
                                            '        Dim result As String = rService.Search(login, query)
                                            '        ThreadHeader(Of String, String).ControlInvoke(Me, AddressOf OnSearch, sIP, result)
                                            '    Else
                                            '        ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Search Failure")
                                            '    End If
                                            'Catch ex As Exception
                                            '    ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Search Failure")
                                            'End Try
                                        End Sub)

        Me.Enabled = False
        thr.Start()
    End Sub
    Private uprgx As New System.Text.RegularExpressions.Regex("\d+")
    Private Sub OnSearch(ByVal IP As String, ByVal Result As String)
        If Result Is Nothing Then
            lbStatus.Text = "Error in Search."
            Enabled = True
        Else
            Dim rNode As BaseService.Node
            Try
                rNode = BinarySerializer.FromString(Result)
            Catch ex As Exception
                lbStatus.Text = "Error in Search."
                Enabled = True
                Exit Sub
            End Try

            lvResults.Items.Clear()
            Dim lvi As ListViewItem
            For Each zNode As BaseService.Node In rNode.Nodes
                lvi = lvResults.Items.Add(zNode.Name, zNode.Type)
                'Name Score Location ID
                lvi.SubItems.Add(zNode.Score.ToString("0.000"))
                lvi.SubItems.Add(String.Format("tcp:\\{0}{1}", IP, zNode.Path))
                lvi.SubItems.Add(zNode.ID)
            Next
            lbStatus.Text = "Search Returned."
        End If
        Enabled = True
    End Sub
#End Region

#Region "读取属性"
    Private Sub ShareOptionsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareOptionsToolStripMenuItem1.Click
        ReadPermission()
    End Sub

    Private Sub ShareOptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareOptionsToolStripMenuItem.Click
        ReadPermission()
    End Sub
    Private Sub ReadPermission()
        If SelectItem Is Nothing OrElse SelectItem.Length = 0 Then Exit Sub
        lbStatus.Text = "Checking..."
        Dim login As New Login With {.UserName = tbName.Text, .Password = mtbPassword.Text}
        Dim sIP As String = cbIP.Text
        Dim ip As String = cbIP.Text + ":8928"

        Dim query As String = ParseLevel2Address(SelectItem)
        tslFilename.Text = query

        Dim thr As New Threading.Thread(Sub()
                                            'Try
                                            '    If frmMain.smMain.ConnectTo(ip, "Synthenome Vexcutor", rService) Then
                                            '        Dim users As String = rService.GetUserList(login, "")
                                            '        Dim result As String = rService.GetPermission(login, query)

                                            '        ThreadHeader(Of String, String, String).ControlInvoke(Me, AddressOf OnPermission, sIP, result, users)
                                            '    Else
                                            '        ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Check Failure")
                                            '    End If
                                            'Catch ex As Exception
                                            '    ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Check Failure")
                                            'End Try
                                        End Sub)

        Me.Enabled = False
        thr.Start()
    End Sub
    Private Sub OnPermission(ByVal sIP As String, ByVal Result As String, ByVal Users As String)
        If Result Is Nothing Or Users Is Nothing Then
            lbStatus.Text = "Error in Permission."
        ElseIf Not (Result Is Nothing) AndAlso Result = "<Denied>" Then
            lbStatus.Text = "Permission Denied."
        Else
            tsbUpdatePermission.Checked = False
            If Result = "-" Then
                lbStatus.Text = "Error in Permission."
                Enabled = True
                Exit Sub
            End If
            tsShare.Visible = True
            tsShare.Focus()
            Dim ids As String() = ParsePermissionToArray(Result)
            Dim vidl As New List(Of String)
            vidl.AddRange(ids)

            tsbEveryone.Checked = vidl.Contains("0")

            Dim usr As String() = Users.Split(New Char() {ControlChars.NewLine, ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf}, System.StringSplitOptions.RemoveEmptyEntries)
            Dim tsi As ToolStripButton

            tsbUsers.DropDownItems.Clear()
            For i As Integer = 0 To usr.Length - 1 Step 2
                If i + 1 < usr.Length Then
                    'id 
                    'name
                    tsi = New ToolStripButton()
                    tsbUsers.DropDownItems.Add(tsi)
                    tsi.Tag = usr(i)
                    tsi.AutoSize = True
                    tsi.Text = String.Format("ID({0}) : {1}", usr(i), usr(i + 1))
                    tsi.Width = tsi.Text.Length * 8
                    tsi.CheckOnClick = True
                    If vidl.Contains(usr(i)) Then tsi.Checked = True

                End If
            Next
            lbStatus.Text = "Permission Returned."
        End If
        Enabled = True
    End Sub

    Private SelectItem As String = ""
#End Region

#Region "属性相关函数"

    Public Shared Function ParseLevel2Address(ByVal vAddress As String) As String
        '"tcp:\\ip\user\type\name.vnt\sub.vnt"
        If vAddress.StartsWith("tcp:\\") Then
            Dim lvs As String() = vAddress.Split(New Char() {"\"}, System.StringSplitOptions.RemoveEmptyEntries)
            If lvs.Length = 6 Then
                Return vAddress.Substring(0, vAddress.LastIndexOf("\"))
            ElseIf lvs.Length = 5 Then
                Return vAddress
            End If
        Else
            Return ""
        End If
        Return ""
    End Function

    Private Function ParsePermissionToArray(ByVal vPermission As String) As String()

        Dim vList As New List(Of String)
        For Each m As System.Text.RegularExpressions.Match In uprgx.Matches(vPermission)
            vList.Add(m.Groups(0).Value)
        Next
        Return vList.ToArray
    End Function
    Private Function ParseArrayToPermission(ByVal vUsers As String()) As String
        Dim stb As New System.Text.StringBuilder
        For Each u As String In vUsers
            stb.Append("+")
            stb.Append(u)
            stb.Append(";")
        Next
        Return stb.ToString
    End Function
    Private Sub lvResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvResults.SelectedIndexChanged
        If lvResults.SelectedItems.Count > 0 And lvResults.Focused Then
            SelectItem = lvResults.SelectedItems(0).SubItems(2).Text
        End If
        tsShare.Visible = False
    End Sub

    Private Sub tvFolder_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvFolder.AfterSelect
        If Not (tvFolder.SelectedNode Is Nothing) And tvFolder.Focused Then
            If tvFolder.SelectedNode.ImageIndex > 0 And tvFolder.SelectedNode.Name.StartsWith("tcp:\\") Then
                SelectItem = tvFolder.SelectedNode.Name
            End If
        End If
        tsShare.Visible = False
    End Sub
#End Region

#Region "写入属性"
    Private Sub tsbUpdatePermission_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUpdatePermission.Click
        WritePermission()
    End Sub
    Private Sub WritePermission()
        If SelectItem Is Nothing OrElse SelectItem.Length = 0 Then Exit Sub
        lbStatus.Text = "Writing..."
        Dim login As New Login With {.UserName = tbName.Text, .Password = mtbPassword.Text}
        Dim sIP As String = cbIP.Text
        Dim ip As String = cbIP.Text + ":8928"
        Dim stb As New System.Text.StringBuilder
        If tsbEveryone.Checked Then
            stb.Append("+0;")
        End If
        For Each tsi As ToolStripButton In tsbUsers.DropDownItems
            If tsi.Checked Then
                Dim i As String = tsi.Tag
                Try
                    Dim z As Integer = CInt(i)
                    stb.Append("+")
                    stb.Append(i)
                    stb.Append(";")
                Catch ex As Exception

                End Try
            End If
        Next

        Dim permission As String = stb.ToString
        Dim query As String = ParseLevel2Address(SelectItem)
        tslFilename.Text = query

        Dim thr As New Threading.Thread(Sub()
                                            'Try
                                            '    If frmMain.smMain.ConnectTo(ip, "Synthenome Vexcutor", rService) Then
                                            '        Dim users As String = rService.GetUserList(login, "")
                                            '        Dim result As Boolean = rService.SetPermission(login, query, permission)
                                            '        If result Then
                                            '            Dim rlist As String = rService.List(login, "")
                                            '            ThreadHeader(Of String, String).ControlInvoke(Me, AddressOf OnList, sIP, rlist)
                                            '        Else
                                            '            ThreadHeader(Of Boolean).ControlInvoke(Me, AddressOf OnWritePermission, result)
                                            '        End If
                                            '    Else
                                            '        ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Permisstion Failure")
                                            '    End If
                                            'Catch ex As Exception
                                            '    ThreadHeader(Of String).ControlInvoke(Me, AddressOf OnFail, "Permisstion Failure")
                                            'End Try
                                        End Sub)

        Me.Enabled = False
        thr.Start()
    End Sub
    Private Sub OnWritePermission(ByVal result As Boolean)
        If result Then
            tsbUpdatePermission.Checked = True
            lbStatus.Text = "Permisstion Updated."
        Else
            lbStatus.Text = "Permisstion Failure."
        End If
        Enabled = True
    End Sub
#End Region


End Class
