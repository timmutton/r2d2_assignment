using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MyHMM;

// all possible gestures used in the system
public enum GestureEnum{
	SQUARE = 8,
	V_DOWN = 9,
	V_UP = 10,
	HORIZONTAL_LINE = 11,
	VERTICAL_LINE = 12
}

public class HMMRecognizer : MonoBehaviour
{
	//all possible directions
	public const int NORTH = 0;
	public const int SOUTH = 1;
	public const int EAST = 2;
	public const int WEST = 3;
	public const int NORTH_EAST = 4;
	public const int NORTH_WEST = 5;
	public const int SOUTH_EAST = 6;
	public const int SOUTH_WEST = 7;
	
	/*
	 * this method passes all the possible directions 
	 * that can create a square to the HMM library 
	 * as observations. A and B are the state and
	 * observation probabilities for the model
	 * pi is the intial state of the model
	 * the function returns true if the input matches any 
	 * of the observations used to train the model
	*/
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
	
	/*
	 * this method passes all the possible directions 
	 * that can create a V down to the HMM library 
	 * as observations. A and B are the state and
	 * observation probabilities for the model
	 * pi is the intial state of the model
	 * the function returns true if the input matches any 
	 * of the observations used to train the model
	*/
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
	
	/*
	 * this method passes all the possible directions 
	 * that can create a V up to the HMM library 
	 * as observations. A and B are the state and
	 * observation probabilities for the model
	 * pi is the intial state of the model
	 * the function returns true if the input matches any 
	 * of the observations used to train the model
	*/
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
	
	/*
	 * this method passes all the possible directions 
	 * that can create a vertical line to the HMM library 
	 * as observations. A and B are the state and
	 * observation probabilities for the model
	 * pi is the intial state of the model
	 * the function returns true if the input matches any 
	 * of the observations used to train the model
	*/
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
	
	/*
	 * this method passes all the possible directions 
	 * that can create a horizontal line to the HMM library 
	 * as observations. A and B are the state and
	 * observation probabilities for the model
	 * pi is the intial state of the model
	 * the function returns true if the input matches any 
	 * of the observations used to train the model
	*/
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
	
	/**
	 * this function runs all the gesture models above on any given
	 * input and compares the results to get the best possible
	 * gesture.
	 **/
	public int hmmEvalute(int [] input){
		
		bool square = squareEvalution(input);
		bool v_down = vDownEvalution(input);
		bool v_up = vUpEvalution(input);
		bool horizontal = horizontalEvalution(input);
		bool vertical = verticalEvalution(input);
		
		if(vertical){
			return (int)GestureEnum.VERTICAL_LINE;
		}else if(horizontal){
			return (int)GestureEnum.HORIZONTAL_LINE;
		}else if(v_up){
			return (int)GestureEnum.V_UP;
		}else if(v_down){
			return (int)GestureEnum.V_DOWN;
		}else if(square){
			return (int)GestureEnum.SQUARE;
		}else{
			throw new GestureNotFoundException("GestureEnum not found!");
		}
	}      
}

