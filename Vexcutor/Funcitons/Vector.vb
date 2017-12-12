Public Structure Vector
    Public X As Single
    Public Y As Single
    Public Sub New(ByVal vX As Single, ByVal vY As Single)
        X = vX
        Y = vY
    End Sub
    Public Shared Widening Operator CType(ByVal vPoint As PointF) As Vector
        Return New Vector(vPoint.X, vPoint.Y)
    End Operator
    Public Shared Widening Operator CType(ByVal vPoint As Point) As Vector
        Return New Vector(vPoint.X, vPoint.Y)
    End Operator
    Public Shared Narrowing Operator CType(ByVal vVector As Vector) As PointF
        Return New PointF(vVector.X, vVector.Y)
    End Operator
    Public Shared Narrowing Operator CType(ByVal vVector As Vector) As Point
        Return New Point(vVector.X, vVector.Y)
    End Operator
    Public Shared Operator -(ByVal vVector1 As Vector, ByVal vVector2 As Vector) As Vector
        Return New Vector(vVector1.X - vVector2.X, vVector1.Y - vVector2.Y)
    End Operator
    Public Shared Operator +(ByVal vVector1 As Vector, ByVal vVector2 As Vector) As Vector
        Return New Vector(vVector1.X + vVector2.X, vVector1.Y + vVector2.Y)
    End Operator
    Public Shared Operator *(ByVal vVector1 As Vector, ByVal vValue As Single) As Vector
        Return New Vector(vVector1.X * vValue, vVector1.Y * vValue)
    End Operator
    Public Shared Operator /(ByVal vVector1 As Vector, ByVal vValue As Single) As Vector
        Return New Vector(vVector1.X / vValue, vVector1.Y / vValue)
    End Operator
End Structure
