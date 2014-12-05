Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Partial Public Class rToolTip
    Inherits Component

#Region " CLASES INTERNAS "
    Private NotInheritable Class Win32
        Public Const TOOLTIPS_CLASS As String = "tooltips_class32"
        Public Const TTS_ALWAYSTIP As Integer = &H1
        Public Const TTS_NOFADE As Integer = &H10
        Public Const TTS_NOANIMATE As Integer = &H20
        Public Const TTS_BALLOON As Integer = &H40
        Public Const TTF_IDISHWND As Integer = &H1
        Public Const TTF_CENTERTIP As Integer = &H2
        Public Const TTF_TRACK As Integer = &H20
        Public Const TTF_TRANSPARENT As Integer = &H100
        Public Const WM_SETFONT As Integer = &H30
        Public Const WM_GETFONT As Integer = &H31
        Public Const WM_PRINTCLIENT As Integer = &H318
        Public Const WM_USER As Integer = &H400
        Public Const TTM_TRACKACTIVATE As Integer = WM_USER + 17
        Public Const TTM_TRACKPOSITION As Integer = WM_USER + 18
        Public Const TTM_SETMAXTIPWIDTH As Integer = WM_USER + 24
        Public Const TTM_GETBUBBLESIZE As Integer = WM_USER + 30
        Public Const TTM_ADDTOOL As Integer = WM_USER + 50
        Public Const TTM_DELTOOL As Integer = WM_USER + 51
        Public Const SWP_NOSIZE As Integer = &H1
        Public Const SWP_NOACTIVATE As Integer = &H10
        Public Const SWP_NOOWNERZORDER As Integer = &H200
        Public Shared ReadOnly HWND_TOPMOST As New IntPtr(-1)

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure TOOLINFO
            Public cbSize As Integer
            Public uFlags As Integer
            Public hwnd As IntPtr
            Public uId As IntPtr
            Public rect As RECT
            Public hinst As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)> _
            Public lpszText As String

            Public lParam As System.UInt32
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure SIZE
            Public cx As Integer
            Public cy As Integer
        End Structure

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Integer
        End Function

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, ByRef lParam As TOOLINFO) As Integer
        End Function

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, ByRef lParam As RECT) As Integer
        End Function

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
        End Function

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, X As Integer, Y As Integer, cx As Integer, cy As Integer, _
                uFlags As Integer) As Boolean
        End Function

        <DllImport("User32", SetLastError:=True)> _
        Public Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr
        End Function
    End Class

    Private Class cPanelContenedor
        Inherits UserControl

#Region " DECLARACIONES "
        Private iToolTipHwnd As IntPtr
#End Region

#Region " CONSTRUCTORES "
        Public Sub New(ByVal eToolTipHWnd As IntPtr)
            iToolTipHwnd = eToolTipHWnd
            Win32.SetParent(Handle, eToolTipHWnd)
        End Sub
#End Region

#Region " METODOS SOBRECARGADOS "
        Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
            Win32.SendMessage(iToolTipHwnd, Win32.WM_PRINTCLIENT, CInt(e.Graphics.GetHdc()), 0)
        End Sub
#End Region
    End Class

    Private Class cToolTipInterno
        Inherits NativeWindow
        Implements IDisposable

#Region " DECLARACIONES "
        ''' <summary>
        ''' Distancia desde el borde hasta el pico
        ''' </summary>
        Private Const iDistanciaPico As Integer = 16

        ''' <summary>
        ''' Estructura con la información  WIN32 necesaria para la gestión
        ''' del ToolTip por la API de Windows
        ''' </summary>
        Private iToolInfo As Win32.TOOLINFO

        ''' <summary>
        ''' Determina si el ratón se encuentra sobre el control
        ''' </summary>
        Private iRatonEncima As Boolean = False

        ''' <summary>
        ''' Control asociado al ToolTip
        ''' </summary>
        Public iControlAsociado As IWin32Window
#End Region

#Region " PROPIEDADES "
        ''' <summary>
        ''' Panel contenedor del control mostrado en el ToolTip
        ''' </summary>
        Public ReadOnly Property PanelContenedor As cPanelContenedor
            Get
                Return iPanelContenedor
            End Get
        End Property
        Private iPanelContenedor As cPanelContenedor
#End Region

