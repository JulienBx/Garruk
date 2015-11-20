using UnityEngine;
using System.Collections.Generic;

public class Leader : GameSkill
{
	public Modifyer getBonusAttack(int level){	
		List<Modifyer> modifyers = new List<Modifyer>();
		int type = 76 ; 
		int duration = -1;
		int amount = 0 ;
		string title = "Bonus Leader";
		
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
		
		string description = "+"+amount+" ATK\n<i>Tant que le leader est en vie</i>" ;
		
		return (new Modifyer(amount, duration, type, title, description));
	}
	
	public Modifyer getBonusPV(int level){	
		List<Modifyer> modifyers = new List<Modifyer>();
		int type = 76 ; 
		int duration = -1;
		int amount = 0 ;
		string title = "Bonus Leader";
		
		if(level<=1){
			amount = 0;
		}
		else if(level<=3){
			amount = 1;
		}
		else if(level<=5){
			amount = 2;
		}
		else if(level<=7){
			amount = 3;
		}
		else if(level<=9){
			amount = 4;
		}
		else{
			amount = 5;
		}
		
		string description = "+"+amount+" PV\n<i>Tant que le leader est en vie</i>" ;
		
		return (new Modifyer(amount, duration, type, title, description));
	}
}
