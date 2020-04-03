Imports DotKakasi

Module Module1

    Sub Main()
        Dim k As New kakasi
        For Each l In k.convert("かな漢字交じり文")
            For Each p In l
                Console.WriteLine(p.Key & " : " & p.Value)
            Next
        Next
        For Each l In k.convert("せーのっ！")
            For Each p In l
                Console.WriteLine(p.Key & " : " & p.Value)
            Next
        Next

        k.setMode("E", "a") ' symbol To ascii, Default: no Conversion
        k.setMode("H", "a") ' Hiragana To ascii, Default: no Conversion
        k.setMode("K", "a") ' Katakana To ascii, Default: no Conversion
        k.setMode("J", "a") ' Japanese To ascii, Default: no Conversion
        k.setMode("r", "Hepburn") ' Default: use Hepburn Roman table
        k.setMode("s", True) ' add space, Default: no separator
        k.setMode("C", True) ' capitalize, Default: no capitalize
        k.initConverter()
        Console.WriteLine(k.do_("かな漢字交じり文"))
        Console.WriteLine(k.do_("せーのっ！"))
        Console.WriteLine(k.do_("（ホﾟクホﾟクホﾟクホﾟクホﾟクホﾟクホﾟ）"))
        Console.WriteLine(k.do_("ЭЮЯ（書記のチカ、書記のチカ、ラー！）"))
    End Sub

End Module
