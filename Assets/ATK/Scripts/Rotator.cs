using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [SerializeField]
    float rotationSpeed;
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(transform.up,rotationSpeed * Time.deltaTime);
	}
}
