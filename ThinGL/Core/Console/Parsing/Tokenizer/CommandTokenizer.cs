﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

using ThinGin.Core.Common.Parsing;
using ThinGin.Core.Common.Types;

using static ThinGin.Core.Common.UnicodeCommon;

namespace ThinGin.Core.Console.Parsing.Tokenizer
{
    /// <summary>
    /// A virtual console command tokenizer derived from the W3C standards for parsing CSS3.
    /// </summary>
    public class CommandTokenizer
    {
        #region Properties
        private readonly TextConsumer Stream;
        private readonly CToken[] tokens = null;

        public ReadOnlyMemory<CToken> Tokens => tokens;
        #endregion

        #region Operators
        public CToken this[int index]
        {
            get
            {
                return tokens[index];
            }
        }
        #endregion

        #region Constructors
        public CommandTokenizer(ReadOnlyMemory<char> Text)
        {
            Stream = new TextConsumer(Text);
            var Tokens = new LinkedList<CToken>();
            CToken last;
            do
            {
                last = Consume_Token(Stream);
                Tokens.AddLast(last);
            }
            while (last.Type != ECTokenType.EOF);

            tokens = new CToken[Tokens.Count];
            var node = Tokens.First;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (node is null)
                {
                    throw new ParserFailedException("Error while parsing console command. Tokens list length was incorrect, null tokens present!");
                }

                tokens[i] = node.Value;
                node = node.Next;
            }
        }
        #endregion

        #region Parsing
        public static CToken[] Parse(ReadOnlyMemory<char> Text)
        {
            var tokenizer = new CommandTokenizer(Text);
            return tokenizer.tokens;
        }

        /// <summary>
        /// Consumes an escaped character from the current reading position
        /// </summary>
        /// <returns></returns>
        private static char Consume_Escaped(DataConsumer<char> Stream)
        {// Docs:  https://www.w3.org/TR/css-syntax-3/#consume-escaped-code-point
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            if (Hex.IsHexChar(Stream.Next))
            {// Consume as many hex digits as possible but no more then 5 (for a total of 6)
                if (!Stream.Consume_While(Hex.IsHexChar, out ReadOnlySpan<char> hexDigits, 6))
                    throw new ParserFailedException();

                if (char.IsWhiteSpace(Stream.Next)) Stream.Consume();

                int hex = Convert.ToInt32(hexDigits.ToString(), 16);

                if (hex == 0 || hex > CHAR_UNICODE_MAX || char.IsSurrogate((char)hex))
                    return CHAR_REPLACEMENT;

                return (char)hex;
            }
            else
            {
                if (Stream.Next == EOF)
                    return CHAR_REPLACEMENT;
                else
                    return Stream.Consume();
            }
        }

