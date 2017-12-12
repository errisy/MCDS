Imports System.Management
Friend Class Equipment
    Shared Function GetProcessorID() As List(Of String)
        Dim IDs As New List(Of String)

        'Declare following three object variables

        Dim objMOS As ManagementObjectSearcher

        Dim objMOC As Management.ManagementObjectCollection

        Dim objMO As Management.ManagementObject = Nothing

        'Now, execute the query to get the results

        objMOS = New ManagementObjectSearcher("Select * From Win32_Processor")

        objMOC = objMOS.Get

        'Finally, get the CPU's id.

        For Each objMO In objMOC

            IDs.Add(objMO("ProcessorID"))
        Next

        'Dispose object variables.

        objMOS.Dispose()

        objMOS = Nothing

        objMO.Dispose()

        objMO = Nothing
        Return IDs
    End Function

    Shared Function GetMAC() As List(Of String)
        Dim MACs As New List(Of String)
        Dim wmi As New System.Management.ManagementObjectSearcher("select * from win32_networkadapterconfiguration")
        For Each wmiobj As System.Management.ManagementObject In wmi.Get
            If CBool(wmiobj("ipenabled")) Then
                '用MAC地址作为KEY
                MACs.Add(wmiobj("MACAddress"))
            End If
        Next
        Return MACs
    End Function

    Friend Shared Function GenerateMachineHash() As List(Of String)
        Dim SM As New SecureMessage
        Dim MachineKeys As New List(Of String)
        MachineKeys.AddRange(GetProcessorID)
        MachineKeys.AddRange(GetMAC)
        Dim F = Function() As List(Of String)
                    Dim WV As New List(Of String)
                    For Each s As String In MachineKeys
                        WV.Add(SM.DoubleHasher(s))
                    Next
                    Return WV
                End Function
        Return F()
    End Function
    Friend Shared Function EnPack(ByVal Hashs As List(Of String)) As String
        Dim Keys As New Dictionary(Of String, String)
        Dim v As Integer = 0
        Dim id As String

        For Each h As String In Hashs
            v += 1
            id = Now.ToBinary.ToString + v.ToString
            Keys.Add(id, h)
        Next
        Dim ms As New System.IO.MemoryStream
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        bf.Serialize(ms, Keys)
        Dim sm As New SecureMessage
        Return Convert.ToBase64String(sm.SSLEN(ms.GetBuffer))
    End Function
    Friend Shared Function DePack(ByVal Value As String) As List(Of String)
        Dim sm As New SecureMessage
        Dim ms As New System.IO.MemoryStream(sm.SSLDE(Convert.FromBase64String(Value)))
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim Keys As Dictionary(Of String, String) = bf.Deserialize(ms)
        Dim hashs As New List(Of String)
        For Each v As String In Keys.Values
            hashs.Add(v)
        Next
        Return hashs
    End Function
    Friend Shared Function GenerateKey(ByVal Hashs As List(Of String), ByVal RSA As RSACrypt) As Byte()
        Dim F = Function() As Byte()
                    Dim Keys As New Dictionary(Of String, String)
                    Dim v As Integer = 0
                    Dim id As String
                    For Each h As String In Hashs
                        v += 1
                        id = Now.ToBinary.ToString + v.ToString
                        Keys.Add(id, RSA.SignData(h))
                    Next
                    Dim ms As New System.IO.MemoryStream()
                    Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    bf.Serialize(ms, Keys)
                    ms.Position = 0
                    Dim by As Byte() = New Byte(ms.Length - 1) {}
                    ms.Read(by, 0, ms.Length)
                    Return by
                End Function
        Dim bytes As Byte() = F()
        Dim sm As New SecureMessage
        Return sm.SSLEN(bytes)
    End Function

    Friend Shared Function CheckKey(ByVal bytes As Byte()) As List(Of String)
        Dim sm As New SecureMessage
        Dim mem As Byte() = sm.SSLDE(bytes)
        Dim ms As New System.IO.MemoryStream(mem)
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim gusm As Dictionary(Of String, String) = bf.Deserialize(ms)
        Dim z As New List(Of String)
        For Each w As String In gusm.Values
            z.Add(w)
        Next
        Return z
    End Function
End Class
