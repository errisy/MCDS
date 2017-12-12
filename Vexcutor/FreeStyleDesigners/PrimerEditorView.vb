Public Class PrimerEditorView
    Implements ITabHostingControl
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        wpfPrimerEditor.fsMain.RelatedAnalysisView = pafPrimerDesign
        wpfPrimerEditor.RequirePCR = AddressOf RequirePCR
    End Sub
    Public Sub TabParentChanged(vParent As System.Windows.Forms.TabControl) Implements ITabHostingControl.TabParentChanged
        If vParent.NO Then
            wpfPrimerEditor.UnloadView()
        End If
    End Sub
    Public Sub RequirePCR(SourceGeneFiles As List(Of Nuctions.GeneFile), Primers As Dictionary(Of String, String))
        RaiseEvent RequirePCRView(Me, New MCDS.PCRViewEventArgs(SourceGeneFiles, Primers))
    End Sub
    Public Event RequirePCRView(ByVal sender As Object, ByVal e As PCRViewEventArgs)
    Public Property PropertyTab As PropertyControl
        Get
            Return wpfPrimerEditor.RelatedPropertyTab
        End Get
        Set(value As PropertyControl)
            wpfPrimerEditor.RelatedPropertyTab = value
        End Set
    End Property
End Class
