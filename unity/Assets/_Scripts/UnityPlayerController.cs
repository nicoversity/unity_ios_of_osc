using UnityEngine;
using System.Collections;

public class UnityPlayerController : MonoBehaviour {

	void Start () {
		// display GameObject with green color 
		this.gameObject.GetComponent<Renderer>().material.color = Color.green;
	}
}