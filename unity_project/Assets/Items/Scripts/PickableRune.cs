public enum RuneType {
    Water,
    Earth,
    Fire
}

public class PickableRune : PickableItem {
    public RuneType Type;
    protected override void DoActionOnPlayer(PlayerProperties playerProperties) {
        playerProperties.Inventory.Add(new Rune { Type = this.Type, Icon = this.Icon });
    }
}