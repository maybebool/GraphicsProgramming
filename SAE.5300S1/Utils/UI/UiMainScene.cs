using System.Numerics;
using ImGuiNET;

namespace SAE._5300S1.Utils.UI;

public class UiMainScene {

    private static UiIcosahedron _uiIcosahedron;
    private static UiDiamond _uiDiamond;
    private static UiSpiral _uiSpiral;
    private static UiSkull _uiSkull;
    private static UiMoebiusStrip _uiMoebiusStrip;

    private IUi? _updateUi;

    public UiMainScene() {
        _uiIcosahedron = new UiIcosahedron();
        _uiDiamond = new UiDiamond();
        _uiSpiral = new UiSpiral();
        _uiSkull = new UiSkull();
        _uiMoebiusStrip = new UiMoebiusStrip();
    }

    public void UpdateMainUi() {
        ImGui.SetNextWindowPos(new Vector2(0,Program.Height - 320));
        ImGui.SetNextWindowSize(new Vector2(Program.Width, 320));
        ImGui.Begin("Settings", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse);
        ImGui.Columns(5);
        if (ImGui.Button("Icosahedron",new Vector2(ImGui.GetColumnWidth(),22)))
            _updateUi = _uiIcosahedron;

        ImGui.NextColumn();
        if (ImGui.Button("Diamond", new Vector2(ImGui.GetColumnWidth(),22)))
            _updateUi = _uiDiamond;

        ImGui.NextColumn();
        if (ImGui.Button("Spiral", new Vector2(ImGui.GetColumnWidth(),22)))
            _updateUi = _uiSpiral;
        
        ImGui.NextColumn();
        if (ImGui.Button("Skull", new Vector2(ImGui.GetColumnWidth(),22)))
            _updateUi = _uiSkull;
        ImGui.NextColumn();
        
        if (ImGui.Button("Moebius Strip", new Vector2(ImGui.GetColumnWidth(),22)))
            _updateUi = _uiMoebiusStrip;

        ImGui.End();
        if (_updateUi != null) {
            _updateUi.UpdateUi();
        }
    }
}