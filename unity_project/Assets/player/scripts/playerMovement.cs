using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float rotationSpeed = 1f, movementSpeed = 1.0f;
	public bool useKeyboard = false, invertXAxes = false;
	private ClientWiiState state;
	private Transform playerCam;
	
	void Start(){
		state = new ClientWiiState();
		playerCam = transform.FindChild("Main Camera");
	}
	
	void Update(){
		float rotationX = 0.0f, rotationY = 0.0f;
		float directionY = 0.0f, directionX = 0.0f;
		
		if(useKeyboard){
			directionY = Input.GetAxis("moveY");
			directionX = Input.GetAxis("moveX");
			rotationY = Input.GetAxis("lookY");
			rotationX = Input.GetAxis("lookX");
			
		}else{
			directionY = state.ncJoyY;
			directionX = state.ncJoyX;
			
			if(!state.B){
				if(state.Left)
					rotationX = -1.0f;
				if(state.Right)
					rotationX = 1.0f;
				
				if(state.Down)
					rotationY = -1.0f;
				if(state.Up)
					rotationY = 1.0f;
			}
		}
		
		if(!invertXAxes){
			rotationX *= -1.0f;
		}
		
		if(playerCam.rotation.eulerAngles.x > 45 && playerCam.rotation.eulerAngles.x < 180){
			if(rotationX > 0) rotationX = 0;
		}else if(playerCam.rotation.eulerAngles.x < 315 && playerCam.rotation.eulerAngles.x >= 180){
			print(rotationX);
			if(rotationX < 0) rotationX = 0;
		}
				
		//rotate player
//		rotationX * rotationSpeed
		transform.Rotate(new Vector3(0, rotationY * rotationSpeed, 0));
		playerCam.Rotate(new Vector3(rotationX * rotationSpeed, 0, 0));
		//set velocity in forward direction
		rigidbody.AddForce(transform.forward * directionY  *  movementSpeed);
		rigidbody.AddForce(transform.right * directionX  *  movementSpeed);
	}
	
	void updateWiiState(ClientWiiState _state){
		state = _state;
	}
}
