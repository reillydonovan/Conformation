using UnityEngine;
using System;

namespace M4DLib
{

    /// <summary>
    /// Extensions shortcut and helper for Vector4 manipulations
    /// </summary>
    public static class Vec4
    {
        public static Vector4 zero { get { return new Vector4 { x = 0f, y = 0f, z = 0f, w = 0f }; } }

        public static Vector4 one { get { return new Vector4 { x = 1f, y = 1f, z = 1f, w = 1f }; } }

        public static Vector4 forward { get { return new Vector4 { x = 0f, y = 0f, z = 1f, w = 0f }; } }

        public static Vector4 back { get { return new Vector4 { x = 0f, y = 0f, z = -1f, w = 0f }; } }

        public static Vector4 up { get { return new Vector4 { x = 0f, y = 1f, z = 0f, w = 0f }; } }

        public static Vector4 down { get { return new Vector4 { x = 0f, y = -1f, z = 0f, w = 0f }; } }

        public static Vector4 left { get { return new Vector4 { x = -1f, y = 0f, z = 0f, w = 0f }; } }

        public static Vector4 right { get { return new Vector4 { x = 1f, y = 0f, z = 0f, w = 0f }; } }

        public static Vector4 over { get { return new Vector4 { x = 0f, y = 0f, z = 0f, w = 1f }; } }

        public static Vector4 under { get { return new Vector4 { x = 0f, y = 0f, z = 0f, w = -1f }; } }

        static public float Dot(Vector4 lhs, Vector4 rhs)
        {
            return (lhs.x * rhs.x) + (lhs.y * rhs.y) + (lhs.z * rhs.z) + (lhs.w * rhs.w);
        }

        static public Vector4 Cross(Vector4 U, Vector4 V, Vector4 W)
        {
            float A, B, C, D, E, F; // Intermediate Values 
        
            Vector4 r = Vector4.zero;
            A = (V.x * W.y) - (V.y * W.x);
            B = (V.x * W.z) - (V.z * W.x);
            C = (V.x * W.w) - (V.w * W.x);
            D = (V.y * W.z) - (V.z * W.y);
            E = (V.y * W.w) - (V.w * W.y);
            F = (V.z * W.w) - (V.w * W.z);

            r.x = (U.y * F) - (U.z * E) + (U.w * D);
            r.y = -(U.x * F) + (U.z * C) - (U.w * B);
            r.z = (U.x * E) - (U.y * C) + (U.w * A);
            r.w = -(U.x * D) + (U.y * B) - (U.z * A);

            return r;
        }

        static public float Min(Vector4 vec)
        {
            return Mathf.Min(Mathf.Min(vec.x, vec.y), Mathf.Min(vec.z, vec.w));
        }

        static public float Max(Vector4 vec)
        {
            return Mathf.Max(Mathf.Max(vec.x, vec.y), Mathf.Max(vec.z, vec.w));
        }

        static public Vector4 Abs(Vector4 v)
        {
            return new Vector4
            {
                x = Mathf.Abs(v.x),
                y = Mathf.Abs(v.y),
                z = Mathf.Abs(v.z),
                w = Mathf.Abs(v.w),
            };
        }

        public static Vector4 Project(Vector4 vector, Vector4 onNormal)
        {
            float num = Vector4.Dot(onNormal, onNormal);
            if (num < Mathf.Epsilon)
                return Vector4.zero;
            else
                return onNormal * Vector4.Dot(vector, onNormal) / num;
        }

        public static Vector4 ProjectOnPlane(Vector4 vector, Vector4 planeNormal)
        {
            return vector - Vector4.Project(vector, planeNormal);
        }

        public static Vector4 Reflect(Vector4 inDirection, Vector4 inNormal)
        {
            return -2 * Vector4.Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        public static Vector4 Scale (Vector4 a, Vector4 b)
        {
            return new Vector4 { x = a.x * b.x, y = a.y * b.y, z = a.z * b.z, w = a.w * b.w };
        }

        public static Vector4 Divide (Vector4 a, Vector4 b)
        {
            return new Vector4 { x = a.x / b.x, y = a.y / b.y, z = a.z / b.z, w = a.w / b.w };
        }
    }
}