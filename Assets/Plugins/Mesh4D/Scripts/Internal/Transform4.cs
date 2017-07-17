using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib
{

    [DisallowMultipleComponent]
    public class Transform4 : MonoBehaviour
    {
        [SerializeField] Vector4 m_Position = Vector4.zero;
        [SerializeField] Quaternion4 m_Rotation = Quaternion4.identity;
        [SerializeField] Vector4 m_Scale = Vector4.one;

        public Vector4 localPosition
        {
            get
            {
                return m_Position;
            }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    SetDirty();
                }
            }
        }

        public Rotation4 localEulerAngles
        {
            get
            {
                return m_Rotation.ToEuler();
            }
            set
            {
                m_Rotation = Quaternion4.Euler(value);
                SetDirty();
            }
        }

        public Quaternion4 localRotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                if (m_Rotation != value)
                {
                    m_Rotation = value;
                    SetDirty();
                }
            }
        }

        public Vector4 localScale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                if (m_Scale != value)
                {
                    m_Scale = value;
                    SetDirty();
                }
            }
        }

        public Vector4 position
        {
            get
            {
                return localToWorldMatrix.GetColumn(4);
            }
            set
            {
                localPosition = worldMatrixInverse * value;
            }
        }

        public Quaternion4 rotation
        {
            get
            {
                return parent ? parent.rotation * localRotation  : localRotation;
            }
            set
            {
                localRotation = parent ? Quaternion4.Inverse(parent.rotation) * value : value;
            }
        }

        public Vector4 scale
        {
            get
            {
                return parent ? Vec4.Scale(localScale, parent.scale) : localScale;
            }
            set
            {
                localScale = parent ? Vec4.Divide(value, parent.scale) : value;
            }
        }


        /// <summary>
        /// The transformation matrix compared to its parent
        /// </summary>
        public Matrix5x5 localMatrix
        {
            get
            {
                return Matrix5x5.TRS(m_Position, m_Rotation, m_Scale);
            }
        }

        /// <summary>
        /// The parent transformation matrix without affects from this Transform
        /// </summary>
        public Matrix5x5 worldMatrix
        {
            get
            {
                if (parent)
                    return _parent.localToWorldMatrix;
                else
                    return Matrix5x5.identity;
            }
        }

        public Matrix5x5 worldMatrixInverse
        {
            get
            {
                if (parent)
                    return _parent.worldToLocalMatrix;
                else
                    return Matrix5x5.identity;
            }
        }

        void ProcessMatrix()
        {
            if (parent)
            {
                var mtx = _parent.localToWorldMatrix;
                _localToWorld = mtx * localMatrix;
            }
            else
            {
                _localToWorld = localMatrix;
            }
            _worldToLocal = Matrix5x5.Inverse(_localToWorld);
            _matrixDirty = false;
        }

        /// <summary>
        /// The analogous localToWorldMatrix just for 4D manipulations
        /// </summary>
        public Matrix5x5 localToWorldMatrix
        {
            get
            {
                if (_matrixDirty)
                    ProcessMatrix();
                return _localToWorld;
            }
        }

        /// <summary>
        /// The analogous worldToLocalMatrix just for 4D manipulations
        /// </summary>
        public Matrix5x5 worldToLocalMatrix
        {
            get
            {
                if (_matrixDirty)
                    ProcessMatrix();
                return _worldToLocal;
            }
        }

        /// <summary>
        /// Tell if this transform has changed recently.
        /// DO NOT ATTEMPT TO SET THIS VARIABLE MANUALLY.
        /// </summary>
        [NonSerialized] public bool hasChanged;
        [NonSerialized] List<Transform4> _childs = new List<Transform4>();
        [NonSerialized] Transform4 _parent;
        [NonSerialized] IViewer _viewer;
        Matrix5x5 _localToWorld;
        Matrix5x5 _worldToLocal;
        [NonSerialized] bool _matrixDirty = true;


        /// <summary>
        /// Get Transform4 in ancestor. Note that not every objects have Transform4.
        /// </summary>
        public Transform4 parent
        {
            get
            {
                if (!_parent)
                    _parent = M4Utility.GetComponentFromParent<Transform4>(transform);
                return _parent;
            }
        }

        /// <summary>
        /// Get root viewer.
        /// </summary>
        public IViewer viewer
        {
            get
            {
                if (_viewer == null)
                    _viewer = M4Utility.GetComponentFromParent<IViewer>(transform);
                return _viewer;
            }
        }

        /// <summary>
        /// Get Transform4 in childrens. NOTE THIS RETURN NOT JUST ONE DEPTH
        /// </summary>
        List<Transform4> childrens
        {
            get
            {
                if (_childs.Count == 0)
                    GetComponentsInChildren<Transform4>(true, _childs);
                return _childs;
            }
        }

        void OnTransformParentChanged()
        {
            _parent = null;
            SetDirty();
        }

        void OnTransformChildrenChanged()
        {
            _childs.Clear();
        }

        void OnTransform4Dirty()
        {
            hasChanged = true;
            _matrixDirty = true;    
            #if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.EditorUtility.SetDirty(gameObject);
            #endif   
        }

        void OnValidate()
        {
            SetDirty();
        }

        public void SetDirty()
        {
            if (_matrixDirty)
                return;

            for (int i = childrens.Count; i-- > 0;)
            {
                // This component got included too
                if (_childs[i])
                    _childs[i].OnTransform4Dirty();
            }
        }


        #if UNITY_EDITOR

        [ContextMenu("Reset Position")]
        void _ResetPosition ()
        {
            UnityEditor.Undo.RecordObject(this, "Reset Position4");
            localPosition = Vector4.zero;
        }

        [ContextMenu("Reset Rotation")]
        void _ResetRotation ()
        {
            UnityEditor.Undo.RecordObject(this, "Reset Rotation4");
            localRotation = Quaternion4.identity;
        }

        [ContextMenu("Reset Scale")]
        void _ResetScale ()
        {
            UnityEditor.Undo.RecordObject(this, "Reset Scale4");
            localScale = Vector4.one;
        }

        #endif
    }
}