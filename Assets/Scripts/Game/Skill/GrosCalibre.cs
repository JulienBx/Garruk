using UnityEngine;
using System.Collections.Generic;

public class GrosCalibre : GameSkill
{
	public GrosCalibre()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Gros Calibre";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		int isFou = 1 ;
		if(currentCard.isFou()){
			if(Random.Range(1,101)<26){
				isFou = -1 ;
				List<Tile> potentialTiles = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).getImmediateNeighbourTiles() ;
				List<int> potentialTargets = new List<int>();
				for (int i = 0 ; i < potentialTiles.Count ; i++){
					if(GameView.instance.getTileController(potentialTiles[i]).getCharacterID()!=-1){
						potentialTargets.Add(GameView.instance.getTileController(potentialTiles[i]).getCharacterID());
					}
				}
				if(potentialTargets.Count>1){
					List<int> targets = new List<int>();
					for (int i = 0 ; i < potentialTargets.Count ; i++){
						if(potentialTargets[i]!=target){
							targets.Add(potentialTargets[i]);
						}
					}
					target = targets[Random.Range(0,targets.Count)];
				}
			}
		}

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, isFou);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(1f+level/10f)));
		string text = "-"+damages+"PV";				
		if(value==-1){
			text+="\nse trompe de cible!";
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,26,base.name,damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 26);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(1f+level/10f)));
		string text = "-"+damages+"PV";		
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
