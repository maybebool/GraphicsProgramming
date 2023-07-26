using Silk.NET.OpenGL;

namespace SAE._5300S1; 

public class Material : Shader {
    public Material(GL gl, string vertexPath, string fragmentPath)
        : base(gl, vertexPath, fragmentPath) { }
    
    private Dictionary<string, ProgramParam> shaderParams = new();
    
    public ProgramParam this[string name] {
        get { return shaderParams.ContainsKey(name) ? shaderParams[name] : null; }
    }
}