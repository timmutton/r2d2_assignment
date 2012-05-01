using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefensiveSpellBehaviour : MonoBehaviour {
	public float damageMultiplier = 1.0f, strongAgainstMultiplier = 1.0f, weakAgainstMultiplier = 1.0f,spellActiveTime = 1.0f;
	public Material baseMat;
	
	SpellProperties properties;
	//bool started = false;
	List<Renderer> playerRenderers = new List<Renderer>();
	Transform parent;
	
	void Start() {		
		properties = GetComponent<SpellProperties>();			
		//yeild return
		StartCoroutine("applyTexture");
	}
	
	IEnumerator applyTexture(){
		parent = transform.parent;
	
		playerRenderers.Add(parent.Find("model/hatBase").renderer);
		playerRenderers.Add(parent.Find("model/hatTop").renderer);
		playerRenderers.Add(parent.Find("model/torso").renderer);
		
		print("changing mat to elem");
		foreach(Renderer tRend in playerRenderers)
			tRend.material = properties.spellMat[(int)properties.spellElem];
		
		yield return new WaitForSeconds(spellActiveTime);
		
		print("changing mat to drab");
		foreach(Renderer tRend in playerRenderers)
			tRend.material = baseMat;

		Destroy(gameObject);
		
	}
	
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
					//break;
				case SpellElement.water:
					if(spellElem == SpellElement.fire)
						return dSpellBehave.strongAgainstMultiplier;
					else if(spellElem == SpellElement.earth)
						return dSpellBehave.weakAgainstMultiplier;
					else
						return 1.0f;
					//break;
				case SpellElement.earth:
					if(spellElem == SpellElement.water)
						return dSpellBehave.strongAgainstMultiplier;
					else if(spellElem == SpellElement.fire)
						return dSpellBehave.weakAgainstMultiplier;
					else
						return 1.0f;
					//break;
			}
		}
		return 1.0f;
	}
}
