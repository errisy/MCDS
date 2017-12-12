
'Public Class ___hwhfow0zBPvh2eyL89asdhviaudak23a7sdosSdsivgavisdwFW
'    Private Converters As New ConverterDictionary
'    Private ReferencePool As New Dictionary(Of Integer, XmlObject)
'    Private ObjectPool As New Dictionary(Of Object, Integer)
'    Private KeyPool As New Dictionary(Of String, Integer)
'    Private TypeDescriptionPool As New Dictionary(Of Type, TypeDescriptionOptions)
'    Public Diagnostic As Boolean = False
'    Public DiagnosticStack As New DiagnosticStack
'    Private ShallowSerl As New AkimeHomura
'    Public Sub New()
'        For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
'            Converters.Add(cb)
'        Next
'    End Sub
'    Public Sub New(ByVal IncludePredefinedConverters As Boolean)
'        If IncludePredefinedConverters Then
'            For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
'                Converters.Add(cb)
'            Next
'        End If
'    End Sub
'    Public Sub AddConverter(ByVal vConverter As ConverterBase)
'        Converters.Add(vConverter)
'    End Sub
'    Public Sub RemoveConverter(ByVal vConverter As ConverterBase)
'        Converters.Remove(vConverter.TypeName)
'    End Sub
'    Public Sub AddTypeDescription(ByVal vTypeDescription As TypeDescriptionOptions)
'        TypeDescriptionPool.Add(vTypeDescription.Type, vTypeDescription)
'    End Sub
'    Public Sub RemoveTypeDescription(ByVal vTypeDescription As TypeDescriptionOptions)
'        TypeDescriptionPool.Remove(vTypeDescription.Type)
'    End Sub
'    <System.ComponentModel.Description("Add an object which is dependent on the excuting program or device and can not be automatically built by the deserializer.")> _
'    Public Function AddDeviceObject(ByVal source As Object, ByVal key As String) As Integer
'        Dim xo As New XmlObject
'        xo.IsDeviceDependent = True
'        Dim t As Type = source.GetType
'        Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
'        xo.AssemblyString = ass.ToString
'        xo.AssemblyLocation = ass.Location
'        Dim i As Integer = ObjectPool.Count
'        ObjectPool.Add(source, i)
'        KeyPool.Add(key, i)
'        ReferencePool.Add(i, xo)
'    End Function
'    Public Function Add(ByVal [Object] As Object, ByVal Key As String) As Integer
'        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(Key, [Object], Nothing))
'        ParseObject([Object], Key)
'        If Diagnostic Then DiagnosticStack.Pop()
'    End Function

'    Private Function IsNotInBlackList(ByVal [Object] As Object) As Boolean
'        If [Object] Is Nothing Then Return True
'        If TypeOf [Object] Is IntPtr Then Return False
'        Dim t As Type = [Object].GetType
'        If IsDerivedFrom(t, GetType(System.Delegate)) Then Return False
'        Return True
'    End Function

'    Private Function IsDerivedFrom(ByVal T As Type, ByVal B As Type) As Boolean
'        Dim vt As Type = T
'        While Not (vt.BaseType Is Nothing)
'            If vt.Equals(B) Then Return True
'            vt = vt.BaseType
'        End While
'        Return False
'    End Function

'    Private Function ParseObject(ByRef source As Object, ByRef key As String) As Integer
'        If source Is Nothing Then
'            Return -1
'        ElseIf ObjectPool.ContainsKey(source) Then
'            Return ObjectPool(source)
'        ElseIf TypeOf source Is Integer Then
'            Dim vI As Integer = ObjectPool.Count
'            ShallowSerl.Add(vI, source)
'            ObjectPool.Add(source, vI)
'            KeyPool.Add(key, vI)
'        ElseIf source.GetType.IsEnum Then
'            Dim vI As Integer = ObjectPool.Count
'            ShallowSerl.Add(vI, CInt(source))
'            ObjectPool.Add(source, vI)
'            KeyPool.Add(key, vI)
'        Else
'            Dim t As Type = source.GetType
'            Dim i As Integer = ObjectPool.Count
'            ObjectPool.Add(source, i)
'            KeyPool.Add(key, i)
'            Dim xo As XmlObject
'            If Converters.ContainsKey(t.AssemblyQualifiedName) Then
'                Dim cb As ConverterBase = Converters(t.AssemblyQualifiedName)
'                Dim c As Object = cb
'                ReferencePool.Add(i, c.ConvertFrom(source))
'            ElseIf t.IsEnum Then
'                xo = New XmlObject
'                xo.TypeName = t.AssemblyQualifiedName
'                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
'                xo.AssemblyString = ass.ToString
'                xo.AssemblyLocation = ass.Location
'                xo.Fields.Add("Enum", source.ToString)
'                ReferencePool.Add(i, xo)
'            Else
'                xo = New XmlObject
'                xo.TypeName = t.AssemblyQualifiedName
'                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
'                xo.AssemblyString = ass.ToString
'                xo.AssemblyLocation = ass.Location
'                ReferencePool.Add(i, xo)
'                Dim IsDescribe As Boolean = True
'                Dim IsDescribeField As Boolean
'                For Each att As Attribute In t.GetCustomAttributes(True)
'                    If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                        Dim ba As System.ComponentModel.BindableAttribute = att
'                        IsDescribe = ba.Bindable
'                    End If
'                    '改之前的代码
'                    'If TypeOf att Is XmlDescribeAttribute Then
'                    '    Dim XdAtt As XmlDescribeAttribute = att
'                    '    IsDescribe = XdAtt.IsDescribe
'                    'End If
'                Next
'                If Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
'                    Dim iDic As IDictionary = source
'                    Dim iD As IDictionaryEnumerator = iDic.GetEnumerator
'                    Dim j As Integer = 0

