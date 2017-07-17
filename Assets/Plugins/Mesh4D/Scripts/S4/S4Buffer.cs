using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib
{

    [Serializable]
    public class S4Buffer
    {
  
        public List<int> trimids = new List<int>();
        public List<Vector4> vertices = new List<Vector4>();
        public List<VertProfile> profiles = new List<VertProfile>();

        /// <summary>
        /// Clear existing buffer. CALLED IN INTERNAL ONLY
        /// </summary>
        public void Clear()
        {
            trimids.Clear();
            vertices.Clear();
            profiles.Clear();
        }

        /// <summary>
        /// Expand (predict the capacity) of vert and tris
        /// Just a micro-optimization (doesn't matter anyway)
        /// </summary>
        public void Expand(int predictedVert, int predictedTris)
        {
            vertices.Expand(predictedVert);
            trimids.Expand(predictedTris);
        }

        /// <summary>
        /// Add a trimid with given vert indexs.
        /// </summary>
        public void AddTrimid(int v0, int v1, int v2, int v3)
        {
            trimids.Add(v0);
            trimids.Add(v1);
            trimids.Add(v2);
            trimids.Add(v3);
        }

        /// <summary>
        /// Add a trimid with new vert positions;
        /// </summary>
        public void AddTrimid(Vector4 a, Vector4 b, Vector4 c, Vector4 d)
        {
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);

            trimids.Add(vertices.Count - 4);
            trimids.Add(vertices.Count - 3);
            trimids.Add(vertices.Count - 2);
            trimids.Add(vertices.Count - 1);
        }

        /// <summary>
        /// Helper to add a cube from 8 existing verts index.
        /// A cube is constructed from 5 trimids.
        /// </summary>
        public void AddCube(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7)
        {
            // The cube is splitted to 5 trimids.
            AddTrimid(v0, v1, v3, v5);
            AddTrimid(v0, v2, v3, v6);
            AddTrimid(v0, v4, v5, v6);
            AddTrimid(v3, v5, v6, v7);
            AddTrimid(v0, v3, v5, v6);
        }

        public int AddVert(Vector4 vert)
        {
            return AddVert(vert, VertProfile.initial);
        }

        public int AddVert(Vector4 vert, Vector4 uv)
        {
            return AddVert(vert, new VertProfile(uv));
        }

        public int AddVert(Vector4 vert, Color color, Vector4 uv)
        {
            return AddVert(vert, new VertProfile(color, uv));
        }

        public int AddVert(Vector4 vert, Color color, Vector4 uv, Vector4 uv2, Vector4 uv3)
        {
            return AddVert(vert, new VertProfile(color, uv, uv2, uv3));
        }

        /// <summary>
        /// Add a vertex and return its index
        /// </summary>
        public int AddVert(Vector4 vert, VertProfile profile)
        {
            vertices.Add(vert);
            profiles.Add(profile);
            return vertices.Count - 1;
        }


    }

}