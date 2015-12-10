using UnityEngine;
using System.Collections.Generic;

public class CoupeJambes : GameSkill
{
	public CoupeJambes(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Coupe-Jambes";
		base.ciblage = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{	
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
				isSuccess = true ;
			}
			else{
				GameController.instance.esquive(target,15);
			}
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int malusMove = this.getMalusMove(level);
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
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "-"+damages+" PV\n"+malusMove+" MOV pour 1 tour"));
		GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(malusMove, 1, 15, text, malusMove+" MOV pour 1 tour"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.recalculateDestinations();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getPercentage(int level){
		int percentage = -1;
		if(level<2){
			percentage = 10;
		}
		else if(level<3){
			percentage = 20;
		}
		else if(level<4){
			percentage = 30;
		}
		else if(level<7){
			percentage = 40;
		}
		else if(level<9){
			percentage = 50;
		}
		else if(level<10){
			percentage = 60;
		}
		else{
			percentage = 70;
		}
		return percentage;
	}
	
	public int getMalusMove(int level){
		int malusMove = -1;
		if(level<6){
			malusMove = -1;
		}
		else{
			malusMove = -2;
		}
		return malusMove;
	}
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int percentage = this.getPercentage(level);
		int malusMove = this.getMalusMove(level);
		int damages = currentCard.getDamagesAgainst(targetCard, percentage);
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages+"\n"+malusMove+" MOV pour 1 tour");
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
