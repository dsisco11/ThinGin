using System.Collections.Generic;
using System.Text;

namespace ThinGin.Core.Console.Parsing
{
    public class CFunction : CComponent
    {
        #region Properties
        public readonly string Name;
        public List<CToken> Arguments = new List<CToken>();
        #endregion

        #region Constructors
        public CFunction(string name) : base(ECTokenType.Function)
        {
            Name = name;
        }
        #endregion


        public override string Encode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append("(");
            foreach (var t in Arguments) { sb.Append(t.Encode()); }
            sb.Append(")");

            return sb.ToString();
        }
    }
}
