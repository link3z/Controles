Public Class rCheckOnOff
#Region " PROPIEDADES "
    ''' <summary>
    ''' Determina si el control está en seleccionado o no
    ''' </summary>
    Public Property Checked As Boolean
        Get
            Return iChecked
        End Get
        Set(value As Boolean)
            iChecked = value
            pintarControl()
        End Set
    End Property
    Private iChecked As Boolean = False

    ''' <summary>
    ''' Color que se mostrará en el fondo al estar Checked = true
    ''' </summary>
    Public Property ColorON As Color
        Get
            Return iColorOn
        End Get
        Set(value As Color)
            iColorOn = value
        End Set
    End Property
    Private iColorOn As Color = Sistema.Presentacion.Errores._OK_CUADRO

    ''' <summary>
    ''' Color que se mostrará en el fondo al no estar Checked = False
    ''' </summary>
    Public Property ColorOff As Color
        Get
            Return iColorOff
        End Get
        Set(value As Color)
            iColorOff = value
        End Set
    End Property
    Private iColorOff As Color = Sistema.Presentacion.Errores._KO_CUADRO
#End Region

#Region " EVENTOS "
    ''' <summary>
    ''' Evento que se lanza cuando se produce un cambio en el estado
    ''' </summary>
    Public Event Checked_Changed(ByVal eSender As Object, ByVal eChecked As Boolean)
#End Region

#Region " CONSTRUCTORES "
    Private Sub aOnOff_Load(sender As Object, e As EventArgs) Handles Me.Load
        pintarControl()
    End Sub
#End Region

#Region " CONTROLES "
    Private Sub btnEstado_Click(sender As Object, e As EventArgs) Handles btnEstado.Click
        Me.Checked = Not Me.Checked
        RaiseEvent Checked_Changed(Me, iChecked)
    End Sub
#End Region

#Region " METODOS INTERNOS "
    ''' <summary>
    ''' Pinta el control en función del estado del Check
    ''' </summary>
    Private Sub pintarControl()
        If iChecked Then
            kgrContenedor.StateCommon.Back.Color1 = iColorOn
            kgrContenedor.StateCommon.Back.Color2 = iColorOn
            btnEstado.Dock = DockStyle.Right
        Else
            kgrContenedor.StateCommon.Back.Color1 = iColorOff
            kgrContenedor.StateCommon.Back.Color2 = iColorOff
            btnEstado.Dock = DockStyle.Left
        End If
    End Sub
#End Region
End Class
