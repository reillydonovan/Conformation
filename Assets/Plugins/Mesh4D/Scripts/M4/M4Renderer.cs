using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace M4DLib
{

    [AddComponentMenu("Scripts/Mesh4D/M4 Renderer")]
    [RequireComponent(typeof(Transform4), typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
    public partial class M4Renderer : MonoBehaviour
    {
        [Tooltip("Should all submeshes combined to one?")]
        public bool combineSubmeshes = true;
        public VertexProfiles vertexProfile = VertexProfiles.UV1;

        void LateUpdate()
        {
            if (m_dirtyBake == true)
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
            else if (m_dirty)
            {
                CalculateMesh();
            }
        }

        /// <summary>
        /// Bake this renderer now. 
        /// </summary>
        void Bake()
        {
            if (m_dirtyBake != true)
                return;
            m_dirtyBake = null;
            ValidateBuffer();
            for (int i = 0; i < uploaders.Count; i++)
            {
                if (_uploaders[i].enabled)
                    _uploaders[i].UploadBuffer(_buffer);

                #if UNITY_EDITOR
                if (!Application.isPlaying && _buffer.vertices.Count != _buffer.buffer.m_Verts.Count)
                {
                    Debug.LogErrorFormat(this.gameObject, "{0} is not supplying the right vertex amount! (Buffers: {1} Expected: {2})", _uploaders[i], _buffer.vertices.Count, _buffer.buffer.m_Verts.Count);
                    break;
                }
                #endif
            }
            _buffer.buffer.Apply(_mesh, vertexProfile);
            m_dirtyBake = false;
        }

        /// <summary>
        /// Sync verts with Transform4 
        /// </summary>
        void SynchronizeTransform()
        {   
            var mtx = transform4.localToWorldMatrix;
            var verts = _buffer.vertices;
            var count = verts.Count;
            if (count != _verts4.Length)
            {
                _verts4 = new Vector4[count];
                _verts = new Vector3[count];
            }
            for (int i = 0; i < verts.Count; i++)
            {
                _verts4[i] = mtx * verts[i];
            }
            transform4.hasChanged = false;
        }

    
        /// <summary>
        /// Calculate and upload the final projection 
        /// </summary>
        void CalculateMesh()
        {
            if (!manager)
                return;
                
            manager.ValidateMatrix();
           
            // Load parameters to the heap (faster)
            var v3 = _verts;
            var v4 = _verts4;
            var count = v4.Length;
            var m = _mesh;

            for (int i = 0; i < count; i++)
            {
                var vert = M4Projector.Project(v4[i]);

                /*
                #if UNITY_EDITOR
                if (vert.x != vert.x) // Check if NaN
                    vert = new Vector3();
                #endif
                */

                v3[i] = vert;
            }
        
            m.vertices = v3;
            m.RecalculateBounds();

            //if (calculateLighting)
            {
                m.RecalculateNormals();
                m.RecalculateTangents();
            }
            m_dirty = false;
        }
    }

}