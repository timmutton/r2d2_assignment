using UnityEngine;
using System;

public class PlayerProperties : MonoBehaviour {
    [SerializeField]
	private float health;
	
	public float Health {
		get { return this.health; }
		private set {
			this.health = value;
			
			//check if dead and act accordingly
			if(this.health <= 0) {
				this.playerDeath();
			}
		}
	}

	public float minHealth = 0.0f, maxHealth = 100.0f, startHealth = 100.0f;
	
	public event EventHandler Death;
	
	private void OnDeath() {
		var handler = this.Death;
		if(handler != null) {
			handler(this, EventArgs.Empty);
		}
	}


    // Use this for initialization
	void Start () {
		//init health
		this.Health = startHealth;
	}
	
	public void Damage(float hp){
		//aplly damage to health
		this.Health -= hp;
	}
	
	public void Heal(float hp) {
		this.Health = Math.Min(this.Health + hp, this.maxHealth);
	}
	
	//will need to be updated
	private void playerDeath(){
		this.OnDeath();
		Destroy(gameObject);
	}

    public void Update() {
       
    }
}
