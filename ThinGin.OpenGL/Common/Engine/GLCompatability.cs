using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Engine.Common;

namespace ThinGin.OpenGL.Common.Engine
{
    public class GLCompatability : EngineCompatabilityList
    {
        public GLCompatability()
        {
            MaxTextureSize = GL.GetInteger(GetPName.MaxTextureSize);
            MaxArrayTextureLayers = GL.GetInteger(GetPName.MaxArrayTextureLayers);

            Max3DTextureSize = GL.GetInteger(GetPName.Max3DTextureSize);
            MaxCubeMapTextureSize = GL.GetInteger(GetPName.MaxCubeMapTextureSize);

            MaxTextureBufferSize = GL.GetInteger(GetPName.MaxTextureBufferSize);
            MaxRenderbufferSize = GL.GetInteger(GetPName.MaxRenderbufferSize);

            MaxTextureUnits = GL.GetInteger(GetPName.MaxTextureUnits);
            MaxTextureImageUnits = GL.GetInteger(GetPName.MaxTextureImageUnits);

            MaxElementsIndices = GL.GetInteger(GetPName.MaxElementsIndices);
            MaxElementsVertices = GL.GetInteger(GetPName.MaxElementsVertices);

            MaxVertexAttribs = GL.GetInteger(GetPName.MaxVertexAttribs);
            MaxUniformBufferBindings = GL.GetInteger(GetPName.MaxUniformBufferBindings);


            var vMajor = GL.GetInteger(GetPName.MajorVersion);
            var vMinor = GL.GetInteger(GetPName.MinorVersion);


            if (vMajor >= 3)
            {
                if (vMinor >= 1)
                {
                    UniformBufferObjects = true;
                }

                if (vMinor >= 3)
                {
                    AttributeDivisors = true;
                }
            }
        }
    }
}
