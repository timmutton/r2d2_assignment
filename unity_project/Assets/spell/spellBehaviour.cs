using UnityEngine;
using System.Collections;

public class spellBehaviour : MonoBehaviour {
	public float movementSpeed, maxDistance = 100.0f, damageAmt;
	private Vector3 startPos;
	
	// Use this for initialization
	void Start() {
		//tag = "spell";
		rigidbody.velocity = transform.forward * movementSpeed;
		startPos = transform.position;
	}
	
	void OnCollisionEnter(Collision col){
		Debug.Log(col.gameObject.name);
		if(col.gameObject.tag == "Player")
			col.gameObject.SendMessage("applyDamage", damageAmt);
		Destroy(gameObject);
	}
	
	void Update(){
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(this.gameObject);
	}
}
