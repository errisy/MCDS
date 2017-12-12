Imports System.Windows, System.Windows.Media, System.Windows.Input

Public MustInherit Class GeneViewModel
    Inherits Errisy.GeometryViewModel
    'GeneFeatureViewModel->Center As Point Default: New Point()
    Public Property Center As Point
        Get
            Return GetValue(CenterProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(CenterProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CenterProperty As DependencyProperty = _
                           DependencyProperty.Register("Center", _
                           GetType(Point), GetType(GeneViewModel), _
                           New PropertyMetadata(New Point()))
    'GeneViewModel->CenterAngle As Double Default: 0#
    Public Property CenterAngle As Double
        Get
            Return GetValue(CenterAngleProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(CenterAngleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CenterAngleProperty As DependencyProperty = _
                           DependencyProperty.Register("CenterAngle", _
                           GetType(Double), GetType(GeneViewModel), _
                           New PropertyMetadata(0.0#))
    'GeneFeatureViewModel->File As Nuctions.GeneFile Default: Nothing
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
                           GetType(Nuctions.GeneFile), GetType(GeneViewModel), _
                           New PropertyMetadata(Nothing))
    'GeneFeatureViewModel->IsSelected As Boolean with Event Default: False
    Public Property IsSelected As Boolean
        Get
            Return GetValue(IsSelectedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsSelectedProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsSelectedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSelected", _
                           GetType(Boolean), GetType(GeneViewModel), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsSelectedChanged)))
    Private Shared Sub SharedIsSelectedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, GeneViewModel).IsSelectedChanged(d, e)
    End Sub
    Private Sub IsSelectedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            Stroke = Brushes.Blue
            StrokeThickness = 1.5#
        Else
            Stroke = Brushes.Black
            StrokeThickness = 1.0#
        End If
    End Sub
End Class
Public MustInherit Class GeneFeatureViewModel
    Inherits GeneViewModel
    'GeneFeatureViewModel->Feature As Nuctions.GeneAnnotation Default: Nothing
    Public Property Feature As Nuctions.GeneAnnotation
        Get
            Return GetValue(FeatureProperty)
        End Get
        Set(ByVal value As Nuctions.GeneAnnotation)
            SetValue(FeatureProperty, value)
        End Set
    End Property
    Public Shared ReadOnly FeatureProperty As DependencyProperty = _
                           DependencyProperty.Register("Feature", _
                           GetType(Nuctions.GeneAnnotation), GetType(GeneFeatureViewModel), _
                           New PropertyMetadata(Nothing))

End Class
Public MustInherit Class GeneEnzymeViewModel
    Inherits GeneViewModel
    'GeneEnzymeViewModel->EnzymeAnalysis As Nuctions.EnzymeAnalysis Default: Nothing
    Friend Property EnzymeAnalysis As Nuctions.EnzymeAnalysis
        Get
            Return GetValue(EnzymeAnalysisProperty)
        End Get
        Set(ByVal value As Nuctions.EnzymeAnalysis)
            SetValue(EnzymeAnalysisProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EnzymeAnalysisProperty As DependencyProperty = _
                           DependencyProperty.Register("EnzymeAnalysis", _
                           GetType(Nuctions.EnzymeAnalysis), GetType(GeneEnzymeViewModel), _
                           New PropertyMetadata(Nothing))

    'GeneEnzymeViewModel->IsSingleCut As Boolean with Event Default: False
    Public Property IsSingleCut As Boolean
        Get
            Return GetValue(IsSingleCutProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsSingleCutProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsSingleCutProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSingleCut", _
                           GetType(Boolean), GetType(GeneEnzymeViewModel), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsSingleCutChanged)))
    Private Shared Sub SharedIsSingleCutChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, GeneEnzymeViewModel).IsSingleCutChanged(d, e)
    End Sub
    Private Sub IsSingleCutChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            Stroke = Brushes.Brown
        Else
            Stroke = Brushes.Black
        End If
    End Sub
End Class