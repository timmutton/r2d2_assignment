using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	public float roundTime = 180.0f;
	public int maxRounds = 3;
	public GameObject player;
	public GameObject spawn1;
	public GameObject spawn2;
	private float currentTime;

	// Use this for initialization
	void Start () {
		//Instantiates the prefabs of each player
		GameObject Player2_inst = (GameObject)Instantiate(player, spawn2.transform.position, spawn2.transform.rotation);
		GameObject Player1_inst = (GameObject)Instantiate(player, spawn1.transform.position, spawn1.transform.rotation);
		
		//Sets variables relevant to the individual characters. eg. camera position, player name
		Player2_inst.GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
//		Player2_inst.GetComponent<playerMovement>().inputPrefix = "player2";
		Player1_inst.name = "player1";		
		Player2_inst.name = "player2";

		Player1_inst.GetComponentInChildren<playerMovement>().useKeyboard = true;
		
		currentTime = roundTime;
	}
	
	// Update is called once per frame
	void Update () {
		int minutes;
		int seconds;
		
		//Decrements the round timer and the remaining number of rounds
		if(currentTime > 0 && maxRounds > 0){
    		currentTime -= Time.deltaTime;
		}else if(currentTime <= 0 && maxRounds > 0){
			--maxRounds;
			currentTime = roundTime;
		}
		
		//Condition if all the rounds are over and is finished
		if(maxRounds == 0){
			Debug.Log("GAME OVER");	
		}
		
		
  		//Calculates the minutes and seconds
 		minutes = (int)currentTime / 60;
		seconds = (int)currentTime % 60;
  		
		//Formats and prints the time to the GUI
		var text = string.Format ("{0:0}:{1:00}", minutes, seconds);	
		guiText.text = "Time: "+text;	
	}
}