'                    iD.Reset()
'                    While iD.MoveNext

'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictKey", iD.Entry.Key, Nothing))
'                        xo.Fields.Add("K" + j.ToString, ParseObject(iD.Entry.Key).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()

'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictValue", iD.Entry.Value, Nothing))
'                        xo.Fields.Add("V" + j.ToString, ParseObject(iD.Entry.Value).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()
'                        j += 1
'                    End While
'                    xo.Fields.Add("Count", j.ToString)
'                    IsDescribe = False
'                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
'                    Dim iEl As IEnumerable = source
'                    Dim iE As IEnumerator = iEl.GetEnumerator
'                    Dim j As Integer = 0

'                    iE.Reset()
'                    While iE.MoveNext
'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("ListValue", iE.Current, Nothing))
'                        xo.Fields.Add("V" + j.ToString, ParseObject(iE.Current).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()
'                        j += 1
'                    End While
'                    xo.Fields.Add("Count", j.ToString)
'                    IsDescribe = False
'                End If

'                If TypeDescriptionPool.ContainsKey(t) Then
'                    Dim tdp As TypeDescriptionOptions = TypeDescriptionPool(t)
'                    If tdp.DeepSerialization Then
'                        xo.Fields.Add("DeepSerialization", "Yes")
'                        'Or System.Reflection.BindingFlags.Instance
'                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
'                                If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
'                                xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
'                                If Diagnostic Then DiagnosticStack.Pop()
'                            End If
'                        Next
'                    Else
'                        xo.Fields.Add("DeepSerialization", "No")
'                    End If
'                    If tdp.ShallowSerialization Then
'                        xo.Fields.Add("ShallowSerialization", "Yes")
'                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            If IsDescribeField Then
'                                If f.CanRead And f.CanWrite And f.GetIndexParameters.Length = 0 Then
'                                    Dim tv As Type
'                                    If Not (f.GetValue(source, New Object() {}) Is Nothing) Then
'                                        tv = f.GetValue(source, New Object() {}).GetType
'                                        If Converters.ContainsKey(tv.AssemblyQualifiedName) Or ObjectPool.ContainsKey(f.GetValue(source, New Object() {})) Then
'                                            'only serialized defined values
'                                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("Value", f.GetValue(source, New Object() {}), f))
'                                            xo.Fields.Add("S" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
'                                            If Diagnostic Then DiagnosticStack.Pop()
'                                        End If

'                                    End If
'                                End If
'                            End If
'                        Next
'                    Else
'                        xo.Fields.Add("ShallowSerialization", "No")
'                    End If
'                Else
'                    xo.Fields.Add("DeepSerialization", "Yes")
'                    xo.Fields.Add("ShallowSerialization", "No")
'                    'Or System.Reflection.BindingFlags.Instance
'                    For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
'                        IsDescribeField = IsDescribe
'                        For Each att As Attribute In f.GetCustomAttributes(True)
'                            If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                Dim ba As System.ComponentModel.BindableAttribute = att
'                                IsDescribe = ba.Bindable
'                            End If
'                            'If TypeOf att Is XmlDescribeAttribute Then
'                            '    Dim XdAtt As XmlDescribeAttribute = att
'                            '    IsDescribeField = XdAtt.IsDescribe
'                            'End If
'                        Next

