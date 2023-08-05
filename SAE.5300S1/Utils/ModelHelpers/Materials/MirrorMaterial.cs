using Silk.NET.OpenGL;

namespace SAE._5300S1.Utils.ModelHelpers.Materials; 

public class MirrorMaterial {
    public static MirrorMaterial Instance => Lazy.Value;
    public Material Material;
    private static GL Gl;
    private static readonly Lazy<MirrorMaterial> Lazy = new(() => new());
    
    private MirrorMaterial() {
        Material = new Material(Program.Gl, "mirrorShader.vert", "mirrorShader.frag");
    }
}