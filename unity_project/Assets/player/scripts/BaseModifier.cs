using UnityEngine;

class BaseModifier : MonoBehaviour {
	public float DurationSeconds = 10.0f;

	private float durationRemainingSeconds;

	public void Start() {
		this.Activate();
	}

	private void Activate() {
		this.durationRemainingSeconds = this.DurationSeconds;
		Debug.Log(string.Format("{0} activated", this));
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
		Debug.Log(string.Format("{0} deactivated", this));
		MonoBehaviour.Destroy(this);
	}


}
