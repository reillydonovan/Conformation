using UnityEngine;
using System.Collections;

namespace M4DLib {
public static class Radians
{
    public static float FromDegree (float degree)
    {
        return degree * Mathf.Deg2Rad;
    }

    public const float R30 = Mathf.PI / 6f;
    public const float R45 = Mathf.PI / 4f;
    public const float R60 = Mathf.PI / 3f;
    public const float R90 = Mathf.PI / 2f;
    public const float R180 = Mathf.PI;
    public const float R270 = Mathf.PI * 1.5f;
    public const float R360 = Mathf.PI * 2.0f;
}

}