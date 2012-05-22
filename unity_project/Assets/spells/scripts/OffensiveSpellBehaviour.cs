using UnityEngine;

public class OffensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed = 10.0f, maxDistance = 10000.0f, maxAngle = 10.0f, rotateSpeed = 10.0f;
	
	float damageAmt;
	Vector3 startPos;
	SpellProperties properties;
	Transform target;
	public AudioClip hitSound;
	
	// Use this for initialization
	void Start() {		
		//start position is used to destroy object after a certain Distance
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
	
	//send damage message to player
	private void InteractWithCollider(Collider col) {
		if (col.name != properties.parent.name) {
			if (this.hitSound != null) {
				AudioUtil.PlaySound(this.hitSound, this.gameObject.transform.position);
			}

			if (col.name.ToLower().Contains("player")) {
				col.SendMessage("Damage", properties, SendMessageOptions.DontRequireReceiver);
			}

			Destroy(gameObject);
		}
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
		
		rigidbody.velocity = transform.forward * movementSpeed;
		
		if(Vector3.Distance(startPos, transform.position) > maxDistance){
			Debug.Log("TOO FAR: " + maxDistance);
			Destroy(gameObject);
		}
		
		
		Debug.Log("MAX DISTANCE: " + maxDistance);
	}
}
