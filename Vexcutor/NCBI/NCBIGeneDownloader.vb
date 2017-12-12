Imports System.Windows, System.Threading.Tasks
Namespace NCBI
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

        Public Async Function StartDownloadTask(sList As IEnumerable(Of String)) As Task(Of List(Of Nuctions.GeneFile))
            Total = sList.Count
            Dim cQueue As New System.Collections.Concurrent.ConcurrentQueue(Of String)
            Dim rList As New System.Collections.Concurrent.ConcurrentBag(Of Nuctions.GeneFile)
            For Each id In sList
                cQueue.Enqueue(id)
            Next
            Dim pTasks As New List(Of Task)
            For i As Integer = 1 To Math.Min(Threads, cQueue.Count)
                Dim t As New Task(Sub()
                                      Dim id As String = Nothing
                                      While id Is Nothing AndAlso cQueue.Count > 0
                                          If cQueue.TryDequeue(id) Then
                                              Dim value = NCBIUtil.EFTECH(NCBIDBEnum.nuccore, id, RETTYPEEnum.gbwithparts, RETMODEEnum.text)
                                              Dim gf = Nuctions.GeneFile.LoadFromGeneBankFormatString(value)
                                              rList.Add(gf)
                                              id = Nothing
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
            Dim gList As New List(Of Nuctions.GeneFile)
            For Each g In rList
                gList.Add(g)
            Next
            Return gList
        End Function
    End Class
End Namespace