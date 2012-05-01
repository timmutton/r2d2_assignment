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
        	float width = cameraRect.width - 10;
        	int xCount = (int) width/50;
            GUI.SelectionGrid(new Rect(cameraRect.x + 10, cameraRect.yMax - 60, width, 50),
				0, textures, xCount, GUIStyle.none);
        }
    }

    private Texture2D[] GetTextures() {
        return this.items.Select(rune => rune.Icon).OrderBy(d => d.GetHashCode()).ToArray();
    }
	
	public IInventoryItem HasItem(RuneType type){
		foreach(Rune item in items){
			if(item.Type == type)
			return item;
		}
		return null;
	}
}