using UnityEngine;
using System.Collections;

namespace M4DLib.Legacy {

[RequireComponent(typeof(Mesh4DMorpher)), ExecuteInEditMode]
public class MorphHelper : MonoBehaviour
{
	[Range(0,1), SerializeField]	private float m_phase = 0;
	[SerializeField]	private Vector3Lerp m_Offset;
	[SerializeField]	private Vector3Lerp m_Rotation;
	[SerializeField]	private Vector3Lerp m_Scale ;
	[SerializeField]	private FloatLerp m_Pivot;
	[SerializeField]	private bool m_allLinear = false;
	[SerializeField]	private ScaleInterpolationType m_scaleType = ScaleInterpolationType.LinearOrCurve;
	[SerializeField]	private bool m_keepScale = false;
	[SerializeField]	private Vector3 m_scaleFix = Vector3.one;
	[System.NonSerialized]    private bool m_dirty = true;
    
	public float phase {
        get { return m_phase; }
		set { if (m_dirty |= value != m_phase) m_phase = value; }				
    }
    public Vector3Lerp offset {
        get { return m_Offset; }
    	set { if (m_dirty |= value != m_Offset) m_Offset = value; }				
    }
	public Vector3Lerp rotation {
        get { return m_Rotation; }
    	set { if (m_dirty |= value != m_Rotation) m_Rotation = value; }				
    }
    public Vector3Lerp scale {
        get { return m_Scale; }
    	set { if (m_dirty |= value != m_Scale) m_Scale = value; }				
    }
    public FloatLerp pivot {
        get { return m_Pivot; }
    	set { if (m_dirty |= value != m_Pivot) m_Pivot = value; }				
    }
    public bool allLinear {
        get { return m_allLinear; }
    	set { if (m_dirty |= value != m_allLinear) m_allLinear = value; }				
    }
    public ScaleInterpolationType scaleType {
        get { return m_scaleType; }
    	set { if (m_dirty |= value != m_scaleType) m_scaleType = value; }				
    }
    public bool keepScale {
        get { return m_keepScale; }
    	set { if (m_dirty |= value != m_keepScale) m_keepScale = value; }				
    }
    public Vector3 scaleFix {
        get { return m_scaleFix; }
    	set { if (m_dirty |= value != m_scaleFix) m_scaleFix = value; }				
    }
	
    [SerializeField]
	private bool m_keepRotation = false;
	public bool keepRotation {
        get { return m_keepRotation; }
    	set { if (m_dirty |= value != m_keepRotation) m_keepRotation = value; }				
    }
    [SerializeField]
	private Vector3 m_rotationFix = Vector3.one;
	public Vector3 rotationFix {
        get { return m_rotationFix; }
    	set { if (m_dirty |= value != m_rotationFix) m_rotationFix = value; }				
    }

    Mesh4DMorpher mo;
	int mLID, mRID;
	Vector3 mLC, mRC;
	Mesh meshID;
	
	public enum ScaleInterpolationType{
		LinearOrCurve = 0,
		Logarithmic = 1,
		LogarithmicInverse = 2
	}
	void Reset ()
	{
		m_Scale = new Vector3Lerp (Vector3.one *0.5f, Vector3.one * 2f);
		m_Offset = new Vector3Lerp (Vector3.zero, Vector3.zero);
		m_Rotation = new Vector3Lerp(Vector3.zero ,Vector3.one *180);
		m_Pivot  = new FloatLerp (0,1);
	}
	
	void Update ()
	{
		if (m_dirty)
			Recalculate();
	}
	
	public void SetDirty () {
        m_dirty = true;
    }
	
	public void Recalculate(){
		if (mo == null) {
			mo = GetComponent< Mesh4DMorpher> ();
		}
		if (mo.MeshL != null && mo.MeshR != null) {
			if (!allLinear) {
				mo.wRotation = m_Rotation.Lerp (phase);
				mo.wPivot = m_Pivot.Lerp (phase);
				mo.wOffset = m_Offset.Lerp (phase);
			} else {
				mo.wRotation = m_Rotation.Lerp (phase);
				mo.wPivot = m_Pivot.LerpLinear (phase);
				mo.wOffset = m_Offset.LerpLinear (phase);
			}
			switch (scaleType) {
			case ScaleInterpolationType.LinearOrCurve:
				if(allLinear)
					mo.wScale =	m_Scale.LerpLinear (phase);
				else
					mo.wScale =	m_Scale.Lerp (phase);
				break;
			case ScaleInterpolationType.Logarithmic:
				if(allLinear)
					mo.wScale =	m_Scale.LogarithmicLinearLerp(phase,false);
				else
					mo.wScale =	m_Scale.LogarithmicLerp (phase,false);
				break;
			case ScaleInterpolationType.LogarithmicInverse:
				if(allLinear)
					mo.wScale =	m_Scale.LogarithmicLinearLerp(phase,true);
				else
					mo.wScale =	m_Scale.LogarithmicLerp (phase,true);
				break;
			}
			try {
				if (mLID != mo.MeshL.GetInstanceID () || mRID != mo.MeshR.GetInstanceID ()) 
					reGrabData ();
				if(meshID != null && meshID.triangles.Length > 0){
					if (keepScale) 	
						transform.localScale = scaleFix * (Vector3.Lerp (mLC, mRC, phase).magnitude / MeshUtilities.vNoZero ( meshID.bounds.size.magnitude));
					if(keepRotation)
						transform.localRotation = Quaternion.Euler ( rotationFix) * (Quaternion.FromToRotation (meshID.vertices[0],mo.oriVertL[0]));
				}
			} catch {	
			}
		}
        m_dirty = false;
    }
	void OnEnable(){
        m_dirty = true;
    }
	public void reGrabData ()
	{
		mLC = mo.MeshL.bounds.size;
		mRC = mo.MeshR.bounds.size;
		mLID = mo.MeshL.GetInstanceID ();
		mRID = mo.MeshR.GetInstanceID ();
		meshID = mo.getMesh ();
	}
}

}