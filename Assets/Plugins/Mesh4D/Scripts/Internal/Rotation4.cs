using UnityEngine;
using System;

namespace M4DLib {
/// Struct for holds a Vector4 Rotations
/// in 4D there are 4! or 6 rotation axes
[Serializable]
public struct Rotation4
{
    /// XY Plane, in radians
    [SerializeField] float XY;
    /// XZ Plane, in radians
    [SerializeField] float XZ;
    /// YZ Plane, in radians
    [SerializeField] float YZ;
    /// XW Plane, in radians
    [SerializeField] float XW;
    /// YW Plane, in radians
    [SerializeField] float YW;
    /// ZW Plane, in radians
    [SerializeField] float ZW;

    public float this[int i]
    {
        get {
            switch (i)
            {
                case 0:
                    return yz;
                case 1:
                    return xz;
                case 2:
                    return xy;
                case 3:
                    return xw;
                case 4:
                    return yw;
                case 5:
                    return zw;
                default:
                    throw new IndexOutOfRangeException("Invalid Rotation4 Index!");
            }
        }
        set {
            switch (i)
            {
                case 0:
                    yz = value;
                    break;
                case 1:
                    xz = value;
                    break;
                case 2:
                    xy = value;
                    break;
                case 3:
                    xw = value;
                    break;
                case 4:
                    yw = value;
                    break;
                case 5:
                    zw = value;
                    break;
            }
        }
    }

    /// <summary> XY Plane, in degrees </summary>
    public float xy {
        get {
            return XY * Mathf.Rad2Deg;
        }
        set {
            XY = value * Mathf.Deg2Rad;
        }
    }

    /// <summary> XZ Plane, in degrees </summary>
    public float xz {
        get {
            return XZ * Mathf.Rad2Deg;
        }
        set {
            XZ = value * Mathf.Deg2Rad;
        }
    }
    /// <summary> XW Plane, in degrees </summary>
    public float xw {
        get {
            return XW * Mathf.Rad2Deg;
        }
        set {
            XW = value * Mathf.Deg2Rad;
        }
    }
    /// <summary> YZ Plane, in degrees </summary>
    public float yz {
        get {
            return YZ * Mathf.Rad2Deg;
        }
        set {
            YZ = value * Mathf.Deg2Rad;
        }
    }

    /// <summary> YW Plane, in degrees </summary>
    public float yw {
        get {
            return YW * Mathf.Rad2Deg;
        }
        set {
            YW = value * Mathf.Deg2Rad;
        }
    }

    /// <summary> ZW Plane, in degrees </summary>
    public float zw {
        get {
            return ZW * Mathf.Rad2Deg;
        }
        set {
            ZW = value * Mathf.Deg2Rad;
        }
    }

    
    public static Rotation4 identity {
        get {
            return new Rotation4();            
        }
    }

    public Rotation4(Vector3 xyz, Vector3 w)
    {
        YZ = xyz.x * Mathf.Deg2Rad;
        XZ = xyz.y * Mathf.Deg2Rad;
        XY = xyz.z * Mathf.Deg2Rad;
        XW = w.x * Mathf.Deg2Rad;
        YW = w.y * Mathf.Deg2Rad;
        ZW = w.z * Mathf.Deg2Rad;
    }

    public Rotation4(float x, float y, float z, float wx, float wy, float wz)
    {
        YZ = x * Mathf.Deg2Rad;
        XZ = y * Mathf.Deg2Rad;
        XY = z * Mathf.Deg2Rad;
        XW = wx * Mathf.Deg2Rad;
        YW = wy * Mathf.Deg2Rad;
        ZW = wz * Mathf.Deg2Rad;
    }

    public override int GetHashCode()
    {
        return this.XW.GetHashCode() ^ this.YW.GetHashCode() << 2 ^ this.YZ.GetHashCode() >> 2 ^ this.ZW.GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
        if (!(other is Rotation4))
            return false;
        Rotation4 r = (Rotation4)other;
        return this == r;
    }

    public Vector3 xyz
    {
        get
        { 
            return new Vector3(YZ, XZ, XY) * Mathf.Rad2Deg; 
        }
        set
        {
            Vector3 v = value * Mathf.Deg2Rad;
            YZ = v.x;
            XZ = v.y;
            XY = v.z;
        }
    }

    public Vector3 tuv
    {
        get
        { 
            return new Vector3(XW, YW, ZW) * Mathf.Rad2Deg; 
        }
        set
        {
            Vector3 v = value * Mathf.Deg2Rad;
            XW = v.x;
            YW = v.y;
            ZW = v.z;
        }
    }

    /// <summary>
    /// Get the quaternion representation.
    /// Equivalent to Quaternion4.Euler()
    /// </summary>
    public Quaternion4 ToQuaternion()
    {
        return Quaternion4.Euler(this);
    }

