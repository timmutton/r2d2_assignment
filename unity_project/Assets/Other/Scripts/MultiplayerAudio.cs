using UnityEngine;
using System.Collections;

public class MultiplayerAudio : MonoBehaviour {
	private GameObject[] players;
	private Vector3 relPosition;
	
	void PlaySound(AudioClip clip){
		players = GameObject.FindGameObjectsWithTag("Player");
		
		if(players.Length == 0)
			return;
		
		foreach(GameObject player in players){
			relPosition = this.transform.position - player.transform.position;
			AudioSource.PlayClipAtPoint(clip, relPosition);
		}
	}
}
