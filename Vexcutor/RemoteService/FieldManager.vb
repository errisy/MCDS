Imports System.Windows.Forms

Public Class FieldManager
    Inherits Panel

    Private vType As Type
    Private vField2Control As New Dictionary(Of System.Reflection.FieldInfo, Control)
    Private vControl2Field As New Dictionary(Of Control, System.Reflection.FieldInfo)
    Private vProperty2Control As New Dictionary(Of System.Reflection.PropertyInfo, Control)
    Private vControl2Property As New Dictionary(Of Control, System.Reflection.PropertyInfo)
    Private vData As Object

    Private vDataSetting As Boolean = False

    Private vListManager As ListManager

    '用来提供给picturebox打开图片使用的



    <System.ComponentModel.Category("行为"), System.ComponentModel.Browsable(True)> Public Property ListManagerDataSource() As ListManager
        Get
            Return vListManager
        End Get
        Set(ByVal value As ListManager)
            If value Is vListManager Then

            Else
                If Not (vListManager Is Nothing) Then
                    '在丢弃前去掉关联
                    RemoveHandler vListManager.SelectDataChanged, AddressOf OnListManagerDataChanged
                End If


                vListManager = value
                '加入关联
                If Not (value Is Nothing) Then
                    AddHandler vListManager.SelectDataChanged, AddressOf OnListManagerDataChanged
                End If

            End If
        End Set
    End Property

    Public Sub OnListManagerDataChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Not (vListManager Is Nothing) Then
            LoadData(vListManager.SelectedData)
        End If

    End Sub

    Public Sub ControlSwitch(ByVal Enable As Boolean)
        For Each c As Control In Me.Controls
            c.Enabled = Enable
            If Not Enable Then SetValueToControl(Nothing, c)
        Next
    End Sub

    Public Sub LoadData(ByVal Value As Object)
        If Value Is Nothing Then
            ControlSwitch(False)
            Exit Sub
        Else
            ControlSwitch(True)
        End If
        vData = Value
        vType = Value.GetType
        vField2Control.Clear()
        vControl2Field.Clear()
        vProperty2Control.Clear()
        vControl2Property.Clear()

        vDataSetting = True
        For Each fld As System.Reflection.FieldInfo In vType.GetFields()
            For Each att As Attribute In fld.GetCustomAttributes(True)
                If TypeOf att Is FieldDefineAttribute Then
                    '建立数据映射关系
                    Dim fda As FieldDefineAttribute = att
                    If Controls.ContainsKey(fda.FieldName) Then
                        vField2Control.Add(fld, Me.Controls(fda.FieldName))
                        vControl2Field.Add(Me.Controls(fda.FieldName), fld)
                        SetValueToControl(fld.GetValue(Value), Me.Controls(fda.FieldName))
                    End If
                End If
            Next
        Next
        For Each ppt As System.Reflection.PropertyInfo In vType.GetProperties()
            For Each att As Attribute In ppt.GetCustomAttributes(True)
                If TypeOf att Is FieldDefineAttribute Then
                    Dim fda As FieldDefineAttribute = att
                    If Controls.ContainsKey(fda.FieldName) Then
                        vProperty2Control.Add(ppt, Me.Controls(fda.FieldName))
                        vControl2Property.Add(Me.Controls(fda.FieldName), ppt)
                        ppt.GetValue(Value, New Object() {Me.Controls(fda.FieldName), Nothing})
                    End If
                End If
            Next
        Next
        vDataSetting = False
    End Sub

    Private Sub FieldManager_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded
        If TypeOf e.Control Is TextBox Then
            AddHandler CType(e.Control, TextBox).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is LinkLabel Then
            AddHandler CType(e.Control, LinkLabel).LinkClicked, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is Label Then
            AddHandler CType(e.Control, Label).Click, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is PictureBox Then
            AddHandler CType(e.Control, PictureBox).MouseClick, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is ComboBox Then
            AddHandler CType(e.Control, ComboBox).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is CheckBox Then
            AddHandler CType(e.Control, CheckBox).CheckedChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is NumericUpDown Then
            AddHandler CType(e.Control, NumericUpDown).ValueChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is DateTimePicker Then
            AddHandler CType(e.Control, DateTimePicker).ValueChanged, AddressOf ValueChanged
        End If
    End Sub

    Private Sub ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        '
        If vDataSetting Then Exit Sub
        If TypeOf sender Is TextBox Then
            Dim obj As TextBox = sender
            If vControl2Field.ContainsKey(obj) Then
                vControl2Field(obj).SetValue(vData, obj.Text)
            ElseIf vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is LinkLabel Then
            Dim obj As LinkLabel = sender
            If vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is Label Then
            Dim obj As Label = sender
            If vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is PictureBox Then
            Dim obj As PictureBox = sender
            If vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is ComboBox Then
            If TypeOf sender Is ComboManager Then
                Dim obj As ComboManager = sender
                If vControl2Field.ContainsKey(obj) Then
                    vControl2Field(obj).SetValue(vData, obj.Value)
                ElseIf vControl2Property.ContainsKey(obj) Then
                    vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
                End If
            Else
                Dim obj As ComboBox = sender
                If vControl2Field.ContainsKey(obj) Then
                    vControl2Field(obj).SetValue(vData, obj.Text)
                ElseIf vControl2Property.ContainsKey(obj) Then
                    vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
                End If
            End If
        ElseIf TypeOf sender Is CheckBox Then
            Dim obj As CheckBox = sender
            If vControl2Field.ContainsKey(obj) Then
                vControl2Field(obj).SetValue(vData, obj.Checked)
            ElseIf vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is NumericUpDown Then
            Dim obj As NumericUpDown = sender
            If vControl2Field.ContainsKey(obj) Then
                vControl2Field(obj).SetValue(vData, obj.Value)
            ElseIf vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        ElseIf TypeOf sender Is DateTimePicker Then
            Dim obj As DateTimePicker = sender
            If vControl2Field.ContainsKey(obj) Then
                vControl2Field(obj).SetValue(vData, obj.Value)
            ElseIf vControl2Property.ContainsKey(obj) Then
                vControl2Property(obj).SetValue(vData, obj, New Object() {sender, e})
            End If
        End If
        If Not (vListManager Is Nothing) Then vListManager.UpdateSelectedDataRow()


        If TypeOf vData Is EditBase Then
            '这个判断就是为了标记那个数据被修改过了
            CType(vData, EditBase).Status = EditStatus.Modified
        End If
    End Sub

    Private Sub SetValueToControl(ByVal Value As Object, ByVal vControl As Control)
        If TypeOf vControl Is TextBox Then
            Dim obj As TextBox = vControl
            If Value Is Nothing Then
                obj.Text = ""
            Else
                obj.Text = Value.ToString
            End If
        ElseIf TypeOf vControl Is LinkLabel Then

        ElseIf TypeOf vControl Is Label Then

        ElseIf TypeOf vControl Is PictureBox Then
            Dim obj As PictureBox = vControl
            obj.BackgroundImage = Value
        ElseIf TypeOf vControl Is ComboBox Then
            Dim obj As ComboBox = vControl
            If TypeOf obj Is ComboManager Then
                '使用ComboManager要求必须有ToString定义
                If Value Is Nothing Then
                    obj.Text = ""
                Else
                    obj.Text = Value.ToString
                End If
            Else
                If Value Is Nothing Then
                    obj.Text = ""
                Else
                    obj.Text = Value.ToString
                End If
            End If
        ElseIf TypeOf vControl Is CheckBox Then
            Dim obj As CheckBox = vControl
            If Value Is Nothing Then
                obj.Checked = False
            Else
                obj.Checked = CBool(Value)
            End If
        ElseIf TypeOf vControl Is NumericUpDown Then
            Dim obj As NumericUpDown = vControl
            If Value Is Nothing Then
                obj.Value = obj.Minimum
            Else
                obj.Value = CDec(Value)
            End If
        ElseIf TypeOf vControl Is ListManager Then
            Dim obj As ListManager = vControl
            obj.LoadData(Value)
        ElseIf TypeOf vControl Is DateTimePicker Then
            Dim obj As DateTimePicker = vControl
            If TypeOf Value Is Date Then
                obj.Value = Value
            Else
                obj.Value = CDate(Value)
            End If
        End If
    End Sub

    Private Sub FieldManager_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlRemoved
        If TypeOf e.Control Is TextBox Then
            RemoveHandler CType(e.Control, TextBox).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is LinkLabel Then
            'AddHandler CType(e.Control, LinkLabel).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is Label Then
            'AddHandler CType(e.Control, Label).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is PictureBox Then
            RemoveHandler CType(e.Control, PictureBox).MouseClick, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is ComboBox Then
            RemoveHandler CType(e.Control, ComboBox).TextChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is CheckBox Then
            RemoveHandler CType(e.Control, CheckBox).CheckedChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is NumericUpDown Then
            RemoveHandler CType(e.Control, NumericUpDown).ValueChanged, AddressOf ValueChanged
        ElseIf TypeOf e.Control Is DateTimePicker Then
            RemoveHandler CType(e.Control, DateTimePicker).ValueChanged, AddressOf ValueChanged
        End If
    End Sub
