using UnityEngine;

namespace Assets.Code {
    public interface IInventoryItem {

		/// <summary>
		/// Gets texture representing item in the inventory.
		/// </summary>
        Texture2D Icon { get; }
    }
}