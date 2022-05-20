﻿using System;

using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class FunctionNameToken : ValuedTokenBase
    {
        public FunctionNameToken(ReadOnlySpan<char> Value) : base(ECTokenType.FunctionName, Value)
        {
        }

        public override string Encode()
        {
            return string.Concat(Value, UnicodeCommon.CHAR_LEFT_PARENTHESES);
        }
    }
}
