using System.Numerics;
using SAE._5300S1.Scene.Objects;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

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
        private static Texture Texture;
        private static Shader Shader;
        private static Shader SkyboxShader;

        // //Setup the camera's location, directions, and movement speed
        // private static Vector3 CameraPosition = new Vector3(0.0f, 0.0f, 20.0f);
        // private static Vector3 CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        // private static Vector3 CameraUp = Vector3.UnitY;
        // private static Vector3 CameraDirection = Vector3.Zero;
        // private static float CameraYaw = -90f;
        // private static float CameraPitch = 0f;
        // private static float CameraZoom = 45f;
        

        //Used to track change in mouse movement to allow for moving of the Camera
        private static Vector2 LastMousePosition;
        private static Mesh _cubeMesh;
        private static Vector3 _color;

        // Skybox

        private static Skybox _skybox;
        private static MoebiusStrip _moebiusStrip;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(Width, Height);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Closing += OnClose;

            window.Run();

            window.Dispose();
        }

        private static void OnLoad()
        {
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
            // var objConverter = new Parser("MoebiusBand.obj");
            _skybox = new Skybox(Gl, "redDesert",StandardMaterial.Instance.Material, SkyBoxSphere.Instance);
            _moebiusStrip = new MoebiusStrip(Gl, "goldenTexture",StandardMaterial.Instance.Material, SkyBoxSphere.Instance);

            

            // Shader = new (Gl, "shader.vert", "shader.frag");
            //SkyboxShader = new (Gl, "skybox.vert", "skybox.frag");

            // List<Texture> textures = new List<Texture>();
            
            // textures.Add(new (Gl, "goldenTexture.jpg"));
            // _cubeMesh = new Mesh(Gl, objConverter.Vertices, objConverter.Indices, textures);
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            var moveSpeed = 2.5f * (float) deltaTime;

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
            
            _skybox.Render();
            _moebiusStrip.Render();
            

            // Vao.Bind();
            // _cubeMesh.Bind();
            //
            // // Texture.Bind();
            //
            //
            // Shader.Use();
            // // Shader.SetUniform("uTexture0", 0);
            //
            // //Use elapsed time to convert to radians to allow our cube to rotate over time
            // var difference = (float) (window.Time * 100);
            //
            // // var model = Matrix4x4.Identity; //Matrix4x4.CreateRotationY(Calculate.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(difference));
            // var model = Matrix4x4.CreateRotationY(Calculate.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(difference));
            // var view = Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            // var projection = Matrix4x4.CreatePerspectiveFieldOfView(Calculate.DegreesToRadians(CameraZoom), Width / Height, 0.1f, 100.0f);
            //
            // Shader.SetUniform("uModel", model);
            // Shader.SetUniform("uView", view);
            // Shader.SetUniform("uProjection", projection);
            // _color = new Vector3(0.5f, 0.7f, 1f);
            // Shader.SetUniform("color", _color);
            //
            // //We're drawing with just vertices and no indices, and it takes 36 vertices to have a six-sided textured cube
            // Gl.DrawArrays(PrimitiveType.Triangles, 0, _cubeMesh.IndicesLength);
        }

        // static Matrix4x4 GetViewMatrix2() {
        //     return Matrix4x4.Identity
        //            * Matrix4x4.CreateFromQuaternion(Rotation)
        //            * Matrix4x4.CreateScale(Scale)
        //            * Matrix4x4.CreateTranslation(Position);
        // }

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
            // Vbo.Dispose();
            // Ebo.Dispose();
            // Vao.Dispose();
            // Shader.Dispose();
            // Texture.Dispose();
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

        
        // public Matrix4x4 CreateTRS() {
        //     // var modelTranslation = Matrix4x4.CreateTranslation(Position);
        //     // var modelRotationX = Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(Rotation.X),new Vector3(1.0f, 0.0f, 0.0f) );
        //     // var modelRotationY = Matrix4x4.CreateRotationY(Calculate.DegreesToRadians(Rotation.Y), new Vector3(0.0f, 1.0f, 0.0f));
        //     // var modelRotationZ = Matrix4x4.CreateRotationZ(Calculate.DegreesToRadians(Rotation.Z), new Vector3(0.0f, 0.0f, 1.0f));
        //     // var modelRotation = modelRotationX * modelRotationY * modelRotationZ;
        //     // var modelScale = Matrix4x4.CreateScale(Scale);
        //     // var model = modelTranslation * modelRotation * modelScale;// Compose TRS matr
        //     // //var model = modelRotation * modelTranslation * modelScale;// Compose TRS matr
        //     // return model;
        //     return 
        // }
        
        
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
        
        #endregion
        
    }
}
