Imports System.Windows, System.Windows.Controls, System.Windows.Media, System.Windows.Input, System.Windows.Shapes, System.Windows.Media.Animation
Public Class ToolBox
    Inherits TreeView
    Private LinearTools As New EditorGroup With {.Title = "Linear Tools"}
    Private RectangleTools As New EditorGroup With {.Title = "Rectangle Tools"}
    Private ChemicalTools As New EditorGroup With {.Title = "Chemical Tools"}
    Private ArtTools As New EditorGroup With {.Title = "Art Tools"}
    Private LayoutTools As New EditorGroup With {.Title = "Layout Tools"}
    Private ShapeTools As New EditorGroup With {.Title = "Shape Tools"}
    Private BioFeatures As New EditorGroup With {.Title = "Biological Tools"}

    Public Sub New()
        Add(LinearTools)
        LinearTools.Add(New EditorModel(GetType(TextRightArrowLine)))
        LinearTools.Add(New EditorModel(GetType(TextLeftArrowLine)))
        LinearTools.Add(New EditorModel(GetType(TextRectangleLine)))
        LinearTools.Add(New EditorModel(GetType(TextEllipseLine)))
        LinearTools.Add(New EditorModel(GetType(TextDoubleRoundLine)))
        LinearTools.Add(New EditorModel(GetType(TextRightRoundLine)))
        LinearTools.Add(New EditorModel(GetType(TextLeftRoundLine)))
        LinearTools.Add(New EditorModel(GetType(TextSharpLine)))
        LinearTools.Add(New EditorModel(GetType(TextRightSharpLine)))
        LinearTools.Add(New EditorModel(GetType(TextLeftSharpLine)))
        LinearTools.Add(New EditorModel(GetType(LineBindLine)))
        LinearTools.ExpandSubtree()

        Add(RectangleTools)
        RectangleTools.Add(New EditorModel(GetType(TextRightArrow)))
        RectangleTools.Add(New EditorModel(GetType(TextLeftArrow)))
        RectangleTools.Add(New EditorModel(GetType(TextDoubleRoundRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextRightRoundRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextLeftRoundRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextEllipse)))
        RectangleTools.Add(New EditorModel(GetType(TextSharpRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextRightSharpRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextLeftSharpRectangle)))
        RectangleTools.Add(New EditorModel(GetType(TextRoundCornerRectangle)))
        RectangleTools.Add(New EditorModel(GetType(LineBindRectangle)))
        RectangleTools.Add(New EditorModel(GetType(StaticText)))
        RectangleTools.ExpandSubtree()

        Add(ChemicalTools)
        ChemicalTools.Add(New EditorModel(GetType(Atom)))
        ChemicalTools.Add(New EditorModel(GetType(Bond)))
        ChemicalTools.ExpandSubtree()

        Add(ArtTools)
        ArtTools.Add(New EditorModel(GetType(PolyPointTool)))
        ArtTools.ExpandSubtree()

        Add(LayoutTools)
        LayoutTools.Add(New EditorModel(GetType(HorizontalLayout)))
        LayoutTools.Add(New EditorModel(GetType(VerticalLayout)))
        LayoutTools.Add(New EditorModel(GetType(DegreeLineLayout)))
        LayoutTools.Add(New EditorModel(GetType(RelativeDegreeLineLayout)))
        LayoutTools.Add(New EditorModel(GetType(RelativeFreeDegreeLineLayout)))
        LayoutTools.Add(New EditorModel(GetType(RelativeLineLayout)))
        LayoutTools.ExpandSubtree()

        Add(ShapeTools)
        ShapeTools.Add(New EditorModel(GetType(EllipseShape)))
        ShapeTools.Add(New EditorModel(GetType(RectangleShape)))
        ShapeTools.Add(New EditorModel(GetType(RoundRectangleShape)))
        ShapeTools.ExpandSubtree()

        Add(BioFeatures)
        BioFeatures.Add(New EditorModel(GetType(RestrictionSite)))
        BioFeatures.ExpandSubtree()
    End Sub
    Private _Stage As FreeStage
    Public Sub Add(EditorGroupItem As EditorGroup)
        EditorGroupItem.Stage = _Stage
        Items.Add(EditorGroupItem)
    End Sub
    Public Property Stage As FreeStage
        Get
            Return _Stage
        End Get
        Set(value As FreeStage)
            For Each eg As EditorGroup In Items
                eg.Stage = value
            Next
        End Set
    End Property
