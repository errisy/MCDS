<Serializable>
Public Class WPFWorkSpaceViewModel
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _WorkSpace As WorkSpace
#Region "属性"
    Public Sub New(BindingWorkSpace As WorkSpace)
        _WorkSpace = BindingWorkSpace
        _PrintPages.Clear()
        If _WorkSpace.PrintPages IsNot Nothing Then
            For Each pp In _WorkSpace.PrintPages
                _PrintPages.Add(pp)
            Next
        End If
        _PrimerList.Clear()
        If _WorkSpace.PrimerList IsNot Nothing Then
            For Each pi In _WorkSpace.PrimerList
                _PrimerList.Add(pi)
            Next
        End If
        _Hosts.Clear()
        If _WorkSpace.Hosts IsNot Nothing Then
            For Each h In _WorkSpace.Hosts
                _Hosts.Add(h)
            Next
        End If
    End Sub
    Public ReadOnly Property WorkSpace As WorkSpace
        Get
            _WorkSpace.PrintPages.Clear()
            For Each pp In _PrintPages
                _WorkSpace.PrintPages.Add(pp)
            Next
            _WorkSpace.PrimerList.Clear()
            For Each pi In _PrimerList
                _WorkSpace.PrimerList.Add(pi)
            Next
            _WorkSpace.Hosts.Clear()
            For Each h In _Hosts
                _WorkSpace.Hosts.Add(h)
            Next
            Return _WorkSpace
        End Get
    End Property
    Public Property ChartItems As List(Of DNAInfo)
        Get
            Return _WorkSpace.ChartItems
        End Get
        Set(value As List(Of DNAInfo))
            _WorkSpace.ChartItems = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ChartItems"))
        End Set
    End Property
    Public Property EnzymesString As String
        Get
            Dim stb As New System.Text.StringBuilder
            For Each s In Enzymes
                stb.AppendFormat("{0} ", s)
            Next
            Return stb.ToString
        End Get
        Set(value As String)
            'Analyze Enzyme 
            _WorkSpace.Enzymes = SettingEntry.ParseEnzymeString(value)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Enzymes"))
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("EnzymesString"))
        End Set
    End Property
    Public Property Summary As String
        Get
            Return _WorkSpace.Summary
        End Get
        Set(value As String)
            _WorkSpace.Summary = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Summary"))
        End Set
    End Property
    Public Property Scale As Single
        Get
            Return _WorkSpace.Scale
        End Get
        Set(value As Single)
            _WorkSpace.Scale = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Scale"))
        End Set
    End Property
    Public Property OffsetX As Single
        Get
            Return _WorkSpace.OffsetX
        End Get
        Set(value As Single)
            _WorkSpace.OffsetX = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("OffsetX"))
        End Set
    End Property
    Public Property OffsetY As Single
        Get
            Return _WorkSpace.OffsetY
        End Get
        Set(value As Single)
            _WorkSpace.OffsetY = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("OffsetY"))
        End Set
    End Property
    Public Property PrintView As Boolean
        Get
            Return _WorkSpace.PrintView
        End Get
        Set(value As Boolean)
            _WorkSpace.PrintView = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PrintView"))
        End Set
    End Property

    Private _PrintPages As New System.Collections.ObjectModel.ObservableCollection(Of PrintPage)
    Public ReadOnly Property PrintPages As System.Collections.ObjectModel.ObservableCollection(Of PrintPage)
        Get
            Return _PrintPages
        End Get
    End Property
    Private _PrimerList As New System.Collections.ObjectModel.ObservableCollection(Of PrimerInfo)
    Public ReadOnly Property PrimerList As System.Collections.ObjectModel.ObservableCollection(Of PrimerInfo)
        Get
            Return _PrimerList
        End Get
    End Property
    'Public Property Hosts As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Host)
    Private _Hosts As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Host)
    Public ReadOnly Property Hosts As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Host)
        Get
            Return _Hosts
        End Get
    End Property
    Public Property Published As Boolean
        Get
            Return _WorkSpace.Published
        End Get
        Set(value As Boolean)
            _WorkSpace.Published = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Published"))
        End Set
    End Property
    Public Property PublicationID As Integer
        Get
            Return _WorkSpace.PublicationID
        End Get
        Set(value As Integer)
            _WorkSpace.PublicationID = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("PublicationID"))
        End Set
    End Property
    Public Property Quoted As Boolean
        Get
            Return _WorkSpace.Quoted
        End Get
        Set(value As Boolean)
            _WorkSpace.Quoted = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Quoted"))
        End Set
    End Property
    Public Property QuotationID As Integer
        Get
            Return _WorkSpace.QuotationID
        End Get
        Set(value As Integer)
            _WorkSpace.QuotationID = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("QuotationID"))
        End Set
    End Property
    Public Property ProjectServiceStatus As ProjectServiceStatusEnum
        Get
            Return _WorkSpace.ProjectServiceStatus
        End Get
        Set(value As ProjectServiceStatusEnum)
            _WorkSpace.ProjectServiceStatus = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ProjectServiceStatus"))
        End Set
    End Property
    'Public Property FileAddress As String
    Private _FileAddress As String
    Public Property FileAddress As String
        Get
            Return _FileAddress
        End Get
        Set(value As String)
            _FileAddress = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("FileAddress"))
        End Set
    End Property
    'Public Property Features As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Feature)
    Private _Features As New System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Feature)
    Public ReadOnly Property Features As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.Feature)
        Get
            Return _Features
        End Get
    End Property
    'Public Property Enzymes As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.RestrictionEnzyme)
    Private _Enzymes As New System.Collections.ObjectModel.ObservableCollection(Of Nuctions.RestrictionEnzyme)
    Public ReadOnly Property Enzymes As System.Collections.ObjectModel.ObservableCollection(Of Nuctions.RestrictionEnzyme)
        Get
            Return _Enzymes
        End Get
    End Property
    Public _DNAInfos As New System.Collections.ObjectModel.ObservableCollection(Of DNAInfo)
    Public ReadOnly Property DNAInfos As System.Collections.ObjectModel.ObservableCollection(Of DNAInfo)
        Get
            Return _DNAInfos
        End Get
    End Property

    Public ReadOnly Property LinkedView As OperationView
        Get

        End Get
    End Property

