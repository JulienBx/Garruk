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
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

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
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));
		string text = base.name+"\n-"+damages+"PV";

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
				GameView.instance.dropCharacter(target, nextTile, true, true);
				GameView.instance.recalculateDestinations();
				text+="\nRepoussé!";
			}
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 92, text, "-"+damages+" PV"), false);
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 92);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nrepousse l'unité";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
