using UnityEngine;
using System.Collections.Generic;

namespace M4DLib {

[AddComponentMenu("Scripts/Mesh4D/Uploader/MU4 Extruded Uploader")]
public class MU4Extruded : M4UploaderBase
{
    public Mesh baseMesh;
    public float extrudeDistance = 1;
    public float extrudeOffset;
    public Axis4 extrudeAxis = Axis4.W;
    public bool displayExtrusion = true;
    public bool quadrifyExtrusion = true;

    List<Vector3> _verts3 = new List<Vector3>();
    List<Vector2> _uv0 = new List<Vector2>();
    List<int> _tris = new List<int>();

   public override void UploadBuffer(M4Buffer buffer)
    {
        if (!baseMesh)
            return;

        buffer.AddMesh (baseMesh, -1, null);
        buffer.AddMesh (baseMesh, -1, null);

        _verts3.Clear();
        _uv0.Clear();
        baseMesh.GetVertices(_verts3);
        var halfCount = _verts3.Count;
        _verts3.AddRange(_verts3); // Can't believe this works!

        var v = buffer.vertices;
        for (int i = 0; i < _verts3.Count; i++)
        {
            bool phase2 = i >= halfCount;
            v.Add(Modify(_verts3[i], phase2));
        }

        if (!displayExtrusion)
            return;

        // Now we draw the extrusion faces

        baseMesh.GetUVs(0, _uv0);

        for (int s = 0; s < baseMesh.subMeshCount; s++)
        {
            buffer.BeginDraw(); 

            baseMesh.GetTriangles(_tris, s);

            var normals = quadrifyExtrusion ? new Vector3[_tris.Count / 3] : null;

            if (quadrifyExtrusion)
            {
                // Get and cache all triangle normals
                for (int i = 0; i < normals.Length; i++)
                {
                    normals[i] = M4Utility.GetNormal(_verts3, _tris, i * 3);
                }
            }

            // On every triangle..
            for (int i = 0; i < _tris.Count - 2; i+=3)
            {
                // On every edge...
                for (int j = 0; j < 3; j++) {
                    var x = _tris[i + j];
                    var y = _tris[i + (j + 1) % 3];
                    // Check if this edge is quad;
                    // https://gamedev.stackexchange.com/questions/74807/convert-triangles-to-quads-of-a-mesh
                    if (quadrifyExtrusion)
                    {
                        var flag = false;
                        var normal = normals[i / 3];
                        for (int k = 0; k < _tris.Count - 2; k += 3) {
                            if (i == k)
                                continue;
                            for (int l = 0; l < 3; l++) {
                                if (y == _tris[k + l] && x == _tris[k + (l + 1) % 3]) {
                                    if (normals[k / 3] == normal)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                            if (flag)
                                break;
                        }
                        // If yes, skip ahead
                        if (flag)
                            continue;
                    }

                    buffer.DrawQuads(x, y, x + halfCount, y + halfCount /*, ProcessUV(x, y)*/);
                }
            }

            buffer.EndDraw();
        }

    }

    Rect ProcessUV (int x, int y)
    {
        // Right now this thing doing nothing other than copy....
        var uvX = _uv0[x];
        var uvY = _uv0[y];
       // if (Mathf.Approximately(uvX.x, uvY.x)) 
            return Rect.MinMaxRect(uvX.x, uvX.y, uvY.x, uvY.y);
        //else
          //  return Rect.MinMaxRect(uvX.x, uvX.y, uvY.x, uvY.y);
    }

    public Vector4 Modify(Vector3 v, bool phase2)
    {
        var val = (phase2 ? 1 : -1) * extrudeDistance + extrudeOffset;
        switch (extrudeAxis)
        {
            case Axis4.X:
                return new Vector4(val, v.y, v.z, v.x);
            case Axis4.Y:
                return new Vector4(v.x, val, v.z, v.y);
            case Axis4.Z:
                return new Vector4(v.x, v.y, val, v.z);
            case Axis4.W: default:
                return new Vector4(v.x, v.y, v.z, val);
        }
    }
}
}