End Class
Public Class PrimerToolBox
    Inherits TreeView
    Private BioFeatures As New EditorGroup With {.Title = "Primer Design"}

    Public Sub New()
        Add(BioFeatures)
        BioFeatures.Add(New EditorModel(GetType(PrimerOrigin)))
        BioFeatures.Add(New EditorModel(GetType(FeatureDesigner)))
        BioFeatures.Add(New EditorModel(GetType(DNASequence)))
        BioFeatures.Add(New EditorModel(GetType(RestrictionSite)))
        BioFeatures.Add(New EditorModel(GetType(AnimoSequence)))
        BioFeatures.ExpandSubtree()
    End Sub
    Private _Stage As FreeStage
    Public Sub Add(EditorGroupItem As EditorGroup)
        EditorGroupItem.Stage = _Stage
        Items.Add(EditorGroupItem)
    End Sub
    Public Property Stage As FreeStage
        Get
            Return _Stage
        End Get
        Set(value As FreeStage)
            For Each eg As EditorGroup In Items
                eg.Stage = value
            Next
        End Set
    End Property
End Class
Public Class DNAToolBox
    Inherits TreeView
    Private BioFeatures As New EditorGroup With {.Title = "Sequence Design"}
    Public Sub New()
        Add(BioFeatures)
        BioFeatures.Add(New EditorModel(GetType(SequenceOrigin)))
        BioFeatures.Add(New EditorModel(GetType(SequenceEnd)))
        BioFeatures.Add(New EditorModel(GetType(FeatureDesigner)))
        BioFeatures.Add(New EditorModel(GetType(DNASequence)))
        BioFeatures.Add(New EditorModel(GetType(RestrictionSite)))
        BioFeatures.Add(New EditorModel(GetType(AnimoSequence)))
        BioFeatures.ExpandSubtree()
    End Sub
    Private _Stage As FreeStage
    Public Sub Add(EditorGroupItem As EditorGroup)
        EditorGroupItem.Stage = _Stage
        Items.Add(EditorGroupItem)
    End Sub
    Public Property Stage As FreeStage
        Get
            Return _Stage
        End Get
        Set(value As FreeStage)
            For Each eg As EditorGroup In Items
                eg.Stage = value
            Next
        End Set
    End Property
End Class

<Shallow()>
Public Class EditorGroup
    Inherits TreeViewItem
    Private gdHost As New GridBase
    Private ebTitle As New EditBox
    Private btnAdd As New AddButton
    Public Sub New()
        Header = gdHost
        gdHost.Background = Brushes.White
        gdHost.AddColumnItem(IconImages.ImageFromString(IconImages.Folder, 24, 24))
        gdHost.AddColumnItem(ebTitle)
    End Sub
    Public Sub Add(EditorModelItem As EditorModel)
        EditorModelItem.Stage = _Stage
        Items.Add(EditorModelItem)
    End Sub
    Private _Stage As FreeStage
    Public Property Stage As FreeStage
        Get
            Return _Stage
        End Get
        Set(value As FreeStage)
            For Each em As EditorModel In Items
                em.Stage = value
            Next
        End Set
    End Property
    <Save()> Public Property Title As String
        Get
            Return ebTitle.Text
        End Get
        Set(value As String)
            ebTitle.Text = value
        End Set
    End Property
End Class

Public Class EditorModel
    Inherits TreeViewItem
    Private gdHost As New GridBase
    Private ModelType As Type
    Private ebTitle As New EditBox
    Private WithEvents btnAdd As New AddButton
    Public Sub New(Model As Type)
        Header = gdHost
        gdHost.Background = Brushes.White
        ebTitle.IsReadOnly = True
        gdHost.AddColumnItem(btnAdd)
        gdHost.AddColumnItem(ebTitle)
        ModelType = Model
        Try
            Dim el As IActor = Model.Assembly.CreateInstance(Model.FullName)
            el.Size = V(48, 24)
            ebTitle.Text = Model.Name
            gdHost.AddColumnItem(el)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub New(Model As String)
        Header = gdHost
        gdHost.Background = Brushes.White
        ebTitle.IsReadOnly = True
        gdHost.AddColumnItem(btnAdd)
        gdHost.AddColumnItem(ebTitle)
        Try
            Dim tModel As Type = Type.GetType(Model)
            Dim el As FrameworkElement = tModel.Assembly.CreateInstance(tModel.FullName)
            el.Width = 48
            el.Height = 24
            ebTitle.Text = tModel.Name
            gdHost.AddColumnItem(el)
            ModelType = tModel
        Catch ex As Exception

        End Try
    End Sub
    Public Property Stage As FreeStage

    Private Sub btnAdd_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        If TypeOf _Stage Is FreeStage Then
            _Stage.Add(ModelType)
        End If
    End Sub
End Class