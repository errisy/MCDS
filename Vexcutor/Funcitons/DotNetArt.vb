Public Class DotNetArt
    Inherits Control
    Private context As BufferedGraphicsContext '双缓冲buffer管理
    Private grafx As BufferedGraphics '双缓冲的buffer
    Private grfrec As TypeRecorder(Of Graphics)  '记录所有图片绘制过程用来在重绘时快速描绘
    Private myGraphics As Graphics '该空间的DC
    'DNA控件列表
    Private DNAList As New List(Of DotNetArtItemBase)
    '预加载的DNA控件列表 这些控件主要是实现高级功能 比如视图缩放平移和控件拖拽等等
    Private PreloadDNAList As New List(Of DotNetArtItemBase)
    '绘图参数
    Private Setting As New DNASetting(Me)

    '
    Public Sub New()
        '
        context = BufferedGraphicsManager.Current
        ' 此调用是 Windows 窗体设计器所必需的。
        'InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(Width + 1, Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, Width, Height))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = Me.CreateGraphics
    End Sub

#Region "鼠标和键盘事件"
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        '生成鼠标事件
        Dim em As New DNAMouseEvent((e.Location - Setting.Offset) / Setting.Scale, e.Button, e.Delta, Setting)
        If Not (mMouseCaptureDNA Is Nothing) Then
            mMouseCaptureDNA.OnMouseDown(em)
            If em.Block Then Exit Sub
        End If
        '先对预加载控件进行处理MouseDown
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnMouseDown(em)
            If em.Block Then Exit Sub
        Next


        If mSelectDNA Is Nothing Then
            '如果没有任何控件被选中 那么先进行HitTest
            For Each DNA As DotNetArtItemBase In DNAList
                If DNA.HitTest(em) Then Exit For
            Next

            '控件可以在HitTest阶段要求终止该次事件
            If em.Block Then Exit Sub
            '然后对可能被选中的控件进行处理
            If Not (mSelectDNA Is Nothing) Then mSelectDNA.OnMouseDown(em)
        Else
            mSelectDNA.OnMouseDown(em)
        End If
 
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        '生成鼠标事件
        Dim em As New DNAMouseEvent((e.Location - Setting.Offset) / Setting.Scale, e.Button, e.Delta, Setting)

        If Not (mMouseCaptureDNA Is Nothing) Then
            mMouseCaptureDNA.OnMouseMove(em)
            If em.Block Then Exit Sub
        End If
        '先对预加载控件进行处理MouseDown
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnMouseMove(em)
            If em.Block Then Exit Sub
        Next


        If mSelectDNA Is Nothing Then
            '如果没有任何控件被选中 那么先进行HitTest
            'For Each DNA As DotNetArtItemBase In DNAList
            '    DNA.HitTest(em)
            'Next
            ''然后对可能被选中的控件进行处理
            'If Not (mSelectDNA Is Nothing) Then mSelectDNA.OnMouseDown(em)
        Else
            mSelectDNA.OnMouseMove(em)
        End If
 
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        '生成鼠标事件
        Dim em As New DNAMouseEvent((e.Location - Setting.Offset) / Setting.Scale, e.Button, e.Delta, Setting)

        If Not (mMouseCaptureDNA Is Nothing) Then
            mMouseCaptureDNA.OnMouseUp(em)
            If em.Block Then Exit Sub
        End If
        '先对预加载控件进行处理
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnMouseUp(em)
            If em.Block Then Exit Sub
        Next


        If mSelectDNA Is Nothing Then
            '如果没有任何控件被选中 那么先进行HitTest
            'For Each DNA As DotNetArtItemBase In DNAList
            '    DNA.HitTest(em)
            'Next
            ''然后对可能被选中的控件进行处理
            'If Not (mSelectDNA Is Nothing) Then mSelectDNA.OnMouseDown(em)
        Else
            mSelectDNA.OnMouseUp(em)
        End If
 
    End Sub
    Protected Overrides Sub OnMouseDoubleClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDoubleClick(e)
        '生成鼠标事件
        Dim em As New DNAMouseEvent((e.Location - Setting.Offset) / Setting.Scale, e.Button, e.Delta, Setting)

        If Not (mMouseCaptureDNA Is Nothing) Then
            mMouseCaptureDNA.OnMouseDoubleClick(em)
            If em.Block Then Exit Sub
        End If
        '先对预加载控件进行处理
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnMouseDoubleClick(em)
            If em.Block Then Exit Sub
        Next



        If mSelectDNA Is Nothing Then
            '如果没有任何控件被选中 那么先进行HitTest
            'For Each DNA As DotNetArtItemBase In DNAList
            '    DNA.HitTest(em)
            'Next
            ''然后对可能被选中的控件进行处理
            'If Not (mSelectDNA Is Nothing) Then mSelectDNA.OnMouseDown(em)
        Else
            mSelectDNA.OnMouseDoubleClick(em)
        End If
    End Sub
    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyDown(e)
        '生成键盘事件
        Dim ek As New DNAKeyEvent(e.KeyCode, Setting)

        If Not (mKeyCaptureDNA Is Nothing) Then
            mKeyCaptureDNA.OnKeyDown(ek)
            If ek.Block Then Exit Sub
        End If
        '先对预加载控件进行处理
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnKeyDown(ek)
            If ek.Block Then Exit Sub
        Next
        If Not (mSelectDNA Is Nothing) Then
            mSelectDNA.OnKeyDown(ek)
        End If
    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyUp(e)
        '生成键盘事件
        Dim ek As New DNAKeyEvent(e.KeyCode, Setting)

        If Not (mKeyCaptureDNA Is Nothing) Then
            mKeyCaptureDNA.OnKeyUp(ek)
            If ek.Block Then Exit Sub
        End If
        '先对预加载控件进行处理
        For Each DNA As DotNetArtItemBase In PreloadDNAList
            DNA.OnKeyUp(ek)
            If ek.Block Then Exit Sub
        Next
        If Not (mSelectDNA Is Nothing) Then
            mSelectDNA.OnKeyUp(ek)
        End If
    End Sub