        /// <summary>
        /// Consumes and returns a string token
        /// </summary>
        /// <param name="EndChar">Character which indicates the end of a string</param>
        /// <returns>StringToken or BadStringToken</returns>
        private static CToken Consume_String_Token(DataConsumer<char> Stream, char? EndChar = null)
        {// Docs: https://www.w3.org/TR/css-syntax-3/#consume-string-token
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();
            /* 
             * This algorithm may be called with an ending code point, which denotes the code point that ends the string. 
             * If an ending code point is not specified, the current input code point is used.
             */

            if (!EndChar.HasValue)
            {
                EndChar = Stream.Consume();
            }

            StringBuilder Buf = new StringBuilder();
            do
            {
                switch (Stream.Next)
                {
                    case char _ when (Stream.Next == EndChar):
                        {
                            Stream.Consume();
                            return new StringToken(Buf.ToString());
                        }
                    case EOF:
                        {
                            return new StringToken(Buf.ToString());
                        }
                    case CHAR_LINE_FEED:
                        {
                            Stream.Consume();
                            return new BadStringToken(Buf.ToString());
                        }
                    case CHAR_REVERSE_SOLIDUS:
                        {
                            char next = Stream.Consume();
                            switch (Stream.Next)
                            {
                                case EOF:
                                    {
                                        continue;
                                    }
                                case CHAR_LINE_FEED:
                                    {
                                        Stream.Consume();
                                        break;
                                    }
                                default:
                                    {
                                        if (Is_Valid_Escape(next, Stream.Next))
                                        {
                                            Buf.Append(Consume_Escaped(Stream));
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    default:
                        {
                            Buf.Append(Stream.Consume());
                            break;
                        }
                }

            }
            while (!Stream.atEnd && !Stream.atEOF);

            return new StringToken(Buf.ToString());
        }

        public static double Convert_String_To_Number(DataConsumer<char> Stream)
        {/* Docs: https://www.w3.org/TR/css-syntax-3/#convert-string-to-number */
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            bool IsDecimal = false;
            var startPos = Stream.Position;


            if (Stream.Next == CHAR_HYPHEN_MINUS)
            {
                Stream.Consume();
            }
            else if (Stream.Next == CHAR_PLUS_SIGN)
            {
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Integer))
                throw new ParserFailedException();

            if (Stream.Next == CHAR_FULL_STOP)
            {
                IsDecimal = true;
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Fraction))
                throw new ParserFailedException();

            if (Stream.Next == CHAR_E_UPPER || Stream.Next == CHAR_E_LOWER)
            {
                Stream.Consume();
            }

            if (Stream.Next == CHAR_HYPHEN_MINUS)
            {
                Stream.Consume();
            }
            else if (Stream.Next == CHAR_PLUS_SIGN)
            {
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Exponent))
                throw new ParserFailedException();

            //return S * (I + (F * Math.Pow(10, -D))) * Math.Pow(10, T*E);

            var total = Stream.Position - startPos;
            Stream.Reconsume(total);
            var substream = Stream.Slice(new Range(0, total));
            Stream.Consume(total);

            if (IsDecimal)
            {
                return Double.Parse(substream.Span);
            }
            else
            {
                if (Int64.TryParse(substream.Span, out var outNum))
                {
                    return outNum;
                }

                if (UInt64.TryParse(substream.Span, out var outUnsigned))
                {
                    return outUnsigned;
                }

                throw new ParserFailedException($"Unable to parse integer token from string value: \"{substream.ToString()}\"");
            }
        }

        public static void Consume_Number(DataConsumer<char> Stream, out ReadOnlyMemory<char> outResult, out object outNumber, out ENumericTokenType outType)
        {/* Docs: https://www.w3.org/TR/css-syntax-3/#consume-number */
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            outType = ENumericTokenType.Integer;

            int Start = Stream.Position;

            if (Stream.Next == CHAR_HYPHEN_MINUS)
            {
                Stream.Consume();
            }
            else if (Stream.Next == CHAR_PLUS_SIGN)
            {
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Integer))
                throw new ParserFailedException();

            if (Stream.Next == CHAR_FULL_STOP)
            {
                outType = ENumericTokenType.Number;
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Fraction) && outType == ENumericTokenType.Number)
                throw new ParserFailedException();

            bool HasExponent = false;
            if (Stream.Next == CHAR_E_UPPER || Stream.Next == CHAR_E_LOWER)
            {
                HasExponent = true;
                outType = ENumericTokenType.Number;
                Stream.Consume();
            }

            if (Stream.Next == CHAR_HYPHEN_MINUS)
            {
                Stream.Consume();
            }
            else if (Stream.Next == CHAR_PLUS_SIGN)
            {
                Stream.Consume();
            }

            if (!Stream.Consume_While(char.IsDigit, out ReadOnlySpan<char> Exponent) && HasExponent)
                throw new ParserFailedException();

            int len = Stream.Position - Start;
            var substream = Stream.AsMemory().Slice(Start, len);
            outResult = substream;

            //return S * (I + (F * Math.Pow(10, -D))) * Math.Pow(10, T*E);

