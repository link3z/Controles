<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rCheckOnOff
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.kgrContenedor = New ComponentFactory.Krypton.Toolkit.KryptonGroup()
        Me.btnEstado = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        CType(Me.kgrContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.kgrContenedor.Panel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.kgrContenedor.Panel.SuspendLayout()
        Me.kgrContenedor.SuspendLayout()
        Me.SuspendLayout()
        '
        'kgrContenedor
        '
        Me.kgrContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.kgrContenedor.Location = New System.Drawing.Point(0, 0)
        Me.kgrContenedor.Name = "kgrContenedor"
        Me.kgrContenedor.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Blue
        '
        'kgrContenedor.Panel
        '
        Me.kgrContenedor.Panel.Controls.Add(Me.btnEstado)
        Me.kgrContenedor.Size = New System.Drawing.Size(64, 22)
        Me.kgrContenedor.StateCommon.Back.Color1 = System.Drawing.Color.Transparent
        Me.kgrContenedor.StateCommon.Border.DrawBorders = CType((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top Or ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) _
            Or ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) _
            Or ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right), ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)
        Me.kgrContenedor.StateCommon.Border.Rounding = 1
        Me.kgrContenedor.StateCommon.Border.Width = 1
        Me.kgrContenedor.TabIndex = 2
        '
        'btnEstado
        '
        Me.btnEstado.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEstado.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnEstado.Location = New System.Drawing.Point(0, 0)
        Me.btnEstado.Name = "btnEstado"
        Me.btnEstado.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue
        Me.btnEstado.Size = New System.Drawing.Size(34, 20)
        Me.btnEstado.StateCommon.Back.Color1 = System.Drawing.SystemColors.ActiveCaption
        Me.btnEstado.TabIndex = 0
        Me.btnEstado.Values.Text = ""
        '
        'aOnOff
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.kgrContenedor)
        Me.Name = "aOnOff"
        Me.Size = New System.Drawing.Size(64, 22)
        CType(Me.kgrContenedor.Panel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.kgrContenedor.Panel.ResumeLayout(False)
        CType(Me.kgrContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.kgrContenedor.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents kgrContenedor As ComponentFactory.Krypton.Toolkit.KryptonGroup
    Friend WithEvents btnEstado As ComponentFactory.Krypton.Toolkit.KryptonButton

End Class
