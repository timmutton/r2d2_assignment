using UnityEngine;
using System.Collections;

public class offensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed, maxDistance = 100.0f, damageAmt;
	private Vector3 startPos;
	
	// Use this for initialization
	void Start() {
		//tag = "spell";
		//start position is used to destroy object after a certain distance
		startPos = transform.position;
		//set velocity in forward direction
		rigidbody.velocity = transform.forward * movementSpeed;
	}
	
	//if it collides with player, apply damage and destroy it
	void OnCollisionEnter(Collision col){
		Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider col){
		var properties = col.GetComponent<PlayerProperties>();
		if(properties != null) {
			properties.Damage(damageAmt);
			Destroy(gameObject);
		}
	}
	
	//if its gone too far without hitting the player, destroy it
	void Update(){
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(this.gameObject);
	}
}
