Imports System.Windows, System.Windows.Media, System.Windows.Input
Public Class EnzymeLabelViewModel
    Inherits GeneEnzymeLabelViewModel
    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New EnzymeLabelViewModel
    End Function
End Class
