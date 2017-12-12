Public Class ProgressNodeBase
    Inherits Control


    'ProgressNodeBase->IsRunning As Boolean with Event Default: False
    Public Property IsRunning As Boolean
        Get
            Return GetValue(IsRunningProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsRunningProperty, value)
        End Set
    End Property
    Public Shared ReadOnly IsRunningProperty As DependencyProperty = _
                           DependencyProperty.Register("IsRunning", _
                           GetType(Boolean), GetType(ProgressNodeBase), _
                           New PropertyMetadata(False, New PropertyChangedCallback(AddressOf SharedIsRunningChanged)))
    Private Shared Sub SharedIsRunningChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, ProgressNodeBase).IsRunningChanged(d, e)
    End Sub
    Private Sub IsRunningChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Then
            TickNumber = 0
            dTimer.IsEnabled = True
        Else
            dTimer.IsEnabled = False
            For Each p In PathList
                p.Fill = Brushes.Transparent
            Next
        End If
    End Sub

    Private WithEvents dTimer As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = False, .Interval = New TimeSpan(0, 0, 0, 0, 125)}
    Private TickNumber As Integer = 0
    Private Sub dTimer_Tick(sender As Object, e As EventArgs) Handles dTimer.Tick
        TickNumber += 1
        TickNumber = TickNumber Mod 8
        Dim index As Integer = TickNumber
        For i As Integer = 0 To 7
            Dim p = PathList((index + i) Mod 8)
            p.Fill = New SolidColorBrush(Color.FromArgb(16 * (i + 1) - 1, 0, 0, 0))
        Next
    End Sub

    Private S1, S2, S3, S4, S5, S6, S7, S8 As Path
    Private PathList As New List(Of Path)
    Public Overrides Sub OnApplyTemplate()
        S1 = Template.FindName("S1", Me)
        S2 = Template.FindName("S2", Me)
        S3 = Template.FindName("S3", Me)
        S4 = Template.FindName("S4", Me)
        S5 = Template.FindName("S5", Me)
        S6 = Template.FindName("S6", Me)
        S7 = Template.FindName("S7", Me)
        S8 = Template.FindName("S8", Me)
        PathList.Add(S1)
        PathList.Add(S2)
        PathList.Add(S3)
        PathList.Add(S4)
        PathList.Add(S5)
        PathList.Add(S6)
        PathList.Add(S7)
        PathList.Add(S8)
        MyBase.OnApplyTemplate()
    End Sub


End Class
