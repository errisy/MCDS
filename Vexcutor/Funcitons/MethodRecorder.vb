Public Class TypeRecorder(Of T)
    Private vType As Type
    Private vMembers As New List(Of System.Reflection.MemberInfo)
    'Private vProperties As New Dictionary(Of String, System.Reflection.PropertyInfo)
    Private vRecords As New List(Of MemberRecord)

    Public Sub New()
        vType = GetType(T)
        For Each mt As System.Reflection.MethodInfo In vType.GetMethods
            vMembers.Add(mt)
        Next
        For Each pt As System.Reflection.PropertyInfo In vType.GetProperties
            'vProperties.Add(pt.Name, pt)
            vMembers.Add(pt)
        Next
    End Sub

    '返回每个单元记录
    Default Public Property Record(ByVal index As Integer) As MemberRecord
        Get
            Return vRecords(index)
        End Get
        Set(ByVal value As MemberRecord)
            vRecords(index) = value
        End Set
    End Property

    '整体添加记录
    Public Sub AddRange(ByVal vRecorder As TypeRecorder(Of T))
        vRecords.AddRange(vRecorder.vRecords)
    End Sub

    '清除所有的记录
    Public Sub Clear()
        vRecords.Clear()
    End Sub

    Public Sub AddMethodRecord(ByVal Name As String, ByVal Parameters As Object())
        Dim typelist As New List(Of Type)
        For Each para As Object In Parameters
            typelist.Add(para.GetType)
        Next
        For Each mb As System.Reflection.MemberInfo In vMembers
            If TypeOf mb Is System.Reflection.MethodInfo Then
                Dim mt As System.Reflection.MethodInfo = mb
                If mt.Name = Name Then
                    Dim mtParameters As System.Reflection.ParameterInfo() = mt.GetParameters
                    Dim pi As System.Reflection.ParameterInfo
                    Dim i As Integer = 0
                    Dim matchtype As Boolean = True
                    If mtParameters.Length = typelist.Count Then
                        For Each pi In mtParameters
                            'matchtype = matchtype And pi.ParameterType.Equals(typelist(i))
                            'Dim matchtype As Boolean = False
                            Dim paraType As Type = typelist(i)
                            Dim matched As Boolean = False
                            Do
                                If paraType.Equals(pi.ParameterType) Then
                                    matched = True
                                    Exit Do
                                Else
                                    paraType = paraType.BaseType
                                End If
                            Loop Until paraType Is Nothing
                            matchtype = matchtype And matched
                            i += 1
                        Next
                    Else
                        matchtype = False
                    End If
                    If matchtype Then
                        vRecords.Add(New MethodRecord(mt, Parameters))
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub AddPropertySetRecord(ByVal Name As String, ByVal Index As Object(), ByVal Value As Object)
        Dim typelist As New List(Of Type)
        For Each para As Object In Index
            typelist.Add(para.GetType)
        Next
        For Each mb As System.Reflection.MemberInfo In vMembers
            If TypeOf mb Is System.Reflection.PropertyInfo Then
                Dim mt As System.Reflection.PropertyInfo = mb
                If mt.Name = Name Then
                    Dim mtParameters As System.Reflection.ParameterInfo() = mt.GetIndexParameters
                    Dim pi As System.Reflection.ParameterInfo
                    Dim i As Integer = 0
                    Dim matchtype As Boolean = True
                    If mtParameters.Length = typelist.Count Then
                        For Each pi In mtParameters
                            'matchtype = matchtype And pi.ParameterType.Equals(typelist(i))
                            'Dim matchtype As Boolean = False
                            Dim paraType As Type = typelist(i)
                            Dim matched As Boolean = False
                            Do
                                If paraType.Equals(pi.ParameterType) Then
                                    matched = True
                                    Exit Do
                                Else
                                    paraType = paraType.BaseType
                                End If
                            Loop Until paraType Is Nothing
                            matchtype = matchtype And matched
                            i += 1
                        Next
                    Else
                        matchtype = False
                    End If
                    If matchtype Then
                        vRecords.Add(New PropertyRecord(mt, Index, Value))
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub Playback(ByVal Host As Object)
        For Each r As MemberRecord In vRecords
            r.Invoke(Host)
        Next
    End Sub
End Class

Public MustInherit Class MemberRecord
    Public MustOverride Function Invoke(ByVal Host As Object) As Object
End Class

Public Class MethodRecord
    Inherits MemberRecord
    Public Method As System.Reflection.MethodInfo
    Public Data As Object()

    Public Sub New()

    End Sub
    Public Sub New(ByVal vMethod As System.Reflection.MethodInfo, ByVal vData As Object())
        Method = vMethod
        Data = vData
    End Sub
    Public Overrides Function Invoke(ByVal Host As Object) As Object
        Return Method.Invoke(Host, Data)
    End Function
End Class

Public Class PropertyRecord
    Inherits MemberRecord
    Public [Property] As System.Reflection.PropertyInfo
    Public Data As Object
    Public Index As Object()
    Public Sub New()
    End Sub
    Public Sub New(ByVal vProperty As System.Reflection.PropertyInfo, ByVal vIndex As Object(), ByVal vData As Object)
        [Property] = vProperty
        Data = vData
        Index = vIndex
    End Sub
    Public Overrides Function Invoke(ByVal Host As Object) As Object
        [Property].SetValue(Host, Data, Index)
        Return Nothing
    End Function
End Class