Imports System.Windows
Imports System.Xaml
Public Class GeneFileViewModel
    Implements System.ComponentModel.INotifyPropertyChanged
    'Public Property IsAnalyzing As Boolean
    Private _IsAnalyzing As Boolean = False
    Public Property IsAnalyzing As Boolean
        Get
            Return _IsAnalyzing
        End Get
        Set(value As Boolean)
            _IsAnalyzing = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("IsAnalyzing"))
        End Set
    End Property

    'Public Property AnalysisProgress As Double
    Private _AnalysisProgress As Double
    Public Property AnalysisProgress As Double
        Get
            Return _AnalysisProgress
        End Get
        Set(value As Double)
            _AnalysisProgress = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("AnalysisProgress"))
        End Set
    End Property
    'Public Property AnalysisStatus As String
    Private _AnalysisStatus As String
    Public Property AnalysisStatus As String
        Get
            Return _AnalysisStatus
        End Get
        Set(value As String)
            _AnalysisStatus = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("AnalysisStatus"))
        End Set
    End Property

    'Public Property NucleotidesPerGroup As Integer
    Private _NucleotidesPerGroup As Integer = 10
    Public Property NucleotidesPerGroup As Integer
        Get
            Return _NucleotidesPerGroup
        End Get
        Set(value As Integer)
            _NucleotidesPerGroup = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("NucleotidesPerGroup"))
        End Set
    End Property
    'Public Property NucleotidesPerRow As Integer
    Private _NucleotidesPerRow As Integer = 50
    Public Property NucleotidesPerRow As Integer
        Get
            Return _NucleotidesPerRow
        End Get
        Set(value As Integer)
            _NucleotidesPerRow = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("NucleotidesPerRow"))
        End Set
    End Property

    'Public Property GeneFile As Nuctions.GeneFile
    Private _GeneFile As Nuctions.GeneFile
    Public Property GeneFile As Nuctions.GeneFile
        Get
            Return _GeneFile
        End Get
        Set(value As Nuctions.GeneFile)
            _GeneFile = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("GeneFile"))
            StartAnalyzeGeneFile()
        End Set
    End Property

#Region "Content Dependent ReadOnly Properties"
    'Public Property RowCount As Integer
    Private _RowCount As Integer
    Public ReadOnly Property RowCount As Integer
        Get
            Return _RowCount
        End Get
    End Property
    'Public Property ViewHeight As Single
    Private _ViewHeight As Single
    Public ReadOnly Property ViewHeight As Single
        Get
            Return _ViewHeight
        End Get
    End Property
    'Public Property ViewWidth As Single
    Private _ViewWidth As Single
    Public ReadOnly Property ViewWidth As Single
        Get
            Return _ViewWidth
        End Get
    End Property
    'Public Property Rows As System.Collections.ObjectModel.ObservableCollection(Of SequenceRow)
    Private _Rows As New System.Collections.ObjectModel.ObservableCollection(Of SequenceRow)
    Public ReadOnly Property Rows As System.Collections.ObjectModel.ObservableCollection(Of SequenceRow) '设置成为ReadOnly 确保不会被外部更改
        Get
            Return _Rows
        End Get
    End Property

#End Region

    Private Sub StartAnalyzeGeneFile()
        If Not (GeneFileAnalysis.IsCompleted Or GeneFileAnalysis.IsCanceled) Then AnalysisCancellationTokenSource.Cancel()

        GeneFileAnalysis = New System.Threading.Tasks.Task(AddressOf GeneFileAnalysisTask, AnalysisCancellationTokenSource.Token)
        GeneFileAnalysis.Start()
    End Sub
    Private GeneFileAnalysis As System.Threading.Tasks.Task
    Private AnalysisCancellationTokenSource As System.Threading.CancellationTokenSource

    Private Sub GeneFileAnalysisTask(state As Object)
        IsAnalyzing = True

        Dim _GeneFile = state.GeneFile
        Dim token = state.Token
        'Clear Previous Information
        _RowCount = 0
        _ViewHeight = 0
        _ViewWidth = 0
        _Rows.Clear()
        AnalysisStatus = "Calculating Number of Rows..."
        _RowCount = Math.Ceiling(_GeneFile.Length / NucleotidesPerRow)
        OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("RowCount"))
        'Check Cancel
        If token.IsCancellationRequested Then IsAnalyzing = False : Return

        Dim RowIndex As Integer = 0
        'Dim RowList As New List(Of SequenceRow)

        'Analyze each of the rows
        While RowIndex < RowCount
            RowIndex += 1
            AnalysisStatus = String.Format("Generating Row {0} of {1}", RowIndex, RowCount)
            Dim Row As New SequenceRow
            _Rows.Add(Row)
            Dim vSequence = _GeneFile.Sequence
            For i As Integer = (RowIndex - 1) * _NucleotidesPerRow To RowIndex * _NucleotidesPerRow - 1
                Row.ForwardSequence.Add(New SequenceChar(vSequence(i)))
                Row.ComplementSequence.Add(New SequenceChar(Nuctions.ComplementChar(vSequence(i))))
            Next
            'Check Cancel
            If token.IsCancellationRequested Then IsAnalyzing = False : Return
            For Each Annotation In _GeneFile.Features
                If Annotation.StartPosition <= RowIndex * _NucleotidesPerRow Or Annotation.EndPosition > (RowIndex - 1) * _NucleotidesPerRow Then
                    Row.FeatureRowMarks.Add(New FeatureRowMark)
                End If
            Next
            'Check Cancel
            If token.IsCancellationRequested Then IsAnalyzing = False : Return
        End While

        IsAnalyzing = False
    End Sub
    Private _Drawings As New System.Collections.ObjectModel.ObservableCollection(Of Errisy.AllocationViewModel)
    Public ReadOnly Property Drawings As System.Collections.ObjectModel.ObservableCollection(Of Errisy.AllocationViewModel)
        Get
            Return _Drawings
        End Get
    End Property
    Protected Sub OnPropertyChanged(e As System.ComponentModel.PropertyChangedEventArgs)
        '确保在窗体主线程当中调用属性变化
        SettingEntry.MainUIDispatcher.Invoke(Sub() RaiseEvent PropertyChanged(Me, e))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

