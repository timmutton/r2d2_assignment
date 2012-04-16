using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected;
	
	// Use this for initialization
	void Awake () {
		client = new WiiUnityClient();
		connected = client.StartClient();
		
		print("arraylist of vec3's for acceleration");
		
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
				player.SendMessage("getGesture",
				accel, 
				SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnApplicationQuit() {
		if (connected)
			client.EndClient();
	}
}
