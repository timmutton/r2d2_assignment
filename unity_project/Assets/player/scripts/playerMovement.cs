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
	}
	
	void updateWiiState(ClientWiiState _state){
		state = _state;
	}

	public float CurrentMovementSpeed() {
		var haste = this.gameObject.GetComponentInChildren<Haste>();

		return this.movementSpeed * (haste != null ? haste.MovementSpeedMultiplier : 1.0f);
	}
}
