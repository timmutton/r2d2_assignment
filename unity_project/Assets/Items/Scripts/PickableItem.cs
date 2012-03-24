using UnityEngine;

public abstract class PickableItem : MonoBehaviour {
    /// <summary>
    /// Number of seconds after which item respawns
    /// </summary>
    public float RespawnSeconds = 10.0f;

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

    private float secondsToNextRespawn = 0;

    public Texture2D Icon;

    // Use this for initialization
    private void Start() {}

    // Update is called once per frame
    private void Update() {
        if (!this.visible) {
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
        if (collider.CompareTag("Player") && this.visible) {
            this.Pickup(collider);
        }
    }

    private void Pickup(Collider collider)
    {
        this.Visible = false;
        this.secondsToNextRespawn = this.RespawnSeconds;
        var playerProperties = collider.GetComponent<PlayerProperties>();
        this.DoActionOnPlayer(playerProperties);
    }

    protected abstract void DoActionOnPlayer(PlayerProperties playerProperties);
}
