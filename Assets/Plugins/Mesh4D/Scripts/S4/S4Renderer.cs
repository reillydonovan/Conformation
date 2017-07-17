using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace M4DLib
{


    [AddComponentMenu("Scripts/Mesh4D/S4 Renderer")]
    [RequireComponent(typeof(Transform4), typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
    public partial class S4Renderer : MonoBehaviour
    {
        [Tooltip("Additional vertex properties to be included in this renderer")]
        public VertexProfiles vertexProfiles = VertexProfiles.None;

        // Opted out. Experimental and useless
        // public bool autogenerateUV = false;

        void LateUpdate()
        {
            if (_dirtyBake)
            {
                Bake();
                SynchronizeTransform();
                CalculateMesh();
            }
            else if (transform4.hasChanged)
            {
                SynchronizeTransform();
                CalculateMesh();
            }
            else if (_dirty)
            {
                CalculateMesh();
            }
        }

        /// <summary>
        /// Bake this renderer now.
        /// </summary>
        void Bake()
        {
             ValidateBuffer();
            for (int i = 0; i < uploaders.Count; i++)
            {
                if (_uploaders[i].enabled)
                    _uploaders[i].UploadBuffer(_buffer);
            }
            _dirtyBake = false;

        }

        /// <summary>
        /// Sync verts with Transform4
        /// </summary>
        void SynchronizeTransform()
        {   
            var mtx = transform4.localToWorldMatrix;
            var verts = _buffer.vertices;
            var count = verts.Count;
            var bounds = Bounds4.Infinite;
            if (_vertex.Length != count)
                _vertex = new Vector4[count]; // Mostly impossible unless something changed in baking buffers
            for (int i = 0; i < count; i++)
            {
                bounds.Allocate(_vertex[i] = (mtx * verts[i]));
            }
            transform4.hasChanged = false;
            _bounds = bounds;
        }

        /// <summary>
        /// Calculate and upload the final projection
        /// </summary>
        void CalculateMesh()
        {
            if (!manager)
                return;

            manager.ValidateMatrix();

            if (S4Slicer.IsOptedOut(_bounds))
            {
                if (_mesh.vertexCount > 0)
                    _mesh.Clear();
                return;
            }

            _helper.Clear();

            var t4 = _buffer.trimids;
            var v4 = _vertex;
            var b4 = _buffer.profiles;
            S4Slicer._helper = _helper;
            var tv = S4Slicer._preBuffer;
            var tp = S4Slicer._preProfile;
            var useBuffer = S4Slicer._computeProfiles = 
                (vertexProfiles != VertexProfiles.None) && (b4.Count == v4.Length);
            var count = t4.Count - 3;

            for (int i = 0; i < count;)
            {
                int a = t4[i++], b = t4[i++], c = t4[i++], d = t4[i++];
                {
                    tv[0] = v4[a];
                    tv[1] = v4[b];
                    tv[2] = v4[c];
                    tv[3] = v4[d];
                }
                if (useBuffer)
                {
                    tp[0] = b4[a];
                    tp[1] = b4[b];
                    tp[2] = b4[c];
                    tp[3] = b4[d];
                }

                if(S4Slicer.Intersect())
                    S4Slicer.SaveVertexes();
            }
            //if (autogenerateUV)
             // S4UVGenerator.Generate(_helper);

            _helper.Apply(_mesh, vertexProfiles);
            _dirty = false;
        }

    }
}