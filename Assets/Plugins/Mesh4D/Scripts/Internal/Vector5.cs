using UnityEngine;
using System.Collections;
using System;

namespace M4DLib
{
    public struct Vector5
    {


        //
        // Fields
        //
        public float x;

        public float y;

        public float z;

        public float w;

        public float v;

        //
        // Static Properties
        //
        public static Vector5 one
        {
            get
            {
                return new Vector5(1, 1, 1, 1, 1);
            }
        }

        public static Vector5 zero
        {
            get
            {
                return new Vector5();
            }
        }

        //
        // Properties
        //
        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(Vector5.Dot(this, this));
            }
        }

        public Vector5 normalized
        {
            get
            {
                return Vector5.Normalize(this);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return Vector5.Dot(this, this);
            }
        }

        //
        // Indexer
        //
        public float this[int index]
        {
            get
            {
                float result;
                switch (index)
                {
                    case 0:
                        result = this.x;
                        break;
                    case 1:
                        result = this.y;
                        break;
                    case 2:
                        result = this.z;
                        break;
                    case 3:
                        result = this.w;
                        break;
                    case 4:
                        result = this.v;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector5 index!");
                }
                return result;
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    case 3:
                        this.w = value;
                        break;
                    case 4:
                        this.v = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector5 index!");
                }
            }
        }

        //
        // Constructors
        //
        public Vector5(float x, float y, float z, float w, float v)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            this.v = v;
        }

        public Vector5(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            this.v = 0;
        }

        public Vector5(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
            this.v = 0;
        }

        public Vector5(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
            this.w = 0;
            this.v = 0;
        }

        //
        // Static Methods
        //
        public static float Distance(Vector5 a, Vector5 b)
        {
            return Vector5.Magnitude(a - b);
        }

        public static float Dot(Vector5 a, Vector5 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w + a.v * b.v;
        }

        public static Vector5 Lerp(Vector5 a, Vector5 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector5(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t, a.v + (b.v - a.v) * t);
        }

        public static Vector5 LerpUnclamped(Vector5 a, Vector5 b, float t)
        {
            return new Vector5(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t, a.v + (b.v - a.v) * t);
        }

        public static float Magnitude(Vector5 a)
        {
            return Mathf.Sqrt(Vector5.Dot(a, a));
        }

        public static Vector5 Max(Vector5 lhs, Vector5 rhs)
        {
            return new Vector5(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z), Mathf.Max(lhs.w, rhs.w), Mathf.Max(lhs.v, rhs.v));
        }

        public static Vector5 Min(Vector5 lhs, Vector5 rhs)
        {
            return new Vector5(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z), Mathf.Min(lhs.w, rhs.w), Mathf.Min(lhs.v, rhs.v));
        }

        public static Vector5 MoveTowards(Vector5 current, Vector5 target, float maxDistanceDelta)
        {
            Vector5 a = target - current;
            float magnitude = a.magnitude;
            Vector5 result;
            if (magnitude <= maxDistanceDelta || magnitude == 0)
            {
                result = target;
            }
            else
            {
                result = current + a / magnitude * maxDistanceDelta;
            }
            return result;
        }

        public static Vector5 Normalize(Vector5 a)
        {
            float num = Vector5.Magnitude(a);
            Vector5 result;
            if (num > 1E-05)
            {
                result = a / num;
            }
            else
            {
                result = Vector5.zero;
            }
            return result;
        }

        public static Vector5 Project(Vector5 a, Vector5 b)
        {
            return b * Vector5.Dot(a, b) / Vector5.Dot(b, b);
        }

        public static Vector5 Scale(Vector5 a, Vector5 b)
        {
            return new Vector5(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w, a.v * b.v);
        }

        public static float SqrMagnitude(Vector5 a)
        {
            return Vector5.Dot(a, a);
        }

        public override bool Equals(object other)
        {
            bool result;
            if (!(other is Vector5))
            {
                result = false;
            }
            else
            {
                Vector5 vector = (Vector5)other;
                result = (this.x.Equals(vector.x) && this.y.Equals(vector.y) && this.z.Equals(vector.z) && this.w.Equals(vector.w) && this.v.Equals(vector.v));
            }
            return result;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1 ^ this.v.GetHashCode();
        }

        public void Normalize()
        {
            float num = Vector5.Magnitude(this);
            if (num > 1E-05)
            {
                this /= num;
            }
            else
            {
                this = Vector5.zero;
            }
        }

        public void Scale(Vector5 scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
            this.z *= scale.z;
            this.w *= scale.w;
            this.v *= scale.v;
        }

        public void Set(float new_x, float new_y, float new_z, float new_w, float new_v)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.w = new_w;
            this.v = new_v;
        }

        public float SqrMagnitude()
        {
            return Vector5.Dot(this, this);
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1}, {4:F1})", new object[] {
                this.x,
                this.y,
                this.z,
                this.w,
                this.v,
            });
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2}, {3}, {4})", new object[] {
                this.x.ToString(format),
                this.y.ToString(format),
                this.z.ToString(format),
                this.w.ToString(format),
                this.v.ToString(format),
            });
        }

        //
        // Operators
        //
        public static Vector5 operator +(Vector5 a, Vector5 b)
        {
            return new Vector5(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w, a.v + b.v);
        }

        public static Vector5 operator /(Vector5 a, float d)
        {
            return new Vector5(a.x / d, a.y / d, a.z / d, a.w / d, a.v / d);
        }

        public static bool operator ==(Vector5 lhs, Vector5 rhs)
        {
            return Vector5.SqrMagnitude(lhs - rhs) < 9.999999E-11;
        }

        public static implicit operator Vector5(Vector4 v)
        {
            return new Vector5(v.x, v.y, v.z, v.w, 0);
        }

        public static implicit operator Vector4(Vector5 v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static implicit operator Vector5(Vector3 v)
        {
            return new Vector5(v.x, v.y, v.z, 0, 0);
        }

        public static implicit operator Vector3(Vector5 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator Vector5(Vector2 v)
        {
            return new Vector5(v.x, v.y, 0, 0, 0);
        }

        public static implicit operator Vector2(Vector5 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static bool operator !=(Vector5 lhs, Vector5 rhs)
        {
            return !(lhs == rhs);
        }

        public static Vector5 operator *(Vector5 a, float d)
        {
            return new Vector5(a.x * d, a.y * d, a.z * d, a.w * d, a.v * d);
        }

        public static Vector5 operator *(float d, Vector5 a)
        {
            return new Vector5(a.x * d, a.y * d, a.z * d, a.w * d, a.v * d);
        }

        public static Vector5 operator -(Vector5 a, Vector5 b)
        {
            return new Vector5(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w, a.v - a.v);
        }

        public static Vector5 operator -(Vector5 a)
        {
            return new Vector5(-a.x, -a.y, -a.z, -a.w, -a.v);
        }
    }

}