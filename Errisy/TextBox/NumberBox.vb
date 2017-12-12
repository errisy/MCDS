Public Class NumberBox
    Inherits System.Windows.Controls.TextBox
    Public Sub New()
        Text = "0"
    End Sub

    'NumberBox->AllowDecimal As Boolean with Event Default: True
    Public Property AllowDecimal As Boolean
        Get
            Return GetValue(AllowDecimalProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(AllowDecimalProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AllowDecimalProperty As DependencyProperty = _
                           DependencyProperty.Register("AllowDecimal", _
                           GetType(Boolean), GetType(NumberBox), _
                           New PropertyMetadata(True, New PropertyChangedCallback(AddressOf SharedAllowDecimalChanged)))
    Private Shared Sub SharedAllowDecimalChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, NumberBox).AllowDecimalChanged(d, e)
    End Sub
    Private Sub AllowDecimalChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not e.NewValue Then
            Text = Text.Replace(".", "")
        End If
    End Sub
    'NumberBox->AllowNegative As Boolean with Event Default: True
    Public Property AllowNegative As Boolean
        Get
            Return GetValue(AllowNegativeProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(AllowNegativeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AllowNegativeProperty As DependencyProperty = _
                           DependencyProperty.Register("AllowNegative", _
                           GetType(Boolean), GetType(NumberBox), _
                           New PropertyMetadata(True, New PropertyChangedCallback(AddressOf SharedAllowNegativeChanged)))
    Private Shared Sub SharedAllowNegativeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, NumberBox).AllowNegativeChanged(d, e)
    End Sub
    Private Sub AllowNegativeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not e.NewValue Then
            Text = Text.Replace("-", "")
        End If
    End Sub
 
    Protected Overrides Sub OnKeyDown(e As System.Windows.Input.KeyEventArgs)
        Dim sel As Integer
        Select Case e.Key
            Case Windows.Input.Key.Subtract, Windows.Input.Key.OemMinus
                If AllowNegative AndAlso Not Text.Contains("-") Then
                    sel = SelectionStart
                    Text = "-" + Text
                    SelectionStart = IIf(sel + 1 < Text.Length, sel + 1, Text.Length)
                End If
                e.Handled = True
            Case Windows.Input.Key.Add, Windows.Input.Key.OemPlus
                If Text.Contains("-") Then
                    sel = SelectionStart
                    Text = Text.Replace("-", "")
                    SelectionStart = IIf(sel - 1 > -1, sel - 1, 0)
                End If
                e.Handled = True
            Case Windows.Input.Key.Decimal, Windows.Input.Key.OemPeriod
                If AllowDecimal Then
                    sel = SelectionStart
                    Dim di As Integer = Text.IndexOf(".")
                    If di > -1 Then
                        Text = Text.Replace(".", "")
                        SelectionStart = IIf(di < sel, sel - 1, sel)
                    End If
                End If
            Case Windows.Input.Key.Delete

            Case Windows.Input.Key.Back

            Case Windows.Input.Key.D0, Windows.Input.Key.D1, Windows.Input.Key.D2, Windows.Input.Key.D3, Windows.Input.Key.D4, Windows.Input.Key.D5, Windows.Input.Key.D6, Windows.Input.Key.D7, Windows.Input.Key.D8, Windows.Input.Key.D9
            Case Key.NumPad0 To Key.NumPad9
            Case Else
                e.Handled = True
        End Select
        MyBase.OnKeyDown(e)
    End Sub
    Protected Overrides Sub OnTextChanged(e As System.Windows.Controls.TextChangedEventArgs)
        _SettingValue = True
        SetValue(ValueProperty, InnerValue)
        _SettingValue = False
        MyBase.OnTextChanged(e)
    End Sub
    'NumberBox->NumberType As Type with Event Default: GetType(Integer)
    Public Property NumberType As Type
        Get
            Return GetValue(NumberTypeProperty)
        End Get
        Set(ByVal value As Type)
            SetValue(NumberTypeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly NumberTypeProperty As DependencyProperty = _
                           DependencyProperty.Register("NumberType", _
                           GetType(Type), GetType(NumberBox), _
                           New PropertyMetadata(GetType(Integer), New PropertyChangedCallback(AddressOf SharedNumberTypeChanged)))
    Private Shared Sub SharedNumberTypeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, NumberBox).NumberTypeChanged(d, e)
    End Sub
    Private Sub NumberTypeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    End Sub



    'NumberBox -> Value As Object Default: Nothing
    Public Property Value As Object
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(ByVal value As Object)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Public Shared ReadOnly ValueProperty As DependencyProperty = _
                            DependencyProperty.Register("Value", _
                            GetType(Object), GetType(NumberBox), _
                            New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf SharedValueChanged)))
    Private Shared Sub SharedValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, NumberBox).ValueChanged(d, e)
    End Sub
    Private Sub ValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If _SettingValue Then Exit Sub
        InnerValue = e.NewValue
    End Sub
    Private _SettingValue As Boolean = False
    Private Property InnerValue As Object
        Get
            If Text Is Nothing OrElse Text.Length = 0 Then Return 0
            Select Case NumberType
                Case GetType(Int32)
                    Dim i As Integer
                    If Integer.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Int64)
                    Dim i As Long
                    If Long.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Int16)
                    Dim i As Short
                    If Short.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Byte)
                    Dim i As Byte
                    If Byte.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(UInt32)
                    Dim i As UInteger
                    If UInteger.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(UInt64)
                    Dim i As ULong
                    If ULong.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(UInt16)
                    Dim i As UShort
                    If UShort.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Single)
                    Dim i As Single
                    If Single.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Double)
                    Dim i As Double
                    If Double.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case GetType(Decimal)
                    Dim i As Decimal
                    If Decimal.TryParse(Text, i) Then
                        Return i
                    Else
                        Return 0
                    End If
                Case Else
                    Return 0
            End Select
        End Get
        Set(value As Object)
            If TypeOf value Is Integer Then
                NumberType = GetType(Integer)
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Long Then
                NumberType = GetType(Long)
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Short Then
                NumberType = GetType(Short)
                AllowDecimal = False
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Byte Then
                NumberType = GetType(Byte)
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is UInteger Then
                NumberType = GetType(UInteger)
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is ULong Then
                NumberType = GetType(ULong)
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is UShort Then
                NumberType = GetType(UShort)
                AllowDecimal = False
                AllowNegative = False
                Text = value.ToString
            ElseIf TypeOf value Is Single Then
                NumberType = GetType(Single)
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Double Then
                NumberType = GetType(Double)
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            ElseIf TypeOf value Is Decimal Then
                NumberType = GetType(Decimal)
                AllowDecimal = True
                AllowNegative = True
                Text = value.ToString
            End If
        End Set
    End Property
    Protected Overrides Sub OnMouseWheel(e As System.Windows.Input.MouseWheelEventArgs)
        Dim i As Integer
        Try
            If Integer.TryParse(Text, i) Then
                Dim j As Integer = i + e.Delta / 120
                If Not AllowNegative And j < 0 Then j = 0
                Text = j.ToString
            End If
        Catch ex As Exception

        End Try
        _SettingValue = True
        Value = InnerValue
        _SettingValue = False
        MyBase.OnMouseWheel(e)
    End Sub
End Class
