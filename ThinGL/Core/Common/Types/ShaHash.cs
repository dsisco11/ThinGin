using System;
using System.Security.Cryptography;
using System.Text;

namespace ThinGin.Core.Common
{
    public class ShaHash
    {

        #region Values
        public readonly Memory<byte> Data;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public ShaHash(in string Source)
        {
            var bytes = Encoding.Default.GetBytes(Source);
            using (var crypt = SHA256.Create())
            {
                Data = crypt.ComputeHash(bytes);
            }
        }
        #endregion


        public bool Equals(ShaHash other)
        {
            return Data.Span.SequenceEqual(other.Data.Span);
        }

        public bool Equals(ReadOnlySpan<byte> hash)
        {
            return Data.Span.SequenceEqual(hash);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

    }
}
