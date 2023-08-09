using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SAE._5300S1.Utils.UI.InputControllers; 

public class UiController {
    public static UiController Instance => Lazy.Value;
    private static readonly Lazy<UiController> Lazy = new(() => new UiController());

    public ImGuiController ImGuiController { get; private set; }

    private UiController() {
        ImGuiController = new ImGuiController(
            Program.Gl, 
            Program.window, 
            UserInputController.Instance.InputContext);
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }
}