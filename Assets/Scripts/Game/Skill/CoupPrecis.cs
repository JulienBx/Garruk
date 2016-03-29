using UnityEngine;
using System.Collections.Generic;

public class CoupPrecis : GameSkill
{
	public CoupPrecis(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Coup précis";
		base.ciblage = 1 ;
		base.auto = false;
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
		if (Random.Range(1,101) <= proba){
			GameController.instance.applyOn(target);
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f));
		string text = "-"+damages+"PV";				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = "-"+damages+"PV\n(lâche)";
		}
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 59);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f));
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = base.name+"\n-"+damages+"PV\n(lâche)";
		}
		
		text += "\n\nHIT% : 100";
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