'                        If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
'                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
'                            xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
'                            If Diagnostic Then DiagnosticStack.Pop()
'                        End If
'                    Next
'                End If
'            End If
'            Return i
'        End If
'    End Function
'    Private Function ParseObject(ByVal source As Object) As Integer
'        If source Is Nothing Then
'            Return -1
'        ElseIf ObjectPool.ContainsKey(source) Then
'            Return ObjectPool(source)
'        ElseIf TypeOf source Is Integer Then
'            Dim vI As Integer = ObjectPool.Count
'            ShallowSerl.Add(vI, source)
'            ObjectPool.Add(source, vI)
'        ElseIf source.GetType.IsEnum Then
'            Dim vI As Integer = ObjectPool.Count
'            ShallowSerl.Add(vI, CInt(source))
'            ObjectPool.Add(source, vI)
'        Else
'            Dim xo As XmlObject
'            Dim t As Type = source.GetType
'            Dim i As Integer = ObjectPool.Count
'            ObjectPool.Add(source, i)
'            If Converters.ContainsKey(t.AssemblyQualifiedName) Then
'                Dim cb As ConverterBase = Converters(t.AssemblyQualifiedName)
'                Dim c As Object = cb
'                ReferencePool.Add(i, c.ConvertFrom(source))
'            ElseIf t.IsEnum Then
'                xo = New XmlObject
'                xo.TypeName = t.AssemblyQualifiedName
'                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
'                xo.AssemblyString = ass.ToString
'                xo.AssemblyLocation = ass.Location
'                xo.Fields.Add("Enum", source.ToString)
'                ReferencePool.Add(i, xo)
'            Else
'                xo = New XmlObject
'                xo.TypeName = t.AssemblyQualifiedName
'                Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(t)
'                xo.AssemblyString = ass.ToString
'                xo.AssemblyLocation = ass.Location
'                ReferencePool.Add(i, xo)
'                Dim IsDescribe As Boolean = True
'                Dim IsDescribeField As Boolean
'                For Each att As Attribute In t.GetCustomAttributes(True)
'                    If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                        Dim ba As System.ComponentModel.BindableAttribute = att
'                        IsDescribe = ba.Bindable
'                    End If
'                    'If TypeOf att Is XmlDescribeAttribute Then
'                    '    Dim XdAtt As XmlDescribeAttribute = att
'                    '    IsDescribe = XdAtt.IsDescribe
'                    'End If
'                Next
'                If Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
'                    Dim iDic As IDictionary = source
'                    Dim iD As IDictionaryEnumerator = iDic.GetEnumerator
'                    Dim j As Integer = 0

'                    iD.Reset()
'                    While iD.MoveNext

'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictKey", iD.Entry.Key, Nothing))
'                        xo.Fields.Add("K" + j.ToString, ParseObject(iD.Entry.Key).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()

'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("DictValue", iD.Entry.Value, Nothing))
'                        xo.Fields.Add("V" + j.ToString, ParseObject(iD.Entry.Value).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()
'                        j += 1
'                    End While
'                    xo.Fields.Add("Count", j.ToString)
'                    IsDescribe = False
'                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
'                    Dim iEl As IEnumerable = source
'                    Dim iE As IEnumerator = iEl.GetEnumerator
'                    Dim j As Integer = 0

'                    iE.Reset()
'                    While iE.MoveNext
'                        If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("ListValue", iE.Current, Nothing))
'                        xo.Fields.Add("V" + j.ToString, ParseObject(iE.Current).ToString)
'                        If Diagnostic Then DiagnosticStack.Pop()
'                        j += 1
'                    End While
'                    xo.Fields.Add("Count", j.ToString)
'                    IsDescribe = False
'                End If

'                If TypeDescriptionPool.ContainsKey(t) Then
'                    Dim tdp As TypeDescriptionOptions = TypeDescriptionPool(t)
'                    If tdp.DeepSerialization Then
'                        xo.Fields.Add("DeepSerialization", "Yes")
'                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
'                                If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
'                                xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
'                                If Diagnostic Then DiagnosticStack.Pop()
'                            End If
'                        Next
'                    Else
'                        xo.Fields.Add("DeepSerialization", "No")
'                    End If
'                    If tdp.ShallowSerialization Then
'                        xo.Fields.Add("ShallowSerialization", "Yes")
'                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            If IsDescribeField Then
'                                If f.CanRead And f.CanWrite And f.GetIndexParameters.Length = 0 Then
'                                    Dim tv As Type
'                                    If Not (f.GetValue(source, New Object() {}) Is Nothing) Then
'                                        tv = f.GetValue(source, New Object() {}).GetType
'                                        If Converters.ContainsKey(tv.AssemblyQualifiedName) Or ObjectPool.ContainsKey(f.GetValue(source, New Object() {})) Then
'                                            'only serialized defined values
'                                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject("Value", f.GetValue(source, New Object() {}), f))
'                                            xo.Fields.Add("S" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
'                                            If Diagnostic Then DiagnosticStack.Pop()
'                                        End If

