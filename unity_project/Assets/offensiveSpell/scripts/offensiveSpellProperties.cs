using UnityEngine;
using System.Collections;

public class offensiveSpellProperties : MonoBehaviour {
	public enum spellTypeEnum{fire, water, earth};
	private spellTypeEnum spellType;
	public Material[] spellMat;
	
	// Use this for initialization
	void setSpellType(spellTypeEnum type) {
		spellType = type;
		renderer.material = spellMat[(int)type];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
