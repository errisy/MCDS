Module WPFUIDialogExtension
    <System.Runtime.CompilerServices.Extension> Public Function ShowDependentDialog(Owner As System.Windows.Window, Dependent As System.Windows.Window) As Boolean?
        If Owner Is Nothing OrElse Dependent Is Nothing Then Return Nothing
        Dependent.Owner = Owner
        Return Dependent.ShowDialog
    End Function
End Module
