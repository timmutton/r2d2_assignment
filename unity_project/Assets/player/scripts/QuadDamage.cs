using UnityEngine;

class QuadDamage : BaseModifier {
	public float DamageMultipier { get { return 4.0f; }}

	public override void Start() {
		base.Start();
		this.OverlayColor = new Color(0, 0, 1);
	}
}
