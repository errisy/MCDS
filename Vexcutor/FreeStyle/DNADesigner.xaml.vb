﻿Public Class DNADesigner
    Implements IWorkControlSibling
    Protected fsDirector As New Director
    Public RelatedChartItem As ChartItem
    Public RelatedDNAInfo As DNAInfo
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        fsDirector.Host = fsMain
        fsTools.Stage = fsMain
        fsMain.RelatedDirector = fsDirector
        fsMain.RequireWorkControl = AddressOf OnRequireWorkControl
    End Sub
    Friend Property RequireWorkControl As System.Func(Of WorkControl) Implements IWorkControlSibling.RequireWorkControl
    Private Sub DNADesigner_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        fsMain.DesignMode = True
    End Sub
    Private Function OnRequireWorkControl() As WorkControl
        If RequireWorkControl.OK Then
            Return RequireWorkControl.Invoke
        Else
            Return Nothing
        End If
    End Function
    Public Sub LoadView()
        If RelatedDNAInfo.OK Then
            fsMain.HostDNAInfo = RelatedDNAInfo
            fsMain.HostChartItem = RelatedChartItem
            If RelatedDNAInfo.SequenceDesignerData.OK Then
                fsMain.Actors = ShallowSerializer.Deserialize(RelatedDNAInfo.SequenceDesignerData)
            Else
                fsMain.Actors = New ShallowList(Of IActor)
            End If
        End If
    End Sub
    Public Sub UnloadView()
        If RelatedDNAInfo.OK Then
            RelatedDNAInfo.SequenceDesignerData = ShallowSerializer.Serialize(fsMain.Actors)
            Dim vActs As New List(Of IActor)
            vActs.AddRange(fsMain.Actors)
            For Each act As IActor In vActs
                act.Remove()
            Next
            fsMain.HostDNAInfo = Nothing
            fsMain.HostChartItem = Nothing
            RelatedDNAInfo = Nothing
            RelatedChartItem = Nothing
        End If
    End Sub
    Public Property RelatedPropertyTab As PropertyControl
        Get
            Return fsMain.RelatedPropertyTab
        End Get
        Set(value As PropertyControl)
            fsMain.RelatedPropertyTab = value
        End Set
    End Property
End Class
