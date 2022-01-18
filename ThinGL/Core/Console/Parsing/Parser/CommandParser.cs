using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

using ThinGin.Core.Console.Parsing.Tokenizer;

namespace ThinGin.Core.Console.Parsing
{
    public class CommandParser
    {
        #region Properties
        private readonly TokenStream Stream;
        public readonly CCommand[] Items;
        #endregion

        #region Constructors
        public CommandParser(ReadOnlyMemory<char> Text)
        {
            var Tokenizer = new CommandTokenizer(Text);
            Stream = new TokenStream(Tokenizer.Tokens);
        }
        #endregion

        #region Parsing
        public IEnumerable<CCommand> Parse_Command_List() => Consume_Command_List(this.Stream);
        public CCommand Parse_Command() => Consume_Command(this.Stream);
        #endregion

        #region Classification
        private static bool Is_Range_Start(CToken A, CToken B, CToken C)
        {// Format: "[1:100]", "[:100]", "[1:]"
            // Must have a number and a colon in either order
            if (A.Type == ECTokenType.SqBracket_Open)
            {
                return Is_Range_Start(B, C, null);
            }

            if (A.Type == ECTokenType.Number && B.Type == ECTokenType.Colon)
                return true;
            if (A.Type == ECTokenType.Colon && B.Type == ECTokenType.Number)
                return true;

            return false;
        }
        #endregion

        #region Consuming

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CArgValue Consume_CArg(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            CToken Token = Stream.Next;
            switch (Token.Type)
            {
                case ECTokenType.Whitespace:
                    break;
                case ECTokenType.Number:
                    {
                        var tok = Stream.Consume() as NumberToken;
                        var argType = tok.DataType switch
                        {
                            ENumericTokenType.Number => ECArgType.NUMBER,
                            ENumericTokenType.Integer => ECArgType.INTEGER,
                            ENumericTokenType.Unsigned => ECArgType.UNSIGNED,
                            _ => throw new NotImplementedException(tok.DataType.ToString())
                        };

                        return new CArgValue(argType, tok.Number);
                    }
                case ECTokenType.Percentage:
                    {
                        var tok = Stream.Consume() as PercentageToken;
                        return new CArgValue(ECArgType.PERCENT, tok.Number);
                    }
                case ECTokenType.String:
                    {
                        var tok = Stream.Consume() as StringToken;
                        return new CArgValue(ECArgType.STRING, tok.Value);
                    }
                case ECTokenType.Ident:// Keyword
                    {
                        var tok = Stream.Consume() as IdentToken;
                        return new CArgValue(ECArgType.KEYWORD, tok.Value);
                    }
                case ECTokenType.Hash:
                    {
                        var tok = Stream.Consume() as HashToken;
                        // XXX: COLORS
                        return new CArgValue(ECArgType.KEYWORD, tok.Value);
                    }
                case ECTokenType.FunctionName:
                    {
                        var tok = Consume_Function(Stream);
                        return new CArgValue(tok);
                    }
                case ECTokenType.Function:
                    {
                        var tok = Stream.Consume() as CFunction;
                        return new CArgValue(tok);
                    }
                case ECTokenType.Selector:
                    {
                        var tok = Stream.Consume() as CSelectorToken;
                        return new CArgValue(tok);
                    }
                case ECTokenType.At_Sign:
                    {
                        var tok = Consume_Selector(Stream);
                        return new CArgValue(tok);
                    }
                case ECTokenType.EOF:
                    {
                        return CArgValue.Null;
                    }
            }

            throw new SyntaxError(Stream, $"Unhandled token type for command argument!");
        }

