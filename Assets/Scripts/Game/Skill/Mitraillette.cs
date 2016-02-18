using UnityEngine;
using System.Collections.Generic;

public class Mitraillette : GameSkill
{
	public Mitraillette()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Mitraillette";
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

		List<int> potentialTargets = GameView.instance.getOpponents();
		List<int> targets = new List<int>();
		List<int> chosenTargets = new List<int>();
		bool isFirstP = GameView.instance.getIsFirstPlayer();
		Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()); 
		Tile targetTile ; 
		int proba = GameView.instance.getCurrentSkill().proba;
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		int maxDamages = GameView.instance.getCurrentSkill().Power*2;

		for(int i = 0 ; i < potentialTargets.Count ; i++){
			targetTile = GameView.instance.getTile(potentialTargets[i]); 
			if(isFirstP){
				if(currentTile.y<targetTile.y){
					targets.Add(potentialTargets[i]);
				}
			}
			else{
				if(currentTile.y>targetTile.y){
					targets.Add(potentialTargets[i]);
				}
			}
		}

		int nbTargets = 0 ; 

		while (nbTargets<3 && targets.Count>0){
			int chosenTarget = Random.Range(0,targets.Count);

			if (Random.Range(1,101) <= GameView.instance.getCard(targets[chosenTarget]).getMagicalEsquive()){
				GameController.instance.esquive(targets[chosenTarget],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					int value = Random.Range(1,maxDamages+1);
					GameController.instance.applyOn2(targets[chosenTarget], value);
				}
				else{
					GameController.instance.esquive(targets[chosenTarget],base.name);
				}
			}

			targets.RemoveAt(chosenTarget);
			nbTargets++ ;
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, value);

		string text = "-"+damages+"PV";
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,30,base.name,damages+" dégats subis"));
		GameView.instance.addAnim(GameView.instance.getTile(target), 30);
	}
}

