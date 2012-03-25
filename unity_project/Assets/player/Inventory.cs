using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Assets.player {
    public class Inventory {
        private List<Rune> items = new List<Rune>();
        private PlayerProperties owner;

        public Inventory(PlayerProperties owner) {
            this.owner = owner;
        }

        public void Add(Rune item) {
            this.items.Add(item);
        }

        public void Remove(Rune item) {
            this.items.Remove(item);
        }

        public void Draw() {
            var textures = this.GetTextures();
            //GUILayout.SelectionGrid(0, textures, 5);
            var go = this.owner.gameObject;
            var ui = go.GetComponentInChildren<playerUI>();
            var cameraRect = ui.CameraRect;
            if(textures.Length > 0) {
                GUI.SelectionGrid(new Rect(cameraRect.x + 10, cameraRect.yMax - 60, 200, 50), 0, textures, 4);
            }
        }

        private Texture2D[] GetTextures() {
            return this.items.Select(rune => rune.Icon).ToArray();
        }
    }
}