End Class

Public Class FieldDefineAttribute
    Inherits Attribute  
    Public FieldName As String

    Public Sub New(ByVal vFieldName As String)
        FieldName = vFieldName
    End Sub
End Class

Public Class CarField
    <FieldDefine("tbName")> Public Name As String
    Public Model As String
    Public IsTruck As Boolean

    <FieldDefine("cbName")> Public Property cbTruck(ByVal obj As CheckBox, ByVal e As EventArgs) As CheckBox
        Get
            obj.Checked = IsTruck
            Return obj
        End Get
        Set(ByVal value As CheckBox)
            IsTruck = value.Checked
        End Set
    End Property
End Class

Public Class ComboManager
    Inherits ComboBox
    '在这个类当中 我们把combox和一个list manager关联
    Private vListManager As ListManager
    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("行为")> Public Property ListDataSource() As ListManager
        Get
            Return vListManager
        End Get
        Set(ByVal value As ListManager)
            If vListManager Is value Then
            Else
                If Not (vListManager Is Nothing) Then
                    RemoveHandler vListManager.ValueChanged, AddressOf OnSourceValueChanged
                End If
                If Not (value Is Nothing) Then
                    AddHandler value.ValueChanged, AddressOf OnSourceValueChanged
                End If
                vListManager = value
            End If

        End Set
    End Property

    Private vValueDict As Dictionary(Of String, Object)

    Public Sub OnSourceValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.Items.Clear()
        vValueDict = vListManager.ValueDict
        For Each key As String In vValueDict.Keys
            Me.Items.Add(key)
        Next
    End Sub
    Public ReadOnly Property Value() As Object
        Get
            If vValueDict Is Nothing Then
                Return Nothing
            ElseIf vValueDict.ContainsKey(Text) Then
                Return vValueDict(Text)
            Else
                Return Nothing
            End If
        End Get
    End Property
