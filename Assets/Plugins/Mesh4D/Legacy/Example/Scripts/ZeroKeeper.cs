using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent (typeof (Slider))]
public class ZeroKeeper : MonoBehaviour
{

	
		// Update is called once per frame
		public void Reback ()
		{
	GetComponent<Slider>().value = 0;
		}
}