'                                    End If
'                                End If
'                            End If
'                        Next
'                    Else
'                        xo.Fields.Add("ShallowSerialization", "No")
'                    End If
'                Else
'                    xo.Fields.Add("DeepSerialization", "Yes")
'                    xo.Fields.Add("ShallowSerialization", "No")
'                    For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
'                        IsDescribeField = IsDescribe
'                        For Each att As Attribute In f.GetCustomAttributes(True)
'                            If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                Dim ba As System.ComponentModel.BindableAttribute = att
'                                IsDescribe = ba.Bindable
'                            End If
'                        Next
'                        If IsDescribeField And IsNotInBlackList(f.GetValue(source)) Then
'                            If Diagnostic Then DiagnosticStack.Push(New DiagnosticObject(f.Name, f.GetValue(source), f))
'                            xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source)))
'                            If Diagnostic Then DiagnosticStack.Pop()
'                        End If
'                    Next
'                End If
'            End If
'            Return i
'        End If
'    End Function
'    Public Function ToXmlElement(ByVal xDoc As Xml.XmlDocument) As Xml.XmlElement
'        Dim xRoot As Xml.XmlElement = xDoc.CreateElement("XmlObjectSerializer")
'        xDoc.AppendChild(xRoot)
'        Dim xObjects As Xml.XmlElement = xDoc.CreateElement("XmlObjects")
'        xRoot.AppendChild(xObjects)
'        Dim inf As Xml.XmlElement
'        Dim val As Xml.XmlElement
'        For Each ID As Integer In ReferencePool.Keys
'            With ReferencePool(ID)
'                inf = xDoc.CreateElement("TypeInfo")
'                xObjects.AppendChild(inf)
'                inf.SetAttribute("ID", ID.ToString)
'                inf.SetAttribute("TypeName", .TypeName)
'                inf.SetAttribute("AssemblyString", .AssemblyString)
'                inf.SetAttribute("AssemblyLocation", .AssemblyLocation)
'                inf.SetAttribute("IsDeviceDependent", .IsDeviceDependent.ToString)
'                val = xDoc.CreateElement("ValueInfo")
'                inf.AppendChild(val)
'                If Not .IsDeviceDependent Then
'                    For Each field As String In .Fields.Keys
'                        val.SetAttribute(field, .Fields(field))
'                    Next
'                End If
'            End With
'        Next
'        Dim xKeys As Xml.XmlElement = xDoc.CreateElement("XmlKeys")
'        xRoot.AppendChild(xKeys)
'        Dim kem As Xml.XmlElement
'        For Each key As String In KeyPool.Keys
'            kem = xDoc.CreateElement("KeyInfo")
'            xKeys.AppendChild(kem)
'            kem.SetAttribute("K" + key, KeyPool(key).ToString)
'        Next
'        Return xRoot
'    End Function
'    'Public Sub SaveXml(ByVal XmlFilename As String)
'    '    Dim xDoc As New Xml.XmlDocument
'    '    xDoc.AppendChild(ToXmlElement(xDoc))
'    '    xDoc.Save(XmlFilename)
'    'End Sub
'    'Public Sub SaveGZip(ByVal GZipFilename As String)
'    '    Dim xDoc As New Xml.XmlDocument
'    '    xDoc.AppendChild(ToXmlElement(xDoc))
'    '    If IO.File.Exists(GZipFilename) Then IO.File.Delete(GZipFilename)
'    '    Dim fs As New IO.FileStream(GZipFilename, IO.FileMode.OpenOrCreate)
'    '    Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
'    '    xDoc.Save(gz)
'    '    gz.Close()
'    '    fs.Close()
'    'End Sub
'    Public Function SaveToZipBytes() As Byte()
'        'Dim xDoc As New Xml.XmlDocument
'        'xDoc.AppendChild(ToXmlElement(xDoc))
'        'Dim fs As New IO.MemoryStream
'        Dim i As Integer = 0
'        While IO.File.Exists("temp" + i.ToString)
'            i += 1
'        End While
'        Dim GZipFilename As String = "temp" + i.ToString
'        'Dim ts As New IO.FileStream("temp" + i.ToString, IO.FileMode.OpenOrCreate)

'        'Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
'        'Dim xz As New IO.Compression.GZipStream(ts, IO.Compression.CompressionMode.Compress)

'        'xDoc.Save(gz)
'        'xDoc.Save(xz)


