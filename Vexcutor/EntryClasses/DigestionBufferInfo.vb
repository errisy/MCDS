Imports System.ComponentModel
Imports MCDS

' "BamHI" "BamHI-HF" "3.1" "50" "NEB" "37"
<Serializable>
Public Class DigestionBufferInfo
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _EnzymeName As String
    Public Property EnzymeName As String
        Get
            Return _EnzymeName
        End Get
        Set(value As String)
            _EnzymeName = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("EnzymeName"))
        End Set
    End Property
    Private _AliasName As String
    Public Property AliasName As String
        Get
            Return _AliasName
        End Get
        Set(value As String)
            _AliasName = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("AliasName"))
        End Set
    End Property
    Private _BufferName As String
    Public Property BufferName As String
        Get
            Return _BufferName
        End Get
        Set(value As String)
            _BufferName = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("BufferName"))
        End Set
    End Property
    Private _Activity As Single
    Public Property Activity As Single
        Get
            Return _Activity
        End Get
        Set(value As Single)
            _Activity = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Activity"))
        End Set
    End Property
    Private _Supplier As String
    Public Property Supplier As String
        Get
            Return _Supplier
        End Get
        Set(value As String)
            _Supplier = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Supplier"))
        End Set
    End Property
    Private _Temperature As Single
    Public Property Temperature As Single
        Get
            Return _Temperature
        End Get
        Set(value As Single)
            _Temperature = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Temperature"))
        End Set
    End Property
    Private _Additives As String
    Public Property Additives As String
        Get
            Return _Additives
        End Get
        Set(value As String)
            _Additives = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Additives"))
        End Set
    End Property
    Private _HasStarActivity As Boolean
    Public Property HasStarActivity As Boolean
        Get
            Return _HasStarActivity
        End Get
        Set(value As Boolean)
            _HasStarActivity = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("HasStarActivity"))
        End Set
    End Property
    Private _dam As MethylationSensitivity
    Public Property dam As MethylationSensitivity
        Get
            Return _dam
        End Get
        Set(value As MethylationSensitivity)
            _dam = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("dam"))
        End Set
    End Property
    Private _dcm As MethylationSensitivity
    Public Property dcm As MethylationSensitivity
        Get
            Return _dcm
        End Get
        Set(value As MethylationSensitivity)
            _dcm = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("dcm"))
        End Set
    End Property
    Private _CpG As MethylationSensitivity
    Public Property CpG As MethylationSensitivity
        Get
            Return _CpG
        End Get
        Set(value As MethylationSensitivity)
            _CpG = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("CpG"))
        End Set
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("Buffer {0} {1}%@{4}C({2}){3}; ", BufferName, Activity.ToString("0"), AliasName, Additives, Temperature.ToString("0"))
    End Function
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class CombinedDigestionSorter
    Inherits List(Of CombinedDigestionCondition)
    Public Sub Push(c As DigestionBufferInfo)
        'Dim _key = c.AliasKey
        'Dim CBCs As List(Of CombinedDigestionCondition)
        'If ContainsKey(_key) Then
        '    CBCs = Me(_key)
        'Else
        '    CBCs = New List(Of CombinedDigestionCondition)
        '    Me.Add(_key, CBCs)
        'End If
        Dim ACTs = Me.Where(Function(cbc) cbc.Activity = c.Activity)
        Dim ACT As CombinedDigestionCondition
        If ACTs.Any Then
            ACT = ACTs.First
            ACT.Buffers.Add(c.BufferName + IIf(c.HasStarActivity, "*", "") + IIf(c.Additives.Count > 0, "+", "") + String.Join("+", c.Additives))
        Else
            ACT = New CombinedDigestionCondition With {.Activity = c.Activity, .Temperature = c.Temperature}
            ACT.EnzymeName = c.EnzymeName
            ACT.AliasName = c.AliasName
            ACT.Buffers.Add(c.BufferName + IIf(c.HasStarActivity, "*", "") + IIf(c.Additives.Count > 0, "+", "") + String.Join("+", c.Additives))
            Add(ACT)
        End If
    End Sub
    Public Function GetCombined() As List(Of CombinedDigestionCondition)
        Dim cList As New List(Of CombinedDigestionCondition)
        cList.AddRange(Me)
        cList.Sort(New GenericComparer(Of CombinedDigestionCondition)(Function(x, y) Math.Sign(y.Activity - x.Activity)))
        Return cList
    End Function
