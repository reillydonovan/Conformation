using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib {

public partial class S4Renderer : MonoBehaviour
{
    [NonSerialized] bool _dirtyBake = true;
    [NonSerialized] bool _dirty = true;
    [NonSerialized] List<S4UploaderBase> _uploaders = new List<S4UploaderBase>();
    [NonSerialized] Transform4 _transform4;
    [NonSerialized] S4Viewer _manager;

    public S4Viewer manager {
        get { return _manager ?? (_manager = GetComponentInParent<S4Viewer>()); }
    }
    
    void OnTransformParentChanged () {
        _manager = null;

    }

    void OnEnable () {
        _manager = null;
        _uploaders.Clear();
        _dirtyBake = true;
        _dirty = true;
    }



    void OnValidate () {
        SetDirty(true);
    }

    /// <summary>
    /// Set this renderer to be validated later. Set rebake if mesh reconstruction is needed
    /// </summary>
    public void SetDirty (bool rebake) {
        _dirtyBake |= rebake;
#if UNITY_EDITOR
        if (!Application.isPlaying && this)
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        _dirty = true;
    }


    public List<S4UploaderBase> uploaders {
        get {
            if (_uploaders.Count == 0)
                GetComponentsInChildren<S4UploaderBase>(false, _uploaders);
            return _uploaders;
        }
    }

    public void ReacquireUploaders ()
    {
        uploaders.Clear();
        SetDirty(true);
    }


    public Transform4 transform4 {
        get {
            if (!_transform4)
                _transform4 = GetComponent<Transform4>();
            return _transform4;
        }
    }

    internal S4Buffer _buffer = new S4Buffer();
    internal Mesh _mesh;
    internal V3Helper _helper = new V3Helper();
    internal Vector4[] _vertex = new Vector4[0];
    internal Bounds4 _bounds = new Bounds4();


    void ValidateBuffer ()
    {
        if (!_mesh)
        {
            var m = _mesh = new Mesh();
            m.name = "S4 Renderer Buffer";
            m.hideFlags = HideFlags.DontSave;
        } else
            _mesh.Clear();
        GetComponent<MeshFilter>().mesh = _mesh;
        _buffer.Clear();
    }


}

}