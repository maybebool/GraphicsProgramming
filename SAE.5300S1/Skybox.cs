using System.Numerics;
using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;


namespace SAE._5300S1; 

public class Skybox {
    //public Material Material { get; set; }
    public Mesh Mesh { get; set; }

    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;

    private Shader _skyboxShader;
    //private IModel _model;

    public Skybox(GL gl,
        string textureName, Shader skyboxShader
        //Material material
        /*IModel model*/) {
        //_model = model;
        _textureName = textureName;
        _skyboxShader = skyboxShader;
        //Material = material;
        _gl = gl;
        Init();
    }

    private void Init() {
        List<Texture> textures = new List<Texture>();
        var skyMeshConverter = new Parser("spheres.obj");
        Mesh = new Mesh(_gl, skyMeshConverter.Vertices , skyMeshConverter.Indices, textures);
        textures.Add(new (_gl, $"{_textureName}.jpg"));
        //_texture = new Texture(_gl, $"{_textureName}.jpg");
    }
    

    public unsafe void Render() {
        // draw skybox as last
        // _gl.DepthMask(false);
        _gl.Disable(EnableCap.DepthTest);
        Mesh.Bind();
        _skyboxShader.Use();
        //Material.Use();

        var degree = 180f;
        
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateRotationX(Calculate.DegreesToRadians(degree));
        _matrix *= Matrix4x4.CreateScale(500f);

        _skyboxShader.SetUniform("uModel", _matrix);
        _skyboxShader.SetUniform("uView", Camera.Instance.GetViewMatrix());
        _skyboxShader.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());
        //Material.SetUniform("fColor", new Vector3(0.5f, 0.5f, 0.5f));

        _texture.Bind();
        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);

        _gl.Enable(EnableCap.DepthTest);
        // _gl.DepthMask(true); // set depth function back to default
    }
}