using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class SpiralParser : IModel {
    public static SpiralParser Instance => Lazy.Value;
    private static readonly Lazy<SpiralParser> Lazy = new(() => new SpiralParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private SpiralParser() {
        var objWizard = new Parser("spiral.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}