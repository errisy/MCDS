Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
Public Class RestrictionManagementControl
    Inherits UserControl
    Public Frame As New System.Windows.Controls.ScrollViewer With {.VerticalScrollBarVisibility = ScrollBarVisibility.Auto, .HorizontalScrollBarVisibility = ScrollBarVisibility.Auto}
    Public Management As New RestrictionManagement
    Public Sub New()
        Content = Frame
        Frame.Content = Management
    End Sub
End Class

Public Class RestrictionManagement
    Inherits GridBase
    Public NameHeader As New Label With {.Name = "Name"}
    Public SequenceHeader As New Label With {.Content = "Restriction Sequence"}
    Public FCutHeader As New Label With {.Content = "F Cut Position"}
    Public RCutHeader As New Label With {.Content = "R Cut Position"}
    Public Sub New()

    End Sub
End Class

Public Class RestrictionEnzymeEntry
    Public NameBox As New EditBox
    Public Sequence As New EditBox
    Public FCut As New EditBox
    Public RCut As New EditBox
    Public Sub New()
        Grid.SetColumn(NameBox, 0)
        Grid.SetColumn(Sequence, 0)
        Grid.SetColumn(FCut, 0)
        Grid.SetColumn(RCut, 0)
    End Sub
    Public Sub AddTo(vGrid As Grid)
        vGrid.Children.Add(NameBox)
        vGrid.Children.Add(Sequence)
        vGrid.Children.Add(FCut)
        vGrid.Children.Add(RCut)
    End Sub
End Class