using Assets.Code;
using UnityEngine;

public class Rune : IInventoryItem {
    public RuneType Type;
    public Texture2D Icon { get; set; }
}