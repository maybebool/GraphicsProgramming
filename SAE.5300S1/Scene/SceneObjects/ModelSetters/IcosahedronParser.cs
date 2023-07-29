using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class IcosahedronParser : IModel {
    public static IcosahedronParser Instance => Lazy.Value;
    private static readonly Lazy<IcosahedronParser> Lazy = new(() => new IcosahedronParser());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private IcosahedronParser() {
        var objWizard = new Parser("test3.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}