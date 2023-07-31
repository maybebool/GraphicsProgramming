using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class PerfectMirrorParser : IModel {
    public static PerfectMirrorParser Instance => Lazy.Value;
    private static readonly Lazy<PerfectMirrorParser> Lazy = new(() => new PerfectMirrorParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private PerfectMirrorParser() {
        var objWizard = new Parser("cube.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}