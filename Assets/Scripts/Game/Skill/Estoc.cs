using UnityEngine;
using System.Collections.Generic;

public class Estoc : GameSkill
{
	public Estoc(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Estoc";
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
				GameController.instance.esquive(target,11);
			}
		}
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int malusAttack = this.getMalusAttack(level);
		int damages = currentCard.getDamagesAgainst(targetCard, percentage);
		
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+="\nBonus lache";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+="\nBonus lache";
				}
			}
		}
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "-"+damages+" PV\n"+malusAttack+" ATK pour 1 tour"));
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(malusAttack, 1, 11, text, malusAttack+" ATK pour 1 tour"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.getPlayingCardController(target).updateAttack();
	}
	
	public int getPercentage(int level){
		int percentage = -1;
		if(level<4){
			percentage = 30;
		}
		else if(level<8){
			percentage = 40;
		}
		else{
			percentage = 50;
		}
		return percentage;
	}
	
	public int getMalusAttack(int level){
		int malusAttack = -1;
		if(level<2){
			malusAttack = -3;
		}
		else if(level<3){
			malusAttack = -4;
		}
		else if(level<5){
			malusAttack = -5;
		}
		else if(level<6){
			malusAttack = -6;
		}
		else if(level<7){
			malusAttack = -7;
		}
		else if(level<9){
			malusAttack = -8;
		}
		else if(level<10){
			malusAttack = -9;
		}
		else{
			malusAttack = -10;
		}
		return malusAttack;
	}
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int malusAttack = this.getMalusAttack(level);
		int damages = currentCard.getDamagesAgainst(targetCard, percentage);
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages+"\n"+malusAttack+" ATK pour 1 tour");
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
