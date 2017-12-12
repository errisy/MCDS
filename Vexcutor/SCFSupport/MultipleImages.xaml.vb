Imports System.Windows.Input, System.Windows
Public Class MultipleImages

    Public Property NumberContext As Object
        Get
            Return gdNumber.DataContext
        End Get
        Set(value As Object)
            gdNumber.DataContext = value
        End Set
    End Property
    Private Sub AddImage(sender As Object, e As MouseButtonEventArgs)
        Dim fd As New Microsoft.Win32.OpenFileDialog With {.Filter = "Image|*.jpg;*.png"}
        If fd.ShowDialog = True Then
            Dim fi As New IO.FileInfo(fd.FileName)
            Dim im As New BitImage With {.Name = fi.Name, .Data = IO.File.ReadAllBytes(fd.FileName)}
            If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of BitImage) Then
                DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of BitImage)).Add(im)
                TabHost.SelectedItem = im
            End If
        End If
    End Sub
    Private Sub SaveImage(sender As Object, e As MouseButtonEventArgs)
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of BitImage) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of BitImage))
            If TypeOf TabHost.SelectedItem Is BitImage Then
                Dim im = DirectCast(TabHost.SelectedItem, BitImage)
                Dim fd As New Microsoft.Win32.SaveFileDialog With {.Filter = "Image|*.jpg;*.png", .FileName = im.Name}
                If fd.ShowDialog = True Then
                    IO.File.WriteAllBytes(fd.FileName, im.Data)
                End If
            End If
        End If
    End Sub
    Private Sub RemoveImage(sender As Object, e As MouseButtonEventArgs)
        If MsgBox("Are you sure to remove this image?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Return
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of BitImage) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of BitImage))
            If TypeOf TabHost.SelectedItem Is BitImage Then
                imgs.Remove(DirectCast(TabHost.SelectedItem, BitImage))
            End If
        End If
    End Sub
    Private Sub RemoveTab(sender As Object, e As RoutedEventArgs)
        If MsgBox("Are you sure to remove this image?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Return
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of BitImage) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of BitImage))
            If TypeOf TabHost.SelectedItem Is BitImage Then
                imgs.Remove(DirectCast(DirectCast(e.Source, FrameworkElement).DataContext, BitImage))
            End If
        End If
    End Sub
End Class
