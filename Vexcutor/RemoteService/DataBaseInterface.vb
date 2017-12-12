Imports System.ComponentModel
 
  
<Serializable()> Public Enum Gender As Integer
    Unknown
    Male
    Female
End Enum
 
<Serializable()> Public MustInherit Class InfoBase
    Inherits EditBase
    <Browsable(False)> Public Property DateTime As DateTime
    <Browsable(False)> Public Property Modifier As Staff
    'OldVersions用来保存用户更改过的信息
    <Browsable(False)> Public Property OldVersions As New List(Of InfoBase)
    Public MustOverride Function CloneCurrentVersion() As InfoBase
    Public Sub SaveCurrentVersion()
        Dim ov As List(Of InfoBase) = OldVersions
        OldVersions = Nothing
        Dim obj = Me.CloneCurrentVersion()
        ov.Add(obj)
        Me.OldVersions = ov
    End Sub
End Class

<Serializable()> Public Class Staff
    Inherits EditBase

    'Only admin has access to this.
    <System.ComponentModel.Category("Human Resource"), System.ComponentModel.ReadOnly(True)> Public Property ID As Integer
    <System.ComponentModel.Category("Human Resource")> Public Property UserName As String
    <System.ComponentModel.Category("Human Resource")> Public Property Password As String
    <System.ComponentModel.Category("Personal Information")> Public Property Gender As Gender
    <System.ComponentModel.Category("Personal Information")> Public Property GivenName As String
    <System.ComponentModel.Category("Personal Information")> Public Property MiddleName As String
    <System.ComponentModel.Category("Personal Information")> Public Property FamilyName As String
    <System.ComponentModel.Category("Personal Information")> Public Property DateofBirth As Date
    <System.ComponentModel.Category("Human Resource")> Public Property Group As StaffGroups
    <System.ComponentModel.Category("Human Resource")> Public Property Valid As Boolean = True



    Public Overrides Function ToString() As String
        Return DescribeObject(Me)
    End Function
    Class DataGrid
        <ColumnDefine("ID", ColumnTypeEnum.Text, 0, True)> Public Shared Function Key(ByVal ID As Integer) As Integer
            Return ID
        End Function
        <ColumnDefine("ID", ColumnTypeEnum.Text, 60, True)> Public Shared Function _0(ByVal ID As Integer) As Integer
            Return ID
        End Function
        <ColumnDefine("UserName", ColumnTypeEnum.Text, 60, True)> Public Shared Function _1(ByVal UserName As String) As String
            Return UserName
        End Function
        <ColumnDefine("Password", ColumnTypeEnum.Text, 60, True)> Public Shared Function _2(ByVal Password As String) As String
            Return Password
        End Function
        <ColumnDefine("Group", ColumnTypeEnum.Text, 60, True)> Public Shared Function _3(ByVal Group As StaffGroups) As String
            Return [Enum].GetName(GetType(StaffGroups), Group)
        End Function
        <ColumnDefine("Valid", ColumnTypeEnum.Text, 60, True)> Public Shared Function _4(ByVal Valid As Boolean) As String
            Return Valid.ToString
        End Function
        <ColumnDefine("Gender", ColumnTypeEnum.Text, 60, True)> Public Shared Function _5(ByVal Gender As Gender) As String
            Return [Enum].GetName(GetType(Gender), Gender)
        End Function
        <ColumnDefine("GivenName", ColumnTypeEnum.Text, 120, True)> Public Shared Function _6(ByVal GivenName As String) As String
            Return GivenName
        End Function
        <ColumnDefine("MiddleName", ColumnTypeEnum.Text, 120, True)> Public Shared Function _7(ByVal MiddleName As String) As String
            Return MiddleName
        End Function
        <ColumnDefine("FamilyName", ColumnTypeEnum.Text, 60, True)> Public Shared Function _8(ByVal FamilyName As String) As String
            Return FamilyName
        End Function
        <ColumnDefine("DateofBirth", ColumnTypeEnum.Text, 120, True)> Public Shared Function _9(ByVal DateofBirth As Date) As String
            Return DateofBirth.ToString
        End Function

    End Class

    Public Overrides Function GetID() As Integer
        Return ID
    End Function

    Public Overrides Sub SetID(ByVal Value As Integer)
        ID = Value
    End Sub
End Class

