Imports System.Management, System.Security.Cryptography

Public Class ClientRegistration
    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        KI = New Byte() {13, 129, 228, 153,
                  233, 121, 23, 52,
                  9, 1, 76, 38,
                  45, 248, 87, 3,
                  4, 52, 125, 63,
                  255, 241, 23, 1,
                  0, 29, 91, 2,
                  47, 67, 3, 172}
        VI = New Byte() {0, 221, 12, 34,
                      2, 155, 184, 92,
                      92, 12, 38, 38,
                      145, 28, 7, 236}
        Me.DataContext = Customer
        InitAutoComplete()
    End Sub
    Public Property Customer As New SynContract.Customer
    Private Sub InitAutoComplete()
        Dim s As String = <a>Afghanistan
Albania
Algeria
Andorra
Angola
Antigua and Barbuda
Argentina
Armenia
Australia - Eastern States (New South Wales, Victoria, Australian Capital Territory, Tasmania, South Australia)
Australia - Western States (Western Australia, Northern Territory)
Austria
Azerbaijan
Bahamas
Bahrain
Bangladesh
Barbados
Belarus
Belgium
Belize
Benin
Bhutan
Bosnia and Herzegovina
Botswana
Brazil - North Region (Acre, Amapá, Amazonas, Pará, Rondônia, Roraima and Tocantins)
Brazil - Northeast Region (Maranhão, Piauí, Ceará, Rio Grande do Norte, Paraíba, Pernambuco, Alagoas, Sergipe and Bahia)
Brazil - Central-West Region (Goiás, Mato Grosso and Mato Grosso do Sul; along with Distrito Federal)
Brazil - Southeast Region (Espírito Santo, Minas Gerais, Rio de Janeiro and São Paulo)
Brazil - South Region (Paraná, Santa Catarina and Rio Grande do Sul)
Brunei Darussalam
Bulgaria
Burkina Faso
Burundi
Cambodia
Cameroon
Canada - Atlantic Canada (New Brunswick, Prince Edward Island, Nova Scotia, Newfoundland and Labrador)
Canada - Quebec (Quebec)
Canada - Ontario (Ontario)
Canada - Saskatchewan And Manitoba (Manitoba, Saskatchewan)
Canada - Alberta (Alberta)
Canada - British Columbia (British Columbia)
Canada - Northern Canada (Yukon, Northwest Territories, Nunavut)
Cape Verde
Central African Republic
Chad
Chile
China - North China (Beijing, Tianjin, Hebei, Shanxi, Neimenggu)
China - East China (Shanghai, Shandong, Jiangsu, Anhui, Zhejiang, Fujian)
China - South China (Guangdong, Guangxi, Hainan)
China - Northeast China (Liaoning, Jilin, Heilongjiang)
China - South Central China (Hubei, Hunan, Henan, Jiangxi)
China - Southwest China (Sichuan, Yunnan, Guizhou, Xizang, Chongqing)
China - Northwest China (Ningxiq, Xinjiang, Qinghal, Shanxi, Gansu)
China - Hondkong
China - Macau
China - Taiwan
Colombia
Comoros
Congo
Cook Islands
Costa Rica
Croatia
Cuba
Cyprus
Czech Republic
Democratic Republic of the Congo
Denmark
Djibouti
Dominica
Dominican Republic
Ecuador
Egypt
El Salvador
Equatorial Guinea
Eritrea
Estonia
Ethiopia
Fiji
Finland
France
Gabon
Gambia
Georgia
Germany
Ghana
Greece
Grenada
Guatemala
Guinea
Guyana
Haiti
Honduras
Hungary
Iceland
India - East India (West Bengal, Orissa, Bihar and Jharkhand)
India - Northeast India (Arunachal Pradesh, Assam, Manipur, Meghalaya, Mizoram, Nagaland, Sikkim, Tripura)
India - North India (Jammu and Kashmir, Himachal Pradesh, Uttarakhand, Haryana, Punjab, Rajasthan, Uttar Pradesh, Bihar, Jharkhand, Chhattisgarh and Madhya Pradesh)
India - South India (Andhra Pradesh, Karnataka, Kerala, Tamil Nadu, Pondicherry, Lakshadweep)
India - West India (Maharashtra, Gujarat, Goa, Dadra and Nagar Haveli, Daman and Diu)
Indonesia
Iraq
Ireland
Israel
Italy
Jamaica
Japan
Jordan
Kazakhstan
Kenya
Kiribati
Kuwait
Kyrgyzstan
Latvia
Lebanon
Lesotho
Liberia
Libya
Lithuania
Luxembourg
Madagascar
Malawi
Malaysia
Maldives
Mali
Malta
Marshall Islands
Mauritania
Mauritius
Mexico
Monaco
Mongolia
Montenegro
Morocco
Mozambique
Myanmar
Namibia
Nauru
Nepal
Netherlands
New Zealand
Nicaragua
Niger
Nigeria
Niue
Norway
Oman
Pakistan
Palau
Panama
Papua New Guinea
Paraguay
Peru
Philippines
Poland
Portugal
Qatar
Republic of Korea
Republic of Moldova
Romania
Russian Federation - Central Federal District (Belgorod Oblast, Bryansk Oblast, Vladimir Oblast, Voronezh Oblast, Ivanovo Oblast, Kaluga Oblast, Kostroma Oblast, Kursk Oblast, Lipetsk Oblast, Moscow, Moscow Oblast, Oryol Oblast, Ryazan Oblast, Smolensk Oblast, Tambov Oblast, Tver Oblast, Tula Oblast, Yaroslavl Oblast)
Russian Federation - Southern Federal District (Republic of Adygea, Astrakhan Oblast, Volgograd Oblast, Republic of Kalmykia, Krasnodar Krai, Rostov Oblast)
Russian Federation - Northwestern Federal District (Arkhangelsk, Vologda, Kaliningrad, Republic of Karelia, Komi Republic, Leningrad Oblast, Murmansk Oblast, Nenets Autonomous Okrug, Novgorod Oblast, Pskov Oblast and Saint Petersburg)
Russian Federation - Far Eastern Federal District (Amur Oblast, Jewish Autonomous Oblast, Kamchatka Krai, Magadan Oblast, Primorsky Krai, Sakha Republic, Sakhalin Oblast, Khabarovsk Krai, Chukotka Autonomous Okrug)
Russian Federation - Siberian Federal District (Altai Republic, Altai Krai, Buryat Republic, Zabaykalsky Krai, Irkutsk Oblast, Kemerovo Oblast, Krasnoyarsk Krai, Novosibirsk Oblast, Omsk Oblast, Tomsk Oblast, Tuva Republic, Republic of Khakassia)
Russian Federation - Urals Federal District (Kurgan Oblast, Sverdlovsk Oblast, Tyumen Oblast, Khanty–Mansi Autonomous Okrug, Chelyabinsk Oblast, Yamalo-Nenets Autonomous Okrug)
Russian Federation - Volga Federal District (Republic of Bashkortostan, Kirov Oblast, Mari El Republic, Republic of Mordovia, Nizhny Novgorod Oblast, Orenburg Oblast, Penza Oblast, Perm Krai, Samara Oblast, Saratov Oblast, Republic of Tatarstan, Udmurt Republic, Ulyanovsk Oblast, Chuvash Republic)
Russian Federation - North Caucasian Federal District (Republic of Dagestan, Republic of Ingushetia, Kabardino-Balkar Republic, Karachay–Cherkess Republic, Republic of North Ossetia–Alania, Stavropol Krai, Chechen Republic)
Rwanda
Saint Kitts and Nevis
Saint Lucia
Saint Vincent and the Grenadines
Samoa
San Marino
Sao Tome and Principe
Saudi Arabia
Senegal
Serbia
Seychelles
Sierra Leone
Singapore
Slovakia
Slovenia
Solomon Islands
Somalia
South Africa
South Sudan
Spain
Sri Lanka
Sudan
Suriname
Swaziland
Sweden
Switzerland
Syrian Arab Republic
Tajikistan
Thailand
The former Yugoslav Republic of Macedonia
Togo
Tonga
Trinidad and Tobago
Tunisia
Turkey
Turkmenistan
Tuvalu
Uganda
Ukraine
United Arab Emirates
United Kingdom
United Republic of Tanzania
United States of America - New England (Maine, New Hampshire, Vermont, Massachusetts, Rhode Island, and Connecticut)
United States of America - Middle Atlantic (Delaware, Maryland, New Jersey, Pennsylvania, Washington D.C., New York, Virginia, and West Virginia)
United States of America - East North Central States (Illinois, Indiana, Michigan, Ohio, and Wisconsin)
United States of America - West North Central States(Iowa, Kansas, Minnesota, Missouri, Nebraska, North Dakota and South Dakota)
United States of America - Pacific States(Alaska, California, Hawaii, Oregon, Washington)
United States of America - Mountain States(Arizona, Colorado, Idaho, Montana, Nevada, New Mexico, Utah, and Wyoming)
United States of America - West South Central States(Arkansas, Louisiana, Oklahoma, and Texas)
United States of America - East South Central States(Alabama, Kentucky, Mississippi, and Tennessee)
United States of America - South Atlantic States(Delaware, Florida, Georgia, Maryland, North Carolina, South Carolina, Virginia, West Virginia, and the District of Columbia)
Uruguay
Uzbekistan
Vanuatu
Viet Nam
Yemen
Zambia
Zimbabwe</a>

        'cbRegion.ItemsSource =s.Split({vbCr, vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
        acpRegion.Values = s.Split({vbCr, vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
    End Sub
    Private Sub BillingAddressAsAbove(sender As System.Object, e As System.Windows.RoutedEventArgs)
        tbBillingAdress.Text = GenerateAddress()
    End Sub
    Private Sub ShippingAddressAsAbove(sender As System.Object, e As System.Windows.RoutedEventArgs)
        tbShippingAddress.Text = GenerateAddress()
    End Sub
    Private Function GenerateAddress() As String
        Dim stb As New System.Text.StringBuilder
        stb.AppendFormat("{0} {1}, Tel:{2}", tbTitle.Text, tbName.Text, tbPhone.Text)
        stb.AppendLine()
        If tbRoom.Text.Any Then stb.AppendFormat("Room:{0}, ", tbRoom.Text)
        If tbBuilding.Text.Any Then stb.AppendFormat("{0}, ", tbBuilding.Text)
        stb.AppendLine()
        If tbCompany.Text.Any Then stb.AppendFormat("{0}, ", tbCompany.Text)
        If tbInstitution.Text.Any Then stb.AppendFormat("{0}, ", tbInstitution.Text)
        stb.AppendLine()
        If tbStreetNumber.Text.Any Then stb.AppendFormat("{0}, ", tbStreetNumber.Text)
        If tbStreet.Text.Any Then stb.AppendFormat("{0}, ", tbStreet.Text)
        If tbDistrict.Text.Any Then stb.AppendFormat("{0}, ", tbDistrict.Text)
        If tbCityOrTown.Text.Any Then stb.AppendFormat("{0}, ", tbCityOrTown.Text)
        stb.AppendLine()
        If tbState.Text.Any Then stb.AppendFormat("{0}, ", tbState.Text)
        If tbPostCode.Text.Any Then stb.AppendFormat("{0}, ", tbPostCode.Text)
        If tbCountry.Text.Any Then stb.AppendFormat("{0}, ", tbCountry.Text)
        Return stb.ToString
    End Function
    Private Function CheckPassword() As Boolean

        Return pbPassword1.Password = pbPassword2.Password
    End Function
    Private Sub RegisterUser(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Register(Customer)
    End Sub
    Private Async Sub Register(sCust As SynContract.Customer)
        Dim validinfo As Boolean = False

        Dim regName As New System.Text.RegularExpressions.Regex("[^\w^\d]")
        If sCust.ID Is Nothing Then
            lbInfo.Content = "Error. User ID cannot be empty!"
            Exit Sub
        End If
        If regName.IsMatch(sCust.ID) Then
            sCust.ID = regName.Replace(sCust.ID, "")
            lbInfo.Content = "Error. User ID should contain only letter and digital! Illegal char removed!"
            Exit Sub
        End If
        If sCust.ID.Length > 128 Then
            lbInfo.Content = "Error. User ID should no longer than 128 letters!"
            Exit Sub
        End If
        If pbPassword1.Password.Length = 0 Or pbPassword2.Password.Length = 0 Then
            lbInfo.Content = "Error. Empty password!"
            Exit Sub
        End If
        If pbPassword1.Password.Length > 32 Then
            lbInfo.Content = "Error. Password cannot be longer than 32 characters!"
            Exit Sub
        End If
        validinfo = CheckPassword()

        If Not validinfo Then
            lbInfo.Content = "The 'Confirm' password does not match the original password!"
            Exit Sub
        End If
        If Not RFC2822.IsMatch(sCust.EmailAddress) Then
            lbInfo.Content = "Error. Wrong Email Format!"
            Exit Sub
        End If
        If Not CType(acpRegion.Values, IEnumerable(Of String)).Contains(tbRegion.Text) Then
            lbInfo.Content = "The 'Region' is not the the suggestion list!"
            Exit Sub
        Else
            sCust.Region = CType(acpRegion.Values, IEnumerable(Of String)).IndexOf(tbRegion.Text)
        End If
        If sCust.Name Is Nothing OrElse sCust.Name.Length = 0 Then
            lbInfo.Content = "'Name' must be Valid!"
            Exit Sub
        End If
        If sCust.PhoneNumber Is Nothing OrElse Not sCust.PhoneNumber.IsMatch("\d+") Then
            lbInfo.Content = "Telephone Number must contiain digitals!"
            Exit Sub
        End If
        If sCust.Country Is Nothing OrElse Not sCust.Country.IsMatch("\w+") Then
            lbInfo.Content = "'Country' must contiain letters!"
            Exit Sub
        End If
        If (sCust.Company Is Nothing And sCust.Institution Is Nothing) OrElse (Not sCust.Company.IsMatch("\w+") And Not sCust.Institution.IsMatch("\w+")) Then
            lbInfo.Content = "Either 'Institution' or 'Company' must be valid!"
            Exit Sub
        End If
        Dim pass As String = pbPassword1.Password
        sCust.Password = LoginManagement.EncryptPassword(pass)

        lbInfo.Content = "Generating Registration Info ..."
        sCust.B64Key = Await GetB64Key()
        lbInfo.Content = "Getting Computer Key ..."
        sCust.ComputerKey = Await LoginManagement.GetComputerKey
        lbInfo.Content = "Connecting to Synthenome ..."
        Dim sData = Await Async(Of SynContract.ISynData)(Function() As SynContract.ISynData
                                                             Try
                                                                 Return WellKnownExtension.GetService(Of SynContract.ISynData)(ServerAddress, ServerPort, ServiceName)
                                                             Catch ex As Exception
                                                                 Return Nothing
                                                             End Try
                                                         End Function)
        If Not (TypeOf sData Is SynContract.ISynData) Then
            lbInfo.Content = "Unable to Connect Synthenome ..."
            Exit Sub
        End If
        lbInfo.Content = "Registering Customer ..."
        Dim vCust = Await Async(Of Nullable(Of Boolean))(Function() As Boolean?
                                                             Try
                                                                 Return sData.CreateCustomer(sCust)
                                                             Catch ex As Exception
                                                                 Return Nothing
                                                             End Try
                                                         End Function)
        If vCust Is Nothing Then
            lbInfo.Content = "Server Error ..."
        ElseIf vCust = True Then
            LoginManagement.AddUserToList(sCust.ID)
            WPFContainer.GetWinForm(Me).DialogResult = DialogResult.OK
        ElseIf vCust = False Then
            lbInfo.Content = "The User Name has been registered. Please change a different one ..."
        End If
    End Sub

    '用于生成一个B64Key编码的然后传送给Server 然后发送给Server
    'Server端需要解码之后发送一个解密文件给软件
    Private Async Function GetB64Key() As System.Threading.Tasks.Task(Of String)

        '通过IDSTB生成一个特殊的字符串 然后再根据这个字符串生成一个密钥发送给server
        'server根据这个授权证书发布使用权
        Dim IDSTB As New System.Text.StringBuilder
        If tbComputerID.Text Is Nothing OrElse tbComputerID.Text = "" Then tbComputerID.Text = "0"
        IDSTB.AppendLine("Professional")
        IDSTB.AppendLine(tbComputerID.Text)
        IDSTB.AppendLine(tbEmail.Text)
        IDSTB.AppendLine(tbInstitution.Text + tbCompany.Text)
        Await Async(Of Boolean)(Function()
                                    '另一种代码形式的
                                    Dim objMOS As ManagementObjectSearcher
                                    Dim objMOC As Management.ManagementObjectCollection
                                    Dim objMO As Management.ManagementObject = Nothing
                                    'Now, execute the query to get the results
                                    objMOS = New ManagementObjectSearcher("Select * From Win32_Processor")
                                    objMOC = objMOS.Get
                                    'Finally, get the CPU's id.
                                    For Each objMO In objMOC
                                        IDSTB.AppendLine(objMO("ProcessorID"))
                                    Next
                                    'Dispose object variables.
                                    objMOS.Dispose()
                                    objMOS = Nothing
                                    objMO.Dispose()
                                    objMO = Nothing
                                    Return True
                                End Function)
        '//
        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(IDSTB.ToString())

        Dim des As Aes = Aes.Create

        Dim tsf = des.CreateEncryptor(KI, VI)

        Dim cypt As Byte() = tsf.TransformFinalBlock(bytes, 0, bytes.Length)

        Dim email As String = tbEmail.Text
        Dim b64 = ConvertToHexString(cypt)

        Return b64

    End Function
    Private VI As Byte()
    Private KI As Byte()

End Class
