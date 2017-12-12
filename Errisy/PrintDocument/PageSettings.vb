
Public Class PageSettings
    Public Const PointsPerMillimeter As Double = 3.779527559

    Public Shared Function PageSizes() As System.Collections.ObjectModel.ObservableCollection(Of PageSize)
        Dim sizes As New System.Collections.ObjectModel.ObservableCollection(Of PageSize)
        sizes.Add(New PageSize With {.Title = "A4", .Width = 210, .Height = 297, .RealWidth = 793.70078740157476#, .RealHeight = 1122.5196850393702#})
        sizes.Add(New PageSize With {.Title = "B4", .Width = 250, .Height = 353, .RealWidth = 971.33858267716528#, .RealHeight = 1375.748031496063#})
        Return sizes
    End Function
    Public Shared Function PageMargins() As System.Collections.ObjectModel.ObservableCollection(Of PageMargin)
        Dim margins As New System.Collections.ObjectModel.ObservableCollection(Of PageMargin)
        margins.Add(New PageMargin With {.Title = "Brief", .Left = 8.0#, .Top = 8.0#, .Right = 8.0#, .Bottom = 8.0#})
        margins.Add(New PageMargin With {.Title = "Narrow", .Left = 12.7#, .Top = 12.7#, .Right = 12.7#, .Bottom = 12.7#})
        margins.Add(New PageMargin With {.Title = "Moderate", .Left = 19.1#, .Top = 25.4#, .Right = 19.1#, .Bottom = 25.4#})
        margins.Add(New PageMargin With {.Title = "Normal", .Left = 31.8#, .Top = 25.4#, .Right = 31.8#, .Bottom = 25.4#})
        Return margins
    End Function
End Class

Public Class PageSize
    Public Property Title As String
    Public Property Width As Integer
    Public Property Height As Integer

    Public Property RealWidth As Double
    Public Property RealHeight As Double

    Public ReadOnly Property Description As String
        Get
            Return ToString()
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0}: {1}mm x {2}mm", Title, Width, Height)
    End Function
End Class

Public Class PageMargin
    Public Property Title As String
    Public Property Left As Double
    Public Property Top As Double
    Public Property Right As Double
    Public Property Bottom As Double
    Public ReadOnly Property Description As String
        Get
            Return ToString()
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0}: {1}mm {2}mm {3}mm {4}mm", Title, Left.ToString("0.0"), Top.ToString("0.0"), Right.ToString("0.0"), Bottom.ToString("0.0"))
    End Function
End Class