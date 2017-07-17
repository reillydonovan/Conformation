using UnityEngine;
using System.Collections;

namespace M4DLib
{
    public partial struct Matrix5x5
    {

        public static Matrix5x5 Inverse(Matrix5x5 m)
        {
            // Credit to awesome mathematicians and my bot
            // http://github.com/WelloSoft/N-Matrix-Programmer
                
            var A3434 = m.m33 * m.m44 - m.m34 * m.m43;
            var A2434 = m.m32 * m.m44 - m.m34 * m.m42;
            var A2334 = m.m32 * m.m43 - m.m33 * m.m42;
            var A1434 = m.m31 * m.m44 - m.m34 * m.m41;
            var A1334 = m.m31 * m.m43 - m.m33 * m.m41;
            var A1234 = m.m31 * m.m42 - m.m32 * m.m41;
            var A0434 = m.m30 * m.m44 - m.m34 * m.m40;
            var A0334 = m.m30 * m.m43 - m.m33 * m.m40;
            var A0234 = m.m30 * m.m42 - m.m32 * m.m40;
            var A0134 = m.m30 * m.m41 - m.m31 * m.m40;
            var A3424 = m.m23 * m.m44 - m.m24 * m.m43;
            var A2424 = m.m22 * m.m44 - m.m24 * m.m42;
            var A2324 = m.m22 * m.m43 - m.m23 * m.m42;
            var A1424 = m.m21 * m.m44 - m.m24 * m.m41;
            var A1324 = m.m21 * m.m43 - m.m23 * m.m41;
            var A1224 = m.m21 * m.m42 - m.m22 * m.m41;
            var A3423 = m.m23 * m.m34 - m.m24 * m.m33;
            var A2423 = m.m22 * m.m34 - m.m24 * m.m32;
            var A2323 = m.m22 * m.m33 - m.m23 * m.m32;
            var A1423 = m.m21 * m.m34 - m.m24 * m.m31;
            var A1323 = m.m21 * m.m33 - m.m23 * m.m31;
            var A1223 = m.m21 * m.m32 - m.m22 * m.m31;
            var A0424 = m.m20 * m.m44 - m.m24 * m.m40;
            var A0324 = m.m20 * m.m43 - m.m23 * m.m40;
            var A0224 = m.m20 * m.m42 - m.m22 * m.m40;
            var A0423 = m.m20 * m.m34 - m.m24 * m.m30;
            var A0323 = m.m20 * m.m33 - m.m23 * m.m30;
            var A0223 = m.m20 * m.m32 - m.m22 * m.m30;
            var A0124 = m.m20 * m.m41 - m.m21 * m.m40;
            var A0123 = m.m20 * m.m31 - m.m21 * m.m30;

            var B234234 = m.m22 * A3434 - m.m23 * A2434 + m.m24 * A2334;
            var B134234 = m.m21 * A3434 - m.m23 * A1434 + m.m24 * A1334;
            var B124234 = m.m21 * A2434 - m.m22 * A1434 + m.m24 * A1234;
            var B123234 = m.m21 * A2334 - m.m22 * A1334 + m.m23 * A1234;
            var B034234 = m.m20 * A3434 - m.m23 * A0434 + m.m24 * A0334;
            var B024234 = m.m20 * A2434 - m.m22 * A0434 + m.m24 * A0234;
            var B023234 = m.m20 * A2334 - m.m22 * A0334 + m.m23 * A0234;
            var B014234 = m.m20 * A1434 - m.m21 * A0434 + m.m24 * A0134;
            var B013234 = m.m20 * A1334 - m.m21 * A0334 + m.m23 * A0134;
            var B012234 = m.m20 * A1234 - m.m21 * A0234 + m.m22 * A0134;
            var B234134 = m.m12 * A3434 - m.m13 * A2434 + m.m14 * A2334;
            var B134134 = m.m11 * A3434 - m.m13 * A1434 + m.m14 * A1334;
            var B124134 = m.m11 * A2434 - m.m12 * A1434 + m.m14 * A1234;
            var B123134 = m.m11 * A2334 - m.m12 * A1334 + m.m13 * A1234;
            var B234124 = m.m12 * A3424 - m.m13 * A2424 + m.m14 * A2324;
            var B134124 = m.m11 * A3424 - m.m13 * A1424 + m.m14 * A1324;
            var B124124 = m.m11 * A2424 - m.m12 * A1424 + m.m14 * A1224;
            var B123124 = m.m11 * A2324 - m.m12 * A1324 + m.m13 * A1224;
            var B234123 = m.m12 * A3423 - m.m13 * A2423 + m.m14 * A2323;
            var B134123 = m.m11 * A3423 - m.m13 * A1423 + m.m14 * A1323;
            var B124123 = m.m11 * A2423 - m.m12 * A1423 + m.m14 * A1223;
            var B123123 = m.m11 * A2323 - m.m12 * A1323 + m.m13 * A1223;
            var B034134 = m.m10 * A3434 - m.m13 * A0434 + m.m14 * A0334;
            var B024134 = m.m10 * A2434 - m.m12 * A0434 + m.m14 * A0234;
            var B023134 = m.m10 * A2334 - m.m12 * A0334 + m.m13 * A0234;
            var B034124 = m.m10 * A3424 - m.m13 * A0424 + m.m14 * A0324;
            var B024124 = m.m10 * A2424 - m.m12 * A0424 + m.m14 * A0224;
            var B023124 = m.m10 * A2324 - m.m12 * A0324 + m.m13 * A0224;
            var B034123 = m.m10 * A3423 - m.m13 * A0423 + m.m14 * A0323;
            var B024123 = m.m10 * A2423 - m.m12 * A0423 + m.m14 * A0223;
            var B023123 = m.m10 * A2323 - m.m12 * A0323 + m.m13 * A0223;
            var B014134 = m.m10 * A1434 - m.m11 * A0434 + m.m14 * A0134;
            var B013134 = m.m10 * A1334 - m.m11 * A0334 + m.m13 * A0134;
            var B014124 = m.m10 * A1424 - m.m11 * A0424 + m.m14 * A0124;
            var B013124 = m.m10 * A1324 - m.m11 * A0324 + m.m13 * A0124;
            var B014123 = m.m10 * A1423 - m.m11 * A0423 + m.m14 * A0123;
            var B013123 = m.m10 * A1323 - m.m11 * A0323 + m.m13 * A0123;
            var B012134 = m.m10 * A1234 - m.m11 * A0234 + m.m12 * A0134;
            var B012124 = m.m10 * A1224 - m.m11 * A0224 + m.m12 * A0124;
            var B012123 = m.m10 * A1223 - m.m11 * A0223 + m.m12 * A0123;

            var det = m.m00 * (m.m11 * B234234 - m.m12 * B134234 + m.m13 * B124234 - m.m14 * B123234)
            - m.m01 * (m.m10 * B234234 - m.m12 * B034234 + m.m13 * B024234 - m.m14 * B023234)
            + m.m02 * (m.m10 * B134234 - m.m11 * B034234 + m.m13 * B014234 - m.m14 * B013234)
            - m.m03 * (m.m10 * B124234 - m.m11 * B024234 + m.m12 * B014234 - m.m14 * B012234)
            + m.m04 * (m.m10 * B123234 - m.m11 * B023234 + m.m12 * B013234 - m.m13 * B012234);

            if (det * det < 1e-5f)
                return Matrix5x5.identity;

            det = 1 / det;

            return new Matrix5x5()
            {
                m00 = det * (m.m11 * B234234 - m.m12 * B134234 + m.m13 * B124234 - m.m14 * B123234),
                m01 = det * -(m.m01 * B234234 - m.m02 * B134234 + m.m03 * B124234 - m.m04 * B123234),
                m02 = det * (m.m01 * B234134 - m.m02 * B134134 + m.m03 * B124134 - m.m04 * B123134),
                m03 = det * -(m.m01 * B234124 - m.m02 * B134124 + m.m03 * B124124 - m.m04 * B123124),
                m04 = det * (m.m01 * B234123 - m.m02 * B134123 + m.m03 * B124123 - m.m04 * B123123),
                m10 = det * -(m.m10 * B234234 - m.m12 * B034234 + m.m13 * B024234 - m.m14 * B023234),
                m11 = det * (m.m00 * B234234 - m.m02 * B034234 + m.m03 * B024234 - m.m04 * B023234),
                m12 = det * -(m.m00 * B234134 - m.m02 * B034134 + m.m03 * B024134 - m.m04 * B023134),
                m13 = det * (m.m00 * B234124 - m.m02 * B034124 + m.m03 * B024124 - m.m04 * B023124),
                m14 = det * -(m.m00 * B234123 - m.m02 * B034123 + m.m03 * B024123 - m.m04 * B023123),
                m20 = det * (m.m10 * B134234 - m.m11 * B034234 + m.m13 * B014234 - m.m14 * B013234),
                m21 = det * -(m.m00 * B134234 - m.m01 * B034234 + m.m03 * B014234 - m.m04 * B013234),
                m22 = det * (m.m00 * B134134 - m.m01 * B034134 + m.m03 * B014134 - m.m04 * B013134),
                m23 = det * -(m.m00 * B134124 - m.m01 * B034124 + m.m03 * B014124 - m.m04 * B013124),
                m24 = det * (m.m00 * B134123 - m.m01 * B034123 + m.m03 * B014123 - m.m04 * B013123),
                m30 = det * -(m.m10 * B124234 - m.m11 * B024234 + m.m12 * B014234 - m.m14 * B012234),
                m31 = det * (m.m00 * B124234 - m.m01 * B024234 + m.m02 * B014234 - m.m04 * B012234),
                m32 = det * -(m.m00 * B124134 - m.m01 * B024134 + m.m02 * B014134 - m.m04 * B012134),
                m33 = det * (m.m00 * B124124 - m.m01 * B024124 + m.m02 * B014124 - m.m04 * B012124),
                m34 = det * -(m.m00 * B124123 - m.m01 * B024123 + m.m02 * B014123 - m.m04 * B012123),
                m40 = det * (m.m10 * B123234 - m.m11 * B023234 + m.m12 * B013234 - m.m13 * B012234),
                m41 = det * -(m.m00 * B123234 - m.m01 * B023234 + m.m02 * B013234 - m.m03 * B012234),
                m42 = det * (m.m00 * B123134 - m.m01 * B023134 + m.m02 * B013134 - m.m03 * B012134),
                m43 = det * -(m.m00 * B123124 - m.m01 * B023124 + m.m02 * B013124 - m.m03 * B012124),
                m44 = det * (m.m00 * B123123 - m.m01 * B023123 + m.m02 * B013123 - m.m03 * B012123),
            };


        }

