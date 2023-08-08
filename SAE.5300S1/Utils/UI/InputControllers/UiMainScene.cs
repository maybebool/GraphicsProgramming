using System.Numerics;
using ImGuiNET;

namespace SAE._5300S1.Utils.UI.InputControllers;

public class UiMainScene {

    private static UiIcosahedron _uiIcosahedron;
    private static UiDiamond _uiDiamond;
    private static UiSpiral _uiSpiral;
    private static UiIcosaStar _uiIcosaStar;

    private IUi? renderUi;

    public UiMainScene() {
        _uiIcosahedron = new UiIcosahedron();
        _uiDiamond = new UiDiamond();
        _uiSpiral = new UiSpiral();
        _uiIcosaStar = new UiIcosaStar();
    }

    public void UpdateMainUi() {
        ImGui.Begin("Settings");
        ImGui.Columns(4);
        if (ImGui.Button("Icosahedron",new Vector2(ImGui.GetColumnWidth(),22)))
            renderUi = _uiIcosahedron;

        ImGui.NextColumn();
        if (ImGui.Button("Diamond", new Vector2(ImGui.GetColumnWidth(),22)))
            renderUi = _uiDiamond;

        ImGui.NextColumn();
        if (ImGui.Button("Spiral", new Vector2(ImGui.GetColumnWidth(),22)))
            renderUi = _uiSpiral;
        
        ImGui.NextColumn();
        if (ImGui.Button("Marble aaw Sculpture", new Vector2(ImGui.GetColumnWidth(),22)))
            renderUi = _uiIcosaStar;

        ImGui.End();
        if (renderUi != null) {
            renderUi.UpdateUi();
        }
    }
}