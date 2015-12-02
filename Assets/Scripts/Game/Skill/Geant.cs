using UnityEngine;
using System.Collections.Generic;

public class Geant : GameSkill
{
	public int getProba(int level){	
		int amount = 0 ;
		
		if(level<=2){
			amount = 1;
		}
		else if(level<=4){
			amount = 2;
		}
		else if(level<=6){
			amount = 3;
		}
		else if(level<=8){
			amount = 4;
		}
		else if(level<=10){
			amount = 5;
		}
		
		return amount;
	}
}
