using UnityEngine;
using System.Collections;

public class playerProperties : MonoBehaviour {	
	public float Health{get; private set;}
	public float minHealth = 0.0f, maxHealth = 100.0f, startHealth = 100.0f;
	
	// Use this for initialization
	void Start () {
		//init health
		Health = startHealth;
	}
	
	private void applyDamage(float damageAmt){
		//aplly damage to health
		Health -= damageAmt;
		//check if dead and act accordingly
		if(Health <= 0)
			playerDeath();
	}
	
	//will need to be updated
	private void playerDeath(){
		Destroy(gameObject);
	}
}
