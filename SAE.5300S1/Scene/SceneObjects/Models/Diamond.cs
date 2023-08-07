using System.Diagnostics.Contracts;
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

public class Diamond {
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


    // Event Parameters
    public Diamond(GL gl,
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
        
        UiDiamond.ShininessMaterialChangerEvent += value => { _shininessMaterial = value; };
        UiDiamond.AmbientLightColorChangerEvent += value => { _ambientLightColor = value; };
        UiDiamond.DiffuseLightColorChangerEvent += value => { _diffuseLightColor = value; };
        UiDiamond.SpecularLightColorChangerEvent += value => { _specularLightColor = value; };
        UiDiamond.SpecularLightMultiplierChangerEvent += value => { _specularLightMultiplier = value; };
        UiDiamond.UseBlinnCalculationEvent += value => { _useBlinnCalculation = value; };
        UiDiamond.UseDirectionalLightEvent += value => { _useDirectionalLight = value; };
    }

    public unsafe void Render() {
        
        
        float selfRotation = Time.TimeSinceStart * 30.0f;
        Mesh.Bind();
        Material.Use();
        _texture.Bind();
        
        _matrix = Matrix4x4.Identity;
        _matrix *= Matrix4x4.CreateRotationY(selfRotation.DegreesToRadiansOnVariable());
        _matrix *= Matrix4x4.CreateScale(12f);
        _matrix *= Matrix4x4.CreateTranslation(-9.0f, 0, 0.0f);

        Material.SetUniform("uModel", _matrix);
        Material.SetUniform("uView", Camera.Instance.GetViewMatrix());
        Material.SetUniform("uProjection", Camera.Instance.GetProjectionMatrix());

        Material.SetUniform("material.shininess", _shininessMaterial);
        Material.SetUniform("light.viewPosition", Camera.Instance.Position);
        Material.SetUniform("light.position", Light.LightPosition2);
        Material.SetUniform("light.ambient", _ambientLightColor);
        Material.SetUniform("light.diffuse", _diffuseLightColor);
        Material.SetUniform("light.specular", _specularLightColor * _specularLightMultiplier);
        Material.SetUniform("useBlinnAlgorithm", _useBlinnCalculation ? 1 : 0);
        Material.SetUniform("useDirectionalLight", _useDirectionalLight ? 1 : 0);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.IndicesLength);
    }
}