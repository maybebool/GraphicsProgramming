using System.Numerics;
using SAE._5300S1.Scene.SceneObjects.Models;
using SAE._5300S1.Scene.SceneObjects.ModelSetters;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers.Materials;
using SAE._5300S1.Utils.UI;
using SAE._5300S1.Utils.UI.InputControllers;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Camera = SAE._5300S1.Utils.SceneHelpers.Camera;

namespace SAE._5300S1 {
    static class Program {
        
        //Program
        public static IWindow window;
        public static GL Gl;
        public static int Width => _width;
        public static int Height => _height;

        //Scene
        private static int _width = 1920;
        private static int _height = 1080;
        private static UiMainScene _uiMainScene;


        // Scene Models
        private static Skybox _skybox;
        private static Icosahedron _icosahedron;
        private static MoebiusStrip _moebiusStrip;
        private static Skull _skull;
        private static Diamond _diamond;
        private static Spiral _spiral;


        private static void Main(string[] args) {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(_width, _height);
            options.Title = "SAE.5300.S1";
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Closing += OnClose;
            window.Resize += OnResize;

            window.Run();
            window.Dispose();
        }

        private static void OnResize(Vector2D<int> size) {
            Gl.Viewport(size);
            _width = size.X;
            _height = size.Y;
            Camera.Instance.AspectRatio = (float) _width / _height;
        }

        private static void OnLoad() {
            Time.Initialize();

            UserInputController.Instance.OnLoadKeyBindings();
            Gl = GL.GetApi(window);
            OnLoadAllSceneModels();
            _uiMainScene = new UiMainScene();
        }

        private static unsafe void OnUpdate(double deltaTime) {
            Time.Update();
            UserInputController.Instance.OnUpdateCameraMovement();
            UiController.Instance.ImGuiController.Update((float)deltaTime);
            _uiMainScene.UpdateMainUi();
        }

        private static unsafe void OnRender(double deltaTime) {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            OnRenderAllSceneModels();
            UiController.Instance.ImGuiController.Render();
        }

        private static void OnClose() {
            Gl.Dispose();
        }

        // Bind objects with given Texture, Material and model (.obj)
        private static void OnLoadAllSceneModels() {
            _skybox = new Skybox(Gl, "cloudySky", StandardMaterial.Instance.Material, SkyBoxParser.Instance);
            _icosahedron = new Icosahedron(Gl, "concrete", LightMaterial.Instance.Material, IcosahedronParser.Instance);
            _moebiusStrip = new MoebiusStrip(Gl, MirrorMaterial.Instance.Material, MoebiusStripParser.Instance);
            _skull = new Skull(Gl, "marble", LightMaterial.Instance.Material, SkullParser.Instance);
            _diamond = new Diamond(Gl, "pink", LightMaterial.Instance.Material, DiamondParser.Instance);
            _spiral = new Spiral(Gl, "wood", LightMaterial.Instance.Material, SpiralParser.Instance);
        }

        private static void OnRenderAllSceneModels() {
            _skybox.Render();
            _icosahedron.Render();
            _moebiusStrip.Render();
            _skull.Render();
            _diamond.Render();
            _spiral.Render();
        }
    }
}