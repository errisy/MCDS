
Public Structure IntegerRegion
    Public Start As Integer
    Public [End] As Integer
    Public Shared Function FindRegions(IsCircular As Boolean, Complement As Boolean, StartPosition As Integer, EndPosition As Integer,
                                       StartIndex As Integer, NucleotidesPerRow As Integer, Length As Integer) As List(Of IntegerRegion)
        Dim RegionList As New List(Of IntegerRegion)
        Dim _Start As Integer
        Dim _End As Integer
        Dim _RegionStart As Integer
        Dim _RegionEnd As Integer
        If IsCircular Then
            If StartPosition > EndPosition Then
                _Start = 1
                _End = EndPosition
                _RegionStart = Math.Max(StartIndex, _Start)
                _RegionEnd = Math.Min(StartIndex + NucleotidesPerRow - 1, _End)
                If _RegionEnd >= _RegionStart Then
                    RegionList.Add(New IntegerRegion With {.Start = _RegionStart, .End = _RegionEnd})
                End If
                _Start = StartPosition
                _End = Length
                _RegionStart = Math.Max(StartIndex, _Start)
                _RegionEnd = Math.Min(StartIndex + NucleotidesPerRow - 1, _End)
                If _RegionEnd >= _RegionStart Then
                    RegionList.Add(New IntegerRegion With {.Start = _RegionStart, .End = _RegionEnd})
                End If
            Else
                _Start = StartPosition
                _End = EndPosition
                _RegionStart = Math.Max(StartIndex, _Start)
                _RegionEnd = Math.Min(StartIndex + NucleotidesPerRow - 1, _End)
                If _RegionEnd >= _RegionStart Then
                    RegionList.Add(New IntegerRegion With {.Start = _RegionStart, .End = _RegionEnd})
                End If
            End If
        Else
            If Complement Then
                _Start = EndPosition
                _End = StartPosition
                _RegionStart = Math.Max(StartIndex, _Start)
                _RegionEnd = Math.Min(StartIndex + NucleotidesPerRow - 1, _End)
                If _RegionEnd >= _RegionStart Then
                    RegionList.Add(New IntegerRegion With {.Start = _RegionStart, .End = _RegionEnd})
                End If
            Else
                _Start = StartPosition
                _End = EndPosition
                _RegionStart = Math.Max(StartIndex, _Start)
                _RegionEnd = Math.Min(StartIndex + NucleotidesPerRow - 1, _End)
                If _RegionEnd >= _RegionStart Then
                    RegionList.Add(New IntegerRegion With {.Start = _RegionStart, .End = _RegionEnd})
                End If
            End If
        End If
        Return RegionList
    End Function

    Public Shared Operator And(ir1 As IntegerRegion, ir2 As IntegerRegion) As Boolean
        '10 - 20
        '1-3 3-12, 12-15, 15-22, 22-50
        Return ir1.End >= ir2.Start And ir2.End >= ir1.Start
    End Operator
End Structure

Public Class RowRegions
    Inherits List(Of IntegerRegion)
    Public Function IntersectsWith(ir As IntegerRegion) As Boolean
        For Each i In Me
            If (i And ir) Then Return True
        Next
        Return False
    End Function
End Class

Public Class RowSets
    Inherits List(Of RowRegions)
    Public Function Fit(ir As IntegerRegion) As Integer
        Dim row As Integer = -1
        Do
            row += 1
            If row >= Count Then Add(New RowRegions)
        Loop While Me(row).IntersectsWith(ir)
        Me(row).Add(ir)
        Return row
    End Function
End Class