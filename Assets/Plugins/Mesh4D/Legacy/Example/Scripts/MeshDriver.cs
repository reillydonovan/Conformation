using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using M4DLib.Legacy;

public class MeshDriver : MonoBehaviour
{
	public Text Txt;
	public Mesh[] baseMesh;
	public string[] Names;
	
	[HideInInspector] public float initX = 0;
	[HideInInspector] public float initY = 0;
	[HideInInspector] public float initZ = 0;
	[HideInInspector] public float initWX = 0;
	[HideInInspector] public float initWY = 0;
	[HideInInspector] public float initWZ = 0;
	[HideInInspector] public float initOX = 0;
	[HideInInspector] public float initOY = 0;
	[HideInInspector] public float initOZ = 0;
	
	Vector3 rot;
	Vector3 rotW;
	Vector3 OffW;
	int cycleIdx = 0;
	
	public void Update ()
	{
		rot += new Vector3 (initX, initY, initZ) * Time.deltaTime;
		for (int i = 0; i < 3; i++) {
			if (rot [i] > 360)
				rot [i] -= 360;
			if (rot [i] < 360)
				rot [i] += 360;
		}
		transform.rotation = Quaternion.Euler (rot);
		rotW += new Vector3 (initWX, initWY, initWZ) * Time.deltaTime;
		for (int i = 0; i < 3; i++) {
			if (rotW [i] > 360)
				rotW [i] -= 360;
			if (rotW [i] < 360)
				rotW [i] += 360;
		}
		GetComponent<Mesh4DEngine> ().wRotation = rotW;
		OffW += new Vector3 (initOX, initOY, initOZ) * Time.deltaTime;
		GetComponent<Mesh4DEngine> ().wOffset = OffW;
	}
	
	public void Reset ()
	{
		rot = Vector3.zero;
		rotW = Vector3.zero;
	}
	
	public void driveRotX (float v) { initX = v; }
	public void driveRotY (float v) { initY = v; }
	public void driveRotZ (float v) { initZ = v; }
	public void driveRotWX (float v) { initWX = v; }
	public void driveRotWY (float v) { initWY = v; }
	public void driveRotWZ (float v) { initWZ = v; }
	public void driveOffX (float v) { initOX = v; }
	public void driveOffY (float v) { initOY = v; }
	public void driveOffZ (float v) { initOZ = v; }
	public void driveWScale (float v) { GetComponent<Mesh4DEngine> ().wScale  = v; }
	public void driveXYZScale (float v) { transform.localScale  = Vector3.one * v; }
	public void driveWPivot (float v) { GetComponent<Mesh4DEngine> ().wPivot  = v; }
	
	public void CycleNex ()
	{
		cycleIdx ++;
		if (cycleIdx >= baseMesh.Length)
			cycleIdx = 0;
		Mesh4DEngine mo = GetComponent <Mesh4DEngine> ();
		mo.baseMesh = baseMesh [cycleIdx];
		rot =  Vector3.right * -90;
		rotW = Vector3.zero;
		OffW = Vector3.zero;
		Txt.text = Names[cycleIdx];
	}
	
	public void CyclePrev ()
	{
		cycleIdx --;
		if (cycleIdx < 0)
			cycleIdx = baseMesh.Length-1;
		Mesh4DEngine mo = GetComponent <Mesh4DEngine> ();
		mo.baseMesh = baseMesh [cycleIdx];
		rot = Vector3.right * -90;
		rotW = Vector3.zero;
		OffW = Vector3.zero;
		Txt.text = Names[cycleIdx];
	}
}

