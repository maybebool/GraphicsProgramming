using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class LightObject1Parser : IModel {
    public static LightObject1Parser Instance => Lazy.Value;
    private static readonly Lazy<LightObject1Parser> Lazy = new(() => new LightObject1Parser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private LightObject1Parser() {
        var objWizard = new Parser("cube.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}