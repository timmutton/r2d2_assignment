using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerNotFoundException : Exception {
	public PlayerNotFoundException(string message) : base(message) {
	}
}

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected, recording;
	private ArrayList points = new ArrayList();	
	TextWriter tw;
	
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
//				print(state.accelX);
//				print(state.accelY);	
				
				points.Add(new Vector3(state.accelX, state.accelY, 0));
				print(new Vector3(state.accelX, state.accelY, 0).ToString());
				
			}else if(recording){
				recording = false;
				points.Add(new Vector3(state.accelX, state.accelY, 0));
				//print(new Vector3(state.accelX, state.accelY, 0).ToString());
				tw = new StreamWriter("square.txt", true);
				tw.WriteLine(new Vector3(state.accelX, state.accelY, 0).ToString());
				//print("123");
				//tw = new StreamWriter("square.txt", true);
				/*
				string [] str = new string [points.Count];
				int counter = 0;
				foreach(Vector3 point in points){
					str[counter] = ("(" + point.x + "," + point.y + ")");
					counter++;
				}
				File.WriteAllLines("square.txt", str);
				*/
				
				//print(str);
				
				tw.Close();
	
	            // close the stream
	            
				
				/*int gesture = GestureRecognizer.getGestureFromPoints(points);
				print(gesture);*/
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

