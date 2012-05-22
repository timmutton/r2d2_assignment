using UnityEngine;

/// <summary>
/// Base class for player modifier/powerup
/// </summary>
class BaseModifier : MonoBehaviour {
	/// <summary>
	/// Base overlay texture
	/// </summary>
	public Texture2D White;

	/// <summary>
	/// Screen overlay will be drawn using basic texture and this color.
	/// </summary>
	public Color OverlayColor;

	/// <summary>
	/// Gets and sets powerup lasting time
	/// </summary>
	public float DurationSeconds = 25.0f;

	/// <summary>
	/// Current remaining second to exhaust
	/// </summary>
	private float durationRemainingSeconds;

	public virtual void Start() {
		this.Activate();
		this.White = Resources.Load("white") as Texture2D;
	}

	private void Activate() {
		this.durationRemainingSeconds = this.DurationSeconds;
	}

	public void Update() {
		this.OnSecondsPassed(Time.deltaTime);
		if(this.IsExhausted()) {
			this.DeactivateAndDestroy();
		}
	}

	private void OnSecondsPassed(float seconds) {
		this.durationRemainingSeconds -= seconds;
	}

	private bool IsExhausted() {
		return this.durationRemainingSeconds <= 0;
	}

	private void DeactivateAndDestroy() {
		MonoBehaviour.Destroy(this);
	}

	public void OnGUI() {
		var ui = this.gameObject.GetComponentInChildren<playerUI>();
		var rect = ui.CameraRect;

		// There's no simple way to semi-transparent rectangle on the screen (?)
		// So we draw texture using color with alpha component lower than 1.0f
		var previousColor = GUI.color;
		var oc = this.OverlayColor;
		GUI.color = new Color(oc.r, oc.g, oc.b, 0.25f);
		GUI.DrawTexture(rect, this.White);
		GUI.color = previousColor;
	}
}
