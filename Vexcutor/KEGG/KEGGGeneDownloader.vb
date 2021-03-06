﻿Imports System.Windows, System.Threading.Tasks
Namespace KEGG
    Public Class GeneDownloader
        Inherits DependencyObject
        'GeneDownloader->Total As Integer Default: 0
        Public Property Total As Integer
            Get
                Return GetValue(TotalProperty)
            End Get
            Set(ByVal value As Integer)
                SetValue(TotalProperty, value)
            End Set
        End Property
        Public Shared ReadOnly TotalProperty As DependencyProperty =
                               DependencyProperty.Register("Total",
                               GetType(Integer), GetType(GeneDownloader),
                               New PropertyMetadata(0))
        'GeneDownloader->Count As Integer Default: 0
        Public Property Count As Integer
            Get
                Return GetValue(CountProperty)
            End Get
            Set(ByVal value As Integer)
                SetValue(CountProperty, value)
            End Set
        End Property
        Public Shared ReadOnly CountProperty As DependencyProperty =
                               DependencyProperty.Register("Count",
                               GetType(Integer), GetType(GeneDownloader),
                               New PropertyMetadata(0))
        'GeneDownloader->Threads As Integer Default: 5
        Public Property Threads As Integer
            Get
                Return GetValue(ThreadsProperty)
            End Get
            Set(ByVal value As Integer)
                SetValue(ThreadsProperty, value)
            End Set
        End Property
        Public Shared ReadOnly ThreadsProperty As DependencyProperty =
                               DependencyProperty.Register("Threads",
                               GetType(Integer), GetType(GeneDownloader),
                               New PropertyMetadata(5))

        Public Async Function StartDownloadTask(sList As IEnumerable(Of String)) As Task(Of List(Of GeneDetail))
            Total = sList.Count
            Dim cQueue As New System.Collections.Concurrent.ConcurrentQueue(Of List(Of String))
            Dim rList As New System.Collections.Concurrent.ConcurrentBag(Of KEGG.GeneDetail)
            For i As Integer = 0 To sList.Count Step 10
                Dim vList As New List(Of String)
                For j As Integer = i To Math.Min(i + 9, sList.Count - 1)
                    vList.Add(sList(j))
                Next
                cQueue.Enqueue(vList)
            Next

            Dim pTasks As New List(Of Task)
            For i As Integer = 1 To Math.Min(Threads, cQueue.Count)
                Dim t As New Task(Sub()
                                      Dim vList As List(Of String) = Nothing
                                      While vList Is Nothing AndAlso cQueue.Count > 0
                                          If cQueue.TryDequeue(vList) Then
                                              Dim genes = KEGGUtil.GetMultipleGeneDetail(vList)
                                              For Each g In genes
                                                  rList.Add(g)
                                              Next
                                              vList = Nothing
                                          Else
                                              System.Threading.Thread.Sleep(10)
                                          End If
                                          Dispatcher.Invoke(Sub()
                                                                Count = rList.Count
                                                            End Sub)
                                      End While
                                  End Sub)
                t.Start()
                pTasks.Add(t)
            Next


            'Task.WaitAll(pTasks.ToArray)
            Await Task.WhenAll(pTasks.ToArray)
            Dim gList As New List(Of GeneDetail)
            For Each g In rList
                gList.Add(g)
            Next
            Return gList
        End Function

    End Class
End Namespace