using System;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware;

namespace ThinGin.Core.Common.Textures.Types
{
    /// <summary>
    /// Creates or links to an existing <see cref="Texture"/> instance.
    /// </summary>
    public class TextureProxy : RHIResource, ITexture, IDisposable
    {
        #region Properties
        /// <summary>
        /// A string identifier used by the texture cache to differentiate textures.
        /// </summary>
        public readonly string Identifier;
        #endregion

        #region Settings
        /// <summary> <inheritdoc cref="ITexture.Kind"/> </summary>
        public int Kind { get; set; }
        #endregion

        #region Parent
        /// <summary>
        /// The texture instance this object is linked to
        /// </summary>
        public readonly WeakReference<Texture> _rootRef = null;

        public Texture Root => _rootRef.TryGetTarget(out Texture parent) ? parent : null;
        #endregion

        #region Accessors

        /// <summary> <inheritdoc cref="ITexture.Handle"/> </summary>
        public int Handle => Root?.Handle ?? 0;

        /// <summary> <inheritdoc cref="ITexture.Priority"/> </summary>
        public int Priority { get => Root?.ResourcePriority ?? 0; set => Root.ResourcePriority = value; }

        /// <summary> <inheritdoc cref="ITexture.Metadata"/> </summary>
        public TextureDescriptor Metadata => Root.Descriptor;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new shared texture instance but does NOT increment its user count, which is why this constructor is internal...
        /// </summary>
        /// <param name="Root"></param>
        internal TextureProxy(EngineInstance engine, string Identifier, Texture Root) : base(engine)
        {
            this.Identifier = Identifier;
            _rootRef = new WeakReference<Texture>(Root);
        }

        public TextureProxy(EngineInstance engine, string Identifier) : base(engine)
        {
            this.Identifier = Identifier;
            _rootRef = engine.TextureCache.Lookup(Identifier);
        }

        #endregion

        #region IEngineResource
        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => new EngineReleaser<string>(Identifier, RHI.TextureCache.Dereference);
        #endregion

    }

}
