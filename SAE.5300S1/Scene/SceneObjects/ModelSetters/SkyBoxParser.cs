using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class SkyBoxParser : IModel {
    public static SkyBoxParser Instance => Lazy.Value;
    private static readonly Lazy<SkyBoxParser> Lazy = new(() => new SkyBoxParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private SkyBoxParser() {
        var objWizard = new Parser("spheres.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}