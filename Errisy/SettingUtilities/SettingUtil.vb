Public Class SettingUtil
    Public Shared Function SettingFileInfo(Name As String) As IO.FileInfo
        Return New IO.FileInfo(String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, Name))
    End Function
    Public Shared Function LoadXamlSetting(Of T As {New})(info As IO.FileInfo) As T
        Dim _Setting As T
        Try
            If IO.File.Exists(info.FullName) Then
                _Setting = System.Xaml.XamlServices.Load(info.FullName)
            Else
                _Setting = New T
            End If
        Catch ex As Exception
            _Setting = New T
        End Try
        Return _Setting
    End Function
    Public Shared Function SaveXamlSetting(Of T)(info As IO.FileInfo, setting As T) As Boolean
        Try
            System.Xaml.XamlServices.Save(info.FullName, setting)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function TrySaveXamlSetting(Of T)(info As IO.FileInfo, setting As Object) As Boolean
        If TypeOf setting Is T Then
            Try
                System.Xaml.XamlServices.Save(info.FullName, setting)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function
End Class

Public Class SettingLoaderExtension
    Inherits System.Windows.Markup.MarkupExtension
    Public Sub New(SettingAddress As String)
        Address = SettingAddress
    End Sub
    Public Property Type As Type
    Public Property Address As String
    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        Dim fileaddress = String.Format("{0}{1}.xml", AppDomain.CurrentDomain.BaseDirectory, Address)
        If IO.File.Exists(fileaddress) Then
            Dim _Setting As Object
            Try
                _Setting = System.Xaml.XamlServices.Load(fileaddress)
                If Type IsNot Nothing AndAlso _Setting IsNot Nothing AndAlso Type.IsAssignableFrom(_Setting.GetType) Then
                    Return _Setting
                Else
                    Return Type.GetConstructors()(0).Invoke(New Object() {})
                End If
            Catch ex As Exception
                If Type IsNot Nothing Then Return Type.GetConstructors()(0).Invoke(New Object() {})
            End Try
        Else
            If Type IsNot Nothing Then Return Type.GetConstructors()(0).Invoke(New Object() {})
        End If
        Return Nothing
    End Function
End Class