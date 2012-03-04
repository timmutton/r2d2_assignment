using UnityEngine;
using System.Collections;

public class playerUI : MonoBehaviour {
	public Texture2D healthBar, emptyBar;
	public Transform target;
	public float pBarX, pBarY, pBarWidth, pBarHeight;
	
	private float barLeft, barTop, barWidth, barHeight;
	private Rect cameraRect;
	private playerProperties myPlayerProperties;
	
	void OnGUI(){
		drawBar(barLeft, barTop, barWidth, barHeight,
			emptyBar, healthBar, myPlayerProperties.maxHealth,
			myPlayerProperties.Health);
	}
	
	void Start(){
		myPlayerProperties = target.GetComponent<playerProperties>();
		cameraRect = camera.pixelRect;
		barLeft = cameraRect.x + cameraRect.width * pBarX/100;
		barTop = cameraRect.y + cameraRect.height * pBarY/100;
		barWidth = cameraRect.width * pBarWidth/100;
		barHeight = cameraRect.height * pBarHeight/100;
	}
	
	private void drawBar(float barLeft, float barTop, float barWidth, float barHeight,
		Texture2D emptyBarTex, Texture2D fullBarTex, float maxValue, float curValue){
		Rect locationRect = new Rect(barLeft, barTop, barWidth, 
			barHeight);
		Rect drawRect = new Rect(0, 0, barWidth, barHeight);
		
		float drawWidth = barWidth * (curValue/maxValue);
		
		GUI.BeginGroup(locationRect);
			GUI.DrawTexture(drawRect, emptyBarTex);
			GUI.BeginGroup(new Rect(0, 0, drawWidth, barHeight));
				GUI.DrawTexture(drawRect, fullBarTex);
			GUI.EndGroup();
			//GUI.Label();
		GUI.EndGroup();
	}
}
