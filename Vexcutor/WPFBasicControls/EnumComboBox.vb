Imports System.Windows, System.Windows.Controls, System.Windows.Data, System.Windows.Input, System.Windows.Media

Public Class EnumComboBox
    Inherits ComboBox
    Shared Sub New()
        ComboBox.ItemTemplateProperty.OverrideMetadata(GetType(EnumComboBox), New FrameworkPropertyMetadata(New DataTemplate With {.VisualTree = New FrameworkElementFactory(GetType(EnumValueItem))}))
    End Sub
    'EnumComboBox->EnumType As Type with Event Default: Nothing
    Public Property EnumType As Type
        Get
            Return GetValue(EnumTypeProperty)
        End Get
        Set(ByVal value As Type)
            SetValue(EnumTypeProperty, value)
        End Set
    End Property
    Public Shared ReadOnly EnumTypeProperty As DependencyProperty =
                           DependencyProperty.Register("EnumType",
                           GetType(Type), GetType(EnumComboBox),
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedEnumTypeChanged)))
    Private Shared Sub SharedEnumTypeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, EnumComboBox).EnumTypeChanged(d, e)
    End Sub
    Private Sub EnumTypeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If Not (TypeOf e.NewValue Is Type) Then ItemsSource = Nothing
        Dim nType As Type = e.NewValue
        If Not nType.IsEnum Then ItemsSource = Nothing
        Dim Values As New System.Collections.ObjectModel.ObservableCollection(Of Object)
        For Each value In [Enum].GetValues(nType)
            Values.Add(value)
        Next
        ItemsSource = Values
    End Sub
End Class
Public Class EnumValueItem
    Inherits TextBlock
    Private Sub EnumValueItem_DataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
        Dim nType As Type = DataContext.GetType
        If nType.IsEnum Then
            Text = [Enum].GetName(nType, DataContext)
        End If
    End Sub
End Class