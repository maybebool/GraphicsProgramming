using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters;

public class DiamondParser : IModel {
    public static DiamondParser Instance => Lazy.Value;
    private static readonly Lazy<DiamondParser> Lazy = new(() => new DiamondParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private DiamondParser() {
        var objWizard = new Parser("diamond.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}