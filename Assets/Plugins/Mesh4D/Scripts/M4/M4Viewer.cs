using UnityEngine;
using System;
using System.Collections.Generic;

namespace M4DLib
{

    [AddComponentMenu("Scripts/Mesh4D/M4 Viewer")]
    [ExecuteInEditMode]
    public partial class M4Viewer : MonoBehaviour, IViewer
    {
        [SerializeField] Vector4 m_position = Vec4.under * 5;
        [SerializeField] Quaternion4 m_rotation;
        [SerializeField] bool m_perspective = true;
        [SerializeField] float m_scale = 1;
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

        public float scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                if (m_scale != value)
                {
                    m_scale = value;
                    SetDirty(false);
                }
            }
        }

        public bool perspective
        {
            get
            {
                return m_perspective;
            }
            set
            {
                if (m_perspective != value)
                {
                    m_perspective = value;
                    SetDirty(false);
                }
            }
        }


        public bool cull
        {
            get
            {
                return m_cull;
            }
            set
            {
                if (m_cull != value)
                {
                    m_cull = value;
                    SetDirty(false);
                }
            }
        }


        public void ValidateMatrix()
        {
            if (!_matrixDirty)
                return;

            M4Projector.Validate(position, rotation, scale, perspective, cull);
            _matrixDirty = false;
        }

        public Matrix5x5 worldToCameraMatrix
        {
            get {
                return Matrix5x5.Inverse(Matrix5x5.Translate(position) 
                    * rotation.ToMatrix() * Matrix5x5.Scale(Vector4.one * scale));
            }
        }

        List<M4Renderer> _renderers = new List<M4Renderer>();

        public List<M4Renderer> renderers
        {
            get
            {
                if (_renderers.Count == 0)
                    GetComponentsInChildren<M4Renderer>(false, _renderers);
                return _renderers;
            }
        }


        public void RearquireRenderers()
        {
            renderers.Clear();
        }

        bool _matrixDirty = true;

        public void SetDirty(bool rebuild)
        {
            _matrixDirty = true;
            for (int i = 0; i < renderers.Count; i++)
            {
                _renderers[i].SetDirty(rebuild);
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

        public bool WorldToProjectionPoint(Vector4 v, out Vector3 projected)
        {
            ValidateMatrix();
            var plane = M4Projector._cullPlane;
            projected = M4Projector.Project(v);
            return plane.GetDistanceToPoint(v) > M4Projector.NEAR_CLIP;
        }

        public Vector4 ProjectionToWorldPoint(Vector3 v)
        {
            ValidateMatrix();
            return M4Projector._matrixInv.MultiplyPoint4x5(v); // Assume W = 0
        }

        public Vector3 WorldToProjectionVector(Vector4 dir)
        {
            ValidateMatrix();
            return M4Projector._matrix.MultiplyVector(dir);
        }

        public Vector4 ProjectionToWorldVector(Vector3 v)
        {
            ValidateMatrix();
            return M4Projector._matrixInv.MultiplyVector(v); // Assume W = 0
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