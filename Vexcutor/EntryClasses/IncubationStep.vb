Public Class IncubationStep
    Public Medium As String
    Public IsPlate As Boolean = False
    Public Temperature As Single = 37.0F
    Public Antibiotics As New List(Of Nuctions.Antibiotics)
    Public Inducer As String
    Public Time As TimeSpan = TimeSpan.Zero
    Public Function Clone() As IncubationStep
        Dim ibc As New IncubationStep With {.Medium = Medium, .IsPlate = IsPlate, .Temperature = Temperature, .Inducer = Inducer, .Time = Time}
        ibc.Antibiotics.AddRange(Antibiotics)
        Return ibc
    End Function
    Public ReadOnly Property AntibioticsKeys As List(Of String)
        Get
            Dim stb As New List(Of String)
            For Each ant As Nuctions.Antibiotics In Antibiotics
                stb.Add([Enum].GetName(GetType(Nuctions.Antibiotics), ant).ToLower)
            Next
            Return stb
        End Get
    End Property
    Public Property AntibioticsValue As String
        Get
            Dim stb As New System.Text.StringBuilder
            For Each ant As Nuctions.Antibiotics In Antibiotics
                stb.Append([Enum].GetName(GetType(Nuctions.Antibiotics), ant))
                stb.Append(" ")
            Next
            Return stb.ToString
        End Get
        Set(value As String)
            Dim at As New List(Of String)
            Dim v As String = value.ToLower
            Antibiotics.Clear()
            For Each ant As Nuctions.Antibiotics In Antibiotics
                If v.Contains([Enum].GetName(GetType(Nuctions.Antibiotics), ant).ToLower) Then
                    Antibiotics.Add(ant)
                End If
            Next
        End Set
    End Property
End Class