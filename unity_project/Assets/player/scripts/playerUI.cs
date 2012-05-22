using UnityEngine;
using System.Collections;

public class playerUI : MonoBehaviour {
	public Transform target;
	public Texture2D healthBar, emptyBar;
	public Texture2D Crosshair;
	
	public Font font;
	public float x,y,z;

	public Texture2D healthBackground;
	public float pBarX, pBarY, pBarWidth, pBarHeight;
	
	public Texture2D timerBackground;
	public float pTimerX = 58, pTimerY = -5, pTimerWidth = 40, pTimerHeight = 20;
	private float timerLeft, timerTop, timerWidth, timerHeight;
	
	public Texture2D damageTex;
	public float damageDuration;
	private bool _bDrawDamage;
	
	
	private float barLeft, barTop, barWidth, barHeight;
	private Rect camRect;
	private PlayerProperties myPlayerProperties;
	private float screenWidth, screenHeight;

    public Rect CameraRect {
        get { return this.camRect; }
    }
	
	public bool bDrawDamage{
		set {_bDrawDamage = value;}
	}

    void OnGUI(){
		//if(screenWidth != Screen.width && screenHeight != Screen.height)
			updateDimensions();
		
		//draw the health bar
		drawBar(barLeft, barTop, barWidth, barHeight,
			emptyBar, healthBar, myPlayerProperties.maxHealth,
			myPlayerProperties.Health, healthBackground);
		
		//Draws the timer
		DrawTimer (timerLeft, timerTop, timerWidth, timerHeight, gameManager.instance.CurrentTime, timerBackground);
		
		//Draw damage indicator
		StartCoroutine(DrawDamage ());
		
    	this.DrawCrosshair();
    }
	
	void Start(){
		//link to player properties
		myPlayerProperties = target.GetComponent<PlayerProperties>();
		_bDrawDamage = false;
		
		updateDimensions();

	}
	
	//Draws the health HUD
	private void drawBar(float barLeft, float barTop, float barWidth, float barHeight,
		Texture2D emptyBarTex, Texture2D fullBarTex, float maxValue, float curValue, Texture2D background){
		Rect locationRect = new Rect(barLeft, barTop, barWidth, 
			barHeight);
		Rect drawRect = new Rect(0, 0, barWidth, barHeight);
		
		//Creating a new style
		GUIStyle style = new GUIStyle();
		style.fontSize = Screen.width/84;
		style.normal.textColor = Color.white;
		style.font = font;
		
		
		//Grouping the texture and text elements of the health HUD and displays
		GUI.BeginGroup(locationRect);
			GUI.BeginGroup(new Rect(0, 0, barWidth, barHeight));
				GUI.DrawTexture(drawRect, background);
			GUI.EndGroup();
			GUI.Label(new Rect(Screen.width/16, Screen.height/27, barWidth, barHeight), new GUIContent("Health"), style);
		style.fontSize = Screen.width/18;
		style.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(-Screen.width/168, Screen.height/30, barWidth, barHeight), new GUIContent((curValue/maxValue * 100).ToString()), style);
		GUI.EndGroup();
	}
	
	private void DrawCrosshair() {
		float size = 25f;
		GUI.DrawTexture(new Rect(this.camRect.xMin + this.camRect.width /2f - size / 2f,
			this.camRect.yMin + this.camRect.height / 2f - size/2f,
			size, size), this.Crosshair);
	}
	
	private void DrawTimer(float left, float top, float width, float height,
		float time, Texture2D background){
		Rect locationRect = new Rect(left, top, width, height);
		Rect drawRect = new Rect(0, 0, width, height);
		
		
		//Creating a style
		GUIStyle style = new GUIStyle();
		style.fontSize = Screen.width/28;
		style.normal.textColor = Color.white;
		style.font = font;
		
		
	
  		//Calculates the minutes and seconds
 		int minutes = (int)time / 60;
		int seconds = (int)time % 60;
  		
		//Formats and prints the time to the GUI
		var text = string.Format ("{0:0}:{1:00}", minutes, seconds);	

		//Groups the timer elements and display them
		GUI.BeginGroup (locationRect);
			GUI.BeginGroup(new Rect(0, 0, width, height));
				GUI.DrawTexture(drawRect, background);
			GUI.EndGroup();
			GUI.Label(new Rect(Screen.width/20, Screen.height/15, width, height), new GUIContent(text), style);
		GUI.EndGroup();
			
	}
	
	//Draws the red damage indicator around the player window upon taking damage
	IEnumerator DrawDamage(){
		
		//Checks if the player has been damaged and draws indicator
		if(_bDrawDamage){
			GUI.DrawTexture(new Rect(camRect.x, camRect.y, camRect.width, camRect.height), damageTex);
			yield return new WaitForSeconds(damageDuration);
			_bDrawDamage = false;
		}
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
