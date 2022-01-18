﻿using System;

using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Textures.Types
{
    /// <summary>
    /// This is essentially a low-level (device) texture handle, used to link up bits of the engine that don't otherwise connect properly.
    /// </summary>
    public abstract class TextureHandle : EngineObject, ITexture
    {
        #region Values
        protected int _handle = 0;
        #endregion

        #region Properties
        /// <summary> <inheritdoc cref="ITexture.Handle"/> </summary>
        public int Handle => _handle;
        #endregion

        #region Accessors
        /// <summary> <inheritdoc cref="ITexture.Metadata"/> </summary>
        public TextureMetadata Metadata { get; } = new TextureMetadata();

        public int Kind { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region Constructors
        protected TextureHandle(IRenderEngine Engine) : base(Engine)
        {
        }
        #endregion
    }

}