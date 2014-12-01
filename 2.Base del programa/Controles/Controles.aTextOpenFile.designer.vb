<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
        Partial Class aTextOpenFile
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(aTextOpenFile))
        Me.txtRuta = New Controles.aTextBox()
        Me.btnAbrir = New ComponentFactory.Krypton.Toolkit.ButtonSpecAny()
        Me.SuspendLayout()
        '
        'txtRuta
        '
        Me.txtRuta.ButtonSpecs.AddRange(New ComponentFactory.Krypton.Toolkit.ButtonSpecAny() {Me.btnAbrir})
        Me.txtRuta.controlarBotonBorrar = True
        Me.txtRuta.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtRuta.Location = New System.Drawing.Point(0, 0)
        Me.txtRuta.mostrarSiempreBotonBorrar = False
        Me.txtRuta.Name = "txtRuta"
        Me.txtRuta.ReadOnly = True
        Me.txtRuta.seleccionarTodo = True
        Me.txtRuta.Size = New System.Drawing.Size(213, 21)
        Me.txtRuta.TabIndex = 0
        '
        'btnAbrir
        '
        Me.btnAbrir.Image = CType(resources.GetObject("btnAbrir.Image"), System.Drawing.Image)
        Me.btnAbrir.UniqueName = "68C4EB7C1130489330A9703CDEB9BAC3"
        '
        'aTextOpenFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtRuta)
        Me.Name = "aTextOpenFile"
        Me.Size = New System.Drawing.Size(213, 24)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtRuta As Controles.aTextBox
    Friend WithEvents btnAbrir As ComponentFactory.Krypton.Toolkit.ButtonSpecAny


Public ReadOnly Property losComponentes As System.ComponentModel.ComponentCollection
    Get
        If Me.components IsNot Nothing Then
            Return Me.components.Components
        else
            Return Nothing
        End If
    End Get
End Property

End Class