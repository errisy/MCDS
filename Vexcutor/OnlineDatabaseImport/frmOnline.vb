Public Class frmOnline

    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _Model = New OnlineDatabaseImporterModel
        DirectCast(iHost.Child, OnlineDatabaseImporter).DataContext = _Model
    End Sub
    Public Property Model As OnlineDatabaseImporterModel

End Class