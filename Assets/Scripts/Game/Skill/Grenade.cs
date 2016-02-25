using UnityEngine;
using System.Collections.Generic;

public class Grenade : GameSkill
{
	public Grenade()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Grenade";
		base.ciblage = -2 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(1, "Grenade", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = GameView.instance.getCurrentSkill().proba;
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages;
		int maxDamages;
		int playerID;
		int target = 1 ;
		if(currentCard.isFou()){
			if(Random.Range(1,101)<26){
				target = -1 ;
				targetsTile[0]=new Tile(Random.Range(0,GameView.instance.boardWidth), Random.Range(0,GameView.instance.boardHeight));
			}
		}
	
		playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y)).getCharacterID();
		if(playerID!=-1){
			if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
				GameController.instance.esquive(playerID,1);
			}
			else if (Random.Range(1,101) <= proba){
				targetCard = GameView.instance.getCard(playerID);
				minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
				maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
				GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
			}
			else{
				GameController.instance.esquive(playerID,base.name);
			}
		}
		if(targetsTile[0].x<GameView.instance.boardWidth-1){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x+1, targetsTile[0].y)).getCharacterID();
			if(playerID!=-1){
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
					maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
					GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
				}
				else{
					GameController.instance.esquive(playerID,base.name);
				}
			}
		}
		if(targetsTile[0].x>0){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x-1, targetsTile[0].y)).getCharacterID();
			if(playerID!=-1){
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
					maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
					GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
				}
				else{
					GameController.instance.esquive(playerID,base.name);
				}
			}
		}
		if(targetsTile[0].y<GameView.instance.boardHeight-1){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y+1)).getCharacterID();
			if(playerID!=-1){
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
					maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
					GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
				}
				else{
					GameController.instance.esquive(playerID,base.name);
				}
			}
		}
		if(targetsTile[0].y>0){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y-1)).getCharacterID();
			if(playerID!=-1){
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
					maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
					GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
				}
				else{
					GameController.instance.esquive(playerID,base.name);
				}
			}
		}

		GameController.instance.applyOn(target);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 23, base.name, value+" dégats subis"));
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 23);
	}

	public override void applyOn(int target){
		if(target==-1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Fou!\nse trompe de cible!", 0);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 23);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 23);
		}
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
		int maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
