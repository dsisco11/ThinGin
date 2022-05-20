using System;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Types;
using ThinGin.Core.RenderHardware.DMA;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware
{
    /// <summary>
    /// Provides a common, generic, access layer for graphics hardware commands.
    /// <note>In UnrealEngine the methods in this interface would be known as the RenderThread methods within the "DynamicRHI"</note>
    /// </summary>
    public interface IRHIDriver
    {
        #region Properties
        /// <summary>
        /// The RHI driver name
        /// </summary>
        string Name { get; }
        RHIDriverFeatures Limits { get; }
        #endregion

        #region Lifetime
        /// <summary>
        /// Called before the first frame, performs any initialization required.
        /// </summary>
        public void Init();
        /// <summary>
        /// Called before discarding the driver instance, does anything the driver requires. At this point no more commands will be submitted to the driver.
        /// </summary>
        public void Cleanup();
        /// <summary>
        /// Called from the main thread.
        /// </summary>
        /// <param name="DeltaTime"></param>
        public void Tick(double DeltaTime);
        #endregion

        #region Extensions
        string[] Get_Extensions();
        #endregion

        #region Support
        /// <summary>
        /// Checks if the driver supports the given data layout for texture pixel data.
        /// </summary>
        bool Supports_Pixel_Format(in PixelDescriptor pixelDescriptor);

        /// <summary>
        /// Intakes a pixel format and returns the closest format the driver supports which still includes all of the data represented by the requested one
        /// </summary>
        /// <param name="pixelDescriptor">The pixel format being requested </param>
        /// <returns>The closest supported pixel format that still includes all of the data represented by the requested one </returns>
        PixelDescriptor Translate_Pixel_Format(in PixelDescriptor pixelDescriptor);
        #endregion

        #region Error Handling
        /// <summary>
        /// Checks for errors reported by the graphics driver.
        /// </summary>
        bool ErrorCheck(out string Message);
        #endregion

        void Bind_Shader(in RHIHandle handle);
        void Bind_Sampler(in RHISamplerState sampler);

        void Bind_Uniform_Buffer(in RHIHandle handle);

        void Bind_GBuffer(in RHIHandle handle);

        void Bind_Buffer(in RHIHandle handle);

        void Upload_Buffer_Data(in RHIHandle handle, EBufferType type, EBufferUpdate usage, ERHIAccess access, int length, ReadOnlySpan<byte> data);

        void Submit_Uniform_Value(in RHIHandle handle, in DataChunk data);

        #region Textures
        void Bind_Texture(in RHITexture Texture);
        RHITexture1D Create_Texture1D(in TextureDescriptor Descriptor, in uint NumSamples, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITexture1DArray Create_Texture1DArray(in TextureDescriptor Descriptor, in uint NumSamples, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITexture2D Create_Texture2D(in TextureDescriptor Descriptor, in uint NumSamples, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITexture2DArray Create_Texture2DArray(in TextureDescriptor Descriptor, in uint NumSamples, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITexture3D Create_Texture3D(in TextureDescriptor Descriptor, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITextureCube Create_TextureCube(in TextureDescriptor Descriptor, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        RHITextureCubeArray Create_TextureCubeArray(in TextureDescriptor Descriptor, in ERHIAccess InitialState, in ETextureCreationFlags Flags, out DMAInterface outDMA);
        #endregion

        #region Clearing
        void Clear_Color(in Rgba color);
        void Clear_Depth_Stencil(float depth, int stencil);
        #endregion


        #region Resource Creation
        #endregion

        #region Buffer Lock/Unlock (Aka Buffer Mapping) (Aka DMA)
        void Lock_Buffer(in RHIHandle handle, ERHIAccess Access, out IntPtr dmaAddress);
        void Unlock_Buffer(in RHIHandle handle);
        #endregion
    }
}
