using UnityEngine;
using System.Collections;

public class ShowAimer : MonoBehaviour {
	public Texture renderTex;
	private ClientWiiState state = null;
	
	void updateWiiState(ClientWiiState _state){
		state = _state;
	}
	
	void OnGUI(){
		if(state == null)
			print("Bollocks");
		GUI.DrawTexture(new Rect(Screen.width * (1 - state.ir1PosX), Screen.height * state.ir1PosY,
				renderTex.width, renderTex.height), renderTex);
	}
}
