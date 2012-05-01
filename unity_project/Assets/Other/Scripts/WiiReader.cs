using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNotFoundException : Exception {
	public PlayerNotFoundException(string message) : base(message) {
	}
}

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected, recording;
	private ArrayList points = new ArrayList();	
	
	// Use this for initialization
	void Start(){
		if(Application.platform == RuntimePlatform.WindowsEditor
			|| Application.platform == RuntimePlatform.WindowsPlayer){
			client = new WiiUnityClient();
			connected = client.StartClient();
		}else{
			Destroy(this);
		}
		
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

		this.UpdatePlayers();
			
		for(int i = 0; i < client.numWiimotes; ++i){
			client.UpdateButtons(i + 1);
			client.UpdateAccel(i + 1);
			client.UpdateNunchuck(i + 1);
			state = client.GetWiiState(i + 1);
			accel.x = state.accelX;
			accel.y = state.accelY;
			accel.z = 0;
			
			player = GameObject.Find("player" + (i+1).ToString());
			
			player.SendMessage("updateWiiState",
				state, 
				SendMessageOptions.DontRequireReceiver);
			
			if(state.B){
				if(!recording){
					recording = true;
					points.Clear();
				}
				print(state.accelX);
				print(state.accelY);
				
				points.Add(new Vector3(state.accelX, state.accelY, 0));
			}else if(recording){
				recording = false;
				points.Add(new Vector3(state.accelX, state.accelY, 0));
				int gesture = GestureRecognizer.getGesture(points);
				print(gesture);
				
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

	private void UpdatePlayers() {
		for(int i = 0; i < this.client.numWiimotes; ++i) {
			this.UpdatePlayer(i + 1);
		}
	}

	private void UpdatePlayer(int oneBasedPlayerNumber) {
		try {
			var player = this.GetPlayer(oneBasedPlayerNumber);
			var movement = player.GetComponentInChildren<playerMovement>();
			movement.useKeyboard = false;
		}
		catch(PlayerNotFoundException e) {
			Debug.Log(e);
		}
	}

	private GameObject GetPlayer(int oneBasedPlayerNumber) {
		var player = GameObject.Find(string.Format("player{0}", oneBasedPlayerNumber));
		if(player == null) {
			throw new PlayerNotFoundException(string.Format("Player {0} not found", oneBasedPlayerNumber));
		}
		return player;
	}
	
	void OnApplicationQuit() {
		if (connected)
			client.EndClient();
	}
}
