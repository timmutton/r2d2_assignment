using UnityEngine;
using System.Collections;

public class spellBehaviour : MonoBehaviour {
	public float movementSpeed;
	public float maxDistance = 100.0f;
	private Vector3 startPos;
	
	// Use this for initialization
	void Start() {
		//tag = "spell";
		rigidbody.velocity = transform.forward * movementSpeed;
		startPos = transform.position;
	}
	
	void OnCollisionEnter(Collision col){
		if(col.collider.tag == "player")
			Debug.Log("Hit Player");
		Destroy(gameObject);
	}
	
	void Update(){
		if(Vector3.Distance(startPos, transform.position) > maxDistance)
			Destroy(this.gameObject);
	}
}
