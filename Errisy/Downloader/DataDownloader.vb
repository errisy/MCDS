Public Class DataDownloader
    Inherits DependencyObject

    'DataDownloader -> Data As Byte() Default: Nothing
    Public ReadOnly Property Data As Byte()
        Get
            Return GetValue(DataDownloader.DataProperty)
        End Get
    End Property
    Private Shared ReadOnly DataPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Data", _
                              GetType(Byte()), GetType(DataDownloader), _
                              New PropertyMetadata(Nothing))
    Public Shared ReadOnly DataProperty As DependencyProperty = _
                             DataPropertyKey.DependencyProperty

    'DataDownloader -> Length As Long Default: 0
    Public ReadOnly Property Length As Long
        Get
            Return GetValue(DataDownloader.LengthProperty)
        End Get
    End Property
    Private Shared ReadOnly LengthPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Length", _
                              GetType(Long), GetType(DataDownloader), _
                              New PropertyMetadata(0))
    Public Shared ReadOnly LengthProperty As DependencyProperty = _
                             LengthPropertyKey.DependencyProperty

    'DataDownloader -> Downloaded As Long Default: 0
    Public ReadOnly Property Downloaded As Long
        Get
            Return GetValue(DataDownloader.DownloadedProperty)
        End Get
    End Property
    Private Shared ReadOnly DownloadedPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Downloaded", _
                              GetType(Long), GetType(DataDownloader), _
                              New PropertyMetadata(0))
    Public Shared ReadOnly DownloadedProperty As DependencyProperty = _
                             DownloadedPropertyKey.DependencyProperty

    'DataDownloader -> Progress As Single Default: 0F
    Public ReadOnly Property Progress As Single
        Get
            Return GetValue(DataDownloader.ProgressProperty)
        End Get
    End Property
    Private Shared ReadOnly ProgressPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("Progress", _
                              GetType(Single), GetType(DataDownloader), _
                              New PropertyMetadata(0.0F))
    Public Shared ReadOnly ProgressProperty As DependencyProperty = _
                             ProgressPropertyKey.DependencyProperty

    'DataDownloader -> URL As String Default: ""
    Public ReadOnly Property URL As String
        Get
            Return GetValue(DataDownloader.URLProperty)
        End Get
    End Property
    Private Shared ReadOnly URLPropertyKey As DependencyPropertyKey = _
                              DependencyProperty.RegisterReadOnly("URL", _
                              GetType(String), GetType(DataDownloader), _
                              New PropertyMetadata(""))
    Public Shared ReadOnly URLProperty As DependencyProperty = _
                             URLPropertyKey.DependencyProperty
    Public Sub Download(link As String)
        Dim request = System.Net.WebRequest.Create(link)
        Dim t As New Task(Sub()
                              Dim response As System.Net.WebResponse = request.GetResponse()
                              Dim _Length As Long = response.ContentLength
                              Dispatcher.Invoke(Sub()
                                                    SetValue(LengthPropertyKey, _Length)
                                                End Sub)
                              Using stream As System.IO.Stream = response.GetResponseStream
                                  Dim value As Integer
                                  Dim _CurrentLength As Integer = 0
                                  Dim byteList As New List(Of Byte)
                                  value = stream.ReadByte
                                  While value > -1
                                      byteList.Add(value)
                                      _CurrentLength += 1
                                      Dispatcher.Invoke(Sub()
                                                            SetValue(DownloadedPropertyKey, _CurrentLength)
                                                            SetValue(ProgressPropertyKey, CSng(_CurrentLength / _Length))
                                                        End Sub)
                                      value = stream.ReadByte
                                  End While
                                  Dispatcher.Invoke(Sub()
                                                        SetValue(DataPropertyKey, byteList.ToArray)
                                                    End Sub)
                              End Using
                          End Sub)
        t.Start()
    End Sub

End Class
