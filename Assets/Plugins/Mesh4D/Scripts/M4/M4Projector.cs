using UnityEngine;
using System.Collections;
using System;

namespace M4DLib
{

    public static class M4Projector
    {

        internal static Matrix5x5 _matrix;
        internal static Matrix5x5 _matrixInv;
        internal static Plane4 _cullPlane;
        internal static float _ratio;
        internal static bool _perspective;
        internal static bool _cull;

        // pi * 90 / 360
        const float _QUARTERPI = 0.785398163f;

        public static void Validate(Vector4 position, Quaternion4 rotation, float scale, bool perspective, bool cull)
        {
            var rot = rotation.ToMatrix();

            _matrix = Matrix5x5.Translate(-position) * rot;
            _matrixInv =  Matrix4x4.Inverse(rot) * Matrix5x5.Translate(position);
            _cullPlane = new Plane4(rot * Vec4.over, Vec4.over * NEAR_CLIP);
            _cull = cull;

            if (_perspective = perspective)
                _ratio = scale / Mathf.Min(Mathf.Tan(_QUARTERPI / position.magnitude), 1.0f);
            else
                _ratio = scale;
        }

        internal const float NEAR_CLIP = 0.0001f;
        internal const float FOV_LIMIT = 1f;

        public static Vector3 Project(Vector4 v)
        {
            v = _matrix * v;
            if (_cull && v.w < NEAR_CLIP)
                return Vector3.zero;// = _cullPlane.GetNearestPoint(v);

            var r = _perspective ? _ratio / (v.w * v.w < 1e-4f ? 1e-2f : v.w) : _ratio;
            return new Vector3() { x = v.x * r, y = v.y * r, z = v.z * r };
        }

    }

}