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
    private bool _useBlinnCalculation = false;
    private bool _useDirectionalLight = false;

    
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

    public void LoadDemoWindow() {
        
        ImGui.Begin("Icosahedron");
        
        if (ImGui.ColorEdit3("Ambient Light Color", ref _ambientLightColor)) {
            AmbientLightColorChangerEvent.Invoke(_ambientLightColor);
        }
        if (ImGui.ColorEdit3("Diffuse Light Color", ref _diffuseLightColor)) {
            DiffuseLightColorChangerEvent.Invoke(_diffuseLightColor);
        }
        if (ImGui.ColorEdit3("Diffuse Light Color", ref _specularLightColor)) {
            SpecularLightColorChangerEvent.Invoke(_specularLightColor);
        }

        ImGui.Text("Texture must be active for Texture blend with alpha chânnel");
        // ImGui.Checkbox("Color", ref _isColor);
        // ImGui.Checkbox("Texture", ref _isTexture);
        // ImGui.End();
        ImGui.End();
    }

    public void UpdateUi() {
        LoadDemoWindow();
    }

    public void RenderUi() {
        _controller.Render();
    }
}