using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform playerCam;
	private Transform rightHand;
	private Vector3 spawnPos;
	
	void Start(){
		//link to the right hand
		rightHand = transform.FindChild("model/rightHand");
		playerCam = transform.FindChild("Main Camera");
		if(rightHand == null || playerCam == null){
			Debug.Log("Could not find components");
		}
	}

	float GetDamageMultiplier() {
		var quad = this.gameObject.GetComponentInChildren<QuadDamage>();
		return quad != null ? quad.DamageMultipier : 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.name == "player1"){
			if(Input.GetKeyDown("1")){
				createSpell(1);
			}
			if(Input.GetKeyDown("2")){
				createSpell(2);
			}
			if(Input.GetKeyDown("3")){
				createSpell(3);
			}
		}else{
			if(Input.GetKeyDown("[1]")){
				createSpell(1);
			}
			if(Input.GetKeyDown("[2]")){
				createSpell(2);
			}
			if(Input.GetKeyDown("[3]")){
				createSpell(3);
			}
		}
		
		/*if(Input.GetKeyDown("8")){
			createSpell(8);
		}
		if(Input.GetKeyDown("9")){
			createSpell(9);
		}
		if(Input.GetKeyDown("0")){
			createSpell(0);
		}*/
	}
	
	void createSpell(int keyNum){
		//the spawn position will be the players right hand
		//plus the radius of the hand and the diameter of the spell
		Dictionary<int, object> spellParams = new Dictionary<int, object>();
		Rune selectedSpell;
		
		switch(keyNum){
		case 1:
			if((selectedSpell = (Rune)gameObject.GetComponent<Inventory>().HasItem(RuneType.Fire)) == null)
				return; 
			gameObject.GetComponent<Inventory>().Remove(selectedSpell);
			
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.fire;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			break;
		case 2:
			if((selectedSpell = (Rune)gameObject.GetComponent<Inventory>().HasItem(RuneType.Water)) == null)
				return; 
			gameObject.GetComponent<Inventory>().Remove(selectedSpell);
			
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.water;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			break;
		case 3:
			if((selectedSpell = (Rune)gameObject.GetComponent<Inventory>().HasItem(RuneType.Earth)) == null)
				return; 
			gameObject.GetComponent<Inventory>().Remove(selectedSpell);
			
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.earth;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			break;
			
		case 8:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.fire;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			break;
		case 9:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.water;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			break;
		case 0:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.earth;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.defensive;
			break;
			
		}

		spawnPos = rightHand.position + transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.extents.z);
		
		//instantiate the spell at given position, facing the players forward direction
		//GameObject tempSpell = Instantiate(spell, spawnPos, transform.rotation) as GameObject;
		GameObject tempSpell = Instantiate(spell, spawnPos, Quaternion.Euler(playerCam.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0)) as GameObject;
		
		tempSpell.SendMessage("setSpellProperties", 
			spellParams);
	}	
}
