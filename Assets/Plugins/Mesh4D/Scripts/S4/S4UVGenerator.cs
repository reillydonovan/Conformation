using UnityEngine;
using System.Collections.Generic;

namespace M4DLib {

public class S4UVGenerator
{
    public static void Generate (V3Helper helper)
    {
        var v3 = helper.m_Verts;
        var tr = helper.m_Tris[0];
        var uv = helper.m_Uv0;

        while (uv.Count < v3.Count)
        {
            uv.Add(Vector4.zero);
        }

        var B = M4Utility.InfinityBounds;
        for (int i = 0; i < v3.Count; i++)
        {
            B.Encapsulate(v3[i]);
        }

        var T = M4Utility.DivAndCheckInfinity(Vector3.one * 10);
        for (int i = 0; i < tr.Count; i+= 3)
        {
            var a = tr[i];
            var b = tr[i + 1];
            var c = tr[i + 2];
            var N = Vector3.Cross(v3[c] - v3[a], v3[b] - v3[a]).normalized;
            var L = Vector3.Dot(N, N);
            if (L < Mathf.Epsilon)
                continue;
            var Q = Quaternion.FromToRotation(N, Vector3.forward);
            L = 1 / L;
            for (int l = 0; l < 3; l++) {
                var n = tr[i + l];
                var V = v3[n];
                // ProjectOnPlane
                V = V - N * Vector3.Dot(V, N) * L;
                // InverseLerp
                V = Vector3.Scale(V - B.min, T);
                uv[n] = Q * V;
            }
        }
    }
}

}