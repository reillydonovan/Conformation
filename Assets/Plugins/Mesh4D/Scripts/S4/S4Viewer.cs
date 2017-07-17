using UnityEngine;
using System;
using System.Collections.Generic;

namespace M4DLib
{

    [ExecuteInEditMode]
    [AddComponentMenu("Scripts/Mesh4D/S4 Viewer")]
    public partial class S4Viewer : MonoBehaviour, IViewer
    {

        [SerializeField] Vector4 m_position = Vec4.zero;
        [SerializeField] Quaternion4 m_rotation = Quaternion4.identity;
        [SerializeField] bool m_cull = true;

        public Vector4 position
        {
            get
            {
                return m_position;
            }
            set
            {
                if (m_position != value)
                {
                    m_position = value;
                    SetDirty(false);
                }
            }
        }

        public Quaternion4 rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                if (m_rotation != value)
                {
                    m_rotation = value;
                    SetDirty(false);
                }
            }
        }

        public Matrix5x5 worldToCameraMatrix
        {
            get
            {
                return Matrix5x5.Inverse(Matrix5x5.Translate(position)
                    * rotation.ToMatrix());
            }
        }

        public void ValidateMatrix()
        {
            if (!_matrixDirty)
                return;
        
            S4Slicer.LoadSlicer(m_rotation, m_position, m_cull);
            _matrixDirty = false;
        }

        List<S4Renderer> _renderers = new List<S4Renderer>();
        [NonSerialized] bool _matrixDirty = true;

        public List<S4Renderer> renderers
        {
            get
            {
                if (_renderers.Count == 0)
                    GetComponentsInChildren<S4Renderer>(false, _renderers);
                return _renderers;
            }
        }

        void OnValidate()
        {
            SetDirty(false);
        }

        void OnDisable()
        {
        }

        void OnEnable()
        {
            SetDirty(true);
            RearquireRenderers();
        }

        void OnTransformChildrenChanged()
        {
            RearquireRenderers();
        }


        public void RearquireRenderers()
        {
            renderers.Clear();
        }

        public void SetDirty(bool rebuild)
        {
            _matrixDirty = true;
            for (int i = 0; i < renderers.Count; i++)
            {
                if (_renderers[i])
                    _renderers[i].SetDirty(rebuild);
            }
        }

        public bool WorldToProjectionPoint(Vector4 v, out Vector3 projected)
        {
            return WorldToProjectionPoint(v, 1e-4f, out projected);
        }

        public bool WorldToProjectionPoint(Vector4 v, float thickness, out Vector3 projected)
        {
            ValidateMatrix();
            var plane = S4Slicer._slicer;
            projected = S4Slicer._matrix * plane.GetNearestPoint(v); // Always W = 0
            return Math.Abs(plane.GetDistanceToPoint(v)) <= thickness;
        }

        public Vector4 ProjectionToWorldPoint(Vector3 v)
        {
            ValidateMatrix();
            return S4Slicer._matrixInv.MultiplyPoint4x5(v); // Assume W = 0
        }

        public Vector3 WorldToProjectionVector(Vector4 dir)
        {
            ValidateMatrix();
            return S4Slicer._matrix.MultiplyVector(dir); // Always W = 0
        }

        public Vector4 ProjectionToWorldVector(Vector3 v)
        {
            ValidateMatrix();
            return S4Slicer._matrixInv.MultiplyVector(v); // Assume W = 0
        }


        public Quaternion WorldToProjectionOrientation(Quaternion4 dir)
        {
            ValidateMatrix();
            return (rotation * (dir)).xyz;
        }

        public Quaternion4 ProjectionToWorldOrientation(Quaternion v)
        {
            ValidateMatrix();
            return new Quaternion4(v, Quaternion.identity) * rotation; 
        }
   
    }
}