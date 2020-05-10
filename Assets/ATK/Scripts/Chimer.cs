using System.Collections;
using UnityEngine;

public class Chimer : MonoBehaviour {

    WindZone windZone;
    Rigidbody rb;
    Vector3 direction;

	// Use this for initialization
	void Start () {
        windZone = FindObjectOfType<WindZone>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(UpdateDirection());
    }

    IEnumerator UpdateDirection()
    {
        direction = windZone.transform.forward * ((Random.value*windZone.windTurbulence)-windZone.windPulseMagnitude);
        yield return new WaitForSeconds(windZone.windPulseFrequency);
        StartCoroutine(UpdateDirection());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 force = direction * windZone.windMain;
        Debug.Log(force);
        rb.AddForce(force,ForceMode.Acceleration);
	}
}
