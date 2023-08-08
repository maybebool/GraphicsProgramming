using System.Numerics;
using SAE._5300S1.Scene.SceneObjects.Models;
using SAE._5300S1.Scene.SceneObjects.ModelSetters;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.ModelHelpers.Materials;
using SAE._5300S1.Utils.UI;
using SAE._5300S1.Utils.UI.InputController;
using SAE._5300S1.Utils.UI.InputControllers;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace SAE._5300S1
{
   static class Program
    {
        

        //Program
        public static IWindow window;
        public static GL Gl;
        
        //Scene
        private const int Width = 1920;
        private const int Height = 1080;
        private static UiMainScene _uiMainScene;
        

        // Scene Models
        private static Skybox _skybox;
        private static Icosahedron _icosahedron;
        private static LightSourceOne _lightSourceOne;
        private static PerfectMirror _perfectMirror;
        private static MarbleSculpture _marbleSculpture;
        private static Diamond _diamond;
        private static Spiral _spiral;
        
        
        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(Width, Height);
            options.Title = "SAE.5300.S1";
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Closing += OnClose;

            window.Run();

            window.Dispose();
        }

        private static void OnLoad() {
            Time.Initialize();
            
            UserInputController.Instance.OnLoadKeyBindings();

            Gl = GL.GetApi(window);

            OnLoadAllSceneModels();
            
            _uiMainScene = new UiMainScene();
            
        }
        
        private static unsafe void OnUpdate(double deltaTime)
        {
            Time.Update();

            UserInputController.Instance.OnUpdateCameraMovement();
            UiController.Instance.ImGuiController.Update((float)deltaTime);
            _uiMainScene.UpdateMainUi();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            
            OnRenderAllSceneModels();

            UiController.Instance.ImGuiController.Render();
        }
        private static void OnClose()
        {
            Gl.Dispose();
        }

        private static void OnLoadAllSceneModels() {
            _skybox = new Skybox(Gl, "cloudySky",StandardMaterial.Instance.Material, SkyBoxParser.Instance);
            _icosahedron = new Icosahedron(Gl, "concrete", ReflectionMaterial.Instance.Material, IcosahedronParser.Instance);
            _lightSourceOne = new LightSourceOne(Gl, "goldenTexture", StandardMaterial.Instance.Material, LightObject1Parser.Instance);
            _perfectMirror = new PerfectMirror(Gl, MirrorMaterial.Instance.Material, PerfectMirrorParser.Instance);
            _marbleSculpture = new MarbleSculpture(Gl, "marmor", ReflectionMaterial.Instance.Material, MarbleSculptureParser.Instance);
            _diamond = new Diamond(Gl, "pink", ReflectionMaterial.Instance.Material, DiamondParser.Instance);
            _spiral = new Spiral(Gl, "wood", ReflectionMaterial.Instance.Material, SpiralParser.Instance);
        }

        private static void OnRenderAllSceneModels() {
            _skybox.Render();
            _icosahedron.Render();
            _lightSourceOne.Render();
            _perfectMirror.Render();
            _marbleSculpture.Render();
            _diamond.Render();
            _spiral.Render();
        }
    }
}
