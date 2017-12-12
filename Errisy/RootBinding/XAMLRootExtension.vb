
Public Class XAMLRootExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New()
    End Sub
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Dim RootObjectProvider = DirectCast(serviceProvider.GetService(GetType(Xaml.IRootObjectProvider)), Xaml.IRootObjectProvider)
        Return RootObjectProvider.RootObject
    End Function
End Class
