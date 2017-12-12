Public Class XpsExporter
    Public Shared Function SaveAsXps(fileName As String) As Integer
        Dim doc As Object

        Dim fileInfo As New System.IO.FileInfo(fileName)

        Using file As System.IO.FileStream = fileInfo.OpenRead()
            Dim context As New System.Windows.Markup.ParserContext()
            context.BaseUri = New Uri(fileInfo.FullName, UriKind.Absolute)
            doc = System.Windows.Markup.XamlReader.Load(file, context)
        End Using

        If Not (TypeOf doc Is IDocumentPaginatorSource) Then
            Console.WriteLine("DocumentPaginatorSource expected")
            Return -1
        End If

        Using container As System.IO.Packaging.Package = System.IO.Packaging.Package.Open(fileName & ".xps", System.IO.FileMode.Create)
            Using xpsDoc As New XpsDocument(container, CompressionOption.Maximum)
                Dim rsm As New XpsSerializationManager(New XpsPackagingPolicy(xpsDoc), False)

                Dim paginator As DocumentPaginator = DirectCast(doc, IDocumentPaginatorSource).DocumentPaginator

                ' 8 inch x 6 inch, with half inch margin
                paginator = New DocumentPaginatorWrapper(paginator, New Size(768, 676), New Size(48, 48))

                rsm.SaveAsXaml(paginator)
            End Using
        End Using

        Console.WriteLine("{0} generated.", fileName & ".xps")

        Return 0
    End Function
End Class