#End Region
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#Region "控制性事件"
    '控制性事件可以由界面操作触发 然后通过ViewModel执行业务逻辑 最后发送给具体UI做出图形呈现

    Public Event AddPrintPage(sender As Object, e As EventArgs)
    Public Sub RaiseAddPrintPage()
        RaiseEvent AddPrintPage(Me, New EventArgs)
    End Sub
#End Region
#Region "视图指令"
    Public Event ViewRedraw(sender As Object, e As EventArgs)
    Public Sub RaiseViewRedraw()
        RaiseEvent ViewRedraw(Me, New EventArgs)
    End Sub
#End Region
#Region "IO操作"
    Public Sub Save()
        If _FileAddress Is Nothing OrElse _FileAddress.Length = 0 OrElse Not IO.File.Exists(_FileAddress) Then
            If SettingEntry.SaveProjectDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                FileAddress = SettingEntry.SaveProjectDialog.FileName
                SaveTo(_FileAddress)
            End If
        Else
            SaveTo(_FileAddress)
        End If
    End Sub
    Public Sub SaveTo(filename As String)
        SettingEntry.SaveWPFWorkSpaceToSerialization(Me, filename)
    End Sub
    Public Shared Function LoadFrom(filename As String) As WPFWorkSpaceViewModel
        Return SettingEntry.LoadWPFWorkSpaceFromSerialization(filename)
    End Function
    Public Shared Sub Export()
        'Dim stb As New System.Text.StringBuilder
        'GenerateSummary(stb)
        'If sfdExport.ShowDialog = DialogResult.OK Then
        '    Dim fi As New IO.FileInfo(sfdExport.FileName)

        '    IO.File.WriteAllText(fi.FullName, stb.ToString)

        '    Dim fs As OperationView = WinFormOperationView
        '    Dim slist As New List(Of DNAInfo)
        '    For Each ci As ChartItem In WinFormOperationView.Items
        '        slist.Add(ci.MolecularInfo)
        '    Next
        '    fs.LoadSummary(slist, EnzymeCol, FeatureCol)

        '    Dim di As New IO.DirectoryInfo(fi.FullName.Substring(0, fi.FullName.LastIndexOf(".")))
        '    di.Create()
        '    For Each dif As IO.FileInfo In di.GetFiles
        '        dif.Delete()
        '    Next
        '    fs.Draw()
        '    fs.SavePictureTo(di.FullName + "\Project Flowchart")
        '    IO.File.WriteAllText(di.FullName + "\Project Info.txt", stb.ToString)
        '    fs.SaveFilesTo(di.FullName)
        'End If
    End Sub

