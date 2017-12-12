'2011-02-26
'highest version
'support for Bitmap and Cursors

'<System.ComponentModel.Description("Indicate a Class can be described by XmlObjectDescriptor, which class must have a constructor without parameters")> _
'Public Class XmlDescribeAttribute
'    Inherits Attribute
'    Private vIsDescribe As Boolean
'    <System.ComponentModel.Description("XmlDescribe(True) will describe all but fields with XmlDescribe(False), XmlDescribe(False) will describe all but fields with XmlDescribe(True)")> _
'    Public Sub New(Optional ByVal Describe As Boolean = True)
'        vIsDescribe = Describe
'    End Sub
'    Public ReadOnly Property IsDescribe() As Boolean
'        Get
'            Return vIsDescribe
'        End Get
'    End Property
'End Class


'This class provides the methods to save any object in a custom way.
'some of the system objects has reference to a device, which can not be saved and has to be establish at runtime


'used to be testmode = 1
#If ReaderMode = 1 Or TestMode = 0 Then

Public Class XmlObject
    Public Fields As New Dictionary(Of String, String)
    Public TypeName As String
    Public AssemblyLocation As String
    Public AssemblyString As String
    Public IsDeviceDependent As Boolean = False
    'Public ReferenceFields As New Dictionary(Of String, Object)
End Class

'some of the classes need shallow serialization of properties.
Public Class TypeDescriptionOptions
    Public Sub New(ByVal vType As Type, Optional ByVal vShallowSerialization As Boolean = False, Optional ByVal vDeepSerialization As Boolean = True)
        Type = vType
        ShallowSerialization = vShallowSerialization
        DeepSerialization = vDeepSerialization
    End Sub
    Public Type As Type
    Public ShallowSerialization As Boolean = False
    Public DeepSerialization As Boolean = True
End Class

Public Class DiagnosticObject
    Public Key As String
    Public Field As Object
    Public [Object] As Object

    Public Sub New(ByVal vKey As String, ByVal obj As Object, ByVal vField As Object)
        Key = vKey
        [Object] = obj
        Field = vField
    End Sub
    Public Overrides Function ToString() As String
        Return "Diagnose " + Key + " : " + Field.ToString
    End Function
End Class

Public Class DiagnosticStack
    Inherits List(Of DiagnosticObject)
    Public Sub Push(ByVal obj As DiagnosticObject)
        Add(obj)
        If Me.Count > 64 Then Stop
    End Sub
    Public Function Pop() As DiagnosticObject
        Dim obj As DiagnosticObject = Me(Me.Count - 1)
        Me.Remove(obj)
        Return obj
    End Function
