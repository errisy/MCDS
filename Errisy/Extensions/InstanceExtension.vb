Public Class InstanceExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Property Type As Type
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        If Type Is Nothing Then Return Nothing
        Return Type.GetConstructor(New Type() {}).Invoke(New Object() {})
    End Function
End Class
