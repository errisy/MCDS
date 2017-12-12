Public Module TreeHelper
    <System.Runtime.CompilerServices.Extension> Public Function VisualChild(Of T As DependencyObject)(element As DependencyObject, predicate As System.Linq.Expressions.Expression(Of Func(Of T, Boolean))) As T
        Dim predicateFunction = predicate.Compile
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(element) - 1
            Dim Current As DependencyObject = VisualTreeHelper.GetChild(element, i)
            If TypeOf Current Is T AndAlso predicateFunction.Invoke(Current) Then Return Current
            Dim ProvisionalChild As DependencyObject = VisualChild(Of T)(Current, predicateFunction)
            If ProvisionalChild IsNot Nothing Then Return ProvisionalChild
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function VisualChild(Of T As DependencyObject)(element As DependencyObject, predicateFunction As Func(Of T, Boolean)) As T
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(element) - 1
            Dim Current As DependencyObject = VisualTreeHelper.GetChild(element, i)
            If TypeOf Current Is T AndAlso predicateFunction.Invoke(Current) Then Return Current
            Dim ProvisionalChild As DependencyObject = VisualChild(Of T)(Current, predicateFunction)
            If ProvisionalChild IsNot Nothing Then Return ProvisionalChild
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function VisualChild(Of T As DependencyObject)(element As DependencyObject) As T

        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(element) - 1
            Dim Current As DependencyObject = VisualTreeHelper.GetChild(element, i)
            If TypeOf Current Is T Then Return Current
            Dim ProvisionalChild As DependencyObject = VisualChild(Of T)(Current)
            If ProvisionalChild IsNot Nothing Then Return ProvisionalChild
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension> Public Function Child(Of T As DependencyObject)(element As DependencyObject) As T

        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(element) - 1
            Dim Current As DependencyObject = VisualTreeHelper.GetChild(element, i)
            If TypeOf Current Is T Then Return Current
            Dim ProvisionalChild As DependencyObject = Child(Of T)(Current)
            If ProvisionalChild IsNot Nothing Then Return ProvisionalChild
        Next
        For Each obj In LogicalTreeHelper.GetChildren(element)
            Dim Current As DependencyObject = obj
            If TypeOf Current Is T Then Return Current
            Dim ProvisionalChild As DependencyObject = Child(Of T)(Current)
            If ProvisionalChild IsNot Nothing Then Return ProvisionalChild
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function Ancestor(Of T)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp As Tuple(Of Integer, T)
        Dim vp As Tuple(Of Integer, T)
        Dim p
        Dim i As Integer
        p = dObj
        i = 0
        While p IsNot Nothing
            p = LogicalTreeHelper.GetParent(p)
            If TypeOf p Is T Then
                lp = New Tuple(Of Integer, T)(i, p)
                Exit While
            End If
            i += 1
        End While
        p = dObj
        i = 0
        While p IsNot Nothing
            p = VisualTreeHelper.GetParent(p)
            If TypeOf p Is T Then
                vp = New Tuple(Of Integer, T)(i, p)
                Exit While
            End If
            i += 1
        End While
        If vp Is Nothing Then
            If lp Is Nothing Then
                Return Nothing
            Else
                Return lp.Item2
            End If
        Else
            If lp Is Nothing Then
                Return vp.Item2
            Else
                If vp.Item1 > lp.Item1 Then
                    Return lp.Item2
                Else
                    Return vp.Item2
                End If
            End If
        End If
    End Function
End Module
