using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform playerCam;
	private Transform rightHand;
//	private Transform oSpell;
	private Vector3 spawnPos;
	
	void Start(){
		//link to the right hand
		rightHand = transform.Find("model/rightHand");
		//oSpell = spell.transform.Find("offensiveSpell");
		playerCam = transform.FindChild("Main Camera");
		if(rightHand == null){
			Debug.Log("Could not find right hand");
		}
	}

	float GetDamageMultiplier() {
		var quad = this.gameObject.GetComponentInChildren<QuadDamage>();
		return quad != null ? quad.DamageMultipier : 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("1")){
			createSpell(1);
		}
		if(Input.GetKeyDown("2")){
			createSpell(2);
		}
		if(Input.GetKeyDown("3")){
			createSpell(3);
		}
		
		if(Input.GetKeyDown("8")){
			createSpell(8);
		}
		if(Input.GetKeyDown("9")){
			createSpell(9);
		}
		if(Input.GetKeyDown("0")){
			createSpell(0);
		}
	}
	
	void createSpell(int keyNum){
		//the spawn position will be the players right hand
		//plus the radius of the hand and the diameter of the spell
		Dictionary<int, object> spellParams = new Dictionary<int, object>();
		
		switch(keyNum){
		case 1:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.fire;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			spawnPos = rightHand.transform.position + 
			transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.size.z);
			break;
		case 2:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.water;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			spawnPos = rightHand.transform.position + 
			transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.size.z);
			break;
		case 3:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.earth;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			spawnPos = rightHand.transform.position + 
			transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.size.z);
			break;
			
		case 8:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.fire;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			spawnPos = transform.position;
			break;
		case 9:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.water;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			spawnPos = transform.position;
			break;
		case 0:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.earth;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			spawnPos = transform.position;
			break;
			
		}
		
		//instantiate the spell at given position, facing the players forward direction
		GameObject tempSpell = (GameObject)GameObject.Instantiate(spell, 
			spawnPos, 
			//Quaternion.Euler(playerCam.rotation.x, transform.rotation.y, 0)
			Quaternion.Euler(transform.forward)
		);
		
		tempSpell.SendMessage("setSpellProperties", 
			spellParams);
	}	
}
