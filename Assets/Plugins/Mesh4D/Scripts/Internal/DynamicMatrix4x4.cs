using System.Collections.Generic;
using UnityEngine;
using System;

namespace M4DLib
{
    [Serializable]
public class DynamicMatrix4x4 : ICloneable
{
        [SerializeField, HideInInspector] int _dummy;

    [Serializable]
    public struct Operation
    {
        /// 0: Position, 1: Rotation, 2: Scale
        public int mode;
        public Vector3 value;

        public Operation(int Mode, Vector3 Value)
        {
            mode = Mode;
            value = Value;
        }
    }


    public List<Operation> operations = new List<Operation>();

    public void Clear()
    {
        operations.Clear();
    }
    
    public void CopyFrom (DynamicMatrix4x4 other) {
        if (this == other)
            return;
        operations.Clear();
        operations.AddRange(other.operations);
    }

    public void Add(int mode, Vector3 val)
    {
        operations.Add(new Operation(mode, val));
    }

    public bool IsOperationEmpty()
    {
        return operations.Count == 0;
    }

    public Matrix4x4 Build()
    {
        var m = Matrix4x4.identity;
        for (int i = 0; i < operations.Count; i++)
        {
            var op = operations[i].value;

            switch (operations[i].mode)
            {
                case 0:
                    AppendTranslation(ref m, op);
                    break;
                case 1:
                    AppendRotation(ref m, op);
                    break;
                case 2:
                    AppendScale(ref m, op);
                    break;
            }
        }
        return m;
    }

    static public void AppendTranslation(ref Matrix4x4 m, Vector3 pos)
    {
        float x = pos.x, y = pos.y, z = pos.z;
        m.m03 = m.m00 * x + m.m01 * y + m.m02 * z + m.m03;
        m.m13 = m.m10 * x + m.m11 * y + m.m12 * z + m.m13;
        m.m23 = m.m20 * x + m.m21 * y + m.m22 * z + m.m23;
        m.m33 = m.m30 * x + m.m31 * y + m.m32 * z + m.m33;
    }

    static public void AppendRotation(ref Matrix4x4 m, Vector3 rot)
    {
        var Q = Quaternion.Euler(rot);
        AppendRotation(ref m, Q);
    }

    static public void AppendRotation(ref Matrix4x4 m, Quaternion rot)
    {
        m = m * Matrix4x4.TRS (Vector3.zero, rot, Vector3.one);

        /*
        float x2 = rot.x + rot.x;
        float y2 = rot.y + rot.y;
        float z2 = rot.z + rot.z;

        float wx2 = rot.w * x2;
        float wy2 = rot.w * y2;
        float wz2 = rot.w * z2;
        float xx2 = rot.x * x2;
        float xy2 = rot.x * y2;
        float xz2 = rot.x * z2;
        float yy2 = rot.y * y2;
        float yz2 = rot.y * z2;
        float zz2 = rot.y * z2;

        float q11 = 1.0f - yy2 - zz2;
        float q21 = xy2 - wz2;
        float q31 = xz2 + wy2;

        float q12 = xy2 + wz2;
        float q22 = 1.0f - xx2 - zz2;
        float q32 = yz2 - wx2;

        float q13 = xz2 - wy2;
        float q23 = yz2 + wx2;
        float q33 = 1.0f - xx2 - yy2;

        m = new Matrix4x4() {

        // First row
        m00 = m.m00 * q11 + m.m01 * q21 + m.m02 * q31,
        m01 = m.m00 * q12 + m.m01 * q22 + m.m02 * q32,
        m02 = m.m00 * q13 + m.m01 * q23 + m.m02 * q33,
        m03 = m.m03,

        // Second row
        m10 = m.m10 * q11 + m.m11 * q21 + m.m12 * q31,
        m11 = m.m10 * q12 + m.m11 * q22 + m.m12 * q32,
        m12 = m.m10 * q13 + m.m11 * q23 + m.m12 * q33,
        m13 = m.m13,

        // Third row
        m20 = m.m20 * q11 + m.m21 * q21 + m.m22 * q31,
        m21 = m.m20 * q12 + m.m21 * q22 + m.m22 * q32,
        m22 = m.m20 * q13 + m.m21 * q23 + m.m22 * q33,
        m23 = m.m23,

        // Fourth row
        m30 = m.m30 * q11 + m.m31 * q21 + m.m32 * q31,
        m31 = m.m30 * q12 + m.m31 * q22 + m.m32 * q32,
        m32 = m.m30 * q13 + m.m31 * q23 + m.m32 * q33,
        m33 = m.m33,

        };
        */
    }

    static public void AppendScale(ref Matrix4x4 m, Vector3 scl)
    {
        // TR;LD: m * TRS(0, 0, rot)
        float x = scl.x, y = scl.y, z = scl.z;
        m.m00 *= x;
        m.m01 *= y;
        m.m02 *= z;
        m.m10 *= x;
        m.m11 *= y;
        m.m12 *= z;
        m.m20 *= x;
        m.m21 *= y;
        m.m22 *= z;
        m.m30 *= x;
        m.m31 *= y;
        m.m32 *= z;
    }

    public DynamicMatrix4x4 ()
    {
    }

    public DynamicMatrix4x4 (IEnumerable<Operation> list)
    {
        operations.AddRange(list);
    }

    public object Clone ()
    {
        return new DynamicMatrix4x4(operations);
    }
}    
}
