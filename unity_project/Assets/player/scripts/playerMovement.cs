using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float rotationSpeed = 1f, movementSpeed = 1.0f;
	public string inputPrefix = string.Empty;
	private string horizontalAxis = string.Empty, verticalAxis = string.Empty;
	
	// if we've supllied a prefix, get the movement axis'
	void Start(){
		if(inputPrefix == string.Empty)
			Debug.Log("No player input prefix");
		horizontalAxis = inputPrefix + "Hor";
		verticalAxis = inputPrefix + "Vert";
	}
	
	void Update(){
		float rotation = Input.GetAxis(horizontalAxis) * Time.deltaTime * rotationSpeed;
		float direction = Input.GetAxis(verticalAxis) *  movementSpeed;
		
		//rotate player
		transform.Rotate(new Vector3(0, rotation, 0));
		//set velocity in forward direction
		rigidbody.velocity = transform.forward * direction;
	}
}
