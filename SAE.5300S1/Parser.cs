using System.Numerics;

namespace SAE._5300S1;

public class Parser {


    public List<int> VertexIndices, UvIndices, NormalIndices;
    public Vector2 TempUvs;
    public Vector3 TempVertices;
    public Vector3 TempNormals;
    private string? _path;
    private int[] _vertexIndex = new int[3];
    private int[] _uvIndex = new int[3];
    private int[] _normalIndex = new int[3];


    public bool LoadObj(string path,
        out List<Vector3> outVertices,
        out List<Vector2> outUvs,
        out List<Vector3> outNormals) {

        outVertices = null;
        outUvs = null;
        outNormals = null;
        return true;
    }

    public void ReadFile() {
        
        
        if (_path == null) return;
        var sr = new StreamReader(_path);
        try {
            sr.ReadLine();
            while (true)
            {
                var lineHeader = new char[128];

                int res = fscanf(sr, "%s", lineHeader);
                if (res == EOF)
                    break;
                
                if (lineHeader.Equals("v"))
                {
                    Vector3 vertex;
                    float x, y, z;
                    fscanf(sr, "%f %f %f\n", x, y, z);
                    vertex = new Vector3(x, y, z);
                    TempVertices.Add(vertex);
                }
                
                else if ( lineHeader.Equals("vt")) {
                    Vector2 uv;
                    fscanf(sr, "%f %f\n", &uv.x, &uv.y );
                    TempUvs.Add(uv);
                }
                
                else if ( lineHeader.Equals("vn")) {
                    Vector2 normal;
                    fscanf(sr, "%f %f %f\n", &normal.x, &normal.y, &normal.z ;
                    TempUvs.Add(uv);
                }
                
                else if ( lineHeader.Equals("f")) {
                     
                    string vertex1, vertex2, vertex3;
                    int matches = fscanf(file, "%d/%d/%d %d/%d/%d %d/%d/%d\n", &vertexIndex[0], &uvIndex[0], &normalIndex[0], &vertexIndex[1], &uvIndex[1], &normalIndex[1], &vertexIndex[2], &uvIndex[2], &normalIndex[2] );
                    if (matches != 9){
                        printf("File can't be read by our simple parser : ( Try exporting with other options\n");
                        return false;
                    }
                    VertexIndices.Add(_vertexIndex[0]);
                    VertexIndices.Add(_vertexIndex[1]);
                    VertexIndices.Add(_vertexIndex[2]);
                    UvIndices    .Add(_uvIndex[0]);
                    UvIndices    .Add(_uvIndex[1]);
                    UvIndices    .Add(_uvIndex[2]);
                    NormalIndices.Add(_normalIndex[0]);
                    NormalIndices.Add(_normalIndex[1]);
                    NormalIndices.Add(_normalIndex[2]);
                }
                
                
                
            }
            
        }
        catch (Exception e) {
            Console.WriteLine("Impossible to open the file !");

        }
        finally {
            sr.Close();
        }
    }
    
    
    
}