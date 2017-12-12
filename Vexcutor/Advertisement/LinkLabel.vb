'Imports System.Windows.Markup, System.Windows, System.Windows.Data, System.Windows.Media, System.Windows.Controls, System.Windows.Shapes, System.Windows.Documents

'Public Class LinkLabel
'    Inherits Grid
'    Private WithEvents tBlock As New TextBlock
'    Private WithEvents tText As New TextBox With {.Background = Nothing, .Visibility = Windows.Visibility.Hidden}
'    Private WithEvents tEditButton As New Button With {.Background = Nothing, .Width = 24, .Height = 24, .Content = New Image With {.Source = ResourceImage("pack://application:,,,/ModelX;component/Images/Write.png")}}
'    Private uLine As New Underline
'    Private vRun As New Run
'    Public Sub New()
'        MyBase.New()
'        tBlock.Foreground = Brushes.Blue
'        AddColumn(1, GridUnitType.Auto)
'        AddColumn(1, GridUnitType.Auto)
'        AddToColumnInOrder(tBlock, tEditButton)
'        tText.Column(0)
'        Add(tText)
'        tBlock.Inlines.Add(uLine)
'        uLine.Inlines.Add(vRun)
'        vRun.SetBinding(Run.TextProperty, BuildBinding(LinkProperty))
'        vRun.AddMonitor(Run.IsMouseDirectlyOverProperty, AddressOf WhenIsMouseDirectlyOverChanged)
'        tText.SetBinding(TextBox.TextProperty, BuildBinding(LinkProperty))
'    End Sub
'    Private Sub WhenIsMouseDirectlyOverChanged(sender As Object, e As EventArgs)
'        If vRun.IsMouseDirectlyOver Then
'            vRun.Foreground = Brushes.Red
'            tBlock.Cursor = Cursors.Hand
'        Else
'            vRun.Foreground = Brushes.Blue
'            tBlock.Cursor = Cursors.Arrow
'        End If
'    End Sub
'    'LinkLabel -> IsChanged As Boolean Default: False
'    Public Property IsChanged As Boolean
'        Get
'            Return GetValue(IsChangedProperty)
'        End Get
'        Set(ByVal value As Boolean)
'            SetValue(IsChangedProperty, value)
'        End Set
'    End Property
'    Public Shared ReadOnly IsChangedProperty As DependencyProperty = _
'                            DependencyProperty.Register("IsChanged", _
'                            GetType(Boolean), GetType(LinkLabel), _
'                            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsChangedChanged)))
'    Public Shared ReadOnly IsChangedPropertySerializer As Serializer = Serializer.Save(IsChangedProperty, GetType(LinkLabel))
'    Private Shared Sub SharedIsChangedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        DirectCast(d, LinkLabel).IsChangedChanged(d, e)
'    End Sub
'    Private Sub IsChangedChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        If e.NewValue Then
'            Background = ChangedBrush
'        Else
'            Background = NormalBrush
'        End If
'    End Sub
'    'LinkLabel -> IsReadOnly As Boolean Default: False
'    Public Property IsReadOnly As Boolean
'        Get
'            Return GetValue(IsReadOnlyProperty)
'        End Get
'        Set(ByVal value As Boolean)
'            SetValue(IsReadOnlyProperty, value)
'        End Set
'    End Property
'    Public Shared ReadOnly IsReadOnlyProperty As DependencyProperty = _
'                            DependencyProperty.Register("IsReadOnly", _
'                            GetType(Boolean), GetType(LinkLabel), _
'                            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsReadOnlyChanged)))
'    Public Shared ReadOnly IsReadOnlyPropertySerializer As Serializer = Serializer.Save(IsReadOnlyProperty, GetType(LinkLabel))
'    Private Shared Sub SharedIsReadOnlyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        DirectCast(d, LinkLabel).IsReadOnlyChanged(d, e)
'    End Sub
'    Private Sub IsReadOnlyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        If e.NewValue Then
'            tEditButton.Visibility = Windows.Visibility.Visible
'        Else
'            tEditButton.Visibility = Windows.Visibility.Collapsed
'        End If
'    End Sub

'    'LinkLabel -> Link As String Default: ""
'    Public Property Link As String
'        Get
'            Return GetValue(LinkProperty)
'        End Get
'        Set(ByVal value As String)
'            SetValue(LinkProperty, value)
'        End Set
'    End Property
'    Public Shared ReadOnly LinkProperty As DependencyProperty = _
'                            DependencyProperty.Register("Link", _
'                            GetType(String), GetType(LinkLabel), _
'                            New PropertyMetadata(""))
'    Public Shared ReadOnly LinkPropertySerializer As Serializer = Serializer.Save(LinkProperty, GetType(LinkLabel))

'    'LinkLabel -> IsEditing As Boolean Default: False
'    Public Property IsEditing As Boolean
'        Get
'            Return GetValue(IsEditingProperty)
'        End Get
'        Set(ByVal value As Boolean)
'            SetValue(IsEditingProperty, value)
'        End Set
'    End Property
'    Public Shared ReadOnly IsEditingProperty As DependencyProperty = _
'                            DependencyProperty.Register("IsEditing", _
'                            GetType(Boolean), GetType(LinkLabel), _
'                            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsEditingChanged)))
'    Public Shared ReadOnly IsEditingPropertySerializer As Serializer = Serializer.Save(IsEditingProperty, GetType(LinkLabel))
'    Private Shared Sub SharedIsEditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        DirectCast(d, LinkLabel).IsEditingChanged(d, e)
'    End Sub
'    Private Sub IsEditingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
'        If e.NewValue Then
'            tBlock.Visibility = Windows.Visibility.Hidden
'            tText.Visibility = Windows.Visibility.Visible
'        Else
'            tText.Visibility = Windows.Visibility.Hidden
'            tBlock.Visibility = Windows.Visibility.Visible
'        End If
'    End Sub

'#Region "Logic"
'    Private Sub OnEditClick(sender As Object, e As RoutedEventArgs) Handles tEditButton.Click
'        If IsReadOnly Then Exit Sub
'        IsEditing = Not IsEditing
'    End Sub
'    Private Sub Navigate()
'        If Link.Contains("@") Or Link.StartsWith("mailto:") Then
'            Dim processor = Ancestor(Of IMailHost)()
'            If processor IsNot Nothing Then processor.ProcessMail(Link)
'        Else
'            If Not Link.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) Then
'                Link = "http://" + Link
'            End If
'            Dim processor = Ancestor(Of INavigationHost)()
'            If processor IsNot Nothing Then processor.ProcessURL(Link)
'        End If
'    End Sub
'    Private Sub tText_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles tText.KeyDown
'        Select Case e.Key
'            Case Key.Enter, Key.Escape
'                IsEditing = False
'        End Select
'    End Sub
'    Private Sub OnTextLostFocus(sender As Object, e As RoutedEventArgs) Handles tText.LostFocus
'        IsEditing = False
'    End Sub
'    Private Sub tBlock_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles tBlock.MouseLeftButtonDown
'        tBlock.Foreground = Brushes.Purple
'        e.Handled = True
'    End Sub
'    Private Sub tBlock_MouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles tBlock.MouseLeftButtonUp
'        If IsMouseDirectlyOver Then
'            tBlock.Foreground = Brushes.Red
'        Else
'            tBlock.Foreground = Brushes.Blue
'        End If
'        e.Handled = True
'    End Sub
'#End Region

'End Class