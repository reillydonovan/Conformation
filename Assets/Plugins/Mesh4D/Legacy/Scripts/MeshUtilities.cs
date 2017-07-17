using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace M4DLib.Legacy {

public class MeshUtilities
{
    /// <summary>
    /// Rotates Vector3 by Angles.
    /// </summary>
    static public Vector3 vRot(Vector3 v, Vector3 rot, Vector3 pivot)
    {
        return pivot + (Quaternion.Euler(rot) * (v - pivot));
    }

    /// <summary>
    /// Limit/Expand Vector3 Magnitude to Specified Radius
    /// </summary>
    static public Vector3 sphereProject(Vector3 v, float r)
    {
        return v.normalized * r;
    }

    /// <summary>
    /// Returns float Value of Vector if All 3 Axes are same or NaN
    /// </summary>
    public static float DefaultOrNull(Vector3 v)
    {
        if (v[0] == v[1] && v[1] == v[2])
            return 	v[0];
        else
            return  float.NaN;
    }

    /// <summary>
    /// Limit/Expand Vector3 Magnitude to Specified Radius in Box Projection
    /// </summary>
    static public Vector3 boxProject(Vector3 v, float r)
    {
        return v * r / (Mathf.Max(Mathf.Abs(v.x), Mathf.Max(Mathf.Abs(v.y), Mathf.Abs(v.z))));
    }

    /// <summary>
    /// Make Sure Vector didn't have Zero Values
    /// </summary>
    static public Vector3 vNoZero(Vector3 v)
    {
        Vector3 r = v;
        if (r[0] == 0)
            r[0] = 0.0001f;
        if (r[1] == 0)
            r[1] = 0.0001f;
        if (r[2] == 0)
            r[2] = 0.0001f;
        return r;
    }

    static public float vNoZero(float v)
    {
        if (v == 0)
            v = 0.0001f;
        return v;
    }

    /// <summary>
    /// Returns the Maximum of Each Vector3 Axes
    /// </summary>
    static public float vMax(Vector3 v)
    {
        return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
    }

    /// <summary>
    /// Multiplies Vector3 on each axes
    /// </summary>
    static public Vector3 vMult(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    /// <summary>
    /// Divides Vector3 on each axes
    /// </summary>
    static public Vector3 vDiv(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}

}