Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes, System.Windows.Media.Animation
Public Class StyleTree
    Inherits System.Windows.Controls.TreeView
    Private _Root As New StyleElementRoot
    Public Sub New()
        Items.Add(_Root)
    End Sub
    Public ReadOnly Property Root As StyleElementRoot
        Get
            Return _Root
        End Get
    End Property
End Class
Public Class StyleElementRoot
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Private ebTitle As New System.Windows.Controls.Label
    Private WithEvents btnAdd As New AddButton
    Public Sub New()
        Header = gdHost
        ebTitle.Content = "Visual Element Archive"
        gdHost.Background = Brushes.White
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Visual, 24, 24))
        gdHost.AddColumnItem(ebTitle)
        gdHost.AddColumnItem(btnAdd)
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        AddGroup()
    End Sub
    Public Sub AddGroup()
        Dim seg As New StyleElementGroup
        Items.Add(seg)
    End Sub
End Class
Public Class StyleElementGroup
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Private ebTitle As New EditBox
    Private WithEvents btnAdd As New AddButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Folder, 24, 24))
        gdHost.AddColumnItem(ebTitle)
        gdHost.AddColumnItem(btnAdd)
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        AddGroup()
    End Sub
    Public Sub AddGroup()
        Dim se As New StyleElement
        Items.Add(se)
    End Sub
End Class

Public Class StyleElement
    Inherits System.Windows.Controls.TreeViewItem
    Private gdHost As New GridBase
    Private ebTitle As New EditBox
    Private lbType As New System.Windows.Controls.Label
    Private WithEvents btnAdd As New AddButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Element, 24, 24))
        gdHost.AddColumnItem(ebTitle)
        gdHost.AddColumnItem(lbType)
        gdHost.AddColumnItem(btnAdd)
    End Sub
End Class
