Imports System.Windows, System.Windows.Media, System.Windows.Input
Public Class FeatureLabelViewModel
    Inherits GeneFeatureLabelViewModel
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New FeatureLabelViewModel
    End Function
End Class
