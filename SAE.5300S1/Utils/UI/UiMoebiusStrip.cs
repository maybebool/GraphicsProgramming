using ImGuiNET;

namespace SAE._5300S1.Utils.UI; 

public class UiMoebiusStrip : IUi{
    
    private float _speedY;
    private float _speedX = 10f;
    private float _scale = 0.5f;
    
    
    public static Action<float> SpeedXChangerEvent;
    public static Action<float> SpeedYChangerEvent;
    public static Action<float> ScaleChangerEvent;


    public UiMoebiusStrip() {
        SpeedXChangerEvent.Invoke(_speedX);
        SpeedYChangerEvent.Invoke(_speedY);
        ScaleChangerEvent.Invoke(_scale);
    }
    public void UpdateUi() {
        ImGui.Begin("Settings");
        
        ImGui.Spacing();

        ImGui.Text("Scale/Rotation");
        
        if (ImGui.SliderFloat("Scale", ref _scale, 0.1f, 0.5f)) {
            ScaleChangerEvent.Invoke(_scale);
        }
        if (ImGui.SliderFloat("Rotation X", ref _speedX, 0, 20)) {
            SpeedXChangerEvent.Invoke(_speedX);
        }
        if (ImGui.SliderFloat("Rotation Y", ref _speedY, 0, 20)) {
            SpeedYChangerEvent.Invoke(_speedY);
        }
        ImGui.End();
    }
}