        public static Vector5 operator *(Matrix5x5 lhs, Vector5 rhs)
        {
            float x = rhs.x, y = rhs.y, z = rhs.z, w = rhs.w, v = rhs.v;
            return new Vector5
            {
                x = lhs.m00 * x + lhs.m01 * y + lhs.m02 * z + lhs.m03 * w + lhs.m04 * v,
                y = lhs.m10 * x + lhs.m11 * y + lhs.m12 * z + lhs.m13 * w + lhs.m14 * v,
                z = lhs.m20 * x + lhs.m21 * y + lhs.m22 * z + lhs.m23 * w + lhs.m24 * v,
                w = lhs.m30 * x + lhs.m31 * y + lhs.m32 * z + lhs.m33 * w + lhs.m34 * v,
                v = lhs.m40 * x + lhs.m41 * y + lhs.m42 * z + lhs.m43 * w + lhs.m44 * v,
            };
        }

        public static Vector4 operator *(Matrix5x5 lhs, Vector4 rhs)
        {
            return lhs.MultiplyPoint4x5(rhs);
        }

        public Vector4 MultiplyPoint4x5(Vector4 v)
        {
            float x = v.x, y = v.y, z = v.z, w = v.w;
            return new Vector4
            {
                x = m00 * x + m01 * y + m02 * z + m03 * w + m04,
                y = m10 * x + m11 * y + m12 * z + m13 * w + m14,
                z = m20 * x + m21 * y + m22 * z + m23 * w + m24,
                w = m30 * x + m31 * y + m32 * z + m33 * w + m34,
            };
        }

