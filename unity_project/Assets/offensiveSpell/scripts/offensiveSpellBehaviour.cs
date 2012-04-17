using UnityEngine;
using System.Collections;

public class offensiveSpellBehaviour : MonoBehaviour {
	public float movementSpeed = 10.0f, maxDistance = 100.0f;
	private int damageAmt;
	private Vector3 startPos;
	private SpellProperties properties;
	private CharacterController controller;
//	private Transform parent;
	
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
		controller = GetComponent<CharacterController>();
		properties = GetComponent<SpellProperties>();
		damageAmt = properties.spellDamage;
	}
	
	void OnCollisionEnter(Collision col) {
		this.InteractWithCollider(col.collider);
	}

	void OnTriggerEnter(Collider col) {
		this.InteractWithCollider(col);
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		print("hello");
		this.InteractWithCollider(hit.collider);
	}

	private void InteractWithCollider(Collider col) {
		print("interacting");
		print(col.name.ToLower());
		if(col.name.ToLower().Contains("player")){
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
//		print(parent.transform.forward.ToString());
//		transform.Translate(transform.forward * movementSpeed * Time.deltaTime);
		Vector3 moveDir = transform.TransformDirection(transform.forward) * movementSpeed;
		controller.Move(moveDir * Time.deltaTime);
		
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(gameObject);
		
		/*parent.transform.Translate(parent.transform.forward * movementSpeed * Time.deltaTime);
		
		if(Vector3.Distance(startPos, parent.transform.position) > maxDistance)
			Destroy(parent.gameObject);*/
	}
}
