using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellProperties : MonoBehaviour {
	public enum spellElemEnum{fire, water, earth};
	public enum spellTypeEnum{offensive, defensive};
	public enum spellParamArgs{type, element, parent};
	
	public int spellDamage;
	public GameObject offensiveSpell;
	public GameObject defensiveSpell;
	public GameObject trail;
	public Material[] spellMat, trailMat;
	
	private spellElemEnum spellElem;
	private spellTypeEnum spellType;
	private Vector3 spellVel;
	
	// set the element type of the spell
	//and update the material used by the renderer
	void setSpellProperties(Dictionary<int, object> spellParams) {
		spellType = (spellTypeEnum)spellParams[(int)spellParamArgs.type];
		spellElem = (spellElemEnum)spellParams[(int)spellParamArgs.element];
		
		if(spellType == spellTypeEnum.offensive){
			offensiveSpell.renderer.material = spellMat[(int)spellElem];
			trail.renderer.material = trailMat[(int)spellElem];
			Destroy(defensiveSpell);
		}else{
			defensiveSpell.renderer.material = spellMat[(int)spellElem];
			Destroy(offensiveSpell);
		}
	}
}
