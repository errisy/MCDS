Public Class LambdaComparer(Of T)
    Implements System.Collections.Generic.IComparer(Of T)
    Dim _Exp As Object
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Integer)))
        _Exp = exp.Compile
    End Sub
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Single)))
        _Exp = exp.Compile
    End Sub
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Double)))
        _Exp = exp.Compile
    End Sub
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Byte)))
        _Exp = exp.Compile
    End Sub
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Short)))
        _Exp = exp.Compile
    End Sub
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Decimal)))
        _Exp = exp.Compile
    End Sub
    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Return Math.Sign(_Exp.Invoke(x) - _Exp.Invoke(y))
    End Function
End Class

Public Class LambdaIntegerComparer(Of T)
    Implements System.Collections.Generic.IComparer(Of T)
    Dim _Exp As Func(Of T, Integer)
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Integer)))
        _Exp = exp.Compile
    End Sub
    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Return Math.Sign(_Exp(y) - _Exp(x))
    End Function
End Class

Public Class LambdaDoubleComparer(Of T)
    Implements System.Collections.Generic.IComparer(Of T)
    Dim _Exp As Func(Of T, Double)
    Public Sub New(exp As Expressions.Expression(Of Func(Of T, Double)))
        _Exp = exp.Compile
    End Sub
    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Dim _x As Double = _Exp(x)
        Dim _y As Double = _Exp(y)
        If Double.IsNaN(_x) Or Double.IsNegativeInfinity(_x) Then _x = Double.MinValue
        If Double.IsNaN(_y) Or Double.IsNegativeInfinity(_y) Then _y = Double.MinValue
        Return Math.Sign(_y - _x)
    End Function
End Class