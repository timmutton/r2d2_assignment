using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform playerCam;
	private Transform rightHand;
	private Vector3 spawnPos;
	
	private SpellElement elem = SpellElement.invalid;
	private SpellType type = SpellType.invalid;
	
	ArrayList points = new ArrayList();
	Vector3 currentPosition;
	Vector3 initialPosition;
	Event currentEvent;
	
	void Start(){
		//link to the right hand
		rightHand = transform.FindChild("model/rightHand");
		playerCam = transform.FindChild("Main Camera");
		if(rightHand == null || playerCam == null){
			Debug.Log("Could not find components");
		}
		currentEvent = null;
	}

	float GetQuadDamageMultiplier() {
		var quad = this.gameObject.GetComponentInChildren<QuadDamage>();
		return quad != null ? quad.DamageMultipier : 1.0f;
	}
	
	//set spell properties based off passed gesture
	void handleGesture(GestureEnum gest){
		if(gest == GestureEnum.V_DOWN)
			elem = SpellElement.earth;
		else if(gest == GestureEnum.V_UP)
			elem = SpellElement.fire;
		else if(gest == GestureEnum.SQUARE)
			elem = SpellElement.water;
		else if(gest == GestureEnum.HORIZONTAL_LINE)
			type = SpellType.offensive;
		else if(gest == GestureEnum.VERTICAL_LINE)
			type = SpellType.defensive;
		
		if(elem != SpellElement.invalid && type != SpellType.invalid)
			castSpell();
	}

	//cast spell based on properties
	void castSpell() {
		Dictionary<int, object> spellParams = new Dictionary<int, object>();
		Rune selectedSpell;
		Inventory playerInv = gameObject.GetComponent<Inventory>();
		
		print(elem + " " + type);
		
		if(elem == SpellElement.invalid || type == SpellType.invalid){
			clearSpellData();
			return;
		}
		
		//if we have a run for a spell, remove it		
		RuneType runeToCheck;
		if(elem == SpellElement.earth)
			runeToCheck = RuneType.Earth;
		else if(elem == SpellElement.fire)
			runeToCheck = RuneType.Fire;
		else
			runeToCheck = RuneType.Water;
		
		if((selectedSpell = (Rune)playerInv.HasItem(runeToCheck)) == null){
			clearSpellData();
			return;
		}
		playerInv.Remove(selectedSpell);
		
		spellParams[(int)SpellParameter.element] = elem;
		spellParams[(int)SpellParameter.type] = type;
		spellParams[(int)SpellParameter.damageMultiplier] = 
				DefensiveSpellBehaviour.spellDamageMultiplier(gameObject, elem) 
				* GetQuadDamageMultiplier();
		
		
		if(type == SpellType.offensive)
			spawnPos = rightHand.position + transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.extents.z);
		else
			spawnPos = transform.position;
			
		spellParams[(int)SpellParameter.parent] = transform;
		
		//cast spell
		GameObject tempSpell = Instantiate(spell, spawnPos, Quaternion.Euler(playerCam.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0)) as GameObject;
		tempSpell.SendMessage("setSpellProperties", spellParams);
		
		clearSpellData();
	}
	
	//spell data
	void clearSpellData(){
		elem = SpellElement.invalid;
		type = SpellType.invalid;
	}
}
