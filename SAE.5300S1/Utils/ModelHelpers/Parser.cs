using System.Numerics;

namespace SAE._5300S1.Utils.ModelHelpers;

public class Parser {
    public float[] Vertices => _vertices;
    public uint[] Indices => _indices;


    private static readonly string FileLoaderPath = AppContext.BaseDirectory + "models/";
    private string _path;
    private string _mtlFileName;
    private string _useMtlFileName;
    private float[] _vertices = Array.Empty<float>();
    private uint[] _indices = Array.Empty<uint>();
    private List<float> _tempVertices = new();
    private List<uint> _tempIndices = new();
    private List<Vector2> _originUvs = new();
    private List<Vector3> _originNormals = new();
    private List<Vector3> _originVertices = new();
    private List<int> _tempVerticesIndices = new();
    private List<int> _tempNormalIndices = new();
    private List<int> _tempUvIndices = new();

    public Parser(string fileName) {
#if DEBUG
        var startTime = DateTime.Now;
#endif

        ReadFile(FileLoaderPath + fileName);

#if DEBUG
        var endTime = DateTime.Now;
        var time = endTime - startTime;
        Console.WriteLine($"Execution Read OBJ file: {time}");
#endif
    }

    private void ReadFile(string filePath) {
        var sr = new StreamReader(filePath);
        try {
            var line = sr.ReadLine();
            while (line != null) {
                if (!line.StartsWith("#")) {
                    if (line.StartsWith("mtllib")) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        _mtlFileName = parts[1];
                    }
                    else if (line.StartsWith("usemtl")) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        _useMtlFileName = parts[1];
                    }
                    else if (line.StartsWith("vt")) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        _originUvs.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                    }
                    else if (line.StartsWith("vn")) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        _originNormals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]),
                            float.Parse(parts[3])));
                    }
                    else if (line.StartsWith("v")) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        _originVertices.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]),
                            float.Parse(parts[3])));
                    }
                    else if (line.StartsWith("f")) {
                        var fParts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        var faceVertexFirst = fParts[1].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        _tempVerticesIndices.Add(int.Parse(faceVertexFirst[0]));
                        _tempUvIndices.Add(int.Parse(faceVertexFirst[1]));
                        _tempNormalIndices.Add(int.Parse(faceVertexFirst[2]));

                        var faceVertexSecond = fParts[2].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        _tempVerticesIndices.Add(int.Parse(faceVertexSecond[0]));
                        _tempUvIndices.Add(int.Parse(faceVertexSecond[1]));
                        _tempNormalIndices.Add(int.Parse(faceVertexSecond[2]));

                        var faceVertexThird = fParts[3].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        _tempVerticesIndices.Add(int.Parse(faceVertexThird[0]));
                        _tempUvIndices.Add(int.Parse(faceVertexThird[1]));
                        _tempNormalIndices.Add(int.Parse(faceVertexThird[2]));
                    }
                }

                line = sr.ReadLine();
            }
            
            sr.Close();

            for (int i = 0; i < _tempVerticesIndices.Count; i++) {
                if (i == 50) {
                    Console.WriteLine("");
                }
                int indexVerts = _tempVerticesIndices[i];
                Vector3 vertex = _originVertices[indexVerts - 1];
                _tempVertices.Add(vertex.X);
                _tempVertices.Add(vertex.Y);
                _tempVertices.Add(vertex.Z);

                int indexNormal = _tempVerticesIndices[i];
                Vector3 normal = _originNormals[indexNormal - 1];
                _tempVertices.Add(normal.X);
                _tempVertices.Add(normal.Y);
                _tempVertices.Add(normal.Z);

                int indexUv = _tempUvIndices[i];
                Vector2 uv = _originUvs[indexUv - 1];
                _tempVertices.Add(uv.X);
                _tempVertices.Add(uv.Y);

                _tempIndices.Add((uint)i);
            }

            _vertices = _tempVertices.ToArray();
            _indices = _tempIndices.ToArray();

            _tempVertices.Clear();
            _tempIndices.Clear();

            _originNormals.Clear();
            _originUvs.Clear();
            _originVertices.Clear();

            _tempNormalIndices.Clear();
            _tempUvIndices.Clear();
            _tempVerticesIndices.Clear();

            
        }
        catch (Exception ex) {
            sr.Close();
            Console.WriteLine($"Impossible to open the file !: {ex.Message}");
        }
    }
}