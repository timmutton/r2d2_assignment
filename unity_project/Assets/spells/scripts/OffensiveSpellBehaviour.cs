using UnityEngine;
using System.Collections;

public class OffensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed = 10.0f, maxDistance = 100.0f, maxAngle = 10.0f, rotateSpeed = 10.0f;
	
	float damageAmt;
	Vector3 startPos;
	SpellProperties properties;
	Transform target;
	
	// Use this for initialization
	void Start() {		
		//start position is used to destroy object after a certain distance
		startPos = transform.position;
		properties = GetComponent<SpellProperties>();
		
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject gameObj in players){
			if(gameObj.name != properties.parent.name 
				&& Vector3.Angle(transform.forward, (gameObj.transform.position - transform.position)) < maxAngle)
					target = gameObj.transform;
		}
		
		rigidbody.velocity = transform.forward * movementSpeed;
	}
	
	void OnTriggerEnter(Collider col){
		InteractWithCollider(col);
	}

	private void InteractWithCollider(Collider col) {
		/*damageAmt = properties.spellDamage;
		print(damageAmt);*/		
		if(col.name == properties.parent.name)
			return;
		
		if(col.name.ToLower().Contains("player")){
			col.SendMessage("Damage", properties, SendMessageOptions.DontRequireReceiver);
		//}else if(col.name.ToLower().Contains("defensivespell")){
		//	col.BroadcastMessage("Damage", properties, SendMessageOptions.DontRequireReceiver);
		}else{
			return;
		}
		
		/*var asd = collider.GetComponent<PlayerProperties>();
		if(asd != null) {
			asd.Damage(damageAmt);
		}*/
	
		Destroy(gameObject);
	}
	
	//if its gone too far without hitting the player, destroy it
	void Update(){		
		if(target){
			if(Vector3.Distance(startPos, transform.position) > Vector3.Distance(startPos, target.position)){
				target = null;
				return;
			}
			
			Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
    		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
		}
		
//		transform.Translate(transform.forward * movementSpeed * Time.deltaTime);
		rigidbody.velocity = transform.forward * movementSpeed;
		
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(gameObject);
	}
}