'        '-----------------------------
'        Dim ol As New OnlyMyRailGunCanShoot
'        ol.Accelerator = False
'        ol.KamijouTouma = ShallowSerl.Save
'        Dim xDoc As New Xml.XmlDocument
'        xDoc.AppendChild(ToXmlElement(xDoc))
'        Using mem As New System.IO.MemoryStream
'            Using gz As New System.IO.Compression.GZipStream(mem, IO.Compression.CompressionMode.Compress)
'                xDoc.Save(gz)
'                ol.ShiraiKuroko = mem.ToArray
'            End Using
'        End Using
'        Dim buf As Byte() = Utilties.AESEncryt(Utilties.ObjToBit(ol), lslhfh, ssgw)
'        'If IO.File.Exists(GZipFilename) Then IO.File.Delete(GZipFilename)
'        'Dim fs As New IO.FileStream(GZipFilename, IO.FileMode.OpenOrCreate)
'        'Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Compress)
'        'xDoc.Save(gz)
'        'gz.Close()
'        'fs.Close()

'        'Dim rs As New IO.FileStream(GZipFilename, IO.FileMode.Open)


'        'Dim buf As Byte() = New Byte(rs.Length - 1) {}
'        'rs.Read(buf, 0, rs.Length)
'        'rs.Close()

'        'IO.File.Delete(GZipFilename)
'        'ts.Position = 0
'        'fs.Position = 0
'        'fs.Read(buf, 0, fs.Length)
'        'gz.Close()
'        'xz.Close()
'        'fs.Close()
'        'ts.Close()
'        'IO.File.Delete("temp" + i.ToString)
'        Return buf
'    End Function
'    Private lslhfh As Byte() = Utilties.ToBit("SCItf/I4L6WoSA4j2UNajdeSg/9U62jXiY0VY8vt/kI=")
'    Private ssgw As Byte() = Utilties.ToBit("9TQ8qQ94JMZPSTjql/TJXA==")
'    Public Sub Michael(ByVal Gabriel As Dictionary(Of String, Object)) '用来整体保存整个数据列表的函数
'        For Each key As String In Gabriel.Keys
'            Add(Gabriel(key), key)
'        Next
'    End Sub
'    Private Function TryShoot() As AkazaAkane
'        Try

'        Catch ex As Exception
'            Return Nothing
'        End Try
'    End Function
'End Class

'Public Class _________________
'    Private Converters As New ConverterDictionary
'    Private ReferencePool As New Dictionary(Of String, XmlObject)
'    Private ObjectPool As New Dictionary(Of String, Object)
'    Private KeyPool As New Dictionary(Of String, String)

'    Private ReverseKeyPool As New Dictionary(Of String, String)
'    Private DevicePool As DeviceDictionary
'    Private Loaded As Boolean = False
'    Private Deserialized As Boolean = False
'    Private ShallowSerl As New ShallowDictionary(Of Integer, Integer)
'    Public Sub New()
'        For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
'            Converters.Add(cb)
'        Next
'    End Sub
'    Public Sub New(ByVal IncludePredefinedConverters As Boolean)
'        If IncludePredefinedConverters Then
'            For Each cb As ConverterBase In PreDefinedConverters.PreDefinedConverters
'                Converters.Add(cb)
'            Next
'        End If
'    End Sub

'    Public Sub AddConverter(ByVal vConverter As ConverterBase)
'        Converters.Add(vConverter)
'    End Sub
'    Public Sub RemoveConverter(ByVal vConverter As ConverterBase)
'        Converters.Remove(vConverter.TypeName)
'    End Sub
'    Public Function LoadXml(ByVal XmlRoot As Xml.XmlElement) As DeviceDictionary
'        If Loaded Or (XmlRoot Is Nothing) Then Return Nothing
'        Dim xKeys As Xml.XmlElement = Nothing
'        Dim xObjects As Xml.XmlElement = Nothing
'        For Each elm As Xml.XmlElement In XmlRoot.ChildNodes
'            If elm.Name = "XmlObjects" Then
'                xObjects = elm
'            End If
'            If elm.Name = "XmlKeys" Then
'                xKeys = elm
'            End If
'        Next
'        If (xKeys Is Nothing) Or (xObjects Is Nothing) Then Return Nothing
'        Dim vDevice As New HideDeviceDictionary
'        Dim xo As XmlObject
'        Dim vType As HideTypeInfo
'        Try
'            For Each elm As Xml.XmlElement In xKeys.ChildNodes
'                Dim k As String = elm.Attributes(0).Name.Substring(1)
'                KeyPool.Add(k, elm.Attributes(0).Value)
'                ReverseKeyPool.Add(elm.Attributes(0).Value, k)
'            Next

