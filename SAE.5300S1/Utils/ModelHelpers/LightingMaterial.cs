using Silk.NET.OpenGL;

namespace SAE._5300S1.Utils.ModelHelpers; 

public class LightingMaterial {
    public static LightingMaterial Instance => Lazy.Value;
        public Material Material;
        private static GL Gl;
        private static readonly Lazy<LightingMaterial> Lazy = new(() => new());
    
        private LightingMaterial() {
            Material = new Material(Program.Gl, "lightingShader.vert", "lightingShader.frag");
        }
}