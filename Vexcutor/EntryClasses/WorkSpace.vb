<Serializable>
Public Class WorkSpace
    Public ChartItems As New List(Of DNAInfo)
    Public Features As New List(Of Nuctions.Feature)
    Public Enzymes As New List(Of String)
    Public Summary As String
    Public Scale As Single = 1
    Public OffsetX As Single
    Public OffsetY As Single
    Public PrintView As Boolean = False
    Public PrintPages As New List(Of PrintPage)
    Public PrimerList As New List(Of PrimerInfo)
    Public Hosts As New List(Of Nuctions.Host)
    Public Published As Boolean
    Public PublicationID As Integer
    Public Quoted As Boolean
    Public QuotationID As Integer
    Public ProjectServiceStatus As ProjectServiceStatusEnum = ProjectServiceStatusEnum.None
End Class