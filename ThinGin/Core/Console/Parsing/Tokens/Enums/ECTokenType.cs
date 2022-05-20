namespace ThinGin.Core.Console.Parsing
{
    public enum ECTokenType : int
    {
        /// <summary></summary>
        None,
        /// <summary>Represents a collection of 2 or more characters NOT within quotations</summary>
        Ident,
        /// <summary>Represents an identifier followed immediately by an opening-parenthesis "FUNCTION_NAME("</summary>
        FunctionName,
        /// <summary>Represents an at-sign(@)</summary>
        At_Sign,
        /// <summary>Represents an dollar-sign($)</summary>
        VarName,
        /// <summary>Represents an identifier prefixed with a hashtag(#)</summary>
        Hash,
        /// <summary>Represents a collection of characters contained within quotations ("")</summary>
        String,
        /// <summary></summary>
        Bad_String,
        /// <summary></summary>
        Url,
        /// <summary></summary>
        Bad_Url,
        /// <summary>Represents any single character not assigned to another token</summary>
        Delim,
        /// <summary></summary>
        Number,
        /// <summary></summary>
        Percentage,
        /// <summary></summary>
        Dimension,
        /// <summary></summary>
        Indice_Range,
        /// <summary></summary>
        Unicode_Range,
        /// <summary>Represents '|'</summary>
        Column,
        /// <summary>Represents any of ' ', '\n', '\r', '\t', '\f'</summary>
        Whitespace,
        /// <summary>Represents '.'</summary>
        FullStop,
        /// <summary>Represents ':'</summary>
        Colon,
        /// <summary>Represents ';'</summary>
        Semicolon,
        /// <summary>Represents ','</summary>
        Comma,
        /// <summary>Represents '['</summary>
        SqBracket_Open,
        /// <summary>Represents ']'</summary>
        SqBracket_Close,
        /// <summary>Represents '('</summary>
        Parenth_Open,
        /// <summary>Represents ')'</summary>
        Parenth_Close,
        /// <summary>Represents '{'</summary>
        Bracket_Open,
        /// <summary>Represents '}'</summary>
        Bracket_Close,

        /// <summary>Represents an EOF token</summary>
        EOF,

        Operator,
        Command,
        Function,
        /// <summary> A hash-token which references some single value or constant </summary>
        Reference,
        /// <summary> Represents a multitude of items all grouped between some sort of start/end identifier character like parenthesees or curly brackets. </summary>
        SimpleBlock,
        /// <summary> A selector, kind of like an inline function that executes logic to get some resultant value to be used in the command </summary>
        Selector,
    }
}
