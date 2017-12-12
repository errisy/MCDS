
Imports System.Windows.Markup, System.Windows, System.Windows.Data, System.Windows.Media, System.Windows.Controls, System.Windows.Shapes

Public Module TreeHelperExtensions
    <System.Runtime.CompilerServices.Extension()> Function Exists(Of T)(subject As T) As Boolean
        Return subject IsNot Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalParentIs(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp = LogicalTreeHelper.GetParent(dObj)
        If TypeOf lp Is T Then
            Return lp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualParentIs(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim vp = VisualTreeHelper.GetParent(dObj)
        If TypeOf vp Is T Then
            Return vp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalParentIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp = LogicalTreeHelper.GetParent(dObj)
        If TypeOf lp Is T AndAlso TreeHelper.GetID(lp) = ID Then
            Return lp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualParentIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim vp = VisualTreeHelper.GetParent(dObj)
        If TypeOf vp Is T AndAlso TreeHelper.GetID(vp) = ID Then
            Return vp
        Else
            Return Nothing
        End If
    End Function
#Region "GetReferenceInMiddle"
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalParentIs(Of T As DependencyObject)(dObj As DependencyObject, ByRef gRef As T) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp = LogicalTreeHelper.GetParent(dObj)
        If TypeOf lp Is T Then
            gRef = lp
            Return lp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualParentIs(Of T As DependencyObject)(dObj As DependencyObject, ByRef gRef As T) As T
        If dObj Is Nothing Then Return Nothing
        Dim vp = VisualTreeHelper.GetParent(dObj)
        If TypeOf vp Is T Then
            gRef = vp
            Return vp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalParentIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String, ByRef gRef As T) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp = LogicalTreeHelper.GetParent(dObj)
        If TypeOf lp Is T AndAlso TreeHelper.GetID(lp) = ID Then
            gRef = lp
            Return lp
        Else
            Return Nothing
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualParentIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String, ByRef gRef As T) As T
        If dObj Is Nothing Then Return Nothing
        Dim vp = VisualTreeHelper.GetParent(dObj)
        If TypeOf vp Is T AndAlso TreeHelper.GetID(vp) = ID Then
            gRef = vp
            Return vp
        Else
            Return Nothing
        End If
    End Function
