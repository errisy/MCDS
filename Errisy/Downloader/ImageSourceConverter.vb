Public Class ImageSourceConverter
    Public Shared Function LoadImage(imageData As Byte()) As BitmapImage
        If imageData Is Nothing OrElse imageData.Length = 0 Then
            Return Nothing
        End If
        Dim image = New BitmapImage()
        Using mem = New IO.MemoryStream(imageData)
            mem.Position = 0
            image.BeginInit()
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat
            image.CacheOption = BitmapCacheOption.OnLoad
            image.UriSource = Nothing
            image.StreamSource = mem
            image.EndInit()
        End Using
        image.Freeze()
        Return image
    End Function
End Class
