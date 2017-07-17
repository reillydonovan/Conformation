using UnityEngine;
using System.Collections;
using M4DLib;
using System;
using UnityEngine.UI;


public class M4ExampleShowcaser : MonoBehaviour
{

    public Mesh[] bases;
    public Text text;
    public Rotation4 euler;
    public Vector4 scale;
    public float speed;
    public float opacity = 5f;
    Transform4 trs;

    [NonSerialized] float _X = 0;
    [NonSerialized] float _Y = 0;
    [NonSerialized] float _Z = 0;
    [NonSerialized] float _T = 0;
    [NonSerialized] float _U = 0;
    [NonSerialized] float _V = 0;
    float _XYZ = 1;
    float _W = 1;
    int iter = 0;

    void Start ()
    {
        trs = GetComponent<Transform4>();
        SetSycle();
    }

    void Update ()
    {
        _T = Mathf.Sin(Time.deltaTime);
        euler.xyz += new Vector3(_X, _Y, _Z) * speed * Time.deltaTime;
        euler.tuv += new Vector3(_T, _U, _V) * speed * Time.deltaTime;
       // scale.w += _W * speed;
        //proj.w = Mathf.Min(proj.w, -5);
        trs.localRotation = euler.ToQuaternion();
        trs.localScale = new Vector4(_XYZ,_XYZ,_XYZ,_W);
    }

    public void CyclePlus ()
    {
        iter = (iter + 1) % bases.Length;
        SetSycle ();
    }

    public void CycleMinus ()
    {
        iter = (iter == 0 ? bases.Length : iter) - 1;
        SetSycle ();
    }

    public void Reset ()
    {
        euler = Rotation4.identity;
    }

    void SetSycle ()
    {
        var m = GetComponent<MU4Extruded>().baseMesh = bases[iter];
        text.text = m.name;
        GetComponent<MeshRenderer>().material.SetFloat("alpha", opacity / Mathf.Sqrt(m.vertexCount));
        GetComponent<M4Renderer>().SetDirty(true);
    }

    public void DriveX (float v) { _X = v; }
    public void DriveY (float v) { _Y = v; }
    public void DriveZ (float v) { _Z = v; }
    public void DriveT (float v) { _T = v; }
    public void DriveU (float v) { _U = v; }
    public void DriveV (float v) { _V = v; }
    public void DriveXYZ (float v) { _XYZ = v; }
    public void DriveW (float v) { _W = v; }


}

