using UnityEngine;
using System.Collections;
using M4DLib.Legacy;

public class MorphDriver : MonoBehaviour {
	public MorphHelper[] Objects;
	public float distance=5;
	public float speedAngle=10;
	Vector2 curAxis;
	int curObj=0;
	Vector3 Tpos ;

	// Use this for initialization
	void Start () {
		curAxis = Vector2.right * 180;
		curAxis += new Vector2 ( Input.GetAxis("Mouse X")*2,-Input.GetAxis("Mouse Y"))  * speedAngle;
		
		Quaternion Trot = Quaternion.Euler(curAxis[1], curAxis[0], 0);
		Tpos = Trot * new Vector3(0, 0, -distance) + MeshUtilities.vMult (  Objects[curObj].transform.position,new Vector3(1,0,1));
		transform.rotation = Trot;
		transform.position = Tpos;

	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetMouseButton(0)|| smtGo.magnitude > 0.01f){
			if(Input.GetMouseButton(0) && Input.mousePosition.y > Screen.height/5 ){
			curAxis += new Vector2 ( Input.GetAxis("Mouse X")*2,-Input.GetAxis("Mouse Y"))  * speedAngle;
		
			Quaternion Trot = Quaternion.Euler(curAxis[1], curAxis[0], 0);
			Tpos = Trot * new Vector3(0, 0, -distance) + MeshUtilities.vMult (  Objects[curObj].transform.position,new Vector3(1,0,1));
			transform.rotation = Trot;
			transform.position = Tpos;
		}

	}

public	void cycleNex(){
		curObj++;
		if(curObj >= Objects.Length)
			curObj =0;
		Quaternion Trot = Quaternion.Euler(curAxis[1], curAxis[0], 0);
		Tpos = Trot * new Vector3(0, 0, -distance) + MeshUtilities.vMult (  Objects[curObj].transform.position,new Vector3(1,0,1));
		transform.rotation = Trot;
		transform.position = Tpos;

	}
	public	void cyclePrev(){
		curObj--;
		if(curObj < 0)
			curObj =Objects.Length-1;
		Quaternion Trot = Quaternion.Euler(curAxis[1], curAxis[0], 0);
		Tpos = Trot * new Vector3(0, 0, -distance) + MeshUtilities.vMult (  Objects[curObj].transform.position,new Vector3(1,0,1));
		transform.rotation = Trot;
		transform.position = Tpos;
	}
	public void setMorph(float v)
	{
		Objects[curObj].phase = v;
	}
}
