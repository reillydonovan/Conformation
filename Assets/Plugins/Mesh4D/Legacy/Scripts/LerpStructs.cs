using UnityEngine;
using System;

namespace M4DLib.Legacy {

[Serializable]
public class Vector3Lerp
{
    public Vector3 L;
    public Vector3 R;
    public AnimationCurve CurveX;
    public AnimationCurve CurveY;
    public AnimationCurve CurveZ;
    public Vector3Lerp  (){
        CurveX =AnimationCurve.Linear(0,0,1,1);
        CurveY =AnimationCurve.Linear(0,0,1,1);
        CurveZ =AnimationCurve.Linear(0,0,1,1);
        L = Vector3.zero;
        R = Vector3.one;
        
    }
    public Vector3Lerp  (Vector3 l,Vector3 r){
        CurveX =AnimationCurve.Linear(0,0,1,1);
        CurveY =AnimationCurve.Linear(0,0,1,1);
        CurveZ =AnimationCurve.Linear(0,0,1,1);
        R = r;
        L = l;
    }
    public Vector3 Lerp(float phase){
        if(L == R)
            return L;
        else
            return new Vector3 (Mathf.Lerp ( L[0],R[0],CurveX.Evaluate ( phase)),
                                Mathf.Lerp ( L[1],R[1],CurveY.Evaluate ( phase)),
                                Mathf.Lerp ( L[2],R[2],CurveZ.Evaluate ( phase)) );
    }
    public Vector3 Lerp(Vector3 L, Vector3 R, float phase){
        if(L == R)
            return L;
        else
            return new Vector3 (Mathf.Lerp ( L[0],R[0],CurveX.Evaluate ( phase)),
                                Mathf.Lerp ( L[1],R[1],CurveY.Evaluate ( phase)),
                                Mathf.Lerp ( L[2],R[2],CurveZ.Evaluate ( phase)) );
    }
    public Vector3 LerpLinear(float phase){
        return Vector3.Lerp (L,R,phase);
    }
    public Vector3 LogarithmicLerp (float phase, bool inverse, bool abs=false)
    {
        if (phase > 0.5f){
            if(inverse && !abs)
                return Lerp ( Vector3.one,R, (phase - 0.5f) * 2);
            else
                return MeshUtilities.vDiv (Vector3.one,( Lerp (Vector3.one,MeshUtilities.vDiv (Vector3.one, MeshUtilities.vNoZero(R)), (phase - 0.5f) * 2)));
        }else if (phase < 0.5f){
            if(inverse || abs)
                return MeshUtilities.vDiv (Vector3.one , Lerp (MeshUtilities.vDiv (Vector3.one, MeshUtilities.vNoZero( L)),Vector3.one, phase * 2));
            else
                return Lerp (L, Vector3.one, phase * 2);
        }else
            return Vector3.one;     
    }
    public Vector3 LogarithmicLinearLerp (float phase, bool inverse, bool abs=false)
    {
        if (phase > 0.5f){
            if(inverse && !abs)
                return Vector3.Lerp ( Vector3.one,R, (phase - 0.5f) * 2);
            else
                return MeshUtilities.vDiv (Vector3.one,( Vector3.Lerp (Vector3.one,MeshUtilities.vDiv (Vector3.one, MeshUtilities.vNoZero(R)), (phase - 0.5f) * 2)));
        }else if (phase < 0.5f){
            if(inverse || abs)
                return MeshUtilities.vDiv (Vector3.one , Vector3.Lerp (MeshUtilities.vDiv (Vector3.one, MeshUtilities.vNoZero( L)),Vector3.one, phase * 2));
            else
                return Vector3.Lerp (L, Vector3.one, phase * 2);
        }else
            return Vector3.one;     
    }
} 
[Serializable]
public class FloatLerp
{
    public float L;
    public float R;
    public AnimationCurve Curve;
    public FloatLerp (){
        L=0;
        R=1;
        Curve =AnimationCurve.Linear(0,0,1,1);
    }
    public FloatLerp (float l, float r){
        L=l;
        R=r;
        Curve =AnimationCurve.Linear(0,0,1,1);
    }
    public float Lerp(float phase){
        if(L == R)
            return L;
        else
            return  Mathf.Lerp ( L,R,Curve.Evaluate ( phase));
    }
    public float LerpLinear(float phase){
        return  Mathf.Lerp ( L,R, phase);
    }
    public float LogarithmicLerp (float phase, bool inverse)
    {
        if (phase > 0.5f){
            if(inverse)
                return Mathf.Lerp ( 1,R, (phase - 0.5f) * 2);
            else
                return 1/ Mathf.Lerp (1,1/R, (phase - 0.5f) * 2);
        }else if (phase < 0.5f){
            if(inverse)
                return 1/ Mathf.Lerp (1/  L,1, phase * 2);
            else
                return Mathf.Lerp (L, 1, phase * 2);
        }else
            return 1;       
    }
}

}