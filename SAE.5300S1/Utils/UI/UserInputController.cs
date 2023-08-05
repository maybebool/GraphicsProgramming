using System.Numerics;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.SceneHelpers;
using Silk.NET.Input;
namespace SAE._5300S1.Utils.UI; 

public class UserInputController {
    public static UserInputController Instance => Lazy.Value;
    private static readonly Lazy<UserInputController> Lazy = new(() => new UserInputController());
    
    public IInputContext InputContext => _inputContext;
    public IKeyboard PrimaryKeyboard => _primaryKeyboard;
    public Vector2 LastMousePosition => _lastMousePosition;

    
    private bool _insertMode = true;
    private Vector2 _lastMousePosition;
    private IInputContext _inputContext;
    private IKeyboard _primaryKeyboard;
    
    
    private UserInputController() {
        _inputContext = Program.window.CreateInput();
        _primaryKeyboard = _inputContext.Keyboards.FirstOrDefault();
    }
    
    public void OnLoadKeyBindings() {
      if (_primaryKeyboard != null) {
        _primaryKeyboard.KeyDown += KeyDown;
      }

      for (int i = 0; i < _inputContext.Mice.Count; i++) {
        _inputContext.Mice[i].Cursor.CursorMode = CursorMode.Normal;
        _inputContext.Mice[i].MouseMove += OnMouseMove;
        _inputContext.Mice[i].Scroll += OnMouseWheel;
      }
    }

    public void OnUpdateCameraMovement() {
      if (_insertMode)
        return;

      var moveSpeed = 10.5f * Time.DeltaTime;
      var multiplier = 1;
      if (_primaryKeyboard.IsKeyPressed(Key.ShiftLeft)) {
        multiplier = 4;
      }

      if (_primaryKeyboard.IsKeyPressed(Key.W)) {
        Camera.Instance.Position += moveSpeed * multiplier * Camera.Instance.Front;
      }

      if (_primaryKeyboard.IsKeyPressed(Key.S)) {
        Camera.Instance.Position -= moveSpeed * multiplier * Camera.Instance.Front;
      }

      if (_primaryKeyboard.IsKeyPressed(Key.A)) {
        Camera.Instance.Position -= Vector3.Normalize(Vector3.Cross(Camera.Instance.Front, Camera.Instance.Up)) *
                                    moveSpeed * multiplier;
      }

      if (_primaryKeyboard.IsKeyPressed(Key.D)) {
        Camera.Instance.Position += Vector3.Normalize(Vector3.Cross(Camera.Instance.Front, Camera.Instance.Up)) *
                                    moveSpeed * multiplier;
      }
    }

    private void KeyDown(IKeyboard arg1, Key arg2, int arg3) {
      if (arg2 == Key.Escape) {
        Program.window.Close();
      }
      else if (arg2 == Key.Space) {
        _insertMode = !_insertMode;
        _inputContext.Mice[0].Cursor.CursorMode =
          _inputContext.Mice[0].Cursor.CursorMode == CursorMode.Normal ? CursorMode.Raw : CursorMode.Normal;
      }
    }

    private unsafe void OnMouseMove(IMouse mouse, Vector2 position) {
      if (_insertMode)
        return;

      const float lookSensitivity = 0.1f;
      if (_lastMousePosition == default) {
        _lastMousePosition = position;
      }
      else {
        var xOffset = (position.X - _lastMousePosition.X) * lookSensitivity;
        var yOffset = (position.Y - _lastMousePosition.Y) * lookSensitivity;
        _lastMousePosition = position;
        Camera.Instance.ModifyDirection(xOffset, yOffset);
      }
    }

    private unsafe void OnMouseWheel(IMouse mouse, ScrollWheel scrollWheel) {
      if (_insertMode)
        return;
      Camera.Instance.ModifyZoom(scrollWheel.Y);
    }
  
}