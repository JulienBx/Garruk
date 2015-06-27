using UnityEngine;
using System.Collections.Generic;

public class HaloTarget
{
	public int idImage;                      // de combien ça modifie        
	public List<string> textsToDisplay ;                  // -1 permanent ou bien le nombre de tour
	public List<int> stylesID; 

	public HaloTarget(int i)
	{
		this.idImage = i ;
		this.textsToDisplay = new List<string>();
		this.stylesID = new List<int>();
	}
	
	public void addInfo(string s, int i){
		this.textsToDisplay.Add (s);
		this.stylesID.Add (i);
	}
}