#End Region

#Region "控件管理"
    Public Sub AddPreLoadDNA(ByVal vDNAItem As DotNetArtItemBase)
        vDNAItem.UpdateVeiw = AddressOf UpdateView
        vDNAItem.SetMouseCapture = AddressOf SetMouseCapture
        vDNAItem.SetKeyCapture = AddressOf SetKeyCapture
        vDNAItem.SetSelect = AddressOf SetSelect
        PreloadDNAList.Add(vDNAItem)
    End Sub
    Public Sub AddDNAItem(ByVal vDNAItem As DotNetArtItemBase)
        vDNAItem.UpdateVeiw = AddressOf UpdateView
        vDNAItem.SetMouseCapture = AddressOf SetMouseCapture
        vDNAItem.SetKeyCapture = AddressOf SetKeyCapture
        vDNAItem.SetSelect = AddressOf SetSelect
        DNAList.Add(vDNAItem)
    End Sub
    Public Sub RemoveDNAItem(ByVal vDNAItem As DotNetArtItemBase)
        DNAList.Remove(vDNAItem)
        vDNAItem.UpdateVeiw = Nothing
        vDNAItem.SetMouseCapture = Nothing
        vDNAItem.SetKeyCapture = Nothing
        vDNAItem.SetSelect = Nothing
    End Sub
    Public Function UpdateView() As Boolean
        Draw(True)
    End Function
    Private mMouseCaptureDNA As DotNetArtItemBase
    Public Function SetMouseCapture(ByVal Source As DotNetArtItemBase) As Boolean
        mMouseCaptureDNA = Source
    End Function
    Private mKeyCaptureDNA As DotNetArtItemBase
    Public Function SetKeyCapture(ByVal Source As DotNetArtItemBase) As Boolean
        mKeyCaptureDNA = Source
    End Function
    Private mSelectDNA As DotNetArtItemBase
    Public Function SetSelect(ByVal Source As DotNetArtItemBase) As Boolean
        mSelectDNA = Source
    End Function
