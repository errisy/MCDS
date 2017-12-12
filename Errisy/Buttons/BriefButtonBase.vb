Public Class BriefButtonBase
    Inherits Button
    Shared Sub New()
        Button.BackgroundProperty.OverrideMetadata(GetType(BriefButtonBase), New FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender))
    End Sub
End Class
