Public Class PrintPageView

    Private WithEvents page As PrintPage
    <System.ComponentModel.Browsable(False)> Public Property RelatedPrintPage As PrintPage
        Get
            Return page
        End Get
        Set(ByVal value As PrintPage)
            page = value
            UpdateValue()
        End Set
    End Property
    Private Updating As Boolean = False
    Public Sub UpdateValue()
        If page Is Nothing Then Exit Sub
        Updating = True
        tbTitle.Text = page.Title
        tbPageID.Text = page.PageID
        rtbDescription.Text = page.Text

        Dim typestring As String = CInt(page.PageWidth).ToString + "mm x " + CInt(page.PageHeight).ToString + "mm"
        For i As Integer = 0 To cbPaper.Items.Count - 1
            If cbPaper.Items(i).ToString.IndexOf(typestring) > -1 Then
                cbPaper.SelectedIndex = i
                Exit For
            End If
        Next
        snbPageWidth.SetValueWithoutValueChangedEvent(page.PageWidth)
        snbPageHeight.SetValueWithoutValueChangedEvent(page.PageHeight)
        snbLeft.SetValueWithoutValueChangedEvent(page.LeftSpace)
        snbRight.SetValueWithoutValueChangedEvent(page.RightSpace)
        snbTop.SetValueWithoutValueChangedEvent(page.TopSpace)
        snbBottom.SetValueWithoutValueChangedEvent(page.BottomSpace)
        snbDPI.SetValueWithoutValueChangedEvent(page.DPI)
        llLocation.Text = String.Format("Page Location ({0}, {1}, {2}, {3})", page.Left.ToString, page.Top.ToString, page.Width.ToString, page.Height.ToString)
        Updating = False
    End Sub

    Private Sub snbTop_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbTop.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.TopSpace = snbTop.Value
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub snbPageWidth_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbPageWidth.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.PageWidth = snbPageWidth.Value
            page.Width = page.PageWidth / page.PageHeight * page.Height
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub snbPageHeight_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbPageHeight.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.PageHeight = snbPageHeight.Value
            page.Height = page.PageHeight / page.PageWidth * page.Width
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub snbLeft_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbLeft.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.LeftSpace = snbLeft.Value
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub snbRight_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbRight.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.RightSpace = snbRight.Value
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub snbBottom_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbBottom.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.BottomSpace = snbBottom.Value
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub tbTitle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTitle.TextChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.Title = tbTitle.Text
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub tbPageID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPageID.TextChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.PageID = tbPageID.Text
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub rtbDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtbDescription.TextChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.Text = rtbDescription.Text
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Private Sub page_Moved(ByVal sender As Object, ByVal e As System.EventArgs) Handles page.Moved
        llLocation.Text = String.Format("Page Location ({0}, {1}, {2}, {3})", page.Left.ToString, page.Top.ToString, page.Width.ToString, page.Height.ToString)
    End Sub

  
    Private Sub snbDPI_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles snbDPI.ValueChanged
        If Updating Then Exit Sub
        If Not (page Is Nothing) Then
            page.DPI = snbDPI.Value
            RaiseEvent RequireUpdateView(Me, New EventArgs)
        End If
    End Sub

    Public Event RequireUpdateView(ByVal sender As Object, ByVal e As EventArgs)
    Public Event PrintSelectedPage(sender As Object, e As EventArgs)
    Public Event DirectPrintSelectedPage(sender As Object, e As EventArgs)
    Public Event DeleteSeletedPage(sender As Object, e As EventArgs)
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub



    Private Sub llPrintThisPage_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPrintThisPage.LinkClicked
        RaiseEvent PrintSelectedPage(Me, New EventArgs)
    End Sub

    Private Sub llDirectPrintThisPage_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llDirectPrintThisPage.LinkClicked
        RaiseEvent DirectPrintSelectedPage(Me, New EventArgs)
    End Sub

    Private Sub llDeleteThisPage_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llDeleteThisPage.LinkClicked
        RaiseEvent DeleteSeletedPage(Me, New EventArgs)
    End Sub
End Class
