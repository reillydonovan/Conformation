using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M4DLib;

public class M4ExampleDriver : MonoBehaviour {

    M4Viewer view;
    public float speed;
	// Use this for initialization
	void Start () {
		view = GetComponent<M4Viewer>();
	}
	
	// Update is called once per frame
	void Update () {
        view.rotation *= Quaternion4.Euler(0, Input.GetAxis("Horizontal") * speed, 0, 
            0, 0, Input.GetAxis("Vertical") * speed);

	}
}
