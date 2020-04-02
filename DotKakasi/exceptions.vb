Namespace DotKakasi
    Public Class PyKakasiException
        Inherits Exception

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    Public Class UnknownCharacterException
        Inherits PyKakasiException

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    Public Class UnsupportedRomanRulesException
        Inherits PyKakasiException

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    Public Class UnknownOptionsException
        Inherits PyKakasiException

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    Public Class InvalidModeValueException
        Inherits PyKakasiException

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    Public Class InvalidFlagValueException
        Inherits PyKakasiException

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

End Namespace
