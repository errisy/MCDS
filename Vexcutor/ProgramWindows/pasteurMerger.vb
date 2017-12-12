Public Class pasteurMerger

    Dim folder As System.IO.DirectoryInfo
    Private Sub Browse_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblBrowse.LinkClicked
        If fbdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
            folder = New IO.DirectoryInfo(fbdSave.SelectedPath)
        End If
    End Sub

    Private Sub lblAdd_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAdd.LinkClicked
        Dim row As DataGridViewRow = dgvTask.Rows(dgvTask.Rows.Add())
        row.Cells(0).Value = "Task " + row.Index.ToString
    End Sub

    Private Sub lblStart_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblStart.LinkClicked
        If threadcount = 0 Then
            StartTaskSeries()
            Me.lblStart.Text = "Merging"
            tmrWatcher.Enabled = True
        End If
    End Sub

    Private Sub StartTaskSeries()
        Dim thr As New System.Threading.Thread(AddressOf StartTaskByIndex)
        threadcount = dgvTask.Rows.Count
        thr.Start()
    End Sub
    Private Sub StartTaskByIndex()
        Dim t As Task
        For Each row As DataGridViewRow In dgvTask.Rows
            If IO.File.Exists(row.Cells(1).Value) And IO.File.Exists(row.Cells(2).Value) Then
                t = New Task
                t.A = row.Cells(1).Value
                t.B = row.Cells(2).Value
                t.Index = row.Index
                t.path = folder
                Merge(t)
                System.Threading.Thread.Sleep(10000)
            Else
                threadcount -= 1
            End If
        Next
    End Sub

    Private Sub StartTask()
        Dim t As Task
        Dim thr As System.Threading.Thread
        For Each row As DataGridViewRow In dgvTask.Rows
            If IO.File.Exists(row.Cells(1).Value) And IO.File.Exists(row.Cells(2).Value) Then
                t = New Task
                t.A = row.Cells(1).Value
                t.B = row.Cells(2).Value
                t.Index = row.Index
                t.path = folder
                thr = New System.Threading.Thread(AddressOf Merge)
                thr.Start(t)
                threadcount += 1
            End If
        Next
        Me.lblStart.Text = "Merging"
        tmrWatcher.Enabled = True
    End Sub
    Class Task
        Public A As String
        Public B As String
        Public Index As Integer
        Public path As System.IO.DirectoryInfo
    End Class

    Private threadcount As Integer = 0

    Private TaskQueue As New Queue(Of FinishInfo)

    Class FinishInfo
        Public Index As Integer
        Public Info As String
    End Class

    Private Sub Merge(ByVal vdata As Object)
        Dim url As String
        Dim request As Net.HttpWebRequest
        Dim response As Net.HttpWebResponse
        Dim dataStream As IO.Stream
        Dim reader As IO.StreamReader
        Dim responseFromServer As String
        Dim t As Task = vdata
        Dim gSession As New System.Net.CookieContainer

        ' Open the stream using a StreamReader for easy access.
        ' Read the content.

        Dim encoding As System.Text.ASCIIEncoding = New System.Text.ASCIIEncoding()
        Dim postData As String = "email=" + "errisy@tom.com"
        postData += ("&asequence_data=" + Nuctions.TAGCFilter(IO.File.ReadAllText(t.A)))
        postData += ("&bsequence_data=" + Nuctions.TAGCFilter(IO.File.ReadAllText(t.B)))


        url = "http://bioweb.pasteur.fr/cgi-bin/seqanal/merger.pl?"
        request = Net.WebRequest.Create(url)
        request.CookieContainer = gSession
        request.Method = "POST"

        Dim data As Byte() = encoding.GetBytes(postData)

        request.ContentLength = data.Length

        Dim gStream As System.IO.Stream = request.GetRequestStream

        gStream.Write(data, 0, data.Length)
        gStream.Close()

        'tF.AppendText(postData)

        request.Credentials = Net.CredentialCache.DefaultCredentials
        response = CType(request.GetResponse, Net.HttpWebResponse)
        ' Display the status.
        'response.StatusDescription
        ' Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream()
        ' Open the stream using a StreamReader for easy access.


        reader = New IO.StreamReader(dataStream)
        ' Read the content.
        responseFromServer = reader.ReadToEnd()

        'out.AppendText(responseFromServer)

        Dim regH As New System.Text.RegularExpressions.Regex("HREF[\s]*=[\s]*""(http\://bioweb\.pasteur\.fr/seqanal/tmp/merger/[\w]+/outseq\.out)""")

        Dim wc As New System.Net.WebClient
        Dim OK As Boolean = False

        If regH.IsMatch(responseFromServer) Then
            Dim nOutURL As String = regH.Match(responseFromServer).Groups(1).Value
            Dim seq = wc.DownloadString(nOutURL)
            Dim infoA As New IO.FileInfo(t.A)
            Dim infoB As New IO.FileInfo(t.B)
            OK = True
            System.IO.File.WriteAllText(t.path.FullName + "\Merge(" + infoA.Name + infoB.Name + ").txt", Nuctions.TAGCFilter(seq))
        End If
        Dim fi As New FinishInfo
        fi.Index = t.Index
        If OK Then
            fi.Info = t.Index.ToString + " OK"
        Else
            fi.Info = t.Index.ToString + " Failed"
        End If
        TaskQueue.Enqueue(fi)
        threadcount -= 1
    End Sub

    Private Sub dgvTask_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTask.CellContentClick
        If e.ColumnIndex > 0 Then
            If ofdFile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim c As DataGridViewButtonCell = dgvTask.Rows(e.RowIndex).Cells(e.ColumnIndex)
                c.Value = ofdFile.FileName
            End If
        End If
    End Sub

    Private Sub tmrWatcher_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWatcher.Tick
        If threadcount = 0 Then Me.lblStart.Text = "Start Merge" : tmrWatcher.Enabled = False
        While TaskQueue.Count > 0
            Dim fi As FinishInfo = TaskQueue.Dequeue
            dgvTask.Rows(fi.Index).Cells(0).Value = fi.Info
        End While
    End Sub
End Class