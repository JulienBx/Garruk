using UnityEngine;
using System.Collections.Generic;

public class Bombardier : GameSkill
{
	public Bombardier(){
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Bombardier";
		base.ciblage = 0 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentSkill().Description);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryone() ; 
		int maxdamages = GameView.instance.getCurrentSkill().Power*3;
		int proba = GameView.instance.getCurrentSkill().proba;
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) <= GameView.instance.getCard(targets[i]).getMagicalEsquive()){
				GameController.instance.esquive(targets[i],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					GameController.instance.applyOn2(targets[i], Random.Range(1,maxdamages+1));
				}
				else{
					GameController.instance.esquive(targets[i],base.name);
				}
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, amount);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 24, base.name, damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 24);
	}	
}
