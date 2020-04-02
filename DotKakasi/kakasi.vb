Imports System.Globalization
Imports DotKakasi.DotKakasi

Public Class kakasi
    Dim _keys() As String = {"J", "H", "K", "E", "a"}
    Dim _values() As String = {"a", "E", "H", "K"}
    Dim _roman_vals() As String = {"Hepburn", "Kunrei", "Passport"}
    Dim _MAXLEN As Integer = 32
    Dim _LONG_SYMBOL() As Integer = {
        &H30FC,   ' ー
        &H2015,   ' ―
        &H2212,   ' −
        &HFF70   ' ｰ
    }
    ' &H002D, _  ' -
    ' &H2010, _  ' ‐
    ' &H2011, _  ' ‑
    ' &H2013, _  ' –
    ' &H2014, _  ' —
    Dim _conv As Dictionary(Of String, IConverter)
    Dim _mode As Dictionary(Of String, String)
    Dim _furi As Dictionary(Of String, Boolean)
    Protected _flag As Dictionary(Of String, Boolean)
    Dim _option As Dictionary(Of String, String)
    Protected _separator As String
    Dim _separator_string As String
    Protected _jconv As J2
    Dim _hahconv As H2
    Dim _hakconv As H2
    Dim _hapconv As H2
    Dim _hkconv As H2
    Dim _khconv As K2
    Dim _kaconv As K2
    Dim _aeconv As A2
    Dim _saconv As Sym2

    Public Sub New()
        _conv = New Dictionary(Of String, IConverter) ' type Dict[str, Union[J2, H2, K2, A2, Sym2]]
        _mode = New Dictionary(Of String, String) From {{"J", Nothing}, {"H", Nothing}, {"K", Nothing}, {"E", Nothing}, {"a", Nothing}}  ' type: Dict[str, Optional[str]]
        _furi = New Dictionary(Of String, Boolean) From {{"J", False}, {"H", False}, {"K", False}, {"E", False}, {"a", False}}  ' type: Dict[str, bool]
        _flag = New Dictionary(Of String, Boolean) From {{"p", False}, {"s", False}, {"f", False}, {"c", False}, {"C", False}, {"U", False},
                      {"u", False}, {"t", True}}  ' type: Dict[str, bool]
        _option = New Dictionary(Of String, String) From {{"r", "Hepburn"}}  ' type: Dict[str, str]
        _separator = " "  ' type: str
        _separator_string = " "  ' type: str
        _jconv = New J2("H")
        _hahconv = New H2("a", "Hepburn")
        _hakconv = New H2("a", "Kunrei")
        _hapconv = New H2("a", "Passport")
        _hkconv = New H2("K")
        _khconv = New K2("H")
        _kaconv = New K2("a")
        _aeconv = New A2("E")
        _saconv = New Sym2("a")
    End Sub

    Public Function convert(Text As String) As List(Of Dictionary(Of String, String))
        Dim _state As Boolean = True

        If Text.Length = 0 Then
            Dim dict = New Dictionary(Of String, String) From {{"orig", ""}, {"kana", ""}, {"hira", ""}, {"hepburn", ""}, {"passport", ""}, {"kunrei", ""}}
            Return New List(Of Dictionary(Of String, String)) From {dict}
        End If

        Dim otext As String = ""
        Dim _result As New List(Of Dictionary(Of String, String))
        Dim i As Integer = 0
        While True
            If i >= Text.Length Then
                Exit While
            End If

            If _jconv.isRegion(Text(i)) Then
                Dim t_ln = _jconv.convert(Text.Substring(i))
                Dim t = t_ln.Item1
                Dim ln = t_ln.Item2
                If ln <= 0 Then      ' pragma: no cover
                    otext = otext + Text(i)
                    i += 1
                    _state = False
                ElseIf i + ln < Text.Length Then
                    If _state Then
                        _result.Add(_iconv(otext + Text.Substring(i, ln), t))
                        otext = ""
                    Else
                        _result.Add(_iconv(otext, otext))
                        _result.Add(_iconv(Text.Substring(i, ln), t))
                        otext = ""
                        _state = True
                    End If
                    i = i + ln
                Else
                    If _state Then
                        _result.Add(_iconv(otext + Text.Substring(i, ln), t))
                    Else      ' pragma: no cover
                        _result.Add(_iconv(otext, otext))
                        _result.Add(_iconv(Text.Substring(i, ln), t))
                    End If
                    Exit While
                End If
            Else
                _state = False
                otext = otext + Text(i)
                i += 1

                If Ch.endmark.Contains(otext(otext.Length - 1)) Then
                    _result.Append(_iconv(otext, otext))
                    otext = ""
                    _state = True
                End If
            End If
        End While
        Return _result
    End Function

    Private Function _iconv(otext As String, hira As String) As Dictionary(Of String, String)
        Dim tmp As New Dictionary(Of String, String)
        tmp.Add("orig", otext)
        tmp.Add("hira", hira)
        tmp.Add("kana", _h2k(hira))
        tmp.Add("hepburn", _s2a(_h2ah(hira)))
        tmp.Add("kunrei", _s2a(_h2ak(hira)))
        tmp.Add("passport", _s2a(_h2ap(hira)))
        Return tmp
    End Function

    Private Function _s2a(Text As String) As String
        Dim result As String = ""
        Dim i As Integer = 0
        While i < Text.Length
            Dim w As Integer = Math.Min(i + _MAXLEN, Text.Length)
            Dim t_l1 = _saconv.convert(Text.Substring(i, w - i))
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 > 0 Then
                result += t
                i += l1
            Else
                result += Text.Substring(i, 1)
                i += 1
            End If
        End While
        Return result
    End Function

    Private Function _h2k(Text As String) As String
        Dim result As String = ""
        Dim i As Integer = 0
        While i < Text.Length
            Dim w As Integer = Math.Min(i + _MAXLEN, Text.Length)
            Dim t_l1 = _hkconv.convert(Text.Substring(i, w - i))
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 > 0 Then
                result += t
                i += l1
            Else
                result += Text.Substring(i, 1)
                i += 1
            End If
        End While
        Return result
    End Function

    Private Function _h2ak(Text As String) As String
        Dim result As String = ""
        Dim i As Integer = 0
        While i < Text.Length
            Dim w As Integer = Math.Min(i + _MAXLEN, Text.Length)
            Dim t_l1 = _hakconv.convert(Text.Substring(i, w - i))
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 > 0 Then
                result += t
                i += l1
            Else
                result += Text.Substring(i, 1)
                i += 1
            End If
        End While
        Return result
    End Function

    Private Function _h2ah(Text As String) As String
        Dim result As String = ""
        Dim i As Integer = 0
        While i < Text.Length
            Dim w As Integer = Math.Min(i + _MAXLEN, Text.Length)
            Dim t_l1 = _hahconv.convert(Text.Substring(i, w - i))
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 > 0 Then
                result += t
                i += l1
            Else
                result += Text.Substring(i, 1)
                i += 1
            End If
        End While
        Return result
    End Function

    Private Function _h2ap(Text As String) As String
        Dim result As String = ""
        Dim i As Integer = 0
        While i < Text.Length
            Dim w As Integer = Math.Min(i + _MAXLEN, Text.Length)
            Dim t_l1 = _hapconv.convert(Text.Substring(i, w - i))
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 > 0 Then
                result += t
                i += l1
            Else
                result += Text.Substring(i, 1)
                i += 1
            End If
        End While
        Return result
    End Function

    Public Sub setMode(fr As String, Optional to_ As String = Nothing)
        If _keys.Contains(fr) Then
            If IsNothing(to_) Then
                _mode(fr) = Nothing
            ElseIf _values.Contains(to_(0)) Then
                _mode(fr) = to_(0)
                If to_.Length = 2 AndAlso to_(1) = "F" Then
                    _furi(fr) = True
                End If
            Else
                Throw New InvalidModeValueException("Invalid value for mode")
            End If
        ElseIf _flag.ContainsKey(fr) Then
            _flag(fr) = to_
            'Throw New InvalidFlagValueException("Invalid flag value")
        ElseIf fr = "r" Then
            If _roman_vals.Contains(to_) Then
                _option("r") = to_
            Else
                Throw New UnsupportedRomanRulesException("Unknown roman table name")
            End If
        ElseIf fr = "S" Then
            _separator = to_
            'Throw New InvalidFlagValueException("Incompatible separator value")
        Else
            Throw New UnknownOptionsException("Unhandled options")  ' pragma: no cover
        End If
    End Sub

    Public Sub initConverter()
        _conv.Clear()
        _conv.Add("J", New J2(_mode("J"), _option("r")))
        _conv.Add("H", New H2(_mode("H"), _option("r")))
        _conv.Add("K", New K2(_mode("K"), _option("r")))
        _conv.Add("E", New Sym2(_mode("E")))
        _conv.Add("a", New A2(_mode("a")))
    End Sub

    Public Function do_(text As String) As String
        Dim otext As String = ""
        Dim i As Integer = 0
        While True
            If i >= text.Length Then
                Exit While
            End If

            Dim mode As String

            If _conv("J").isRegion(text(i)) Then
                mode = "J"
            ElseIf _conv("H").isRegion(text(i)) Then
                mode = "H"
            ElseIf _conv("K").isRegion(text(i)) Then
                mode = "K"
            ElseIf _conv("E").isRegion(text(i)) Then
                mode = "E"
            ElseIf _conv("a").isRegion(text(i)) Then
                mode = "a"
            Else
                mode = "o"
            End If

            Dim orig As String
            Dim chunk As String
            If mode = "J" OrElse mode = "E" Then
                Dim w As Integer = Math.Min(i + _MAXLEN, text.Length)
                Dim t_l1 = _conv(mode).convert(text.Substring(i, w - i))
                Dim t As String = t_l1.Item1
                Dim l1 As Integer = t_l1.Item2

                If l1 > 0 Then
                    orig = text.Substring(i, l1)
                    chunk = t
                    i += l1
                Else
                    orig = text.Substring(i, 1)
                    If _flag("t") Then
                        chunk = orig
                    Else
                        chunk = "???"
                    End If
                    i += 1
                End If

            ElseIf mode = "H" OrElse mode = "K" OrElse mode = "a" Then
                orig = ""
                chunk = ""

                While i < text.Length

                    If _LONG_SYMBOL.Contains(AscW(text(i))) Then

                        ' FIXME: q&d workaround When hiragana/katanaka dash Is first Char.
                        If Not IsNothing(_mode(mode)) AndAlso chunk.Length > 0 Then
                            ' use previous char as a transliteration for kana-dash
                            orig += text(i)
                            chunk = chunk + chunk(chunk.Length - 1)
                            i += 1
                        ElseIf chunk.Length = 0 Then
                            orig += text(i)
                            chunk += "-"
                            i += 1
                            Exit While
                        Else
                            orig += text(i)
                            chunk += text(i)
                            i += 1
                            Exit While
                        End If

                    ElseIf _conv(mode).isRegion(text(i)) Then
                        Dim w As Integer = Math.Min(i + _MAXLEN, text.Length)
                        Dim t_l1 = _conv(mode).convert(text.Substring(i, w - i))
                        Dim t As String = t_l1.Item1
                        Dim l1 As Integer = t_l1.Item2
                        If l1 > 0 Then
                            orig += text.Substring(i, l1)
                            chunk += t
                            i += l1
                        Else
                            orig = text.Substring(i, 1)
                            If _flag("t") Then
                                chunk = orig
                            Else
                                chunk = "???"
                            End If
                            i += 1
                            Exit While
                        End If

                    Else
                        ' i += 1
                        Exit While
                    End If
                End While

            Else
                otext += text(i)
                i += 1
                Continue While
            End If

            If mode = "J" OrElse mode = "E" Then
                If _flag("U") Then
                    chunk = chunk.ToUpper()
                ElseIf _flag("C") Then
                    chunk = New CultureInfo("en-US").TextInfo.ToTitleCase(chunk.ToLower())
                End If
            End If

            If _keys.Contains(mode) AndAlso _furi(mode) Then
                otext += orig + "[" + chunk + "]"
            Else
                otext += chunk
            End If

            ' insert separator When Option specified And it Is Not a last character And Not an End mark
            If _flag("s") AndAlso otext.Substring(otext.Length - _separator.Length) <> _separator AndAlso i < text.Length AndAlso Not Ch.endmark.Contains(text(i)) Then
                otext += _separator
            End If
        End While
        Return otext
    End Function
