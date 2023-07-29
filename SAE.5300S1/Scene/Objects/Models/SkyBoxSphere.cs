namespace SAE._5300S1.Scene.Objects.Models; 

public class SkyBoxSphere : IModel {
    public static SkyBoxSphere Instance => Lazy.Value;
    private static readonly Lazy<SkyBoxSphere> Lazy = new(() => new SkyBoxSphere());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private SkyBoxSphere() {
        var objWizard = new Parser("spheres.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}