Public Class CustomerDescriptor
    Inherits PropertyDescriptor
    Public Sub New(ByVal Name As String, ByVal Attr As Attribute())
        MyBase.New(Name, Attr)
        DefaultAttributes = Attr
        Dim RO As ReadOnlyAttribute = Nothing
        For Each att As Attribute In Attributes
            If TypeOf att Is ReadOnlyAttribute Then
                RO = att
                Exit For
            End If
        Next
        vIsReadOnly = Not (RO Is Nothing) AndAlso RO.IsReadOnly
    End Sub
    Public DefaultAttributes As Attribute()
    Public List As IList
    Public PropertyCollection As PropertyDescriptorCollection
    Private vIsReadOnly As Boolean = False
    Public Index As Integer
    Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
        Return True
    End Function

    Public Overrides ReadOnly Property ComponentType As System.Type
        Get
            Return GetType(List(Of String))
        End Get
    End Property

    Public Overrides Function GetValue(ByVal component As Object) As Object
        If Not (List Is Nothing) Then
            If Index > -1 And List.Count > Index Then
                Return List(Index)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return vIsReadOnly
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As System.Type
        Get
            Return GetType(String)
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)

    End Sub

    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        If Not (List Is Nothing) Then
            If Index > -1 And List.Count > Index Then
                List(Index) = value
            ElseIf Index = List.Count Then
                List.Add(value)
                Dim PD As New CustomerDescriptor(Index.ToString, DefaultAttributes)
                PD.Index = Index + 1
                PD.List = List
                PropertyCollection.Add(PD)
            End If
        End If
    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return True
    End Function
    Public Overrides ReadOnly Property Converter As System.ComponentModel.TypeConverter
        Get
            Return New ExpandableListConverter
        End Get
    End Property
End Class

Public Class AllowAddAttribute
    Inherits Attribute
    Private vAllow As Boolean
    Public Sub New(ByVal Allow As Boolean)
        vAllow = Allow
    End Sub
    Public ReadOnly Property AllowAdd As Boolean
        Get
            Return vAllow
        End Get
    End Property
End Class

