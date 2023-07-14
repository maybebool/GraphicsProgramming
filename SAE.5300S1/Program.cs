using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;


namespace SAE._5300S1;

public class Program
{
    // Window + OpenGL
    private static GL _gl;
    private static IWindow _window;
    private static IKeyboard primaryKeyboard;
    
    // Buffers + Program
    private static uint _vao;
    private static uint _vbo, _ebo;
    private static uint _program;
    
    
    // Inputs
    private static Vector2 LastMousePosition;
    private static Vector3 CameraPosition = new(0.0f, 0.0f, 3.0f);
    private static Vector3 CameraFront = new(0.0f, 0.0f, -1.0f);
    private static Vector3 CameraUp = Vector3.UnitY;
    private static Vector3 CameraDirection = Vector3.Zero;
    private static float CameraYaw = -90f;
    private static float CameraPitch = 0f;
    private static float CameraZoom = 45f;
    
    static float[] vertices =
    {
        0.5f,  0.5f, 0.0f,
        0.5f, -0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
        -0.5f,  0.5f, 0.0f
    };
    
    
    
    static uint[] indices =
    {
        0u, 1u, 3u,
        1u, 2u, 3u
    };

    public static void Main(string[] args) {
        
        TestExample();
        
        var options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "My first Silk.NET application!"
        };
        
        _window = Window.Create(options);
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Run();
        
        

    }


    private static unsafe void OnLoad() {
        IInputContext input = _window.CreateInput();
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
        
        _gl = GL.GetApi(_window);
        

        _gl = _window.CreateOpenGL();
        _gl.ClearColor(Color.CornflowerBlue);
        
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        fixed (float* buf = vertices)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf,
                BufferUsageARB.StaticDraw);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        fixed (uint* buf = indices)
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf,
                BufferUsageARB.StaticDraw);

        const string vertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";
        
        
        const string fragmentCode = @"
#version 330 core

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 0.5, 0.2, 1.0);
}";
        
      
        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, vertexCode);
        
        
        _gl.CompileShader(vertexShader);

        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
        if (vStatus != (int) GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + _gl.GetShaderInfoLog(vertexShader));
        
        
        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, fragmentCode);

        _gl.CompileShader(fragmentShader);

        _gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
        if (fStatus != (int) GLEnum.True)
            throw new Exception("Fragment shader failed to compile: " + _gl.GetShaderInfoLog(fragmentShader));
        
        _program = _gl.CreateProgram();
        
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);

        _gl.LinkProgram(_program);

        _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int lStatus);
        if (lStatus != (int) GLEnum.True)
            throw new Exception("Program failed to link: " + _gl.GetProgramInfoLog(_program));
        
        _gl.DetachShader(_program, vertexShader);
        _gl.DetachShader(_program, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
        
        
        const uint positionLoc = 0;
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*) 0);
        
        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
 
    }



    private static void OnUpdate(double dt) {
        
        var moveSpeed = 2.5f * (float) dt;

        if (primaryKeyboard.IsKeyPressed(Key.W))
        {
            //Move forwards
            CameraPosition += moveSpeed * CameraFront;
        }
        if (primaryKeyboard.IsKeyPressed(Key.S))
        {
            //Move backwards
            CameraPosition -= moveSpeed * CameraFront;
        }
        if (primaryKeyboard.IsKeyPressed(Key.A))
        {
            //Move left
            CameraPosition -= Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
        }
        if (primaryKeyboard.IsKeyPressed(Key.D))
        {
            //Move right
            CameraPosition += Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
        }
    }

    private static unsafe void OnRender(double dt) {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        
        _gl.BindVertexArray(_vao);
        _gl.UseProgram(_program);
        _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*) 0);
        var projection = Matrix4x4.CreatePerspectiveFieldOfView(Calculate.DegreesToRadians(CameraZoom), (float) _window.Size.X / (float)_window.Size.Y, 0.1f, 100.0f);
        
    }



    #region Input Control Methods

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode) {
        if (key == Key.Escape)
            _window.Close();
    }
    
    private static unsafe void OnMouseWheel(IMouse mouse, ScrollWheel scrollWheel)
    {
        
        CameraZoom = Math.Clamp(CameraZoom - scrollWheel.Y, 1.0f, 45f);
    }
    
    private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
    {
        var lookSensitivity = 0.1f;
        if (LastMousePosition == default)
        {
            LastMousePosition = position;
        }
        else
        {
            var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
            var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
            LastMousePosition = position;

            CameraYaw += xOffset;
            CameraPitch -= yOffset;

            // lock rotation for natural camera behaviour
            CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

            CameraDirection.X = MathF.Cos(Calculate.DegreesToRadians(CameraYaw)) * MathF.Cos(Calculate.DegreesToRadians(CameraPitch));
            CameraDirection.Y = MathF.Sin(Calculate.DegreesToRadians(CameraPitch));
            CameraDirection.Z = MathF.Sin(Calculate.DegreesToRadians(CameraYaw)) * MathF.Cos(Calculate.DegreesToRadians(CameraPitch));
            CameraFront = Vector3.Normalize(CameraDirection);
        }
    }

    #endregion

    
    
    
    


    private static void TestExample() {
        var obj = new Parser("MoebiusBand.obj");
        Console.WriteLine("adsfjsdvfsdf");
    }
    
    
    
}