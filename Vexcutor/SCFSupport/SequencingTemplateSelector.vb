Imports System.Windows, System.Windows.Controls, System.Windows.Data
<System.Windows.Markup.ContentProperty("Templates")>
Public Class SequencingTemplateSelector
    Inherits DataTemplateSelector
    Public ReadOnly Property Templates As New ObjectModel.ObservableCollection(Of DataTemplate)
    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If TypeOf item Is SequenceItem Then
            Dim si As SequenceItem = item
            Select Case si.FileType
                Case SequencingFileTypeEnum.SCF
                    Return Templates(0)
                Case SequencingFileTypeEnum.AB1
                    Return Templates(1)
            End Select
        End If
        Return Nothing
    End Function
End Class
