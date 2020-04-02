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
        Public Shared ReadOnly endmark() As Char = {")", "]", "!", ",", ".", "\u3001", "\u3002"}
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
        {"\u0410", "A"}, {"\u0411", "B"}, {"\u0412", "V"}, ' АБВ
        {"\u0413", "G"}, {"\u0414", "D"}, {"\u0415", "E"}, ' ГДЕ
        {"\u0401", "E"}, {"\u0416", "Zh"}, {"\u0417", "Z"}, ' ЁЖЗ
        {"\u0418", "I"}, {"\u0419", "Y"}, {"\u041a", "K"}, ' ИЙК
        {"\u041b", "L"}, {"\u041c", "M"}, {"\u041d", "N"}, ' ЛМН
        {"\u041e", "O"}, {"\u041f", "P"}, {"\u0420", "R"}, ' ОПР
        {"\u0421", "S"}, {"\u0422", "T"}, {"\u0423", "U"}, ' СТУ
        {"\u0424", "F"}, {"\u0425", "H"}, {"\u0426", "Ts"}, ' ФХЦ
        {"\u0427", "Ch"}, {"\u0428", "Sh"}, {"\u0429", "Sch"}, ' ЧШЩ
        {"\u042a", ""}, {"\u042b", "Y"}, {"\u042c", ""}, ' ЪЫЬ
        {"\u042d", "E"}, {"\u042e", "Yu"}, {"\u042f", "Ya"}, ' ЭЮЯ
        {"\u0430", "a"}, {"\u0431", "b"}, {"\u0432", "v"}, ' абв
        {"\u0433", "g"}, {"\u0434", "d"}, {"\u0435", "e"}, ' где
        {"\u0451", "e"}, {"\u0436", "zh"}, {"\u0437", "z"}, ' ёжз
        {"\u0438", "i"}, {"\u0439", "y"}, {"\u043a", "k"}, ' ийк
        {"\u043b", "l"}, {"\u043c", "m"}, {"\u043d", "n"}, ' лмн
        {"\u043e", "o"}, {"\u043f", "p"}, {"\u0440", "r"}, ' опр
        {"\u0441", "s"}, {"\u0442", "t"}, {"\u0443", "u"}, ' сту
        {"\u0444", "f"}, {"\u0445", "h"}, {"\u0446", "ts"}, ' фхц
        {"\u0447", "ch"}, {"\u0448", "sh"}, {"\u0449", "sch"}, ' чшщ
        {"\u044a", ""}, {"\u044b", "y"}, {"\u044c", ""}, ' ъыь
        {"\u044d", "e"}, {"\u044e", "yu"}, {"\u044f", "ya"}  ' эюя
    }

        Public Shared ReadOnly alpha_table_1() As String = {"\u3000", "\uff01", "\uff02", "\uff03", "\uff04", "\uff05", "\uff06",
                     "\uff07", "\uff08", "\uff09", "\uff0a", "\uff0b", "\uff0c", "\uff0d",
                     "\uff0e", "\uff0f",  ' ！＂＃＄％&＇（）＊＋，－．／
                     "\uff10", "\uff11", "\uff12", "\uff13", "\uff14", "\uff15", "\uff16",
                     "\uff17", "\uff18", "\uff19",  ' ０...９
                     "\uff1a", "\uff1b", "\uff1c", "\uff1d",
                     "\uff1e", "\uff1f", "\uff20"}  ' ：；＜＝＞？＠
        Public Shared ReadOnly alpha_table_2() As String = {"\uff3b", "\uff3c", "\uff3d", "\uff3e", "\uff3f", "\uff40"}  ' ［＼］＾＿｀
        Public Shared ReadOnly alpha_table_3() As String = {"\uff5b", "\uff5c", "\uff5d", "\uff5e"}  ' ｛｜｝～
    End Class
End Namespace
