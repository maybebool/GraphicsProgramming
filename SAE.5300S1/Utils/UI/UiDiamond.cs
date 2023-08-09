using System.Numerics;
using ImGuiNET;

namespace SAE._5300S1.Utils.UI; 

public class UiDiamond : IUi {
    
    // Parameters
    private float _shininessMaterial = 500f;
    private Vector3 _ambientLightColor = new(0.75f);
    private Vector3 _diffuseLightColor = new(0.8f,0.7f,0.8f);
    private Vector3 _specularLightColor = new(1f);
    private float _specularLightMultiplier = 5.0f;
    private bool _useBlinnCalculation;
    private bool _useDirectionalLight;
    private float _rotationSpeed = 30f;
    

    
    // Events
    public static Action<float> ShininessMaterialEvent;
    public static Action<Vector3> AmbientLightColorEvent;
    public static Action<Vector3> DiffuseLightColorEvent;
    public static Action<Vector3> SpecularLightColorEvent;
    public static Action<float> SpecularLightMultiplierEvent;
    public static Action<bool> UseBlinnCalculationEvent;
    public static Action<bool> UseDirectionalLightEvent;
    public static Action<float> RotationSpeedEvent;

    public UiDiamond() {

        ShininessMaterialEvent.Invoke(_shininessMaterial);
        AmbientLightColorEvent.Invoke(_ambientLightColor);
        DiffuseLightColorEvent.Invoke(_diffuseLightColor);
        SpecularLightColorEvent.Invoke(_specularLightColor);
        SpecularLightMultiplierEvent.Invoke(_specularLightMultiplier);
        UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        UseDirectionalLightEvent.Invoke(_useDirectionalLight);
        RotationSpeedEvent.Invoke(_rotationSpeed);
    }
    
    public void UpdateUi() {
        ImGui.Begin("Settings");
        
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("Material");
        if (ImGui.SliderFloat("Material Shininess", ref _shininessMaterial, 20, 500)) {
            ShininessMaterialEvent.Invoke(_shininessMaterial);
        }
        
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("Light");
        if (ImGui.ColorEdit3("Light Ambient Color", ref _ambientLightColor)) {
            AmbientLightColorEvent.Invoke(_ambientLightColor);
        }
        if (ImGui.ColorEdit3("Light Diffuse Color", ref _diffuseLightColor)) {
            DiffuseLightColorEvent.Invoke(_diffuseLightColor);
        }
        if (ImGui.ColorEdit3("Light Specular Color", ref _specularLightColor)) {
            SpecularLightColorEvent.Invoke(_specularLightColor);
        }
        if (ImGui.SliderFloat("Specular Multiplier", ref _specularLightMultiplier, 1, 5)) {
            SpecularLightMultiplierEvent.Invoke(_specularLightMultiplier);
        }
        if (ImGui.Checkbox("Use Blinn Calculation", ref _useBlinnCalculation)) {
            UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        }
        if (ImGui.Checkbox("Use Directional Light", ref _useDirectionalLight)) {
            UseDirectionalLightEvent.Invoke(_useDirectionalLight);
        }
        
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("Model");
        if (ImGui.SliderFloat("Rotation Speed", ref _rotationSpeed, 0, 40)) {
            RotationSpeedEvent.Invoke(_rotationSpeed);
        }
        ImGui.End();
    }
}