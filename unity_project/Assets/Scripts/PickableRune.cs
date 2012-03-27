using UnityEngine;

public enum RuneType {
    Water,
    Earth,
    Fire
}

public class PickableRune : PickableItem {
    public RuneType Type;
    protected override void DoActionOnPlayer(GameObject player) {
        var inventory = player.GetComponentInChildren<Inventory>();
        inventory.Add(new Rune { Type = this.Type, Icon = this.Icon });
    }
}