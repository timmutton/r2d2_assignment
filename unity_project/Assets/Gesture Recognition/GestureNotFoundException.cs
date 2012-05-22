/*
 * This class is used to throw an exception when the 
 * gesture recieved from the user is not recognized 
 * by the HMM Classifier
 * */

using System;

public class GestureNotFoundException : Exception {
	private string message;
	
	public GestureNotFoundException(){
		
	}
	
	public GestureNotFoundException(string message){
		this.message = message;
	}
	
	public string getMessage(){
		return this.message;
	}
}
