using System.Numerics;
using SAE._5300S1.Utils.MathHelpers;

namespace SAE._5300S1.Utils.SceneHelpers; 

public class Camera {
    public static Camera Instance => Lazy.Value;
    public Vector3 Position { get; set; } = new(0.0f, 0.0f, 3.0f);
    public float AspectRatio { get; set; } = 2F;
    public Vector3 Front => _cameraFront;
    public Vector3 Up => _cameraUp;
    public float Yaw => _cameraYaw;
    public float Pitch => _cameraPitch;
    public float Zoom => _cameraZoom;

    private static readonly Lazy<Camera> Lazy = new(() => new Camera());
    private Vector3 _cameraFront = new(0.0f, 0.0f, -1.0f);
    private Vector3 _cameraUp = Vector3.UnitY;
    private float _cameraYaw = -90f;
    private float _cameraPitch;
    private float _cameraZoom = 45f;

    private Camera() {
    }

    public void SetUp(Vector3 position,
      Vector3 front,
      Vector3 up,
      float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio;
      _cameraFront = front;
      _cameraUp = up;
    }

    public void ModifyZoom(float zoomAmount) {
      _cameraZoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);
    }

    public void ModifyDirection(float xOffset, float yOffset) {
      _cameraYaw += xOffset;
      _cameraPitch -= yOffset;

      // clamping for natural camera behaviour, avoiding over head movement
      _cameraPitch = Math.Clamp(Pitch, -89f, 89f);
      var cameraDirection = Vector3.Zero;
      cameraDirection.X = MathF.Cos(Yaw.DegreesToRadiansOnVariable()) * MathF.Cos(Pitch.DegreesToRadiansOnVariable());
      cameraDirection.Y = MathF.Sin(Pitch.DegreesToRadiansOnVariable());
      cameraDirection.Z = MathF.Sin(Yaw.DegreesToRadiansOnVariable()) * MathF.Cos(Pitch.DegreesToRadiansOnVariable());
      _cameraFront = Vector3.Normalize(cameraDirection);
    }

    public Matrix4x4 GetViewMatrix() {
      return Matrix4x4.CreateLookAt(Position, Position + Front, Up);
    }

    public Matrix4x4 GetProjectionMatrix() {
      return Matrix4x4.CreatePerspectiveFieldOfView(Zoom.DegreesToRadiansOnVariable(), AspectRatio, 0.1f, 10000.0f);
    }
}