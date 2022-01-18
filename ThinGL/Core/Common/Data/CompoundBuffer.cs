namespace ThinGin.Core.Common.Data
{
    public class CompoundBuffer
    {
        #region Values
        public readonly CompoundDataDescriptor Layout;
        public readonly byte[] Data;
        #endregion

        #region Constructors
        public CompoundBuffer(CompoundDataDescriptor layout, byte[] data)
        {
            Layout = layout;
            Data = data;
        }
        #endregion
    }
}
