using Assets.Code;
using UnityEngine;

/// <summary>
/// Rune placed in player's inventory
/// </summary>
public class Rune : IInventoryItem {
    public RuneType Type;
    public Texture2D Icon { get; set; }
}