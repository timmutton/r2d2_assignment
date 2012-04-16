using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform playerCam;
	private Transform rightHand;
<<<<<<< HEAD
	private Transform oSpell;
	private Vector3 spawnPos;
=======
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			//the spawn position will be the players right hand
			//plus the radius of the hand and the diameter of the spell
			Vector3 spawnPos = rightHand.transform.position + 
				transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.size.z);
			//instantiate the spell at given position, facing the players forward direction
			GameObject temp = (GameObject)GameObject.Instantiate(spell, 
				spawnPos, transform.rotation);

			var projectile = temp.GetComponentInChildren<offensiveSpellBehaviour>();
			projectile.damageAmt *= this.GetDamageMultiplier();

			//set the spell type
			temp.SendMessage("setSpellType", 
				offensiveSpellProperties.spellTypeEnum.water);
		}
	}
>>>>>>> 26d3d28950f8a0e0ca4f7eca95c00fcc929dea18
	
	void Start(){
		//link to the right hand
		rightHand = transform.Find("model/rightHand");
		oSpell = spell.transform.Find("offensiveSpell");
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
			transform.forward * (rightHand.renderer.bounds.extents.z + oSpell.renderer.bounds.size.z);
			break;
		case 2:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.water;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			spawnPos = rightHand.transform.position + 
			transform.forward * (rightHand.renderer.bounds.extents.z + oSpell.renderer.bounds.size.z);
			break;
		case 3:
			spellParams[(int)SpellProperties.spellParamArgs.element] = SpellProperties.spellElemEnum.earth;
			spellParams[(int)SpellProperties.spellParamArgs.type] = SpellProperties.spellTypeEnum.offensive;
			spawnPos = rightHand.transform.position + 
			transform.forward * (rightHand.renderer.bounds.extents.z + oSpell.renderer.bounds.size.z);
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
		
		Quaternion rotX = Quaternion.AngleAxis(playerCam.rotation.eulerAngles.x, Vector3.right);
		Quaternion rotY = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
		Quaternion combinedRot = rotX * rotY;
		
		print(playerCam.rotation.eulerAngles.x);
		print(transform.rotation.eulerAngles.y);
		print(combinedRot.eulerAngles.ToString());
		
		//instantiate the spell at given position, facing the players forward direction
		GameObject tempSpell = (GameObject)GameObject.Instantiate(spell, 
			spawnPos, 
			transform.rotation
		);
		
		tempSpell.SendMessage("setSpellProperties", 
			spellParams);
	}	
}
