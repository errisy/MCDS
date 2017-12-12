<Serializable()> Public Class Bunbury

    Public Sub New()

    End Sub
    Public Sender As String
    Public Destinations As String()
    Public Data As Byte()
    Public Type As DataType
End Class

<Serializable()> Public Class Docklands
    Public UserName As String
    Public Password As String
End Class

Public Enum DataType As Integer
    Login
    Query
    DataArray
    Command
End Enum

<Serializable()> Public Class Login
    Inherits Docklands
    Public Result As Boolean = False
    Public UserGroup As StaffGroups
    Public Permissions As New List(Of UserPermission)
    Public ID As String
End Class

Public Class ICol
    Inherits List(Of Integer)
    Public Sub New()

    End Sub
    Public Sub New(ByVal ParamArray ints() As Integer)
        For Each i As Integer In ints
            MyBase.Add(i)
        Next
    End Sub
    Public Shared Operator And(ByVal P1 As ICol, ByVal P2 As ICol) As Boolean
        For Each i As Integer In P1
            If P2.Contains(i) Then Return True
        Next
        Return False
    End Operator
    Public Shared Operator And(ByVal P1 As ICol, ByVal P2 As List(Of UserPermission)) As Boolean
        For Each i As Integer In P1
            If P2.Contains(i) Then Return True
        Next
        Return False
    End Operator
    Public Shared Operator And(ByVal P1 As List(Of UserPermission), ByVal P2 As ICol) As Boolean
        For Each i As Integer In P1
            If P2.Contains(i) Then Return True
        Next
        Return False
    End Operator
    Public Shared Operator And(ByVal P1 As ICol, ByVal P2 As Integer) As Boolean
        Return P1.Contains(P2)
    End Operator
    Public Shared Operator And(ByVal P1 As Integer, ByVal P2 As ICol) As Boolean
        Return P2.Contains(P1)
    End Operator
End Class

Public Enum DataAccessLevel As Integer
    ReadWriteAddDelete = 0
    ReadWriteNoAddNoDelete = 1
    [ReadOnly] = 2
End Enum

Public Class DataEventArgs
    Inherits EventArgs
    Public UpdateOwnerCall As SubCall
    Public DataOperation As DataOperationView = DataOperationView.None
    Public Sub New(ByVal vData As Object)
        Data = vData
    End Sub
    Public Data As Object
End Class

Public Enum DataOperationView As Integer
    None
    Order
    Delivery
    Payment
    Cancel
    Trade
    Item
    Customer
    Company
End Enum

Public Delegate Sub SubCall()
