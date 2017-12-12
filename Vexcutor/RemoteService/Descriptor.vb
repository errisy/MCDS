Imports System.ComponentModel, System.Reflection
Public Class QueryDescriptionProvider
    Inherits TypeDescriptionProvider
    Public Overrides Function GetTypeDescriptor(ByVal objectType As System.Type, ByVal instance As Object) As System.ComponentModel.ICustomTypeDescriptor
        Return New QueryDescriptor(objectType, instance)
    End Function
    Public Overrides Function IsSupportedType(ByVal type As System.Type) As Boolean
        Return True
    End Function
End Class
Public Class QueryDescriptor
    Inherits CustomTypeDescriptor
    Public Sub New()
        MyBase.New()
    End Sub
    Private vType As Type
    Private vInstance As Object
    Sub New(ByVal objectType As Type, ByVal instance As Object)
        ' TODO: Complete member initialization 
        vType = objectType
        vInstance = instance
    End Sub
    Public Overrides Function GetProperties(ByVal attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection
        Dim eb As EditBase = Nothing
        If TypeOf vInstance Is EditBase Then
            eb = vInstance
        End If
        Dim pz As New PropertyDescriptorCollection(Nothing)
        Dim pc As QueryPropertyDescriptor
        Dim attList As New List(Of Attribute)
        For Each pd As PropertyInfo In vType.GetProperties()
            attList.Clear()
            For Each obj As Object In pd.GetCustomAttributes(True)
                If TypeOf obj Is Attribute Then attList.Add(obj)
            Next
            pc = New QueryPropertyDescriptor(pd.Name, attList.ToArray, eb)
            pz.Add(pc)
        Next
        Return pz
    End Function
    Public Overrides Function GetProperties() As System.ComponentModel.PropertyDescriptorCollection
        Dim eb As EditBase = Nothing
        If TypeOf vInstance Is EditBase Then
            eb = vInstance
        End If
        Dim pz As New PropertyDescriptorCollection(Nothing)
        Dim pc As QueryPropertyDescriptor
        Dim attList As New List(Of Attribute)
        For Each pd As PropertyInfo In vType.GetProperties()
            attList.Clear()
            For Each obj As Object In pd.GetCustomAttributes(True)
                If TypeOf obj Is Attribute Then attList.Add(obj)
            Next
            pc = New QueryPropertyDescriptor(pd.Name, attList.ToArray, eb)
            pz.Add(pc)
        Next
        Return pz
    End Function

End Class
Public Class QueryPropertyDescriptor
    Inherits PropertyDescriptor
    Private instance As EditBase
    Private vPropertyInfo As PropertyInfo
    Public Sub New(ByVal Name As String, ByVal Attr As Attribute(), Optional ByVal vInstance As EditBase = Nothing)
        MyBase.New(Name, Attr)
        DefaultAttributes = Attr
        instance = vInstance
        vPropertyInfo = instance.GetType.GetProperty(Name)
    End Sub
    Public DefaultAttributes As Attribute()
    Public PropertyCollection As PropertyDescriptorCollection
    Public Index As Integer
    Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
        Return True
    End Function
    Public Overrides Function GetEditor(ByVal editorBaseType As System.Type) As Object
        Return MyBase.GetEditor(editorBaseType)
    End Function
    Public Overrides ReadOnly Property ComponentType As System.Type
        Get
            Return instance.GetType
        End Get
    End Property

    Public Overrides Function GetValue(ByVal component As Object) As Object
        Dim Value As Object
        If vPropertyInfo.PropertyType Is GetType(String) Then
            Value = vPropertyInfo.GetValue(instance, New Object() {})
            If Value Is Nothing Then Value = ""
        Else
            Value = vPropertyInfo.GetValue(instance, New Object() {})
        End If
        Return Value
    End Function
    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As System.Type
        Get
            Return vPropertyInfo.PropertyType
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)

    End Sub

    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)

    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return True
    End Function
    Public Overrides ReadOnly Property Converter As System.ComponentModel.TypeConverter
        Get
            Return New QueryListConverter(Name, instance)
        End Get
    End Property
    Public Overrides ReadOnly Property Name As String
        Get
            Return MyBase.Name
        End Get
    End Property
    Public Overrides ReadOnly Property DisplayName As String
        Get
            Return MyBase.DisplayName
        End Get
    End Property