#End Region
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        grafx.Render(e.Graphics)
        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        context = BufferedGraphicsManager.Current
        context.MaximumBuffer = New Size(Width + 1, Height + 1)
        grafx = context.Allocate(CreateGraphics(), New Rectangle(0, 0, Width, Height))
        grafx.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        myGraphics = Me.CreateGraphics
    End Sub

    Public Sub Draw(Optional ByVal Redraw As Boolean = False)
        'draw the gene list on to the screen
        If Redraw Then
            Dim memStream As New System.IO.MemoryStream()
            grfrec = New TypeRecorder(Of Graphics)
            '请用Typerecorder grfrec来绘制所有过程
            '
            'grafx.Graphics.Clear(Color.White)

            grfrec.AddMethodRecord("Clear", New Object() {Color.White})


            '最后在graphics上绘制
            grfrec.Playback(grafx.Graphics)
        Else
            grfrec.Playback(grafx.Graphics)
        End If
        grafx.Render(myGraphics)
    End Sub
End Class


Public Delegate Function UpdateViewDelegate() As Boolean
Public Delegate Function SetMouseCaptureDelegate(ByVal Source As DotNetArtItemBase) As Boolean
Public Delegate Function SetKeyCaptureDelegate(ByVal Source As DotNetArtItemBase) As Boolean
Public Delegate Function SetSelectDelegate(ByVal Source As DotNetArtItemBase) As Boolean

Public Class DNAMouseEvent
    '鼠标点击的位置 是以绘图坐标为标准的
    Public Position As Vector
    '指定该事件的发生是否阻止程序继续将时间传递给其他控件
    Public Block As Boolean
    '鼠标键
    Public MouseButton As MouseButtons
    '设置
    Public Setting As DNASetting
    '
    Public Wheel As Integer

    Public Sub New(ByVal vPosition As Vector, ByVal vMouseButton As MouseButtons, ByVal vWheel As Integer, ByVal vSetting As DNASetting)
        Position = vPosition
        MouseButton = vMouseButton
        Setting = vSetting
        Wheel = vWheel
    End Sub
End Class
Public Class DNAKeyEvent
    Public KeyCode As Keys
    '指定该事件的发生是否阻止程序继续将时间传递给其他控件
    Public Block As Boolean
    '设置
    Public Setting As DNASetting
    Public Sub New(ByVal vKeyCode As Keys, ByVal vSetting As DNASetting)
        KeyCode = vKeyCode
        Setting = vSetting
    End Sub
End Class
Public Class DNASetting
    '保存了DNA绘图的基本参数
    Public Scale As Single = 1
    Public Offset As Vector
    Public DNAContainer As DotNetArt
    Public Sub New(ByVal vDNA As DotNetArt)
        DNAContainer = vDNA
    End Sub
End Class
Public Class DNADragSetting
    '保存了拖拽操作当中所需的基本参数
    Public OldPosition As Vector
    Public StartMouse As Vector
    Public Dragging As Boolean = False
    Public Sub StartDrag(ByVal vOldPosition As Vector, ByVal vStartMouse As Vector)
        OldPosition = vOldPosition
        StartMouse = vStartMouse
        Dragging = True
    End Sub
    Public Function GetCurrentPosition(ByVal vCurrectMouse As Vector)
        Return OldPosition + vCurrectMouse - StartMouse
    End Function
    Public Function EndPosition(ByVal vCurrectMouse As Vector)
        Dragging = False
        Return OldPosition + vCurrectMouse - StartMouse
    End Function
End Class

Public MustInherit Class DotNetArtItemBase

    '该控件用来绘制的图形的缓存工具 利用这个工具就不需要重新计算绘图参数 可以提高性能
    Protected vGrfRec As TypeRecorder(Of Graphics)

    '用来缓存绘制完毕的图形边界
    Protected vBound As RectangleF

    '用来确定图形是否需要重新绘制
    Protected vChanged As Boolean

    '更新视图的代理函数
    Public UpdateVeiw As UpdateViewDelegate
    '设定是否捕获鼠标事件
    Public SetMouseCapture As SetMouseCaptureDelegate
    '设定是否优先捕获键盘事件
    Public SetKeyCapture As SetKeyCaptureDelegate
    '设定是否出于选中状态 并优先捕获键盘事件
    Public SetSelect As SetSelectDelegate

    '重新绘制控件
    Public Overridable Sub Draw(ByVal grfrec As TypeRecorder(Of Graphics))

    End Sub

    '获取控件边界
    Public Overridable Function GetBound() As RectangleF

    End Function

    '指示控件是否已经发生更新且尚未重新绘制
    Public Overridable ReadOnly Property ViewChanged() As Boolean
        Get
            Return vChanged
        End Get
    End Property

    '鼠标点击测试函数
    Public Function HitTest(ByVal e As DNAMouseEvent) As Boolean

    End Function


    Public Overridable Sub OnMouseDown(ByVal e As DNAMouseEvent)

    End Sub
    Public Overridable Sub OnMouseUp(ByVal e As DNAMouseEvent)

    End Sub
    Public Overridable Sub OnMouseMove(ByVal e As DNAMouseEvent)

    End Sub
    Public Overridable Sub OnMouseWheel(ByVal e As DNAMouseEvent)

    End Sub
    Public Overridable Sub OnMouseDoubleClick(ByVal e As DNAMouseEvent)

    End Sub
    Public Overridable Sub OnKeyDown(ByVal e As DNAKeyEvent)

    End Sub
    Public Overridable Sub OnKeyUp(ByVal e As DNAKeyEvent)

    End Sub