Public Class ExpandableListConverter
    Inherits System.ComponentModel.ExpandableObjectConverter

    Public Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function
    Public Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
        Return MyBase.ConvertFrom(context, culture, value)
    End Function
    Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
        Return DescribeObject(value)
    End Function

    Public Overrides Function CreateInstance(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal propertyValues As System.Collections.IDictionary) As Object
        Return MyBase.CreateInstance(context, propertyValues)
    End Function
    Public Overrides Function GetProperties(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal value As Object, ByVal attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection
        Dim PDC As New PropertyDescriptorCollection(Nothing)
        Dim PD As CustomerDescriptor

        If TypeOf value Is IList Then
            Dim vList As IList = value
            Dim T As Type = value.GetType
            Dim iT As Type() = T.GetGenericArguments()
            If iT.Length = 1 Then
                Dim AA As AllowAddAttribute = Nothing
                For Each att As Attribute In attributes
                    If TypeOf att Is AllowAddAttribute Then
                        AA = att
                        Exit For
                    End If
                Next
                Dim LS As IList = value
                Dim i As Integer = 0
                For Each s As Object In LS
                    PD = New CustomerDescriptor(i.ToString, attributes)
                    PD.Index = i
                    PD.List = value
                    PD.PropertyCollection = PDC
                    i += 1
                    PDC.Add(PD)
                Next
                If Not (AA Is Nothing) AndAlso AA.AllowAdd Then
                    PD = New CustomerDescriptor(i.ToString, attributes)
                    PD.Index = i
                    PD.List = value
                    PD.PropertyCollection = PDC
                    PDC.Add(PD)
                End If
            End If
            Return PDC
        End If
        Return MyBase.GetProperties(context, value, attributes)
    End Function
    Public Overrides Function GetPropertiesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function
End Class
 
<Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> Public Class BankAccount
    Inherits EditBase
    Public Property Branch As String
    Public Property AccountName As String
    Public Property AccountNumber As String

    Public Overrides Function ToString() As String
        Return DescribeObject(Me)
    End Function

    Public Overrides Function GetID() As Integer
        Return 0
    End Function
    Public Overrides Sub SetID(ByVal Value As Integer)
    End Sub
End Class
<Serializable()> Public Class CardAccount
    Inherits EditBase
    Public Property NameOnCard As String
    Public Property CardNumber As String
    Public Property ExpiryDate As String

    Public Overrides Function ToString() As String
        Return DescribeObject(Me)
    End Function

    Public Overrides Function GetID() As Integer
        Return 0
    End Function

    Public Overrides Sub SetID(ByVal Value As Integer)
    End Sub
End Class
 
<Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> Public Class Price
    Inherits InfoBase

    '这个类型可能会被废弃不用 因为这些信息不重要 仅仅是个指导价格
    Public Property ItemID As Integer
    Public Property SalePrice As Decimal
    Public Property SaleManagerPrice As Decimal
    Public Property PurchasePrice As Decimal
    Public Property PurchaseManagerPrice As Decimal
    Public Property CurrencyItemID As Integer
    Class DataGrid
        <ColumnDefine("ID", ColumnTypeEnum.Text, 0, True)> Public Shared Function Key(ByVal ID As Integer) As Integer
            Return ID
        End Function
        <ColumnDefine("ID", ColumnTypeEnum.Text, 60, True)> Public Shared Function _0(ByVal ID As Integer) As String
            Return ID.ToString
        End Function
        <ColumnDefine("SalePrice", ColumnTypeEnum.Text, 60, True)> Public Shared Function _1(ByVal SalePrice As Decimal) As String
            Return SalePrice.ToString
        End Function
        <ColumnDefine("Content", ColumnTypeEnum.Text, 60, True)> Public Shared Function _2(ByVal Content As String) As String
            Return Content
        End Function
        <ColumnDefine("Executor", ColumnTypeEnum.Text, 60, True)> Public Shared Function _3(ByVal Executor As Staff) As String
            Return Executor.UserName
        End Function
        <ColumnDefine("Valid", ColumnTypeEnum.Text, 60, True)> Public Shared Function _4(ByVal Valid As Boolean) As String
            Return Valid.ToString
        End Function
        <ColumnDefine("Finished", ColumnTypeEnum.Text, 60, True)> Public Shared Function _5(ByVal Finished As Boolean) As String
            Return Finished.ToString
        End Function
        <ColumnDefine("RemindTime", ColumnTypeEnum.Text, 120, True)> Public Shared Function _6(ByVal RemindTime As DateTime) As String
            Return RemindTime.ToString
        End Function
    End Class
    Public Overrides Function GetID() As Integer
        Return ItemID
    End Function

    Public Overrides Sub SetID(ByVal Value As Integer)
        ItemID = Value
    End Sub

    Public Overrides Function CloneCurrentVersion() As InfoBase
        Dim t As Price = Clone(Me)
        t.OldVersions.Clear()
        Return t
    End Function
End Class

<Serializable()> Public Enum StaffGroups As Integer
    ADMIN
    DIRECTOR
    GENERALMANAGER
    HUMANRESOURCE
    SALE
    SALEDEBT
    SALEMANAGER
    PURCHASE
    PURCHASEMANAGER
    DELIVERY
    ACCOUNTANT
End Enum

<Serializable()> Public Enum UserPermission As Integer
    Login
    Logoff
    UserAccountView '所有用户的查看
    UserAccountEdit '所有用户权限的编辑
    PermissionView
    PermissionEdit
    CustomerView
    CustomerEdit
    CustomerSafeView
    CustomerSafeEdit
    SaleCustomerView
    PurchaseCustomerView
    SaleCustomerEdit
    PurchaseCustomerEdit
    SaleCustomerSafeView
    PurchaseCustomerSafeView
    SaleCustomerSafeEdit
    PurchaseCustomerSafeEdit
    SelfTradeView '查看自己的交易 
    SelfTradeEdit '编辑自己的交易
    TradeView '查看所有人的交易
    TradeEdit '编辑所有人的交易状态
    ItemView '查看完整商品目录和统计信息
    ItemEdit '编辑完整商品目录
    InventorySale '
    InventorySaleManager
    InventorySaleManagerUpdate
    InventoryPurchase
    InventoryPurchaseManager
    InventoryPurchaseManagerUpdate
    AccountingView
    DebtCollectView
    SaleDeliveryView
    SaleDeliveryEdit
    PurchaseDeliveryView
    PurchaseDeliveryEdit
    OrderSaleView
    OrderSaleEdit
    OrderSaleNew
    OrderPurchaseView
    OrderPurchaseEdit
    OrderPurchaseNew
    CompanyView
    CompanyEdit
    GoodsSaleView
    GoodsSaleEdit
    GoodsPurchaseView
    GoodsPurchaseEdit
    ServiceSaleView
    ServiceSaleEdit
    ServicePurchaseView
    ServicePurchaseEdit
    CurrencyView
    CurrencyEdit
    TaxView
    TaxEdit
    TaskView
    TaskEdit
    MyTaskView
    MyTaskEdit
    HumanResourceView
    HumanResourceEdit
    DeliveryView
    DeliveryEdit
    MyOrderView
    MyOrderEdit
    MyUnpaidOrderView
    MyUnpaidOrderEdit
    UnpaidOrderView
    UnpaidOrderEdit
    DatabaseManagement
    Refusal
End Enum

<Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> Public Class StaffOperation
    Public Sub New()

    End Sub
    Public Sub New(ByVal vID As Integer, ByVal vUserName As String, ByVal vOperation As UserPermission, ByVal vDescription As String, ByVal vIdentity As String, ByVal vTime As DateTime)
        ID = vID
        UserName = vUserName
        Operation = vOperation
        Description = vDescription
        Identity = vIdentity
        Time = vTime
    End Sub
    <System.ComponentModel.Category("Operation")> Public Property ID As Integer
    <System.ComponentModel.Category("Operation")> Public Property UserID As Integer
    <System.ComponentModel.Category("Operation")> Public Property UserName As String
    <System.ComponentModel.Category("Operation")> Public Property Operation As UserPermission
    <System.ComponentModel.Category("Operation")> Public Property Description As String
    <System.ComponentModel.Category("Operation")> Public Property Identity As StaffGroups
    <System.ComponentModel.Category("Operation")> Public Property Time As DateTime

    Public Overrides Function ToString() As String
        Return DescribeObject(Me)
    End Function
    Class DataGrid
        <ColumnDefine("ID", ColumnTypeEnum.Text, DataAccessLevel.ReadOnly, True)> Public Shared Function Key(ByVal ID As Integer) As Integer
            Return ID
        End Function
        <ColumnDefine("UserID", ColumnTypeEnum.Text, 60, True)> Public Shared Function _0(ByVal UserID As Integer) As Integer
            Return UserID
        End Function
        <ColumnDefine("UserName", ColumnTypeEnum.Text, 120, True)> Public Shared Function _1(ByVal UserName As String) As String
            Return UserName
        End Function
        <ColumnDefine("Operation", ColumnTypeEnum.Text, 120, True)> Public Shared Function _2(ByVal Operation As UserPermission) As String
            Return [Enum].GetName(GetType(UserPermission), Operation)
        End Function
        <ColumnDefine("Description", ColumnTypeEnum.Text, 120, True)> Public Shared Function _3(ByVal Description As String) As String
            Return Description
        End Function
        <ColumnDefine("Identity", ColumnTypeEnum.Text, 120, True)> Public Shared Function _4(ByVal Identity As StaffGroups) As String
            Return [Enum].GetName(GetType(StaffGroups), Identity)
        End Function
        <ColumnDefine("Time", ColumnTypeEnum.Text, 120, True)> Public Shared Function _5(ByVal Time As DateTime) As String
            Return Time.ToString
        End Function
    End Class
End Class

<Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> Public Class GroupPermission
    Inherits EditBase
    <System.ComponentModel.Category("Permission"), [ReadOnly](True)> Public Property Group As StaffGroups
    <System.ComponentModel.Category("Permission")> Public Property Permissions As New List(Of UserPermission)
    Public Sub New()
        '用来给序列化提供的初始化函数
    End Sub
    Public Sub New(ByVal vGroup As StaffGroups, ByVal vPermissions As List(Of UserPermission))
        Group = vGroup
        Permissions.AddRange(vPermissions)
    End Sub
    Public Function Contains(ByVal vPermission As UserPermission) As Boolean
        Return Permissions.Contains(vPermission)
    End Function
    Class DataGrid
        <ColumnDefine("Group", ColumnTypeEnum.Text, DataAccessLevel.ReadWriteNoAddNoDelete, True)> Public Shared Function Key(ByVal Group As Integer) As Integer
            Return Group
        End Function
        <ColumnDefine("Group", ColumnTypeEnum.Text, 60, True)> Public Shared Function _0(ByVal Group As StaffGroups) As String
            Return [Enum].GetName(GetType(StaffGroups), Group)
        End Function
        <ColumnDefine("UserName", ColumnTypeEnum.Text, 120, True)> Public Shared Function _1(ByVal Permissions As List(Of UserPermission)) As String
            Dim stb As New System.Text.StringBuilder
            For Each up As UserPermission In Permissions
                stb.Append([Enum].GetName(GetType(UserPermission), up))
                stb.Append(" ")
            Next
            Return stb.ToString
        End Function
    End Class

    Public Overrides Function GetID() As Integer
        Return Group
    End Function
    Public Overrides Sub SetID(ByVal Value As Integer)
        Group = Value
    End Sub
    Public Overrides Function ToString() As String
        Return DescribeObject(Me)
    End Function
End Class


 