    /// <summary>
    /// Gets the rotation in matrix representation
    /// </summary>
    public Matrix4x4 ToMatrix()
    {
        Matrix4x4 M = Matrix4x4.identity, R = Matrix4x4.identity;
        if (XZ != 0)
        {
            float sXZ = Mathf.Sin(XZ), cXZ = Mathf.Cos(XZ);
            M = Matrix4x4.identity;
            M[0, 0] = cXZ;
            M[0, 2] = -sXZ;
            M[2, 0] = sXZ;
            M[2, 2] = cXZ;
            R = M * R;
        }
        if (XY != 0)
        {
            float sXY = Mathf.Sin(XY), cXY = Mathf.Cos(XY);
            M = Matrix4x4.identity;
            M[0, 0] = cXY;
            M[0, 1] = sXY;
            M[1, 0] = -sXY;
            M[1, 1] = cXY;
            R = M * R;
        }
        if (YZ != 0)
        {
            float sYZ = Mathf.Sin(YZ), cYZ = Mathf.Cos(YZ);
            M = Matrix4x4.identity;
            M[1, 1] = cYZ;
            M[1, 2] = sYZ;
            M[2, 1] = -sYZ;
            M[2, 2] = cYZ;
            R = M * R;
        }
        if (YW != 0)
        {
            float sYW = Mathf.Sin(YW), cYW = Mathf.Cos(YW);
            M = Matrix4x4.identity;
            M[1, 1] = cYW;
            M[1, 3] = -sYW;
            M[3, 1] = sYW;
            M[3, 3] = cYW;
            R = M * R;
        }
        if (ZW != 0)
        {
            float sZW = Mathf.Sin(ZW), cZW = Mathf.Cos(ZW);
            M = Matrix4x4.identity;
            M[2, 2] = cZW;
            M[2, 3] = -sZW;
            M[3, 2] = sZW;
            M[3, 3] = cZW;
            R = M * R;
        }
        if (XW != 0)
        {
            float sXW = Mathf.Sin(XW), cXW = Mathf.Cos(XW);
            M = Matrix4x4.identity;
            M[0, 0] = cXW;
            M[0, 3] = -sXW;
            M[3, 0] = sXW;
            M[3, 3] = cXW;
            R = M * R;
        }
        return R;
    }

    /// <summary>
    /// Gets the rotation in inverse matrix representation.
    /// This one is much faster than ToMatrix().inverse
    /// </summary>
    public Matrix4x4 ToMatrixInverse()
    {
        Matrix4x4 M = Matrix4x4.identity, R = Matrix4x4.identity;
        if (XW != 0)
        {
            float sXW = Mathf.Sin(XW), cXW = Mathf.Cos(XW);
            M = Matrix4x4.identity;
            M[0, 0] = cXW;
            M[0, 3] = sXW;
            M[3, 0] = -sXW;
            M[3, 3] = cXW;
            R = M * R;
        }
        if (ZW != 0)
        {
            float sZW = Mathf.Sin(ZW), cZW = Mathf.Cos(ZW);
            M = Matrix4x4.identity;
            M[2, 2] = cZW;
            M[2, 3] = sZW;
            M[3, 2] = -sZW;
            M[3, 3] = cZW;
            R = M * R;
        }
        if (YW != 0)
        {
            float sYW = Mathf.Sin(YW), cYW = Mathf.Cos(YW);
            M = Matrix4x4.identity;
            M[1, 1] = cYW;
            M[1, 3] = sYW;
            M[3, 1] = -sYW;
            M[3, 3] = cYW;
            R = M * R;
        }
        if (YZ != 0)
        {
            float sYZ = Mathf.Sin(YZ), cYZ = Mathf.Cos(YZ);
            M = Matrix4x4.identity;
            M[1, 1] = cYZ;
            M[1, 2] = -sYZ;
            M[2, 1] = sYZ;
            M[2, 2] = cYZ;
            R = M * R;
        }
        if (XY != 0)
        {
            float sXY = Mathf.Sin(XY), cXY = Mathf.Cos(XY);
            M = Matrix4x4.identity;
            M[0, 0] = cXY;
            M[0, 1] = -sXY;
            M[1, 0] = sXY;
            M[1, 1] = cXY;
            R = M * R;
        }
        if (XZ != 0)
        {
            float sXZ = Mathf.Sin(XZ), cXZ = Mathf.Cos(XZ);
            M = Matrix4x4.identity;
            M[0, 0] = cXZ;
            M[0, 2] = sXZ;
            M[2, 0] = -sXZ;
            M[2, 2] = cXZ;
            R = M * R;
        }
         return R;
    }
 
