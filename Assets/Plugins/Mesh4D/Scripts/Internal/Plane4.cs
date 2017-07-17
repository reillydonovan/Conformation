using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib
{
    public struct Plane4
    {

        public float distance;
        public Vector4 normal;

        public Plane4(Vector4 inNormal, Vector4 inPoint)
        {
            this.normal = Vector4.Normalize(inNormal);
            this.distance = -Vector4.Dot(inNormal, inPoint);
        }

        public Plane4(Vector4 inNormal, float d)
        {
            this.normal = Vector4.Normalize(inNormal);
            this.distance = d;
        }

        public Plane4(Vector4 a, Vector4 b, Vector4 c, Vector4 d)
        {
            this.normal = Vector4.Normalize(Vec4.Cross(b - a, c - a, d - a));
            this.distance = -Vector4.Dot(this.normal, a);
        }

        public float GetDistanceToPoint(Vector4 inPt)
        {
            return Vector4.Dot(this.normal, inPt) + this.distance;
        }

        public Vector4 GetNearestPoint(Vector4 inPt)
        {
            float dist = -Vector4.Dot(inPt, this.normal) - this.distance;
            return inPt + this.normal * dist;
        }

        public bool GetSide(Vector4 inPt)
        {
            return Vector4.Dot(this.normal, inPt) + this.distance > 0;
        }


        public float Intersect(Vector4 a, Vector4 b)
        {
            // We don't normalize the delta, so phase is always [0..1]
            // Don't call raycast.. Refactor it instead. (performance)
            // var r = Raycast(a, dir, out phase);
            float num = Vector4.Dot(b - a, this.normal);
            float num2 = -Vector4.Dot(a, this.normal) - this.distance;
            if (num * num > 0.00001f)
                return num2 / num;
            else
                return 0;
        }

        public bool IntersectsBounds(Bounds4 bound)
        {
            var extent = bound.extent;
            var center = bound.center;

            var r = Vector4.Dot(extent, Vec4.Abs(normal));
            var s = Vector4.Dot(center, normal) + distance;
            return  Math.Abs(s) <= r;
        }

        public bool Raycast(Vector4 origin, Vector4 direction, out float enter)
        {
            float num = Vector4.Dot(direction, this.normal);
            float num2 = -Vector4.Dot(origin, this.normal) - this.distance;
            if (num * num < 0.00001f)
            {
                enter = 0;
                return false;
            }
            else
            {
                enter = num2 / num;
                return (enter > 0);
            }
        }

        public bool SameSide(Vector4 inPt0, Vector4 inPt1)
        {
            float distanceToPoint = this.GetDistanceToPoint(inPt0);
            float distanceToPoint2 = this.GetDistanceToPoint(inPt1);
            return (distanceToPoint > 0 && distanceToPoint2 > 0) || (distanceToPoint <= 0 && distanceToPoint2 <= 0);
        }

        public void Set4Points(Vector4 a, Vector4 b, Vector4 c, Vector4 d)
        {
            this.normal = Vector4.Normalize(Vec4.Cross(b - a, c - a, d - a));
            this.distance = -Vector4.Dot(this.normal, a);
        }

        public void SetNormalAndPosition(Vector4 inNormal, Vector4 inPoint)
        {
            this.normal = Vector4.Normalize(inNormal);
            this.distance = -Vector4.Dot(inNormal, inPoint);
        }

    }
}