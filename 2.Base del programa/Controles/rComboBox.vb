Imports ComponentFactory.Krypton.Toolkit

Public Class rComboBox
    Inherits ComponentFactory.Krypton.Toolkit.KryptonComboBox

#Region " DECLARACIONES "
    ''' <summary>
    ''' Botón para limpiar el contenido del control
    ''' </summary>
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

        AddHandler botonBorrar.Click, AddressOf LimpiarControl
        AddHandler ComboBox.GotFocus, AddressOf mostrarBoton
        AddHandler ComboBox.LostFocus, AddressOf mostrarBoton
    End Sub
#End Region

#Region " METODOS SOBRECARGADOS "
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        mostrarBoton(Me, e)
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        mostrarBoton(Me, e)
        MyBase.OnSelectedIndexChanged(e)
    End Sub

    Protected Overrides Sub OnDropDown(ByVal e As System.EventArgs)
        mostrarBoton(Me, e)
        MyBase.OnDropDown(e)
    End Sub
#End Region

#Region " METODOS PUBLICOS "
    Public Sub Clear()
        Text = ""
        SelectedItem = Nothing
        SelectedIndex = -1
    End Sub
#End Region

#Region " METODOS PRIVADOS "
    Public Sub mostrarBoton(ByVal sender As Object, ByVal e As EventArgs)
        botonBorrar.Visible = Me.Visible AndAlso Me.Enabled AndAlso (mostrarSiempreBotonBorrar OrElse (controlarBotonBorrar AndAlso Me.Focused AndAlso (Me.Text <> "" Or Me.SelectedIndex > -1)))

        If botonBorrar.Visible AndAlso Not Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Add(botonBorrar)
        ElseIf Not botonBorrar.Visible AndAlso Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Remove(botonBorrar)
        End If
    End Sub
#End Region
End Class