#End Region

#Region "工程操作"
    Public Function AddNewOperation(ByVal Method As Nuctions.MolecularOperationEnum, Optional ByVal AutoPosition As Boolean = False) As ChartItem

        If SelectedItems.Count = 0 And Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then Return Nothing
        Dim ci As New DNAInfo
        Dim ui As New ChartItem
        If Not (Method = Nuctions.MolecularOperationEnum.FreeDesign Or Method = Nuctions.MolecularOperationEnum.Host) Then
            For Each ui In SelectedItems
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

        ci.DX = LinkedView.MenuLocation.X
        ci.DY = LinkedView.MenuLocation.Y
        Dim ch As ChartItem
        If AutoPosition Then
            If ci.Source.Count > 0 Then
                ch = LinkedView.Add(ci, Enzymes)
                ch.Draw(Graphics.FromImage(New Bitmap(1, 1, Imaging.PixelFormat.Format32bppArgb)))
                ch.AutoFit()
            Else
                ci.DX = (LinkedView.Width / 2) / Scale - LinkedView.Offset.X
                ci.DY = (LinkedView.Height / 2) / Scale - LinkedView.Offset.Y
                ch = LinkedView.Add(ci, Enzymes)
            End If
        Else
            ch = LinkedView.Add(ci, Enzymes)
        End If
        LinkedView.SelectedItems.Clear()
        ch.Selected = True

        '几种简单的修饰方法是直接计算的
        Select Case ci.MolecularOperation
            Case Nuctions.MolecularOperationEnum.Gel
                ci.Calculate()
                ch.Reload(ci, Enzymes)
            Case Nuctions.MolecularOperationEnum.Modify
                ci.Calculate()
                ch.Reload(ci, Enzymes)
            Case Nuctions.MolecularOperationEnum.Host
                ci.DetermineHost(Hosts)
            Case Nuctions.MolecularOperationEnum.Transformation
                ci.DetermineHost(Hosts)
                ci.Calculate()
                ch.Reload(ci, Enzymes)
            Case Nuctions.MolecularOperationEnum.Incubation
                ci.DetermineHost(Hosts)
            Case Nuctions.MolecularOperationEnum.Extraction
                ci.Calculate()
                ch.Reload(ci, Enzymes)
        End Select

        '表明已经修改过文件
        Modified = True

        'WinFormOperationView.Draw()
        RaiseViewRedraw()

        Return ch

    End Function
    Public Sub DeleteSelectedItems()
        If _ReadOnlyMode Then Return

    End Sub
#End Region

#Region "操作性属性"
    '界面通过绑定到这些属性实现所有操作在ViewModel当中完成而不是把业务逻辑分散到各个控件自身当中 这些属性不会通过序列化保存下来
    'Public Property ReadOnlyMode As Boolean
    Private _ReadOnlyMode As Boolean = False
    Public Property ReadOnlyMode As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(value As Boolean)
            _ReadOnlyMode = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("ReadOnlyMode"))
        End Set
    End Property
    'Public Property Modified As Boolean
    Private _Modified As Boolean = False
    Public Property Modified As Boolean
        Get
            Return _Modified
        End Get
        Set(value As Boolean)
            _Modified = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Modified"))
        End Set
    End Property
    'Public Property SelectedItems As System.Collections.ObjectModel.ObservableCollection(Of ChartItem)
    Private WithEvents _SelectedItems As New System.Collections.ObjectModel.ObservableCollection(Of ChartItem)
    Public ReadOnly Property SelectedItems As System.Collections.ObjectModel.ObservableCollection(Of ChartItem)
        Get
            Return _SelectedItems
        End Get
    End Property
    'Public Property SelectedItem As ChartItem
    Private _SelectedItem As ChartItem
    Public Property SelectedItem As ChartItem
        Get
            Return _SelectedItem
        End Get
        Set(value As ChartItem)
            _SelectedItem = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("SelectedItem"))
        End Set
    End Property
    'Public Property SelectedPrintPage As PrintPage
    Private _SelectedPrintPage As PrintPage
    Public Property SelectedPrintPage As PrintPage
        Get
            Return _SelectedPrintPage
        End Get
        Set(value As PrintPage)
            _SelectedPrintPage = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("SelectedPrintPage"))
        End Set
    End Property


#End Region




End Class