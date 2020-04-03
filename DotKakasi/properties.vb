Imports System.IO

Namespace DotKakasi
    ' This class is Borg
    Public Class Configurations
        Public ReadOnly jisyo_hepburn_hira As String = "hepburnhira.gz"
        Public ReadOnly jisyo_passport_hira As String = "passporthira.gz"
        Public ReadOnly jisyo_kunrei_hira As String = "kunreihira.gz"
        Public ReadOnly jisyo_itaiji As String = "itaijidict.gz"
        Public ReadOnly jisyo_kanwa As String = "kanwadict.gz"
        Public ReadOnly jisyo_hepburn As String = "hepburndict.gz"
        Public ReadOnly jisyo_passport As String = "passportdict.gz"
        Public ReadOnly jisyo_kunrei As String = "kunreidict.gz"
    End Class

    Public Class Ch
        Public Shared ReadOnly space As Integer = &H20
        Public Shared ReadOnly at_mark As Integer = &H40
        Public Shared ReadOnly alphabet_A_Upper As Integer = &H41
        Public Shared ReadOnly alphabet_Z_Upper As Integer = &H5A
        Public Shared ReadOnly square_bra As Integer = &H5B
        Public Shared ReadOnly back_quote As Integer = &H60
        Public Shared ReadOnly alphabet_a_Lower As Integer = &H61
        Public Shared ReadOnly alphabet_z_Lower As Integer = &H7A
        Public Shared ReadOnly bracket_bra As Integer = &H7B
        Public Shared ReadOnly tilda As Integer = &H7E
        Public Shared ReadOnly delete As Integer = &H7F
        Public Shared ReadOnly ideographic_space As Integer = &H3000
        Public Shared ReadOnly postal_mark_face As Integer = &H3020
        Public Shared ReadOnly wavy_dash As Integer = &H3030
        Public Shared ReadOnly ideographic_half_fill_space As Integer = &H303F
        Public Shared ReadOnly greece_Alpha_Upper As Integer = &H391
        Public Shared ReadOnly greece_Rho As Integer = &H30A1
        Public Shared ReadOnly greece_Sigma_Upper As Integer = &H30A3
        Public Shared ReadOnly greece_Omega_Upper As Integer = &H3A9
        Public Shared ReadOnly greece_alpha_Lower As Integer = &H3B1
        Public Shared ReadOnly greece_omega_Lower As Integer = &H3C9
        Public Shared ReadOnly cyrillic_A As Integer = &H410
        Public Shared ReadOnly cyrillic_E_Upper As Integer = &H401
        Public Shared ReadOnly cyrillic_e_Lower As Integer = &H451
        Public Shared ReadOnly cyrillic_ya As Integer = &H44F
        Public Shared ReadOnly zenkaku_exc_mark As Integer = &HFF01
        Public Shared ReadOnly zenkaku_slash_mark As Integer = &HFF0F
        Public Shared ReadOnly zenkaku_number_zero As Integer = &HFF10
        Public Shared ReadOnly zenkaku_number_nine As Integer = &HFF1A
        Public Shared ReadOnly zenkaku_A_Upper As Integer = &HFF21
        Public Shared ReadOnly zenkaku_a_Lower As Integer = &HFF41
        Public Shared ReadOnly endmark() As Char = {")", "]", "!", ",", ".", ChrW(&H3001), ChrW(&H3002)}
    End Class

    Public Class Convert_Tables
        'convert symbols To alphabet
        'based on Original KAKASI's EUC_JP - alphabet converter table
        '--------------------------------------------------------------------------
        ' a1 a0 | 　 、 。 ， . ・ ： ； ？ ! ゛ ゜ ´ ｀ ¨
        '         " ",",",".",",",".",".",":",";","?",
        '         "!","\"","(maru)","'","`","..",
        ' a1 b0 | ^ ￣ ＿ ヽ ヾ ゝ ゞ 〃 仝 々 〆 〇 ー ― ‐ /
        '       "~","~","_","(kurikaesi)","(kurikaesi)","(kurikaesi)",
        '       "(kurikaesi)","(kurikaesi)","(kurikaesi)","(kurikaesi)",
        '       "sime","(maru)","^","-","-","/",
        ' a1 c0 | \ ～ ∥ ｜ … ‥ ' ’ “ ” （ ） 〔 〕 ［ ］
        '      "\\","~","||","|","...","..","`","'","\"","\"","(",")","[","]","[","]",
        '      "{","}","<",">","<<",">>","(",")","(",")","(",")","+","-","+-","X",
        ' a1 d0 | ｛ ｝ 〈 〉 《 》 「 」 『 』 【 】 + - ± ×

        ' a1 e0 | ÷ = ≠ <> ≦ ≧ ∞ ∴ ♂ ♀ ° ′ ″ ℃ ￥
        '      "/","=","!=","<",">","<=",">=","(kigou)","...",
        '      "(osu)","(mesu)","(do)","'","\"","(Sessi)","\\",
        ' a1 f0 | ＄ ￠ ￡ ％ ＃ ＆ ＊ ＠ § ☆ ★ ○ ● ◎ ◇
        '      "$","(cent)","(pound)","%","'","&","*","@",
        '      "(setu)","(hosi)","(hosi)","(maru)","(maru)","(maru)","(diamond)"
        '---------------------------------------------------------------------------

        '----------------------------------------------------------
        ' a2 a0 | ◆ □ ■ △ ▲ ▽ ▼ ※ 〒 → ← ↑ ↓ 〓
        ' a2 b0 | ∈ ∋ ⊆ ⊇ ⊂ ⊃ a2 c0 | ∪ ∩ ∧ ∨ ￢ ⇒ ⇔ ∀
        ' a2 d0 | ∃ ∠ ⊥ ⌒ ∂
        ' a2 e0 | ∇ ≡ ≒ ≪ ≫ √ ∽ ∝ ∵ ∫ ∬
        ' a2 f0 | Å ‰ ♯ ♭ ♪ † ‡ ¶ ◯
        '----------------------------------------------------------

        'Greek convertion table
        '----------------------------------------------------------
        '   "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta",
        '   "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho",
        '   "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega",
        '   "", "", "", "", "", "", "", "",
        '   "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta",
        '   "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho",
        '   "sigma", "tau", "upsilon", "phi", "chi", "psi", "omega"
        '----------------------------------------------------------

        ' U3000 - 301F
        ' \u3000、。〃〄〇〆々〈〉《》「」『』【】〒〓〔〕〖〗〘〙
        ' 〚〛〜〝〞〟〠
        Public Shared ReadOnly symbol_table_1() As String = {" ", ",", ".", """", "(kigou)", "(kurikaesi)", "(sime)", "(maru)", "<", ">",
                      "<<", ">>", "(", ")", "(", ")", "(", ")", "(kigou)", "(geta)",
                      "(", ")", "(", ")", "(", ")", "(", ")", "~", "(kigou)", """",
                      "(kigou)", "(kigou)"}
        ' U3030 - 3040
        ' 〰〱〲〳〴〵〶〷〼〽〾〿
        Public Shared ReadOnly symbol_table_2() As String = {"-", "(kurikaesi)",
                      "(kurikaesi)", "(kurikaesi)", "(kurikaesi)", "(kurikaesi)",
                      "(kigou)", "XX", Nothing, Nothing, Nothing, Nothing, "(masu)", "(kurikaesi)", " ", " "}
        ' U0391-03A9
        Public Shared ReadOnly symbol_table_3() As String = {"Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta",
                      "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", Nothing,
                      "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega"}
        ' U03B1-03C9
        Public Shared ReadOnly symbol_table_4() As String = {"alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta",
                      "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "final sigma",
                      "sigma", "tau", "upsilon", "phi", "chi", "psi", "omega"}
        ' UFF01-FF0F
        Public Shared ReadOnly symbol_table_5() As String = {"!", """", "'", "$", "%", "&", "'", "(", ")", "*", "+",
                      ",", "-", ".", "/"}
        ' cyriilic
        Public Shared ReadOnly cyrillic_table As New Dictionary(Of Char, String) From {  ' basic cyrillic characters
        {ChrW(&H410), "A"}, {ChrW(&H411), "B"}, {ChrW(&H412), "V"}, ' АБВ
        {ChrW(&H413), "G"}, {ChrW(&H414), "D"}, {ChrW(&H415), "E"}, ' ГДЕ
        {ChrW(&H401), "E"}, {ChrW(&H416), "Zh"}, {ChrW(&H417), "Z"}, ' ЁЖЗ
        {ChrW(&H418), "I"}, {ChrW(&H419), "Y"}, {ChrW(&H41A), "K"}, ' ИЙК
        {ChrW(&H41B), "L"}, {ChrW(&H41C), "M"}, {ChrW(&H41D), "N"}, ' ЛМН
        {ChrW(&H41E), "O"}, {ChrW(&H41F), "P"}, {ChrW(&H420), "R"}, ' ОПР
        {ChrW(&H421), "S"}, {ChrW(&H422), "T"}, {ChrW(&H423), "U"}, ' СТУ
        {ChrW(&H424), "F"}, {ChrW(&H425), "H"}, {ChrW(&H426), "Ts"}, ' ФХЦ
        {ChrW(&H427), "Ch"}, {ChrW(&H428), "Sh"}, {ChrW(&H429), "Sch"}, ' ЧШЩ
        {ChrW(&H42A), ""}, {ChrW(&H42B), "Y"}, {ChrW(&H42C), ""}, ' ЪЫЬ
        {ChrW(&H42D), "E"}, {ChrW(&H42E), "Yu"}, {ChrW(&H42F), "Ya"}, ' ЭЮЯ
        {ChrW(&H430), "a"}, {ChrW(&H431), "b"}, {ChrW(&H432), "v"}, ' абв
        {ChrW(&H433), "g"}, {ChrW(&H434), "d"}, {ChrW(&H435), "e"}, ' где
        {ChrW(&H451), "e"}, {ChrW(&H436), "zh"}, {ChrW(&H437), "z"}, ' ёжз
        {ChrW(&H438), "i"}, {ChrW(&H439), "y"}, {ChrW(&H43A), "k"}, ' ийк
        {ChrW(&H43B), "l"}, {ChrW(&H43C), "m"}, {ChrW(&H43D), "n"}, ' лмн
        {ChrW(&H43E), "o"}, {ChrW(&H43F), "p"}, {ChrW(&H440), "r"}, ' опр
        {ChrW(&H441), "s"}, {ChrW(&H442), "t"}, {ChrW(&H443), "u"}, ' сту
        {ChrW(&H444), "f"}, {ChrW(&H445), "h"}, {ChrW(&H446), "ts"}, ' фхц
        {ChrW(&H447), "ch"}, {ChrW(&H448), "sh"}, {ChrW(&H449), "sch"}, ' чшщ
        {ChrW(&H44A), ""}, {ChrW(&H44B), "y"}, {ChrW(&H44C), ""}, ' ъыь
        {ChrW(&H44D), "e"}, {ChrW(&H44E), "yu"}, {ChrW(&H44F), "ya"}  ' эюя
    }

        Public Shared ReadOnly alpha_table_1() As String = {ChrW(&H3000), ChrW(&HFF01), ChrW(&HFF02), ChrW(&HFF03), ChrW(&HFF04), ChrW(&HFF05), ChrW(&HFF06),
                     ChrW(&HFF07), ChrW(&HFF08), ChrW(&HFF09), ChrW(&HFF0A), ChrW(&HFF0B), ChrW(&HFF0C), ChrW(&HFF0D),
                     ChrW(&HFF0E), ChrW(&HFF0F),  ' ！＂＃＄％&＇（）＊＋，－．／
                     ChrW(&HFF10), ChrW(&HFF11), ChrW(&HFF12), ChrW(&HFF13), ChrW(&HFF14), ChrW(&HFF15), ChrW(&HFF16),
                     ChrW(&HFF17), ChrW(&HFF18), ChrW(&HFF19),  ' ０...９
                     ChrW(&HFF1A), ChrW(&HFF1B), ChrW(&HFF1C), ChrW(&HFF1D),
                     ChrW(&HFF1E), ChrW(&HFF1F), ChrW(&HFF20)}  ' ：；＜＝＞？＠
        Public Shared ReadOnly alpha_table_2() As String = {chrw(&hff3b), chrw(&hff3c), chrw(&hff3d), chrw(&hff3e), chrw(&hff3f), chrw(&hff40)}  ' ［＼］＾＿｀
        Public Shared ReadOnly alpha_table_3() As String = {ChrW(&HFF5B), ChrW(&HFF5C), ChrW(&HFF5D), ChrW(&HFF5E)}  ' ｛｜｝～
    End Class
End Namespace
