using UnityEngine;
using System.Collections.Generic;

public class CoupPrecis : GameSkill
{
	public CoupPrecis(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Coup précis";
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
		GameController.instance.applyOn(target);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public override void applyOn(int target){
		string text = base.name;
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = Mathf.FloorToInt((50+5*GameView.instance.getCurrentSkill().Power)*currentCard.getDamagesAgainstWS(targetCard)/100);
		
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
		}
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, damages+" dégats subis"));
		GameView.instance.getPlayingCardController(target).updateLife();
	}
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = Mathf.FloorToInt((50+5*GameView.instance.getCurrentSkill().Power)*currentCard.getDamagesAgainstWS(targetCard)/100);
		
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
		}
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);	
		text += "\nHIT% : 100";
		
		return text ;
	}
}
