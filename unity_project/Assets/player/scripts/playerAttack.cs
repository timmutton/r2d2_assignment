using UnityEngine;
using System.Collections;

public class playerAttack : MonoBehaviour {
	public GameObject spell;
	private Transform rightHand;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			Vector3 spawnPos = rightHand.transform.position + 
				new Vector3(0, 0, rightHand.renderer.bounds.extents.z + spell.renderer.bounds.size.z);
			GameObject temp = (GameObject)GameObject.Instantiate(spell, 
				spawnPos, transform.rotation);
			temp.SendMessage("setSpellType", 
				offensiveSpellProperties.spellTypeEnum.fire);
		}
	}
	
	void Start(){
		rightHand = transform.Find("Model/rightHand");
		if(rightHand == null){
			Debug.Log("Could not find right hand");
		}
	}
}
