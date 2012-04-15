using UnityEngine;
using System.Collections;


public class gameManager : MonoBehaviour {
	public float roundTime = 180.0f;
	public int maxRounds = 3;
	private int currentRound;
	private float currentTime;
	
	public GameObject player;
	public GameObject spawn1;
	public GameObject spawn2;
	private GameObject Player2_inst;
	private GameObject Player1_inst;
	private PlayerProperties Player1_properties;
	private PlayerProperties Player2_properties;
	

	public bool displayRound = true;
	public GUIStyle messageStyle;
	public int messageWidth = 400;
	public int messageHeight = 80;
	
	public int round2Boost = 30;
	public int round3Boost = 20;
	
	// Use this for initialization
	void Start () {
		//Instantiates the prefabs of each player
		Player2_inst = (GameObject)Instantiate(player, spawn2.transform.position, spawn2.transform.rotation);
		Player1_inst = (GameObject)Instantiate(player, spawn1.transform.position, spawn1.transform.rotation);
		
		//Sets variables relevant to the individual characters. eg. camera position, player name
		GameObject.Find("Main Camera").GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
		Player1_inst.name = "player1";		
		Player2_inst.name = "player2";
		
		//Caches the players properties
		Player1_properties = (PlayerProperties)Player1_inst.GetComponent<PlayerProperties>();
		Player2_properties = (PlayerProperties)Player2_inst.GetComponent<PlayerProperties>();
		
		currentRound = 1;
		currentTime = roundTime;
	}
	
	// Update is called once per frame
	void Update () {
		int minutes;
		int seconds;
		
		//Pre round upkeeping
		if(currentTime == roundTime){
			PreRound();
		}
		
		//Win condition if one of the players had 0 health
		if(Player1_properties.Health <= 0 || Player2_properties.Health <= 0){
			//Turns off the time and rounds
			currentTime = 0;
			currentRound = maxRounds;
			
			if(Player1_inst.GetComponent<PlayerProperties>().Health <= 0){
				Debug.Log("Player 1 DEAD");
			}else if(Player2_inst.GetComponent<PlayerProperties>().Health <= 0){
				Debug.Log ("Player 2 DEAD");
			}
		}

		//Condition if all the rounds are over and is finished
		if(currentRound == maxRounds){
			Debug.Log("GAME OVER");
		}
		
		//Decrements the round timer and the remaining number of rounds
		if(currentTime > 0 && currentRound <= maxRounds){
    		currentTime -= Time.deltaTime;
		}else if(currentTime <= 0 && maxRounds > 0){
			currentRound++;
			currentTime = roundTime;
			
			//Resets player positions to spawns
			Player1_inst.transform.position = spawn1.transform.position;			
			Player2_inst.transform.position = spawn2.transform.position;
		}
		
	
  		//Calculates the minutes and seconds
 		minutes = (int)currentTime / 60;
		seconds = (int)currentTime % 60;
  		
		//Formats and prints the time to the GUI
		var text = string.Format ("{0:0}:{1:00}", minutes, seconds);	
		guiText.text = "Time: "+text;	
	}
	
	void OnGUI () {
		/*
		if(displayRound){
			GUI.Label(new Rect((Screen.width - messageWidth)/2.0F, (Screen.height - messageHeight)/2.0F, messageWidth, messageHeight), "ROUND 1", messageStyle);
		}*/
	}	
	
	void PreRound(){
		//Gives health boosts at the start of the round
		if(currentRound == 2){
			Player1_properties.Heal(Player1_properties.Health * (round2Boost/100.0F));
			Player2_properties.Heal(Player2_properties.Health * (round2Boost/100.0F));
		}else if(currentRound == 3){
			Player1_properties.Heal(Player1_properties.Health * (round3Boost/100.0F));
			Player2_properties.Heal(Player2_properties.Health * (round3Boost/100.0F));			
		}
	}
	
	private void PostRound(){
		
	}
	
	
}