End Class


<Serializable()> Public MustInherit Class EditBase
    '这个类型是用来标记远程访问对象的 默认情况下是从远程加载进来的数据
    Public Status As EditStatus = EditStatus.Loaded
    Public MustOverride Function GetID() As Integer
    Public MustOverride Sub SetID(ByVal Value As Integer)
    Public Query As New Dictionary(Of String, ValueQuery)
End Class

<Serializable()> Public Enum EditStatus
    Loaded
    Created
    Modified
    Deleted
End Enum


Public Class ValueMonitor(Of T)
    Inherits ValueMonitorBase
    Private Map As New Dictionary(Of T, T)
    Private para As Object() = New Object() {}

    Public Sub New(ByVal vDict As System.Collections.Generic.Dictionary(Of Integer, T))
        Dim vType As Type = GetType(T)
        Dim ci As System.Reflection.ConstructorInfo = vType.GetConstructor(New Type() {})
        For Each obj As T In vDict.Values
            'Dim vClone As T = ci.Invoke(New Object() {})
            'PIs = vType.GetProperties()
            'For Each pi As System.Reflection.PropertyInfo In PIs
            '    pi.SetValue(vClone, Clone(pi.GetValue(obj, para)), para)
            'Next
            Map.Add(obj, Clone(obj))
        Next
    End Sub
    Public Function Clone(ByVal Obj As Object) As Object
        If Obj Is Nothing Then Return Nothing
        Dim vT As Type = Obj.GetType

        If vT.IsValueType Then
            '值传递的类型 一般可以直接比较
            Return Obj
        ElseIf vT Is GetType(String) Then
            Dim Copy As String = ""
            Copy &= Obj
        ElseIf Not (vT.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            Dim iDic1 As IDictionary = Obj
            Dim iDic2 As IDictionary = Copy
            Dim iD1 As IDictionaryEnumerator = iDic1.GetEnumerator

            iD1.Reset()

            While iD1.MoveNext
                iDic2.Add(Clone(iD1.Entry.Key), Clone(iD1.Entry.Value))
            End While
            Return Copy
        ElseIf Not (vT.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            Dim iE1 As IEnumerable = Obj
            Dim iEr1 As IEnumerator = iE1.GetEnumerator
            iEr1.Reset()
            While iEr1.MoveNext
                Copy.Add(iEr1.Current)
            End While
            Return Copy
        Else  '普通的类型
            Dim Copy As Object = vT.GetConstructor(New Type() {}).Invoke(para)
            For Each pi As System.Reflection.PropertyInfo In vT.GetProperties
                pi.SetValue(Copy, Clone(pi.GetValue(Obj, para)), para)
            Next
            Return Copy
        End If
        Return Obj
    End Function
    Public Overrides Function CheckValue(ByVal Value As Object) As Boolean
        If TypeOf Value Is T AndAlso Map.ContainsKey(Value) Then
            Dim Key As T = Value
            Dim Obj As T = Map(Key)
            For Each pi As System.Reflection.PropertyInfo In GetType(T).GetProperties
                If Not CompareValuePair(pi.GetValue(Key, para), pi.GetValue(Obj, para)) Then Return False
            Next
        End If
        Return True
    End Function

    Public Function CompareValuePair(ByVal V1 As Object, ByVal V2 As Object) As Boolean
        If V1 Is Nothing And V2 Is Nothing Then Return True
        If (V1 Is Nothing) Xor (V2 Is Nothing) Then Return False
        Dim T1 As Type = V1.GetType
        Dim T2 As Type = V2.GetType
        If Not (T1 Is T2) Then Return False

        If T1.IsValueType Then
            '值传递的类型 一般可以直接比较
            Return V1 = V2
        ElseIf Not (T1.GetInterface(GetType(IDictionary).ToString) Is Nothing) Then
            Dim iDic1 As IDictionary = V1
            Dim iDic2 As IDictionary = V2
            Dim iD1 As IDictionaryEnumerator = iDic1.GetEnumerator
            Dim iD2 As IDictionaryEnumerator = iDic2.GetEnumerator

            iD1.Reset()
            iD2.Reset()

            Dim N1 As Boolean
            Dim N2 As Boolean
            N1 = iD1.MoveNext
            N2 = iD2.MoveNext
            If Not (N1 = N2) Then Return False
            While N1 And N2
                If Not CompareValuePair(iD1.Entry.Key, iD2.Entry.Key) Then Return False
                If Not CompareValuePair(iD1.Entry.Value, iD2.Entry.Value) Then Return False
                N1 = iD1.MoveNext
                N2 = iD2.MoveNext
                If Not (N1 = N2) Then Return False
            End While
            Return True
        ElseIf Not (T1.GetInterface(GetType(IEnumerable).ToString) Is Nothing) Then
            Dim iE1 As IEnumerable = V1
            Dim iE2 As IEnumerable = V2
            Dim iEr1 As IEnumerator = iE1.GetEnumerator
            Dim iEr2 As IEnumerator = iE2.GetEnumerator

            iEr1.Reset()
            iEr2.Reset()

            Dim N1 As Boolean
            Dim N2 As Boolean
            N1 = iEr1.MoveNext
            N2 = iEr2.MoveNext
            If Not (N1 = N2) Then Return False
            While N1 And N2
                If Not CompareValuePair(iEr1.Current, iEr2.Current) Then Return False
                N1 = iEr1.MoveNext
                N2 = iEr2.MoveNext
                If Not (N1 = N2) Then Return False
            End While
            Return True
        ElseIf Not (V1 Is V2) Then '普通的类型
            For Each pi As System.Reflection.PropertyInfo In T1.GetProperties
                If Not CompareValuePair(pi.GetValue(V1, para), pi.GetValue(V2, para)) Then Return False
            Next
            Return True
        Else
            Return True
        End If
    End Function
End Class
