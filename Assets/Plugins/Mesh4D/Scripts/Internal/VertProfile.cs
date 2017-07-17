using UnityEngine;
using System.Collections;
using System;

namespace M4DLib {
[Serializable]
public struct VertProfile
{
    public Color color;
    public Vector4 uv;
    public Vector4 uv2;
    public Vector4 uv3;

    /// <summary>
    /// Default state for VertProfile
    /// </summary>
    public static VertProfile initial
    {
        get {
            return new VertProfile (Color.white);
        }
    }

    public VertProfile (Color color)
    : this (color, Vector4.zero, Vector4.zero, Vector4.zero) {}
    public VertProfile (Color color, Vector4 uv)
    : this (color, uv, Vector4.zero, Vector4.zero) {}
    public VertProfile (Color color, Vector4 uv, Vector4 uv2)
    : this (color, uv, uv2, Vector4.zero) {}
    public VertProfile (Vector4 uv)
    : this (Color.white, uv, Vector4.zero, Vector4.zero) {}
    public VertProfile (Vector4 uv, Vector4 uv2)
    : this (Color.white, uv, uv2, Vector4.zero) {}
    public VertProfile (Vector4 uv, Vector4 uv2, Vector4 uv3)
    : this (Color.white, uv, uv2, uv3) {}


    public VertProfile (Color color, Vector4 uv, Vector4 uv2, Vector4 uv3)
    {
        this.color = color;
        this.uv = uv;
        this.uv2 = uv2;
        this.uv3 = uv3;
    }

    public static VertProfile Lerp (VertProfile a, VertProfile b, float t)
    {
        t = Mathf.Clamp01(t);
        return new VertProfile {
            color = Color.LerpUnclamped(a.color, b.color, t),
            uv = Vector4.LerpUnclamped(a.uv, b.uv, t),
            uv2 = Vector4.LerpUnclamped(a.uv2, b.uv2, t),
            uv3 = Vector4.LerpUnclamped(a.uv3, b.uv3, t),
        };
    }
}
}