using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class SkullParser : IModel {
    public static SkullParser Instance => Lazy.Value;
    private static readonly Lazy<SkullParser> Lazy = new(() => new SkullParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private SkullParser() {
        var objWizard = new Parser("skull.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}