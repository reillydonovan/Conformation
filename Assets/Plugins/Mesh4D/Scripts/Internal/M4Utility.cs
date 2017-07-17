using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace M4DLib {

public static class M4Utility
{
    /// <summary>
    /// Sets the w and return Vector4
    /// </summary>
    static public Vector4 SetW(this Vector3 xyz, float w)
    {
        return new Vector4(xyz.x, xyz.y, xyz.z, w);
    }

    /// <summary>
    /// Expand list capacity to given capacity target
    /// </summary>
    static public void Expand<T>(this List<T> list, int capacity)
    {
        if (list.Capacity < capacity)
            list.Capacity = capacity;
    }

    /// <summary>
    /// Expand list capacity to what another list has
    /// </summary>
    static public void Expand<T, T2>(this List<T> list, List<T2> capacity)
    {
        if (list.Capacity < capacity.Capacity)
            list.Capacity = capacity.Capacity;
    }

    static public void CopyTo(this Mesh source, Mesh target)
    {
        CombineInstance c = new CombineInstance()
        { 
            mesh = source,
            subMeshIndex = 0
        };
        
        target.CombineMeshes(new CombineInstance[1] { c }, false, false);
       
       // target.triangles = source.triangles;
    }

    static public void CopyTo(this Mesh source, Mesh target, Matrix4x4 TRS)
    {
        CombineInstance c = new CombineInstance()
            { 
                mesh = source,
                transform = TRS
            };
        target.CombineMeshes(new CombineInstance[1] { c }, false, true);
        // target.triangles = source.triangles;
    }


    static public void CopyTo(this Mesh[] source, Mesh target)
    {
        List<int> tris = new List<int>();
        int lastC = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if(i > 0)
                lastC += source[i - 1].vertices.Length;
            tris.AddRange(Array.ConvertAll<int, int>(source[i].triangles, x => x + lastC));
        }
        CombineInstance[] c = Array.ConvertAll<Mesh, CombineInstance>(
            source, x => new CombineInstance() { mesh = x });
        target.CombineMeshes(c, false, false);
        target.triangles = tris.ToArray();
    }

    static public bool ColorEqual(Color32 a, Color32 b)
    {
        return Mathf.Abs(a.r - b.r) < 2 && Mathf.Abs(a.g - b.g) < 2 
            && Mathf.Abs(a.b - b.b) < 2;// && Mathf.Abs(a.a - b.a) < 2;
    }

    public static int MatchIndex<TSource> (this TSource[] source, TSource match)
    {
        for (int i = 0; i < source.Length; i++)
        {
            if(match.Equals(source[i]))
                return i;
        }
        return -1;
    }

    public static Color Hue (float h)
    {
        return Color.HSVToRGB(h, 1, 1);
    }

    public static T GetComponentFromParent <T> (Transform start) where T:class
    {
        while (start = start.parent)
        {
            if (start.GetComponent<T>() == null)
                continue;
            return start.GetComponent<T>();
        }
        return null;
    }


    public static Bounds InfinityBounds {
        get {
            return new Bounds(Vector3.zero, Vector3.one * Mathf.NegativeInfinity);
        }
    }

    public static Vector3 DivAndCheckInfinity (Vector3 v)
    {
        for (int i = 0; i < 3; i++)
        {
            v[i] = 1 / v[i];
            if (float.IsInfinity(v[i]))
                v[i] = 1;
        }
        return v;
    }

    // https://stackoverflow.com/questions/38100945/project-3d-coordinates-to-a-2d-plane
    public static Vector2 ProjectUV (Vector3 V, Vector3 N, Bounds B)
    {
        V = Vector3.ProjectOnPlane(V, N);
        V.x = V.x - B.min.x / (B.max.x - B.min.x);
        V.y = V.y - B.min.y / (B.max.y - B.min.y);
        V.z = V.z - B.min.z / (B.max.z - B.min.z);
        V = Quaternion.FromToRotation(N, Vector3.forward) * V;
        return V;
    }

    public static void Destroy (UnityEngine.Object obj)
    {
        if (!obj)
            return;
        if (Application.isPlaying)
            UnityEngine.Object.Destroy(obj);
        else
            UnityEngine.Object.DestroyImmediate(obj);
        
    }

    public static Vector3 GetNormal (Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Normalize(Vector3.Cross(c - a, b - a));
    }

    public static Vector3 GetNormal (List<Vector3> verts, List<int> tris, int startingTriIdx)
    {
            var i = startingTriIdx;
            return GetNormal(verts[tris[i]], verts[tris[i + 1]], verts[tris[i + 2]]);
    }
}
}