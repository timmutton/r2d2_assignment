using UnityEngine;
using System.Collections;

public class offensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed = 10.0f, maxDistance = 100.0f;
	private float damageAmt;
	private Vector3 startPos;
	private SpellProperties properties;
	
	// Use this for initialization
	void Awake() {
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
		
		rigidbody.velocity = transform.forward * movementSpeed;
	}
	
	void OnTriggerEnter(Collider col){
		InteractWithCollider(col);
	}

	private void InteractWithCollider(Collider col) {
//		damageAmt = properties.spellDamage;
		print(col.name);
		print(damageAmt);
		damageAmt = 10.0f;
		if(col.name.ToLower().Contains("player")){
			col.SendMessage("Damage", damageAmt, SendMessageOptions.DontRequireReceiver);
		}else if(col.name.ToLower().Contains("defensivespell")){
			col.BroadcastMessage("Damage", properties, SendMessageOptions.DontRequireReceiver);
		}else{
			return;
		}
		
		var asd = collider.GetComponent<PlayerProperties>();
		if(asd != null) {
			asd.Damage(damageAmt);
		}
	
		Destroy(gameObject);
	}
	
	//if its gone too far without hitting the player, destroy it
	void Update(){		
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(gameObject);
	}
}
