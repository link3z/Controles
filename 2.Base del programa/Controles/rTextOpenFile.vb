Imports Recompila.Helper

Public Class rTextOpenFile

#Region " DECLARACIONES "
    ''' <summary>
    ''' Finalidad del control
    ''' </summary>
    Public Enum AperturaTipo
        OpenFile = 0
        SaveFile = 1
        SelectFolder = 2
        SelectFolderLocal = 3
        SelectFolderNetwork = 4
    End Enum
#End Region

#Region " PROPIEDADES "
    ''' <summary>
    ''' Como se va a compoartar el control al pulsar sobre el botón de apertura
    ''' </summary>
    Public Property Apertura As AperturaTipo = AperturaTipo.OpenFile

    ''' <summary>
    ''' Ruta inicial donde se va a situar el control de apertura
    ''' </summary>
    Public Property RutaInicial As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments

    ''' <summary>
    ''' Archivos que se van a poder abrir
    ''' </summary>
    Public Property ExtensionesArchivo As String = "*.*|*.*"

    ''' <summary>
    ''' Nombre del archivo a abrir
    ''' </summary>
    Public Property NombreArchivo As String = ""
#End Region

#Region " EVENTOS "
    ''' <summary>
    ''' Evento que se lanza cada vez que se detecta un cambio en la ruta del control
    ''' </summary>
    ''' <param name="eRuta">Nueva ruta cargada</param>
    Public Event CambioRuta(ByVal eRuta As String)
#End Region

#Region " MANEJADORES "
    Private Sub txtRuta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRuta.TextChanged
        RaiseEvent CambioRuta(txtRuta.Text)
    End Sub
#End Region

#Region " SOBRECARGAS "
    Overloads Property text As String
        Get
            Return Me.txtRuta.Text
        End Get
        Set(ByVal value As String)
            Me.txtRuta.Text = value
        End Set
    End Property

    Public ReadOnly Property StateCommon As ComponentFactory.Krypton.Toolkit.PaletteInputControlTripleRedirect
        Get
            Return Me.txtRuta.StateCommon
        End Get
    End Property
#End Region

#Region " APERTURA DE DOCUMENTOS "
    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click
        If Apertura = AperturaTipo.OpenFile Then
            txtRuta.Text = Ficheros.Buscar.buscarArchivo("", RutaInicial, ExtensionesArchivo, NombreArchivo)
        ElseIf Apertura = AperturaTipo.SaveFile Then
            txtRuta.Text = Ficheros.Buscar.BuscarArchivoGuardar("", RutaInicial, ExtensionesArchivo, NombreArchivo)
        ElseIf Apertura = AperturaTipo.SelectFolder Then
            txtRuta.Text = Ficheros.Buscar.buscarDirectorio(RutaInicial)
        ElseIf Apertura = AperturaTipo.SelectFolderLocal Then
            txtRuta.Text = Ficheros.Buscar.buscarDirectorioLocal(RutaInicial)
        ElseIf Apertura = AperturaTipo.SelectFolderNetwork Then
            txtRuta.Text = Ficheros.Buscar.buscarDirectorioRed(RutaInicial)
        End If
    End Sub
#End Region
End Class
