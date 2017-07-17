using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib
{

    [Serializable]
    public sealed class V3Helper
    {
        public List<Vector3> m_Verts = new List<Vector3>();
        public List<Color> m_Colors = new List<Color>();
        public List<Vector4> m_Uv0 = new List<Vector4>();
        public List<Vector4> m_Uv2 = new List<Vector4>();
        public List<Vector4> m_Uv3 = new List<Vector4>();
        public List<List<int>> m_Tris = new List<List<int>>();

        [SerializeField] int m_subMesh = 0;
        // Current submesh idx

        public V3Helper()
        {
        }

        public V3Helper(Mesh m)
            : this(m, -1)
        {
        }

        public V3Helper(Mesh m, int submesh)
        {
            m.GetVertices(m_Verts);
            m.GetColors(m_Colors);
            m.GetUVs(0, m_Uv0);
            m.GetUVs(1, m_Uv2);
            m.GetUVs(2, m_Uv3);

            if (submesh >= 0)
            {
                SetSubmesh(0);
                m.GetTriangles(m_Tris[0], submesh);
            }
            else
            {
                SetSubmesh(m.subMeshCount - 1);
                for (int i = m.subMeshCount; i-- > 0;)
                {
                    m.GetTriangles(m_Tris[i], i);
                }
            }
        }

        public void Clear()
        {
            for (int i = m_Tris.Count; i-- > 0;)
            {
                ListPool<int>.Release(m_Tris[i]);
            }
            m_Verts.Clear();
            m_Tris.Clear();
            m_Colors.Clear();
            m_Uv0.Clear();
            m_Uv2.Clear();
            m_Uv3.Clear();
            m_Tris.Add(ListPool<int>.Get());
            m_subMesh = 0;
        }

        public void Apply(Mesh m, VertexProfiles profile)
        {
            m.Clear();
            m.SetVertices(m_Verts);
            m.subMeshCount = m_Tris.Count;

            for (int i = 0; i < m_Tris.Count; i++)
            {
                m.SetTriangles(m_Tris[i], i);
            }

            if (profile >= VertexProfiles.ColorOnly)
                m.SetColors(m_Colors);

            var maxUV = (int)profile % 4;
            if (maxUV >= 1)
                m.SetUVs(0, m_Uv0);
            if (maxUV >= 2)
                m.SetUVs(1, m_Uv2);
            if (maxUV >= 2)
                m.SetUVs(2, m_Uv3);

            m.RecalculateBounds();
            m.RecalculateNormals();
            m.RecalculateTangents();
        }

        public void AddVert(Vector3 v)
        {
            m_Verts.Add(v);
        }

        public void AddVert(IEnumerable<Vector3> v)
        {
            m_Verts.AddRange(v);
        }

        public void AddTris(int i)
        {
            m_Tris[m_subMesh].Add(i);
        }

        public void AddTris(IEnumerable<int> i)
        {
            m_Tris[m_subMesh].AddRange(i);
        }

        public void AddTris(int i, int subMesh)
        {
            m_Tris[subMesh].Add(i);
        }

        public void SetSubmesh()
        {
            if (m_Tris[m_subMesh].Count > 0)
                SetSubmesh(m_subMesh + 1);
        }

        public void SetSubmesh(int idx)
        {
            while (m_Tris.Count <= idx)
            {
                m_Tris.Add(ListPool<int>.Get());
            }
            m_subMesh = idx;
        }

        public void AddProfile(VertProfile p)
        {
            m_Colors.Add(p.color);
            m_Uv0.Add(p.uv);
            m_Uv2.Add(p.uv2);
            m_Uv3.Add(p.uv3);
        }

        public void AddProfile(IEnumerable<VertProfile> p)
        {
            foreach (var profile in p)
            {
                m_Colors.Add(profile.color);
                m_Uv0.Add(profile.uv);
                m_Uv2.Add(profile.uv2);
                m_Uv3.Add(profile.uv3);
            }
        }

        public void AddMesh(Mesh m, bool mergeSubmesh = true, int subMeshIdx = -1)
        {
            if (m.vertexCount == 0)
                return;
            var v = new V3Helper(m, subMeshIdx);
            Append(v, mergeSubmesh, mergeSubmesh || m_Tris[m_subMesh].Count == 0 ? m_subMesh : m_subMesh + 1);
        }


        public void Append(V3Helper v, bool mergeSubmesh, int startingSubmesh)
        {
            var offset = m_Verts.Count;
            m_Verts.AddRange(v.m_Verts);
            m_Uv0.AddRange(v.m_Uv0); 
            m_Uv2.AddRange(v.m_Uv2);
            m_Uv3.AddRange(v.m_Uv3);

            for (int i = 0; i < v.m_Tris.Count; i++)
            {
                if (i == 0)
                    SetSubmesh(startingSubmesh);
                else if (!mergeSubmesh)
                    SetSubmesh();
                var t = v.m_Tris[i];

                for (int j = 0; j < t.Count; j++)
                {
                    AddTris(offset + t[j]);
                }
            }
        }
    }
}