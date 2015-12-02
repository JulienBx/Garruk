using UnityEngine;
using System.Collections.Generic;

public class TirALarc : GameSkill
{
	public TirALarc()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Tir à l'arc";
		base.ciblage = 0 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;
		int target ;
		
		List<int> everyone = GameView.instance.getOpponents();
		target = everyone[Random.Range (0,everyone.Count)];

		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				int value = this.getValue(level);
				GameController.instance.applyOn2(target,value);
			}
			else{
				GameController.instance.esquive(target,8);
			}
		}
		
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getValue(int level){
		int value = -1;
		if(level==1){
			value=Random.Range(1,3);
		}
		else if(level==2){
			value=Random.Range(1,5);
		}
		else if(level==3){
			value=Random.Range(2,6);
		}
		else if(level==4){
			value=Random.Range(2,8);
		}
		else if(level==5){
			value=Random.Range(3,9);
		}
		else if(level==6){
			value=Random.Range(3,11);
		}
		else if(level==7){
			value=Random.Range(4,12);
		}
		else if(level==8){
			value=Random.Range(4,14);
		}
		else if(level==9){
			value=Random.Range(5,15);
		}
		else if(level==10){
			value=Random.Range(5,17);
		}
		return value;
	}
	
	public int getMaxDamages(int level){
		int value = -1;
		if(level==1){
			value=2;
		}
		else if(level==2){
			value=4;
		}
		else if(level==3){
			value=5;
		}
		else if(level==4){
			value=7;
		}
		else if(level==5){
			value=8;
		}
		else if(level==6){
			value=10;
		}
		else if(level==7){
			value=11;
		}
		else if(level==8){
			value=13;
		}
		else if(level==9){
			value=14;
		}
		else if(level==10){
			value=16;
		}
		return value;
	}
	
	public int getMinDamages(int level){
		int value = -1;
		if(level==1){
			value=1;
		}
		else if(level==2){
			value=1;
		}
		else if(level==3){
			value=2;
		}
		else if(level==4){
			value=2;
		}
		else if(level==5){
			value=3;
		}
		else if(level==6){
			value=3;
		}
		else if(level==7){
			value=4;
		}
		else if(level==8){
			value=4;
		}
		else if(level==9){
			value=5;
		}
		else if(level==10){
			value=5;
		}
		return value;
	}
	
	public override void applyOn(int target, int value){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 0, text, "-"+value+" PV"));
		GameView.instance.getPlayingCardController(target).updateLife();
	}
}
