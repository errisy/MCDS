Public Class TabRibbonButton
    Inherits System.Windows.Controls.Ribbon.RibbonButton
    'TabRibbonButton->TabHost As TabControl with Event Default: Nothing
    Public Property TabHost As TabControl
        Get
            Return GetValue(TabHostProperty)
        End Get
        Set(ByVal value As TabControl)
            SetValue(TabHostProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TabHostProperty As DependencyProperty = _
                           DependencyProperty.Register("TabHost", _
                           GetType(TabControl), GetType(TabRibbonButton), _
                           New PropertyMetadata(Nothing))

    'TabRibbonButton->TabContentTemplate As DataTemplate Default: Nothing
    Public Property TabContentTemplate As DataTemplate
        Get
            Return GetValue(TabContentTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(TabContentTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TabContentTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("TabContentTemplate", _
                           GetType(DataTemplate), GetType(TabRibbonButton), _
                           New PropertyMetadata(Nothing))
    'TabRibbonButton->TabTitle As String Default: ""
    Public Property TabTitle As String
        Get
            Return GetValue(TabTitleProperty)
        End Get
        Set(ByVal value As String)
            SetValue(TabTitleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TabTitleProperty As DependencyProperty = _
                           DependencyProperty.Register("TabTitle", _
                           GetType(String), GetType(TabRibbonButton), _
                           New PropertyMetadata(""))
    'TabRibbonButton->TabHeaderTemplate As DataTemplate Default: Nothing
    Public Property TabHeaderTemplate As DataTemplate
        Get
            Return GetValue(TabHeaderTemplateProperty)
        End Get
        Set(ByVal value As DataTemplate)
            SetValue(TabHeaderTemplateProperty, value)
        End Set
    End Property
    Public Shared ReadOnly TabHeaderTemplateProperty As DependencyProperty = _
                           DependencyProperty.Register("TabHeaderTemplate", _
                           GetType(DataTemplate), GetType(TabRibbonButton), _
                           New PropertyMetadata(Nothing))

    ''TabRibbonButton->HasCloseButton As Boolean Default: True
    'Public Property HasCloseButton As Boolean
    '    Get
    '        Return GetValue(HasCloseButtonProperty)
    '    End Get
    '    Set(ByVal value As Boolean)
    '        SetValue(HasCloseButtonProperty, value)
    '    End Set
    'End Property
    'Public Shared ReadOnly HasCloseButtonProperty As DependencyProperty = _
    '                       DependencyProperty.Register("HasCloseButton", _
    '                       GetType(Boolean), GetType(TabRibbonButton), _
    '                       New PropertyMetadata(True))
    'TabRibbonButton->AskBeforeClose As Boolean Default: False
    Public Property AskBeforeClose As Boolean
        Get
            Return GetValue(AskBeforeCloseProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(AskBeforeCloseProperty, value)
        End Set
    End Property
    Public Shared ReadOnly AskBeforeCloseProperty As DependencyProperty = _
                           DependencyProperty.Register("AskBeforeClose", _
                           GetType(Boolean), GetType(TabRibbonButton), _
                           New PropertyMetadata(False))
    ''TabRibbonButton->TabDataContext As NavigationTrackingModel Default: Nothing
    'Public Property TabDataContext As NavigationTrackingModel
    '    Get
    '        Return GetValue(TabDataContextProperty)
    '    End Get
    '    Set(ByVal value As NavigationTrackingModel)
    '        SetValue(TabDataContextProperty, value)
    '    End Set
    'End Property
    'Public Shared ReadOnly TabDataContextProperty As DependencyProperty = _
    '                       DependencyProperty.Register("TabDataContext", _
    '                       GetType(NavigationTrackingModel), GetType(TabRibbonButton), _
    '                       New PropertyMetadata(Nothing))
    'TabRibbonButton->SaveDataContextOnCreation As Boolean Default: False
    Public Property SaveDataContextOnCreation As Boolean
        Get
            Return GetValue(SaveDataContextOnCreationProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(SaveDataContextOnCreationProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SaveDataContextOnCreationProperty As DependencyProperty = _
                           DependencyProperty.Register("SaveDataContextOnCreation", _
                           GetType(Boolean), GetType(TabRibbonButton), _
                           New PropertyMetadata(False))
    'TabRibbonButton->Suffix As String Default: ""
    Public Property Suffix As String
        Get
            Return GetValue(SuffixProperty)
        End Get
        Set(ByVal value As String)
            SetValue(SuffixProperty, value)
        End Set
    End Property
    Public Shared ReadOnly SuffixProperty As DependencyProperty = _
                           DependencyProperty.Register("Suffix", _
                           GetType(String), GetType(TabRibbonButton), _
                           New PropertyMetadata(""))


    Protected Overrides Sub OnClick()
        If TypeOf TabHost Is TabControl AndAlso TypeOf TabContentTemplate Is DataTemplate Then
            Dim obj As FrameworkElement = TabContentTemplate.LoadContent
 
            Dim ti = New ClosableTabItem With {.Content = obj, .AskBeforeClose = AskBeforeClose}
            If TypeOf TabHeaderTemplate Is DataTemplate Then
                ti.Header = TabHeaderTemplate.LoadContent
            Else
                ti.Header = TabTitle
            End If
            AddHandler ti.TabClose, AddressOf TabClose
            TabHost.Items.Add(ti)
            TabHost.SelectedItem = ti
        End If
        MyBase.OnClick()
    End Sub
    Private Sub TabClose(sender As Object, e As RoutedEventArgs)
        If TypeOf TabHost Is TabControl AndAlso TypeOf TabContentTemplate Is DataTemplate Then
            Dim ti As ClosableTabItem = e.OriginalSource
            RemoveHandler ti.TabClose, AddressOf TabClose
            TabHost.Items.Remove(ti)
        End If
    End Sub
End Class
