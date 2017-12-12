Public Class RadioButtonMap
    Inherits Dictionary(Of Integer, RadioButton)
    Public Property Value() As Integer
        Get
            For Each i As Integer In Keys
                If Item(i).Checked Then Return i
            Next
        End Get
        Set(ByVal value As Integer)
            Dim obj As RadioButton = Nothing
            TryGetValue(value, obj)
            If Not (obj Is Nothing) Then obj.Checked = True
        End Set
    End Property
End Class
