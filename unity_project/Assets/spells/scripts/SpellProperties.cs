using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellElement{
	invalid,
	fire, 
	water, 
	earth
};

public enum SpellType{
	invalid,
	offensive, 
	defensive
};

public enum SpellParameter{
	parent,
	type, 
	element, 
	damageMultiplier
};

public class SpellProperties : MonoBehaviour {	
	public float spellDamage = 10.0f;
	public GameObject trail;
	public Material[] spellMat, trailMat;
	
	public SpellElement spellElem;
	public SpellType spellType;
	public Transform parent;
	private OffensiveSpellBehaviour oSpellBehave;
	private DefensiveSpellBehaviour dSpellBehave;
	
	
	//Find spell behaviours
	void Awake(){
		oSpellBehave = GetComponent<OffensiveSpellBehaviour>();
		dSpellBehave = GetComponent<DefensiveSpellBehaviour>();
		
		oSpellBehave.enabled = false;
		dSpellBehave.enabled = false;
	}
	
	// set the element type of the spell
	//and update the material used by the renderer
	void setSpellProperties(Dictionary<int, object> spellParams) {
		parent = (Transform)spellParams[(int)SpellParameter.parent];
		spellType = (SpellType)spellParams[(int)SpellParameter.type];
		spellElem = (SpellElement)spellParams[(int)SpellParameter.element];
		
		if(spellType == SpellType.offensive){
			spellDamage *= (float)spellParams[(int)SpellParameter.damageMultiplier];
			
			gameObject.name = "offensiveSpell";
			oSpellBehave.enabled = true;
			renderer.material = spellMat[(int)spellElem];
			trail.renderer.material = trailMat[(int)spellElem];
			Destroy(dSpellBehave);
		//Defensice spell
		}else{
			gameObject.name = "defensiveSpell";
			dSpellBehave.enabled = true;
			renderer.enabled = false;
			Destroy(oSpellBehave);
			Destroy(trail);
		}
	}
}
