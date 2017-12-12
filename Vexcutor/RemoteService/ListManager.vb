Imports System.Windows.Forms
Public Class ListManager

    Private vType As Type
    Private oType As Type '上一次使用的类型 如果相同就不需要重新刷新整个视图了

    Private vDataGridMethods As New Dictionary(Of Integer, ColumnDefineAttribute)
    Private vDataFields As New Dictionary(Of String, System.Reflection.PropertyInfo)
    Private vData As Object
    Private vKeyMethod As ColumnDefineAttribute
    'Private vKeyField As System.Reflection.PropertyInfo
    Private vModifyBox As Boolean = True
    Private vIsIntergerKey As Boolean '从readonly字段获取
    Private vAccessLevel As DataAccessLevel

    Public Property MyValueMonitor As ValueMonitorBase
    Private rsakey As String = "<RSAKeyValue><Modulus>qW7kxBP6F8hPqPWi+Q88zZRiPlgcRnHnKQ458bFsmh+WEJ1mUFn6GQGuhUFx1PyzBtcoDURKoV/b2e9LScMjqQE94A2YV0gnD7wHa0XzKV35yBlxpbankZDvpM0/jf5K0UdjUoVLwL6P+4gUOHlZ0Z0d18Z1wf8ls1L1fkN19Uc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

    Sub New()
        Dim di As New System.IO.DirectoryInfo(Application.StartupPath)
        Dim hs As List(Of String) = Equipment.GenerateMachineHash
        Dim ky As List(Of String) = Nothing

        For Each fi As System.IO.FileInfo In di.GetFiles("*.epm")
            Try
                Dim bytes As Byte() = System.IO.File.ReadAllBytes(fi.FullName)
                ky = Equipment.CheckKey(bytes)
            Catch ex As Exception

            End Try
        Next
        If ky Is Nothing Then Application.Exit()
        Dim RSA As New RSACrypt(rsakey)
        For i As Integer = 0 To ky.Count - 1
            If Not RSA.VerifyData(ky(i), hs(i)) Then Application.Exit()
        Next
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    <System.ComponentModel.Category("行为"), System.ComponentModel.DefaultValue(True)> Public Property ModifyBox As Boolean
        Set(ByVal value As Boolean)
            vModifyBox = value
            If vModifyBox Then
                dgvView.Height = Height - 16
                llAdd.Visible = True
                llDelete.Visible = True
                llSave.Visible = True
            Else
                llAdd.Visible = False
                llDelete.Visible = False
                llSave.Visible = False
                dgvView.Height = Height
            End If
        End Set
        Get
            Return vModifyBox
        End Get
    End Property

    Public Sub GenerateValueMonitor(Of T)()
        MyValueMonitor = New MCDS.ValueMonitor(Of T)(vData)
    End Sub
    Public Sub LoadData(Of T)(ByVal SourceList As IDictionary(Of Integer, T))
        vData = SourceList
        GenerateValueMonitor(Of T)()
        Dim x As Type = SourceList.GetType
        vType = x.GetGenericArguments()(1)
        vData = SourceList
        LoadCollection(SourceList.Values)
    End Sub
    Public Sub LoadData(Of T)(ByVal SourceList As IList(Of T))
        vData = SourceList
        Dim x As Type = SourceList.GetType
        vType = x.GetGenericArguments()(0)
        vData = SourceList
        LoadCollection(SourceList)
    End Sub
    'Public Sub LoadData(ByVal SourceList As IDictionary)
    '    vData = SourceList
    '    Dim t As Type = SourceList.GetType
    '    vType = t.GetGenericArguments()(1)
    '    vData = SourceList
    '    LoadCollection(SourceList.Values)
    'End Sub
    'Public Sub LoadData(ByVal SourceList As IList)
    '    vData = SourceList
    '    Dim t As Type = SourceList.GetType
    '    vType = t.GetGenericArguments()(0)
    '    vData = SourceList
    '    LoadCollection(SourceList)
    'End Sub

    Public Sub LoadCollection(ByVal SourceList As ICollection)
        'Try
        '重置系统信息
        dgvView.Rows.Clear()


        If oType Is Nothing OrElse Not (vType Is oType) Then
            oType = vType
            vDataGridMethods.Clear()
            vDataFields.Clear()
            dgvView.Columns.Clear()
            '分析内部类型

            '分析类型中的子类型 查看其中是否有DataGrid类型
            For Each gt As Type In vType.GetNestedTypes()
                If gt.Name = "DataGrid" Then
                    '分析gt的shared方法
                    For Each mi As System.Reflection.MethodInfo In gt.GetMethods()
                        If mi.DeclaringType Is gt Then
                            If mi.Name = "Key" Then
                                For Each att As Attribute In mi.GetCustomAttributes(True)
                                    If TypeOf att Is ColumnDefineAttribute Then
                                        Dim cda As ColumnDefineAttribute = att
                                        cda.Method = mi
                                        cda.Parameters = mi.GetParameters()
                                        vKeyMethod = cda
                                        vIsIntergerKey = cda.DataReadOnly
                                        vAccessLevel = cda.Width
                                        '如果宽度定义为-1 说明这个系统不接受添加或者删除
                                        '如果宽度定义为-2 说明这个系统完全只读
                                        Select Case vAccessLevel
                                            Case DataAccessLevel.ReadWriteNoAddNoDelete
                                                llAdd.Visible = False
                                                llDelete.Visible = False
                                                llSave.Visible = True And Not vReadOnly
                                            Case DataAccessLevel.ReadOnly
                                                llAdd.Visible = False
                                                llDelete.Visible = False
                                                llSave.Visible = False
                                            Case DataAccessLevel.ReadWriteAddDelete
                                                llAdd.Visible = True And Not vReadOnly
                                                llDelete.Visible = True And Not vReadOnly
                                                llSave.Visible = True And Not vReadOnly
                                        End Select
                                    End If
                                Next
                            Else
                                For Each att As Attribute In mi.GetCustomAttributes(True)
                                    If TypeOf att Is ColumnDefineAttribute Then
                                        Dim cda As ColumnDefineAttribute = att
                                        cda.Method = mi
                                        cda.Index = CInt(mi.Name.Substring(1))
                                        cda.Parameters = mi.GetParameters()
                                        vDataGridMethods.Add(cda.Index, cda)
                                    End If
                                Next
                            End If

                        End If
                    Next
                End If
            Next

            '获取处理方法


            '获取映射关系
            For Each fld As System.Reflection.PropertyInfo In vType.GetProperties
                vDataFields.Add(fld.Name, fld)
            Next


            '排列顺序 生成列
            Dim cList As New List(Of ColumnDefineAttribute)
            cList.AddRange(vDataGridMethods.Values)
            cList.Sort()
            Dim clmn As DataGridViewColumn

            For Each cda As ColumnDefineAttribute In cList
                Select Case cda.ColumnDataType
                    Case ColumnTypeEnum.Text
                        clmn = New DataGridViewTextBoxColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewTextBoxColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                    Case ColumnTypeEnum.Check
                        clmn = New DataGridViewCheckBoxColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewTextBoxColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                    Case ColumnTypeEnum.Button
                        clmn = New DataGridViewButtonColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewButtonColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                    Case ColumnTypeEnum.Image
                        clmn = New DataGridViewImageColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewImageColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                    Case ColumnTypeEnum.Combo
                        clmn = New DataGridViewComboBoxColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewComboBoxColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                    Case ColumnTypeEnum.Link
                        clmn = New DataGridViewLinkColumn()
                        Dim id As Integer = dgvView.Columns.Add(New DataGridViewLinkColumn())
                        dgvView.Columns(id).HeaderText = cda.HeaderName
                        dgvView.Columns(id).ReadOnly = cda.DataReadOnly
                        dgvView.Columns(id).Width = cda.Width
                End Select
            Next
        End If
        '加载数据

        ViewUpdating = True
        Dim row As DataGridViewRow
        For Each obj As Object In SourceList
            row = dgvView.Rows(dgvView.Rows.Add())
            SetDataToRow(obj, row)
        Next
        dgvView.PerformLayout()
        ViewUpdating = False
        'Catch ex As Exception

        'End Try
        RaiseEvent ValueChanged(Me, New EventArgs)
        'Select Data Row 1
        If dgvView.Rows.Count > 0 Then
            '没有办法 但是只能无病呻吟一下 因为创建行的时候已经加载选中该行
            If dgvView.Rows(0).Selected Then dgvView.Rows(0).Selected = False
            dgvView.Rows(0).Selected = True
        End If
    End Sub

    Private vSettingData As Boolean = False

    '用来给通知数据值发生变化
    Public Event ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    Public ReadOnly Property ValueDict() As Dictionary(Of String, Object)
        Get
            Dim vList As New Dictionary(Of String, Object)
            Dim i As Integer
            Dim key As String
            If TypeOf vData Is IList Then
                For Each obj As Object In vData
                    key = obj.ToString
                    i = 0
                    If vList.ContainsKey(key) Then
                        While vList.ContainsKey(key + i.ToString)
                            i += 1
                        End While
                        key = key + i.ToString
                    End If
                    vList.Add(obj.ToString, obj)
                Next
            ElseIf TypeOf vData Is IDictionary Then
                For Each key In vData.Keys
                    vList.Add(key, vData(key))
                Next
            End If
            Return vList
        End Get
    End Property
    Public Sub SetDataToRow(ByVal obj As Object, ByVal row As DataGridViewRow)
        vSettingData = True
        Dim fldList As New List(Of Object)
        For i As Integer = 0 To dgvView.Columns.Count - 1
            fldList.Clear()
            For Each pi As System.Reflection.ParameterInfo In vDataGridMethods(i).Parameters
                fldList.Add(vDataFields(pi.Name).GetValue(obj, New Object() {}))
            Next
            row.Cells(i).Value = vDataGridMethods(i).Method.Invoke(Nothing, fldList.ToArray)
            row.Tag = obj
        Next
        If vSearchMode Then System.ComponentModel.TypeDescriptor.AddProvider(New QueryDescriptionProvider, obj)
        vSettingData = False
    End Sub
    Public Sub UpdateSelectedDataRow()
        If dgvView.SelectedRows.Count > 0 Then
            If TypeOf vData Is IList Then
                SetDataToRow(dgvView.SelectedRows(0).Tag, dgvView.SelectedRows(0))
                CheckRemoteRowModify(dgvView.SelectedRows(0).Tag, dgvView.SelectedRows(0))
                RaiseEvent ValueChanged(Me, New EventArgs)

            ElseIf TypeOf vData Is IDictionary Then
                '验证key是否改变
                Dim obj As Object = dgvView.SelectedRows(0).Tag

                '读取key字段
                If vIsIntergerKey Then
                    '确定oldKey和newKey是否相同
                    Dim oldKey As Integer
                    For Each oldKey In vData.Keys
                        If vData(oldKey) Is obj Then Exit For
                    Next
                    Dim newKey As Integer = vDataFields(vKeyMethod.Parameters(0).Name).GetValue(obj, New Object() {})

                    If Not (oldKey = newKey) Then
                        vData.Remove(oldKey)
                        If vData.ContainsKey(newKey) Then
                            Dim xObj As Object = vData(newKey)
                            If xObj Is obj Then
                                '如果是同一个东西 那么说明key没有变化

                            Else
                                '如果是不同的东西 说明key有冲突 重新获得一个自由的key
                                Dim i As Integer = 0
                                While vData.ContainsKey(i)
                                    i += 1
                                End While
                                newKey += i
                                vDataFields(vKeyMethod.Parameters(0).Name).SetValue(obj, newKey, New Object() {})
                                vData.Add(newKey, obj)
                            End If
                        Else
                            vData.Add(newKey, obj)
                        End If
                    End If
                Else
                    Dim key As String = vKeyMethod.HeaderName
                    Dim oldKey As String = Nothing
                    For Each oldKey In vData.Keys
                        If vData(oldKey) Is obj Then Exit For
                    Next
                    Dim newKey As Integer = vDataFields(vKeyMethod.Parameters(0).Name).GetValue(obj, New Object() {})
                    If oldKey Is Nothing Then
                        '不需要删除原来的
                        vData.Add(newKey, obj)
                    Else
                        If Not (oldKey = newKey) Then
                            vData.Remove(oldKey)
                            If vData.ContainsKey(newKey) Then
                                Dim xObj As Object = vData(newKey)
                                If xObj Is obj Then
                                    '如果是同一个东西 那么说明key没有变化

                                Else
                                    '如果是不同的东西 说明key有冲突 重新获得一个自由的key
                                    Dim i As Integer = 0
                                    While vData.ContainsKey(key + i.ToString)
                                        i += 1
                                    End While
                                    newKey += i
                                    vDataFields(vKeyMethod.Parameters(0).Name).SetValue(obj, newKey, New Object() {})
                                    vData.Add(newKey, obj)
                                End If
                            Else
                                vData.Add(newKey, obj)
                            End If
                        End If
                    End If

                End If

                SetDataToRow(dgvView.SelectedRows(0).Tag, dgvView.SelectedRows(0))
                CheckRemoteRowModify(dgvView.SelectedRows(0).Tag, dgvView.SelectedRows(0))
                RaiseEvent ValueChanged(Me, New EventArgs)
            End If
        End If
    End Sub

    Public ReadOnly Property SelectedData() As Object
        Get
            If dgvView.SelectedRows.Count > 0 Then
                Return dgvView.SelectedRows(0).Tag
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Event SelectDataChanged(ByVal sender As Object, ByVal e As EventArgs)

    Public Event SelectData(ByVal sender As Object, ByVal e As DataEventArgs)
    Private Sub dgvView_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvView.CellDoubleClick
        If e.RowIndex > -1 And e.RowIndex < dgvView.Rows.Count Then
            If Not (dgvView.Rows(e.RowIndex).Tag Is Nothing) Then
                RaiseEvent SelectData(Me, New DataEventArgs(dgvView.Rows(e.RowIndex).Tag))
            End If
        End If
    End Sub


    Private Sub dgvView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvView.CellValueChanged
        If vSettingData Then Exit Sub
        If e.RowIndex > -1 Then
            Dim clmn As DataGridViewColumn = dgvView.Columns(e.ColumnIndex)
            If Not clmn.ReadOnly Then
                Dim value As Object = Nothing

                If TypeOf clmn Is DataGridViewComboBoxColumn Then
                    Dim combo As DataGridViewComboBoxColumn = clmn
                    For Each obj As Object In vComboItems(e.ColumnIndex)
                        If obj.ToString = dgvView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Then
                            value = obj
                            Exit For
                        End If
                    Next
                Else
                    value = dgvView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                End If
                vDataFields(vDataGridMethods(e.ColumnIndex).Parameters(0).Name).SetValue(vData(e.RowIndex), value, New Object() {})
            End If
            RaiseEvent ValueChanged(Me, New EventArgs)
        End If
    End Sub

    '防止在集中处理过程中发出选择更新事件
    Private ViewUpdating As Boolean = False

    Private lastSelectRow As DataGridViewRow

    Private Sub dgvView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvView.Click
        MyBase.OnClick(e)
    End Sub

    Private Sub dgvView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvView.MouseWheel

        If e.Delta > 0 AndAlso dgvView.FirstDisplayedScrollingRowIndex < dgvView.RowCount - 1 Then
            dgvView.FirstDisplayedScrollingRowIndex += 1
        ElseIf e.Delta < 0 AndAlso dgvView.FirstDisplayedScrollingRowIndex > 0 Then
            dgvView.FirstDisplayedScrollingRowIndex -= 1
        End If

    End Sub

    Private Sub dgvView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvView.SelectionChanged
        If ViewUpdating Then Exit Sub
        RaiseEvent SelectDataChanged(Me, e)
        'check the value change here 
        CheckLastSelectedRow()
        If dgvView.SelectedRows.Count > 0 Then
            lastSelectRow = dgvView.SelectedRows(0)
        Else
            lastSelectRow = Nothing
        End If
    End Sub

    Private Sub CheckLastSelectedRow()
        If Not (lastSelectRow Is Nothing) AndAlso Not (MyValueMonitor Is Nothing) Then
            Dim Obj As Object = lastSelectRow.Tag
            If Not MyValueMonitor.CheckValue(Obj) Then
                Dim eb As EditBase = Obj
                eb.Status = EditStatus.Modified
                If Not ModifiedData.Contains(Obj) Then
                    ModifiedData.Add(Obj)
                End If
                If eb.Status = EditStatus.Modified Then SetRowColor(lastSelectRow, Drawing.Color.LightGreen)
            End If
        End If
    End Sub

    Public Sub UpdateComboItems()
        For Each i As Integer In vComboItems.Keys
            If i > -1 And i < dgvView.Columns.Count Then
                If TypeOf dgvView.Columns(i) Is DataGridViewComboBoxColumn Then
                    Dim combo As DataGridViewComboBoxColumn = dgvView.Columns(i)
                    For Each obj As Object In vComboItems(i)
                        combo.Items.Add(obj.ToString)
                    Next
                End If
            End If
        Next
    End Sub

    Private vComboItems As New Dictionary(Of Integer, IList)

    Public ReadOnly Property ComboItems() As Dictionary(Of Integer, IList)
        Get
            Return vComboItems
        End Get
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("外观")> Public Property RowHeight() As Integer
        Get
            Return dgvView.RowTemplate.Height
        End Get
        Set(ByVal value As Integer)
            dgvView.RowTemplate.Height = value
        End Set
    End Property

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llAdd.LinkClicked
        If TypeOf vData Is IList Then
            Dim ci As System.Reflection.ConstructorInfo
            ci = vType.GetConstructor(System.Type.EmptyTypes)
            Dim xObj As Object = ci.Invoke(New Object() {})
            vData.Add(xObj)
            ViewUpdating = True
            Dim row As DataGridViewRow = dgvView.Rows(dgvView.Rows.Add())
            dgvView.PerformLayout()
            ViewUpdating = False
            SetDataToRow(xObj, row)
            If TypeOf xObj Is EditBase Then
                '标记为远程添加
                Dim eb As EditBase = xObj
                eb.Status = EditStatus.Created
                ModifiedData.Add(eb)
            End If
            If dgvView.SelectedRows.Count = 1 Then
                RaiseEvent SelectDataChanged(Me, New EventArgs)
            End If
            RaiseEvent ValueChanged(Me, New EventArgs)
        ElseIf TypeOf vData Is IDictionary Then
            Dim ci As System.Reflection.ConstructorInfo
            ci = vType.GetConstructor(System.Type.EmptyTypes)
            Dim xObj As Object = ci.Invoke(New Object() {})
            Dim key As String = vKeyMethod.HeaderName
            Dim i As Integer = 0
            If vIsIntergerKey Then
                While vData.ContainsKey(i)
                    i += 1
                End While
                vDataFields(vKeyMethod.Parameters(0).Name).SetValue(xObj, i, New Object() {})
                vData.Add(i, xObj)
            Else
                While vData.ContainsKey(key + i.ToString)
                    i += 1
                End While
                vDataFields(vKeyMethod.Parameters(0).Name).SetValue(xObj, key + i.ToString, New Object() {})
                vData.Add(key + i.ToString, xObj)
            End If
            ViewUpdating = True
            Dim row As DataGridViewRow = dgvView.Rows(dgvView.Rows.Add())
            dgvView.PerformLayout()
            ViewUpdating = False
            SetDataToRow(xObj, row)
            CheckRemoteRowAdd(xObj, row)
            If dgvView.SelectedRows.Count = 1 Then
                RaiseEvent SelectDataChanged(Me, New EventArgs)
            End If
            RaiseEvent ValueChanged(Me, New EventArgs)
        End If
    End Sub

    Private Sub CheckRemoteRowAdd(ByVal xObj As Object, ByVal row As DataGridViewRow)
        If TypeOf xObj Is EditBase Then
            '标记为远程添加
            Dim eb As EditBase = xObj
            eb.Status = EditStatus.Created
            ModifiedData.Add(eb)
            '用黄色标记数据尚未更新到服务器
            SetRowColor(row, Drawing.Color.LemonChiffon)
        End If

    End Sub
    Private Sub CheckRemoteRowModify(ByVal xObj As Object, ByVal row As DataGridViewRow)
        If TypeOf xObj Is EditBase Then
            Dim eb As EditBase = xObj
            '如果是新建的不要标记成为Modified。
            eb.Status = IIf(eb.Status = EditStatus.Created, EditStatus.Created, EditStatus.Modified)
            If Not ModifiedData.Contains(xObj) Then
                ModifiedData.Add(xObj)
            End If
            If eb.Status = EditStatus.Modified Then SetRowColor(row, Drawing.Color.LightGreen)
        End If
    End Sub
    Private Sub SetRowColor(ByVal row As DataGridViewRow, ByVal c As System.Drawing.Color)
        For Each cell As DataGridViewCell In row.Cells
            cell.Style.BackColor = c
        Next
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDelete.LinkClicked
        RemoveSelectedRows()
    End Sub

    Public Sub RemoveSelectedRows()
        If dgvView.SelectedRows.Count > 0 Then
            Dim delList As New List(Of DataGridViewRow)

            For Each row As DataGridViewRow In dgvView.SelectedRows
                delList.Add(row)
            Next

            ViewUpdating = True
            For Each row As DataGridViewRow In delList
                dgvView.Rows.Remove(row)
            Next
            dgvView.PerformLayout()
            ViewUpdating = False

            '确定哪些是需要远程删除的
            For Each row As DataGridViewRow In delList
                If TypeOf row.Tag Is EditBase Then
                    Dim eb As EditBase = row.Tag
                    eb.Status = EditStatus.Deleted
                    ModifiedData.Add(eb)
                End If
            Next

            If TypeOf vData Is IList Then
                For Each row As DataGridViewRow In delList
                    vData.Remove(row.Tag)
                Next
            ElseIf TypeOf vData Is IDictionary Then
                For Each row As DataGridViewRow In delList
                    If vIsIntergerKey Then
                        Dim Key As Integer = vDataFields(vKeyMethod.Parameters(0).Name).GetValue(row.Tag, New Object() {})
                        vData.Remove(Key)
                    Else
                        vData.Remove(row.Tag.ToString)
                    End If
                Next
            End If

            RaiseEvent ValueChanged(Me, New EventArgs)
        End If
    End Sub

    Public ModifiedData As New List(Of EditBase)
    Public SearchCacheData As New List(Of EditBase)

    Private Sub llSave_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSave.LinkClicked
        If vSearchMode Then
            '"Back"
            SearchMode = False
            LoadData(vBackupData)
            MyValueMonitor = vBackupMonitor
            ModifiedData.Clear()
            ModifiedData.AddRange(SearchCacheData)
            SearchCacheData.Clear()
        Else
            '"Save"
            CheckLastSelectedRow()
            Dim RDUE As New RemoteDataEventArgs

            RDUE.RemoteData = ModifiedData
            RaiseEvent RemoteDataUpdate(Me, RDUE)
            '在远程数据更新之前 本控件不可编辑
            'Me.Enabled = False
        End If
    End Sub
    Public Event GetDataInstance(ByVal sender As Object, ByVal e As RemoteDataEventArgs)
    Public Event SearchRemoteData(ByVal sender As Object, ByVal e As RemoteDataEventArgs)
    Private vBackupData As Object
    Private vBackupMonitor As ValueMonitorBase
    Private vSearchMode As Boolean = False
    Private vReadOnly As Boolean = False
    Public Property [ReadOnly] As Boolean
        Get
            Return vReadOnly
        End Get
        Set(ByVal value As Boolean)
            vReadOnly = value
            If vReadOnly Then
                llAdd.Visible = False
                llDelete.Visible = False
                dgvView.ReadOnly = True
                llSave.Visible = False
            End If
        End Set
    End Property

    Public Property AllowRemoteSearch As Boolean
        Set(ByVal value As Boolean)
            llSearch.Visible = value
        End Set
        Get
            Return llSearch.Visible
        End Get
    End Property
    Private vSaveVisible As Boolean
    Public Property SearchMode As Boolean
        Set(ByVal value As Boolean)
            vSearchMode = value
            llSearch.LinkColor = IIf(vSearchMode, System.Drawing.Color.DeepPink, System.Drawing.Color.DarkViolet)
            If vSearchMode Then
                vSaveVisible = llSave.Visible
                llSave.Visible = True
                llSave.Text = "Back"
            Else
                llSave.Visible = vSaveVisible
                llSave.Text = "Save"
            End If
        End Set
        Get
            Return vSearchMode
        End Get
    End Property
    Private Sub llSearch_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llSearch.LinkClicked
        If vSearchMode Then
            'Set the color until return
            SearchMode = False
            CheckLastSelectedRow()
            Dim RDUE As New RemoteDataEventArgs
            RDUE.RemoteData = ModifiedData
            RaiseEvent SearchRemoteData(Me, RDUE)
        Else
            'In Search mode
            SearchMode = True
            vBackupData = vData
            vBackupMonitor = MyValueMonitor
            SearchCacheData.Clear()
            SearchCacheData.AddRange(ModifiedData)
            ModifiedData.Clear()
            llSearch.LinkColor = Drawing.Color.DeepPink
            CheckLastSelectedRow()
            Dim RDUE As New RemoteDataEventArgs
            RaiseEvent GetDataInstance(Me, RDUE)
        End If
    End Sub

    Public Event RemoteDataUpdate(ByVal sender As Object, ByVal e As RemoteDataEventArgs)
    Private Sub llRefresh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llRefresh.LinkClicked
        If vSearchMode Then SearchMode = False
        RaiseEvent RemoteDataRefresh(Me, New EventArgs)
    End Sub

    Public Event RemoteDataRefresh(ByVal sender As Object, ByVal e As EventArgs)
    Public Event Close(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub llClose_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClose.LinkClicked
        If vSearchMode Then SearchMode = False
        RaiseEvent Close(Me, New EventArgs)
    End Sub

    Private Sub tbFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFilter.Click
        tbFilter.SelectAll()
    End Sub
    Private BlockNextChar As Boolean = False
    Private Sub tbFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbFilter.KeyDown
        If e.KeyCode = Keys.Enter Then
            SearchData(tbFilter.Text)
        ElseIf e.KeyCode = Windows.Forms.Keys.Back AndAlso e.Modifiers = System.Windows.Forms.Keys.Control Then
            Dim tb As System.Windows.Forms.TextBox = sender
            e.Handled = True
            BlockNextChar = True
            With tb
                Dim sp As Integer = .Text.LastIndexOf(" ", .SelectionStart)
                If sp >= 0 Then
                    .Text = .Text.Substring(0, sp) + .Text.Substring(.SelectionStart)
                    .SelectionStart = sp
                Else
                    .Text = ""
                End If
            End With
        End If
    End Sub

    Private Sub tbFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbFilter.KeyPress
        If BlockNextChar Then e.Handled = True : BlockNextChar = False
    End Sub

    Private Sub SearchData(ByVal Keywords As String)
        Dim keys As New List(Of String)
        keys.AddRange(Keywords.Split(New String() {" "}, System.StringSplitOptions.RemoveEmptyEntries))

        For Each dc As DataGridViewColumn In dgvView.Columns
            dc.SortMode = DataGridViewColumnSortMode.Programmatic
            dc.HeaderCell.SortGlyphDirection = SortOrder.Descending
        Next
        dgvView.Sort(New WordComparer(keys, dgvView.Columns, dgvView.Rows))
        For Each dc As DataGridViewColumn In dgvView.Columns
            dc.SortMode = DataGridViewColumnSortMode.Automatic
            dc.HeaderCell.SortGlyphDirection = SortOrder.None
        Next

        If dgvView.Rows.Count > 0 Then
            dgvView.ClearSelection()
            dgvView.Rows(0).Selected = True
            'dgvView.
            dgvView.FirstDisplayedScrollingRowIndex = 0
        End If
    End Sub

    Public Class WordComparer
        Implements IComparer

        Private vKeys As New List(Of String)
        Private vC As Integer = -1
        Private vColumns As New List(Of Integer)
        Private vRowScores As New Dictionary(Of DataGridViewRow, Integer)
        Public Sub New(ByVal keywords As List(Of String), ByVal columns As DataGridViewColumnCollection, ByVal rows As DataGridViewRowCollection)
            vKeys = keywords
            Dim FindMin As Boolean = False
            For i As Integer = 0 To columns.Count - 1
                If TypeOf columns(i) Is DataGridViewTextBoxColumn Then
                    '找到最小的文本编号行
                    If Not FindMin Then vC = i : FindMin = True
                    '找出所有文本列
                    vColumns.Add(i)
                End If
            Next
            Dim V As String
            For Each row As DataGridViewRow In rows
                Dim score As Integer = 0
                For Each c As Integer In vColumns
                    V = row.Cells(c).Value

                    Dim i As Integer
                    If Not (V Is Nothing) AndAlso V.Length > 0 Then
                        For Each k As String In vKeys
                            i = -1

                            Do
                                i = V.IndexOf(k, i + 1, System.StringComparison.CurrentCultureIgnoreCase)
                                If i > -1 Then score += 1
                            Loop While i > -1

                        Next
                    End If
                Next
                vRowScores.Add(row, score)
            Next
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim R1 As DataGridViewRow = x
            Dim R2 As DataGridViewRow = y
            Dim V1 As String
            Dim V2 As String
            If vKeys.Count = 0 Then
                If vC > -1 Then
                    V1 = R1.Cells(vC).Value
                    V2 = R2.Cells(vC).Value
                    If Not (V1 Is Nothing) And Not (V2 Is Nothing) Then
                        Return V1.CompareTo(V2)
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If
            Else
                Return Math.Sign(vRowScores(R2) - vRowScores(R1))
            End If
        End Function
    End Class



End Class
Public Class RemoteDataEventArgs
    Inherits EventArgs
    Public RemoteData As List(Of EditBase)
End Class

Public Class ColumnDefineAttribute
    Inherits Attribute
    Implements IComparable(Of ColumnDefineAttribute)
    '标签名称
    Public HeaderName As String
    '列的类型
    Public ColumnDataType As ColumnTypeEnum
    '数据是否只读
    Public DataReadOnly As Boolean

    Public Method As System.Reflection.MethodInfo

    Public Parameters As System.Reflection.ParameterInfo()

    Public Index As Integer

    Public Width As Integer


    Public Sub New(ByVal vHeaderName As String, ByVal vColumnDataType As ColumnTypeEnum, ByVal vWidth As Integer, ByVal vDataReadOnly As Boolean)
        HeaderName = vHeaderName
        DataReadOnly = vDataReadOnly
        ColumnDataType = vColumnDataType
        Width = vWidth
    End Sub

    Public Function CompareTo(ByVal other As ColumnDefineAttribute) As Integer Implements System.IComparable(Of ColumnDefineAttribute).CompareTo
        Return Math.Sign(Index - other.Index)
    End Function
End Class

Public Enum ColumnTypeEnum As Integer
    Text
    Image
    Check
    Combo
    Button
    Link
End Enum

Public Class Car
    Public Name As String
    Public Model As String
    Public IsTruck As Boolean
    Class DataGrid
        <ColumnDefine("Name", ColumnTypeEnum.Text, 60, True)> Public Shared Function _0(ByVal Name As String) As String
            Return Name
        End Function
        <ColumnDefine("Model", ColumnTypeEnum.Text, 200, True)> Public Shared Function _1(ByVal Model As String) As String
            Return Model
        End Function
        <ColumnDefine("Type", ColumnTypeEnum.Text, 120, True)> Public Shared Function _2(ByVal IsTruck As Boolean) As String
            Return IIf(IsTruck, "Truck", "Car")
        End Function
    End Class
End Class

Public MustInherit Class ValueMonitorBase
    Public MustOverride Function CheckValue(ByVal Value As Object) As Boolean
End Class
