Module IOUtil
    Friend CurentDirectory As System.IO.DirectoryInfo
    Friend CurrentFile As System.IO.FileInfo
    Friend FileList As New List(Of IO.FileInfo)
    Friend Const QRT2 As Double = (5 ^ 0.5 - 1) ^ (1 / 2)
    Public Function ApplyFile(filename As String) As Boolean
        Try
            If IO.File.Exists(filename) Then
                Dim fi As New IO.FileInfo(filename)
                CurentDirectory = fi.Directory
                FileList.AddRange(CurentDirectory.GetFiles("*.jpg"))
                FileList.AddRange(CurentDirectory.GetFiles("*.png"))
                FileList.AddRange(CurentDirectory.GetFiles("*.gif"))
                FileList.AddRange(CurentDirectory.GetFiles("*.bmp"))
                Dim fs = FileList.Where(Function(f) f.FullName = fi.FullName).ToArray
                CurrentFile = fs(0)
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetFileByIndex(i As Integer) As IO.FileInfo
        If Not FileList.Any Then Return Nothing
        While i >= FileList.Count
            i -= FileList.Count
        End While
        While i < 0
            i += FileList.Count
        End While
        Return FileList(i)
    End Function
End Module
Public Class Maneuver
    Public Property MoveSpeed As Vector = New Vector(0.0#, 0.0#)
    Public Property MoveAccelerator As Vector = New Vector(0.0#, 0.0#)
    Public Property ZoomSpeed As Double = 0.0#
    Public Property ZoomAccelerator As Double = 0.0#
    Public Property ZoomCenter As Point
    Public Property RotateSpeed As Double = 0.0#
    Public Property RotateAccelerator As Double = 0.0#
    Public Property NextFile As Integer
    Public Property NextFileCountDown As Integer
    Public Property Alive As Boolean = True
    Public Overridable Sub Tick()

    End Sub
    Public Sub BounceX()
        MoveSpeed = New Vector(-MoveSpeed.X, MoveSpeed.Y)
        MoveAccelerator = New Vector(-MoveAccelerator.X, MoveAccelerator.Y)
    End Sub
    Public Sub BounceY()
        MoveSpeed = New Vector(MoveSpeed.X, -MoveSpeed.Y)
        MoveAccelerator = New Vector(MoveAccelerator.X, -MoveAccelerator.Y)
    End Sub
End Class

Public Class StoppingManeuver
    Inherits Maneuver
    Public Overrides Sub Tick()
        Dim MoveAlive As Boolean = True
        Dim ZoomAlive As Boolean = True
        Dim RotateAlive As Boolean = True
        If MoveSpeed.Length > (MoveSpeed + MoveAccelerator).Length Then
            MoveSpeed += MoveAccelerator
        Else
            MoveSpeed = New Vector(0.0#, 0.0#)
            MoveAlive = False
        End If
        If Math.Abs(ZoomSpeed) > Math.Abs(ZoomSpeed + ZoomAccelerator) Then
            ZoomSpeed += ZoomAccelerator
        Else
            ZoomSpeed = 0.0#
            ZoomAlive = False
        End If
        If Math.Abs(RotateSpeed) > Math.Abs(RotateSpeed + RotateAccelerator) Then
            RotateSpeed += RotateAccelerator
        Else
            RotateSpeed = 0.0#
            RotateAlive = False
        End If
        Alive = MoveAlive Or ZoomAlive Or RotateAlive
    End Sub
End Class


Public Class ForeverManeuver
    Inherits Maneuver
    Public Overrides Sub Tick()
        MoveSpeed += MoveAccelerator
        ZoomSpeed += ZoomAccelerator
        RotateSpeed += RotateAccelerator
    End Sub
End Class

