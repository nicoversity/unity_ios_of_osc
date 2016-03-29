using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using VVVV_OSC;

public class OSCConnetor : MonoBehaviour {

	// specify OSC port for incoming messages (listening)
	private int OSC_PORT = 30001;	// == iOS outgoing port

	// public references to GameObjects representing the openFrameworks Player Controller
	// and its target vector (for updating the GameObjects position accordingly)
	public GameObject of_player_controller;		// (set within the Unity editor)		
	public Vector3 of_player_controller_target_vector;

	// list of defined OSC addresses
	private string OF_update_position = "/openFrameworks/position";
	private string OF_heartbeat = "/misc/heartbeat";

	// thread and receiver for handling incoming OSC messages
	private Thread thread;
	private OSCReceiver oscin;


	void Start () {
		// setup OSC receiver to listening at the specified port
		oscin = new OSCReceiver( OSC_PORT );

		// setup and start thread for handling incoming OSC messages
		thread = new Thread( new ThreadStart( UpdateOSC ) );
		thread.Start();

		// initialize GameObject's target vector
		of_player_controller_target_vector = new Vector3(0f, 0f, 0f);
	}


	void OnApplicationQuit() {
		oscin.Close ();
		thread.Interrupt ();
		if( !thread.Join(2000) ) { 
			thread.Abort();
		}
	}


	void UpdateOSC() {
		while( true ) {
			OSCPacket msg = oscin.Receive ();
			if ( msg != null ) {
				if ( msg.IsBundle() ) {
					OSCBundle b = (OSCBundle) msg;
					foreach( OSCPacket subm in b.Values ) {
						parseMessage( subm );
					}
				} else {
					parseMessage( msg );
				}
			}
			Thread.Sleep( 5 );
		}
	}


	void parseMessage(OSCPacket packet){

		// react to incoming messages according their the OSC address
		//

		// heartbeat
		if(packet.Address.Equals(OF_heartbeat)){
			Debug.Log("heart beat at time = " + (int)packet.Values[0]);
		}

		// update position
		else if(packet.Address.Equals(OF_update_position)){

			// extract OSC message's values
			// (as int values)
			int x = (int)packet.Values[0];
			int y = (int)packet.Values[1];

			//Debug.Log("Position updated to x|y = " + x + "|" + y);

			// update the position of the openFrameworks player controller GameObject
			updatePositionOfOpenFrameworksPlayerController(x, y);
		}
	}

	// function to update the position of the openFrameworks pplayer controller GameObject
	void updatePositionOfOpenFrameworksPlayerController(int x, int y)
	{
		// check if GameObject was assigned (within the Unity Editor)
		if(of_player_controller != null)
		{
			// update position of the shared public property
			// normalization to smaller Unity units: multiply by 0.01f
			// iOS y coordinate == Unity z coordinate
			of_player_controller_target_vector = new Vector3(x * 0.01f, 0.0f, y * 0.01f);	
		}
	}
}