End Class

Public Class CombinedDigestionCondition
    Public Buffers As New HashSet(Of String)
    Public Activity As Single = 100.0F
    Public EnzymeName As String
    Public AliasName As String
    Public Temperature As Single
    Public Additives As New HashSet(Of String)
    Public HasStarActivity As Boolean = False
    Public Overrides Function ToString() As String
        Return String.Format("Buffer {0} {1}%@{4}C({2}){3}; ", String.Join("/", Buffers), Activity.ToString("0"), AliasName, String.Join(",", Additives), Temperature.ToString("0"))
    End Function
End Class

Public Class DigestionBufferCondition
    Public BufferName As String = Nothing
    Public Activity As Single = 100.0F
    Public EnzymeNames As New HashSet(Of String)
    Public AliasNames As New HashSet(Of String)
    Public Temperature As Single '= Nothing
    Public Additives As New HashSet(Of String)
    Public HasStarActivity As Boolean = False
    Public Function ApplyDigestionInfo(info As DigestionBufferInfo) As DigestionBufferCondition
        If BufferName Is Nothing Then
            Dim dbc As New DigestionBufferCondition With {.BufferName = info.BufferName}
            dbc.EnzymeNames.Add(info.EnzymeName)
            dbc.AliasNames.Add(info.AliasName)
            dbc.Activity = info.Activity
            dbc.Temperature = info.Temperature
            If info.Additives IsNot Nothing Then
                For Each additive In info.Additives.Split(New Char() {" "c, ","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
                    dbc.Additives.Add(additive)
                Next
            End If
            dbc.HasStarActivity = info.HasStarActivity
            Return dbc
        ElseIf BufferName = info.BufferName AndAlso Temperature = info.Temperature AndAlso Math.Min(Activity, info.Activity) >= 50.0F Then
            Dim dbc As New DigestionBufferCondition With {.BufferName = BufferName, .Temperature = Temperature}
            For Each _Name In EnzymeNames
                dbc.EnzymeNames.Add(_Name)
            Next
            dbc.EnzymeNames.Add(info.EnzymeName)
            For Each _Name In AliasNames
                dbc.AliasNames.Add(_Name)
            Next
            dbc.AliasNames.Add(info.AliasName)
            dbc.Activity = Math.Min(Activity, info.Activity)
            For Each additive In Additives
                dbc.Additives.Add(additive)
            Next
            If info.Additives IsNot Nothing Then
                For Each additive In info.Additives.Split(New Char() {" "c, ","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
                    dbc.Additives.Add(additive)
                Next
            End If
            dbc.HasStarActivity = HasStarActivity Or info.HasStarActivity
            Return dbc
        Else
            Return Nothing
        End If
    End Function
    Public Overrides Function ToString() As String
        Return String.Format("Buffer {0} {1}%@{4}C({2}){3}; ", BufferName, Activity.ToString("0"), String.Join(",", AliasNames), String.Join(",", Additives), Temperature.ToString("0"))
    End Function
    Public ReadOnly Property AliasKey As String
        Get
            Dim vList As New List(Of String)
            For Each _AliasName In AliasNames
                vList.Add(_AliasName)
            Next
            vList.Sort()
            Return String.Join(", ", vList)
        End Get
    End Property
End Class

Public Class CombinedBufferSorter
    Inherits Dictionary(Of String, List(Of CombinedBufferCondition))
    Public Sub Push(c As DigestionBufferCondition)
        Dim _key = c.AliasKey
        Dim CBCs As List(Of CombinedBufferCondition)
        If ContainsKey(_key) Then
            CBCs = Me(_key)
        Else
            CBCs = New List(Of CombinedBufferCondition)
            Me.Add(_key, CBCs)
        End If
        Dim ACTs = CBCs.Where(Function(cbc) cbc.Activity = c.Activity)
        Dim ACT As CombinedBufferCondition
        If ACTs.Any Then
            ACT = ACTs.First
            ACT.Buffers.Add(c.BufferName + IIf(c.HasStarActivity, "*", "") + IIf(c.Additives.Count > 0, "+", "") + String.Join("+", c.Additives))
        Else
            ACT = New CombinedBufferCondition With {.Activity = c.Activity, .Temperature = c.Temperature}
            ACT.EnzymeNames = c.EnzymeNames
            ACT.AliasNames = c.AliasNames
            ACT.Buffers.Add(c.BufferName + IIf(c.HasStarActivity, "*", "") + IIf(c.Additives.Count > 0, "+", "") + String.Join("+", c.Additives))
            CBCs.Add(ACT)
        End If
    End Sub
    Public Function GetCombined() As List(Of CombinedBufferCondition)
        Dim cList As New List(Of CombinedBufferCondition)
        For Each vList In Me.Values
            cList.AddRange(vList)
        Next
        cList.Sort(New GenericComparer(Of CombinedBufferCondition)(Function(x, y) Math.Sign(y.Activity - x.Activity)))
        Return cList
    End Function
End Class
Public Class CombinedBufferCondition
    Public Buffers As New HashSet(Of String)
    Public Activity As Single = 100.0F
    Public EnzymeNames As New HashSet(Of String)
    Public AliasNames As New HashSet(Of String)
    Public Temperature As Single '= Nothing
    Public Additives As New HashSet(Of String)
    Public HasStarActivity As Boolean = False
    Public Overrides Function ToString() As String
        Return String.Format("Buffer {0} {1}%@{4}C({2}){3}; ", String.Join("/", Buffers), Activity.ToString("0"), String.Join(",", AliasNames), String.Join(",", Additives), Temperature.ToString("0"))
    End Function
End Class
Public Class GenericComparer(Of T)
    Implements IComparer(Of T)
    Private _Selector As Func(Of T, T, Integer)
    Public Sub New(Selector As Func(Of T, T, Integer))
        _Selector = Selector
    End Sub
    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Return _Selector(x, y)
    End Function
End Class
Public Class DigestionBufferConditionComparer
    Implements IComparer(Of DigestionBufferCondition)
    Public Shared ReadOnly [Default] As New DigestionBufferConditionComparer
    Public Function Compare(ByVal x As DigestionBufferCondition, ByVal y As DigestionBufferCondition) As Integer Implements System.Collections.Generic.IComparer(Of DigestionBufferCondition).Compare
        Return Math.Sign(y.Activity - x.Activity)
    End Function
End Class

Public Class DigestionBufferInfoComparer
    Implements IComparer(Of DigestionBufferInfo)
    Public Shared ReadOnly [Default] As New DigestionBufferInfoComparer
    Public Function Compare(ByVal x As DigestionBufferInfo, ByVal y As DigestionBufferInfo) As Integer Implements System.Collections.Generic.IComparer(Of DigestionBufferInfo).Compare
        Return Math.Sign(y.Activity - x.Activity)
    End Function
End Class
<Serializable>
Public Enum MethylationSensitivity
    NotSensitive
    ImpairedbySomeCombinationsofOverlapping
    ImpairedbyOverlapping
    BlockedbySomeCombinationsofOverlapping
    BlockedbyOverlapping
    Blocked
End Enum

<Serializable>
Public Class DigestionBufferData
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _DiegsetionBufferSystems As New ObjectModel.ObservableCollection(Of DigestionBufferSystem)
    Public ReadOnly Property DigestionBufferSystems As ObjectModel.ObservableCollection(Of DigestionBufferSystem)
        Get
            Return _DiegsetionBufferSystems
        End Get
    End Property
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class

<Serializable>
Public Class DigestionBufferSystem
    Implements System.ComponentModel.INotifyPropertyChanged
    Private _SupplierName As String
    Public Property SupplierName As String
        Get
            Return _SupplierName
        End Get
        Set(value As String)
            _SupplierName = value
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("SupplierName"))
        End Set
    End Property
    Private _DigestionBufferInfos As New ObjectModel.ObservableCollection(Of DigestionBufferInfo)
    Public ReadOnly Property DigestionBufferInfos As ObjectModel.ObservableCollection(Of DigestionBufferInfo)
        Get
            Return _DigestionBufferInfos
        End Get
    End Property
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class