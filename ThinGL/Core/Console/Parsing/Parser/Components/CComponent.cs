namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// A structural component of a virtual console command which is emitted by the parser
    /// </summary>
    public abstract class CComponent : CToken
    {
        #region Constructors
        public CComponent(ECTokenType Type) : base(Type)
        {
        }
        #endregion

        public override string Encode()
        {
            throw new System.NotImplementedException();
        }
    }
}
