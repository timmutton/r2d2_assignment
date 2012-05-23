using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float rotationSpeed = 1f, movementSpeed = 6.0f, jumpSpeed = 8.0f, gravityForce = 20.0f;
	public bool useKeyboard = false, invertXAxes = false;
	
	private ClientWiiState state;
	private Transform playerCam;
	private CharacterController controller;
	private Vector3 moveDir = Vector3.zero, rotDir = Vector3.zero;
	
	private bool bJump;
	
	void Start(){
		state = new ClientWiiState();
		playerCam = transform.FindChild("Main Camera");
		controller = GetComponent<CharacterController>();
		bJump = true;
	}

	void Update(){
		if(controller.isGrounded){
			//keyboard input for movement
			if(useKeyboard){				
				moveDir = new Vector3(Input.GetAxis("moveX")* 0.7F, 0, Input.GetAxis("moveZ"));
				moveDir = transform.TransformDirection(moveDir) * CurrentMovementSpeed();
				
				if(Input.GetButton("jump")){
					if(bJump){
						moveDir.y = jumpSpeed;
					}
				}
			}else{ //wiimote input for movement
				moveDir = new Vector3(0, 0, 0);
				if(state.Left)
					moveDir.x = -1.0f;
				if(state.Right)
					moveDir.x = 1.0f;
					
				if(state.Down)
					moveDir.z = -1.0f;
				if(state.Up)
					moveDir.z = 1.0f;
				
				moveDir = transform.TransformDirection(moveDir) * CurrentMovementSpeed();
				
				if(state.ncZ){
					moveDir.y = jumpSpeed;
				}
			}
		}else{
			moveDir.y -= gravityForce * Time.deltaTime;
		}
		
		controller.Move(moveDir * Time.deltaTime);
		
		//rotation
		if(useKeyboard){				
			rotDir = new Vector3(Input.GetAxis("lookX"), Input.GetAxis("lookY"), 0);
		}else{
			rotDir = new Vector3((Mathf.Abs(state.ncJoyY) > 0.25)?(state.ncJoyY):0, (Mathf.Abs(state.ncJoyX) > 0.25)?(state.ncJoyX):0, 0);	
		}
		
		if(!invertXAxes){
			rotDir.x *= -1.0f;
		}
		
		
		//camera rotation campling
		if(playerCam.rotation.eulerAngles.x > 45 && playerCam.rotation.eulerAngles.x < 180){
			if(rotDir.x > 0) rotDir.x = 0;
		}else if(playerCam.rotation.eulerAngles.x < 315 && playerCam.rotation.eulerAngles.x >= 180){
			if(rotDir.x < 0) rotDir.x = 0;
		}
		
		//apply movement
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
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.normal.y <= 0.6){
			bJump = false;
		}else{
			bJump = true;
		}
	}
}
