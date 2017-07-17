using UnityEngine;
using System.Collections;

namespace M4DLib {

[RequireComponent(typeof(S4Renderer)), ExecuteInEditMode]
public abstract class S4UploaderBase : MonoBehaviour
{
    public abstract void UploadBuffer (S4Buffer buffer);

    protected virtual void OnEnable ()
    {
        var rend = GetComponent<S4Renderer>();
        rend.ReacquireUploaders();
    }

    protected virtual void OnDisable ()
    {
        var rend = GetComponent<S4Renderer>();
        if (rend)
            rend.ReacquireUploaders();
    }

    protected virtual void OnValidate ()
    {
        var rend = GetComponent<S4Renderer>();
        rend.SetDirty(true);
    }
}

}