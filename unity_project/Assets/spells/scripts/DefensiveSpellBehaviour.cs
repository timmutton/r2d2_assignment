using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefensiveSpellBehaviour : MonoBehaviour {
	public float damageMultiplier = 1.0f, strongAgainstMultiplier = 1.0f, weakAgainstMultiplier = 1.0f,spellActiveTime = 1.0f;
	public Material baseMat;
	
	SpellProperties properties;
	List<Renderer> playerRenderers = new List<Renderer>();
	Transform parent;
	
	void Start() {		
		properties = GetComponent<SpellProperties>();	
		StartCoroutine("applyTexture");
	}
	
	//apply texture based off spell to caster
	IEnumerator applyTexture(){
		parent = properties.parent;
	
		playerRenderers.Add(parent.Find("model/hatBase").renderer);
		playerRenderers.Add(parent.Find("model/hatTop").renderer);
		playerRenderers.Add(parent.Find("model/torso").renderer);

		foreach(Renderer tRend in playerRenderers)
			tRend.material = properties.spellMat[(int)properties.spellElem];
		
		yield return new WaitForSeconds(spellActiveTime);

		foreach(Renderer tRend in playerRenderers)
			tRend.material = baseMat;

		Destroy(gameObject);
		
	}
	
	//multiply damage if offensive spell type is defensive spell type
	public static float spellDamageMultiplier(GameObject player, SpellElement spellElem){
		Transform defensiveSpell;
		SpellProperties properties;
		DefensiveSpellBehaviour dSpellBehave;
		
		if(defensiveSpell = player.transform.Find("defensiveSpell")){
			properties = defensiveSpell.GetComponent<SpellProperties>();
			if(properties.spellElem == spellElem){
				dSpellBehave = defensiveSpell.GetComponent<DefensiveSpellBehaviour>();
				return dSpellBehave.damageMultiplier;
			}
		}
		return 1.0f;
	}
	
	//get rock paper scissor damage multiplier
	public static float spellResistanceMultiplier(GameObject player, SpellElement spellElem){
		Transform defensiveSpell;
		SpellProperties properties;
		DefensiveSpellBehaviour dSpellBehave;
			
		if(defensiveSpell = player.transform.Find("defensiveSpell")){
			properties = defensiveSpell.GetComponent<SpellProperties>();
			dSpellBehave = defensiveSpell.GetComponent<DefensiveSpellBehaviour>();
			switch(properties.spellElem){
				case SpellElement.fire:
					if(spellElem == SpellElement.earth)
						return dSpellBehave.strongAgainstMultiplier;
					else if(spellElem == SpellElement.water)
						return dSpellBehave.weakAgainstMultiplier;
					else
						return 1.0f;
				case SpellElement.water:
					if(spellElem == SpellElement.fire)
						return dSpellBehave.strongAgainstMultiplier;
					else if(spellElem == SpellElement.earth)
						return dSpellBehave.weakAgainstMultiplier;
					else
						return 1.0f;
				case SpellElement.earth:
					if(spellElem == SpellElement.water)
						return dSpellBehave.strongAgainstMultiplier;
					else if(spellElem == SpellElement.fire)
						return dSpellBehave.weakAgainstMultiplier;
					else
						return 1.0f;
			}
		}
		return 1.0f;
	}
}
