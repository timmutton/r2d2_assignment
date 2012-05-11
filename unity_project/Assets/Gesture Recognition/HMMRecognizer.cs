using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Accord.Statistics.Models.Markov;

public enum Gesture{
	SQUARE = 8,
	V_DOWN = 9,
	V_UP = 10,
	HORIZONTAL_LINE = 11,
	VERTICAL_LINE = 12
}
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
		
	/*public int [] states = {SQUARE, V_DOWN, V_UP, H_LINE, V_LINE};
	public double [,] A = new double[8,8] 
		{
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0}
		};
	
	public double [,] B = new double[8,5] 
		{
			{0.5, 0, 0, 0, 0.5},
			{0.5, 0, 0, 0, 0.5},
			{1, 0, 0, 0, 0},
			{1, 0, 0, 0, 0},
			{0, 0.5, 0.5, 0, 0},
			{0, 0.5, 0.5, 0, 0},
			{0, 0.5, 0.5, 0, 0},
			{0, 0.5, 0.5, 0, 0}
		};
	
	public double [] pi = new double [] {1/8,1/8,1/8,1/8, 1/8, 1/8, 1/8, 1/8};*/
	
	void Start(){
		Debug.Log("Final Result: " + hmmEvalute(new int[]{EAST, WEST}) + "\n");
	} 
	
	bool squareEvalution(int [] input){
		if(input.Length != 4){
			return false;
		}
		
		int[][] sequences = new int[][] 
		{
		    new int[]{ NORTH, EAST, SOUTH, WEST},
			new int[] { EAST, SOUTH, WEST, NORTH},
			new int[] { SOUTH, WEST, NORTH, EAST},
		    new int[] { WEST, NORTH, EAST, SOUTH}
		};
		
		double [,] A = new double[8,8] 
		{
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}
		};
		
		double [,] B = new double[8,8]
		{
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1}
		};
		
		double [] pi = new double [] {0.25,0.25,0.25,0.25, 0, 0, 0, 0};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 0.0001);
		
		if(model.Evaluate(input) >= 0.25){
			return true;
		}else{
			return false;
		}		
	}
	
	bool vDownEvalution(int [] input){
		if(input.Length != 2){
			return false;
		}
		
		int[][] sequences = new int[][] 
		{
		    new int[]{ SOUTH_EAST, NORTH_EAST},
			new int[] { SOUTH_WEST, NORTH_WEST}
		};
		
		double [,] A = new double[8,8] 
		{
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0}
		};
		
		double [,] B = new double[8,8]
		{
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1}
		};
		
		double [] pi = new double [] {0, 0, 0, 0, 0, 0, 0.5, 0.5};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 0.0001);
		
		if(model.Evaluate(input) >= 0.5){
			return true;
		}else{
			return false;
		}
	}
	
	bool vUpEvalution(int [] input){
		
		if(input.Length != 2){
			return false;
		}
		
		int[][] sequences = new int[][] 
		{
		    new int[]{ NORTH_EAST, SOUTH_EAST},
			new int[] { NORTH_WEST, SOUTH_WEST}
		};
		
		double [,] A = new double[8,8] 
		{
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}
		};
		
		double [,] B = new double[8,8]
		{
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1}
		};
		
		double [] pi = new double [] {0, 0, 0, 0, 0.5, 0.5, 0, 0};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 0.0001);
		
		if(model.Evaluate(input) >= 0.5){
			return true;
		}else{
			return false;
		}
	}
	
	bool verticalEvalution(int [] input){
		if(input.Length != 1){
			return false;
		}
		
		int[][] sequences = new int[][] 
		{
		    new int[]{ NORTH },
			new int[] { SOUTH }
		};
		
		double [,] A = new double[8,8] 
		{
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}
		};
		
		double [,] B = new double[8,8]
		{
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1}
		};
		
		double [] pi = new double [] {0.5, 0.5, 0, 0, 0, 0, 0, 0};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 0.0001);
		
		if(model.Evaluate(input) >= 0.5){
			return true;
		}else{
			return false;
		}
	}
	
	bool horizontalEvalution(int [] input){
		
		if(input.Length != 1){
			return false;
		}
		
		int[][] sequences = new int[][] 
		{
		    new int[]{ EAST },
			new int[] { WEST }
		};
		
		double [,] A = new double[8,8] 
		{
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}
		};
		
		double [,] B = new double[8,8]
		{
			{1, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 0, 0},
			{0, 0, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 1}
		};
		
		double [] pi = new double [] {0, 0, 0.5, 0.5, 0, 0, 0, 0};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 0.0001);
		
		if(model.Evaluate(input) >= 0.5){
			return true;
		}else{
			return false;
		}
	}
	
	public int hmmEvalute(int [] input){
		
		bool square = squareEvalution(input);
		bool v_down = vDownEvalution(input);
		bool v_up = vUpEvalution(input);
		bool horizontal = horizontalEvalution(input);
		bool vertical = verticalEvalution(input);
		
		if(vertical){
			return Gesture.VERTICAL_LINE;
		}else if(horizontal){
			return Gesture.HORIZONTAL_LINE;
		}else if(v_up){
			return Gesture.V_UP;
		}else if(v_down){
			return Gesture.V_DOWN;
		}else if(square){
			return Gesture.SQUARE;
		}else{
			throw new GestureNotFoundException("Gesture not found!");
		}
	}      
}

