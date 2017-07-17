using UnityEngine;
using System.Collections.Generic;

namespace M4DLib {

/// <summary>
/// Internal helper class for slicing 4D data into 3D mesh representation
/// </summary>
static public class S4Slicer
{
        static public bool _computeProfiles = false;
        static public V3Helper _helper;

        static readonly int[] _leftEdges = new int[] {
            0, 0, 0, 1, 1, 2,
        };

        static readonly int[] _rightEdges = new int[] {
            1, 2, 3, 2, 3, 3,
        };

        static readonly int[] _quadToTris = new int[] {
            0, 1, 2, 1, 2, 3,
        };

        // These actually not static but... hell yeah for crazy micro optimisation

        static bool[] _trimidSides = new bool[4];

        // Actually filled by commented function below
        // But due to performance reason we have to opt-it-out
        // And fill directly to the code manually
        static public Vector4[] _preBuffer = new Vector4[4];
        static Vector3[] _postBuffer = new Vector3[4];

        static int _postIndices = 0;
        static public VertProfile[] _preProfile = new VertProfile[4];
        static VertProfile[] _postProfile = new VertProfile[4];

        static internal Plane4 _slicer;
        static internal Matrix5x5 _matrix;
        static internal Matrix5x5 _matrixInv;
        static bool _cull;

        /*
        /// <summary>
        /// Loads the vertex to buffer. Called per one trimid
        /// </summary>
        public static void LoadVertex (Vector4 a, Vector4 b, Vector4 c, Vector4 d)
        {
            _preBuffer[0] = _preMatrix * a;
            _preBuffer[1] = _preMatrix * b;
            _preBuffer[2] = _preMatrix * c;
            _preBuffer[3] = _preMatrix * d;
        }

        /// <summary>
        /// Loads the vertex profile if needed. Called per one trimid
        /// </summary>
        public static void LoadProfiles (VertProfile a, VertProfile b, VertProfile c, VertProfile d)
        {
            _preProfile[0] = a;
            _preProfile[1] = b;
            _preProfile[2] = c;
            _preProfile[3] = d;
        }*/

        /// <summary>
        /// Push the calculated vertexes. Called per one trimid after Intersect()
        /// </summary>
        static public void SaveVertexes ()
        {
            if (_postIndices == 0)
                return;
            var v = _helper.m_Verts; var t = _helper.m_Tris[0];
            var n = _postIndices == 4 ? 6 : 3;
            for (int i = 0; i < n; i++)
            {
                 v.Add (_postBuffer[_quadToTris[i]]);
            }
            var l = v.Count;
            for (int i = n; i >= 1; i--)
            {
                 t.Add(l - i);
            }

            if (_computeProfiles)
            {
                for (int i = 0; i < n; i++)
                {
                    _helper.AddProfile(_postProfile[_quadToTris[i]]);
                }
            }
        }

        /// <summary>
        /// Internally load the slicer parameters. Called before all sciling is performed
        /// </summary>
        static public void LoadSlicer (Quaternion4 orientation, Vector4 position, bool cull)
        {
            var mtx = orientation.ToMatrix();
             // For the plane, we just need to set up the direcion/rotation
             _slicer = new Plane4(mtx * Vec4.over, position);
             // This for reorienting verts so W will always zero.
            _matrix = Matrix4x4.Inverse(mtx) * Matrix5x5.Translate(-position);
            _matrixInv = Matrix5x5.Translate(position) * mtx;
            _cull = cull;
        }

        static public bool IsOptedOut (Bounds4 bounds)
        {
            return _cull && !(_slicer.IntersectsBounds(bounds));
        }

        /// <summary>
        /// Core of all slicing operation
        /// </summary>
        static public bool Intersect ()
        {
            int i;
            {
                // Determine if trimid points is above the plane or not
                var allFalse = false;
                var allTrue = true;
                for (i = 0; i < 4; i++)
                {
                    // Equivalent to Plane4.GetSide but we won't add another depth call here
                    var eval = _trimidSides[i] = _slicer.GetSide(_preBuffer[i]);
                    allFalse |= eval;
                    allTrue &= eval;
                }

                if (!allFalse | allTrue) 
                    return false;
            }

            var iter = 0;

            for (i = 0; i < 6; i++)
            {
                var a = _leftEdges[i];
                var b = _rightEdges[i];
                if (_trimidSides[a] ^ _trimidSides[b])
                {
                       var x = _preBuffer[a];
                       var y = _preBuffer[b];
                       float phase = _slicer.Intersect(x, y);
                       var v = _matrix * (Vector4.LerpUnclamped(x, y, phase));
                        if (v.w * v.w > 0.001)
                           Debug.LogError(v.ToString() + " " + (Vector4.LerpUnclamped(x, y, phase).ToString()));
                       _postBuffer[iter] = v;
                       if (_computeProfiles)
                            _postProfile[iter] = (VertProfile.Lerp(_preProfile[a], _preProfile[b], phase));
                       iter++;
                }
            }
            _postIndices = iter;
            return true;
        }


}

}