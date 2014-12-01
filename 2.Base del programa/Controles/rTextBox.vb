Imports ComponentFactory.Krypton.Toolkit
Imports System.ComponentModel

Public Class rTextBox
    Inherits ComponentFactory.Krypton.Toolkit.KryptonTextBox

#Region " DECLARACIONES "
    ''' <summary>
    ''' Botón para limpiar el contenido del control
    ''' </summary>
    Private botonBorrar As New ButtonSpecAny With {
            .Type = PaletteButtonSpecStyle.Close,
            .Visible = False
        }

    ''' <summary>
    ''' Permite guardar el texto original del control
    ''' </summary>
    Private iTextoOriginal As String = MyBase.Text
#End Region

#Region " PROPIEDADES "
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
    ''' Determina si se tiene que seleccionar todo el texto del control al obtener el foco
    ''' </summary>
    Public Property seleccionarTodo As Boolean = True
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
    End Sub
#End Region


#Region " METODOS SOBRECARGADOS "
    Overloads Sub Clear()
        iTextoOriginal = String.Empty
        MyBase.Clear()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MostrarBoton()
        MyBase.OnTextChanged(e)
    End Sub

    Public Overrides Property Text As String
        Get
            Dim Resultado As String

            If Me.Focused Then
                Resultado = MyBase.Text
            Else
                Resultado = iTextoOriginal
            End If

            If Resultado Is Nothing Then
                Return String.Empty
            Else
                Return Resultado
            End If

        End Get
        Set(ByVal value As String)
            iTextoOriginal = value
        End Set
    End Property

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MostrarBoton()
        If seleccionarTodo Then TextBox.SelectAll()
        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        iTextoOriginal = MyBase.Text
        MostrarBoton()
        MyBase.OnLostFocus(e)
    End Sub
#End Region

#Region " METODOS PRIVADOS "
    Private Sub MostrarBoton()
        botonBorrar.Visible = Me.Enabled AndAlso Not Me.ReadOnly AndAlso Me.Visible AndAlso (mostrarSiempreBotonBorrar OrElse (controlarBotonBorrar AndAlso Me.Focused AndAlso Me.Text <> ""))

        If botonBorrar.Visible AndAlso Not Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Add(botonBorrar)
        ElseIf Not botonBorrar.Visible AndAlso Me.ButtonSpecs.Contains(botonBorrar) Then
            Me.ButtonSpecs.Remove(botonBorrar)
        End If
    End Sub
#End Region
End Class