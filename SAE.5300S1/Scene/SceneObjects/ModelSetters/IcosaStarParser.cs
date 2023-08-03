using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class IcosaStarParser : IModel {
    public static IcosaStarParser Instance => Lazy.Value;
    private static readonly Lazy<IcosaStarParser> Lazy = new(() => new IcosaStarParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private IcosaStarParser() {
        var objWizard = new Parser("icosaStar.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}