using System.Collections.Generic;
using UnityEngine;
using System;

namespace M4DLib
{
    [Serializable]
public class DynamicMatrix5x5 : ICloneable
{
    [SerializeField, HideInInspector] int _dummy;

    [Serializable]
    public struct Operation
    {
        /// 0: Position, 1: Rotation 3D, 2: Rotation 4D, 3: Scale
        public int mode;
        public Vector4 value;

        public Operation(int Mode, Vector4 Value)
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
    
    public void CopyFrom (DynamicMatrix5x5 other) {
        if (this == other)
            return;
        operations.Clear();
        operations.AddRange(other.operations);
    }

    public void Add(int mode, Vector4 val)
    {
        operations.Add(new Operation(mode, val));
    }

    public bool IsOperationEmpty()
    {
        return operations.Count == 0;
    }

    public Matrix5x5 Build()
    {
        var m = Matrix5x5.identity;
        for (int i = 0; i < operations.Count; i++)
        {
            var op = operations[i].value;

            switch (operations[i].mode)
            {
                case 0:
                    AppendTranslation(ref m, op);
                    break;
                case 1:
                    AppendRotation3D(ref m, op);
                    break;
                case 2:
                    AppendRotation4D(ref m, op);
                    break;
                case 3:
                    AppendScale(ref m, op);
                    break;
            }
        }
        return m;
    }

    static public void AppendTranslation(ref Matrix5x5 m, Vector4 pos)
    {
        float x = pos.x, y = pos.y, z = pos.z, w = pos.w;
        m.m04 = m.m00 * x + m.m01 * y + m.m02 * z + m.m03 * w + m.m04;
        m.m14 = m.m10 * x + m.m11 * y + m.m12 * z + m.m13 * w + m.m14;
        m.m24 = m.m20 * x + m.m21 * y + m.m22 * z + m.m23 * w + m.m24;
        m.m34 = m.m30 * x + m.m31 * y + m.m32 * z + m.m33 * w + m.m34;
        m.m44 = m.m40 * x + m.m41 * y + m.m42 * z + m.m43 * w + m.m44;
    }

    static public void AppendRotation3D(ref Matrix5x5 m, Vector3 rot)
    {
        m = m * new Rotation4(rot, Vector3.zero).ToMatrix();
    }

    static public void AppendRotation4D(ref Matrix5x5 m, Vector3 rot)
    {
        m = m * new Rotation4(Vector3.zero, rot).ToMatrix();
    }

    static public void AppendScale(ref Matrix5x5 m, Vector4 scl)
    {
        // TR;LD: m * TRS(0, 0, rot)
        float x = scl.x, y = scl.y, z = scl.z, w = scl.w;
        m.m00 *= x;
        m.m01 *= y;
        m.m02 *= z;
        m.m03 *= w;
        m.m10 *= x;
        m.m11 *= y;
        m.m12 *= z;
        m.m13 *= w;
        m.m20 *= x;
        m.m21 *= y;
        m.m22 *= z;
        m.m23 *= w;
        m.m30 *= x;
        m.m31 *= y;
        m.m32 *= z;
        m.m33 *= w;
        m.m40 *= x;
        m.m41 *= y;
        m.m42 *= z;
        m.m43 *= w;
    }


    public DynamicMatrix5x5 ()
    {
    }

    public DynamicMatrix5x5 (IEnumerable<Operation> list)
    {
        operations.AddRange(list);
    }

    public object Clone ()
    {
        return new DynamicMatrix5x5(operations);
    }


}
    
}
