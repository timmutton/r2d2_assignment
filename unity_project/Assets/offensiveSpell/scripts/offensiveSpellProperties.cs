using UnityEngine;
using System.Collections;

public class offensiveSpellProperties : MonoBehaviour {
	public enum spellTypeEnum{fire, water, earth};
	private spellTypeEnum spellType;
	public Material[] spellMat;
	
	// set the element type of the spell
	//and update the material used by the renderer
	void setSpellType(spellTypeEnum type) {
		spellType = type;
		renderer.material = spellMat[(int)type];
	}
}
