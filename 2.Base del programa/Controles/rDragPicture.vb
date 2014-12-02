Imports Recompila.Helper

Public Class rDragPicture
    Inherits PictureBox

#Region " ENUMERADOS "
    ''' <summary>
    ''' Que tipo de resalte se utilizará durante el arrastre del control
    ''' </summary>    
    Public Enum tipoResalteDragPicture
        Original = 0
        EscalaGrises = 1
        Transparencia = 2
    End Enum
#End Region

#Region " DECLARACIONES "
    ''' <summary>
    ''' Controla si se está moviendo el control o no
    ''' </summary>    
    Private seMueve As Boolean = False

    ''' <summary>
    ''' Picturebox interna que realmente será la que se moverá por el formulario padre
    ''' </summary>
    Private picMovimiento As PictureBox = Nothing

    ''' <summary>
    ''' Posición inicial del cursor en el momento de comenzar el movimiento
    ''' </summary>
    Private posicionInicial As New Point(0, 0)

    ''' <summary>
    ''' Posición inicial del cursor con respecto a la pantalla
    ''' </summary>
    Private posicionInicialPantalla As New Point(0, 0)
#End Region

#Region " PROPIEDADES "
    ''' <summary>
    ''' Ancho que debe tener el control durante el arrastre. 
    ''' Si se deja en -1 se utilizará el de la imagen del PictureBox
    ''' </summary>
    Public Property Ancho As Long
        Get
            If iAncho <= 0 Then
                If Me.Image IsNot Nothing Then
                    Return Me.Image.Width
                Else
                    Return 0
                End If
            Else
                Return iAncho
            End If
        End Get
        Set(ByVal value As Long)
            iAncho = value
        End Set
    End Property
    Private iAncho As Long = -1

    ''' <summary>
    ''' Alto que debe tener el control durante el arrastre. 
    ''' Si se deja en -1 se utilizará el de la imagen del PictureBox
    ''' </summary>
    Public Property Alto As Long
        Get
            If iAlto <= 0 Then
                If Me.Image IsNot Nothing Then
                    Return Me.Image.Height
                Else
                    Return 0
                End If
            Else
                Return iAlto
            End If
        End Get
        Set(ByVal value As Long)
            iAlto = value
        End Set
    End Property
    Private iAlto As Long = -1

    ''' <summary>
    ''' Cursor que se mostrará durante el arrastre
    ''' </summary>
    Public Overrides Property Cursor As Cursor = Cursors.Hand

    ''' <summary>
    ''' Tipo de resalte que se utilizará durante el arrastre del control
    ''' </summary>
    Public Property TipoResalte As tipoResalteDragPicture = tipoResalteDragPicture.EscalaGrises

    ''' <summary>
    ''' Nivel de transparencia que se le aplicará al resalte si seusa la transparencia
    ''' </summary>
    Public Property Transparencia As Integer = 75
#End Region

