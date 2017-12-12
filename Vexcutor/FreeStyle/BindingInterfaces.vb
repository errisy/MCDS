Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes
Public Interface IPointBind
    Property BindingPoints As ShallowDictionary(Of ControlPointMapping, Double)
    ReadOnly Property Adorners As System.Collections.Generic.Dictionary(Of String, ControlPoint)
End Interface
Public Interface IAbsoluteLineBind
    Inherits IPointBind
    Property LinearX As System.Windows.Point
    Property LinearY As System.Windows.Point
    Property Rotation As Double
    Property ActualSize As System.Windows.Vector
    Function TempSender() As Object
End Interface
Public Module MAbsoluteLineBind
    <System.Runtime.CompilerServices.Extension()> Public Function BindPoint(iBind As IAbsoluteLineBind, cp As ControlPoint) As Boolean
        If iBind.Adorners.ContainsValue(cp) Then Return False
        Dim ct As System.Windows.Vector = (iBind.LinearX.AsVector + iBind.LinearY.AsVector) / 2
        Dim az As System.Windows.Vector = iBind.ActualSize / 2
        Dim p As System.Windows.Vector = cp.Position

        Dim lp As System.Windows.Vector = ct + V(-az.X, 0).RotateByDegree(iBind.Rotation)
        Dim rp As System.Windows.Vector = ct + V(az.X, 0).RotateByDegree(iBind.Rotation)

        Dim bx As System.Windows.Vector = V(1, 0).RotateByDegree(iBind.Rotation)
        Dim by As System.Windows.Vector = V(0, 1).RotateByDegree(iBind.Rotation)
        Dim x1 As Double = bx.X * lp.X + bx.Y * lp.Y
        Dim x2 As Double = bx.X * rp.X + bx.Y * rp.Y
        Dim y As Double = by.X * lp.X + by.Y * lp.Y
        Dim xl As Double = Math.Min(x1, x2)
        Dim xr As Double = Math.Max(x1, x2)
        p = V(bx.X * p.X + bx.Y * p.Y, by.X * p.X + by.Y * p.Y)
        If Math.Abs(p.Y - y) < 6D AndAlso p.X > xl - 6D AndAlso p.X < xr + 6D Then
            If p.X < xl Then p.X = xl
            If p.X > xr Then p.X = xr
            Dim t As Double = Math.Abs(p.X - x1)
            iBind.BindingPoints.Add(cp.Mapping(iBind), t)
            cp.BindingTarget.AdornerLocation(iBind, cp.ID) = iBind.LinearX + V(t, 0).RotateByDegree(iBind.Rotation)
            'cp.PositionMove(iBind, , True)
            Return True
        End If
        Return False
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Function RelativeBindPoint(iBind As IAbsoluteLineBind, cp As ControlPoint) As Boolean
        If iBind.Adorners.ContainsValue(cp) Then Return False
        Dim ct As System.Windows.Vector = (iBind.LinearX.AsVector + iBind.LinearY.AsVector) / 2
        Dim az As System.Windows.Vector = iBind.ActualSize / 2
        Dim p As System.Windows.Vector = cp.Position

        Dim lp As System.Windows.Vector = ct + V(-az.X, 0).RotateByDegree(iBind.Rotation)
        Dim rp As System.Windows.Vector = ct + V(az.X, 0).RotateByDegree(iBind.Rotation)

        Dim bx As System.Windows.Vector = V(1, 0).RotateByDegree(iBind.Rotation)
        Dim by As System.Windows.Vector = V(0, 1).RotateByDegree(iBind.Rotation)
        Dim x1 As Double = bx.X * lp.X + bx.Y * lp.Y
        Dim x2 As Double = bx.X * rp.X + bx.Y * rp.Y
        Dim y As Double = by.X * lp.X + by.Y * lp.Y
        Dim xl As Double = Math.Min(x1, x2)
        Dim xr As Double = Math.Max(x1, x2)
        p = V(bx.X * p.X + bx.Y * p.Y, by.X * p.X + by.Y * p.Y)
        If Math.Abs(p.Y - y) < 6D AndAlso p.X > xl - 6D AndAlso p.X < xr + 6D Then
            If p.X < xl Then p.X = xl
            If p.X > xr Then p.X = xr
            Dim t As Double = (p.X - x1) / (x2 - x1)
            iBind.BindingPoints.Add(cp.Mapping(iBind), t)
            cp.BindingTarget.AdornerLocation(iBind, cp.ID) = iBind.LinearX + V(t * (x2 - x1), 0).RotateByDegree(iBind.Rotation)
            'cp.PositionMove(iBind, , True)
            Return True
        End If
        Return False
    End Function
    <System.Runtime.CompilerServices.Extension()> Public Sub ProcessRelativeBindingPoints(iBind As IAbsoluteLineBind)
        Dim t As Double
        Dim ct As System.Windows.Vector = (iBind.LinearX.AsVector + iBind.LinearY.AsVector) / 2
        Dim az As System.Windows.Vector = iBind.ActualSize / 2
        Dim lp As System.Windows.Vector = ct + V(-az.X, 0).RotateByDegree(iBind.Rotation)
        Dim rp As System.Windows.Vector = ct + V(az.X, 0).RotateByDegree(iBind.Rotation)
        Dim bx As System.Windows.Vector = V(1, 0).RotateByDegree(iBind.Rotation)
        Dim by As System.Windows.Vector = V(0, 1).RotateByDegree(iBind.Rotation)
        Dim x1 As Double = bx.X * lp.X + bx.Y * lp.Y
        Dim x2 As Double = bx.X * rp.X + bx.Y * rp.Y
        For Each cp In iBind.BindingPoints.Keys
            t = iBind.BindingPoints(cp)
            If iBind.TempSender IsNot cp.Target Then
                cp.Target.AdornerLocation(iBind.TempSender, cp.Point.ID) = iBind.LinearX + V(t * (x2 - x1), 0).RotateByDegree(iBind.Rotation)
            End If
        Next
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub ReleasePoint(iBind As IAbsoluteLineBind, cp As ControlPoint)
        Dim keys = iBind.BindingPoints.Where(Function(kvp) kvp.Key.Point Is cp).Select(Function(kvp) kvp.Key).ToArray
        For Each Key In keys
            If iBind.BindingPoints.ContainsKey(Key) Then iBind.BindingPoints.Remove(Key)
        Next
    End Sub
    <System.Runtime.CompilerServices.Extension()> Public Sub ProcessBindingPoints(iBind As IAbsoluteLineBind)
        Dim t As Double
        For Each cp In iBind.BindingPoints.Keys
            t = iBind.BindingPoints(cp)
            If iBind.TempSender IsNot cp.Target Then
                cp.Target.AdornerLocation(iBind.TempSender, cp.Point.ID) = iBind.LinearX + V(t, 0).RotateByDegree(iBind.Rotation)
            End If
        Next
    End Sub
End Module