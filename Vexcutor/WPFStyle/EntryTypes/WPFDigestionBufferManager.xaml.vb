Imports System.Windows

Public Class WPFDigestionBufferManager

    Private _DigestionBufferData As DigestionBufferData
    Private Sub WPFDigestionBufferManager_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        dgTable.ItemsSource = Items
        acpEnzymeList.Values = SettingEntry.EnzymeCol.Keys.ToArray
        If IO.File.Exists(SettingEntry.DigestionBufferFilePath) Then
            Try
                Dim obj = System.Xaml.XamlServices.Parse(IO.File.ReadAllText(SettingEntry.DigestionBufferFilePath))
                If TypeOf obj Is DigestionBufferData Then _DigestionBufferData = obj
            Catch ex As Exception
                _DigestionBufferData = New DigestionBufferData
            End Try
        Else
            _DigestionBufferData = New DigestionBufferData
        End If
        If _DigestionBufferData Is Nothing Then _DigestionBufferData = New DigestionBufferData
        LoadDigesteionBufferData()
    End Sub

    Private Sub LoadDigesteionBufferData()
        cbSuppliers.ItemsSource = _DigestionBufferData.DigestionBufferSystems
        If _DigestionBufferData.DigestionBufferSystems.Count > 0 Then
            Dim _Supplier As DigestionBufferSystem = _DigestionBufferData.DigestionBufferSystems(0)
            cbSuppliers.SelectedItem = _Supplier
            BuildBufferTable()
        End If
    End Sub
    Private Sub DeleteSupplier(sender As Object, e As RoutedEventArgs)
        Dim _Supplier As DigestionBufferSystem = cbSuppliers.SelectedItem
        If _Supplier Is Nothing Then Return
        If _DigestionBufferData.DigestionBufferSystems.Contains(_Supplier) Then _DigestionBufferData.DigestionBufferSystems.Remove(_Supplier)
    End Sub
    Private Sub SaveSuppliers(sender As Object, e As RoutedEventArgs)
        Dim _Supplier As DigestionBufferSystem = cbSuppliers.SelectedItem
        If _Supplier IsNot Nothing Then
            _Supplier.DigestionBufferInfos.Clear()
            'need to load all the items from the 
            For Each item In Items
                For Each key In item.GetDynamicMemberNames
                    Dim obj = item.Member(key)
                    If TypeOf obj Is DigestionBufferInfo Then
                        Dim info As DigestionBufferInfo = obj
                        _Supplier.DigestionBufferInfos.Add(info)
                    End If

                Next
            Next
        End If
        Dim value = System.Xaml.XamlServices.Save(_DigestionBufferData)
        IO.File.WriteAllText(SettingEntry.DigestionBufferFilePath, value)
        SettingEntry.LoadDigestionBufferData()
    End Sub
    Private Sub AddNewSupplier(sender As Object, e As Windows.RoutedEventArgs)
        Dim _Supplier As DigestionBufferSystem = New DigestionBufferSystem With {.SupplierName = tbNewSupplier.Text}
        _DigestionBufferData.DigestionBufferSystems.Add(_Supplier)
        cbSuppliers.SelectedItem = _Supplier
        tbNewSupplier.Clear()
    End Sub
    Private Sub SupplierSelected(sender As Object, e As Controls.SelectionChangedEventArgs)
        BuildBufferTable()
    End Sub

    'Private Headers As New Dictionary(Of String, System.Windows.Controls.DataGridTemplateColumn)
    Private BufferTypes As New HashSet(Of String)

    Private Items As New ObjectModel.ObservableCollection(Of DynamicItem)
    Private ItemsDict As New Dictionary(Of String, DynamicItem)
    Private Sub BuildBufferTable()
        Dim _Supplier As DigestionBufferSystem = cbSuppliers.SelectedItem
        If _Supplier Is Nothing Then Return
        'Found out types of buffers
        BufferTypes.Clear()
        For Each info In _Supplier.DigestionBufferInfos
            BufferTypes.Add(info.BufferName)
        Next

        'Build headers
        dgTable.Columns.Clear()
        For Each buf In BufferTypes
            Dim dt = New DataTemplate()
            Dim wi = New FrameworkElementFactory(GetType(WPFDigestionBufferItem))
            dt.VisualTree = wi
            wi.SetBinding(WPFDigestionBufferItem.DataContextProperty, New System.Windows.Data.Binding With {.Path = New PropertyPath(DynamicItem.PathStringEncode(buf))})
            dgTable.Columns.Add(New System.Windows.Controls.DataGridTemplateColumn With {.Header = New WPFDigestionBufferColumnHeader With {.Text = buf, .RemoveBuffer = AddressOf RemoveBuffer}, .CellTemplate = dt})
        Next


        If _Supplier Is Nothing Then Return

        ItemsDict.Clear()
        Items.Clear()

        For Each info In _Supplier.DigestionBufferInfos
            Dim di As DynamicItem
            If ItemsDict.ContainsKey(info.AliasName) Then
                di = ItemsDict(info.AliasName)
            Else
                di = BuildItem(info.AliasName, info.EnzymeName, _Supplier.SupplierName, BufferTypes)
                ItemsDict.Add(info.AliasName, di)
            End If
            di.EncodedMember(info.BufferName) = info
        Next
        For Each di In ItemsDict.Values
            Items.Add(di)
        Next
    End Sub
    Private Sub RemoveBuffer(_BufferName As String)
        If BufferTypes.Contains(_BufferName) Then
            BufferTypes.Remove(_BufferName)
            Dim rmColumn As System.Windows.Controls.DataGridTemplateColumn
            For Each column In dgTable.Columns
                If DirectCast(column.Header, WPFDigestionBufferColumnHeader).Text = _BufferName Then
                    rmColumn = column
                    Exit For
                End If
            Next
            If rmColumn IsNot Nothing Then
                dgTable.Columns.Remove(rmColumn)
            End If
        End If
    End Sub
    Private Sub AddBuffer(sender As Object, e As RoutedEventArgs)
        Dim _Supplier As DigestionBufferSystem = cbSuppliers.SelectedItem
        If _Supplier Is Nothing Then
            MsgBox(String.Format("Please Create/Select a Supplier First!", tbBuffers.Text), MsgBoxStyle.OkOnly, "Error")
            Return
        End If
        Dim _BufferName As String = tbBuffers.Text
        If BufferTypes.Contains(_BufferName) Then
            MsgBox(String.Format("The Buffer ""{0}"" already exists!", tbBuffers.Text), MsgBoxStyle.OkOnly, "Error")
            Return
        Else
            BufferTypes.Add(_BufferName)
            Dim dt = New DataTemplate()
            Dim wi = New FrameworkElementFactory(GetType(WPFDigestionBufferItem))
            dt.VisualTree = wi
            wi.SetBinding(WPFDigestionBufferItem.DataContextProperty, New System.Windows.Data.Binding With {.Path = New PropertyPath(DynamicItem.PathStringEncode(_BufferName))})
            dgTable.Columns.Add(New System.Windows.Controls.DataGridTemplateColumn With {.Header = New WPFDigestionBufferColumnHeader With {.Text = _BufferName, .RemoveBuffer = AddressOf RemoveBuffer}, .CellTemplate = dt})
            For Each di In ItemsDict.Values
                If TypeOf di.EncodedMember(_BufferName) IsNot DigestionBufferInfo Then
                    di.EncodedMember(_BufferName) = New DigestionBufferInfo With {.AliasName = di.Member("_Key"), .EnzymeName = di.Member("_Enzyme"), .Supplier = _Supplier.SupplierName, .BufferName = _BufferName}
                End If
            Next
        End If
    End Sub
    Private Sub ApplyBuffersByEnter(sender As Object, e As Input.KeyEventArgs)
        If e.Key = Input.Key.Enter Then BuildBufferTable()
    End Sub
    Private Sub AddNewEnzymeAlias(sender As Object, e As Windows.RoutedEventArgs)
        Dim _Supplier As DigestionBufferSystem = cbSuppliers.SelectedItem
        If _Supplier Is Nothing Then
            MsgBox(String.Format("Please Create/Select a Supplier First!", tbBuffers.Text), MsgBoxStyle.OkOnly, "Error")
            Return
        End If
        Dim _AliasName As String = tbNewAlias.Text
        If ItemsDict.ContainsKey(_AliasName) Then MsgBox(String.Format("The Alias Name ""{0}"" already exists!", _AliasName), MsgBoxStyle.OkOnly, "Error") : Return
        Dim _EnzymeName As String = tbEnzyme.Text
        Dim di = BuildItem(_AliasName, _EnzymeName, _Supplier.SupplierName, BufferTypes)
        ItemsDict.Add(_AliasName, di)
        Items.Add(di)
    End Sub
    Private Function BuildItem(_Alias As String, _Enzyme As String, _Supplier As String, buffers As IEnumerable(Of String)) As DynamicItem
        Dim di = New DynamicItem
        For Each buf In buffers
            di.EncodedMember(buf) = New DigestionBufferInfo With {.AliasName = _Alias, .EnzymeName = _Enzyme, .BufferName = buf, .Supplier = _Supplier}
        Next
        di.Member("_Remove") = New ViewModelCommand(AddressOf OnRowRequireRemove)
        di.Member("_Key") = _Alias
        di.Member("_Enzyme") = _Enzyme
        Return di
    End Function
    Private Sub OnRowRequireRemove(value As Object)
        Dim _Alias As String = value
        If Not ItemsDict.ContainsKey(_Alias) Then Return
        Dim di = ItemsDict(_Alias)
        ItemsDict.Remove(_Alias)
        Items.Remove(di)
    End Sub

    Public Property ParentTab As TabPage

    Public Event CloseTab As EventHandler




End Class
