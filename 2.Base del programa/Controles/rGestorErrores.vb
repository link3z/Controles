Imports System.ComponentModel
Imports ComponentFactory.Krypton.Toolkit

''' <summary>
''' Gestor de errores personalizao
''' </summary>
Public Class rGestorErrores
    Inherits System.Windows.Forms.ErrorProvider

#Region " EVENTOS "
    <Browsable(True)> _
    Public Event CountChanged()
#End Region

#Region " PROPIEDADES "    

    ''' <summary>
    ''' Indica el número de controles con errores
    ''' </summary>
    Public ReadOnly Property Count() As Integer
        Get
            Return Controles.Count
        End Get
    End Property

    ''' <summary>
    ''' Obtiene una lista con todos los controles que contienen errores de validación
    ''' </summary>
    Public ReadOnly Property Controles As List(Of Control)
        Get
            If iControlesConErrores Is Nothing Then
                iControlesConErrores = New List(Of Control)
            End If            
            Return iControlesConErrores
        End Get
    End Property
    Private iControlesConErrores As New List(Of Control)

    ''' <summary>
    ''' Indica si existe actualmente algún control con errores
    ''' </summary>
    Public ReadOnly Property HasErrors() As Boolean
        Get
            Return Count > 0
        End Get
    End Property

    ''' <summary>
    ''' Alineación del icono de error
    ''' </summary>
    Public Property Alineacion As ErrorIconAlignment = ErrorIconAlignment.MiddleLeft
#End Region

#Region " GESTIÓN DE CONTROLES CON ERRORES "
    ''' <summary>
    ''' Fija un error a un determinado control
    ''' </summary>
    ''' <param name="eControl">Control al que se le va a fijar un error</param>
    ''' <param name="eError">Mensaje que se tiene que mostrar en la información de error</param>
    Public Shadows Sub SetError(ByVal eControl As System.Windows.Forms.Control, _
                                ByVal eError As String)
        Dim Aux As Integer = Controles.IndexOf(eControl)
        MyBase.SetError(eControl, eError)
        pintarError(eControl, Not String.IsNullOrEmpty(eError))

        If String.IsNullOrEmpty(eError) Then
            If Aux >= 0 Then
                Controles.RemoveAt(Aux)
                RaiseEvent CountChanged()
            End If
        ElseIf Aux = -1 Then
            MyBase.SetIconAlignment(eControl, Alineacion)
            Controles.Add(eControl)
            RaiseEvent CountChanged()
        End If

    End Sub

    ''' <summary>
    ''' Limpia todos los errores
    ''' </summary>
    Public Shadows Sub Clear()
        Controles.Clear()
        MyBase.Clear()
        RaiseEvent CountChanged()
    End Sub
#End Region

#Region " PINTAR COLORES "
    ''' <summary>
    ''' Modifica el control para que se muestre o se oculte el error
    ''' </summary>
    ''' <param name="eControl">Control que se está controlando</param>
    ''' <param name="eConError">Si hay un error lo pinta con los colores de error, en caso contrario lo pinta normal</param>
    Public Sub pintarError(ByVal eControl As Object, _
                           ByVal eConError As Boolean)
        If TypeOf (eControl) Is KryptonTextBox Then
            If eConError Then
                CType(eControl, KryptonTextBox).StateCommon.Border.Color1 = Sistema.Presentacion.Errores._KO_CUADRO
                CType(eControl, KryptonTextBox).StateCommon.Back.Color1 = Sistema.Presentacion.Errores._KO_RELLENO
            Else
                CType(eControl, KryptonTextBox).StateCommon.Border.Color1 = Nothing
                CType(eControl, KryptonTextBox).StateCommon.Back.Color1 = Nothing
            End If
        ElseIf TypeOf (eControl) Is KryptonComboBox Then
            If eConError Then
                CType(eControl, KryptonComboBox).BackColor = Sistema.Presentacion.Errores._KO_RELLENO
                CType(eControl, KryptonComboBox).StateCommon.ComboBox.Border.Color1 = Sistema.Presentacion.Errores._KO_CUADRO
                CType(eControl, KryptonComboBox).StateCommon.ComboBox.Back.Color1 = Sistema.Presentacion.Errores._KO_RELLENO
            Else
                CType(eControl, KryptonComboBox).BackColor = Nothing
                CType(eControl, KryptonComboBox).StateCommon.ComboBox.Border.Color1 = Nothing
                CType(eControl, KryptonComboBox).StateCommon.ComboBox.Back.Color1 = Nothing
            End If
        ElseIf TypeOf (eControl) Is rTextOpenFile Then
            If eConError Then
                CType(eControl, rTextOpenFile).txtRuta.StateCommon.Border.Color1 = Sistema.Presentacion.Errores._KO_CUADRO
                CType(eControl, rTextOpenFile).txtRuta.StateCommon.Back.Color1 = Sistema.Presentacion.Errores._KO_RELLENO
            Else
                CType(eControl, rTextOpenFile).txtRuta.StateCommon.Border.Color1 = Nothing
                CType(eControl, rTextOpenFile).txtRuta.StateCommon.Back.Color1 = Nothing
            End If
        End If
    End Sub
#End Region
End Class