'            For Each inf As Xml.XmlElement In xObjects.ChildNodes
'                Dim val As Xml.XmlElement = inf.ChildNodes(0)
'                xo = New XmlObject
'                xo.TypeName = inf.GetAttribute("TypeName")
'                xo.AssemblyString = inf.GetAttribute("AssemblyString")
'                xo.AssemblyLocation = inf.GetAttribute("AssemblyLocation")
'                xo.IsDeviceDependent = CBool(inf.GetAttribute("IsDeviceDependent"))
'                Dim ID As String = inf.GetAttribute("ID")
'                ReferencePool.Add(ID, xo)
'                If xo.IsDeviceDependent Then
'                    vType = New HideTypeInfo(ReverseKeyPool(ID), xo.TypeName, xo.AssemblyString, xo.AssemblyLocation)
'                    vDevice.Add(vType.Key, vType)
'                End If
'                For Each att As Xml.XmlAttribute In val.Attributes
'                    'Dim k As String = att.Name
'                    'If Converters.ContainsKey(xo.TypeName) AndAlso k.StartsWith("X") Then k = k.Substring(1)
'                    xo.Fields.Add(att.Name, att.Value)
'                Next
'            Next
'        Catch ex As Exception
'            KeyPool.Clear()
'            ReverseKeyPool.Clear()
'            ReferencePool.Clear()
'            Return Nothing
'        End Try
'        DevicePool = vDevice
'        Loaded = True
'        Return DevicePool
'    End Function
'    Public Shared Function GetXmlRootFromXmlFile(ByVal vFilename As String) As Xml.XmlElement
'        If IO.File.Exists(vFilename) Then
'            Try
'                Dim fs As New IO.FileStream(vFilename, IO.FileMode.Open)
'                Dim xDoc As New Xml.XmlDocument
'                Try
'                    xDoc.Load(fs)
'                Catch ex As Exception
'                    fs.Close()
'                    Return Nothing
'                End Try
'                fs.Close()
'                Return xDoc.ChildNodes(0)
'            Catch ex As Exception
'                Return Nothing
'            End Try
'        Else
'            Return Nothing
'        End If
'    End Function
'    Public Shared Function GetXmlRootFromGZipFile(ByVal vFilename As String) As Xml.XmlElement
'        If IO.File.Exists(vFilename) Then
'            Try
'                Dim fs As New IO.FileStream(vFilename, IO.FileMode.Open)
'                Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Decompress)
'                Dim xDoc As New Xml.XmlDocument
'                Try
'                    xDoc.Load(gz)
'                Catch ex As Exception
'                    gz.Close()
'                    fs.Close()
'                    Return Nothing
'                End Try
'                gz.Close()
'                fs.Close()
'                Return xDoc.ChildNodes(0)
'            Catch ex As Exception
'                Return Nothing
'            End Try
'        Else
'            Return Nothing
'        End If
'    End Function
'    Public Function Michael(ByVal Gabriel As String, ByVal BlackList As List(Of String)) As Dictionary(Of String, Object)
'        Dim DVD As DeviceDictionary = LoadXml(GetXmlRootFromGZipFile(Gabriel))
'        Deserialize(DVD)
'        Dim Dict As New Dictionary(Of String, Object)
'        For Each key As String In BlackList
'            Dict.Add(key, GetObjectByKey(key))
'        Next
'        Return Dict
'    End Function
'    Private bbshoaglhlas As Byte() = Utilties.ToBit("SCItf/I4L6WoSA4j2UNajdeSg/9U62jXiY0VY8vt/kI=")
'    Private vhallhsl As Byte() = Utilties.ToBit("9TQ8qQ94JMZPSTjql/TJXA==")
'    Public Function Michael(ByVal Gabriel As Byte(), ByVal BlackList As List(Of String)) As Dictionary(Of String, Object)
'        Try
'            Dim ol As OnlyMyRailGunCanShoot
'            ol = Utilties.BitToObj(Utilties.AESDecryt(Gabriel, bbshoaglhlas, vhallhsl))
'            If ol.OK Then
'                If ol.Accelerator Then
'                    ShallowSerl = Utilties.BitToObj(ol.KamijouTouma)

'                Else
'                    'try to read from online

'                End If
'                Dim DVD As DeviceDictionary = LoadXml(LoadFromGZipBytes(ol.ShiraiKuroko))
'                Deserialize(DVD)
'            End If
'        Catch ex As Exception

'        End Try
'        Dim Dict As New Dictionary(Of String, Object)
'        For Each key As String In BlackList
'            Dict.Add(key, GetObjectByKey(key))
'        Next
'        Return Dict
'    End Function
'    Public Shared Function LoadFromGZipBytes(ByVal buf As Byte()) As Xml.XmlElement
'        Try
'            Dim xDoc As New Xml.XmlDocument
'            Dim fs As New IO.MemoryStream(buf)
'            Dim gz As New IO.Compression.GZipStream(fs, IO.Compression.CompressionMode.Decompress)
'            xDoc.Load(gz)
'            Dim XLE As Xml.XmlElement = xDoc.ChildNodes(0)
'            gz.Close()
'            fs.Close()
'            Return XLE
'        Catch ex As Exception
'            Return Nothing
'        End Try
'    End Function

