using UnityEngine;
using System.Collections.Generic;

namespace M4DLib {

[AddComponentMenu("Scripts/Mesh4D/Uploader/MU4 Constant Uploader")]
public class MU4Constant : M4UploaderBase
{

    public Mesh baseMesh;
    public DynamicMatrix4x4 transformation;
    public float constantPosition;
    public Axis4 constantAxis = Axis4.W;
   
    List<Vector3> _verts3 = new List<Vector3>();
    List<Vector4> _verts = new List<Vector4>();

    public override void UploadBuffer(M4Buffer buffer)
    {
        if (!baseMesh)
            return;
         {        
            _verts3.Clear();
            _verts.Clear();
            baseMesh.GetVertices(_verts3);
            _verts.Expand(_verts3);
            var mtx = transformation.Build();

            for (int i = 0; i < _verts3.Count; i++)
            {
                _verts.Add(Modify(mtx.MultiplyPoint3x4(_verts3[i])));
            }
        }

        buffer.AddMesh(baseMesh, -1, _verts);
    }

    public Vector4 Modify(Vector3 v)
    {
        switch (constantAxis)
        {
            case Axis4.X:
                return new Vector4(constantPosition, v.y, v.z, v.x);
            case Axis4.Y:
                return new Vector4(v.x, constantPosition, v.z, v.y);
            case Axis4.Z:
                return new Vector4(v.x, v.y, constantPosition, v.z);
            case Axis4.W: default:
                return new Vector4(v.x, v.y, v.z, constantPosition);
        }
    }
}

}