using Silk.NET.OpenGL;

namespace SAE._5300S1; 

public class StandardMaterial {
    public static StandardMaterial Instance => Lazy.Value;
    public Material Material;
    private static GL Gl;
    private static readonly Lazy<StandardMaterial> Lazy = new(() => new());

    private StandardMaterial() {
        Material = new Material(Program.Gl, "shader.vert", "shader.frag");
    }
}