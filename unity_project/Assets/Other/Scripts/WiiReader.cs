using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected, recording;
	private ArrayList points = new ArrayList();	
	
	// Use this for initialization
	void Awake () {
		client = new WiiUnityClient();
		connected = client.StartClient();
		
		/*if(players.Length != client.numWiimotes){
			print("Number of players and number of wiimotes do not match");
			return;
		}*/
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!connected)
			return;
		
		Vector3 accel;
		ClientWiiState state;
		GameObject player;
			
		for(int i = 0; i < client.numWiimotes; ++i){
			client.UpdateButtons(i + 1);
			client.UpdateAccel(i + 1);
			client.UpdateNunchuck(i + 1);
			state = client.GetWiiState(i + 1);
			accel.x = state.accelX;
			accel.y = state.accelY;
			accel.z = 0;
			
			player = GameObject.Find("player" + i.ToString());
			
			player.SendMessage("updateWiiState",
				state, 
				SendMessageOptions.DontRequireReceiver);
			
			if(state.B){
				if(!recording){
					recording = true;
					points.Clear();
				}
				points.Add(new Vector3(state.accelX, state.accelY, state.accelZ));
			}else if(recording){
				recording = false;
				points.Add(new Vector3(state.accelX, state.accelY, state.accelZ));
				player.SendMessage("printGesture", points, SendMessageOptions.DontRequireReceiver);
				
				/*
				try {
					int gesture = getGesture(points);
					string gestureName = "";
					switch (gesture) {
						case 1: gestureName = "HORIZONTAL_LINE"; break;
						case 2: gestureName = "VERTICAL_LINE"; break;
						case 3: gestureName = "V_UP"; break;
						case 4: gestureName = "V_DOWN"; break;
						case 5: gestureName = "SQUARE"; break;
						default: gestureName = "Unknown gesture..."; break;
					}
					Debug.Log("Gesture: " + gestureName);
				} catch(GestureNotFoundException e) {
					Debug.Log("" + e.Message);
				}
			*/
			}
		}
	}
	
	void OnApplicationQuit() {
		if (connected)
			client.EndClient();
	}
}
