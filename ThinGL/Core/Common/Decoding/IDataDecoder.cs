using System;

namespace ThinGin.Core.Common.Decoding
{
    public interface IDataDecoder
    {
        /// <summary>
        /// Attempts to decode the data contained within <paramref name="Encoded"/> into a useful format.
        /// </summary>
        /// <param name="Encoded">Encoded data</param>
        /// <param name="Decoded">Decoded data</param>
        /// <returns>Success</returns>
        bool TryDecode(ReadOnlyMemory<byte> Encoded, out byte[] Decoded);
    }

    public interface IDataDecoder<MetaDataTy>
    {
        /// <summary>
        /// Attempts to decode the data contained within <paramref name="Encoded"/> into a useful format.
        /// </summary>
        /// <param name="Encoded">Encoded data</param>
        /// <param name="Decoded">Decoded data</param>
        /// <param name="Metadata">Metadata</param>
        /// <returns>Success</returns>
        bool TryDecode(ReadOnlyMemory<byte> Encoded, out byte[] Decoded, out MetaDataTy Metadata);
    }
}
