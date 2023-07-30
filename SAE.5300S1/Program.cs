using System.Numerics;
using SAE._5300S1.Scene.SceneObjects;
using SAE._5300S1.Scene.SceneObjects.Models;
using SAE._5300S1.Scene.SceneObjects.ModelSetters;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.SceneHelpers;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Shader = SAE._5300S1.Utils.ModelHelpers.Shader;
using Texture = SAE._5300S1.Utils.ModelHelpers.Texture;

namespace SAE._5300S1
{
   static class Program
    {
        private static IWindow window;
        public static GL Gl;
        private static IKeyboard primaryKeyboard;

        private const int Width = 1920;
        private const int Height = 1080;

        private static BufferObject<float> Vbo;
        private static BufferObject<uint> Ebo;
        private static VertexArrayObject<float, uint> Vao;



        //Used to track change in mouse movement to allow for moving of the Camera
        private static Vector2 LastMousePosition;
        private static Mesh _cubeMesh;
        private static Vector3 _color;

        // Skybox

        private static Skybox _skybox;
        private static Icosahedron _icosahedron;
        private static LightObject _lightObject;
        


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
            Calculate.DeltaTime = 0f;
            Time.Initialize();
            IInputContext input = window.CreateInput();
            primaryKeyboard = input.Keyboards.FirstOrDefault();
            if (primaryKeyboard != null)
            {
                primaryKeyboard.KeyDown += KeyDown;
            }
            for (int i = 0; i < input.Mice.Count; i++)
            {
                input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                input.Mice[i].MouseMove += OnMouseMove;
                input.Mice[i].Scroll += OnMouseWheel;
            }

            Gl = GL.GetApi(window);
            
            //LightingShader = new Shader(Gl, "shader.vert", "lightingShader.frag");
            
            _skybox = new Skybox(Gl, "cloudySky",StandardMaterial.Instance.Material, SkyBoxParser.Instance);
            _icosahedron = new Icosahedron(Gl, "metallic", ReflectionMaterial.Instance.Material, IcosahedronParser.Instance);
            _lightObject = new LightObject(Gl, "redSand", StandardMaterial.Instance.Material, LightObjectSetter.Instance);
            //_lightSource = new LightSource(Gl, "metallic", LightingMaterial.Instance.Material, LightSourceParser.Instance);

        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            var moveSpeed = 10.5f * (float) deltaTime;
            _icosahedron.OnUpdate();
            Calculate.UpdateDeltaTime(deltaTime);
            Time.Update();

            if (primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                Camera.Instance.Position += moveSpeed * Camera.Instance.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                Camera.Instance.Position -= moveSpeed * Camera.Instance.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                Camera.Instance.Position -= Vector3.Normalize(Vector3.Cross(Camera.Instance.Front, Camera.Instance.Up)) * moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                Camera.Instance.Position += Vector3.Normalize(Vector3.Cross(Camera.Instance.Front, Camera.Instance.Up)) * moveSpeed;
            }
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            
            //_icosidodecahedron.Render();
            _icosahedron.Render();
            _skybox.Render();
            _lightObject.Render();
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
