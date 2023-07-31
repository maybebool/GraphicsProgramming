using Silk.NET.OpenGL;

namespace SAE._5300S1.Utils.ModelHelpers; 

public class ReflectionMaterial {
    public static ReflectionMaterial Instance => Lazy.Value;
    public Material Material;
    private static GL Gl;
    private static readonly Lazy<ReflectionMaterial> Lazy = new(() => new());

    private ReflectionMaterial() {
        Material = new Material(Program.Gl, "reflectionShader.vert", "reflectionShader.frag");
    }
}