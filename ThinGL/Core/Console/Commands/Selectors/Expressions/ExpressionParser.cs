using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console.Commands.Selectors.Expressions
{
    public class ExpressionParser
    {
        #region Properties
        private readonly TokenStream Stream;
        public readonly Expression[] Items;
        #endregion

        #region Constructors
        public ExpressionParser(IEnumerable<CToken> Tokens)
        {
            Stream = new TokenStream(Tokens);
        }
        #endregion

        #region Compiling
        public List<Expression> Parse()
        {
            var expList = new List<Expression>();

            do
            {
                var e = Consume_Component(Stream, null);
                expList.Add(e);
            }
            while (!Stream.atEnd);

            return expList;
        }
        #endregion

        #region Classification
        private static bool Is_Valid_Accessor_Token(CToken Token) => (Token.Type == ECTokenType.Ident || Token.Type == ECTokenType.Delim);
        private static bool Is_Accessor_Chain_Start(CToken A, CToken B, CToken C)
        {
            if (A.Type != ECTokenType.VarName && !Is_Valid_Accessor_Token(A))
                return false;

            if (Is_Valid_Accessor_Token(B))
                return true;

            return (B.Type == ECTokenType.FullStop && Is_Valid_Accessor_Token(C));
        }
        #endregion

        #region Consuming

        private static Expression Consume_Component(TokenStream Stream, Expression Head)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            do
            {
                switch (Stream.Next.Type)
                {
                    case ECTokenType.Whitespace:
                        continue;
                    case ECTokenType.EOF:
                        return null;
                    case ECTokenType.Ident:
                    case ECTokenType.Delim:
                    case ECTokenType.VarName:
                        {
                            if (Is_Accessor_Chain_Start(Stream.Next, Stream.NextNext, Stream.NextNextNext))
                            {
                                return Consume_Accessor_Chain(Stream);
                            }

                            return Consume_Variable_Component(Stream);
                        }
                    case ECTokenType.Number:
                        {
                            var num = Stream.Consume<NumberToken>();
                            switch (num.DataType)
                            {
                                case ENumericTokenType.Number:
                                    return Expression.Constant(num.Number, typeof(double));
                                case ENumericTokenType.Integer:
                                    return Expression.Constant(num.Number, typeof(Int64));
                                case ENumericTokenType.Unsigned:
                                    return Expression.Constant(num.Number, typeof(UInt64));
                                default:
                                    throw new NotImplementedException(num.DataType.ToString());
                            }
                        }
                    case ECTokenType.Operator:
                        {
                            return Consume_Operator(Stream, Head);
                        }
                    case ECTokenType.Reference:
                        {
                            var refTok = Stream.Consume<CReference>();
                            Expression<Func<ConsoleContext, IEnumerable<object>>> refCall = (ctx) => ctx.Console.Select(refTok);

                            var ctxParam = Expression.Parameter(typeof(ConsoleContext));
                            return Expression.Lambda(refCall, new[] { ctxParam });
                        }
                }
            }
            while (Stream.Next.Type != ECTokenType.EOF);

            return null;
        }


        private static Expression Consume_Function_Call(TokenStream Stream, Expression Head)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            if (Stream.Next.Type != ECTokenType.Function) throw new SyntaxError(Stream, "Expected function reference.");

            var Token = Stream.Consume<CFunction>();

            /// XXX: TODO: Finish function calls, its probably best to just treat function calls as calls to console commands?
            // Gotta be able to differentiate between static functions and instance ones, but most would have to be static.
            // Gotta be able to consume the function arguments here too

            return null;// Expression.Call();

        }

        private static Expression Consume_Operator(TokenStream Stream, Expression Head)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            if (Stream.Next.Type != ECTokenType.Operator) throw new SyntaxError(Stream, "Expected operator.");

            OperatorToken tok = Stream.Consume<OperatorToken>();

            var left = Head;
            var right = Consume_Component(Stream, left);

            switch (tok.OperatorType)
            {
                case EOperatorType.And:
                    return Expression.AndAlso(left, right);
                case EOperatorType.Or:
                    return Expression.OrElse(left, right);
                case EOperatorType.Not:
                    return Expression.Not(right);
                case EOperatorType.Equal:
                    return Expression.Equal(left, right);
                case EOperatorType.NotEqual:
                    return Expression.NotEqual(left, right);
                case EOperatorType.BitwiseAnd:
                    return Expression.And(left, right);
                case EOperatorType.BitwiseOr:
                    return Expression.Or(left, right);
                case EOperatorType.Add:
                    return Expression.Add(left, right);
                case EOperatorType.Subtract:
                    return Expression.Subtract(left, right);
                case EOperatorType.Multiply:
                    return Expression.Multiply(left, right);
                case EOperatorType.Divide:
                    return Expression.Divide(left, right);
                case EOperatorType.Less:
                    return Expression.LessThan(left, right);
                case EOperatorType.Greater:
                    return Expression.GreaterThan(left, right);
                case EOperatorType.LessEq:
                    return Expression.LessThanOrEqual(left, right);
                case EOperatorType.GreaterEq:
                    return Expression.GreaterThanOrEqual(left, right);
                default:
                    throw new NotImplementedException(tok.OperatorType.ToString());
            }
        }

        private static Expression Consume_Variable_Component(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            if (Stream.Next is ValuedTokenBase)
            {
                var tok = Stream.Consume<ValuedTokenBase>();
                return Expression.Variable(typeof(object), tok.Value);
            }
            else if (Stream.Next is DelimToken)
            {
                var tok = Stream.Consume<DelimToken>();
                return Expression.Variable(typeof(object), tok.Value.ToString());
            }

            throw new NotImplementedException($"Unable to build expression for token: {Stream.Next}");
        }

        private static Expression Consume_Accessor_Chain(TokenStream Stream)
        {
            if (Stream is null) throw new ArgumentNullException(nameof(Stream));
            Contract.EndContractBlock();

            Expression Head = null;
            do
            {
                switch (Stream.Next.Type)
                {
                    case ECTokenType.Whitespace:
                    case ECTokenType.EOF:
                        return Head;
                    case ECTokenType.VarName:
                        {
                            if (Head is object)
                            {
                                Stream.Reconsume();
                                return Head;
                            }

                            var vt = Stream.Consume<VarNameToken>();
                            Head = Expression.Variable(typeof(object), vt.Value);
                        }
                        break;
                    case ECTokenType.Ident:
                        {
                            var tok = Stream.Consume<IdentToken>();
                            Head = Expression.PropertyOrField(Head, tok.Value);
                        }
                        break;
                    case ECTokenType.Delim:
                        {
                            var tok = Stream.Consume<DelimToken>();
                            Head = Expression.PropertyOrField(Head, tok.Value.ToString());
                        }
                        break;
                }
            }
            while (Stream.Next.Type != ECTokenType.EOF);

            return Head;
        }

        #endregion
    }
}
