using System.Text;

namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// A hash-token single value reference.
    /// <notes>Examples: "#player:0", "#player:[0:100]", "#player:[10:]"</notes>
    /// </summary>
    public class CReference : CComponent
    {
        #region Properties
        public readonly CToken Name;
        public readonly CToken Value;
        #endregion

        #region Constructors
        public CReference(CToken name, CToken value) : base(ECTokenType.Reference)
        {
            Name = name;
            Value = value;
        }
        #endregion


        public override string Encode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("#");

            if (Name is object)
            {
                sb.Append(Name);
            }

            if (Value is object)
            {
                sb.Append(":");
                sb.Append(Value.Encode());
            }

            return sb.ToString();
        }
    }
}
