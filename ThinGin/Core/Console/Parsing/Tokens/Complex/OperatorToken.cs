using System;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class OperatorToken : CToken
    {
        #region Instances
        public static OperatorToken And = new OperatorToken(EOperatorType.And);
        public static OperatorToken Or = new OperatorToken(EOperatorType.Or);

        public static OperatorToken Not = new OperatorToken(EOperatorType.Not);
        public static OperatorToken Equal = new OperatorToken(EOperatorType.Equal);
        public static OperatorToken NotEqual = new OperatorToken(EOperatorType.NotEqual);

        public static OperatorToken Add = new OperatorToken(EOperatorType.Add);
        public static OperatorToken Subtract = new OperatorToken(EOperatorType.Subtract);

        public static OperatorToken LessThan = new OperatorToken(EOperatorType.Less);
        public static OperatorToken GreaterThan = new OperatorToken(EOperatorType.Greater);
        public static OperatorToken LessThanEq = new OperatorToken(EOperatorType.LessEq);
        public static OperatorToken GreaterThanEq = new OperatorToken(EOperatorType.GreaterEq);

        public static OperatorToken Multiply = new OperatorToken(EOperatorType.Multiply);
        public static OperatorToken Divide = new OperatorToken(EOperatorType.Divide);

        public static OperatorToken BitwiseAnd = new OperatorToken(EOperatorType.BitwiseAnd);
        public static OperatorToken BitwiseOr = new OperatorToken(EOperatorType.BitwiseOr);
        #endregion

        #region Properties
        public readonly EOperatorType OperatorType;
        #endregion

        #region Constructors
        public OperatorToken(EOperatorType operatorType) : base(ECTokenType.Operator)
        {
            OperatorType = operatorType;
        }
        #endregion

        public override string Encode()
        {
            return OperatorType switch
            {
                EOperatorType.And => "&&",
                EOperatorType.Or => "||",
                EOperatorType.Not => "!",
                EOperatorType.Equal => "==",
                EOperatorType.NotEqual => "!=",
                EOperatorType.BitwiseAnd => "&",
                EOperatorType.BitwiseOr => "|",
                EOperatorType.Add => "+",
                EOperatorType.Subtract => "-",
                EOperatorType.Less => "<",
                EOperatorType.Greater => ">",
                EOperatorType.LessEq => "<=",
                EOperatorType.GreaterEq => ">=",
                EOperatorType.Multiply => "*",
                EOperatorType.Divide => "/",
                _ => throw new NotImplementedException(OperatorType.ToString())
            };
        }
    }
}