End Class
Public Class XmlObjectSerializer
    Private Converters As New ConverterDictionary
    Private ReferencePool As New Dictionary(Of Integer, XmlObject)
    Private ObjectPool As New Dictionary(Of Object, Integer)
    Private KeyPool As New Dictionary(Of String, Integer)
    Private TypeDescriptionPool As New Dictionary(Of Type, TypeDescriptionOptions)
    Public Diagnostic As Boolean = False
    Public DiagnosticStack As New DiagnosticStack
    Public Sub New()
        For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
            Converters.Add(cb)
        Next
    End Sub
    Public Sub New(ByVal IncludePredefinedConverters As Boolean)
        If IncludePredefinedConverters Then
            For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
                Converters.Add(cb)
            Next
        End If
    End Sub
    Public Sub AddConverter(ByVal vConverter As ConverterBase)
        Converters.Add(vConverter)
    End Sub
    Public Sub RemoveConverter(ByVal vConverter As ConverterBase)
        Converters.Remove(vConverter.TypeName)
    End Sub
    Public Sub AddTypeDescription(ByVal vTypeDescription As TypeDescriptionOptions)
        TypeDescriptionPool.Add(vTypeDescription.Type, vTypeDescription)
    End Sub
    Public Sub RemoveTypeDescription(ByVal vTypeDescription As TypeDescriptionOptions)
        TypeDescriptionPool.Remove(vTypeDescription.Type)
    End Sub
    <System.ComponentModel.Description("Add an object which is dependent on the excuting program or device and can not be automatically built by the deserializer.")> _
    Public Function AddDeviceObject(ByVal source As Object, ByVal key As String) As Integer
        Dim xo As New XmlObject
        xo.IsDeviceDependent = True
        Dim t As Type = source.GetType
        Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
        xo.AssemblyString = ass.ToString
        xo.AssemblyLocation = ass.Location
        Dim i As Integer = ObjectPool.Count
        ObjectPool.Add(source, i)
        KeyPool.Add(key, i)
        ReferencePool.Add(i, xo)
    End Function
    Public Function Add(ByVal [Object] As Object, ByVal Key As String) As Integer
        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(Key, [Object], Nothing))
        ParseObject([Object], Key)
        If Diagnostic Then DiagnosticStack.Pop()
    End Function

    Private Function IsNotInBlackList(ByVal [Object] As Object) As Boolean
        If [Object] Is Nothing Then Return True
        If TypeOf [Object] Is IntPtr Then Return False
        Dim t As Type = [Object].GetType
        If IsDerivedFrom(t, GetType(System.Delegate)) Then Return False
        Return True
    End Function

    Private Function IsDerivedFrom(ByVal T As Type, ByVal B As Type) As Boolean
        Dim vt As Type = T
        While Not (vt.BaseType Is Nothing)
            If vt.Equals(B) Then Return True
            vt = vt.BaseType
        End While
        Return False
    End Function

    Private Function ParseObject(ByRef source As Object, ByRef key As String) As Integer
        If source Is Nothing Then
            Return -1
        ElseIf ObjectPool.ContainsKey(source) Then
            Return ObjectPool(source)
        Else
            Dim t As Type = source.GetType
            Dim i As Integer = ObjectPool.Count
            ObjectPool.Add(source, i)
            KeyPool.Add(key, i)
            Dim xo As XmlObject
            If Converters.ContainsKey(t.AssemblyQualifiedName) Then
                Dim cb As ConverterBase = Converters(t.AssemblyQualifiedName)
                Dim c As Object = cb
                ReferencePool.Add(i, c.ConvertFrom(source))
            ElseIf t.IsEnum Then
                xo = New XmlObject
                xo.TypeName = t.AssemblyQualifiedName
                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
                xo.AssemblyString = ass.ToString
                xo.AssemblyLocation = ass.Location
                xo.Fields.Add("Enum", source.ToString)
                ReferencePool.Add(i, xo)
            Else
                xo = New XmlObject
                xo.TypeName = t.AssemblyQualifiedName
                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
                xo.AssemblyString = ass.ToString
                xo.AssemblyLocation = ass.Location
                ReferencePool.Add(i, xo)
                Dim IsDescribe As Boolean = True
                Dim IsDescribeField As Boolean
                For Each att As Attribute In t.GetCustomAttributes(True)
                    If TypeOf att Is System.ComponentModel.BindableAttribute Then
                        Dim ba As System.ComponentModel.BindableAttribute = att
                        IsDescribe = ba.Bindable
                    End If
                    '改之前的代码
                    'If TypeOf att Is XmlDescribeAttribute Then
                    '    Dim XdAtt As XmlDescribeAttribute = att
                    '    IsDescribe = XdAtt.IsDescribe
                    'End If
                Next
                If Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
                    Dim iDic As IDictionary = source
                    Dim iD As IDictionaryEnumerator = iDic.GetEnumerator
                    Dim j As Integer = 0

                    iD.Reset()
                    While iD.MoveNext

                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictKey", iD.Entry.Key, Nothing))
                        xo.Fields.Add("K" + j.ToString, ParseObject(iD.Entry.Key).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()

                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictValue", iD.Entry.Value, Nothing))
                        xo.Fields.Add("V" + j.ToString, ParseObject(iD.Entry.Value).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()
                        j += 1
                    End While
                    xo.Fields.Add("Count", j.ToString)
                    IsDescribe = False
                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
                    Dim iEl As IEnumerable = source
                    Dim iE As IEnumerator = iEl.GetEnumerator
                    Dim j As Integer = 0

                    iE.Reset()
                    While iE.MoveNext
                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("ListValue", iE.Current, Nothing))
                        xo.Fields.Add("V" + j.ToString, ParseObject(iE.Current).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()
                        j += 1
                    End While
                    xo.Fields.Add("Count", j.ToString)
                    IsDescribe = False
                End If

                If TypeDescriptionPool.ContainsKey(t) Then
                    Dim tdp As TypeDescriptionOptions = TypeDescriptionPool(t)
                    If tdp.DeepSerialization Then
                        xo.Fields.Add("DeepSerialization", "Yes")
                        'Or System.Reflection.BindingFlags.Instance
                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
                                If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
                                xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
                                If Diagnostic Then DiagnosticStack.Pop()
                            End If
                        Next
                    Else
                        xo.Fields.Add("DeepSerialization", "No")
                    End If
                    If tdp.ShallowSerialization Then
                        xo.Fields.Add("ShallowSerialization", "Yes")
                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            If IsDescribeField Then
                                If f.CanRead And f.CanWrite And f.GetIndexParameters.Length = 0 Then
                                    Dim tv As Type
                                    If Not (f.GetValue(source, New Object() {}) Is Nothing) Then
                                        tv = f.GetValue(source, New Object() {}).GetType
                                        If Converters.ContainsKey(tv.AssemblyQualifiedName) Or ObjectPool.ContainsKey(f.GetValue(source, New Object() {})) Then
                                            'only serialized defined values
                                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("Value", f.GetValue(source, New Object() {}), f))
                                            xo.Fields.Add("S" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
                                            If Diagnostic Then DiagnosticStack.Pop()
                                        End If

                                    End If
                                End If
                            End If
                        Next
                    Else
                        xo.Fields.Add("ShallowSerialization", "No")
                    End If
                Else
                    xo.Fields.Add("DeepSerialization", "Yes")
                    xo.Fields.Add("ShallowSerialization", "No")
                    'Or System.Reflection.BindingFlags.Instance
                    For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
                        IsDescribeField = IsDescribe
                        For Each att As Attribute In f.GetCustomAttributes(True)
                            If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                Dim ba As System.ComponentModel.BindableAttribute = att
                                IsDescribe = ba.Bindable
                            End If
                            'If TypeOf att Is XmlDescribeAttribute Then
                            '    Dim XdAtt As XmlDescribeAttribute = att
                            '    IsDescribeField = XdAtt.IsDescribe
                            'End If
                        Next

                        If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
                            xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
                            If Diagnostic Then DiagnosticStack.Pop()
                        End If
                    Next
                End If
            End If
            Return i
        End If
    End Function
    Private Function ParseObject(ByVal source As Object) As Integer
        If source Is Nothing Then
            Return -1
        ElseIf ObjectPool.ContainsKey(source) Then
            Return ObjectPool(source)
        Else
            Dim xo As XmlObject
            Dim t As Type = source.GetType
            Dim i As Integer = ObjectPool.Count
            ObjectPool.Add(source, i)
            If Converters.ContainsKey(t.AssemblyQualifiedName) Then
                Dim cb As ConverterBase = Converters(t.AssemblyQualifiedName)
                Dim c As Object = cb
                ReferencePool.Add(i, c.ConvertFrom(source))
            ElseIf t.IsEnum Then
                xo = New XmlObject
                xo.TypeName = t.AssemblyQualifiedName
                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
                xo.AssemblyString = ass.ToString
                xo.AssemblyLocation = ass.Location
                xo.Fields.Add("Enum", source.ToString)
                ReferencePool.Add(i, xo)
            Else
                xo = New XmlObject
                xo.TypeName = t.AssemblyQualifiedName
                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
                xo.AssemblyString = ass.ToString
                xo.AssemblyLocation = ass.Location
                ReferencePool.Add(i, xo)
                Dim IsDescribe As Boolean = True
                Dim IsDescribeField As Boolean
                For Each att As Attribute In t.GetCustomAttributes(True)
                    If TypeOf att Is System.ComponentModel.BindableAttribute Then
                        Dim ba As System.ComponentModel.BindableAttribute = att
                        IsDescribe = ba.Bindable
                    End If
                    'If TypeOf att Is XmlDescribeAttribute Then
                    '    Dim XdAtt As XmlDescribeAttribute = att
                    '    IsDescribe = XdAtt.IsDescribe
                    'End If
                Next
                If Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
                    Dim iDic As IDictionary = source
                    Dim iD As IDictionaryEnumerator = iDic.GetEnumerator
                    Dim j As Integer = 0

                    iD.Reset()
                    While iD.MoveNext

                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictKey", iD.Entry.Key, Nothing))
                        xo.Fields.Add("K" + j.ToString, ParseObject(iD.Entry.Key).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()

                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictValue", iD.Entry.Value, Nothing))
                        xo.Fields.Add("V" + j.ToString, ParseObject(iD.Entry.Value).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()
                        j += 1
                    End While
                    xo.Fields.Add("Count", j.ToString)
                    IsDescribe = False
                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
                    Dim iEl As IEnumerable = source
                    Dim iE As IEnumerator = iEl.GetEnumerator
                    Dim j As Integer = 0

                    iE.Reset()
                    While iE.MoveNext
                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("ListValue", iE.Current, Nothing))
                        xo.Fields.Add("V" + j.ToString, ParseObject(iE.Current).ToString)
                        If Diagnostic Then DiagnosticStack.Pop()
                        j += 1
                    End While
                    xo.Fields.Add("Count", j.ToString)
                    IsDescribe = False
                End If

                If TypeDescriptionPool.ContainsKey(t) Then
                    Dim tdp As TypeDescriptionOptions = TypeDescriptionPool(t)
                    If tdp.DeepSerialization Then
                        xo.Fields.Add("DeepSerialization", "Yes")
                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
                                If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
                                xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
                                If Diagnostic Then DiagnosticStack.Pop()
                            End If
                        Next
                    Else
                        xo.Fields.Add("DeepSerialization", "No")
                    End If
                    If tdp.ShallowSerialization Then
                        xo.Fields.Add("ShallowSerialization", "Yes")
                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            If IsDescribeField Then
                                If f.CanRead And f.CanWrite And f.GetIndexParameters.Length = 0 Then
                                    Dim tv As Type
                                    If Not (f.GetValue(source, New Object() {}) Is Nothing) Then
                                        tv = f.GetValue(source, New Object() {}).GetType
                                        If Converters.ContainsKey(tv.AssemblyQualifiedName) Or ObjectPool.ContainsKey(f.GetValue(source, New Object() {})) Then
                                            'only serialized defined values
                                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("Value", f.GetValue(source, New Object() {}), f))
                                            xo.Fields.Add("S" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
                                            If Diagnostic Then DiagnosticStack.Pop()
                                        End If

                                    End If
                                End If
                            End If
                        Next
                    Else
                        xo.Fields.Add("ShallowSerialization", "No")
                    End If
                Else
                    xo.Fields.Add("DeepSerialization", "Yes")
                    xo.Fields.Add("ShallowSerialization", "No")
                    For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
                        IsDescribeField = IsDescribe
                        For Each att As Attribute In f.GetCustomAttributes(True)
                            If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                Dim ba As System.ComponentModel.BindableAttribute = att
                                IsDescribe = ba.Bindable
                            End If
                        Next
                        If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
                            xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
                            If Diagnostic Then DiagnosticStack.Pop()
                        End If
                    Next
                End If
            End If
            Return i
        End If
    End Function
    Public Function ToXmlElement(ByVal xDoc As Xml.XmlDocument) As Xml.XmlElement
        Dim xRoot As Xml.XmlElement = xDoc.CreateElement("XmlObjectSerializer")
        xDoc.AppendChild(xRoot)
        Dim xObjects As Xml.XmlElement = xDoc.CreateElement("XmlObjects")
        xRoot.AppendChild(xObjects)

        Dim inf As Xml.XmlElement
        Dim val As Xml.XmlElement
        For Each ID As Integer In ReferencePool.Keys
            With ReferencePool(ID)
                inf = xDoc.CreateElement("TypeInfo")
                xObjects.AppendChild(inf)
                inf.SetAttribute("ID", ID.ToString)
                inf.SetAttribute("TypeName", .TypeName)
                inf.SetAttribute("AssemblyString", .AssemblyString)
                inf.SetAttribute("AssemblyLocation", .AssemblyLocation)
                inf.SetAttribute("IsDeviceDependent", .IsDeviceDependent.ToString)
                val = xDoc.CreateElement("ValueInfo")
                inf.AppendChild(val)
                If Not .IsDeviceDependent Then
                    For Each field As String In .Fields.Keys
                        val.SetAttribute(field, .Fields(field))
                    Next
                End If
            End With
        Next
        Dim xKeys As Xml.XmlElement = xDoc.CreateElement("XmlKeys")
        xRoot.AppendChild(xKeys)
        Dim kem As Xml.XmlElement
        For Each key As String In KeyPool.Keys
            kem = xDoc.CreateElement("KeyInfo")
            xKeys.AppendChild(kem)
            kem.SetAttribute("K" + key, KeyPool(key).ToString)
        Next
        Return xRoot
    End Function
    Public Sub SaveXml(ByVal XmlFilename As String)
        Dim xDoc As New Xml.XmlDocument
        xDoc.AppendChild(ToXmlElement(xDoc))
        xDoc.Save(XmlFilename)
    End Sub
    Public Sub SaveGZip(ByVal GZipFilename As String)
        Dim xDoc As New Xml.XmlDocument
        xDoc.AppendChild(ToXmlElement(xDoc))
        If IO.File.Exists(GZipFilename) Then IO.File.Delete(GZipFilename)
        Dim fs As New IO.FileStream(GZipFilename, IO.FileMode.OpenOrCreate)
        Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
        xDoc.Save(gz)
        gz.Close()
        fs.Close()
    End Sub
    Public Function SaveToZipBytes() As Byte()
        'Dim xDoc As New Xml.XmlDocument
        'xDoc.AppendChild(ToXmlElement(xDoc))
        'Dim fs As New IO.MemoryStream
        Dim i As Integer = 0
        While IO.File.Exists("temp" + i.ToString)
            i += 1
        End While
        Dim GZipFilename As String = "temp" + i.ToString
        'Dim ts As New IO.FileStream("temp" + i.ToString, IO.FileMode.OpenOrCreate)

        'Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
        'Dim xz As New IO.Compression.GZipStream(ts, IO.Compression.CompressionMode.Compress)

        'xDoc.Save(gz)
        'xDoc.Save(xz)


        '-----------------------------

        Dim xDoc As New Xml.XmlDocument
        xDoc.AppendChild(ToXmlElement(xDoc))
        If IO.File.Exists(GZipFilename) Then IO.File.Delete(GZipFilename)
        Dim fs As New IO.FileStream(GZipFilename, IO.FileMode.OpenOrCreate)
        Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
        xDoc.Save(gz)
        gz.Close()
        fs.Close()

        Dim rs As New IO.FileStream(GZipFilename, IO.FileMode.Open)


        Dim buf As Byte() = New Byte(rs.Length - 1) {}
        rs.Read(buf, 0, rs.Length)
        rs.Close()

        IO.File.Delete(GZipFilename)
        'ts.Position = 0
        'fs.Position = 0
        'fs.Read(buf, 0, fs.Length)
        'gz.Close()
        'xz.Close()
        'fs.Close()
        'ts.Close()
        'IO.File.Delete("temp" + i.ToString)
        Return buf
    End Function
    Public Sub Michael(ByVal Gabriel As Dictionary(Of String, Object)) '用来整体保存整个数据列表的函数
        For Each key As String In Gabriel.Keys
            Add(Gabriel(key), key)
        Next
    End Sub
End Class
Public Class XmlObjectDeserializer
    Private Converters As New ConverterDictionary
    Private ReferencePool As New Dictionary(Of String, XmlObject)
    Private ObjectPool As New Dictionary(Of String, Object)
    Private KeyPool As New Dictionary(Of String, String)

    Private ReverseKeyPool As New Dictionary(Of String, String)
    Private DevicePool As DeviceDictionary
    Private Loaded As Boolean = False
    Private Deserialized As Boolean = False
    Public Sub New()
        For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
            Converters.Add(cb)
        Next
    End Sub
    Public Sub New(ByVal IncludePredefinedConverters As Boolean)
        If IncludePredefinedConverters Then
            For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
                Converters.Add(cb)
            Next
        End If
    End Sub

    Public Sub AddConverter(ByVal vConverter As ConverterBase)
        Converters.Add(vConverter)
    End Sub
    Public Sub RemoveConverter(ByVal vConverter As ConverterBase)
        Converters.Remove(vConverter.TypeName)
    End Sub
    Public Function LoadXml(ByVal XmlRoot As Xml.XmlElement) As DeviceDictionary
        If Loaded Or (XmlRoot Is Nothing) Then Return Nothing
        Dim xKeys As Xml.XmlElement = Nothing
        Dim xObjects As Xml.XmlElement = Nothing
        For Each elm As Xml.XmlElement In XmlRoot.ChildNodes
            If elm.Name = "XmlObjects" Then
                xObjects = elm
            End If
            If elm.Name = "XmlKeys" Then
                xKeys = elm
            End If
        Next

        If (xKeys Is Nothing) Or (xObjects Is Nothing) Then Return Nothing
        Dim vDevice As New HideDeviceDictionary
        Dim xo As XmlObject
        Dim vType As HideTypeInfo
        Try
            For Each elm As Xml.XmlElement In xKeys.ChildNodes
                Dim k As String = elm.Attributes(0).Name.Substring(1)
                KeyPool.Add(k, elm.Attributes(0).Value)
                ReverseKeyPool.Add(elm.Attributes(0).Value, k)
            Next

            For Each inf As Xml.XmlElement In xObjects.ChildNodes
                Dim val As Xml.XmlElement = inf.ChildNodes(0)
                xo = New XmlObject
                xo.TypeName = inf.GetAttribute("TypeName")
                xo.AssemblyString = inf.GetAttribute("AssemblyString")
                xo.AssemblyLocation = inf.GetAttribute("AssemblyLocation")
                xo.IsDeviceDependent = CBool(inf.GetAttribute("IsDeviceDependent"))
                Dim ID As String = inf.GetAttribute("ID")
                ReferencePool.Add(ID, xo)
                If xo.IsDeviceDependent Then
                    vType = New HideTypeInfo(ReverseKeyPool(ID), xo.TypeName, xo.AssemblyString, xo.AssemblyLocation)
                    vDevice.Add(vType.Key, vType)
                End If
                For Each att As Xml.XmlAttribute In val.Attributes
                    'Dim k As String = att.Name
                    'If Converters.ContainsKey(xo.TypeName) AndAlso k.StartsWith("X") Then k = k.Substring(1)
                    xo.Fields.Add(att.Name, att.Value)
                Next
            Next
        Catch ex As Exception
            KeyPool.Clear()
            ReverseKeyPool.Clear()
            ReferencePool.Clear()
            Return Nothing
        End Try
        DevicePool = vDevice
        Loaded = True
        Return DevicePool
    End Function
    Public Shared Function GetXmlRootFromXmlFile(ByVal vFilename As String) As Xml.XmlElement
        If IO.File.Exists(vFilename) Then
            Try
                Dim fs As New IO.FileStream(vFilename, IO.FileMode.Open)
                Dim xDoc As New Xml.XmlDocument
                Try
                    xDoc.Load(fs)
                Catch ex As Exception
                    fs.Close()
                    Return Nothing
                End Try
                fs.Close()
                Return xDoc.ChildNodes(0)
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
    End Function
    Public Shared Function GetXmlRootFromGZipFile(ByVal vFilename As String) As Xml.XmlElement
        If IO.File.Exists(vFilename) Then
            Try
                Dim fs As New IO.FileStream(vFilename, IO.FileMode.Open)
                Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Decompress)
                Dim xDoc As New Xml.XmlDocument
                Try
                    xDoc.Load(gz)
                Catch ex As Exception
                    gz.Close()
                    fs.Close()
                    Return Nothing
                End Try
                gz.Close()
                fs.Close()
                Return xDoc.ChildNodes(0)
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
    End Function

    Public Function Michael(ByVal Gabriel As String, ByVal BlackList As List(Of String)) As Dictionary(Of String, Object)
        Dim DVD As DeviceDictionary = LoadXml(GetXmlRootFromGZipFile(Gabriel))
        Deserialize(DVD)
        Dim Dict As New Dictionary(Of String, Object)
        For Each key As String In BlackList
            Dict.Add(key, GetObjectByKey(key))
        Next
        Return Dict
    End Function

    Public Function Michael(ByVal Gabriel As Byte(), ByVal BlackList As List(Of String)) As Dictionary(Of String, Object)
        Dim DVD As DeviceDictionary = LoadXml(LoadFromGZipBytes(Gabriel))
        Deserialize(DVD)
        Dim Dict As New Dictionary(Of String, Object)
        For Each key As String In BlackList
            Dict.Add(key, GetObjectByKey(key))
        Next
        Return Dict
    End Function
    Public Shared Function LoadFromGZipBytes(ByVal buf As Byte()) As Xml.XmlElement
        Try
            Dim xDoc As New Xml.XmlDocument
            Dim fs As New IO.MemoryStream(buf)
            Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Decompress)
            xDoc.Load(gz)
            Dim XLE As Xml.XmlElement = xDoc.ChildNodes(0)
            gz.Close()
            fs.Close()
            Return XLE
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Deserialize(ByVal localDevices As DeviceDictionary) As Boolean
        If localDevices Is Nothing Then Exit Function

        Deserialized = False
        For Each ti As TypeInfo In localDevices.Values
            If ti.Value Is Nothing Then
                Deserialized = True
                Exit For
            End If
        Next
        If Deserialized Then Return False
        For Each key As String In localDevices.Keys
            ObjectPool.Add(KeyPool(key), localDevices(key).Value)
        Next
        For Each key As String In KeyPool.Keys
            GetObject(KeyPool(key))
        Next
        Deserialized = True
        Return Deserialized

    End Function
    Private AssemblyDict As New Dictionary(Of String, System.Reflection.Assembly)

    Private Function GetObject(ByVal ID As String) As Object
        If ID = -1 Then Return Nothing
        If ObjectPool.ContainsKey(ID) Then Return ObjectPool(ID)
        Dim xo As XmlObject = ReferencePool(ID)
        Dim xObj As Object
        Static bytesname As String = GetType(Byte()).AssemblyQualifiedName
        If (Converters.ContainsKey(xo.TypeName) AndAlso Not xo.TypeName = bytesname) OrElse (xo.TypeName = bytesname AndAlso Not xo.Fields.ContainsKey("Count")) Then
            Dim cv As Object = Converters(xo.TypeName)
            xObj = cv.ConvertTo(xo)
            ObjectPool.Add(ID, xObj)
        Else
            Dim ass As System.Reflection.Assembly
            If AssemblyDict.ContainsKey(xo.AssemblyString) Then
                ass = AssemblyDict(xo.AssemblyString)
            Else
                ass = System.Reflection.Assembly.Load(xo.AssemblyString)
                If ass Is Nothing Then ass = System.Reflection.Assembly.LoadFile(xo.AssemblyLocation)
                If ass Is Nothing Then
                    Dim e As New Exception("Assembly does not exist.")
                    Throw e
                Else
                    AssemblyDict.Add(xo.AssemblyString, ass)
                End If
            End If

            Dim t As Type = Type.GetType(xo.TypeName)

            If t Is Nothing Then t = ass.GetType(xo.TypeName)

            Dim ci As System.Reflection.ConstructorInfo
            'ci = t.GetConstructor(Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic, Nothing, System.Type.EmptyTypes, New System.Reflection.ParameterModifier() {})
            ci = t.GetConstructor(System.Type.EmptyTypes)
            Dim cis As System.Reflection.ConstructorInfo() = t.GetConstructors


            If t.IsEnum Then
                xObj = [Enum].Parse(t, xo.Fields("Enum"))
                ObjectPool.Add(ID, xObj)
            Else

                If t.IsArray Then
                    ci = cis(0)
                Else
                    If ci Is Nothing Then Throw New Exception(String.Format("Type {0} does not have a constructor without parameters.", t.AssemblyQualifiedName))
                End If



                If t.IsArray Then
                    Dim Count As Integer = CInt(xo.Fields("Count"))
                    xObj = ci.Invoke(New Object() {Count})
                    ObjectPool.Add(ID, xObj)
                    For j As Integer = 0 To Count - 1
                        xObj(j) = GetObject(xo.Fields("V" + j.ToString))
                    Next
                ElseIf Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
                    xObj = ci.Invoke(New Object() {})
                    ObjectPool.Add(ID, xObj)
                    Dim Count As Integer = CInt(xo.Fields("Count"))

                    Dim iDic As Object = xObj
                    For j As Integer = 0 To Count - 1
                        iDic.Add(GetObject(xo.Fields("K" + j.ToString)), GetObject(xo.Fields("V" + j.ToString)))
                    Next

                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
                    xObj = ci.Invoke(New Object() {})
                    ObjectPool.Add(ID, xObj)
                    Dim Count As Integer = CInt(xo.Fields("Count"))
                    Dim iDic As Object = xObj
                    For j As Integer = 0 To Count - 1
                        iDic.Add(GetObject(xo.Fields("V" + j.ToString)))
                    Next
                Else
                    xObj = ci.Invoke(New Object() {})
                    ObjectPool.Add(ID, xObj)
                    Dim IsDescribe As Boolean = True
                    For Each att As Attribute In t.GetCustomAttributes(True)
                        If TypeOf att Is System.ComponentModel.BindableAttribute Then
                            Dim ba As System.ComponentModel.BindableAttribute = att
                            IsDescribe = ba.Bindable
                        End If
                        'If TypeOf att Is XmlDescribeAttribute Then
                        '    Dim xdAtt As XmlDescribeAttribute = att
                        '    IsDescribe = xdAtt.IsDescribe
                        'End If
                    Next

                    Dim IsDescribeField As Boolean
                    'xo.Fields.Add("DeepSerialization", "Yes")
                    'xo.Fields.Add("ShallowSerialization", "No")
                    If xo.Fields.ContainsKey("DeepSerialization") AndAlso xo.Fields("DeepSerialization") = "Yes" Then
                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            'Dim obj As Object
                            'If xo.Fields.ContainsKey("X" + f.Name) Then obj = 
                            'If TypeOf obj Is Dictionary(Of Integer, Nuctions.GeneAnnotation) Then
                            '    xObj.Features.AddRange(obj.Values)
                            '    'If IsDescribeField And xo.Fields.ContainsKey("X" + f.Name) Then f.SetValue(xObj, obj)
                            'Else
                            If IsDescribeField And xo.Fields.ContainsKey("X" + f.Name) Then f.SetValue(xObj, GetObject(xo.Fields("X" + f.Name)))
                            'End If
                        Next
                    End If
                    If xo.Fields.ContainsKey("ShallowSerialization") AndAlso xo.Fields("ShallowSerialization") = "Yes" Then
                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                            IsDescribeField = IsDescribe
                            For Each att As Attribute In f.GetCustomAttributes(True)
                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
                                    Dim ba As System.ComponentModel.BindableAttribute = att
                                    IsDescribe = ba.Bindable
                                End If
                                'If TypeOf att Is XmlDescribeAttribute Then
                                '    Dim XdAtt As XmlDescribeAttribute = att
                                '    IsDescribeField = XdAtt.IsDescribe
                                'End If
                            Next
                            If IsDescribeField And xo.Fields.ContainsKey("S" + f.Name) Then
                                If f.CanWrite And f.GetIndexParameters.Length = 0 Then
                                    'xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
                                    f.SetValue(xObj, GetObject(xo.Fields("S" + f.Name)), New Object() {})
                                End If
                            End If

                        Next
                    End If
                End If

            End If
        End If
        Return xObj
    End Function
    Private Class HideDeviceDictionary
        Inherits DeviceDictionary
    End Class
    Private Class HideTypeInfo
        Inherits TypeInfo
        Sub New(ByVal mKey As String, ByVal mTypeName As String, ByVal mAssemblyName As String, ByVal mAssemblyLocation As String)
            vKey = mKey
            vTypeName = mTypeName
            vAssemblyName = mAssemblyName
            vAssemblyLocation = mAssemblyLocation
        End Sub
    End Class
    Public Function GetObjectByKey(ByVal key As String) As Object
        If Deserialized AndAlso KeyPool.ContainsKey(key) AndAlso ObjectPool.ContainsKey(KeyPool(key)) Then
            Return ObjectPool(KeyPool(key))
        Else
            Return Nothing
        End If
    End Function
    Public ReadOnly Property Keys() As IEnumerable
        Get
            Return KeyPool.Keys
        End Get
    End Property
    Public ReadOnly Property Objects() As IEnumerable
        Get
            Return ObjectPool.Values
        End Get
    End Property
End Class

Public MustInherit Class DeviceDictionary
    Inherits Dictionary(Of String, TypeInfo)
    Protected Class DeviceIndependentDictionary
        Inherits DeviceDictionary
    End Class

    <System.ComponentModel.Description("If all the objects to be deserialized are all device-independent, use this empty one; Otherwise, please customize a DeviceDictionary by inheriting it.")> _
    Public Shared ReadOnly Property DeviceIndependent() As DeviceDictionary
        Get
            Return New DeviceIndependentDictionary
        End Get
    End Property

End Class
Public MustInherit Class TypeInfo
    Protected vKey As String
    Protected vTypeName As String
    Protected vAssemblyName As String
    Protected vAssemblyLocation As String
    Protected vValue As Object

    Public ReadOnly Property Key() As String
        Get
            Return vKey
        End Get
    End Property
    Public ReadOnly Property TypeName() As String
        Get
            Return vTypeName
        End Get
    End Property
    Public ReadOnly Property AssemblyName() As String
        Get
            Return vAssemblyName
        End Get
    End Property
    Public ReadOnly Property AssemblyLocation() As String
        Get
            Return vAssemblyLocation
        End Get
    End Property
    Public Property Value() As Object
        Get
            Return vValue
        End Get
        Set(ByVal value As Object)
            vValue = value
        End Set
    End Property
End Class


Public MustInherit Class ConverterBase
    Public MustOverride ReadOnly Property TypeName() As String
    Public MustOverride ReadOnly Property IsValueType() As Boolean
End Class

Public Class ConverterDictionary
    Inherits Dictionary(Of String, ConverterBase)
    Public Shadows Sub Add(ByVal Item As ConverterBase)
        MyBase.Add(Item.TypeName.Substring(0, Item.TypeName.IndexOf(",")), Item)
    End Sub
    Public Shadows Function ContainsKey(ByVal key As String) As Boolean
        If key.IndexOf("`") > -1 Then Return False
        Dim i As Integer = key.IndexOf(",")
        If i > -1 Then
            Dim sKey As String = key.Substring(0, i)
            For Each vKey As String In Keys
                If vKey = sKey Then Return True
            Next
        Else
            For Each vKey As String In Keys
                If vKey = key Then Return True
            Next
        End If
        Return False
    End Function
    Default Public Shadows Property Item(ByVal key As String) As ConverterBase
        Get
            Return MyBase.Item(key.Substring(0, key.IndexOf(",")))
        End Get
        Set(ByVal value As ConverterBase)
            MyBase.Item(key.Substring(0, key.IndexOf(","))) = value
        End Set
    End Property
End Class

Public MustInherit Class Converter(Of T)
    Inherits ConverterBase
    Public Overrides ReadOnly Property TypeName() As String
        Get
            Return GetType(T).AssemblyQualifiedName
        End Get
    End Property
    Public MustOverride Function ConvertFrom(ByVal value As T) As XmlObject
    Public MustOverride Function ConvertTo(ByVal value As XmlObject) As T
End Class

Public Class PreDefinedConverters
    Public Shared ReadOnly Property PreDefinedConverters() As ConverterBase()
        Get
            Dim ListCB As New List(Of ConverterBase)
            With ListCB
                .Add(StringConverter)
                .Add(Int64Converter)
                .Add(Int32Converter)
                .Add(Int16Converter)
                .Add(ByteConverter)
                .Add(BooleanConverter)
                .Add(SingleConverter)
                .Add(DoubleConverter)
                .Add(DecimalConverter)
                .Add(CharConverter)
                .Add(DateConverter)
                .Add(ColorConverter)
                .Add(PointConverter)
                .Add(PointFConverter)
                .Add(SizeConverter)
                .Add(SizeFConverter)
                .Add(RectangleConverter)
                .Add(RectangleFConverter)
                .Add(PenConverter)
                .Add(SolidBrushConverter)
                .Add(FontConverter)
                .Add(ImageConverter)
                .Add(BitmapConverter)
                .Add(CursorConverter)
                .Add(PaddingConverter)
                .Add(TimeSpanConverter)
                .Add(BytesConverter)
                '.Add(FFTInfoConverter)
            End With

            Return ListCB.ToArray
        End Get
    End Property
    Public Shared ReadOnly Property StringConverter() As ConverterBase
        Get
            Return New preStringConverter
        End Get
    End Property
    Public Shared ReadOnly Property Int32Converter() As ConverterBase
        Get
            Return New preIntegerConverter
        End Get
    End Property
    Public Shared ReadOnly Property Int64Converter() As ConverterBase
        Get
            Return New preLongConverter
        End Get
    End Property
    Public Shared ReadOnly Property Int16Converter() As ConverterBase
        Get
            Return New preShortConverter
        End Get
    End Property
    Public Shared ReadOnly Property ByteConverter() As ConverterBase
        Get
            Return New preByteConverter
        End Get
    End Property
    Public Shared ReadOnly Property BooleanConverter() As ConverterBase
        Get
            Return New preBooleanConverter
        End Get
    End Property
    Public Shared ReadOnly Property SingleConverter() As ConverterBase
        Get
            Return New preSingleConverter
        End Get
    End Property
    Public Shared ReadOnly Property DoubleConverter() As ConverterBase
        Get
            Return New preDoubleConverter
        End Get
    End Property
    Public Shared ReadOnly Property DecimalConverter() As ConverterBase
        Get
            Return New preDecimalConverter
        End Get
    End Property
    Public Shared ReadOnly Property CharConverter() As ConverterBase
        Get
            Return New preCharConverter
        End Get
    End Property
    Public Shared ReadOnly Property DateConverter() As ConverterBase
        Get
            Return New preDateConverter
        End Get
    End Property
    Public Shared ReadOnly Property ColorConverter() As ConverterBase
        Get
            Return New preColorConverter
        End Get
    End Property
    Public Shared ReadOnly Property PointConverter() As ConverterBase
        Get
            Return New prePointConverter
        End Get
    End Property
    Public Shared ReadOnly Property PointFConverter() As ConverterBase
        Get
            Return New prePointFConverter
        End Get
    End Property
    Public Shared ReadOnly Property SizeConverter() As ConverterBase
        Get
            Return New preSizeConverter
        End Get
    End Property
    Public Shared ReadOnly Property SizeFConverter() As ConverterBase
        Get
            Return New preSizeFConverter
        End Get
    End Property
    Public Shared ReadOnly Property RectangleConverter() As ConverterBase
        Get
            Return New preRectangleConverter
        End Get
    End Property
    Public Shared ReadOnly Property RectangleFConverter() As ConverterBase
        Get
            Return New preRectangleFConverter
        End Get
    End Property
    Public Shared ReadOnly Property PenConverter() As ConverterBase
        Get
            Return New prePenConverter
        End Get
    End Property
    Public Shared ReadOnly Property SolidBrushConverter() As ConverterBase
        Get
            Return New preSolidBrushConverter
        End Get
    End Property
    Public Shared ReadOnly Property FontConverter() As ConverterBase
        Get
            Return New preFontConverter
        End Get
    End Property
    Public Shared ReadOnly Property ImageConverter() As ConverterBase
        Get
            Return New preImageConverter
        End Get
    End Property
    Public Shared ReadOnly Property BitmapConverter() As ConverterBase
        Get
            Return New preBitmapConverter
        End Get
    End Property
    Public Shared ReadOnly Property CursorConverter() As ConverterBase
        Get
            Return New preCursorConverter
        End Get
    End Property
    Public Shared ReadOnly Property PaddingConverter() As ConverterBase
        Get
            Return New prePaddingConverter
        End Get
    End Property
    Public Shared ReadOnly Property TimeSpanConverter() As ConverterBase
        Get
            Return New preTimeSpanConverter
        End Get
    End Property
    Public Shared ReadOnly Property BytesConverter() As ConverterBase
        Get
            Return New preBytesConverter
        End Get
    End Property
    'Public Shared ReadOnly Property FFTInfoConverter() As ConverterBase
    '    Get
    '        Return New preFFTInfoConverter
    '    End Get
    'End Property
    Private Class preStringConverter
        Inherits Converter(Of String)
        Public Overrides Function ConvertFrom(ByVal value As String) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(String).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(String))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As String
            Return value.Fields("Value")
        End Function

        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preBytesConverter
        Inherits Converter(Of Byte())
        Public Overrides Function ConvertFrom(ByVal value As Byte()) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Byte()).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Byte()))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", Convert.ToBase64String(value))
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Byte()
            If value.Fields.ContainsKey("Value") Then
                Return Convert.FromBase64String(DirectCast(value.Fields("Value"), String))
            Else
                Dim c As Integer = CInt(value.Fields("Count"))
                Dim bl As New List(Of Byte)
                For i As Integer = 0 To c - 1
                    bl.Add(CByte(value.Fields("V" + i.ToString)))
                Next
                Return bl.ToArray
            End If
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    ''' <summary>
    ''' 为了加密需要 所以把这个部分阉割了
    ''' </summary>
    ''' <remarks></remarks>
    Private Class preIntegerConverter
        Inherits Converter(Of Int32)
        Public Overrides Function ConvertFrom(ByVal value As Int32) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Int32).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Int32))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Int32
            Return CInt(value.Fields("Value"))
        End Function

        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preLongConverter
        Inherits Converter(Of Int64)
        Public Overrides Function ConvertFrom(ByVal value As Int64) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Int64).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Int64))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Int64
            Return CLng(value.Fields("Value"))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preShortConverter
        Inherits Converter(Of Int16)
        Public Overrides Function ConvertFrom(ByVal value As Int16) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Int16).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Int16))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Int16
            Return CShort(value.Fields("Value"))
        End Function

        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preByteConverter
        Inherits Converter(Of Byte)
        Public Overrides Function ConvertFrom(ByVal value As Byte) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Byte).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Byte))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Byte
            Return CByte(value.Fields("Value"))
        End Function

        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preBooleanConverter
        Inherits Converter(Of Boolean)

        Public Overrides Function ConvertFrom(ByVal value As Boolean) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Boolean).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Boolean))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Boolean
            Return CBool(value.Fields("Value"))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preSingleConverter
        Inherits Converter(Of Single)
        Public Overrides Function ConvertFrom(ByVal value As Single) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Single).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Single))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Single
            Return CSng(value.Fields("Value"))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preDoubleConverter
        Inherits Converter(Of Double)
        Public Overrides Function ConvertFrom(ByVal value As Double) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Double).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Double))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Double
            Return CDbl(value.Fields("Value"))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preDecimalConverter
        Inherits Converter(Of Decimal)
        Public Overrides Function ConvertFrom(ByVal value As Decimal) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Decimal).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Decimal))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Decimal
            Return CDec(value.Fields("Value"))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preCharConverter
        Inherits Converter(Of Char)
        Public Overrides Function ConvertFrom(ByVal value As Char) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Char).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Char))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", AscW(value).ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Char
            Return ChrW(CInt(value.Fields("Value")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preDateConverter
        Inherits Converter(Of Date)
        Public Overrides Function ConvertFrom(ByVal value As Date) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Date).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Date))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToOADate.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Date
            Return Date.FromOADate(CDbl(value.Fields("Value")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class preColorConverter
        Inherits Converter(Of Color)
        Public Overrides Function ConvertFrom(ByVal value As Color) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Color).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Color))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("ARGB", value.ToArgb.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Color
            Return Color.FromArgb(CInt(value.Fields("ARGB")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class prePointConverter
        Inherits Converter(Of Point)
        Public Overrides Function ConvertFrom(ByVal value As Point) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Point).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Point))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("X", value.X.ToString)
            xo.Fields.Add("Y", value.Y.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Point
            Return New Point(CInt(value.Fields("X")), CInt(value.Fields("Y")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class prePointFConverter
        Inherits Converter(Of PointF)
        Public Overrides Function ConvertFrom(ByVal value As PointF) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(PointF).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(PointF))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("X", value.X.ToString)
            xo.Fields.Add("Y", value.Y.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As PointF
            Return New Point(CSng(value.Fields("X")), CSng(value.Fields("Y")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preSizeConverter
        Inherits Converter(Of Size)
        Public Overrides Function ConvertFrom(ByVal value As Size) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Size).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Size))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Size
            Return New Size(CInt(value.Fields("Width")), CInt(value.Fields("Height")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preSizeFConverter
        Inherits Converter(Of SizeF)
        Public Overrides Function ConvertFrom(ByVal value As SizeF) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(SizeF).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(SizeF))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As SizeF
            Return New SizeF(CSng(value.Fields("Width")), CSng(value.Fields("Height")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preRectangleConverter
        Inherits Converter(Of Rectangle)
        Public Overrides Function ConvertFrom(ByVal value As Rectangle) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Rectangle).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Rectangle))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Left", value.Left.ToString)
            xo.Fields.Add("Top", value.Top.ToString)
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Rectangle
            Dim e As Xml.XmlElement = Nothing
            Return New Rectangle(CInt(value.Fields("Left")), CInt(value.Fields("Top")), CInt(value.Fields("Width")), CInt(value.Fields("Height")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    Private Class preRectangleFConverter
        Inherits Converter(Of RectangleF)
        Public Overrides Function ConvertFrom(ByVal value As RectangleF) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(RectangleF).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(RectangleF))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Left", value.Left.ToString)
            xo.Fields.Add("Top", value.Top.ToString)
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As RectangleF
            Dim e As Xml.XmlElement = Nothing
            Return New RectangleF(CSng(value.Fields("Left")), CSng(value.Fields("Top")), CSng(value.Fields("Width")), CSng(value.Fields("Height")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    Private Class prePenConverter
        Inherits Converter(Of Pen)
        Public Overrides Function ConvertFrom(ByVal value As Pen) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Pen).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Pen))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Color", value.Color.ToArgb.ToString)
            xo.Fields.Add("Width", value.Width.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Pen
            Return New Pen(Color.FromArgb(CInt(value.Fields("Color"))), CSng(value.Fields("Width")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class

    Private Class preSolidBrushConverter
        Inherits Converter(Of SolidBrush)
        Public Overrides Function ConvertFrom(ByVal value As SolidBrush) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(SolidBrush).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(SolidBrush))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Color", value.Color.ToArgb.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As SolidBrush
            Return New SolidBrush(Color.FromArgb(CInt(value.Fields("Color"))))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class

    'new supports for windows controls.
    'we are planning to use these supports to save data from or load data to a window form.
    Private Class preFontConverter
        Inherits Converter(Of Font)
        Public Overrides Function ConvertFrom(ByVal value As Font) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Font).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Font))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("FamilyName", value.FontFamily.Name)
            xo.Fields.Add("Size", value.Size.ToString)
            xo.Fields.Add("Style", CInt(value.Style).ToString)
            xo.Fields.Add("Unit", CInt(value.Unit).ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Font
            Return New Font(value.Fields("FamilyName"), CSng(value.Fields("Size")), CInt(value.Fields("Style")), CInt(value.Fields("Unit")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
    Private Class preImageConverter
        Inherits Converter(Of Image)
        Public Overrides Function ConvertFrom(ByVal value As Image) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Image).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Image))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Dim bmp As New Bitmap(value.Width, value.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.DrawImage(value, New Rectangle(0, 0, value.Width, value.Height))
            g.Dispose()
            Dim Scan As Integer = Math.Ceiling(Image.GetPixelFormatSize(bmp.PixelFormat) / 8)
            xo.Fields.Add("Scan", Scan.ToString)
            Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat)
            Dim code As Byte() = New Byte(Scan * bmp.Width * bmp.Height - 1) {}
            System.Runtime.InteropServices.Marshal.Copy(vBitmapData.Scan0, code, 0, Scan * bmp.Width * bmp.Height - 1)
            xo.Fields.Add("Base64", Convert.ToBase64String(code))
            bmp.UnlockBits(vBitmapData)
            bmp.Dispose()
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Image
            Dim w As Integer = CInt(value.Fields("Width"))
            Dim h As Integer = CInt(value.Fields("Height"))
            Dim scan As Integer = CInt(value.Fields("Scan"))
            Dim bmp As New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, w, h), Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat)
            Dim code As Byte() = Convert.FromBase64String(value.Fields("Base64"))
            System.Runtime.InteropServices.Marshal.Copy(code, 0, vBitmapData.Scan0, scan * bmp.Width * bmp.Height - 1)
            bmp.UnlockBits(vBitmapData)
            Return bmp
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
    Private Class preBitmapConverter
        Inherits Converter(Of Bitmap)
        Public Overrides Function ConvertFrom(ByVal value As Bitmap) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Bitmap).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Bitmap))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Width", value.Width.ToString)
            xo.Fields.Add("Height", value.Height.ToString)
            Dim bmp As New Bitmap(value.Width, value.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.DrawImage(value, New Rectangle(0, 0, value.Width, value.Height))
            g.Dispose()
            Dim Scan As Integer = Math.Ceiling(Image.GetPixelFormatSize(bmp.PixelFormat) / 8)
            xo.Fields.Add("Scan", Scan.ToString)
            Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat)
            Dim code As Byte() = New Byte(Scan * bmp.Width * bmp.Height - 1) {}
            System.Runtime.InteropServices.Marshal.Copy(vBitmapData.Scan0, code, 0, Scan * bmp.Width * bmp.Height - 1)
            xo.Fields.Add("Base64", Convert.ToBase64String(code))
            bmp.UnlockBits(vBitmapData)
            bmp.Dispose()
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Bitmap
            Dim w As Integer = CInt(value.Fields("Width"))
            Dim h As Integer = CInt(value.Fields("Height"))
            Dim scan As Integer = CInt(value.Fields("Scan"))
            Dim bmp As New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, w, h), Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat)
            Dim code As Byte() = Convert.FromBase64String(value.Fields("Base64"))
            System.Runtime.InteropServices.Marshal.Copy(code, 0, vBitmapData.Scan0, scan * bmp.Width * bmp.Height - 1)
            bmp.UnlockBits(vBitmapData)
            Return bmp
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
    Private Class preCursorConverter
        Inherits Converter(Of Cursor)
        Public Overrides Function ConvertFrom(ByVal value As Cursor) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Cursor).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Cursor))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            Dim t As Type = GetType(Cursors)
            Dim IsSystemCursor As Boolean = False
            Dim CursorName As String = ""
            For Each pi As System.Reflection.PropertyInfo In t.GetProperties(Reflection.BindingFlags.Static Or Reflection.BindingFlags.Public)
                If value Is pi.GetValue(Nothing, New Object() {}) Then
                    CursorName = pi.Name
                    IsSystemCursor = True
                    Exit For
                End If
            Next
            If IsSystemCursor Then
                xo.Fields.Add("IsSystemCursor", IsSystemCursor.ToString)
                xo.Fields.Add("CursorName", CursorName)
            Else
                xo.Fields.Add("IsSystemCursor", IsSystemCursor.ToString)
                xo.Fields.Add("Width", value.Size.Width.ToString)
                xo.Fields.Add("Heigh", value.Size.Height.ToString)
                xo.Fields.Add("HotspotX", value.HotSpot.X.ToString)
                xo.Fields.Add("HotspotY", value.HotSpot.Y.ToString)
                Dim bmp As New Bitmap(value.Size.Width, value.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim g As Graphics = Graphics.FromImage(bmp)
                value.Draw(g, New Rectangle(0, 0, value.Size.Width, value.Size.Height))
                g.Dispose()
                Dim Scan As Integer = Math.Ceiling(Image.GetPixelFormatSize(bmp.PixelFormat) / 8)
                xo.Fields.Add("Scan", Scan.ToString)
                Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat)
                Dim code As Byte() = New Byte(Scan * bmp.Width * bmp.Height - 1) {}
                System.Runtime.InteropServices.Marshal.Copy(vBitmapData.Scan0, code, 0, Scan * bmp.Width * bmp.Height - 1)
                xo.Fields.Add("Base64", Convert.ToBase64String(code))
                bmp.UnlockBits(vBitmapData)
                bmp.Dispose()
            End If

            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Cursor
            Dim IsSystemCursor As Boolean = CBool(value.Fields("IsSystemCursor"))
            Dim c As Cursor = Cursors.Default

            If IsSystemCursor Then
                Dim CursorName As String = value.Fields("CursorName")
                Dim t As Type = GetType(Cursors)
                Dim find As Boolean = False
                For Each pi As System.Reflection.PropertyInfo In t.GetProperties(Reflection.BindingFlags.Static Or Reflection.BindingFlags.Public)
                    If CursorName = pi.Name Then
                        c = pi.GetValue(Nothing, New Object() {})
                        find = True
                        Exit For
                    End If
                Next
            Else
                Dim w As Integer = CInt(value.Fields("Width"))
                Dim h As Integer = CInt(value.Fields("Height"))
                Dim scan As Integer = CInt(value.Fields("Scan"))
                Dim HotspotX As Integer = CInt(value.Fields("HotspotX"))
                Dim HotspotY As Integer = CInt(value.Fields("HotspotY"))


                Dim bmp As New Bitmap((w - HotspotX) * 2, (h - HotspotY) * 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim g As Graphics = Graphics.FromImage(bmp)
                g.Clear(Color.Transparent)
                g.Dispose()
                'Dim bmp As New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim vBitmapData As System.Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(bmp.Width - w, bmp.Height - h, w, h), Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat)
                Dim code As Byte() = Convert.FromBase64String(value.Fields("Base64"))
                System.Runtime.InteropServices.Marshal.Copy(code, 0, vBitmapData.Scan0, scan * bmp.Width * bmp.Height - 1)
                bmp.UnlockBits(vBitmapData)
                c = New Cursor(bmp.GetHicon)
            End If
            Return c
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
    Private Class prePaddingConverter
        Inherits Converter(Of Padding)
        Public Overrides Function ConvertFrom(ByVal value As Padding) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(Padding).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(Padding))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Left", value.Left.ToString)
            xo.Fields.Add("Top", value.Top.ToString)
            xo.Fields.Add("Right", value.Right.ToString)
            xo.Fields.Add("Bottom", value.Bottom.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As Padding
            Return New Padding(CInt(value.Fields("Left")), CInt(value.Fields("Top")), CInt(value.Fields("Right")), CInt(value.Fields("Bottom")))
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
    Private Class preTimeSpanConverter
        Inherits Converter(Of TimeSpan)
        Public Overrides Function ConvertFrom(ByVal value As TimeSpan) As XmlObject
            Dim xo As New XmlObject
            xo.TypeName = GetType(TimeSpan).AssemblyQualifiedName
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(TimeSpan))
            xo.AssemblyString = ass.ToString
            xo.AssemblyLocation = ass.Location
            xo.Fields.Add("Value", value.ToString)
            Return xo
        End Function
        Public Overrides Function ConvertTo(ByVal value As XmlObject) As TimeSpan
            If value.Fields.ContainsKey("Value") Then
                Dim ts As TimeSpan
                If TimeSpan.TryParse(value.Fields("Value"), ts) Then
                    Return ts
                Else
                    Return TimeSpan.Zero
                End If
            Else
                Return TimeSpan.Zero
            End If
        End Function
        Public Overrides ReadOnly Property IsValueType() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
    'Private Class preRegexConverter
    '    Inherits Converter(Of System.Text.RegularExpressions.Regex)
    '    Public Overrides Function ConvertFrom(ByVal value As System.Text.RegularExpressions.Regex) As XmlObject
    '        Dim xo As New XmlObject
    '        xo.TypeName = GetType(System.Text.RegularExpressions.Regex).AssemblyQualifiedName
    '        Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(System.Text.RegularExpressions.Regex))
    '        xo.AssemblyString = ass.ToString
    '        xo.AssemblyLocation = ass.Location
    '        xo.Fields.Add("Value", value.)
    '        'xo.Fields.Add("Count", l)
    '        Return xo
    '    End Function
    '    Public Overrides Function ConvertTo(ByVal value As XmlObject) As System.Text.RegularExpressions.Regex
    '        Dim ffti As New System.Text.RegularExpressions.Regex
    '        Dim base64 As String = value.Fields("Value")
    '        Dim l As Integer = ffti.F.Count
    '        Dim bytes As Byte() = Convert.FromBase64String(base64)
    '        Dim bytePtr As IntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(l * 4)
    '        System.Runtime.InteropServices.Marshal.Copy(bytes, 0, bytePtr, l * 4)
    '        System.Runtime.InteropServices.Marshal.Copy(bytePtr, ffti.F, 0, l)
    '        System.Runtime.InteropServices.Marshal.FreeHGlobal(bytePtr)
    '        Return ffti
    '    End Function
    '    Public Overrides ReadOnly Property IsValueType() As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property
    'End Class
End Class

#End If