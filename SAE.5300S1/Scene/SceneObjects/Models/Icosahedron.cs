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

public class Icosahedron {
    public Mesh Mesh { get; set; }
    public Material Material { get; set; }
    
    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;
    private IModel _model;
    
    // Ui/Shader values
    private float _shininessMaterial;
    private Vector3 _ambientLightColor;
    private Vector3 _diffuseLightColor;
    private Vector3 _specularLightColor;
    private float _specularLightMultiplier;
    private bool _useBlinnCalculation;
    private bool _useDirectionalLight;
    private bool _useOrbit;
    private float _speed;
    private float _orbit;

    public Icosahedron(GL gl,
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
        Mesh = new Mesh(_gl, _model.Vertices, _model.Indices);
        _texture = new Texture(_gl, $"{_textureName}.jpg");

        UiIcosahedron.ShininessMaterialEvent += value => { _shininessMaterial = value; };
        UiIcosahedron.AmbientLightColorEvent += value => { _ambientLightColor = value; };
        UiIcosahedron.DiffuseLightColorEvent += value => { _diffuseLightColor = value; };
        UiIcosahedron.SpecularLightColorEvent += value => { _specularLightColor = value; };
        UiIcosahedron.SpecularLightMultiplierEvent += value => { _specularLightMultiplier = value; };
        UiIcosahedron.UseBlinnCalculationEvent += value => { _useBlinnCalculation = value; };
        UiIcosahedron.UseDirectionalLightEvent += value => { _useDirectionalLight = value; };
        UiIcosahedron.UseOrbitEvent += value => { _useOrbit = value; };
        UiIcosahedron.RotationSpeedEvent += value => { _speed = value; };
    }


    public unsafe void Render() {
        
        float multi;
        multi = _useOrbit ? 1 : 0;
        
        var selfRotation = Time.TimeSinceStart * 13.5f;
        _orbit = _orbit.Rotation360(multi * _speed);
        
        Mesh.Bind();
        Material.Use();
        _texture.Bind();
        
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateRotationY(selfRotation.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateRotationX(selfRotation.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateRotationY(_orbit.DegreesToRadiansOnVariable(), Light.LightPosition1);
        _matrix *= Matrix4x4.CreateScale(1f);
        
        Material.SetUniform("uModel", _matrix);
        Material.SetUniform("uView", Camera.Instance.GetViewMatrix());
        Material.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());

        Material.SetUniform("material.shininess", _shininessMaterial);
        Material.SetUniform("light.viewPosition", Camera.Instance.Position);
        Material.SetUniform("light.position", Light.LightPosition1);
        Material.SetUniform("light.ambient", _ambientLightColor);
        Material.SetUniform("light.diffuse", _diffuseLightColor);
        Material.SetUniform("light.specular", _specularLightColor * _specularLightMultiplier);
        Material.SetUniform("useBlinnAlgorithm", _useBlinnCalculation ? 1 : 0);
        Material.SetUniform("useDirectionalLight", _useDirectionalLight ? 1 : 0);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);
    }
}