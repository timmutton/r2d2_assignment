using UnityEngine;
using System.Collections;
using System;

public static class LevDist {	
	public static int levDistance(string string1, string string2) {
		int rows = string1.Length, 
			columns = string2.Length;
		int[,] levMat = new int[rows + 1, columns + 1];
		
	    //allocate memory and fill in first column
	    for(int i = 0; i <= rows; ++i) {
	        levMat[i, 0] = i;
	    }
	
	    //fill in first row
	    for(int j = 0; j <= columns; ++j) {
	        levMat[0, j]  = j;
	    }
	
	    //calc distance
	    for(int i = 1; i <= rows; ++i) {
	        for(int j = 1; j <= columns; ++j){
				int cost = (string2[j - 1] == string1[i - 1])?0:1;
				
				levMat[i, j] = (int)Math.Min(
					Math.Min(levMat[i - 1, j] + 1, levMat[i, j - 1] + 1),
					levMat[i - 1, j - 1] + cost);
			}
	    }
	
	    return levMat[rows, columns];
	}
}
