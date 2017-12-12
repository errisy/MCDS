Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Net.Mail

Public Class ResponseEventArgs
    Inherits EventArgs
    Private mSuccess As Boolean
    Private mResponse As System.Net.HttpWebResponse
    Private mResponseException As Exception
    Private mPostTag As Object
    Private mBytes As List(Of Byte)
    Public Sub New(vSuccess As Boolean, vResponse As System.Net.HttpWebResponse, vResponseException As Exception)
        mSuccess = vSuccess
        mResponse = vResponse
        mResponseException = vResponseException
    End Sub
    Public ReadOnly Property Success As Boolean
        Get
            Return mSuccess
        End Get
    End Property
    Public ReadOnly Property Response As System.Net.HttpWebResponse
        Get
            Return mResponse
        End Get
    End Property
    Public ReadOnly Property ResponseException As Exception
        Get
            Return mResponseException
        End Get
    End Property
    Public ReadOnly Property ByteList As List(Of Byte)
        Get
            If mBytes Is Nothing Then
                mBytes = New List(Of Byte)
                Dim rStream As System.IO.Stream = mResponse.GetResponseStream
                Dim block As Byte() = New Byte(1023) {}
                Dim bytes As New List(Of Byte)
                Dim read As Integer
                Do
                    read = rStream.Read(block, 0, 1024)
                    mBytes.AddRange(block.Take(read))
                Loop While read > 0
            End If
            Return mBytes
        End Get
    End Property
    Public ReadOnly Property MappedStream() As System.IO.MemoryStream
        Get
            Dim memStream As New System.IO.MemoryStream(ByteList.ToArray)
            Return memStream
        End Get
    End Property
    Public ReadOnly Property MappedHexCode As String
        Get
            Dim stb As New System.Text.StringBuilder
            For Each b As Byte In ByteList
                stb.Append(Hex(b Mod 16))
                stb.Append(Hex(b \ 16))
                stb.Append(" ")
            Next
            Return stb.ToString
        End Get
    End Property

End Class