#Region " EVENTOS "
        ''' <summary>
        ''' Evento que se lanza cuando el ratón entra sobre el control
        ''' </summary>
        Public Event MouseEnter(ByVal sender As Object, ByVal e As EventArgs)

        ''' <summary>
        ''' Evento que se lanza cuando el ratón sale del control
        ''' </summary>
        Public Event MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region " MANEJADORES "
        Private Sub manejadorMouseEnter(ByVal sender As Object, ByVal e As EventArgs)
            RaiseEvent MouseEnter(Me, e)
        End Sub

        Private Sub manejadorMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            RaiseEvent MouseLeave(Me, e)
        End Sub
#End Region

#Region " CONSTRUCTORES "
        Public Sub New(ByRef eControlInterno As Control, _
                       ByVal eControlAsociado As IWin32Window, _
                       ByVal eX As Integer, _
                       ByVal eY As Integer, _
                       ByVal ePosicionPico As enmPosicionPico, _
                       ByVal eUsarAnimacion As Boolean, _
                       ByVal eUsarFading As Boolean)
            ' Se guarda el control asociado para operaciones posteriores
            iControlAsociado = eControlAsociado

            ' Se crean los parámetros para ajustar el ToolTip
            Dim losParametros As New CreateParams()
            With losParametros
                .ClassName = Win32.TOOLTIPS_CLASS
                .Style = Win32.TTS_ALWAYSTIP Or Win32.TTS_BALLOON
                If Not eUsarAnimacion Then .Style = .Style Or Win32.TTS_NOANIMATE
                If Not eUsarFading Then .Style = .Style Or Win32.TTS_NOFADE
            End With
            CreateHandle(losParametros)

            ' Se obtiene el tamaño que tiene que medir la ventana simulando una cadena
            ' de texto que ocupe lo mismo que el control que se quiere mostrar ya que
            ' al ToolTip no se le puede fijar de forma automática el tamaño
            Dim contentSpacing As String = control2ContentSpacing(eControlInterno)

            ' Se realizan los primeros cálculos para obtener la posición y el tamaño
            ' a partir del string que se generó y se obtienen las dimensiones de la 
            ' pantalla para realizar los cálculos de las posiciones relativas
            Dim lasDimensiones As Rectangle = calcularDimensionesToolTip(contentSpacing, eX, eY, ePosicionPico)
            Dim laPantalla As Screen = Screen.FromHandle(iControlAsociado.Handle)
            Dim laPantallaDimensiones As Rectangle = laPantalla.WorkingArea

            ' Se calcula donde se tiene que mostrar el pico a partir de los cálculos
            ' preliminares obteniendo la posición real
            ePosicionPico = calcularPosicionPico(ePosicionPico, lasDimensiones, laPantallaDimensiones)

            ' Se ajustan las dimensiones a partir de los nuevos cálculos 
            lasDimensiones = calcularDimensionesToolTip(contentSpacing, eX, eY, ePosicionPico)
            lasDimensiones.X = Math.Max(0, lasDimensiones.X)
            lasDimensiones.Y = Math.Max(0, lasDimensiones.Y)

            ' Se crea el ToolInfo para la comunicación con Windows
            iToolInfo = crearToolTip(contentSpacing, iControlAsociado, ePosicionPico)

            ' Posición inicial del control para asegurarse que el pico se muestra en
            ' la posición correcta
            Dim xInicial As Integer = laPantallaDimensiones.X
            Dim yInicial As Integer = laPantallaDimensiones.Y

            If enmPosicionPico.ArribaIzquierda = ePosicionPico OrElse enmPosicionPico.AbajoIzquierda = ePosicionPico Then
                xInicial += iDistanciaPico
            ElseIf enmPosicionPico.ArribaCentro = ePosicionPico OrElse enmPosicionPico.AbajoCentro = ePosicionPico Then
                xInicial += laPantallaDimensiones.Width \ 2
            Else
                xInicial += laPantallaDimensiones.Width - iDistanciaPico
            End If

            If enmPosicionPico.AbajoIzquierda = ePosicionPico OrElse enmPosicionPico.AbajoCentro = ePosicionPico OrElse enmPosicionPico.AbajoDerecha = ePosicionPico Then
                yInicial += laPantallaDimensiones.Height
            End If

            Win32.SendMessage(Handle, Win32.TTM_TRACKPOSITION, 0, (yInicial << 16) Or xInicial)

            ' Se muestra el ToolTip 
            Win32.SendMessage(Handle, Win32.TTM_TRACKACTIVATE, 1, iToolInfo)
            generarLayOut(eControlInterno, ePosicionPico, lasDimensiones)

            ' Se añaden los manejadores para controlar cuando el raton entra en el
            ' panel y sale de este para controlar si se tiene que ocultar 
            AddHandler iPanelContenedor.MouseEnter, AddressOf manejadorMouseEnter
            AddHandler iPanelContenedor.MouseLeave, AddressOf manejadorMouseLeave
        End Sub

        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub

        Private Sub Dispose(disposing As Boolean)
            If disposing Then
                ' Se envían los mensajes a la API de Windows para la destrucción del ToolTip
                Win32.SendMessage(Handle, Win32.TTM_TRACKACTIVATE, 0, iToolInfo)
                Win32.SendMessage(Handle, Win32.TTM_DELTOOL, 0, iToolInfo)

                ' Se destruye el control cargando en el panel contenedor
                ' y el propio panel contenedor
                If iPanelContenedor IsNot Nothing Then
                    With iPanelContenedor
                        .Controls.Clear()
                        .Dispose()
                    End With
                End If

                ' Se destruyen los manejadores asignados al control
                DestroyHandle()
            End If
        End Sub
