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
