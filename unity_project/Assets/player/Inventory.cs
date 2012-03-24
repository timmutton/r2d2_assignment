using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Assets.player {
    public class Inventory {
        private List<Rune> items = new List<Rune>();

        public void Add(Rune item) {
            this.items.Add(item);
        }

        public void Remove(Rune item) {
            this.items.Remove(item);
        }

        public void Draw() {
            var textures = this.GetTextures();
            GUILayout.SelectionGrid(0, textures, 5);
        }

        private Texture2D[] GetTextures() {
            return this.items.Select(rune => rune.Icon).ToArray();
        }
    }
}
