using UnityEngine;
using System.Collections;

public class offensiveSpellProperties : MonoBehaviour {
	public enum spellTypeEnum{fire, water, earth};
	public GameObject trail;
	public Material[] spellMat, trailMat;
	
	private spellTypeEnum spellType;
	
	// set the element type of the spell
	//and update the material used by the renderer
	void setSpellType(spellTypeEnum type) {
		spellType = type;
		renderer.material = spellMat[(int)type];
		trail.renderer.material = trailMat[(int)type];
	}
}
