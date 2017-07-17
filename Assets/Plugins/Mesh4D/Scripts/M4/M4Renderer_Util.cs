using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib {

public partial class M4Renderer : MonoBehaviour
{
    /// Null means is in progress
    [NonSerialized] bool? m_dirtyBake = true;
    [NonSerialized] bool m_dirty = true;
    [NonSerialized] List<M4UploaderBase> _uploaders = new List<M4UploaderBase>();
    [NonSerialized] Transform4 _transform4;
    [NonSerialized] M4Viewer m_manager;

    public M4Viewer manager {
        get { return m_manager ?? (m_manager = GetComponentInParent<M4Viewer>()); }
    }
    
    void OnTransformParentChanged () {
       m_manager = null;
    }

    void OnEnable () {
        m_manager = null;
        _uploaders.Clear();
        m_dirtyBake = true;
        m_dirty = true;
    }


    void OnValidate () {
        SetDirty(true);
    }

    /// <summary>
    /// Set this renderer to be validated later. Set rebake if mesh reconstruction is needed
    /// </summary>
    public void SetDirty (bool rebake) {
        m_dirtyBake |= rebake;
#if UNITY_EDITOR
        if (!Application.isPlaying && this)
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        m_dirty = true;
    }

    public void ReacquireUploaders ()
    {
        uploaders.Clear();
        SetDirty(true);
    }

    public List<M4UploaderBase> uploaders {
        get {
            if (_uploaders.Count == 0)
                GetComponents<M4UploaderBase>(_uploaders);
            return _uploaders;
        }
    }


    public Transform4 transform4 {
        get {
            if (!_transform4)
                _transform4 = GetComponent<Transform4>();
            return _transform4;
        }
    }

    M4Buffer _buffer = new M4Buffer();
    Mesh _mesh;
    Vector3[] _verts = new Vector3[0];
    Vector4[] _verts4 = new Vector4[0];


    void ValidateBuffer ()
    {
        if (!_mesh)
        {
            var m = _mesh = new Mesh();
            m.name = "M4 Renderer Buffer";
            m.hideFlags = HideFlags.DontSave;
        } else
            _mesh.Clear();
        GetComponent<MeshFilter>().mesh = _mesh;
        _buffer.buffer.Clear();
        _buffer.vertices.Clear();
        _buffer.mergeSubmeshes = combineSubmeshes;
    }

}

}