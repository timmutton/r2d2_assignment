using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform playerCam;
	private Transform rightHand;
	private Vector3 spawnPos;
	
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
	
	void OnGUI() {
        currentEvent = Event.current;
    }
	
	// Update is called once per frame
	void Update () {
		if(currentEvent == null){
		}else if (currentEvent.type == EventType.MouseDown) {
			initialPosition = Input.mousePosition;
			points.Clear();
			points.Add(initialPosition);
		} else if(currentEvent.type == EventType.MouseDrag) {
			currentPosition = Input.mousePosition;
			points.Add(currentPosition);
		} else if(currentEvent.type == EventType.MouseUp) {
			points.Add(currentPosition);
		}
		
		if(gameObject.name == "player2"){
			if(currentEvent != null && currentEvent.type == EventType.MouseUp) {
				var recognizer = GestureRecognizer.GetSharedInstance();
				var mouseGestures = new MouseGestures();
				var geture = mouseGestures.getGestureFromPoints(points);
				try {
					var gesture = recognizer.RecognizeGesture(geture);
					Debug.Log(string.Format("Recognized gesture: {0}", gesture));
					createSpell(gesture.Name);
					
				}
				catch (UnityException e) {
					Debug.Log(e);
				}
			}
			
			if(Input.GetKeyDown("[1]")){
				createSpell(1);
			}
			if(Input.GetKeyDown("[2]")){
				createSpell(2);
			}
			if(Input.GetKeyDown("[3]")){
				createSpell(3);
			}
			if(Input.GetKeyDown("[7]")){
				createSpell(8);
			}
			if(Input.GetKeyDown("[8]")){
				createSpell(9);
			}
			if(Input.GetKeyDown("[9]")){
				createSpell(0);
			}
		}else{
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
	}
	
	void createSpell(int spell) {
		Debug.Log("Deprecated, remove this function");
		if (spell == 1)
			createSpell("hline");
		else if (spell == 2)
			createSpell("vline");
		else if(spell == 3)
			createSpell("vup");
		else if(spell == 4)
			createSpell("vdown");
		else if(spell == 5)
			createSpell("square");
	}


	void createSpell(string name) {
		int keyNum = 0;
		if (name.Equals("hline"))
			keyNum = 1;
		else if (name.Equals("vline"))
			keyNum = 2;
		else if (name.Equals("vup"))
			keyNum = 3;
		else if (name.Equals("vdown"))
			keyNum = 4;
		else if (name.Equals("square"))
			keyNum = 5;
		else {
			throw new NotImplementedException(string.Format("No such spell: {0}", name));
		}

		//the spawn position will be the players right hand
		//plus the radius of the hand and the diameter of the spell
		Dictionary<int, object> spellParams = new Dictionary<int, object>();
		Rune selectedSpell;
		Inventory playerInv = gameObject.GetComponent<Inventory>();

		
		switch(keyNum){
		case 1:
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Fire)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.fire;
			spellParams[(int)SpellParameter.type] = SpellType.offensive;
			spellParams[(int)SpellParameter.damageMultiplier] = 
				DefensiveSpellBehaviour.spellDamageMultiplier(gameObject, SpellElement.fire) 
				* GetQuadDamageMultiplier();
			spawnPos = rightHand.position + transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.extents.z);
			
			break;
		case 2:
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Water)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.water;
			spellParams[(int)SpellParameter.type] = SpellType.offensive;
			spellParams[(int)SpellParameter.damageMultiplier] = 
				DefensiveSpellBehaviour.spellDamageMultiplier(gameObject, SpellElement.water) 
				* GetQuadDamageMultiplier();
			spawnPos = rightHand.position + transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.extents.z);
			
			break;
		case 3:
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Earth)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.earth;
			spellParams[(int)SpellParameter.type] = SpellType.offensive;
			spellParams[(int)SpellParameter.damageMultiplier] = 
				DefensiveSpellBehaviour.spellDamageMultiplier(gameObject, SpellElement.earth) 
				* GetQuadDamageMultiplier();
			spawnPos = rightHand.position + transform.forward * (rightHand.renderer.bounds.extents.z + spell.renderer.bounds.extents.z);
			
			break;
			
		//defensive spells	
		case 8:
			if(transform.Find("defensiveSpell")){
				print("defensive spell exists");
				return;
			}
				
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Fire)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.fire;
			spellParams[(int)SpellParameter.type] = SpellType.defensive;
			spawnPos = transform.position;
			
			break;
		case 9:
			if(transform.Find("defensiveSpell")){
				print("defensive spell exists");
				return;
			}
			
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Water)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.water;
			spellParams[(int)SpellParameter.type] = SpellType.defensive;
			spawnPos = transform.position;
			
			break;
		case 0:
			if(transform.Find("defensiveSpell")){
				print("defensive spell exists");
				return;
			}
			
			if((selectedSpell = (Rune)playerInv.HasItem(RuneType.Earth)) == null)
				return; 
			playerInv.Remove(selectedSpell);
			
			spellParams[(int)SpellParameter.element] = SpellElement.earth;
			spellParams[(int)SpellParameter.type] = SpellType.defensive;
			spawnPos = transform.position;
			
			break;
			
		}
		
		spellParams[(int)SpellParameter.parent] = transform;
		
		//instantiate the spell at given position, facing the players forward direction
		//GameObject tempSpell = Instantiate(spell, spawnPos, transform.rotation) as GameObject;
		GameObject tempSpell = Instantiate(spell, spawnPos, Quaternion.Euler(playerCam.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0)) as GameObject;
//		tempSpell.transform.parent = transform;
		tempSpell.SendMessage("setSpellProperties", spellParams);
	}	
}
