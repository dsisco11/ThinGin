using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MeshRendering
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            // Initialize GLFW
            GLFW.Init();

            // Create a new gamewindow for rendering
            var window = new ThinGinExample();
            window.Run();
        }

    }
}
