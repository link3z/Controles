Imports ComponentFactory.Krypton.Toolkit

Public Class rDateTimePicker
    Inherits ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker

#Region " ENUMERADOS "
    ''' <summary>
    ''' Que se tiene que limpiar al hacer click sobre el botón borrar
    ''' </summary>
    Public Enum TiposLimpieza
        Value
        ValueNullable
        ValueYValueNullable
    End Enum
#End Region

#Region " DECLARACIONES "
    Private botonBorrar As New ButtonSpecAny With {
            .Type = PaletteButtonSpecStyle.Close,
            .Visible = False
        }
#End Region

#Region " PROPIEDADES "
    ''' <summary>
    ''' Determina si se tiene que controlar el botón borrar
    ''' </summary>
    Public Property controlarBotonBorrar As Boolean = True

    ''' <summary>
    ''' Determina si el botón borrar se muestra siempre
    ''' </summary>
    Public Property mostrarSiempreBotonBorrar As Boolean = False

    ''' <summary>
    ''' Determina si se tiene que limpiar el control al pulsar el botón borrar
    ''' </summary>
    Public Property limpiarAlPulsarBoton As Boolean = True

    ''' <summary>
    ''' Tipo de limpieza que se tiene que aplicar al pulsar sobre el botón borrar
    ''' </summary>
    Public Property tipoLimpieza As TiposLimpieza = TiposLimpieza.ValueYValueNullable

#End Region

#Region " EVENTOS "
    ''' <summary>
    ''' Evento que se lanza cuando se pulsa sobre el botón borrar
    ''' </summary>
    Public Event pulsoBotonBorrar()
#End Region

#Region " MANEJADORES "
    ''' <summary>
    ''' Limpia el contenido del control al pulsar el botón
    ''' </summary>
    Private Sub LimpiarControl(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If limpiarAlPulsarBoton Then Clear()

        ' Se lanza el evento indicando que se pulso sobre el botón borrar
        RaiseEvent pulsoBotonBorrar()
    End Sub
#End Region

#Region " CONSTRUCTORES "
    Public Sub New()
        MyBase.New()

        Me.CalendarTodayText = "Hoy:"
        Me.CustomNullText = "(Sin fecha)"
        AddHandler botonBorrar.Click, AddressOf LimpiarControl
    End Sub
#End Region

#Region " METODOS SOBRECARGADOS "
    Protected Overrides Sub OnLayout(ByVal levent As System.Windows.Forms.LayoutEventArgs)
        mostrarBoton(Me, Nothing)
        MyBase.OnLayout(levent)
    End Sub
#End Region

#Region " METODOS PUBLICOS "
    Public Sub Clear()
        If tipoLimpieza = TiposLimpieza.Value OrElse tipoLimpieza = TiposLimpieza.ValueYValueNullable Then
            Me.Value = DateTime.Now
        End If

        If tipoLimpieza = TiposLimpieza.ValueNullable OrElse tipoLimpieza = TiposLimpieza.ValueYValueNullable Then
            Me.ValueNullable = Nothing
        End If
    End Sub
#End Region

#Region " METODOS PRIVADOS "
    Public Sub mostrarBoton(ByVal sender As Object, ByVal e As EventArgs)
        botonBorrar.Visible = Me.Enabled AndAlso Me.Visible AndAlso (mostrarSiempreBotonBorrar OrElse (controlarBotonBorrar AndAlso Me.Focused AndAlso (Me.Text <> "" Or Not IsDBNull(Me.ValueNullable))))

        If botonBorrar.Visible AndAlso Not Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Add(botonBorrar)
        ElseIf Not botonBorrar.Visible AndAlso Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Remove(botonBorrar)
        End If
    End Sub
#End Region
End Class