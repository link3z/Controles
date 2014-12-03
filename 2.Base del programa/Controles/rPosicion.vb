Public Class rPosicion

#Region " PROPIEDADES "
    ''' <summary>
    ''' Numero a mostrar en el control
    ''' </summary>
    Public Property Numero As String
        Get
            Return iNumero
        End Get
        Set(value As String)
            iNumero = value
            lblNumero.Text = iNumero
        End Set
    End Property
    Private iNumero As String = "1"

    ''' <summary>
    ''' Título a mostrar en el control
    ''' </summary>
    Public Property Titulo As String
        Get
            Return iTitulo
        End Get
        Set(value As String)
            iTitulo = value
            lblTitulo.Text = iTitulo
        End Set
    End Property
    Private iTitulo As String = "Título"

    ''' <summary>
    ''' Alineación del título
    ''' </summary>
    Public Property AlieancionTitulo As System.Drawing.ContentAlignment
        Get
            Return iAlineacionTitulo
        End Get
        Set(value As System.Drawing.ContentAlignment)
            iAlineacionTitulo = value
            lblTitulo.TextAlign = iAlineacionTitulo
        End Set
    End Property
    Private iAlineacionTitulo As System.Drawing.ContentAlignment = ContentAlignment.BottomCenter

    ''' <summary>
    ''' Color del objeto
    ''' </summary>
    Public Property Color As Color
        Get
            Return iColor
        End Get
        Set(value As Color)
            iColor = value
            aplicarColor()
        End Set
    End Property
    Private iColor As Color = Color.FromArgb(66, 66, 66)
#End Region

#Region " CONTROL GRAFICO "
    ''' <summary>
    ''' Aplica los colores a los objetos que componen el control
    ''' </summary>
    Private Sub aplicarColor()
        lblTitulo.StateCommon.TextColor = iColor
        lblNumero.StateCommon.TextColor = iColor
        Invalidate()
    End Sub

    ''' <summary>
    ''' Dibuja el control
    ''' </summary>
    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Dim lapiz As New Pen(iColor, 6)

        e.Graphics.DrawEllipse(lapiz, 5, 25, 40, 40)
        e.Graphics.DrawLine(lapiz, 45, 45, 160, 45)
        e.Graphics.FillEllipse(New SolidBrush(iColor), 157, 41, 7, 7)

        lapiz.Dispose()
    End Sub
#End Region
End Class
