using System.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SAE._5300S1.Utils.UI;

public class UiIcosahedron : IUiInterface {

    private ImGuiController _controller;


    // Parameters
    private float _diffuseMaterial = 0.2f;
    private float _specularMaterial = 1f;
    private float _shininessMaterial = 180f;
    
    private Vector3 _ambientLightColor= new(0.6f);
    private Vector3 _diffuseLightColor= new(0.6f);
    private Vector3 _specularLightColor = new(0.6f);
    private bool _useBlinnCalculation;
    private bool _useDirectionalLight;

    
    // Events
    public static Action<float> DiffuseMaterialChangerEvent;
    public static Action<float> SpecularMaterialChangerEvent;
    public static Action<float> ShininessMaterialChangerEvent;
    
    public static Action<Vector3> AmbientLightColorChangerEvent;
    public static Action<Vector3> DiffuseLightColorChangerEvent;
    public static Action<Vector3> SpecularLightColorChangerEvent;
    public static Action<bool> UseBlinnCalculationEvent;
    public static Action<bool> UseDirectionalLightEvent;

    public UiIcosahedron() {
        _controller = UiController.Instance.ImGuiController;
        
        DiffuseMaterialChangerEvent.Invoke(_diffuseMaterial);
        SpecularMaterialChangerEvent.Invoke(_specularMaterial);
        ShininessMaterialChangerEvent.Invoke(_shininessMaterial);
        
        AmbientLightColorChangerEvent.Invoke(_ambientLightColor);
        DiffuseLightColorChangerEvent.Invoke(_diffuseLightColor);
        SpecularLightColorChangerEvent.Invoke(_specularLightColor);
        
        UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        UseDirectionalLightEvent.Invoke(_useDirectionalLight);
    }
    
    public void UpdateUi() {
        ImGui.Begin("Icosahedron");

        if (ImGui.SliderFloat("Material Diffusion", ref _diffuseMaterial, 0, 1)) {
            DiffuseMaterialChangerEvent.Invoke(_diffuseMaterial);
        }
        if (ImGui.SliderFloat("Material Specular", ref _specularMaterial, 0, 1)) {
            SpecularMaterialChangerEvent.Invoke(_specularMaterial);
        }
        if (ImGui.SliderFloat("Material Shininess", ref _shininessMaterial, 20, 500)) {
            SpecularMaterialChangerEvent.Invoke(_specularMaterial);
        }
        
        if (ImGui.ColorEdit3("Light Ambient Color", ref _ambientLightColor)) {
            AmbientLightColorChangerEvent.Invoke(_ambientLightColor);
        }
        if (ImGui.ColorEdit3("Light Diffuse Color", ref _diffuseLightColor)) {
            DiffuseLightColorChangerEvent.Invoke(_diffuseLightColor);
        }
        if (ImGui.ColorEdit3("Light Specular Color", ref _specularLightColor)) {
            SpecularLightColorChangerEvent.Invoke(_specularLightColor);
        }

        if (ImGui.Checkbox("Use Blinn Calculation", ref _useBlinnCalculation)) {
            UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        }
        if (ImGui.Checkbox("Use Directional Light", ref _useDirectionalLight)) {
            UseDirectionalLightEvent.Invoke(_useDirectionalLight);
        }
        
        // ImGui.Checkbox("Color", ref _isColor);
        // ImGui.Checkbox("Texture", ref _isTexture);
        // ImGui.End();
        ImGui.End();
    }

    public void RenderUi() {
        _controller.Render();
    }
}