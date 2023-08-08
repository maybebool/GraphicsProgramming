using SAE._5300S1.Utils.ModelHelpers;

namespace SAE._5300S1.Scene.SceneObjects.ModelSetters; 

public class MarbleSculpture : IModel {
    public static MarbleSculpture Instance => Lazy.Value;
    private static readonly Lazy<MarbleSculpture> Lazy = new(() => new MarbleSculpture());
    public float[] Vertices { get; }
    public uint[] Indices { get; }

    private MarbleSculpture() {
        var objWizard = new Parser("skull.obj");
        Vertices = objWizard.Vertices;
        Indices = objWizard.Indices;
    }
}