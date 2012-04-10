using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct movement{
	public float x, y;
}

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected;
	
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
		
		movement mv;
		ClientWiiState state;
		GameObject player;
			
		for(int i = 0; i < client.numWiimotes; ++i){
			client.UpdateButtons(i + 1);
			client.UpdateAccel(i + 1);
			client.UpdateNunchuck(i + 1);
			state = client.GetWiiState(i + 1);
			mv.x = state.accelX;
			mv.y = state.accelY;
			
			player = GameObject.Find("player" + i.ToString());
			
			player.SendMessage("updateWiiState",
				state, 
				SendMessageOptions.DontRequireReceiver);
			
			print("update function name");
			if(state.B){
				player.SendMessage("functionName",
				mv, 
				SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnApplicationQuit() {
		if (connected)
			client.EndClient();
	}
}