End Class

Public Class PreloadDNAScrollZoom
    Inherits DotNetArtItemBase
    Public MouseDragMode As Boolean = False
    Public Overrides Sub OnKeyDown(ByVal e As DNAKeyEvent)
        If e.KeyCode = Keys.Space Then
            MouseDragMode = True
            e.Block = True
        End If
    End Sub
    Public Overrides Sub OnKeyUp(ByVal e As DNAKeyEvent)
        If e.KeyCode = Keys.Space And MouseDragMode Then
            MouseDragMode = False
            e.Block = True
        End If
    End Sub
    Private ViewDrag As New DNADragSetting
    Public Overrides Sub OnMouseDown(ByVal e As DNAMouseEvent)
        If MouseDragMode Then
            ViewDrag.StartDrag(e.Setting.Offset, e.Position)
            e.Block = True
        End If
    End Sub
    Public Overrides Sub OnMouseMove(ByVal e As DNAMouseEvent)
        If MouseDragMode And ViewDrag.Dragging Then
            e.Setting.Offset = ViewDrag.GetCurrentPosition(e.Position)
            e.Block = True
            UpdateVeiw.Invoke()
        End If
    End Sub
    Public Overrides Sub OnMouseUp(ByVal e As DNAMouseEvent)
        If MouseDragMode Then
            e.Setting.Offset = ViewDrag.EndPosition(e.Position)
            e.Block = True
            UpdateVeiw.Invoke()
        End If
    End Sub
    Public Overrides Sub OnMouseWheel(ByVal e As DNAMouseEvent)
        Select Case Control.ModifierKeys
            Case Keys.Control
                e.Setting.Scale *= 0.75 ^ (e.Wheel / 120)
                UpdateVeiw.Invoke()
            Case Keys.Shift
                e.Setting.Offset.X += 30 * (e.Wheel / 120) / e.Setting.Scale
                UpdateVeiw.Invoke()
            Case Else
                e.Setting.Offset.Y += 30 * (e.Wheel / 120) / e.Setting.Scale
                UpdateVeiw.Invoke()
        End Select
    End Sub
End Class

Public Class LineShape
    Inherits DotNetArtItemBase
    Protected vPoints As PointF()


    Public Property Points() As PointF()
        Get
            Return vPoints
        End Get
        Set(ByVal value As PointF())
            vPoints = value
        End Set
    End Property

    Protected vFillColor As Color
    Public Property FillColor() As Color
        Get
            Return vFillColor
        End Get
        Set(ByVal value As Color)
            vFillColor = value
        End Set
    End Property

    Protected vLineColor As Color
    Public Property LineColor() As Color
        Get
            Return vLineColor
        End Get
        Set(ByVal value As Color)
            vLineColor = value
        End Set
    End Property

    Public Overrides Sub Draw(ByVal grfrec As TypeRecorder(Of Graphics))
        'vGraphics.FillClosedCurve(New SolidBrush(vFillColor), vPoints, Drawing2D.FillMode.Winding, 0)
        'vGraphics.DrawClosedCurve(New Pen(vLineColor), vPoints, 0, Drawing2D.FillMode.Winding)
        'vGraphics.Dispose()
    End Sub

    Public Overrides Function GetBound() As System.Drawing.RectangleF

    End Function
End Class