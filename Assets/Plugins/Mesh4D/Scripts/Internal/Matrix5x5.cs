using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine;

namespace M4DLib
{
    /// <summary>
    /// Representation of 5x5 Matrix.
    /// Mainly used for containing extra translation of 4D manipulations.
    /// </summary>
    [Serializable]
    public partial struct Matrix5x5
    {
        //
        // Fields
        //
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m04;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m14;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m24;
        public float m30;
        public float m31;
        public float m32;
        public float m33;
        public float m34;
        public float m40;
        public float m41;
        public float m42;
        public float m43;
        public float m44;

        //
        // Static Properties
        //
        public static Matrix5x5 identity
        {
            get
            {
                return new Matrix5x5
                {
                    m00 = 1,
                    m11 = 1,
                    m22 = 1,
                    m33 = 1,
                    m44 = 1,
                };
            }
        }

        public static Matrix5x5 zero
        {
            get
            {
                return new Matrix5x5();
            }
        }

        //
        // Indexer
        //
        public float this [int row, int column]
        {
            get
            {
                return this[row + column * 5];
            }
            set
            {
                this[row + column * 5] = value;
            }
        }

        public float this [int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.m00;
                    case 1:
                        return this.m10;
                    case 2:
                        return this.m20;
                    case 3:
                        return this.m30;
                    case 4:
                        return this.m40;
                    case 5:
                        return this.m01;
                    case 6:
                        return this.m11;
                    case 7:
                        return this.m21;
                    case 8:
                        return this.m31;
                    case 9:
                        return this.m41;
                    case 10:
                        return this.m02;
                    case 11:
                        return this.m12;
                    case 12:
                        return this.m22;
                    case 13:
                        return this.m32;
                    case 14:
                        return this.m42;
                    case 15:
                        return this.m03;
                    case 16:
                        return this.m13;
                    case 17:
                        return this.m23;
                    case 18:
                        return this.m33;
                    case 19:
                        return this.m43;
                    case 20:
                        return this.m04;
                    case 21:
                        return this.m14;
                    case 22:
                        return this.m24;
                    case 23:
                        return this.m34;
                    case 24:
                        return this.m44;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.m00 = value;
                        break;
                    case 1:
                        this.m10 = value;
                        break;
                    case 2:
                        this.m20 = value;
                        break;
                    case 3:
                        this.m30 = value;
                        break;
                    case 4:
                        this.m30 = value;
                        break;
                    case 5:
                        this.m01 = value;
                        break;
                    case 6:
                        this.m11 = value;
                        break;
                    case 7:
                        this.m21 = value;
                        break;
                    case 8:
                        this.m31 = value;
                        break;
                    case 9:
                        this.m41 = value;
                        break;
                    case 10:
                        this.m02 = value;
                        break;
                    case 11:
                        this.m12 = value;
                        break;
                    case 12:
                        this.m22 = value;
                        break;
                    case 13:
                        this.m32 = value;
                        break;
                    case 14:
                        this.m42 = value;
                        break;
                    case 15:
                        this.m03 = value;
                        break;
                    case 16:
                        this.m13 = value;
                        break;
                    case 17:
                        this.m23 = value;
                        break;
                    case 18:
                        this.m33 = value;
                        break;
                    case 19:
                        this.m43 = value;
                        break;
                    case 20:
                        this.m04 = value;
                        break;
                    case 21:
                        this.m14 = value;
                        break;
                    case 22:
                        this.m24 = value;
                        break;
                    case 23:
                        this.m34 = value;
                        break;
                    case 24:
                        this.m44 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        //
        // Static Methods
        //

        /// <summary>
        /// Create a 5x5 matrix for scaling vectors
        /// </summary>
        public static Matrix5x5 Scale(Vector4 v)
        {
            return new Matrix5x5
            {
                m00 = v.x,
                m11 = v.y,
                m22 = v.z,
                m33 = v.w,
                m44 = 1,
            };
        }

        /// <summary>
        /// Create a 5x5 matrix for translating vectors
        /// </summary>
        public static Matrix5x5 Translate(Vector4 v)
        {
            return new Matrix5x5
            {
                m00 = 1,
                m04 = v.x,
                m11 = 1,
                m14 = v.y,
                m22 = 1,
                m24 = v.z,
                m33 = 1,
                m34 = v.w,
                m44 = 1,
            };
        }

        public static Matrix5x5 Translate(Matrix5x5 m, Vector4 v)
        {
            m.m04 += v.x;
            m.m14 += v.y;
            m.m24 += v.z;
            m.m34 += v.w;
            return m;
        }

        public static Matrix5x5 TRS(Vector4 position, Quaternion4 rotation, Vector4 scale)
        {
            //return  Matrix5x5.Translate(position) * rotation.ToMatrix() * Matrix5x5.Scale(scale);
            return Matrix5x5.Translate(rotation.ToMatrix(scale), position);
        }

        //
        // Methods
        //
        public override bool Equals(object other)
        {
            bool result;
            if (!(other is Matrix5x5))
            {
                result = false;
            }
            else
            {
                Matrix5x5 m = (Matrix5x5)other;
                result = (this.GetColumn(0).Equals(m.GetColumn(0)) && this.GetColumn(1).Equals(m.GetColumn(1)) && this.GetColumn(2).Equals(m.GetColumn(2)) && this.GetColumn(3).Equals(m.GetColumn(3)) && this.GetColumn(4).Equals(m.GetColumn(4)));
            }
            return result;
        }

        public Vector5 GetColumn(int i)
        {
            return new Vector5(this[0, i], this[1, i], this[2, i], this[3, i], this[4, i]);
        }

        public override int GetHashCode()
        {
            return this.GetColumn(0).GetHashCode() ^ this.GetColumn(1).GetHashCode() << 2 ^ this.GetColumn(2).GetHashCode() >> 2 ^ this.GetColumn(3).GetHashCode() >> 1;
        }

        public Vector5 GetRow(int i)
        {
            return new Vector5(this[i, 0], this[i, 1], this[i, 2], this[i, 3], this[i, 4]);
        }


        public void SetColumn(int i, Vector5 v)
        {
            this[0, i] = v.x;
            this[1, i] = v.y;
            this[2, i] = v.z;
            this[3, i] = v.w;
            this[4, i] = v.v;
        }

        public void SetRow(int i, Vector5 v)
        {
            this[i, 0] = v.x;
            this[i, 1] = v.y;
            this[i, 2] = v.z;
            this[i, 3] = v.w;
            this[i, 4] = v.v;
        }

        //
        // Operators
        //
        public static bool operator ==(Matrix5x5 lhs, Matrix5x5 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        public static bool operator !=(Matrix5x5 lhs, Matrix5x5 rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return ToString("F1");
        }

        public string ToString(string format)
        {
            var s = new System.Text.StringBuilder("Matrix5x5: \n");
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    s.Append(this[y, x].ToString(format));
                    s.Append(x == 4 ? '\n': '\t');
                }
            }
            return s.ToString();
        }

    }
}