using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WiiReader : MonoBehaviour {
	private WiiUnityClient client;
	private bool connected;
	private bool[] recording = {false, false};
	private List<Vector2>[] points = {new List<Vector2>(), new List<Vector2>()};	
	
	private List<GameObject> players = new List<GameObject>();

	
	// Use this for initialization
	void Start(){
		if(Application.platform == RuntimePlatform.WindowsEditor
			|| Application.platform == RuntimePlatform.WindowsPlayer){
			client = new WiiUnityClient();
			connected = client.StartClient();
		}else{
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!connected)
			return;
		
		ClientWiiState state;
		
		//Get players
		if(players.Count == 0){
			for(int i = 0; i < client.numWiimotes; ++i){
				players.Add(GameObject.Find("player" + (i+1).ToString()));
				client.ToggleIR(i + 1);	
			}
		}
		
		//Get updated wiimote input
		for(int i = 0; i < client.numWiimotes; ++i){
			client.UpdateButtons(i + 1);
			client.UpdateAccel(i + 1);
			client.UpdateNunchuck(i + 1);
			client.UpdateIR(i + 1);
			state = client.GetWiiState(i + 1);
			
			var player = players[i];
			
			//send values to player
			player.SendMessage("updateWiiState",
				state, 
				SendMessageOptions.DontRequireReceiver);
			var cam = player.transform.FindChild("Main Camera");
			cam.SendMessage("updateWiiState",
				state, 
				SendMessageOptions.DontRequireReceiver);
			
			//record input
			if(state.B){
				if(!recording[i]){
					recording[i] = true;
					points[i].Clear();	
				}
				
				points[i].Add(new Vector2( state.ir1PosX, state.ir1PosY));
				print("Point " +  (1 - state.ir1PosX) + " " + (1 - state.ir1PosY));
			}else if(recording[i]){
				recording[i] = false;
				
				//Get guesture from input
				var recognizer = gameObject.AddComponent<HMMRecognizer>();
				var wiiGestures = new WiiGestures();
				var gesture = wiiGestures.GetGestureFromPoints(points[i].ToArray());
				
				try {
					var hmm = recognizer.hmmEvalute(gesture.HmmDirections);
					Debug.Log(string.Format("Recognized gesture: {0}", hmm));
					
					player.SendMessage("handleGesture", hmm, SendMessageOptions.DontRequireReceiver);
				}
				catch (UnityException e) {
					Debug.Log(e);
				}
				
				GameObject.Destroy(recognizer);
			}
		}
	}
	
	void OnApplicationQuit() {
		if (connected)
			client.EndClient();
	}
}

