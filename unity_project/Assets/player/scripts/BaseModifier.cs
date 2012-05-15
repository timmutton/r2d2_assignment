using UnityEngine;

class BaseModifier : MonoBehaviour {
	public Texture2D White;
	public Color OverlayColor;

	public float DurationSeconds = 25.0f;

	private float durationRemainingSeconds;

	public virtual void Start() {
		this.Activate();
		this.White = Resources.Load("white") as Texture2D;
	}

	private void Activate() {
		this.durationRemainingSeconds = this.DurationSeconds;
//		Debug.Log(string.Format("{0} activated", this));
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
//		Debug.Log(string.Format("{0} deactivated", this));
		MonoBehaviour.Destroy(this);
	}

	public void OnGUI() {
		var ui = this.gameObject.GetComponentInChildren<playerUI>();
		var rect = ui.CameraRect;

		var previousColor = GUI.color;
		GUI.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.3f);
		GUI.DrawTexture(rect, this.White);
		GUI.color = previousColor;
	}
}
