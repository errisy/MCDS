Public Module UntilityFunctions
    <Runtime.CompilerServices.Extension> Public Function ContainsAnyOf(Value As String, Keywords As IEnumerable(Of String)) As Boolean
        For Each keyword In Keywords
            If Value.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) > -1 Then Return True
        Next
        Return False
    End Function
    <Runtime.CompilerServices.Extension> Public Function ContainsAllOf(Value As String, Keywords As IEnumerable(Of String)) As Boolean
        For Each keyword In Keywords
            If Not Value.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) > -1 Then Return False
        Next
        Return True
    End Function
End Module
