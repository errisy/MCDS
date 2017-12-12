Public Class PCRv1_0
    Public pcr_Forward As String, pcr_Reverse As String, pcr_Template As String
    Private pcr_CheckInfo As String
    Private pcr_SearchInfo As String
    Private pcr_Product As String
    Public Function Run() As String
        'check the input
        If Not CheckInput() Then Return pcr_CheckInfo
        If Not SearchProduct() Then Return pcr_SearchInfo

        Return pcr_Product
    End Function
    Private Function CheckInput() As Boolean
        Dim chkTAGC As Boolean = True
        If Not Nuctions.CheckTAGC(Me.pcr_Forward) Then chkTAGC = False : Me.pcr_CheckInfo &= "Forward Primer contains illegal characters." & ControlChars.NewLine
        If Not Nuctions.CheckTAGC(Me.pcr_Reverse) Then chkTAGC = False : Me.pcr_CheckInfo &= "Reverse Primer contains illegal characters." & ControlChars.NewLine
        If Not Nuctions.CheckTAGC(Me.pcr_Template) Then chkTAGC = False : Me.pcr_CheckInfo &= "Template sequence contains illegal characters." & ControlChars.NewLine

        If Not chkTAGC Then Return False

        Dim chkLength As Boolean = True
        If Me.pcr_Forward.Length < 10 Then chkLength = False : Me.pcr_CheckInfo &= "Forward Primer is shorter than 10 nucleatides." & ControlChars.NewLine
        If Me.pcr_Reverse.Length < 10 Then chkLength = False : Me.pcr_CheckInfo &= "Reverse Primer is shorter than 10 nucleatides." & ControlChars.NewLine

        If Not chkLength Then Return False

        Return True
    End Function
    Private Function SearchProduct() As Boolean
        'use the end 10 characters to search product
        'if there are two or more binding site, show that this primer pair surfers severe false priming.
        'if the programe fail to find a match. Show that there is no obvious binding sites for this primer.
        Dim f10 As String = Me.pcr_Forward.Substring(Me.pcr_Forward.Length - 10, 10)
        Dim r10 As String = Me.pcr_Reverse.Substring(Me.pcr_Reverse.Length - 10, 10)
        r10 = Nuctions.ReverseComplement(r10)

        Dim temp As String = Me.pcr_Template.ToUpper

        Dim regexF As New System.Text.RegularExpressions.Regex(f10)
        Dim regexR As New System.Text.RegularExpressions.Regex(r10)

        Dim mcF As System.Text.RegularExpressions.MatchCollection = regexF.Matches(temp)
        Dim mcR As System.Text.RegularExpressions.MatchCollection = regexR.Matches(temp)

        Dim chkSearch As Boolean = True

        If mcF.Count = 0 Then chkSearch = False : Me.pcr_SearchInfo &= "The forward primer does not match the template." & ControlChars.NewLine
        If mcF.Count > 1 Then chkSearch = False : Me.pcr_SearchInfo &= "The forward primer surfers severe false priming." & ControlChars.NewLine
        If mcR.Count = 0 Then chkSearch = False : Me.pcr_SearchInfo &= "The Reverse primer does not match the template." & ControlChars.NewLine
        If mcR.Count > 1 Then chkSearch = False : Me.pcr_SearchInfo &= "The Reverse primer surfers severe false priming." & ControlChars.NewLine

        If Not chkSearch Then Return False

        'check if the two primer bounds have any unions.
        Dim pcr_Left, pcr_Right As Integer
        pcr_Left = mcF.Item(0).Index + 10
        pcr_Right = mcR.Item(0).Index
        If pcr_Left > pcr_Right Then chkSearch = False : Me.pcr_SearchInfo &= "The primer pair cannot give any product although bind to the template specifically."

        If Not chkSearch Then Return False

        pcr_Product = Me.pcr_Forward.ToUpper + temp.Substring(pcr_Left, pcr_Right - pcr_Left).ToUpper + Nuctions.ReverseComplement(Me.pcr_Reverse.ToUpper)

        Return True

    End Function

End Class
