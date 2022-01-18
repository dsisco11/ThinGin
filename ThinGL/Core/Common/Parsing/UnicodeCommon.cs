using System;

namespace ThinGin.Core.Common
{
    public static class UnicodeCommon
    {/* https://en.wikipedia.org/wiki/List_of_Unicode_characters */
        #region Constants

        #region /* Specials */
        public const int CHAR_UNICODE_MAX = 0x10FFFF;
        public const char CHAR_MAX = char.MaxValue;
        public const char EOF = '\u0000';
        public const char CHAR_NULL = '\u0000';
        public const char CHAR_REPLACEMENT = '\uFFFD';// U+FFFD
        #endregion


        #region /* Ascii whitespace */
        /* Docs: https://infra.spec.whatwg.org/#ascii-whitespace */
        public const char CHAR_TAB = '\u0009';
        /// <summary>
        /// Newline (\n)
        /// </summary>
        public const char CHAR_LINE_FEED = '\u000A';
        /// <summary>
        /// Form Feed
        /// </summary>
        public const char CHAR_FORM_FEED = '\u000C';
        /// <summary>
        /// Carriage Return (\r)
        /// </summary>
        public const char CHAR_CARRIAGE_RETURN = '\u000D';
        /// <summary>
        /// " "
        /// </summary>
        public const char CHAR_SPACE = '\u0020';
        #endregion


        #region /* C0 Control codes */
        /* Docs: https://infra.spec.whatwg.org/#c0-control */
        /// <summary>
        /// C0 Control code: INFORMATION SEPARATOR ONE
        /// </summary>
        public const char CHAR_C0_INFO_SEPERATOR = '\u001F';
        public const char CHAR_C0_DELETE = '\u007F';
        public const char CHAR_C0_APPLICATION_PROGRAM_COMMAND = '\u009F';
        #endregion


        #region /* Ascii Symbols */

        /// <summary>
        /// !
        /// </summary>
        public const char CHAR_EXCLAMATION_POINT = '\u0021';
        /// <summary>
        /// ?
        /// </summary>
        public const char CHAR_QUESTION_MARK = '\u003F';
        /// <summary>
        /// @
        /// </summary>
        public const char CHAR_AT_SIGN = '\u0040';
        /// <summary>
        /// $
        /// </summary>
        public const char CHAR_DOLLAR_SIGN = '\u0024';
        /// <summary>
        /// &
        /// </summary>
        public const char CHAR_AMPERSAND = '\u0026';
        /// <summary>
        /// *
        /// </summary>
        public const char CHAR_ASTERISK = '\u002A';
        /// <summary>
        /// ^
        /// </summary>
        public const char CHAR_CARET = '\u005E';
        /// <summary>
        /// `
        /// </summary>
        public const char CHAR_BACKTICK = '\u0060';
        /// <summary>
        /// ~
        /// </summary>
        public const char CHAR_TILDE = '\u007E';
        /// <summary>
        /// |
        /// </summary>
        public const char CHAR_PIPE = '\u007C';
        /// <summary>
        /// =
        /// </summary>
        public const char CHAR_EQUALS = '\u003D';

        /// <summary>
        /// </summary>
        public const char CHAR_LEFT_CHEVRON = '\u003C';
        /// <summary>
        /// </summary>
        public const char CHAR_RIGHT_CHEVRON = '\u003E';

        /// <summary>
        /// {
        /// </summary>
        public const char CHAR_LEFT_CURLY_BRACKET = '\u007B';
        /// <summary>
        /// }
        /// </summary>
        public const char CHAR_RIGHT_CURLY_BRACKET = '\u007D';

        /// <summary>
        /// [
        /// </summary>
        public const char CHAR_LEFT_SQUARE_BRACKET = '\u005B';
        /// <summary>
        /// ]
        /// </summary>
        public const char CHAR_RIGHT_SQUARE_BRACKET = '\u005D';

        /// <summary>
        /// (
        /// </summary>
        public const char CHAR_LEFT_PARENTHESES = '\u0028';
        /// <summary>
        /// )
        /// </summary>
        public const char CHAR_RIGHT_PARENTHESES = '\u0029';
        #endregion


        #region /* Ascii digits */
        /* Docs: https://infra.spec.whatwg.org/#ascii-digit */
        public const char CHAR_DIGIT_0 = '\u0030';
        public const char CHAR_DIGIT_1 = '\u0031';
        public const char CHAR_DIGIT_2 = '\u0032';
        public const char CHAR_DIGIT_3 = '\u0033';
        public const char CHAR_DIGIT_4 = '\u0034';
        public const char CHAR_DIGIT_5 = '\u0035';
        public const char CHAR_DIGIT_6 = '\u0036';
        public const char CHAR_DIGIT_7 = '\u0037';
        public const char CHAR_DIGIT_8 = '\u0038';
        public const char CHAR_DIGIT_9 = '\u0039';

        public static ReadOnlySpan<char> ASCII_DIGITS => new char[] { CHAR_DIGIT_0, CHAR_DIGIT_1, CHAR_DIGIT_2, CHAR_DIGIT_3, CHAR_DIGIT_4, CHAR_DIGIT_5, CHAR_DIGIT_6, CHAR_DIGIT_7, CHAR_DIGIT_8, CHAR_DIGIT_9 };
        #endregion


