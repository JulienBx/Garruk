using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Terreur";
		base.ciblage = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,20);
			}
		}
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int damages = currentCard.getDamagesAgainst(targetCard, percentage);
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "-"+damages+" PV\nParalysé pour 1 tour"));
		GameView.instance.getCard(target).setState(new Modifyer(0, 1, 4, text, "Paralysé. Ne peut pas agir pendant 1 tour"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getPercentage(int level){
		int percentage = -1;
		if(level<2){
			percentage = 50;
		}
		else if(level<4){
			percentage = 60;
		}
		else if(level<6){
			percentage = 70;
		}
		else if(level<8){
			percentage = 80;
		}
		else if(level<10){
			percentage = 90;
		}
		else{
			percentage = 100;
		}
		return percentage;
	}
		
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int damages = currentCard.getDamagesAgainst(targetCard, percentage);
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages+"\nParalysé");
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
