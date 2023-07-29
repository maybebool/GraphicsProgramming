namespace SAE._5300S1.Scene.Objects.Models; 

public class IcosahedronForm : IModel {
    public static IcosahedronForm Instance => Lazy.Value;
    private static readonly Lazy<IcosahedronForm> Lazy = new(() => new IcosahedronForm());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private IcosahedronForm() {
        var objWizard = new Parser("cube2.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}