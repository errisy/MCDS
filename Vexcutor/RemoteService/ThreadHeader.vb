Public Class ThreadHeaderBase

End Class
Public Class ThreadHeader
    Inherits ThreadHeaderBase
    Public Delegate Sub ThreadStart()
    Dim mDelegate As ThreadStart
    Public Sub New(ByVal dlgt As ThreadStart)
        mDelegate = dlgt
        Dim thr As New Threading.Thread(AddressOf Start)
        thr.Start()
    End Sub
    Private Sub Start()
        mDelegate.Invoke()
    End Sub
    Public Shared Function ControlInvoke(ByVal Host As System.Windows.Forms.Control, ByVal dlgt As ThreadStart) As Boolean
        If Host.InvokeRequired Then
            Host.Invoke(dlgt, New Object() {})
            Return True
        Else
            Return False
        End If
    End Function
End Class
Public Class ThreadHeader(Of T1)
    Inherits ThreadHeaderBase
    Public Delegate Sub ThreadStart(ByVal P1 As T1)
    Dim mDelegate As ThreadStart
    Public mP1 As T1
    Public Sub New(ByVal dlgt As ThreadStart, ByVal P1 As T1)
        mDelegate = dlgt
        mP1 = P1
        Dim thr As New Threading.Thread(AddressOf Start)
        thr.Start()
    End Sub
    Private Sub Start()
        mDelegate.Invoke(mP1)
    End Sub
    Public Shared Function ControlInvoke(ByVal Host As System.Windows.Forms.Control, ByVal dlgt As ThreadStart, ByVal P1 As T1) As Boolean
        If Host.InvokeRequired Then
            Host.Invoke(CType(dlgt, ThreadStart), New Object() {P1})
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class ThreadHeader(Of T1, T2)
    Inherits ThreadHeaderBase
    Public Delegate Sub ThreadStart(ByVal P1 As T1, ByVal P2 As T2)
    Dim mDelegate As ThreadStart
    Public mP1 As T1
    Public mP2 As T2
    Public Sub New(ByVal dlgt As ThreadStart, ByVal P1 As T1, ByVal P2 As T2)
        mDelegate = dlgt
        mP1 = P1
        mP2 = P2
        Dim thr As New Threading.Thread(AddressOf Start)
        thr.Start()
    End Sub
    Private Sub Start()
        mDelegate.Invoke(mP1, mP2)
    End Sub
    Public Shared Function ControlInvoke(ByVal Host As System.Windows.Forms.Control, ByVal dlgt As ThreadStart, ByVal P1 As T1, ByVal P2 As T2) As Boolean
        If Host.InvokeRequired Then
            Host.Invoke(dlgt, New Object() {P1, P2})
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class ThreadHeader(Of T1, T2, T3)
    Inherits ThreadHeaderBase
    Public Delegate Sub ThreadStart(ByVal P1 As T1, ByVal P2 As T2, ByVal P3 As T3)
    Dim mDelegate As ThreadStart
    Public mP1 As T1
    Public mP2 As T2
    Public mP3 As T3
    Public Sub New(ByVal dlgt As ThreadStart, ByVal P1 As T1, ByVal P2 As T2, ByVal P3 As T3)
        mDelegate = dlgt
        mP1 = P1
        mP2 = P2
        mP3 = P3
        Dim thr As New Threading.Thread(AddressOf Start)
        thr.Start()
    End Sub
    Private Sub Start()
        mDelegate.Invoke(mP1, mP2, mP3)
    End Sub
    Public Shared Function ControlInvoke(ByVal Host As System.Windows.Forms.Control, ByVal dlgt As ThreadStart, ByVal P1 As T1, ByVal P2 As T2, ByVal P3 As T3) As Boolean
        If Host.InvokeRequired Then
            Host.Invoke(dlgt, New Object() {P1, P2, P3})
            Return True
        Else
            Return False
        End If
    End Function
End Class