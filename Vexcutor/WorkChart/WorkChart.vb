Public Class WorkChart

    Public ItemCol As New Collection
    Public FeatureCol As New List(Of Nuctions.Feature)
    Public EnzymeCol As New List(Of Nuctions.RestrictionEnzyme)

    Public Sub AddItem(ByVal item As ChartItem)
        lv_Chart.Add(item)
        ItemCol.Add(item, item.Name)
        'item.MolecularInfo.ItemCol = ItemCol
        'item.MolecularInfo.FeatureCol = FeatureCol
        frmRecord.AddIndex(item.Name)
    End Sub

    Private Sub LoadVectorLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadVectorLToolStripMenuItem.Click
        If Not ofdGeneFile.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub
        Dim vec As Nuctions.GeneFile = Nuctions.GeneFile.LoadFromGeneBankFile(ofdGeneFile.FileName)
        Dim ci As New ChartItem()
        Dim gn As Nuctions.GeneAnnotation
        Dim ft As Nuctions.Feature

        For Each gn In vec.Features.Values
            'Add the annotation to the collection so that we can store the features
            'The features are useful in the ligation and screen
            ft = New Nuctions.Feature(gn.Label + " <" + vec.Name + "> (" + FeatureCol.Count.ToString + ")", gn.GetSuqence, gn.Label + " <" + vec.Name + ">", gn.Type, gn.Note)
            FeatureCol.Add(ft)
        Next
        ci.MolecularInfo.DNAs.Add(vec)
        If ItemCol.Contains(vec.Name) Then MsgBox("There is already the " + vec.Name + " item!", MsgBoxStyle.OkOnly, "Loading Failed") : Exit Sub
        ci.Name = vec.Name
        ci.Text = vec.Name
        ci.MolecularInfo.MolecularOperation = Nuctions.MolecularOperationEnum.Vector
        ci.MolecularInfo.File_Filename = ofdGeneFile.FileName
        AddItem(ci)
    End Sub

    Private Sub lv_Chart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv_Chart.DoubleClick
        If lv_Chart.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = lv_Chart.SelectedItems(0)
            If Form.ModifierKeys = Keys.Alt Then
                'view the property
                ShowProperties(Me.lv_Chart.SelectedItems(0))
            Else
                If ci.MolecularInfo.DNAs.Count = 1 Then
                    Dim gf As Nuctions.GeneFile = ci.MolecularInfo.DNAs(1)
                    gf.WriteToFile("Temp.gb")
                    Nuctions.ShowGBFile("Temp.gb")
                Else
                    'view the property
                    ShowProperties(Me.lv_Chart.SelectedItems(0))
                End If
            End If
        End If
    End Sub
    Public Function ShowProperties(ByVal vChartItem As ChartItem) As Windows.Forms.DialogResult
        Dim fp As New frmProperty
        'Return fp.ShowDialog(vChartItem, Me)
    End Function

    Private Sub FunctionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnzymeDigestionEToolStripMenuItem.Click, GelSeparationGToolStripMenuItem.Click, LigationLToolStripMenuItem.Click, ModificationMToolStripMenuItem.Click, PCRRToolStripMenuItem.Click, TransformationScreenTToolStripMenuItem.Click
        If Me.lv_Chart.SelectedItems.Count = 0 Then Exit Sub
        Dim ci As New ChartItem
        Dim ui As ChartItem
        For Each ui In Me.lv_Chart.SelectedItems
            ci.MolecularInfo.Source.Add(ui.MolecularInfo)
        Next
        ci.MolecularInfo.MolecularOperation = sender.tag
        'ci.MolecularInfo.ItemCol = Me.ItemCol
        'ci.MolecularInfo.FeatureCol = Me.FeatureCol
        ci.Text = "[Creating]"
        ci.MolecularInfo.Creating = True
        Me.lv_Chart.Items.Add(ci)
        Me.lv_Chart.SelectedItems.Clear()
        ci.Selected = True
        DrawRelations()
        If ShowProperties(ci) = Windows.Forms.DialogResult.OK Then
            ci.MolecularInfo.Creating = False
            Me.ItemCol.Add(ci, ci.Name)
            Me.frmRecord.AddIndex(ci.Name)
        Else
            lv_Chart.Items.Remove(ci)
        End If
    End Sub

    Private Sub PropertyPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertyPToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count = 1 Then
            ShowProperties(Me.lv_Chart.SelectedItems(0))
        End If
    End Sub

    Private Sub ViewVToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewVToolStripMenuItem.Click
        If lv_Chart.SelectedItems.Count = 1 Then
            Dim ci As ChartItem = lv_Chart.SelectedItems(0)
            Dim DNA As Nuctions.GeneFile
            Dim i As Integer = 0
            For Each DNA In ci.MolecularInfo.DNAs
                DNA.WriteToFile("Temp" + i.ToString + ".gb")
                Nuctions.ShowGBFile("Temp" + i.ToString + ".gb")
                i += 1
            Next
        End If
    End Sub

    Public Sub SaveTo(ByVal filename As String)

        Dim XOS As New XmlObjectSerializer
        Dim WS As New WorkSpace

        WS.ChartItems.AddRange(lv_Chart.Items)

        WS.Features.AddRange(FeatureCol)

        WS.Enzymes.AddRange(EnzymeCol)

        XOS.Add(WS, "WorkChart")

        XOS.SaveGZip(filename)

        Dim fi As New IO.FileInfo(filename)

        Me.Text = fi.Name

        'save all the features
        'save all the chartitems
        'save all the enzymes
        'Dim nsURL As String = "Stone's Vecute Projects"
        'Dim xfile As New System.Xml.XmlDocument
        'Dim rootNode As System.Xml.XmlNode = xfile.CreateNode(Xml.XmlNodeType.Element, "Vecute", nsURL)
        'Dim featNode As System.Xml.XmlElement = xfile.CreateNode(Xml.XmlNodeType.Element, "Features", nsURL)
        'Dim chartNode As System.Xml.XmlElement = xfile.CreateNode(Xml.XmlNodeType.Element, "ChartItems", nsURL)
        'xfile.AppendChild(rootNode)
        'rootNode.AppendChild(featNode)
        'rootNode.AppendChild(chartNode)

        'Dim sourceNode As System.Xml.XmlElement
        'Dim dnaNode As System.Xml.XmlElement
        'Dim gnNode As System.Xml.XmlElement
        'Dim scCol As System.Xml.XmlElement
        'Dim dnaCol As System.Xml.XmlElement
        'Dim enzCol As System.Xml.XmlElement
        'Dim enzNode As System.Xml.XmlElement

        'Dim DNA As Nuctions.GeneFile
        'Dim GN As Nuctions.GeneAnnotation
        'Dim FT As Nuctions.Feature

        'Dim eml As System.Xml.XmlElement
        'Dim ci As ChartItem
        'Dim cis As ChartItem
        'Dim ez As Nuctions.RestrictionEnzyme

        'For Each ci In Me.lv_Chart.Items
        '    eml = xfile.CreateNode(Xml.XmlNodeType.Element, "ChartItem", nsURL)
        '    eml.SetAttribute("Index", ci.Index)
        '    eml.SetAttribute("Name", ci.Name)
        '    eml.SetAttribute("File_Filename", ci.File_Filename)
        '    eml.SetAttribute("Gel_Maximun", ci.Gel_Maximun)
        '    eml.SetAttribute("Gel_Minimum", ci.Gel_Minimum)
        '    eml.SetAttribute("Ligation_TriFragment", ci.Ligation_TriFragment)
        '    eml.SetAttribute("Modify_Method", ci.Modify_Method)
        '    eml.SetAttribute("PCR_ForwardPrimer", ci.PCR_ForwardPrimer)
        '    eml.SetAttribute("PCR_ReversePrimer", ci.PCR_ReversePrimer)
        '    eml.SetAttribute("OperationDescription", ci.OperationDescription)
        '    eml.SetAttribute("MolecularOperation", ci.MolecularOperation)
        '    eml.SetAttribute("Position_X", ci.SetPosition.X)
        '    eml.SetAttribute("Position_Y", ci.SetPosition.Y)
        '    'added parts in 2.3.0
        '    eml.SetAttribute("Screen_FPrimer", ci.Screen_FPrimer)
        '    eml.SetAttribute("Screen_RPrimer", ci.Screen_RPrimer)
        '    eml.SetAttribute("Screen_Mode", ci.Screen_Mode)
        '    eml.SetAttribute("Screen_PCRMax", ci.Screen_PCRMax)
        '    eml.SetAttribute("Screen_PCRMin", ci.Screen_PCRMin)

        '    dnaCol = xfile.CreateNode(Xml.XmlNodeType.Element, "DNAs", nsURL)

        '    For Each DNA In ci.DNAs
        '        dnaNode = xfile.CreateNode(Xml.XmlNodeType.Element, "DNA", nsURL)
        '        dnaNode.SetAttribute("End_F", DNA.End_F)
        '        dnaNode.SetAttribute("End_R", DNA.End_R)
        '        dnaNode.SetAttribute("Iscircular", DNA.Iscircular)
        '        dnaNode.SetAttribute("ModificationDate", DNA.ModificationDate)
        '        dnaNode.SetAttribute("Name", DNA.Name)
        '        dnaNode.SetAttribute("Phos_F", DNA.Phos_F)
        '        dnaNode.SetAttribute("Phos_R", DNA.Phos_R)
        '        dnaNode.SetAttribute("Sequence", DNA.Sequence)
        '        For Each GN In DNA.Features.Values
        '            gnNode = xfile.CreateNode(Xml.XmlNodeType.Element, "Annotation", nsURL)
        '            gnNode.SetAttribute("Index", GN.Index)
        '            gnNode.SetAttribute("Label", GN.Label)
        '            gnNode.SetAttribute("Note", GN.Note)
        '            gnNode.SetAttribute("Type", GN.Type)
        '            gnNode.SetAttribute("StartPosition", GN.StartPosition)
        '            gnNode.SetAttribute("EndPosition", GN.EndPosition)
        '            gnNode.SetAttribute("Complement", GN.Complement)
        '            dnaNode.AppendChild(gnNode)
        '        Next
        '        dnaCol.AppendChild(dnaNode)
        '    Next
        '    eml.AppendChild(dnaCol)

        '    scCol = xfile.CreateNode(Xml.XmlNodeType.Element, "Scourse", nsURL)
        '    For Each cis In ci.Source
        '        sourceNode = xfile.CreateNode(Xml.XmlNodeType.Element, "SourceItem", nsURL)
        '        sourceNode.SetAttribute("Index", cis.Index)
        '        scCol.AppendChild(sourceNode)
        '    Next
        '    eml.AppendChild(scCol)

        '    enzCol = xfile.CreateNode(Xml.XmlNodeType.Element, "EnzymeSelection", nsURL)
        '    For Each ez In ci.Enzyme_Enzymes
        '        enzNode = xfile.CreateNode(Xml.XmlNodeType.Element, "Enzyme", nsURL)
        '        enzNode.SetAttribute("ACut", ez.ACut)
        '        enzNode.SetAttribute("Name", ez.Name)
        '        enzNode.SetAttribute("SCut", ez.SCut)
        '        enzNode.SetAttribute("Sequence", ez.Sequence)
        '        enzCol.AppendChild(enzNode)
        '    Next
        '    eml.AppendChild(enzCol)

        '    'save selected features.
        '    enzCol = xfile.CreateNode(Xml.XmlNodeType.Element, "FeatureSelection", nsURL)
        '    For Each FT In ci.Screen_Features
        '        enzNode = xfile.CreateNode(Xml.XmlNodeType.Element, "Feature", nsURL)
        '        enzNode.SetAttribute("Name", FT.Name)
        '        enzCol.AppendChild(enzNode)
        '    Next
        '    eml.AppendChild(enzCol)

        '    chartNode.AppendChild(eml)
        'Next

        'For Each FT In Me.FeatureCol
        '    eml = xfile.CreateNode(Xml.XmlNodeType.Element, "Feature", nsURL)
        '    eml.SetAttribute("Label", FT.Label)
        '    eml.SetAttribute("Name", FT.Name)
        '    eml.SetAttribute("Note", FT.Note)
        '    eml.SetAttribute("Sequence", FT.Sequence)
        '    eml.SetAttribute("Type", FT.Type)
        '    eml.SetAttribute("Useful", FT.Useful)
        '    featNode.AppendChild(eml)
        'Next
        'Dim recNode As System.Xml.XmlElement = xfile.CreateNode(Xml.XmlNodeType.Element, "Records", nsURL)
        'Dim i As Integer
        'Dim rec As Nuctions.ExperimentRecord

        'For i = 0 To frmRecord.Count - 1
        '    rec = frmRecord(i)
        '    eml = xfile.CreateNode(Xml.XmlNodeType.Element, "Record", nsURL)
        '    eml.SetAttribute("ID", rec.ID.ToString)
        '    eml.SetAttribute("ExpDate", rec.ExpDate.ToString)
        '    eml.SetAttribute("InheritID", rec.InheritID)
        '    eml.SetAttribute("ExpIndex", rec.ExpIndex)
        '    eml.SetAttribute("Success", rec.Success.ToString)
        '    eml.SetAttribute("Record", rec.Record)
        '    eml.SetAttribute("NextIndex", rec.NextIndex)
        '    eml.SetAttribute("Visible", rec.Visible.ToString)
        '    recNode.AppendChild(eml)
        'Next

        'rootNode.AppendChild(recNode)

        'xfile.Save(filename)
        'nFilename = filename
        'Dim vFile As New IO.FileInfo(nFilename)
        'Me.Text = vFile.Name

    End Sub
    Private nFilename As String = ""

    Public Shared Function LoadFrom(ByVal filename As String) As WorkChart
        'do not load a file if there are already files.
        Dim wc As New WorkChart
        Dim XOD As New XmlObjectDeserializer

        Dim DvD As DeviceDictionary = XOD.LoadXml(XmlObjectDeserializer.GetXmlRootFromGZipFile(filename))

        XOD.Deserialize(DvD)

        Dim WS As WorkSpace

        WS = XOD.GetObjectByKey("WorkChart")
        wc.nFilename = filename
        Dim vFile As New IO.FileInfo(wc.nFilename)
        wc.Text = vFile.Name

        'For Each ci As ChartItem In WS.ChartItems
        '    wc.lv_Chart.Items.Add(ci)
        'Next

        wc.FeatureCol = WS.Features

        For Each Str As String In WS.Enzymes
            wc.EnzymeCol.Add(frmMain.EnzymeCol.RE(Str))
        Next


        ''Dim wc As New WorkChart
        'wc.nFilename = filename
        'Dim vFile As New IO.FileInfo(wc.nFilename)
        'wc.Text = vFile.Name

        'Dim xfile As New System.Xml.XmlDocument
        'xfile.Load(filename)

        'Dim rootNode As System.Xml.XmlNode = xfile.ChildNodes(0)
        'Dim featNode As System.Xml.XmlElement = rootNode.Item("Features")

        'Dim chartNode As System.Xml.XmlElement = rootNode.Item("ChartItems")

        'Dim sourceNode As System.Xml.XmlElement
        'Dim dnaNode As System.Xml.XmlElement
        'Dim gnNode As System.Xml.XmlElement
        'Dim scCol As System.Xml.XmlElement
        'Dim dnaCol As System.Xml.XmlElement
        'Dim enzCol As System.Xml.XmlElement
        'Dim enzNode As System.Xml.XmlElement

        'Dim DNA As Nuctions.GeneFile
        'Dim GN As Nuctions.GeneAnnotation
        'Dim FT As Nuctions.Feature

        'Dim eml As System.Xml.XmlElement
        'Dim ci As ChartItem
        'Dim cis As ChartItem
        'Dim ez As Nuctions.RestrictionEnzyme

        'Dim ChartDic As New Dictionary(Of String, ChartItem)

        'For Each eml In chartNode.ChildNodes

        '    'eml = xfile.CreateNode(Xml.XmlNodeType.Element, "ChartItem", nsURL)
        '    ci = New ChartItem


        '    ChartDic.Add(eml.GetAttribute("Index"), ci)
        '    'eml.SetAttribute("Index", ci.Index)
        '    'eml = xfile.CreateNode(Xml.XmlNodeType.Element, ci.Name, nsURL)

        '    'eml.SetAttribute("Name", ci.Name)
        '    ci.Name = eml.GetAttribute("Name")

        '    'eml.SetAttribute("File_Filename", ci.File_Filename)
        '    ci.File_Filename = eml.GetAttribute("File_Filename")

        '    'eml.SetAttribute("Gel_Maximun", ci.Gel_Maximun)
        '    ci.Gel_Maximun = CInt(eml.GetAttribute("Gel_Maximun"))

        '    'eml.SetAttribute("Gel_Minimum", ci.Gel_Minimum)
        '    ci.Gel_Minimum = CInt(eml.GetAttribute("Gel_Minimum"))

        '    'eml.SetAttribute("Ligation_TriFragment", ci.Ligation_TriFragment)
        '    ci.Ligation_TriFragment = CBool(eml.GetAttribute("Ligation_TriFragment"))

        '    'eml.SetAttribute("Modify_Method", ci.Modify_Method)
        '    ci.Modify_Method = CInt(eml.GetAttribute("Modify_Method"))

        '    'eml.SetAttribute("PCR_ForwardPrimer", ci.PCR_ForwardPrimer)
        '    ci.PCR_ForwardPrimer = eml.GetAttribute("PCR_ForwardPrimer")

        '    'eml.SetAttribute("PCR_ReversePrimer", ci.PCR_ReversePrimer)
        '    ci.PCR_ReversePrimer = eml.GetAttribute("PCR_ReversePrimer")

        '    'eml.SetAttribute("OperationDescription", ci.OperationDescription)
        '    ci.OperationDescription = eml.GetAttribute("OperationDescription")

        '    'eml.SetAttribute("MolecularOperation", ci.MolecularOperation)
        '    ci.MolecularOperation = CInt(eml.GetAttribute("MolecularOperation"))

        '    'eml.SetAttribute("Position_X", ci.Position.X)
        '    'eml.SetAttribute("Position_Y", ci.Position.Y)

        '    ci.SetPosition = New Point(CInt(eml.GetAttribute("Position_X")), CInt(eml.GetAttribute("Position_Y")))

        '    'added parts in 2.3.0

        '    'eml.SetAttribute("Screen_FPrimer", ci.Screen_FPrimer)
        '    If eml.HasAttribute("Screen_FPrimer") Then ci.Screen_FPrimer = eml.GetAttribute("Screen_FPrimer")
        '    'eml.SetAttribute("Screen_RPrimer", ci.Screen_RPrimer)
        '    If eml.HasAttribute("Screen_RPrimer") Then ci.Screen_RPrimer = eml.GetAttribute("Screen_RPrimer")
        '    'eml.SetAttribute("Screen_Mode", ci.Screen_Mode)
        '    If eml.HasAttribute("Screen_Mode") Then ci.Screen_Mode = CInt(eml.GetAttribute("Screen_Mode"))
        '    'eml.SetAttribute("Screen_PCRMax", ci.Screen_PCRMax)
        '    If eml.HasAttribute("Screen_PCRMax") Then ci.Screen_PCRMax = CInt(eml.GetAttribute("Screen_PCRMax"))
        '    'eml.SetAttribute("Screen_PCRMin", ci.Screen_PCRMin)
        '    If eml.HasAttribute("Screen_PCRMin") Then ci.Screen_PCRMin = CInt(eml.GetAttribute("Screen_PCRMin"))

        '    dnaCol = eml.Item("DNAs")

        '    For Each dnaNode In dnaCol.ChildNodes
        '        DNA = New Nuctions.GeneFile

        '        'dnaNode = xfile.CreateNode(Xml.XmlNodeType.Element, "DNA", nsURL)
        '        'dnaNode.SetAttribute("End_F", DNA.End_F)
        '        DNA.End_F = dnaNode.GetAttribute("End_F")

        '        'dnaNode.SetAttribute("End_R", DNA.End_R)
        '        DNA.End_R = dnaNode.GetAttribute("End_R")

        '        'dnaNode.SetAttribute("Iscircular", DNA.Iscircular)
        '        DNA.Iscircular = CBool(dnaNode.GetAttribute("Iscircular"))

        '        'dnaNode.SetAttribute("ModificationDate", DNA.ModificationDate)
        '        DNA.ModificationDate = CDate(dnaNode.GetAttribute("ModificationDate"))

        '        'dnaNode.SetAttribute("Name", DNA.Name)
        '        DNA.Name = dnaNode.GetAttribute("Name")

        '        'dnaNode.SetAttribute("Phos_F", DNA.Phos_F)
        '        DNA.Phos_F = CBool(dnaNode.GetAttribute("Phos_F"))

        '        'dnaNode.SetAttribute("Phos_R", DNA.Phos_R)
        '        DNA.Phos_R = CBool(dnaNode.GetAttribute("Phos_R"))

        '        'dnaNode.SetAttribute("Sequence", DNA.Sequence)
        '        DNA.Sequence = dnaNode.GetAttribute("Sequence")

        '        For Each gnNode In dnaNode.ChildNodes
        '            'gnNode = xfile.CreateNode(Xml.XmlNodeType.Element, "Annotation", nsURL)
        '            GN = New Nuctions.GeneAnnotation

        '            'gnNode.SetAttribute("Index", GN.Index)
        '            GN.Index = CInt(gnNode.GetAttribute("Index"))

        '            'gnNode.SetAttribute("Label", GN.Label)
        '            GN.Label = gnNode.GetAttribute("Label")

        '            'gnNode.SetAttribute("Note", GN.Note)
        '            GN.Note = gnNode.GetAttribute("Note")

        '            'gnNode.SetAttribute("Type", GN.Type)
        '            GN.Type = gnNode.GetAttribute("Type")

        '            'gnNode.SetAttribute("StartPosition", GN.StartPosition)
        '            GN.StartPosition = CInt(gnNode.GetAttribute("StartPosition"))

        '            'gnNode.SetAttribute("EndPosition", GN.EndPosition)
        '            GN.EndPosition = CInt(gnNode.GetAttribute("EndPosition"))

        '            'gnNode.SetAttribute("Complement", GN.Complement)
        '            GN.Complement = CBool(gnNode.GetAttribute("Complement"))

        '            'dnaNode.AppendChild(gnNode)
        '            DNA.Features.Add(GN.Index, GN)
        '        Next
        '        'dnaCol.AppendChild(dnaNode)
        '        ci.DNAs.Add(DNA)
        '    Next
        '    'eml.AppendChild(dnaCol)

        '    'scCol = xfile.CreateNode(Xml.XmlNodeType.Element, "Scourse", nsURL)
        '    scCol = eml.Item("Scourse")
        '    For Each sourceNode In scCol.ChildNodes
        '        'we will process the "index" as last
        '        ci.Source.Add(sourceNode.GetAttribute("Index"))
        '    Next

        '    enzCol = eml.Item("EnzymeSelection")
        '    For Each enzNode In enzCol.ChildNodes
        '        ez = New Nuctions.RestrictionEnzyme( _
        '        enzNode.GetAttribute("Name"), enzNode.GetAttribute("Sequence"), _
        '        CInt(enzNode.GetAttribute("SCut")), CInt(enzNode.GetAttribute("ACut")))
        '        ci.Enzyme_Enzymes.Add(ez, ez.Name)
        '    Next
        '    'chartNode.AppendChild(eml)

        '    'get selected features.
        '    enzCol = eml.Item( "FeatureSelection")
        '    For Each enzNode In enzCol.ChildNodes
        '        ci.Screen_Features.Add(enzNode.GetAttribute("Name"))
        '    Next
        '    eml.AppendChild(enzCol)

        '    ci.Text = ci.Name
        '    wc.ItemCol.Add(ci, ci.Name)
        '    wc.frmRecord.AddIndex(ci.Name)
        '    ci.ItemCol = wc.ItemCol
        '    ci.FeatureCol = wc.FeatureCol

        '    wc.lv_Chart.Items.Add(ci)


        'Next

        'Dim IndexCol As Collection
        'Dim idx As String
        'For Each cis In wc.lv_Chart.Items
        '    IndexCol = cis.Source
        '    cis.Source = New Collection
        '    For Each idx In IndexCol
        '        cis.Source.Add(ChartDic(idx), ChartDic(idx).Name)
        '    Next
        'Next

        'For Each eml In featNode.ChildNodes
        '    'eml = xfile.CreateNode(Xml.XmlNodeType.Element, "Feature", nsURL)
        '    FT = New Nuctions.Feature(eml.GetAttribute("Name"), _
        '    eml.GetAttribute("Sequence"), eml.GetAttribute("Label"), _
        '    eml.GetAttribute("Type"), eml.GetAttribute("Note"), eml.GetAttribute("Useful"))
        '    wc.FeatureCol.Add(FT, FT.Name)
        'Next

        'Dim selFeat As Collection
        'For Each ci In wc.lv_Chart.Items
        '    selFeat = New Collection
        '    For Each s As String In ci.Screen_Features
        '        selFeat.Add(wc.FeatureCol(s), s)
        '    Next
        '    ci.Screen_Features = selFeat
        'Next

        'Dim recNode As System.Xml.XmlElement = rootNode.Item("Records")

        'Dim rec As Nuctions.ExperimentRecord

        'For Each eml In recNode.ChildNodes
        '    rec.ID = CInt(eml.GetAttribute("ID"))
        '    rec.ExpDate = CDate(eml.GetAttribute("ExpDate"))
        '    rec.InheritID = eml.GetAttribute("InheritID")
        '    rec.ExpIndex = eml.GetAttribute("ExpIndex")
        '    rec.Success = CBool(eml.GetAttribute("Success"))
        '    rec.Record = eml.GetAttribute("Record")
        '    rec.NextIndex = eml.GetAttribute("NextIndex")
        '    rec.Visible = CBool(eml.GetAttribute("Visible"))
        '    wc.frmRecord.AddRecord(rec.ID, rec.ExpDate, rec.InheritID, rec.ExpIndex, rec.Success, rec.Record, rec.NextIndex, rec.Visible)
        'Next
        Return wc
    End Function

    Private Sub WorkChart_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case MsgBox("Do you want save before closing the current file?", MsgBoxStyle.YesNoCancel, "File Closing")
            Case MsgBoxResult.Yes
                If nFilename.Length = 0 Then
                    If sfdProject.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Me.SaveTo(sfdProject.FileName)
                    End If
                Else
                    SaveTo(nFilename)
                End If
            Case MsgBoxResult.No
                'the file will be lost
            Case MsgBoxResult.Cancel
                e.Cancel = True
        End Select
    End Sub

    Private Sub AddFeatureFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFeatureFToolStripMenuItem.Click
        Dim ff As New frmFeatureManage
        'ff.ShowDialog(FeatureCol, Me)
    End Sub

    Private Sub DeleteItemDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteItemDToolStripMenuItem.Click
        If MsgBox("Do You Really Want to Delete All the Selected Items?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim ci As ChartItem, ui As ChartItem
            Dim delCol As New List(Of ChartItem)
            For Each ci In Me.lv_Chart.SelectedItems
                delCol.Add(ci)
            Next
            For Each ci In delCol
                ItemCol.Remove(ci.Name)
                lv_Chart.Items.Remove(ci)
                frmExpRecord.RemoveIndex(ci.Name)
            Next
            For Each ui In delCol
                For Each ci In Me.lv_Chart.Items
                    If ci.MolecularInfo.Source.Contains(ui.MolecularInfo) Then ci.MolecularInfo.Source.Remove(ui.MolecularInfo)
                Next
            Next
        End If
    End Sub

    Dim dragging As Boolean = False


    Private Sub lv_Chart_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv_Chart.MouseDown
        dragging = True
        cX = e.X
        cY = e.Y
    End Sub

    Dim dX As Integer, dY As Integer 'start position
    Dim cX As Integer, cY As Integer
    Private Sub lv_Chart_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv_Chart.MouseMove
        If Not dragging Then Exit Sub
        If Me.lv_Chart.SelectedItems.Count > 0 Then
            Dim ci As ChartItem
            dX = e.X - cX
            dY = e.Y - cY
            cX = e.X
            cY = e.Y
            For Each ci In Me.lv_Chart.SelectedItems
                ci.Position = New Point(ci.Position.X + dX, ci.Position.Y + dY)
            Next
        End If
    End Sub

    Private Sub lv_Chart_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv_Chart.MouseUp
        If dragging Then
            dragging = False
            SavePositions()
        End If
    End Sub

    Private Sub SetPositionPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetPositionPToolStripMenuItem.Click

        For Each ci As ChartItem In lv_Chart.Items
            ci.Position = ci.MolecularInfo.SetPosition
            'frmMain.Text += IIf(ci.Position.Equals(cj.SetPosition), "1", "0")
        Next


        DrawRelations()
    End Sub

    'Public OffsetO As Point = New Point(0, 0)

    '每次移动之后 记录所有项的位置 以及屏幕的offset
    Public Sub SavePositions()
        For Each ci As ChartItem In lv_Chart.Items
            ci.MolecularInfo.SetPosition = ci.Bounds.Location
            'OffsetO.X = Math.Min(OffsetO.X, ci.Position.X)
            'OffsetO.Y = Math.Min(OffsetO.Y, ci.Position.Y)
        Next
        DrawRelations()
    End Sub

    'Private Sub SetPositionRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    For Each ci As ChartItem In lv_Chart.Items
    '        ci.SetPosition = ci.Position
    '    Next
    '    'Dim p As PointF = GetAngle(New Point(0, 0), New Point(45, 45))
    'End Sub
    Private Sub DrawRelations()
        Dim sP As Point, eP As Point
        Dim g As Graphics = lv_Chart.CreateGraphics
        g.Clear(Color.White)
        lv_Chart.Refresh()
        Dim GrdBrush As System.Drawing.Drawing2D.LinearGradientBrush
        Dim pnts As PointF()

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        'g.TranslateTransform(-OffsetO.X, -OffsetO.Y)
        Dim ori As ChartItem

        For Each ci As ChartItem In lv_Chart.Items
            For Each ui As DNAInfo In ci.MolecularInfo.Source
                'if there is a source, draw it
                ori = ui.GetParetntChartItem
                sP = ori.Bounds.Location
                eP = ci.Bounds.Location

                sP.Offset(32, 32)
                eP.Offset(32, 32)

                Dim p As PointF = GetAngle(sP, eP)
                If p.X < 80 Then Continue For
                sP = New Point(sP.X + 30 * Math.Cos(p.Y), sP.Y + 30 * Math.Sin(p.Y))
                eP = New Point(eP.X - 30 * Math.Cos(p.Y), eP.Y - 30 * Math.Sin(p.Y))

                If Not sP.Equals(eP) Then
                    GrdBrush = New System.Drawing.Drawing2D.LinearGradientBrush(sP, eP, Color.Yellow, Color.Red)
                    pnts = GetArrow(sP, eP, 45, 45)
                    g.FillPolygon(GrdBrush, pnts)
                    g.DrawPolygon(Pens.Black, pnts)
                End If
            Next
        Next
    End Sub

    Private Function GetArrow(ByVal V0 As Vector2, ByVal VE As Vector2, ByVal prefix As Single, ByVal suffix As Single) As PointF()
        Dim VX As Vector2 = VE - V0
        Dim VY As Vector2 = VX.GetBase.Turn(Math.PI / 2)
        Dim L As Single = VX.GetLength
        VX = VX.GetBase
        Return New PointF() {V0 + VX * prefix + VY * 6, VE - VX * (suffix + 12) + VY * 6, _
                             VE - VX * (suffix + 12) + VY * 12, VE - VX * suffix, _
                             VE - VX * (suffix + 12) - VY * 12, VE - VX * (suffix + 12) - VY * 6, V0 + VX * prefix - VY * 6}
        'Dim t As PointF = New PointF(Param.X - 90, Param.Y)
        'Return New PointF() {New PointF(0, 6), New PointF(t.X - 12, 6), New Point(t.X - 15, 15), New Point(t.X, 0), New Point(t.X - 15, -15), New Point(t.X - 12, -6), New Point(0, -6)}
    End Function

    Private Sub DrawArrow(ByVal startPoint As Point, ByVal endPoint As Point, ByVal Param As PointF)
        If startPoint.Equals(endPoint) Then Exit Sub
        Dim g As Graphics = lv_Chart.CreateGraphics
        Dim p0 As Point
        For Each ci As ListViewItem In lv_Chart.Items
            p0.X = Math.Min(p0.X, ci.Position.X)
            p0.Y = Math.Min(p0.Y, ci.Position.Y)
        Next
        g.TranslateTransform(-p0.X, -p0.Y)
        Dim t As PointF = New PointF(Param.X - 90, Param.Y)
        Dim P As PointF() = {New PointF(0, 6), New PointF(t.X - 12, 6), New Point(t.X - 15, 15), New Point(t.X, 0), New Point(t.X - 15, -15), New Point(t.X - 12, -6), New Point(0, -6)}
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.TranslateTransform(startPoint.X, startPoint.Y)
        g.RotateTransform(t.Y / Math.PI * 180)
        g.FillPolygon(Brushes.DarkBlue, P)
    End Sub
    Private Function GetAngle(ByVal startPoint As Point, ByVal endPoint As Point) As PointF
        Return New PointF(Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y)), _
        IIf((endPoint.X - startPoint.X) = 0, IIf((endPoint.Y - startPoint.Y) > 0, Math.PI / 2, -Math.PI / 2), _
        Math.Atan((endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X)) + Math.PI * (IIf(endPoint.X - startPoint.X > 0, 0, 1))))
    End Function
    Public Function Save() As Boolean
        If nFilename.Length = 0 Then Exit Function
        Dim fi As New IO.FileInfo(nFilename)
        If fi.Exists Then
            SaveTo(fi.FullName)
            Return True
        Else
            Return False
        End If
    End Function

    Private frmRecord As New frmExpRecord

    Private Sub ExperimentLogLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExperimentLogLToolStripMenuItem.Click
        ShowRecord()
    End Sub

    Private Sub WorkChart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        frmRecord.Owner = Me
        If lv_Chart.Items.Count > 0 Then
            For Each ci As ChartItem In lv_Chart.Items
                ci.Position = ci.MolecularInfo.SetPosition
            Next
        End If
    End Sub
    Public Sub ShowRecord()
        If ExperimentLogLToolStripMenuItem.Checked Then
            frmRecord.Hide()
            ExperimentLogLToolStripMenuItem.Checked = False
        Else
            frmRecord.Location = New Point(0, My.Computer.Screen.Bounds.Size.Height - frmRecord.Height)
            frmRecord.Show()
            ExperimentLogLToolStripMenuItem.Checked = True
        End If
    End Sub
End Class