using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;

namespace M4DLib
{
    // Serializable for debugging purpose
    [Serializable]
    public class M4Buffer
    {
        /// <summary>
        /// Automatically set. DO NOT SET MANUALLY
        /// </summary>
        internal bool mergeSubmeshes;

        /// <summary>
        /// Primary mesh buffer
        /// </summary>
        public V3Helper buffer = new V3Helper();

        /// <summary>
        /// Primary vertices replacements
        /// </summary>
        public List<Vector4> vertices = new List<Vector4>();

        /// <summary>
        /// Add mesh to buffer. Use -1 for all submesh.
        /// Do not call this in middle of 'draw'() blocks.
        /// Also fill the V4 replacements in verts (null if do it later manually)
        /// </summary>
        public void AddMesh(Mesh m, int submesh, IEnumerable<Vector4> verts)
        {
            buffer.AddMesh(m, mergeSubmeshes, submesh);
            if (verts != null)
                vertices.AddRange(verts);
        }

        /// <summary>
        /// Add meshes to buffer. Use -1 for all submesh
        /// </summary>
        public void AddMeshes(IEnumerable<Mesh> m, IEnumerable<Vector4> verts)
        {
            foreach (var mm in m)
            {
                buffer.AddMesh(mm, mergeSubmeshes);
            }

            if (verts != null)
                vertices.AddRange(verts);
        }


        //-------------
        bool d_beginned = false;
        bool d_useProfile = false;
        List<Vector4> d_verts = new List<Vector4>();
        List<VertProfile> d_profiles = new List<VertProfile>();
        List<int> d_tris = new List<int>();

        /// <summary>
        /// Call this function before any Draw* or Add* functions.
        /// Returning the index where the drawing starts
        /// </summary>
        public int BeginDraw(bool useProfiles = true)
        {
            if (d_beginned)
                return 0;
            d_beginned = true;
            d_useProfile = useProfiles;
            d_verts.Clear();
            d_profiles.Clear();
            d_tris.Clear();

            return vertices.Count;
        }

        /// <summary>
        /// Mark the end of Draw calls and send the data to mesh buffer
        /// </summary>
        public void EndDraw()
        {
            if (!d_beginned)
                return;
            d_beginned = false;

            if (d_verts.Count > 0)
            {
                var v = ListPool<Vector3>.ConvertAll(d_verts, x => x);
                var offset = buffer.m_Verts.Count;
                buffer.AddVert(v);
                buffer.AddProfile(d_profiles);

                if (!mergeSubmeshes)
                    buffer.SetSubmesh();
                for (int i = 0; i < d_tris.Count; i++)
                {
                    buffer.AddTris(d_tris[i] + offset);
                }

                vertices.AddRange(d_verts);
                ListPool<Vector3>.Release(v);
            }
        }

        /// <summary>
        /// Clone the existing vertex in given index then return new one
        /// </summary>
        public int AddVert(int index)
        {
            d_verts.Add(vertices[index]);
            if (d_useProfile)
                d_profiles.Add(VertProfile.initial);
            return d_verts.Count - 1;
        }

        /// <summary>
        /// Clone the existing vertex in given index and set new profile then return new one
        /// </summary>
        public int AddVert(int index, VertProfile profile)
        {
            d_verts.Add(vertices[index]);
            if (d_useProfile)
                d_profiles.Add(profile);
            return d_verts.Count - 1;
        }

        /// <summary>
        /// Add vertex and return the index
        /// </summary>
        public int AddVert(Vector4 vert)
        {
            d_verts.Add(vert);
            if (d_useProfile)
                d_profiles.Add(VertProfile.initial);
            return d_verts.Count - 1;
        }

        /// <summary>
        /// Add vertex with profile and return the index
        /// </summary>
        public int AddVert(Vector4 vert, VertProfile profile)
        {
            d_verts.Add(vert);
            if (d_useProfile)
                d_profiles.Add(profile);
            return d_verts.Count - 1;
        }

        /// <summary>
        /// Add new tris which connect existing vertexes
        /// </summary>
        public void AddTris(int a, int b, int c)
        {
            d_tris.Add(a);
            d_tris.Add(b);
            d_tris.Add(c);
        }