End Class

Public Class QueryListConverter
    Inherits System.ComponentModel.ExpandableObjectConverter
    Public Query As ValueQuery
    Private name As String
    Private instance As EditBase
    Private vPropertyInfo As PropertyInfo
    Public Sub New(ByVal vName As String, ByVal vInstance As EditBase)
        name = vName
        instance = vInstance
        vPropertyInfo = instance.GetType.GetProperty(name)
    End Sub
    Public Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean
        If sourceType Is GetType(String) Then
            Return True
        Else
            Return MyBase.CanConvertFrom(context, sourceType)
        End If
        'Return MyBase.CanConvertFrom(context, sourceType)
    End Function
    Private Sub ReadDigitalValue(Of T)(ByVal Value As String, ByVal Converter As Func(Of String, T))
        Dim str As String = Value
        str = str.Replace(" ", "")
        If str.StartsWith(">") Then
            If instance.Query.ContainsKey(name) Then
                Try
                    instance.Query(name).QueryType = ValueQueryType.GreaterThan
                    instance.Query(name).Value = Converter.Invoke(str.Substring(1))
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim vq As New ValueQuery(Of T) With {.PropertyName = name, .QueryType = ValueQueryType.GreaterThan, .Value = Converter.Invoke(str.Substring(1))}
                    instance.Query.Add(name, vq)
                Catch ex As Exception

                End Try
            End If
        ElseIf str.StartsWith("<") Then
            If instance.Query.ContainsKey(name) Then
                Try
                    instance.Query(name).QueryType = ValueQueryType.LessThan
                    instance.Query(name).Value = Converter.Invoke(str.Substring(1))
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim vq As New ValueQuery(Of T) With {.PropertyName = name, .QueryType = ValueQueryType.LessThan, .Value = Converter.Invoke(str.Substring(1))}
                    instance.Query.Add(name, vq)
                Catch ex As Exception

                End Try
            End If
        ElseIf str.StartsWith("=") Then
            If instance.Query.ContainsKey(name) Then
                Try
                    instance.Query(name).QueryType = ValueQueryType.EqualTo
                    instance.Query(name).Value = Converter.Invoke(str.Substring(1))
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim vq As New ValueQuery(Of T) With {.PropertyName = name, .QueryType = ValueQueryType.EqualTo, .Value = Converter.Invoke(str.Substring(1))}
                    instance.Query.Add(name, vq)
                Catch ex As Exception

                End Try
            End If
        ElseIf str.StartsWith("\") Then
            If instance.Query.ContainsKey(name) Then
                Try
                    instance.Query(name).QueryType = ValueQueryType.NotEqualTo
                    instance.Query(name).Value = Converter.Invoke(str.Substring(1))
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim vq As New ValueQuery(Of T) With {.PropertyName = name, .QueryType = ValueQueryType.NotEqualTo, .Value = Converter.Invoke(str.Substring(1))}
                    instance.Query.Add(name, vq)
                Catch ex As Exception

                End Try
            End If
        ElseIf str.ToLower.StartsWith("-") Then
            If instance.Query.ContainsKey(name) Then
                Try
                    instance.Query(name).QueryType = ValueQueryType.Contains
                    instance.Query(name).Value = Converter.Invoke(str.Substring(1))
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim vq As New ValueQuery(Of T) With {.PropertyName = name, .QueryType = ValueQueryType.Contains, .Value = Converter.Invoke(str.Substring(1))}
                    instance.Query.Add(name, vq)
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
        If TypeOf value Is String Then
            Dim str As String = value
            If vPropertyInfo.PropertyType Is GetType(String) Then
                If str.ToLower.StartsWith("- ") Then
                    If instance.Query.ContainsKey(name) Then
                        Try
                            instance.Query(name).QueryType = ValueQueryType.Contains
                            instance.Query(name).Value = str.Substring(2)
                        Catch ex As Exception

                        End Try
                    Else
                        Try
                            Dim vq As New ValueQuery(Of String) With {.PropertyName = name, .QueryType = ValueQueryType.Contains, .Value = str.Substring(2)}
                            instance.Query.Add(name, vq)
                        Catch ex As Exception

                        End Try
                    End If
                Else
                    If instance.Query.ContainsKey(name) Then
                        Try
                            instance.Query(name).QueryType = ValueQueryType.Contains
                            instance.Query(name).Value = str
                        Catch ex As Exception

                        End Try
                    Else
                        Try
                            Dim vq As New ValueQuery(Of String) With {.PropertyName = name, .QueryType = ValueQueryType.Contains, .Value = str}
                            instance.Query.Add(name, vq)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            ElseIf vPropertyInfo.PropertyType Is GetType(Integer) Then
                Dim F = Function(v As String) As Integer
                            Return CInt(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(Long) Then
                Dim F = Function(v As String) As Long
                            Return CLng(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(Single) Then
                Dim F = Function(v As String) As Single
                            Return CSng(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(Double) Then
                Dim F = Function(v As String) As Double
                            Return CDbl(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(Decimal) Then
                Dim F = Function(v As String) As Decimal
                            Return CDec(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(Date) Then
                Dim F = Function(v As String) As Date
                            Return CDate(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType.IsEnum Then
                Dim F = Function(v As String) As Integer
                            Return CInt(v)
                        End Function
                ReadDigitalValue(str, F)
            ElseIf vPropertyInfo.PropertyType Is GetType(List(Of String)) Then
                If str.ToLower.StartsWith("- ") Then
                    If instance.Query.ContainsKey(name) Then
                        Try
                            instance.Query(name).QueryType = ValueQueryType.Contains
                            instance.Query(name).Value = str.Substring(2)
                        Catch ex As Exception

                        End Try
                    Else
                        Try
                            Dim vq As New ValueQuery(Of String) With {.PropertyName = name, .QueryType = ValueQueryType.Contains, .Value = str.Substring(2)}
                            instance.Query.Add(name, vq)
                        Catch ex As Exception

                        End Try
                    End If
                Else
                    If instance.Query.ContainsKey(name) Then
                        Try
                            instance.Query(name).QueryType = ValueQueryType.Contains
                            instance.Query(name).Value = str
                        Catch ex As Exception

                        End Try
                    Else
                        Try
                            Dim vq As New ValueQuery(Of String) With {.PropertyName = name, .QueryType = ValueQueryType.Contains, .Value = str}
                            instance.Query.Add(name, vq)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            End If
        End If
        Return value
        'Return MyBase.ConvertFrom(context, culture, value)
    End Function
    Public Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean
        Return MyBase.CanConvertTo(context, destinationType)
    End Function
    Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
        Dim NA As Boolean = False

        If TypeOf value Is Integer Then
            NA = True
        ElseIf TypeOf value Is Long Then
            NA = True
        ElseIf TypeOf value Is Single Then
            NA = True
        ElseIf TypeOf value Is Double Then
            NA = True
        ElseIf TypeOf value Is Decimal Then
            NA = True
        ElseIf TypeOf value Is Date Then
            NA = True
        ElseIf vPropertyInfo.PropertyType Is GetType(String) Then
            NA = True
        ElseIf TypeOf value Is [Enum] Then
            NA = True
        ElseIf vPropertyInfo.PropertyType Is GetType(List(Of String)) Then
            NA = True
        End If

        If NA AndAlso instance.Query.ContainsKey(name) AndAlso instance.Query(name).QueryType <> ValueQueryType.NotAppliable Then
            Dim q As ValueQuery = instance.Query(name)
            If vPropertyInfo.PropertyType Is GetType(String) AndAlso q.Value Is Nothing Then
                q.Value = ""
                Select Case q.QueryType
                    Case ValueQueryType.LessThan, ValueQueryType.GreaterThan
                        q.QueryType = ValueQueryType.Contains
                End Select
            End If
            Select Case q.QueryType
                Case ValueQueryType.GreaterThan
                    Return "> " + q.Value.ToString
                Case ValueQueryType.LessThan
                    Return "< " + q.Value.ToString
                Case ValueQueryType.EqualTo
                    Return "= " + q.Value.ToString
                Case ValueQueryType.NotEqualTo
                    Return "\ " + q.Value.ToString
                Case ValueQueryType.Contains
                    Return "- " + q.Value.ToString
            End Select
        Else
            Return "N/A"
        End If
        Return "N/A"
    End Function

    Public Overrides Function CreateInstance(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal propertyValues As System.Collections.IDictionary) As Object
        Return MyBase.CreateInstance(context, propertyValues)
    End Function
    Public Overrides Function GetProperties(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal value As Object, ByVal attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection
        Dim PDC As New PropertyDescriptorCollection(Nothing)
        If TypeOf value Is IList Then
            Dim vList As IList = value
            Dim T As Type = value.GetType
            Dim iT As Type() = T.GetGenericArguments()
            If OfType(iT(0), GetType(EditBase)) Then
                If Not instance.Query.ContainsKey(name) Then instance.Query.Add(name, New ValueQuery(Of Object) With {.PropertyName = name})
                If vList.Count = 0 Then
                    Dim obj As Object = iT(0).GetConstructor(New Type() {}).Invoke(New Object() {})
                    vList.Add(obj)
                    For Each pi As System.Reflection.PropertyInfo In iT(0).GetProperties
                        Dim QPD As New QueryPropertyDescriptor(pi.Name, attributes, obj)
                        PDC.Add(QPD)
                    Next
                Else
                    Dim obj As Object = vList(0)
                    For Each pi As System.Reflection.PropertyInfo In iT(0).GetProperties
                        Dim QPD As New QueryPropertyDescriptor(pi.Name, attributes, obj)
                        PDC.Add(QPD)
                    Next
                End If
                Return PDC
            End If
        ElseIf OfType(vPropertyInfo.PropertyType, GetType(EditBase)) Then
            If Not instance.Query.ContainsKey(name) Then instance.Query.Add(name, New ValueQuery(Of Object) With {.PropertyName = name})
            If value Is Nothing Then
                Dim obj As Object = vPropertyInfo.PropertyType.GetConstructor(New Type() {}).Invoke(New Object() {})
                vPropertyInfo.SetValue(instance, obj, New Object() {})
                For Each pi As System.Reflection.PropertyInfo In vPropertyInfo.PropertyType.GetProperties
                    Dim QPD As New QueryPropertyDescriptor(pi.Name, attributes, obj)
                    PDC.Add(QPD)
                Next
            Else
                For Each pi As System.Reflection.PropertyInfo In vPropertyInfo.PropertyType.GetProperties
                    Dim QPD As New QueryPropertyDescriptor(pi.Name, attributes, value)
                    PDC.Add(QPD)
                Next
            End If
            Return PDC
        ElseIf Not (instance Is Nothing) Then
            If instance.Query.ContainsKey(name) Then
                'read the query data
                Query = instance.Query(name)
                Dim attList As New List(Of Attribute)
                PDC.Add(New QueryTypeDescriptor("Compare Method", attList.ToArray, Query))
                PDC.Add(New CriterionDescriptor("Criterion Value", attList.ToArray, Query))
            Else
                'add the query data
                Dim vType As Type = Nothing

                If TypeOf value Is Integer Then
                    Query = New ValueQuery(Of Integer) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is Long Then
                    Query = New ValueQuery(Of Long) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is Single Then
                    Query = New ValueQuery(Of Single) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is Double Then
                    Query = New ValueQuery(Of Double) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is Decimal Then
                    Query = New ValueQuery(Of Decimal) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is Date Then
                    Query = New ValueQuery(Of Date) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = Now}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf vPropertyInfo.PropertyType Is GetType(String) Then
                    Query = New ValueQuery(Of String) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = ""}
                    'PDC = TypeDescriptor.GetProperties(Query)
                ElseIf TypeOf value Is [Enum] Then
                    Query = New ValueQuery(Of Integer) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    vType = value.GetType
                ElseIf vPropertyInfo.PropertyType Is GetType(List(Of String)) Then
                    Query = New ValueQuery(Of Integer) With {.PropertyName = name, .QueryType = ValueQueryType.NotAppliable, .Value = 0}
                    vType = value.GetType
                End If
                Dim attList As New List(Of Attribute)
                If Not (Query Is Nothing) Then
                    PDC.Add(New QueryTypeDescriptor("Compare Method", attList.ToArray, Query))
                    PDC.Add(New CriterionDescriptor("Criterion Value", attList.ToArray, Query, vType))
                    instance.Query.Add(name, Query)
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
<Serializable()> Public MustInherit Class ValueQuery
    Public PropertyName As String
    Public Property QueryType As ValueQueryType
    Public MustOverride Property Value() As Object

End Class
<Serializable()> Public Class ValueQuery(Of T)
    Inherits ValueQuery
    Public Property Criterion As T
    Public Overrides Property Value As Object
        Get
            Return Criterion
        End Get
        Set(ByVal value As Object)
            Criterion = value
        End Set
    End Property
End Class

<Serializable()> Public Enum ValueQueryType As Integer
    NotAppliable
    EqualTo
    LessThan
    GreaterThan
    NotEqualTo
    Contains '以字符串形式进行比较 包含某一个数值或者文本
End Enum

Public Class CriterionDescriptor
    Inherits PropertyDescriptor
    Private Query As ValueQuery
    Private pType As Type
    Public Sub New(ByVal Name As String, ByVal Attr As Attribute(), ByVal vQuery As ValueQuery, Optional ByVal vType As Type = Nothing)
        MyBase.New(Name, Attr)
        vIsReadOnly = False
        Query = vQuery
        pType = vType
    End Sub
    Public DefaultAttributes As Attribute()
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
        If Not (Query Is Nothing) Then
            Dim Value As Object = Query.Value
            If Value Is Nothing Then
                If PropertyType Is GetType(String) Then Value = ""
            End If
            Return Value
        Else
            Return Nothing
        End If
    End Function

    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return vIsReadOnly
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As System.Type
        Get
            If pType Is Nothing Then
                Return Query.GetType.GetGenericArguments(0)
            Else
                Return pType
            End If
        End Get
    End Property
    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        If Not (Query Is Nothing) Then
            Query.Value = value
        End If
    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return True
    End Function
    Public Overrides ReadOnly Property Converter As System.ComponentModel.TypeConverter
        Get
            Return MyBase.Converter
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)

    End Sub
End Class

Public Class QueryTypeDescriptor
    Inherits PropertyDescriptor
    Private Query As ValueQuery
    Public Sub New(ByVal Name As String, ByVal Attr As Attribute(), ByVal vQuery As ValueQuery)
        MyBase.New(Name, Attr)
        vIsReadOnly = False
        Query = vQuery
    End Sub
    Public DefaultAttributes As Attribute()
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
        If Not (Query Is Nothing) Then
            Return Query.QueryType
        Else
            Return Nothing
        End If
    End Function

    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return vIsReadOnly
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As System.Type
        Get
            Return GetType(ValueQueryType)
        End Get
    End Property
    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        If Not (Query Is Nothing) Then
            Query.QueryType = value
        End If
    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return True
    End Function
    Public Overrides ReadOnly Property Converter As System.ComponentModel.TypeConverter
        Get
            Return MyBase.Converter
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)

    End Sub
End Class

Public Class CustomerEditor
    Inherits System.Drawing.Design.UITypeEditor
    Public Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object
        Dim edSvc As System.Windows.Forms.Design.IWindowsFormsEditorService = provider.GetService(GetType(System.Windows.Forms.Design.IWindowsFormsEditorService))

        Return MyBase.EditValue(context, provider, value)
    End Function
    Public Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return System.Drawing.Design.UITypeEditorEditStyle.DropDown
    End Function
    Public Overrides ReadOnly Property IsDropDownResizable As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides Sub PaintValue(ByVal e As System.Drawing.Design.PaintValueEventArgs)
        MyBase.PaintValue(e)

    End Sub
End Class