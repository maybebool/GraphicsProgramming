using System.Numerics;
using ImGuiNET;

namespace SAE._5300S1.Utils.UI; 

public class UiSpiral : IUi {


    // Parameters
    private float _shininessMaterial = 20f;
    private Vector3 _ambientLightColor = new(0.7f);
    private Vector3 _diffuseLightColor = new(0.6f);
    private Vector3 _specularLightColor = new(0f);
    private float _specularLightMultiplier = 1.0f;
    private bool _useBlinnCalculation;
    private bool _useDirectionalLight;


    // Events
    public static Action<float> ShininessMaterialChangerEvent;
    public static Action<Vector3> AmbientLightColorChangerEvent;
    public static Action<Vector3> DiffuseLightColorChangerEvent;
    public static Action<Vector3> SpecularLightColorChangerEvent;
    public static Action<float> SpecularLightMultiplierChangerEvent;
    public static Action<bool> UseBlinnCalculationEvent;
    public static Action<bool> UseDirectionalLightEvent;

    public UiSpiral() {
        
        ShininessMaterialChangerEvent.Invoke(_shininessMaterial);
        AmbientLightColorChangerEvent.Invoke(_ambientLightColor);
        DiffuseLightColorChangerEvent.Invoke(_diffuseLightColor);
        SpecularLightColorChangerEvent.Invoke(_specularLightColor);
        SpecularLightMultiplierChangerEvent.Invoke(_specularLightMultiplier);
        UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        UseDirectionalLightEvent.Invoke(_useDirectionalLight);
    }
    
    public void UpdateUi() {
        ImGui.Begin("Settings");
        ImGui.Text("Material");
        if (ImGui.SliderFloat("Material Shininess", ref _shininessMaterial, 20, 500)) {
            ShininessMaterialChangerEvent.Invoke(_shininessMaterial);
        }
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("Light");
        if (ImGui.ColorEdit3("Light Ambient Color", ref _ambientLightColor)) {
            AmbientLightColorChangerEvent.Invoke(_ambientLightColor);
        }
        if (ImGui.ColorEdit3("Light Diffuse Color", ref _diffuseLightColor)) {
            DiffuseLightColorChangerEvent.Invoke(_diffuseLightColor);
        }
        if (ImGui.ColorEdit3("Light Specular Color", ref _specularLightColor)) {
            SpecularLightColorChangerEvent.Invoke(_specularLightColor);
        }
        if (ImGui.SliderFloat("Specular Multiplier", ref _specularLightMultiplier, 1, 5)) {
            SpecularLightMultiplierChangerEvent.Invoke(_specularLightMultiplier);
        }
        if (ImGui.Checkbox("Use Blinn Calculation", ref _useBlinnCalculation)) {
            UseBlinnCalculationEvent.Invoke(_useBlinnCalculation);
        }
        if (ImGui.Checkbox("Use Directional Light", ref _useDirectionalLight)) {
            UseDirectionalLightEvent.Invoke(_useDirectionalLight);
        }
        ImGui.End();
    }
}