<ValueConversion(GetType(Double), GetType(Double))>
Public Class MultiplyConverter
    Inherits System.Windows.Markup.MarkupExtension
    Implements IValueConverter
    Public Sub New()
    End Sub
    Public Sub New(nMultiplier As Double)
        _Multiplier = nMultiplier
    End Sub
    Public Property Multiplier As Double = 1.0#
    Public Property Offset As Double = 0.0#
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Double Then
            Return value * _Multiplier + _Offset
        Else
            Return value
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf value Is Double Then
            Return value / _Multiplier
        Else
            Return value
        End If
    End Function
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return Me
    End Function
End Class

<ValueConversion(GetType(String), GetType(String))>
Public Class FilenameConverter
    Inherits System.Windows.Markup.MarkupExtension
    Implements IValueConverter
    Public Sub New()
    End Sub
    Public Sub New(nShowDirectory As Double)
        _ShowDirectory = nShowDirectory
    End Sub
    Public Property ShowExtension As Boolean = True
    Public Property ShowDirectory As Boolean = False
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is String Then
            Dim fn As String = value
            If Not _ShowDirectory And fn.Contains("\") Then
                fn = fn.Substring(fn.LastIndexOf("\"))
            End If
            If Not _ShowExtension And fn.Contains(".") Then
                fn = fn.Substring(0, fn.LastIndexOf("."))
            End If
            Return fn
        Else
            Return value
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return value
    End Function
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return Me
    End Function
End Class


<ValueConversion(GetType(Boolean), GetType(Visibility))>
Public Class VisibilityConverter
    Inherits System.Windows.Markup.MarkupExtension
    Implements IValueConverter
    Public Sub New()
    End Sub
    Public Sub New(IsTrueForVisible As Boolean)
        _TrueForVisible = IsTrueForVisible
    End Sub
    Public Property TrueForVisible As Boolean = True
    Public Property UseCollapsed As Boolean = False
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Boolean Then
            If value Then
                If _TrueForVisible Then
                    Return Visibility.Visible
                Else
                    If _UseCollapsed Then
                        Return Visibility.Collapsed
                    Else
                        Return Visibility.Hidden
                    End If
                End If
            Else
                If _TrueForVisible Then
                    If _UseCollapsed Then
                        Return Visibility.Collapsed
                    Else
                        Return Visibility.Hidden
                    End If
                Else
                    Return Visibility.Visible
                End If
            End If
        Else
            If _TrueForVisible Then
                If _UseCollapsed Then
                    Return Visibility.Collapsed
                Else
                    Return Visibility.Hidden
                End If
            Else
                Return Visibility.Visible
            End If
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf value Is Visibility Then
            Dim v As Visibility = value
            Select Case v
                Case Visibility.Visible
                    Return _TrueForVisible
                Case Else
                    Return Not _TrueForVisible
            End Select
        Else
            Return Not _TrueForVisible
        End If
    End Function
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return Me
    End Function
End Class


<ValueConversion(GetType(Long), GetType(String))>
Public Class LongConverter
    Inherits System.Windows.Markup.MarkupExtension
    Implements IValueConverter
    Public Sub New()
    End Sub
    Public Sub New(nShowDirectory As Double)
        _ShowDirectory = nShowDirectory
    End Sub
    Public Property ShowExtension As Boolean = True
    Public Property ShowDirectory As Boolean = False
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is Long Then
            Return value.ToString
        Else
            Return value
        End If
    End Function
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf value Is String Then
            Dim l As Long
            If Long.TryParse(value, l) Then
                Return l
            Else
                Return 0L
            End If
        Else
            Return 0L
        End If
    End Function
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Return Me
    End Function
End Class