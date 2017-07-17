using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using M4DLib;
using System;

namespace M4DLib.Legacy {

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class Mesh4DEngine : MonoBehaviour
{
    public enum WFaceOption
    {
        None = 0,
        Generated = 1,
        LegacyImport = 2,
    }

    #region Properties

    [SerializeField] Mesh m_baseMesh;
    [SerializeField] Vector3 m_wOffset = Vector3.zero;
    [SerializeField] Vector3 m_wRotation = Vector3.zero;
    [SerializeField] float m_wScale = 1;
    [Range(0, 1), SerializeField] float m_wPivot = 0.5f;
    [SerializeField] WFaceOption m_useWFaces = WFaceOption.Generated;
	
    [SerializeField] Mesh m_wFaces;
    [SerializeField] float m_smallScale;
    [SerializeField] float m_bigScale;
    [SerializeField] bool m_separateSubMeshes = false;
    [NonSerialized] bool m_dirty = true;
    [NonSerialized] bool m_dirtyBake = true;

    public Mesh baseMesh
    {
        get { return m_baseMesh; }
        set
        {
            if (m_dirty |= m_dirtyBake |= value != m_baseMesh)
                m_baseMesh = value;
        }
    }

    public Vector3 wOffset
    {
        get { return m_wOffset; }
        set
        {
            if (m_dirty |= value != m_wOffset)
                m_wOffset = value;
        }
    }

    public Vector3 wRotation
    {
        get { return m_wRotation; }
        set
        {
            if (m_dirty |= value != m_wRotation)
                m_wRotation = value;
        }
    }


    public float wScale
    {
        get { return m_wScale; }
        set
        {
            if (m_dirty |= value != m_wScale)
                m_wScale = value;
        }
    }

    public float wPivot
    {
        get { return m_wPivot; }
        set
        {
            if (m_dirty |= value != m_wPivot)
                m_wPivot = value;
        }
    }

    public WFaceOption useWFaces
    {
        get { return m_useWFaces; }
        set
        {
            if (m_dirty |= value != m_useWFaces)
                m_useWFaces = value;
        }
    }


    public Mesh wFaces
    {
        get { return m_wFaces; }
        set
        {
            if (m_dirty |= m_dirtyBake |= value != m_wFaces)
                m_wFaces = value;
        }
    }


    public float smallScale
    {
        get { return m_smallScale; }
        set
        {
            if (m_dirty |= m_dirtyBake |= value != m_smallScale)
                m_smallScale = value;
        }
    }


    public float bigScale
    {
        get { return m_bigScale; }
        set
        {
            if (m_dirty |= m_dirtyBake |= value != m_bigScale)
                m_bigScale = value;
        }
    }

    public 	Vector3[] oriVert;
    //Default Vertices
    public float wBakeStat = -1;
    bool hasBakeWMesh;
	
    Mesh mesh;

    #endregion

    void Update()
    {
        if (m_baseMesh)
        {
            if (mesh == null || m_dirtyBake)
                BakeMesh();
            if (m_dirty)
                CalculateMesh();
        }
	
    }

    void RecheckObject()
    {
        if (mesh == null)
        {
            MeshFilter f = GetComponent<MeshFilter>();
            mesh = new Mesh();
            mesh.hideFlags = HideFlags.DontSave;
            mesh.name = "Mesh4D_" + transform.name;
            f.mesh = mesh;
        }
    }

    void Reset()
    {
        OnEnable();
    }

    void OnEnable()
    {
        if (mesh == null)
        {
            m_dirtyBake = true;
            m_dirty = true;
        }
    }

    public void SetDirty(bool bakeDirty = false)
    {
        m_dirtyBake |= bakeDirty;
        m_dirty = true;
    }

    public void BakeMesh()
    {
        if (mesh == null)
            RecheckObject();
        oriVert = new Vector3[]{ };
        mesh.Clear();
        CombineInstance[] combines = new CombineInstance[2];
        combines[0].mesh = m_baseMesh;
        combines[1].mesh = m_baseMesh;
		
        mesh.CombineMeshes(combines, !m_separateSubMeshes, false);
        oriVert = (m_baseMesh.vertices);
        wBakeStat = 0;

        m_dirtyBake = false;
        if (m_useWFaces == WFaceOption.LegacyImport)
            BakeWMesh();
        if (m_useWFaces == WFaceOption.Generated)
            BakeWMeshGenerated();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void BakeWMesh()
    {
        if (wBakeStat <= 0.75f)
        {
            var Wvert = ListPool<Vector3>.Get();
            var Wbase = ListPool<Vector3>.Get();
            var Wtris = ListPool<int>.Get();
            wFaces.GetVertices(Wvert);
            m_baseMesh.GetVertices(Wbase);
            wFaces.GetTriangles(Wtris, 0);

            var WbaseSmall = ListPool<Vector3>.ConvertAll(Wbase, o => o * smallScale);
            var WbaseBig = ListPool<Vector3>.ConvertAll(Wbase, o => o * bigScale);
            float failDesity = 0;

            for (int i = 0; i < Wtris.Count; i++)
            {
                var idx = WbaseSmall.IndexOf(Wvert[Wtris[i]]);
                if (idx < 0)
                if ((idx = WbaseBig.IndexOf(Wvert[Wtris[i]])) >= 0)
                    idx += m_baseMesh.vertexCount;
                if (idx < 0)
                {
                    int klopI = (i / 3) * 3;
                    failDesity += 1f / Wtris.Count;
                    Wtris[klopI] = 0;
                    Wtris[klopI + 1] = 0;
                    Wtris[klopI + 2] = 0;
                }
                else
                    Wtris[i] = idx;
            }
            if (failDesity > 0.999f)
                failDesity = 1;
            else if (failDesity < 0.001f)
                failDesity = 0;
			
            wBakeStat = 1f - failDesity;
            if (failDesity < 0.25 && !m_dirtyBake)
                mesh.triangles = mesh.triangles.Concat(Wtris).ToArray();		
        }
    }

    public void BakeWMeshGenerated()
    {
        var v = ListPool<Vector3>.Get();
        var t = ListPool<int>.Get();
        var t2 = ListPool<int>.Get();
        var n = ListPool<Vector3>.Get();
        baseMesh.GetVertices(v);
        baseMesh.GetTriangles(t, 0);

        // Get and cache all triangle normals
        for (int i = 0; i < t.Count; i += 3)
        {
            n.Add(M4Utility.GetNormal(v, t, i));
        }
   
        // On every triangle..
        for (int i = 0; i < t.Count - 2; i += 3)
        {
            // On every edge...
            for (int j = 0; j < 3; j++)
            {
                var x = t[i + j];
                var y = t[i + (j + 1) % 3];

                // Check if this edge is quad;
                {
                    var flag = false;
                    var normal = n[i / 3];
                    for (int k = 0; k < t.Count - 2; k += 3)
                    {
                        if (i == k)
                            continue;
                        for (int l = 0; l < 3; l++)
                        {
                            if (y == t[k + l] && x == t[k + (l + 1) % 3])
                            {
                                if (n[k / 3] == normal)
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

                t2.Add(x);
                t2.Add(y);
                t2.Add(x + v.Count);
                t2.Add(y);
                t2.Add(y + v.Count);
                t2.Add(x + v.Count);
            }
        }
        t2.InsertRange(0, mesh.triangles);
        mesh.triangles = t2.ToArray();        
        ListPool<int>.Release(t);
        ListPool<int>.Release(t2);
        ListPool<Vector3>.Release(v);
        ListPool<Vector3>.Release(n);
        wBakeStat = 1;
            
    }

    public void CalculateMesh()
    {
		
        Vector3 DiffA = Vector3.Lerp(Vector3.zero, m_wOffset, m_wPivot);
        Vector3 DiffB = Vector3.Lerp(m_wOffset, Vector3.zero, m_wPivot);
        float FacA = MeshUtilities.vNoZero(Mathf.Lerp(1, m_wScale, m_wPivot));
        float FacB = MeshUtilities.vNoZero(Mathf.Lerp(m_wScale, 1, m_wPivot));
		
        mesh.vertices =	
			Array.ConvertAll(oriVert, x => MeshUtilities.vRot(x * FacA + DiffA, m_wRotation, x)).Concat(
            Array.ConvertAll(oriVert, x => MeshUtilities.vRot(x / FacB - DiffB, m_wRotation, x))).ToArray();
		
        mesh.RecalculateBounds();
        m_dirty = false;
    }
}

}