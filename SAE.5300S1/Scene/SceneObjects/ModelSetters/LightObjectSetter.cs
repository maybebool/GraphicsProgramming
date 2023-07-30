using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class LightObjectSetter : IModel {
    public static LightObjectSetter Instance => Lazy.Value;
    private static readonly Lazy<LightObjectSetter> Lazy = new(() => new LightObjectSetter());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private LightObjectSetter() {
        var objWizard = new Parser("cube.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}