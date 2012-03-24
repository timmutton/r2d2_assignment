public enum ModifierType {
    Heast,
    Quad
}

public class PickableModifier : PickableItem {
    public ModifierType Type;
    protected override void DoActionOnPlayer(PlayerProperties playerProperties) {
        //
    }
}