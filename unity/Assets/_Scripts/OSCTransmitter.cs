using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCTransmitter : MonoBehaviour {

	// reference to the Unity Player Controller GameObject
	public GameObject unity_player_controller;

	// list of defined OSC addresses
	private string unity_update_position = "/unity/position";


	void Start () {
	
		// initialize handler for outgoing OSC messages (sending)
		// NOTE: Port for outgoing messages has to be set within 
		// Assets/ThirdParty/OSC/OSCHandler.cs, see lines 57-60 and 113-115
		OSCHandler.Instance.Init();
	}
	
	// Update is called once per frame
	void Update () {

		// get the current position of the player in Vector3 format
		Vector3 playerPos = unity_player_controller.transform.position;

		// create a list of values that are going to be send via OSC
		List<object> posValues = new List<object>();

		// set the values of the OSC value list
		// concretely, send 3 values representing the player position's x, y and z coordinates
		posValues.AddRange(new object[]{playerPos.x, playerPos.y, playerPos.z});

		// send the outgoing message
		OSCHandler.Instance.SendMessageToClient(OSCHandler.CLIENT_NAME, unity_update_position, posValues);

		//Debug.Log("Message sent");
	}
}