#End Region

#Region " METODOS PRIVADOS "
        Private Function crearToolTip(ByVal eContentSpacing As String, _
                                      ByVal eWindow As IWin32Window, _
                                      ByVal ePosicionPico As enmPosicionPico) As Win32.TOOLINFO
            Dim paraDevolver As Win32.TOOLINFO = Nothing

            Try
                paraDevolver = New Win32.TOOLINFO()
                With paraDevolver
                    .cbSize = Marshal.SizeOf(paraDevolver)
                    .uFlags = Win32.TTF_IDISHWND Or Win32.TTF_TRACK Or Win32.TTF_TRANSPARENT
                    .uId = iControlAsociado.Handle
                    .hwnd = iControlAsociado.Handle
                    .lpszText = eContentSpacing

                    If ePosicionPico = enmPosicionPico.AbajoCentro OrElse ePosicionPico = enmPosicionPico.ArribaCentro Then
                        .uFlags = .uFlags Or Win32.TTF_CENTERTIP
                    End If

                    If Win32.SendMessage(Handle, Win32.TTM_ADDTOOL, 0, paraDevolver) = 0 Then
                        paraDevolver = Nothing
                    End If

                    ' Se activa la opción de multi linea
                    Win32.SendMessage(Handle, Win32.TTM_SETMAXTIPWIDTH, 0, SystemInformation.MaxWindowTrackSize.Width)
                End With
            Catch ex As Exception
                paraDevolver = Nothing
            End Try

            Return paraDevolver
        End Function

        Private Function calcularPosicionPico(ByVal ePosicionPicoIncial As enmPosicionPico, _
                                              ByRef eTamanhoToolTip As Rectangle, _
                                              ByRef eTamanhoPantalla As Rectangle) As enmPosicionPico
            If eTamanhoToolTip.Left < eTamanhoPantalla.Left Then
                If enmPosicionPico.ArribaCentro = ePosicionPicoIncial OrElse enmPosicionPico.ArribaDerecha = ePosicionPicoIncial Then
                    ePosicionPicoIncial = enmPosicionPico.ArribaIzquierda
                ElseIf enmPosicionPico.AbajoCentro = ePosicionPicoIncial OrElse enmPosicionPico.AbajoDerecha = ePosicionPicoIncial Then
                    ePosicionPicoIncial = enmPosicionPico.AbajoIzquierda
                End If
            ElseIf eTamanhoToolTip.Right > eTamanhoPantalla.Right Then
                If enmPosicionPico.ArribaCentro = ePosicionPicoIncial OrElse enmPosicionPico.ArribaIzquierda = ePosicionPicoIncial Then
                    ePosicionPicoIncial = enmPosicionPico.ArribaDerecha
                ElseIf enmPosicionPico.AbajoCentro = ePosicionPicoIncial OrElse enmPosicionPico.AbajoIzquierda = ePosicionPicoIncial Then
                    ePosicionPicoIncial = enmPosicionPico.AbajoDerecha
                End If
            End If

            If eTamanhoToolTip.Top < eTamanhoPantalla.Top Then
                Select Case ePosicionPicoIncial
                    Case enmPosicionPico.AbajoIzquierda
                        ePosicionPicoIncial = enmPosicionPico.ArribaIzquierda

                    Case enmPosicionPico.AbajoCentro
                        ePosicionPicoIncial = enmPosicionPico.ArribaCentro

                    Case enmPosicionPico.AbajoDerecha
                        ePosicionPicoIncial = enmPosicionPico.ArribaDerecha
                End Select
            ElseIf eTamanhoToolTip.Bottom > eTamanhoPantalla.Bottom Then
                Select Case ePosicionPicoIncial
                    Case enmPosicionPico.ArribaIzquierda
                        ePosicionPicoIncial = enmPosicionPico.AbajoIzquierda

                    Case enmPosicionPico.ArribaCentro
                        ePosicionPicoIncial = enmPosicionPico.AbajoCentro

                    Case enmPosicionPico.ArribaDerecha
                        ePosicionPicoIncial = enmPosicionPico.AbajoDerecha
                End Select
            End If

            Return ePosicionPicoIncial
        End Function

        Private Function calcularDimensionesToolTip(ByVal eContentSpacing As String, _
                                                    ByVal eX As Integer, _
                                                    ByVal eY As Integer, _
                                                    ByVal ePosicionPico As enmPosicionPico) As Rectangle
            Dim paraDevolver As New Rectangle()
            Dim elTamanhoToolTip As Size = obtenerTamanhoToolTip(eContentSpacing)
            Dim elTamanhoVentana As New Win32.RECT()

            Win32.GetWindowRect(iControlAsociado.Handle, elTamanhoVentana)
            eX += elTamanhoVentana.left

            If enmPosicionPico.ArribaIzquierda = ePosicionPico OrElse enmPosicionPico.AbajoIzquierda = ePosicionPico Then
                paraDevolver.X = eX - iDistanciaPico
            ElseIf enmPosicionPico.ArribaCentro = ePosicionPico OrElse enmPosicionPico.AbajoCentro = ePosicionPico Then
                paraDevolver.X = eX - (elTamanhoToolTip.Width \ 2)
            Else
                paraDevolver.X = eX - elTamanhoToolTip.Width + iDistanciaPico
            End If

            If enmPosicionPico.ArribaIzquierda = ePosicionPico OrElse enmPosicionPico.ArribaCentro = ePosicionPico OrElse enmPosicionPico.ArribaDerecha = ePosicionPico Then
                paraDevolver.Y = elTamanhoVentana.bottom - eY
            Else
                paraDevolver.Y = eY + elTamanhoVentana.top - elTamanhoToolTip.Height
            End If

            paraDevolver.Width = elTamanhoToolTip.Width
            paraDevolver.Height = elTamanhoToolTip.Height

            Return paraDevolver
        End Function

        Private Function obtenerTamanhoToolTip(ByVal eContentSpacing As String) As Size
            Dim laInfo As New Win32.TOOLINFO()
            With laInfo
                .cbSize = Marshal.SizeOf(laInfo)
                .uFlags = Win32.TTF_TRACK
                .lpszText = eContentSpacing
            End With

            If 0 = Win32.SendMessage(Handle, Win32.TTM_ADDTOOL, 0, laInfo) Then
                Throw New Exception()
            End If

            ' Se activa el ajuste multilinea
            Win32.SendMessage(Handle, Win32.TTM_SETMAXTIPWIDTH, 0, SystemInformation.MaxWindowTrackSize.Width)
            Win32.SendMessage(Handle, Win32.TTM_TRACKACTIVATE, 1, laInfo)

            Dim rect As New Win32.RECT()
            Win32.GetWindowRect(Handle, rect)

            Win32.SendMessage(Handle, Win32.TTM_TRACKACTIVATE, 0, laInfo)
            Win32.SendMessage(Handle, Win32.TTM_DELTOOL, 0, laInfo)

            Return New Size(rect.right - rect.left, rect.bottom - rect.top)
        End Function

        Private Sub generarLayOut(ByVal eControl As Control, _
                                  ByVal ePosicionPico As enmPosicionPico, _
                                  ByRef eTamanhoToolTip As Rectangle)
            ' Se realizan los cálculos para obtener el tamaño del globo
            Dim globoTamanho As Integer = Win32.SendMessage(Handle, Win32.TTM_GETBUBBLESIZE, 0, iToolInfo)
            Dim globoAncho As Integer = globoTamanho And &HFFFF
            Dim globoAlto As Integer = globoTamanho >> 16

            ' Se centra el control en el centro del globo (X)
            eControl.Left = (globoAncho - eControl.Width) \ 2

            ' Se posiciona el control (Y) relativo a la posición del Pico
            ' ya que se le tienen que sumar OffSets dependiendo de la posición
            If enmPosicionPico.AbajoIzquierda = ePosicionPico OrElse enmPosicionPico.AbajoCentro = ePosicionPico OrElse enmPosicionPico.AbajoDerecha = ePosicionPico Then
                ' Pico está debajo del globo
                eControl.Top = (globoAlto - eControl.Height) \ 2
            Else
                ' Pico está encima del globo
                Dim bubbleOffset As Integer = eTamanhoToolTip.Height - globoAlto
                eControl.Top = (globoAlto - eControl.Height) \ 2 + bubbleOffset
            End If

            ' Se crea el panel contenedor para el control y se
            ' añade el control que tiene que mostrar
            iPanelContenedor = New cPanelContenedor(Handle)
            With iPanelContenedor
                .Width = eTamanhoToolTip.Width
                .Height = eTamanhoToolTip.Height
                .Controls.Add(eControl)
            End With

            ' Se fija la posición del globo mediante la llamada a la API
            Win32.SetWindowPos(Handle, Win32.HWND_TOPMOST, eTamanhoToolTip.X, eTamanhoToolTip.Y, 0, 0, Win32.SWP_NOACTIVATE Or Win32.SWP_NOSIZE Or Win32.SWP_NOOWNERZORDER)
        End Sub

        Private Function control2ContentSpacing(ByVal eControl As Control) As String
            ' No se pueden fijar directamente las dimensiones del control ya que este se ajusta al tamaño del texto
            ' contenido. Vara simularlo se genera un string falso con el tamaño del control
            Dim elStringBuilder As New StringBuilder()
            Dim laFuente As Font = Font.FromHfont(CType(Win32.SendMessage(Handle, Win32.WM_GETFONT, 0, 0), IntPtr))

            ' Se usa una fuente pequeña para ajustar mejor la precisión del tamaño
            ' y se obtiene lo que mide un caracter
            laFuente = New Font(laFuente.FontFamily, 1.0F)
            Win32.SendMessage(Handle, Win32.WM_SETFONT, CInt(laFuente.ToHfont()), 1)
            Dim tamanhoCaracter As Size = TextRenderer.MeasureText(" ", laFuente)

            ' Se calculan todas las lineas que hay que generar para ajustar el 
            ' alto de la cadena de texto y se añaden a la cadena
            Dim totalLineas As Integer = (eControl.Height + tamanhoCaracter.Height - 1) \ tamanhoCaracter.Height
            For n As Integer = 0 To totalLineas - 1
                elStringBuilder.Append(vbCr & vbLf)
            Next

            ' Se añaden los caracteres necesarios para ajustar el ancho
            tamanhoCaracter = TextRenderer.MeasureText(elStringBuilder.ToString(), laFuente)
            Dim elAncho As Integer = eControl.Width + tamanhoCaracter.Height - eControl.Height
            While tamanhoCaracter.Width < elAncho
                elStringBuilder.Append(" ")
                tamanhoCaracter = TextRenderer.MeasureText(elStringBuilder.ToString(), laFuente)
            End While

            Return elStringBuilder.ToString()
        End Function
