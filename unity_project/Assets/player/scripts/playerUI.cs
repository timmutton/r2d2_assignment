using UnityEngine;
using System.Collections;

public class playerUI : MonoBehaviour {
	public Transform target;
	public Texture2D healthBar, emptyBar;
	public Texture2D Crosshair;

	public float pBarX, pBarY, pBarWidth, pBarHeight;
	
	public Texture2D timerBackground;
	public float pTimerX = 58, pTimerY = -5, pTimerWidth = 40, pTimerHeight = 20;
	private float timerLeft, timerTop, timerWidth, timerHeight;
	
	
	private float barLeft, barTop, barWidth, barHeight;
	private Rect camRect;
	private PlayerProperties myPlayerProperties;
	private float screenWidth, screenHeight;

    public Rect CameraRect {
        get { return this.camRect; }
    }

    void OnGUI(){
		//if(screenWidth != Screen.width && screenHeight != Screen.height)
			updateDimensions();
		
		//draw the health bar
		drawBar(barLeft, barTop, barWidth, barHeight,
			emptyBar, healthBar, myPlayerProperties.maxHealth,
			myPlayerProperties.Health);
		
		//Debug.Log("timerHeight: " + timerHeight + "timerWidth: " + timerWidth);
		DrawTimer (timerLeft, timerTop, timerWidth, timerHeight, gameManager.instance.CurrentTime, timerBackground);

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
		GUI.DrawTexture(new Rect(this.camRect.left + this.camRect.width /2f - size / 2f,
			this.camRect.top + this.camRect.height / 2f - size/2f,
			size, size), this.Crosshair);
	}
	
	private void DrawTimer(float left, float top, float width, float height,
		float time, Texture2D background){
		Debug.Log("timerHeight: " + height + "timerWidth: " + width);
		Rect locationRect = new Rect(left, top, width, height);
		Rect drawRect = new Rect(0, 0, width, height);
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 50;
		style.normal.textColor = Color.white;
		
		Debug.Log("timerHeight: " + height + "timerWidth: " + width);
		
	
  		//Calculates the minutes and seconds
 		int minutes = (int)time / 60;
		int seconds = (int)time % 60;
  		
		//Formats and prints the time to the GUI
		var text = string.Format ("{0:0}:{1:00}", minutes, seconds);	

		
		GUI.BeginGroup (locationRect);
			GUI.BeginGroup(new Rect(0, 0, width, height));
				GUI.DrawTexture(drawRect, background);
			GUI.EndGroup();
			GUI.Label(new Rect(80, 65, width, height), new GUIContent(text), style);
		GUI.EndGroup();
			
	}
	
	//set bar dimentions and pos (by converting size percentage to pixels)
	void updateDimensions(){
		//normalized view port rect
		camRect = camera.pixelRect;
		barLeft = camRect.x + camRect.width * pBarX/100;
		barTop = camRect.height * pBarY/100;
		barWidth = camRect.width * pBarWidth/100;
		barHeight = camRect.height * pBarHeight/100;
		
		timerLeft = camRect.x + camRect.width * pTimerX/100;
		timerTop = camRect.height * pTimerY/100;
		timerWidth = camRect.width * pTimerWidth/100;
		timerHeight = camRect.height * pTimerHeight/100;
		
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
}
