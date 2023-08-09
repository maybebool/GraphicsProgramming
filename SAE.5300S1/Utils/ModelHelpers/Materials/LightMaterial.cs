using Silk.NET.OpenGL;

namespace SAE._5300S1.Utils.ModelHelpers.Materials; 

public class LightMaterial {
    public static LightMaterial Instance => Lazy.Value;
    public Material Material;
    private static GL Gl;
    private static readonly Lazy<LightMaterial> Lazy = new(() => new());

    private LightMaterial() {
        Material = new Material(Program.Gl, "shader.vert", "lightShader.frag");
    }
}