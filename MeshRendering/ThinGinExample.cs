using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

using ThinGin.Core.Common;
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Geometry.Filters;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Shaders;

namespace MeshRendering
{
    public class ThinGinExample : GameWindow
    {
        #region Settings
        public static Size DefaultSize = new Size(1280, 720);
        public static Version TargetGLVersion = new Version(3, 3);

        static GameWindowSettings defaultGameSettings = new GameWindowSettings() { IsMultiThreaded = true };
        static NativeWindowSettings defaultNativeSettings = new NativeWindowSettings()
        {
            Title = $"ThinGin | OpenGL: {TargetGLVersion}",
            Flags = OpenTK.Windowing.Common.ContextFlags.ForwardCompatible,
            AutoLoadBindings = true,
            //API = OpenTK.Windowing.Common.ContextAPI.OpenGL,
            APIVersion = TargetGLVersion,
            Profile = OpenTK.Windowing.Common.ContextProfile.Core,
            Size = new OpenTK.Mathematics.Vector2i(DefaultSize.Width, DefaultSize.Height),
            IsEventDriven = false,
        };
        #endregion

        #region Values
        private IRenderEngine _engine = null;
        private IShader shader = null;
        private Mesh _cubeMesh = null;
        private Mesh _gizmoMesh = null;
        private Mesh _gridMesh = null;

        private Mesh _camDebugMesh = null;

        private ShaderVars shaderVars = null;
        #endregion

        #region Properties
        public IRenderEngine Engine { get => _engine; protected set => _engine = value; }

        float modelRotation = 0f;
        Matrix4x4 modelMatrix = Matrix4x4.Identity;
        #endregion

        #region Constructors
        public ThinGinExample() : base(defaultGameSettings, defaultNativeSettings)
        {
        }
        #endregion

        #region Events
        protected unsafe override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GLFW.GetFramebufferSize(WindowPtr, out int screenWidth, out int screenHeight);
            Engine.Set_Viewport(new Rectangle(0, 0, screenWidth, screenHeight));
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var dT = (float)args.Time;
            var deltaR = 10 * (float)args.Time;
            modelRotation = (modelRotation + deltaR) % 360;
            modelMatrix = Matrix4x4.CreateRotationZ(MathE.ToRadians(modelRotation));

            var cam = Engine.Camera;