            if (outType == ENumericTokenType.Number)
            {
                outNumber = Double.Parse(substream.Span);
            }
            else
            {
                if (Int64.TryParse(substream.Span, out var outNum))
                {
                    outNumber = outNum;
                    outType = ENumericTokenType.Integer;
                }

                if (UInt64.TryParse(substream.Span, out var outUnsigned))
                {
                    outNumber = outUnsigned;
                    outType = ENumericTokenType.Unsigned;
                }

                throw new ParserFailedException($"Unable to parse integer token from string value: \"{substream.ToString()}\"");
            }
        }

        /// <summary>
        /// Consumes and returns a numeric token
        /// </summary>
        private static CToken Consume_Numeric_Token(DataConsumer<char> Stream)
        {// Docs:  https://www.w3.org/TR/css-syntax-3/#consume-a-numeric-token
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            Consume_Number(Stream, out ReadOnlyMemory<char> nStr, out object N, out ENumericTokenType nType);

            if (Is_Identifier_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
            {
                string nUnit = Consume_Name(Stream);
                return new DimensionToken(nType, nStr.Span, N, nUnit);
            }

            if (Stream.Next == CHAR_PERCENT)
            {
                Stream.Consume();
                return new PercentageToken(nStr.Span, (double)N);
            }

            return new NumberToken(nType, nStr.Span, N);
        }

        private static void Consume_Bad_Url_Remnants(DataConsumer<char> Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            char tok;
            do
            {
                tok = Stream.Consume();
                if (tok == CHAR_RIGHT_PARENTHESES || tok == EOF)
                {
                    return;
                }
                else if (Is_Valid_Escape(tok, Stream.Next))
                {
                    Consume_Escaped(Stream);
                }
            }
            while (tok != EOF);
        }

        private static UrlToken Consume_Url_Token(DataConsumer<char> Stream)
        {// Docs:  https://www.w3.org/TR/css-syntax-3/#consume-a-url-token
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            string Result = "";
            Stream.Consume_While(char.IsWhiteSpace);
            if (Stream.Next == EOF)
            {
                return new UrlToken(Result);
            }
            else if (Stream.Next == CHAR_QUOTATION_MARK || Stream.Next == CHAR_APOSTRAPHE)
            {
                char c = Stream.Consume();
                CToken stok = Consume_String_Token(Stream, c);
                if (stok.Type == ECTokenType.Bad_String)
                {
                    Consume_Bad_Url_Remnants(Stream);
                    return new BadUrlToken();
                }

                Result = ((StringToken)stok).Value;
                Stream.Consume_While(char.IsWhiteSpace);
                if (Stream.Next == CHAR_RIGHT_PARENTHESES || Stream.Next == EOF)
                {
                    Stream.Consume();
                    return new UrlToken(Result);
                }

                Consume_Bad_Url_Remnants(Stream);
                return new BadUrlToken();
            }

            do
            {
                switch (Stream.Next)
                {
                    case char _ when (char.IsWhiteSpace(Stream.Next)):
                        {
                            Stream.Consume_While(char.IsWhiteSpace);
                        }
                        break;
                    case EOF:
                    case CHAR_RIGHT_PARENTHESES:
                        {
                            return new UrlToken(Result);
                        }
                    case CHAR_QUOTATION_MARK:
                    case CHAR_APOSTRAPHE:
                    case CHAR_LEFT_PARENTHESES:
                    case char _ when (char.IsControl(Stream.Next)):
                        {
                            Consume_Bad_Url_Remnants(Stream);
                            return new BadUrlToken();
                        }
                    case CHAR_REVERSE_SOLIDUS:
                        {
                            if (Is_Valid_Escape(Stream.Next, Stream.NextNext))
                            {
                                Result += Consume_Escaped(Stream);
                            }
                            else
                            {
                                Stream.Consume();
                                Consume_Bad_Url_Remnants(Stream);
                                return new BadUrlToken();
                            }
                        }
                        break;
                    default:
                        {
                            Result += Stream.Consume();
                        }
                        break;
                }
            }
            while (!Stream.atEnd && !Stream.atEOF);

            return new BadUrlToken();
        }

        private static CToken Consume_Ident_Like_Token(DataConsumer<char> Stream)
        {// SEE:  https://www.w3.org/TR/css-syntax-3/#consume-an-ident-like-token0
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            string Name = Consume_Name(Stream);

            if (Name.Equals("url", StringComparison.InvariantCultureIgnoreCase) && Stream.Next == CHAR_LEFT_PARENTHESES)
            {
                Stream.Consume();
                return Consume_Url_Token(Stream);
            }
            else if (Stream.Next == CHAR_LEFT_PARENTHESES)
            {
                Stream.Consume();
                return new FunctionNameToken(Name);
            }

            return new IdentToken(Name);
        }

        private static UnicodeRangeToken Consume_Unicode_Range_Token(DataConsumer<char> Stream)
        {// SEE:  https://www.w3.org/TR/css-syntax-3/#consume-a-unicode-range-token
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            // Consume hex and UnicodeCommon.CHAR_QUESTION_MARK digits up to a maximum of 6
            if (!Stream.Consume_While(ch => (Hex.IsHexChar(ch) || ch == CHAR_QUESTION_MARK), out ReadOnlyMemory<char> Digits, 6))
            {
                throw new ParserFailedException();
            }

            if (Digits.Span.Contains(CHAR_QUESTION_MARK))
            {
                string DigitsStr = Digits.Span.ToString();
                int Start = Convert.ToInt32(DigitsStr.Replace(CHAR_QUESTION_MARK, CHAR_DIGIT_0), 16);
                int End = Convert.ToInt32(DigitsStr.Replace(CHAR_QUESTION_MARK, CHAR_F_UPPER), 16);
                return new UnicodeRangeToken(Start, End);
            }

            int StartRange = Convert.ToInt32(Digits.ToString(), 16);
            int EndRange = StartRange;
            if (Stream.Next == CHAR_HYPHEN_MINUS && Hex.IsHexChar(Stream.NextNext))
            {
                Stream.Consume();
                // Consume hex digits up to a maximum of 6
                if (Stream.Consume_While(Hex.IsHexChar, out ReadOnlySpan<char> endDigits, 6))
                {
                    EndRange = Convert.ToInt32(endDigits.ToString(), 16);
                }
            }

            return new UnicodeRangeToken(StartRange, EndRange);
        }

        private static string Consume_Name(DataConsumer<char> Stream)
        {// Docs:  https://www.w3.org/TR/css-syntax-3/#consume-a-name
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            StringBuilder Result = new StringBuilder();
            do
            {
                if (Is_Name_Char(Stream.Next))
                {
                    Result.Append(Stream.Consume());
                }
                else if (Is_Valid_Escape(Stream.Next, Stream.NextNext))
                {
                    Result.Append(Consume_Escaped(Stream));
                }
                else
                {
                    break;
                }

            }
            while (!Stream.atEnd && !Stream.atEOF);

            return Result.ToString();
        }
        #endregion

        #region Character Identification
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Is_Identifier_Start(char A, char B, char C)
        {
            if (A == CHAR_HYPHEN_MINUS)
            {
                return (Is_Name_Start_Char(B) || Is_Valid_Escape(B, C));
            }
            else if (Is_Name_Start_Char(A))
            {
                return true;
            }
            else if (A == CHAR_REVERSE_SOLIDUS)
            {
                return Is_Valid_Escape(A, B);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Is_Number_Start(char A, char B, char C)
        {
            if (A == CHAR_PLUS_SIGN || A == CHAR_HYPHEN_MINUS)
            {
                if (char.IsDigit(B)) return true;
                if (B == CHAR_FULL_STOP && char.IsDigit(C)) return true;
                return false;
            }
            else if (A == CHAR_FULL_STOP)
            {
                return char.IsDigit(B);
            }
            else if (char.IsDigit(A)) return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Is_Name_Char(char token)
        {
            if ((token >= CHAR_A_UPPER && token <= 'Z') || (token >= CHAR_A_LOWER && token <= 'z')) return true;
            if (token >= '\u0080') return true;
            if (token == '_') return true;
            if (token >= CHAR_DIGIT_0 && token <= CHAR_DIGIT_9) return true;
            if (token == CHAR_HYPHEN_MINUS) return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Is_Name_Start_Char(char token)
        {
            if ((token >= CHAR_A_UPPER && token <= 'Z') || (token >= CHAR_A_LOWER && token <= 'z')) return true;
            if (token >= '\u0080') return true;
            if (token == '_') return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Is_Valid_Escape(char A, char B)
        {// SEE:  https://www.w3.org/TR/css-syntax-3/#check-if-two-code-points-are-a-valid-escape
            if (A != CHAR_REVERSE_SOLIDUS) return false;
            else if (B == CHAR_LINE_FEED) return false;

            return true;
        }

        #endregion

        /// <summary>
        /// Consumes and returns the next Token
        /// </summary>
        /// <returns></returns>
        private static CToken Consume_Token(DataConsumer<char> Stream)
        {// SEE:  https://www.w3.org/TR/css-syntax-3/#consume-a-token0
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            switch (Stream.Next)
            {
                case var _ when (char.IsWhiteSpace(Stream.Next)):
                    {// Consume as much whitepace as possible
                        Stream.Consume_While(char.IsWhiteSpace, out ReadOnlySpan<char> Value);
                        return new WhitespaceToken(Value.ToString());
                    }
                case CHAR_QUOTATION_MARK:
                    {
                        Stream.Consume();
                        return Consume_String_Token(Stream, CHAR_QUOTATION_MARK);
                    }
                case CHAR_HASH:
                    {
                        Stream.Consume();
                        if (Is_Name_Char(Stream.Next) || Is_Valid_Escape(Stream.Next, Stream.NextNext))
                        {
                            if (Is_Identifier_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                            {
                                return new HashToken(EHashTokenType.Name, Consume_Name(Stream));
                            }
                            else
                            {
                                return new HashToken(EHashTokenType.Value, Consume_Token(Stream).Encode());
                            }
                        }

                        return new HashToken(EHashTokenType.Value, Consume_Token(Stream).Encode());
                        //return new DelimToken(CHAR_HASH);
                    }
                case CHAR_DOLLAR_SIGN:
                    {
                        if (Is_Identifier_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        {
                            string Name = Consume_Name(Stream);
                            return new VarNameToken(Name);
                        }
                        return new DelimToken(CHAR_DOLLAR_SIGN);
                    }
                case CHAR_APOSTRAPHE:
                    {
                        return Consume_String_Token(Stream);
                    }
                case CHAR_LEFT_PARENTHESES:
                    {
                        Stream.Consume();
                        return ParenthesisOpenToken.Instance;
                    }
                case CHAR_RIGHT_PARENTHESES:
                    {
                        Stream.Consume();
                        return ParenthesisCloseToken.Instance;
                    }
                case CHAR_AMPERSAND:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_AMPERSAND)
                        {
                            Stream.Consume();
                            return OperatorToken.And;
                        }

                        return OperatorToken.BitwiseAnd;
                    }
                case CHAR_PIPE:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_PIPE)
                        {
                            Stream.Consume();
                            return OperatorToken.Or;
                        }

                        return OperatorToken.BitwiseOr;
                    }
                case CHAR_EXCLAMATION_POINT:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_EQUALS)
                        {
                            Stream.Consume();
                            return OperatorToken.NotEqual;
                        }

                        return OperatorToken.Not;
                    }
                case CHAR_EQUALS:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_EQUALS)
                        {
                            Stream.Consume();
                            return OperatorToken.Equal;
                        }

                        return new DelimToken(CHAR_EQUALS);
                    }
                case CHAR_ASTERISK:
                    {
                        Stream.Consume();
                        return OperatorToken.Multiply;
                        //return new DelimToken(CHAR_ASTERISK);
                    }
                case CHAR_PLUS_SIGN:
                    {
                        Stream.Consume();
                        if (Is_Number_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        {
                            return Consume_Numeric_Token(Stream);
                        }

                        return OperatorToken.Add;
                        //return new DelimToken(CHAR_PLUS_SIGN);
                    }
                case CHAR_COMMA:
                    {
                        Stream.Consume();
                        return CommaToken.Instance;
                    }
                case CHAR_HYPHEN_MINUS:
                    {
                        Stream.Consume();
                        if (Is_Number_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        {
                            return Consume_Numeric_Token(Stream);
                        }
                        else if (Is_Identifier_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        {
                            return Consume_Ident_Like_Token(Stream);
                        }

                        return OperatorToken.Subtract;
                        //return new DelimToken(CHAR_HYPHEN_MINUS);
                    }
                case CHAR_FULL_STOP:
                    {
                        Stream.Consume();
                        if (Is_Number_Start(CHAR_FULL_STOP, Stream.Next, Stream.NextNext))
                        {
                            return Consume_Numeric_Token(Stream);
                        }

                        return FullStopToken.Instance;
                        //return new DelimToken(CHAR_FULL_STOP);
                    }
                case CHAR_SOLIDUS:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_ASTERISK)// This is the start of a comment block, consume it all
                        {
                            Stream.Consume();
                            while (!Stream.atEnd && !Stream.atEOF)
                            {
                                char c = Stream.Consume();
                                if (c == CHAR_ASTERISK && Stream.Next == CHAR_SOLIDUS)
                                {
                                    Stream.Consume();
                                    return Consume_Token(Stream);
                                }
                            }
                        }

                        return OperatorToken.Divide;
                        //return new DelimToken(CHAR_SOLIDUS);
                    }
                case CHAR_COLON:
                    {
                        Stream.Consume();
                        return ColonToken.Instance;
                    }
                case CHAR_SEMICOLON:
                    {
                        Stream.Consume();
                        return SemicolonToken.Instance;
                    }
                case CHAR_LEFT_CHEVRON:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_EQUALS)
                        {
                            Stream.Consume();
                            return OperatorToken.LessThanEq;
                        }

                        //return new DelimToken(CHAR_LEFT_CHEVRON);
                        return OperatorToken.LessThan;
                    }
                case CHAR_RIGHT_CHEVRON:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_EQUALS)
                        {
                            Stream.Consume();
                            return OperatorToken.GreaterThanEq;
                        }

                        //return new DelimToken(CHAR_RIGHT_CHEVRON);
                        return OperatorToken.GreaterThan;
                    }
                case CHAR_AT_SIGN:
                    {
                        Stream.Consume();
                        //if (Is_Identifier_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        //{
                        //    string Name = Consume_Name(Stream);
                        //    return new AtToken(Name);
                        //}
                        //return new DelimToken(CHAR_AT_SIGN);
                        return AtToken.Instance;
                    }
                case CHAR_LEFT_SQUARE_BRACKET:
                    {
                        Stream.Consume();
                        return SqBracketOpenToken.Instance;
                    }
                case CHAR_REVERSE_SOLIDUS:
                    {
                        Stream.Consume();
                        if (Is_Valid_Escape(CHAR_REVERSE_SOLIDUS, Stream.Next))
                        {
                            Stream.Reconsume();
                            return Consume_Ident_Like_Token(Stream);
                        }

                        return new DelimToken(CHAR_REVERSE_SOLIDUS);
                    }
                case CHAR_RIGHT_SQUARE_BRACKET:
                    {
                        Stream.Consume();
                        return SqBracketCloseToken.Instance;
                    }
                case CHAR_CARET:
                    {
                        Stream.Consume();
                        return new DelimToken(CHAR_CARET);
                    }
                case CHAR_LEFT_CURLY_BRACKET:
                    {
                        Stream.Consume();
                        return BracketOpenToken.Instance;
                    }
                case CHAR_RIGHT_CURLY_BRACKET:
                    {
                        Stream.Consume();
                        return BracketCloseToken.Instance;
                    }
                case var _ when (char.IsDigit(Stream.Next)):
                    {
                        return Consume_Numeric_Token(Stream);
                    }
                case CHAR_U_UPPER:
                case CHAR_U_LOWER:
                    {
                        Stream.Consume();
                        if (Stream.Next == CHAR_PLUS_SIGN
                            && (Hex.IsHexChar(Stream.NextNext) || Stream.NextNext == CHAR_QUESTION_MARK))
                        {
                            Stream.Consume(1);
                            return Consume_Unicode_Range_Token(Stream);
                        }

                        return Consume_Ident_Like_Token(Stream);
                    }
                case var _ when (Is_Name_Start_Char(Stream.Next)):
                    {
                        return Consume_Ident_Like_Token(Stream);
                    }
                case EOF:
                    {
                        return EOFToken.Instance;
                    }
                default:
                    {
                        return new DelimToken(Stream.Consume());
                    }
            }

        }
    }
}
