Imports System.Windows, System.Windows.Input, System.Windows.Data, System.Windows.Media

Public MustInherit Class GeneLabelViewModel
    Inherits Errisy.FormatedTextViewModel
    'GeneLabelViewModel->File As Nuctions.GeneFile Default: Nothing
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
                           GetType(Nuctions.GeneFile), GetType(GeneLabelViewModel), _
                           New PropertyMetadata(Nothing))
    'GeneLabelViewModel->StartPoint As Point Default: New Point()
    Public Property StartPoint As Point
        Get
            Return GetValue(StartPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(StartPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly StartPointProperty As DependencyProperty = _
                           DependencyProperty.Register("StartPoint", _
                           GetType(Point), GetType(GeneLabelViewModel), _
                           New PropertyMetadata(New Point()))

    'GeneLabelViewModel->EndPoint As Point Default: New Point()
    Public Property EndPoint As Point
        Get
            Return GetValue(EndPointProperty)
        End Get
        Set(ByVal value As Point)
            SetValue(EndPointProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EndPointProperty As DependencyProperty = _
                           DependencyProperty.Register("EndPoint", _
                           GetType(Point), GetType(GeneLabelViewModel), _
                           New PropertyMetadata(New Point()))
    'GeneLabelViewModel->Angle As Double Default: 0#
    Public Property Angle As Double
        Get
            Return GetValue(AngleProperty)
        End Get
        Set(ByVal value As Double)
            SetValue(AngleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AngleProperty As DependencyProperty = _
                           DependencyProperty.Register("Angle", _
                           GetType(Double), GetType(GeneLabelViewModel), _
                           New PropertyMetadata(0.0#))
End Class

Public Class GeneLabelComparer
    Implements System.Collections.Generic.IComparer(Of GeneLabelViewModel)
    Private _GreaterFirst As Boolean = False
    Public Sub New(GreaterFirst As Boolean)
        _GreaterFirst = GreaterFirst
    End Sub
    Public Function Compare(x As GeneLabelViewModel, y As GeneLabelViewModel) As Integer Implements IComparer(Of GeneLabelViewModel).Compare
        If Not _GreaterFirst Then
            Return Math.Sign(x.Angle - y.Angle)
        Else
            Return Math.Sign(y.Angle - x.Angle)
        End If
    End Function
End Class
Public Class BackboneLabelViewModel
    Inherits GeneLabelViewModel
End Class

Public MustInherit Class GeneFeatureLabelViewModel
    Inherits GeneLabelViewModel
    'GeneFeatureLabelViewModel->Feature As Nuctions.GeneAnnotation Default: Nothing
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
                           GetType(Nuctions.GeneAnnotation), GetType(GeneFeatureLabelViewModel), _
                           New PropertyMetadata(Nothing))

End Class
Public MustInherit Class GeneEnzymeLabelViewModel
    Inherits GeneLabelViewModel
    'GeneEnzymeLabelViewModel->EnzymeAnalysis As Nuctions.EnzymeAnalysis Default: Nothing
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
                           GetType(Nuctions.EnzymeAnalysis), GetType(GeneEnzymeLabelViewModel), _
                           New PropertyMetadata(Nothing))
    'GeneEnzymeLabelViewModel->IsSingleCut As Boolean with Event Default: False
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
                           GetType(Boolean), GetType(GeneEnzymeLabelViewModel), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsSingleCutChanged)))
    Private Shared Sub SharedIsSingleCutChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, GeneEnzymeLabelViewModel).IsSingleCutChanged(d, e)
    End Sub
    Private Sub IsSingleCutChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub

End Class