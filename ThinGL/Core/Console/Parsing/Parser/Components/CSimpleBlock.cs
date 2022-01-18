using System.Collections.Generic;
using System.Text;

namespace ThinGin.Core.Console.Parsing
{
    public class CSimpleBlock : CComponent
    {
        public readonly CToken StartToken;
        public List<CToken> Values = new List<CToken>();

        public CSimpleBlock(CToken StartToken) : base(ECTokenType.SimpleBlock)
        {
            this.StartToken = StartToken;
        }


        public override string Encode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StartToken.Encode());
            sb.AppendLine();
            foreach (CToken t in Values) { sb.Append(t.Encode()); }
            sb.AppendLine();
            switch (StartToken.Type)
            {
                case ECTokenType.Parenth_Open:
                    sb.Append(")");
                    break;
                case ECTokenType.Bracket_Close:
                    sb.Append("}");
                    break;
                case ECTokenType.SqBracket_Close:
                    sb.Append("]");
                    break;
            }

            return sb.ToString();
        }
    }
}
