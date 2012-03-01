using UnityEngine;
using System.Collections;

public class playerProperties : MonoBehaviour {	
	public float Health{get; private set;}
	public float minHealth = 0.0f, maxHealth = 100.0f, startHealth = 100.0f;
	
	// Use this for initialization
	void Start () {
		Health = startHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
