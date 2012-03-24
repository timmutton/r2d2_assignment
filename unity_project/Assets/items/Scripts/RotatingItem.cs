using UnityEngine;
using System.Collections;

public class RotatingItem : MonoBehaviour {
    public float degreesPerSecond = 90f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(Vector3.up, Time.deltaTime * Mathf.Deg2Rad * this.degreesPerSecond);
	}
}
