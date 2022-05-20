
using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Engine.Common;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.Engine
{
    public class RenderManager
    {
        #region Data
        private int _disposed = 0;
        private int _initialized = 0;
        #endregion

        #region Values
        private IRHIDriver _rhi = null;
        private WeakReference<EngineInstance> _engineRef;
        private Texture _default_texture = null;
        private HashSet<string> APIExtensions = null;
        public EngineCompatabilityList Compatability { get; protected set; } = null;

        // XXX: Need a generic RHIResourceCache type of thing
        public TextureCache TextureCache { get; private set; }

        #endregion

        #region Properties
        public readonly RHIResourceManager Objects;
        #endregion

        #region Accessors
        public EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public IRHIDriver RHI => _rhi;
        #endregion

        #region Default Texture
        /// <summary>
        /// The default texture is used in scenarios wherein a missing or otherwise invalid texture is referenced.
        /// <para>By default this texture is a Source-esq magenta checkerboard pattern, however this can be overriden by simply setting this Default value reference to some other texture.</para>
        /// </summary>
        public Texture Default_Texture  // move to render manager
        {
            get
            {
                if (_default_texture is null)
                {
                    _default_texture = Engine.Provider.Textures.Create(Engine, PixelDescriptor.Rgb);
                    _default_texture.TryLoad(new TextureDescriptor(PixelDescriptor.Rgb, sizeX: 1, sizeY: 1), new byte[] { 255, 105, 180 });
                }

                return _default_texture;
            }
            set => _default_texture = value;
        }
        #endregion

        #region Constructors
        public RenderManager(EngineInstance engine)
        {
            _engineRef = new WeakReference<EngineInstance>(engine);
            Objects = new RHIResourceManager();
        }
        #endregion

        #region Events
        private void OnInit()
        {
            if (System.Threading.Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var extList = from str in Populate_Extensions() select str.ToLowerInvariant();
                APIExtensions = new HashSet<string>(extList);
            }
        }
        #endregion

        #region Extensions
        /// <summary>
        /// Checks for RHI extension support
        /// </summary>
        /// <param name="extension"></param>
        /// <returns><see langword="true"/> if extension supported</returns>
        public virtual bool IsSupported(string extension)
        {
            if (APIExtensions is null)
            {// Fetch the extensions
                var extList = from str in _rhi.Get_Extensions() select str.ToLowerInvariant();
                APIExtensions = new HashSet<string>(extList);
            }

            return APIExtensions.Contains(extension.ToLowerInvariant());
        }
        #endregion

        #region Textures
        public virtual PixelDescriptor Get_Default_Texture_Internal_Pixel_Layout() => PixelDescriptor.Rgba;
        #endregion

        #region Processing
        public void Process()
        {
            Objects.Process();
        }

        /// <summary>
        /// Fired prior to frame processing
        /// </summary>
        protected void PreFrame()
        {
        }

        /// <summary>
        /// Fired after frame processing
        /// </summary>
        protected void PostFrame()
        {
        }
        #endregion
    }
}
