using UnityEngine;
using System.Collections.Generic;
using System;

public class Desequilibre : GameSkill
{
	public Desequilibre()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Déséquilibre";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		if (UnityEngine.Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (UnityEngine.Random.Range(1,101) <= proba){
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*level/10f));
		string text = "-"+damages+"PV";

		Tile targetTile = GameView.instance.getPlayingCardController(target).getTile();
		Tile currentTile = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile();
		Tile nextTile;
		
		if(!targetCard.isMine){
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		else{
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		if(nextTile.x>=0 && nextTile.x<GameView.instance.getBoardWidth() && nextTile.y>=0 && nextTile.y<GameView.instance.getBoardHeight()){
			if(!GameView.instance.getTileController(nextTile).isRock() && GameView.instance.getTileController(nextTile).getCharacterID()==-1){
				GameView.instance.clickDestination(nextTile,target);
				text+="\nRepoussé!";
			}
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 92, text, "-"+damages+" PV"));
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 92);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*level/10f));

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nrepousse l'unité";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