        /// <summary>
        /// Add new tris with cloning given vertex indexes
        /// </summary>
        public void DrawTris(int a, int b, int c)
        {
            d_tris.Add(AddVert(a));
            d_tris.Add(AddVert(b));
            d_tris.Add(AddVert(c));
        }

        /// <summary>
        /// Add new tris with cloning given vertex indexes
        /// </summary>
        public void DrawTris(int a, int b, int c, VertProfile A, VertProfile B, VertProfile C)
        {
            d_tris.Add(AddVert(a, A));
            d_tris.Add(AddVert(b, B));
            d_tris.Add(AddVert(c, C));
        }


        public void DrawTris(params int[] idx)
        {
            for (int i = 0; i < idx.Length;)
            {
                DrawTris(idx[i++], idx[i++], idx[i++]);
            }
        }

        static Rect _uv01 = new Rect(0, 0, 1, 1);

        public void DrawQuads(int a, int b, int c, int d)
        {
            DrawQuads(a, b, c, d, _uv01);
        }

        public void DrawQuads(int a, int b, int c, int d, Rect uv)
        {
            if (!d_useProfile)
            {
                DrawTris(a, b, c);
                DrawTris(b, d, c);
            }
            else
            {
                DrawTris(a, b, c, new VertProfile(new Vector4(uv.xMin, uv.yMin)), new VertProfile(new Vector4(uv.xMax, uv.yMin)), new VertProfile(new Vector4(uv.xMin, uv.yMax)));
                DrawTris(b, d, c, new VertProfile(new Vector4(uv.xMax, uv.yMin)), new VertProfile(new Vector4(uv.xMax, uv.yMax)), new VertProfile(new Vector4(uv.xMin, uv.yMax)));
            }
        }

        /// <summary>
        /// Add new quad with cloning given vertex indexes and new profiles.
        /// </summary>
        public void DrawQuads(int a, int b, int c, int d, VertProfile A, VertProfile B, VertProfile C, VertProfile D)
        {
            if (!d_useProfile)
            {
                DrawTris(a, b, c);
                DrawTris(b, d, c);
            }
            else
            {
                DrawTris(a, b, c, A, B, C);
                DrawTris(b, d, c, B, D, C);
            }
        }

        public void DrawQuads(params int[] idx)
        {
            for (int i = 0; i < idx.Length;)
            {
                DrawQuads(idx[i++], idx[i++], idx[i++], idx[i++]);
            }
        }

        public void DrawTrimids(int a, int b, int c, int d)
        {
            DrawTris(a, b, c);
            DrawTris(a, b, d);
            DrawTris(b, c, d);
            DrawTris(c, a, d);
        }

        public void DrawTrimids(int a, int b, int c, int d, VertProfile A, VertProfile B, VertProfile C, VertProfile D)
        {
            if (!d_useProfile)
            {
                DrawTrimids(a, b, c, d);
            }
            else
            {
                DrawTris(a, b, c, A, B, C);
                DrawTris(a, b, d, A, B, D);
                DrawTris(b, c, d, B, C, D);
                DrawTris(c, a, d, C, A, D);
            }
        }

        public void DrawCubes(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            DrawQuads(a, b, c, d);
            DrawQuads(e, f, g, h);

            DrawQuads(a, b, e, f);
            DrawQuads(b, c, f, g);
            DrawQuads(c, d, g, h);
            DrawQuads(d, a, h, e);
        }

        public void DrawCubes(int a, int b, int c, int d, int e, int f, int g, int h, 
                              VertProfile A, VertProfile B, VertProfile C, VertProfile D, VertProfile E, VertProfile F, VertProfile G, VertProfile H)
        {
            if (!d_useProfile)
            {
                DrawCubes(a, b, c, d, e, f, g, h);
            }
            else
            {
                DrawQuads(a, b, c, d, A, B, C, D);
                DrawQuads(e, f, g, h, E, F, G, H);

                DrawQuads(a, b, e, f, A, B, E, F);
                DrawQuads(b, c, f, g, B, C, F, G);
                DrawQuads(c, d, g, h, C, D, G, H);
                DrawQuads(d, a, h, e, D, A, H, E);
            }
        }
    }
}