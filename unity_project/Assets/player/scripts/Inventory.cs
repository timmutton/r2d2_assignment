using System;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour {
	public GUIStyle GuiStyle;

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
        var groupings = this.GetGroupings().ToArray();

		for(int i = 0; i < groupings.Length; ++i) {
			var grouping = groupings[i];
			var rect = this.RectForItemIndex(i);
			GUI.DrawTexture(rect, grouping.Key);
			GUI.Label(rect, string.Format("{0}", grouping.Count()), this.GuiStyle);
		}
    }

	private Rect RectForItemIndex(int index) {
		var ui = this.gameObject.GetComponentInChildren<playerUI>();
		var cameraRect = ui.CameraRect;

		float width = 50;
		float height = 50;

		float left = cameraRect.x + 10;
		float top = cameraRect.yMax - (height + 10);

		return new Rect(left + index*width, top, width, height);
	}

	
	private IOrderedEnumerable<IGrouping<Texture2D, IInventoryItem>> GetGroupings() {
    	return this.items
    		.GroupBy(rune => rune.Icon)
    		.OrderBy(grouping => grouping.Key.GetHashCode());
    }
	
	public IInventoryItem HasItem(RuneType type){
		foreach(Rune item in items){
			if(item.Type == type)
			return item;
		}
		return null;
	}
}