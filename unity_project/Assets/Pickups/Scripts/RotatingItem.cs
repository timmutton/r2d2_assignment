using UnityEngine;

public class RotatingItem : MonoBehaviour {
    public float DegreesPerSecond = 90f;

	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(Vector3.up, Time.deltaTime * Mathf.Deg2Rad * this.DegreesPerSecond);
	}
}
