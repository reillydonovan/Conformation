using UnityEngine;
using System.Collections;

namespace M4DLib {

    public interface IViewer
    {
        Vector4 position { get; set; }

        Quaternion4 rotation { get; set; }

        bool WorldToProjectionPoint (Vector4 v, out Vector3 projected);

        Vector4 ProjectionToWorldPoint (Vector3 v);

        Vector3 WorldToProjectionVector (Vector4 v);

        Vector4 ProjectionToWorldVector (Vector3 v);

        Quaternion WorldToProjectionOrientation (Quaternion4 v);

        Quaternion4 ProjectionToWorldOrientation (Quaternion v);

    }

}