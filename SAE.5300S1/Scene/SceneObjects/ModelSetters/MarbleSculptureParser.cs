using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class MarbleSculptureParser : IModel {
    public static MarbleSculptureParser Instance => Lazy.Value;
    private static readonly Lazy<MarbleSculptureParser> Lazy = new(() => new MarbleSculptureParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private MarbleSculptureParser() {
        var objWizard = new Parser("skull.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}