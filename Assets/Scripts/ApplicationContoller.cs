using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationContoller : MonoBehaviour {
	/* This class just "listens" for the ESC key and if it is pressed it exits/quits the application.
	This will not work in the editor, it will work only while a build is running.*/

	// Use this for initialization
	void Start () {
		// nothing is needed here
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}
