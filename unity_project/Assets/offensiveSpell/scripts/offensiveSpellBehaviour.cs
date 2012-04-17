using UnityEngine;
using System.Collections;

public class offensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed = 10.0f, maxDistance = 100.0f;
	private int damageAmt;
	private Vector3 startPos;
	private SpellProperties properties;
	
	// Use this for initialization
	void Start() {
		/*parent = transform.parent;
		//tag = "spell";
		//start position is used to destroy object after a certain distance
		startPos = parent.transform.position;
		//set velocity in forward direction
		//rigidbody.velocity = transform.forward * movementSpeed;
		properties = parent.GetComponent<SpellProperties>();
		damageAmt = properties.spellDamage;*/
		
		//start position is used to destroy object after a certain distance
		startPos = transform.position;
		properties = GetComponent<SpellProperties>();
		damageAmt = properties.spellDamage;
		
		rigidbody.velocity = transform.forward * movementSpeed;
	}
	
	void OnTriggerEnter(Collider col){
		InteractWithCollider(col);
	}

	private void InteractWithCollider(Collider col) {
		//print(col.name.ToLower());
		if(col.name.ToLower().Contains("player")){
			print("sending to player");
			col.SendMessage("Damage", damageAmt, SendMessageOptions.DontRequireReceiver);
		}else if(col.name.ToLower().Contains("spell")){
			col.BroadcastMessage("Damage", properties, SendMessageOptions.DontRequireReceiver);
		}
		/*var properties = collider.GetComponent<PlayerProperties>();
		if(properties != null) {
			properties.Damage(damageAmt);
		}*/
	
		Destroy(gameObject);
	}
	
	//if its gone too far without hitting the player, destroy it
	void Update(){		
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(gameObject);
	}
}
