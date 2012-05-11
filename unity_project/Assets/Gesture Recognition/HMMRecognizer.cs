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
	
	public const int SQUARE = 8;
	public const int V_DOWN = 9;
	public const int V_UP = 10;
	public const int H_LINE = 11;
	public const int V_LINE = 12;
	
	double squareEvalution(int [] input){
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
			{0.25, 0, 0, 0, 0, 0, 0, 0},
			{0, 0.25, 0, 0, 0, 0, 0, 0},
			{0, 0, 0.25, 0, 0, 0, 0, 0},
			{0, 0, 0, 0.25, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}
		};
		
		double [] pi = new double [] {0.25,0.25,0.25,0.25, 0, 0, 0, 0};
		
		HiddenMarkovModel model = new HiddenMarkovModel(A, B, pi);
		model.Learn(sequences, 4, 0.0001);
		
		return model.Evaluate(input);
	}
	
	double vDownEvalution(int [] input){
		
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
		
		return model.Evaluate(input);
	}
	
	double vUpEvalution(int [] input){
		
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
		
		return model.Evaluate(input);
	}
	
	double verticalEvalution(int [] input){
		
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
		
		return model.Evaluate(input);
	}
	
	double horizontalEvalution(int [] input){
		
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
		
		return model.Evaluate(input);
	}
	
	public int hmmEvalute(int [] input){
		double square = squareEvalution(input);
		double v_down = vDownEvalution(input);
		double v_up = vUpEvalution(input);
		double horizontal = horizontalEvalution(input);
		double vertical = verticalEvalution(input);
		
		Debug.Log("SQUARE: " + square + "\n");
		Debug.Log("V_DOWN: " + v_down + "\n");
		Debug.Log("V_UP: " + v_up + "\n");
		Debug.Log("Horizontal Line: " + horizontal + "\n");
		Debug.Log("Vertical Line: " + vertical + "\n");
		
		if(vertical >= 0.5){
			return V_LINE;
		}else if(horizontal >= 0.5){
			return H_LINE;
		}else if(v_up >= 0.5){
			return V_UP;
		}else if(v_down >= 0.5){
			return V_DOWN;
		}else if(square >= 0.25){
			return SQUARE;
		}else{
			throw new GestureNotFoundException("Gesture not found!");
		}
	}
	
	void Start(){
		Debug.Log("Final Result: " + hmmEvalute(new int[]{ NORTH }) + "\n");
	}       
}