End Class

Public Class wakati : Inherits kakasi

    Dim _state As Boolean

    Public Sub New()
        _state = True
    End Sub

    Public Overloads Function getConverter() As wakati
        Return Me
    End Function

    Public Overloads Sub setMode(fr As String, Optional to_ As String = Nothing)
        If _flag.ContainsKey(fr) Then
            _flag(fr) = to_
            'Throw New InvalidFlagValueException("Invalid flag value")
            Throw New UnknownOptionsException("Unhandled options")
        End If
    End Sub

    Public Overloads Function do_(text As String) As String

        If text.Length = 0 Then
            Return ""
        End If

        Dim otext As String = ""
        Dim i As Integer = 0
        While True
            If i >= text.Length Then
                Exit While
            End If

            If _jconv.isRegion(text(i)) Then
                Dim _ln = _jconv.convert(text(i))
                Dim ln = _ln.Item2
                If ln <= 0 Then      ' pragma: no cover
                    otext = otext + text(i)
                    i += 1
                    _state = False
                ElseIf i + ln < text.Length Then
                    If _state Then
                        otext = otext + text.Substring(i, ln) + _separator
                    Else
                        otext = otext + _separator + text.Substring(i, ln) + _separator
                        _state = True
                    End If
                    i = i + ln
                Else
                    If _state Then
                        otext = otext + text.Substring(i, ln)
                    Else      ' pragma: no cover
                        otext = otext + _separator + text.Substring(i, ln)
                    End If
                    Exit While
                End If

            Else
                _state = False
                otext = otext + text(i)
                i += 1
            End If
        End While

        Return otext
    End Function
End Class
