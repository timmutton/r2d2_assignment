using UnityEngine;
using System.Collections;

public class ShowAimer : MonoBehaviour {
	public Texture renderTex;
	private ClientWiiState state = null;
	
	void updateWiiState(ClientWiiState _state){
		state = _state;
	}
	
	//draws texture on screen to show where player is aiming
	void OnGUI(){
		if(state == null)
			return;
		GUI.DrawTexture(new Rect(Screen.width * (1 - state.ir1PosX), Screen.height * state.ir1PosY,
				renderTex.width, renderTex.height), renderTex);
	}
}
