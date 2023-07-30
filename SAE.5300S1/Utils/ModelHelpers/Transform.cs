using System.Numerics;

namespace SAE._5300S1.Utils.ModelHelpers; 

public class Transform {
    public Vector3 Position { get; set; } = new(0, 0, 0);

    public float Scale { get; set; } = 1f;

    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    

    //TRS
    public Matrix4x4 ModelMatrix => Matrix4x4.Identity * Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale);
    public Matrix4x4 ViewMatrix => Matrix4x4.Identity * Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale);
    
    
    public static Quaternion RotateY(float rotationAngle) {
        var q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationAngle);
        var aVector = Vector3.UnitX;
        return q * (new Quaternion(aVector, 0)) * Quaternion.Conjugate(q);
    }
    
    public static Quaternion RotateZ(float rotationAngle) {
        var q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rotationAngle);
        var aVector = Vector3.UnitY;
        return q * (new Quaternion(aVector, 0)) * Quaternion.Conjugate(q);
    }
    
    public static Quaternion RotateX(float rotationAngle) {
        var q = Quaternion.CreateFromAxisAngle(Vector3.UnitX, rotationAngle);
        var aVector = Vector3.UnitY;
        return q * (new Quaternion(aVector, 0)) * Quaternion.Conjugate(q);
    }
    
}
