Imports System.Windows.Controls, System.Windows.Controls.Primitives, System.Windows
Public Class AminoAcidDropDown
    Inherits System.Windows.Controls.Primitives.Popup
    Public Border As New Border With {
        .CornerRadius = New CornerRadius(2.0#)
    }
    Public Grid As New Grid
    ''' <summary>
    ''' 默认的自带的table
    ''' </summary>
    ''' <remarks></remarks>
    Private _DefaultTable As New Nuctions.CodonTable
    Public Sub New()
        Child = Border
        Border.Child = Grid
        For i As Integer = 0 To 4
            Grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = GridLength.Auto})
        Next
        For i As Integer = 0 To 4
            Grid.RowDefinitions.Add(New RowDefinition With {.Height = GridLength.Auto})
        Next
        For i As Integer = 0 To 20
            Dim rbtn As New System.Windows.Controls.Ribbon.RibbonButton With {.Content = Nuctions.ParseNumberToAminoAcid(i)}
            Grid.SetColumn(rbtn, i Mod 5)
            Grid.SetRow(rbtn, i \ 5)
            Grid.Children.Add(rbtn)
            AddHandler rbtn.Click, AddressOf OnClick
        Next
    End Sub
    Public Sub OnClick(sender As Object, e As RoutedEventArgs)

    End Sub
    Public Sub Show(Target As Object)
        Me.PlacementTarget = Target
    End Sub
    'AminoAcidDropDown->CodonTable As Nuctions.CodonTable with Event Default: Nothing
    Public Property CodonTable As Nuctions.CodonTable
        Get
            Return GetValue(CodonTableProperty)
        End Get
        Set(ByVal value As Nuctions.CodonTable)
            SetValue(CodonTableProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CodonTableProperty As DependencyProperty = _
                           DependencyProperty.Register("CodonTable", _
                           GetType(Nuctions.CodonTable), GetType(AminoAcidDropDown), _
                           New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf SharedCodonTableChanged)))
    Private Shared Sub SharedCodonTableChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, AminoAcidDropDown).CodonTableChanged(d, e)
    End Sub
    Private Sub CodonTableChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If e.NewValue Is _DefaultTable Then
            'nothing to do
        Else
            Dim tb As Nuctions.CodonTable = e.NewValue
            _DefaultTable.Name = tb.Name
            _DefaultTable.Clear()
            For Each cod In tb.Values
                Dim c As Nuctions.Codon = Nuctions.AnminoAcidParse(cod.ShortName)
                For Each g As Nuctions.GeneticCode In C.CodeList
                    c.AddCode(g.Clone)
                Next
            Next
        End If
    End Sub
End Class

Public Class CodonTable
    Inherits Grid
    Public Sub New()
        For i As Integer = 0 To 63
            Dim cd = Nuctions.ParseNumberToCodon(i, 3)
            Dim vCode As New Code
        Next
    End Sub
End Class
Public Class Code
    Inherits Grid
    Public Code As New Label With {.Padding = New Thickness(2)}
    Public AminoAcid As New AminoAcidDropDown
    Public Ratio As New NumberBox
    Public CanStart As New ToggleButton
    Public StartRatio As New NumberBox
    Public Sub New()
        CanStart.Background = Nothing
        CanStart.Content = "Init"
        ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(24)})
        ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(14)})
        ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(20)})
        ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(14)})
        ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(20)})
        Children.Add(Code)
        Grid.SetColumn(Code, 0)
        Children.Add(AminoAcid)
        Grid.SetColumn(AminoAcid, 1)
        Children.Add(Ratio)
        Grid.SetColumn(Ratio, 2)
        Children.Add(CanStart)
        Grid.SetColumn(CanStart, 3)
        Children.Add(StartRatio)
        Grid.SetColumn(StartRatio, 4)
    End Sub
    'Code->Codon As String with Event Default: ""
    Public Property Codon As String
        Get
            Return GetValue(CodonProperty)
        End Get
        Set(ByVal value As String)
            SetValue(CodonProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CodonProperty As DependencyProperty = _
                           DependencyProperty.Register("Codon", _
                           GetType(String), GetType(Code), _
                           New PropertyMetadata("", New PropertyChangedCallback(AddressOf SharedCodonChanged)))
    Private Shared Sub SharedCodonChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        DirectCast(d, Code).CodonChanged(d, e)
    End Sub
    Private Sub CodonChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Code.Content = e.NewValue
    End Sub

End Class