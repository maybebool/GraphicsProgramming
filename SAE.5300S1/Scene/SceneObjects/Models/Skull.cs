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

public class Skull {
     public Mesh Mesh { get; set; }
    public Material Material { get; set; }

    private Texture _texture;
    private GL _gl;
    private string _textureName;
    private Matrix4x4 _matrix;
    private IModel _model;
    
    
    
    
    private float _shininessMaterial;
    private Vector3 _ambientLightColor;
    private Vector3 _diffuseLightColor;
    private Vector3 _specularLightColor;
    private float _specularLightMultiplier;
    private bool _useBlinnCalculation;
    private bool _useDirectionalLight;

    public Skull(GL gl,
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
        
        UiSkull.ShininessMaterialChangerEvent += value => { _shininessMaterial = value; };
        UiSkull.AmbientLightColorChangerEvent += value => { _ambientLightColor = value; };
        UiSkull.DiffuseLightColorChangerEvent += value => { _diffuseLightColor = value; };
        UiSkull.SpecularLightColorChangerEvent += value => { _specularLightColor = value; };
        UiSkull.SpecularLightMultiplierChangerEvent += value => { _specularLightMultiplier = value; };
        UiSkull.UseBlinnCalculationEvent += value => { _useBlinnCalculation = value; };
        UiSkull.UseDirectionalLightEvent += value => { _useDirectionalLight = value; };

    }

    private bool _myBool = false;
    public unsafe void Render() {
        
        Mesh.Bind();
        Material.Use();
        _texture.Bind();
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateScale(3f);
        _matrix *= Matrix4x4.CreateRotationY(-90f.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateTranslation(-27f, 1f, 0f);
        
        Material.SetUniform("uModel", _matrix);
        Material.SetUniform("uView", Camera.Instance.GetViewMatrix());
        Material.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());
        
        Material.SetUniform("material.shininess", _shininessMaterial);
        Material.SetUniform("light.viewPosition", Camera.Instance.Position);
        Material.SetUniform("light.position", Light.LightPosition4);
        Material.SetUniform("light.ambient", _ambientLightColor);
        Material.SetUniform("light.diffuse", _diffuseLightColor);
        Material.SetUniform("light.specular", _specularLightColor * _specularLightMultiplier);
        Material.SetUniform("useBlinnAlgorithm", _useBlinnCalculation ? 1 : 0);
        Material.SetUniform("useDirectionalLight", _useDirectionalLight ? 1 : 0);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);
        
    }
}