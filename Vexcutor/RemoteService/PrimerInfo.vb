Imports System.ComponentModel

<Serializable>
Public Class PrimerInfo
    Implements System.ComponentModel.INotifyPropertyChanged

    Public Name As String = ""
    Public Sequence As String = ""
    Public Length As Integer = 0
    Public GCRatio As Single = 0.0F
    Public TmBind As Single = 0.0F
    Public TmFull As Single = 0.0F
    Public NeedSynthesis As Boolean = False
    Public Useful As Boolean = False
    Public UserCreated As Boolean = False '如果不是用户自己创建的话 就当没有来源时就会被删除
    <NonSerialized> Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub Calculate()
        If Sequence.LastIndexOf(">") > -1 Then
            Sequence = Nuctions.TAGCFilter(Sequence.Substring(0, Sequence.LastIndexOf(">"))) + ">" + Nuctions.TAGCFilter(Sequence.Substring(Sequence.LastIndexOf(">")))
        Else
            Sequence = Nuctions.TAGCFilter(Sequence)
        End If
        Length = GetLength(Sequence)
        GCRatio = GetGCRatio(Sequence)
        TmBind = GetTmBind(Sequence)
        TmFull = GetTmFull(Sequence)
    End Sub
    Private Shared Function GetLength(vSequence As String) As Integer
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0
        Return Nuctions.TAGCFilter(vSequence).Length
    End Function
    Private Shared Function GetGCRatio(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return "N/A"
        Dim GC As Integer = 0
        For i As Integer = 0 To vSequence.Length - 1
            Select Case vSequence(i)
                Case "G", "g", "C", "c"
                    GC += 1
            End Select
        Next
        Return CSng(GC) / CSng(vSequence.Length)
    End Function
    Private Shared Function GetTmBind(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0.0F
        Dim vSeq As String = ""
        If vSequence.IndexOf(">") < 0 Then
            vSeq = Nuctions.TAGCFilter(vSequence)
        Else
            vSeq = Nuctions.TAGCFilter(vSequence.Substring(vSequence.IndexOf(">")))
        End If
        If vSeq.Length = 0 Then
            Return 0.0F
        Else
            Return Nuctions.CalculateTm(vSeq, 80 * 0.001, 625 * 0.000000001).Tm
        End If
    End Function
    Private Shared Function GetTmFull(vSequence As String) As Single
        If vSequence Is Nothing OrElse vSequence.Length = 0 Then Return 0.0F
        Dim vSeq As String = ""
        vSeq = Nuctions.TAGCFilter(vSequence)
        If vSeq.Length = 0 Then
            Return 0.0F
        Else
            Return Nuctions.CalculateTm(vSeq, 80 * 0.001, 625 * 0.000000001).Tm
        End If
    End Function
End Class

Public Class PrimerEventArgs
    Inherits EventArgs
    Private mPrimers As New List(Of PrimerInfo)
    Public Sub New()
    End Sub
    Public Sub New(vPrimers As List(Of PrimerInfo))
        mPrimers = vPrimers
    End Sub
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property Primers As List(Of PrimerInfo)
        Get
            Return mPrimers
        End Get
        Set(value As List(Of PrimerInfo))
            mPrimers = value
        End Set
    End Property
End Class
