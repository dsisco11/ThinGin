
using System;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Font;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Types;

namespace ThinGin.Core.Text
{
    public class TextRun : GObject
    {
        #region Static
        /// <summary>
        /// This value holds the global font provider for all text rendering, unless a font provider implementation is assigned here at runtime no text can render...
        /// </summary>
        public static IFontProvider FontProvider = null;
        #endregion

        #region Values
        protected ITexture _texture = null;
        protected Mesh _mesh = null;
        /// <summary> If True, then the text needs to be resolved, meaning its rasterization is somehow different and its bitmap and bounds must be updated! </summary>
        protected bool _invalidated = false;

        private double _sizePt = 12d;
        private string _text = null;
        private IFontRasterizer _font = null;
        private IColorObject _foreColor = Color.White;
        private System.Drawing.RectangleF _bounds = System.Drawing.RectangleF.Empty;
        #endregion

        #region Properties
        public double SizePt { get => _sizePt; private set { _sizePt = value; _invalidated = true; } }
        public string Text { get => _text; private set { _text = value; _invalidated = true; } }
        public IFontRasterizer Font { get => _font; private set { _font = value; _invalidated = true; } }
        public IColorObject ForeColor { get => _foreColor; protected set => _foreColor = value; }

        public System.Drawing.RectangleF Bounds => _bounds;
        #endregion

        #region Constructors
        public TextRun(IEngine Engine) : base(Engine)
        {
        }
        public TextRun(IEngine Engine, IFontRasterizer Font) : base(Engine)
        {
            this.Font = Font;
        }
        public TextRun(IEngine Engine, string Text) : base(Engine)
        {
            Set_Text(Text);
        }
        #endregion

        #region Setters
        public void Set_Font(EFontFamily Family, EFontStyle Style) => Font = FontProvider.Create(Family, Style);
        public void Set_Font(string Family, EFontStyle Style) => Font = FontProvider.Create(Family, Style);

        public void Set_Size(double Pt)
        {
            SizePt = Pt;
        }

        public void Set_ForeColor(IColorObject Color)
        {
            ForeColor = Color;
        }

        public void Set_Text(string Text)
        {
            // First make sure the text is even different
            if (ReferenceEquals(this.Text, Text))
            {
                return;
            }

            if (string.Equals(this.Text, Text))
            {
                return;
            }

            this.Text = Text;
            _mesh = null;
            TryRasterize();
        }
        #endregion

        #region Rasterization
        public bool TryRasterize()
        {
            _bounds = System.Drawing.RectangleF.Empty;

            if (_texture is object)
            {
                _texture.Dispose();
                _texture = null;
            }

            if (_mesh is object)
            {
                _mesh.Dispose();
                _mesh = null;
            }

            if (_font is null)
            {
                return false;
            }

            if (_font.TryRasterize(_text.AsSpan(), _sizePt, out byte[] outBitmap, out System.Drawing.Size outSize, out PixelDescriptor outLayout))
            {
                var meta = new TextureMetadata(outLayout, outSize.Width, outSize.Height);
                PixelDescriptor gpulayout = Engine.IsSupported(outLayout) ? outLayout : PixelDescriptor.Rgba;

                var texture = Engine.Provider.Textures.Create(Engine, gpulayout);
                texture.TryLoad(meta, outBitmap);
                _texture = texture;

                if (_font.TryMeasure(_text.AsSpan(), _sizePt, out var outTextBounds))
                {
                    _mesh = Mesh.Create_Textured_Quad(Engine, outTextBounds);
                }
                else
                {
                    var rect = new System.Drawing.Rectangle(System.Drawing.Point.Empty, outSize);
                    _mesh = Mesh.Create_Textured_Quad(Engine, rect);
                }

                return true;
            }

            return false;
        }
        #endregion

        #region Rendering
        public void Render()
        {
            if (_invalidated)
            {
                TryRasterize();
                _invalidated = false;
            }

            if (_texture is null)
            {
                return;
            }

            // If the mesh is an invalid object at this point then we pretty much WANT to error out so we know something is wrong and dont just mysteriously have missing text.
            _mesh.Render();
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => TryRasterize());
        public override IEngineDelegate Get_Updater() => new EngineDelegate(() => TryRasterize());
        public override IEngineDelegate Get_Releaser() => null;
        #endregion
    }
}
