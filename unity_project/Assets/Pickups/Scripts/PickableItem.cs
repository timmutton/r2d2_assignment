using UnityEngine;

public abstract class PickableItem : MonoBehaviour {
    /// <summary>
    /// Number of seconds after which item respawns
    /// </summary>
    public float RespawnSeconds = 10.0f;

	public bool Respawnable { get { return this.RespawnSeconds > 0.0f; } }

	/// <summary>
	/// True if item is active and ready to be picked up,
	/// false if waits for respawn.
	/// </summary>
    private bool visible = true;

    /// <summary>
    /// Gets and sets visibility of item
    /// </summary>
    public bool Visible {
        get { return this.visible; }
        set {
            this.visible = value;
            foreach (var r in this.GetComponentsInChildren<Renderer>()) {
                r.enabled = this.visible;
            }
        }
    }

	/// <summary>
	/// Countdown timer for item respawn
	/// </summary>
    private float secondsToNextRespawn = 0;

	/// <summary>
	/// Item inventory icon
	/// </summary>
    public Texture2D Icon;

	/// <summary>
	/// Sound that's played when item is picked up
	/// </summary>
    public AudioClip pickupSound;

    // Update is called once per frame
    private void Update() {
        if(!this.visible && this.Respawnable) {
            this.secondsToNextRespawn -= Time.deltaTime;
            if (this.secondsToNextRespawn <= 0) {
                this.Respawn();
            }
        }
    }

    private void Respawn() {
        this.Visible = true;
    }

    public void OnTriggerEnter(Collider collider) {
        Collide(collider);
    }

    public void OnTriggerStay(Collider collider) {
        Collide(collider);
    }

    private void Collide(Collider collider) {
        if (collider.CompareTag("Player") && visible) {
            Pickup(collider);
        }
    }

    private void Pickup(Collider collider) {
		var player = collider.gameObject;

		if (this.CanBePickedByPlayer(player)) {
			if (this.pickupSound != null) {
				AudioUtil.PlaySound(this.pickupSound, this.gameObject.transform.position);
			}

			this.Hide();
			this.DoActionOnPlayer(player);
		}
    }

	private void Hide() {
		this.Visible = false;
		this.secondsToNextRespawn = this.RespawnSeconds;
	}

	protected virtual bool CanBePickedByPlayer(GameObject player) {
		return true;
	}

    protected abstract void DoActionOnPlayer(GameObject player);
}