        public Vector4 MultiplyVector(Vector4 v)
        {
            float x = v.x, y = v.y, z = v.z, w = v.w;
            return new Vector4
            {
                x = m00 * x + m01 * y + m02 * z + m03 * w,
                y = m10 * x + m11 * y + m12 * z + m13 * w,
                z = m20 * x + m21 * y + m22 * z + m23 * w,
                w = m30 * x + m31 * y + m32 * z + m33 * w,
            };
        }

        public static Matrix5x5 operator *(Matrix5x5 lhs, Matrix5x5 rhs)
        {
            return new Matrix5x5
            {
                m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30 + lhs.m04 * rhs.m40,
                m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31 + lhs.m04 * rhs.m41,
                m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32 + lhs.m04 * rhs.m42,
                m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33 + lhs.m04 * rhs.m43,
                m04 = lhs.m00 * rhs.m04 + lhs.m01 * rhs.m14 + lhs.m02 * rhs.m24 + lhs.m03 * rhs.m34 + lhs.m04 * rhs.m44,

                m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30 + lhs.m14 * rhs.m40,
                m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31 + lhs.m14 * rhs.m41,
                m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32 + lhs.m14 * rhs.m42,
                m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33 + lhs.m14 * rhs.m43,
                m14 = lhs.m10 * rhs.m04 + lhs.m11 * rhs.m14 + lhs.m12 * rhs.m24 + lhs.m13 * rhs.m34 + lhs.m14 * rhs.m44,

                m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30 + lhs.m24 * rhs.m40,
                m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31 + lhs.m24 * rhs.m41,
                m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32 + lhs.m24 * rhs.m42,
                m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33 + lhs.m24 * rhs.m43,
                m24 = lhs.m20 * rhs.m04 + lhs.m21 * rhs.m14 + lhs.m22 * rhs.m24 + lhs.m23 * rhs.m34 + lhs.m24 * rhs.m44,

                m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30 + lhs.m34 * rhs.m40,
                m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31 + lhs.m34 * rhs.m41,
                m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32 + lhs.m34 * rhs.m42,
                m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33 + lhs.m34 * rhs.m43,
                m34 = lhs.m30 * rhs.m04 + lhs.m31 * rhs.m14 + lhs.m32 * rhs.m24 + lhs.m33 * rhs.m34 + lhs.m34 * rhs.m44,

                m40 = lhs.m40 * rhs.m00 + lhs.m41 * rhs.m10 + lhs.m42 * rhs.m20 + lhs.m43 * rhs.m30 + lhs.m44 * rhs.m40,
                m41 = lhs.m40 * rhs.m01 + lhs.m41 * rhs.m11 + lhs.m42 * rhs.m21 + lhs.m43 * rhs.m31 + lhs.m44 * rhs.m41,
                m42 = lhs.m40 * rhs.m02 + lhs.m41 * rhs.m12 + lhs.m42 * rhs.m22 + lhs.m43 * rhs.m32 + lhs.m44 * rhs.m42,
                m43 = lhs.m40 * rhs.m03 + lhs.m41 * rhs.m13 + lhs.m42 * rhs.m23 + lhs.m43 * rhs.m33 + lhs.m44 * rhs.m43,
                m44 = lhs.m40 * rhs.m04 + lhs.m41 * rhs.m14 + lhs.m42 * rhs.m24 + lhs.m43 * rhs.m34 + lhs.m44 * rhs.m44,
            };
        }

        public static implicit operator Matrix5x5(Matrix4x4 m)
        {
            return new Matrix5x5
            {
                m00 = m.m00, m01 = m.m01, m02 = m.m02, m03 = m.m03, m04 = 0,
                m10 = m.m10, m11 = m.m11, m12 = m.m12, m13 = m.m13, m14 = 0,
                m20 = m.m20, m21 = m.m21, m22 = m.m22, m23 = m.m23, m24 = 0,
                m30 = m.m30, m31 = m.m31, m32 = m.m32, m33 = m.m33, m34 = 0,
                m40 = 0, m41 = 0, m42 = 0, m43 = 0, m44 = 1,
            };
        }

        public static explicit operator Matrix4x4(Matrix5x5 m)
        {
            return new Matrix4x4
            {
                m00 = m.m00, m01 = m.m01, m02 = m.m02, m03 = m.m03,
                m10 = m.m10, m11 = m.m11, m12 = m.m12, m13 = m.m13,
                m20 = m.m20, m21 = m.m21, m22 = m.m22, m23 = m.m23,
                m30 = m.m30, m31 = m.m31, m32 = m.m32, m33 = m.m33,
            };
        }
    }

}