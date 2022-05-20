using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures.Decoders;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Resources.Common;

namespace ThinGin.Core.Common.Textures
{
    /// <summary>
    /// Facilitates the instantiation of textures
    /// </summary>
    public static class Factory
    {
        #region Properties
        private static HashSet<ITextureDecoder> Decoders = new HashSet<ITextureDecoder>();
        #endregion

        #region Decoding
        /// <summary>
        /// Registers a new image data decoder
        /// </summary>
        /// <param name="Decoder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Register(ITextureDecoder Decoder)
        {
            if (Decoder is null)
            {
                throw new ArgumentNullException(nameof(Decoder));
            }

            Decoders.Add(Decoder);
        }

        /// <summary>
        /// Unregisters an image data decoder
        /// </summary>
        /// <param name="Decoder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Unregister(ITextureDecoder Decoder)
        {
            if (Decoder is null)
            {
                throw new ArgumentNullException(nameof(Decoder));
            }

            Decoders.Remove(Decoder);
        }


        /// <summary>
        /// Traverses the list of registered image decoders and attempts to find one which can successfully decode the encoded image file data provided.
        /// </summary>
        /// <param name="EncodedData"></param>
        /// <param name="DecodedData"></param>
        /// <returns></returns>
        public static bool TryDecode(ReadOnlyMemory<byte> EncodedData, out byte[] DecodedData, out TextureDescriptor Metadata)
        {
            foreach (var decoder in Decoders)
            {
                if (decoder.TryDecode(EncodedData, out byte[] outData, out TextureDescriptor outMetadata))
                {
                    DecodedData = outData;
                    Metadata = outMetadata;
                    return true;
                }
            }

            DecodedData = null;
            Metadata = new TextureDescriptor();
            return false;
        }
        #endregion

        #region Creation (Cached)

        /// <summary>
        /// Creates a new texture instance, retrieving it from the cache if possible.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="Name">String identifier such as a filename or fullpath (or a search string if your resource loaders support it) which the resource loading system will use to fetch the texture</param>
        /// <param name="Settings"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="Exception"></exception>
        public static TextureProxy Create(EngineInstance engine, string Name) => Create(engine, Name, engine.Renderer.Get_Default_Texture_Internal_Pixel_Layout());

        /// <summary>
        /// Creates a new texture instance, retrieving it from the cache if possible.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="Name">String identifier such as a filename or fullpath (or a search string if your resource loaders support it) which the resource loading system will use to fetch the texture</param>
        /// <param name="Settings"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="Exception"></exception>
        public static TextureProxy Create(EngineInstance engine, string Name, PixelDescriptor GpuLayout)
        {
            if (!engine.Renderer.TextureCache.IsCached(Name))
            {
                if (!ResourceSystem.TryRead(Name, out byte[] RawData))
                {
                    throw new System.IO.IOException($"Failed to read data for texture (Identifier: {Name})!");
                }

                if (!TryDecode(RawData, out byte[] DecodedData, out TextureDescriptor Metadata))
                {
                    throw new Exception($"Failed to decode texture (Identifier: {Name})!");
                }

                Texture tex = engine.Provider.Textures.Create(engine, GpuLayout);
                tex.TryLoad(Metadata, DecodedData);
                if (!engine.Renderer.TextureCache.TryRegister(Name, tex))
                {
                    throw new Exception($"Unable to register texture in cache! (Identifier: {Name})");
                }
            }

            if (!engine.Renderer.TextureCache.TryReference(Name, out Texture outRoot))
            {
                throw new Exception($"Unable to register new user for cached texture! (Identifier: {Name})");
            }

            return new TextureProxy(engine, Name, outRoot);
        }
        #endregion

    }
}
