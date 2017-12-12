Public Class SequenceEditorView
    Implements ITabHostingControl
    Public Sub TabParentChanged(vParent As System.Windows.Forms.TabControl) Implements ITabHostingControl.TabParentChanged
        If vParent.NO Then
            wpfDNADesigner.UnloadView()
        End If
    End Sub
    Public Property PropertyTab As PropertyControl
        Get
            Return wpfDNADesigner.RelatedPropertyTab
        End Get
        Set(value As PropertyControl)
            wpfDNADesigner.RelatedPropertyTab = value
        End Set
    End Property
End Class