        #region Ascii Upper Alpha
        /* Docs: https://infra.spec.whatwg.org/#ascii-upper-alpha */
        public const char CHAR_A_UPPER = '\u0041';
        public const char CHAR_B_UPPER = '\u0042';
        public const char CHAR_C_UPPER = '\u0043';
        public const char CHAR_D_UPPER = '\u0044';
        public const char CHAR_E_UPPER = '\u0045';
        public const char CHAR_F_UPPER = '\u0046';
        public const char CHAR_G_UPPER = '\u0047';
        public const char CHAR_H_UPPER = '\u0048';
        public const char CHAR_M_UPPER = '\u004D';
        public const char CHAR_P_UPPER = '\u0050';
        public const char CHAR_S_UPPER = '\u0053';
        public const char CHAR_T_UPPER = '\u0054';
        public const char CHAR_U_UPPER = '\u0055';
        public const char CHAR_V_UPPER = '\u0056';
        public const char CHAR_W_UPPER = '\u0057';
        public const char CHAR_X_UPPER = '\u0058';
        public const char CHAR_Y_UPPER = '\u0059';
        public const char CHAR_Z_UPPER = '\u005A';
        #endregion


        #region Ascii Lower Alpha
        /* Docs: https://infra.spec.whatwg.org/#ascii-lower-alpha */
        public const char CHAR_A_LOWER = '\u0061';
        public const char CHAR_B_LOWER = '\u0062';
        public const char CHAR_C_LOWER = '\u0063';
        public const char CHAR_D_LOWER = '\u0064';
        public const char CHAR_E_LOWER = '\u0065';
        public const char CHAR_F_LOWER = '\u0066';
        public const char CHAR_G_LOWER = '\u0067';
        public const char CHAR_H_LOWER = '\u0068';
        public const char CHAR_M_LOWER = '\u006D';
        public const char CHAR_S_LOWER = '\u0073';
        public const char CHAR_T_LOWER = '\u0074';
        public const char CHAR_U_LOWER = '\u0075';
        public const char CHAR_V_LOWER = '\u0076';
        public const char CHAR_W_LOWER = '\u0077';
        public const char CHAR_X_LOWER = '\u0078';
        public const char CHAR_Y_LOWER = '\u0079';
        public const char CHAR_Z_LOWER = '\u007A';
        #endregion


        #region /* Common */
        /// <summary>
        /// "
        /// </summary>
        public const char CHAR_QUOTATION_MARK = '\u0022';
        /// <summary>
        /// '
        /// </summary>
        public const char CHAR_APOSTRAPHE = '\u0027';
        /// <summary>
        /// +
        /// </summary>
        public const char CHAR_PLUS_SIGN = '\u002B';
        /// <summary>
        /// %
        /// </summary>
        public const char CHAR_PERCENT = '\u0025';
        /// <summary>
        /// -
        /// </summary>
        public const char CHAR_HYPHEN_MINUS = '\u002D';
        /// <summary>
        /// _
        /// </summary>
        public const char CHAR_UNDERSCORE = '\u005F';
        /// <summary>
        /// .
        /// </summary>
        public const char CHAR_FULL_STOP = '\u002E';
        /// <summary>
        /// /
        /// </summary>
        public const char CHAR_SOLIDUS = '\u002F';
        /// <summary>
        /// \
        /// </summary>
        public const char CHAR_REVERSE_SOLIDUS = '\u005C';
        /// <summary>
        /// #
        /// </summary>
        public const char CHAR_HASH = '\u0023';

        /// <summary>
        /// ,
        /// </summary>
        public const char CHAR_COMMA = '\u002C';
        /// <summary>
        /// :
        /// </summary>
        public const char CHAR_COLON = '\u003A';
        /// <summary>
        /// ;
        /// </summary>
        public const char CHAR_SEMICOLON = '\u003B';

        /// <summary>
        /// &nbsp
        /// </summary>
        public const char CHAR_NBSP = '\u00A0';
        #endregion

        #endregion

        #region Modifier Keys
        /* These are the character representations for common keys */
        public const char KEY_CTRL_MODIFIER = '⌃';
        public const char KEY_ALT_MODIFIER = '⌥';
        public const char KEY_SHIFT_MODIFIER = '⇧';
        public const char KEY_META_MODIFIER = '⌘';

        public const char KEY_ESCAPE = '⎋';
        public const char KEY_BACKSPACE = '⌫';
        public const char KEY_CAPSLOCK = '⇪';
        public const char KEY_ENTER = '↵';
        public const char KEY_TAB = '⇥';
        public const char KEY_SPACE = ' ';

        public const char KEY_DELETE = '⌦';
        public const char KEY_END = '↘';
        public const char KEY_INSERT = '↖';
        public const char KEY_HOME = '↖';
        public const char KEY_PGDOWN = '⇟';
        public const char KEY_PGUP = '⇞';

        public const char KEY_UP = '↑';
        public const char KEY_RIGHT = '→';
        public const char KEY_DOWN = '↓';
        public const char KEY_LEFT = '←';

        #endregion

    }
}
