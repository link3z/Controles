Public Class rComboIcon
    Inherits rComboBox

#Region " PROPIEDADES "
    ''' <summary>
    ''' Image list con las imágenes que se tienen que cargar con los elementos cargados en el combo
    ''' </summary>
    Public Property ImageList() As ImageList
        Get
            Return iImageList
        End Get
        Set(ByVal ListaImagem As ImageList)
            iImageList = ListaImagem
        End Set
    End Property
    Private iImageList As New ImageList

    ''' <summary>
    ''' Determina si se tiene que mostrar el borde del control
    ''' </summary>
    Public Property conBorde As Boolean = True

    ''' <summary>
    ''' Objeto realmente seleccionado, el cual se encuentra albergado en el objeto rComboBoxIconItem
    ''' </summary>
    Public ReadOnly Property SelectedItemReal As Object
        Get
            If MyBase.SelectedItem IsNot Nothing AndAlso TypeOf (MyBase.SelectedItem) Is rComboBoxIconItem Then
                Return CType(MyBase.SelectedItem, rComboBoxIconItem).Item
            Else
                Return Nothing
            End If
        End Get
    End Property
#End Region

#Region " CONSTRUCTORES "
    Public Sub New()
        Me.ComboBox.DrawMode = DrawMode.OwnerDrawFixed
        AddHandler Me.ComboBox.DrawItem, AddressOf MetodoDibujoItem

        If conBorde Then
            Me.StateCommon.ComboBox.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True
        Else
            Me.StateCommon.ComboBox.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False
        End If
    End Sub
#End Region

#Region " DIBJADO DE LOS ITEMS "
    ''' <summary>
    ''' Método que se encarga del dibujado de los items del control
    ''' </summary>
    Protected Sub MetodoDibujoItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)
        Me.SuspendLayout()

        Try
            If conBorde Then
                Me.StateCommon.ComboBox.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True
            Else
                Me.StateCommon.ComboBox.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False
            End If

            e.DrawBackground()
            e.DrawFocusRectangle()

            Dim item As New rComboBoxIconItem
            Dim imageSize As New Size
            imageSize = iImageList.ImageSize

            Dim bounds As New Rectangle
            bounds = e.Bounds

            If e.Index > -1 Then
                Try
                    item = Me.Items(e.Index)
                    If (item.ImageIndex <> -1) Then
                        Me.ImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex)
                        e.Graphics.DrawString(item.ToString, e.Font, New SolidBrush(e.ForeColor), bounds.Left + imageSize.Width, bounds.Top)
                    Else
                        e.Graphics.DrawString(item.ToString, e.Font, New SolidBrush(e.ForeColor), bounds.Left, bounds.Top)
                    End If
                Catch ex As System.Exception
                    If (e.Index <> -1) Then
                        e.Graphics.DrawString(Items(e.Index).ToString(), e.Font, New SolidBrush(e.ForeColor), bounds.Left, bounds.Top)
                    Else
                        e.Graphics.DrawString(Text, e.Font, New SolidBrush(e.ForeColor), bounds.Left, bounds.Top)
                    End If
                End Try
            End If
        Catch ex As Exception
        End Try

        Me.ResumeLayout()
    End Sub
#End Region
End Class

''' <summary>
''' Objetos que se pueden cargar en el rComboIcon
''' </summary>
Public Class rComboBoxIconItem
#Region " PROPIEDADES "
    ''' <summary>
    ''' Item albargado en el objeto
    ''' </summary>
    Property Item() As Object
        Get
            Return iItem
        End Get
        Set(ByVal Value As Object)
            iItem = Value
        End Set
    End Property
    Private iItem As Object

    ''' <summary>
    ''' Indice de la imagen asociada al objeto
    ''' </summary>
    Property ImageIndex() As Integer
        Get
            Return iImageIndex
        End Get

        Set(ByVal Value As Integer)
            iImageIndex = Value
        End Set
    End Property
    Private iImageIndex As Integer
#End Region

#Region " CONSTRUCTORES "
    Public Sub New()
        iItem = Nothing
    End Sub

    Public Sub New(ByVal eItem As Object)
        iItem = eItem
    End Sub

    Public Sub New(ByVal eItem As Object, ByVal imageIndex As Integer)
        iItem = eItem
        iImageIndex = imageIndex
    End Sub
#End Region

#Region " SOBRECARGAS "
    Public Overrides Function ToString() As String
        If iItem IsNot Nothing Then
            Return iItem.ToString
        Else
            Return ""
        End If
    End Function
#End Region
End Class