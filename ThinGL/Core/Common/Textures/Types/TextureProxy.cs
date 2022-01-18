using System;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;

namespace ThinGin.Core.Common.Textures.Types
{
    /// <summary>
    /// Creates or links to an existing <see cref="Texture"/> instance.
    /// </summary>
    public class TextureProxy : EngineObject, ITexture, IDisposable
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
        public int Priority { get => Root?.Priority ?? 0; set => Root.Priority = value; }

        /// <summary> <inheritdoc cref="ITexture.Metadata"/> </summary>
        public TextureMetadata Metadata => Root.Metadata;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new shared texture instance but does NOT increment its user count, which is why this constructor is internal...
        /// </summary>
        /// <param name="Root"></param>
        internal TextureProxy(IRenderEngine Engine, string Identifier, Texture Root) : base(Engine)
        {
            this.Identifier = Identifier;
            _rootRef = new WeakReference<Texture>(Root);
        }

        public TextureProxy(IRenderEngine Engine, string Identifier) : base(Engine)
        {
            this.Identifier = Identifier;
            _rootRef = Engine.TextureCache.Lookup(Identifier);
        }

        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => null;
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<string>(Identifier, Engine.TextureCache.Dereference);
        #endregion

    }

}
