using UnityEngine;
using System.Collections.Generic;

namespace M4DLib {
public static class MeshPool
{

    static Stack<Mesh> _pooledObject = new Stack<Mesh>();
    static int iter = 0;
    static public Mesh Get ()
    {   
        if (_pooledObject.Count == 0)
        {
            var m = new Mesh();
            m.name = "TemporaryMesh " + (iter++).ToString();
            m.hideFlags = HideFlags.DontSave;
            return m;
        } else {
            return _pooledObject.Pop();
        }

    }

    static public void Release (Mesh m)
    {
        m.Clear();
        _pooledObject.Push(m);
    }
}

}