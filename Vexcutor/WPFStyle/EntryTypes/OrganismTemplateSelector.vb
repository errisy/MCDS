Imports System.Windows

<System.Windows.Markup.ContentProperty("DataTemplates")>
Public Class OrganismTemplateSelector
    Inherits Controls.DataTemplateSelector
    Public ReadOnly Property DataTemplates As New ObjectModel.ObservableCollection(Of DataTemplate)
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If Not (TypeOf item Is OrganismStatusEnum) Then Return Nothing
        If _DataTemplates.Count < 2 Then Return Nothing
        Select Case DirectCast(item, OrganismStatusEnum)
            Case OrganismStatusEnum.NotAnalyzed
                Return _DataTemplates(0)
            Case OrganismStatusEnum.Analyzed
                Return _DataTemplates(1)
        End Select
        Return Nothing
    End Function
End Class

<Serializable>
Public Enum OrganismStatusEnum
    NotAnalyzed
    Analyzed
End Enum