        /// <summary>
        /// Continually consumes tokens until the current token is not a whitespace one
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]// Private static function called in loops, inline it
        static void Consume_All_Whitespace(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            Stream.Consume_While(tok => tok.Type == ECTokenType.Whitespace);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IEnumerable<CCommand> Consume_Command_List(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            var Commands = new LinkedList<CCommand>();

            CToken Token;
            do
            {
                Token = Stream.Consume();
                switch (Token.Type)
                {
                    case ECTokenType.Whitespace:
                        continue;
                    case ECTokenType.EOF:
                        return Commands;
                    case ECTokenType.Ident:
                        {
                            Stream.Reconsume();
                            // consume tokens until encountering an EOF or semicolon
                            var StartPos = Stream.Position;
                            var EndPos = Stream.Length;

                            if (Stream.Scan(SemicolonToken.Instance, out int outIndex))
                                EndPos = outIndex;

                            // pump all the items we consumed into a new substream
                            var count = StartPos - EndPos;
                            var range = Range.EndAt(Index.FromStart(count));
                            var subslice = Stream.Slice(range);
                            var substream = new TokenStream(subslice);

                            var item = Consume_Command(substream);
                            if (item is object) Commands.AddLast(item);
                        }
                        break;
                }
            }
            while (Token != CToken.EOF);

            return Commands;
        }

        static CCommand Consume_Command(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            // A command is formatted as such: <name> <args>;
            // So first we grab the name and then loop and consume stuff until we hit a semicolon
            Consume_All_Whitespace(Stream);
            if (Stream.Next.Type != ECTokenType.Ident)
                throw new SyntaxError(Stream);

            // The name is composed of all tokens from the first ident until the first whitespace
            //Stream.Consume_While(ctok => ctok.Type != ECTokenType.Whitespace, out ReadOnlySpan<CToken> NameTokens);
            var NameToken = Stream.Consume<IdentToken>();
            var items = new LinkedList<CArgValue>();

            CToken Token;
            do
            {
                Token = Stream.Consume();
                switch (Token.Type)
                {
                    case ECTokenType.Whitespace:
                        continue;
                    case ECTokenType.EOF:
                    case ECTokenType.Semicolon:
                        break;
                    default:
                        {
                            Stream.Reconsume();
                            //var item = Consume_Component(Stream);
                            var item = Consume_CArg(Stream);
                            if (item is object) items.AddLast(item);
                        }
                        break;
                }
            }
            while (Token != CToken.EOF);

            return new CCommand(NameToken.Value, items.ToArray());
        }


        /// <summary>
        /// Attempts to consume all tokens within a matching pair of () or {} brackets and otherwise just returns the next token.
        /// </summary>
        /// <param name="Stream"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CToken Consume_Component(TokenStream Stream)
        {
            if (Stream is null)  throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            switch (Stream.Next.Type)
            {
                case ECTokenType.Bracket_Open:
                case ECTokenType.Parenth_Open:
                    {
                        return Consume_SimpleBlock(Stream);
                    }
                case ECTokenType.SqBracket_Open:
                    {
                        if (Is_Range_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                        {
                            Consume_Range(Stream);
                        }

                        return Consume_SimpleBlock(Stream);
                    }
                case ECTokenType.FunctionName:
                    {
                        return Consume_Function(Stream);
                    }
                case ECTokenType.At_Sign:
                    {
                        return Consume_Selector(Stream);
                    }
                case ECTokenType.Hash:
                    {// A hash token is always a reference token
                        // and a reference token is a selector
                        var rTok = Consume_Reference(Stream);
                        if (Stream.Next.Type == ECTokenType.Colon)
                        {
                            Stream.Consume();

                            if (Stream.Next.Type != ECTokenType.Ident && Stream.Next.Type != ECTokenType.FullStop)
                            {
                                throw new SyntaxError(Stream, "Expected property identifier!");
                            }

                            var idents = Consume_FullStop_Seperated_Ident_List(Stream);
                            return new CSelectorToken(ECSelectorType.Reference, idents, rTok);
                        }

                        return new CSelectorToken(ECSelectorType.Reference, null, rTok);
                    }
            }

            return Stream.Consume();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IEnumerable<IEnumerable<CToken>> Consume_Comma_Seperated_Component_Value_List(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            var cvls = new LinkedList<LinkedList<CToken>>();
            var node = cvls.AddLast(new LinkedList<CToken>());

            do
            {
                switch (Stream.Next.Type)
                {
                    case ECTokenType.Comma:
                    case ECTokenType.EOF:
                        {
                            cvls.AddLast(node);
                        }
                        break;
                    default:
                        {
                            node.Value.AddLast(Consume_Component(Stream));
                        }
                        break;
                }
            }
            while (Stream.Next != CToken.EOF);

            return cvls;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IEnumerable<CToken> Consume_FullStop_Seperated_Ident_List(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            var items = new LinkedList<CToken>();
            do
            {
                switch (Stream.Next.Type)
                {
                    case ECTokenType.EOF:
                        {
                            return items;
                        }
                    case ECTokenType.FullStop:
                        continue;
                    case ECTokenType.Ident:
                        {
                            items.AddLast(Stream.Consume());
                        }
                        break;
                    default:
                        {
                            return items;
                        }
                }
            }
            while (Stream.Next.Type != ECTokenType.EOF);

            return items;
        }


        /// <summary>
        /// Consumes a block of tokens encased inbetween one of: [], {}, or ()
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CSimpleBlock Consume_SimpleBlock(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            CToken StartToken = Stream.Consume();
            CToken EndToken;
            switch (StartToken.Type)
            {
                case ECTokenType.Bracket_Open:
                    EndToken = BracketCloseToken.Instance;
                    break;
                case ECTokenType.Parenth_Open:
                    EndToken = ParenthesisCloseToken.Instance;
                    break;
                case ECTokenType.SqBracket_Open:
                    EndToken = SqBracketCloseToken.Instance;
                    break;
                default:
                    throw new SyntaxError(Stream, "Expected ending bracket");
            }

            var Block = new CSimpleBlock(StartToken);
            CToken Token;
            do
            {
                Token = Stream.Consume();
                if (Token.Type == EndToken.Type) return Block;
                if (Token.Type == ECTokenType.EOF) return Block;

                Block.Values.Add(Consume_Component(Stream));
            }
            while (Token.Type != ECTokenType.EOF);

            return Block;
        }

        /// <summary>
        /// Consumes a range token consisting of atleast a single nubmer and colon encased inbetween square brackets: []
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CReference Consume_Reference(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            if (Stream.Next.Type != ECTokenType.Hash) throw new SyntaxError(Stream, "Expected hash token for reference indicator!");

            var hTok = Stream.Consume<HashToken>();
            if (Stream.NextNext.Type == ECTokenType.Colon)
            {// A hash token followed by a colon creates a reference token
                Stream.Consume();//consume the colon
                // now consume the reference value
                var vTok = Consume_Component(Stream);

                return new CReference(hTok, vTok);
            }

            return new CReference(hTok, null);
        }

        /// <summary>
        /// Consumes a range token consisting of atleast a single nubmer and colon encased inbetween square brackets: []
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static RangeToken Consume_Range(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            if (Stream.Next.Type != ECTokenType.SqBracket_Open) throw new SyntaxError(Stream, "Expected opening bracket for range specifier!");

            Stream.Consume();
            // Ranges will start with either a number and a colon
            // Or a colon and a number
            if (Stream.Next.Type == ECTokenType.Number)
            {
                var tokL = Stream.Consume<NumberToken>();
                int numL = (int)tokL.Number;
                var idxL = (int)numL < 0 ? Index.FromEnd(numL) : Index.FromStart(numL);

                if (Stream.Next.Type != ECTokenType.Colon)
                {
                    if (Stream.NextNext.Type != ECTokenType.SqBracket_Close)
                    {
                        throw new SyntaxError(Stream, "Expecting closing bracket for range specifier!");
                    }

                    return new RangeToken(Range.StartAt(idxL));
                }

                Stream.Consume();// consume colon

                if (Stream.Next.Type != ECTokenType.Number)
                {
                    throw new SyntaxError(Stream, "Expecting end index within range specifier!");
                }

                var tokR = Stream.Consume<NumberToken>();
                int numR = (int)tokR.Number;
                var idxR = (int)numR < 0 ? Index.FromStart(numR) : Index.FromEnd(numR);

                if (Stream.NextNext.Type != ECTokenType.SqBracket_Close)
                {
                    throw new SyntaxError(Stream, "Expecting closing bracket for range specifier!");
                }

                return new RangeToken(new Range(idxL, idxR));
            }
            else if (Stream.Next.Type == ECTokenType.Colon)
            {
                Stream.Consume();

                if (Stream.Next.Type == ECTokenType.Number)
                {
                    var tokR = Stream.Consume<NumberToken>();
                    int numR = (int)tokR.Number;
                    var idxR = (int)numR < 0 ? Index.FromStart(numR) : Index.FromEnd(numR);

                    if (Stream.Next.Type != ECTokenType.SqBracket_Close)
                    {
                        throw new SyntaxError(Stream, "Expecting closing bracket for range specifier!");
                    }

                    return new RangeToken(Range.EndAt(idxR));
                }
            }

            throw new SyntaxError(Stream, "Expecting one of either a starting or ending index within range specifier but found neither!");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CFunction Consume_Function(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            if (Stream.Next.Type != ECTokenType.FunctionName) throw new SyntaxError(Stream, "Expected function name");

            var name = (Stream.Next as FunctionNameToken).Value;
            var Func = new CFunction(name);

            CToken Token;
            do
            {
                Token = Stream.Consume();
                switch (Token.Type)
                {
                    case ECTokenType.EOF:
                    case ECTokenType.Parenth_Close:
                        return Func;
                    default:
                        Stream.Reconsume();
                        Func.Arguments.Add(Consume_Component(Stream));
                        break;
                }
            }
            while (Token.Type != ECTokenType.EOF);

            return Func;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static CSelectorToken Consume_Selector(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            if (Stream.Next.Type != ECTokenType.At_Sign) throw new SyntaxError(Stream, "Expected selector start token!");

            Stream.Consume();// Consume the at sign, we now know we are working with a selector

            CToken Token;
            do
            {
                Token = Consume_Component(Stream);
                switch (Token.Type)
                {
                    case ECTokenType.Whitespace:
                    case ECTokenType.EOF:
                        return null;
                    case ECTokenType.SimpleBlock:
                        {
                            return new CSelectorToken(Token);
                        }
                    case ECTokenType.FullStop:
                    case ECTokenType.Ident:
                        {
                            return new CSelectorToken(Consume_FullStop_Seperated_Ident_List(Stream));
                        }
                }
            }
            while (Token.Type != ECTokenType.EOF);

            return null;
        }


        #endregion
    }
}
