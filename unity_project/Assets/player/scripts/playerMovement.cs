using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float rotationSpeed = 1.0f, movementSpeed = 1.0f;
	public string inputPrefix = string.Empty;
	private string horizontalAxis = string.Empty, verticalAxis = string.Empty;
	
	// Use this for initialization
	void Start(){
		if(inputPrefix == string.Empty)
			Debug.Log("No player input prefix");
		horizontalAxis = inputPrefix + "Hor";
		//Debug.Log(horizontalAxis);
		verticalAxis = inputPrefix + "Vert";
		//Debug.Log(verticalAxis);
	}
	
	// Update is called once per frame
	void Update(){
		float rotation = Input.GetAxis(horizontalAxis) * Time.deltaTime * rotationSpeed;
		float direction = Input.GetAxis(verticalAxis) *  movementSpeed;
		
		transform.Rotate(new Vector3(0, rotation, 0) );
		rigidbody.velocity = transform.forward * direction;
	}
}