'    Public Function Deserialize(ByVal localDevices As DeviceDictionary) As Boolean
'        If localDevices Is Nothing Then Exit Function

'        Deserialized = False
'        For Each ti As TypeInfo In localDevices.Values
'            If ti.Value Is Nothing Then
'                Deserialized = True
'                Exit For
'            End If
'        Next
'        If Deserialized Then Return False
'        For Each key As String In localDevices.Keys
'            ObjectPool.Add(KeyPool(key), localDevices(key).Value)
'        Next
'        For Each key As String In KeyPool.Keys
'            GetObject(KeyPool(key))
'        Next
'        Deserialized = True
'        Return Deserialized

'    End Function
'    Private AssemblyDict As New Dictionary(Of String, System.Reflection.Assembly)

'    Private Function GetObject(ByVal ID As String) As Object
'        If ID = -1 Then Return Nothing
'        Dim jjjj As Integer
'        If Integer.TryParse(ID, jjjj) Then
'            If ShallowSerl.ContainsKey(jjjj) Then Return ShallowSerl(jjjj)
'        End If
'        If ObjectPool.ContainsKey(ID) Then Return ObjectPool(ID)
'        Dim xo As XmlObject = ReferencePool(ID)
'        Dim xObj As Object
'        If Converters.ContainsKey(xo.TypeName) Then
'            Dim cv As Object = Converters(xo.TypeName)
'            xObj = cv.ConvertTo(xo)
'            ObjectPool.Add(ID, xObj)
'        Else
'            Dim ass As System.Reflection.Assembly
'            If AssemblyDict.ContainsKey(xo.AssemblyString) Then
'                ass = AssemblyDict(xo.AssemblyString)
'            Else
'                ass = System.Reflection.Assembly.Load(xo.AssemblyString)
'                If ass Is Nothing Then ass = System.Reflection.Assembly.LoadFile(xo.AssemblyLocation)
'                If ass Is Nothing Then
'                    Dim e As New Exception("Assembly does not exist.")
'                    Throw e
'                Else
'                    AssemblyDict.Add(xo.AssemblyString, ass)
'                End If
'            End If

'            Dim t As Type = Type.GetType(xo.TypeName)

'            If t Is Nothing Then t = ass.GetType(xo.TypeName)

'            Dim ci As System.Reflection.ConstructorInfo
'            'ci = t.GetConstructor(Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic, Nothing, System.Type.EmptyTypes, New System.Reflection.ParameterModifier() {})
'            ci = t.GetConstructor(System.Type.EmptyTypes)
'            Dim cis As System.Reflection.ConstructorInfo() = t.GetConstructors


'            If t.IsEnum Then
'                xObj = [Enum].Parse(t, xo.Fields("Enum"))
'                ObjectPool.Add(ID, xObj)
'            Else

'                If t.IsArray Then
'                    ci = cis(0)
'                Else
'                    If ci Is Nothing Then Throw New Exception(String.Format("Type {0} does not have a constructor without parameters.", t.AssemblyQualifiedName))
'                End If



'                If t.IsArray Then
'                    Dim Count As Integer = CInt(xo.Fields("Count"))
'                    xObj = ci.Invoke(New Object() {Count})
'                    ObjectPool.Add(ID, xObj)
'                    For j As Integer = 0 To Count - 1
'                        xObj(j) = GetObject(xo.Fields("V" + j.ToString))
'                    Next
'                ElseIf Not (t.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
'                    xObj = ci.Invoke(New Object() {})
'                    ObjectPool.Add(ID, xObj)
'                    Dim Count As Integer = CInt(xo.Fields("Count"))

'                    Dim iDic As Object = xObj
'                    For j As Integer = 0 To Count - 1
'                        iDic.Add(GetObject(xo.Fields("K" + j.ToString)), GetObject(xo.Fields("V" + j.ToString)))
'                    Next

'                ElseIf Not (t.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
'                    xObj = ci.Invoke(New Object() {})
'                    ObjectPool.Add(ID, xObj)
'                    Dim Count As Integer = CInt(xo.Fields("Count"))
'                    Dim iDic As Object = xObj
'                    For j As Integer = 0 To Count - 1
'                        iDic.Add(GetObject(xo.Fields("V" + j.ToString)))
'                    Next
'                Else
'                    xObj = ci.Invoke(New Object() {})
'                    ObjectPool.Add(ID, xObj)
'                    Dim IsDescribe As Boolean = True
'                    For Each att As Attribute In t.GetCustomAttributes(True)
'                        If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                            Dim ba As System.ComponentModel.BindableAttribute = att
'                            IsDescribe = ba.Bindable
'                        End If
'                        'If TypeOf att Is XmlDescribeAttribute Then
'                        '    Dim xdAtt As XmlDescribeAttribute = att
'                        '    IsDescribe = xdAtt.IsDescribe
'                        'End If
'                    Next

