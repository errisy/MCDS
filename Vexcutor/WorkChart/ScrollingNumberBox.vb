Public Class ScrollingNumberBox
    Inherits TextBox
    Public Event ValueChanged(ByVal sender As Object, ByVal e As EventArgs)

    Private _Value As Integer = 0
    Sub New()
        MyBase.New()
        Text = "0"
    End Sub
    <System.ComponentModel.Category("行为")> Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            _Value = value
            TextChanging = True
            Text = _Value.ToString
            TextChanging = False
            If DesignMode Then Exit Property
            RaiseEvent ValueChanged(Me, New EventArgs)
        End Set
    End Property
    Private _IncrementValue As Integer = 100
    <System.ComponentModel.Category("行为")> Public Property IncrementValue As Integer
        Get
            Return _IncrementValue
        End Get
        Set(ByVal value As Integer)
            _IncrementValue = value
        End Set
    End Property
    Private _Minimum As Integer = 0
    Private _Maximum As Integer = 4000
    <System.ComponentModel.Category("行为")> Public Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            _Minimum = value
        End Set
    End Property
    <System.ComponentModel.Category("行为")> Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            _Maximum = value
        End Set
    End Property
    Public Sub SetValueWithoutValueChangedEvent(ByVal Value As Integer)
        _Value = Value
        TextChanging = True
        Text = _Value.ToString
        TextChanging = False
    End Sub
    Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim newValue As Integer = _Value
        newValue += e.Delta / 120 * _IncrementValue
        If newValue > _Maximum Then newValue = _Maximum
        If newValue < _Minimum Then newValue = _Minimum
        If newValue <> _Value Then
            _Value = newValue
            TextChanging = True
            Text = _Value.ToString
            TextChanging = False
            RaiseEvent ValueChanged(Me, New EventArgs)
        End If
        'MyBase.OnMouseWheel(e)
    End Sub
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Select Case e.KeyChar
            Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"

            Case ControlChars.Back

            Case Else
                e.Handled = True
        End Select
        MyBase.OnKeyPress(e)
    End Sub
    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        If Text = "" Then
            Text = "0"
        End If
        MyBase.OnLostFocus(e)
    End Sub
    Private TextChanging As Boolean = False
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        If TextChanging Then Exit Sub
        Dim newValue As Integer = _Value
        If Text = "" Then
        ElseIf Text.Length > 0 Then
            Try
                newValue = CInt(Text)
                If newValue <> _Value Then
                    _Value = newValue
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            Catch ex As Exception
                _Value = 0
                TextChanging = True
                Text = "0"
                TextChanging = False
            End Try
        End If
        MyBase.OnTextChanged(e)
    End Sub
End Class
