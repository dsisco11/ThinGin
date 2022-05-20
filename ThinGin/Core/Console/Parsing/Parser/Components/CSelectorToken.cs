using System.Collections.Generic;

namespace ThinGin.Core.Console.Parsing
{

    /// <summary>
    /// Selectors start with the AT symbol '@' and are followed by either some kind of single identifier, number, or simple block (blocks that essentially form a LINQ function)
    /// </summary>
    public class CSelectorToken : CComponent
    {
        #region Properties
        public readonly ECSelectorType SelectorType;
        public CReference Target = null;
        public List<CToken> Values = new List<CToken>();
        #endregion

        #region Constructors
        public CSelectorToken(ECSelectorType selectorType, IEnumerable<CToken> values) : base(ECTokenType.Selector)
        {
            SelectorType = selectorType;
            Values = new List<CToken>(values);
        }

        public CSelectorToken(ECSelectorType selectorType, IEnumerable<CToken> values, CReference target) : base(ECTokenType.Selector)
        {
            SelectorType = selectorType;
            Target = target;
            Values = new List<CToken>(values);
        }

        public CSelectorToken(CToken cToken) : base(ECTokenType.Selector)
        {
            SelectorType = ECSelectorType.Simple;
            if (cToken is CSimpleBlock block)
            {
                if (block.StartToken.Type == ECTokenType.Parenth_Open)
                    SelectorType = ECSelectorType.Filter;
                else
                    SelectorType = ECSelectorType.Compound;
            }

            Values = new List<CToken>();
            Values.Add(cToken);
        }

        public CSelectorToken(IEnumerable<CToken> values) : base(ECTokenType.Selector)
        {
            SelectorType = ECSelectorType.Simple;
            Values = new List<CToken>(values);
        }
        #endregion
    }
}
