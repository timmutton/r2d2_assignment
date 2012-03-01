using UnityEngine;
using System.Collections;

public class playerUI : MonoBehaviour {
	public Texture2D healthBar, emptyBar;
	public Transform target;
	public float offsetX, offsetY;
	
	private playerProperties myPlayerProperties;
	// Use this for initialization
	void OnGUI(){
		Rect cameraRect = camera.pixelRect;
		drawBar(cameraRect.x + offsetX, cameraRect.y + offsetY, emptyBar, healthBar, 
			myPlayerProperties.maxHealth, myPlayerProperties.Health);
	}
	
	void Start(){
		myPlayerProperties = target.GetComponent<playerProperties>();
	}
	
	private void drawBar(float barLeft, float barTop, Texture2D emptyBarTex,
			Texture2D fullBarTex, float maxValue, float curValue){
		Rect locationRect = new Rect(barLeft, barTop, emptyBarTex.width, 
			emptyBarTex.height);
		Rect drawRect = new Rect(0, 0, emptyBarTex.width, emptyBarTex.height);
		
		float drawWidth = emptyBarTex.width * (curValue/maxValue);
		
		GUI.BeginGroup(locationRect);
			GUI.DrawTexture(drawRect, emptyBarTex);
			GUI.BeginGroup(new Rect(0, 0, drawWidth, emptyBarTex.height));
				GUI.DrawTexture(drawRect, fullBarTex);
			GUI.EndGroup();
			//GUI.Label();
		GUI.EndGroup();
	}
}
