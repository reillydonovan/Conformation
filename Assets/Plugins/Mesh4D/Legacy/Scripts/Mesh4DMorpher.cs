using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace M4DLib.Legacy {

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class Mesh4DMorpher : MonoBehaviour
{
    [SerializeField]    Mesh m_MeshL;
    [SerializeField]    Mesh m_MeshR;
    [SerializeField]    Vector3 m_wOffset = Vector3.zero;
    [SerializeField]    Vector3 m_wRotation = Vector3.zero;
    [SerializeField]    Vector3 m_wScale = Vector3.one;
    [Range(0, 1), SerializeField]    float m_wPivot = 1;
    [SerializeField]    bool m_separateSubMeshes = false;
    [NonSerialized]    bool m_dirty = true;
    [NonSerialized]    bool m_dirtyBake = true;
	[SerializeField]	PivotShape m_morphPivotShape = PivotShape.Sphere;	
	[SerializeField]	float m_morphPivotRadius = 1f;	
	
    public Mesh MeshL
    {
        get { return m_MeshL; }
        set { if (m_dirty |= m_dirtyBake |= value != m_MeshL) m_MeshL = value; }
    }

    public Mesh MeshR
    {
        get { return m_MeshR; }
        set { if (m_dirty |= m_dirtyBake |= value != m_MeshL) m_MeshR = value; }
    }


    public Vector3 wOffset
    {
        get { return m_wOffset; }
        set { if (m_dirty |= value != m_wOffset) m_wOffset = value; }
    }


    public Vector3 wRotation
    {
        get { return m_wRotation; }
        set { if (m_dirty |= value != m_wRotation) m_wRotation = value; }
    }


    public Vector3 wScale
    {
        get { return m_wScale; }
        set { if (m_dirty |= value != m_wScale) m_wScale = value; }
    }

    public float wPivot
    {
        get { return m_wPivot; }
        set { if (m_dirty |= value != m_wPivot) m_wPivot = value; }
  	}

	public PivotShape morphPivotShape {
        get { return m_morphPivotShape; }
        set { if (m_dirty |= m_dirtyBake |= value != m_morphPivotShape) m_morphPivotShape = value; }
    }
    public float morphPivotRadius {
        get { return m_morphPivotRadius; }
        set { if (m_dirty |= m_dirtyBake |= value != m_morphPivotRadius) m_morphPivotRadius = value; }
	}

    public enum PivotShape{
		Sphere = 0,
		Box = 1,
		Self =2
	}
	
	#region Data Bakes
	public 	Vector3[] oriVertL; //Default Vertices
	public 	Vector3[] oriVertR; //Default Vertices
	public 	Vector3[] pivot; //Pivot Point
	Vector3[] vert=new Vector3[]{};
	public bool hasBaked=false;
	Mesh mesh;
	float lastMorphRadius=0;
	#endregion 
	
	void RecheckObject(){
		if(mesh == null) {
			MeshFilter f = GetComponent<MeshFilter>();
			mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			mesh.name = "Morph4D_" + transform.name;
			f.mesh = mesh;
		}
	}
	
	void Update ()
	{
		if (m_MeshL != null && m_MeshR != null) {
			if (mesh == null || m_dirtyBake)
				BakeMesh ();
			if (m_dirty)
				CalculateMesh ();
		}
	}
	
	void OnEnable()
	{
        m_dirtyBake = true;
        m_dirty = true;
    }
	void Reset ()
	{
		mesh = null;
        OnEnable();
    }
	
	public void SetDirty (bool bakeDirty = false) {
		m_dirtyBake |= bakeDirty;
        m_dirty = true;
    }
	
	public void BakeMesh ()
	{
		if (m_MeshL != null && m_MeshR != null) {
			if(mesh == null)
				RecheckObject();
			
			oriVertL = new Vector3[]{};
			mesh.Clear();
			CombineInstance[] combines = new CombineInstance[2];
			combines[0].mesh = m_MeshL;
			combines[1].mesh = m_MeshR;
			
			mesh.CombineMeshes (combines,!m_separateSubMeshes,false);
			oriVertL = (m_MeshL.vertices);
			oriVertR = (m_MeshR.vertices);
			
			hasBaked = true;
			GetComponent<MeshFilter>().mesh = mesh;
			
			pivot = oriVertL.Concat(oriVertR).ToArray ();
			switch (m_morphPivotShape) {
			case PivotShape.Sphere:
				pivot = System.Array.ConvertAll (pivot, x =>  MeshUtilities.sphereProject (x ,m_morphPivotRadius));
				break;
			case PivotShape.Box:
				pivot = System.Array.ConvertAll (pivot,x =>  MeshUtilities.boxProject (x ,m_morphPivotRadius));
				break;
			case PivotShape.Self:
				pivot = System.Array.ConvertAll (pivot,x =>  (x * m_morphPivotRadius));
				break;
			}
			hasBaked = true;
			lastMorphRadius = m_morphPivotRadius;
			
			if(GetComponent<MorphHelper>()!= null)
				GetComponent<MorphHelper>().reGrabData();
				
            m_dirtyBake = false;
        }
	}
	public Mesh getMesh(){
		return mesh;
	}
	
	public void CalculateMesh ()
	{
		if	(m_MeshL != null && m_MeshR != null && mesh != null){
			var DiffA = Vector3.Lerp (Vector3.zero , m_wOffset,m_wPivot);
			var DiffB = Vector3.Lerp ( m_wOffset,Vector3.zero ,m_wPivot);
			var FacA = MeshUtilities.vNoZero ( Vector3.Lerp (Vector3.one, m_wScale,m_wPivot));
			var FacB = MeshUtilities.vNoZero (  Vector3.Lerp (m_wScale,Vector3.one, m_wPivot));

            vert = new Vector3[mesh.vertices.Length];
			for (int i = 0; i < vert.Length; i++) {
				if(i<oriVertL.Length)
					vert [i] =  MeshUtilities.vRot (MeshUtilities.vMult ( oriVertL [i], FacA) + DiffA , m_wRotation,  pivot[i]);
				else
					vert [i] =  MeshUtilities.vRot (MeshUtilities.vDiv ( oriVertR [i-oriVertL.Length], FacB) - DiffB, m_wRotation,  pivot[i]);
			}
			mesh.vertices = vert;
			mesh.RecalculateBounds ();
            m_dirty = false;
        }
	}
	
	void OnDrawGizmosSelected ()
	{
		if (m_MeshL != null && m_MeshR != null) {
			if(hasBaked){
				Gizmos.color = Color.cyan;
				if(m_morphPivotShape == PivotShape.Sphere)
					Gizmos.DrawWireSphere (transform.position,lastMorphRadius);// * Mathf.Max (new float[]{ transform.lossyScale.x,transform.lossyScale.y,transform.lossyScale.z}));
				else	if(m_morphPivotShape == PivotShape.Box)
					Gizmos.DrawWireCube (transform.position,Vector3.one *lastMorphRadius*2);// * lastMorphRadius*2);
			}
			if(lastMorphRadius != m_morphPivotRadius){
				Gizmos.color = Color.magenta;
				if(m_morphPivotShape == PivotShape.Sphere)
					Gizmos.DrawWireSphere (transform.position,m_morphPivotRadius);// * Mathf.Max (new float[]{ transform.lossyScale.x,transform.lossyScale.y,transform.lossyScale.z}));
				else if(m_morphPivotShape == PivotShape.Box)
					Gizmos.DrawWireCube (transform.position,Vector3.one* m_morphPivotRadius*2);
			}
		}
	}
}
}