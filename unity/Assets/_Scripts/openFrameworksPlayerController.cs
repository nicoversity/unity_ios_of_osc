using UnityEngine;
using System.Collections;

public class openFrameworksPlayerController : MonoBehaviour {

	// reference to the OSCConnetor GameObject, handling the incoming OSC traffic
	public OSCConnetor oscConnector;	// (set within the Unity editor)

	void Start() {
		// display GameObject with red color 
		this.gameObject.GetComponent<Renderer>().material.color = Color.red;
	}

	void Update () {

		// update GameObject's position based on the received target vector
		// (received via OSC from the iOS openFrameworks application)
		this.transform.position = oscConnector.of_player_controller_target_vector;
	}
}
