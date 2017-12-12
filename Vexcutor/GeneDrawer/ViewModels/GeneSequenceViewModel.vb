Imports Errisy
Imports System.Windows, System.Windows.Media, System.Windows.Input, System.Windows.Data
Public MustInherit Class GeneSequenceViewModel
    Inherits GeometryViewModel
    'GeneSequenceViewModel -> Location As Point Default: New Point()
    Public ReadOnly Property Location As Point
        Get
            Return GetValue(GeneSequenceViewModel.LocationProperty)
        End Get
    End Property
    Protected Shared ReadOnly LocationPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Location", _
                              GetType(Point), GetType(GeneSequenceViewModel), _
                              New PropertyMetadata(New Point()))
    Public Shared ReadOnly LocationProperty As DependencyProperty = _
                             LocationPropertyKey.DependencyProperty
    'GeneSequenceViewModel -> File As Nuctions.GeneFile Default: Nothing
    Public ReadOnly Property File As Nuctions.GeneFile
        Get
            Return GetValue(GeneSequenceViewModel.FileProperty)
        End Get
    End Property
    Protected Shared ReadOnly FilePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("File", _
                              GetType(Nuctions.GeneFile), GetType(GeneSequenceViewModel), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly FileProperty As DependencyProperty = _
                             FilePropertyKey.DependencyProperty

    'GeneSequenceViewModel -> RegionStart As Integer Default: 0
    Public ReadOnly Property RegionStart As Integer
        Get
            Return GetValue(GeneSequenceViewModel.RegionStartProperty)
        End Get
    End Property
    Protected Shared ReadOnly RegionStartPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("RegionStart", _
                              GetType(Integer), GetType(GeneSequenceViewModel), _
                              New PropertyMetadata(0))
    Public Shared ReadOnly RegionStartProperty As DependencyProperty = _
                             RegionStartPropertyKey.DependencyProperty
    'GeneSequenceViewModel -> RegionEnd As Integer Default: 0
    Public ReadOnly Property RegionEnd As Integer
        Get
            Return GetValue(GeneSequenceViewModel.RegionEndProperty)
        End Get
    End Property
    Protected Shared ReadOnly RegionEndPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("RegionEnd", _
                              GetType(Integer), GetType(GeneSequenceViewModel), _
                              New PropertyMetadata(0))
    Public Shared ReadOnly RegionEndProperty As DependencyProperty = _
                             RegionEndPropertyKey.DependencyProperty

End Class
