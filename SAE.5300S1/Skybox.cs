using System.Numerics;
using Silk.NET.Assimp;
using Silk.NET.OpenGL;

using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;


namespace SAE._5300S1; 

public class Skybox {
    //public Material Material { get; set; }
    public Mesh Mesh { get; set; }
    // private Shader SkyboxShader { get; set; }
    public Material Material { get; set; }

    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;
    private IModel _model;

    //private IModel _model;

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
        //Material  = new (Program.Gl, "skybox.vert", "skybox.frag");
        //var skyMeshConverter = new Parser("spheres.obj");
        _texture = new Texture(_gl, $"{_textureName}.jpg");
        
    }
    

    public unsafe void Render() {
        // draw skybox as last
        _gl.DepthMask(false);
        // _gl.Disable(EnableCap.DepthTest);
        Mesh.Bind();
        // SkyboxShader.Use();
        Material.Use();

        var degree = 180f;
        
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(degree));
        _matrix *= Matrix4x4.CreateScale(600f);

        Material.SetUniform("uModel", _matrix);
        Material.SetUniform("uView", Camera.Instance.GetViewMatrix());
        Material.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());
        Material.SetUniform("fColor", new Vector3(1.0f, 1.0f, 1.0f));
        _texture.Bind();

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);

        // _gl.Enable(EnableCap.DepthTest);
        _gl.DepthMask(true); // set depth function back to default
    }
}