using System.Numerics;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.ModelHelpers.Materials;
using SAE._5300S1.Utils.SceneHelpers;
using SAE._5300S1.Utils.UI;
using Silk.NET.OpenGL;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using Texture = SAE._5300S1.Utils.ModelHelpers.Texture;


namespace SAE._5300S1.Scene.SceneObjects.Models;

public class MoebiusStrip {
    public Mesh Mesh { get; set; }
    public Material Material { get; set; }


    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;
    private IModel _model;

    // UI values
    private float _speedY;
    private float _speedX;
    private float _scale;

    public MoebiusStrip(GL gl,
        Material material,
        IModel model) {
        _model = model;
        Material = material;
        _gl = gl;
        Init();
    }

    private void Init() {
        Mesh = new Mesh(_gl, _model.Vertices, _model.Indices);
        Mesh.Textures.Add(new Texture(_gl, new List<string> {
            "right.jpg",
            "left.jpg",
            "top.jpg",
            "bottom.jpg",
            "front.jpg",
            "back.jpg"
        }));

        UiMoebiusStrip.SpeedXChangerEvent += value => { _speedX = value; };
        UiMoebiusStrip.SpeedYChangerEvent += value => { _speedY = value; };
        UiMoebiusStrip.ScaleChangerEvent += value => { _scale = value; };
    }


    public unsafe void Render() {
        Mesh.BindVAO();
        Material.Use();

        var degree = 180f;
        var constantRotX = Time.TimeSinceStart * _speedX;
        var constantRotY = Time.TimeSinceStart * _speedY;
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateRotationX(constantRotX.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateRotationY(constantRotY.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateScale(_scale);
        _matrix *= Matrix4x4.CreateTranslation(15, 0, 0);
        _matrix *= Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(degree));

        Material.SetUniform("model", _matrix);
        Material.SetUniform("view", Camera.Instance.GetViewMatrix());
        Material.SetUniform("projection", Camera.Instance.GetProjectionMatrix());
        Material.SetUniform("cameraPos", Camera.Instance.Position);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);
    }
}