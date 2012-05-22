using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MultiplayerAudioArgs{
	position,
	audioClip
}

public class MultiplayerAudio : MonoBehaviour {
	private GameObject[] players;
	private Vector3 relPosition;
	
	//get players and play audio relative to each of them
	public void PlaySound(Dictionary<int, object> args){
		players = GameObject.FindGameObjectsWithTag("Player");
		
		if(players.Length == 0)
			return;
		
		foreach(GameObject player in players){
			relPosition = transform.position - ((Vector3)args[(int)MultiplayerAudioArgs.position] - player.transform.position);
			AudioSource.PlayClipAtPoint((AudioClip)args[(int)MultiplayerAudioArgs.audioClip], relPosition);
		}
	}
}
