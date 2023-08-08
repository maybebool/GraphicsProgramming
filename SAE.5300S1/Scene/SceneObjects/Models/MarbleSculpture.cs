using System.Diagnostics.Contracts;
using System.Numerics;
using SAE._5300S1.Utils.MathHelpers;
using SAE._5300S1.Utils.ModelHelpers;
using SAE._5300S1.Utils.ModelHelpers.Materials;
using SAE._5300S1.Utils.SceneHelpers;
using SAE._5300S1.Utils.UI;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using Texture = SAE._5300S1.Utils.ModelHelpers.Texture;

namespace SAE._5300S1.Scene.SceneObjects.Models; 

public class MarbleSculpture {
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

    public MarbleSculpture(GL gl,
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
        
        UiIcosaStar.ShininessMaterialChangerEvent += value => { _shininessMaterial = value; };
        UiIcosaStar.AmbientLightColorChangerEvent += value => { _ambientLightColor = value; };
        UiIcosaStar.DiffuseLightColorChangerEvent += value => { _diffuseLightColor = value; };
        UiIcosaStar.SpecularLightColorChangerEvent += value => { _specularLightColor = value; };
        UiIcosaStar.SpecularLightMultiplierChangerEvent += value => { _specularLightMultiplier = value; };
        UiIcosaStar.UseBlinnCalculationEvent += value => { _useBlinnCalculation = value; };
        UiIcosaStar.UseDirectionalLightEvent += value => { _useDirectionalLight = value; };

    }

    private bool _myBool = false;
    public unsafe void Render() {

        float angle = Time.TimeSinceStart * 13.5f;
        Mesh.Bind();
        Material.Use();
        _texture.Bind();
        _matrix = Matrix4x4.Identity;
        // _matrix *= Matrix4x4.CreateRotationY(angle.DegreesToRadiansOnVariable());
        // _matrix *= Matrix4x4.CreateRotationX(angle.DegreesToRadiansOnVariable());
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