using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float rotationSpeed = 1f, movementSpeed = 1.0f, gravityForce = 20.0f;
	public bool useKeyboard = false, invertXAxes = false;
	
	private ClientWiiState state;
	private Transform playerCam;
	private CharacterController controller;
	private Vector3 moveDir, rotDir;
	
	void Start(){
		state = new ClientWiiState();
		playerCam = transform.FindChild("Main Camera");
		controller = GetComponent<CharacterController>();
	}
	
<<<<<<< HEAD
	void FixedUpdate(){
		if(useKeyboard){
			moveDir = new Vector3(Input.GetAxis("moveX"), 0, Input.GetAxis("moveZ"));
			rotDir = new Vector3(Input.GetAxis("lookX"), Input.GetAxis("lookY"), 0);
		}else{
			moveDir = new Vector3(state.ncJoyX, 0, state.ncJoyY);
			if(!state.B){
				if(state.Left)
					rotDir.x = -1.0f;
				if(state.Right)
					rotDir.y = 1.0f;
				
				if(state.Down)
					rotDir.y = -1.0f;
				if(state.Up)
					rotDir.y = 1.0f;
			}
		}
		
		moveDir = transform.TransformDirection(moveDir) * movementSpeed;
		moveDir.y = controller.velocity.y - gravityForce * Time.deltaTime;
		controller.Move(moveDir * Time.deltaTime);
		
		if(!invertXAxes){
			rotDir.x *= -1.0f;
		}
		
		if(playerCam.rotation.eulerAngles.x > 45 && playerCam.rotation.eulerAngles.x < 180){
			if(rotDir.x > 0) rotDir.x = 0;
		}else if(playerCam.rotation.eulerAngles.x < 315 && playerCam.rotation.eulerAngles.x >= 180){
			if(rotDir.x < 0) rotDir.x = 0;
		}
		
		transform.Rotate(new Vector3(0, rotDir.y * rotationSpeed, 0));
		playerCam.Rotate(new Vector3(rotDir.x * rotationSpeed, 0, 0));
		
		/*float rotationX = 0.0f, rotationY = 0.0f;
=======
	void Update() {
		var speed = this.CurrentMovementSpeed();
		float rotationX = 0.0f, rotationY = 0.0f;
>>>>>>> 26d3d28950f8a0e0ca4f7eca95c00fcc929dea18
		float directionY = 0.0f, directionX = 0.0f;
		
		if(useKeyboard){
			directionY = Input.GetAxis("moveY");
			directionX = Input.GetAxis("moveX");
			rotationY = Input.GetAxis("lookY");
			rotationX = Input.GetAxis("lookX");
			
		}else{
			directionY = state.ncJoyY;
			directionX = state.ncJoyX;
			
<<<<<<< HEAD
			//print("update to use IR sensor");
=======
>>>>>>> 26d3d28950f8a0e0ca4f7eca95c00fcc929dea18
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
<<<<<<< HEAD
		rigidbody.AddForce(transform.forward * directionY  *  movementSpeed);
		rigidbody.AddForce(transform.right * directionX  *  movementSpeed);*/
=======
		rigidbody.AddForce(transform.forward * directionY * speed);
		rigidbody.AddForce(transform.right * directionX * speed);
>>>>>>> 26d3d28950f8a0e0ca4f7eca95c00fcc929dea18
	}
	
	void updateWiiState(ClientWiiState _state){
		state = _state;
	}

	public float CurrentMovementSpeed() {
		var haste = this.gameObject.GetComponentInChildren<Haste>();

		return this.movementSpeed * (haste != null ? haste.MovementSpeedMultiplier : 1.0f);
	}
}