#End Region
    End Class
#End Region

#Region " ENUMERADOS "
    ''' <summary>
    ''' Posición donde se tiene que mostrar el pico del globo
    ''' </summary>
    Public Enum enmPosicionPico
        AbajoIzquierda = 10
        AbajoCentro = 11
        AbajoDerecha = 12

        ArribaIzquierda = 20
        ArribaCentro = 21
        ArribaDerecha = 22
    End Enum
#End Region

#Region " DECLARACIONES "
    ''' <summary>
    ''' Timer que permite cerrar el ToolTip de forma automática si no se iteratúa con este
    ''' </summary>
    Private iTimerCerrado As Timer

    ''' <summary>
    ''' Tiempo para el cerrado automático del tooltip si se activa
    ''' </summary>
    Private iTiempoCerrado As Integer = 0

    ''' <summary>
    ''' Control cVentanaToolTip interno
    ''' </summary>
    Private iToolTip As cToolTipInterno
#End Region

#Region " PROPIEDADES "
    ''' <summary>
    ''' Determina si se tiene que mostrar la animación al mostrar el ToolTip
    ''' </summary>
    Public Property usarAnimacion() As Boolean
        Get
            Return iUsarAnimacion
        End Get
        Set(value As Boolean)
            iUsarAnimacion = value
        End Set
    End Property
    Private iUsarAnimacion As Boolean = True

    ''' <summary>
    ''' Determina si se tiene que usar el efecto Fade en el Show y en el Hide
    ''' </summary>
    Public Property usarFade() As Boolean
        Get
            Return iUsarFade
        End Get
        Set(value As Boolean)
            iUsarFade = value
        End Set
    End Property
    Private iUsarFade As Boolean = True

    ''' <summary>
    ''' Acceso al control interno mostrado en el ToolTip para poder
    ''' realizar operaciones de configuración o cambio de estado
    ''' sobre este
    ''' </summary>
    Public ReadOnly Property ControlInterno As Control
        Get
            Dim paraDevolver As Control = Nothing

            Try
                If iToolTip IsNot Nothing Then
                    If iToolTip.PanelContenedor IsNot Nothing AndAlso iToolTip.PanelContenedor.Controls.Count > 0 Then
                        paraDevolver = iToolTip.PanelContenedor.Controls(0)
                    End If
                End If
            Catch ex As Exception                
                paraDevolver = Nothing
            End Try

            Return paraDevolver
        End Get
    End Property
#End Region

#Region " EVENTOS "
    ''' <summary>
    ''' Evento que se lanza cuando se termina de mostrar el ToolTip
    ''' </summary>
    Public Event seMostro()

    ''' <summary>
    ''' Evento que se lanza cuando se termina de ocultar el ToolTip
    ''' </summary>
    Public Event seOculto()
#End Region

#Region " MANEJADORES "
    Private Sub detenerTimerCerrado()
        If iTimerCerrado IsNot Nothing Then iTimerCerrado.Stop()
    End Sub

    Private Sub iniciarTimerCerrado()
        If iTiempoCerrado > 0 AndAlso iTimerCerrado IsNot Nothing Then
            iTimerCerrado.Start()
        End If
    End Sub

    Private Sub cicloTimerCerrado(sender As Object, e As EventArgs)
        Me.Hide()
    End Sub
#End Region

#Region " CONSTRUCTORES "
    Public Sub New()
        InitializeComponent()

        ' De forma predeterminada se activa la animación y el fade
        usarAnimacion = True
        usarFade = True

        ' Se crea el timer de cerrado y se añaden el manejador para el 
        ' cerrado automático del ToolTip
        Try
            iTimerCerrado = New Timer()
            components.Add(iTimerCerrado)
            AddHandler iTimerCerrado.Tick, AddressOf cicloTimerCerrado
        Catch ex As Exception            
        End Try
    End Sub

    Public Sub New(container As IContainer)
        Me.New()
        container.Add(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub
#End Region

#Region " METODOS SHOW "

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    Public Sub Show(ByRef eControlInterno As Control, ByVal eControlAsociado As IWin32Window)
        Show(eControlInterno, eControlAsociado, enmPosicionPico.AbajoIzquierda)
    End Sub

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' prefijando la posición donde se quiere que se muestre el pico del globo
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    ''' <param name="ePosicionPico">Posición donde se quiere mostrar el pico</param>
    Public Sub Show(ByRef eControlInterno As Control, _
                    ByVal eControlAsociado As IWin32Window, _
                    ByVal ePosicionPico As enmPosicionPico)
        Show(eControlInterno, eControlAsociado, 0, 0, ePosicionPico, 0)
    End Sub

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' prefijando las coordenadas donde se quiere mostrar el control
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    ''' <param name="eCoordenadas">Coordenadas donde se quiere mostrar el control</param>
    Public Sub Show(ByRef eControlInterno As Control, _
                    ByVal eControlAsociado As IWin32Window, _
                    ByVal eCoordenadas As Point)
        Show(eControlInterno, eControlAsociado, eCoordenadas.X, eCoordenadas.Y)
    End Sub

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' prefijando las coordenadas donde se quiere mostrar el control
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    ''' <param name="eX">Coordenada X para mostrar el control</param>
    ''' <param name="eY">Coordenada Y para mostrar el control</param>
    Public Sub Show(ByRef eControlInterno As Control, _
                    ByVal eControlAsociado As IWin32Window, _
                    ByVal eX As Integer, _
                    ByVal eY As Integer)
        Show(eControlInterno, eControlAsociado, eX, eY, enmPosicionPico.AbajoIzquierda, 0)
    End Sub

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' prefijando las coordenadas donde se quiere mostrar el control
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    ''' <param name="eCoordenadas">Coordenadas donde se quiere mostrar el control</param>
    ''' <param name="ePosicionPico">Posición donde se quiere mostrar el pico</param>
    ''' <param name="eTiempoCerrado">Tiempo que se tiene que mostrar el tooltip</param>
    Public Sub Show(ByRef eControlInterno As Control, _
                    ByVal eControlAsociado As IWin32Window, _
                    ByVal eCoordenadas As Point, _
                    ByVal ePosicionPico As enmPosicionPico, _
                    ByVal eTiempoCerrado As Integer)
        Show(eControlInterno, eControlAsociado, eCoordenadas.X, eCoordenadas.Y, ePosicionPico, eTiempoCerrado)
    End Sub

    ''' <summary>
    ''' Muestra el control que se le pasa como parámetro en el ToolTip asociado a un control
    ''' prefijando las coordenadas donde se quiere mostrar el control
    ''' </summary>
    ''' <param name="eControlInterno">Control a mostrar en el ToolTip</param>
    ''' <param name="eControlAsociado">Control al que se va a asociar el tooltip</param>
    ''' <param name="eX">Coordenada X para mostrar el control</param>
    ''' <param name="eY">Coordenada Y para mostrar el control</param>
    ''' <param name="ePosicionPico">Posición donde se quiere mostrar el pico</param>
    ''' <param name="eTiempoCerrado">Tiempo que se tiene que mostrar el tooltip</param>
    Public Sub Show(ByRef eControlInterno As Control, _
                    ByVal eControlAsociado As IWin32Window, _
                    ByVal eX As Integer, _
                    ByVal eY As Integer, _
                    ByVal ePosicionPico As enmPosicionPico, _
                    ByVal eTiempoCerrado As Integer)

        ' Si no se le pasa el control a mostrar o el control asociado no
        ' se puede mostrar, por lo que se sale del procedimiento
        If eControlInterno Is Nothing Or eControlAsociado Is Nothing Then            
            Exit Sub
        End If

        ' Se oculta por si ya estuviese visible y se liberan los recursos
        ' que pudiera estár ocupando
        Hide()

        ' Se crea el objeto ToolTip
        iToolTip = New cToolTipInterno(eControlInterno, eControlAsociado, eX, eY, ePosicionPico, usarAnimacion, usarFade)

        ' Si se configuró una duracción, se añaden los manejadores para contorlar el MouseEnter
        ' y el MouseLeave para poder reinciiar el timer de cerrrado y se inica este
        iTiempoCerrado = eTiempoCerrado
        If iTiempoCerrado > 0 AndAlso iTimerCerrado IsNot Nothing Then
            AddHandler iToolTip.MouseEnter, AddressOf detenerTimerCerrado
            AddHandler iToolTip.MouseLeave, AddressOf iniciarTimerCerrado

            ' Se inicializa el timer de cerrado con el intervalo configurado
            iTimerCerrado.Interval = iTiempoCerrado * 1000
            iTimerCerrado.Start()
        End If

        ' Se le da el foco al control
        eControlInterno.Focus()

        ' Se lanza el evento que indica que ya se mostró el control
        RaiseEvent seMostro()
    End Sub

    ''' <summary>
    ''' Oculta el ToolTip liberando todos los recursos que está consumiendo
    ''' </summary>
    Public Sub Hide()
        ' Se detiene el timer si este está activo
        If iTimerCerrado IsNot Nothing Then iTimerCerrado.Stop()

        Dim toolTip As cToolTipInterno = iToolTip
        Dim window As IWin32Window

        If toolTip IsNot Nothing Then
            iToolTip = Nothing
            window = toolTip.iControlAsociado
            toolTip.Dispose()

            ' Se lanza el evento que indica que se finalizó la ocultación
            RaiseEvent seOculto()
        End If
    End Sub
#End Region
End Class
