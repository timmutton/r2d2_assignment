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

	/// <summary>
	/// Calculate rect for each inventory item icon (depending on item index in inventory)
	/// </summary>
	/// <param name="index">Item index in the inventory (0-based)</param>
	/// <returns>Rect for icon</returns>
	private Rect RectForItemIndex(int index) {
		var ui = this.gameObject.GetComponentInChildren<playerUI>();
		var cameraRect = ui.CameraRect;

		float width = 50;
		float height = 50;

		float left = cameraRect.x + 10;
		float top = cameraRect.yMax - (height + 10);

		return new Rect(left + index*width, top, width, height);
	}

	
	/// <summary>
	/// Gets inventory items grouped by texture.
	/// </summary>
	/// <returns></returns>
	private IOrderedEnumerable<IGrouping<Texture2D, IInventoryItem>> GetGroupings() {
    	return this.items
    		.GroupBy(rune => rune.Icon)
    		.OrderBy(grouping => grouping.Key.GetHashCode());
    }
	
	/// <summary>
	/// Gets rune of specified type from the inventory or null if not found
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public Rune GetRune(RuneType type) {
		return this.items.Cast<Rune>().FirstOrDefault(item => item.Type == type);
	}
}