#End Region
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChild(dObj As DependencyObject, ID As String) As DependencyObject
        If dObj Is Nothing Then Return Nothing
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TreeHelper.GetID(child) = ID Then Return child
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChild(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TypeOf child Is T AndAlso TreeHelper.GetID(child) = ID Then Return child
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChild(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TypeOf child Is T Then Return child
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChild(dObj As DependencyObject, ID As String) As DependencyObject
        If dObj Is Nothing Then Return Nothing
        Dim c As DependencyObject
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TreeHelper.GetID(c) = ID Then Return c
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChild(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim c As DependencyObject
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TypeOf c Is T AndAlso TreeHelper.GetID(c) = ID Then Return c
        Next
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChild(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim c As DependencyObject
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TypeOf c Is T Then Return c
        Next
        Return Nothing
    End Function

#Region "Children"
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChildren(dObj As DependencyObject, ID As String) As System.Collections.ObjectModel.ObservableCollection(Of DependencyObject)
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of DependencyObject)
        If dObj Is Nothing Then Return vList
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TreeHelper.GetID(child) = ID Then vList.Add(child)
        Next
        Return vList
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChildren(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As System.Collections.ObjectModel.ObservableCollection(Of T)
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of T)
        If dObj Is Nothing Then Return vList
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TypeOf child Is T AndAlso TreeHelper.GetID(child) = ID Then vList.Add(child)
        Next
        Return vList
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalChildren(Of T As DependencyObject)(dObj As DependencyObject) As System.Collections.ObjectModel.ObservableCollection(Of T)
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of T)
        If dObj Is Nothing Then Return vList
        For Each child In LogicalTreeHelper.GetChildren(dObj)
            If TypeOf child Is T Then vList.Add(child)
        Next
        Return vList
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChildren(dObj As DependencyObject, ID As String) As System.Collections.ObjectModel.ObservableCollection(Of DependencyObject)
        Dim c As DependencyObject
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of DependencyObject)
        If dObj Is Nothing Then Return vList
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TreeHelper.GetID(c) = ID Then vList.Add(c)
        Next
        Return vList
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChildren(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As System.Collections.ObjectModel.ObservableCollection(Of T)
        Dim c As DependencyObject
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of T)
        If dObj Is Nothing Then Return vList
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TypeOf c Is T AndAlso TreeHelper.GetID(c) = ID Then vList.Add(c)
        Next
        Return vList
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualChildren(Of T As DependencyObject)(dObj As DependencyObject) As System.Collections.ObjectModel.ObservableCollection(Of T)
        Dim c As DependencyObject
        Dim vList As New System.Collections.ObjectModel.ObservableCollection(Of T)
        If dObj Is Nothing Then Return vList
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(dObj) - 1
            c = VisualTreeHelper.GetChild(dObj, i)
            If TypeOf c Is T Then vList.Add(c)
        Next
        Return vList
    End Function
#End Region

#Region "Ancestor"
    <System.Runtime.CompilerServices.Extension()>
    Public Function Ancestor(Of T)(dObj As DependencyObject) As T
        Dim dList As New HashSet(Of DependencyObject) From {dObj}
        Dim nList As New HashSet(Of DependencyObject)
        While dList.Count > 0
            For Each d In dList
                If d IsNot Nothing Then
                    If TypeOf d Is Visual Then
                        Dim pV As Object = VisualTreeHelper.GetParent(d)
                        If TypeOf pV Is T Then
                            Return pV
                        Else
                            nList.Add(pV)
                        End If
                    End If
                    Dim pL As Object = LogicalTreeHelper.GetParent(d)
                    If TypeOf pL Is T Then
                        Return pL
                    Else
                        nList.Add(pL)
                    End If
                End If
            Next
            dList = nList
            nList = New HashSet(Of DependencyObject)
        End While
        Return Nothing
    End Function


    <System.Runtime.CompilerServices.Extension()>
    Public Function Ancestor(Of T)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim lp As Tuple(Of Integer, T)
        Dim vp As Tuple(Of Integer, T)
        Dim p
        Dim i As Integer
        p = dObj
        i = 0
        While p IsNot Nothing
            p = LogicalTreeHelper.GetParent(p)
            If TypeOf p Is T AndAlso TreeHelper.GetID(p) = ID Then
                lp = New Tuple(Of Integer, T)(i, p)
                Exit While
            End If
            i += 1
        End While
        p = dObj
        i = 0
        While p IsNot Nothing
            p = VisualTreeHelper.GetParent(p)
            If TypeOf p Is T AndAlso TreeHelper.GetID(p) = ID Then
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
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalAncestorIs(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim p
        p = dObj
        While p IsNot Nothing
            p = LogicalTreeHelper.GetParent(p)
            If TypeOf p Is T Then Return p
        End While
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualAncestorIs(Of T As DependencyObject)(dObj As DependencyObject) As T
        If dObj Is Nothing Then Return Nothing
        Dim p
        p = dObj
        While p IsNot Nothing
            p = VisualTreeHelper.GetParent(p)
            If TypeOf p Is T Then Return p
        End While
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function LogicalAncestorIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim p
        p = dObj
        While p IsNot Nothing
            p = LogicalTreeHelper.GetParent(p)
            If TypeOf p Is T AndAlso TreeHelper.GetID(p) = ID Then Return p
        End While
        Return Nothing
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function VisualAncestorIs(Of T As DependencyObject)(dObj As DependencyObject, ID As String) As T
        If dObj Is Nothing Then Return Nothing
        Dim p
        p = dObj
        While p IsNot Nothing
            p = VisualTreeHelper.GetParent(p)
            If TypeOf p Is T AndAlso TreeHelper.GetID(p) = ID Then Return p
        End While
        Return Nothing
    End Function
#End Region

    <System.Runtime.CompilerServices.Extension()>
    Public Function [Do](Of T)(obj As T, method As Action(Of T))
        If TypeOf obj Is T AndAlso method IsNot Nothing Then
            method.Invoke(obj)
            Return True
        Else
            Return False
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()>
    Public Function [Do](Of T, TResult)(obj As T, method As Func(Of T, TResult)) As TResult
        If TypeOf obj Is T AndAlso method IsNot Nothing Then
            Return method.Invoke(obj)
        Else
            Return Nothing
        End If
    End Function
    Public Function IsType(Of T)(obj As Object) As T
        If TypeOf obj Is T Then Return obj
        Return Nothing
    End Function

End Module
Public Class TreeHelper
    Inherits DependencyObject
    Private Shared ReadOnly ThisType As Type = GetType(TreeHelper)
    Protected Event Initializing As RoutedEventHandler
    Public Sub New()
        RaiseEvent Initializing(Me, New RoutedEventArgs)
    End Sub
    Public Shared Function GetID(ByVal element As DependencyObject) As String
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If

        Return element.GetValue(IDProperty)
    End Function
    Public Shared Sub SetID(ByVal element As DependencyObject, ByVal value As String)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If

        element.SetValue(IDProperty, value)
    End Sub
    Public Shared ReadOnly IDProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("ID", _
                           GetType(String), GetType(TreeHelper), _
                           New FrameworkPropertyMetadata(Nothing))
End Class