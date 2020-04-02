Imports System.IO
Imports System.IO.Compression
Imports System.Resources
Imports Newtonsoft.Json

Namespace DotKakasi
    Public Class J2
        Implements IConverter

        ReadOnly converter As Char

        Dim _cl_table() As String = {"", "aiueow", "aiueow", "aiueow", "aiueow", "aiueow", "aiueow", "aiueow",
                     "aiueow", "aiueow", "aiueow", "k", "g", "k", "g", "k", "g", "k", "g", "k",
                     "g", "s", "zj", "s", "zj", "s", "zj", "s", "zj", "s", "zj", "t", "d", "tc",
                     "d", "aiueokstchgzjfdbpw", "t", "d", "t", "d", "t", "d", "n", "n", "n", "n",
                     "n", "h", "b", "p", "h", "b", "p", "hf", "b", "p", "h", "b", "p", "h", "b",
                     "p", "m", "m", "m", "m", "m", "y", "y", "y", "y", "y", "y", "rl", "rl",
                     "rl", "rl", "rl", "wiueo", "wiueo", "wiueo", "wiueo", "w", "n", "v", "k",
                     "k", "", "", "", "", "", "", "", "", ""}
        Dim _kanwa As Kanwa
        Dim _hconv As H2
        Dim _itaiji As Itaiji

        Public Sub New(Optional mode As Char = "H", Optional method As String = "Hepburn")

            _kanwa = New Kanwa()
            _itaiji = New Itaiji()
            If mode = "H" Then
                converter = "h"
            ElseIf mode = "a" OrElse mode = "K" Then
                _hconv = New H2(mode, method)
                converter = "o" 'nonh
            Else
                converter = "n" 'noop
            End If
        End Sub

        Public Function isRegion(Cha As Char) As Boolean Implements IConverter.isRegion
            If &H3400 <= AscW(Cha) AndAlso AscW(Cha) < &HE000 Then
                Return True
            End If
            If _itaiji.haskey(Cha) Then
                Return True
            End If
            Return False
        End Function

        Public Function convert(Text As String) As Tuple(Of String, Integer) Implements IConverter.convert
            Select Case converter
                Case "h"
                    Return convert_h(Text)
                Case "o"
                    Return convert_nonh(Text)
                Case Else
                    Return convert_noop(Text)
            End Select
        End Function

        Private Function isCletter(l As String, c As String) As Boolean
            If (&H3041 <= AscW(c) AndAlso AscW(c) <= &H309F) AndAlso _cl_table(AscW(c) - &H3040).IndexOf(l) <> -1 Then  ' ぁ:= u\3041
                Return True
            End If
            Return False
        End Function

        Private Function convert_h(ByVal iText As String) As Tuple(Of String, Integer)
            Dim max_len As Integer = 0
            Dim Hstr As String = ""
            Dim Text As String = _itaiji.convert(iText)
            Dim num_vs = iText.Length - Text.Length
            Dim table = _kanwa.load(Text(0))
            If IsNothing(table) Then
                Return Tuple.Create("", 0)
            End If
            For Each kv In table
                Dim k = kv.Key
                Dim v = kv.Value
                Dim length As Integer = k.Length
                If Text.Length >= length Then
                    If Text.StartsWith(k) Then
                        For Each yomi_tail In v
                            Dim yomi = yomi_tail(0)
                            Dim tail = yomi_tail(1)
                            If tail = "" Then
                                If max_len < length Then
                                    Hstr = yomi
                                    max_len = length
                                End If

                            ElseIf max_len < length + 1 AndAlso Text.Length > length AndAlso isCletter(tail, Text(length)) Then
                                Hstr = yomi & Text(length)
                                max_len = length + 1
                            End If
                        Next
                    End If
                End If
            Next
            For i = 0 To num_vs - 1 'when converting string with kanji wit variation selector, calculate max_len again
                If max_len > iText.Length Then
                    Exit For
                ElseIf Text(max_len - 1) <> iText(max_len - 1) Then
                    max_len += 1
                ElseIf max_len < num_vs + Text.Length AndAlso max_len <= iText.Length AndAlso _is_vschr(iText(max_len)) Then
                    max_len += 1
                End If
            Next
            Return Tuple.Create(Hstr, max_len)
        End Function

        Private Function _is_vschr(Cha As Char) As Boolean
            Dim c As Integer = AscW(Cha)
            If &HE0100 <= c AndAlso c <= &HE1EF Then
                Return True
            End If
            If &HFE00 <= c AndAlso c <= &HFE02 Then
                Return True
            End If
            Return False
        End Function

        Private Function convert_nonh(Text As String) As Tuple(Of String, Integer)
            If Not isRegion(Text(0)) Then
                Return Tuple.Create("", 0)
            End If

            Dim t_l1 = convert_h(Text)
            Dim t As String = t_l1.Item1
            Dim l1 As Integer = t_l1.Item2
            If l1 <= 0 Then       ' pragma: no cover
                Return Tuple.Create("", 0)
            End If

            Dim m = 0
            Dim otext = ""

            While True
                If m >= t.Length Then
                    Exit While
                End If
                Dim s_n = _hconv.convert(t.Substring(m))
                Dim s = s_n.Item1
                Dim n = s_n.Item2
                If n <= 0 Then        ' pragma: no cover
                    m = m + 1
                Else
                    m = m + n
                    otext = otext + s
                End If
            End While
            Return Tuple.Create(otext, l1)
        End Function

        Private Function convert_noop(Text As String) As Tuple(Of String, Integer)
            Return Tuple.Create(Text(0).ToString(), 1)
        End Function

    End Class

    Public Class Itaiji
        ' this class Is Borg/Singleton
        Dim _itaijidict As Dictionary(Of String, String) = Nothing
        Dim _lock As New Object

        Public Sub New()
            If IsNothing(_itaijidict) Then
                SyncLock _lock
                    If IsNothing(_itaijidict) Then
                        Dim conf As New Configurations()

                        Using resource As New MemoryStream(My.Resources.ResourceManager.GetObject(Path.GetFileNameWithoutExtension(conf.jisyo_itaiji)), False)
                            Using decompressionStream As GZipStream = New GZipStream(resource, CompressionMode.Decompress)
                                Using textReader As StreamReader = New StreamReader(decompressionStream)
                                    Dim json As String = textReader.ReadToEnd()
                                    _itaijidict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(json)
                                End Using
                            End Using
                        End Using
                    End If
                End SyncLock
            End If
        End Sub

        Public Function haskey(c As Char) As Boolean
            Return _itaijidict.ContainsKey(c)
        End Function

        Public Function convert(Text As String) As String
            Dim Ret As String = ""
            For Each Cha In Text
                If _itaijidict.ContainsKey(Cha) Then
                    Ret = Ret & _itaijidict(Cha)
                Else
                    Ret = Ret & Cha
                End If
            Next
            Return Ret
        End Function
    End Class

    Public Class Kanwa
        ' This Class is Borg/Singleton
        ' It provides same results becase lookup from a Static dictionary.
        ' There is no state rather dictionary dbm.
        Dim _jisyo_table As Dictionary(Of String, Dictionary(Of String, List(Of List(Of String)))) = Nothing
        Dim _lock As New Object

        Public Sub New()
            If IsNothing(_jisyo_table) Then
                SyncLock _lock
                    If IsNothing(_jisyo_table) Then
                        Dim conf As New Configurations()

                        Using resource As New MemoryStream(My.Resources.ResourceManager.GetObject(Path.GetFileNameWithoutExtension(conf.jisyo_kanwa)), False)
                            Using decompressionStream As GZipStream = New GZipStream(resource, CompressionMode.Decompress)
                                Using textReader As StreamReader = New StreamReader(decompressionStream)
                                    Dim json As String = textReader.ReadToEnd()
                                    _jisyo_table = JsonConvert.DeserializeObject(Of Dictionary(Of String, Dictionary(Of String, List(Of List(Of String)))))(json)
                                End Using
                            End Using
                        End Using
                    End If
                End SyncLock
            End If
        End Sub

        Public Function load(Cha As Char) As Dictionary(Of String, List(Of List(Of String)))
            Dim key As String = String.Format("{0:x4}", AscW(Cha))
            If _jisyo_table.ContainsKey(key) Then
                Return _jisyo_table(key)
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace