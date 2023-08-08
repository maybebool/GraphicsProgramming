using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SAE._5300S1.Utils.UI.InputControllers;

public class UiMainController : IUi {
    private ImGuiController _controller;

    private static UiIcosahedron _uiIcosahedron;
    private static UiDiamond _uiDiamond;
    private static UiSpiral _uiSpiral;
    private static UiIcosaStar _uiIcosaStar;

    private IUi? renderUi;

    public UiMainController() {
        _controller = UiController.Instance.ImGuiController;
    }

    public void OnLoadAllUis() {
        _uiIcosahedron = new UiIcosahedron();
        _uiDiamond = new UiDiamond();
        _uiSpiral = new UiSpiral();
        _uiIcosaStar = new UiIcosaStar();
    }
    
    public void UpdateUi() {
        
        ImGui.Begin("Settings");
        ImGui.Columns(4);
        
        if (ImGui.Button("Icosahedron"))
            renderUi = _uiIcosahedron;
        
        ImGui.SameLine();
        if (ImGui.Button("Diamond"))
            renderUi = _uiDiamond;
        
        ImGui.SameLine();
        if (ImGui.Button("Spiral"))
            renderUi = _uiSpiral;
        
        ImGui.SameLine();
        if (ImGui.Button("IcosaStar"))
            renderUi = _uiIcosaStar;

        ImGui.End();
    }

    public void RenderUi() {
        if (renderUi != null) {
            renderUi.UpdateUi();
            renderUi.RenderUi();
        }
        _controller.Render();
    }
}