using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M4DLib;

public class S4ExampleSpawner : MonoBehaviour {

    public Bounds4 bounds;
    public Material[] mats;
    public int count = 10;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < count; i++)
        {
            var g = new GameObject("Instance 4D", typeof(SU4Primitive));
            g.transform.SetParent(transform, false);
            g.GetComponent<MeshRenderer>().sharedMaterial = mats[Random.Range(0, mats.Length)];
            g.GetComponent<Transform4>().localPosition = 
                Bounds4.Lerp(bounds, new Vector4(Random.value, Random.value, Random.value, Random.value));
            g.GetComponent<Transform4>().localRotation = new Quaternion4 (
                Quaternion.identity, Random.rotation);

        }
	}
	

}
