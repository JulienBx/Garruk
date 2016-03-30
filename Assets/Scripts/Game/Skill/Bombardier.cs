using UnityEngine;
using System.Collections.Generic;

public class Bombardier : GameSkill
{
	public Bombardier(){
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Bombardier";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryone() ; 
		int maxdamages = 10+GameView.instance.getCurrentSkill().Power*2;

		if(currentCard.isFou()){
			maxdamages = Mathf.RoundToInt(1.25f*maxdamages);
		}
		int proba = GameView.instance.getCurrentSkill().proba;
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
		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, amount);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 24, base.name, damages+" dégats subis"),  (target==GameView.instance.getCurrentPlayingCard()));
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 24);
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
