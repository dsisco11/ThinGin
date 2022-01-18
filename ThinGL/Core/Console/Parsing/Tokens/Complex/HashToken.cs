using System;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class HashToken : ValuedTokenBase
    {
        #region Properties
        public readonly EHashTokenType HashType = EHashTokenType.Value;
        #endregion

        public HashToken(EHashTokenType HashType, ReadOnlySpan<char> Value) : base(ECTokenType.Hash, Value)
        {
            this.HashType = HashType;
        }


        #region Equality Operators
        public override bool Equals(object o)
        {
            if (o is HashToken Other)
            {
                return Type == Other.Type && HashType == Other.HashType && Value.Equals(Other.Value, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
        #endregion
    }
}
