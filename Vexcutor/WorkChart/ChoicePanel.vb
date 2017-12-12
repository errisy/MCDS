Public Class ChoicePanel
    Inherits Panel
    Public Property Choices As Dictionary(Of Object, String)
        Get
            Return _Choices
        End Get
        Set(ByVal value As Dictionary(Of Object, String))
            _Choices = value
            UpdateView()
        End Set
    End Property
    Public Sub RemoveItem(ByVal Item As Object)
        If Not (_Choices Is Nothing) Then
            If _Choices.ContainsKey(Item) Then
                _Choices.Remove(Item)
                UpdateView()
            End If
        End If
    End Sub
    Public Sub AddItem(ByVal Item As Object, ByVal Text As String)
        If Not (_Choices Is Nothing) Then
            If Not _Choices.ContainsKey(Item) Then
                _Choices.Add(Item, Text)
                UpdateView()
            End If
        End If
    End Sub
    Public Event ItemChoosen(ByVal sender As Object, ByVal e As ItemChoosenEventArgs)
    Private _Choices As New Dictionary(Of Object, String)
    Private Sub LinkLabelItemChoosen(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        If e.Button = MouseButtons.Left Then
            Dim ll As LinkLabel = sender
            RaiseEvent ItemChoosen(Me, New ItemChoosenEventArgs(ll.Tag, ll.Text))
        End If
    End Sub
    Private Sub UpdateView()
        Dim ll As LinkLabel
        For Each obj As Object In Controls
            If TypeOf obj Is LinkLabel Then
                ll = obj
                RemoveHandler ll.LinkClicked, AddressOf LinkLabelItemChoosen
            End If
        Next
        Controls.Clear()
        Dim i As Integer = 0
        For Each key As Object In _Choices.Keys
            ll = New LinkLabel
            ll.Text = _Choices(key)
            ll.Tag = key
            ll.Location = New Point((i Mod 5) * 100, (i \ 5) * 20)
            Controls.Add(ll)
            AddHandler ll.LinkClicked, AddressOf LinkLabelItemChoosen
            i += 1
        Next
    End Sub
    Public Class ItemChoosenEventArgs
        Inherits EventArgs
        Public Item As Object
        Public Text As String
        Public Sub New()
        End Sub
        Public Sub New(ByVal vItem As Object, ByVal vText As String)
            Item = vItem
            Text = vText
        End Sub
    End Class
End Class
