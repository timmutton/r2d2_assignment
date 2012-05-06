using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Accord.Statistics.Models.Markov;

public class HMMRecognizer : MonoBehaviour
{
	public const int NORTH = 0;
	public const int SOUTH = 1;
	public const int EAST = 2;
	public const int WEST = 3;
	public const int NORTH_EAST = 4;
	public const int NORTH_WEST = 5;
	public const int SOUTH_EAST = 6;
	public const int SOUTH_WEST = 7;
	
	private int[][] square_sequences = new int[][] 
	{
	    new int[]{ NORTH, EAST, SOUTH, WEST},
		new int[] { EAST, SOUTH, WEST, NORTH},
		new int[] { SOUTH, WEST, NORTH, EAST},
	    new int[] { WEST, NORTH, EAST, SOUTH}
	};
	
	double [,] A = new double[4,4] 
	{
		{0.5,0.5,0,0},
		{0,0.5,0,0},
		{0,0,0.5,0},
		{0,0,0.5,0.5}
	};
	
	double [,] B = new double[4,4]
	{
		{0.5,0.5,0,0},
		{0,0.5,0,0},
		{0,0,0.5,0},
		{0,0,0.5,0.5}
	};
	
	double [] pi = new double [] {0.5, 0.5, 0.0, 0.0};
	
	Vector3 currentPosition;
	Vector3 initialPosition;
	Event currentEvent;
	ArrayList points = new ArrayList();

	void testHmm(){
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(square_sequences, 4, 0.0001);
		
		Debug.Log("\nEvaluation 1: " + model.Evaluate(new int[] {NORTH, EAST, SOUTH, WEST}));
		
		Debug.Log("\nEvaluation 2: " + model.Evaluate(new int[] { NORTH, EAST, WEST, SOUTH}));
		
		Debug.Log("\nEvaluation 3: " + model.Evaluate(new int[] { NORTH, SOUTH, EAST, WEST}));
		
		Debug.Log("\nEvaluation 4: " + model.Evaluate(new int[] { EAST, SOUTH, WEST, NORTH}));
	}
	
	void Start(){
		testHmm();
	}
	
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

