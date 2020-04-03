Imports System.IO
Imports System.IO.Compression
Imports Newtonsoft.Json

Namespace DotKakasi

    Public Interface IConverter
        Function isRegion(Cha As Char) As Boolean
        Function convert(Text As String) As Tuple(Of String, Integer)
    End Interface

    Public Class H2
        Implements IConverter

        ReadOnly converter As Char

        Dim _kanadict As Jisyo

        Const _diff As Integer = &H30A1 - &H3041  ' KATAKANA LETTER A - HIRAGANA A
        Const _ediff As Integer = &H1B164 - &H1B150

        Public Sub New(mode As Char, Optional method As String = "Hepburn")
            Dim conf = New Configurations()
            If mode = "a" Then
                If method = "Hepburn" Then
                    _kanadict = New Jisyo(conf.jisyo_hepburn_hira)
                ElseIf method = "Passport" Then
                    _kanadict = New Jisyo(conf.jisyo_passport_hira)
                ElseIf method = "Kunrei" Then
                    _kanadict = New Jisyo(conf.jisyo_kunrei_hira)
                Else
                    Throw New UnsupportedRomanRulesException("Unsupported roman rule")
                End If
                converter = "a"
            ElseIf mode = "K" Then
                converter = "K"
            Else
                converter = "n"
            End If
        End Sub

        Public Function isRegion(Cha As Char) As Boolean Implements IConverter.isRegion
            If &H3040 < AscW(Cha) AndAlso AscW(Cha) < &H3097 Then
                Return True
            End If
            If &H1B150 <= AscW(Cha) AndAlso AscW(Cha) <= &H1B152 Then
                Return True
            End If
            Return False
        End Function

        Public Function convert(Text As String) As Tuple(Of String, Integer) Implements IConverter.convert
            Select Case converter
                Case "a"
                    Return convert_a(Text)
                Case "K"
                    Return convert_K(Text)
                Case Else
                    Return convert_noop(Text)
            End Select
        End Function

        Private Function convert_a(Text As String) As Tuple(Of String, Integer)
            Dim Hstr As String = ""
            Dim max_len As Integer = -1
            Dim r As Integer = Math.Min(_kanadict.maxkeylen(), Text.Length)
            For x = 1 To r
                Dim LeftPart As String = Text.Substring(0, x)
                If _kanadict.haskey(LeftPart) Then
                    If max_len < x Then
                        max_len = x
                        Hstr = _kanadict.lookup(LeftPart)
                    End If
                End If
            Next
            Return Tuple.Create(Hstr, max_len)
        End Function

        Private Function convert_K(Text As String) As Tuple(Of String, Integer)
            Dim Hstr As String = ""
            Dim max_len As Integer = -1
            Dim r As Integer = Text.Length
            For x = 0 To r - 1
                Dim c As Integer = AscW(Text(x))
                If &H3040 < c AndAlso c < &H3097 Then
                    Hstr = Hstr + ChrW(c + _diff)
                    max_len += 1
                ElseIf &H1B150 <= c AndAlso c <= &H1B152 Then
                    Hstr = Hstr + ChrW(c + _ediff)
                    max_len += 1
                Else          ' pragma: no cover
                    Exit For
                End If
            Next
            Return Tuple.Create(Hstr, max_len)
        End Function

        Private Function convert_noop(Text As String) As Tuple(Of String, Integer)
            Return Tuple.Create(Text(0).ToString(), 1)
        End Function
    End Class

    Public Class K2
        Implements IConverter

        ReadOnly converter As Char

        Dim _kanadict As Jisyo

        Const _diff As Integer = &H30A1 - &H3041  ' KATAKANA LETTER A - HIRAGANA A
        Const _ediff As Integer = &H1B164 - &H1B150

        Public Sub New(mode As Char, Optional method As String = "Hepburn")
            Dim conf = New Configurations()
            If mode = "a" Then
                If method = "Hepburn" Then
                    _kanadict = New Jisyo(conf.jisyo_hepburn)
                ElseIf method = "Passport" Then
                    _kanadict = New Jisyo(conf.jisyo_passport)
                ElseIf method = "Kunrei" Then
                    _kanadict = New Jisyo(conf.jisyo_kunrei)
                Else
                    Throw New UnsupportedRomanRulesException("Unsupported roman rule")  ' pragma: no cover
                End If

                converter = "a"
            ElseIf mode = "H" Then
                converter = "h"
            Else
                converter = "n"
            End If
        End Sub

        Public Function isRegion(Cha As Char) As Boolean Implements IConverter.isRegion
            If &H30A0 < AscW(Cha) AndAlso AscW(Cha) < &H30FD Then
                Return True
            End If
            If &H1B164 <= AscW(Cha) AndAlso AscW(Cha) <= &H1B167 Then
                Return True
            End If
            Return False
        End Function

        Public Function convert(Text As String) As Tuple(Of String, Integer) Implements IConverter.convert
            Select Case converter
                Case "a"
                    Return convert_a(Text)
                Case "h"
                    Return convert_h(Text)
                Case Else
                    Return convert_noop(Text)
            End Select
        End Function

        Private Function convert_a(Text As String) As Tuple(Of String, Integer)
            Dim Hstr As String = ""
            Dim max_len As Integer = -1
            Dim r As Integer = Math.Min(_kanadict.maxkeylen(), Text.Length)
            For x = 1 To r
                Dim LeftPart As String = Text.Substring(0, x)
                If _kanadict.haskey(LeftPart) Then
                    If max_len < x Then
                        max_len = x
                        Hstr = _kanadict.lookup(LeftPart)
                    End If
                End If
            Next
            Return Tuple.Create(Hstr, max_len)
        End Function

        Private Function convert_h(Text As String) As Tuple(Of String, Integer)
            Dim Hstr As String = ""
            Dim max_len As Integer = -1
            Dim r As Integer = Text.Length
            For x = 0 To r - 1
                Dim c As Integer = AscW(Text(x))
                If &H1B164 <= c AndAlso c < &H1B167 Then
                    Hstr = Hstr + ChrW(c - _ediff)
                    max_len += 1
                ElseIf c = &H1B167 Then
                    Hstr = Hstr + ChrW(&H3093)
                    max_len += 1
                ElseIf &H30A0 < c AndAlso c < &H30F7 Then
                    Hstr = Hstr + ChrW(c - _diff)
                    max_len += 1
                ElseIf &H30F7 <= c AndAlso c < &H30FD Then
                    Hstr = Hstr + Text(x)
                    max_len += 1
                Else             ' pragma: no cover
                    Exit For
                End If
            Next
            Return Tuple.Create(Hstr, max_len)
        End Function

        Private Function convert_noop(Text As String) As Tuple(Of String, Integer)
            Return Tuple.Create(Text(0).ToString(), 1)
        End Function
    End Class

    Public Class Jisyo
        Dim _dict As Dictionary(Of String, String)
        Dim _max_key_len_ As Integer

        Public Sub New(dictname As String)
            Using resource As New MemoryStream(My.Resources.ResourceManager.GetObject(Path.GetFileNameWithoutExtension(dictname)), False)
                Using decompressionStream As GZipStream = New GZipStream(resource, CompressionMode.Decompress)
                    Using textReader As StreamReader = New StreamReader(decompressionStream)
                        Dim json As String = textReader.ReadToEnd()
                        _dict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(json)

                        For Each k In _dict.Keys
                            Dim keylen As Integer = k.Length
                            If _max_key_len_ < keylen Then
                                _max_key_len_ = keylen
                            End If
                        Next
                    End Using
                End Using
            End Using
        End Sub

        Public Function haskey(key As String) As Boolean
            Return _dict.ContainsKey(key)
        End Function

        Public Function lookup(key As String) As String
            Return _dict(key)
        End Function

        Public Function maxkeylen() As Integer
            Return _max_key_len_
        End Function
    End Class

    Public Class Sym2
        Implements IConverter

        ReadOnly converter As Char

        Public Sub New(mode As Char)
            If mode = "a" Then
                converter = "a"
            Else
                converter = "n"
            End If
        End Sub

        Public Function isRegion(Cha As Char) As Boolean Implements IConverter.isRegion
            Dim c = AscW(Cha)
            Return (Ch.ideographic_space <= c AndAlso c <= Ch.postal_mark_face) OrElse
               (Ch.wavy_dash <= c AndAlso c <= Ch.ideographic_half_fill_space) OrElse
               (Ch.greece_Alpha_Upper <= c AndAlso c <= Ch.greece_Rho) OrElse (Ch.greece_Sigma_Upper <= c AndAlso c <= Ch.greece_Omega_Upper) OrElse
               (Ch.greece_alpha_Lower <= c AndAlso c <= Ch.greece_omega_Lower) OrElse
               (Ch.cyrillic_A <= c AndAlso c <= Ch.cyrillic_ya) OrElse
               (Ch.zenkaku_exc_mark <= c AndAlso c <= Ch.zenkaku_number_nine) OrElse
               (&HFF20 <= c AndAlso c <= &HFF5E) OrElse c = &H451 OrElse c = &H401
        End Function

        Public Function convert(Text As String) As Tuple(Of String, Integer) Implements IConverter.convert
            Select Case converter
                Case "a"
                    Return convert_a(Text)
                Case Else
                    Return convert_noop(Text)
            End Select
        End Function

        Private Function _convert(Text As String) As String
            Dim c = AscW(Text(0))
            If Ch.ideographic_space <= c AndAlso c <= Ch.postal_mark_face Then
                Return Convert_Tables.symbol_table_1(c - Ch.ideographic_space)
            ElseIf Ch.wavy_dash <= c AndAlso c <= Ch.ideographic_half_fill_space Then
                Return Convert_Tables.symbol_table_2(c - Ch.wavy_dash)
            ElseIf Ch.greece_Alpha_Upper <= c AndAlso c <= Ch.greece_Omega_Upper Then
                Return Convert_Tables.symbol_table_3(c - Ch.greece_Alpha_Upper)
            ElseIf Ch.greece_alpha_Lower <= c AndAlso c <= Ch.greece_omega_Lower Then
                Return Convert_Tables.symbol_table_4(c - Ch.greece_alpha_Lower)
            ElseIf Ch.cyrillic_A <= c AndAlso c <= Ch.cyrillic_ya Then
                Return Convert_Tables.cyrillic_table(Text(0))
            ElseIf c = Ch.cyrillic_E_Upper OrElse c = Ch.cyrillic_e_Lower Then
                Return Convert_Tables.cyrillic_table(Text(0))
            ElseIf Ch.zenkaku_exc_mark <= c AndAlso c <= Ch.zenkaku_slash_mark Then
                Return Convert_Tables.symbol_table_5(c - Ch.zenkaku_exc_mark)
            ElseIf Ch.zenkaku_number_zero <= c AndAlso c <= Ch.zenkaku_number_nine Then
                Return ChrW(c - Ch.zenkaku_number_zero + AscW("0"))
            ElseIf &HFF20 <= c AndAlso c <= &HFF40 Then
                Return ChrW(&H41 + c - &HFF21)  ' u\ff21Ａ => u\0041:@A..Z[\]^_`
            ElseIf &HFF41 <= c AndAlso c < &HFF5F Then
                Return ChrW(&H61 + c - &HFF41)  ' u\ff41ａ => u\0061:a..z{|}
            Else
                Return ""  ' pragma: no cover
            End If
        End Function

        Private Function convert_a(Text As String) As Tuple(Of String, Integer)
            Dim t As String = _convert(Text)
            If Not String.IsNullOrEmpty(t) Then
                Return Tuple.Create(t, 1)
            Else
                Return Tuple.Create("", 0)
            End If
        End Function

        Private Function convert_noop(Text As String) As Tuple(Of String, Integer)
            Return Tuple.Create(Text(0).ToString(), 1)
        End Function
    End Class

    Public Class A2
        Implements IConverter

        ReadOnly converter As Char

        Public Sub New(mode As Char)
            If mode = "E" Then
                converter = "E"
            Else
                converter = "n"
            End If
        End Sub

        Public Function isRegion(Cha As Char) As Boolean Implements IConverter.isRegion
            Dim c = AscW(Cha)
            Return Ch.space <= c AndAlso c < Ch.delete
        End Function

        Public Function convert(Text As String) As Tuple(Of String, Integer) Implements IConverter.convert
            Select Case converter
                Case "E"
                    Return convert_E(Text)
                Case Else
                    Return convert_noop(Text)
            End Select
        End Function

        Private Function _convert(Text As String) As String
            Dim c = AscW(Text(0))
            If Ch.space <= c AndAlso c <= Ch.at_mark Then
                Return Convert_Tables.alpha_table_1(c - Ch.space)
            ElseIf Ch.alphabet_A_Upper <= c AndAlso c <= Ch.alphabet_Z_Upper Then
                Return ChrW(Ch.zenkaku_A_Upper + c - Ch.alphabet_A_Upper)  ' u\0041A => u\ff21Ａ
            ElseIf Ch.square_bra <= c AndAlso c <= Ch.back_quote Then
                Return Convert_Tables.alpha_table_2(c - Ch.square_bra)
            ElseIf Ch.alphabet_a_Lower <= c AndAlso c <= Ch.alphabet_z_Lower Then
                Return ChrW(Ch.zenkaku_a_Lower + c - Ch.alphabet_a_Lower)  ' u\0061a => u\ff41ａ
            ElseIf Ch.bracket_bra <= c AndAlso c <= Ch.tilda Then
                Return Convert_Tables.alpha_table_3(c - Ch.bracket_bra)
            Else
                Return ""  ' pragma: no cover
            End If
        End Function

        Private Function convert_E(Text As String) As Tuple(Of String, Integer)
            Dim t As String = _convert(Text)
            If t.Length > 0 Then
                Return Tuple.Create(t, 1)
            Else
                Return Tuple.Create("", 0)
            End If
        End Function

        Private Function convert_noop(Text As String) As Tuple(Of String, Integer)
            Return Tuple.Create(Text(0).ToString(), 1)
        End Function
    End Class
End Namespace

