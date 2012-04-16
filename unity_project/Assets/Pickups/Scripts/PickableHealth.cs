using UnityEngine;

public class PickableHealth : PickableItem {
	/// <summary>
	/// Hit points contained by healt pack
	/// </summary>
	public int Hp = 20;

	protected override void DoActionOnPlayer(GameObject player) {
		var properties = player.GetComponent<PlayerProperties>();
		if (properties) {
			properties.Heal(this.Hp);
		}
	}
}