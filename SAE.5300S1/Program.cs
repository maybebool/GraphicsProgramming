using System.Numerics;
using SAE._5300S1.Scene.SceneObjects.Models;
using SAE._5300S1.Scene.SceneObjects.ModelSetters;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.ModelHelpers.Materials;
using SAE._5300S1.Utils.SceneHelpers;
using SAE._5300S1.Utils.UI;
using SAE._5300S1.Utils.UI.InputController;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace SAE._5300S1
{
   static class Program
    {
        public static IWindow window;
        public static GL Gl;
        private static UiIcosahedron _uiIcosahedron;
        private static UiDiamond _uiDiamond;
        private static UiSpiral _uiSpiral;
        private static UiIcosaStar _uiIcosaStar;
        

        private const int Width = 1920;
        private const int Height = 1080;

        private static BufferObject<float> Vbo;
        private static BufferObject<uint> Ebo;
        private static VertexArrayObject<float, uint> Vao;
        

        //Used to track change in mouse movement to allow for moving of the Camera
        private static Vector2 LastMousePosition;
        private static Mesh _cubeMesh;
        private static Vector3 _color;
        

        // Objects
        private static Skybox _skybox;
        private static Icosahedron _icosahedron;
        private static LightSourceOne _lightSourceOne;
        private static PerfectMirror _perfectMirror;
        private static IcosaStar _icosaStar;
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

            _skybox = new Skybox(Gl, "cloudySky",StandardMaterial.Instance.Material, SkyBoxParser.Instance);
            _icosahedron = new Icosahedron(Gl, "redSand", ReflectionMaterial.Instance.Material, IcosahedronParser.Instance);
            _lightSourceOne = new LightSourceOne(Gl, "goldenTexture", StandardMaterial.Instance.Material, LightObject1Parser.Instance);
            _perfectMirror = new PerfectMirror(Gl, MirrorMaterial.Instance.Material, PerfectMirrorParser.Instance);
            _icosaStar = new IcosaStar(Gl, "redSand", ReflectionMaterial.Instance.Material, IcosaStarParser.Instance);
            _diamond = new Diamond(Gl, "redSand", ReflectionMaterial.Instance.Material, DiamondParser.Instance);
            _spiral = new Spiral(Gl, "redSand", ReflectionMaterial.Instance.Material, SpiralParser.Instance);

            _uiIcosahedron = new UiIcosahedron();
            _uiDiamond = new UiDiamond();
            _uiSpiral = new UiSpiral();
            _uiIcosaStar = new UiIcosaStar();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            Time.Update();

            UserInputController.Instance.OnUpdateCameraMovement();
            UiController.Instance.ImGuiController.Update((float)deltaTime);
            _uiIcosahedron.UpdateUi();
            _uiDiamond.UpdateUi();
            _uiSpiral.UpdateUi();
            _uiIcosaStar.UpdateUi();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            
            _skybox.Render();
            _icosahedron.Render();
            _lightSourceOne.Render();
            _perfectMirror.Render();
            _icosaStar.Render();
            _diamond.Render();
            _spiral.Render();
            
            
            
            _uiIcosahedron.RenderUi();
            _uiDiamond.RenderUi();
            _uiSpiral.RenderUi();
            _uiIcosaStar.RenderUi();
        }

        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
            var lookSensitivity = 0.1f;
            if (LastMousePosition == default) { LastMousePosition = position; }
            else
            {
                var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;
                Camera.Instance.ModifyDirection(xOffset, yOffset);
            }
        }

        private static unsafe void OnMouseWheel(IMouse mouse, ScrollWheel scrollWheel)
        {
            Camera.Instance.ModifyZoom(scrollWheel.Y);
        }

        private static void OnClose()
        {
            Gl.Dispose();
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                window.Close();
            }
        }
        
        #region Transformation Matrix
        private static Matrix4x4 GetViewMatrix()
        {
            var viewTranslation = Matrix4x4.Identity;
            var viewRotation = Matrix4x4.Identity;
            var viewScale = Matrix4x4.Identity;

            viewTranslation = Matrix4x4.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
            //viewRotation = Matrix4x4.ro(new Vector3(0.0f, 0.0f, 1.0f), 0.0f);
            viewScale = Matrix4x4.CreateScale(new Vector3(1.0f, 1.0f, 1.0f));

            //Matrix4 view = viewTranslation * viewRotation * viewScale;// TRS matrix -> scale, rotate then translate -> All applied in WORLD Coordinates
            var view = viewRotation * viewTranslation * viewScale;// RTS matrix -> scale, rotate then translate -> All applied in LOCAL Coordinates

            return view;
        }
        
        
        private static Matrix4x4 GetProjectionMatrix()
        {
            float fov = 45;

            float aspectRatio = (float)Width / (float)Height;
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(Mathf.ToRad(fov), (float)width / height, 0.1f, 1000f);
            var projection = Matrix4x4.CreateOrthographic(1, 1, 0.1f, 1000f);
            
            //projection = Matrix4.CreateOrthographic0.0f, (float)screenWidth, 0.0f, (float)screenHeight, 0.1f, 100.0f);

            return projection;
        }


        private static void SetLight() {
            
        }
        
        #endregion
        
    }
}
