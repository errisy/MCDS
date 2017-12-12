Partial Public Class BaseService
    '这些类型在外科类当中重写来实现

    'Public Property USERTYPE As String
    'Public Property LoginCrediential As Login

    <Serializable()> Public Class Node
        Implements IComparable(Of Node)
        Public Name As String
        Public Path As String
        Public Type As Integer
        Public Score As Single
        Public ID As String
        Public Nodes As New List(Of Node)
        Public Function CompareTo(ByVal other As Node) As Integer Implements System.IComparable(Of Node).CompareTo
            Return Math.Sign(other.Score - Score)
        End Function
    End Class

    <Serializable()> Public Class SearchInfo
        Public SearchType As Integer
        Public IncludeVector As Boolean
        Public IncludeProject As Boolean
        Public Query As String
    End Class

    Public Overridable Function List(ByVal vLogin As Login, ByVal Query As String) As String
        Return ""
    End Function
    Public Overridable Function GetUserList(ByVal vLogin As Login, ByVal Query As String) As String
        Return ""
    End Function
    Public Overridable Function Search(ByVal vLogin As Login, ByVal Query As String) As String
        Return ""
    End Function

    Public Overridable Function Save(ByVal vLogin As Login, ByVal Address As String, ByVal File As String) As String

    End Function

    Public Overridable Function Load(ByVal vLogin As Login, ByVal Address As String) As String
        Return ""
    End Function


    Public Function GetPermission(ByVal vLogin As Login, ByVal vSubAddress As String) As String
        Return ""
    End Function
    Public Function SetPermission(ByVal vLogin As Login, ByVal vSubAddress As String, ByVal Users As String) As Boolean

    End Function

    Public Overridable Function User(ByVal vLogin As Login) As Dictionary(Of Integer, Staff)
        Return New Dictionary(Of Integer, Staff)
    End Function
    Public Overridable Function UserSearch(ByVal vLogin As Login, ByVal vList As List(Of Staff)) As Dictionary(Of Integer, Staff)
        Return New Dictionary(Of Integer, Staff)
    End Function
    Public Overridable Function UserUpdate(ByVal vLogin As Login, ByVal vList As List(Of Staff)) As Dictionary(Of Integer, Staff)
        Return New Dictionary(Of Integer, Staff)
    End Function




    Public Overridable Function Permission(ByVal vLogin As Login) As Dictionary(Of Integer, GroupPermission)
        Return New Dictionary(Of Integer, GroupPermission)
    End Function
    Public Overridable Function PermissionUpdate(ByVal vLogin As Login, ByVal vList As List(Of GroupPermission)) As Dictionary(Of Integer, GroupPermission)
        Return New Dictionary(Of Integer, GroupPermission)
    End Function



    Public Overridable Function StaffOperation(ByVal vLogin As Login) As Dictionary(Of Integer, StaffOperation)
        Return New Dictionary(Of Integer, StaffOperation)
    End Function
    Public Overridable Function StaffOperationSearch(ByVal vLogin As Login, ByVal vList As List(Of StaffOperation)) As Dictionary(Of Integer, StaffOperation)
        Return New Dictionary(Of Integer, StaffOperation)
    End Function
    Public Overridable Function StaffOperationUpdate(ByVal vLogin As Login, ByVal vList As List(Of StaffOperation)) As Dictionary(Of Integer, StaffOperation)
        Return New Dictionary(Of Integer, StaffOperation)
    End Function



End Class