'                    Dim IsDescribeField As Boolean
'                    'xo.Fields.Add("DeepSerialization", "Yes")
'                    'xo.Fields.Add("ShallowSerialization", "No")
'                    If xo.Fields.ContainsKey("DeepSerialization") AndAlso xo.Fields("DeepSerialization") = "Yes" Then
'                        For Each f As System.Reflection.FieldInfo In t.GetFields(System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            'Dim obj As Object
'                            'If xo.Fields.ContainsKey("X" + f.Name) Then obj = 
'                            'If TypeOf obj Is Dictionary(Of Integer, Nuctions.GeneAnnotation) Then
'                            '    xObj.Features.AddRange(obj.Values)
'                            '    'If IsDescribeField And xo.Fields.ContainsKey("X" + f.Name) Then f.SetValue(xObj, obj)
'                            'Else
'                            If IsDescribeField And xo.Fields.ContainsKey("X" + f.Name) Then f.SetValue(xObj, GetObject(xo.Fields("X" + f.Name)))
'                            'End If
'                        Next
'                    End If
'                    If xo.Fields.ContainsKey("ShallowSerialization") AndAlso xo.Fields("ShallowSerialization") = "Yes" Then
'                        For Each f As System.Reflection.PropertyInfo In t.GetProperties(System.Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
'                            IsDescribeField = IsDescribe
'                            For Each att As Attribute In f.GetCustomAttributes(True)
'                                If TypeOf att Is System.ComponentModel.BindableAttribute Then
'                                    Dim ba As System.ComponentModel.BindableAttribute = att
'                                    IsDescribe = ba.Bindable
'                                End If
'                                'If TypeOf att Is XmlDescribeAttribute Then
'                                '    Dim XdAtt As XmlDescribeAttribute = att
'                                '    IsDescribeField = XdAtt.IsDescribe
'                                'End If
'                            Next
'                            If IsDescribeField And xo.Fields.ContainsKey("S" + f.Name) Then
'                                If f.CanWrite And f.GetIndexParameters.Length = 0 Then
'                                    'xo.Fields.Add("X" + f.Name, ParseObject(f.GetValue(source, New Object() {})))
'                                    f.SetValue(xObj, GetObject(xo.Fields("S" + f.Name)), New Object() {})
'                                End If
'                            End If

'                        Next
'                    End If
'                End If

'            End If
'        End If
'        Return xObj
'    End Function
'    Private Class HideDeviceDictionary
'        Inherits DeviceDictionary
'    End Class
'    Private Class HideTypeInfo
'        Inherits TypeInfo
'        Sub New(ByVal mKey As String, ByVal mTypeName As String, ByVal mAssemblyName As String, ByVal mAssemblyLocation As String)
'            vKey = mKey
'            vTypeName = mTypeName
'            vAssemblyName = mAssemblyName
'            vAssemblyLocation = mAssemblyLocation
'        End Sub
'    End Class
'    Public Function GetObjectByKey(ByVal key As String) As Object
'        If Deserialized AndAlso KeyPool.ContainsKey(key) AndAlso ObjectPool.ContainsKey(KeyPool(key)) Then
'            Return ObjectPool(KeyPool(key))
'        Else
'            Return Nothing
'        End If
'    End Function
'    Public ReadOnly Property Keys() As IEnumerable
'        Get
'            Return KeyPool.Keys
'        End Get
'    End Property
'    Public ReadOnly Property Objects() As IEnumerable
'        Get
'            Return ObjectPool.Values
'        End Get
'    End Property
'End Class

<Serializable()>
Public Class OnlyMyRailGunCanShoot
    Public Accelerator As Boolean
    Public KamijouTouma As Byte()
    Public ShiraiKuroko As Byte()
End Class

Public Class AsyncFilePortal
    Private CallBackSave As System.Action(Of Byte())
    Friend Sub New(data As OnlyMyRailGunCanShoot, cb As System.Action(Of Byte()))

        CallBackSave = cb
        Run()
    End Sub
    Private Async Sub Run()


    End Sub

    Private Function Authencate(data As Byte()) As System.Threading.Tasks.Task(Of Byte())

    End Function

End Class

Public Class PortalService
    Public Function Process(data As Byte()) As Byte()
        Dim u As New KanameMadoka

    End Function
End Class