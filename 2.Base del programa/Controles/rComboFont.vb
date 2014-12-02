Public Class rComboFont
    Inherits rComboBox

#Region " CONSTRUCTORES "
    Public Sub New()
        ' Se le indica al sistema que se va controlar el dibuajado de los elementos del combo
        Me.ComboBox.DrawMode = DrawMode.OwnerDrawVariable
        AddHandler Me.ComboBox.DrawItem, AddressOf DibujarFuente
        CargarFuentesSistema()

        ' Se selecciona la fuente Verdada de forma predeterminada si esta existe
        If Me.ComboBox.FindString("Verdana") > -1 Then Me.ComboBox.SelectedIndex = Me.ComboBox.FindString("Verdana")
    End Sub
#End Region

#Region " CONTROL DEL COMBO "
    ''' <summary>
    ''' Obtiene las fuentes instaladas en el sistema y las carga en el combobox
    ''' </summary>
    Private Sub CargarFuentesSistema()
        ' Se eliminan los datos previos si estos existían
        If Me.ComboBox.Items.Count > 0 Then
            For i As Integer = 0 To Me.ComboBox.Items.Count - 1
                CType(Me.ComboBox.Items(i), Font).Dispose()
            Next

            Me.ComboBox.Items.Clear()
        End If

        ' Se obtiene el listado de fuentes disponibles en el sistema
        For Each fuente As FontFamily In FontFamily.Families
            Dim f As Font = Nothing

            ' Se crean las fuentas en función de los estilos disponibles
            If (fuente.IsStyleAvailable(FontStyle.Regular)) Then
                f = New Font(fuente.Name, Me.ComboBox.Font.Size)
            ElseIf (fuente.IsStyleAvailable(FontStyle.Bold)) Then
                f = New Font(fuente.Name, Me.ComboBox.Font.Size, FontStyle.Bold)
            ElseIf (fuente.IsStyleAvailable(FontStyle.Italic)) Then
                f = New Font(fuente.Name, Me.ComboBox.Font.Size, FontStyle.Italic)
            ElseIf (fuente.IsStyleAvailable(FontStyle.Underline)) Then
                f = New Font(fuente.Name, Me.ComboBox.Font.Size, FontStyle.Underline)
            End If

            ' Se añade la fuente al combo
            If f IsNot Nothing AndAlso Not Me.ComboBox.Items.Contains(f) Then Me.ComboBox.Items.Add(f)
        Next

        ' Se le indica al combo cual es la propiedad que se debe mostrar
        Me.ComboBox.DisplayMember = "FontFamily.Name"
    End Sub

    ''' <summary>
    ''' Se dibuja cada fuente de forma independiente
    ''' </summary>
    ''' <param name="sender">Objeto que desencadena el evento. Por defecto es el combobox </param>
    ''' <param name="e">Parámetros de la llamada a la función</param>
    Private Sub DibujarFuente(ByVal sender As Object, ByVal e As DrawItemEventArgs)
        ' Si el índice del elemento no es válido no se puede dibujar
        If ((e.Index = -1) OrElse (e.Index >= Me.ComboBox.Items.Count)) Then Exit Sub

        ' Se dibuja el fondo del elemento
        e.DrawBackground()

        ' Se comprueba si hay que dibujar el cuadro de selección y si es así se dibuja
        If (e.State = DrawItemState.Focus) Then e.DrawFocusRectangle()

        ' Se crean los objetos necesarios para el dibujado de la fuenta y se dibuja
        Dim pincel As Brush = Nothing
        Try
            pincel = New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(CType(Me.ComboBox.Items(e.Index), Font).Name, CType(Me.ComboBox.Items(e.Index), Font), pincel, e.Bounds)
        Finally
            pincel.Dispose()
            pincel = Nothing
        End Try
    End Sub
#End Region
End Class
