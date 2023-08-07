using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SAE._5300S1.Utils.UI.InputControllers; 

public class UiMainController : IUi {
    
    private ImGuiController _controller;
    
    private static UiIcosahedron _uiIcosahedron;
    private static UiDiamond _uiDiamond;
    private static UiSpiral _uiSpiral;
    private static UiIcosaStar _uiIcosaStar;

    private static bool onIcosahedronTap;
    private static bool onDiamondTap;
    private static bool onSpiralTap;
    private static bool onIcosaStarTap;


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
        
        if (ImGui.Button("Icosahedron")) {
            onIcosahedronTap = true;
            _uiIcosahedron.UpdateUi();
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Diamond")) {
            onDiamondTap = true;
            _uiDiamond.UpdateUi();
        } 
        ImGui.SameLine();
        if (ImGui.Button("Spiral")) {
            onSpiralTap = true;
            _uiSpiral.UpdateUi();
        } 
        ImGui.SameLine();
        if (ImGui.Button("IcosaStar")) {
            onIcosaStarTap = true;
            _uiIcosaStar.UpdateUi();
        }

        ImGui.End(); 
    }

    public void RenderUi() {
        if (onIcosahedronTap) {
            _uiIcosahedron.RenderUi();
        }
        if (onDiamondTap) {
            _uiDiamond.RenderUi();
        }
        if (onSpiralTap) {
            _uiSpiral.RenderUi();
        }
        if (onIcosaStarTap) {
            _uiIcosaStar.RenderUi();
        }
        _controller.Render();
    }
}