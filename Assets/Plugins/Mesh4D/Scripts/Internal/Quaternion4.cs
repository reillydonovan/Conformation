

using System;
using UnityEngine.Internal;
using UnityEngine;
using M4DLib;

/// <summary>
/// Quaternion for Vector4 rotation.
/// Uses a pair of 3D Quaternion to represent 6 axis of 4D rotations.
/// </summary>
[Serializable]
public struct Quaternion4
{

    public Quaternion xyz;
    public Quaternion tuv;

    public Quaternion4 (Vector3 XYZ, Vector3 TUV)
    {
        xyz = Quaternion.Euler(XYZ);
        tuv = Quaternion.Euler(TUV);
    }

    public Quaternion4 (Quaternion XYZ, Quaternion TUV)
    {
        xyz = XYZ;
        tuv = TUV;
    }

    public static Quaternion4 identity {
        get {
            return new Quaternion4 { xyz = Quaternion.identity, tuv = Quaternion.identity };
        }
    }

    /// <summary>
    /// Get Quaternion4 from euler degrees
    /// </summary>
    public static Quaternion4 Euler (Rotation4 r)
    {
        return new Quaternion4 { xyz = Quaternion.Euler(r.xyz), tuv = Quaternion.Euler(r.tuv) };
    }

    /// <summary>
    /// Get Quaternion4 from euler degrees
    /// </summary>
    public static Quaternion4 Euler (Vector3 xyz, Vector3 tuv)
    {
        return new Quaternion4 { xyz = Quaternion.Euler(xyz), tuv = Quaternion.Euler(tuv) };
    }

    /// <summary>
    /// Get Quaternion4 from euler degrees
    /// </summary>
    public static Quaternion4 Euler (float x, float y, float z, float t, float u, float v)
    {
        return new Quaternion4 { xyz = Quaternion.Euler(x, y, z), tuv = Quaternion.Euler(t, u, v) };
    }


    /// <summary>
    /// Get Euler representation from Quaternion4
    /// </summary>
    public Rotation4 ToEuler ()
    {
        var a = xyz.eulerAngles;
        var b = tuv.eulerAngles;
        return new Rotation4(a, b);
    }

    public void Normalize ()
    {
        var q = Normalize(this);
        xyz = q.xyz; 
        tuv = q.tuv;
    }

    /// <summary>
    /// Get Matrix representation from Quaternion4
    /// </summary>
    public Matrix4x4 ToMatrix ()
    {
        Normalize();
        return ToMatrix (xyz, tuv);
    }

    public Matrix4x4 ToMatrix (Vector4 scale)
    {
        Normalize();
        return ToMatrix (xyz, tuv) * new Matrix4x4 { m00 = scale.x, m11 = scale.y, m22 = scale.z, m33 = scale.w };
    }

    /// <summary>
    /// Get the matrix representation so it can be multiplied with vectors efficiently
    /// </summary>
    static Matrix4x4 ToMatrix (Quaternion a, Quaternion b)
    {

        // a = xyz, b = tuv
        // OK. Not fast enough but this is most readable form of building pair of Quaternions to manipulate 4D.

        var m = new Matrix4x4 () {
            m00 = a.w, m01 = -a.z, m02 = a.y, m03 = a.x,
            m10 = a.z, m11 = a.w, m12 = -a.x, m13 = a.y,
            m20 = -a.y, m21 = a.x, m22 = a.w, m23 = a.z,
            m30 = -a.x, m31 = -a.y, m32 = -a.z, m33 = a.w,
        };

        var n = new Matrix4x4 () {
            m00 = a.w, m01 = -a.z, m02 = a.y, m03 = -a.x,
            m10 = a.z, m11 = a.w, m12 = -a.x, m13 = -a.y,
            m20 = -a.y, m21 = a.x, m22 = a.w, m23 = -a.z,
            m30 = a.x, m31 = a.y, m32 = a.z, m33 = a.w,
        };

        var o = new Matrix4x4 () {
            m00 = b.w, m01 = -b.z, m02 = b.y, m03 = -b.x,
            m10 = b.z, m11 = b.w, m12 = -b.x, m13 = -b.y,
            m20 = -b.y, m21 = b.x, m22 = b.w, m23 = -b.z,
            m30 = b.x, m31 = b.y, m32 = b.z, m33 = b.w,
        };

        var p = new Matrix4x4 () {
            m00 = b.w, m01 = b.z, m02 = -b.y, m03 = -b.x,
            m10 = -b.z, m11 = b.w, m12 = b.x, m13 = -b.y,
            m20 = b.y, m21 = -b.x, m22 = b.w, m23 = -b.z,
            m30 = b.x, m31 = b.y, m32 = b.z, m33 = b.w,
        };

        return m * n * o * p;

    }

    static public Quaternion4 Normalize (Quaternion4 q)
    {
        var xyz = q.xyz;
        var tuv = q.tuv;

        var scale = (Quaternion.Dot(xyz, xyz));
        if (scale < 1e-6)
            xyz = Quaternion.identity;
        else if (scale < 0.999f || scale > 1.0001f)
            xyz = Scale(xyz, 1 / Mathf.Sqrt(scale));
        
        scale = (Quaternion.Dot(tuv, tuv));
        if (scale < 1e-6)
            tuv = Quaternion.identity;
        else if (scale < 0.999f || scale > 1.0001f)
            tuv = Scale(tuv, 1 / Mathf.Sqrt(scale));

       return new Quaternion4(xyz, tuv);
    }

    static Quaternion Scale(Quaternion q, float scale)
    {
        q.x *= scale;
        q.y *= scale;
        q.z *= scale;
        q.w *= scale;
        return q;
    }

    static public Quaternion4 Inverse (Quaternion4 q)
    {
        return new Quaternion4 { xyz = Quaternion.Inverse(q.xyz), tuv = Quaternion.Inverse(q.tuv) };
    }

    static public Quaternion4 Slerp (Quaternion4 a, Quaternion4 b, float t)
    {
        t = Mathf.Clamp01(t);
        return new Quaternion4(Quaternion.SlerpUnclamped(a.xyz, b.xyz, t), Quaternion.SlerpUnclamped(a.tuv, b.tuv, t));
    }

    // static public Quaternion4 SmoothDamp (Quaternion4 current, Quaternion4 target, float time, ref Quaternion4 speed);

    public static Quaternion4 operator * (Quaternion4 a, Quaternion4 b)
    {
        return new Quaternion4 { xyz = a.xyz * b.xyz, tuv = a.tuv * b.tuv };
    }

    public static Vector4 operator * (Quaternion4 a, Vector4 b)
    {
        return a.ToMatrix() * b;
    }


    public static implicit operator Quaternion4 (Quaternion q)
    {
        return new Quaternion4(q, Quaternion.identity);
    }

    public static bool operator == (Quaternion4 a, Quaternion4 b)
    {
        return a.xyz == b.xyz && a.tuv == b.tuv;
    }

    public static bool operator != (Quaternion4 a, Quaternion4 b)
    {
        return a.xyz != b.xyz || a.tuv != b.tuv;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Quaternion4))
            return false;
        return this == (Quaternion4)obj;
    }

    public override int GetHashCode()
    {
        return xyz.GetHashCode() ^ tuv.GetHashCode();
    }

}
 
