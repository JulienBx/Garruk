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
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*level/10f));
		string text = "-"+damages+"PV\n-2MOV";				
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)\n-2MOV";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)\n-2MOV";
				}
			}
		}
		GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(-2, 1, 11, base.name, "-2MOV. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 15);
		GameView.instance.recalculateDestinations();
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*level/10f));
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nMOV : "+targetCard.getMove()+" -> "+Mathf.Max(1, targetCard.getMove()-2)+" pour 1 tour";				
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)\nMOV : "+targetCard.getMove()+" -> "+Mathf.Max(1, targetCard.getMove()-2)+" pour 1 tour";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)\nMOV : "+targetCard.getMove()+" -> "+Mathf.Max(1, targetCard.getMove()-2)+" pour 1 tour";
				}
			}
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
