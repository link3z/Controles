Imports System.Drawing
Imports System.Collections.Generic

Namespace Sistema
    Namespace Presentacion
        Namespace Errores
            Public Module modSistemaPresentacionErrores
#Region " PROPIEDADES "
                ' Color utilizado para indicar que hay error
                Public _KO_CUADRO As Color = Color.Red
                Public _KO_RELLENO As Color = Color.FromArgb(251, 214, 220)
                Public _KO_MATRIZ As Single() = {1.0, 0.0, 0.0, 0, 0}

                ' Color utilizado para indicar que no hay error
                Public _OK_CUADRO As Color = Color.DarkGreen
                Public _OK_RELLENO As Color = Color.FromArgb(239, 250, 180)
                Public _OK_MATRIZ As Single() = {0.0, 0.13, 0.0, 0, 0}
#End Region
            End Module
        End Namespace
    End Namespace
End Namespace
