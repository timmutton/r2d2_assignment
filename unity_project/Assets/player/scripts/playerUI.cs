using UnityEngine;
using System.Collections;

public class playerUI : MonoBehaviour {
	public Transform target;
	public Texture2D healthBar, emptyBar;
	public Texture2D Crosshair;

	public float pBarX, pBarY, pBarWidth, pBarHeight;
	
	private float barLeft, barTop, barWidth, barHeight;
	private Rect camRect;
	private PlayerProperties myPlayerProperties;
	private float screenWidth, screenHeight;

    public Rect CameraRect {
        get { return this.camRect; }
    }

    void OnGUI(){
		if(screenWidth != Screen.width && screenHeight != Screen.height)
			updateDimensions();
		
		//draw the health bar
		drawBar(barLeft, barTop, barWidth, barHeight,
			emptyBar, healthBar, myPlayerProperties.maxHealth,
			myPlayerProperties.Health);

    	this.DrawCrosshair();
    }
	
	void Start(){
		//link to player properties
		myPlayerProperties = target.GetComponent<PlayerProperties>();
		
		updateDimensions();

	}
	
	//draws a bar of given dimensions and pos using a full and empty bar texture
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
		GUI.Label(new Rect(0, 0, barWidth, barHeight), new GUIContent(curValue/maxValue * 100 + "%"));
		GUI.EndGroup();
	}
	
	private void DrawCrosshair() {
		float size = 25f;
		GUI.DrawTexture(new Rect(this.camRect.xMin + this.camRect.width /2f - size / 2f,
			this.camRect.yMin + this.camRect.height / 2f - size/2f,
			size, size), this.Crosshair);
	}
	//set bar dimentions and pos (by converting size percentage to pixels)
	void updateDimensions(){
		//normalized view port rect
		camRect = camera.pixelRect;
		barLeft = camRect.x + camRect.width * pBarX/100;
		barTop = camRect.height * pBarY/100;
		barWidth = camRect.width * pBarWidth/100;
		barHeight = camRect.height * pBarHeight/100;
		
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
}