Public Class PostPackage
    Private vRequest As System.Net.HttpWebRequest
    Private vCookieContainer As New System.Net.CookieContainer
    Private vURL As String
    Private vDomain As String
    Private Shared vDomainRegex As New System.Text.RegularExpressions.Regex("^http://([^/]+)", RegexOptions.IgnoreCase)
    Private Data As New Dictionary(Of String, Object)
    Private vContainsBinary As Boolean = False
    Private vCookieData As New Dictionary(Of String, String)
    Public Event OnResponse(sender As Object, e As ResponseEventArgs)
    Private Delegate Sub ResponseReturn(hwResponse As System.Net.HttpWebResponse, vHandler As ResponseEventHandler)
    Private Delegate Sub ExceptionReturn(vException As Exception, vHandler As ResponseEventHandler)
    '<System.ComponentModel.Browsable(True), System.ComponentModel.Category("行为")> Public Property ContainerControl As ContainerControl
    Public Sub New()
        MyBase.New()
    End Sub
    Public Property ContainsBinary As Boolean
        Get
            Return vContainsBinary
        End Get
        Set(value As Boolean)
            vContainsBinary = value
        End Set
    End Property
    Public Sub AddCookie(Key As String, Value As String)
        vCookieData.Add(Key, Value)
    End Sub
    Public Sub ClearCookie()
        vCookieData.Clear()
    End Sub
    Public Sub AddData(Key As String, Value As String)
        Data.Add(Key, System.Net.WebUtility.HtmlEncode(Value))
    End Sub
    Public Sub AddData(Key As String, Value As Byte())
        vContainsBinary = True
        Data.Add(Key, Value)
    End Sub
    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("行为")> Public Property KeepCookie As Boolean = False
    Private Function Code(Value As String) As Byte()
        Return System.Text.Encoding.UTF8.GetBytes(Value)
    End Function
    Public Function PostData(url As String, Optional vHandler As ResponseEventHandler = Nothing) As System.Threading.Tasks.Task(Of HttpWebResponse)
        If Not url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) Then
            url = "http://" + url
        End If

        vURL = url

        If vDomainRegex.IsMatch(vURL) Then
            vDomain = vDomainRegex.Match(url).Groups(1).Value
        End If

        vRequest = System.Net.HttpWebRequest.Create(vURL)

        If Not KeepCookie Then
            vCookieContainer = Nothing
        End If

        If vCookieContainer Is Nothing Then
            vCookieContainer = New System.Net.CookieContainer
            vRequest.CookieContainer = vCookieContainer
            For Each key As String In vCookieData.Keys
                vCookieContainer.Add(New System.Net.Cookie(key, vCookieData(key), "/", vDomain))
            Next
        Else
            vRequest.CookieContainer = vCookieContainer
        End If

        If Not KeepCookie Then
            ClearCookie()
        End If
        Dim thr As System.Threading.Tasks.Task(Of HttpWebResponse)
        If vContainsBinary Then
            Dim boundary As String = "-------------------------" + DateTime.Now.Ticks.ToString("x")
            Dim spacer As Byte() = Code(vbCrLf & "--" & boundary & vbCrLf)
            Dim CrLf As Byte() = Code(vbCrLf)
            Dim dataname As Byte() = Code("Content-Disposition: form-data; name=""")
            Dim quota As Byte() = Code("""")
            Dim img As Byte() = Code("Content-Type: image/jpeg")
            vRequest.Method = "POST"
            vRequest.ContentType = String.Format("multipart/form-data; boundary={0}", boundary)
            Dim rStream As System.IO.Stream = vRequest.GetRequestStream
            Dim sWriter As New System.IO.BinaryWriter(rStream, System.Text.Encoding.UTF8)
            sWriter.Write(vbCrLf)

            For Each key As String In Data.Keys
                If Data(key) Is Nothing Then Continue For
                sWriter.Write(spacer)
                sWriter.Write(dataname)
                sWriter.Write(Code(key))
                sWriter.Write(quota)
                sWriter.Write(CrLf)
                If TypeOf Data(key) Is String Then
                    sWriter.Write(CrLf)
                    sWriter.Write(Code(Data(key)))
                Else
                    sWriter.Write(img)
                    sWriter.Write(CrLf)
                    sWriter.Write(CrLf)
                    If Data(key) IsNot Nothing Then
                        sWriter.Write(Data(key))
                    End If
                End If
                sWriter.Write(spacer)
            Next
            rStream.Flush()
            sWriter.Close()
            rStream.Close()
        Else
            Dim equal As Byte() = Code("=")
            Dim link As Byte() = Code("&")
            vRequest.Method = "POST"
            vRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8"
            Dim rStream As System.IO.Stream = vRequest.GetRequestStream

            'Dim kstream As New System.IO.MemoryStream

            Dim sWriter As New System.IO.BinaryWriter(rStream, System.Text.Encoding.UTF8)

            Dim i As Integer = 0
            For Each key As String In Data.Keys
                sWriter.Write(Code(key))
                sWriter.Write(equal)
                If Data(key) IsNot Nothing Then
                    sWriter.Write(Code(Data(key)))
                End If

                i += 1
                If i < Data.Count Then
                    sWriter.Write(link)
                End If
            Next
            rStream.Flush()
            sWriter.Close()
            rStream.Close()
        End If
        thr = New System.Threading.Tasks.Task(Of HttpWebResponse)(Function()
                                                                      Return vRequest.GetResponse
                                                                  End Function)
        thr.Start()
        Data.Clear()
        Return thr
    End Function
    Private Sub OnResponseReturn(hwResponse As System.Net.HttpWebResponse, Optional vHandler As ResponseEventHandler = Nothing)
        If vHandler Is Nothing Then
            RaiseEvent OnResponse(Me, New ResponseEventArgs(True, hwResponse, Nothing))
        Else
            vHandler.Invoke(Me, New ResponseEventArgs(True, hwResponse, Nothing))
        End If
    End Sub
    Private Sub OnExceptionReturn(vException As Exception, Optional vHandler As ResponseEventHandler = Nothing)
        If vHandler Is Nothing Then
            RaiseEvent OnResponse(Me, New ResponseEventArgs(False, Nothing, vException))
        Else
            vHandler.Invoke(Me, New ResponseEventArgs(False, Nothing, vException))
        End If
    End Sub
    Public Delegate Sub ResponseEventHandler(sender As Object, e As ResponseEventArgs)
End Class

 