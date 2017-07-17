using UnityEngine;
using System.Collections;
using System;

namespace M4DLib
{

    /// <summary>
    /// Representation for bounds in 4D
    /// </summary>
    [Serializable]
    public struct Bounds4
    {
        public Vector4 min;
        public Vector4 max;

        public Vector4 center {
            get {
                return new Vector4 {
                    x = (max.x + min.x) * 0.5f,
                    y = (max.y + min.y) * 0.5f,
                    z = (max.z + min.z) * 0.5f,
                    w = (max.w + min.w) * 0.5f,
                };
            }
        }
        
        public Vector4 extent {
            get {
                return new Vector4 {
                    x = (max.x - min.x) * 0.5f,
                    y = (max.y - min.y) * 0.5f,
                    z = (max.z - min.z) * 0.5f,
                    w = (max.w - min.w) * 0.5f,
                };
            }
        }
        
        public Vector4 size {
            get {
                return new Vector4 {
                    x = max.x - min.x,
                    y = max.y - min.y,
                    z = max.z - min.z,
                    w = max.w - min.w,
                };
            }
        }

        public static Bounds4 Infinite {
            get {
                return new Bounds4 {
                  min = new Vector4 { x = float.PositiveInfinity, y = float.PositiveInfinity, z = float.PositiveInfinity, w = float.PositiveInfinity },
                  max = new Vector4 { x = float.NegativeInfinity, y = float.NegativeInfinity, z = float.NegativeInfinity, w = float.NegativeInfinity },
                };
            }
        }
        
        public static Bounds4 Zero {
            get {
                return new Bounds4();
            }
        }

        // Extend this bounds to given point if needed
        // Yes. Micro-optimization
        public void Allocate (Vector4 pos) {
            float x = pos.x, y = pos.y, z = pos.z, w = pos.w;

            min = new Vector4 { 
                x = min.x < x ? min.x : x,
                y = min.y < y ? min.y : y,
                z = min.z < z ? min.z : z,
                w = min.w < w ? min.w : w};
            max = new Vector4 { 
                x = max.x > x ? max.x : x,
                y = max.y > y ? max.y : y,
                z = max.z > z ? max.z : z,
                w = max.w > w ? max.w : w};
        }
        
        // Extend this bounds to given another bound if needed
        public void Allocate (Bounds4 pos) {
            Allocate(pos.min);
            Allocate(pos.max);
        }
        
        public Bounds4(Vector4 Min, Vector4 Max) {
            min = Min;
            max = Max;
        }

        public static Vector4 Lerp (Bounds4 bound, Vector4 t)
        {
            return bound.min + Vector4.Scale(bound.max - bound.min, t);
        }
    }


}