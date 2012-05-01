using UnityEngine;
using System.Collections;

	
enum Outcome{ 
	draw,
	player1, 
	player2,
};
	

public class gameManager : MonoBehaviour {
	public static gameManager instance;
	
	public float roundTime = 180.0f;
	public int maxRounds = 3;
	private int currentRound;
	private float _currentTime;
	private bool bGameOver;
	private Outcome winner;

	public GameObject player;
	public GameObject spawn1;
	public GameObject spawn2;
	private GameObject Player2_inst;
	private GameObject Player1_inst;
	private PlayerProperties Player1_properties;
	private PlayerProperties Player2_properties;
	

	public bool bDisplayRound = true;
	public GUIStyle messageStyle;
	public int messageWidth = 400;
	public int messageHeight = 80;
	public int messageTime = 3;
	private string endMessage;
	
	public float CurrentTime{
		get { return _currentTime;}
		set { _currentTime = value;}
	} 
	
	public int round2Boost = 30;
	public int round3Boost = 20;
	
	void Awake(){
		gameManager.instance = this;
	}
	
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

		var player2Movement = Player2_inst.GetComponentInChildren<playerMovement>();
		player2Movement.useKeyboard = true;
		
		currentRound = 1;
		_currentTime = roundTime;
		bGameOver = false;
		
//		Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
		int minutes;
		int seconds;
		
		if(!bGameOver){
			//Pre round upkeeping
			if(_currentTime == roundTime){
				PreRound();
			}
		
			//Win condition if one of the players has 0 health
			if(Player1_properties.Health <= 0 || Player2_properties.Health <= 0){
				//Turns off the time and rounds
				_currentTime = 0;
				currentRound = maxRounds;
			
				//Player 1 dies, player 2 wins
				if(Player1_properties.Health <= 0){
					Debug.Log("Player 1 DEAD");
					Debug.Log("Player 2 Wins");
					bGameOver = true;
					winner = Outcome.player2;
					EndGame();
					return;
				//Player 2 dies, player 1 wins
				}else if(Player2_properties.Health <= 0){
					Debug.Log ("Player 2 DEAD");
					Debug.Log("Player 1 Wins");
					bGameOver = true;
					winner = Outcome.player1;
					EndGame();
					return;
				}
			}

			//Condition if all the rounds are over and is finished
			if(currentRound == maxRounds + 1){
				//Player 1 wins
				if(Player1_properties.Health > Player2_properties.Health){
					Debug.Log("Player 1 Wins");
					winner = Outcome.player1;
				//Player 2 wins
				}else if(Player2_properties.Health > Player1_properties.Health){
					Debug.Log("Player 2 Wins");	
					winner = Outcome.player2;
				//Draw
				}else{
					Debug.Log("Draw");
					winner = Outcome.draw;
				}
				Debug.Log("GAME OVER");	
				bGameOver = true;
				EndGame();
				return;
			}
		

			//Decrements the round timer and the remaining number of rounds
			if(_currentTime > 0 && currentRound <= maxRounds){
    			_currentTime -= Time.deltaTime;
			}else if(_currentTime <= 0){
				currentRound++;
				_currentTime = roundTime;
			
				//Resets player positions to spawns
				Player1_inst.transform.position = spawn1.transform.position;			
				Player2_inst.transform.position = spawn2.transform.position;
			}
		}
		
		//Turn off the player movement when gameover
		/*if(bGameOver){
//			Debug.Log("PLAYER OFF");
			Player1_inst.GetComponent<playerMovement>().enabled = false;
			Player2_inst.GetComponent<playerMovement>().enabled = false;
			Player1_inst.GetComponent<playerAttack>().enabled = false;
			Player2_inst.GetComponent<playerAttack>().enabled = false;
		}*/
	}
	
	void OnGUI () {
		if(bDisplayRound){
			if(_currentTime >= roundTime - messageTime && currentRound <= maxRounds){
				GUI.Label(new Rect((Screen.width - messageWidth)/2.0F, (Screen.height - messageHeight)/2.0F, messageWidth, messageHeight), "ROUND " + currentRound, messageStyle);
			}
		}
		if(bGameOver){
			if(winner == Outcome.player1)
				endMessage = "GAMEOVER\nPLAYER 1 WINS";
			else if(winner == Outcome.player2)
				endMessage = "GAMEOVER\nPLAYER 2 WINS";
			else
				endMessage = "GAMEOVER\nDRAW";
			GUI.Label(new Rect((Screen.width - messageWidth)/2.0F, (Screen.height - messageHeight)/2.0F, messageWidth, messageHeight), endMessage, messageStyle);			
		}
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
	
	private void EndGame(){
//		Debug.Log("POST ROUND");
		if(bGameOver){
			Time.timeScale = 0.0f;
//			Debug.Log("PLAYER OFF");
//			Player1_inst.GetComponent<playerMovement>().enabled = false;
//			Player2_inst.GetComponent<playerMovement>().enabled = false;
		}
	}
	
	
}