            const float moveSpeed = 0.5f;
            float moveScalar = moveSpeed * dT;
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                var mvec = cam.Transform.Get_Forward();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                var mvec = cam.Transform.Get_Forward();
                mvec *= moveScalar;
                cam.Transform.Position -= mvec;
            }

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                var mvec = cam.Transform.Get_Right();
                mvec *= moveScalar;
                cam.Transform.Position -= mvec;
            }
            else if (KeyboardState.IsKeyDown(Keys.D))
            {
                var mvec = cam.Transform.Get_Right();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }

            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                var mvec = Engine.Space.Up;
                mvec *= moveScalar;

                if (KeyboardState.IsKeyDown(Keys.LeftShift))
                    cam.Transform.Position -= mvec;
                else
                    cam.Transform.Position += mvec;
            }

            if (KeyboardState.IsKeyDown(Keys.KeyPad4))
            {
                var mvec = cam.Transform.Get_Right();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }
            else if (KeyboardState.IsKeyDown(Keys.KeyPad6))
            {
                var mvec = cam.Transform.Get_Left();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }

            if (KeyboardState.IsKeyDown(Keys.KeyPad8))
            {
                var mvec = cam.Transform.Get_Up();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }
            else if (KeyboardState.IsKeyDown(Keys.KeyPad2))
            {
                var mvec = cam.Transform.Get_Down();
                mvec *= moveScalar;
                cam.Transform.Position += mvec;
            }

            const float rotSpeed = 45.0f;
            var rotScalar = rotSpeed * dT;

            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                cam.Transform.Rotate(cam.Transform.Get_Right(), -rotScalar);
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                cam.Transform.Rotate(cam.Transform.Get_Right(), rotScalar);
            }

            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                cam.Transform.Rotate(Engine.Space.Up, rotScalar);
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                cam.Transform.Rotate(Engine.Space.Up, -rotScalar);
            }

        }
        #endregion

        #region Input

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Keys.R)
            {
                Engine.Camera.Transform.Position = Vector3.Zero;
                Engine.Camera.Transform.Orientation = Quaternion.Identity;
            }

            if (e.Key == Keys.V)
            {
                var vmi = (int)Engine.Camera.ViewMode;
                var vmb = (vmi == 1);
                var mode = (ECameraViewMode)(!vmb ? 1 : 0);

                Engine.Camera.ViewMode = mode;
                System.Diagnostics.Trace.TraceInformation($"ECameraViewMode: {mode}");
            }

            if (e.Key == Keys.P)
            {
                var pmi = (int)Engine.Camera.ProjectionMode;
                var pmb = (pmi == 1);
                var mode = (EProjectionMode)(!pmb ? 1 : 0);

                Engine.Camera.ProjectionMode = mode;
                System.Diagnostics.Trace.TraceInformation($"EProjectionMode: {mode}");
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            switch (Engine.Camera.ViewMode)
            {
                case ECameraViewMode.FirstPerson:
                    {
                        Engine.Camera.FieldOfView += (e.OffsetY * 5f);
                        System.Diagnostics.Trace.TraceInformation($"Camera.FOV: {Engine.Camera.FieldOfView}");
                        break;
                    }
                case ECameraViewMode.Orbital:
                case ECameraViewMode.Orbital_Free:
                    {
                        Engine.Camera.Transform.Position += (Engine.Space.Up * (e.OffsetY * 0.2f));
                        break;
                    }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (MouseState.IsButtonDown(MouseButton.Middle))
            {
                const float sensitivity = 0.35f;
                var delta = e.Delta * sensitivity;

                if (delta.Y != 0)
                {
                    Engine.Camera.Transform.Rotate(Engine.Camera.Transform.Get_Right(), delta.Y);
                }

                if (delta.X != 0)
                {
                    Engine.Camera.Transform.Rotate(Engine.Space.Up, delta.X);
                }
            }
        }
        #endregion

        #region Main Loop

        protected unsafe override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            ProcessEvents(0);

            Engine.Camera.Invalidate();
            shaderVars["MVP_Matrix"].Set(Engine.Camera.Matrix);
            //shaderVars["modelMatrix"].Set(modelMatrix);
            _camDebugMesh.Update(Engine.Camera.Get_Debug_Vis());

            Engine.Think();
            Engine.BeginFrame();

            Engine.Bind(shader);
            Engine.Bind(shaderVars);

            _gridMesh.Render();
            _gizmoMesh.Render();
            _cubeMesh.Render();


            var vp = Engine.Get_Viewport();
            //shaderVars["MVP_Matrix"].Set(Matrix4x4.CreatePerspective(vp.Width, vp.Height, 0.001f, 10f));
            shaderVars["MVP_Matrix"].Set(Matrix4x4.CreatePerspectiveFieldOfView(MathE.ToRadians(45f), Engine.Get_AspectRatio(), 0.001f, 10f));
            Engine.Bind(shaderVars);
            _camDebugMesh.Render();

            Engine.EndFrame();


            Context.SwapBuffers();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Engine = new ThinGin.OpenGL.GL3.GL3Engine(null);
            Engine.Initialize();
            Engine.Camera.ProjectionMode = EProjectionMode.Perspective;
            Engine.Camera.ViewMode = ECameraViewMode.FirstPerson;
            Engine.Camera.Transform.Position = new Vector3(1f, 0f, 0f);

            _cubeMesh = new Mesh(Engine, Generate_Cube(0.5f));
            _gizmoMesh = new Mesh(Engine, Generate_AxisGizmo(1f));
            _gridMesh = new Mesh(Engine, Generate_The_Grid(50, 0.5f));

            _camDebugMesh = new Mesh(Engine, Engine.Camera.Get_Debug_Vis());

            string vertexSliceName = "default.vert.glsl";
            string fragmentSliceName = "default.frag.glsl";

            string vertexSlice = System.IO.File.ReadAllText(vertexSliceName);
            string fragmentSlice = System.IO.File.ReadAllText(fragmentSliceName);


            shader = new Shader(Engine);
            shader.Include(EShaderType.Vertex, vertexSliceName, vertexSlice);
            shader.Include(EShaderType.Fragment, fragmentSliceName, fragmentSlice);

            shader.Compile();
            shaderVars = new ShaderVars(shader);


            shaderVars["modelMatrix"].Set(Matrix4x4.Identity);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            //Engine.Dispose();
        }
        #endregion


        #region Geometry
        static MeshBuilder Generate_Triangle(float Size)
        {
            var points = new[] {
                new DataChunk(Size, 0f, 0f),// top front left
                new DataChunk(0f, Size, 0f),// top front right
                new DataChunk(Size, Size, 0f),// top back right
            };

            var vertexLayout = new VertexLayout(
                Position: AttributeDescriptor.FLOAT3
                );

            var builder = new MeshBuilder(vertexLayout, ETopology.Triangles, AutoOptimize: false);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), new[] { 0, 1, 2 });

            return builder;
        }

        static MeshBuilder Generate_AxisGizmo(float Size)
        {
            var points = new[] {
                new DataChunk(0f, 0f, 0f),
                new DataChunk(Size, 0f, 0f),
                new DataChunk(0f, Size, 0f),
                new DataChunk(0f, 0f, Size),
            };

            var normals = new[] {
                new DataChunk(0f, 0f, 0f),
            };

            var colors = new[] {
                new DataChunk(1f, 0f, 0f),
                new DataChunk(0f, 1f, 0f),
                new DataChunk(0f, 0f, 1f),
            };

            var vertexLayout = new VertexLayout(
                Position: AttributeDescriptor.FLOAT3,
                //Normal: AttributeDescriptor.FLOAT3,
                Color: AttributeDescriptor.FLOAT3
                );

            var builder = new MeshBuilder(vertexLayout, ETopology.Lines, AutoOptimize: false);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), new[] { 0, 1, 0, 2, 0, 3 });

            //builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Normal), normals);
            //builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Normal), new[] { 0, 0, 0, 0, 0, 0 });

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Color), colors);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Color), new[] { 0, 0, 1, 1, 2, 2 });

            return builder;
        }

        static MeshBuilder Generate_Cube(float Volume)
        {
            // Form a list of all 8 vertex positions for the cube
            var points = new[] {
                new DataChunk(Volume, -Volume, Volume),// top front left
                new DataChunk(Volume, Volume, Volume),// top front right

                new DataChunk(-Volume, Volume, Volume),// top back right
                new DataChunk(-Volume, -Volume, Volume),// top back left

                new DataChunk(Volume, -Volume, -Volume),// bottom front left
                new DataChunk(Volume, Volume, -Volume),// bottom front right

                new DataChunk(-Volume, Volume, -Volume),// bottom back right
                new DataChunk(-Volume, -Volume, -Volume),// bottom back left
            };

            // Form a list of all 6 face normals for the cube
            var normals = new[] {
                new DataChunk(0f, 0f, +1f),// +Z
                new DataChunk(0f, 0f, -1f),// -Z

                new DataChunk(+1f, 0f, 0f),// +X
                new DataChunk(-1f, 0f, 0f),// -X

                new DataChunk(0f, +1f, 0f),// +Y
                new DataChunk(0f, -1f, 0f),// -Y
            };

            // Form a list of all 8 corner colors for the cube
            var colors = new[] {
                new DataChunk(1f, 0f, 1f),// top front left
                new DataChunk(1f, 1f, 1f),// top front right

                new DataChunk(0f, 1f, 1f),// top back right
                new DataChunk(0f, 0f, 1f),// top back left

                new DataChunk(1f, 0f, 0f),// bottom front left
                new DataChunk(1f, 1f, 0f),// bottom front right

                new DataChunk(0f, 1f, 0f),// bottom back right
                new DataChunk(0f, 0f, 0f),// bottom back left
            };


            // All data handled by ThinGin is specified and held within DataChunks!
            // DataChunk allow the engine to maintain a seperation of concerns, most parts of the engine do not care what kind of value type the data is or how many values there are,
            // As far as the engine is concerned its all just bits and bytes, so data chunks allow all systems to easily interact with any possible data specification.

            // The MeshBuilder class enables the easy generation and manipulation of mesh geometry along with compiling said geometry data into an
            // efficiently interleaved and thus unified data buffer which can be uploaded to the rendering device.
            // == More clearly explained ==
            // GPU wants all vertex data in its RAM, and also wants to access all vertex data from a single location.
            // so all vertex attributes such as position, normal, color, etc. Should ideally be in the same data array (buffer) and close together for cache efficiency.
            // If the GPU goes to render a vertex and reads the position of said vertex but then needs the normal and that normal data is in on the other end of the buffer,
            // or worse in a DIFFERENT buffer, then it has to jump around a lot and thats SLOW!
            // So to render fast we need all of the info for a given vertex to be packed close together.
            // This is called interleaving.
            // The process of interleaving all of this data can be annoying, so the MeshBuilder does it for you automatically!

            // Another thing to know is that the engine does still need to know what the format of a vertex is in order to properly describe it to the rendering hardware.
            // We describe the contents of a vertex using a VertexLayout instance.
            // Using a vertex layout we can describe to the engine what the data we are giving to it means.
            var vertexLayout = new VertexLayout(
                Position: AttributeDescriptor.FLOAT3,
                //Normal: AttributeDescriptor.FLOAT3,
                Color: AttributeDescriptor.FLOAT3
                );

            var builder = new MeshBuilder(vertexLayout, ETopology.Quads, AutoOptimize: true);

            // Modern OpenGL has depreciated drawing quads (for good reasons), so we have to send it triangles.
            // But we also don't want to generate the mesh as triangles because its annoying and quads are easier for humans to visualize.
            // So ThinGin provides a simple triangulation geometry filter we can use to do it for us.
            // We use the triangulator by adding an instance of it to the mesh builders filters list.
            // The triangulator will take in any kind of primitive type supported by the engine
            // and rebuild the meshes index list to represent the starting primitives as a series of triangles.
            builder.Filters.Add(new TriangulationFilter());

            // Now we upload all position data for the cubes 8 points into the builder at once.

            // According to the vertex layout we gave the builder, attribute 0 is for positional data.
            // But if we didnt know that then we could query the layout object for that info like so.
            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), 
                new[] {
                    0, 1, 2, 3,// top
                    7, 6, 5, 4,// bottom
                    4, 5, 1, 0,// front
                    5, 6, 2, 1,// right
                    6, 7, 3, 2,// back
                    7, 4, 0, 3,// left
            });

            // Upload normals
            //builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Normal), normals);
            //builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Normal),
            //    new[] {
            //        0, 0, 0, 0,// top
            //        1, 1, 1, 1,// bottom
            //        2, 2, 2, 2,// front
            //        4, 4, 4, 4,// right
            //        3, 3, 3, 3,// back
            //        5, 5, 5, 5,// left
            //});

            // Upload colors
            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Color), colors);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Color),
                new[] {
                    0, 1, 2, 3,// top
                    7, 6, 5, 4,// bottom
                    4, 5, 1, 0,// front
                    5, 6, 2, 1,// right
                    6, 7, 3, 2,// back
                    7, 4, 0, 3,// left
            });

            return builder;
        }

        static MeshBuilder Generate_The_Grid(int count, float stepSize)
        {
            var points = new List<DataChunk>();
            float countF = (float)count;
            float PS = (stepSize * +countF);// Positive side
            float NS = (stepSize * -countF);// Negative side

            for (int i = -count; i < count; i++)
            {
                float f = (float)i;
                var ax = stepSize * f;
                // X
                points.Add(new DataChunk(PS, ax, 0f));
                points.Add(new DataChunk(NS, ax, 0f));

                // Y
                points.Add(new DataChunk(ax, PS, 0f));
                points.Add(new DataChunk(ax, NS, 0f));
            }

            var normals = new[] {
                new DataChunk(0f, 0f, 0f)
            };

            float gv = 0.2f;
            var colors = new[] {
                new DataChunk(1f, gv, gv),
                new DataChunk(gv, 1f, gv),
            };

            int[] vIdx = new int[points.Count];
            int[] nIdx = new int[points.Count];
            int[] cIdx = new int[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                vIdx[i] = i;
                nIdx[i] = 0;
                cIdx[i] = (i / 2) % 2;
            }

            var vertexLayout = new VertexLayout(
            Position: AttributeDescriptor.FLOAT3,
            //Normal: AttributeDescriptor.FLOAT3,
            Color: AttributeDescriptor.FLOAT3
            );

            var builder = new MeshBuilder(vertexLayout, ETopology.Lines, AutoOptimize: false);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), vIdx);

            //builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Normal), normals);
            //builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Normal), nIdx);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Color), colors);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Color), cIdx);

            return builder;
        }
        #endregion
    }
}
