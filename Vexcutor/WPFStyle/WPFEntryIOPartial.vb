Partial Public Class SettingEntry
#Region "Dialogs"
    Public Shared SaveGeneDialog As New Microsoft.Win32.SaveFileDialog With {.Title = "MCDS " + Now.Year.ToString() + " - Save Gene File",
                                                                             .Filter = "Vector File|*.vct;|Genebank File|*.gb;"}
    Public Shared OpenGeneDialog As New Microsoft.Win32.OpenFileDialog With {.Title = "MCDS " + Now.Year.ToString() + " - Open Gene File",
                                                                             .Filter = "All Files|*.vct;*.gb;*.gbk;"}

    Public Shared SaveProjectDialog As New Microsoft.Win32.SaveFileDialog With {.Title = "MCDS " + Now.Year.ToString() + " - Save Project File",
                                                                             .Filter = "MCDS Project File|*.vxt;"}
    Public Shared OpenProjectDialog As New Microsoft.Win32.OpenFileDialog With {.Title = "MCDS " + Now.Year.ToString() + " - Open Project File",
                                                                             .Filter = "MCDS Project File|*.vxt;"}

    Public Shared OpenFileDialog As New Microsoft.Win32.OpenFileDialog With {.Title = "MCDS " + Now.Year.ToString() + " - Open File",
                                                                             .Filter = "All Files|*.vxt;*.vct;*.gb;*.gbk;|Vector File|*.vct;|Genebank File|*.gb;*.gbk;"}


#End Region

#Region "IO Methods"

    Public Shared VectorSerializationList As New List(Of String) From {"DNA", "Enzyme"}
    Public Shared WorkChartList As New List(Of String) From {"WorkChart"}

    Shared ReadOnly Property GetXOS As Object
        Get
            Return New _Misaka
        End Get
    End Property
    Shared ReadOnly Property GetXOD As Object
        Get
            Return New _Mikoto
        End Get
    End Property
    Shared Sub SaveWPFWorkSpaceToSerialization(ByVal vWorkSpace As WPFWorkSpaceViewModel, ByVal vFilename As String)
        Dim ws As WorkSpace = vWorkSpace.WorkSpace

    End Sub
    Shared Function LoadWPFWorkSpaceFromSerialization(ByVal vFilename As String) As WPFWorkSpaceViewModel

    End Function
    Shared Sub SaveToZXML(ByVal vDict As Dictionary(Of String, Object), ByVal vFilename As String)
        Dim XOS = GetXOS
        XOS.Michael(vDict)
        XOS.SaveGZip(vFilename)
    End Sub
    Shared Function SaveToZXMLBytes(ByVal vDict As Dictionary(Of String, Object)) As Byte()
        Dim XOS = GetXOS
        XOS.Michael(vDict)
        Return XOS.SaveToZipBytes
    End Function
    Shared Function LoadFromZXML(ByVal vList As List(Of String), ByVal vFilename As String) As Dictionary(Of String, Object)
        Dim XOD = GetXOD
        Return XOD.Michael(vFilename, vList)
    End Function
    Shared Function LoadFromZXMLBytes(ByVal vList As List(Of String), ByVal buf As Byte()) As Dictionary(Of String, Object)
        Dim XOD = GetXOD
        Return XOD.Michael(buf, vList)
    End Function
#End Region

#Region "Database"
    Public Shared DatabasePath As String = AppDomain.CurrentDomain.BaseDirectory + "Database\"
    ''' <summary>
    ''' 更新数据库文件菜单
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Sub UpdateDatabasePath()

    End Sub

    Public Shared Sub AddDNAViewTab(ByVal gf As Nuctions.GeneFile, ByVal ez As List(Of String), Optional ByVal FileAddress As String = "", Optional ByVal Username As String = "", Optional ByVal Password As String = "")
        MainUIWindow.AddDNAViewTab(gf, ez, FileAddress)
    End Sub

    Public Shared Sub TryCloseTab(TabPage As Object)
        MainUIWindow.TryCloseTab(TabPage)
    End Sub

    Public Shared Sub AddTmViewerTab(ByVal gf As Nuctions.GeneFile)
        MainUIWindow.AddTmViewerTab(gf)
    End Sub
#End Region

#Region "Operations"

#End Region
End Class
