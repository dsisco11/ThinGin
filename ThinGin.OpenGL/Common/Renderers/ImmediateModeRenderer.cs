
using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Exceptions;
using ThinGin.Core.Rendering.Common;

namespace ThinGin.OpenGL.Common.Renderers
{
    /// <summary>
    /// Implements immediate mode (fixed pipeline) rendering for compiled mesh data.
    /// <para>
    /// NOTE: Immediate mode / Fixed pipeline rendering is depreciated as of OpenGL v3.2.
    /// This is an extremely crude form of rendering, unsuitable for use on modern hardware.
    /// Please for the love of science do NOT use this, it is here only for the utmost of legacy support!
    /// </para>
    /// </summary>
    public class ImmediateModeRenderer : MeshRenderer
    {
        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public ImmediateModeRenderer(IEngine Engine, Mesh mesh) : base(Engine, mesh)
        {
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => null;
        public override IEngineDelegate Get_Releaser() => null;
        public override IEngineDelegate Get_Updater() => null;
        #endregion

        #region May god have mercy on my soul
        private unsafe void sendVertex(byte* vPtr)
        {
            var attrib = mesh.Layout.Get(EVertexAttribute.Position);
            switch (attrib.ValueType)
            {
                case EValueType.SHORT:
                case EValueType.USHORT:
                    {
                        var ptr = ((short*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Vertex4(ptr);
                                break;
                            case 3:
                                GL.Vertex3(ptr);
                                break;
                            default:
                                GL.Vertex2(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.FLOAT:
                    {
                        var ptr = ((float*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Vertex4(ptr);
                                break;
                            case 3:
                                GL.Vertex3(ptr);
                                break;
                            default:
                                GL.Vertex2(ptr);
                                break;
                        }
                    }
                    break;
                //case EEngineDataType.FLOAT_HALF:
                //    {
                //        var ptr = ((System.Half*)vPtr);
                //        switch (attrib.ComponentCount)
                //        {
                //            case 4:
                //                GL.Vertex4(ptr);
                //                break;
                //            case 3:
                //                GL.Vertex3(ptr);
                //                break;
                //            default:
                //                GL.Vertex2(ptr);
                //                break;
                //        }
                //    }
                //    break;
                case EValueType.DOUBLE:
                    {
                        var ptr = ((double*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Vertex4(ptr);
                                break;
                            case 3:
                                GL.Vertex3(ptr);
                                break;
                            default:
                                GL.Vertex2(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.INT:
                case EValueType.UINT:
                    {
                        var ptr = ((int*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Vertex4(ptr);
                                break;
                            case 3:
                                GL.Vertex3(ptr);
                                break;
                            default:
                                GL.Vertex2(ptr);
                                break;
                        }
                    }
                    break;
            }
        }

        private unsafe void sendNormal(byte* vPtr)
        {
            var attrib = mesh.Layout.Get(EVertexAttribute.Normal);
            switch (attrib.ValueType)
            {
                case EValueType.BYTE:
                case EValueType.SBYTE:
                    {
                        var ptr = ((byte*)vPtr);
                        if (attrib.Count > 2)
                        {
                            GL.Normal3(ptr);
                        }
                    }
                    break;
                case EValueType.SHORT:
                case EValueType.USHORT:
                    {
                        var ptr = ((short*)vPtr);
                        if (attrib.Count > 2)
                        {
                            GL.Normal3(ptr);
                        }
                    }
                    break;
                case EValueType.FLOAT:
                    {
                        var ptr = ((float*)vPtr);
                        if (attrib.Count > 2)
                        {
                            GL.Normal3(ptr);
                        }
                    }
                    break;
                //case EEngineDataType.FLOAT_HALF:
                //    {
                //        var ptr = ((System.Half*)vPtr);
                //        if (attrib.ComponentCount > 2)
                //        {
                //            GL.Normal3(ptr);
                //        }
                //    }
                //    break;
                case EValueType.DOUBLE:
                    {
                        var ptr = ((double*)vPtr);
                        if (attrib.Count > 2)
                        {
                            GL.Normal3(ptr);
                        }
                    }
                    break;
                case EValueType.INT:
                case EValueType.UINT:
                    {
                        var ptr = ((int*)vPtr);
                        if (attrib.Count > 2)
                        {
                            GL.Normal3(ptr);
                        }
                    }
                    break;
            }
        }

        private unsafe void sendColor(byte* vPtr)
        {
            var attrib = mesh.Layout.Get(EVertexAttribute.Color);
            switch (attrib.ValueType)
            {
                case EValueType.BYTE:
                    {
                        var ptr = ((byte*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.SBYTE:
                    {
                        var ptr = ((sbyte*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.SHORT:
                    {
                        var ptr = ((short*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.USHORT:
                    {
                        var ptr = ((ushort*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.FLOAT:
                    {
                        var ptr = ((float*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                //case EEngineDataType.FLOAT_HALF:
                //    {
                //        var ptr = ((System.Half*)vPtr);
                //        switch (attrib.ComponentCount)
                //        {
                //            case 4:
                //                GL.Color4(ptr);
                //                break;
                //            case 3:
                //                GL.Color3(ptr);
                //                break;
                //        }
                //    }
                //    break;
                case EValueType.DOUBLE:
                    {
                        var ptr = ((double*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.INT:
                    {
                        var ptr = ((int*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.UINT:
                    {
                        var ptr = ((uint*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.Color4(ptr);
                                break;
                            case 3:
                                GL.Color3(ptr);
                                break;
                        }
                    }
                    break;
            }
        }

        private unsafe void sendUV(byte* vPtr)
        {
            var attrib = mesh.Layout.Get(EVertexAttribute.UVMap);
            switch (attrib.ValueType)
            {
                case EValueType.SHORT:
                case EValueType.USHORT:
                    {
                        var ptr = ((short*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.TexCoord4(ptr);
                                break;
                            case 3:
                                GL.TexCoord3(ptr);
                                break;
                            default:
                                GL.TexCoord2(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.FLOAT:
                    {
                        var ptr = ((float*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.TexCoord4(ptr);
                                break;
                            case 3:
                                GL.TexCoord3(ptr);
                                break;
                            default:
                                GL.TexCoord2(ptr);
                                break;
                        }
                    }
                    break;
                //case EEngineDataType.FLOAT_HALF:
                //    {
                //        var ptr = ((System.Half*)vPtr);
                //        switch (attrib.ComponentCount)
                //        {
                //            case 4:
                //                GL.TexCoord4(ptr);
                //                break;
                //            case 3:
                //                GL.TexCoord3(ptr);
                //                break;
                //            default:
                //                GL.TexCoord2(ptr);
                //                break;
                //        }
                //    }
                //    break;
                case EValueType.DOUBLE:
                    {
                        var ptr = ((double*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.TexCoord4(ptr);
                                break;
                            case 3:
                                GL.TexCoord3(ptr);
                                break;
                            default:
                                GL.TexCoord2(ptr);
                                break;
                        }
                    }
                    break;
                case EValueType.INT:
                case EValueType.UINT:
                    {
                        var ptr = ((int*)vPtr);
                        switch (attrib.Count)
                        {
                            case 4:
                                GL.TexCoord4(ptr);
                                break;
                            case 3:
                                GL.TexCoord3(ptr);
                                break;
                            default:
                                GL.TexCoord2(ptr);
                                break;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Rendering
        public override void Render(ETopology Primitive)
        {
            var primitive = Common.Bridge.Translate(Primitive);
            GL.Begin(primitive);

            Initiate_Trainwreck();

            GL.End();
        }

        internal unsafe void Initiate_Trainwreck()
        {
            var Layout = mesh.Layout;

            for (int vidx = 0; vidx < mesh.Count; vidx++)
            {
                int voff = (vidx * Layout.Size);// Vertex data offset

                if (Layout.Has(EVertexAttribute.Position))
                {
                    int offset = voff + Layout.Get(EVertexAttribute.Position).Offset;
                    fixed (byte* ptr = &mesh.Data[offset])
                    {
                        sendVertex(ptr);
                    }
                }

                if (Layout.Has(EVertexAttribute.Normal))
                {
                    int offset = voff + Layout.Get(EVertexAttribute.Normal).Offset;
                    fixed (byte* ptr = &mesh.Data[offset])
                    {
                        sendNormal(ptr);
                    }
                }

                if (Layout.Has(EVertexAttribute.Color))
                {
                    int offset = voff + Layout.Get(EVertexAttribute.Color).Offset;
                    fixed (byte* ptr = &mesh.Data[offset])
                    {
                        sendColor(ptr);
                    }
                }

                if (Layout.Has(EVertexAttribute.UVMap))
                {
                    int offset = voff + Layout.Get(EVertexAttribute.UVMap).Offset;
                    fixed (byte* ptr = &mesh.Data[offset])
                    {
                        sendUV(ptr);
                    }
                }
            }

        }
        #endregion
    }
}
