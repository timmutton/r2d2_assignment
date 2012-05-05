using UnityEngine;
using System.Collections;

public enum Gestures : int
{
	HORIZONTAL_LINE = 1,
	VERTICAL_LINE = 2,
	V_UP = 3,
	V_DOWN = 4,
	SQUARE = 5
}

public class GestureRecognizer : MonoBehaviour {
	public Shader shader;
	Vector3 currentPosition;
	Vector3 initialPosition;
	Event currentEvent;
	//ArrayList points = new ArrayList();	
	private const double MIN_DISTANCE = 0.1;
	
	private const int HORIZONTAL_LINE = 1;
	private const int VERTICAL_LINE = 2;
	private const int V_UP = 3;
	private const int V_DOWN = 4;
	private const int SQUARE = 5;

	
	/*void OnGUI() {
        currentEvent = Event.current;
    }*/
	
/*	void Update() {
//		return;
		currentEvent = Event.current;
		
		if (currentEvent.type == EventType.MouseDown) {
			initialPosition = Input.mousePosition;
			points.Clear();
			points.Add(initialPosition);
			//Debug.Log("Initial: x: " + initialPosition.x + " y: " + initialPosition.y + " z: " + initialPosition.z); 
		} else if(currentEvent.type == EventType.MouseDrag) {
			currentPosition = Input.mousePosition;
			points.Add(currentPosition);
			//Debug.Log("Dragging");
		} else if(currentEvent.type == EventType.MouseUp) {
			points.Add(currentPosition);
			//Debug.Log("x: " + currentPosition.x + " y: " + currentPosition.y + " z: " + 
			try {
				int gesture = getGestureFromPoints(points);
				string gestureName = "";
				switch (gesture) {
					case 1: gestureName = "HORIZONTAL_LINE"; break;
					case 2: gestureName = "VERTICAL_LINE"; break;
					case 3: gestureName = "V_UP"; break;
					case 4: gestureName = "V_DOWN"; break;
					case 5: gestureName = "SQUARE"; break;
					default: gestureName = "Unknown gesture..."; break;
				}
				Debug.Log("Gesture: " + gestureName);
			} catch(GestureNotFoundException e) {
				Debug.Log("" + e.Message);
			}
		}
	}
*/
	void printGesture(ArrayList points){
		try {
			int gesture = getGestureFromPoints(points);
			string gestureName = "";
			switch (gesture) {
				case 1: gestureName = "HORIZONTAL_LINE"; break;
				case 2: gestureName = "VERTICAL_LINE"; break;
				case 3: gestureName = "V_UP"; break;
				case 4: gestureName = "V_DOWN"; break;
				case 5: gestureName = "SQUARE"; break;
				default: gestureName = "Unknown gesture..."; break;
			}
			Debug.Log("Gesture: " + gestureName);
		} catch(GestureNotFoundException e) {
			Debug.Log("" + e.Message);
		}
	}
	
	public static int getGestureFromPoints(ArrayList points){
		Vector2 avg;
		avg.x = 0;
		avg.y = 0;
		for (int i=0; i<points.Count; i++){
			Vector3 point = (Vector3)points[i];
			avg.x += point.x;
			avg.y += point.y;
		}
		avg.x = avg.x/points.Count;
		avg.y = avg.y/points.Count;
		
		Vector2 sd;
		sd.x = 0;
		sd.y = 0;
		for(int i=0; i<points.Count; i++){
			Vector3 point = (Vector3)points[i];
			sd.x += Mathf.Pow((point.x - avg.x), 2);
			sd.y += Mathf.Pow((point.y - avg.y), 2);
		}
		
		sd.x = Mathf.Sqrt(sd.x);
		sd.y = Mathf.Sqrt(sd.y);
		
		float avg2 = (sd.x + sd.y)/2;
		float sd2 = Mathf.Sqrt(Mathf.Pow((sd.x - avg2),2) + Mathf.Pow((sd.y - avg2),2));
		
		//Debug.Log ("AVG: (" + avg.x + "," + avg.y + ") SD: (" + sd.x + "," + sd.y + ")");
		if(sd.x < avg.x*MIN_DISTANCE){
			return 2;
		}else if(sd.y < avg.y*MIN_DISTANCE){
			return 1;
		}else if(sd2 <= avg2*0.4){
			Vector3 firstPoint = (Vector3)points[0];
			Vector3 lastPoint = (Vector3)points[points.Count - 1];
			Vector3 midPoint = (Vector3)points[points.Count/2];
			
			Vector3 avg3 = (firstPoint + lastPoint)/2;
			Vector3 sd3;
			sd3.x = Mathf.Sqrt(Mathf.Pow(firstPoint.x - avg3.x,2) + Mathf.Pow(lastPoint.x - avg3.x,2));
			sd3.y = Mathf.Sqrt(Mathf.Pow(firstPoint.y - avg3.y,2) + Mathf.Pow(lastPoint.y - avg3.y,2));
			if(sd3.y <= avg3.y*0.8){
				if(sd3.x > avg3.x*0.5){
					if(firstPoint.y < midPoint.y){
						return 3;
					}else{
						return 4;
					}
				}
			}
		}
		throw new GestureNotFoundException("Invalid Gesture");
	}
	
	string printVectors(ArrayList points){
		string result = "";
		for(int i=0; i<points.Count; i++){
			Vector3 point = (Vector3) points[i];
			if(i==0){
				result = "(" + point.x + "," + point.y + ")";
			}else{
				result += " - (" + point.x + "," + point.y + ")";
			}
		}
		return result;
	}                                        
	
	double calculateAngle(Vector3 intersection, Vector3 point1, Vector3 point2){
		Vector3 vector1 = point1 - intersection;
		Vector3 vector2 = point2 - intersection;
						
		float scalarProduct = (vector1.x*vector2.x) + (vector1.y*vector2.y);
		
		float m1 = Mathf.Sqrt(Mathf.Pow(vector1.x,2) + Mathf.Pow(vector1.y,2));
		float m2 = Mathf.Sqrt(Mathf.Pow(vector2.x,2) + Mathf.Pow(vector2.y,2));
		
		return Mathf.Acos(scalarProduct/(m1*m2))*180/Mathf.PI;
	}
}
