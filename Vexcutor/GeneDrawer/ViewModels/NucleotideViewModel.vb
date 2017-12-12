Imports System.Windows, System.Windows.Input, System.Windows.Data, System.Windows.Media
Public Class NucleotideViewModel
    Inherits Errisy.FormatedTextViewModel
    'NucleotideViewModel->Row As WPFSequenceRow Default: Nothing
    Public Property Row As WPFSequenceGeometryRow
        Get
            Return GetValue(RowProperty)
        End Get
        Set(ByVal value As WPFSequenceGeometryRow)
            SetValue(RowProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RowProperty As DependencyProperty = _
                           DependencyProperty.Register("Row", _
                           GetType(WPFSequenceGeometryRow), GetType(NucleotideViewModel), _
                           New PropertyMetadata(Nothing))
    'NucleotideViewModel->Index As Integer Default: 0
    Public Property Index As Integer
        Get
            Return GetValue(IndexProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(IndexProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IndexProperty As DependencyProperty = _
                           DependencyProperty.Register("Index", _
                           GetType(Integer), GetType(NucleotideViewModel), _
                           New PropertyMetadata(0))
    'NucleotideViewModel->RowIndex As Integer Default: 0
    Public Property RowIndex As Integer
        Get
            Return GetValue(RowIndexProperty)
        End Get
        Set(ByVal value As Integer)
            SetValue(RowIndexProperty, value)
        End Set
    End Property
    Public Shared ReadOnly RowIndexProperty As DependencyProperty = _
                           DependencyProperty.Register("RowIndex", _
                           GetType(Integer), GetType(NucleotideViewModel), _
                           New PropertyMetadata(0))
    'NucleotideViewModel->File As Nuctions.GeneFile Default: Nothing
    Public Property File As Nuctions.GeneFile
        Get
            Return GetValue(FileProperty)
        End Get
        Set(ByVal value As Nuctions.GeneFile)
            SetValue(FileProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FileProperty As DependencyProperty = _
                           DependencyProperty.Register("File", _
                           GetType(Nuctions.GeneFile), GetType(NucleotideViewModel), _
                           New PropertyMetadata(Nothing))

    Public Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        Fill = Brushes.Blue
    End Sub

End Class
