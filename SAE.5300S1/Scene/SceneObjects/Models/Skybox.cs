using System.Numerics;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.SceneHelpers;
using Silk.NET.OpenGL;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using Texture = SAE._5300S1.Utils.ModelHelpers.Texture;


namespace SAE._5300S1.Scene.SceneObjects.Models; 

public class Skybox {
    public Mesh Mesh { get; set; }
    public Material Material { get; set; }

    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;
    private IModel _model;
    

    public Skybox(GL gl,
        string textureName,
        Material material,
        IModel model) {
        _model = model;
        _textureName = textureName;
        Material = material;
        _gl = gl;
        Init();
    }

    private void Init() {
        Mesh = new Mesh(_gl, _model.Vertices , _model.Indices);
        _texture = new Texture(_gl, $"{_textureName}.jpg");
        
    }
    

    public unsafe void Render() {
        
        _gl.DepthMask(false);
        Mesh.Bind();
        Material.Use();
        var degreeX = 180f;
        var degreeY = 90f;
        
        _matrix = Matrix4x4.Identity; 
        _matrix *= Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(degreeX));
        //_matrix *= Matrix4x4.CreateRotationY(Calculate.DegreesToRadians(degreeY));
        _matrix *= Matrix4x4.CreateScale(4000f);

        Material.SetUniform("uModel", _matrix);
        Material.SetUniform("uView", Camera.Instance.GetViewMatrix());
        Material.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());
        Material.SetUniform("fColor", new Vector3(1.0f, 1.0f, 1.0f));
        _texture.Bind();

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);

        
        _gl.DepthMask(true); // set depth function back to default
    }
}