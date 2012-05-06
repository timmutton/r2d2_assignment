using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DTW;

public class HMMRecognizer : MonoBehaviour
{
	private int[][] sequences = new int[][] 
	{
	    new int[] { 0,1,1,1,1,1,1 },
	    new int[] { 0,1,1,1 },
	    new int[] { 0,1,1,1,1 },
	    new int[] { 0,1, },
	    new int[] { 0,1,1 },
	};	
	
	Vector3 currentPosition;
	Vector3 initialPosition;
	Event currentEvent;
	ArrayList points = new ArrayList();
	
	void OnGUI() {
        currentEvent = Event.current;
    }

	void Update(){
		if(currentEvent.type == EventType.mouseDown){
			initialPosition = Input.mousePosition;
			points.Clear();
			points.Add(initialPosition);
		}else if(currentEvent.type == EventType.mouseDrag){
			currentPosition = Input.mousePosition;
			points.Add(currentPosition);
		}else if(currentEvent.type == EventType.MouseUp){
			points.Add(currentPosition);
			Debug.Log("Before filtering:\n" + printVectors(points));
			
			Debug.Log("Directions:\n" + printVectors2(GestureRecognizer.getAccelerationsFromPoints(points)));
			try{
			}catch(GestureNotFoundException e){
				Debug.Log("" + e.Message);
			}
		}
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
	
	string printVectors2(List<Vector2> points){
		string result = "";
		for(int i=0; i<points.Count; i++){
			Vector2 point = points[i];
			if(i==0){
				result = "(" + point.x + "," + point.y + ")";
			}else{
				result += " - (" + point.x + "," + point.y + ")";
			}
		}
		return result;
	}       
}