    /// <summary>
    /// Gets the rotation in inverse matrix representation
    /// You can also input the scale directly after rotation
    /// </summary>
   /* public Matrix4x4 ToMatrix(Vector4 scale)
    {
        if (scale == Vector4.one)
            return ToMatrix();
        Matrix4x4 M = Matrix4x4.identity, R = Matrix4x4.identity;
        float rX = scale.x, rY = scale.y, rZ = scale.z, rW = scale.w;
        if (XY != 0 || rX != 1 || rY != 1)
        {
            float sXY = Mathf.Sin(XY), cXY = Mathf.Cos(XY);
            M = Matrix4x4.identity;
            M[0, 0] = cXY * rX;
            M[0, 1] = sXY * rX;
            M[1, 0] = -sXY * rY;
            M[1, 1] = cXY * rY;
            R = M * R;
        }
        if (XZ != 0 || rX != 1 || rZ != 1)
        {
            float sXZ = Mathf.Sin(XZ), cXZ = Mathf.Cos(XZ);
            M = Matrix4x4.identity;
            M[0, 0] = cXZ * rX;
            M[0, 2] = sXZ * rX;
            M[2, 0] = -sXZ * rZ;
            M[2, 2] = cXZ * rZ;
            R = M * R;
        }
        if (XW != 0 || rX != 1 || rW != 1)
        {
            float sXW = Mathf.Sin(XW), cXW = Mathf.Cos(XW);
            M = Matrix4x4.identity;
            M[0, 0] = cXW * rX;
            M[0, 3] = sXW * rX;
            M[3, 0] = -sXW * rW;
            M[3, 3] = cXW * rW;
            R = M * R;
        }
        if (YZ != 0 || rY != 1 || rZ != 1)
        {
            float sYZ = Mathf.Sin(YZ), cYZ = Mathf.Cos(YZ);
            M = Matrix4x4.identity;
            M[1, 1] = cYZ * rY;
            M[1, 2] = sYZ * rY;
            M[2, 1] = -sYZ * rZ;
            M[2, 2] = cYZ * rZ;
            R = M * R;
        }
        if (YW != 0 || rY != 1 || rW != 1)
        {
            float sYW = Mathf.Sin(YW), cYW = Mathf.Cos(YW);
            M = Matrix4x4.identity;
            M[1, 1] = cYW * rY;
            M[1, 3] = sYW * rY;
            M[3, 1] = -sYW * rW;
            M[3, 3] = cYW * rW;
            R = M * R;
        }
        if (ZW != 0 || rZ != 1 || rW != 1)
        {
            float sZW = Mathf.Sin(ZW), cZW = Mathf.Cos(ZW);
            M = Matrix4x4.identity;
            M[2, 2] = cZW * rZ;
            M[2, 3] = sZW * rZ;
            M[3, 2] = -sZW * rW;
            M[3, 3] = cZW * rW;
            R = M * R;
        }
        return R;
    }
    */

    static public Rotation4 SmoothDamp (Rotation4 current, Rotation4 target, ref Rotation4 currentVelocity, float smoothTime) {
        current.XY = Mathf.SmoothDampAngle(current.XY, target.XY, ref currentVelocity.XY, smoothTime);
        current.XZ = Mathf.SmoothDampAngle(current.XZ, target.XZ, ref currentVelocity.XZ, smoothTime);
        current.XW = Mathf.SmoothDampAngle(current.XW, target.XW, ref currentVelocity.XW, smoothTime);
        current.YZ = Mathf.SmoothDampAngle(current.YZ, target.YZ, ref currentVelocity.YZ, smoothTime);
        current.YW = Mathf.SmoothDampAngle(current.YW, target.YW, ref currentVelocity.YW, smoothTime);
        current.ZW = Mathf.SmoothDampAngle(current.ZW, target.ZW, ref currentVelocity.ZW, smoothTime);
        return current;
    }

    static public Matrix4x4 operator *(Rotation4 a, Rotation4 b)
    {
        return a.ToMatrix() * b.ToMatrix();
    }
    
    static public implicit operator Matrix4x4(Rotation4 a) {
        return a.ToMatrix();
    }
    
    static public Vector4 operator *(Rotation4 a, Vector4 b)
    {
        return a.ToMatrix() * b;
    }

    static public bool operator ==(Rotation4 a, Rotation4 b)
    {
        return (a.XY == b.XY) && (a.XZ == b.XZ) && (a.XW == b.XW) && (a.YZ == b.YZ) && (a.YW == b.YW) && (a.ZW == b.ZW);
    }

    static public bool operator !=(Rotation4 a, Rotation4 b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return string.Format("[Rotation4:{0}]", ToMatrix());
    }
}
}