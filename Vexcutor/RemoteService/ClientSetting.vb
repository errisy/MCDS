<Serializable()> Public Class ClientSetting
    Public Accounts As New List(Of Account)
    Public Sub AddAccount(ByVal acc As Account)
        Dim contains As Boolean = False

        For Each a As Account In Accounts
            If a.IP = acc.IP AndAlso a.UserName = acc.UserName AndAlso a.Password = acc.Password Then
                contains = True
            End If
        Next
        If Not contains Then Accounts.Add(acc)
    End Sub
End Class

<Serializable()> Public Class Account
    Public IP As String
    Public UserName As String
    Public Password As String
    Public Overrides Function ToString() As String
        Return IP
    End Function
End Class
