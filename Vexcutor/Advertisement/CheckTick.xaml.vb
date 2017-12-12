Imports System.Windows
Class CheckTick

    'CheckTick -> IsChecked As Boolean Default: True
    Public Property IsChecked As Boolean
        Get
            Return GetValue(IsCheckedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsCheckedProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsCheckedProperty As DependencyProperty = _
                            DependencyProperty.Register("IsChecked", _
                            GetType(Boolean), GetType(CheckTick), _
                            New PropertyMetadata(True, New PropertyChangedCallback(AddressOf SharedIsCheckedChanged)))
    Private Shared Sub SharedIsCheckedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, CheckTick).IsCheckedChanged(d, e)
    End Sub
    Private Sub IsCheckedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            Tick.Visibility = Windows.Visibility.Visible
        Else
            Tick.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

End Class
