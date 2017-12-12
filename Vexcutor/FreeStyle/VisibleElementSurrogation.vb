Imports System.Runtime.Serialization, System.Reflection, System.Windows.Controls, System.Windows.Media, System.Windows.Shapes, System.Windows.Input
<Serializable()>
Public NotInheritable Class ShallowObject
    Public Property Name As String
    Public Fields As New Dictionary(Of String, Integer)
    Public Properties As New Dictionary(Of String, Integer)
    Public LateProperties As New Dictionary(Of String, Integer)
    Public List As List(Of Integer)
    Public Dict As Dictionary(Of Integer, Integer)
End Class
<Serializable()>
Public Class ShallowSerializer
    Private i2o As New Dictionary(Of Integer, Object)
    <NonSerialized()> Private o2i As New Dictionary(Of Object, Integer)
    <NonSerialized()> Private s2o As New Dictionary(Of ShallowObject, Object)
    <NonSerialized()> Private o2s As New Dictionary(Of Object, ShallowObject)
    <NonSerialized()> Private iso As New Dictionary(Of Integer, Object)
    Private table As New Dictionary(Of String, Integer)
    Public Sub New()
    End Sub
    Public Sub New(bytes As Byte())
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim ss As ShallowSerializer
        Using mem As New System.IO.MemoryStream(bytes)
            Using gz As New System.IO.Compression.GZipStream(mem, IO.Compression.CompressionMode.Decompress)
                ss = bf.Deserialize(gz)
            End Using
        End Using
        i2o = ss.i2o
        table = ss.table
        For Each id As Integer In ss.i2o.Keys
            If TypeOf ss.i2o(id) Is EarlyBinder Then
                devicemapping.Add(id, DirectCast(ss.i2o(id), EarlyBinder).DeviceID)
            End If
        Next
        UnmappedDevices.AddRange(DeviceMapping.Select(Function(kvp) kvp.Value).ToList())
    End Sub
    <NonSerialized()> Private DeviceMapping As New Dictionary(Of Integer, String)
    Public ReadOnly Property Devices As List(Of String)
        Get
            Return DeviceMapping.Select(Function(kvp) kvp.Value).ToList()
        End Get
    End Property
    Public ReadOnly Property RequiredDevices As List(Of String)
        Get
            Return UnmappedDevices
        End Get
    End Property
    <NonSerialized()> Private UnmappedDevices As New List(Of String)
    Public Sub AddDevice(vID As String, dev As Object)
        If DeviceMapping.ContainsValue(vID) Then
            For Each index As Integer In DeviceMapping.GetKeys(vID)
                i2o.Remove(index)
                i2o.Add(index, dev)
                If UnmappedDevices.Contains(vID) Then UnmappedDevices.Remove(vID)
            Next
        End If
    End Sub
    Public Sub AddObj(key As String, obj As Object)
        If o2i.ContainsKey(obj) Then
            table.Add(key, o2i(obj))
        Else
            'dessemble this object
            Dim i As Integer = Dessemble(obj)
            table.Add(key, i)
        End If
    End Sub

    Private Function AddShallow(obj, shallow) As Integer
        Dim i As Integer = i2o.Count
        o2s.Add(obj, shallow)
        s2o.Add(shallow, obj)
        i2o.Add(i, shallow)
        o2i.Add(shallow, i)
        Return i
    End Function
    Private Function Add(obj) As Integer
        Dim i As Integer = i2o.Count
        i2o.Add(i, obj)
        o2i.Add(obj, i)
        Return i
    End Function
    Private Shared Empty As Object() = New Object() {}
    Private Function Dessemble(obj As Object) As Integer
        If obj Is Nothing Then Return -1
        If o2i.ContainsKey(obj) Then
            Return o2i(obj)
        ElseIf o2s.ContainsKey(obj) Then
            Return o2i(o2s(obj))
        Else
            Dim index As Integer
            Dim vType As Type = obj.GetType
            Dim Shallow As Boolean = False
            For Each att As Attribute In vType.GetCustomAttributes(True)
                If TypeOf att Is ShallowAttribute Then
                    Shallow = True
                End If
            Next
            If Shallow Then
                Dim so As New ShallowObject
                index = AddShallow(obj, so) '这步放在前面是为了防止循环调用
                so.Name = vType.AssemblyQualifiedName
                For Each fi As FieldInfo In vType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    For Each att As Attribute In fi.GetCustomAttributes(True)
                        If TypeOf att Is SaveAttribute Then
                            Dim value As Object
                            value = fi.GetValue(obj)
                            Dim vindex As Integer = Dessemble(value)
                            so.Fields.Add(fi.Name, Dessemble(value))
                        ElseIf TypeOf att Is EarlyBindAttribute Then
                            so.Fields.Add(fi.Name, Dessemble(DirectCast(att, EarlyBindAttribute).Device))
                        End If
                    Next
                Next
                For Each pi As PropertyInfo In vType.GetProperties(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    For Each att As Attribute In pi.GetCustomAttributes(True)
                        If TypeOf att Is SaveAttribute Then
                            If pi.GetIndexParameters.Count = 0 Then
                                Dim value As Object
                                value = pi.GetValue(obj, Empty)
                                Dim vindex As Integer = Dessemble(value)
                                so.Properties.Add(pi.Name, Dessemble(value))
                            End If
                        ElseIf TypeOf att Is LateLoadAttribute Then
                            If pi.GetIndexParameters.Count = 0 Then
                                Dim PropertyName As String = pi.Name
                                Dim value As Object
                                value = pi.GetValue(obj, Empty)
                                Dim vindex As Integer = Dessemble(value)
                                so.LateProperties.Add(PropertyName, vindex)
                            End If
                        ElseIf TypeOf att Is EarlyBindAttribute Then
                            so.Properties.Add(pi.Name, Dessemble(DirectCast(att, EarlyBindAttribute).Device))
                        End If
                    Next
                Next
                'Enumerator
                If vType.IsGenericType Then
                    If vType.GetGenericTypeDefinition.IsAssignableFrom(GetType(ShallowList(Of ))) Then
                        Dim il As IList = obj
                        so.List = New List(Of Integer)
                        For Each v As Object In il
                            so.List.Add(Dessemble(v))
                        Next
                    ElseIf vType.GetGenericTypeDefinition.IsAssignableFrom(GetType(ShallowDictionary(Of ,))) Then
                        Dim id As IDictionary = obj
                        so.Dict = New Dictionary(Of Integer, Integer)
                        For Each k As Object In id.Keys
                            so.Dict.Add(Dessemble(k), Dessemble(id(k)))
                        Next
                    End If
                End If

                Return index
            Else
                Return Add(obj)
            End If
        End If
    End Function
    Public Function GetObject(key As String) As Object
        If UnmappedDevices IsNot Nothing AndAlso UnmappedDevices.Count > 0 Then
            Throw New Exception(String.Format("Unlinked devices detected! Please call AddDevice to map the following devices: {0}.", String.Join(", ", UnmappedDevices.ToArray)))
        End If
        If iso Is Nothing Then iso = New Dictionary(Of Integer, Object)
        If table.ContainsKey(key) Then
            Dim i As Integer = table(key)
            Dim lTasks As New List(Of LateLoader)
            Dim obj = Assemble(i, lTasks)
            lTasks.Sort()
            For Each ll As LateLoader In lTasks
                ll.Loader.Invoke()
            Next
            Return obj
        Else
            Return Nothing
        End If
    End Function
    Private Function Assemble(index As Integer, lateTasks As List(Of LateLoader)) As Object
        If index = -1 Then Return Nothing
        If iso.ContainsKey(index) Then
            Return iso(index)
        ElseIf i2o.ContainsKey(index) Then
            Dim obj As Object = i2o(index)
            If TypeOf obj Is ShallowObject Then
                Dim so As ShallowObject = obj
                so.Name = so.Name.Replace("Vecute", "MCDS").Replace("PublicKeyToken=6ca157640498cbe3", "PublicKeyToken=null")
                Dim vType As Type = Type.GetType(so.Name)
                Dim real As Object = vType.Assembly.CreateInstance(vType.FullName)
                '需要先加载内部内容 否则会出问题
                If so.List IsNot Nothing Then
                    Dim il As IList = real
                    For Each iKey As Integer In so.List
                        il.Add(Assemble(iKey, lateTasks))
                    Next
                ElseIf so.Dict IsNot Nothing Then
                    Dim id As IDictionary = real
                    For Each iKey As Integer In so.Dict.Keys
                        id.Add(Assemble(iKey, lateTasks), Assemble(so.Dict(iKey), lateTasks))
                    Next
                End If
                iso.Add(index, real) '这一步也是为了防止循环调用 所以先返回值然后在
                For Each fs As String In so.Fields.Keys
                    Dim fi As FieldInfo = vType.GetField(fs, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    Dim value As Object = Assemble(so.Fields(fs), lateTasks)
                    fi.SetValue(real, value)
                Next
                For Each fs As String In so.Properties.Keys
                    Dim pi As PropertyInfo = vType.GetProperty(fs, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    Dim value As Object = Assemble(so.Properties(fs), lateTasks)
                    pi.SetValue(real, value, Empty)
                Next
                For Each fs As String In so.LateProperties.Keys
                    Dim pi As PropertyInfo = vType.GetProperty(fs, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    Dim value As Object = Assemble(so.LateProperties(fs), lateTasks)
                    Dim f As System.Action = Sub()
                                                 pi.SetValue(real, value, Empty)
                                             End Sub
                    lateTasks.Add(GetLateLoadAttribute(pi).MakeLoader(f))
                Next
                Return real
            Else
                Return obj
            End If
        Else
            Return Nothing
        End If
    End Function
    Private Shared Function GetLateLoadAttribute(pi As System.Reflection.PropertyInfo) As LateLoadAttribute
        For Each att As Attribute In pi.GetCustomAttributes(True)
            If TypeOf att Is LateLoadAttribute Then Return att
        Next
        Return New LateLoadAttribute
    End Function
    Public Function GetBytes() As Byte()
        Dim bytes As Byte()
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using mem As New System.IO.MemoryStream
            Using gz As New System.IO.Compression.GZipStream(mem, IO.Compression.CompressionMode.Compress)
                bf.Serialize(gz, Me)
            End Using
            bytes = mem.ToArray
        End Using
        Return bytes
    End Function
    Public Shared Function Serialize(obj As Object) As Byte()
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim ss As New ShallowSerializer
        ss.AddObj("", obj)
        Dim bytes As Byte()
        Using mem As New System.IO.MemoryStream
            Using gz As New System.IO.Compression.GZipStream(mem, IO.Compression.CompressionMode.Compress)
                bf.Serialize(gz, ss)
            End Using
            bytes = mem.ToArray
        End Using
        Return bytes
    End Function
    Public Shared Function Deserialize(bytes As Byte()) As Object
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        bf.Binder = New VecuteBinder
        Dim ss As ShallowSerializer
        Using mem As New System.IO.MemoryStream(bytes)
            Using gz As New System.IO.Compression.GZipStream(mem, IO.Compression.CompressionMode.Decompress)
                ss = bf.Deserialize(gz)
            End Using
        End Using
        Return ss.GetObject("")
    End Function
    Public Shared Function IsShallow(obj As Object) As Boolean
        Dim vT As Type = obj.GetType
        For Each att As Attribute In vT.GetCustomAttributes(True)
            If TypeOf att Is ShallowAttribute Then Return True
        Next
        Return False
    End Function
End Class

Friend Class VecuteBinder
    Inherits System.Runtime.Serialization.SerializationBinder
    Public Overrides Function BindToType(assemblyName As String, typeName As String) As Type
        Dim MCDSAssembly = assemblyName.Replace("Vecute", "MCDS").Replace("PublicKeyToken=6ca157640498cbe3", "PublicKeyToken=null")
        Dim MCDSType = typeName.Replace("Vecute", "MCDS").Replace("PublicKeyToken=6ca157640498cbe3", "PublicKeyToken=null")
        Dim vType = Type.GetType(MCDSType)
        If vType Is Nothing Then Stop
        Return vType
    End Function
End Class

Public Interface IShallow
    <System.ComponentModel.Description("Tells the list whether this element is already Initialized by its defining container.")>
 <Save()> Property HostInitialized As Boolean
End Interface

<Shallow()>
Public Class ShallowGrid
    Inherits GridBase
    Implements IShallow
    <Save()> Property ShallowElements As ShallowDictionary(Of Object, GridLayout)
        Get
            Dim sd As New ShallowDictionary(Of Object, GridLayout)
            Dim sl As IShallow
            For Each fe As System.Windows.FrameworkElement In Children
                If ShallowSerializer.IsShallow(fe) Then
                    If TypeOf fe Is IShallow Then
                        sl = fe
                        If Not sl.HostInitialized Then
                            sd.Add(sl, GridLayout.Layout(fe))
                        End If
                    Else
                        sd.Add(fe, GridLayout.Layout(fe))
                    End If
                End If
            Next
            Return sd
        End Get
        Set(value As ShallowDictionary(Of Object, GridLayout))
            If value IsNot Nothing Then
                For Each fe As System.Windows.FrameworkElement In value.Keys
                    Children.Add(fe)
                    GridLayout.Layout(fe) = value(fe)
                Next
            End If
        End Set
    End Property
    <Save()> Public Property HostInitialized As Boolean Implements IShallow.HostInitialized
End Class
<Serializable()>
Public Structure GridLayout
    Public RowIndex As Integer
    Public RowSpan As Integer
    Public ColumnIndex As Integer
    Public ColumnSpan As Integer
    Public Shared Property Layout(obj As ILayout) As GridLayout
        Get
            Return New GridLayout With {.RowIndex = System.Windows.Controls.Grid.GetRow(obj),
                            .RowSpan = System.Windows.Controls.Grid.GetRowSpan(obj),
                            .ColumnIndex = System.Windows.Controls.Grid.GetColumn(obj),
                            .ColumnSpan = System.Windows.Controls.Grid.GetColumnSpan(obj)}
        End Get
        Set(value As GridLayout)
            System.Windows.Controls.Grid.SetRow(obj, value.RowIndex)
            System.Windows.Controls.Grid.SetRowSpan(obj, value.RowSpan)
            System.Windows.Controls.Grid.SetColumn(obj, value.ColumnIndex)
            System.Windows.Controls.Grid.SetColumnSpan(obj, value.ColumnSpan)
        End Set
    End Property
End Structure
<Shallow()>
Public Class ShallowCanvas
    Inherits System.Windows.Controls.Canvas
    Implements IShallow
    <Save()> Public Property HostInitialized As Boolean Implements IShallow.HostInitialized
    <Save()> Property ShallowElements As ShallowDictionary(Of Object, CanvasLayout)
        Get
            Dim sd As New ShallowDictionary(Of Object, CanvasLayout)
            Dim sl As IShallow
            For Each fe As System.Windows.FrameworkElement In Children
                If ShallowSerializer.IsShallow(fe) Then
                    If TypeOf fe Is IShallow Then
                        sl = fe
                        If Not sl.HostInitialized Then
                            sd.Add(sl, CanvasLayout.Layout(fe))
                        End If
                    Else
                        sd.Add(fe, CanvasLayout.Layout(fe))
                    End If
                End If
            Next
            Return sd
        End Get
        Set(value As ShallowDictionary(Of Object, CanvasLayout))
            If value IsNot Nothing Then
                For Each fe As System.Windows.FrameworkElement In value.Keys
                    Children.Add(fe)
                    CanvasLayout.Layout(fe) = value(fe)
                Next
            End If
        End Set
    End Property
End Class

<Serializable()>
Public Structure CanvasLayout
    Public X As Double
    Public Y As Double
    Public Z As Integer
    Public Shared Property Layout(element As System.Windows.FrameworkElement) As CanvasLayout
        Get
            Return New CanvasLayout With {.X = System.Windows.Controls.Canvas.GetLeft(element),
                                          .Y = System.Windows.Controls.Canvas.GetTop(element),
                                          .Z = System.Windows.Controls.Canvas.GetZIndex(element)}
        End Get
        Set(value As CanvasLayout)
            System.Windows.Controls.Canvas.SetLeft(element, value.X)
            System.Windows.Controls.Canvas.SetTop(element, value.Y)
            System.Windows.Controls.Canvas.SetZIndex(element, value.Z)
        End Set
    End Property
End Structure

<Shallow()>
Public Class ShallowList(Of T)
    Inherits List(Of T)
End Class
<Shallow()>
Public Class ShallowDictionary(Of TKey, TValue)
    Inherits Dictionary(Of TKey, TValue)
End Class

<AttributeUsage(AttributeTargets.Class)>
Public Class ShallowAttribute
    Inherits Attribute
    Public Sub New()
        _IsFeildsDefined = False
    End Sub
    Public Property IsFeildsDefined As Boolean
    Public Property DefinedFields As IEnumerable(Of String)
    Public Sub New(ParamArray Fields As String())
        If Fields.Any Then
            _IsFeildsDefined = True
            _DefinedFields = Fields
        End If
    End Sub
End Class
<AttributeUsage(AttributeTargets.Property)>
Public Class NonSaveElementAttribute
    Inherits Attribute
End Class
<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field)>
Public Class SaveAttribute
    Inherits Attribute
End Class
<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field)>
Public Class LateLoadAttribute
    Inherits Attribute
    Public Sub New()
    End Sub
    Private _Index As Integer = 0
    Public Sub New(vIndex As Integer)
        _Index = vIndex
    End Sub
    Public Function MakeLoader(ac As System.Action) As LateLoader
        Return New LateLoader With {.Index = _Index, .Loader = ac}
    End Function
End Class
Public Class LateLoader
    Implements IComparable(Of LateLoader)
    Public Index As Integer
    Public Loader As System.Action
    Public Function CompareTo(other As LateLoader) As Integer Implements System.IComparable(Of LateLoader).CompareTo
        Return Math.Sign(Index - other.Index)
    End Function
End Class

<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field)>
Public Class EarlyBindAttribute
    Inherits Attribute
    Private _Binder As EarlyBinder
    Public Sub New(DeviceID As String)
        _Binder = New EarlyBinder With {.DeviceID = DeviceID}
    End Sub
    Public ReadOnly Property Device As EarlyBinder
        Get
            Return _Binder
        End Get
    End Property
End Class
<Serializable()>
Public Class EarlyBinder
    Public DeviceID As String
End Class
Public Class ShallowSurrogateSelector
    Implements System.Runtime.Serialization.ISurrogateSelector
    Public Sub New()
    End Sub
    Public Sub ChainSelector(selector As System.Runtime.Serialization.ISurrogateSelector) Implements System.Runtime.Serialization.ISurrogateSelector.ChainSelector
    End Sub
    Public Function GetNextSelector() As System.Runtime.Serialization.ISurrogateSelector Implements System.Runtime.Serialization.ISurrogateSelector.GetNextSelector
        Return Nothing
    End Function

    Public Function GetSurrogate(type As System.Type, context As System.Runtime.Serialization.StreamingContext, ByRef selector As System.Runtime.Serialization.ISurrogateSelector) As System.Runtime.Serialization.ISerializationSurrogate Implements System.Runtime.Serialization.ISurrogateSelector.GetSurrogate
        For Each att In type.GetCustomAttributes(True)
            If TypeOf att Is ShallowAttribute Then
                Dim se As ShallowAttribute = att
                If se.IsFeildsDefined Then
                    Return New ShallowSerializationSurrogate(se.DefinedFields.ToArray)
                Else
                    Return New ShallowSerializationSurrogate
                End If
            End If
        Next
        Return Nothing
    End Function
End Class
Public Class ShallowSerializationSurrogate
    Implements System.Runtime.Serialization.ISerializationSurrogate
    Public Sub New()
        _IsFeildsDefined = False
    End Sub
    Public Property IsFeildsDefined As Boolean
    Public Property DefinedFields As IEnumerable(Of String)
    Public Sub New(Fields As String())
        If Fields.Any Then
            _IsFeildsDefined = True
            _DefinedFields = Fields
        End If
    End Sub
    Public Sub GetObjectData(obj As Object, info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializationSurrogate.GetObjectData
        Dim vT As Type = obj.GetType
        Dim value As Object
        If _IsFeildsDefined Then
            For Each pi As PropertyInfo In vT.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                If _DefinedFields.Contains(pi.Name) AndAlso pi.GetIndexParameters.Count = 0 Then
                    value = pi.GetValue(obj, New Object() {})
                    info.AddValue(pi.Name, value)
                End If
            Next
        Else
            For Each pi As PropertyInfo In vT.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                For Each att As Attribute In pi.GetCustomAttributes(True)
                    If TypeOf att Is SaveAttribute Then
                        If pi.GetIndexParameters.Count = 0 Then
                            value = pi.GetValue(obj, New Object() {})
                            info.AddValue(pi.Name, value)
                        End If
                    End If
                Next
            Next
        End If
        For Each fi As FieldInfo In vT.GetFields
            For Each att As Attribute In fi.GetCustomAttributes(True)
                If TypeOf att Is SaveAttribute Then
                    value = fi.GetValue(obj)
                    info.AddValue(fi.Name, value)
                End If
            Next
        Next
    End Sub
    Public Function SetObjectData(obj As Object, info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext, selector As System.Runtime.Serialization.ISurrogateSelector) As Object Implements System.Runtime.Serialization.ISerializationSurrogate.SetObjectData
        Dim vT As Type = obj.GetType
        Dim value As Object
        'Set the Fields First
        For Each fi As FieldInfo In vT.GetFields
            For Each att As Attribute In fi.GetCustomAttributes(True)
                If TypeOf att Is SaveAttribute Then
                    value = info.GetValue(fi.Name, fi.FieldType)
                    fi.SetValue(obj, value)
                End If
            Next
        Next
        If _IsFeildsDefined Then
            For Each pi As PropertyInfo In vT.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                If _DefinedFields.Contains(pi.Name) AndAlso pi.GetIndexParameters.Count = 0 Then
                    value = info.GetValue(pi.Name, pi.PropertyType)
                    pi.SetValue(obj, value, New Object() {})
                End If
            Next
        Else
            For Each pi As PropertyInfo In vT.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                For Each att As Attribute In pi.GetCustomAttributes(True)
                    If TypeOf att Is SaveAttribute Then
                        If pi.GetIndexParameters.Count = 0 Then
                            value = info.GetValue(pi.Name, pi.PropertyType)
                            pi.SetValue(obj, value, New Object() {})
                        End If
                    End If
                Next
            Next
        End If
        Return Nothing
    End Function
End Class
 

Public Interface ILayout
End Interface

<Serializable()>
Public Class LayoutColumnDefination
    Inherits System.Windows.Controls.ColumnDefinition
    <Save()> Property Define As System.Windows.GridLength
        Get
            Return MyBase.Width
        End Get
        Set(value As System.Windows.GridLength)
            MyBase.Width = value
        End Set
    End Property
    'Public Shared Function FromBase(base As System.Windows.Controls.ColumnDefinition) As LayoutColumnDefination
    '    Return New LayoutColumnDefination With {.Width = base.Width}
    'End Function
    'Public Shared Function ToBase(instance As LayoutColumnDefination) As System.Windows.Controls.ColumnDefinition
    '    Return New System.Windows.Controls.ColumnDefinition With {.Width = instance.Width}
    'End Function
End Class

<Serializable()>
Public Class LayoutRowDefination
    Inherits System.Windows.Controls.RowDefinition
    <Save()> Public Property Define As System.Windows.GridLength
        Get
            Return MyBase.Height
        End Get
        Set(value As System.Windows.GridLength)
            MyBase.Height = value
        End Set
    End Property
    'Public Shared Function FromBase(base As System.Windows.Controls.RowDefinition) As LayoutRowDefination
    '    Return New LayoutRowDefination With {.Height = base.Height}
    'End Function
    'Public Shared Function ToBase(instance As LayoutRowDefination) As System.Windows.Controls.RowDefinition
    '    Return New System.Windows.Controls.RowDefinition With {.Height = instance.Height}
    'End Function
End Class
<Serializable()>
Public Class LayoutGrid
    Inherits System.Windows.Controls.Grid
    Implements ILayout
    <Save()> Public Property RowInfo As List(Of LayoutRowDefination)
        Get
            Dim vList As New List(Of LayoutRowDefination)
            For Each rd As System.Windows.Controls.RowDefinition In MyBase.RowDefinitions
                vList.Add(rd)
            Next
            Return vList
        End Get
        Set(value As List(Of LayoutRowDefination))
            For Each rd As System.Windows.Controls.RowDefinition In value
                MyBase.RowDefinitions.Add(rd)
            Next
        End Set
    End Property
    <Save()> Public Property ColumnInfo As List(Of LayoutColumnDefination)
        Get
            Dim vList As New List(Of LayoutColumnDefination)
            For Each rd As System.Windows.Controls.ColumnDefinition In MyBase.ColumnDefinitions
                vList.Add(rd)
            Next
            Return vList
        End Get
        Set(value As List(Of LayoutColumnDefination))
            For Each rd As System.Windows.Controls.ColumnDefinition In value
                MyBase.ColumnDefinitions.Add(rd)
            Next
        End Set
    End Property
    <Save()> Public Property LayoutControls As Dictionary(Of ILayout, GridLayout)
        Get
            Dim vList As New Dictionary(Of ILayout, GridLayout)
            For Each obj As ILayout In Children
                vList.Add(obj, GridLayout.Layout(obj))
            Next
            Return vList
        End Get
        Set(value As Dictionary(Of ILayout, GridLayout))
            For Each obj As ILayout In value.Keys
                GridLayout.Layout(obj) = value(obj)
            Next
        End Set
    End Property
End Class



<Serializable()>
Public Class LayoutLabel
    Inherits System.Windows.Controls.Label
    Implements ILayout
    <Save()> Public Property Value As Object
        Get
            Return Content
        End Get
        Set(value As Object)
            Content = value
        End Set
    End Property
End Class


Public Class GridBase
    Inherits System.Windows.Controls.Grid
    Private CurrentRowIndex As Integer = 0
    Private CurrentColumnIndex As Integer = 0
    Public Sub AddNewRowItem(Value As System.Windows.FrameworkElement, Optional vHeight As String = "AUTO")
        If CurrentColumnIndex <> 0 Then CurrentColumnIndex = 0
        AddRow(vHeight)
        Children.Add(Value)
        Grid.SetRow(Value, CurrentRowIndex)
        Grid.SetColumn(Value, CurrentColumnIndex)
        CurrentRowIndex += 1
    End Sub
    Public Sub AddRowItem(Value As System.Windows.FrameworkElement, Optional vHeight As String = "AUTO")
        If CurrentColumnIndex <> 0 Then CurrentColumnIndex = 0
        AddRow(vHeight)
        Children.Add(Value)
        Grid.SetRow(Value, CurrentRowIndex)
        Grid.SetColumn(Value, CurrentColumnIndex)
        CurrentRowIndex += 1
    End Sub
    Public Sub AddNewColumnItem(Value As System.Windows.FrameworkElement, Optional vWidth As String = "AUTO")
        If CurrentRowIndex <> 0 Then CurrentRowIndex = 0
        AddColumn(vWidth)
        Children.Add(Value)
        Grid.SetRow(Value, CurrentRowIndex)
        Grid.SetColumn(Value, CurrentColumnIndex)
        CurrentColumnIndex += 1
    End Sub
    Public Sub AddColumnItem(Value As System.Windows.FrameworkElement, Optional vWidth As String = "AUTO")
        AddColumn(vWidth)
        Children.Add(Value)
        Grid.SetRow(Value, CurrentRowIndex)
        Grid.SetColumn(Value, CurrentColumnIndex)
        CurrentColumnIndex += 1
    End Sub
    Public Sub AddRow(Optional vHeight As String = "AUTO")
        Dim rd As New System.Windows.Controls.RowDefinition
        Select Case vHeight.ToLower
            Case "*"
                rd.Height = New System.Windows.GridLength(1, System.Windows.GridUnitType.Star)
            Case "auto"
                rd.Height = New System.Windows.GridLength(0, System.Windows.GridUnitType.Auto)
            Case Else
                Dim v As Double
                If Double.TryParse(vHeight, v) Then
                    rd.Height = New System.Windows.GridLength(v)
                Else
                    rd.Height = New System.Windows.GridLength(0, System.Windows.GridUnitType.Auto)
                End If
        End Select
        RowDefinitions.Add(rd)
    End Sub
    Public Sub AddColumn(Optional vWidth As String = "AUTO")
        Dim rd As New System.Windows.Controls.ColumnDefinition
        Select Case vWidth.ToLower
            Case "*"
                rd.Width = New System.Windows.GridLength(1, System.Windows.GridUnitType.Star)
            Case "auto"
                rd.Width = New System.Windows.GridLength(0, System.Windows.GridUnitType.Auto)
            Case Else
                Dim v As Double
                If Double.TryParse(vWidth, v) Then
                    rd.Width = New System.Windows.GridLength(v)
                Else
                    rd.Width = New System.Windows.GridLength(0, System.Windows.GridUnitType.Auto)
                End If
        End Select
        ColumnDefinitions.Add(rd)
    End Sub
    Public Sub SetPosition(el As System.Windows.FrameworkElement, row As Integer, column As Integer, Optional rowspan As Integer = 0, Optional columnspan As Integer = 0)
        System.Windows.Controls.Grid.SetRow(el, row)
        If rowspan > 0 Then System.Windows.Controls.Grid.SetRowSpan(el, rowspan)
        System.Windows.Controls.Grid.SetColumn(el, column)
        If columnspan > 0 Then System.Windows.Controls.Grid.SetColumnSpan(el, columnspan)
    End Sub
    <Save()> Public Overridable Property ActualSize As System.Windows.Vector
        Get
            Dim w As Double
            Dim h As Double
            w = Width
            If w = 0D Then w = ActualWidth
            h = Height
            If h = 0D Then h = ActualHeight
            If Double.IsNaN(w) Then w = 0D
            If Double.IsNaN(h) Then h = 0D
            Return V(w, h)
        End Get
        Set(value As System.Windows.Vector)
            Width = value.X
            Height = value.Y
        End Set
    End Property
    <Save()> Public Property CanvasLocation As System.Windows.Vector
        Get
            If Double.IsNaN(Canvas.GetLeft(Me)) Or Double.IsNaN(Canvas.GetTop(Me)) Then Me.SetCanvasLocation(V(0D, 0D))
            Return V(Canvas.GetLeft(Me), Canvas.GetTop(Me))
        End Get
        Set(value As System.Windows.Vector)
            Canvas.SetLeft(Me, value.X)
            Canvas.SetTop(Me, value.Y)
        End Set
    End Property
    Protected Overridable ReadOnly Property RotationCenter As System.Windows.Vector
        Get
            Return ActualSize / 2
        End Get
    End Property
    <Save()> Public Property Rotation As Double
        Get
            If TypeOf RenderTransform Is RotateTransform Then
                Return CType(RenderTransform, RotateTransform).Angle
            Else
                Return 0D
            End If
        End Get
        Set(value As Double)
            Dim az As System.Windows.Vector = RotationCenter
            Dim rt As New RotateTransform(value, az.X, az.Y)
            RenderTransform = rt
        End Set
    End Property
    Public Sub DefineColumnsByDoubles(ParamArray values As Double())
        Dim cd As ColumnDefinition
        For Each d As Double In values
            If d > 0D Then
                cd = New ColumnDefinition With {.Width = New System.Windows.GridLength(d, Windows.GridUnitType.Pixel)}
            ElseIf d = 0D Then
                cd = New ColumnDefinition With {.Width = New System.Windows.GridLength(d, Windows.GridUnitType.Auto)}
            ElseIf d = Double.MinValue Then
                cd = New ColumnDefinition With {.Width = New System.Windows.GridLength(0, Windows.GridUnitType.Pixel)}
            Else
                cd = New ColumnDefinition With {.Width = New System.Windows.GridLength(-d, Windows.GridUnitType.Star)}
            End If
            ColumnDefinitions.Add(cd)
        Next
    End Sub
    Public Sub DefineRowsByDoubles(ParamArray values As Double())
        Dim cr As RowDefinition
        For Each d As Double In values
            If d > 0D Then
                cr = New RowDefinition With {.Height = New System.Windows.GridLength(d, Windows.GridUnitType.Pixel)}
            ElseIf d = 0D Then
                cr = New RowDefinition With {.Height = New System.Windows.GridLength(d, Windows.GridUnitType.Auto)}
            ElseIf d = Double.MinValue Then
                cr = New RowDefinition With {.Height = New System.Windows.GridLength(0, Windows.GridUnitType.Pixel)}
            Else
                cr = New RowDefinition With {.Height = New System.Windows.GridLength(-d, Windows.GridUnitType.Star)}
            End If
            RowDefinitions.Add(cr)
        Next
    End Sub
    Public Sub AddChild(child As System.Windows.FrameworkElement, vX As Integer, vY As Integer, vW As Integer, vH As Integer)
        Children.Add(child)
        Grid.SetColumn(child, vX)
        Grid.SetRow(child, vY)
        Grid.SetColumnSpan(child, vW)
        Grid.SetRowSpan(child, vH)
    End Sub
End Class

Public Module EnumerableIndex
    <System.Runtime.CompilerServices.Extension()> Public Function IndexOf(Of T)(value As IEnumerable(Of T), item As T) As Integer
        Dim i As Integer = 0
        For Each it As T In value
            If it.Equals(item) Then
                Return i
            End If
            i += 1
        Next
        Return -1
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function GetKey(Of TKey, TValue)(value As IDictionary(Of TKey, TValue), item As TValue) As TKey
        For Each Key As TKey In value.Keys
            If value(Key).Equals(item) Then
                Return Key
            End If
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function GetKeys(Of TKey, TValue)(value As IDictionary(Of TKey, TValue), item As TValue) As List(Of TKey)
        Dim vList As New List(Of TKey)
        For Each Key As TKey In value.Keys
            If value(Key).Equals(item) Then
                vList.Add(Key)
            End If
        Next
        Return vList
    End Function
End Module

Public Class StringDictionaryTable
    Private ValueList As New Dictionary(Of String, List(Of String))
    Public Sub New()
    End Sub
    Public Sub New(valuetable As String, ParamArray tabChars As String())
        Dim lines As String() = valuetable.Split(New Char() {vbCr, vbLf, vbCrLf}, System.StringSplitOptions.RemoveEmptyEntries)
        If tabChars.Length = 0 Then
            tabChars = New String() {vbTab}
        End If
        For Each l As String In lines
            Dim vLine As New List(Of String)
            vLine.AddRange(l.Split(tabChars, StringSplitOptions.RemoveEmptyEntries))
            ValueList.Add(vLine(0), vLine)
        Next
    End Sub
    Public Function GetValue(Key As String, Index As Integer) As String
        If ValueList.ContainsKey(Key) Then
            Dim vLine As List(Of String) = ValueList(Key)
            If vLine.Count > Index And Index > -1 Then
                Return vLine(Index)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
End Class
Public Class StringListTable
    Private ValueList As New List(Of List(Of String))
    Public Sub New()
    End Sub
    Public Sub New(valuetable As String, ParamArray tabChars As String())
        Dim lines As String() = valuetable.Split(New Char() {vbCr, vbLf, vbCrLf}, System.StringSplitOptions.RemoveEmptyEntries)
        If tabChars.Length = 0 Then
            tabChars = New String() {vbTab}
        End If
        For Each l As String In lines
            Dim vLine As New List(Of String)
            vLine.AddRange(l.Split(tabChars, StringSplitOptions.RemoveEmptyEntries))
            ValueList.Add(vLine)
        Next
    End Sub
    Public Function GetValue(LineID As Integer, Index As Integer) As String
        If LineID > -1 And ValueList.Count > LineID Then
            Dim vLine As List(Of String) = ValueList(LineID)
            If vLine.Count > Index And Index > -1 Then
                Return vLine(Index)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
End Class