'Public Class GeneFileAnalysisStateObject
'    Public GeneFile As Nuctions.GeneFile
'    Public PixelsPerKbp As Double
'    Public Token As System.Threading.CancellationToken
'End Class

<Serializable>
Public Class SequenceRow
    Implements System.ComponentModel.INotifyPropertyChanged

 
    'Public Property ForwardSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
    Private _ForwardSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
    Public ReadOnly Property ForwardSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
        Get
            Return _ForwardSequence
        End Get
    End Property
    'Public Property ComplementSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
    Private _ComplementSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
    Public ReadOnly Property ComplementSequence As System.Collections.ObjectModel.ObservableCollection(Of SequenceChar)
        Get
            Return _ComplementSequence
        End Get
    End Property


    'Public Property FeatureRowMarks As System.Collections.ObjectModel.ObservableCollection(Of FeatureRowMark )
    Private _FeatureRowMarks As New System.Collections.ObjectModel.ObservableCollection(Of FeatureRowMark)
    Public ReadOnly Property FeatureRowMarks As System.Collections.ObjectModel.ObservableCollection(Of FeatureRowMark)
        Get
            Return _FeatureRowMarks
        End Get
    End Property
    'Public Property RestrictionEnzymeSiteRowMarks As System.Collections.ObjectModel.ObservableCollection(Of RestrictionEnzymeSiteRowMark)
    Private _RestrictionEnzymeSiteRowMarks As System.Collections.ObjectModel.ObservableCollection(Of RestrictionEnzymeSiteRowMark)
    Public ReadOnly Property RestrictionEnzymeSiteRowMarks As System.Collections.ObjectModel.ObservableCollection(Of RestrictionEnzymeSiteRowMark)
        Get
            Return _RestrictionEnzymeSiteRowMarks
        End Get
    End Property
    Protected Sub OnPropertyChanged(e As System.ComponentModel.PropertyChangedEventArgs)
        '确保在窗体主线程当中调用属性变化
        SettingEntry.MainUIDispatcher.Invoke(Sub() RaiseEvent PropertyChanged(Me, e))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

<Serializable>
Public Class FeatureRowMark
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Sub New()
    End Sub
    Public Sub New(vFeature As Nuctions.GeneAnnotation, vRowStartPosition As Integer, vRowEndPosition As Integer)
        _StartPosition = Math.Max(vRowStartPosition, vFeature.StartPosition)
        _EndPosition = Math.Min(vRowEndPosition, vFeature.EndPosition)
    End Sub
    'Public Property StartPosition As Integer
    Private _StartPosition As Integer
    Public Property StartPosition As Integer
        Get
            Return _StartPosition
        End Get
        Set(value As Integer)
            _StartPosition = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("StartPosition"))
        End Set
    End Property
    'Public Property EndPosition As Integer
    Private _EndPosition As Integer
    Public Property EndPosition As Integer
        Get
            Return _EndPosition
        End Get
        Set(value As Integer)
            _EndPosition = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("EndPosition"))
        End Set
    End Property
    Protected Sub OnPropertyChanged(e As System.ComponentModel.PropertyChangedEventArgs)
        '确保在窗体主线程当中调用属性变化
        SettingEntry.MainUIDispatcher.Invoke(Sub() RaiseEvent PropertyChanged(Me, e))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
<Serializable>
Public Class RestrictionEnzymeSiteRowMark

End Class

<Serializable>
Public Class SequenceChar
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Sub New()
    End Sub
    Public Sub New(vNucleotide As Char)
        _Nucleotide = vNucleotide
    End Sub
    'Public Property Nucleotide As Char
    Private _Nucleotide As Char
    Public Property Nucleotide As Char
        Get
            Return _Nucleotide
        End Get
        Set(value As Char)
            _Nucleotide = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("Nucleotide"))
        End Set
    End Property
    'Public Property IsSelected As Boolean
    Private _IsSelected As Boolean
    Public Property IsSelected As Boolean
        Get
            Return _IsSelected
        End Get
        Set(value As Boolean)
            _IsSelected = value
            OnPropertyChanged(New System.ComponentModel.PropertyChangedEventArgs("IsSelected"))
        End Set
    End Property
    Protected Sub OnPropertyChanged(e As System.ComponentModel.PropertyChangedEventArgs)
        '确保在窗体主线程当中调用属性变化
        SettingEntry.MainUIDispatcher.Invoke(Sub() RaiseEvent PropertyChanged(Me, e))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class