Public Class DownloadProgressImageBase
    Inherits Control
    'DownloadProgressImageBase -> DownloadedSource As ImageSource Default: Nothing
    Public ReadOnly Property DownloadedSource As ImageSource
        Get
            Return GetValue(DownloadProgressImageBase.DownloadedSourceProperty)
        End Get
    End Property
    Private Shared ReadOnly DownloadedSourcePropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("DownloadedSource", _
                              GetType(ImageSource), GetType(DownloadProgressImageBase), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly DownloadedSourceProperty As DependencyProperty = _
                             DownloadedSourcePropertyKey.DependencyProperty

    'DownloadProgressImageBase -> ContentLength As Long Default: 0L
    Public ReadOnly Property ContentLength As Long
        Get
            Return GetValue(DownloadProgressImageBase.ContentLengthProperty)
        End Get
    End Property
    Private Shared ReadOnly ContentLengthPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("ContentLength", _
                              GetType(Long), GetType(DownloadProgressImageBase), _
                              New PropertyMetadata(0L))
    Public Shared ReadOnly ContentLengthProperty As DependencyProperty = _
                             ContentLengthPropertyKey.DependencyProperty

    'DownloadProgressImageBase -> DownloadedLength As Long Default: 0L
    Public ReadOnly Property DownloadedLength As Long
        Get
            Return GetValue(DownloadProgressImageBase.DownloadedLengthProperty)
        End Get
    End Property
    Private Shared ReadOnly DownloadedLengthPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("DownloadedLength", _
                              GetType(Long), GetType(DownloadProgressImageBase), _
                              New PropertyMetadata(0L))
    Public Shared ReadOnly DownloadedLengthProperty As DependencyProperty = _
                             DownloadedLengthPropertyKey.DependencyProperty

    'DownloadProgressImageBase -> Progress As Double Default: 0#
    Public ReadOnly Property Progress As Double
        Get
            Return GetValue(DownloadProgressImageBase.ProgressProperty)
        End Get
    End Property
    Private Shared ReadOnly ProgressPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Progress", _
                              GetType(Double), GetType(DownloadProgressImageBase), _
                              New PropertyMetadata(0.0#))
    Public Shared ReadOnly ProgressProperty As DependencyProperty = _
                             ProgressPropertyKey.DependencyProperty

    'DownloadProgressImageBase->URL As String with Event Default: Nothing
    Public Property URL As String
        Get
            Return GetValue(URLProperty)
        End Get
        Set(ByVal value As String)
            SetValue(URLProperty, value)
        End Set
    End Property
    Public Shared ReadOnly URLProperty As DependencyProperty = _
                           DependencyProperty.Register("URL", _
                           GetType(String), GetType(DownloadProgressImageBase), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedURLChanged)))
    Private Shared Sub SharedURLChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, DownloadProgressImageBase).URLChanged(d, e)
    End Sub
    Private Sub URLChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim uri As System.Uri = Nothing
        uri.TryCreate(e.NewValue, UriKind.Absolute, uri)
        Download(uri.AbsoluteUri)
    End Sub


    Private Sub Download(link As String)
        Dim request = System.Net.WebRequest.Create(link)
        Dim t As New Task(Sub()
                              Dim response As System.Net.WebResponse = request.GetResponse()
                              Dim _Length As Long = response.ContentLength
                              Dispatcher.Invoke(Sub()
                                                    SetValue(ContentLengthPropertyKey, _Length)
                                                End Sub)
                              Using stream As System.IO.Stream = response.GetResponseStream
                                  Dim _CurrentLength As Long = 0L
                                  Dim byteList As New List(Of Byte)

                                  Dim bytes As Byte() = New Byte(4095) {}
                                  Dim count As Long

                                  Do
                                      count = stream.Read(bytes, 0, 4096)
                                      byteList.AddRange(bytes.Take(count))
                                      _CurrentLength += count
                                      Dispatcher.Invoke(Sub()
                                                            SetValue(DownloadedLengthPropertyKey, _CurrentLength)
                                                            SetValue(ProgressPropertyKey, CDbl(_CurrentLength / _Length))
                                                        End Sub)
                                  Loop While count > 0

                                  Dim img = ImageSourceConverter.LoadImage(byteList.ToArray)

                                  Dispatcher.Invoke(Sub()
                                                        SetValue(DownloadedSourcePropertyKey, img)
                                                    End Sub)
                              End Using
                          End Sub)
        t.Start()
    End Sub
End Class
