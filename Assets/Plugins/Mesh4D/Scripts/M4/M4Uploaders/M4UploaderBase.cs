using UnityEngine;
using System.Collections;

namespace M4DLib {
[RequireComponent(typeof(M4Renderer)), ExecuteInEditMode]
public abstract class M4UploaderBase : MonoBehaviour
{
    public abstract void UploadBuffer (M4Buffer buffer);

    protected virtual void OnEnable ()
    {
        var rend = GetComponent<M4Renderer>();
        rend.ReacquireUploaders();
    }

    protected virtual void OnDisable ()
    {
        var rend = GetComponent<M4Renderer>();
        if (rend)
            rend.ReacquireUploaders();
    }

    protected virtual void OnValidate ()
    {
        var rend = GetComponent<M4Renderer>();
        rend.SetDirty(true);
    }


}

}