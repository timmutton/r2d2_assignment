using System.Collections.Generic;
using Assets.Code;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour {
    private List<IInventoryItem> items;

    public void Start() {
        this.items = new List<IInventoryItem>();
    }

    public void Add(Rune item) {
        this.items.Add(item);
    }

    public void Remove(Rune item) {
        this.items.Remove(item);
    }

    public void OnGUI() {
        var textures = this.GetTextures();
        var ui = this.gameObject.GetComponentInChildren<playerUI>();
        var cameraRect = ui.CameraRect;
        if(textures.Length > 0) {
            GUI.SelectionGrid(new Rect(cameraRect.x + 10, cameraRect.yMax - 60, 200, 50), 0, textures, 4);
        }
    }

    private Texture2D[] GetTextures() {
        return this.items.Select(rune => rune.Icon).ToArray();
    }
}