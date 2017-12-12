Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes

Public Class GridTable
    Inherits GridBase
    Public Sub New()
    End Sub
    Public Sub AddTableRow(vRow As GridTableRow)
        If vRow.ParentTable IsNot Nothing Then
            vRow.DeleteRowItems()
        End If
        vRow.ParentTable = Me
        vRow.AddRowItems(Rows.Count)
    End Sub
    Public Rows As New List(Of GridTableRow)
End Class

Public MustInherit Class GridTableRow
    Public ParentTable As GridTable
    Public Items As New List(Of FrameworkElement)
    Public Overridable Sub AddRowItems(vIndex As Integer)
        For Each it In Items
            ParentTable.Children.Add(it)
            Grid.SetRow(it, vIndex)
        Next
        If Not ParentTable.Rows.Contains(Me) Then ParentTable.Rows.Add(Me)
    End Sub
    Public Overridable Sub DeleteRowItems()
        For Each it In Items
            ParentTable.Children.Remove(it)
        Next
        If ParentTable.Rows.Contains(Me) Then ParentTable.Rows.Remove(Me)
    End Sub
    Public Overridable Sub SetRowIndex(vIndex As Integer)
        For Each it In Items
            Grid.SetRow(it, vIndex)
        Next
    End Sub
End Class
 
