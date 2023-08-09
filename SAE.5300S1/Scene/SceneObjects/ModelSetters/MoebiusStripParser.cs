using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters;

public class MoebiusStripParser : IModel {
    public static MoebiusStripParser Instance => Lazy.Value;
    private static readonly Lazy<MoebiusStripParser> Lazy = new(() => new MoebiusStripParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private MoebiusStripParser() {
        var objWizard = new Parser("moebiusBand.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}