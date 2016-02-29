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
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentSkill().Description);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryone() ; 
		int maxdamages = 10+GameView.instance.getCurrentSkill().Power*2;
		int level = GameView.instance.getCurrentSkill().Power;
		int myLevel = currentCard.Skills[0].Power;

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
		GameController.instance.endPlay();
		if(currentCard.isFou()){
			GameController.instance.launchFou(24,GameView.instance.getCurrentPlayingCard());
		}
	}

	public override void launchFou(int c){
		int myLevel = GameView.instance.getCard(c).Skills[0].Power;
		GameView.instance.getPlayingCardController(c).addDamagesModifyer(new Modifyer((10-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"));
		GameView.instance.displaySkillEffect(c, base.name+"\n-"+(10-myLevel)+"PV", 0);
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, amount);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 24, base.name, damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, base.name+"\n-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 24);
	}	
}
