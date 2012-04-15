using System;
using UnityEngine;

public enum ModifierType {
	QuadDamage,
	Heast,
}

public class PickableModifier : PickableItem {

	public ModifierType Type;

	protected override void DoActionOnPlayer(GameObject player) {
		switch(this.Type) {
			case ModifierType.QuadDamage:
				player.AddComponent<QuadDamage>();
				break;
			case ModifierType.Heast:
				player.AddComponent<Heast>();
				break;
		}
	}
}