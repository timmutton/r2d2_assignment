using System;
using UnityEngine;

class Haste : BaseModifier {
	public float MovementSpeedMultiplier { get { return 3.0f; } }

	public override void Start() {
		base.Start();
		this.OverlayColor = new Color(1, 1, 1);
	}
}