#Region " EVENTOS "
    ''' <summary>
    ''' Se produce cuando el control es soltado
    ''' </summary>
    ''' <param name="eVentana">Ventana a la que se está realizando el movimento</param>
    ''' <param name="eDragPicture">El propio control que se estaba arrastrando por el formuario</param>
    ''' <param name="e">MouseEventArgs para realizar cálculos sobre la posición</param>
    Public Event seSuelta(ByVal eVentana As Form, _
                          ByVal eDragPicture As rDragPicture, _
                          ByVal e As System.Windows.Forms.MouseEventArgs)

    ''' <summary>
    ''' Se produce cuando el control se empieza a mover
    ''' </summary>
    ''' <param name="eVentana">Ventana desde la que se está realizando el movimento</param>
    ''' <param name="ePicture">PictureBox que realmente se va a mover sobre el formulario</param>
    Public Event seEmpiezaAMover(ByVal eVentana As Form, ByVal ePicture As PictureBox)
#End Region

#Region " CONSTRUCTORES "
    Public Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
    End Sub
#End Region

#Region " SOBRECARGA FUNCIONES "
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)

        ' Si se esta moviendo es que se tiene pulsado el botón, por lo que si se recibe otro click
        ' se elimina el movimento (boton derecho)
        If seMueve Then
            Dim VentanaContenedora As Form = Me.FindForm
            If VentanaContenedora IsNot Nothing Then
                EliminarControl(VentanaContenedora)
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As System.EventArgs)
        MyBase.OnMouseHover(e)
        Cursor.Current = _Cursor
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim CursorNormal As Boolean = True

        Dim VentanaContenedora As Form = Me.FindForm
        If VentanaContenedora IsNot Nothing Then
            If seMueve AndAlso picMovimiento IsNot Nothing Then
                Dim NuevaPosX As Long = e.X + posicionInicial.X
                Dim NuevaPosY As Long = e.Y + posicionInicial.Y

                Cursor.Current = Cursors.Hand
                CursorNormal = False
                picMovimiento.Location = New Point(NuevaPosX, NuevaPosY)
                picMovimiento.Refresh()
            End If
        End If

        If CursorNormal Then Cursor.Current = Cursors.Arrow

        MyBase.OnMouseMove(e)
    End Sub

    Public Function CalcularLeft(ByVal deQuien As Control) As Long
        If Not TypeOf (deQuien) Is Form Then
            If deQuien.Parent IsNot Nothing Then
                Return (deQuien.Left + CalcularLeft(deQuien.Parent))
            Else
                Return deQuien.Left
            End If
        Else
            Return 0
        End If
    End Function

    Public Function CalcularTop(ByVal deQuien As Control) As Long
        If Not TypeOf (deQuien) Is Form Then
            If deQuien.Parent IsNot Nothing Then
                Return (deQuien.Top + CalcularTop(deQuien.Parent))
            Else
                Return deQuien.Top
            End If
        Else
            Return 0
        End If
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        ' Si todavía no se estaba moviendo se considera que es el primer movimento,
        ' por lo que se crean los objetos y se inicializan las variables
        If Not seMueve Then
            Dim VentanaContenedora As Form = Me.FindForm
            If VentanaContenedora IsNot Nothing Then
                picMovimiento = New PictureBox
                With picMovimiento
                    .Name = "picMovimiento"
                    Select Case TipoResalte
                        Case tipoResalteDragPicture.Original
                            .Image = Me.Image

                        Case tipoResalteDragPicture.EscalaGrises
                            .Image = Imagenes.obtenerEscalaGrises(Me.Image, Me.Image.Width, Me.Image.Height)

                        Case tipoResalteDragPicture.Transparencia
                            .Image = Imagenes.obtenerTransparencia(Me.Image, Me.Transparencia, Me.Image.Width, Me.Image.Height)

                    End Select

                    .Width = Ancho
                    .Height = Alto
                End With

                ' Se añade el nuevo control al formulario donde está contenido
                ' el rDragPicture 
                VentanaContenedora.Controls.Add(picMovimiento)
                picMovimiento.Visible = True
                picMovimiento.Left = CalcularLeft(Me)
                picMovimiento.Top = CalcularTop(Me)

                posicionInicial.X = picMovimiento.Left
                posicionInicial.Y = picMovimiento.Top

                posicionInicialPantalla.X = Me.PointToScreen(e.Location).X - e.X
                posicionInicialPantalla.Y = Me.PointToScreen(e.Location).Y - e.Y

                picMovimiento.BackColor = Color.Transparent
                picMovimiento.SizeMode = PictureBoxSizeMode.StretchImage
                picMovimiento.BringToFront()

                ' Se lanza el evento que indica que se empieza a realizar el movimiento
                ' del control sobre el formulario
                RaiseEvent seEmpiezaAMover(VentanaContenedora, picMovimiento)

                seMueve = True
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)

        Dim VentanaContenedora As Form = Me.FindForm
        If seMueve Then
            If VentanaContenedora IsNot Nothing Then
                ' Se avisa a los Listeners que se ha soltado el control
                ' pasándole información del propio control
                RaiseEvent seSuelta(VentanaContenedora, Me, e)
            End If
        End If

        EliminarControl(VentanaContenedora)
    End Sub
#End Region

#Region " RESTO DE FUNCIONES "
    Private Sub EliminarControl(ByRef VentanaContenedora As Form)
        Try
            If VentanaContenedora IsNot Nothing AndAlso picMovimiento IsNot Nothing Then
                VentanaContenedora.Controls.Remove(picMovimiento)
                picMovimiento.Dispose()
                picMovimiento = Nothing
            End If
        Catch ex As System.Exception
        End Try

        seMueve = False
    End Sub
#End Region

End Class
