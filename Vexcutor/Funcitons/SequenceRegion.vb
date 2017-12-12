Public Class SequenceRegion
    Public Sub New(ByVal vCircular As Boolean, ByVal vLength As Integer)
        Circular = vCircular
        Length = vLength
    End Sub
    Public Property Circular As Boolean
    Public Property Length As Integer
    Public Property Start As Integer
    Public Property [End] As Integer
    Public Shared Operator And(ByVal Index As Integer, ByVal vSR As SequenceRegion) As Boolean
        If vSR.Circular Then
            '环形质粒 0-0
            If vSR.Start <= vSR.End Then
                Return Index >= vSR.Start And Index <= vSR.End
            Else
                Return (Index >= 0 And Index <= vSR.End) Or (Index >= vSR.Start And Index <= vSR.Length - 1)
            End If
        Else
            Return Index >= vSR.Start And Index <= vSR.End
        End If
    End Operator
End Class

Friend Class RegionLocator
    Inherits SortedList(Of Integer, MCDS.SequenceViewer.Line)

    Public Shadows Sub Add(ByVal vL As MCDS.SequenceViewer.Line)
        MyBase.Add(vL.Y, vL)
    End Sub

    Public Function GetIndex(ByVal Value As Integer) As Integer
        If Keys.Count = 0 Then
            Return -1
        Else
            Dim i As Single = (Count - 1) / 2 + 0.01
            Dim mI As Integer = Math.Floor(i)
            Dim dI As Single = (Count - 1) / 4
            If Value <= 0 Then Return 0
            If Value >= Keys(Count - 1) Then Return Count - 1
            While True
                If Keys(Math.Ceiling(i)) > Value Then
                    i -= dI
                    mI = Math.Floor(i)
                    If i < 0 Then i = 0.01
                    If i >= Me.Count - 1 Then i = Me.Count - 1.99
                    If Keys(Math.Ceiling(i)) > Value And Keys(Math.Floor(i)) <= Value Then Exit While
                Else
                    i += dI
                    mI = i - 1
                    If i < 0 Then i = 0.01
                    If i >= Me.Count - 1 Then i = Me.Count - 1.99
                    If Keys(Math.Ceiling(i)) > Value And Keys(Math.Floor(i)) <= Value Then Exit While
                End If

                If dI < 2 Then
                    dI = 1
                Else
                    dI = dI / 2
                End If
            End While
            If mI < 0 Then mI = 0
            Return mI
        End If
    End Function
    Default Public Shadows ReadOnly Property Line(ByVal Y As Integer) As Integer
        Get
            Return GetIndex(Y)
        End Get
    End Property
    Public Shared Operator >>(ByVal RL As RegionLocator, ByVal Y As Integer) As Integer
        Return RL.GetIndex(Y)
    End Operator
End Class
