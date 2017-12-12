Imports System.Windows, System.Windows.Input
Public Class MultipleSequencingResults
    
    Public Property NumberContext As Object
        Get
            Return gdSequencing.DataContext
        End Get
        Set(value As Object)
            gdSequencing.DataContext = value
        End Set
    End Property
    Private Sub RemoveTab(sender As Object, e As RoutedEventArgs)
        If MsgBox("Are you sure to remove this sequence?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Return
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of SequenceItem) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of SequenceItem))
            If TypeOf TabHost.SelectedItem Is SequenceItem Then
                imgs.Remove(DirectCast(DirectCast(e.Source, FrameworkElement).DataContext, SequenceItem))
            End If
        End If
    End Sub
    Private Sub AddSequence(sender As Object, e As RoutedEventArgs)
        Dim fd As New Microsoft.Win32.OpenFileDialog With {.Filter = "Sequencing Result File|*.scf;*.ab1"}
        If fd.ShowDialog = True Then
            Dim fi As New IO.FileInfo(fd.FileName)
            Select Case fi.Extension.ToUpper
                Case ".SCF"
                    Dim im As New SequenceItem With {.Name = fi.Name, .Data = IO.File.ReadAllBytes(fd.FileName), .FileType = SequencingFileTypeEnum.SCF}
                    Dim scf As New SCFData
                    scf.Read(New IO.MemoryStream(im.Data))
                    im.RawData = scf.GetRawData
                    If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of SequenceItem) Then
                        DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)).Add(im)
                        TabHost.SelectedItem = im
                    End If
                Case ".AB1"
                    Dim im As New SequenceItem With {.Name = fi.Name, .Data = IO.File.ReadAllBytes(fd.FileName), .FileType = SequencingFileTypeEnum.AB1}
                    Dim ab1 As New AB1Data
                    ab1.Read(im.Data)
                    im.RawData = ab1.GetRawData
                    If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of SequenceItem) Then
                        DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of SequenceItem)).Add(im)
                        TabHost.SelectedItem = im
                    End If
            End Select
        End If
    End Sub
    Private Sub RemoveSequence(sender As Object, e As RoutedEventArgs)
        If MsgBox("Are you sure to remove this sequence?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Return
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of SequenceItem) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of SequenceItem))
            If TypeOf TabHost.SelectedItem Is SequenceItem Then
                imgs.Remove(DirectCast(TabHost.SelectedItem, SequenceItem))
            End If
        End If
    End Sub
    Private Sub SaveSequence(sender As Object, e As RoutedEventArgs)
        If TypeOf TabHost.ItemsSource Is System.Collections.ObjectModel.ObservableCollection(Of SequenceItem) Then
            Dim imgs = DirectCast(TabHost.ItemsSource, System.Collections.ObjectModel.ObservableCollection(Of SequenceItem))
            If TypeOf TabHost.SelectedItem Is SequenceItem Then
                Dim im = DirectCast(TabHost.SelectedItem, SequenceItem)
                Dim fd As New Microsoft.Win32.SaveFileDialog With {.FileName = im.Name}
                Select Case im.FileType
                    Case SequencingFileTypeEnum.AB1
                        fd.Filter = "AB1|*.ab1"
                    Case SequencingFileTypeEnum.SCF
                        fd.Filter = "SCF|*.scf"
                End Select
                If fd.ShowDialog = True Then
                    IO.File.WriteAllBytes(fd.FileName, im.Data)
                End If
            End If
        End If
    End Sub
End Class
