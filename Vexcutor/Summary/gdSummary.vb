Imports System.Windows, System.Windows.Controls, System.Windows.Input, System.Windows.Media
Public Class gdSummary
    Inherits GridBase
    'Private AccountGrid As New GridBase
    'Private MyNameLabel As New Label With {.Content = "Name"}
    'Private MyName As New TextBox With {.MinWidth = 60}
    'Private MyEmailLabel As New Label With {.Content = "Email"}
    'Private MyEmail As New ComboBox With {.MinWidth = 80}
    'Private MyOrganisationLabel As New Label With {.Content = "Email"}
    'Private MyOrganisation As New ComboBox With {.MinWidth = 80}
    'Private MyAddressLabel As New Label With {.Content = "Address"}
    'Private MyAddress As New ComboBox With {.MinWidth = 120}
    Private svMain As New ScrollViewer
    Private gdScrollView As New GridBase

    Friend expConstruction As New Expander
    Friend gdConstructionHeader As New Label With {.Content = "Project Service", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdConstructionService As New ConstructionProject

    Friend expProject As New Expander
    Friend gdProjectHeader As New Label With {.Content = "Project Information", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdProject As New GridBase
    Friend gdProjectView As New GridBase
    Friend txtProjectInfo As New TextBox With {.TextWrapping = TextWrapping.Wrap, .AcceptsTab = True, .AcceptsReturn = True, .IsReadOnly = True, .FontFamily = New FontFamily("Arial"), .FontSize = 16}
    Friend gdProjectCommercer As New GridBase
    'Friend synConsume As New ConsumerablePurchaseOrder

    Friend expStrain As New Expander
    Friend gdStrainsHeader As New Label With {.Content = "Strains", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdStrains As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 255, 225, 225))}
    Friend gdStrainView As New GridBase
    Friend gdStrainCommercer As New GridBase
    'Friend synStrain As New StrainPurchaseOrder

    Friend expVector As New Expander
    Friend gdVectorsHeader As New Label With {.Content = "Vectors & DNA Fragments", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdVectors As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 255, 255, 225))}
    Friend gdVectorView As New GridBase
    Friend gdVectorCommercer As New GridBase
    'Friend synVector As New VectorPurchaseOrder

    Friend expEnzyme As New Expander
    Friend gdEnzymeHeader As New Label With {.Content = "Restriction Enzymes", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdEnzyme As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 225, 255, 225))}
    Friend gdEnzymeView As New GridBase
    Friend gdEnzymeCommercer As New GridBase
    'Friend synEnzyme As New EnzymePurchaseOrder

    Friend expTool As New Expander
    Friend gdToolHeader As New Label With {.Content = "Tool Enzymes", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdTool As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 225, 255, 255))}
    Friend gdToolView As New GridBase
    Friend gdToolCommercer As New GridBase
    'Friend synTool As New ToolPurchaseOrder

    Friend expPrimer As New Expander
    Friend gdPrimerHeader As New Label With {.Content = "Primers", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdPrimer As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 225, 225, 255))}
    Friend gdPrimerView As New GridBase
    Friend gdPrimerCommercer As New GridBase
    'Friend synPrimer As New PrimerSynthesis

    Friend expSequence As New Expander
    Friend gdSequenceHeader As New Label With {.Content = "Synthesized Sequences", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend gdSequence As New GridBase With {.Background = New SolidColorBrush(Color.FromArgb(255, 255, 225, 255))}
    Friend gdSequenceView As New GridBase
    Friend gdSequenceCommercer As New GridBase
    'Friend synSynthesis As New GeneSynthesisPanel

    Friend expProtocol As New Expander
    Friend gdProtocolSummary As New GridBase
    Friend SummaryTitle As New Label With {.Content = "Experimental Procedure", .FontSize = 18, .FontWeight = FontWeights.Bold}
    Friend SummaryText As New TextBox With {.TextWrapping = TextWrapping.Wrap, .AcceptsTab = True, .AcceptsReturn = True, .IsReadOnly = True, .FontFamily = New FontFamily("Arial"), .FontSize = 16}
    Public Sub New()
        'AddRowItem(AccountGrid)
        'AccountGrid.AddColumnItem(MyNameLabel)
        'AccountGrid.AddColumnItem(MyName)
        'AccountGrid.AddColumnItem(MyEmailLabel)
        'AccountGrid.AddColumnItem(MyEmail)
        'AccountGrid.AddColumnItem(MyOrganisationLabel)
        'AccountGrid.AddColumnItem(MyOrganisation)
        'AccountGrid.AddColumnItem(MyAddressLabel)
        'AccountGrid.AddColumnItem(MyAddress)

        AddRowItem(svMain, "*")
        svMain.Content = gdScrollView

        gdScrollView.AddRowItem(expConstruction)
        expConstruction.IsExpanded = True
        expConstruction.Header = gdConstructionHeader
        expConstruction.Content = gdConstructionService
        expProject.IsExpanded = True

        gdScrollView.AddRowItem(expProject)
        gdProject.AddColumnItem(gdProjectView, "*")
        gdProject.AddColumnItem(gdProjectCommercer, "*")
        gdProjectView.AddRowItem(txtProjectInfo, "*")
        expProject.Header = gdProjectHeader
        expProject.Content = gdProject
        expProject.IsExpanded = True
        'gdProjectCommercer.Children.Add(synConsume)

        gdScrollView.AddRowItem(expStrain)
        gdStrains.AddColumnItem(gdStrainView, "*")
        gdStrains.AddColumnItem(gdStrainCommercer, "*")
        expStrain.Header = gdStrainsHeader
        expStrain.Content = gdStrains
        expStrain.IsExpanded = True
        'gdStrainCommercer.Children.Add(synStrain)

        gdScrollView.AddRowItem(expVector)
        gdVectors.AddColumnItem(gdVectorView, "*")
        gdVectors.AddColumnItem(gdVectorCommercer, "*")
        expVector.Header = gdVectorsHeader
        expVector.Content = gdVectors
        expVector.IsExpanded = True
        'gdVectorCommercer.Children.Add(synVector)

        gdScrollView.AddRowItem(expEnzyme)
        gdEnzyme.AddColumnItem(gdEnzymeView, "*")
        gdEnzyme.AddColumnItem(gdEnzymeCommercer, "*")
        expEnzyme.Header = gdEnzymeHeader
        expEnzyme.Content = gdEnzyme
        expEnzyme.IsExpanded = True
        'gdEnzymeCommercer.Children.Add(synEnzyme)

        gdScrollView.AddRowItem(expTool)
        gdTool.AddColumnItem(gdToolView, "*")
        gdTool.AddColumnItem(gdToolCommercer, "*")
        expTool.Header = gdToolHeader
        expTool.Content = gdTool
        expTool.IsExpanded = True
        'gdToolCommercer.Children.Add(synTool)

        gdScrollView.AddRowItem(expPrimer)
        gdPrimer.AddColumnItem(gdPrimerView, "*")
        gdPrimer.AddColumnItem(gdPrimerCommercer, "*")
        expPrimer.Header = gdPrimerHeader
        expPrimer.Content = gdPrimer
        expPrimer.IsExpanded = True
        'gdPrimerCommercer.Children.Add(synPrimer)

        gdScrollView.AddRowItem(expSequence)
        gdSequence.AddColumnItem(gdSequenceView, "*")
        gdSequence.AddColumnItem(gdSequenceCommercer, "*")
        expSequence.Header = gdSequenceHeader
        expSequence.Content = gdSequence
        expSequence.IsExpanded = True
        'gdSequenceCommercer.Children.Add(synSynthesis)

        gdScrollView.AddRowItem(expProtocol)
        gdProtocolSummary.AddRowItem(SummaryText)
        expProtocol.Header = SummaryTitle
        expProtocol.Content = gdProtocolSummary
        expProtocol.IsExpanded = True
    End Sub
    Public Property Summary As String
        Get
            Return SummaryText.Text
        End Get
        Set(value As String)
            SummaryText.Text = value
        End Set
    End Property
    Public Sub LoadSummary(smr As SummaryEventArgs, wControl As WorkControl, Section As SummarySectionEnum)
        gdConstructionService.Project = wControl.GetWorkSpace
        gdConstructionService.ProjectName = wControl.CurrentFileName
        'gdConstructionService.DataContext = LoginManagement.ProjectPrice

        'If wControl.Quoted Then
        '    If LoginManagement.Customer.IsContractor Then
        '        expConstruction.Content = New ProjectUpdater With {.HostWorkControl = wControl, .ProjectName = wControl.CurrentFileName}
        '    Else
        '        expConstruction.Content = New ProjectRefresher With {.HostWorkControl = wControl, .ProjectName = wControl.CurrentFileName}
        '    End If
        'End If

        Dim sConsumeOrder As New SynContract.ConsumerableOrder
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Gel Extraction Kit"})
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Plasmid Mini Prep Kit"})
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Triptone"})
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Yeast Extract"})
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Agar"})
        sConsumeOrder.Consumerables.Add(New SynContract.ConsumerableOrderItem With {.Name = "Electrophosis Agarose"})
        'synConsume.Order = sConsumeOrder

        SummaryText.Text = smr.Summary
        gdStrainView.Children.Clear()
        For Each sb As SummaryBase In smr.Strains.Values
            sb.Show()
            gdStrainView.AddRowItem(sb)
        Next
        Dim sStrainOrder As New SynContract.StrainOrder
        sStrainOrder.Name = wControl.CurrentFileName
        For Each sb As SummaryBase In smr.Strains.Values
            Dim sqs As HostSummary = sb
            sStrainOrder.Strains.Add(New SynContract.StrainOrderItem With {.Name = sqs.Cell.Name})
        Next
        'synStrain.Order = sStrainOrder

        gdVectorView.Children.Clear()
        For Each sb As SummaryBase In smr.Vectors.Values
            sb.Show()
            gdVectorView.AddRowItem(sb)
        Next
        Dim sVectorOrder As New SynContract.VectorOrder
        sVectorOrder.Name = wControl.CurrentFileName
        For Each sb As SummaryBase In smr.Vectors.Values
            Dim sqs As VectorSummary = sb
            sVectorOrder.Vectors.Add(New SynContract.VectorOrderItem With {.Name = sqs.Vector.Name, .IsCircular = sqs.Vector.Iscircular, .Sequence = sqs.Vector.Sequence})
        Next
        'synVector.Order = sVectorOrder

        gdEnzymeView.Children.Clear()
        For Each sb As SummaryBase In smr.Enzymes.Values
            sb.Show()
            gdEnzymeView.AddRowItem(sb)
        Next
        Dim sEnzymeOrder As New SynContract.EnzymeOrder
        sEnzymeOrder.Name = wControl.CurrentFileName
        For Each sb As SummaryBase In smr.Enzymes.Values
            Dim sqs As EnzymeSummary = sb
            sEnzymeOrder.Enzymes.Add(New SynContract.EnzymeOrderItem With {.Name = sqs.Enzyme})
        Next
        'synEnzyme.Order = sEnzymeOrder

        gdToolView.Children.Clear()
        For Each sb As SummaryBase In smr.ToolEnzymes.Values
            sb.Show()
            gdToolView.AddRowItem(sb)
        Next
        Dim sToolEnzymeOrder As New SynContract.ToolEnzymeOrder
        sToolEnzymeOrder.Name = wControl.CurrentFileName
        For Each sb As SummaryBase In smr.ToolEnzymes.Values
            Dim sqs As ToolSummary = sb
            sToolEnzymeOrder.ToolEnzymes.Add(New SynContract.ToolEnzymeOrderItem With {.Name = sqs.Enzyme})
        Next
        'synTool.Order = sToolEnzymeOrder

        gdPrimerView.Children.Clear()
        For Each sb As SummaryBase In smr.Primers.Values
            sb.Show()
            gdPrimerView.AddRowItem(sb)
        Next
        Dim sPrimerOrder As New SynContract.PrimerOrder
        sPrimerOrder.Name = wControl.CurrentFileName
        For Each sb As SummaryBase In smr.Primers.Values
            Dim sqs As PrimerSummary = sb
            sPrimerOrder.Primers.Add(New SynContract.PrimerOrderItem With {.Sequence = Nuctions.TAGCFilter(sqs.Sequence), .Length = Nuctions.TAGCFilter(sqs.Sequence).Length, .Name = sqs.Name})
        Next
        'synPrimer.Order = sPrimerOrder
        'synPrimer.Price = LoginManagement.PrimerPrice

        gdSequenceView.Children.Clear()
        For Each sb As SummaryBase In smr.Sequences.Values
            sb.Show()
            gdSequenceView.AddRowItem(sb)
        Next
        Dim sSynthesisOrder As New SynContract.GeneSynthesisOrder
        For Each sb As SummaryBase In smr.Sequences.Values
            Dim sqs As SequenceSummary = sb
            sSynthesisOrder.DNAList.Add(New SynContract.DNAOrderItem With {.Sequence = sqs.Sequence, .Length = sqs.Sequence.Length, .DNAID = sqs.Name})
        Next
        'synSynthesis.Order = sSynthesisOrder
        'synSynthesis.Price = LoginManagement.Price
        SummaryText.Text = smr.Append

        ScrollSection = Section
        ScrollToSection()
    End Sub
 
    Private ScrollSection As SummarySectionEnum = SummarySectionEnum.Project
    Private Sub gdSummary_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ScrollToSection()
    End Sub
    Private Sub ScrollToSection()
        Dim ViewStart As UIElement = Nothing
        Select Case ScrollSection
            Case SummarySectionEnum.Project
                ViewStart = expConstruction
            Case SummarySectionEnum.Strain
                ViewStart = expStrain
            Case SummarySectionEnum.Vector
                ViewStart = expVector
            Case SummarySectionEnum.Enzyme
                ViewStart = expEnzyme
            Case SummarySectionEnum.Tool
                ViewStart = expTool
            Case SummarySectionEnum.Synthesis
                ViewStart = expSequence
            Case SummarySectionEnum.Primer
                ViewStart = expPrimer
            Case SummarySectionEnum.Protocol
                ViewStart = expProtocol
        End Select
        If ViewStart IsNot Nothing Then
            Dim p = ViewStart.TranslatePoint(New Point(0.0#, 0.0#), svMain)
            svMain.ScrollToVerticalOffset(p.Y)
        End If
    End Sub
End Class

Public Class ExtensiveStringBuilder
    Private builder As New System.Text.StringBuilder
    Public Sub AppendLine(Value As String)
        If TypeOf Value Is String Then
            builder.AppendLine(Value)
        Else
            builder.AppendLine("")
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return builder.ToString()
    End Function
End Class
Public MustInherit Class SummaryBase
    Inherits GridBase
    Private Shared gfBrush As New SolidColorBrush(Color.FromArgb(255, 215, 255, 128))
    Protected Friend CheckButton As New CartButton
    Protected Friend NameLabel As New Label With {.FontWeight = FontWeights.Bold, .Foreground = gfBrush, .Background = Brushes.Black}
    Protected Friend NodeLabel As New Label With {.FontStyle = FontStyles.Italic, .Foreground = Brushes.DarkBlue}
    Protected Friend AdditionalGrid As New GridBase
    Protected Friend CommentText As New TextBox With {.Background = Nothing, .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        DefineRowsByDoubles(24D, 24D, 0D, -1D)
        DefineColumnsByDoubles(36D, -1D)
        AddChild(CheckButton, 0, 0, 1, 2)
        AddChild(NameLabel, 1, 0, 1, 1)
        AddChild(NodeLabel, 1, 1, 1, 1)
        AddChild(AdditionalGrid, 0, 2, 2, 1)
        AddChild(CommentText, 0, 3, 2, 1)
    End Sub
    Public Property Selected As Boolean
        Get
            Return CheckButton.Checked
        End Get
        Set(value As Boolean)
            CheckButton.Checked = value
        End Set
    End Property
    Public MustOverride Sub Show()
End Class

Public Class VectorSummary
    Inherits SummaryBase

    Public Vector As Nuctions.GeneFile
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder

    Public Overrides Sub Show()
        NameLabel.Content = Vector.Name + " (" + Vector.Length.ToString + "bp)"
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class

Public Class HostSummary
    Inherits SummaryBase

    Public Cell As Nuctions.Host
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder

    Public Overrides Sub Show()
        NameLabel.Content = Cell.Name
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class

Public Class EnzymeSummary
    Inherits SummaryBase

    Public Enzyme As String = ""
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder

    Public Overrides Sub Show()
        NameLabel.Content = Enzyme
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class

Public Class ToolSummary
    Inherits SummaryBase

    Public Enzyme As String = ""
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder

    Public Overrides Sub Show()
        NameLabel.Content = Enzyme
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class

Public Class PrimerSummary
    Inherits SummaryBase

    Public Shadows Name As String = ""
    Public AliasNames As New List(Of String)
    Public Sequence As String = ""
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder

    Protected Friend SequenceText As New TextBox With {.TextWrapping = TextWrapping.Wrap, .Background = Nothing, .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        AdditionalGrid.AddRowItem(SequenceText)
    End Sub
    Public Overrides Sub Show()
        NameLabel.Content = Name
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        SequenceText.Text = Sequence
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class

Public Class SequenceSummary
    Inherits SummaryBase

    Public Shadows Name As String = ""
    Public Sequence As String = ""
    Public Nodes As New List(Of String)
    Public Comments As New ExtensiveStringBuilder
    Protected Friend SequenceText As New TextBox With {.TextWrapping = TextWrapping.Wrap, .Background = Nothing, .BorderBrush = Brushes.Black, .IsReadOnly = True}
    Public Sub New()
        AdditionalGrid.AddRowItem(SequenceText)
    End Sub
    Public Overrides Sub Show()
        NameLabel.Content = Name
        NodeLabel.Content = String.Join(", ", Nodes.ToArray)
        SequenceText.Text = Sequence
        CommentText.Text = Comments.ToString
        If CommentText.Text.Length = 0 Then CommentText.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class


Public Class SupplierItem
    Inherits GridBase
    Private CommerceGrid As New GridBase
    Private LogoImage As New Image
    Private Header As New Label
    Private InformationGrid As New GridBase
    Private CommentsLabel As New Label With {.Content = "Additional Information"}
    Private EmailTo As New EmailButton
    Private Comments As New TextBox With {.HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .VerticalAlignment = Windows.VerticalAlignment.Stretch}
    Public GetEmailContent As System.Func(Of String)
    Public GetAttached As System.Func(Of Byte())
    Public GetSender
    Public lbTelephone As New Label
    'Private AccountGrid As New GridBase
    'Private MyNameLabel As New Label With {.Content = "Name"}
    'Private MyName As New TextBox With {.MinWidth = 60}
    'Private MyEmailLabel As New Label With {.Content = "Email"}
    'Private MyEmail As New ComboBox With {.MinWidth = 80}
    'Private MyLabLabel As New Label With {.Content = "Email"}
    'Private MyLab As New ComboBox With {.MinWidth = 80}
    Public Sub New()
        CommerceGrid.Height = 120
        CommerceGrid.AddColumnItem(LogoImage, "240")
        CommerceGrid.AddColumnItem(Header, "*")
        AddRowItem(CommerceGrid)
        InformationGrid.AddColumnItem(CommentsLabel)
        InformationGrid.AddColumnItem(EmailTo)
        AddRowItem(InformationGrid)
        AddRowItem(Comments)
        'AddRowItem(AccountGrid)
        'AccountGrid.AddRowItem(MyNameLabel)
        'AccountGrid.AddRowItem(MyName)
        'AccountGrid.AddRowItem(MyEmailLabel)
        'AccountGrid.AddRowItem(MyEmail)
    End Sub
    <Save()> Public Property Logo As Byte()
    <Save()> Public Property CompanyName As Byte()
    <Save()> Public Property Email As String
    <Save()> Public Property Telephone As String
    <Save()> Public Property URL As String
    <Save()> Public Property Introduction As String
    <Save()> Public Property Details As String 'Details are actually the HTML webpage. It will direct the HTML page to the website.
End Class

Public Class EmailButton
    Inherits BaseButton
    Public Sub New()
        MyBase.New(CommercePackImage.Email, 18)
    End Sub
    Protected Overrides ReadOnly Property Base64Image As String
        Get
            Return IconImages.Animator
        End Get
    End Property
End Class

Public Class CartButton
    Inherits BaseButton
    Public Sub New()
        MyBase.New(CommercePackImage.Cart, 36)
        IsCheckable = True
    End Sub
    Protected Overrides ReadOnly Property Base64Image As String
        Get
            Return IconImages.Animator
        End Get
    End Property
End Class

Public Class CommercePackImage
    Friend Const Email As String = "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABGdBTUEAALGOfPtRkwAAACBjSFJNAACHDwAAjA8AAP1SAACBQAAAfXkAAOmLAAA85QAAGcxzPIV3AAAKOWlDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAEjHnZZ3VFTXFofPvXd6oc0w0hl6ky4wgPQuIB0EURhmBhjKAMMMTWyIqEBEEREBRZCggAGjoUisiGIhKKhgD0gQUGIwiqioZEbWSnx5ee/l5ffHvd/aZ+9z99l7n7UuACRPHy4vBZYCIJkn4Ad6ONNXhUfQsf0ABniAAaYAMFnpqb5B7sFAJC83F3q6yAn8i94MAUj8vmXo6U+ng/9P0qxUvgAAyF/E5mxOOkvE+SJOyhSkiu0zIqbGJIoZRomZL0pQxHJijlvkpZ99FtlRzOxkHlvE4pxT2clsMfeIeHuGkCNixEfEBRlcTqaIb4tYM0mYzBXxW3FsMoeZDgCKJLYLOKx4EZuImMQPDnQR8XIAcKS4LzjmCxZwsgTiQ7mkpGbzuXHxArouS49uam3NoHtyMpM4AoGhP5OVyOSz6S4pyalMXjYAi2f+LBlxbemiIluaWltaGpoZmX5RqP+6+Dcl7u0ivQr43DOI1veH7a/8UuoAYMyKarPrD1vMfgA6tgIgd/8Pm+YhACRFfWu/8cV5aOJ5iRcIUm2MjTMzM424HJaRuKC/6386/A198T0j8Xa/l4fuyollCpMEdHHdWClJKUI+PT2VyeLQDf88xP848K/zWBrIieXwOTxRRKhoyri8OFG7eWyugJvCo3N5/6mJ/zDsT1qca5Eo9Z8ANcoISN2gAuTnPoCiEAESeVDc9d/75oMPBeKbF6Y6sTj3nwX9+65wifiRzo37HOcSGExnCfkZi2viawnQgAAkARXIAxWgAXSBITADVsAWOAI3sAL4gWAQDtYCFogHyYAPMkEu2AwKQBHYBfaCSlAD6kEjaAEnQAc4DS6Ay+A6uAnugAdgBIyD52AGvAHzEARhITJEgeQhVUgLMoDMIAZkD7lBPlAgFA5FQ3EQDxJCudAWqAgqhSqhWqgR+hY6BV2ArkID0D1oFJqCfoXewwhMgqmwMqwNG8MM2An2hoPhNXAcnAbnwPnwTrgCroOPwe3wBfg6fAcegZ/DswhAiAgNUUMMEQbigvghEUgswkc2IIVIOVKHtCBdSC9yCxlBppF3KAyKgqKjDFG2KE9UCIqFSkNtQBWjKlFHUe2oHtQt1ChqBvUJTUYroQ3QNmgv9Cp0HDoTXYAuRzeg29CX0HfQ4+g3GAyGhtHBWGE8MeGYBMw6TDHmAKYVcx4zgBnDzGKxWHmsAdYO64dlYgXYAux+7DHsOewgdhz7FkfEqeLMcO64CBwPl4crxzXhzuIGcRO4ebwUXgtvg/fDs/HZ+BJ8Pb4LfwM/jp8nSBN0CHaEYEICYTOhgtBCuER4SHhFJBLVidbEACKXuIlYQTxOvEIcJb4jyZD0SS6kSJKQtJN0hHSedI/0ikwma5MdyRFkAXknuZF8kfyY/FaCImEk4SXBltgoUSXRLjEo8UISL6kl6SS5VjJHslzypOQNyWkpvJS2lIsUU2qDVJXUKalhqVlpirSptJ90snSxdJP0VelJGayMtoybDFsmX+awzEWZMQpC0aC4UFiULZR6yiXKOBVD1aF6UROoRdRvqP3UGVkZ2WWyobJZslWyZ2RHaAhNm+ZFS6KV0E7QhmjvlygvcVrCWbJjScuSwSVzcopyjnIcuUK5Vrk7cu/l6fJu8onyu+U75B8poBT0FQIUMhUOKlxSmFakKtoqshQLFU8o3leClfSVApXWKR1W6lOaVVZR9lBOVd6vfFF5WoWm4qiSoFKmclZlSpWiaq/KVS1TPaf6jC5Ld6In0SvoPfQZNSU1TzWhWq1av9q8uo56iHqeeqv6Iw2CBkMjVqNMo1tjRlNV01czV7NZ874WXouhFa+1T6tXa05bRztMe5t2h/akjpyOl06OTrPOQ12yroNumm6d7m09jB5DL1HvgN5NfVjfQj9ev0r/hgFsYGnANThgMLAUvdR6KW9p3dJhQ5Khk2GGYbPhqBHNyMcoz6jD6IWxpnGE8W7jXuNPJhYmSSb1Jg9MZUxXmOaZdpn+aqZvxjKrMrttTjZ3N99o3mn+cpnBMs6yg8vuWlAsfC22WXRbfLS0suRbtlhOWWlaRVtVWw0zqAx/RjHjijXa2tl6o/Vp63c2ljYCmxM2v9ga2ibaNtlOLtdZzllev3zMTt2OaVdrN2JPt4+2P2Q/4qDmwHSoc3jiqOHIdmxwnHDSc0pwOub0wtnEme/c5jznYuOy3uW8K+Lq4Vro2u8m4xbiVun22F3dPc692X3Gw8Jjncd5T7Snt+duz2EvZS+WV6PXzAqrFetX9HiTvIO8K72f+Oj78H26fGHfFb57fB+u1FrJW9nhB/y8/Pb4PfLX8U/z/z4AE+AfUBXwNNA0MDewN4gSFBXUFPQm2Dm4JPhBiG6IMKQ7VDI0MrQxdC7MNaw0bGSV8ar1q66HK4RzwzsjsBGhEQ0Rs6vdVu9dPR5pEVkQObRGZ03WmqtrFdYmrT0TJRnFjDoZjY4Oi26K/sD0Y9YxZ2O8YqpjZlgurH2s52xHdhl7imPHKeVMxNrFlsZOxtnF7YmbineIL4+f5rpwK7kvEzwTahLmEv0SjyQuJIUltSbjkqOTT/FkeIm8nhSVlKyUgVSD1ILUkTSbtL1pM3xvfkM6lL4mvVNAFf1M9Ql1hVuFoxn2GVUZbzNDM09mSWfxsvqy9bN3ZE/kuOd8vQ61jrWuO1ctd3Pu6Hqn9bUboA0xG7o3amzM3zi+yWPT0c2EzYmbf8gzySvNe70lbEtXvnL+pvyxrR5bmwskCvgFw9tst9VsR23nbu/fYb5j/45PhezCa0UmReVFH4pZxde+Mv2q4quFnbE7+0ssSw7uwuzi7Rra7bD7aKl0aU7p2B7fPe1l9LLCstd7o/ZeLV9WXrOPsE+4b6TCp6Jzv+b+Xfs/VMZX3qlyrmqtVqreUT13gH1g8KDjwZYa5ZqimveHuIfu1nrUttdp15UfxhzOOPy0PrS+92vG140NCg1FDR+P8I6MHA082tNo1djYpNRU0gw3C5unjkUeu/mN6zedLYYtta201qLj4Ljw+LNvo78dOuF9ovsk42TLd1rfVbdR2grbofbs9pmO+I6RzvDOgVMrTnV32Xa1fW/0/ZHTaqerzsieKTlLOJt/duFczrnZ86nnpy/EXRjrjup+cHHVxds9AT39l7wvXbnsfvlir1PvuSt2V05ftbl66hrjWsd1y+vtfRZ9bT9Y/NDWb9nffsPqRudN65tdA8sHzg46DF645Xrr8m2v29fvrLwzMBQydHc4cnjkLvvu5L2key/vZ9yff7DpIfph4SOpR+WPlR7X/aj3Y+uI5ciZUdfRvidBTx6Mscae/5T+04fx/Kfkp+UTqhONk2aTp6fcp24+W/1s/Hnq8/npgp+lf65+ofviu18cf+mbWTUz/pL/cuHX4lfyr468Xva6e9Z/9vGb5Dfzc4Vv5d8efcd41/s+7P3EfOYH7IeKj3ofuz55f3q4kLyw8Bv3hPP74uYdwgAAAAlwSFlzAAALEgAACxIB0t1+/AAABPlJREFUSEu11n1oVXUcx/Hvyd17FLJpWaI0UG4gOFJsaWJYMfCBiXM52BAHs6k4XOqsIQ4f2tDmctoybXnEEMMeoOgJohQzdyvpiEnqztFterZ77+bSTZ0P0xHUr/fv3HNnBv25P/bviw+f7+f87kQpJYP5N6i4Di57Hjkju0eeNXaNOiv1o8/JzjFNsj3DkdpxjtREXNky4bxUZ16QzZOaZeMzLVI57aKsm3FRXn/hkpRne7J6druU5bRLaW5cli9MSElBhxQv7pCi4svGoqVdovHXwKPgjTvHNp2oy2g6WTvOtWuecu2tE87b4Pbmyc32xqwWu3Jaqw1ug9trsz179Zw2u2xeu126IG4vz4/bJYWJk8WLO38tWnK5cdGyrmhB6R9vC/g+cAWu6jKcLpK7JPdI7lVPvOCR3NuQ1eJVPtfqgXvgHsk9knvg3ooFMQ/cKyns8MBbioo7W8AVuMp/9UpU3hl9bo/Gt2c0KfCfST4K3CB5CDxELSGSh8BD4CHw0OrZbSFqCZUuiIWW5SdCrxR2hMDTqOUhaqkvKO3SuMorv3pEdo49txe8H3w/uAI/Bp5+v/NWv/MKOqcWOm8LOo/J8nw6L0ykOheS1wXJfwDvnF/R/aPUPdlkgf9FLaPBN1CLemNScyPJR1RO/T9cHzQe4J36oELytwpWUEvZle9fLr86Mreiu3Xe+p6oXosVJB8XrKUGXFHLMZKn319LKnmA+2sZwGv95GVXjuStuTqc5AKemLvxWlTALZIrkkdYi7AWpti6DVxRy1E6T2ctwkGFzoNagikuIfmyrm3/qmWEj1f2hMHjs6quR4UpWtWZ53UtkQBPdb4DXNH5d+DDk/h/avFx/6DH6fyxILnM3XQtDB7P3nojKiS3OKiilgjJBz6i4KBvshYF/i348AcOunQg+WHwdI3nrO8Rkgt4GDz+Ym1vVH+h1ga/84uRB79Qv/Oa0tyYYi2KnX/DFzosOOgW/6DJKX7JQcN0LnOSuICHX6rtjc+suxnVn78FrtbNuBRJHXQVU1yZ077dxxfGvwDfxUEVuM1avg7Wcpi1NMyv6FHgX5F8WIALeBg8PqP+VlQf1AoOGlmb3SYaL8uJJZMvTBwtKUg8HKxlK/ht8D9Zy2d5a7pTtbwPrmZV3fjcT76tV8DDz9ffik9/93ZUd26RXNH5+GAtVXSua/mJzkf4eHItwlrGUMv4vPJuCdaiDzoE/ANwRfKPwYeQ3ABPTGu4E9WvosVa/l41u/2JlfNiFbwtGv8F/NGBnSdxARc6B+9JHnTTdZlV7XeexkEPztxxU4EfmL77dhr4pSyrL6qf3L1M8R4HfY9XMYWPCp5c/YU+gHNQ/RGl1gLeK+ACnkYth0iuwA+Ct0/Zf/e4flv2+FPM9fETJH88SK7fc4PODZIbHNQAN8ANOjeoxSC5QecGtRjgAp42teHOp89afWrK/j41+cC9I3qKVrAWpphIkPw3nlwX3KUWl4O61OLSuQvugrusxQV36dwFd8FdcJfkv2ft62sOcPX0h/ca+SWKrWctp/klOkXyM0xR4w7JHXCHt8UBd8AdcAfcAXfAHXCHzh1wh+QOyV3ws5MP3D0Ffjrzo/4G/fmnkXwouFm0pNNkLSbJTWoxqcUENzmoCW6Cm+AmuEnnJslNcJPkJrhJ5ya4CW5mHuofOvGT/tDg/+gP5r8s2v4H3KjvDvZq2tkAAAAASUVORK5CYII="
    Friend Const Cart As String = "iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAACKZJREFUWEftl3tU0+cZx2ldW9e19qxHT1ETAhSlotN60CromWI9bTedbRXt8agbs86zza1qK4g38C4oiFyqIJAruSfkSiC/JIQQggTCVcBLXXUIFiVKEQwXIc+eN8LK5nq4TP/acs5zfr/fm/fyyff5vs+bn5fX/z9jUMCVNp/hzlz01hiGPJ+u4OX1Qs/p4OjHksWtj/VLb/bmL9nwfFYa5aw922e+8yh+fjdYlwFcDYOB/IUN7qzQ10c5/Nl369gUGNh1YK4L5CHQb10DTfLPaoHlNfHZrzTKGUnKXDtm7+mLndP099RPb4mF4usymeydUQ5/ft0ebQ2c1vq531tikfi8XCbXNTc3z+zs7Jzd0dExw+VyMbq6uqY+fPhwstPpnAQAEzEmPD+aYTPzeLwgmVTacfXK1fZbt251YbQ3NTW13r59uwkhb2A03LlzpxKj9O7du6a2tra8+/fvy+7du8fH5yx8TnE+cMYheEx7e/uetgdtO/C7ra2tLRtbWlo+xvgAxy7DORfevHlzZGtIJJIJYoHQYimyQEmJDWw2G5SVlYHdXv7PKC8vh4ryCignge3kvqLCAZWOSqiqrILamlqov1wPjQ2NcKXxCjQ2NkJDfQPU1V2GmpoacDgcUFVV1YP3lN1u9x5RaUFOzkGNWg1GoxEKC81gMhXivQkoygB5eTrPNT+/ALTaPJBKpCASiEAikoBULAU+jw/ZmdnAzGICl831fMfj5gCbyQYOi+P5XiaWefobKSP5weEjAnG53FCxUNhHQBCoyWAwrzYazeEGg6mVAOr1lEano36t1Wr5Go0WlAoVyCSyWtwQh/g5/LIcBCAwCHCPy+LK8do8+EzAetnZ7DpWNstF4OQS+W9HBMrKynqdy+E0GFAJi6W4cGhAYWGhnaTRYDCsI216vf6XqNiATqsDhULhaROJRGFCgWhAwBcAgkWQNvyBv+FxeB51OBxOJLEFi8X6XMgXEvCRgcgkrOzsTIU8F8xmc09RkUWAMFJM2wDek5TVUpTxACpViuFJn1qtVeHnI1RKIJXIANUiQOf4fP4MhDg8BISKRemSda/w2LxtYwLKvngxnMNioxpGNDMxsAOKi60IWARWa4nH8OQZFfS0kX75ugKPx7SaPFApVR4oVOB7kkLiJRII1IcprMH2bqIYgm0eMWWkA5PJpGdcuNCqVCg9i+OizajSOvTSZlSqjUCgUg/R+EcpijqNKrl0unxQqdR9ilyVCtOiFwnEbjGalwBx2Jw6BLk6BEcUI/ejBkKmF75OS9Pk8HierW+xWIZ5yVxus5WS3UcN/TqEsxNIhDINteGuq1bkKgH9dCnDkfESQr6BylzxAOKOGyuQV3JS8q6M9HQoQI9YrdZui8XKNJstfNxpfWRxo7HQiQrtRD/tRS89JLsSlbqh1RaEKZXKVYpcxR0V7kCJUFIj4Uh8xDxeIAL9bfxACQnzE88kPBIJRahSKRY/UgCfFMSyMrvHQ6R9KMizB0pX0K9Ra90ew6OviEqYvu8EPEEb8dWgmceuUGxs7MS4k6eq0UtQVPTEzMXFxb3FxUWZqFQ8eqqDtKOn3BhFWA6wEhj6jQYTkJKh1WjrUSGtXJ7brVKoQSFXAIFC/9STujTmlBEvnDp2LCkh/jRWZS2U4TGBaWsY8ghCqYhSqMrlH7xkNJPSgOo4DVqtP2nHHZeeh7VKJpW1S4XS0IyMjFcRSETUQrjfjWqXDXU6cujIr47ExAIrmwmXLpURlTpLSkq+RKNvwYWbrVYbqeZ3sJqvRMUWoVLXBstAF+6+jZSGmoFKUU9Sp/g+Nzc3RKfTvYIVXUyOkBxOzuoxAcXu2eO9P3pf0+m4eEybBUpLL6GHyj0eGiwHHi+hSn1o8m7Sh5x75IghZx4q1Tl43AClN5C0uWQSaVMe1ipBjsBKdt6YgEjn6MhIycF9+0GtUuMpXY2ndSVU4qlOTm0SxOSkjZz4xOBDBXTwQB5AXz0oKNDfVqvzriuV6nqFXOnIleUmyeXyqWOGIQO+2rVr+1e7d8PZxCSnzV5bVmSrLjOXVhuNNofKUmrnYfpSMXUn0OBRWK+2YYlYj9cPTRZLqDS/evYZ2QNGghomz+W6f+YF8OK4IIYP2h0V9Yv9B2L69h887BSf/YuxPvljw7VT76vyI9dKjkVF86OPx0lijhzNjzlyXI9XT8QePUbti4nTnD2+R15x7iPF3eSFqm+PhqplkZu1h0/E647HJ5acjE9YMy44zPPLKefTv3CIIpwDpSvw7WQ5wPUwgLoV8F36GuAfPqg/ez4jMSXtfMpQnEvLSJKyD1Mdhk8AGnBM00qAWxjF74N9b3hfeurXqSkXLrw3LiAyyH1jwVq3LaT7cX4I9BqXQg+1BHrEi6D/y1nQ8yHNiC8LT/3HdtctlkFhCPSocYwmBLpFi8AVNw8eb/Drc4e9uXXcMACxL7qvrrBA1XLoFy6Ax4lzoXdfEDyKeBtc6/0A1s4E1wdvfzZ8AffNlbMG6pZ3AQHPCobuE/Pg0c4g6NgyE/o3BELnqgAHLPP6ybigCJAoaYH4YvRCMKVuBmP8JqBiN8I36dFQEbsNUpcGDmiWMD4dPrlDHTx5+/pZ147+aSXI0/ZC5sE/gOniSbiiYMLpjavgr3N8+bD+aVVHDThhwrTVfvQA+P2WCIg7cw6SL2SCXJ0HX+zcCdN9A77x8vr5U/Vk0iQae8G7wRAdfQjiE1OBK5JBJpMFi0OXwEve00f3P+jHCGk02k/9fBjK4DlBsGndJ7Brxx9hU/hamBs0y+1HY/xHPzAYjHcDfH1bwkLeg+0RW+DPWyNgxdJQmOHrSwUEBEwatRo/1tHf3/8NXxrthB/d53IAg/GtH51eSJ9G/xfv/PtY3+m+8xg0H7Y/3afWn04v9/GedpI2ifbmfw0zfIIpU6a85v2a9xRsG8Pb69RXsf/LzxTkf26yfwCsm3iEtLN8xwAAAABJRU5ErkJggg=="
End Class