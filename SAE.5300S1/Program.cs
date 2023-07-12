using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;


namespace SAE._5300S1;

public class Program
{
    
    private static GL _gl;
    private static IWindow _window;

    public static void Main(string[] args) {
        
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


    private static void OnLoad() {
        IInputContext input = _window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private static void OnUpdate(double dt) { }

    private static void OnRender(double dt) { }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode) {
        if (key == Key.Escape)
            _window.Close();
    }
}