Public Interface ISerializer
    Function Serialize(Value As Object) As Byte()
    Function Deserialize(Bytes As Byte()) As Object
    Function Deserialize(Of T)(Bytes As Byte()) As T
End Interface

Public Class Serializer
    Public PropertyID As String
    Public FieldType As FieldTypeEnum
    Public [DepedencyProperty] As DependencyProperty
    Public [FieldInfo] As System.Reflection.FieldInfo
    Public [PropertyInfo] As System.Reflection.PropertyInfo
    Public Setter As Action(Of Object, Object)
    Public Getter As Func(Of Object, Object)
    Public SerializationType As SerializationTypeEnum
    Public PropertyType As Type = GetType(Object)
    Public LateIndex As Integer
    Public BinderID As String
    Public ClassType As Type
    Private Shared EmptyArray As Object() = New Object() {}
    Public Overridable Property Value(obj As Object) As Object
        Get
            Select Case FieldType
                Case FieldTypeEnum.DependencyProperty
                    Dim dep As DependencyObject = obj
                    Return dep.GetValue([DepedencyProperty])
                Case FieldTypeEnum.Field
                    Return FieldInfo.GetValue(obj)
                Case FieldTypeEnum.Property
                    Return PropertyInfo.GetValue(obj, EmptyArray)
                Case FieldTypeEnum.Direct
                    Return Getter.Invoke(obj)
                Case Else
                    Return Nothing
            End Select
        End Get
        Set(newValue As Object)
            Select Case FieldType
                Case FieldTypeEnum.DependencyProperty
                    Dim dep As DependencyObject = obj
                    dep.SetValue([DepedencyProperty], newValue)
                Case FieldTypeEnum.Field
                    FieldInfo.SetValue(obj, newValue)
                Case FieldTypeEnum.Property
                    PropertyInfo.SetValue(obj, newValue, EmptyArray)
                Case FieldTypeEnum.Direct
                    Setter.Invoke(obj, newValue)
                Case Else
            End Select
        End Set
    End Property
    Public Shared Function ParsePropertyID(dp As DependencyProperty) As String
        'Return dp.OwnerType.AssemblyQualifiedName + vbTab + dp.Name
        Return dp.Name
    End Function
    Public Shared Function ParsePropertyID(prop As String) As String
        'Return "Property:" + prop
        Return prop
    End Function
    Public Shared Function ParseFieldID(fld As String) As String
        'Return "Field:" + fld
        Return fld
    End Function
    Public Shared Function ParseDirectID(drt As String) As String
        'Return "Direct:" + drt
        Return drt
    End Function
    Public Shared Function Save(dp As DependencyProperty, target As Type) As Serializer
        Dim s As New Serializer With {.[DepedencyProperty] = dp, .PropertyID = ParsePropertyID(dp),
                                      .SerializationType = SerializationTypeEnum.Save, .FieldType = FieldTypeEnum.DependencyProperty, .PropertyType = dp.PropertyType}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(dp As DependencyProperty, target As Type, Index As Integer) As Serializer
        Dim s As New Serializer With {.[DepedencyProperty] = dp, .PropertyID = ParsePropertyID(dp),
                                      .SerializationType = SerializationTypeEnum.Late, .LateIndex = Index, .FieldType = FieldTypeEnum.DependencyProperty, .PropertyType = dp.PropertyType}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(dp As DependencyProperty, target As Type, BinderID As String) As Serializer
        Dim s As New Serializer With {.[DepedencyProperty] = dp, .PropertyID = ParsePropertyID(dp),
                                      .SerializationType = SerializationTypeEnum.Early, .BinderID = BinderID, .FieldType = FieldTypeEnum.DependencyProperty, .PropertyType = dp.PropertyType}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(pi As System.Reflection.PropertyInfo) As Serializer
        Dim s As New Serializer With {.PropertyInfo = pi, .PropertyID = ParsePropertyID(pi.Name),
                              .SerializationType = SerializationTypeEnum.Save, .FieldType = FieldTypeEnum.Property, .PropertyType = pi.PropertyType}
        SerializationMap.RegisterSerializer(pi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(pi As System.Reflection.PropertyInfo, Index As Integer) As Serializer
        Dim s As New Serializer With {.PropertyInfo = pi, .PropertyID = ParsePropertyID(pi.Name),
                              .SerializationType = SerializationTypeEnum.Late, .LateIndex = Index, .FieldType = FieldTypeEnum.Property, .PropertyType = pi.PropertyType}
        SerializationMap.RegisterSerializer(pi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(pi As System.Reflection.PropertyInfo, BinderID As String) As Serializer
        Dim s As New Serializer With {.PropertyInfo = pi, .PropertyID = ParsePropertyID(pi.Name),
                                      .SerializationType = SerializationTypeEnum.Early, .BinderID = BinderID, .FieldType = FieldTypeEnum.Property, .PropertyType = pi.PropertyType}
        SerializationMap.RegisterSerializer(pi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(fi As System.Reflection.FieldInfo) As Serializer
        Dim s As New Serializer With {.FieldInfo = fi, .PropertyID = ParseFieldID(fi.Name),
                              .SerializationType = SerializationTypeEnum.Save, .FieldType = FieldTypeEnum.Property, .PropertyType = fi.FieldType}
        SerializationMap.RegisterSerializer(fi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(fi As System.Reflection.FieldInfo, Index As Integer) As Serializer
        Dim s As New Serializer With {.FieldInfo = fi, .PropertyID = ParseFieldID(fi.Name),
                              .SerializationType = SerializationTypeEnum.Late, .LateIndex = Index, .FieldType = FieldTypeEnum.Property, .PropertyType = fi.FieldType}
        SerializationMap.RegisterSerializer(fi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(fi As System.Reflection.FieldInfo, BinderID As String) As Serializer
        Dim s As New Serializer With {.FieldInfo = fi, .PropertyID = ParseFieldID(fi.Name),
                                      .SerializationType = SerializationTypeEnum.Early, .BinderID = BinderID, .FieldType = FieldTypeEnum.Property, .PropertyType = fi.FieldType}
        SerializationMap.RegisterSerializer(fi.DeclaringType, s)
        Return s
    End Function
    Public Shared Function Save(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object), Optional TargetMemberType As Type = Nothing) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                              .SerializationType = SerializationTypeEnum.Save, .FieldType = FieldTypeEnum.Direct, .PropertyType = IIf(TargetMemberType Is Nothing, GetType(Object), TargetMemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object), Index As Integer, Optional TargetMemberType As Type = Nothing) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                              .SerializationType = SerializationTypeEnum.Late, .LateIndex = Index, .FieldType = FieldTypeEnum.Direct, .PropertyType = IIf(TargetMemberType Is Nothing, GetType(Object), TargetMemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object), BinderID As String, Optional TargetMemberType As Type = Nothing) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                                      .SerializationType = SerializationTypeEnum.Early, .BinderID = BinderID, .FieldType = FieldTypeEnum.Direct, .PropertyType = IIf(TargetMemberType Is Nothing, GetType(Object), TargetMemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(Of MemberType)(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object)) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                              .SerializationType = SerializationTypeEnum.Save, .FieldType = FieldTypeEnum.Direct, .PropertyType = GetType(MemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(Of MemberType)(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object), Index As Integer) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                              .SerializationType = SerializationTypeEnum.Late, .LateIndex = Index, .FieldType = FieldTypeEnum.Direct, .PropertyType = GetType(MemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function
    Public Shared Function Save(Of MemberType)(Name As String, target As Type, st As Action(Of Object, Object), gt As Func(Of Object, Object), BinderID As String) As Serializer
        Dim s As New Serializer With {.Setter = st, .Getter = gt, .PropertyID = ParseDirectID(Name),
                                      .SerializationType = SerializationTypeEnum.Early, .BinderID = BinderID, .FieldType = FieldTypeEnum.Direct, .PropertyType = GetType(MemberType)}
        SerializationMap.RegisterSerializer(target, s)
        Return s
    End Function

    Public IsEnum As Boolean = False
    Public IsNullable As Boolean = False
    Public EnumType As Type
    Public Function ToData(value As Object) As Object
        If IsEnum Then
            If IsNullable Then
                If value Is Nothing Then
                    Return Nothing
                Else
                    Return ParseEnumFlags(value.ToString)
                End If
            Else
                If value Is Nothing Then
                    Return [Enum].GetValues(EnumType)(0).ToString()
                Else
                    Return ParseEnumFlags(value.ToString)
                End If
            End If
        Else
            Return value
        End If
    End Function
    Private Shared Function ParseEnumFlags(value As String) As String
        Dim values As String() = System.Text.RegularExpressions.Regex.Split(value, "[,;\s]+")
        Dim vList As New List(Of String)
        For Each s In values
            If s.Length > 0 Then
                vList.Add(s)
            End If
        Next
        Return String.Join(",", vList.ToArray)
    End Function
    Public Function ToValue(value As Object) As Object
        If IsEnum Then
            If IsNullable Then
                If value Is Nothing Then
                    Return Nothing
                Else
                    Return [Enum].Parse(EnumType, value)
                End If
            Else
                If value Is Nothing Then
                    Return [Enum].GetValues(EnumType)(0)
                Else
                    Return [Enum].Parse(EnumType, value)
                End If
            End If
        Else
            Return value
        End If
    End Function
    Public Function EnumRegister(vIsEnum As Boolean, vEnumType As Type, vIsNullable As Boolean) As Serializer
        IsEnum = vIsEnum
        EnumType = vEnumType
        IsNullable = vIsNullable
        Return Me
    End Function
    Public ForceUseExistingInstanceOnDeserialization As Boolean = False
    Public Function ForceUseExistingInstance(ForceExisting As Boolean) As Serializer
        ForceUseExistingInstanceOnDeserialization = ForceExisting
        Return Me
    End Function
    Public FieldModel As FieldModelEnum
    Public FieldName As String
    Public SourceEntity As String
    Public TableName As String
    Public IsKey As Boolean
    Public IsSummary As Boolean
    Public AutoIncrement As Boolean
    Private _DataNavigator As Func(Of Object, IDataNavigator)
    Public ReadOnly Property DataNavigator(obj As Object) As IDataNavigator
        Get
            If _DataNavigator IsNot Nothing Then
                Return _DataNavigator.Invoke(obj)
            End If
            Return Nothing
        End Get
    End Property
    Public Function Data(vTableName As String, vFieldName As String, vFieldModel As FieldModelEnum, vIsKey As Boolean, vIsSummary As Boolean, vAutoIncrement As Boolean, Optional Navigator As Func(Of Object, IDataNavigator) = Nothing) As Serializer
        TableName = vTableName
        'SourceEntity = vSourceEntity
        FieldModel = vFieldModel
        FieldName = vFieldName
        IsKey = vIsKey
        IsSummary = vIsSummary
        AutoIncrement = vAutoIncrement
        _DataNavigator = Navigator
        SerializationMap.RegisterData(Me)
        Return Me
    End Function
    Public RelatedType As Type
    Public RelatedField As String
    Public NavigationTable As String
    Public Function Relation(vRelatedType As Type, vFieldName As String, TableName As String) As Serializer
        RelatedType = vRelatedType
        RelatedField = vFieldName
        NavigationTable = TableName
        Return Me
    End Function
    Private AssociationTree As TypeManager
    'Public Function Associate(vAssociatedType As Type, vAssociatedField As String) As Serializer
    '    AssociatedType = vAssociatedType
    '    AssociatedField = vAssociatedField
    '    Return Me
    'End Function
    Public Associated As Boolean = False
    Public Function Associate(Of TAssociation As Class)(vAssociatedField As String) As Serializer
        If AssociationTree Is Nothing Then
            AssociationTree = New TypeManager
            AssociationTree.Add(Of Object)("")
            Associated = True
        End If
        AssociationTree.Add(Of TAssociation)(vAssociatedField)
        'AssociatedField = vAssociatedField
        Return Me
    End Function
    Public Function AssignFrom(Target As Object, [From] As Object) As Boolean
        Dim tt As TypeTree(Of String) = AssociationTree.MatchField([From])
        If tt IsNot Nothing Then
            If IDataNavigatorType.IsAssignableFrom(PropertyType) Then
                If IDataNavigatorType.IsAssignableFrom(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).PropertyType) Then
                    Try
                        DirectCast(Value(Target), IDataNavigator).Value = DirectCast(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([From]), IDataNavigator).Value
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                Else
                    Try
                        DirectCast(Value(Target), IDataNavigator).Value = SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([From])
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            Else
                If IDataNavigatorType.IsAssignableFrom(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).PropertyType) Then
                    Try
                        Value(Target) = DirectCast(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([From]), IDataNavigator).Value
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                Else
                    Try
                        Value(Target) = SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([From])
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            End If
        End If
        Return False
    End Function
    Private Shared IDataNavigatorType As Type = GetType(IDataNavigator)
    Public Function AssignTo(Target As Object, [To] As Object) As Boolean
        Dim tt As TypeTree(Of String) = AssociationTree.MatchField([To])
        If tt IsNot Nothing Then
            If IDataNavigatorType.IsAssignableFrom(PropertyType) Then
                If IDataNavigatorType.IsAssignableFrom(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).PropertyType) Then
                    Try
                        DirectCast(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([To]), IDataNavigator).Value = DirectCast(Value(Target), IDataNavigator).Value
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                Else
                    Try
                        SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([To]) = DirectCast(Value(Target), IDataNavigator).Value
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            Else
                If IDataNavigatorType.IsAssignableFrom(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).PropertyType) Then
                    Try
                        DirectCast(SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([To]), IDataNavigator).Value = Value(Target)
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                Else
                    Try
                        SerializationMap.GetMap(tt.Type)(tt.AssociatedField).Value([To]) = Value(Target)
                        Return True
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            End If
        End If
        Return False
    End Function
End Class
