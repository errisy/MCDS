Public Class DocumentPaginatorWrapper
    Inherits DocumentPaginator
    Private m_PageSize As Size
    Private m_Margin As Size
    Private m_Paginator As DocumentPaginator
    Private m_Typeface As Typeface

    Public Sub New(paginator As DocumentPaginator, pageSize As Size, margin As Size)
        m_PageSize = pageSize
        m_Margin = margin
        m_Paginator = paginator
        m_Paginator.PageSize = New Size(m_PageSize.Width - margin.Width * 2, m_PageSize.Height - margin.Height * 2)
    End Sub

    Private Function Move(rect As Rect) As Rect
        If rect.IsEmpty Then
            Return rect
        Else
            Return New Rect(rect.Left + m_Margin.Width, rect.Top + m_Margin.Height, rect.Width, rect.Height)
        End If
    End Function

    Public Overrides Function GetPage(pageNumber As Integer) As DocumentPage
        Dim page As DocumentPage = m_Paginator.GetPage(pageNumber)

        ' Create a wrapper visual for transformation and add extras
        Dim newpage As New ContainerVisual()

        Dim title As New DrawingVisual()

        Using ctx As DrawingContext = title.RenderOpen()
            If m_Typeface Is Nothing Then
                m_Typeface = New Typeface("Times New Roman")
            End If

            Dim text As New FormattedText("Page " & (pageNumber + 1), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, m_Typeface, 14, Brushes.Black)

            ' 1/4 inch above page content
            ctx.DrawText(text, New Point(0, -96 \ 4))
        End Using

        Dim background As New DrawingVisual()

        Using ctx As DrawingContext = background.RenderOpen()
            ctx.DrawRectangle(New SolidColorBrush(Color.FromRgb(240, 240, 240)), Nothing, page.ContentBox)
        End Using

        newpage.Children.Add(background)
        ' Scale down page and center
        Dim smallerPage As New ContainerVisual()
        smallerPage.Children.Add(page.Visual)
        smallerPage.Transform = New MatrixTransform(0.95, 0, 0, 0.95, 0.025 * page.ContentBox.Width, 0.025 * page.ContentBox.Height)

        newpage.Children.Add(smallerPage)
        newpage.Children.Add(title)

        newpage.Transform = New TranslateTransform(m_Margin.Width, m_Margin.Height)

        Return New DocumentPage(newpage, m_PageSize, Move(page.BleedBox), Move(page.ContentBox))
    End Function

    Public Overrides ReadOnly Property IsPageCountValid() As Boolean
        Get
            Return m_Paginator.IsPageCountValid
        End Get
    End Property

    Public Overrides ReadOnly Property PageCount() As Integer
        Get
            Return m_Paginator.PageCount
        End Get
    End Property

    Public Overrides Property PageSize() As Size
        Get
            Return m_Paginator.PageSize
        End Get

        Set(value As Size)
            m_Paginator.PageSize = value
        End Set
    End Property

    Public Overrides ReadOnly Property Source() As IDocumentPaginatorSource
        Get
            Return m_Paginator.Source
        End Get
    End Property
End Class